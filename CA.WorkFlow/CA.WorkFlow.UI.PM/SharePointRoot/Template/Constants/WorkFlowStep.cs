namespace CA.WorkFlow.UI.Constants
{
    /// <summary>
    /// 工作流Step And Account常量
    /// </summary>
    public class WorkFlowStep
    {
        #region WorkFlow--TimeOffWorkFlow
        //工作流Accounts
        public const string TimeOffDepartmentHeadAccount = "DepartmentHeadAccount";
        public const string TimeOffHRAccounts = "HRAccounts";
        public const string TimeOffHRHeadAccount = "HRHeadAccount";
        //工作流Step
        public const string TimeOffDepartmentHeaderApprove = "DepartmentHeaderApprove";
        public const string TimeOffHRReview = "HRReview";
        public const string TimeOffHRHeadApprove = "HRHeadApprove";
        public const string TimeOffApplicantModify = "ApplicantModify";//初始提交
        //工作流任务操作人
        public const string TimeOffApprovers = "Approvers";
        public const string TimeOffApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--New Employee Equipment Application
        //工作流Accounts
        public const string EquipmentFunctionalManager = "FunctionalManager";
        public const string EquipmentDH = "DH";
        public const string EquipmentITGroup = "ITGroup";
        //工作流Step
        public const string EquipmentFunctionalManagerApprove = "FunctionalManagerApprove";
        public const string EquipmentDepartmentHeadApprove = "DepartmentHeadApprove";
        public const string EquipmentITConfirm = "ITConfirm";
        public const string EquipmentHRSubmit = "HRSubmit";//初始提交
        //工作流任务操作人
        public const string EquipmentApprovers = "Approvers";
        public const string EquipmentApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--Business Card Application
        //工作流Accounts
        public const string BusinessCardDDepartmentHeadApprove_User = "DepartmentHeadApprove_User";
        public const string BusinessCardDReceptionistConfirm_Group = "ReceptionistConfirm_Group";
        //工作流Step
        public const string BusinessCardDDepartmentHeadApprove = "DepartmentHeadApprove";
        public const string BusinessCardDReceptionistConfirm = "ReceptionistConfirm";
        public const string BusinessCardDApplicantedit = "applicantedit";//初始提交
        //工作流任务操作人
        public const string BusinessCardApprovers = "Approvers";
        public const string BusinessCardApproverLoginName = "ApproversLoginName";
        #endregion

        #region //WorkFlow--Travel Request
        #endregion

        #region WorkFlow--Supplier Reticketing Workflow
        //工作流Accounts
        public const string SupplierReticketingBuyingApproveUsers = "BuyingApproveUsers";
        public const string SupplierReticketingDepartmentHead = "DepartmentHead";
        public const string SupplierReticketingFinanceTaskUsers = "FinanceTaskUsers";
        //工作流Step
        public const string SupplierReticketingBuyingApprove = "BuyingApprove";
        public const string SupplierReticketingDepartmentHeadApproval = "DepartmentHeadApproval";
        public const string SupplierReticketingFinanceTask = "FinanceTask";
        public const string SupplierReticketingCompleteTask = "CompleteTask";//初始提交
        //工作流任务操作人
        public const string SupplierReticketingApprovers = "Approvers";
        public const string SupplierReticketingApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--Store Sampling Workflow
        //工作流Accounts
        public const string StoreSamplingBuyer = "Buyer";
        public const string StoreSamplingStoreManager = "StoreManager";
        public const string StoreSamplingAreaManagerApproveUser = "AreaManagerApproveUser";
        public const string StoreSamplingBSSHeadGroup = "BSSHeadGroup";
        public const string StoreSamplingBSSTeamAccount = "BSSTeamAccount";
        public const string StoreSamplingFinanceGroup = "FinanceGroup";
        //工作流Step
        public const string StoreSamplingBuyerApprove = "BuyerApprove";
        public const string StoreSamplingStoreManagerApprove = "StoreManagerApprove";
        public const string StoreSamplingAreaManagerApprove = "AreaManagerApprove";
        public const string StoreSamplingBSSHeadApprove = "BSSHeadApprove";
        public const string StoreSamplingBSSTeamApprove = "BSSTeamApprove";
        public const string StoreSamplingFinanceConfirm = "FinanceConfirm";
        public const string StoreSamplingStoreAdminSubmit = "StoreAdminSubmit";//初始提交
        //工作流任务操作人
        public const string StoreSamplingApprovers = "Approvers";
        public const string StoreSamplingApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--New Supplier Creation WorkFlow
        //工作流Accounts
        public const string SupplierCreationManager = "Manager";
        public const string SupplierCreationBuyDirector = "BuyDirector";
        public const string SupplierCreationHeader = "Header";
        public const string SupplierCreationBBSTeamAccount = "BBSTeamAccount";
        //工作流Step
        public const string SupplierCreationDivisionManagerApprove = "DivisionManagerApprove";
        public const string SupplierCreationBuyingDirector = "BuyingDirector";
        public const string SupplierCreationCommercialHeader = "CommercialHeader";
        public const string SupplierCreationBSSTeam = "BSSTeam";
        public const string SupplierCreationTaskUpdate = "TaskUpdate";//初始提交
        //工作流任务操作人
        public const string SupplierCreationApprovers = "Approvers";
        public const string SupplierCreationApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--Construction Purchasing Workflow
        //工作流Accounts
        public const string ConstructionPurchasingDepartmentHeadAccount = "DepartmentHeadAccount";
        public const string ConstructionPurchasingConstructionAccount = "ConstructionAccount";
        public const string ConstructionPurchasingConstructionHeadAccount = "ConstructionHeadAccount";
        public const string ConstructionPurchasingStoreOperationTeamAccount = "StoreOperationTeamAccount";
        public const string ConstructionPurchasingCFOAccount = "CFOAccount";
        //工作流Step
        public const string ConstructionPurchasingConstructionHead = "ConstructionHead";//DepartmentHeadAccount
        public const string ConstructionPurchasingConstruction = "Construction";//ConstructionAccount
        public const string ConstructionPurchasingConstructionHeadAgain = "ConstructionHeadAgain";//ConstructionHeadAccount
        public const string ConstructionPurchasingDepartmentHead = "DepartmentHead";//DepartmentHeadAccount
        public const string ConstructionPurchasingStoreOperationTeam = "StoreOperationTeam";//StoreOperationTeamAccount
        public const string ConstructionPurchasingCFO = "CFO";//CFOAccount
        public const string ConstructionPurchasingPlacesTheOrder = "PlacesTheOrder";//ConstructionAccount
        public const string ConstructionPurchasingOrderHandover = "OrderHandover";//订单移交到初始提交人
        public const string ConstructionPurchasingUpdateTask = "UpdateTask";//初始提交
        //工作流任务操作人
        public const string ConstructionPurchasingApprovers = "Approvers";
        public const string ConstructionPurchasingApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--Chopping Application Workflow
        //工作流Accounts
        public const string ChoppingDepartHeadAccount = "DepartHeadAccount";
        public const string ChoppingCFCOAccount = "CFCOAccount";
        public const string ChoppingLegalCounselAccount = "LegalCounselAccount";
        public const string ChoppingLegalHeadAccount = "LegalHeadAccount";
        public const string ChoppingCEOAccount = "CEOAccount";
        public const string ChoppingManagerAccount = "ManagerAccount";
        public const string ChoppingOtherLegalAccount = "OtherLegalAccount";
        //工作流Step
        public const string ChoppingDepartHeadApprove = "DepartHeadApprove";
        public const string ChoppingCFCOApprove = "CFCOApprove";
        public const string ChoppingLegalApprove = "LegalApprove";
        public const string ChoppingLegalHeadApprovet = "LegalHeadApprove";
        public const string ChoppingCEOApprove = "CEOApprove";
        public const string ChoppingManagerApprove = "ManagerApprove";
        public const string ChoppingOtherLegalApprove = "OtherLegalApprove";
        public const string ChoppingApplicantEdit = "ApplicantEdit";//初始提交
        //工作流任务操作人
        public const string ChoppingApprovers = "Approvers";
        public const string ChoppingApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--Change Request Workflow
        //工作流Accounts
        public const string ChangeRequestBusinessManagerGroup = "BusinessManagerGroup";
        public const string ChangeRequestITHead = "ITHead";
        public const string ChangeRequestITAppManagerGroup = "ITAppManagerGroup";
        //工作流Step 
        public const string ChangeRequestBusinessManagerGroupApprove = "BusinessManagerGroupApprove";//BusinessManagerGroup
        public const string ChangeRequestITHeadApprove = "ITHeadApprove";//ITHead
        public const string ChangeRequestITAppManagerGroupExecutes = "ITAppManagerGroupExecutes";//ITAppManagerGroup
        public const string ChangeRequestITAppManagerGroupSupplies = "ITAppManagerGroupSupplies";//ITAppManagerGroup
        public const string ChangeRequestITHeadApprove2 = "ITHeadApprove2";//ITHead
        public const string ChangeRequestBusinessManagerGroupApprove2 = "BusinessManagerGroupApprove2";//BusinessManagerGroup
        public const string ChangeRequestEmployeeTests = "EmployeeTests";//初始提交
        public const string ChangeRequestEmployeeSubmit = "EmployeeSubmit";//初始提交
        //工作流任务操作人
        public const string ChangeRequestApprovers = "Approvers";
        public const string ChangeRequestApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--IT Request WorkFlow
        //工作流Accounts
        public const string ITRequestITMemberAccounts = "ITMemberAccounts";
        public const string ITRequestDepartHeaderAccounts = "DepartHeaderAccounts";
        public const string ITRequestITHeaderAccounts = "ITHeaderAccounts";
        public const string ITRequestFOCOAccounts = "FOCOAccounts";
        //工作流Step 
        public const string ITRequestITMember = "ITMember";
        public const string ITRequestDepartmentHeader = "DepartmentHeader";
        public const string ITRequestITHeader = "ITHeader";
        public const string ITRequestFOCO = "FOCO";
        public const string ITRequestUpdateTask = "UpdateTask";//初始提交
        //工作流任务操作人
        public const string ITRequestApprovers = "Approvers";
        public const string ITRequestApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--New Store Construction Budget Workflow
        //工作流Accounts
        public const string StoreBudgetDepartmentHead = "DepartmentHead";
        public const string StoreBudgetCFOApprovalUser = "CFOApprovalUser";
        public const string StoreBudgetCEOApprovalUser = "CEOApprovalUser";
        public const string StoreBudgetFinanceTaskUsers = "FinanceTaskUsers";
        //工作流Step 
        public const string StoreBudgetConstructionHeadApproval = "ConstructionHeadApproval";
        public const string StoreBudgetCFOApproval = "CFOApproval";
        public const string StoreBudgetCEOApproval = "CEOApproval";
        public const string StoreBudgetFinanceTask = "FinanceTask";
        public const string StoreBudgetComplete = "Complete";//初始提交
        public const string StoreBudgetReSubmit = "ReSubmit";//初始提交
        //工作流任务操作人
        public const string StoreBudgetApprovers = "Approvers";
        public const string StoreBudgetApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--Supplier Reinspection Workflow
        //工作流Accounts
        public const string SupplierReinspectionDepartmentHead = "DepartmentHead";
        public const string SupplierReinspectionFinanceTaskUsers = "FinanceTaskUsers";
        //工作流Step 
        public const string SupplierReinspectionDepartmentHeadApproval = "DepartmentHeadApproval";
        public const string SupplierReinspectionFinanceTask = "FinanceTask";
        public const string SupplierReinspectionComplete = "Complete";//初始提交
        public const string SupplierReinspectionReSubmit = "ReSubmit";//初始提交
        //工作流任务操作人
        public const string SupplierReinspectionApprovers = "Approvers";
        public const string SupplierReinspectionApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--Store Maintenance Workflow
        //工作流Accounts
        public const string StoreMaintenanceCMManagerGroup = "CMManagerGroup";
        public const string StoreMaintenanceStoreManager = "StoreManager";
        public const string StoreMaintenanceAreaManager = "AreaManager";
        public const string StoreMaintenanceConstructionHead = "ConstructionHead";
        public const string StoreMaintenanceSOHead = "SOHead";
        //工作流Step 
        public const string StoreMaintenanceCMManagerGroupReview = "CMManagerGroupReview";//CMManagerGroup
        public const string StoreMaintenanceStoreManagerApprove = "StoreManagerApprove";//StoreManager
        public const string StoreMaintenanceAreaManagerApprove = "AreaManagerApprove";//AreaManager
        public const string StoreMaintenanceConstructionHeadApprove = "ConstructionHeadApprove";//ConstructionHead
        public const string StoreMaintenanceSOHeadApprove = "SOHeadApprove";//SOHead
        public const string StoreMaintenanceCMManagerGroupOrders = "CMManagerGroupOrders";//CMManagerGroup
        public const string StoreMaintenanceStoreManagerEvaluates = "StoreManagerEvaluates";//StoreManager
        public const string StoreMaintenanceRequestSubmit = "RequestSubmit";//初始提交
        //工作流任务操作人
        public const string StoreMaintenanceApprovers = "Approvers";
        public const string StoreMaintenanceApproverLoginName = "ApproversLoginName";
        #endregion

        #region WorkFlow--Non Trade Supplier Setup Maintenance Workflow
        //工作流Accounts
        public const string SupplierSetupMaintenanceDepartmentHeadTaskUsers = "DepartmentManagerTaskUsers";
        public const string SupplierSetupMaintenanceMDMTaskUsers = "MdmTaskUsers";
        public const string SupplierSetupMaintenanceCFOTaskUsers = "CfoTaskUsers";
        //工作流Step  
        public const string SupplierSetupMaintenanceDepartmentHeadTask = "DepartmentManagerTask";
        public const string SupplierSetupMaintenanceMDMTask = "MdmTask";
        public const string SupplierSetupMaintenanceCFOTask = "CfoTask";
        public const string SupplierSetupMaintenanceCompleteTask = "CompleteTask";//初始提交
        //工作流任务操作人
        public const string SupplierSetupMaintenanceApprovers = "ApproversLoginName";
        public const string SupplierSetupMaintenanceApproverLoginName = "Approvers";
        #endregion

        #region WorkFlow--CreationOrder
        //工作流Accounts
        public const string CreationOrderDepartmentManagerTaskUsers = "DepartmentManagerTaskUsers";
        public const string CreationOrderCFOTaskUsers = "CFOTaskUsers";
        public const string CreationOrderFinanceAnalystTaskUsers = "FinanceAnalystTaskUsers";
        //工作流Step  
        public const string CreationOrderDepartmentManagerTask = "DepartmentManagerTask";
        public const string CreationOrderCFOTask = "CFOTask";
        public const string CreationOrderFinanceAnalystTask = "FinanceAnalystTask";
        public const string CreationOrderCompleteTask = "CompleteTask";//初始提交
        //工作流任务操作人
        public const string CreationOrderApprovers = "ApproversLoginName";
        public const string CreationOrderApproverLoginName = "Approvers";
        #endregion

        #region WorkFlow--Internal Order Maintenance Workflow
        //工作流Accounts
        public const string InternalOrderMaintenanceDepartmentManagerTaskUsers = "DepartmentManagerTaskUsers";
        public const string InternalOrderMaintenanceCFOTaskUsers = "CfoTaskUsers";
        public const string InternalOrderMaintenanceFinanceAnalystTaskUsers = "FinanceAnlystTaskUsers";
        //工作流Step  
        public const string InternalOrderMaintenanceDepartmentManagerTask = "DepartmentManagerTask";
        public const string InternalOrderMaintenanceCFOTask = "CfoTask";
        public const string InternalOrderMaintenanceFinanceAnalystTask = "FinanceAnalystTask";
        public const string InternalOrderMaintenanceCompleteTask = "CompleteTask";//初始提交
        //工作流任务操作人
        public const string InternalOrderMaintenanceApprovers = "ApproversLoginName";
        public const string InternalOrderMaintenanceApproverLoginName = "Approvers";
        #endregion

        #region WorkFlow--TimeOffWorkCancel Workflow
        //工作流Accounts 
        public const string TimeOffWorkCancelDepartmentHeadAccount = "DepartmentHeadAccount";
        public const string TimeOffWorkCancelhrAccounts = "hrAccounts";
        public const string TimeOffWorkCancelHRHeadAccount = "HRHeadAccount";
        //工作流Step   
        public const string TimeOffWorkCancelDepartmentHeaderApprove = "DepartmentHeaderApprove";
        public const string TimeOffWorkCancelHRReview = "HRReview";
        public const string TimeOffWorkCancelHRHeadApprove = "HRHeadApprove";
        #endregion

        #region CashAdvance WorkFlow
        //工作流Accounts
        public const string CashAdvanceNextApproveTaskUsers = "NextApproveTaskUsers";
        //工作流Step
        public const string CashAdvanceNextApproveTask = "NextApproveTask";
        public const string CashAdvanceFinaceApprove = "FinaceApprove";
        public const string CashAdvanceFinaceConfirm = "FinaceConfirm";
        public const string CashAdvanceFinaceConfirmEnd = "FinaceConfirmEnd";
        //工作流任务操作人
        public const string CashAdvanceApprovers = "Approvers";
        public const string CashAdvanceApproversLoginName = "ApproversLoginName";
        #endregion

        #region EmployeeExpenseClaimWorkflow
        //工作流Accounts
        public const string EmployeeExpenseClaimNextApproveTaskUsers = "NextApproveTaskUsers";
        //工作流Step
        public const string EmployeeExpenseClaimNextApproveTask = "NextApproveTask";
        public const string EmployeeExpenseClaimConfirmTask = "ConfirmTask";
        //工作流任务操作人
        public const string EmployeeExpenseClaimApproversPerson = "ApproversLoginName";
        public const string EmployeeExpenseClaimApprovers = "Approvers";
        #endregion

        #region CreditCardClaimWorkflow
        //工作流Accounts
        public const string CreditCardClaimNextApproveTaskUsers = "NextApproveTaskUsers";
        public const string CreditCardClaimConfirmTaskUsers = "ConfirmTaskUsers";
        //工作流Step
        public const string CreditCardClaimNextApproveTask = "NextApproveTask";
        public const string CreditCardClaimConfirmTask = "ConfirmTask";
        //工作流任务操作人
        public const string CreditCardClaimApproversPerson = "Approvers";
        public const string CreditCardClaimClaimApprovers = "ApproversLoginName";
        #endregion
        
    }
}