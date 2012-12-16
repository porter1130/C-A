<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoryView.ascx.cs"
    Inherits="CA.WorkFlow.UI.PaymentRequest.HistoryView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="8" class="value align-center h30">
            <h3>
                &nbsp;Payment Request Information 分期付款信息</h3>
        </td>
    </tr>
    <tr>
        <td class="label align-center w5 h30">
            &nbsp;Index
        </td>
        <td class="label align-center w15 h30">
            &nbsp;PR ID
        </td>
        <td class="label align-center w15 h25">
            &nbsp;Total Amount
        </td>
        <td class="label align-center w10 h25">
            Paid Before
        </td>
        <td class="label align-center w15 h25">
            Paid this time
        </td>
        <td class="label align-center w10 h25">
            Balance
        </td>
        <td class="label align-center w10 h25">
            Status
        </td>
        <td class="label align-center w15 h25">
            Requested Date
        </td>
    </tr>
    <asp:Repeater ID="rptPRInfo" runat="server">
        <ItemTemplate>
            <tr>
                <td class="label align-center">
                    <%#Eval("PaidInd")%>
                </td>
                <td class="label align-center h30">
                    <a href='<%= ResolveClientUrl("~/Lists/PaymentRequestItems/DispForm.aspx?ID=")%><%#Eval("ID")%>'>
                        <%#Eval("SubPRNo")%></a>
                </td>
                <td class="label align-center">
                    <%#Eval("TotalAmount")%>
                </td>
                <td class="label align-center">
                    <div style="margin-top:5px; margin-bottom:5px;">
                        <%#Eval("PaidBefore")%>
                        %
                    </div>
                    <div style="padding-top:5px; margin-bottom:5px; border-top: 1px solid #999;">
                        <%#Eval("PaidBeforeAmount")%></div>
                </td>
                <td class="label align-center">
                    <div style="margin-top:5px; margin-bottom:5px;">
                        <%#Eval("PaidThisTime")%>
                        %
                    </div>
                    <div style="padding-top:5px; margin-bottom:5px; border-top: 1px solid #999;">
                        <%#Eval("PaidThisTimeAmount")%></div>
                </td>
                <td class="label align-center">
                    <div style="padding-top:5px; padding-bottom:5px;">
                        <%#Eval("Balance")%>
                        %
                    </div>
                    <div style="padding-top:5px; margin-bottom:5px; border-top: 1px solid #999;">
                        <%#Eval("BalanceAmount")%></div>
                </td>
                <td class="label align-center">
                    <%#Eval("Status")%>
                </td>
                <td class="label align-center">
                    <%#Eval("SubmitDate", "{0:d}")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
