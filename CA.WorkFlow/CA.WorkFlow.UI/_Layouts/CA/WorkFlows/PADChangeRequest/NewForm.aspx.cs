using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using QuickFlow.UI.Controls;
using QuickFlow.Core;
using CA.WorkFlow.UI._Layouts.CA.WorkFlows.PADChangeRequest;
using CA.SharePoint.Utilities.Common;

using System.Text;
using System.Data;
using Microsoft.SharePoint.Workflow; 

namespace CA.WorkFlow.UI.PADChangeRequest
{
    public partial class NewForm : CAWorkFlowPage
    {
        string workflowNumber = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Excuting);
            this.StartWorkflowButton2.Executing += new EventHandler<System.ComponentModel.CancelEventArgs>(StartWorkflowButton1_Excuting);
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton1.OnClientClick = "return beforeSubmit(this)";
            this.StartWorkflowButton2.OnClientClick = "return beforeSubmit(this)";
            if (IsPostBack)
            {
                return;
            }
        }


        void StartWorkflowButton1_Excuting(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WorkflowPerson.IsCEO(this.DataForm1.Applicant.UserAccount))
            {
                DisplayMessage("当前流程不支持CEO提交申请，请重新填写！");
                e.Cancel = true;
                return;
            }

            bool isCheckOK = CheckData();
            if (!isCheckOK)//未能通过数据验证,如新日期，老日期为空，新日期等于老日期,所有的数据都 是Delivered
            {
                e.Cancel = true;
                return;
            }
            var btn = sender as StartWorkflowButton;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            SPUser ApplicantSPUser = SPContext.Current.Web.EnsureUser(this.DataForm1.Applicant.UserAccount);
            string sWorkflowNumber = HiddenFieldWorkflowNO.Value;
            if (string.IsNullOrEmpty(sWorkflowNumber))
            {
                sWorkflowNumber = CreateWorkFlowNumber();
                HiddenFieldWorkflowNO.Value = sWorkflowNumber;
            }
            fields["Title"] = sWorkflowNumber;
            fields["ApplicantSPUser"] = ApplicantSPUser;
            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                SaveDataToPADChangeRequest(sWorkflowNumber);
                WorkflowContext.Current.UpdateWorkflowVariable("isSave", true);
                WorkflowContext.Current.UpdateWorkflowVariable("isSubmit", false);
                WorkflowContext.Current.UpdateWorkflowVariable("isFree", false);
                WorkflowContext.Current.UpdateWorkflowVariable("completeTaskT", "PAD Change Request needs to Submit");
                WorkflowContext.Current.UpdateWorkflowVariable("editUrl", "/_Layouts/CA/WorkFlows/PADChangeRequest/BatchEditForm.aspx");
                fields["Status"] = CAWorkflowStatus.InProgress;
            }
            else//submit
            {
                bool IsNeedApprove = IsNeedWorkFlowStep();//是否需要工作流审批，只要有一个记录的原始日期大于新输入的日期,则需要进入工作流审批流程,
                if (!IsNeedApprove)//不需要工作流审批流程
                {
                    bool isUpdateSucc = UpdateToSAP(sWorkflowNumber);
                   if (isUpdateSucc)
                   {
                       SaveDataToPADChangeRequest(sWorkflowNumber);
                       WorkflowContext.Current.UpdateWorkflowVariable("isSubmit", true);
                       WorkflowContext.Current.UpdateWorkflowVariable("isFree", true);
                       WorkflowContext.Current.UpdateWorkflowVariable("isSave", false);
                       fields["Status"] = CAWorkflowStatus.Completed;
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
                    var managerEmp = SapCommonPADChangeRequest.GetApproverByLevelPAD(this.DataForm1.Applicant);// WorkFlowUtil.GetApproverByLevelPAD(this.DataForm1.Applicant);
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
                    fields["CurrManager"] = managerEmp.UserAccount;// WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString()))).UserAccount;
                    SaveDataToPADChangeRequest(sWorkflowNumber);
                }
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private string CreateWorkFlowNumber()
        {
            var department = this.DataForm1.Department;
            return "PAD" + department + WorkFlowUtil.CreateWorkFlowNumber("PADChangeRequest" + department).ToString("0000");
        }
        private void StartWorkflowButton_Executed(object sender, EventArgs e)
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
                            bool isOK = CheckExcelData(excelData);
                            if (isOK)
                            {
                                DataTable dt = CreateSAPPOData(excelData);
                                RepeaterPOData.DataSource = dt;
                                RepeaterPOData.DataBind();
                                SetRepeaterSelectedDate();
                                SetPOSearchTextBox(excelData);
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
            dtPO=CreateInputPOData(sPONOs.Split(','));
            DataTable dt = CreateSAPPOData(dtPO);
            RepeaterPOData.DataSource = dt;
            RepeaterPOData.DataBind();
            SetRepeaterSelectedDate();
            LabelCount.Text = dt.Rows.Count.ToString();
        }

        /// <summary>
        /// 设置Repeater里的CADateTimeFrom先中日期
        /// </summary>
        void SetRepeaterSelectedDate()
        {
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
               HiddenField HiddenFieldDate= item.FindControl("HiddenFieldDate") as HiddenField;
               CADateTimeControl CADateTimeFrom = item.FindControl("CADateTimeFrom") as CADateTimeControl;
               DateTime dt = DateTime.Now;
               if(DateTime.TryParse(HiddenFieldDate.Value,out dt))
               {
                   CADateTimeFrom.SelectedDate = dt;
               }
            }
        }

        /// <summary>
        /// 验证上传的Excel中的日期格式是否正确
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        bool CheckExcelData(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                DateTime dtDate=DateTime.Now;
                string sDate = dr[1].ToString();

                if (!DateTime.TryParse(sDate, out dtDate))
                {
                    sb.Append(string.Concat(dr[0], " has a error date format ", sDate, "\\n"));
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
            dt.Columns.Add("PONO");
            dt.Columns.Add("Date");
            dt.Columns.Add("PAD");
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
            
            List<string> listStr = new List<string>();

            StringBuilder sb = new StringBuilder();
            foreach (DataRow drPO in dtPO.Rows)
            {
                string sPONO = drPO[0].ToString().Trim() ;///去重复
                if (listStr.Contains(sPONO))
                {
                    continue;
                }
                listStr.Add(sPONO);

                SapCommonPADChangeRequest sapcommonpad = new SapCommonPADChangeRequest();
                if (sapcommonpad.SapSearchPAD(sPONO))
                {
                    DataRow dr = dt.NewRow();
                    dr["PONO"] = sPONO.Trim();
                    dr["Date"] = drPO[1].ToString();
                    dr["PAD"] = Convert.ToDateTime(sapcommonpad.PAD).ToString("MM\\/dd\\/yyyy");
                    dr["SupplierName"] = sapcommonpad.SupplierName.Trim();
                    dr["SADweek"] = sapcommonpad.SADweek.Trim();
                    dr["SADyear"] = sapcommonpad.SADyear.Trim();
                    dr["OMU"] = sapcommonpad.OMU.Trim();
                    dr["PADweek"] = sapcommonpad.PADweek.Trim();
                    dr["PADyear"] = sapcommonpad.PADyear.Trim();
                    dr["OSP"] = sapcommonpad.OSP.Trim();
                    dr["ValueForStory"] = sapcommonpad.ValueForStory.Trim();
                    dr["IsNeedApprove"] = sapcommonpad.Delivered;
                    dr["IsSuccess"] = 0;
                    dr["PoQty"] = sapcommonpad.SPOQTY.Trim();

                    int iStyleNO = 0;
                    int.TryParse(sapcommonpad.STYLENUMBER,out iStyleNO);
                    dr["StyleNumber"] = iStyleNO.ToString();// sapcommonpad.STYLENUMBER;
                    
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
        bool  CheckData()
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
                //HiddenFieldstoredelivery
                if (string.IsNullOrEmpty(LabelPAD.Text.Trim()))//原始日期为空 
                {
                    sbErorInfo.Append(string.Concat("Current PAD can not empty for ", sPONO, "\\n"));
                }
                DateTime  dtNew = CADateTimeFrom.SelectedDate;
                DateTime dtCurrent=DateTime.Now;
                if (DateTime.TryParse(LabelPAD.Text.Trim(), out dtCurrent))//新日期与原始日期相等
                {
                    if (dtCurrent == dtNew)
                    {
                        sbErorInfo.Append(string.Concat("Current PAD can not equal to Proposed New PAD  for ", sPONO,"\\n"));
                    }
                }
                else//原始日期格式不合法
                {
                    sbErorInfo.Append(string.Concat("Error Current PAD format for ", sPONO, "\\n"));
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
                dr["PONumber"] = LabelPONO.Text.Trim() ;
                dr["CurrentPAD"] =  LabelPAD.Text.Trim();
                dr["NewPAD"] = CADateTimeFrom.SelectedDate.ToString("yyyy-MM-dd");
                dr["SupplierName"] =  TextBoxSupplierName.Text.Trim();
                dr["PADyear"] = TextBoxPADYear.Text.Trim();
                dr["PADweek"] = TextBoxPADweek.Text.Trim();
                dr["OSP"] = TextBoxOSP.Text.Trim();
                dr["ValueForStory"] =TextBoxValueForStory.Text.Trim();
                dr["SADyear"] = TextBoxSADyear.Text.Trim();
                dr["SADweek"] =  TextBoxSADweek.Text.Trim();
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
                        sbErrorInfo.Append(string.Concat("Update ", sPONO, " to SAP failed,SAP error info:", sapcommonpad.ErrorMsg.Replace("'", "‘").Replace("\\n", "  "), "\\n"));
                    }
                    else
                    {
                        HiddenFieldIsSuccess.Value = "1";
                        CADateTimeFrom.Enabled = false;
                        sbSucessInfo.Append(string.Concat("\\nUpdate ", sPONO, " to SAP success"));
                    }
                }
            }
            if (sbErrorInfo.Length > 0)
            {
                DisplayMessage(string.Concat("There are some errors occoured:\\n" + sbErrorInfo.ToString(), sbSucessInfo.ToString()));
                //DisplayMessage("There are some errors occoured:\\n"+sbErrorInfo.ToString());
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

            List<string> listStr = new List<string>();
            foreach (string item in arrayPONO)
            {
                string sPONO = item.Trim();
                if (!listStr.Contains(sPONO))//去重复
                {
                    DataRow dr = dt.NewRow();
                    dr["PONO"] = sPONO;
                    dt.Rows.Add(dr);
                    listStr.Add(sPONO);
                }
            }
            return dt;
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
            decimal dValue = 0;
            decimal.TryParse(sValue, out dValue);
           return Math.Round(dValue,0).ToString();
        }
    }
}
