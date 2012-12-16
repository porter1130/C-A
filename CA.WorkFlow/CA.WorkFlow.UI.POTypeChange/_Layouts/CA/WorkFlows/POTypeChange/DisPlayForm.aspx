﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="DisPlayForm.aspx.cs" Inherits="CA.WorkFlow.UI.POTypeChange.DisPlayForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register src="DataView.ascx" tagname="DataView" tagprefix="uc2" %>
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
    <qfl:listformcontrol id="ListFormControl1" runat="server" formmode="Display">
<div id="ca-pr-form">
        <div class="ca-workflow-form-buttons">
            <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
        </div>
</div>
     <uc2:DataView ID="DataView1" runat="server" />
     <div class="trace-list-grid full-width">
         <cc1:TaskTraceList ID="Trace1" runat="server" BorderColor="#999999" BorderWidth="1px" GridLines="Horizontal" />
     </div>

</qfl:listformcontrol>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>