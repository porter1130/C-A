using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Microsoft.SharePoint;
using System.Data;
using System.Collections;
using CA.SharePoint;
using CA.SharePoint.Utilities.Common;
using System.Threading;
using QuickFlow;
using QuickFlow.Core;
using Microsoft.SharePoint.Workflow;
using System.Globalization;

namespace CA.WorkFlow.UI.CreditCardClaim
{
    public partial class UploadExcel : System.Web.UI.UserControl
    {

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (uploadAttachment.HasFile)
            {
                //get file extension
                string fileExt = Path.GetExtension(uploadAttachment.FileName).ToLower();
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(uploadAttachment.FileName).ToLower();

                if (fileExt == ".xlsx")
                {
                    try
                    {
                        SPList list = SPContext.Current.Web.Lists[WorkflowListName.WorkflowDocumentLibrary];
                        string filePath = uploadAttachment.PostedFile.FileName;

                        string primaryKey = ExcelService.GetExcelConfigInfo(WorkflowConfigName.CreditCardPK);
                        string colsKey = ExcelService.GetExcelConfigInfo(WorkflowConfigName.CreditCardCol);
                        string positionKey = ExcelService.GetExcelConfigInfo(WorkflowConfigName.CreditCardPosition);

                        string sheetName = ExcelService.GetExcelConfigInfo(WorkflowConfigName.CreditCardSheetName);

                        //string salaryExp = "B='工资' OR B='本地转帐'";
                        string[] exps = ExcelService.GetExcelConfigInfo(WorkflowConfigName.CreditCardBillExp).Split(',');

                        string salaryExp = GetExpByFormat(exps.Clone(), @"B='{0}'", "OR");

                        SPFile file = list.RootFolder.SubFolders[WorkflowFolderName.CreditCardClaim].Files.Add(uploadAttachment.FileName,
                            uploadAttachment.PostedFile.InputStream, true);

                        int monthNumber;
                        DateTime uploadDate = DateTime.Now;
                        if (rblUploadYear.SelectedValue == "0")
                        {
                            uploadDate = uploadDate.AddYears(-1);
                        }
                        string[] fileNameArray = fileNameWithoutExtension.Split('_');
                        if (int.TryParse(fileNameArray[fileNameArray.Length - 1], out monthNumber))
                        {
                            DateTime.TryParse(uploadDate.Year.ToString() + '-' + monthNumber.ToString(), out uploadDate);
                        }

                        DataTable excelDataTable = ExcelService.ParseExcel(file, positionKey, primaryKey, colsKey, sheetName);
                        DataTable resultDataTable = CreditCardClaimCommon.GetDataSource(excelDataTable, salaryExp);

                        Hashtable billHash = new Hashtable();
                        billHash.Add("A", "TransDate");
                        billHash.Add("B", "TransDesc");
                        billHash.Add("C", "MerchantName");
                        billHash.Add("D", "MerchantType");
                        billHash.Add("E", "TransAmt");
                        billHash.Add("F", "Currency");
                        billHash.Add("G", "DepositAmt");
                        billHash.Add("H", "PayAmt");
                        billHash.Add("I", "Title");
                        billHash.Add("J", "EmployeeName");
                        billHash.Add("K", "DCBalance");
                        billHash.Add("L", "FCBalance");

                        if (IsExistSameMonthData(uploadDate.ToString("yyyy-MM")))
                        {
                            SPQuery query = new SPQuery();
                            string queryFormt = @" <Where>
                                                      <Eq>
                                                         <FieldRef Name='UploadDate' />
                                                         <Value Type='Text'>{0}</Value>
                                                      </Eq>
                                                   </Where>";

                            query.Query = string.Format(queryFormt, uploadDate.ToString("yyyy-MM"));
                            query.ViewAttributes = "Scope='Recursive'";
                            WorkFlowUtil.BatchDeleteItems(WorkflowConfigName.CreditCardBill, query);
                        }
                        DataTable rmbDT = CreditCardClaimCommon.GetDataSource(excelDataTable, @"F='RMB'");
                        DataTable usdDT = CreditCardClaimCommon.GetDataSource(excelDataTable, @"F<>'RMB'");

                        CreditCardClaimCommon.BatchInsertExcelData(CreditCardClaimCommon.GetDataSource(rmbDT, GetExpByFormat(exps.Clone(), @"B<>'{0}'", "AND")), WorkflowConfigName.CreditCardBill, billHash, uploadDate.ToString("yyyy-MM"));
                        CreditCardClaimCommon.BatchInsertExcelData(CreditCardClaimCommon.GetDataSource(usdDT, GetExpByFormat(exps.Clone(), @"B<>'{0}'", "AND")), WorkflowConfigName.CreditCardBill, billHash, uploadDate.ToString("yyyy-MM"));

                        this.lblDistinctRecords.Text = excelDataTable.Rows.Count.ToString();
                        this.lblSalary.Text = resultDataTable.Rows.Count.ToString();
                        this.lblRecordsCount.Text = (excelDataTable.Rows.Count - resultDataTable.Rows.Count).ToString();

                        string SAPNo = string.Empty;
                        List<string> unMappingCardNoList;
                        FindUnMappingCardNoList(CreditCardClaimCommon.GetDataSource(excelDataTable, GetExpByFormat(exps.Clone(), @"B<>'{0}'", "AND")),
                                                            uploadDate.ToString("yyyy-MM"),
                                                            out unMappingCardNoList);
                        if (unMappingCardNoList.Count > 0)
                        {
                            string unMappingCardNos = string.Join("," + Environment.NewLine, unMappingCardNoList.ToArray());
                            this.lblCardInfoTitle.Text = "以下的信用卡号未能匹配到有效的员工信息:";
                            this.lblCardInfo.Text = unMappingCardNos;
                            DisplayMessage("Failed to upload/post the e-statement as there are " + unMappingCardNoList.Count + " unmapped items.");
                        }
                        else
                        {
                            SAPNo = CreditCardClaimCommon.BatchPostToSAP(GetUploadMonthData(uploadDate.ToString("yyyy-MM")));
                        }


                        if (SAPNo.IsNotNullOrWhitespace())
                        {
                            this.lblSAPNoTitle.Text = "生成的SAP Number:";
                            this.lblSAPNo.Text = SAPNo;


                            List<string> unNoticedApplicantsList;
                            NoticeApplicants(CreditCardClaimCommon.GetDataSource(excelDataTable, GetExpByFormat(exps.Clone(), @"B<>'{0}'", "AND")),
                                                            uploadDate.ToString("yyyy-MM"),
                                                            out unMappingCardNoList,
                                                            out unNoticedApplicantsList);

                            if (unNoticedApplicantsList.Count > 0)
                            {
                                string unNoticedApplicants = string.Join("," + Environment.NewLine, unNoticedApplicantsList.ToArray());
                                this.lblApplicantsTitle.Text = "通知以下用户时启动工作流引擎失败:";
                                this.lblApplicantsInfo.Text = unNoticedApplicants;
                            }
                        }
                        else
                        {
                            BatchDeleteBillItems(uploadDate.ToString("yyyy-MM"));
                            DisplayMessage("Failed to post to SAP, please check excel format and try it again.");

                        }

                        //TerminateAndStartWFForFinanceGroup();

                    }
                    catch (Exception ex)
                    {
                        DisplayMessage("Upload excel fail, you have no right to upload it or the excel format is not right,please try it again.");
                        CommonUtil.logError("Upload file fail, error:" + ex.Message + "\nStackTrace:" + ex.StackTrace);
                    }
                }
                else
                {
                    DisplayMessage("Please upload excel 2007 format document(.xlsx).");
                }
            }
        }

        private void BatchDeleteBillItems(string uploadDate)
        {
            SPQuery query = new SPQuery();
            string queryFormat = @"<Where>
                                      <Eq>
                                         <FieldRef Name='UploadDate' />
                                         <Value Type='Text'>{0}</Value>
                                      </Eq>
                                   </Where>";

            query.Query = string.Format(queryFormat, uploadDate);

            WorkFlowUtil.BatchDeleteItems(WorkflowListName.CreditCardBill, query);
        }

        private void FindUnMappingCardNoList(DataTable dataTable, string uploadDate, out List<string> unMappingCardNoList)
        {
            unMappingCardNoList = new List<string>();

            string cardNoColName = "I";

            DataTable dt = new DataTable();
            dt = dataTable.DefaultView.ToTable(true, new string[] { cardNoColName });

            foreach (DataRow row in dt.Rows)
            {
                Employee employee = GetEmployeeByCardNo(row[cardNoColName].AsString());
                if (employee == null)
                {
                    unMappingCardNoList.Add(row[cardNoColName].AsString());
                }

            }
        }

        private void TerminateAndStartWFForFinanceGroup()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        web.AllowUnsafeUpdates = true;

                        SPList list = web.Lists[WorkflowListName.CreditCardBill];
                        SPWorkflowAssociation wfAss = list.WorkflowAssociations.GetAssociationByName(WorkflowConfigName.CreditCardBillWorkflow, System.Globalization.CultureInfo.CurrentCulture);

                        SPQuery query = new SPQuery();

                        query.Query = CreditCardClaimCommon.GetQuery("WorkflowStep", "UploadBillTask");

                        SPListItemCollection items = list.GetItems(query);

                        if (items.Count > 0)
                        {
                            foreach (SPWorkflow wfItem in items[0].Workflows)
                            {
                                foreach (SPWorkflowTask wfTask in wfItem.Tasks)
                                {
                                    wfTask["Status"] = "Canceled";
                                    wfTask.SystemUpdate();
                                }

                                SPWorkflowManager.CancelWorkflow(wfItem);
                            }

                            WorkflowVariableValues vs = new WorkflowVariableValues();
                            vs["UploadBillTaskTitle"] = "Please upload credit card e-statement";
                            vs["UploadBillTaskFormURL"] = "/_Layouts/CA/WorkFlows/CreditCardClaim/UploadExcelBill.aspx";

                            var taskUsers = new NameCollection();
                            List<string> groupUsers = null;

                            groupUsers = WorkFlowUtil.UserListInGroup(WorkflowGroupName.WF_FinanceConfirm);
                            taskUsers.AddRange(groupUsers.ToArray());

                            vs["UploadBillTaskUsers"] = taskUsers;

                            string eventData = SerializeUtil.Serialize(vs);

                            site.WorkflowManager.StartWorkflow(items[0], wfAss, eventData);
                        }

                        web.AllowUnsafeUpdates = false;
                    }
                }
            });

        }

        private string GetExpByFormat(object obj, string expFormat, string type)
        {
            string[] exps = (string[])obj;
            for (int i = 0; i < exps.Length; i++)
            {
                exps[i] = string.Format(expFormat, exps[i]);
            }

            return string.Join(" " + type + " ", exps);
        }

        private DataTable GetUploadMonthData(string uploadDate)
        {
            SPList list = SPContext.Current.Web.Lists[WorkflowConfigName.CreditCardBill];

            SPQuery query = new SPQuery();
            string queryFormat = @"<Where>
      <Eq>
         <FieldRef Name='UploadDate' />
         <Value Type='Text'>{0}</Value>
      </Eq>
   </Where>";

            query.Query = string.Format(queryFormat, uploadDate);

            #region Add EmployeeId column for Post to SAP
            DataTable dt = list.GetItems(query).GetDataTable();
            dt.Columns.Add("EmployeeId");
            SPQuery cardNoQuery;
            SPList mappingList = SPContext.Current.Web.Lists[WorkflowListName.CreditCardEmployeeMapping];
            foreach (DataRow dr in dt.Rows)
            {
                if (!dr["Currency"].AsString().Equals("RMB", StringComparison.CurrentCultureIgnoreCase))
                {
                    dr["Currency"] = "USD";
                }
                cardNoQuery = new SPQuery();
                cardNoQuery.Query = CreditCardClaimCommon.GetQuery("CardNo", dr["Title"].AsString());
                SPListItemCollection items = mappingList.GetItems(cardNoQuery);
                if (items.Count > 0)
                {
                    dr["EmployeeId"] = items[0]["EmployeeId"].AsString();
                }

            }
            #endregion

            return dt ?? null;
        }


        private bool IsExistSameMonthData(string uploadDate)
        {
            bool isExist = false;
            SPList list = SPContext.Current.Web.Lists[WorkflowConfigName.CreditCardBill];

            SPQuery query = new SPQuery();
            string queryFormt = @"   <Where>
      <Eq>
         <FieldRef Name='UploadDate' />
         <Value Type='Text'>{0}</Value>
      </Eq>
   </Where>";

            query.Query = string.Format(queryFormt, uploadDate);

            if (list.GetItems(query).Count > 0)
            {
                isExist = true;
            }

            return isExist;
        }

        private void NoticeApplicants(DataTable dataTable, string uploadDate, out List<string> unMappingCardNoList, out List<string> unNoticedApplicantsList)
        {
            unMappingCardNoList = new List<string>();

            string cardNoColName = "I";

            List<Employee> employeeList = new List<Employee>();

            DataTable dt = new DataTable();
            dt = dataTable.DefaultView.ToTable(true, new string[] { cardNoColName });

            foreach (DataRow row in dt.Rows)
            {
                Employee employee = GetEmployeeByCardNo(row[cardNoColName].AsString());
                if (employee != null)
                {
                    if (!IsStart(employee, uploadDate))
                    {
                        //SendEmail(employee, "BillNotice",uploadDate);
                        if (!employeeList.Contains(employee))
                        {
                            employeeList.Add(employee);
                        }
                    }
                }
                else
                {
                    unMappingCardNoList.Add(row[cardNoColName].AsString());
                }

            }

            unNoticedApplicantsList = StartCreditCardClaimWF(employeeList, uploadDate);

        }

        private List<string> StartCreditCardClaimWF(List<Employee> employeeList, string uploadDate)
        {
            SPList list;
            SPListItem item;
            string wfId = string.Empty;

            List<string> unMappingList = new List<string>();

            foreach (Employee employee in employeeList)
            {
                try
                {
                    SPUser user = EnsureUser(employee.UserAccount);
                    if (user != null)
                    {

                        using (SPSite site = new SPSite(SPContext.Current.Site.ID, user.UserToken))
                        {
                            using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                            {
                                WorkflowVariableValues vs = new WorkflowVariableValues();
                                vs["CompleteTaskTitle"] = "Your " + uploadDate + " Credit Card Statement has arrived; please file claim";
                                vs["CompleteTaskFormURL"] = WorkflowConfigName.CreditCardClaimUrl + "EditForm.aspx";
                                vs["IsSave"] = true;

                                string eventData = SerializeUtil.Serialize(vs);

                                list = web.Lists[WorkflowListName.CreditCardClaim];
                                wfId = CreateWorkflowNumber();

                                item = list.Items.Add();
                                item["WorkflowNumber"] = wfId;
                                item["Status"] = CAWorkflowStatus.CCFinancePending;

                                item["Applicant"] = employee.DisplayName + "(" + employee.UserAccount + ")";
                                item["ApplicantSPUser"] = this.EnsureUser(employee.UserAccount);
                                item["Department"] = employee.Department;
                                item["Month"] = uploadDate;
                                item["InvoiceStatus"] = "1";
                                item["PreTotalAmount"] = 0;
                                item["SaveStatus"] = "";

                                item.Web.AllowUnsafeUpdates = true;
                                item.Update();


                                SPWorkflowAssociation wfAss = list.WorkflowAssociations.GetAssociationByName(WorkflowConfigName.CreditCardClaim, CultureInfo.CurrentCulture);

                                site.WorkflowManager.StartWorkflow(item, wfAss, eventData);
                                WorkFlowUtil.UpdateWorkflowPath(item, eventData);

                                item.Web.AllowUnsafeUpdates = false;
                            }
                        }
                        SendEmail(employee, uploadDate);
                    }
                }
                catch (Exception ex)
                {
                    CommonUtil.logError(string.Format("Start Credit Card Claim WF Error: {0} \n. Details: WorkflowNumber:{1}\n{2}'s {3} credit card bill info upload fails.\n",
                                                                ex.Message,
                                                                wfId,
                                                                employee.UserAccount,
                                                                uploadDate));
                    unMappingList.Add(employee.DisplayName);
                }

            }

            return unMappingList;

        }

        private void SendEmail(Employee employee, string uploadDate)
        {
            SendEmail(employee, "BillNotice", uploadDate, null);
            SPQuery query = new SPQuery();
            query.Query = CreditCardClaimCommon.GetQuery("OriginalCardHolderAccount", employee.UserAccount);
            SPList list = SPContext.Current.Web.Lists[WorkflowListName.CreditCardEmployeeDelegateMapping];
            SPListItemCollection items = list.GetItems(query);
            foreach (SPListItem item in items)
            {
                Employee delegateEmp = UserProfileUtil.GetEmployeeEx(item["DelegateCardHolderAccount"].AsString());
                if (delegateEmp != null)
                {
                    SendEmail(delegateEmp, "BillNoticeForDelegate", uploadDate, employee);
                }
            }
        }

        private string CreateWorkflowNumber()
        {
            return "CCC_" + WorkFlowUtil.CreateWorkFlowNumber("CreditCardClaimWorkflow").ToString("000000");
        }

        private bool IsStart(Employee employee, string uploadDate)
        {
            bool isStart = false;

            SPList list = SPContext.Current.Web.Lists[WorkflowConfigName.CreditCardClaim];
            SPQuery query = new SPQuery();
            string queryFormat = @" <Where>
      <And>
            <Eq>
               <FieldRef Name='ApplicantSPUser' />
               <Value Type='User'>{0}</Value>
            </Eq>
            <Eq>
               <FieldRef Name='Month' />
               <Value Type='Text'>{1}</Value>
            </Eq>
         </And>
   </Where>";
            query.Query = string.Format(queryFormat, employee.UserAccount, uploadDate);

            SPListItemCollection items = list.GetItems(query);

            if (items.Count > 0)
            {
                isStart = true;
            }
            return isStart;
        }

        private void SendEmail(Employee employee, string type, string uploadDate, Employee originalEmp)
        {
            var templateTitle = "CreditCardClaim" + type;

            var applicantAccount = employee.UserAccount;
            var applicantName = employee.DisplayName;

            string detailLink = string.Format("{0}/CA/MyTasks.aspx",
                                                SPContext.Current.Site.Url);


            List<string> parameters = new List<string>();
            parameters.Add("");
            parameters.Add(uploadDate);
            parameters.Add(detailLink);
            List<string> to = new List<string>();
            to.Add(applicantAccount);

            switch (type)
            {
                case "BillNotice":
                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;
                case "BillNoticeForDelegate":
                    detailLink = string.Format("{0}/{1}/NewForm.aspx",
                                                SPContext.Current.Site.Url,
                                                SPContext.Current.Web.Lists[WorkflowConfigName.CreditCardClaim].RootFolder.ServerRelativeUrl);
                    parameters.Clear();
                    parameters.Add("");
                    parameters.Add(originalEmp.DisplayName);
                    parameters.Add(uploadDate);
                    parameters.Add(detailLink);

                    SendNotificationMail(templateTitle, parameters, to, true);
                    break;
                default:
                    break;
            }
        }

        private Employee GetEmployeeByCardNo(string cardNo)
        {
            Employee employee = null;
            SPList list = SPContext.Current.Web.Lists[WorkflowConfigName.CreditCardEmployeeMapping];
            SPQuery query = new SPQuery();
            string queryFormat = @"<Where>
      <Eq>
         <FieldRef Name='CardNo' />
         <Value Type='Text'>{0}</Value>
      </Eq>
   </Where>";
            query.Query = string.Format(queryFormat, cardNo);

            SPListItemCollection items = list.GetItems(query);

            if (items.Count > 0)
            {
                employee = UserProfileUtil.GetEmployeeByProp("EmployeeId", items[0]["EmployeeId"].AsString());
            }

            return employee;
        }

        protected void DisplayMessage(string msg)
        {
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);

            //this.Script.Alert(msg); 用这个就可以
        }

        public SPUser EnsureUser(string strUser)
        {
            SPUser user = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        user = web.EnsureUser(strUser);
                    }
                }
            });
            return user;
        }

        protected void SendNotificationMail(string templateName, List<string> parameters, List<string> to, bool toApplicant)
        {
            var emailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(templateName);
            if (emailTemplate == null)
            {
                return;
            }
            string bodyTemplate = emailTemplate["Body"].AsString();
            if (bodyTemplate.IsNotNullOrWhitespace())
            {
                string subject = emailTemplate["Subject"].AsString();

                WorkFlowUtil.SendMail(toApplicant ? subject : parameters[1] + ":" + subject, bodyTemplate, parameters, to);
            }
        }

    }
}