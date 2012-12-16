using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Microsoft.SharePoint;
using System.Text;
using CA.WorkFlow.UI.Code;
using CA.WorkFlow.UI.PADChangeRequest;

namespace CA.WorkFlow.UI.POTypeChange
{
    public partial class DataEdit : BaseWorkflowUserControl
    {
        public bool bIsEdt = false;
        public bool IsAllocated = false;
        string sCurrentEmployee;

        public string SCurrentEmployee
        {
            get { return sCurrentEmployee; }
            set { sCurrentEmployee = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            IsAllocated = !IsNeedApprove();
            if (IsPostBack)
            {
                return;
            }
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
                        string libraryName = Common.WorkflowDocumentLibrary;
                        SPList list = SPContext.Current.Web.Lists[libraryName];
                        string filePath = FileUploadExcel.PostedFile.FileName;

                        string subFolder = "POTypeChange";

                        SPFile file = list.RootFolder.SubFolders[subFolder].Files.Add(FileUploadExcel.FileName, FileUploadExcel.PostedFile.InputStream, true);

                        var fileName = file.Name;
                        fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                        var sheetNames = fileName.Split('_');
                        if (sheetNames.Length > 1)
                        {
                            string positionKeyValue = Common.GetExcelConfigInfo(Common.POTypeChangePosition);
                            string primaryKeyValue = Common.GetExcelConfigInfo(Common.POTypeChangePK);
                            string colsKeyValue = Common.GetExcelConfigInfo(Common.POTypeChangeCol);

                            excelData = ExcelService.ParseExcel(file, positionKeyValue, primaryKeyValue, colsKeyValue, sheetNames[1]);
                            if (excelData != null && excelData.Rows.Count>0)
                            {
                                List<string> listSeachCondition = GetSearchCondtionFromExcel(excelData);
                                BindRepeater(listSeachCondition);
                                SetSearchText();
                                BindAndSetType();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonUtil.logError("PO Type Change .Upload or read file fail, error:" + ex.Message);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>alert('Error excel template')</script>"));
                        //throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 从PO集合得到PO信息数据集
        /// </summary>
        /// <param name="dtExcel"></param>
        /// <returns></returns>
        List<string> GetSearchCondtionFromExcel(DataTable dtExcel)
        {
            List<string> listStr = new List<string>();
            foreach (DataRow drPO in dtExcel.Rows)
            {
                string sPONO = drPO[0].ToString().Trim();///去重复
                if (listStr.Contains(sPONO))
                {
                    continue;
                }
                listStr.Add(sPONO);
            }
            return listStr;
        }

        protected void ButtonBatchSearch_Click(object sender, EventArgs e)
        {
            string sPONOs = TextBoxPONOs.Text.Trim();
            if (sPONOs.Length == 0)
            {
                return;
            }

            List<string> listStyleNO = new List<string>();
            listStyleNO = sPONOs.Split(',').ToList();
            Common comm = new Common();
            listStyleNO = comm.GetDistingctList(listStyleNO);///去除重复的StyleNo.
            BindRepeater(listStyleNO);
            BindAndSetType();
        }

        /// <summary>
        /// 绑定 Repeater
        /// </summary>
        /// <param name="listSeachCondition"></param>
        void BindRepeater(List<string> listSeachCondition)
        {
            Common comm = new Common();
            DataTable dt = new DataTable();
            dt = comm.GetPOChangeTypeInfoFromSAP(listSeachCondition);
            if (comm.sErrorMsg.Length > 0)
            {
                string sMessage = comm.sErrorMsg.ToString();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>alert('{0}')</script>", sMessage));
            }
            RepeaterPOData.DataSource = dt;
            RepeaterPOData.DataBind();
            LabelCount.Text = dt.Rows.Count.ToString();
            SetPADData();
        }

        /// <summary>
        /// 得到Repeater里的数据
        /// </summary>
        /// <param name="sWorkflowNumber"></param>
        /// <returns></returns>
        public DataTable GetResultDt(string sWorkflowNumber)
        {
            DataTable dt = new DataTable();
            Common comm = new Common();
            dt = comm.CreateData();
            StringBuilder sbErrorInfo = new StringBuilder();
            bool isAllocation= !IsNeedApprove();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                Label LabelTitle = item.FindControl("LabelTitle") as Label;
                Label LabePAD = item.FindControl("LabePAD") as Label;
                Label LabelSAD = item.FindControl("LabelSAD") as Label;
                Label LabelOMU = item.FindControl("LabelOMU") as Label;
                Label LabelQty = item.FindControl("LabelQty") as Label;
                HiddenField HiddenFieldISSuccess = item.FindControl("HiddenFieldISSuccess") as HiddenField;
                DropDownList DDLType = item.FindControl("DDLType") as DropDownList;
                HiddenField HiddenFieldIsPADSuccess = item.FindControl("HiddenFieldIsPADSuccess") as HiddenField;
                CADateTimeControl CADateTimePAD = item.FindControl("CADateTimePAD") as CADateTimeControl;
                Label LabelIsAllocated = item.FindControl("LabelIsAllocated") as Label;
                
                string sPONO = LabelTitle.Text.Trim();
                if (CADateTimePAD.IsDateEmpty)
                {
                    sbErrorInfo.Append("New PAD can not be empty for PO No:");
                    sbErrorInfo.Append(sPONO);
                    sbErrorInfo.Append("\\n");
                    continue;
                }

                DateTime dtPAD = DateTime.Now;
                if (!DateTime.TryParse(LabePAD.Text, out dtPAD))
                {
                    sbErrorInfo.Append("PAD error for PO No:");
                    sbErrorInfo.Append(sPONO);
                    sbErrorInfo.Append("\\n");
                    continue;
                }

                DateTime dtSAD = DateTime.Now;
                if (!DateTime.TryParse(LabelSAD.Text, out dtSAD))
                {
                    sbErrorInfo.Append("PAD error for PO No:");
                    sbErrorInfo.Append(sPONO);
                    sbErrorInfo.Append("\\n");
                    continue;
                }

                TimeSpan tsPAD = DateTime.Parse(dtPAD.ToString()) - DateTime.Now;
                if (tsPAD.Days < 7 && !isAllocation)
                {
                    sbErrorInfo.Append("PO type change is only allowed 7 days before the PAD for PO No:");
                    sbErrorInfo.Append(sPONO);
                    sbErrorInfo.Append("\\n");
                    continue; 
                }
                string sAllocated = LabelIsAllocated.Text;
                if (sAllocated.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    sbErrorInfo.Append(sPONO);
                    sbErrorInfo.Append(" can not be allocated!");
                    sbErrorInfo.Append("\\n");
                    continue; 
                }

                //CADateTimePAD
                DataRow dr = dt.NewRow();
                dr["Title"] = sPONO;
                dr["WorkflowNumber"] = sWorkflowNumber;
                dr["PAD"] = dtPAD.ToShortDateString();
                dr["SAD"] = dtSAD.ToShortDateString();// LabelSAD.Text.Trim();
                dr["OMU"] = LabelOMU.Text.Trim();
                dr["Qty"] = LabelQty.Text.Trim();
                dr["IsSuccess"] = HiddenFieldISSuccess.Value.Trim();
                dr["NewTypeValue"] = DDLType.SelectedValue;
                dr["NewType"] = DDLType.SelectedItem.Text.Trim();
                dr["IsPADSuccess"] = HiddenFieldIsPADSuccess.Value;
                dr["NewPAD"] = CADateTimePAD.SelectedDate.ToShortDateString();
                dr["IsAllocated"] = sAllocated;
                //LabelIsAllocated   
                dt.Rows.Add(dr);
            }
            if (sbErrorInfo.Length > 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>alert('{0}')</script>", sbErrorInfo.ToString()));
                return null;
            }
            else
            {
                return dt;
            }
        }

        /// <summary>
        /// 验证Repeater里的数据是否合法
        /// </summary>
        /// <returns></returns>
        public bool CheckData()  //xu
        {
            if (RepeaterPOData.Items.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>alert('No Data!')</script>"));
                return false;
            }
            StringBuilder sbErrorInfo = new StringBuilder();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                DropDownList DDLType = item.FindControl("DDLType") as DropDownList;
                Label LabelTitle = item.FindControl("LabelTitle") as Label;
                if (DDLType.SelectedIndex==0)
                {
                    sbErrorInfo.Append(string.Format("Please select New PO Type for {0}! \\n", LabelTitle.Text.Trim()));
                    continue;
                }
            }
            if (sbErrorInfo.Length > 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>alert('{0}')</script>", sbErrorInfo.ToString()));
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 绑定Workflow里的Item数据
        /// </summary>
        /// <param name="sWorkflowNO"></param>
        public void BindData(string sWorkflowNO)
        {
            Common comm = new Common();
            DataTable dt = new DataTable();
            dt = comm.GetData(sWorkflowNO);

            RepeaterPOData.DataSource = dt;
            RepeaterPOData.DataBind();
            SetSearchText();
            BindAndSetType();
            LabelCount.Text = dt.Rows.Count.ToString();
        }

        /// <summary>
        /// 构造查询条件时的DataTable
        /// </summary>
        /// <returns></returns>
        DataTable IniteSearchCondition()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PONO");
            return dt;
        }

        /// <summary>
        /// 设置搜索框里面文字
        /// </summary>
        void SetSearchText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                Label LabelPONO = item.FindControl("LabelTitle") as Label;
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append(LabelPONO.Text.Trim());
            }
            TextBoxPONOs.Text = sb.ToString();
        }


        /// <summary>
        ///  工作流是否需要审批
        /// </summary>
        /// <returns></returns>
        public bool IsNeedApprove()
        {
            return !WorkFlowUtil.IsInGroup(sCurrentEmployee, "wf_Allocation");
        }

        /// <summary>
        /// 更新到SAP，并返回是否全部更新成功
        /// </summary>
        /// <param name="sWorkflowNo"></param>
        /// <returns></returns>
        public bool UpdateToSAP(string sWorkflowNo,ref List<string> lisSucPO)
        { 
            StringBuilder sbError = new StringBuilder();
            StringBuilder sbSuccess = new StringBuilder();
            bool isAllSuccess = true;


            DataTable dtResult = new DataTable();
            DataTable dtPars = new DataTable();
            Common comm = new Common();

            dtPars = GetUpdatePars();
            dtResult = comm.UpdateOSPPrice(dtPars);// xu???
            foreach (DataRow dr in dtResult.Rows)
            {
                string sPONO = dr["PONO"] == null ? string.Empty : dr["PONO"].ToString();
                string sStatus = dr["Status"] == null ? string.Empty : dr["Status"].ToString();
                if (sStatus == "1")//是更新成功的
                {
                    UpdateItemStaus(sWorkflowNo, sPONO, true);
                    lisSucPO.Add(sPONO);
                    sbSuccess.Append(string.Format("PO No. {0} update successed \\n", sPONO));
                }
                else//更新失败。
                {
                    string sError = dr["ErrorInfo"] == null ? string.Empty : dr["ErrorInfo"].ToString();
                    sbError.Append(string.Format("Style No. {0} update failed,error info:{1} \\n", sPONO, sError));
                    isAllSuccess = false;
                    UpdateItemStaus(sWorkflowNo, sPONO, false);
                } 
            }
            string sPADUpdateError= UpdatePAD(sWorkflowNo, dtPars);
            if (sPADUpdateError.Length > 0)
            {
                isAllSuccess = false;
                CommonUtil.logError("PoTypeChangeWorkflow update PAD to sap error:" + sPADUpdateError);
            }
            if (sbError.Length > 0)
            {
                CommonUtil.logError("PoTypeChangeWorkflow update to sap error:" + sbError.ToString());
            }
            
            
            return isAllSuccess;
        }

        /// <summary>
        /// sWorkflowNo
        /// </summary>
        /// <param name="sWorkflowNo"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        string UpdatePAD(string sWorkflowNo, DataTable dt)
        {
            StringBuilder sbErrorInfo = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["NewPAD"] == null || dr["NewPAD"].ToString().Trim().Length == 0)
                {
                    continue;
                }
                string sPONO = dr["PONO"].ToString();
                string sPAD = dr["NewPAD"].ToString();

                SapCommonPADChangeRequest sapcommonpad = new SapCommonPADChangeRequest();
                if (!sapcommonpad.SapUpdatePAD(sPONO, Convert.ToDateTime(sPAD).ToString("yyyy-MM-dd")))
                {
                    sbErrorInfo.Append(string.Concat("Update ", sPONO, " to SAP failed,error info:", sapcommonpad.ErrorMsg.Replace("'", "‘").Replace("\n", "  "), "\\n"));
                }
                else
                {
                    UpdateItemPADState(sWorkflowNo, sPONO,true);
                }
            }
            return sbErrorInfo.ToString();
        }


        /// <summary>
        /// 得到需要更新到 SAP的参数 
        /// </summary>
        /// <returns></returns>
        DataTable GetUpdatePars()
        {
            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("PONO");
            dtUpdate.Columns.Add("NewType");
            dtUpdate.Columns.Add("NewPAD");
            bool isAllocation = !IsNeedApprove();

            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                HiddenField HiddenFieldISSuccess = item.FindControl("HiddenFieldISSuccess") as HiddenField;
                HiddenField HiddenFieldIsPADSuccess = item.FindControl("HiddenFieldIsPADSuccess") as HiddenField;
                bool ISSuccess=false;
                bool IsPADSuccess = false;
                if (HiddenFieldISSuccess.Value == "True" || HiddenFieldISSuccess.Value == "1")
                {
                    ISSuccess = true;
                }

                if (HiddenFieldIsPADSuccess.Value == "True" || HiddenFieldIsPADSuccess.Value == "1")
                {
                    IsPADSuccess = true;
                }

                if (!ISSuccess || !IsPADSuccess)//没有更新成功
                {
                    Label LabelTitle = item.FindControl("LabelTitle") as Label;

                    DataRow dr = dtUpdate.NewRow();
                    dr["PONO"] = LabelTitle.Text.Trim();
                    if (!ISSuccess)
                    {
                    DropDownList DDLType = item.FindControl("DDLType") as DropDownList;
                        dr["NewType"] = DDLType.SelectedValue;
                    }
                    if (!IsPADSuccess)
                    {
                        CADateTimeControl CADateTimePAD = item.FindControl("CADateTimePAD") as CADateTimeControl;
                        Label LabePAD = item.FindControl("LabePAD") as Label;
                        DateTime dtPAD = DateTime.Now;
                        if (DateTime.TryParse(LabePAD.Text, out dtPAD))
                        {
                            if (CADateTimePAD.SelectedDate != dtPAD)
                            {
                                dr["NewPAD"] = CADateTimePAD.SelectedDate.ToShortDateString();
                            }
                        }
                    }
                    dtUpdate.Rows.Add(dr);
                }
            }
            return dtUpdate;
        }


        /// <summary>
        /// 更新list里该 StyleNO的审批为True,SAP的更新状态。 
        /// </summary>
        /// <param name="sWorkflowNo"></param>
        /// <param name="sPONO"></param>
        /// <param name="isSuccess"></param>
        /// <param name="isPADSuccess"></param>
        void UpdateItemStaus(string sWorkflowNo, string sPONO, bool isSuccess)
        {
            Common comm = new Common();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                Label LabelPONO = item.FindControl("LabelTitle") as Label;
                if (sPONO == LabelPONO.Text.Trim())
                {
                    HiddenField HiddenFieldISSuccess = item.FindControl("HiddenFieldISSuccess") as HiddenField;
                    HiddenField HiddenFieldIsPADSuccess = item.FindControl("HiddenFieldIsPADSuccess") as HiddenField;
                    //comm.UpdateItemSapStatus(sWorkflowNo, sPONO, true, isSuccess);///更新list里该 StyleNO的审批为True,SAP的更新状态。 
                    comm.UpdateItemSapStatus(sWorkflowNo, sPONO, true,isSuccess);
                    HiddenFieldISSuccess.Value = isSuccess ? "True" : "False";
                }
            }
        }
        
        /// <summary>
        /// 更新list里该 StyleNO的IsPADSuccess的更新状态。 
        /// </summary>
        /// <param name="sWorkflowNo"></param>
        /// <param name="sPONO"></param>
        /// <param name="IsPADSuccess"></param>
        void UpdateItemPADState(string sWorkflowNo, string sPONO, bool isPADSuccess)
        {
            Common comm = new Common();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                Label LabelPONO = item.FindControl("LabelTitle") as Label;
                if (sPONO == LabelPONO.Text.Trim())
                {
                    HiddenField HiddenFieldIsPADSuccess = item.FindControl("HiddenFieldIsPADSuccess") as HiddenField;
                    comm.UpdatePADStatus(sWorkflowNo, sPONO, isPADSuccess);
                    HiddenFieldIsPADSuccess.Value = isPADSuccess ? "True" : "False";
                }
            }
        }



        /// <summary>
        /// 绑定和设置Type的值
        /// </summary>
        public void BindAndSetType()
        {
            bool isAllocation = !IsNeedApprove(); //WorkFlowUtil.IsInGroup(sCurrentEmployee, "wf_Allocation");
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                DropDownList DDLType = item.FindControl("DDLType") as DropDownList;
                HiddenField HiddenFieldNewTypeValue = item.FindControl("HiddenFieldNewTypeValue") as HiddenField;
                HiddenField HiddenFieldNewPAD = item.FindControl("HiddenFieldNewPAD") as HiddenField;
                CADateTimeControl CADateTimePAD = item.FindControl("CADateTimePAD") as CADateTimeControl;

                string sTypeValue = HiddenFieldNewTypeValue.Value.Trim();
                if (isAllocation)
                {
                    DDLType.DataSource = Common.AllocationChangeType();
                }
                else
                {
                    DDLType.DataSource = Common.ChangeType();
                }
                DDLType.DataTextField = "Name";
                DDLType.DataValueField = "Value";
                DDLType.DataBind();
                DDLType.Items.Insert(0, "");
                
                if (sTypeValue.Length != 0)
                {
                    DDLType.SelectedValue = sTypeValue;
                }

                DateTime dtPAD = DateTime.Now;
                if (DateTime.TryParse(HiddenFieldNewPAD.Value, out dtPAD))
                {
                    CADateTimePAD.SelectedDate = dtPAD;
                }
            }
        }
        /// <summary>
        /// 设置NewPAD的时间默认为PAD
        /// </summary>
        void SetPADData()
        {
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                CADateTimeControl CADateTimePAD = item.FindControl("CADateTimePAD") as CADateTimeControl;
                Label LabePAD = item.FindControl("LabePAD") as Label;
                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(LabePAD.Text, out dt))
                {
                    CADateTimePAD.SelectedDate = dt;
                }
            }
        }

        /// <summary>
        /// 得到日期格式
        /// </summary>
        /// <param name="sDate"></param>
        /// <returns></returns>
        public string ConvertDateStr(object sDate)
        {
            if (null == sDate)
            {
                return string.Empty;
            }

            DateTime dt = DateTime.Now;
            if (DateTime.TryParse(sDate.ToString(), out dt))
            {
                sDate = dt.ToString("d", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            return sDate.ToString();
        }

    }
}