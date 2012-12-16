<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" Inherits="CA.WorkFlow.UI.POTypeChange.NewForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %> 

<%@ Register src="DataEdit.ascx" tagname="DataEdit" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
   PO Type Change
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
   PO Type Change
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<QFL:listformcontrol id="ListFormControl1" runat="server" formmode="New">
<asp:HiddenField ID="HiddenFieldIsNotNeedApprove" Value="0" runat="server" />
<div id="ca-pr-form">
        <div class="ca-workflow-form-buttons">
            <QFC:StartWorkflowButton ID="StartWorkflowButtonSubmit" WorkflowName="POTypeChangeWorkflow" runat="server" Text="Submit" CausesValidation="false"/>
            <QFC:StartWorkflowButton ID="StartWorkflowButtonSave" WorkflowName="POTypeChangeWorkflow" runat="server" Text="Save" CausesValidation="false"/>
            <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
        </div>
        <uc2:DataEdit ID="DataEdit1" runat="server" />
    <SharePoint:FormDigest ID="FormDigest1" runat="server"></SharePoint:FormDigest>
</div>
</QFL:listformcontrol>
<asp:HiddenField ID="HiddenFieldID" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
