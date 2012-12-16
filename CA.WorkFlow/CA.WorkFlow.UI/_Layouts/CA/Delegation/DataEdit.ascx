<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.Delegation.DataEdit" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="ca" %>
<div class="ca-workflow-form-buttons">
    <asp:Button ID="btnSave" runat="server" Text="OK" OnClick="btnSave_Click" CausesValidation="true"
        OnClientClick="return CheckSubmit()" />
    <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
</div>
<style type="text/css">
    .ca-workflow-form-table input, .ca-workflow-form-table select, .ca-workflow-form-table textarea
    {
        width: 30px;
        margin-left: 3px;
    }
    
    .ca-workflow-form-table input
    {
        padding: 4px 2px 0;
    }
    .ck
    {
        padding-left: 25px;
        padding-top: 5px;
        margin-top: 0px;
    }
    .ck input
    {
        border: none;
    }
    .ck1
    {
        padding-left: 10px;
        font-weight: bold;
        padding-top: 0px;
        margin-top: 0px;
    }
    #capf div
    {
        padding-top: 4px;
    }
    #capf img
    {
        margin-top: 2px;
    }
    #capf input
    {
        height: 18px;
    }
    .pf1
    {
        height: 23px;
    }
    .pf2
    {
        height: 19px;
    }
    .pf3
    {
        height: 19px;
    }
    .pf4
    {
        height: 19px;
    }
    .pf5
    {
        height: 19px;
    }
    .pf6
    {
        height: 19px;
    }
    .pf7
    {
        height: 19px;
    }
    .pf8
    {
        height: 19px;
    }
</style>
<style type="text/css">
    .pftd
    {
        padding: 3px;
        line-height: 15px;
    }
    .pftd input
    {
        border: none;
    }
    .pfck
    {
        padding-left: 30px;
    }
    .pfTitle
    {
        font-weight: bold;
    }
    .pftd img
    {
        margin-top: 3px;
    }
</style>
<div id="ca-user-delegation">
    <table class="ca-workflow-form-table">
        <tr>
            <td class="label align-center w25">
                Delegate From
                <br />
                被代理人
            </td>
            <td class="value">
                <asp:Label ID="lblUser" runat="server" />
                <asp:HiddenField ID="hidUserAccount" runat="server" />
            </td>
        </tr>
        <tr style="display: none">
            <td class="label align-center w25">
                Delegate To
                <br />
                代理人
            </td>
            <td class="value">
                <ca:CAPeopleFinder ID="pfSelector" runat="server" AllowTypeIn="true" MultiSelect="false"
                    Width="200" CssClass="ca-people-finder" />
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Workflows Delegated
                <br />
                代理审批流程
            </td>
            <!--<td class="value" id="ca-moss-menu" >-->
            <td style="line-height: 23px; cursor: pointer" class="value">
                <table>
                    <!--固定Input、PeopleFinder控件-->
                    <tr>
                        <td class="pftd pfTitle">
                            Commercial
                        </td>
                        <td class="pftd">
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck108" /><span>Supplier Re-Ticketing Charge</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf108" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck109" /><span>Store Sampling</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf109" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck110" /><span>New Supplier Creation</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf110" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck111" /><span>Supplier Re-Inspection Charge</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf111" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck127" /><span>PAD Change Request</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf127" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck131" /><span>General Purchase Request</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf131" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfTitle">
                            Construction
                        </td>
                        <td class="pftd">
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck112" /><span>Construction Purchasing Request</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf112" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck113" /><span>New Store Budget Application</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf113" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck114" /><span>Store Maintenance</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf114" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck118" /><span>Purchase Request</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf118" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck119" /><span>Purchase Order</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf119" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfTitle">
                            Finance
                        </td>
                        <td class="pftd">
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck115" /><span>Non Trade Supplier Setup Maintenance</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf115" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck116" /><span>Project Control Creation</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf116" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck117" /><span>Project Control Maintenance</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf117" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck120" /><span>Cash Advance Request</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf120" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck121" /><span>Employee Expense Claim</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf121" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck122" /><span>Travel Expense Claim</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf122" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck123" /><span>Credit Card Claim</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf123" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck124" /><span>Employee Expense Claim For SAP</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf124" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck125" /><span>Credit Card Claim For SAP</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf125" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck126" /><span>Cash Advance Request For SAP</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf126" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck128" /><span>Travel Expense Claim For SAP</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf128" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck129" /><span>Payment Request</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf129" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck130" /><span>Expatriate Benefit Claim</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf130" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfTitle">
                            HR
                        </td>
                        <td class="pftd">
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck103" /><span>Leave Application</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf103" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck104" /><span>New Employee Equipment Application</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf104" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck105" /><span>Business Card Application</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf105" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck106" /><span>Travel Request</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf106" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfTitle">
                            IT
                        </td>
                        <td class="pftd">
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck101" /><span>Change Request</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf101" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck102" /><span>It Request</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf102" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfTitle">
                            Legal
                        </td>
                        <td class="pftd">
                        </td>
                    </tr>
                    <tr>
                        <td class="pftd pfck">
                            <input type="checkbox" id="ck107" /><span>Chopping Application</span>
                        </td>
                        <td class="pftd">
                            <ca:CAPeopleFinder ID="pf107" runat="server" AllowTypeIn="true" MultiSelect="false"
                                Width="150" CssClass="ca-people-finder pf" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                From Date
                <br />
                超始日期
            </td>
            <td class="value">
                <ca:CADateTimeControl runat="server" ID="dtBegin" DateOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                End Date
                <br />
                结束日期
            </td>
            <td class="value">
                <ca:CADateTimeControl runat="server" ID="dtEnd" DateOnly="true" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfModule" runat="server" Value="" />
    <script type="text/javascript" src="../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
    <script type="text/javascript">
        $(function () {
            var $list = $("td.pfck");
            $list.each(function () {
                $(this).mousemove(function () {
                    $(this).css("backgroundColor", "#D0FBE3");
                });
                $(this).mouseout(function () {
                    $(this).css("backgroundColor", "white");
                });
            });
            var $hfModule = $("input[id$='hfModule']");
            var Module = $hfModule.val().split(";");
            for (var i = 0; i < Module.length; i++) {
                if (Module[i] != "") {
                    var id = "ck" + Module[i];
                    $("#" + id).attr("checked", true);
                }
            }
        });

        function CheckSubmit() {
            var result = true;
            var tag = "";
            var $input = $("td.pfck input");
            var $hfModule = $("input[id$='hfModule']");
            $hfModule.val("");
            $input.each(function () {
                if ($(this).attr("checked")) {
                    tag += $(this).attr("id").replace("ck", "") + ";";
                }
            });
            if (tag == "") {
                alert("Please select at least 1 module for delegation assignment.");
                result = false;
            }
            if (result) {
                $hfModule.val(tag);
            }
            return result;
        }

    </script>
</div>
