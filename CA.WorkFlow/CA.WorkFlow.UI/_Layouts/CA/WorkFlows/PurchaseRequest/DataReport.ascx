<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataReport.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequest.DataReport" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
    <script type="text/javascript">
        function popexcel(url) {
            var w = window.open(url, '_blank');
            w.location.href = url;
        }
</script>
<style type="text/css">
    .ReaportWhiteColor
    {
        background-color:White;
        }
</style>

<table cellpadding="5" cellspacing="5">
    <tr>
        <td>
                <cc1:CADateTimeControl ID="CADateTimeFrom" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" />
        </td>
        <td>
                <cc1:CADateTimeControl ID="CADateTimeTo" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" />
        </td>
        <td>
            <asp:DropDownList ID="DDLCostCenter" Visible="false" runat="server"></asp:DropDownList>
        </td>
        <td>
            <asp:Button ID="btnQuery" runat="server" Text="Query" OnClick="btnQuery_Click" />
        </td>
        <td>
            <asp:Button ID="btnReportPRPO" runat="server" Text="Export Report" OnClick="btnReportPRPO_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="4">
            
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="uplCustomer" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
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
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickNext" />
        <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickPrevious" />
    </Triggers>
</asp:UpdatePanel>
