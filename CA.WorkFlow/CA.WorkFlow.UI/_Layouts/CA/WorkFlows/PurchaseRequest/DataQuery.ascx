<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataQuery.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequest.DataQuery" %>

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
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4" style="text-align: left; border-bottom: none;">
            Please input purchase order workflow number:
        </td>
    </tr>
    <tr>
        <td style="width: 100px; border-right: none;">
            <asp:TextBox ID="txtPONumber" runat="server" />
        </td>
        <td class="ca-workflow-form-buttons" style="width: 80px; border-right: none;">
            <input type="button" onclick="return dispatchAction(this)" value="Query" />
        </td>
        <td style="border-right: none;">
        </td>
        <td style="border-right: none;">
        </td>
    </tr>
</table>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table">
            <tr>
                <td colspan="9">
                    Purchase Order Info
                </td>
            </tr>
            <tr>
                <td class="w5">
                    No
                </td>
                <td class="w10">
                    Item Code
                </td>
                <td class="w15">
                    PO Number
                </td>
                <td class="w15">
                    CostCenter
                </td>
                <td class="w15">
                    Request Quantity
                </td>
                <td class="w15">
                    Trans Quantity
                </td>
                <td class="w15">
                    Total Quantity
                </td>
                <td class="w20">
                    Unit Price
                </td>
                <td class="w20">
                    Total Price
                </td>
            </tr>
            <asp:Repeater ID="rptPOItem" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Container.ItemIndex + 1 %>
                        </td>
                        <td>
                            <%# Eval("ItemCode")%>
                        </td>
                        <td>
                            <a href='<%# "/WorkFlowCenter/_Layouts/CA/WorkFlows/PurchaseRequest/PORedirct.aspx?PONO="+Eval("Title")%>'><%# Eval("Title")%></a>
                        </td>
                        <td>
                            <%# Eval("CostCenter")%>
                        </td>
                        <td>
                            <%# Eval("RequestQuantity")%>
                        </td>
                        <td>
                            <%# Eval("TransQuantity")%>
                        </td>
                        <td>
                            <%# Eval("TotalQuantity")%>
                        </td>
                        <td>
                            <%# Eval("UnitPrice")%>
                        </td>
                        <td>
                            <%# Eval("TotalPrice")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td class="label align-center">Total</td>
                <td colspan="8" style="text-align: right;"><asp:Label ID="lbPOTotal" runat="server" /></td>
            </tr>
        </table>

        <table class="ca-workflow-form-table">
            <tr>
                <td colspan="9">
                    Purchase Request Info
                </td>
            </tr>
            <tr>
                <td class="w5">
                    No
                </td>
                <td class="w10">
                    Item Code
                </td>
                <td class="w15">
                    PR Number
                </td>
                <td class="w15">
                    CostCenter
                </td>
                <td class="w15">
                    Request Quantity
                </td>
                <td class="w15">
                    Trans Quantity
                </td>
                <td class="w15">
                    Total Quantity
                </td>
                <td class="w20">
                    Unit Price
                </td>
                <td class="w20">
                    Total Price
                </td>
            </tr>
            <asp:Repeater ID="rptPRItem" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Container.ItemIndex + 1 %>
                        </td>
                        <td>
                            <%# Eval("ItemCode")%>
                        </td>
                        <td>
                            <a href='#' onclick="return OpenWindow(this, 'pr', 'PurchaseRequest');" prnum='<%# Eval("Title")%>' target="_blank">
                            <%# Eval("Title")%>
                            </a>
                        </td>
                        <td>
                            <%# Eval("CostCenter")%>
                        </td>
                        <td>
                            <%# Eval("RequestQuantity")%>
                        </td>
                        <td>
                            <%# Eval("TransQuantity")%>
                        </td>
                        <td>
                            <%# Eval("TotalQuantity")%>
                        </td>
                        <td>
                            <%# Eval("UnitPrice")%>
                        </td>
                        <td>
                            <%# Eval("TotalPrice")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td>Total</td>
                <td colspan="8" style="text-align: right;"><asp:Label ID="lbPRTotal" runat="server" /></td>
            </tr>
        </table>
        <asp:HiddenField ID="hidNewWindowLink" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnOpenDetail" EventName="Click" />
    </Triggers>                
</asp:UpdatePanel>


<asp:HiddenField ID="hidPONum" runat="server" />
<asp:HiddenField ID="hidDetail" runat="server" />
<asp:Button runat="server" ID="btnQuery" OnClick="btnQuery_Click"
    Text="Query" CausesValidation="False" CssClass="hidden" />
<asp:Button runat="server" ID="btnOpenDetail" OnClick="btnOpenDetail_Click"
    Text="Query" CausesValidation="False" CssClass="hidden" OnClientClick="this.Form.Target='_blank'" />

<script type="text/javascript">
    $(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });

    function EndRequestHandler() {
        var newWinLink = $('#<%= this.hidNewWindowLink.ClientID %>').val();
        $('#<%= this.hidNewWindowLink.ClientID %>').val('');

        if (!ca.util.emptyString(newWinLink)) {
            window.open(newWinLink);
        }
    }

    function dispatchAction(sender) {
        var poNumber = $('#<%= this.txtPONumber.ClientID %>').val();
        if (ca.util.emptyString(poNumber)) {
            alert('Please input PO number first.');
            return false;
        }
        $('#<%= this.hidPONum.ClientID %>').val(jQuery.trim(poNumber));
        $('#<%= this.btnQuery.ClientID %>').click();
        return false;
    }

    function OpenWindow(sender, module, folder) {
        var detail = '';
        switch (module) {
            case 'pr':
                detail = 'Purchase Request Workflow' + ';' + $(sender).attr('prnum') + ';' + 'PurchaseRequest';
                break;
            case 'po':
                detail = 'Purchase Order Workflow' + ';' + $(sender).attr('ponum') + ';' + 'PurchaseOrder';
                break;
            default:
                break;
        }
        $('#<%= this.hidDetail.ClientID %>').val(detail);
        $('#<%= this.btnOpenDetail.ClientID %>').click();
        return false;
    }
</script>
