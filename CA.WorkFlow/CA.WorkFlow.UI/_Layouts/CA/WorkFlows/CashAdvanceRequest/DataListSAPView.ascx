<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataListSAPView.ascx.cs"
    Inherits="CA.WorkFlow.UI.CashAdvanceRequest.DataListSAPView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<style type="text/css">
    .ca-workflow-form-table td
    {
        padding: 5px;
        border-bottom: 1px solid #CCCCCC;
        text-align: left;
        margin: 0;
        line-height: 15px;
        cursor: pointer;
    }
    .ca-workflow-form-table
    {
        margin-top: 25px;
    }
    .ca-workflow-form-table input
    {
        border: #cccccc 1px solid;
        background-color: transparent;
        width: auto;
    }
    .spgvSAPtr
    {
        background-color: transparent;
    }
    .spgvSAPtrbg
    {
        background-color: #f2f2f2;
    }
    .saptd
    {
        padding-left: 38px;
    }
</style>
<table class="ca-workflow-form-table" id="expenseTypetable">
    <tr style="height: 25px">
        <td style="text-align: center" colspan="3">
            <h3>
                Cash Advance Request Sap Details</h3>
        </td>
    </tr>
    <tr>
        <td style="font-weight: bold; width: 250px">
            <input type="checkbox" class="ckAll" title="Select All Items" />　　Cash Advance Request Form ID
        </td>
        <td style="padding-left: 26px; font-weight: bold; width: 300px">
            Cash Advance Request WorkFlowNumber
        </td>
        <td style="text-align: left; font-weight: bold;" colspan="">
            Post Count
        </td>
    </tr>
    <asp:Repeater ID="rpSAPData" runat="server">
        <ItemTemplate>
            <tr class="wftr">
                <td class="td_check">
                    <img src="../images/pixelicious_001.png" alt="Expand Detail Items" align="absmiddle"
                        onclick="DisplayDetails(this)" />
                    <asp:CheckBox ID="ckAllItems" runat="server" CssClass="itemCK" />
                    <span class="span_wftitle">
                        <%#Eval("Title") %></span>
                </td>
                <td style="padding-left: 26px">
                    <%# Eval("CAWorkflowNumber")%>
                </td>
                <td class="pc" style="text-align: left" colspan="2">
                    <%#Eval("PostCount") %>
                    <asp:HiddenField ID="hfwfID" runat="server" Value='<%# Eval("Title")%>' />
                    <asp:HiddenField ID="hfEmployeeID" runat="server" Value='<%# Eval("EmployeeID")%>' />
                    <asp:HiddenField ID="hfEmployeeName" runat="server" Value='<%# Eval("EmployeeName")%>' />
                    <asp:HiddenField ID="hfCAWorkflowNumber" runat="server" Value='<%# Eval("CAWorkflowNumber")%>' />
                    <asp:HiddenField ID="hfAmount" runat="server" Value='<%# Eval("Amount")%>' />
                    <asp:HiddenField ID="hfAdvanceType" runat="server" Value='<%# Eval("AdvanceType")%>' />
                    <asp:HiddenField ID="hfAdvanceRemark" runat="server" Value='<%# Eval("AdvanceRemark")%>' />
                </td>
            </tr>
            <tr class="tr_details hidden">
                <td class="saptd" style="padding-left: 53px">
                    Requested Name<br />
                    申请人姓名
                </td>
                <td colspan="2" style="padding-left: 26px">
                    Amount<br />
                    金额(RMB)
                </td>
            </tr>
            <tr class="tr_details hidden">
                <td class="saptd" style="padding-left: 53px">
                    <%#Eval("EmployeeName")%>
                </td>
                 <td colspan="2" style="padding-left: 26px">
                    <%#Eval("Amount")%>
                </td>
            </tr>
            <tr class="tr_details hidden">
                <td class="saptd" style="padding-left: 53px">
                    <%#Eval("AdvanceType").ToString()=="Cash"?"Cash-RMB 11010100":"bank-DB 11020600"%>
                </td>
                <td colspan="2" style="padding-left: 26px">
                    -<%#Eval("Amount")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>

<script type="text/javascript">
    function CheckSubmit() {
        CreateForbidDIV();
        var result = false;
        var $chkitem = $("input[name$='ckAllItems']");
       
        $chkitem.each(function () {
            if ($(this).attr("checked")) {
                result = true;
                $(this).attr("title", "checked");
            }
        });
        if (!result) {
            alert("Please Select SAP Claim Items.");
            ClearForbidDIV();
            
        }
        return result;
    }

    function DisplayDetails(obj) {
        var $trdetail1 = $(obj).parents('tr').first().next();
        var $trdetail2 = $(obj).parents('tr').first().next().next();
        var $trdetail3 = $(obj).parents('tr').first().next().next().next();
        $trdetail1.toggle();
        $trdetail2.toggle();
        $trdetail3.toggle();
        //$(obj).next().toggle();
        if ($(obj).parents('tr').first().next().is(":visible")) {
            $(obj).parents('tr').first().find("td.td_check img").attr("src", "../images/pixelicious_028.png");
            $(obj).parents('tr').first().find("td.td_check img").attr("alt", "Collapse Detail Items");
        } else {
            $(obj).parents('tr').first().find("td.td_check img").attr("src", "../images/pixelicious_001.png");
            $(obj).parents('tr').first().find("td.td_check img").attr("alt", "Expand Detail Items");
        }
    }

    $(function () {
        var $ckAll = $("input.ckAll");
        var $item = $("span.itemCK input");
        $ckAll.click(function () {
            var result = $(this).attr("checked");
            $item.each(function () {
                $(this).attr("checked", result);
            });
        });
    });

</script>
