using System.Collections;
using System.Configuration;
namespace CA.WorkFlow.UI
{
    internal static class WorkflowListName
    {
        public const string WorkflowDocumentLibrary = "Workflow Document Library";

        public const string ExpenseClaimSAPGLAccount = "Expense Claim SAP GLAccount";

        //Travel Expense Claim
        public const string TravelExpenseClaim = "Travel Expense Claim";
        public const string TravelExpenseClaimDetails = "Travel Expense Claim Details";
        public const string TravelExpenseClaimForSAP = "Travel Expense Claim For SAP";
        public const string TravelExpenseClaimDetailsForSAP = "Travel Expense Claim Details For SAP";

        #region Credit Card Claim
        public const string CreditCardClaim = "Credit Card Claim Workflow";
        public const string CreditCardBill = "Credit Card Bill";
        #endregion
    }

    internal static class WorkflowFolderName
    {
        public const string PurchaseRequest = "PurchaseRequest";

        public const string CreditCardClaim = "CreditCardClaim";
    }

    internal static class WorkflowConfigName
    {
        //Purchase Request Workflow
        public const string PRPaperBagCol = "PR_PaperBag_ExcelCols";
        public const string PRPaperBagPK = "PR_PaperBag_ExcelPrimary";
        public const string PRPaperBagPosition = "PR_PaperBag_ExcelPos";
        public const string PRHOCol = "PR_HO_ExcelCols";
        public const string PRHOPK = "PR_HO_ExcelPrimary";
        public const string PRHOPosition = "PR_HO_ExcelPos";

        //Credit Card Claim Workflow
        public const string CreditCardCol = "CC_ExcelCols";
        public const string CreditCardPK = "CC_ExcelPrimary";
        public const string CreditCardPosition = "CC_ExcelPos";
        public const string CreditCardClaim = "Credit Card Claim Workflow";
        public const string CreditCardBill = "Credit Card Bill";
        public const string CreditCardClaimDetail = "Credit Card Claim Detail";
        public const string CreditCardEmployeeMapping = "Credit Card Employee Mapping";
        public const string CreditCardClaimUrl = "/_Layouts/CA/WorkFlows/CreditCardClaim/";

        //Travel Expense Claim For SAP
        public const string TravelExpenseClaimForSAP = "Travel Expense Claim For SAP Workflow";
        public const string TravelExpenseClaimForSAPUrl = "/_Layouts/CA/WorkFlows/TravelExpenseClaimForSAP/";
    }

    //PRPO中服务费常量，配置在web.config中
    internal static class WorkflowItemCode
    {
        public static string INSTALLATION = ConfigurationManager.AppSettings["installation"];
        public static string TRANSPORTATION = ConfigurationManager.AppSettings["transportation"];
        public static string PACKAGING = ConfigurationManager.AppSettings["packaging"];
        public static string DISCOUNT = ConfigurationManager.AppSettings["discount"];
    }

    internal static class WebURL
    {
        public static string RootURL = CommonUtil.GetRootURL(ConfigurationManager.AppSettings["rootweburl"]);
        public static string RootWFURL = WebURL.RootURL + "WorkFlowCenter/";
    }

    internal static class WorkflowGroupName
    {
        //Travel Expense Claim For SAP WF
        public static string WF_Accountants = "wf_Accountants";
        public static string WF_FinanceManager = "wf_FinanceManager";
    }

}