<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="PaperBagForCostCenter.aspx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequest.PaperBagForCostCenter" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"  Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Paper Bag Report-Items Cost
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
    Paper Bag Report-Items Cost
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<table>
    <tr>
       <td><cc1:CADateTimeControl ID="CADateTimeFrom" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></td>
       <td><cc1:CADateTimeControl ID="CADateTimeTo" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></td>
       <td><asp:DropDownList ID="DDLCostCenter" runat="server"></asp:DropDownList></td>
       <td>BaseOn:</td>
       <td>
                <asp:DropDownList ID="DDLBaseOn" runat="server">
                    <asp:ListItem Value="0">PR</asp:ListItem>
                    <asp:ListItem Value="1">PO</asp:ListItem>
                </asp:DropDownList>
       </td>
       <td><div class="ca-workflow-form-buttons"><asp:Button ID="ButtonQuery" Text="Query" runat="server" onclick="ButtonQuery_Click" /></div></td>
       <td><div class="ca-workflow-form-buttons"><asp:Button ID="ButtonExport" Text="Export" runat="server" onclick="ButtonExport_Click"/></div></td>
       <td><div class="ca-workflow-form-buttons"><asp:Button ID="ButtonViewLog" 
               Text="View Logs" runat="server" onclick="ButtonViewLog_Click" /></div></td>
    </tr>
</table>
<table style="background-color:White">
    <tr>
        <td>
            <SharePoint:SPGridView ID="SPGridViewPaperbage" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="100" BorderColor="#9dabb6" BorderStyle="Solid" BorderWidth="1px" CssClass="ms-listviewtable inner-table" EnableTheming="False" GridLines="Horizontal">
            <AlternatingRowStyle CssClass="each-row ms-alternating" />
            <RowStyle CssClass="each-row ReaportWhiteColor" />
            <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
            </SharePoint:SPGridView>
        </td>
    </tr>
</table>
    <asp:Literal ID="LiteralLog" runat="server"></asp:Literal> 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
