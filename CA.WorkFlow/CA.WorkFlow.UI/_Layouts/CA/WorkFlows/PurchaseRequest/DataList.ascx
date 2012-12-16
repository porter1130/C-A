<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataList.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequest.DataList" %>

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
</style>

<div class="ca-workflow-form-buttons">
    <input type="button" onclick="return dispatchAction(this)" value="Create EWF PO" />
    <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx'" />
</div>

<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4" class="label align-center">
            <h3>
                Pending Purchase Request
            </h3>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="inner-table">
                        <tr>
                            <td class="cell align-center w5">
                                &nbsp;
                            </td>
                            <th class="label align-center w10">
                                PR No
                            </th>
                            <th class="label align-center w20">
                                Created
                            </th>
                            <th class="label align-center w15">
                                Request Type
                            </th>
                            <th class="label align-center w15">
                                Returned
                            </th>
                            <th class="label align-center w20">
                                Applicant
                            </th>
                            <th class="label align-center w20">
                                Request Date
                            </th>
                            <th class="label align-center w100">
                                Type
                            </th>
                        </tr>
                        <asp:Repeater ID="rptItem" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="cell align-center">
                                        <asp:CheckBox ID="cbSelect" runat="server" prid='<%# Eval("ID")%>' prnum='<%# Eval("Title")%>' requesttype='<%# Eval("RequestType")%>' isreturn='<%# Eval("IsReturn")%>' />
                                    </td>
                                    <td class="label align-center">
                                        <%# Eval("Title")%>
                                    </td>
                                    <td class="label align-center">
                                        <%# Eval("Created")%>
                                    </td>
                                    <td class="label align-center">
                                        <%# Eval("CapexType")%>
                                    </td>
                                    <td class="label align-center IsReturn">
                                        <%# Eval("IsReturn")%>
                                    </td>
                                    <td class="label align-center">
                                        <%# Eval("Applicant")%>
                                    </td>
                                    <td class="label align-center">
                                        <%# Eval("Created","{0:d}")%>
                                    </td>
                                    <td class="label align-center">
                                        <%# Eval("PRStorePurpose")%>
                                    </td>
                                    
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>                        
                    </table>
                    <asp:HiddenField ID="hidCreatedPONumber" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnCreatePO" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>    
</table>
<asp:HiddenField ID="hidSelectNums" runat="server" />
<asp:HiddenField ID="hidSelectIds" runat="server" />
<asp:Button runat="server" ID="btnCreatePO" OnClick="btnCreatePO_Click"
    Text="Query" CausesValidation="False" CssClass="hidden" />

<script type="text/javascript">
    var isReturn;
    $(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        $('#<%= this.UpdatePanel1.ClientID %> td.IsReturn').each(function () {
            isReturn = jQuery.trim($(this).text());
            if (isReturn === '0') {
                $(this).text('No');
            } else {
                $(this).text('Yes');
            }
        });
    });

    function EndRequestHandler() {
        var $createdPONums = $('#<%= this.hidCreatedPONumber.ClientID %>');
        if ($createdPONums.val().length > 0) {
            alert('The new created PO: ' + $createdPONums.val());
            $createdPONums.val('');
        }
        ClearForbidDIV();
    }

    function dispatchAction(sender) {
        $('#<%= this.hidSelectNums.ClientID %>').val('');
        $('#<%= this.hidSelectIds.ClientID %>').val('');
        var prnums = '';
        var ids = '';
        var diff1 = 0;
        var diff2 = 0;
        $('#<%= this.UpdatePanel1.ClientID %> input').each(function () {
            if ($(this).is(':checked')) {
                $parentSpan = $(this).parent('span');
                prnums += $parentSpan.attr('prnum') + ';';
                ids += $parentSpan.attr('prid') + ';';
                if ($parentSpan.attr('isreturn') === '1') {
                    diff1++;
                } else {
                    diff2++;
                }
            }
        });
        if (diff1 > 0 && diff2 > 0) {
            alert('You can not select multilines which contain return and normal type.');
            return false;
        }

        prnums = prnums.substring(0, prnums.length - 1);
        ids = ids.substring(0, ids.length - 1);
        $('#<%= this.hidSelectNums.ClientID %>').val(prnums);
        $('#<%= this.hidSelectIds.ClientID %>').val(ids);

        if ($('#<%= this.hidSelectNums.ClientID %>').val().length === 0 || $('#<%= this.hidSelectIds.ClientID %>').val().length === 0) {
            alert('Please select purchase request first.');
            return false;
        }
        CreateForbidDIV();
        $('#<%= this.btnCreatePO.ClientID %>').click();
    }
</script>
