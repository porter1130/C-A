using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using QuickFlow.UI.Controls;
using QuickFlow.Core;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using CA.SharePoint;
using System.Data;
using System.Text;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.UI.PADChangeRequest
{
    public partial class BatchApproveForm : CAWorkFlowPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (!SecurityValidate(uTaskId, uListGUID, uID, true) && !SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            {
                RedirectToTask();
            }
            this.Actions.ActionExecuting += new EventHandler<QuickFlow.UI.Controls.ActionEventArgs>(Actions_ActionExecuting);
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;


            if (!this.Page.IsPostBack)
            {
                this.Trace1.GridLines = GridLines.Horizontal;
                this.Trace1.BorderStyle = BorderStyle.Solid;
                BindData();
            }
        }

        /// <summary>
        /// 是否出现供审核的DropDownList
        /// </summary>
        /// <param name="isItemNeedApprove"></param>
        /// <returns></returns>
        public bool IsNeeddApprove()
        {
            if (WorkflowContext.Current.Step.Equals("SuperManagerApprove"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            SapCommonPADChangeRequest sapcommonpad = new SapCommonPADChangeRequest();
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sWorkflowNumber = fields["Title"].ToString();
            switch (WorkflowContext.Current.Step)
            {
                case "ManagerApprove":
                    if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
                    {  
                        ///现审请者的manager为4，则结束流程
                        ///现审请者的manager为5，现审请者的manager的manager为4以上，则结束

                        bool isNeedBD = true;

                        Employee eCurrManager=UserProfileUtil.GetEmployeeEx(fields["CurrManager"].ToString());
                        int iCurrLevel = SapCommonPADChangeRequest.GetLevel(eCurrManager.JobLevel);

                        if (iCurrLevel == 4)//当前审批者的级别为4,不需要审批
                        {
                            isNeedBD = false;
                        }
                        else////当前审请者的级别为5
                        { 
                           // QuickFlow.NameCollection BDMApprover = new QuickFlow.NameCollection();
                            Employee eBD = WorkFlowUtil.GetNextApprover(eCurrManager);
                            if (null == eBD)//当前审批者没有manager,不需要审批
                            {
                                isNeedBD = false;
                            }
                            else
                            {
                                int iLevel = SapCommonPADChangeRequest.GetLevel(eBD.JobLevel.AsString());
                                if (iLevel != 4)////当前审批者的manager不为4,不需要审批
                                {
                                    isNeedBD = false;
                                }
                            
                            }
                        }
                        if (!isNeedBD)//不需要下一级审批
                        {
                            if (UpdateToSAP(sWorkflowNumber))
                            {
                                WorkflowContext.Current.UpdateWorkflowVariable("isOnlyApp", true);
                                WorkflowContext.Current.UpdateWorkflowVariable("updateResult", true);

                                //fields["CurrManager"] = WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString()))).UserAccount;
                                fields["Approvers"] = ReturnAllApprovers(fields["CurrManager"].ToString());
                                fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["CurrManager"].ToString());
                                if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(fields["CurrManager"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                {
                                    fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", SPContext.Current.Web.CurrentUser.LoginName);
                                }

                                fields["Status"] = CAWorkflowStatus.Completed;
                                SendNoticeMail("approved");
                            }
                            else
                            {
                                //DisplayMessage("更新SAP数据失败，请联系IT人员或稍后审批.Error:" + sapcommonpad.ErrorMsg.Replace("'", "‘").Replace("\\n", "  "));
                                e.Cancel = true;
                                return;
                            }
                        }
                        else///需要经过SuperManagerApprove审批
                        {
                            QuickFlow.NameCollection SuperApproveUser = new QuickFlow.NameCollection();
                            //var applicant = WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString());
                            Employee supmanagerEmp = WorkFlowUtil.GetNextApprover(eCurrManager);// WorkFlowUtil.GetApproverIgnoreRight(WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(applicant)));

                            if (supmanagerEmp == null)
                            {
                                DisplayMessage("此用户没有Level-4级审批用户，无法提交");
                                e.Cancel = true;
                                return;
                            }
                            SuperApproveUser.Add(supmanagerEmp.UserAccount);
                            var deleman = WorkFlowUtil.GetDeleman(supmanagerEmp.UserAccount, "127");
                            if (deleman != null)
                            {
                                SuperApproveUser.Add(deleman);
                            }
                            WorkflowContext.Current.UpdateWorkflowVariable("isOnlyApp", false);
                            WorkflowContext.Current.UpdateWorkflowVariable("secApproveU", SuperApproveUser);
                            WorkflowContext.Current.UpdateWorkflowVariable("SuperManagerT", "PAD Change Request needs to Approve");
                            WorkflowContext.Current.UpdateWorkflowVariable("approveUrl", "/_Layouts/CA/WorkFlows/PADChangeRequest/BatchApproveForm.aspx");
                           
                            fields["Approvers"] = ReturnAllApprovers(fields["CurrManager"].ToString());
                            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["CurrManager"].ToString());
                            if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(fields["CurrManager"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", SPContext.Current.Web.CurrentUser.LoginName);
                            }
                            UpdateItemApprove();
                            fields["Status"] = CAWorkflowStatus.InProgress;
                            fields["CurrManager"] = supmanagerEmp.UserAccount;// WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString()))).UserAccount;
                        }
                    }
                    else//Reject
                    {
                        UpdateItemReject();
                       // fields["CurrManager"] = CurrentEmployee.UserAccount;// WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString()))).UserAccount;
                        fields["Status"] = CAWorkflowStatus.Completed;
                        fields["Approvers"] = ReturnAllApprovers(fields["CurrManager"].ToString());
                        fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["CurrManager"].ToString());
                        SendNoticeMail("rejected");
                    }
                    break;
                case "SuperManagerApprove":
                    if (e.Action.Equals("Approve", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (UpdateToSAP(sWorkflowNumber))
                        {
                            WorkflowContext.Current.UpdateWorkflowVariable("updateResult", true);

                            var applicant = WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString());
                            var supmanagerEmp = WorkFlowUtil.GetApproverIgnoreRight(WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(applicant)));
                           // fields["CurrManager"] = supmanagerEmp.UserAccount;
                            fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["CurrManager"].ToString());
                            if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(fields["CurrManager"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", SPContext.Current.Web.CurrentUser.LoginName);
                            }
                            fields["Status"] = CAWorkflowStatus.Completed;
                            SendNoticeMail("approved");
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        UpdateItemReject();
                       // fields["CurrManager"] = CurrentEmployee.UserAccount;// WorkFlowUtil.GetApproverByLevelPAD(UserProfileUtil.GetEmployeeEx(WorkFlowUtil.GetApplicantAccount(WorkflowContext.Current.DataFields["Applicant"].ToString()))).UserAccount;
                        fields["Status"] = CAWorkflowStatus.Completed;
                        fields["Approvers"] = ReturnAllApprovers(fields["CurrManager"].ToString());
                        fields["ApproversSPUser"] = ReturnAllApproversSP("ApproversSPUser", fields["CurrManager"].ToString());
                        SendNoticeMail("rejected");
                    }
                    break;
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
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
                SetItemApproveState();
                LabelCount.Text = dt.Rows.Count.ToString();
            }
        }
        /// <summary>
        /// 设置每个 PO的Approve状态
        /// </summary>
        void SetItemApproveState()
        {
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                HiddenField HiddenFieldIsNeedApprove = item.FindControl("HiddenFieldIsNeedApprove") as HiddenField;
                if (HiddenFieldIsNeedApprove.Value != "1")
                {
                    DropDownList DDLApproveStatus = item.FindControl("DDLApproveStatus") as DropDownList;
                    DDLApproveStatus.SelectedIndex = 1;
                }
            }
        }

        /// <summary>
        /// 修改item的状态，是通过还是拒绝
        /// </summary>
        void UpdateItemApprove()
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    web.AllowUnsafeUpdates = true;
                    foreach (RepeaterItem item in RepeaterPOData.Items)
                    {
                        HiddenField HiddenFieldID = item.FindControl("HiddenFieldID") as HiddenField;
                        DropDownList DDLApproveStatus = item.FindControl("DDLApproveStatus") as DropDownList;
                        bool isApprove = DDLApproveStatus.SelectedValue == "1" ? true : false;

                        string sID = HiddenFieldID.Value;
                        SPQuery query = new SPQuery();
                        query.Query = string.Format(@"<Where>
                                          <Eq>
                                             <FieldRef Name='ID' />
                                             <Value Type='Counter'>{0}</Value>
                                          </Eq>
                                       </Where>", sID);
                        SPListItemCollection splic = web.Lists["PADChangeRequestItems"].GetItems(query);
                        foreach (SPListItem listItem in splic)
                        {
                            listItem["IsApprove"] = isApprove;
                            listItem.Update();
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 数据更新到SAP
        /// </summary>
        /// <param name="sWorkFlowNO"></param>
        /// <returns></returns>
        bool UpdateToSAP(string sWorkFlowNO)
        {
            bool IsAllSeccuss = true;

            if (RepeaterPOData.Items.Count == 0)
            {
                return true;
            }

            StringBuilder sbErrorInfo = new StringBuilder();
            StringBuilder sbSucessInfo = new StringBuilder();
            foreach (RepeaterItem item in RepeaterPOData.Items)
            {
                Label LabelNewPAD = item.FindControl("LabelNewPAD") as Label;
                DropDownList DDLApproveStatus = item.FindControl("DDLApproveStatus") as DropDownList;

                if (DDLApproveStatus.SelectedValue == "1")
                {
                    Label LabelPONO = item.FindControl("LabelPONO") as Label;
                    string sPONO = LabelPONO.Text.Trim();
                    SapCommonPADChangeRequest sapcommonpad = new SapCommonPADChangeRequest();
                    HiddenField HiddenFieldIsSuccess = item.FindControl("HiddenFieldIsSuccess") as HiddenField;
                    if (HiddenFieldIsSuccess.Value == "1")
                    {
                        continue;
                    }
                    if (!sapcommonpad.SapUpdatePAD(LabelPONO.Text.ToString(), Convert.ToDateTime(LabelNewPAD.Text.Trim()).ToString("yyyy-MM-dd")))//更新到 SAP失败
                    {
                        IsAllSeccuss = false;
                        sbErrorInfo.Append(string.Concat("Update ", sPONO, " to SAP failed,error info:", sapcommonpad.ErrorMsg.Replace("'", "‘").Replace("\n", "  "), "\\n"));
                    }
                    else
                    {
                        sbSucessInfo.Append(string.Concat("\\nUpdate ", sPONO, " to SAP success"));
                        HiddenFieldIsSuccess.Value = "1";
                        bool isApprove = DDLApproveStatus.SelectedValue == "1" ? true : false;
                        sapcommonpad.UpdateItemSapStatus(sPONO, sWorkFlowNO);
                    }
                }
            }
            if (sbErrorInfo.Length > 0)
            {
                sbErrorInfo.Append("\\nPlease contact IT for further help!");
                DisplayMessage(string.Concat("There are some errors occoured:\\n" + sbErrorInfo.ToString(), sbSucessInfo.ToString()));
                CommonUtil.logError(sbErrorInfo.ToString());
            }
            UpdateItemApprove();
            return IsAllSeccuss;
        }

        /// <summary>
        /// 若审批状态为拒绝 ，则将所有的Item改为拒绝。
        /// </summary>
        public void UpdateItemReject()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sPONO = fields["Title"].ToString();
            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    SPQuery query = new SPQuery();
                    query.Query = string.Format(@"<Where>
                                      <Eq>
                                         <FieldRef Name='Title' />
                                         <Value Type='Text'>{0}</Value>
                                      </Eq>
                                   </Where>", sPONO);
                    SPListItemCollection splic = web.Lists["PADChangeRequestItems"].GetItems(query);
                    if (null != splic)
                    {
                        web.AllowUnsafeUpdates = true;
                        foreach (SPListItem item in splic)
                        {
                            item["IsApprove"] = false;
                        }
                    }
                }
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
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sStatus"></param>
        void SendNoticeMail(string sStatus)
        { 
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            SapCommonPADChangeRequest comm=new SapCommonPADChangeRequest();
            try
            {
                comm.SendMail(WorkFlowUtil.GetApplicantAccount(fields["Applicant"].ToString()), sStatus, fields["Title"].ToString(), CurrentEmployee.UserAccount);// GetApprover(WorkFlowUtil.GetApplicantAccount(fields["Approvers"].ToString())));
            }
            catch(Exception e)
            {
                CommonUtil.logError("Sent PAD Mail failed:"+e.ToString());
            }
        }

    }
}