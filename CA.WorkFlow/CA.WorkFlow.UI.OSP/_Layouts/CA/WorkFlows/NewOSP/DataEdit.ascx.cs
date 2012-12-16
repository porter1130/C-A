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

namespace CA.WorkFlow.UI.NewOSP
{
    public partial class DataEdit : BaseWorkflowUserControl
    {
        public bool bIsEdt = false;
        protected void Page_Load(object sender, EventArgs e)
        {
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
                        string libraryName = OSPCommon.WorkflowDocumentLibrary;
                        SPList list = SPContext.Current.Web.Lists[libraryName];
                        string filePath = FileUploadExcel.PostedFile.FileName;

                        string subFolder = "OSP";

                        SPFile file = list.RootFolder.SubFolders[subFolder].Files.Add(FileUploadExcel.FileName, FileUploadExcel.PostedFile.InputStream, true);

                        var fileName = file.Name;
                        fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                        var sheetNames = fileName.Split('_');
                        if (sheetNames.Length > 1)
                        {
                            string positionKeyValue = OSPCommon.GetExcelConfigInfo(OSPCommon.OSPPosition);
                            string primaryKeyValue = OSPCommon.GetExcelConfigInfo(OSPCommon.OSPPK);
                            string colsKeyValue = OSPCommon.GetExcelConfigInfo(OSPCommon.OSPCol);

                            //excelData = ExcelService.ParseExcel(file, "A,2", "A", "A,B", sheetNames[1]);
                            excelData = ExcelService.ParseExcel(file, positionKeyValue, primaryKeyValue, colsKeyValue, sheetNames[1]);
                            bool isOK = CheckExcelData(excelData);
                            if (isOK)
                            {
                                DataTable dtSeachCondition = GetSearchCondtionFromExcel(excelData);
                                BindRepeater(dtSeachCondition);
                                SetSearchText();
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
        /// <summary>
        /// 验证上传的Excel中的值是否正确
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        bool CheckExcelData(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                decimal dValue = 0;
                string sValue = dr[1].ToString();

                if (!decimal.TryParse(sValue, out dValue))
                {
                    sb.Append(string.Concat(dr[0], " has a error date formate ", sValue, "\\n"));
                }
            }
            if (sb.Length > 0)
            {
               // DisplayMessage(sb.ToString());
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
        /// <param name="dtExcel"></param>
        /// <returns></returns>
        DataTable GetSearchCondtionFromExcel(DataTable dtExcel)
        {
            List<string> listStr = new List<string>();
            DataTable dt = new DataTable();
            dt=IniteSearchCondition();

            StringBuilder sb = new StringBuilder();
            foreach (DataRow drPO in dtExcel.Rows)
            {
                string sStyleNO = drPO[0].ToString().Trim();///去重复
                if (listStr.Contains(sStyleNO))
                {
                    continue;
                }
                listStr.Add(sStyleNO);

                DataRow dr = dt.NewRow();
                dr["StyleNO"]=sStyleNO;
                dr["NewOSP"] = drPO[1].ToString().Trim();
                dt.Rows.Add(dr);
            }
            return dt;
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
            OSPCommon comm = new OSPCommon();

            listStyleNO = comm.GetDistingctList(listStyleNO);///去除重复的StyleNo.

            DataTable dtSearchConditon = IniteSearchCondition();
            foreach (string str in listStyleNO)
            {
                DataRow dr = dtSearchConditon.NewRow();
                dr["StyleNO"] = str;
                //dr["NewOSP"] = 100;
                dtSearchConditon.Rows.Add(dr);
            }
            BindRepeater(dtSearchConditon);
        }

        /// <summary>
        /// 绑定 Repeater
        /// </summary>
        /// <param name="dtSearchCondition"></param>
        void BindRepeater(DataTable dtSearchCondition)
        {
            OSPCommon comm = new OSPCommon();
            DataTable dt = new DataTable();
            dt = comm.GetOSPInfoFromSAP(dtSearchCondition);
            if (comm.sErrorMsg.Length > 0)
            {
                string sMessage = comm.sErrorMsg.ToString();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>alert('{0}')</script>", sMessage));
            }
            RepeaterPOData.DataSource = dt;
            RepeaterPOData.DataBind();
            LabelCount.Text = dt.Rows.Count.ToString();
        }

        /// <summary>
        /// 得到Repeater里的数据
        /// </summary>
        /// <param name="sWorkflowNumber"></param>
        /// <returns></returns>
        public DataTable GetResultDt(string sWorkflowNumber)
        {
            DataTable dt = new DataTable();
            OSPCommon comm = new OSPCommon();
            dt = comm.CreateData();
            StringBuilder sbErrorInfo = new StringBuilder();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                TextBox TextBoxNewOSP = item.FindControl("TextBoxNewOSP") as TextBox;
                Label LabelStyleNO = item.FindControl("LabelStyleNO") as Label;
                decimal dNewOSP = 0;
                if (!decimal.TryParse(TextBoxNewOSP.Text.Trim(), out dNewOSP))
                {
                    sbErrorInfo.Append(string.Format("Style No.:{0} ,NewOSP Value format error or empty ! \\n"));
                    continue;
                }
                Label LabelAllocatedDate = item.FindControl("LabelAllocatedDate") as Label;
                string sAllocatedDate = LabelAllocatedDate.Text.Trim();
                if (sAllocatedDate.Length > 0)
                {
                    continue;
                }

                Label LabelOriginalOsp = item.FindControl("LabelOriginalOsp") as Label;
                Label LabelPONO = item.FindControl("LabelPONO") as Label;
                Label LabelSubDiv = item.FindControl("LabelSubDiv") as Label;
                Label LabelClass = item.FindControl("LabelClass") as Label;
                Label LabelQty = item.FindControl("LabelQty") as Label;
                Label LabelCurrentOMU = item.FindControl("LabelCurrentOMU") as Label;
                Label LabelCreatedBy = item.FindControl("LabelCreatedBy") as Label;
                Label LabelPAD = item.FindControl("LabelPAD") as Label;
                Label LabelSAD = item.FindControl("LabelSAD") as Label;
                Label LabelGR = item.FindControl("LabelGR") as Label;
                HiddenField HiddenFieldISSuccess = item.FindControl("HiddenFieldISSuccess") as HiddenField;
                HiddenField HiddenFieldCost = item.FindControl("HiddenFieldCost") as HiddenField;
                decimal dNewOMU = 0;
                decimal dCost=0;
                DataRow dr = dt.NewRow();
                if (decimal.TryParse(HiddenFieldCost.Value, out dCost) && dNewOSP != 0)
                {
                    dNewOMU = ((dNewOSP - dCost) / dNewOSP)*100;
                    dr["NewOMU"] = dNewOMU.ToString("0.00")+"%";
                }

                decimal dOrigialOSP = 0;
                string sOMUReduction = string.Empty;
                decimal dQty = 0;
                if (decimal.TryParse(LabelOriginalOsp.Text.Trim(), out dOrigialOSP)&&decimal.TryParse(LabelQty.Text.Trim(), out dQty))
                {
                    sOMUReduction = Math.Round((dOrigialOSP - dNewOSP) * dQty,0).ToString();// ((dOrigialOSP - dNewOSP) * dQty)
                    dr["OMUReduction"] = sOMUReduction;
                }
                dr["Title"] = LabelStyleNO.Text.Trim();
                dr["WorkflowNumber"] = sWorkflowNumber;
                dr["Class"] = LabelClass.Text.Trim() ;
                dr["SubDiv"] = LabelSubDiv.Text.Trim();
                dr["PONO"] = LabelPONO.Text.Trim();
                dr["Qty"] = dQty.ToString();
                dr["OriginalOsp"] = dOrigialOSP.ToString();
                dr["NewOSP"] = dNewOSP;
                dr["CurrentOMU"] = LabelCurrentOMU.Text.Trim();
                dr["CreatedBy"] = LabelCreatedBy.Text.Trim();
                dr["PAD"] = LabelPAD.Text.Trim();
                dr["SAD"] = LabelSAD.Text.Trim();
                dr["GR"] = LabelGR.Text.Trim();
                dr["Cost"] = dCost;
                dr["AllocatedDate"] = sAllocatedDate;
                string sIsSuccess = HiddenFieldISSuccess.Value;
                dr["IsSuccess"] = sIsSuccess;
                if (sIsSuccess == "1")
                {
                    dr["IsApproved"] = "1";
                }

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
        public bool CheckData()
        {
            if (RepeaterPOData.Items.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>alert('No Data!')</script>"));
                return false;
            }
            StringBuilder sbErrorInfo = new StringBuilder();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                TextBox TextBoxNewOSP = item.FindControl("TextBoxNewOSP") as TextBox;
                Label LabelStyleNO = item.FindControl("LabelStyleNO") as Label;
                decimal dNewOSP = 0;
                if (!decimal.TryParse(TextBoxNewOSP.Text.Trim(), out dNewOSP))
                {
                    sbErrorInfo.Append(string.Format("Style No.:{0} ,NewOSP Value format error or empty ! \\n", LabelStyleNO.Text.Trim()));
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
            OSPCommon comm = new OSPCommon();
            DataTable dt = new DataTable();
            dt = comm.GetData(sWorkflowNO);

            RepeaterPOData.DataSource = dt;
            RepeaterPOData.DataBind();
            SetSearchText();
        }

        /// <summary>
        /// 构造查询条件时的DataTable
        /// </summary>
        /// <returns></returns>
        DataTable IniteSearchCondition()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StyleNO");
            dt.Columns.Add("NewOSP");
            return dt;
        }

        void SetSearchText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                Label LabelStyleNO = item.FindControl("LabelStyleNO") as Label;
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append(LabelStyleNO.Text.Trim());
            }
            TextBoxPONOs.Text = sb.ToString();
        }


        /// <summary>
        ///  工作流是否需要审批
        /// </summary>
        /// <returns></returns>
        public bool IsNotNeedApprove()
        {
            bool isNotNeedApprove = true;
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                Label LabelOriginalOsp = item.FindControl("LabelOriginalOsp") as Label;
                TextBox TextBoxNewOSP = item.FindControl("TextBoxNewOSP") as TextBox;
                HiddenField HiddenFieldISSuccess = item.FindControl("HiddenFieldISSuccess") as HiddenField;
                if (HiddenFieldISSuccess.Value == "1")
                {
                    continue;
                }
                
                decimal dOriginalOSP = 0;
                decimal dNewOSP = 0;
                decimal.TryParse(LabelOriginalOsp.Text.Trim(), out dOriginalOSP);
                decimal.TryParse(TextBoxNewOSP.Text.Trim(), out dNewOSP);
                if (dNewOSP < dOriginalOSP)
                {
                    isNotNeedApprove = false;
                    return isNotNeedApprove;
                }
            }
            return isNotNeedApprove;
        }

        /// <summary>
        /// 数字为Int格式。（去掉前面的0）
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public string IntFormate(string sValue)
        {
            decimal dValue = 0;
            decimal.TryParse(sValue, out dValue);
            return Math.Round(dValue, 0).ToString();
        }

        /// <summary>
        /// 更新到SAP，并返回是否全部更新成功
        /// </summary>
        /// <param name="sWorkflowNo"></param>
        /// <returns></returns>
        public bool UpdateToSAP(string sWorkflowNo)
        {
            StringBuilder sbError = new StringBuilder();
            StringBuilder sbSuccess = new StringBuilder();
            bool isAllSuccess = true;


            DataTable dtResult = new DataTable();
            DataTable dtPars = new DataTable();
            OSPCommon comm = new OSPCommon();

            dtPars = GetUpdatePars();
            dtResult = comm.UpdateOSPPrice(dtPars);
            foreach (DataRow dr in dtResult.Rows)
            {
                string sStyleNO = dr["StyleNO"] == null ? string.Empty : dr["StyleNO"].ToString();
                string sStatus = dr["Status"] == null ? string.Empty : dr["Status"].ToString();
                if (sStatus == "1")//是更新成功的
                {
                    UpdateItemStaus(sWorkflowNo, sStyleNO, true);
                    sbSuccess.Append(string.Format("Style No. {0} update successed \\n", sStyleNO));
                }
                else//更新失败。
                {
                    string sError = dr["ErrorInfo"] == null ? string.Empty : dr["ErrorInfo"].ToString();
                    sbError.Append(string.Format("Style No. {0} update failed,error info:{1} \\n", sStyleNO, sError));
                    isAllSuccess = false;
                    UpdateItemStaus(sWorkflowNo, sStyleNO, false);
                } 
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>alert('{0}\\n\\n{1}')</script>", sbError.ToString(), sbSuccess.ToString()));
            return isAllSuccess;
        }


        /// <summary>
        /// 得到需要更新到 SAP的参数 
        /// </summary>
        /// <returns></returns>
        DataTable GetUpdatePars()
        {
            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("StyleNO");
            dtUpdate.Columns.Add("NewOSP");
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                HiddenField HiddenFieldISSuccess = item.FindControl("HiddenFieldISSuccess") as HiddenField;
                if (HiddenFieldISSuccess.Value == "0")//没有更新成功
                {
                    Label LabelStyleNO = item.FindControl("LabelStyleNO") as Label;
                    TextBox TextBoxNewOSP = item.FindControl("TextBoxNewOSP") as TextBox;

                    DataRow dr = dtUpdate.NewRow();
                    dr["StyleNO"] = LabelStyleNO.Text;
                    dr["NewOSP"] = TextBoxNewOSP.Text.Trim();
                    dtUpdate.Rows.Add(dr);
                }
            }
            return dtUpdate;
        }


        /// <summary>
        /// 更新list里该 StyleNO的审批为True,SAP的更新状态。 
        /// </summary>
        /// <param name="sWorkflowNo"></param>
        /// <param name="sStyleNo"></param>
        /// <param name="isSuccess"></param>
        void UpdateItemStaus(string sWorkflowNo, string sStyleNo, bool isSuccess)
        {
            OSPCommon comm = new OSPCommon();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                Label LabelStyleNO = item.FindControl("LabelStyleNO") as Label;
                if (sStyleNo == LabelStyleNO.Text.Trim())
                {
                    HiddenField HiddenFieldISSuccess = item.FindControl("HiddenFieldISSuccess") as HiddenField;
                    comm.UpdateItemSapStatus(sWorkflowNo,sStyleNo, true, isSuccess);///更新list里该 StyleNO的审批为True,SAP的更新状态。 
                    HiddenFieldISSuccess.Value = isSuccess ? "1" : "0";
                }
            }
        }



    }
}