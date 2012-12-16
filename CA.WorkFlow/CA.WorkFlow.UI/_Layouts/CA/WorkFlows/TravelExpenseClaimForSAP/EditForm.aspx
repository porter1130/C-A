<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" EnableEventValidation="false"
    MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.TravelExpenseClaimForSAP.EditForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Travel Expense Claim Form For SAP
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
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Travel Expense Claim Form For SAP
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
            <div class="ca-workflow-form-buttons noPrint">
                <%--<asp:Button ID="btnTRNumber" Text="TRNumber" runat="server" OnClick="btnTRNumber_Click" CausesValidation="false" />--%>
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
    
</asp:Content>