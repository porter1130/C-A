<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataPRPOQuery.ascx.cs"
    Inherits="CA.WorkFlow.UI.PurchaseRequest.DataPRPOQuery" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<style type="text/css">
    .ca-workflow-form-buttons input
    {
        border: 1px solid #9DABB6;
        cursor: pointer;
        height: 24px;
        margin: 5px 0 10px 5px;
        padding: 0 2px;
        text-align: center;
        width: 120px;
        color: #000;
    }
    .ca-workflow-form-table td
    {
        padding: 5px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        text-align: center;
        margin: 0;
    }
    .DIVPadding
    {
            height:20px;
            text-align:right;
            padding-right:20px;
        }
</style>
<%--<div>
    <ul>
        <li>Purchase Request Number:</li>
        <li>
            <asp:TextBox ID="txtPRNumber" runat="server" /></li>
        <li>Purchase Order Number:</li>
        <li>
            <asp:TextBox ID="txtPONumber" runat="server" /></li>
        <li><asp:Button ID="btnQuery" Text="Query" OnClick="btnQuery_Click" runat="server" /></li>
    </ul>
</div>--%>
<table cellpadding="10">
    <tr>
        <td>Purchase Request Number:<asp:TextBox ID="txtPRNumber" runat="server" /></td>
        <td id="PO" runat="server">Purchase Order Number:<asp:TextBox ID="txtPONumber" runat="server" /></td>
        <td><div class="ca-workflow-form-buttons"><asp:Button ID="btnQuery" Text="Query" OnClick="btnQuery_Click" runat="server" /></div></td>
    </tr>
</table>
<fieldset>
    <legend>Purchase Request Items </legend>
    <div class="DIVPadding">Total Price: <asp:Label ID="LabelPRPrice" runat="server"></asp:Label></div>
         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <SharePoint:SPGridView ID="SPGridView1" runat="server" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="100" OnPageIndexChanging="SPGridView1_PageIndexChanging"
                        BorderColor="#9dabb6" BorderStyle="Solid" BorderWidth="1px" CssClass="ms-listviewtable inner-table"
                        EnableTheming="False" GridLines="Horizontal">
                        <AlternatingRowStyle CssClass="each-row ms-alternating" />
                        <RowStyle CssClass="each-row" />
                        <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
                        <Columns>
                            <asp:BoundField DataField="Title" HeaderText="Title" />
                            <asp:BoundField DataField="PONumber" HeaderText="PONumber" />
                            <asp:BoundField DataField="VendorName" HeaderText="VendorName" />
                            <asp:BoundField DataField="TotalPrice" HeaderText="TotalPrice" />
                        </Columns>
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
    
    <div class="DIVPadding"></div>
</fieldset>
    <div class="DIVPadding"></div>
    <div class="DIVPadding"></div>
<fieldset>
    <legend>Purchase Order Items </legend>
    <div class="DIVPadding">Total Price:<asp:Label ID="LabelPOPrice" runat="server"></asp:Label> </div>
         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <SharePoint:SPGridView ID="SPGridView2" runat="server" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="100" OnPageIndexChanging="SPGridView2_PageIndexChanging"
                        BorderColor="#9dabb6" BorderStyle="Solid" BorderWidth="1px" CssClass="ms-listviewtable inner-table"
                        EnableTheming="False" GridLines="Horizontal">
                        <AlternatingRowStyle CssClass="each-row ms-alternating" />
                        <RowStyle CssClass="each-row" />
                        <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
                        <Columns>
                            <asp:BoundField DataField="Title" HeaderText="Title" />
                            <asp:BoundField DataField="PONumber" HeaderText="PONumber" />
                            <asp:BoundField DataField="VendorName" HeaderText="VendorName" />
                            <asp:BoundField DataField="TotalPrice" HeaderText="TotalPrice" />
                        </Columns>
                    </SharePoint:SPGridView>
                    <div class="align-center">
                        <SharePoint:SPGridViewPager ID="SPGridViewPager2" runat="server" GridViewId="SPGridView1">
                        </SharePoint:SPGridViewPager>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="SPGridViewPager2" EventName="ClickNext" />
                    <asp:AsyncPostBackTrigger ControlID="SPGridViewPager2" EventName="ClickPrevious" />
                </Triggers>
            </asp:UpdatePanel>
    
    <div class="DIVPadding"></div>
</fieldset>
