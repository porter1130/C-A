<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayForm.aspx.cs" MasterPageFile="~/_Layouts/CA/Layout.Master"
    Inherits="CA.WorkFlow.UI.NonTradeSupplierSetupMaintenance2.DisplayForm" %>

<%@ Register Src="DataView.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Non Trade Supplier Setup & Maintenance
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Non Trade Supplier Setup & Maintenance
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <QFL:ListFormControl ID="ListFormControl3" runat="server" FormMode="New">
        <div class="ca-workflow-form-buttons noPrint">
            <input type="button" value="Back" onclick="window.history.go(-1);" />
        </div>
        <uc1:DataForm ID="DataForm1" runat="server" />
        <uc2:TaskTrace ID="TaskTrace1" runat="server" />
        <SharePoint:FormDigest ID="FormDigest3" runat="server">
        </SharePoint:FormDigest>
    </QFL:ListFormControl>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
<div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
        <div class="middle">
            1. Please print out the application form, attach officially chopped copies of Business
            License, Tax Registration License, and Bank information from vendors, and pass them
            to Finance for checking and record.
            <br />
            请打印申请表，附上供应商盖上公章的企业法人营业执照，税务登记证和银行信息，提交财务部进行核查及记录。
            <br />
            2. Payment term less than 30 days required CFO’s special approval.
            <br />
            若付款期限少于30天，需要CFO的特别批准。
            <br />
            3. For change of vendor information, please fill-in the name of the vendor and information
            that required to be changed (no need to fill-in the whole form again). Please print
            out the application form; attach officially chopped request from vendor, and pass
            them to Finance for checking and record.
            <br />
            若要更改供应商的信息，请填写供应商的名称及需要更改的资料 （不用重新填写整份表格）。请打印申请表，附上供应商盖好公章的更改信息，提交财务部进行核查及记录。
            <br />
            4. If block/release vendor, please provide reason in the box above。
            <br />
            若需要冻结/解除冻结供应商，请把原因填写在申请表上。
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
</asp:Content>
