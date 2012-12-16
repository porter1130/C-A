<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" EnableEventValidation="false"
    MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.CreditCardClaim.NewForm" Title="Company Credit Card Claim" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
    <%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Company Credit Card Claim
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Company Credit Card Claim
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
            <div class="ca-workflow-form-buttons noPrint">
                <QFC:StartWorkflowButton ID="StartWorkflowButton1" WorkflowName="Credit Card Claim Workflow"
                    runat="server" Text="Submit"  OnClientClick="return CheckClaim()"/>
                <QFC:StartWorkflowButton ID="StartWorkflowButton2" WorkflowName="Credit Card Claim Workflow"
                    runat="server" Text="Save" CausesValidation="false" OnClientClick="return CheckClaim()"/>
                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
            </div>
            <uc1:DataForm ID="DataForm1" runat="server" RequireValidation="true" ControlMode="New"/>
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
            1. This Credit Card Claim Form is for clearing of balances you spent by signing
            the Company credit card. However, this form does NOT cover:<br />
            i. Credit card item you spent during business trips applied through Travel Request;
            for those items, please file the claim through Travel Expense Claim Form.<br />
            ii. Personal spending; please pass cash to Finance to clear the related outstanding
            balance with Finance.<br />
            For the remaining credit card spending, please use this Credit Card Claim Form to
            file the claim.<br />
            公司信用卡报销申请表是给员工报销用公司信用卡签署的销费，但这申请单并不包括:<br />
            i. 经差旅申请批准的出差时签署的信用卡销费; 这些销费请用差旅报销申请表报销。<br />
            ii. 个人销费; 请将相关的款项交致财务部以清理你名下的公司欠款。<br />
            其他的信用卡销费请用此公司信用卡报销申请表进行报销。<br />
            2. Finance will base on the Travel Expense and the Credit Card Claim Forms to clear
            your credit card payable with the Company and review the outstanding balances with
            you on a periodical basis.<br />
            财务将据出差报销申请表及信用卡报销申请表清理你信用卡帐单下欠公司的款项，并定期审查追讨余额。<br />
            3. Please print out the approved Credit Card Claim Form; attach the original invoices
            of expenses and submitted to Finance for checking.<br />
            通过直线经理审批后，请打印公司信用卡报销申请单，将所有原始发票附在申请单后面，提交财务核查。
            <br />
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
</asp:Content>
