using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using QuickFlow.Core;
using System.Text;

namespace CA.WorkFlow.UI.OSP
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

            OSPCommon comm = new OSPCommon();
            DataTable dt = new DataTable();
            dt = comm.GetData(sWorkflowNO);

            RepeaterPOData.DataSource = dt;
            RepeaterPOData.DataBind();
            LabelCount.Text = dt.Rows.Count.ToString();
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
            OSPCommon comm = new OSPCommon();

            dtPars = GetUpdatePars();
            dtResult= comm.UpdateOSPPrice(dtPars);
            foreach (DataRow dr in dtResult.Rows)
            {
                string sStyleNO=dr["StyleNO"]==null?string.Empty:dr["StyleNO"].ToString();
                string sStatus=dr["Status"]==null?string.Empty:dr["Status"].ToString();
                if (sStatus == "1")//是更新成功的
                {
                    UpdateItemStaus(sStyleNO, true);
                    sbSuccess.Append(string.Format("Style No. {0} update successed \\n", sStyleNO));
                }
                else//更新失败。
                {
                    string sError = dr["ErrorInfo"] == null ? string.Empty : dr["ErrorInfo"].ToString();
                    sbError.Append(string.Format("Style No. {0} update failed,error info:{1} \\n", sStyleNO, sError));
                    isAllSuccess = false;
                    UpdateItemStaus(sStyleNO,false);
                }
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>alert('{0}\\n\\n{1}')</script>", sbError.ToString(), sbSuccess.ToString()));
            return isAllSuccess;
        }

        /// <summary>
        /// 更新list里该 StyleNO的审批为True,SAP的更新状态。 
        /// </summary>
        /// <param name="sStyleNo"></param>
        /// <param name="isSuccess"></param>
        void UpdateItemStaus(string sStyleNo,bool isSuccess)
        {
            OSPCommon comm = new OSPCommon();

            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                Label LabelStyleNO = item.FindControl("LabelStyleNO") as Label;
                if (sStyleNo == LabelStyleNO.Text.Trim())
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
            dtUpdate.Columns.Add("StyleNO");
            dtUpdate.Columns.Add("NewOSP");
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                DropDownList DropDownListApprove = item.FindControl("DropDownListApprove") as DropDownList;
                HiddenField HiddenFieldISSuccess = item.FindControl("HiddenFieldISSuccess") as HiddenField;
                if (HiddenFieldISSuccess.Value == "0")//没有更新成功
                {
                    bool IsApproved = DropDownListApprove.SelectedValue == "1" ? true : false;
                    HiddenField HiddenFieldID = item.FindControl("HiddenFieldID") as HiddenField;///当前Item的 ID 
                    if (IsApproved)//审批通过,加入到要修改的集合中。 
                    {
                        Label LabelStyleNO = item.FindControl("LabelStyleNO") as Label;
                        Label LabelNewOSP = item.FindControl("LabelNewOSP") as Label;

                        DataRow dr = dtUpdate.NewRow();
                        dr["StyleNO"] = LabelStyleNO.Text;
                        dr["NewOSP"] = LabelNewOSP.Text.Trim();
                        dtUpdate.Rows.Add(dr);
                    }
                    else//审批不通过
                    {
                        OSPCommon comm = new OSPCommon();
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
            if (fields["Status"].ToString() == CAWorkflowStatus.InProgress)
            { 
                return CAWorkflowStatus.InProgress;
            }
            else
            {
                return sIsSuccess == "1" ? "Approved" : "Rejected";
            }
        }
    }
}