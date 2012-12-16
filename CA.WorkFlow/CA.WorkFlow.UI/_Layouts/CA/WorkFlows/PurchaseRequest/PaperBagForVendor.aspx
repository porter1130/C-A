<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="PaperBagForVendor.aspx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequest.PaperBagForVendor" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"  Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Paper Bag Report-Items Summary
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
    Paper Bag Report-Items Summary
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
    <table style="background-color:White;padding:10px;margin:10px;">
        <tr>
            <td ><cc1:CADateTimeControl ID="CADateTimeFrom" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></td>
            <td><cc1:CADateTimeControl ID="CADateTimeTo" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></td>
            <td><asp:DropDownList ID="DDLCostCenter" runat="server"></asp:DropDownList></td>
            <td>BaseOn:</td>
            <td>
               <asp:DropDownList ID="DDLBaseOn" runat="server">
                    <asp:ListItem Value="0">PR</asp:ListItem>
                    <asp:ListItem Value="1">PO</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <div class="ca-workflow-form-buttons">
                    <asp:Button ID="ButtonQuery" Text="Query" runat="server" onclick="ButtonQuery_Click" />
                </div>
            </td>
            <td>
            <div class="ca-workflow-form-buttons">
                <asp:Button ID="ButtonExportExcel" Text="Export" runat="server" 
                    onclick="ButtonExportExcel_Click" />
            </div>
            </td>
            <td>
            <div class="ca-workflow-form-buttons">
                <asp:Button ID="ButtonLog" Text="View Log" runat="server" 
                    onclick="ButtonLog_Click" />
            </div>
            </td>
        </tr>
        </table>
    <table style="background-color:White">
        <tr>
            <td colspan="5" >
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel ID="uplCustomer" runat="server" UpdateMode="Conditional">
                          <ContentTemplate>
                                <SharePoint:SPGridView ID="SPGridViewPaperPabage" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="100" OnPageIndexChanging="SPGridViewPaperPabage_PageIndexChanging"
                                 BorderColor="#9dabb6" BorderStyle="Solid" BorderWidth="1px" CssClass="ms-listviewtable inner-table"
                                 EnableTheming="False" GridLines="Horizontal">
                                <AlternatingRowStyle CssClass="each-row ms-alternating" />
                                <RowStyle CssClass="each-row ReaportWhiteColor" />
                                <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
                                <Columns>
                                    <asp:BoundField DataField="ItemCode" HeaderText="编号Item Code" />
                                    <asp:BoundField DataField="Description" HeaderText="品名 Pager Bag List" />
                                    <asp:BoundField DataField="TotalQuantity" HeaderText="预计送货数量 Qty .(pc)" />
                                    <asp:BoundField DataField="PackagedUnite" HeaderText="单位 Unit" />
                                    <asp:BoundField DataField="PackagedRegulation" HeaderText="包装规则 PackagedRegulation" />
                                    <asp:BoundField DataField="Carton" HeaderText="预计送货箱量Qty. (carton)" />
                                    <asp:BoundField DataField="Unit" HeaderText="单位 Unit" />
                                    <asp:BoundField DataField="CostCenterName" HeaderText="到货门店 Store" />
                                    <asp:BoundField DataField="CostCenter" HeaderText="成本中心 Cost Center" />
                                </Columns>
                              </SharePoint:SPGridView>
                              <div class="align-center">
                        <SharePoint:SPGridViewPager ID="SPGridViewPagerPaper" runat="server" GridViewId="SPGridViewPaperPabage">
                        </SharePoint:SPGridViewPager>
                    </div>
                         </ContentTemplate>
                          <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="SPGridViewPagerPaper" EventName="ClickNext" />
                    <asp:AsyncPostBackTrigger ControlID="SPGridViewPagerPaper" EventName="ClickPrevious" />
                </Triggers>
            </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="5" style="height:50px;">
                
            </td>
        </tr>
        <tr>
            <td colspan="5">
                门店送货汇总
            </td>
        </tr>
        <tr>
            <td colspan="5">
    <SharePoint:SPGridView ID="SPGridViewItemCode" runat="server" AutoGenerateColumns="False"
        BorderColor="#9dabb6" BorderStyle="Solid" BorderWidth="1px" CssClass="ms-listviewtable inner-table"
        EnableTheming="False" GridLines="Horizontal">
        <AlternatingRowStyle CssClass="each-row ms-alternating" />
        <RowStyle CssClass="each-row ReaportWhiteColor" HorizontalAlign="Center" />
        <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
        <Columns>
            <asp:BoundField DataField="ItemCode" HeaderText="编号Item Code" />
            <asp:BoundField DataField="Description" HeaderText="品名 Pager Bag List" />
            <asp:BoundField DataField="TotalQuantity" HeaderText="预计送货数量 Qty .(pc)" />
            <asp:BoundField DataField="PackagedUnite" HeaderText="单位 Unit" />
            <asp:BoundField DataField="PackagedRegulation" HeaderText="包装规则 PackagedRegulation" />
            <asp:BoundField DataField="Carton" HeaderText="预计门店发货箱量 Qty. (carton)" />
            <asp:BoundField DataField="Unit" HeaderText="单位 Unit" />
        </Columns>
    </SharePoint:SPGridView>

            </td>
        </tr>
    </table>
    <asp:Literal ID="LiteralLog" runat="server"></asp:Literal> 
</asp:Content>
