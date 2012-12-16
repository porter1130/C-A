<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI._Layouts.CA.WorkFlows.CompanyCardClaim.DataEdit" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    #tb_travelexpense
    {
        background-image: url("../images/background_white.JPG");
        background-repeat: repeat-y;
        background-position: right;
    }
    #tb_travelexpense td.td_expensetype
    {
        width: 15%;
    }
    #tb_travelexpense td.td_date
    {
        width: 16%;
    }
    #tb_travelexpense td.td_costcenter
    {
        width: 10%;
    }
    #tb_travelexpense td.td_originalamt
    {
        width: 8%;
    }
    #tb_travelexpense td.td_currency
    {
        width: 8%;
    }
    #tb_travelexpense td.td_exchrate
    {
        width: 8%;
    }
    #tb_travelexpense td.td_rmbamt
    {
        width: 8%;
    }
    #tb_travelexpense td.td_paidbycredit
    {
        width: 10%;
    }
    #tb_travelexpense td.td_companystd
    {
        width: 8%;
    }
    #tb_travelexpense td.td_specialapproval
    {
        width: 9%;
    }
    
    #tb_travelexpense input
    {
        width: 50px;
    }
    #tb_travelexpense td.td_purpose input
    {
        width: 500px;
    }
    #tb_travelexpense td.td_expensetype input
    {
        width: 72px;
    }
    
    #tb_travelexpense tr.tr_remark td.td_remark input
    {
        width: 500px;
    }
    #tb_travelexpense td.td_remark
    {
        display: none;
    }
    #tb_travelexpense td.td_paidbycredit input
    {
        width: 15px;
    }
    #tb_travelexpense td.td_specialapproval input
    {
        width: 15px;
    }
    #tb_travelexpense td.cc
    {
        padding-top: 6px;
        vertical-align: top;
    }
    
    select.width-fix
    {
        width: 60px;
        z-index: 1000;
    }
    select.expand
    {
        position: absolute;
        width: auto; /* Let the browser handle it. */
    }
    
    #boxes #dialog
    {
        background: url(../images/notice.png) no-repeat 0 0 transparent;
        width: 326px;
        height: 229px;
        padding: 50px 0 20px 25px;
    }
    #dialog table
    {
        margin-top: 70px;
    }
    #dialog select
    {
        margin-top: 20px;
        margin-left: 50px;
    }
    .form-table
    {
        border: none;
        margin: 10px 0px 10px 0px;
    }
    .tdlabel
    {
        border-bottom: none;
        border-right: none;
        padding: 5px;
    }
    .ca-workflow-form-table h3
    {
        padding: 5px;
    }
    .w60
    {
        width: 60%;
    }
    .form-table1
    {
        border-top: none;
    }
    .form-table2
    {
        margin-bottom: 0px;
    }
    .tdpadding
    {
        height: 40px;
    }
    .lable1
    {
        border-left: none;
        border-bottom: #ccc 1px solid;
        border-top: #ccc 1px solid;
        text-align: center;
    }
    .lable2
    {
        border-bottom: #ccc 1px solid;
        border-top: #ccc 1px solid;
        border-right: #ccc 1px solid;
        padding: 7px;
    }
    .ca-workflow-form-table input
    {
        border-bottom: #ccc 1px solid;
    }
    .lable2 input
    {
        border: none;
        padding: 0px;
        margin: 0px;
        width: 20%;
        background-color: white;
    }
    .lable4
    {
        border-bottom: #ccc 1px solid;
        border-top: #ccc 1px solid;
        border-right: #ccc 1px solid;
        padding: 7px;
        vertical-align: middle;
    }
    .lable4 input
    {
        border: #ccc 1px solid;
        padding: 4px;
        margin: 0px;
        margin-right: 5px;
        width: 60px;
        background-color: white;
        color: #3d3d3d;
        cursor: pointer;
    }
    .w80
    {
        width: 80%;
    }
    .w80 input
    {
        width: 45%;
    }
    .lable3
    {
        padding-left: 5px;
    }
    .tablebottom
    {
        margin-bottom: 0px;
    }
    .lable3
    {
        border-bottom: #ccc 1px solid;
        border-top: #ccc 1px solid;
        border-right: none;
        padding: 7px;
    }
    .w45
    {
        width: 45%;
    }
    .lable3 textarea{ overflow:hidden}
</style>
<table class="ca-workflow-form-table  form-table">
    <tr>
        <td class="tdlabel w5">
            日期<br />
            Date
        </td>
        <td class="tdlabel w25">
            <asp:TextBox ID="txtTitleDate" runat="server"></asp:TextBox>
        </td>
        <td class="tdlabel w10">
            文档编号<br />
            Doc.No
        </td>
        <td class="tdlabel w25">
            <asp:TextBox ID="txtDocNo" runat="server"></asp:TextBox>
        </td>
        <td class="tdlabel w10">
            SAP编号<br />
            SAP.No
        </td>
        <td class="tdlabel w25">
            <asp:TextBox ID="txtSAPNo" runat="server"></asp:TextBox>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table form-table2">
            <tr>
                <td colspan="6" class="value align-center">
                    <h3>
                        公司卡报销申请表<br />
                        Company Card Claim Form</h3>
                </td>
            </tr>
            <tr>
                <td class="label align-center w5">
                    公司<br />
                    COMPANY
                </td>
                <td class="label align-center w20">
                    <asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
                </td>
                <td class="label align-center w15">
                    部门<br />
                    DEPT
                </td>
                <td class="label align-center w20">
                    <asp:TextBox ID="txtDept" runat="server"></asp:TextBox>
                </td>
                <td class="label align-center w20">
                    申请人<br />
                    REQUESTEDBY
                </td>
                <td class="label align-center w20">
                    <asp:TextBox ID="txtRequestedBy" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label align-center w5">
                    月份<br />
                    Month
                </td>
                <td class="label align-center w20">
                    <asp:TextBox ID="txtMonth" runat="server"></asp:TextBox>
                </td>
                <td class="label align-center w15">
                    信用卡币种<br />
                    Currency
                </td>
                <td class="label align-center w20">
                    <asp:TextBox ID="txtCurrency" runat="server"></asp:TextBox>
                </td>
                <td class="label align-center w20">
                    信用卡账单金额<br />
                    Amount
                </td>
                <td class="label align-center w20">
                    <asp:TextBox ID="txtAmount1" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label align-center w5">
                    单据描述<br />
                    Description
                </td>
                <td colspan="3" class="lable3">
                    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
                </td>
            </tr>
        </table>
        <table class="ca-workflow-form-table form-table1 tablebottom">
            <tr>
                <td colspan="7" class="value align-center">
                    <h3>
                        Detail</h3>
                </td>
            </tr>
            <tr>
                <td class="label align-center w10">
                    序号<br />
                    No
                </td>
                <td class="label align-center w15">
                    日期<br />
                    Date
                </td>
                <td class="label align-center w15">
                    明细摘要及用途<br />
                    Purpose
                </td>
                <td class="label align-center w15">
                    成本中心<br />
                    CostCenter
                </td>
                <td class="label align-center w20">
                    费用类别<br />
                    Type
                </td>
                <td class="label align-center w10">
                    金额<br />
                    Amount
                </td>
                <td class="label align-center w15">
                    备注<br />
                    Remark
                </td>
            </tr>
            <tr>
                <td class="label align-center w10">
                    <asp:TextBox ID="txtNo" runat="server"></asp:TextBox>
                </td>
                <td class="label align-center w15">
                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                </td>
                <td class="label align-center w15">
                    <asp:TextBox ID="txtPurpose" runat="server"></asp:TextBox>
                </td>
                <td class="lable2 align-center w15">
                    <asp:DropDownList ID="dplCostCenter" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="12201-D11C" Value="12201-D11C"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="lable2 align-center w20">
                    <asp:DropDownList ID="dplType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Office Stationery" Value="Office Stationery"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="label align-center w10">
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                </td>
                <td class="label align-center w15">
                    <asp:TextBox ID="txtRemark" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="lable3">
                </td>
                <td class="label align-center">
                    总计<br />
                    Total
                </td>
                <td class="label">
                    <asp:TextBox ID="txtTotal" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="label">
                    总金额<br />
                    Total Amount
                </td>
                <td class="lable3 align-center">
                    <asp:TextBox ID="txtTotalAmount" runat="server"></asp:TextBox>
                </td>
                <td class="label align-center">
                </td>
                <td class="label align-center">
                    付款方式<br />
                    Term
                </td>
                <td colspan="2" class="lable2 align-center">
                    <asp:DropDownList ID="dplTerm" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="转账(Transfer)" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="label">
                    应付金额<br />
                    Amount Due
                </td>
                <td class="lable3 align-center">
                    <asp:TextBox ID="txtAmountDue" runat="server"></asp:TextBox>
                </td>
                <td colspan="4">
                </td>
            </tr>
        </table>
        <table class="ca-workflow-form-table form-table1">
            <tr>
                <td colspan="4" class="value align-center">
                    <h3>
                        附件</h3>
                </td>
            </tr>
            <tr>
                <td class="label w15">
                    附件检查<br />
                    Doc.Check
                </td>
                <td class="lable2 w25">
                    <asp:DropDownList ID="dplDocCheck" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Original Invoice" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="label w15 align-center">
                    备注<br />
                    Remark
                </td>
                <td class="label w45">
                    <asp:TextBox ID="txtRemark1" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">
                    附件<br />
                    Attachment
                </td>
                <td class="label" colspan="3">
                    <asp:TextBox ID="txtAttachment" runat="server" Width="280px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">
                    四级经理
                </td>
                <td class="lable4">
                    <asp:Button ID="Button1" runat="server" Text="Approve" />
                    <asp:Button ID="Button2" runat="server" Text="Reject" />
                </td>
                <td class="label" colspan="2">
                    Remark
                    <asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                    <asp:TextBox ID="TextBox2" runat="server" Width="60px"></asp:TextBox>
                    <asp:TextBox ID="TextBox3" runat="server" Width="60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="label" align="center">
                    <h3>
                        Inancial</h3>
                </td>
            </tr>
            <tr>
                <td class="lable3 align-center">
                    Remark
                </td>
                <td class="lable3 align-center">
                    <asp:TextBox ID="TextBox4" runat="server" TextMode="MultiLine" Height="50px" Width="200px"></asp:TextBox>
                </td>
                <td class="lable3 align-center">
                    Reference
                </td>
                <td class="lable3 align-center" >
                    <asp:TextBox ID="TextBox5" runat="server" TextMode="MultiLine" Height="50px" Width="200px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
