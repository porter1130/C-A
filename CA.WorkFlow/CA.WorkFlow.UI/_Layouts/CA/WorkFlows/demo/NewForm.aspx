<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" EnableEventValidation="false"
    MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.demo.NewForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Demo
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <qfl:listformcontrol id="ListFormControl1" runat="server" formmode="New">
            <div class="ca-workflow-form-buttons noPrint">
                <QFC:StartWorkflowButton ID="StartWorkflowButton1" WorkflowName="Demo Workflow" runat="server"
                    Text="Submit" />
                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
            </div>
            <SharePoint:FormDigest ID="FormDigest1" runat="server">
            </SharePoint:FormDigest>
            <uc2:TaskTrace id="TaskTrace1" runat="server" />
        </qfl:listformcontrol>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
