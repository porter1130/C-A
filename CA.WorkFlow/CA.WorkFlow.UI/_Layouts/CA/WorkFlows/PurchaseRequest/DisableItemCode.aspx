<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="DisableItemCode.aspx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequest.DisableItemCode" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
  Enable/Disable Item Code
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
  Enable/Disable Item Code
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
        <table class="ca-workflow-form-table" cellpadding="10">
            <tr>
                <td class="label" style="width: 20%" rowspan="2">
                    ItemCode<br /><br />
                    Please fill Item codes in rigth area and split with ",".
                </td>
                <td class="label align-center" style="width: 60%" rowspan="2">
                    <asp:TextBox ID="TextBoxItemCodes" TextMode="MultiLine" class="PaymnetOnceCommets" Rows="8" Columns="50" runat="server"></asp:TextBox>
                </td>
                <td class="label align-center" style="width: 20%">
                     <div class="ca-workflow-form-buttons"><asp:Button ID="ButtonDisable" Text="Disable" 
                             runat="server" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1" 
                             onclick="ButtonDisable_Click" /></div>
                </td>
            </tr>
            <tr>
                <td class="label align-center" style="width: 20%">
                <div class="ca-workflow-form-buttons"><asp:Button ID="ButtonEnable" Text="Enable" 
                        runat="server" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1" 
                        onclick="Button2_Click" /></div>
                </td>
            </tr>
            <tr>
                <td class="label" style="width: 20%">
                </td>
                <td class="label align-center">
                <div class="ca-workflow-form-buttons"><input id="GoBack" style="width:100px;border:1px solid #999999" type="button" value="Back" onclick="location.href('/WorkFlowCenter/Lists/PurchaseRequestWorkflow/MyApply.aspx')" /></div>
                     <%--<asp:Button ID="Button1" Text="Back" runat="server" Width="100" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1" /></div>--%>
                </td>
                <td class="label">
                    
                </td>
            </tr>
        </table>
    
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
