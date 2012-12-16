<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataListView.ascx.cs"
    Inherits="CA.WorkFlow.UI.CreditCardClaim.DataListView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .ca-workflow-form-table td
    {
        padding: 5px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        text-align: center;
        margin: 0;
        line-height: 15px;
    }
    .ca-workflow-form-table
    {
        margin-top: 25px;
    }
     .ca-workflow-form-table input
     {
         width:80%;
     }
    .ca-workflow-form-table select
    {
        margin-left: 0px;
        width: 100%;
    }
    .ExpenseType1
    {
        position: absolute;
        left: 0px !important;
        left: -45px;
        top: -15px;
        z-index: 1000;
    }
    .CostCenter
    {
        position: absolute;
        left: 0px !important;
        left: -62px;
        top: -15px;
    }
    .CostCenterTD
    {
        width: 260px;
    }
    .ExpenseTypeTD
    {
        width: 215px;
    }
    .wrapdiv
    {
        padding: 2px;
        border: 1px solid red;
    }
    .dealamount
    {
        color:#06c;
     }
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4">
            <h3>
                Credit Card Claim Form<br />
                员工报销申请表
            </h3>
        </td>
    </tr>
    <tr>
        <td class="w20">
            WorkFlowNumber<br />
            工作流ID
        </td>
        <td colspan="3" >
            <asp:Label ID="lblWorkFlowNumber" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="w20">
            Requested ID<br />
            申请人ID
        </td>
        <td class="w30">
            <asp:Label ID="lblRequestedID" runat="server"></asp:Label>
        </td>
        <td class="w20">
            Requested Name<br />
            申请人姓名
        </td>
        <td class="w30">
            <asp:Label ID="lblRequestedBy" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Expense Description<br />
            描述
        </td>
        <td colspan="3">
            <asp:Label ID="txtExpenseDescription" runat="server"></asp:Label>
        </td>
    </tr>
     <tr class="SAPNo">
        <td>
            SAP.No<br />
            SAP编号
        </td>
        <td colspan="3">
            <asp:Label ID="lblSAPNo" runat="server"  CssClass="SAPNo"></asp:Label>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table" id="RMBExpenseTypetable">
    <tr style="height: 20px">
        <td colspan="6">
            <h3>
                Credit Card Claim RMB details</h3>
        </td>
    </tr>
    <%--  <tr>
        <td class="w5">
            No
        </td>
        <%--<td class="w15">
                    ExpenseType<br />
                    费用类别
                </td>
                <td class="w20">
                    CostCenter<br />
                    成本中心
                </td>
                <td class="w15">
                    Deal Amount<br />
                    交易金额(RMB)
                </td>
                <td class="w15">
                    Deposit Amount<br />
                    存入金额(RMB)
                </td>
                <td class="w15">
                    Pay Amount<br />
                    支出金额(RMB)
                </td>
                <td class="w15">
                    GL Account<br />
                    总帐会计
                </td>--%>
    <%--<td class="w15">
            ExpenseType
        </td>
        <td class="w20">
            CostCenter
        </td>
        <td class="w15">
            Claim Amount
        </td>
        <td class="w5">
            Currency
        </td>
        <td class="w15">
            GL Account
        </td>
        <td class="w25">
            Transaction Description
        </td>
    </tr>--%>
    <tr style="font-weight: bold">
       <%-- <td class="w5" style="padding: 10px">
            No
        </td>--%>
        <td class="w5" style="padding: 10px">
            Dr/Cr
        </td>
        <td class="w20">
            ExpenseType
        </td>
        <td class="w15">
            GL Account
        </td>
        <td class="w15">
            CostCenter
        </td>
        <td class="w25">
            Transaction Description
        </td>
        <td class="w15">
            Amount(RMB)
        </td>
    </tr>
    <asp:Repeater ID="rptItem" runat="server">
        <ItemTemplate>
            <tr>
               <%-- <td style="padding: 8px">
                    <%# Container.ItemIndex + 1 %>
                </td>--%>
                <td class="DrCr" style="text-align: left;padding: 8px">
                    Dr
                </td>
                <td class="ExpenseType" style="text-align: left">
                    <%# Eval("ExpenseType")%>
                </td>
                <td>
                    <%# Eval("GLAccount")%>
                </td>
                <td>
                    <%# Eval("CostCenter")%>
                </td>
                <td>
                    <%# Eval("TransactionDescription")%>
                </td>
                <td class="ItemAmount" style="text-align: left">
                    <%# Eval("DealAmount")%>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="height: 20px;">
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <tr class="catr">
        <td>
        </td>
        <td style="padding: 10px; text-align: left" colspan="4">
            Total claimed amount
        </td>
        <td style="text-align: right" class="totalClaimedAmount">
            1
        </td>
    </tr>
    <tr>
        <td style="height: 20px;" colspan="6">
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td style="padding: 10px; text-align: left" colspan="4">
            Net balance
        </td>
        <td style="text-align: right">
            0
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table" id="USDExpenseTypetable">
    <tr style="height: 20px">
        <td colspan="6">
            <h3>
                Credit Card Claim USD details</h3>
        </td>
    </tr>
    <%--   <tr>
        <td class="w5">
            No
        </td>
        <%--<td class="w15">
                    ExpenseType<br />
                    费用类别
                </td>
                <td class="w20">
                    CostCenter<br />
                    成本中心
                </td>
                <td class="w15">
                    Deal Amount<br />
                    交易金额(RMB)
                </td>
                <td class="w15">
                    Deposit Amount<br />
                    存入金额(RMB)
                </td>
                <td class="w15">
                    Pay Amount<br />
                    支出金额(RMB)
                </td>
                <td class="w15">
                    GL Account<br />
                    总帐会计
                </td>--%>
    <%-- <td class="w15">
            ExpenseType
        </td>
        <td class="w20">
            CostCenter
        </td>
        <td class="w15">
            Claim Amount
        </td>
        <td class="w5">
            Currency
        </td>
        <td class="w15">
            GL Account
        </td>
        <td class="w25">
            Transaction Description
        </td>
    </tr>--%>
    <tr style="font-weight: bold">
       <%-- <td class="w5" style="padding: 10px">
            No
        </td>--%>
         <td class="w5" style="padding: 10px">
            Dr/Cr
        </td>
        <td class="w20">
            ExpenseType
        </td>
        <td class="w15">
            GL Account
        </td>
        <td class="w15">
            CostCenter
        </td>
        <td class="w25">
            Transaction Description
        </td>
        <td class="w15">
            Amount(RMB)
        </td>
    </tr>
    <asp:Repeater ID="rptUSDItem" runat="server">
        <ItemTemplate>
            <tr>
               <%-- <td style="padding: 8px">
                    <%# Container.ItemIndex + 1 %>
                </td>--%>
                <td class="DrCr" style="text-align: left;padding: 8px">
                    Dr
                </td>
                <td class="ExpenseType" style="text-align: left">
                    <%# Eval("ExpenseType")%>
                </td>
                <td>
                    <%# Eval("GLAccount")%>
                </td>
                <td>
                    <%# Eval("CostCenter")%>
                </td>
                <td>
                    <%# Eval("TransactionDescription")%>
                </td>
                <td class="ItemAmount" style="text-align: left">
                    <%# Eval("DealAmount")%>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="height: 20px;">
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <tr class="catr">
        <td>
        </td>
        <td style="padding: 10px; text-align: left" colspan="4">
            Total claimed amount
        </td>
        <td style="text-align: right" class="totalClaimedAmount">
            1
        </td>
    </tr>
    <tr>
        <td style="height: 20px;" colspan="6">
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td style="padding: 10px; text-align: left" colspan="4">
            Net balance
        </td>
        <td style="text-align: right">
            0
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table" style="display: none">
    <tr>
        <td style="width: 430px">
            Total Amount<br />
            总金额(RMB)
        </td>
        <td>
            <asp:Label ID="lbTotalAmount" runat="server" CssClass="TotalAmount"></asp:Label>
            <asp:HiddenField ID="hfTableStatus" runat="server" Value="" />
        </td>
    </tr>
</table>
<script type="text/javascript">
    $(function () {
        DisplayOrHideTable();
        ChangeRMBCr();
        ChangeUSDCr();
        var $SAPNo = $("span.SAPNo");
        if ($SAPNo.text() == ";") {
            $("tr.SAPNo").hide();
        }
    });

    function DisplayOrHideTable() {
        var $hfTableStatus = $('#<%=this.hfTableStatus.ClientID %>');
        if ($hfTableStatus.val() == "RMB") {
            $("#RMBExpenseTypetable").hide();
        }
        if ($hfTableStatus.val() == "USD") {
            $("#USDExpenseTypetable").hide();
        }
    }

    function ChangeRMBCr() {
        var $expenseType = $("#RMBExpenseTypetable tr td.ExpenseType");
        var $totalClaimedAmount = $("#RMBExpenseTypetable tr.catr td.totalClaimedAmount");
        var amount = 0;
        $expenseType.each(function () {
            var type = $(this).text();
            var $DrCr = $(this).prev();
            var $ItemAmount = $(this).parent().find("td.ItemAmount");
            if (type.indexOf("OR - employee vendor") != -1) {
                $(this).css("textAlign", "Right");
                $DrCr.css("textAlign", "Right");
                $DrCr.text("Cr");
                $ItemAmount.css("textAlign", "Right");
            } else {
                amount += parseFloat($ItemAmount.text());
            }
        });
        amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
        $totalClaimedAmount.text(amount);
    }

    function ChangeUSDCr() {
        var $expenseType = $("#USDExpenseTypetable tr td.ExpenseType");
        var $totalClaimedAmount = $("#USDExpenseTypetable tr.catr td.totalClaimedAmount");
        var amount = 0;
        $expenseType.each(function () {
            var type = $(this).text();
            var $DrCr = $(this).prev();
            var $ItemAmount = $(this).parent().find("td.ItemAmount");
            if (type.indexOf("OR - employee vendor") != -1) {
                $(this).css("textAlign", "Right");
                $DrCr.css("textAlign", "Right");
                $DrCr.text("Cr");
                $ItemAmount.css("textAlign", "Right");
            } else {
                amount += parseFloat($ItemAmount.text());
            }
        });
        amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
        $totalClaimedAmount.text(amount);
    }

</script>
