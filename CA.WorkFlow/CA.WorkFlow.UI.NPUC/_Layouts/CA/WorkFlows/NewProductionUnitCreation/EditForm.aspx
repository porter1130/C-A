<%@ Page Title="New Production Unit Creation" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master"
    AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" Inherits="CA.WorkFlow.UI.NPUC.EditForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    New Production Unit Creation Form
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    New Production Unit Creation Form
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="ca-supplier-form">
        <div class="ContentDiv">
            <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
            <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
                <div class="ca-workflow-form-buttons noPrint">
                    <cc1:CAActionsButton ID="Actions" runat="server" OnClientClick="return CheckSubmit()" />
                    <asp:Button runat="server" ID="btnSave" Text="Save" OnClientClick="return CheckSubmit()" />
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
            please write down the reason of add this new PU. Will you delete any existing new
            PU ?<br />
            please provide supplier profile & audit assessment form with completed information
            .
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
</asp:Content>
