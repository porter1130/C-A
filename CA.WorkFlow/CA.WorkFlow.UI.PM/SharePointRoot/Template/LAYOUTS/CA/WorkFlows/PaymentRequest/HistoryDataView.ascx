<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoryDataView.ascx.cs" Inherits="CA.WorkFlow.UI.PaymentRequest.HistoryDataView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .out_table input[type='radio']
    {
        border: 0 none red;
        float: left;
        width: auto;
        margin: 0;
        padding: 0;
    }
    
    .out_table_td
    {
        width: 212px;
    }
    
    .ms-RadioText
    {
    }
</style>
<div class="ContentDiv">

    <table class="ca-workflow-form-table form-table3 out_table" id="out_table">
        <tr>
            <td class="label align-center" colspan="6">
                Payment Request付款申请单
            </td>
        </tr>
        <tr>
            <td class="label out_table_td">
                部门<br />
                Dept
            </td>
            <td class="value out_table_td">
                <asp:Label ID="txtDept" runat="server"></asp:Label>
            </td>
            <td class="label out_table_td" colspan="2">
            </td>
            <td class="label out_table_td">
                申请人<br />
                Request by
            </td>
            <td class="value out_table_td">
                <asp:Label ID="txtApplicant" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="label">
                供应商编号<br />
                Vender Code
            </td>
            <td class="value">
                <asp:Label ID="txtVenderCode" runat="server"></asp:Label>
            </td>
            <td class="label">
                供应商名称<br />
                Vender Name
            </td>
            <td class="value" colspan="3">
                <asp:Label ID="txtVenderName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="label w15">
                银行名称<br />
                Bank Name
            </td>
            <td class="value w20">
                <asp:Label ID="txtBankName" runat="server" Width="101px"></asp:Label>
            </td>
            <td class="label">
                银行账号<br />
                Bank A/C
            </td>
            <td class="value">
                <asp:Label ID="txtBankAC" runat="server" Width="98px"></asp:Label>
            </td>
            <td class="label">
                Swift码<br />
                Swift Code
            </td>
            <td class="value">
                <asp:Label ID="txtSwiftCode" runat="server" Width="86px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="label">
                单据描述<br />
                Payment<br />
                Descriptions
            </td>
            <td class="value" colspan="5">
                <asp:Label ID="txtPaymentDesc" runat="server" TextMode="MultiLine" Width="555px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="6" style="padding-left: 100px; width: 100%;">
                <asp:RadioButtonList ID="radioExpenceType" runat="server" CssClass="out_table_radio ms-RadioText "
                    RepeatDirection="Horizontal" Width="293px">
                    <asp:ListItem Value="Asset" Selected="True"> 资产 Asset</asp:ListItem>
                    <asp:ListItem Value="Expense"> 费用 Expense</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="label TRBoldLine">
                成本中心<br />
                Cost Center
            </td>
            <td class="label TRBoldLine" colspan="2">
                <asp:Label ID="txtCostCenter" runat="server" Height="26px" Width="205px"></asp:Label>
            </td>
            <td class="label">
                分期付款<br />
                Installment Payment
            </td>
            <td class="label" colspan="2">
                <div class="RadioWidth">
                    <asp:RadioButtonList ID="radioInstallment" runat="server" RepeatDirection="Horizontal"
                        Width="120px" CssClass="ms-RadioText">
                        <asp:ListItem Selected="True">Yes</asp:ListItem>
                        <asp:ListItem Value="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="6">
                <table class="InsPay">
                    <tr>
                        <td class="label" align="center" width="12.5%">
                            总金额<br />
                            TotalAmount
                        </td>
                        <td class="label" align="center" width="12.5%">
                            <asp:Label ID="txtTotalAmount" runat="server" Width="76px"></asp:Label>
                        </td>
                        <td class="label" align="center" width="12.5%">
                            己付<br />
                            Paid Before
                        </td>
                        <td class="label" align="center" width="12.5%">
                            <asp:Label ID="txtPaidBefore" runat="server" Width="73px"></asp:Label>
                        </td>
                        <td class="label" align="center" width="12.5%">
                            本次付<br />
                            Paid this time
                        </td>
                        <td class="label" align="center" width="12.5%">
                            <asp:Label ID="txtPaidThisTime" runat="server" Width="89px"></asp:Label>
                        </td>
                        <td class="label" align="center" width="12.5%">
                            余额<br />
                            Balance
                        </td>
                        <td class="label" align="center" width="12.5%">
                            <asp:Label ID="txtBalance" runat="server" Width="67px"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="label TRBoldLine" colspan="2">
                己签署有付款金额的合同<br />
                Contract/PO with Paid Amount Sign
            </td>
            <td class="label align-center w15">
                <asp:RadioButtonList ID="radioContractPO" runat="server" RepeatDirection="Horizontal"
                    Width="120px" CssClass="ms-RadioText">
                    <asp:ListItem Selected="True"> Yes</asp:ListItem>
                    <asp:ListItem> No</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="label">
                合同编号<br />
                Contract/PO No
            </td>
            <td class="label align-center w30" colspan="2">
                <asp:Label ID="txtContractPO" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="2">
                己做系统订单<br />
                System Po has been done
            </td>
            <td class="label align-center w15">
                <asp:RadioButtonList ID="radioSystemPO" runat="server" RepeatDirection="Horizontal"
                    Width="120px" CssClass="ms-RadioText">
                    <asp:ListItem Selected="True"> Yes</asp:ListItem>
                    <asp:ListItem> No</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="label">
                系统订单编号<br />
                System PO NO
            </td>
            <td class="label align-center w30" colspan="2">
                <asp:Label ID="txtSystemPO" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="2">
                已接受合同描述的商品/服务<br />
                Goods/Services received as agreed
            </td>
            <td class="label w15" colspan="2">
                <asp:RadioButtonList ID="radioContractGR" runat="server" RepeatDirection="Horizontal"
                    Width="120px" CssClass="ms-RadioText">
                    <asp:ListItem Selected="True"> Yes</asp:ListItem>
                    <asp:ListItem> No</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                如没有，为什么要求付款：<br />
                If Not,why request to make payment:
                <br />
                <asp:Label ID="txtPaymentReason" runat="server" TextMode="MultiLine" Width="210px"></asp:Label>
            </td>
            <td class="label w15" colspan="2">
                <table border="0" style="height: 52px">
                    <tr>
                        <td>
                            己做系统收货货<br />
                            System GR Done
                        </td>
                        <td>
                            <asp:RadioButtonList ID="radioSystemGR" runat="server" Width="120px" CssClass="ms-RadioText"
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True"> Yes</asp:ListItem>
                                <asp:ListItem> No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="2">
                附原发票票<br />
                Original Invoice Attached
            </td>
            <td class="label" colspan="4">
                <asp:RadioButtonList ID="radioInvoice" runat="server" RepeatDirection="Horizontal"
                    Width="120px" CssClass="ms-RadioText">
                    <asp:ListItem Selected="True"> Yes</asp:ListItem>
                    <asp:ListItem> No</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="label">
                备注Remark<br />
            </td>
            <td class="label" colspan="5">
                <asp:Label ID="txtRemark" runat="server" TextMode="MultiLine" Width="585px" Height="30px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="label">
                附件 Attachment
            </td>
            <td class="label" colspan="5">
                
            </td>
        </tr>
    </table>
</div>
