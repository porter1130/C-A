<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadExcelBill.aspx.cs" 
MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.CreditCardClaim.UploadExcelBill" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="UploadExcel.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Upload 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Upload Excel Bill
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
            <div class="ca-workflow-form-buttons noPrint">
                <QFC:StartWorkflowButton ID="StartWorkflowButton1" WorkflowName="Credit Card Bill Workflow"
                    runat="server" Text="Start" CssClass="hidden"/>             
                <input type="button" value="Back" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
            </div>
            <SharePoint:FormDigest ID="FormDigest1" runat="server">
            </SharePoint:FormDigest>
        </QFL:ListFormControl>    
    <uc1:DataForm ID="DataForm1" runat="server" />
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>