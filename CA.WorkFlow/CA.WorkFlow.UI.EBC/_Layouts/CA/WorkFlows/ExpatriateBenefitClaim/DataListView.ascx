<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataListView.ascx.cs"
    Inherits="CA.WorkFlow.UI.EBC.DataListView" %>
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
        left: -95px;
        top: -15px;
        z-index: 1000;
    }
    .CostCenter
    {
        position: absolute;
        left: 0px !important;
        left: -95px;
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
   
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4">
            <h3>
                Expatriate Benefit Claim Form<br />
                外籍员工福利报销申请单
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
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table" id="expenseTypetable">
            <tr style="height: 20px">
                <td colspan="5">
                    <h3>
                        Expatriate benefit claim details</h3>
                </td>
            </tr>
            <tr style="font-weight:bold">
                <%--<td class="w5" style="padding:10px">
                    No
                </td>--%>
                <td class="w10" style="padding:10px">
                    Dr/Cr
                </td>
                <td class="w45">
                    BenefitType
                </td>
                <td class="w15">
                    GL Account
                </td>
                <td class="w15">
                    CostCenter
                </td>
                <td class="w15">
                    Amount
                </td>
            </tr>
            <asp:Repeater ID="rptItem" runat="server" >
                <ItemTemplate>
                    <tr>
                        <%--<td style="padding: 8px">
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
                        <td class="ItemAmount" style="text-align: left">
                            <%# Eval("ItemAmount")%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="height: 20px;">
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="catr">
               <%-- <td>
                </td>--%>
                <td style="padding: 10px; text-align: left" colspan="4">
                    Total claimed amount
                </td>
                <td style="text-align: right" class="totalClaimedAmount">
                    1
                </td>
            </tr>
            <tr>
                <td style="height: 20px;" colspan="5">
                </td>
            </tr>
            <tr>
                <%--<td>
                </td>--%>
                <td style="padding: 10px; text-align: left" colspan="4">
                    Net balance
                </td>
                <td style="text-align: right">
                    0
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<table class="ca-workflow-form-table">
    <tr>
        <td style="width: 565px">
            Total Amount<br />
            总金额(RMB)
        </td>
        <td style="text-align:right">
            <asp:Label ID="lbTotalAmount" runat="server" CssClass="TotalAmount"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Cash Advance<br />
            员工借款(RMB)
        </td>
        <td style="text-align:right">
            <asp:Label ID="lblCashAdvanceAmount" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Payable to Employee/(Refund to Finance)<br />
            应付员工/(财务应收) (RMB)
        </td>
         <td style="text-align:right">
            <asp:Label ID="lblPreTotalAmount" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        var $SAPNo = $("span.SAPNo");
        if ($SAPNo.text() == "") {
            $("tr.SAPNo").hide();
        }
        ChangeCr();
    });

    function ChangeCr() {
        var $expenseType = $("#expenseTypetable tr td.ExpenseType");
        var $totalClaimedAmount = $("#expenseTypetable tr.catr td.totalClaimedAmount");
        var amount = 0;
        $expenseType.each(function () {
            var type = $(this).text();
            var $DrCr = $(this).prev();
            var $ItemAmount = $(this).parent().find("td.ItemAmount");
            if (type.indexOf("OR - cash advance") != -1
                || type.indexOf("OR - employee vendor") != -1) {
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
