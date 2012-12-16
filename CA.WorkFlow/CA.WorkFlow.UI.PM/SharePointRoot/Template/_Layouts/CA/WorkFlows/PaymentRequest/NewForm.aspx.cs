
using System;
using System.ComponentModel;
using QuickFlow.UI.Controls;
using QuickFlow.Core;
using QuickFlow;
using CA.SharePoint;
using CA.SharePoint.Utilities.Common;
using Microsoft.SharePoint;
using System.Configuration;
using System.Collections.Generic;
using CodeArt.SharePoint.CamlQuery;
using SAP.Middleware.Exchange;

namespace CA.WorkFlow.UI.TravelRequest3
{
    public partial class NewForm : CAWorkFlowPage
    {
        string workflowNumber = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;
        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            //局部变量定义
            WorkflowContext context = WorkflowContext.Current;
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            var departmentManagerTaskUsers = new NameCollection();
            var btn = sender as StartWorkflowButton;
            string manager = string.Empty;
            bool isCeo = false;

            if (string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
            {
                //判断输入数据是否正确，错误则直接返回
                var msg = this.DataForm1.ValidateForSave();
                if (msg.IsNotNullOrWhitespace())
                {
                    DisplayMessage(msg);
                    e.Cancel = true;
                    return;
                }
                fields["Status"] = CAWorkflowStatus.Pending;
                context.UpdateWorkflowVariable("IsSave", true);
                context.UpdateWorkflowVariable("CompleteTaskTitle", "please complete Travel Request");
            }
            else
            {
                //判断输入数据是否正确，错误则直接返回
                var msg = this.DataForm1.ValidateForSubmit();
                if (msg.IsNotNullOrWhitespace())
                {
                    DisplayMessage(msg);
                    e.Cancel = true;
                    return;
                }

                //以下代码获取上级审批人信息
                var managerEmp = WorkFlowUtil.GetNextApprover(this.DataForm1.Applicant.UserAccount);
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
                else
                {
                    manager = managerEmp.UserAccount;
                    departmentManagerTaskUsers.Add(manager);
                }

                //var deleman = WorkFlowUtil.GetDeleman(manager, "106");
                //if (deleman != null){
                //    departmentManagerTaskUsers.Add(deleman);
                //}

                context.UpdateWorkflowVariable("IsSave", false);
            }

            //WorkflowNumber
            workflowNumber = CreateTRWorkFlowNumber();
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

            //发送邮件
            if (!string.Equals(btn.Text, "Save", StringComparison.CurrentCultureIgnoreCase)){
                SendMailForSubmit(departmentManagerTaskUsers);
            }
        }

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

        private void SendMailForSubmit(NameCollection departmentManagerTaskUsers)
        {
            //Send mail to Onsite and Receptionist
            var templateTitle = "TravelRequest2Submit2";
            List<string> parameters = new List<string>();
            var applicantStr = WorkflowContext.Current.DataFields["Applicant"].AsString();
            var applicantName = WorkflowContext.Current.DataFields["EnglishName"].AsString();
            List<string> to = TravelRequest3Common.GetMailMembers("Receptionist", "C-Trip");
            string rootweburl = GetRootURL(System.Configuration.ConfigurationManager.AppSettings["rootweburl"]);
            string detailLink = rootweburl + "WorkFlowCenter/Lists/TravelRequestWorkflow2/TRPending.aspx";
            parameters.Add("");
            parameters.Add(applicantName);
            parameters.Add(workflowNumber);
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            List<SapParameter> mSapParametersCD = new List<SapParameter>();
            //Cash Advance 数据
            SapParameter mSapParameters3 = new SapParameter()
            {
                BusAct = "RFBU",
                CompCode = "CA10",
                DocType = "SA",
                BusArea = "0001",
                Currency = "RMB",
                EmployeeID = "6000000150",  //
                EmployeeName = "TEST",        //
                ExchRate = 1,
                Header = "Cash Advance",
                RefDocNo = "CA" + DateTime.Now.ToString("yyyyMMddHHmmss"),// 
                UserName = "acnotes",
                CashAmount = 200,                                      //
                PaidByCC = 100
            };
            mSapParametersCD.Add(mSapParameters3);

            ISapExchange sapExchange = SapExchangeFactory.GetCashAdvance();
            List<object[]> result = sapExchange.ImportDataToSap(mSapParametersCD);
            for (int i = 0; i < result.Count; i++)
            {
                SapParameter sp = (SapParameter)result[i][0];
               
                bool bl = (bool)result[i][2];
                if (bl)
                {
                    SapResult sr = (SapResult)result[i][1];
                    Response.Write("Sap ID : " + sr.OBJ_KEY);
                }
                else
                {
                    Response.Write(result[i][1].ToString());
                }
            }
        }
    }
}