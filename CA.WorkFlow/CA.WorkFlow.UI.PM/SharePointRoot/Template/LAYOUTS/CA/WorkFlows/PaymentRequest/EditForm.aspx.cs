namespace CA.WorkFlow.UI.PaymentRequest
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
    using System.Data;
    using QuickFlow.UI.Controls;
    using System.Configuration;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class EditForm : NewCAWorkFlowPage
    {
        /// <summary>
        /// 工作流ID
        /// </summary>
        private string mWorkflowNumber = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.hfNoticeStatus.Value = "FromIMG";
            //如果是从PO过来的分期付款
            if ((bool)WorkflowContext.Current.DataFields["IsFromPO"])
            {
                SetShowPODetailsInfo(WorkflowContext.Current.DataFields["PONo"].ToString());
                this.hfNoticeStatus.Value = "FromPO";
                this.PaymentDataForm.ChooseEmployeeStatus = "none";
            }

            Session["PRMode"] = PaymentRequestMode.Edit;
            PaymentDataForm.PRMode = PaymentRequestMode.Edit;
            PaymentDataForm.IsFromPO = (bool)WorkflowContext.Current.DataFields["IsFromPO"];
            PaymentDataForm.PRNO = WorkflowContext.Current.DataFields["PRNo"].ToString();
            PaymentDataForm.PONO = WorkflowContext.Current.DataFields["PONo"].ToString();
            PaymentDataForm.SubPRNO = WorkflowContext.Current.DataFields["SubPRNo"].ToString();
            InstallmentForm.IsFromPO = (bool)WorkflowContext.Current.DataFields["IsFromPO"];
            InstallmentForm.PONO = WorkflowContext.Current.DataFields["PONo"].ToString();

            if (!WorkflowContext.Current.DataFields["SubPRNo"].ToString().Contains("_1"))
            {
                this.PaymentDataForm.ChooseEmployeeStatus = "none";
            }

            Actions.ActionExecuting += Actions_ActionExecuting;
            Actions.ActionExecuted += Actions_ActionExecuted;

            PaymentDataForm.RequestId = WorkflowContext.Current.DataFields["SubPRNo"].ToString();
            InstallmentForm.RequestId = WorkflowContext.Current.DataFields["SubPRNo"].ToString();

            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            this.PaymentDataForm.SummaryExpenseType1 = fields["SummaryExpenseType"].AsString();

            string ocStatus = WorkflowContext.Current.DataFields["RequestType"].ToString();
            //根据路径分别设置Opex_Capex_Status、Installment_Opex_Capex_Status
            this.PaymentDataForm.Opex_Capex_Status = ocStatus;
            this.PaymentDataForm.RequestType = ocStatus;
            this.InstallmentForm.Installment_Opex_Capex_Status = ocStatus;
            this.PaymentDataForm.HF_D_RequestType = ocStatus;
            this.InstallmentForm.HF_I_RequestType = ocStatus;
            this.PaymentDataForm.GRStatus = fields["GRStatus"].AsString() == "" ? "0" : fields["GRStatus"].AsString();
            if (!this.IsPostBack)
            {
                string strAccount = fields["Applicant"].AsString().Substring(fields["Applicant"].AsString().IndexOf("(") + 1, fields["Applicant"].AsString().IndexOf(")") - fields["Applicant"].AsString().IndexOf("(") - 1);
                Employee employee = UserProfileUtil.GetEmployeeEx(strAccount);
                this.PaymentDataForm.ApplicantEmployee = employee;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void SetShowPODetailsInfo(string poId)
        {
            DataTable dTable = PaymentRequestComm.GetPruchaseOrderInfo(poId).GetDataTable();
            if (dTable != null && dTable.Rows.Count > 0)
            {
                string script = "window.open('" + ConfigurationManager.AppSettings["rootweburl"];
                script += "/WorkFlowCenter/Lists/PurchaseOrderWorkFlow/DispForm.aspx?ID=";
                script += dTable.Rows[0]["ID"].ToString() + "', '_blank'); return false;";

                btnShowPoDetails.OnClientClick = script;
                btnShowPoDetails.UseSubmitBehavior = false;
                btnShowPoDetails.Visible = true;
            }
        }

        private void Actions_ActionExecuting(object sender, ActionEventArgs e)
        {
            NameCollection managerNameColl = new NameCollection();
            bool isSave = false, isConfirm = false;
            var status = CAWorkflowStatus.InProgress;
            var manager = string.Empty;
            string url = Request.UrlReferrer.ToString();
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (e.Action.Equals("Save", StringComparison.CurrentCultureIgnoreCase))
            {
                isSave = true;
                status = CAWorkflowStatus.Pending;
            }
            else
            {
                bool isCeo = PaymentRequestComm.IsCEO(this.PaymentDataForm.ApplicantEmployee.UserAccount);
                if (isCeo == false && (PaymentDataForm.IsSystemGR == false || PaymentDataForm.IsContractPO == false))
                {
                    string userName = this.PaymentDataForm.ApplicantEmployee.UserAccount;
                    PaymentRequestComm.GetManagers(managerNameColl, ref manager, userName, isCeo);
                    if (isCeo == false && manager.IsNullOrWhitespace())
                    {
                        //DisplayMessage("The manager is not set in the system.");

                        Response.Write("<script type=\"text/javascript\">alert('The manager is not set in the system.');window.location = '" + url + "';</script>");
                        Response.End();
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    bool isFromPO = (bool)WorkflowContext.Current.DataFields["IsFromPO"];
                    //string groupName = (isFromPO == true ? PaymentRequestGroupNames.Opex_ConstructionPO_SAPReview :
                    //    PaymentRequestGroupNames.Opex_GeneralPO_SAPReview);
                    string groupName = "";
                    if (isFromPO == true)
                    {
                        if (fields["RequestType"].ToString().ToLower() == "opex")
                        {
                            groupName = PaymentRequestGroupNames.Opex_ConstructionPO_SAPReview;
                        }
                        else
                        {
                            groupName = PaymentRequestGroupNames.Capex_ConstructionPO_SAPReview;
                        }
                    }
                    else
                    {
                        if (fields["RequestType"].ToString().ToLower() == "opex")
                        {
                            groupName = PaymentRequestGroupNames.Opex_GeneralPO_SAPReview;
                        }
                        else
                        {
                            groupName = PaymentRequestGroupNames.Capex_GeneralPO_SAPReview;
                        }
                    }

                    if (PaymentDataForm.IsContractPO == false)
                    {
                        List<string> cfos = WorkflowPerson.GetCFO();
                        if (cfos.Count == 0)
                        {
                            //DisplayMessage("The init error about WorkflowPerson in the system.");
                            Response.Write("<script type=\"text/javascript\">alert('The init error about WorkflowPerson in the system.');window.location = '" + url + "';</script>");
                            Response.End();
                            e.Cancel = true;
                            return;
                        }
                        managerNameColl.Add(UserProfileUtil.GetEmployeeEx(cfos[0]).UserAccount);
                        manager = UserProfileUtil.GetEmployeeEx(cfos[0]).UserAccount;
                    }
                    else
                    {
                        if (PaymentDataForm.IsSystemGR == true)
                        {
                            managerNameColl = WorkFlowUtil.GetUsersInGroup(groupName);
                            isConfirm = true;
                        }
                        else
                        {
                            List<string> cfos1 = WorkflowPerson.GetCFO();
                            if (cfos1.Count == 0)
                            {
                                //DisplayMessage("The init error about WorkflowPerson in the system.");
                                Response.Write("<script type=\"text/javascript\">alert('The init error about WorkflowPerson in the system.');window.location = '" + url + "';</script>");
                                Response.End();
                                e.Cancel = true;
                                return;
                            }
                            managerNameColl.Add(UserProfileUtil.GetEmployeeEx(cfos1[0]).UserAccount);
                            manager = UserProfileUtil.GetEmployeeEx(cfos1[0]).UserAccount;
                        }
                    }
                }
            }

            bool result = PaymentDataForm.SavePeymentRequestData(InstallmentForm, WorkflowContext.Current.DataFields);
            if (result == false)
            {
                //DisplayMessage(PaymentDataForm.ErrorMessage);
                Response.Write("<script type=\"text/javascript\">alert('" + PaymentDataForm.ErrorMessage + "');window.location = '" + url + "';</script>");
                Response.End();
                e.Cancel = true;
                return;
            }

            SetWorkflowVaribale(managerNameColl, status, manager, isSave, isConfirm);

            fields["SummaryExpenseType"] = this.InstallmentForm.SummaryExpenseType;
            fields["ReasonsResult"] = "0";
            fields["TaxPrice"] = this.InstallmentForm.TaxPrice;
            fields["GRStatus"] = this.PaymentDataForm.GRStatus;
            fields["ExchRate"] = this.InstallmentForm.ExchangeRate;
            fields["Currency"] = this.InstallmentForm.Currency;

            fields["Applicant"] = this.PaymentDataForm.ApplicantEmployee.DisplayName + "(" + this.PaymentDataForm.ApplicantEmployee.UserAccount + ")";
            //this.PaymentDataForm.Update();
            //PaymentRequestComm.AddItemTable(this.PaymentDataForm, fields);
            //PaymentRequestComm.DeleteAllDraftSAPItems(fields["SubPRNo"].AsString());
            //PaymentRequestComm.SaveSAPItemsDetails(this.PaymentDataForm);

            if ((bool)WorkflowContext.Current.DataFields["IsFromPO"] == false && int.Parse(PaymentDataForm.PaidIndex) == 1)
            {
                //InstallmentForm
                InstallmentForm.Update();
                PaymentRequestComm.AddItemTable1(this.InstallmentForm, fields);
                PaymentRequestComm.DeleteAllDraftSAPItems(fields["SubPRNo"].AsString());
                PaymentRequestComm.SaveSAPItemsDetails1(this.InstallmentForm);
            }
            else
            {
                PaymentDataForm.Update();
                PaymentRequestComm.AddItemTable(this.PaymentDataForm, fields);
                PaymentRequestComm.DeleteAllDraftSAPItems(fields["SubPRNo"].AsString());
                PaymentRequestComm.SaveSAPItemsDetails(this.PaymentDataForm);
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current, "SubPRNo");
        }

        /// <summary>
        /// 设置工作流变量值
        /// </summary>
        private void SetWorkflowVaribale(NameCollection managerNameColl, string status, string manager, bool isSave, bool isConfirm)
        {
            var context = WorkflowContext.Current;
            var userName = SPContext.Current.Web.CurrentUser.LoginName;
            var applicant = UserProfileUtil.GetEmployeeEx(userName);
            //var taskTitle = this.PaymentDataForm.ApplicantEmployee.DisplayName + "'s Payment Request ";
            var taskTitle = PaymentDataForm.SubPRWorkFlowNumber + " " + PaymentDataForm.VendorNameText + " " + PaymentDataForm.ApproveAmount + " " + this.PaymentDataForm.ApplicantEmployee.DisplayName + "'s Payment Request ";
            context.DataFields["Manager"] = manager;
            context.UpdateWorkflowVariable("IsPending", false);
            context.UpdateWorkflowVariable("IsReject", false);
            context.UpdateWorkflowVariable("IsSave", isSave);
            context.UpdateWorkflowVariable("IsConfirm", isConfirm);
            //context.UpdateWorkflowVariable("ConfirmTaskUsers", managerNameColl);
            //context.UpdateWorkflowVariable("ApproveTaskUsers", managerNameColl);
            context.UpdateWorkflowVariable("ConfirmTaskUsers", GetDelemanNameCollection(managerNameColl, "129"));
            context.UpdateWorkflowVariable("ApproveTaskUsers", GetDelemanNameCollection(managerNameColl, "129"));

            context.UpdateWorkflowVariable("ApproveTaskTitle", taskTitle );
            //  context.UpdateWorkflowVariable("CompleteTaskTitle", "Please complete Payment Request");
        }

        private void Actions_ActionExecuted(object sender, EventArgs e)
        {
            RedirectToTask();
        }
    }
}