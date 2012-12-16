<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true"
    CodeBehind="ApproveForm.aspx.cs" Inherits="CA.WorkFlow.UI.EmployeeExpenseClaim2.ApproveForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<%@ Register Src="DataView.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Employee Expense Claim Form
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Employee Expense Claim Form
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
            <div class="ca-workflow-form-buttons noPrint">
                <cc1:CAActionsButton ID="Actions" runat="server" CausesValidation="true" />
                <asp:Button ID="btnNotice" runat="server" Text="Notice" />
                <QFC:MoreApproveButton runat="server" ID="More" />
                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx'" />
            </div>
            <table class="ca-workflow-form-table full-width" id="commettable">
                <tr>
                    <td class="label align-center w25">
                        Comments<br />
                        批注:
                    </td>
                    <td class="value" id="comment-task">
                        <QFL:TaskPanel runat="server" ID="task1">
                            <QFL:CommentTaskField runat="server" ID="CommentTaskField1" ControlMode="Edit" />
                        </QFL:TaskPanel>
                    </td>
                </tr>
            </table>
            <uc1:DataForm ID="DataForm1" runat="server" ControlMode="Display"></uc1:DataForm>
            <uc2:TaskTrace ID="TaskTrace1" runat="server" />
            <SharePoint:FormDigest ID="FormDigest3" runat="server">
            </SharePoint:FormDigest>
        </QFL:ListFormControl>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
    <div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
         <div class="middle">
            Note:<br />
            1. If you claim expenses in foreign currency, please fill-in the RMB equivalent
            amount. You can convert the expenses to RMB using the exchange rate you used to
            convert your currency, and attach the bank slip as supporting. Without the supporting,
            Finance will review the rate to ensure reasonableness. If deemed unreasonable, Finance
            has the right to use BOC’s current rate.<br />
            报销外币费用时，请使用换钱时使用的汇率，折算后输入人民币金额，并附上相应的银行小票。如无法提供 银行小票，财务会审视汇率是否合理，必要时会改用现时的中国银行中间价结算报销金额。<br />
            2. If Mobile Phone and Meal Allowance claimed exceed the amount specified in the
            Company Policy, special approvals from line managers are required. Managers are
            required to indicate approval on the “Special Approval” box next to each overspending
            item. If rejected, the amount claimed will fall back to the Company standard amount.<br />
            如果所报销的手机话费和餐饮补贴超过公司规定的标准，需要直线经理做特殊审批。即在每项超支后面出现 的特殊审批下的选择框中示意批准，若不获批准，将改用公司标准作为报销金额。<br />
            3. For mobile phone long distance call expenses, applicant must attach the detail
            phone list. Otherwise, Finance will reject your claim.<br />
            手机的长途话费需提供明细清单，否则，将被财务退回。<br />
            4. If you have cash advances, please select the cash advance to be deducted from
            your expense claim. According to Company Policy, cash advance has to be cleared
            one month after the cash advance is released to you. Finance has the right to deduct
            outstanding cash advance from your claim.<br />
            若有员工借款, 请在报销单中选择扣除，依照公司相关政策，员工向公司预借的现金，必需在拿到后的一个 月内报销结清。财务可从报销中扣除员工借款。<br />
            5. Please print out the approved Expense Claim Form; attach the original invoices
            of expenses and submitted to Finance for checking and reimbursement process.<br />
            通过直线经理审批后，请打印员工报销申请单，将所有原始发票附在申请单后面，提交财务核查并进行报销 付款流程。
            <br />
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
    <script type="text/javascript">
        function dispatchAction(sender) {
            //CheckPending
            if (sender.value === 'Pending') {
                var result1 = CheckPending();
                return result1;
            }
            if (sender.value === 'Approve') {
                var result2 = checkApprove();
                return result2;
            }
            if (sender.value === 'Reject') {
                var step = GetStep();
                if (step == "NextApproveTask") {
                    if (ca.util.emptyString($('#comment-task textarea').val())) {
                        if (jQuery.browser.msie) {
                            event.cancelBubble = true;
                        }
                        alert('Please fill in the Reject Comments.');
                        return false;
                    }
                }
                if (step == "ConfirmTask") {
                    var result3 = CheckPending();
                    return result3;
                }
            }
            return true;
        }
    </script>
</asp:Content>
