<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataListView.ascx.cs"
    Inherits="CA.WorkFlow.UI.PaymentRequestSAP.DataListView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .CurrencyTR
    {
         display:none;
     }
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
                Payment Request SAP WorkFlow<br />
                付款申请表
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
    <tr class="CurrencyTR">
        <td class="w20">
            Currency<br />
            币种
        </td>
        <td class="w30">
            <asp:Label ID="lblCurrency" runat="server" Text="" CssClass="lblCurrency"></asp:Label>
        </td>
        <td class="w20">
            Exchange Rate<br />
            汇率
        </td>
        <td class="w30">
            <asp:Label ID="lblExchangeRate" runat="server" Text="" CssClass="lblExchangeRate"></asp:Label>
        </td>
    </tr>
     <tr>
        <td>
            Payment Description<br />
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
                <td colspan="7">
                    <h3>
                         Payment Request SAP Details</h3>
                </td>
            </tr>
            <tr style="font-weight:bold">
                <%--<td class="w5" style="padding:10px">
                    No
                </td>--%>
                <td class="w10" style="padding:10px">
                    Dr/Cr
                </td>
                <td class="w10 FANO">
                    Asset No
                </td>
                <td class="w20">
                     <asp:Label ID="lblExpenseType" runat="server" Text="Label"></asp:Label>
                </td>
                <td class="w15">
                     <asp:Label ID="lblGLAccount" runat="server" Text="Label"></asp:Label>
                </td>
                <td class="w15">
                    CostCenter
                </td>
                <td class="w10 ba" >
                    BusinessArea
                </td>
                <td class="w15">
                    Amount
                </td>
            </tr>
            <asp:Repeater ID="rptItem" runat="server"  OnItemDataBound="rptItem_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <%--<td style="padding: 8px">
                            <%# Container.ItemIndex + 1 %>
                        </td>--%>
                        <td class="DrCr" style="text-align: left;padding: 8px">
                            Dr
                        </td>
                         <td class="FANO">
                            <%# Eval("AssetNo")%>
                        </td>
                        <td class="ExpenseType" style="text-align: left">
                            <asp:Label ID="lblExpenseTypeName" runat="server" />
                        </td>
                        <td>
                            <%# Eval("GLAccount")%>
                        </td>
                        <td>
                            <%# Eval("CostCenter")%>
                        </td>
                        <td class="ba ba2" >
                            <%# Eval("BusinessArea")%>
                        </td>
                        <td class="ItemAmount" style="text-align: left">
                            <%# Eval("ItemAmount")%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="height: 20px;">
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="catr">
               <%-- <td>
                </td>--%>
                <td style="padding: 10px; text-align: left" colspan="6" class="ba1">
                    Total paid amount
                </td>
                <td style="text-align: right" class="totalClaimedAmount">
                    1
                </td>
            </tr>
            <tr>
                <td style="height: 20px;" colspan="7">
                </td>
            </tr>
            <tr>
                <%--<td>
                </td>--%>
                <td style="padding: 10px; text-align: left" colspan="6" class="ba1">
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
            总金额(<asp:Label ID="lblAmountCurrency" runat="server" Text=""></asp:Label>)
        </td>
        <td style="text-align:right">
            <asp:Label ID="lbTotalAmount" runat="server" CssClass="TotalAmount"></asp:Label>
        </td>
    </tr>
   <%-- <tr>
        <td>
            Cash Advance<br />
            员工借款(RMB)
        </td>
        <td style="text-align:right">
            <asp:Label ID="lblCashAdvanceAmount" runat="server"></asp:Label>
        </td>
    </tr>--%>
    <%--<tr>
        <td>
            Payable to Employee/(Refund to Finance)<br />
            应付员工/财务应收(RMB)
        </td>
         <td style="text-align:right">
            <asp:Label ID="lblPreTotalAmount" runat="server"></asp:Label>
        </td>
    </tr>--%>
</table>
<asp:HiddenField ID="hfStatus" runat="server"  Value=""/>
<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        var $SAPNo = $("span.SAPNo");
        if ($SAPNo.text() == "") {
            $("tr.SAPNo").hide();
        }
        var $hfStatus = $('#<%=this.hfStatus.ClientID %>');
        if ($hfStatus.val() == "capex") {
            $("td.FANO").show();
            $("td.ba").hide();
            $("td.ba1").attr("colspan", "5");

        } else {
            $("td.FANO").hide();
            $("td.FANO").next().attr("colspan", "2");
            $("td.lblCRText").prev().hide();
            $("td.lblCRText").attr("colspan", "2");
        }

        $("td.ba2").each(function () {
            if ($(this).html() != "") {
                $(this).prev().html("");
            }
        });

        var $CurrencyTR = $("tr.CurrencyTR");
        var $lblCurrency = $("span.lblCurrency");
//        if ($lblCurrency.html() != "") {
//            $CurrencyTR.show();
//        }

        ChangeCr();
    });

    function ChangeCr() {
        var $expenseType = $("#expenseTypetable tr td.ExpenseType");
        var $totalClaimedAmount = $("#expenseTypetable tr.catr td.totalClaimedAmount");
        var amount = 0;
        $expenseType.each(function () {
            var type = $(this).text();
            var $DrCr = $(this).prev().prev();
            var $ItemAmount = $(this).parent().find("td.ItemAmount");
            if (type.indexOf("GRIR vendor code") != -1
                || type.indexOf("OP-Non-trade vendor") != -1) {
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
