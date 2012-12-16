<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentRequestTypeSelect.aspx.cs"
    MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.PaymentRequest.PaymentRequestTypeSelect" %>

<%@ Register Src="DataView.ascx" TagName="DataView" TagPrefix="uc1" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="PRTS.ascx" TagName="prts" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Payment Request Form
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Payment Request Form
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="ca-supplier-form">
        <uc1:prts ID="prts" runat="server" />
    </div>
    <div id="BGDiv">
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
    <div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
        <div class="middle">
            Note:<br />
            1.Asset is generally referred to purchased items with value > rmb2,000 and useful
            life > 1 year. If in doubt, please contact Finance.
            <br />
            资产是泛指购买的物品价值高于人民币两千元及使用年限超过一年。如有疑问，请向财务查询。<br />
            2.If contract/PO has been signed, please pass it to Finance, and get a contractno
            before preparation of the Payment Request.
            <br />
            若已签署合同，请在申请付款前，提交合同至财务部并拿取合同编号。<br />
            3.Payment Request with contract number and system GR done need not go through any
            approval/confirmation process. Submitted Payment Request will go directly to Finance.
            <br />
            若付款申请有合同编号并有做系统收货，申请不需经过任何审批/确认流程。提交的付款申请会直达财务。<br />
            4.Payment Request with contractnumber (but system GR not done) requires one level
            of managers (L7 or above) to confirm receipt of the goods.
            <br />
            注有合同编号的付款申请若没做系统收货，需要一层L7或以上的经理确认已收妥合同中的服务/商品。<br />
            5.If Payment Request is not supported by contract, full approval process needs to
            be carried out.<br />
            若申请付款前没有签署合同，付款申请需经过付款批准流程。<br />
            6.Please print out the form; attach original invoice and pass them to Finance. Finance
            will review the Payment Request. If there is no problem, Finance will process the
            payment.
            <br />
            请打印付款申请单，将原始发票附在申请单后面，提交财务。财务会审核付款申请，若没有问题，财务会安排付款。<br />
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
</asp:Content>
