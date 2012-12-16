<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayForm.aspx.cs" EnableEventValidation="false"
    MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.TE.DisplayForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="DataView.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Travel Expense Claim Form
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
    <%--Modal Dialog--%>
    <div id="mask">
    </div>
    <%--End Modal Dialog--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Travel Expense Claim Form
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
            <div class="ca-workflow-form-buttons noPrint">
                <input type="button" value="Back" onclick="window.history.go(-1)" />
            </div>
            <uc1:DataForm ID="DataForm1" runat="server" />
            <uc2:TaskTrace id="TaskTrace1" runat="server" />
            <SharePoint:FormDigest ID="FormDigest3" runat="server">
            </SharePoint:FormDigest>
        </QFL:ListFormControl>
    </div>
    <script type="text/javascript">
        var wf_Status = '<%=this.Status %>';
        var wf_Step = '<%=this.Step %>';

        var fapiaoStatus = '<%=this.FapiaoStatus %>';
        var informationStatus = '<%=this.InformationStatus %>';
        var claimedAmtStatus = '<%=this.ClaimedAmtStatus %>';
        var otherReasonsStatus = '<%=this.OtherReasonsStatus %>';

        var TE_Finance_Pending = 'Finance Pending';
        var TE_Finance_Reject = 'Finance Rejected';
        var wf_Status_Completed = 'Completed';

        var wf_Task_FinanceConfirm = 'ConfirmTask';
        var wf_Task_Complete = 'CompleteTask';
        var wf_Task_End = 'end1';

        $(function () {
            SetControlMode();

        });
        function SetControlMode() {
            $('#tb_travelexpense td.td_specialapproval input').each(function () {
                $(this).attr('disabled', true);
                $(this).hide();
            });

            if (wf_Status == TE_Finance_Pending
                || (wf_Status == TE_Finance_Reject && wf_Step == wf_Task_Complete)) {

                if (fapiaoStatus != 'hidden'
                    || informationStatus != 'hidden'
                    || claimedAmtStatus != 'hidden'
                    || otherReasonsStatus != 'hidden') {
                    $('#table_pendingform').show();
                }
                if (fapiaoStatus == 'hidden') {
                    $('#table_pendingform tr.tr_fapiao').hide();
                    $('#table_pendingform tr.tr_fapiao').next().hide();
                }
                if (informationStatus == 'hidden') {
                    $('#table_pendingform tr.tr_information').hide();
                    $('#table_pendingform tr.tr_information').next().hide();
                }
                if (claimedAmtStatus == 'hidden') {
                    $('#table_pendingform tr.tr_claimedamt').hide();
                    $('#table_pendingform tr.tr_claimedamt').next().hide();
                }
                if (otherReasonsStatus == 'hidden') {
                    $('#table_pendingform tr.tr_otherreasons').hide();
                    $('#table_pendingform tr.tr_otherreasons').next().hide();
                }

                $('#table_pendingform :input').attr('contentEditable', false);
            }
            $('#tb_travelexpense td.td_originalamt').each(function () {
                var originalAmt = Escape($(this).text(), /,/);
                var $exchRate = Escape($(this).nextAll('.td_exchrate'), /,/);
                var $approvedRmbAmt = $(this).nextAll('.td_rmbamt');
                var $companyStandsValue = $(this).nextAll('.td_companystd');

                if (!isNaN($companyStandsValue.text())) {
                    var rmbAmt = (parseFloat(originalAmt) * parseFloat($exchRate.text())).toFixed(2);
                    if (rmbAmt != parseFloat($approvedRmbAmt.text())) {
                        $companyStandsValue.next().text('Reject');
                    } else {
                        if (rmbAmt > parseFloat($companyStandsValue.text())) {

                            if (wf_Step == ''
                            || wf_Step == wf_Task_End
                            || wf_Status == wf_Status_Completed) {
                                $companyStandsValue.next().text('Approve');
                            } else {
                                $companyStandsValue.next().text('In Progress');
                            }
                        }
                    }
                }
            });
            $('#div_overlay').show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
    <div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
        <div class="middle">
            Note:<br />
            1. The standard exchange rate is only for reference. If you claim expenses in foreign
            currency, you can use the rate you used to convert your currency, and attach the
            bank slip as supporting. Without the supporting, Finance will review if the rate
            is reasonable. If deemed unreasonable, Finance has the right to use BOC’s rate at
            the time of the travel. <br />
            报销外币费用时，报销申请表上的标准汇率只供参考, 请输入你换钱时使用的汇率，并附上相应的银行小票。如无法提供银行小票，财务会审视汇率是否合理，必要时会改用费用发生时的中国银行中间价结算报销金额。<br />
            2. If Hotel and Meal Allowance claimed exceed the Company standard, special approval
            from line manager is required. Managers are required to indicate approval on the
            “Special Approval” box next to each overspending item. If rejected, the amount claimed
            will fall back to the Company standard amount. <br />
            如果所报销的住宿和餐饮补贴超过公司规定的标准，需要直线经理做特殊审批，即在每项超支后面出现的特殊审批下的选择框中示意批准，若不获批准，将改用公司标准作为报销金额。<br />
            3. If your travel expense is paid by Company credit card, please click on the “Paid
            by Company Credit Card” box. Finance will base on this to clear your credit card
            payable with the Company. <br />
            如果申报的出差费用中有使用公司信用卡的，请在“由公司信用卡支付”的选择框中打勾。财务将据此清理你信用卡帐单下欠公司的款项。<br />
            4. Please print out the Travel Expense Claim Form; attach the original invoices
            (include those paid by Company credit card) and submitted to Finance for checking
            and reimbursement. <br />
            请打印出差报销申请单，将所有原始发票（包括信用卡支付的费用）附在申请单后面，提交财务核查并进行报销付款流程。
            <br />
            <br />
            <a href="/WorkFlowCenter/FlowCharts/TravelRequest.doc">Click here to view the flowchart
                of the workflow</a>
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
</asp:Content>
