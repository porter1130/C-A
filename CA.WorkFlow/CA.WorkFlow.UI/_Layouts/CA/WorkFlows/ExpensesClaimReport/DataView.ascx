<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.ExpensesClaimReport.DataView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .ca-workflow-form-table1 td
    {
        padding: 10px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        text-align: center;
        margin: 0;
        line-height: 15px;
    }
    .ca-workflow-form-table1
    {
        margin-top: 20px;
        cursor: pointer;
    }
    .ca-workflow-form-table td
    {
        padding: 10px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        margin: 0;
        line-height: 15px;
    }
    .ca-workflow-form-table
    {
        margin-top: 20px;
        cursor: pointer;
    }
    .wrapdiv
    {
        border: 1px solid red;
        padding: 2px;
    }
    .amountcolor
    {
        color: #06c;
    }
    .field
    {
        font-weight:bold;
    }
    .fieldright
    {
        text-align: right;
    }
    .label1
    {
        text-align: center;
    }
</style>
<script type="text/javascript">
    function popexcel(url) {
        var w = window.open(url, '_blank');
        w.location.href = url;
    }
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table ca-workflow-form-table1">
            <tr>
                <td colspan="4">
                    <h3>
                        Expense claim report conditions</h3>
                </td>
            </tr>
            <tr class="field">
                <td class="w15">
                    From / Year
                </td>
                <td class="w15">
                    To / Year
                </td>
                <td class="w35">
                    Module Name
                </td>
                <td class="w35">
                    Expense Type
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlFrom" runat="server" CssClass="ddlFrom">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTo" runat="server" CssClass="ddlTo">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlModule" runat="server" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="Travel Expense Claim" Value="Travel Expense Claim"></asp:ListItem>
                        <asp:ListItem Text="Employee Expense Claim" Value="Employee Expense Claim"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="ddlExpenseType">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="3">
            <h3 class="label1 et1">
                Expense Type - Hotel</h3>
        </td>
    </tr>
    <tr class="field label1">
        <td class="w30">
            Department /Team
        </td>
        <td class="w30">
            No of Special Approval Granted
        </td>
        <td class="w40">
            Total Amt > Company Std (in Rmb)
        </td>
    </tr>
    <asp:Repeater ID="rpDepartmentItem" runat="server">
        <ItemTemplate>
            <tr class="label1">
                <td>
                    <%# Eval("Department")%>
                </td>
                <td class="DepartmentItem">
                    <%# Eval("TotalCount")%>
                </td>
                <td class="fieldright DepartmentItemAmount">
                    <%# Eval("TotalAmount")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <tr class="field label1">
        <td>
            Total
        </td>
        <td class="TotalDepartmentItem">
        </td>
        <td class="fieldright TotalDepartmentItemAmount">
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4">
            <h3 class="label1 et1">
                Expense Type - Hotel</h3>
        </td>
    </tr>
    <tr class="field label1">
        <td class="w20">
            Department /Team
        </td>
        <td class="w15">
            Name of Staff
        </td>
        <td class="w30">
            No of Special Approval Granted
        </td>
        <td class="w35">
            Total Amt > Company Std (in Rmb)
        </td>
    </tr>
    <asp:Repeater ID="rpDepartmentStaffItem" runat="server">
        <ItemTemplate>
            <tr class="label1 DepartmentStaff">
                <td class="DepartmentColumn">
                    <%# Eval("Department")%>
                </td>
                <td class="NameColumn">
                    <%# Eval("Name")%>
                </td>
                <td class="DepartmentStaffItem">
                    <%# Eval("TotalCount")%>
                </td>
                <td class="fieldright DepartmentStaffItemAmount">
                    <%# Eval("TotalAmount")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <tr class="field label1">
        <td>
            Total
        </td>
        <td>
        </td>
        <td class="TotalDepartmentStaffItem">
        </td>
        <td class="fieldright TotalDepartmentStaffItemAmount">
        </td>
    </tr>
</table>
<asp:HiddenField ID="hfExportStatus" runat="server"  Value="0" />
<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    function CheckExport() {
        var result = true;
        var $hfExportStatus = $("input[id$='hfExportStatus']");
        if ($hfExportStatus.val() == "0") {
            result = false;
            alert("Please select expense claim report conditions to query !");
        }
        return result;
    }

    function CheckSelectToYear(obj) {
        var result = true;
        var $toYear = obj;
        var $fromYear = $toYear.parent().prev().find("select.ddlFrom");
        if (parseInt($toYear.val()) < parseInt($fromYear.val())) {
            alert("The end date should be greater than or equal to the start date !");
            var date = new Date();
            $fromYear.val(date.getYear());
            $toYear.val(date.getYear());
            result = false;
        }
        return result;
    }

    function SetExpenseType() {
        var $ddlExpenseType = $("select.ddlExpenseType");
        var $et1 = $("h3.et1");
        var et = "Expense Type - ";
        $et1.each(function () {
            $(this).html(et + $ddlExpenseType.val());
        });
    }

    function BindEvent() {
        var $ddlTo = $("select.ddlTo");
        $ddlTo.change(function () {
            var result = CheckSelectToYear($(this));
            return false;
        });

        var $ddlExpenseType = $("select.ddlExpenseType");
        var $et1 = $("h3.et1");
        var et = "Expense Type - ";
        $ddlExpenseType.change(function () {
            var expenseType = $(this).val();
            $et1.each(function () {
                $(this).html(et + expenseType);
            });
        });
    }

    function CalAmount() {
        // II Special Approval Granted by Dept/Staff 
        var $NameColumn = $("tr.DepartmentStaff td.NameColumn");
        $NameColumn.each(function () {
            if ($(this).html() == "") {
                var $tr = $(this).parent();
                $tr.css("fontWeight", "bold");
            }
        });

        var $TotalDepartmentStaffItem = $("td.TotalDepartmentStaffItem");
        var $DepartmentStaffItem = $("tr.DepartmentStaff td.DepartmentStaffItem");
        var total = 0;
        $DepartmentStaffItem.each(function () {
            total += parseInt($(this).html());
        });
        $TotalDepartmentStaffItem.html(total / 2);

        var $TotalDepartmentStaffItemAmount = $("td.TotalDepartmentStaffItemAmount");
        var $DepartmentStaffItemAmount = $("tr.DepartmentStaff td.DepartmentStaffItemAmount");
        var totalAmount = 0;
        $DepartmentStaffItemAmount.each(function () {
            totalAmount += parseFloat($(this).html());
        });
        totalAmount = totalAmount / 2;
        var amount = Math.round(totalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        $TotalDepartmentStaffItemAmount.html(amount);

        // I
        var $TotalDepartmentItem = $("td.TotalDepartmentItem");
        var $DepartmentItem = $("td.DepartmentItem");
        var total1 = 0;
        $DepartmentItem.each(function () {
            total1 += parseInt($(this).html());
        });
        $TotalDepartmentItem.html(total1);

        var $TotalDepartmentItemAmount = $("td.TotalDepartmentItemAmount");
        var $DepartmentItemAmount = $("td.DepartmentItemAmount");
        var totalAmount1 = 0;
        $DepartmentItemAmount.each(function () {
            totalAmount1 += parseFloat($(this).html());
        });
        var amount1 = Math.round(totalAmount1 * Math.pow(10, 2)) / Math.pow(10, 2);
        $TotalDepartmentItemAmount.html(amount1);
    }

    $(function () {
        SetExpenseType();
        BindEvent();
        CalAmount();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });

    function EndRequestHandler() {
        SetExpenseType();
        BindEvent();
        CalAmount();
    }
</script>
