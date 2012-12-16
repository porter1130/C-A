<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataListEdit.ascx.cs"
    Inherits="CA.WorkFlow.UI.CashAdvanceRequest.DataListEdit" %>
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
        width: 80%;
    }
     .wrapdiv
    {
        padding: 2px;
        border: 1px solid red;
    }
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4">
            <h3>
                Cash Advance Request Form<br />
                借款申请表</h3>
        </td>
    </tr>
    <tr>
        <td>
            WorkFlowNumber<br />
            工作流ID
        </td>
        <td>
            <asp:Label ID="lblWorkFlowNumber" runat="server"></asp:Label>
        </td>
        <td>
            Requested ID<br />
            申请人ID
        </td>
        <td>
            <asp:Label ID="lblRequestedID" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="w20">
            Requested Name<br />
            申请人姓名
        </td>
        <td class="w30">
            <asp:Label ID="lblRequestedBy" runat="server"></asp:Label>
        </td>
        <td class="w20">
            Dept<br />
            部门
        </td>
        <td class="w30">
            <asp:Label ID="lblDept" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Description<br />
            描述
        </td>
        <td colspan="3" >
            <asp:TextBox ID="txtAdvanceRemark" runat="server" class="ExpenseDescription"></asp:TextBox>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table">
    <tr style="height: 20px">
        <td colspan="3">
            <h3>
                Cash advance SAP details</h3>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">
            Payment Method<br />
            付款方式
        </td>
        <td colspan="2">
            <asp:DropDownList ID="dplTerm" runat="server" Width="80px" CssClass="term">
                <asp:ListItem Value="Cash" Text="Cash"></asp:ListItem>
                <asp:ListItem Value="Transfer" Text="Transfer"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr style="font-weight: bold;">
        <td style="width: 20%;">
            Dr/Cr
        </td>
        <td class="w40">
            ExpenseType
        </td>
        <td>
            Amount
        </td>
    </tr>
    <tr>
        <td style="text-align: left; padding-left: 30px;">
            Dr
        </td>
        <td style="text-align: left; padding-left: 30px;">
            OR-employee vendor
        </td>
        <td align="left">
            <asp:TextBox ID="txtAmount" runat="server" CssClass="amount"></asp:TextBox>
            <asp:HiddenField ID="hfAmount" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="text-align: right; padding-right:30px;">
            Cr
        </td>
        <td style="text-align: right; padding-right:30px;" class="cr">
            
        </td>
         <td >
            <asp:TextBox ID="txtEmployeeVendor" runat="server" Style="border: none;text-align: right;" ReadOnly="true"></asp:TextBox>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table">
    <tr>
        <td style="width: 396px">
            Total Amount<br />
            总金额(RMB)
        </td>
        <td style="text-align: right;padding-right:25px;">
            <asp:Label ID="lbTotalAmount" runat="server" CssClass="TotalAmount"></asp:Label>
        </td>
    </tr>
</table>
<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    function BindAmountEvent() {
        var $amount = $("input.amount");
        $amount.blur(function () {
            if (isNaN($(this).val()) || $(this).val() <= 0 || $(this).val() > 100000000) {
                $(this).val("");
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).wrap("<span class=\"wrapdiv\"></span>");
                }
                alert('Please fill the valid number.');
                return;
            } else {
                $(this).parent().removeClass("wrapdiv");
            }
        });
    }

    function CalAmount() {
        var result = false;
        var $inputAmount = $("input.amount");
        var $TotalAmount = $('#<%=this.hfAmount.ClientID %>');
        var totalAmount = 0;
        totalAmount += parseFloat($inputAmount.val());
        if (totalAmount != parseFloat($TotalAmount.val())) {
            //alert('Not equal to the total amount!\nPlease fill the valid number.');
            result = true;
        }
        return result;
    }

    function CheckSubmit() {
        $(".wrapdiv").removeClass("wrapdiv");
        var result = true;
        var msg = "";
        var $amount = $("input.amount");
        if ($.trim($amount.val()) == "") {
            msg += "Please fill the Amount.\n";
            if (!$amount.parent().hasClass("wrapdiv")) {
                $amount.wrap("<span class=\"wrapdiv\"></span>");
            }
            result = false;
        }
        var calAmountresult = CalAmount();
        if (calAmountresult) {
            msg += "Please check cash advance amount because it is not equal to original cash advance amount.\nPlease fill the Amount.";
            var $amount = $("input.amount");
            if (!$amount.parent().hasClass("wrapdiv")) {
                $amount.wrap("<span class=\"wrapdiv\"></span>");
            }
            result = false;
        }
        var $ExpenseDescription = $("input.ExpenseDescription");
        if ($.trim($ExpenseDescription.val()) == "") {
            msg += "Please fill the description.\n";
            if (!$ExpenseDescription.parent().hasClass("wrapdiv")) {
                $ExpenseDescription.wrap("<span class=\"wrapdiv\"></span>");
            }
            result = false;
        }
        if (msg != "") {
            alert(msg);
        }
        return result;
    }

    $(function () {
        BindAmountEvent();
        SetCRText();
    });

    function SetCRText() {
        var $term = $("select.term");
        var $cr = $("td.cr");
        if ($term.val() == "Cash") {
            $cr.html("Cash-RMB  11010100");
        } else {
            $cr.html("bank-DB  11020600");
        }
        $term.change(function () {
            if ($(this).val() == "Cash") {
                $cr.html("Cash-RMB  11010100");
            } else {
                $cr.html("bank-DB  11020600");
            }
        });
     }
</script>
