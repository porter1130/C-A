<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" EnableEventValidation="false"
    MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.TravelExpenseClaim.EditForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Travel Expense Claim Form
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/jquery-ui.css" />
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/jquery-ui.custom.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery.bgiframe.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
    <script type="text/javascript">
        $(function () {
            SetDataViewStyle();
        });

        function SetDataViewStyle() {
            
            $('#tb_travelexpense td.td_rmbamt input').each(function () {

                var preId = GetPreId($(this).attr('id'), 'txtRmbAmt');
                var companyStandsValue = $('#' + preId + 'lblCompanyStandards').text();
                var rmbAmt = $(this).val();

                if (!ca.util.emptyString(companyStandsValue)
                && !ca.util.emptyString(rmbAmt)
                && !isNaN(companyStandsValue)
                && !isNaN(rmbAmt)
                && (parseFloat(rmbAmt) > parseFloat(companyStandsValue))) {
                    $(this).attr('style', 'color:red');
                    //$('#' + preId + 'lblSpecialApprove').text('Yes');
                    $(this).parents('tr').first().next().find('td.td_remark').show();
                }

            });

            $('#<%=this.btnTRNumber.ClientID %>').val('<%=this.DataForm1.TrWorkflowNumber %>');
        }
    </script>
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
                <asp:Button ID="btnTRNumber" Text="TRNumber" runat="server" OnClick="btnTRNumber_Click" CausesValidation="false" />
                <cc1:CAActionsButton ID="Actions" runat="server" CausesValidation="true" />
                <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="false" />
                <input type="button" value="Back" onclick="window.location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx';" />
            </div>
            <uc1:DataForm ID="DataForm1" runat="server" RequireValidation="true" />
            <SharePoint:FormDigest ID="FormDigest1" runat="server">
            </SharePoint:FormDigest>
        </QFL:ListFormControl>
    </div>
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
