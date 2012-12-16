<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true"
    CodeBehind="POReport.aspx.cs" Inherits="CA.WorkFlow.UI._Layouts.CA.WorkFlows.PurchaseOrder.POReport" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
      PO Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript">
        function popexcel(url) {
            var w = window.open(url, '_blank');
            w.location.href = url;
        }
    </script>
    <style type="text/css">
        .ReaportWhiteColor
        {
            background-color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Nontrade List Report 
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
    <table cellpadding="5" cellspacing="5">
        <tr>
            <td>
                Created Date:
            </td>
            <td>
                <cc1:cadatetimecontrol id="CADateTimeFrom" runat="server" dateonly="true" cssclasstextbox="HotelInfomation DateTimeControl" />
            </td>
            <td>
                <cc1:cadatetimecontrol id="CADateTimeTo" runat="server" dateonly="true" cssclasstextbox="HotelInfomation DateTimeControl" />
            </td>
            <td>
                <asp:DropDownList ID="DDLCostCenter" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnQuery" runat="server" Text="Query" OnClick="btnQuery_Click" />
            </td>
            <td>
                <asp:Button ID="btnReportPRPO" runat="server" Text="Export Report" OnClick="btnReportPRPO_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="5">
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="uplCustomer" runat="server" UpdateMode="Conditional">
        <contenttemplate>
        <SharePoint:SPGridView ID="SPGridView1" runat="server" AutoGenerateColumns="False"
            AllowPaging="True" PageSize="100" OnPageIndexChanging="SPGridView1_PageIndexChanging"
            BorderColor="#9dabb6" BorderStyle="Solid" BorderWidth="1px" CssClass="ms-listviewtable inner-table"
            EnableTheming="False" GridLines="Horizontal">
            <AlternatingRowStyle CssClass="each-row ms-alternating" />
            <RowStyle CssClass="each-row ReaportWhiteColor" />
            <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
        </SharePoint:SPGridView>
        <div class="align-center">
            <SharePoint:SPGridViewPager ID="SPGridViewPager1" runat="server" GridViewId="SPGridView1">
            </SharePoint:SPGridViewPager>
        </div>
    </contenttemplate>
        <triggers>
        <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickNext" />
        <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickPrevious" />
    </triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
