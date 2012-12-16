<%@ Assembly Name="CA.WorkFlow.Common.UnlockWorkflow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fd3b9f063d89dee2" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UnlockWorkflowWebPartUserControl.ascx.cs"
    Inherits="CA.WorkFlow.Common.UnlockWorkflow.UI.UnlockWorkflowWebPartUserControl" %>
<div class="div_table">
    <div class="div_tr">
        <div class="div_td">
            <span>Please Select Workflow Name: </span>
        </div>
        <div class="div_td">
            <asp:DropDownList ID="ddlWFName" runat="server">
            </asp:DropDownList>
        </div>
    </div>
    <div class="div_tr">
        <div class="div_td">
            <span>Please Input Unlock Workflow Number:</span>
        </div>
        <div class="div_td">
            <span>
                <asp:TextBox ID="txtWFNumber" runat="server" /></span>
        </div>
    </div>
    <div class="div_tr">
        <div class="div_td">
            <asp:Button ID="btnUnlock" runat="server" OnClick="btnUnlock_Click" Text="Unlock" />
        </div>
    </div>
</div>
