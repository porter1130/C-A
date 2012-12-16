﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="OSPReport.aspx.cs" Inherits="CA.WorkFlow.UI.OSP.OSPReport" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
OSPReport
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<script type="text/javascript">
    function popexcel(url) {
        var w = window.open(url, '_blank');
        w.location.href = url;
    }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
OSPReport
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">

<table cellpadding="5" cellspacing="5">
    <tr>
        <td>
                <cc1:CADateTimeControl ID="CADateTimeFrom" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" />
        </td>
        <td>
                <cc1:CADateTimeControl ID="CADateTimeTo" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" />
        </td>
        <td>
            <div class="ca-workflow-form-buttons"><asp:Button ID="btnQuery" runat="server" Text="Query" OnClick="btnQuery_Click" /></div>
        </td>
        <td>
            <div class="ca-workflow-form-buttons"><asp:Button ID="ButtonExport" runat="server" 
                    Text="Export" onclick="ButtonExport_Click"/></div>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            
        </td>
    </tr>
</table>
<br />
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplCustomer" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <SharePoint:SPGridView ID="SPGridViewOsp" runat="server" AutoGenerateColumns="False"
            AllowPaging="True" PageSize="100" OnPageIndexChanging="SPGridViewOsp_PageIndexChanging"
            BorderColor="#9dabb6" BorderStyle="Solid" BackColor="White" BorderWidth="1px" CssClass="ms-listviewtable inner-table"
            EnableTheming="False" GridLines="Horizontal">
            <AlternatingRowStyle CssClass="each-row ms-alternating" />
            <RowStyle CssClass="each-row ReaportWhiteColor" />
            <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
            <Columns>
                <asp:BoundField DataField="WorkflowNumber" HeaderText="Workflow No." />
                <asp:BoundField DataField="CreatedBy" HeaderText="Created by" />
                <asp:BoundField DataField="Dep" HeaderText="Dep" />
                <asp:BoundField DataField="Created" HeaderText="Creation date" />
                <asp:BoundField DataField="Modified" HeaderText="Completion date" />
                <asp:BoundField DataField="Title" HeaderText="Style No." />
                <asp:BoundField DataField="SubDiv" HeaderText="Sub Div" />
                <asp:BoundField DataField="Class" HeaderText="Class" />
                <asp:BoundField DataField="PAD" HeaderText="PAD" />
                <asp:BoundField DataField="SAD" HeaderText="SAD" />
                <asp:BoundField DataField="OriginalOsp" HeaderText="Original OSP" />
                <asp:BoundField DataField="CurrentOMU" HeaderText="Original OMU" />
                <asp:BoundField DataField="NewOSP" HeaderText="New OSP" />
                <asp:BoundField DataField="NewOMU" HeaderText="New OMU" />
                <asp:BoundField DataField="PONO" HeaderText="PO No." />
                <asp:BoundField DataField="Qty" HeaderText="PO Qty" />
                <asp:BoundField DataField="OMUReduction" HeaderText="OMU Reduction" />
            </Columns>
        </SharePoint:SPGridView>
        <div class="align-center">
            <SharePoint:SPGridViewPager ID="SPGridViewPager1" runat="server" GridViewId="SPGridViewOsp">
            </SharePoint:SPGridViewPager>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickNext" />
        <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickPrevious" />
    </Triggers>
</asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
