using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using QuickFlow.Core;
using System.Text;
using CA.WorkFlow.UI.PADChangeRequest;

namespace CA.WorkFlow.UI.POTypeChange
{
    public partial class DataView : BaseWorkflowUserControl
    {
        public bool isApproveStep = false;
        public bool isDisplayStep = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindData();
        }

        /// <summary>
        /// 绑定Workflow里的Item数据
        /// </summary>
        public void BindData()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sWorkflowNO = fields["Title"].ToString();

            Common comm = new Common();
            DataTable dt = new DataTable();
            dt = comm.GetData(sWorkflowNO);

            RepeaterPOData.DataSource = dt;
            RepeaterPOData.DataBind();
            LabelCount.Text = dt.Rows.Count.ToString();
            SetItemApproveStatus();
        }

        /// <summary>
        /// 更新到SAP
        /// </summary>
        /// <returns></returns>
        public bool UpdateToSAP()
        {
            StringBuilder sbError = new StringBuilder();
            StringBuilder sbSuccess = new StringBuilder();
            bool isAllSuccess = true;

            DataTable dtResult = new DataTable();
            DataTable dtPars = new DataTable();
            Common comm = new Common();

            dtPars = GetUpdatePars();
            dtResult= comm.UpdateOSPPrice(dtPars);
            foreach (DataRow dr in dtResult.Rows)
            {
                string sPONO = dr["PONO"] == null ? string.Empty : dr["PONO"].ToString();
                string sStatus=dr["Status"]==null?string.Empty:dr["Status"].ToString();
                if (sStatus == "1")//是更新成功的
                {
                    UpdateItemStaus(sPONO, true);
                    sbSuccess.Append(string.Format("PO No. {0} update successed \\n", sPONO));
                }
                else//更新失败。
                {
                    string sError = dr["ErrorInfo"] == null ? string.Empty : dr["ErrorInfo"].ToString();
                    sbError.Append(string.Format("PO No. {0} update failed,error info:{1} \\n", sPONO, sError));
                    isAllSuccess = false;
                    UpdateItemStaus(sPONO, false);
                }
            }

            string sPADUpdateError = UpdatePAD(dtPars);
            if (sPADUpdateError.Length > 0)
            {
                isAllSuccess = false;
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>alert('{0}\\n{1}\\n\\n{2}')</script>", sbError.ToString(), sPADUpdateError, sbSuccess.ToString()));
            return isAllSuccess;
        }

        /// <summary>
        /// 更新list里该 StyleNO的审批为True,SAP的更新状态。 
        /// </summary>
        /// <param name="sStyleNo"></param>
        /// <param name="isSuccess"></param>
        void UpdateItemStaus(string sStyleNo,bool isSuccess)
        {
            Common comm = new Common();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                Label LabelPONO = item.FindControl("LabelPONO") as Label;
                if (sStyleNo == LabelPONO.Text.Trim())
                {
                    HiddenField HiddenFieldID = item.FindControl("HiddenFieldID") as HiddenField;///当前Item的 ID 
                    HiddenField HiddenFieldISSuccess = item.FindControl("HiddenFieldISSuccess") as HiddenField;///当前Item的 ID 
                    comm.UpdateItemSapStatus(HiddenFieldID.Value, true, isSuccess);///更新list里该 StyleNO的审批为True,SAP的更新状态。 
                    HiddenFieldISSuccess.Value = isSuccess?"1":"0";
                }
            }
        }

        /// <summary>
        /// 得到更新到 SAP的参数 ,并将审批不通过数据修改到list
        /// </summary>
        /// <returns></returns>
        DataTable GetUpdatePars()
        {
            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("PONO");
            dtUpdate.Columns.Add("NewType");
            dtUpdate.Columns.Add("NewPAD");
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                DropDownList DropDownListApprove = item.FindControl("DropDownListApprove") as DropDownList;
                HiddenField HiddenFieldISSuccess = item.FindControl("HiddenFieldISSuccess") as HiddenField;
                HiddenField HiddenFieldIsPADSuccess = item.FindControl("HiddenFieldIsPADSuccess") as HiddenField;
                if (HiddenFieldISSuccess.Value == "0" || HiddenFieldIsPADSuccess.Value == "0")//没有更新成功
                {
                 
                    bool IsApproved = DropDownListApprove.SelectedValue == "1" ? true : false;
                    HiddenField HiddenFieldID = item.FindControl("HiddenFieldID") as HiddenField;///当前Item的 ID 
                    HiddenField HiddenFieldNewTypeValue = item.FindControl("HiddenFieldNewTypeValue") as HiddenField;///当前Type的Value
                    if (IsApproved)//审批通过,加入到要修改的集合中。 
                    {
                        Label LabelPONO = item.FindControl("LabelPONO") as Label;
                        Label LabelNewPAD = item.FindControl("LabelNewPAD") as Label;

                        DataRow dr = dtUpdate.NewRow();
                        dr["PONO"] = LabelPONO.Text;
                        if (HiddenFieldISSuccess.Value == "0")
                        {
                            dr["NewType"] = HiddenFieldNewTypeValue.Value.Trim();
                        }
                        if (HiddenFieldIsPADSuccess.Value == "0")
                        {
                            Label LabePAD = item.FindControl("LabePAD") as Label;
                            DateTime dtPAD = DateTime.Now;
                            DateTime dtNewPAD = DateTime.Now;
                            if (DateTime.TryParse(LabePAD.Text, out dtPAD) && DateTime.TryParse(LabelNewPAD.Text, out dtNewPAD))
                            {
                                if (dtPAD != dtNewPAD)
                                {
                                    dr["NewPAD"] = LabelNewPAD.Text.Trim();
                                }
                            }
                        }
                        dtUpdate.Rows.Add(dr);
                    }
                    else//审批不通过
                    {
                        Common comm = new Common();
                        comm.UpdateItemSapStatus(HiddenFieldID.Value, IsApproved, false);//将审批不通过修改到Item的状态 中 
                        HiddenFieldISSuccess.Value = "1";//页面上标注为己经更新。
                    }
                }
            }
            return dtUpdate;
        }


        public string GetApprovedSatus(string sIsSuccess)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (fields["Status"].ToString() == CAWorkflowStatus.InProgress || fields["Status"].ToString() == CAWorkflowStatus.Rejected)
            { 
                return CAWorkflowStatus.InProgress;
            }
            else
            {
                return sIsSuccess == "1" ? "Approved" : "Rejected";
            }
        }
        /// <summary>
        /// 将拒绝的Item更新到list
        /// </summary>
        public void UpdateItemApproveStatus()
        {
            Common com = new Common();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                HiddenField HiddenFieldID = item.FindControl("HiddenFieldID") as HiddenField;
                DropDownList DropDownListApprove = item.FindControl("DropDownListApprove") as DropDownList;
                if (DropDownListApprove.SelectedValue=="0")
                {
                    com.UpdateItemApproveStatus(HiddenFieldID.Value,false);
                }
            } 
        }

        /// <summary>
        /// 设置Item的审批状态
        /// </summary>
        void SetItemApproveStatus()
        {
            Common com = new Common();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                HiddenField HiddenFieldIsApproved = item.FindControl("HiddenFieldIsApproved") as HiddenField;
                DropDownList DropDownListApprove = item.FindControl("DropDownListApprove") as DropDownList;
                if (HiddenFieldIsApproved.Value == "0")
                {
                    DropDownListApprove.SelectedValue = "0";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        string UpdatePAD(DataTable dt)
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sWorkflowNo=fields["Title"].ToString();
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
                    UpdateItemPADState(sWorkflowNo, sPONO, true);
                }
            }
            return sbErrorInfo.ToString();
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
                Label LabelPONO = item.FindControl("LabelPONO") as Label;
                if (sPONO == LabelPONO.Text.Trim())
                {
                    HiddenField HiddenFieldIsPADSuccess = item.FindControl("HiddenFieldIsPADSuccess") as HiddenField;
                    comm.UpdatePADStatus(sWorkflowNo, sPONO, isPADSuccess);
                    HiddenFieldIsPADSuccess.Value = isPADSuccess ? "1" : "0";
                }
            }
        }


    }
}