<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataListView.ascx.cs" Inherits="CA.WorkFlow.UI.CashAdvanceRequest.DataListView"   %>
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
        <td colspan="3" class="ExpenseDescription">
            <asp:Label ID="lblAdvanceRemark" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="SAPNo">
        <td>
            SAP.No<br />
            SAP编号
        </td>
        <td colspan="3">
            <asp:Label ID="lblSAPNo" runat="server" CssClass="SAPNo"></asp:Label>
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
            <asp:Label ID="lblTerm" runat="server" Text="" CssClass="term"></asp:Label>
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
        <td align="left" style="text-align: left; padding-left:30px;">
             <asp:Label ID="lblAmount" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="text-align: right; padding-right:30px;">
            Cr
        </td>
        <td style="text-align: right; padding-right:30px;" class="cr">
           
        </td>
         <td style="text-align: right; padding-right:30px;">
             <asp:Label ID="lblEmployeeVendor" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table">
    <tr>
        <td style="width: 396px">
            Total Amount<br />
            总金额(RMB)
        </td>
        <td style="text-align: right;padding-right:30px;">
            <asp:Label ID="lbTotalAmount" runat="server" CssClass="TotalAmount"></asp:Label>
        </td>
    </tr>
</table>

<script type="text/javascript">
    $(function () {
        var $SAPNo = $("span.SAPNo");
        if ($SAPNo.text() == "") {
            $("tr.SAPNo").hide();
        }
        SetCRText();
    });
    function SetCRText() {
        var $term = $("span.term");
        var $cr = $("td.cr");
        if ($term.text() == "Cash") {
            $cr.html("Cash-RMB  11010100");
        } else {
            $cr.html("bank-DB  11020600");
        }
        
    }
</script>
