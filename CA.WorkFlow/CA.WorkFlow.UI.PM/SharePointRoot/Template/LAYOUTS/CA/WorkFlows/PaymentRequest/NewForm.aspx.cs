
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
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace CA.WorkFlow.UI.PaymentRequest
{
    public partial class NewForm : NewCAWorkFlowPage
    {
        /// <summary>
        /// 工作流ID
        /// </summary>
        private string mWorkflowNumber = string.Empty;  


        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    string ocStatues = OpexCapexStatus.Capex_AssetNo;
        //    if (IsPostBack == false)          
        //    {
        //        if (Request.QueryString["IsFromIco"] != null)
        //        {
        //            this.Panel1.Visible = false;
        //            this.Panel2.Visible = true;
        //        }
        //        else
        //        {
        //            this.Panel1.Visible = true;
        //            this.Panel2.Visible = false;

        //            DataTable dTable = null;
        //            string poId = Request.QueryString["PONO"].ToString();
        //            if (CheckPaymentNextStep(poId) == false){
        //                return;
        //            }
        //            //如果是从PO过来的分期付款
        //            if (Request.QueryString["IsFromPO"] != null)
        //            {
        //                //dTable = PaymentRequestComm.GetPruchaseRequestInfo(poId).GetDataTable();
        //                dTable = WorkFlowUtil.GetCollectionByList("Purchase Request Workflow")
        //                                     .GetDataTable()
        //                                     .AsEnumerable()
        //                                     .Where(dr => dr.Field<string>("PONumber").AsString().Contains(poId))
        //                                     .CopyToDataTable();
        //                SetShowPODetailsInfo(poId);
        //            }
        //            else{
        //                dTable = PaymentRequestComm.GetPaymentRequestInfo(poId).GetDataTable();
        //            }

        //            if (dTable.Rows.Count > 0)
        //            {
        //                switch (dTable.Rows[0]["RequestType"].ToString())
        //                {
        //                    case "Capex":
        //                        ocStatues = OpexCapexStatus.Capex_AssetNo;
        //                        break;
        //                    case "Opex":
        //                        ocStatues = OpexCapexStatus.Opex;
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //            this.PaymentDataForm.FromPO = string.Empty;
        //            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
        //            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
        //            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
        //            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;
        //        }
        //    }
        //    else
        //    {
        //        this.Panel1.Visible = true;
        //        this.Panel2.Visible = false;

        //        if (radioAssetType.SelectedValue == "Opex")
        //        {
        //            ocStatues = OpexCapexStatus.Opex;
        //        }
        //        else
        //        {
        //            switch (radioFANO.SelectedValue)
        //            {
        //                case "FANO":
        //                    ocStatues = OpexCapexStatus.Capex_AssetNo;
        //                    break;
        //                case "WithoutFANO":
        //                    ocStatues = OpexCapexStatus.Capex_NoAssetNo;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }

        //    //根据路径分别设置Opex_Capex_Status、Installment_Opex_Capex_Status
        //    this.PaymentDataForm.Opex_Capex_Status = ocStatues;
        //    this.InstallmentForm.Installment_Opex_Capex_Status = ocStatues;

        //    Session["ContractPONo"] = null;
        //    Session["PRMode"] = PaymentRequestMode.New;

        //}
        private string msg = "";
        private string url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string ocStatues = OpexCapexStatus.Opex;
            if (!IsPostBack)
            {
                Session["ContractPONo"] = null;
                Session["PRMode"] = PaymentRequestMode.New;
                if (Request.QueryString["PONO"] != null)
                {
                    DataTable dTable = null;
                    string poId = Request.QueryString["PONO"].ToString();
                    if (CheckPaymentNextStep(poId) == false)
                    {
                        Response.Write("<script type=\"text/javascript\">alert('" + msg + "');window.location = '" + url + "';</script>");
                        Response.End();
                        return;
                    }
                    //如果是从PO过来的分期付款
                    if (Request.QueryString["IsFromPO"] != null)
                    {
                        //dTable = PaymentRequestComm.GetPruchaseRequestInfo(poId).GetDataTable();
                        dTable = WorkFlowUtil.GetCollectionByList("Purchase Request Workflow")
                                             .GetDataTable()
                                             .AsEnumerable()
                                             .Where(dr => dr.Field<string>("PONumber").AsString().Contains(poId))
                                             .CopyToDataTable();
                        SetShowPODetailsInfo(poId);
                        this.hfNoticeStatus.Value = "FromPO";
                        this.PaymentDataForm.ChooseEmployeeStatus = "none";
                        Employee employeeFromPO = UserProfileUtil.GetEmployeeEx(SPContext.Current.Web.CurrentUser.LoginName);
                        this.PaymentDataForm.ApplicantEmployee = employeeFromPO;
                    }
                    else
                    {
                        dTable = PaymentRequestComm.GetPaymentRequestInfo(poId).GetDataTable();
                        this.hfNoticeStatus.Value = "FromIMG";
                        this.PaymentDataForm.ChooseEmployeeStatus = "none";
                        DataTable dt = PaymentRequestComm.GetPaymentRequestItemsInfoByPONO(poId).GetDataTable();
                        string strAccount = dt.Rows[0]["Applicant"].AsString().Substring(dt.Rows[0]["Applicant"].AsString().IndexOf("(") + 1, dt.Rows[0]["Applicant"].AsString().IndexOf(")") - dt.Rows[0]["Applicant"].AsString().IndexOf("(") - 1);
                        Employee employee = UserProfileUtil.GetEmployeeEx(strAccount);
                        this.PaymentDataForm.ApplicantEmployee = employee;
                    }

                    if (dTable.Rows.Count > 0)
                    {
                        switch (dTable.Rows[0]["RequestType"].ToString())
                        {
                            case "Capex":
                                ocStatues = OpexCapexStatus.Capex_AssetNo;
                                break;
                            case "Opex":
                                ocStatues = OpexCapexStatus.Opex;
                                break;
                            case "Capex_AssetNo":
                                ocStatues = OpexCapexStatus.Capex_AssetNo;
                                break;
                            default:
                                break;
                        }
                    }

                    this.ViewState["ocStatues"] = ocStatues;
                    this.PaymentDataForm.RequestType = ocStatues;
                    this.PaymentDataForm.HF_D_RequestType = ocStatues;
                    this.PaymentDataForm.Opex_Capex_Status = ocStatues;
                    this.InstallmentForm.Installment_Opex_Capex_Status = ocStatues;
                    this.InstallmentForm.HF_I_RequestType = ocStatues;

                }
                else
                {
                    object obj = Request.QueryString["RequestType"];
                    if (null != obj)
                    {
                        switch (obj.ToString())
                        {
                            case "Opex":
                                ocStatues = OpexCapexStatus.Opex;
                                break;
                            case "Capex_AssetNo":
                                ocStatues = OpexCapexStatus.Capex_AssetNo;
                                break;
                            case "Capex_NoAssetNo":
                                ocStatues = OpexCapexStatus.Capex_NoAssetNo;
                                break;
                        }
                    }
                    this.hfNoticeStatus.Value = "FromIMG";
                    this.ViewState["ocStatues"] = ocStatues;
                    this.PaymentDataForm.RequestType = ocStatues;
                    this.PaymentDataForm.HF_D_RequestType = ocStatues;
                    this.PaymentDataForm.Opex_Capex_Status = ocStatues;
                    this.InstallmentForm.Installment_Opex_Capex_Status = ocStatues;
                    this.InstallmentForm.HF_I_RequestType = ocStatues;
                    this.PaymentDataForm.GRStatus = "1";

                    object obj2 = Request.QueryString["WorkFlowNumber"];
                    if(null!=obj2)
                    {
                        string workFlowNumber = obj2.AsString();
                        this.PaymentDataForm.CopyStatus = "Copy";
                        this.InstallmentForm.CopyStatus = "Copy";
                        this.PaymentDataForm.WorkFlowNumber = workFlowNumber;
                        this.InstallmentForm.WorkFlowNumber = workFlowNumber;
                        this.InstallmentForm.RequestId = workFlowNumber;
                        DataTable data = PaymentRequestComm.GetPaymentRequestItemsInfoBySUBPRNO(workFlowNumber).GetDataTable();
                        InstallmentForm.PONO = data.Rows[0]["PONo"].ToString();
                        this.InstallmentForm.SummaryAmount = "SummaryAmount";
                    }
                }
            }

            this.StartWorkflowButton1.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton2.Executing += this.StartWorkflowButton_Executing;
            this.StartWorkflowButton1.Executed += this.StartWorkflowButton_Executed;
            this.StartWorkflowButton2.Executed += this.StartWorkflowButton_Executed;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poId"></param>
        /// <returns></returns>
        private bool CheckPaymentNextStep(string poId)
        {
            DataTable piTable = PaymentRequestComm.GetPaymentInstallmentInfo(poId).GetDataTable();
            if (piTable != null && piTable.Rows.Count > 0)
            {
                foreach (DataRow row in piTable.Rows)
                {
                    if (row["IsPaid"].ToString() == "0" || row["IsPaid"].ToString().IsNullOrWhitespace())
                    {
                        if (row["IsNeedGR"].ToString() == "1")
                        {
                            bool isGR = true;
                            DataTable dt = PaymentRequestComm.GetPurchaseOrderWorkflowInfo(poId).GetDataTable();
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                string s = dt.Rows[0]["IsSystemGR"].ToString();
                                if (s == "0" || s.IsNullOrWhitespace())
                                {
                                    isGR = false;
                                }
                            }

                            if (isGR == false)
                            {
                                //string msg = string.Empty;
                                //string url = Request.UrlReferrer.ToString();
                                url = Request.UrlReferrer.ToString();
                                msg = piTable.Rows.Count == 1 ? "当前是一次性付款，需要商店做完系统收货后才能进行付款" : "当前是第 " + row["Index"].ToString() + " 次付款申请，需要商店做完系统收货后才能进行付款";



                                return false;
                            }
                        }
                        break;
                    }
                }
            }

            return true;
        }

        private void StartWorkflowButton_Executing(object sender, CancelEventArgs e)
        {
            NameCollection managerNameColl = new NameCollection();
            bool isSave = false, isConfirm = false, isContinue = false;
            var startWorkflosButten = sender as StartWorkflowButton;
            var status = CAWorkflowStatus.InProgress;
            var manager = string.Empty;

            if (string.Equals(startWorkflosButten.Text, "Save", StringComparison.CurrentCultureIgnoreCase))
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
                        url = Request.UrlReferrer.ToString();
                        Response.Write("<script type=\"text/javascript\">alert('The manager is not set in the system.');window.location = '" + url + "';</script>");
                        Response.End();
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    bool isFromPO = (Request.QueryString["IsFromPO"] != null);
                    //string groupName = (isFromPO == true ? PaymentRequestGroupNames.Opex_ConstructionPO_SAPReview : 
                    //    PaymentRequestGroupNames.Opex_GeneralPO_SAPReview);
                    string groupName = "";
                    if (isFromPO == true)
                    {
                        if (ViewState["ocStatues"].ToString().ToLower() == "opex")
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
                        if (ViewState["ocStatues"].ToString().ToLower() == "opex")
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
                            url = Request.UrlReferrer.ToString();
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
                                url = Request.UrlReferrer.ToString();
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

            SetPaymentDataFormProperty(manager);
            bool result = PaymentDataForm.SavePeymentRequestData(InstallmentForm, WorkflowContext.Current.DataFields);
            if (result == false)
            {
                //DisplayMessage(PaymentDataForm.ErrorMessage);
                url = Request.UrlReferrer.ToString();
                Response.Write("<script type=\"text/javascript\">alert('" + PaymentDataForm.ErrorMessage + "');window.location = '" + url + "';</script>");
                Response.End();
                e.Cancel = true;
                return;
            }

            SetWorkFlowApproveUser(managerNameColl);
            SetWorkflowVaribale(status, isSave, isConfirm, isContinue);

            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            fields["SummaryExpenseType"] = this.InstallmentForm.SummaryExpenseType;
            fields["RequestType"] = this.ViewState["ocStatues"].ToString();
            fields["TaxPrice"] = this.InstallmentForm.TaxPrice;
            fields["GRStatus"] = this.PaymentDataForm.GRStatus;

            fields["ExchRate"] = this.InstallmentForm.ExchangeRate;
            fields["Currency"] = this.InstallmentForm.Currency;

            fields["Applicant"] = this.PaymentDataForm.ApplicantEmployee.DisplayName + "(" + this.PaymentDataForm.ApplicantEmployee.UserAccount + ")";

            if (PaymentDataForm.IsFromPO == false && int.Parse(PaymentDataForm.PaidIndex) == 1)
            {
                //InstallmentForm
                InstallmentForm.Update();
                PaymentRequestComm.AddItemTable1(this.InstallmentForm, fields);
                //PaymentRequestComm.DeleteAllDraftSAPItems(fields["SubPRNo"].AsString());
                PaymentRequestComm.SaveSAPItemsDetails1(this.InstallmentForm);
            }
            else
            {
                PaymentDataForm.Update();
                PaymentRequestComm.AddItemTable(this.PaymentDataForm, fields);
                //PaymentRequestComm.DeleteAllDraftSAPItems(fields["SubPRNo"].AsString());
                PaymentRequestComm.SaveSAPItemsDetails(this.PaymentDataForm);
            }

            WorkFlowUtil.UpdateWorkflowPath(WorkflowContext.Current, "SubPRNo");
        }

        private void SetPaymentDataFormProperty(string manager)
        {
            var userName = this.PaymentDataForm.ApplicantEmployee.UserAccount;
            var applicant = UserProfileUtil.GetEmployeeEx(userName);
            PaymentDataForm.Manager = manager;
            PaymentDataForm.PRNO = mWorkflowNumber;
            PaymentDataForm.Applicant = applicant.DisplayName;
            if (manager.IsNullOrWhitespace() == false)
            {
                PaymentDataForm.Approvers = ReturnAllApprovers(manager);
                PaymentDataForm.ApproversSPUser = ReturnAllApproversSP("ApproversSPUser", manager);
            }

            PaymentDataForm.Applicant = applicant != null ? applicant.DisplayName + "(" + applicant.UserAccount + ")" : string.Empty;
            PaymentDataForm.ApplicantSPUser = SPContext.Current.Web.EnsureUser(applicant.UserAccount);
            PaymentDataForm.Status = CAWorkflowStatus.InProgress;
        }

        private void SetWorkFlowApproveUser(NameCollection managerNameColl)
        {
            var context = WorkflowContext.Current;
            //context.UpdateWorkflowVariable("ConfirmTaskUsers", managerNameColl);
            //context.UpdateWorkflowVariable("ApproveTaskUsers", managerNameColl);
            context.UpdateWorkflowVariable("ConfirmTaskUsers", GetDelemanNameCollection(managerNameColl, "129"));
            context.UpdateWorkflowVariable("ApproveTaskUsers", GetDelemanNameCollection(managerNameColl, "129"));
        }

        /// <summary>
        /// 设置工作流变量值
        /// </summary>
        private void SetWorkflowVaribale(string status, bool isSave, bool isConfirm, bool isContinue)
        {
            var context = WorkflowContext.Current;
            var userName = SPContext.Current.Web.CurrentUser.LoginName;
            var applicant = UserProfileUtil.GetEmployeeEx(userName);
            //PaymentDataForm
            //var taskTitle = this.PaymentDataForm.ApplicantEmployee.DisplayName + "'s Payment Request ";
            var taskTitle = PaymentDataForm.SubPRWorkFlowNumber + " " + PaymentDataForm.VendorNameText + " " + PaymentDataForm.ApproveAmount + " " + this.PaymentDataForm.ApplicantEmployee.DisplayName + "'s Payment Request ";
            var editURL = "/_Layouts/CA/WorkFlows/PaymentRequest/EditForm.aspx";
            var approveURL = "/_Layouts/CA/WorkFlows/PaymentRequest/ApproveForm.aspx";
            context.UpdateWorkflowVariable("IsSave", isSave);
            context.UpdateWorkflowVariable("IsConfirm", isConfirm);
            context.UpdateWorkflowVariable("IsContinue", isContinue);
            context.UpdateWorkflowVariable("CompleteTaskFormUrl", editURL);
            context.UpdateWorkflowVariable("ApproveTaskFormUrl", approveURL);
            context.UpdateWorkflowVariable("ConfirmTaskFormUrl", approveURL);
            context.UpdateWorkflowVariable("ApproveTaskTitle", taskTitle );
            context.UpdateWorkflowVariable("ConfirmTaskTitle", taskTitle);
            context.UpdateWorkflowVariable("CompleteTaskTitle", "Please complete Payment Request");
        }

        private void StartWorkflowButton_Executed(object sender, EventArgs e)
        {
            RedirectToTask();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            //对需要加载页上的所有其他控件的任务使用该事件。
            base.OnLoadComplete(e);
        }
    }
}