namespace CA.WorkFlow.UI.PurchaseRequest
{
    using System;
    using QuickFlow.Core;
    using QuickFlow.UI.Controls;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint;
    using System.Collections;
    using System.Collections.Generic;
    using CA.SharePoint;

    public partial class ConfirmForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccount();

            CheckSecurity();

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;

            if (!IsPostBack)
            {
                this.DataForm1.RequestId = WorkflowContext.Current.DataFields["WorkflowNumber"].AsString();
                this.TaskTrace1.Applicant = WorkflowContext.Current.DataFields["Applicant"].AsString();
                DataForm1.DisplayMode = "Display";
            }

            this.Actions.OnClientClick = "return dispatchAction(this);";
        }

        private void CheckAccount()
        {
            //HO可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (PurchaseRequestCommon.IsHO(current) || PurchaseRequestCommon.isAdmin())
            {
                
            }
            else
            { 
                RedirectToTask();
            }
        }

        private void CheckSecurity()
        {
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (SPContext.Current.Web.CurrentUser.LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            {
                //spsadmin will ignore the security check
            }
            else if (!SecurityValidate(uTaskId, uListGUID, uID, true))
            {
                RedirectToTask();
            }
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;

            switch (e.Action)
            {
                case "To PO":
                    Hashtable hashPOs = PurchaseRequestCommon.CreatePOByReqestIds(fields["WorkflowNumber"].AsString());
                    var poNumber = string.Empty;
                    foreach (System.Collections.DictionaryEntry objDE in hashPOs)
                    {
                        poNumber += objDE.Value.ToString();
                    }

                    fields["POStatus"] = "Created";
                    fields["PONumber"] = poNumber;
                    fields["Status"] = CAWorkflowStatus.Completed;
                    break;
                case "Confirm":
                    fields["POStatus"] = "Pending";
                    fields["Status"] = CAWorkflowStatus.Completed;
                    break;
                case "Reject":
                    if (!Validate(e.Action, e))
                    {
                        return;
                    }
                    fields["Status"] = CAWorkflowStatus.Rejected;
                    fields["LevelHistory"] = string.Empty;
                    context.UpdateWorkflowVariable("CompleteTaskTitle", fields["WorkflowNumber"].ToString() + " :Please complete the rejected PR");
                    break;
                default:
                    DisplayMessage("Error. Please contact IT for further help.");
                    e.Cancel = true;
                    return;
            }

            SendApprovedMail();

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current);
        }

        private bool Validate(string action, QuickFlow.UI.Controls.ActionEventArgs e)
        {
            bool flag = true;
            if (!this.DataForm1.Validate(action))
            {
                DisplayMessage(this.DataForm1.MSG);
                e.Cancel = true;
                flag = false;
            }
            return flag;
        }

        private void SendApprovedMail()
        {
            var templateTitle = "PurchaseRequestApprovedToApplicant";

//To Applicant
//Dear {0},<br /><br /> 
//Your Purchase Request has been approved. 
//<br />
//<br /> 
//Please view the detailed information by <a href='{1}' target="_blank">clicking here</a>! 
//<br/>
//<br/> 
//-EWF System Account
            List<string> parameters = new List<string>();
            var applicantStr = WorkflowContext.Current.DataFields["Applicant"].AsString();
            var applicantAccount = WorkFlowUtil.GetApplicantAccount(applicantStr);
            List<string> to = new List<string>();
            to.Add(applicantAccount);

            string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
            string detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/PurchaseRequest/DisplayForm.aspx?List="
                                + Request.QueryString["List"]
                                + "&ID=" + Request.QueryString["ID"];

            var length = applicantStr.IndexOf("(");
            var applicantName = length > 0 ? applicantStr.Substring(0, length) : applicantAccount;
            parameters.Add(applicantName);
            parameters.Add(detailLink);

            SendNotificationMail(templateTitle, parameters, to, true);

//To HO
//Dear {0},<br /><br /> 
//{1}'s Purchase Request has been approved. 
//<br />
//PR Number: {2}
//PO Number: {3}
//<br />
//<br /> 
//Please view the detailed information by <a href='{4}' target="_blank">clicking here</a>! 
//<br/>
//<br/> 
//-EWF System Account
            templateTitle = "PurchaseRequestApprovedToHO";
            to = new List<string>();
            parameters = new List<string>();
            var confirmManager = WorkflowContext.Current.DataFields["CheckPerson"].AsString();
            to.Add(confirmManager);
            parameters.Add(string.Empty);
            parameters.Add(applicantName);
            var prNumber = WorkflowContext.Current.DataFields["WorkflowNumber"].ToString();
            var poNumber = WorkflowContext.Current.DataFields["PONumber"].AsString();
            parameters.Add(prNumber);
            parameters.Add(poNumber.IsNullOrWhitespace() ? "Pending" : poNumber);
            parameters.Add(detailLink);

            SendNotificationMail(templateTitle, parameters, to, false);
        }

    }
}