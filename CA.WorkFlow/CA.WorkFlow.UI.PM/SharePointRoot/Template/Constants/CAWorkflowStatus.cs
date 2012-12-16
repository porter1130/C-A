namespace CA.WorkFlow.UI
{
    public static class CAWorkflowStatus
    {
        public const string Pending = "Pending";
        public const string InProgress = "In Progress";
        public const string Completed = "Completed";
        public const string Rejected = "Rejected";

        public const string NTVDepartmentHeadApprove = "Department Manager Approved";
        public const string NTVDepartmentHeadReject = "Department Manager Rejected";
        public const string NTVCFOApprove = "CFO Approved";
        public const string NTVCFOReject = "CFO Rejected";
        public const string NTVFinanceReject = "Finance Rejected";

        public const string IODepartmentManagerApprove = "Department Manager Approved";
        public const string IODepartmentManagerReject = "Department Manager Rejected";
        public const string IOToCFO = "To CFO";
        public const string IOFinanceReject = "Finance Rejected";
        public const string IOCFOApprove = "CFO Approved";
        public const string IOCFOReject = "CFO Rejected";

        public const string TRDepartmentManagerApprove = "Department Manager Approved";
        public const string TRDepartmentManagerReject = "Department Manager Rejected";
        public const string TRMTMApprove = "MTM Approved";
        public const string TRMTMReject = "MTM Rejected";
        public const string TRCFOApprove = "CFO Approved";
        public const string TRCFOReject = "CFO Rejected";
        public const string TRCEOApprove = "CEO Approved";
        public const string TRCEOReject = "CEO Rejected";
        public const string TRReceptionistTaskConfirming = "Confirming";

        public const string TE_Finance_Reject = "Finance Rejected";
        public const string TE_Finance_Pending = "Finance Pending";

        #region Travel Expense Claim For SAP Workflow
        public const string Reviewed = "Reviewed";
        #endregion

        #region Credit Card Claim Workflow
        public const string CCFinancePending = "Finance Pending";
        #endregion
    }
}