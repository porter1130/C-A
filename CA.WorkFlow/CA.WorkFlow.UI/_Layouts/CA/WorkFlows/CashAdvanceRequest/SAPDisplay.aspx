﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true"
    CodeBehind="SAPDisplay.aspx.cs" Inherits="CA.WorkFlow.UI.CashAdvanceRequest.SAPDisplay"
    EnableEventValidation="false" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="DataListView.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>
    <%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Cash Advance Request SAP Form
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Cash Advance Request SAP Form
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
            <div class="ca-workflow-form-buttons noPrint">
                <div style="display: none">
                    <asp:Button ID="btnCashAdvanceRelateToSAP" runat="server" Text="Cash Advance Relate To SAP"
                        OnClientClick="return CashAdvanceToSAPForm()" Width="170px" /></div>
                <input type="button" value="Back" onclick="window.history.go(-1)" />
            </div>
            <uc1:DataForm ID="DataForm1" runat="server">
            </uc1:DataForm>
             <uc2:TaskTrace id="TaskTrace1" runat="server" />
            <SharePoint:FormDigest ID="FormDigest3" runat="server">
            </SharePoint:FormDigest>
        </QFL:ListFormControl>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
   
    <script type="text/javascript">
        function CashAdvanceToSAPForm() {
            window.open('CashAdvanceRelateToSAP.aspx', "_blank", "", "");
            //window.location = 'CashAdvanceRelateToSAP.aspx';
            return false;
        }
    </script>
</asp:Content>
