<%@ Page Title="Expatriate Benefit Claim Form" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true"
    CodeBehind="EditForm.aspx.cs" Inherits="CA.WorkFlow.UI.EBC.EditForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="DataEdit2.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Expatriate Benefit Claim Form
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Expatriate Benefit Claim Form
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="ca-supplier-form">
        <div class="ContentDiv">
            <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
            <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
                <div class="ca-workflow-form-buttons noPrint">
                    <cc1:CAActionsButton ID="Actions" runat="server" OnClientClick="return CheckSubmit()" />
                    <asp:Button runat="server" ID="btnSave" Text="Save"   OnClientClick="return CheckSubmit()" />
                    <input type="button" value="Back" onclick="window.location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx';" />
                </div>
                <uc1:DataForm ID="DataForm" runat="server" />
                <SharePoint:FormDigest ID="FormDigest1" runat="server">
                </SharePoint:FormDigest>
            </QFL:ListFormControl>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
    <div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
        <div class="middle">
            Note:<br />
            1. If you claim benefits in foreign currency, please fill-in the RMB equivalent
            amount. You can convert the expense to RMB using the exchange rate you used to make
            the payment, and attach the bank slip as supporting. Without the supporting, Finance
            will review if the rate is reasonable. If deemed unreasonable, Finance has the right
            to use BOC’s current rate. If you are not sure what rate to use, please consult
            Finance.
            <br />
            报销外币福利时，请使用付款时使用的汇率，折算后输入人民币金额，并附上相应的银行小票。 如无法提供银行小票，财务会审视汇率是否合 理，必要时会改用现时的中国银行中间价结算报销金额。如不确定用什么汇率，请向财务咨询。<br />
            2. Please print out the approved claim form; attach original invoice and pass them
            to Finance. Finance will review the supporting. If there is no problem, Finance
            will process payment.
            <br />
            请打印经审批后的福利报销申请单，将原始发票附在申请单后面，提交财务。财务会审核申请，若没有问题，财务会安排付款。
            <br />
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
</asp:Content>
