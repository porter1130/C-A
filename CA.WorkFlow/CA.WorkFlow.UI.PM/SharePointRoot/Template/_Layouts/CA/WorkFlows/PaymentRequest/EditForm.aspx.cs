namespace CA.WorkFlow.UI.TravelRequest3
{
    using System;
    using System.ComponentModel;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using QuickFlow;
    using CA.SharePoint;
    using Microsoft.SharePoint;
    using System.Collections.Generic;
    using CodeArt.SharePoint.CamlQuery;

    public partial class EditForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataForm1.Mode = "Edit";

            //Check security
            string uListGUID = Request.QueryString["List"];
            string uID = Request.QueryString["ID"];
            string uTaskListGUID = Request.QueryString["TaskList"];
            string uTaskId = Request.QueryString["TaskId"];
            if (!SecurityValidate(uTaskId, uListGUID, uID, false))
            {
                RedirectToTask();
            }

            this.Actions.ActionExecuting += this.Actions_ActionExecuting;
            this.Actions.ActionExecuted += this.Actions_ActionExecuted;
            this.btnSave.Click += this.btnSave_Click;

            if (!IsPostBack)
            {
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                this.DataForm1.RequestId = fields["WorkflowNumber"].AsString();
                if (fields["Applicant"].AsString() != null)
                {
                    this.DataForm1.UserAccount = GetUserAccount(fields["Applicant"].AsString());
                }
                this.DataForm1.TotalBudget = fields["TravelTotalCost"].AsString();
                this.DataForm1.OtherPurpose = fields["TravelOtherPurpose"].AsString();
                this.DataForm1.ChosenFlight = Convert.ToBoolean(fields["IsBusiness"].AsString());
                this.DataForm1.NextFlight = Convert.ToBoolean(fields["IsNextFlight"].AsString());
                this.DataForm1.IsBookHotel = !Convert.ToBoolean(fields["IsBookHotel"].AsString());
            }

        }

        private string GetUserAccount(string applicant)
        {
            if (applicant.IsNotNullOrWhitespace())
            {
                char[] split = { '(', ')' };
                return applicant.Split(split)[1].ToString();
            }
            return applicant;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var msg = this.DataForm1.ValidateForSave();
            if (msg.IsNotNullOrWhitespace())
            {
                DisplayMessage(msg);
                return;
            }
            else
            {
                this.DataForm1.Update(); //Save the inputed data to datatable

                WorkflowContext context = WorkflowContext.Current;
                WorkflowDataFields fields = WorkflowContext.Current.DataFields;
                Employee applicant = this.DataForm1.Applicant;

                fields["Applicant"] = applicant != null ? applicant.DisplayName + "(" + applicant.UserAccount + ")" : string.Empty;
                fields["ApplicantSPUser"] = SPContext.Current.Web.EnsureUser(applicant.UserAccount);
                fields["Department"] = DataForm1.Department;

                decimal totalCost = this.DataForm1.GetTotal();
                fields["TravelTotalCost"] = totalCost;
                fields["TravelOtherPurpose"] = this.DataForm1.OtherPurpose;
                var isBusiness = this.DataForm1.ChosenFlight;
                var isNextFlight = this.DataForm1.NextFlight;
                var isBookHotel = this.DataForm1.IsBookHotel;
                fields["IsBusiness"] = isBusiness;
                fields["IsNextFlight"] = isNextFlight;
                fields["IsBookHotel"] = !isBookHotel;
                var flightClass = string.Empty;
                if (isBusiness)
                {
                    flightClass = "Business";
                }
                else if (isNextFlight)
                {
                    flightClass = "Other available flight";
                }
                else
                {
                    flightClass = "Economy";
                }
                fields["FlightClass"] = flightClass;

                //Delete all draft items before saving
                TravelRequest3Common.DeleteAllDraftItems(fields["WorkflowNumber"].AsString());

                TravelRequest3Common.SaveDetails(this.DataForm1, fields["WorkflowNumber"].AsString()); //Save request details to lists

                WorkflowContext.Current.SaveTask();
                RedirectToTask();
            }
        }

        private void Actions_ActionExecuting(object sender, CancelEventArgs e)
        {
            //局部变量定义
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            var departmentManagerTaskUsers = new NameCollection();
            string manager = string.Empty;
            bool isCeo = false;

            //判断输入数据是否正确，错误则直接返回
            var msg = this.DataForm1.ValidateForSubmit();
            if (msg.IsNotNullOrWhitespace())
            {
                DisplayMessage(msg);
                e.Cancel = true;
                return;
            }

            //以下代码获取上级审批人信息
            var managerEmp = WorkFlowUtil.GetEmployeeApprover(this.DataForm1.Applicant);
            if (managerEmp == null)
            {
                isCeo = IsCEO(DataForm1.Applicant.UserAccount);
                if (!isCeo){
                    DisplayMessage("The manager is not set in the system.");
                    e.Cancel = true;
                    return;
                }
                else{
                    //获取前台确认的员工
                    departmentManagerTaskUsers = TravelRequest3Common.GetTaskUsersByModuleWithoutDeleman("wf_ReceptionCtrip_TR", "TravelRequestWorkFlow");
                }
            }
            else{
                manager = managerEmp.UserAccount;
                departmentManagerTaskUsers.Add(manager);
            }

            //WorkflowNumber
            string workflowNumber = CreateTRWorkFlowNumber();
            //首先保存用户输入的数据
            DataForm1.Update();
            //保存详细信息
            TravelRequest3Common.SaveDetails(DataForm1, workflowNumber);
            //保存数据
            Employee applicant = this.DataForm1.Applicant;
            fields["Applicant"] = applicant != null ? applicant.DisplayName + "(" + applicant.UserAccount + ")" : string.Empty;
            fields["ApplicantSPUser"] = SPContext.Current.Web.EnsureUser(applicant.UserAccount);
            fields["Department"] = DataForm1.Department;

            fields["Department"] = this.DataForm1.Department;
            fields["TravelOtherPurpose"] = this.DataForm1.OtherPurpose;
            var isBusiness = this.DataForm1.ChosenFlight;
            var isNextFlight = this.DataForm1.NextFlight;
            var isBookHotel = this.DataForm1.IsBookHotel;
            fields["IsBusiness"] = isBusiness;
            fields["IsNextFlight"] = isNextFlight;
            fields["IsBookHotel"] = !isBookHotel;
            fields["Managers"] = manager;

            var flightClass = string.Empty;
            if (isBusiness){
                flightClass = "Business";
            }
            else if (isNextFlight){
                flightClass = "Other available flight";
            }
            else{
                flightClass = "Economy";
            }
            fields["FlightClass"] = flightClass;
            fields["WorkflowNumber"] = workflowNumber;
            fields["TravelTotalCost"] = DataForm1.GetTotal();
            fields["Status"] = CAWorkflowStatus.InProgress;

            //判断是否CEO，若是，则直接跳转到前台确认
            if (isCeo)
            {
                context.UpdateWorkflowVariable("CompleteTaskTitle", "please submit Travel Request");
                context.UpdateWorkflowVariable("ConfirmTaskUsers", departmentManagerTaskUsers);
                context.UpdateWorkflowVariable("IsCeo", true);
            }
            else
            {
                //定义标题
                string taskTitle = DataForm1.EnglishName + "'s Travel Request ";
                context.UpdateWorkflowVariable("NextApproveTaskTitle", taskTitle + "needs approval");
                context.UpdateWorkflowVariable("NextApproveTaskUsers", departmentManagerTaskUsers);
                context.UpdateWorkflowVariable("IsContinue", true);
            }

            //设置工作流URL
            var editURL = "/_Layouts/CA/WorkFlows/TravelRequest3/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/TravelRequest3/ApproveForm.aspx";
            context.UpdateWorkflowVariable("CompleteTaskFormURL", editURL);
            context.UpdateWorkflowVariable("NextApproveTaskFormURL", approveURL);
            context.UpdateWorkflowVariable("ConfirmTaskFormURL", approveURL);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        private bool IsCEO(string userAccount)
        {
            bool isCEO = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPGroup group = web.Groups["wf_CEO"];
                        foreach (SPUser user in group.Users)
                        {
                            if (user.LoginName.Equals(userAccount, StringComparison.CurrentCultureIgnoreCase))
                            {
                                isCEO = true;
                                break;
                            }
                        }
                    }
                }
            });
            return isCEO;
        }

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private string CreateTRWorkFlowNumber()
        {
            var department = DataForm1.Department;
            return "TR" + department + CreateWorkFlowNumber2("TravelRequestWorkflow2" + department).ToString("0000");
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        private void SendMailForSubmit(NameCollection departmentManagerTaskUsers)
        {
            //Send mail to Onsite and Receptionist
            var templateTitle = "TravelRequest2Submit2";
            List<string> parameters = new List<string>();
            var applicantStr = WorkflowContext.Current.DataFields["Applicant"].AsString();
            var applicantName = WorkflowContext.Current.DataFields["EnglishName"].AsString();
            List<string> to = TravelRequest3Common.GetMailMembers("Receptionist", "C-Trip");
            string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
            string detailLink = rootweburl + "WorkFlowCenter/_Layouts/CA/WorkFlows/TravelRequest3/DisplayForm.aspx?List="
                    + Request.QueryString["List"]
                    + "&ID=" + Request.QueryString["ID"]
                    + "&Source=/WorkFlowCenter/Lists/TravelRequest3/MyApply.aspx";
            parameters.Add("");
            parameters.Add(applicantName);
            parameters.Add(WorkflowContext.Current.DataFields["WorkflowNumber"].AsString());
            parameters.Add(detailLink);
            SendNotificationMail(templateTitle, parameters, to, false);

            //Send mail to Applicant
            to = new List<string>();
            parameters = new List<string>();
            var applicantAccount = WorkFlowUtil.GetApplicantAccount(applicantStr);
            var approverNames = WorkFlowUtil.GetDisplayNames(TravelRequest3Common.ConvertToList(departmentManagerTaskUsers));
            templateTitle = "TravelRequest2Submit1";
            //detailLink = rootweburl + "WorkFlowCenter/Lists/TravelRequestWorkflow2/MyApply.aspx";
            to.Add(applicantAccount);
            parameters.Add("");
            parameters.Add(approverNames);
            //parameters.Add(detailLink);
            SendNotificationMail(templateTitle, parameters, to, true);

            //Send mail to Department Manager
            parameters = new List<string>();
            to = TravelRequest3Common.ConvertToList(departmentManagerTaskUsers);
            templateTitle = "TravelRequest2Submit3";
            detailLink = rootweburl + "CA/MyTasks.aspx";
            parameters.Add("");
            parameters.Add(applicantName);
            parameters.Add(detailLink);
            SendNotificationMail(templateTitle, parameters, to, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public int CreateWorkFlowNumber2(string workflowName)
        {
            int nNum = 1;
            //CA.SharePoint.ISharePointService sps = ServiceFactory.GetSharePointService(true);
            //SPList list = sps.GetList(CAWorkFlowConstants.ListName.WorkFlowNumber.ToString());

            var list = SPContext.Current.Web.Lists[CAWorkFlowConstants.ListName.WorkFlowNumber.ToString()];
            QueryField field = new QueryField("Title");

            //SPListItemCollection items = sps.Query(list, field.Equal(workflowName), 1, null);
            SPQuery query = new SPQuery();
            var querystr = @"<Where>
                                <Eq>
                                   <FieldRef Name='Title' />
                                   <Value Type='Text'>{0}</Value>
                                </Eq>                 
                             </Where>";

            query.Query = string.Format(querystr, workflowName);
            var items = list.GetItems(query);

            if (items != null && items.Count > 0)
            {
                SPListItem item = items[0];//list.GetItemById(items[0].ID);
                nNum = Convert.ToInt32(item["Number"]) + 1;//Convert.ToInt32(items[0]["Number"]) + 1;
                item["Number"] = Convert.ToDouble(nNum);
                item.Web.AllowUnsafeUpdates = true;
                item.Update();
            }
            else
            {
                SPListItem item = list.Items.Add();
                item["WorkFlowName"] = workflowName;
                item["Number"] = nNum;
                item.Web.AllowUnsafeUpdates = true;
                item.Update();
            }

            return nNum;
        }
    }
}