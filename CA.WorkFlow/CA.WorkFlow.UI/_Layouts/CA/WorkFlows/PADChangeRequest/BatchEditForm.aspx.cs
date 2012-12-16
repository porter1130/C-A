using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using QuickFlow.UI.Controls;
using QuickFlow.Core;
using CA.SharePoint;
using CA.WorkFlow.UI.PADChangeRequest;
using System.Text;
using System.Data;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.PADChangeRequest
{
    public partial class BatchEditForm : CAWorkFlowPage
    {
        string workflowNumber = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            Actions.OnClientClick = "return beforeSubmit(this)";

            if (!IsPostBack)
            {
                BindData();
            }
        }


        void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            bool isCheckOK = CheckData();
            if (!isCheckOK)//未能通过数据验证
            {
                e.Cancel = true;
                return;
            }

            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sWorkflowNumber = fields["Title"].ToString();

            if (e.Action.Equals("Save", StringComparison.CurrentCultureIgnoreCase))
            {
                WorkflowContext.Current.UpdateWorkflowVariable("isSave", true);
                WorkflowContext.Current.UpdateWorkflowVariable("isSubmit", false);
                WorkflowContext.Current.UpdateWorkflowVariable("isFree", false);
                WorkflowContext.Current.UpdateWorkflowVariable("completeTaskT", "PAD Change Request needs to Submit");
                WorkflowContext.Current.UpdateWorkflowVariable("editUrl", "/_Layouts/CA/WorkFlows/PADChangeRequest/BatchEditForm.aspx");
                fields["Status"] = CAWorkflowStatus.InProgress;

                DeleteData(sWorkflowNumber);
                SaveDataToPADChangeRequest(sWorkflowNumber);
            }
            else//submit
            {
                bool IsNeedApprove = IsNeedWorkFlowStep();//是否需要工作流审批，只要有一个记录的原始日期大于新输入的日期,则需要进入工作流审批流程,
                if (!IsNeedApprove)//不需要工作流审批流程
                {
                    bool isSuccess = UpdateToSAP(sWorkflowNumber);
                    if (isSuccess)
                    {
                        WorkflowContext.Current.UpdateWorkflowVariable("isSubmit", true);
                        WorkflowContext.Current.UpdateWorkflowVariable("isFree", true);
                        WorkflowContext.Current.UpdateWorkflowVariable("isSave", false);
                        fields["Status"] = CAWorkflowStatus.Completed;

                        DeleteData(sWorkflowNumber);
                        SaveDataToPADChangeRequest(sWorkflowNumber);
                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else//需要工作流审批流程
                {
                    QuickFlow.NameCollection ApproveUser = new QuickFlow.NameCollection();
                    var applicant = WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString());

                    var managerEmp = SapCommonPADChangeRequest.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(applicant));// WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(applicant));
                    if (managerEmp == null)
                    {
                        DisplayMessage("此用户没有Level-5,Level-4级的审批用户，无法提交");
                        e.Cancel = true;
                        return;
                    }
                    ApproveUser.Add(managerEmp.UserAccount);
                    var deleman = WorkFlowUtil.GetDeleman(managerEmp.UserAccount, CA.WorkFlow.UI.Constants.CAModules.PADChangeRequest);
                    if (deleman != null)
                    {
                        ApproveUser.Add(deleman);
                    }
                    WorkflowContext.Current.UpdateWorkflowVariable("isSave", false);
                    WorkflowContext.Current.UpdateWorkflowVariable("isSubmit", true);
                    WorkflowContext.Current.UpdateWorkflowVariable("isFree", false);
                    WorkflowContext.Current.UpdateWorkflowVariable("firstApproveUser", ApproveUser);
                    WorkflowContext.Current.UpdateWorkflowVariable("ManagerApproveT", "PAD Change Request needs to Approve");
                    WorkflowContext.Current.UpdateWorkflowVariable("approveUrl", "/_Layouts/CA/WorkFlows/PADChangeRequest/BatchApproveForm.aspx");
                    fields["Status"] = CAWorkflowStatus.InProgress;
                    fields["CurrManager"] = managerEmp.UserAccount;
                    DeleteData(sWorkflowNumber);
                    SaveDataToPADChangeRequest(sWorkflowNumber);
                }
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        protected void ButtonLoad_Click(object sender, EventArgs e)
        {
            if (FileUploadExcel.HasFile)
            {
                string sFileExtenstion = System.IO.Path.GetExtension(FileUploadExcel.FileName);
                if (sFileExtenstion.Equals(".xlsx", StringComparison.InvariantCultureIgnoreCase))
                {
                    DataTable excelData = null;
                    try
                    {
                        string libraryName = WorkflowListName.WorkflowDocumentLibrary;
                        SPList list = SPContext.Current.Web.Lists[libraryName];
                        string filePath = FileUploadExcel.PostedFile.FileName;

                        string subFolder = "PADChangeRequeast";

                        SPFile file = list.RootFolder.SubFolders[subFolder].Files.Add(FileUploadExcel.FileName, FileUploadExcel.PostedFile.InputStream, true);

                        var fileName = file.Name;
                        fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                        char[] split = new char[] { '_' };
                        var sheetNames = fileName.Split(split);
                        if (sheetNames.Length > 1)
                        {
                            string positionKeyValue = ExcelService.GetExcelConfigInfo(WorkflowConfigName.PADPosition);
                            string primaryKeyValue = ExcelService.GetExcelConfigInfo(WorkflowConfigName.PADPK);
                            string colsKeyValue = ExcelService.GetExcelConfigInfo(WorkflowConfigName.PADCol);

                            //excelData = ExcelService.ParseExcel(file, "A,2", "A", "A,B", sheetNames[1]);
                            excelData = ExcelService.ParseExcel(file, positionKeyValue, primaryKeyValue, colsKeyValue, sheetNames[1]);
                            bool isOK = CeckExcelData(excelData);
                            if (isOK)
                            {
                                DataTable dt = CreateSAPPOData(excelData);
                                RepeaterPOData.DataSource = dt;
                                RepeaterPOData.DataBind();
                                SetRepeaterSelectedDate();
                                LabelCount.Text = dt.Rows.Count.ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonUtil.logError("Upload or read file fail, error:" + ex.Message);
                        throw ex;
                    }
                }
            }
        }

        protected void ButtonBatchSearch_Click(object sender, EventArgs e)
        {
            string sPONOs = TextBoxPONOs.Text.Trim();
            if (sPONOs.Length == 0)
            {
                return;
            }
            DataTable dtPO = new DataTable();
            dtPO = CreateInputPOData(sPONOs.Split(','));
            DataTable dt = CreateSAPPOData(dtPO);
            RepeaterPOData.DataSource = dt;
            RepeaterPOData.DataBind();
            LabelCount.Text = dt.Rows.Count.ToString();
        }

        /// <summary>
        /// 验证上传的Excel中的日期格式是否正确
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        bool CeckExcelData(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                DateTime dtDate = DateTime.Now;
                string sDate = dr[1].ToString();
                if (!DateTime.TryParse(sDate, out dtDate))
                {
                    sb.Append(string.Concat(dr[0], " has a error date formate", "\n"));
                }
            }
            if (sb.Length > 0)
            {
                DisplayMessage(sb.ToString());
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 从PO集合得到PO信息数据集
        /// </summary>
        /// <param name="dtPO"></param>
        /// <returns></returns>
        DataTable CreateSAPPOData(DataTable dtPO)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PONumber");
            dt.Columns.Add("CurrentPAD");
            dt.Columns.Add("NewPAD");
            dt.Columns.Add("SupplierName");
            dt.Columns.Add("SADweek");
            dt.Columns.Add("SADyear");
            dt.Columns.Add("OMU");
            dt.Columns.Add("PADweek");
            dt.Columns.Add("PADyear");
            dt.Columns.Add("OSP");
            dt.Columns.Add("ValueForStory");
            dt.Columns.Add("IsNeedApprove");
            dt.Columns.Add("IsSuccess");
            dt.Columns.Add("PoQty");
            dt.Columns.Add("StyleNumber");

            //
            StringBuilder sb = new StringBuilder();
            foreach (DataRow drPO in dtPO.Rows)
            {
                SapCommonPADChangeRequest sapcommonpad = new SapCommonPADChangeRequest();
                string sPONO = drPO[0].ToString();
                string sNewPAD = drPO[1].ToString();
                if (sapcommonpad.SapSearchPAD(sPONO))
                {
                    DataRow dr = dt.NewRow();
                    dr["PONumber"] = sPONO;
                    dr["CurrentPAD"] = Convert.ToDateTime(sapcommonpad.PAD).ToString("MM\\/dd\\/yyyy");
                    dr["SupplierName"] = sapcommonpad.SupplierName;
                    dr["SADweek"] = sapcommonpad.SADweek;
                    dr["SADyear"] = sapcommonpad.SADyear;
                    dr["OMU"] = sapcommonpad.OMU;
                    dr["PADweek"] = sapcommonpad.PADweek;
                    dr["PADyear"] = sapcommonpad.PADyear;
                    dr["OSP"] = sapcommonpad.OSP;
                    dr["ValueForStory"] = sapcommonpad.ValueForStory;
                    dr["IsNeedApprove"] = sapcommonpad.Delivered;
                    dr["IsSuccess"] = 0;
                    dr["NewPAD"] = sNewPAD;
                    dr["PoQty"] = sapcommonpad.SPOQTY;
                    dr["StyleNumber"] = sapcommonpad.STYLENUMBER;
                    
                    dt.Rows.Add(dr);
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("\\n");
                    }
                    sb.Append("Can not find ");
                    sb.Append(sPONO);
                }
            }
            if (sb.Length > 0)
            {
                DisplayMessage(sb.ToString());
            }

            return dt;
        }

        /// <summary>
        /// 验证数据是否合法
        /// </summary>
        /// <returns></returns>
        bool CheckData()
        {
            if (RepeaterPOData.Items.Count == 0)
            {
                DisplayMessage("数据为空，请重新填写！");
                return false;
            }

            bool isAlldelivery = true;
            StringBuilder sbErorInfo = new StringBuilder();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                CADateTimeControl CADateTimeFrom = item.FindControl("CADateTimeFrom") as CADateTimeControl;
                Label LabelPAD = item.FindControl("LabelPAD") as Label;
                Label LabelPONO = item.FindControl("LabelPONO") as Label;
                HiddenField HiddenFieldstoredelivery = item.FindControl("HiddenFieldstoredelivery") as HiddenField;

                if (HiddenFieldstoredelivery.Value == "False")
                {
                    isAlldelivery = false;
                }

                string sPONO = LabelPONO.Text.Trim();
                if (CADateTimeFrom.IsDateEmpty)//新日期为空
                {
                    sbErorInfo.Append(string.Concat("Proposed New PAD can not empty for ", sPONO, "\\n"));
                }

                if (string.IsNullOrEmpty(LabelPAD.Text.Trim()))//原始日期为空 
                {
                    sbErorInfo.Append(string.Concat("Current PAD can not empty for ", sPONO, "\\n"));
                }
                DateTime dtNew = CADateTimeFrom.SelectedDate;
                DateTime dtCurrent = DateTime.Now;
                if (DateTime.TryParse(LabelPAD.Text.Trim(), out dtCurrent))//新日期与原始日期相等
                {
                    if (dtCurrent == dtNew)
                    {
                        sbErorInfo.Append(string.Concat("Current PAD can not equal Proposed New PAD  for ", sPONO, "\\n"));
                    }
                }
                else//原始日期格式不合法
                {
                    sbErorInfo.Append(string.Concat("Error Current PAD formate for ", sPONO, "\\n"));
                }
            }

            if (isAlldelivery)//所有的 PO都是己经isAlldelivery
            {
                sbErorInfo.Append("There are no avaliable po(Error value Delivered)");
                DisplayMessage(sbErorInfo.ToString());
                return false;
            }

            if (sbErorInfo.Length > 0)
            {
                DisplayMessage(sbErorInfo.ToString());
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 保存数据到SaveDataToPADChangeRequest
        /// </summary>
        /// <param name="sWorkflowNumber"></param>
        void SaveDataToPADChangeRequest(string sWorkflowNumber)
        {
            DataTable dtPADChangeRequest = CreatePADChangeTamplate();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                HiddenField HiddenFieldstoredelivery = item.FindControl("HiddenFieldstoredelivery") as HiddenField;
                if (HiddenFieldstoredelivery.Value == "True")
                {
                    continue;
                }
                Label LabelPONO = item.FindControl("LabelPONO") as Label;
                Label LabelPAD = item.FindControl("LabelPAD") as Label;
                CADateTimeControl CADateTimeFrom = item.FindControl("CADateTimeFrom") as CADateTimeControl;
                TextBox TextBoxSupplierName = item.FindControl("TextBoxSupplierName") as TextBox;
                TextBox TextBoxPADYear = item.FindControl("TextBoxPADYear") as TextBox;
                TextBox TextBoxPADweek = item.FindControl("TextBoxPADweek") as TextBox;
                TextBox TextBoxOSP = item.FindControl("TextBoxOSP") as TextBox;
                TextBox TextBoxValueForStory = item.FindControl("TextBoxValueForStory") as TextBox;
                TextBox TextBoxSADyear = item.FindControl("TextBoxSADyear") as TextBox;
                TextBox TextBoxSADweek = item.FindControl("TextBoxSADweek") as TextBox;
                TextBox TextBoxOMU = item.FindControl("TextBoxOMU") as TextBox;
                HiddenField HiddenFieldIsSuccess = item.FindControl("HiddenFieldIsSuccess") as HiddenField;
                TextBox TextBoxPOQTY = item.FindControl("TextBoxPOQTY") as TextBox;
                TextBox TextBoxStyleNO = item.FindControl("TextBoxStyleNO") as TextBox;

                DataRow dr = dtPADChangeRequest.NewRow();
                dr["Title"] = sWorkflowNumber;
                dr["PONumber"] = LabelPONO.Text.Trim();
                dr["CurrentPAD"] = LabelPAD.Text.Trim();
                dr["NewPAD"] = CADateTimeFrom.SelectedDate;
                dr["SupplierName"] = TextBoxSupplierName.Text.Trim();
                dr["PADyear"] = TextBoxPADYear.Text.Trim();
                dr["PADweek"] = TextBoxPADweek.Text.Trim();
                dr["OSP"] = TextBoxOSP.Text.Trim();
                dr["ValueForStory"] = TextBoxValueForStory.Text.Trim();
                dr["SADyear"] = TextBoxSADyear.Text.Trim();
                dr["SADweek"] = TextBoxSADweek.Text.Trim();
                dr["OMU"] = TextBoxOMU.Text.Trim();
                dr["IsSuccess"] = HiddenFieldIsSuccess.Value;
                dr["PoQty"] = TextBoxPOQTY.Text.Trim();
                dr["StyleNumber"] = TextBoxStyleNO.Text.Trim();

                dtPADChangeRequest.Rows.Add(dr);
            }

            SapCommonPADChangeRequest scpadcr = new SapCommonPADChangeRequest();
            scpadcr.BatchAddToListByDatatable(dtPADChangeRequest, "PADChangeRequestItems");
        }

        /// <summary>
        /// 创建批量保存到PADChangeRequest 的表模板
        /// </summary>
        /// <returns></returns>
        DataTable CreatePADChangeTamplate()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("PONumber");
            dt.Columns.Add("CurrentPAD");
            dt.Columns.Add("NewPAD");
            dt.Columns.Add("SupplierName");
            dt.Columns.Add("PADyear");
            dt.Columns.Add("PADweek");
            dt.Columns.Add("OSP");
            dt.Columns.Add("ValueForStory");
            dt.Columns.Add("SADyear");
            dt.Columns.Add("SADweek");
            dt.Columns.Add("OMU");
            dt.Columns.Add("IsSuccess");
            dt.Columns.Add("PoQty");
            dt.Columns.Add("StyleNumber");
            return dt;
        }

        /// <summary>
        /// 是否需要工作流审批，只要有一个记录的原始日期大于新输入的日期,则需要进入工作流审批流程,
        /// </summary>
        /// <returns></returns>
        bool IsNeedWorkFlowStep()
        {
            bool IsNeedApprove = false;
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                HiddenField HiddenFieldstoredelivery = item.FindControl("HiddenFieldstoredelivery") as HiddenField;
                if (HiddenFieldstoredelivery.Value == "True")
                {
                    continue;
                }

                Label LabelPAD = item.FindControl("LabelPAD") as Label;
                CADateTimeControl CADateTimeFrom = item.FindControl("CADateTimeFrom") as CADateTimeControl;

                if (Convert.ToDateTime(LabelPAD.Text.Trim()) > Convert.ToDateTime(CADateTimeFrom.SelectedDate))
                {
                    IsNeedApprove = true;
                    break;
                }
            }
            return IsNeedApprove;
        }

        /// <summary>
        /// 数据更新到SAP
        /// </summary>
        /// <param name="sWorkFlowNO"></param>
        /// <returns></returns>
        bool UpdateToSAP(string sWorkFlowNO)
        {
            bool isOK = true;
            StringBuilder sbErrorInfo = new StringBuilder();
            StringBuilder sbSucessInfo = new StringBuilder();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                HiddenField HiddenFieldstoredelivery = item.FindControl("HiddenFieldstoredelivery") as HiddenField;
                if (HiddenFieldstoredelivery.Value == "True")
                {
                    continue;
                }
                Label LabelPAD = item.FindControl("LabelPAD") as Label;
                CADateTimeControl CADateTimeFrom = item.FindControl("CADateTimeFrom") as CADateTimeControl;
                HiddenField HiddenFieldIsSuccess = item.FindControl("HiddenFieldIsSuccess") as HiddenField;
                if (HiddenFieldIsSuccess.Value == "1")
                {
                    continue;
                }
                if (Convert.ToDateTime(LabelPAD.Text.Trim()) < Convert.ToDateTime(CADateTimeFrom.SelectedDate))
                {
                    Label LabelPONO = item.FindControl("LabelPONO") as Label;
                    string sPONO = LabelPONO.Text.Trim();
                    SapCommonPADChangeRequest sapcommonpad = new SapCommonPADChangeRequest();
                    if (!sapcommonpad.SapUpdatePAD(LabelPONO.Text.ToString(), Convert.ToDateTime(CADateTimeFrom.SelectedDate).ToString("yyyy-MM-dd")))//更新到 SAP失败
                    {
                        sbErrorInfo.Append(string.Concat("Update ", sPONO, " to SAP failed,error info:", sapcommonpad.ErrorMsg.Replace("'", "‘").Replace("\\n", "  "), "\\n"));
                    }
                    else
                    {
                        sbSucessInfo.Append(string.Concat("\\nUpdate ", sPONO, " to SAP success"));
                        HiddenFieldIsSuccess.Value = "1";
                        sapcommonpad.UpdateItemSapStatus(sPONO, sWorkFlowNO);
                        CADateTimeFrom.Enabled = false;
                    }
                }
            }
            if (sbErrorInfo.Length > 0)
            {
                sbErrorInfo.Append("\\nPlease contact IT form further help!");
                DisplayMessage(string.Concat("There are some errors occoured:\\n" + sbErrorInfo.ToString(), sbSucessInfo.ToString()));
                isOK = false;
            }
            return isOK;

        }

        /// <summary>
        /// 用户输入的PO转换成DataTable
        /// </summary>
        /// <param name="arrayPONO"></param>
        /// <returns></returns>
        DataTable CreateInputPOData(string[] arrayPONO)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PONO");
            dt.Columns.Add("Date");
            foreach (string item in arrayPONO)
            {
                DataRow dr = dt.NewRow();
                dr["PONO"] = item;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        void DeleteData(string sPONO)
        {
            SPQuery query = new SPQuery();
            string sQueryFormat = @"
                                   <Where>
                                      <Eq>
                                         <FieldRef Name='Title' />
                                         <Value Type='Text'>{0}</Value>
                                      </Eq>
                                   </Where>";
            query.Query = string.Format(sQueryFormat, sPONO);
            WorkFlowUtil.BatchDeleteItems("PADChangeRequestItems", query);
        }

        /// <summary>
        /// /绑定数据到Repeater
        /// </summary>
        void BindData()
        {
            DataTable dt = new DataTable();
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sPONO = fields["Title"].ToString();
          
            SPQuery query = new SPQuery();
            query.Query = string.Format(@"<Where>
                                <Eq>
                                    <FieldRef Name='Title' />
                                    <Value Type='Text'>{0}</Value>
                                </Eq>
                            </Where>", sPONO);
            dt = SPContext.Current.Web.Lists["PADChangeRequestItems"].GetItems(query).GetDataTable();
            if (null != dt && dt.Rows.Count > 0)
            {
                RepeaterPOData.DataSource = dt;
                RepeaterPOData.DataBind();
                SetRepeaterSelectedDate();
            }
        }

        public string IsDelivery(string sValue)
        {
            if (sValue == "False" || sValue == "1")
            {
                //False的值为从SAP中取，1为从List中取
                return "False";
            }
            else
            {
                return "True";
            }
        }

        /// <summary>
        /// 设置日期格式
        /// </summary>
        /// <param name="sDate"></param>
        /// <returns></returns>
        public string DateFormate(string sDate)
        {
            DateTime dtDate = DateTime.Now;
            if (DateTime.TryParse(sDate, out dtDate))
            {
                return dtDate.ToString("MM\\/dd\\/yyyy");
            }
            else
            {
                return string.Empty;
            }
            //DateFomate
        }
        /// <summary>
        /// 设置Repeater里的CADateTimeFrom先中日期
        /// </summary>
        void SetRepeaterSelectedDate()
        {
            StringBuilder sbPO =new StringBuilder();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                HiddenField HiddenFieldDate = item.FindControl("HiddenFieldDate") as HiddenField;
                CADateTimeControl CADateTimeFrom = item.FindControl("CADateTimeFrom") as CADateTimeControl;
                Label LabelPONO = item.FindControl("LabelPONO") as Label;

                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(HiddenFieldDate.Value, out dt))
                {
                    CADateTimeFrom.SelectedDate = dt;
                }

                string sPO = LabelPONO.Text;
                if (sbPO.Length > 0)
                {
                    sbPO.Append(",");
                }
                sbPO.Append(sPO);
            }
            TextBoxPONOs.Text = sbPO.ToString();
        }
        
        /// <summary>
        /// 上传Excel时，设置PO搜索框里的值
        /// </summary>
        /// <param name="dt"></param>
        void SetPOSearchTextBox(DataTable dt)
        {
            List<string> listStr = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                string sPONO = dr[0].ToString().Trim();///去重复
                if (!listStr.Contains(sPONO))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(sPONO);
                }
            }
            TextBoxPONOs.Text = sb.ToString();
        }
        public string IntFormate(string sValue)
        {
            int iIntValue = 0;
            int.TryParse(sValue, out iIntValue);
            return iIntValue.ToString();
        }
    }
}