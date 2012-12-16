using System.Collections;
using System.Configuration;
namespace CA.WorkFlow.UI
{
    public static class WorkflowListName
    {
        public const string UnLockWorkflow = "UnlockWorkflow";

        public const string WorkflowDocumentLibrary = "Workflow Document Library";

        public const string ExpenseClaimSAPGLAccount = "Expense Claim SAP GLAccount";

        //Travel Request
        public const string TravelRequestWorkflow2 = "Travel Request Workflow2";
        public const string TravelHotelInfo2 = "Travel Hotel Info2";
        public const string TravelDetails2 = "Travel Details2";
        public const string TravelVehicleInfo2 = "Travel Vehicle Info2";


        //Travel Expense Claim
        public const string TravelExpenseClaim = "Travel Expense Claim";
        public const string TravelExpenseClaimDetails = "Travel Expense Claim Details";
        public const string TravelExpenseClaimForSAP = "Travel Expense Claim For SAP";
        public const string TravelExpenseClaimDetailsForSAP = "Travel Expense Claim Details For SAP";

        #region Credit Card Claim
        public const string CreditCardClaim = "Credit Card Claim Workflow";
        public const string CreditCardBill = "Credit Card Bill";
        public const string CreditCardEmployeeMapping = "Credit Card Employee Mapping";
        public const string CreditCardEmployeeDelegateMapping = "Credit Card Employee Delegate Mapping";
        
        #endregion
    }

    public static class WorkflowFolderName
    {
        public const string PurchaseRequest = "PurchaseRequest";

        public const string CreditCardClaim = "CreditCardClaim";
    }

    public static class WorkflowConfigName
    {
        //Purchase Request Workflow
        public const string PRPaperBagCol = "PR_PaperBag_ExcelCols";
        public const string PRPaperBagPK = "PR_PaperBag_ExcelPrimary";
        public const string PRPaperBagPosition = "PR_PaperBag_ExcelPos";
        public const string PRHOCol = "PR_HO_ExcelCols";
        public const string PRHOPK = "PR_HO_ExcelPrimary";
        public const string PRHOPosition = "PR_HO_ExcelPos";

        //PADPosition   PADPK  PADCol
        public const string PADPosition = "PAD_ExcelPos";
        public const string PADPK = "PAD_ExcelPrimary";
        public const string PADCol = "PAD_ExcelCols";

        //Credit Card Claim Workflow
        public const string CreditCardCol = "CC_ExcelCols";
        public const string CreditCardPK = "CC_ExcelPrimary";
        public const string CreditCardPosition = "CC_ExcelPos";
        public const string CreditCardBillExp = "CC_ExcelBillExp";
        public const string CreditCardSheetName = "CC_ExcelSheetName";
        public const string CreditCardClaim = "Credit Card Claim Workflow";
        public const string CreditCardBill = "Credit Card Bill";
        public const string CreditCardBillWorkflow = "Credit Card Bill Workflow";
        public const string CreditCardClaimDetail = "Credit Card Claim Detail";
        public const string CreditCardEmployeeMapping = "Credit Card Employee Mapping";
        public const string CreditCardClaimUrl = "/_Layouts/CA/WorkFlows/CreditCardClaim/";

        //Travel Expense Claim For SAP
        public const string TravelExpenseClaimForSAP = "Travel Expense Claim For SAP Workflow";
        public const string TravelExpenseClaimForSAPUrl = "/_Layouts/CA/WorkFlows/TravelExpenseClaimForSAP/";
    }

    //PRPO中服务费常量，配置在web.config中
    public static class WorkflowItemCode
    {
        public static string INSTALLATION = ConfigurationManager.AppSettings["installation"];
        public static string TRANSPORTATION = ConfigurationManager.AppSettings["transportation"];
        public static string PACKAGING = ConfigurationManager.AppSettings["packaging"];
        public static string DISCOUNT = ConfigurationManager.AppSettings["discount"];
    }

    public static class WebURL
    {
        public static string RootURL = CommonUtil.GetRootURL(ConfigurationManager.AppSettings["rootweburl"]);
        public static string RootWFURL = WebURL.RootURL + "WorkFlowCenter/";
    }

    public static class WorkflowGroupName
    {
        //Travel Expense Claim For SAP WF
        public static string WF_Accountants = "wf_Accountants";
        public static string WF_FinanceManager = "wf_FinanceManager";
        public static string WF_FinanceConfirm = "wf_FinanceConfirm";

        public static string wf_FinanceConfirm_CreditCard = "wf_FinanceConfirm_CreditCard";
        public static string WF_FinanceManager_CreditCard = "WF_FinanceManager_CreditCard";
    }

}