<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.TE.DataView" %>
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
    
    #tb_travelexpense td.td_subtotal
    {
        text-align: right;
        padding-right: 10px;
        padding-top: 5px;
    }
    
    #tb_travelexpense td.td_hotelsubtotal, .td_mealsubtotal, td_transsubtotal, td_samplesubtotal, td_otherssubtotal
    {
        padding-top: 5px;
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
    #table_pendingform .radiobuttonlist input
    {
        width: 20px;
    }
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td class="label align-center w25">
            Workflow Number:
        </td>
        <td class="value">
            <QFL:FormField ID="ffWorkflowNo" runat="server" FieldName="Title" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td class="label align-center w25">
            Applicant
        </td>
        <td class="value">
            <QFL:FormField ID="ffApplicant" runat="server" FieldName="Applicant" ControlMode="Display">
            </QFL:FormField>
        </td>
        <%--<td class="value">
            <asp:Button ID="btnFindTR" runat="server" OnClick="btnFindTR_Click" Text="Find" CssClass="form-button" />
        </td>--%>
    </tr>
</table>
<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table">
            <tr>
                <td colspan="4" class="value align-center">
                    <h3>
                        Employee Information 员工信息</h3>
                </td>
            </tr>
            <tr>
                <td class="label align-center w25">
                    Chinese Name
                    <br />
                    中文姓名
                </td>
                <td class="label align-center w25">
                    English Name<br />
                    英文姓名
                </td>
                <td class="label align-center w25">
                    ID/Passport No.<br />
                    身份证/护照号码
                </td>
                <td class="value align-center w25">
                    Department
                    <br />
                    部门
                </td>
            </tr>
            <tr>
                <td class="label align-center w25">
                    <QFL:FormField ID="ffChineseName" runat="server" FieldName="ChineseName" ControlMode="Display">
                    </QFL:FormField>
                </td>
                <td class="label align-center w25">
                    <QFL:FormField ID="ffEnglishName" runat="server" FieldName="EnglishName" ControlMode="Display">
                    </QFL:FormField>
                </td>
                <td class="label align-center w25">
                    <QFL:FormField ID="ffIDNumber" runat="server" FieldName="IDNumber" ControlMode="Display">
                    </QFL:FormField>
                </td>
                <td class="label align-center w25">
                    <QFL:FormField ID="ffDepartment" runat="server" FieldName="Department" ControlMode="Display">
                    </QFL:FormField>
                </td>
            </tr>
            <tr class="last">
                <td colspan="4">
                    <table class="inner-table">
                        <tr>
                            <td class="label align-center">
                                Mobile Phone No.<br />
                                手机号码
                            </td>
                            <td class="label align-center w15">
                                <QFL:FormField ID="ffMobile" runat="server" FieldName="Mobile" ControlMode="Display">
                                </QFL:FormField>
                            </td>
                            <td class="label align-center">
                                Office Ext. No.<br />
                                分机号码
                            </td>
                            <td class="value align-center w15">
                                <QFL:FormField ID="ffOfficeExt" runat="server" FieldName="OfficeExt" ControlMode="Display">
                                </QFL:FormField>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<table class="inner-table ca-workflow-form-table" id="tb_travelexpense" style="width: 850px;">
    <tr>
        <td class="value align-center" colspan="10">
            <h3>
                Travel Expense Claim 出差费用报销
            </h3>
        </td>
    </tr>
    <tr>
        <td class="label">
            Purpose of business trip
        </td>
        <td class="value td_purpose" colspan="9">
            <QFL:FormField ID="ffPurpose" runat="server" FieldName="Purpose" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td class="label align-center td_expensetype">
            Expense Type
        </td>
        <td class="label align-center td_date">
            Date
            <%--<table class="inner-table" style="height:71px;">
                        <tr>
                            <td colspan="2" class="label align-center">
                                Date
                            </td>
                        </tr>
                        <tr>
                            <td class="label align-center w50">
                                From
                            </td>
                            <td class="label align-center w50">
                                To
                            </td>
                        </tr>
                    </table>--%>
        </td>
        <td class="label align-center td_costcenter">
            Cost
            <br />
            Center
        </td>
        <td class="label align-center td_originalamt">
            Original
            <br />
            Amt
        </td>
        <td class="label align-center td_currency">
            Currency
        </td>
        <td class="label align-center td_exchrate">
            Exch
            <br />
            Rate
        </td>
        <td class="label align-center td_rmbamt">
            Claim
            <br />
            Amt
        </td>
        <td class="label align-center td_paidbycredit">
            Paid by Co
            <br />
            Credit Card
        </td>
        <td class="label align-center td_companystd">
            Company
            <br />
            Std
        </td>
        <td class="value align-center td_specialapproval">
            Special
            <br />
            Approval
        </td>
    </tr>
    <tr>
        <td colspan="10">
            <table class="inner-table">
                <tr style="width: inherit;">
                    <td class="value align-center" colspan="10" style="padding-left: 0; padding-right: 0;">
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left;">
                                    Hotel
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:Repeater ID="rptHotel" runat="server">
                    <ItemTemplate>
                        <tr style="width: inherit;">
                            <td class="label align-center td_expensetype" style="padding-left: 0; padding-right: 0;">
                                <table style="width: 100%">
                                    <tr>
                                        <td class="align-center">
                                            <%# Eval("ExpenseDetail")%>
                                            <asp:HiddenField ID="ItemID" runat="server" Value='<%#Eval("ID")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="label align-center td_date" style="padding: 0;">
                                <table class="inner-table">
                                    <tr style="width: inherit;">
                                        <td class="value align-center w5">
                                            From
                                        </td>
                                        <td class="align-center">
                                            <%# Eval("TravelDateFrom", "{0:d}")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="value align-center w5" style="border-bottom: none;">
                                            To
                                        </td>
                                        <td class="align-center">
                                            <%# Eval("TravelDateTo", "{0:d}")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="label align-center td_costcenter cc">
                                <%# Eval("CostCenter")%>
                            </td>
                            <td class="label align-center td_originalamt">
                                <%# Eval("OriginalAmt")%>
                            </td>
                            <td class="label align-center td_currency">
                                <%# Eval("Currency")%>&nbsp;<%#Eval("OtherCurrency") %>
                            </td>
                            <td class="label align-center td_exchrate">
                                <%# Eval("ExchRate")%>
                            </td>
                            <td class="label align-center td_rmbamt td_hotelrmb">
                                <asp:Label ID="lblRmbAmt" runat="server" Text='<%# Eval("RmbAmt")%>' />
                                <asp:Label ID="lblApprovedRmbAmt" runat="server" Text='<%# Eval("ApprovedRmbAmt")%>' />
                                <asp:HiddenField ID="hidRmbAmt" runat="server" Value='<%# Eval("ApprovedRmbAmt")%>' />
                            </td>
                            <td class="label align-center td_paidbycredit">
                                <asp:CheckBox ID="cbPaidByCredit" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("IsPaidByCredit").ToString()))%>' />
                            </td>
                            <td class="label align-center td_companystd">
                                <%# Eval("CompanyStandards")%><asp:HiddenField ID="hidCompanyStandards" runat="server"
                                    Value='<%# Eval("CompanyStandards")%>' />
                            </td>
                            <td class="value align-center td_specialapproval">
                                <asp:CheckBox ID="cbSpecialApprove" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("SpecialApprove").ToString()))%>' />
                                <asp:CheckBox ID="cbSpecialApproved" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("SpecialApproved").ToString()))%>'
                                    onclick="CheckSpecialApproved(this)" CssClass="hidden" />
                                <asp:DropDownList ID="ddlSpecialApprove" runat="server" onchange='SelectSpecialApprove(this)'
                                    CssClass="hidden">
                                    <asp:ListItem Value="-1" Text="" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1">Approve</asp:ListItem>
                                    <asp:ListItem Value="0">Reject</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="tr_remark">
                            <td class="value align-center td_remark">
                                Remark:
                            </td>
                            <td class="value td_remark" colspan="9">
                                <span>
                                    <%# Eval("Remark")%></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
    </tr>
    <tr class="tr_subtotal">
        <td colspan="6" class="td_subtotal">
            Sub Total:
        </td>
        <td class="td_hotelsubtotal" colspan="4">
            <QFL:FormField ID="ffHotelSubTotal" runat="server" FieldName="HotelSubTotal">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="10" style="height: 5px;">
        </td>
    </tr>
    <tr>
        <td colspan="10">
            <table id="tb_mealallowance" class="inner-table" style="width: 100%;">
                <tr style="width: inherit;">
                    <td class="value align-center w15" colspan="10" style="padding-left: 0; padding-right: 0;">
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left;">
                                    Meal Allowance
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:Repeater ID="rptMeal" runat="server">
                    <ItemTemplate>
                        <tr style="width: inherit;">
                            <td class="label align-center td_expensetype" style="padding-left: 0; padding-right: 0;">
                                <table style="width: 100%">
                                    <tr>
                                        <td class="align-center">
                                            <%# Eval("ExpenseDetail")%>
                                            <asp:HiddenField ID="ItemID" runat="server" Value='<%#Eval("ID")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="label align-center td_date" style="padding: 0;">
                                <%# Eval("Date")%>
                            </td>
                            <td class="label align-center td_costcenter cc">
                                <%# Eval("CostCenter")%>
                            </td>
                            <td class="label align-center td_originalamt">
                                <%# Eval("OriginalAmt")%>
                            </td>
                            <td class="label align-center td_currency">
                                <%# Eval("Currency")%>&nbsp;<%#Eval("OtherCurrency") %>
                            </td>
                            <td class="label align-center td_exchrate">
                                <%# Eval("ExchRate")%>
                            </td>
                            <td class="label align-center td_rmbamt td_mealrmb">
                                <asp:Label ID="lblRmbAmt" runat="server" Text='<%# Eval("RmbAmt")%>' />
                                <asp:Label ID="lblApprovedRmbAmt" runat="server" Text='<%# Eval("ApprovedRmbAmt")%>' />
                                <asp:HiddenField ID="hidRmbAmt" runat="server" Value='<%# Eval("ApprovedRmbAmt")%>' />
                            </td>
                            <td class="label align-center td_paidbycredit">
                                <asp:CheckBox ID="cbPaidByCredit" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("IsPaidByCredit").ToString()))%>' />
                            </td>
                            <td class="label align-center td_companystd">
                                <%# Eval("CompanyStandards")%><asp:HiddenField ID="hidCompanyStandards" runat="server"
                                    Value='<%# Eval("CompanyStandards")%>' />
                            </td>
                            <td class="value align-center td_specialapproval">
                                <asp:CheckBox ID="cbSpecialApprove" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("SpecialApprove").ToString()))%>' />
                                <asp:CheckBox ID="cbSpecialApproved" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("SpecialApproved").ToString()))%>'
                                    onclick="CheckSpecialApproved(this)" CssClass="hidden" />
                                <asp:DropDownList ID="ddlSpecialApprove" runat="server" onchange='SelectSpecialApprove(this)'
                                    CssClass="hidden">
                                    <asp:ListItem Value="-1" Text="" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1">Approve</asp:ListItem>
                                    <asp:ListItem Value="0">Reject</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="tr_remark">
                            <td class="value align-center td_remark">
                                Remark:
                            </td>
                            <td class="value td_remark" colspan="9">
                                <span>
                                    <%# Eval("Remark")%></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
    </tr>
    <tr class="tr_subtotal">
        <td colspan="6" class="td_subtotal">
            Sub Total:
        </td>
        <td class="td_mealsubtotal" colspan="4">
            <QFL:FormField ID="ffMealSubTotal" runat="server" FieldName="MealSubTotal">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="10" style="height: 5px;">
        </td>
    </tr>
    <tr>
        <td colspan="10">
            <table class="inner-table">
                <tr style="width: inherit;">
                    <td class="value align-center w15" colspan="10" style="padding-left: 0; padding-right: 0;">
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left;">
                                    Transportation
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:Repeater ID="rptTrans" runat="server">
                    <ItemTemplate>
                        <tr style="width: inherit;">
                            <td class="label align-center td_expensetype" style="padding-left: 0; padding-right: 0;">
                                <table style="width: 100%">
                                    <tr>
                                        <td class="align-center">
                                            <%# Eval("ExpenseDetail")%>
                                            <asp:HiddenField ID="ItemID" runat="server" Value='<%#Eval("ID")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="label align-center td_date" style="padding: 0;">
                                <%# Eval("Date")%>
                            </td>
                            <td class="label align-center td_costcenter cc">
                                <%# Eval("CostCenter")%>
                            </td>
                            <td class="label align-center td_originalamt">
                                <%# Eval("OriginalAmt")%>
                            </td>
                            <td class="label align-center td_currency">
                                <%# Eval("Currency")%>&nbsp;<%#Eval("OtherCurrency") %>
                            </td>
                            <td class="label align-center td_exchrate">
                                <%# Eval("ExchRate")%>
                            </td>
                            <td class="label align-center td_rmbamt td_transrmb">
                                <asp:Label ID="lblRmbAmt" runat="server" Text='<%# Eval("RmbAmt")%>' />
                                <asp:Label ID="lblApprovedRmbAmt" runat="server" Text='<%# Eval("ApprovedRmbAmt")%>' />
                                <asp:HiddenField ID="hidRmbAmt" runat="server" Value='<%# Eval("ApprovedRmbAmt")%>' />
                            </td>
                            <td class="label align-center td_paidbycredit">
                                <asp:CheckBox ID="cbPaidByCredit" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("IsPaidByCredit").ToString()))%>' />
                            </td>
                            <td class="label align-center td_companystd">
                                <%# Eval("CompanyStandards")%><asp:HiddenField ID="hidCompanyStandards" runat="server"
                                    Value='<%# Eval("CompanyStandards")%>' />
                            </td>
                            <td class="value align-center td_specialapproval">
                                <asp:CheckBox ID="cbSpecialApprove" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("SpecialApprove").ToString()))%>' />
                                <asp:CheckBox ID="cbSpecialApproved" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("SpecialApproved").ToString()))%>'
                                    onclick="CheckSpecialApproved(this)" />
                            </td>
                        </tr>
                        <tr class="tr_remark">
                            <td class="value align-center td_remark">
                                Remark:
                            </td>
                            <td class="value td_remark" colspan="9">
                                <span>
                                    <%# Eval("Remark")%></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
    </tr>
    <tr class="tr_subtotal">
        <td colspan="6" class="td_subtotal">
            Sub Total:
        </td>
        <td class="td_transsubtotal" colspan="4">
            <QFL:FormField ID="ffTransSubTotal" runat="server" FieldName="TransSubTotal">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="10" style="height: 5px;">
        </td>
    </tr>
    <tr>
        <td colspan="10">
            <table class="inner-table">
                <tr style="width: inherit;">
                    <td class="value align-center w15" colspan="10" style="padding-left: 0; padding-right: 0;">
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left;">
                                    Sample Purchase
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:Repeater ID="rptSample" runat="server">
                    <ItemTemplate>
                        <tr style="width: inherit;">
                            <td class="label align-center td_expensetype" style="padding-left: 0; padding-right: 0;">
                                <table style="width: 100%">
                                    <tr>
                                        <td class="align-center">
                                            <%# Eval("ExpenseDetail")%>
                                            <asp:HiddenField ID="ItemID" runat="server" Value='<%#Eval("ID")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="label align-center td_date" style="padding: 0;">
                                <%# Eval("Date")%>
                            </td>
                            <td class="label align-center td_costcenter cc">
                                <%# Eval("CostCenter")%>
                            </td>
                            <td class="label align-center td_originalamt">
                                <%# Eval("OriginalAmt")%>
                            </td>
                            <td class="label align-center td_currency">
                                <%# Eval("Currency")%>&nbsp;<%#Eval("OtherCurrency") %>
                            </td>
                            <td class="label align-center td_exchrate">
                                <%# Eval("ExchRate")%>
                            </td>
                            <td class="label align-center td_rmbamt td_samplermb">
                                <asp:Label ID="lblRmbAmt" runat="server" Text='<%# Eval("RmbAmt")%>' />
                                <asp:Label ID="lblApprovedRmbAmt" runat="server" Text='<%# Eval("ApprovedRmbAmt")%>' />
                                <asp:HiddenField ID="hidRmbAmt" runat="server" Value='<%# Eval("ApprovedRmbAmt")%>' />
                            </td>
                            <td class="label align-center td_paidbycredit">
                                <asp:CheckBox ID="cbPaidByCredit" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("IsPaidByCredit").ToString()))%>' />
                            </td>
                            <td class="label align-center td_companystd">
                                <%# Eval("CompanyStandards")%><asp:HiddenField ID="hidCompanyStandards" runat="server"
                                    Value='<%# Eval("CompanyStandards")%>' />
                            </td>
                            <td class="value align-center td_specialapproval">
                                <asp:CheckBox ID="cbSpecialApprove" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("SpecialApprove").ToString()))%>' />
                                <asp:CheckBox ID="cbSpecialApproved" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("SpecialApproved").ToString()))%>'
                                    onclick="CheckSpecialApproved(this)" />
                            </td>
                        </tr>
                        <tr class="tr_remark">
                            <td class="value align-center td_remark">
                                Remark:
                            </td>
                            <td class="value td_remark" colspan="9">
                                <span>
                                    <%# Eval("Remark")%></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
    </tr>
    <tr class="tr_subtotal">
        <td colspan="6" class="td_subtotal">
            Sub Total:
        </td>
        <td class="td_samplesubtotal" colspan="4">
            <QFL:FormField ID="ffSampleSubTotal" runat="server" FieldName="SampleSubTotal">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="10" style="height: 5px;">
        </td>
    </tr>
    <tr>
        <td colspan="10">
            <table class="inner-table">
                <tr style="width: inherit;">
                    <td class="value align-center w15" colspan="10" style="padding-left: 0; padding-right: 0;">
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left;">
                                    Others
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:Repeater ID="rptOthers" runat="server">
                    <ItemTemplate>
                        <tr style="width: inherit;">
                            <td class="label align-center td_expensetype" style="padding-left: 0; padding-right: 0;">
                                <table style="width: 100%">
                                    <tr>
                                        <td class="align-center">
                                            <%# Eval("ExpenseDetail")%>
                                            <asp:HiddenField ID="ItemID" runat="server" Value='<%#Eval("ID")%>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="label align-center td_date" style="padding: 0;">
                                <%# Eval("Date")%>
                            </td>
                            <td class="label align-center td_costcenter cc">
                                <%# Eval("CostCenter")%>
                            </td>
                            <td class="label align-center td_originalamt">
                                <%# Eval("OriginalAmt")%>
                            </td>
                            <td class="label align-center td_currency">
                                <%# Eval("Currency")%>&nbsp;<%#Eval("OtherCurrency") %>
                            </td>
                            <td class="label align-center td_exchrate">
                                <%# Eval("ExchRate")%>
                            </td>
                            <td class="label align-center td_rmbamt td_othersrmb">
                                <asp:Label ID="lblRmbAmt" runat="server" Text='<%# Eval("RmbAmt")%>' />
                                <asp:Label ID="lblApprovedRmbAmt" runat="server" Text='<%# Eval("ApprovedRmbAmt")%>' />
                                <asp:HiddenField ID="hidRmbAmt" runat="server" Value='<%# Eval("ApprovedRmbAmt")%>' />
                            </td>
                            <td class="label align-center td_paidbycredit">
                                <asp:CheckBox ID="cbPaidByCredit" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("IsPaidByCredit").ToString()))%>' />
                            </td>
                            <td class="label align-center td_companystd">
                                <%# Eval("CompanyStandards")%><asp:HiddenField ID="hidCompanyStandards" runat="server"
                                    Value='<%# Eval("CompanyStandards")%>' />
                            </td>
                            <td class="value align-center td_specialapproval">
                                <asp:CheckBox ID="cbSpecialApprove" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("SpecialApprove").ToString()))%>' />
                                <asp:CheckBox ID="cbSpecialApproved" runat="server" Checked='<%#Convert.ToBoolean(Int32.Parse(Eval("SpecialApproved").ToString()))%>'
                                    onclick="CheckSpecialApproved(this)" />
                            </td>
                        </tr>
                        <tr class="tr_remark">
                            <td class="value align-center td_remark">
                                Remark:
                            </td>
                            <td class="value td_remark" colspan="9">
                                <span>
                                    <%# Eval("Remark")%></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
    </tr>
    <tr class="tr_subtotal">
        <td colspan="6" class="td_subtotal">
            Sub Total:
        </td>
        <td class="td_otherssubtotal" colspan="4">
            <QFL:FormField ID="ffOthersSubTotal" runat="server" FieldName="OthersSubTotal">
            </QFL:FormField>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table" id="table_claimsummary">
    <tr>
        <td class="value align-center">
            <h3>
                Claim Summary</h3>
        </td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td colspan="2" style="text-align: right; padding-right: 10px; width: 600px;">
                        Total
                    </td>
                    <td>
                        <QFL:FormField ID="ffTotalCost" runat="server" FieldName="TotalCost">
                        </QFL:FormField>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right; padding-right: 10px; width: 600px;">
                        Cash Advance
                    </td>
                    <td>
                        <QFL:FormField ID="ffCashAdvanced" runat="server" FieldName="CashAdvanced">
                        </QFL:FormField>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right; padding-right: 10px; width: 600px;">
                        Paid by Co Credit Card
                    </td>
                    <td>
                        <QFL:FormField ID="ffPaidByCreditCard" runat="server" FieldName="PaidByCreditCard">
                        </QFL:FormField>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right; padding-right: 10px; width: 600px;">
                        Payable to Employee/(Refund to Finance)
                    </td>
                    <td>
                        <QFL:FormField ID="ffNetPayable" runat="server" FieldName="NetPayable">
                        </QFL:FormField>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right; padding-right: 10px; width: 600px;">
                        Approved Budget less Flight Charges
                    </td>
                    <td>
                        <QFL:FormField ID="ffTotalExceptFlight" runat="server" FieldName="TotalExceptFlight">
                        </QFL:FormField>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right; padding-right: 10px; width: 600px;">
                        Lower/(Higher) than Budget
                    </td>
                    <td style="width: 20%;">
                        <QFL:FormField ID="ffComparedToApproved" runat="server" FieldName="ComparedToApproved">
                        </QFL:FormField>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="height: 5px;">
        </td>
    </tr>
    <tr>
        <td>
            Reasons for why travel expenses exceeded the travel budget (if applicable):
        </td>
    </tr>
    <tr>
        <td>
            <span style="color: #0066CC;">
                <QFL:FormField ID="ffReasons" runat="server" FieldName="Reasons" ControlMode="Display">
                </QFL:FormField>
            </span>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4" class="value align-center">
            <h3>
                Finance Section</h3>
        </td>
    </tr>
    <tr>
        <td class="w25">
            Supporting submitted
        </td>
        <td class="w25">
            <QFL:FormField ID="ffSupportingSubmitted" runat="server" FieldName="SupportingSubmitted"
                ControlMode="Display">
            </QFL:FormField>
        </td>
        <td class="w25">
            Date of submission
        </td>
        <td class="w25">
            <QFL:FormField ID="ffSubmissionDate" runat="server" FieldName="SubmissionDate" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Attachment:
        </td>
        <td colspan="3" style="text-align: left;">
            <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Display">
            </QFL:FormAttachments>
        </td>
    </tr>
    <tr>
        <td>
            Finance Remark
        </td>
        <td colspan="3" style="text-align: left;">
            <QFL:FormField ID="ffFinanceRemark" runat="server" FieldName="FinanceRemark">
            </QFL:FormField>
        </td>
    </tr>
</table>
<table id="table_pendingform" class="ca-workflow-form-table hidden">
    <tr>
        <td colspan="4" style="position: relative">
            <div style="position: absolute; left: 0px; top: 0px; width: 679px; height: 448px;
                z-index: 10000; background-color: White; filter: Alpha(opacity=0.5); display: none"
                id="div_overlay">
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="4" class="value align-center">
            <h3>
                Your claim is pending/rejected due to :</h3>
        </td>
    </tr>
    <tr class="tr_fapiao">
        <td>
            Invoice
        </td>
        <td colspan="3" style="text-align: left;" class="td_fapiao">
            <asp:RadioButtonList ID="rblFapiao" runat="server" CssClass="radiobuttonlist radio"
                RepeatDirection="Vertical">
                <asp:ListItem Text="Invoice not attached" Value="Invoice not attached" />
                <asp:ListItem Text="Invoice amount not match/insufficient" Value="Invoice amount not match/insufficient" />
                <asp:ListItem Text="other reasons, please state" Value="other reasons, please state" />
            </asp:RadioButtonList>
            <asp:TextBox ID="txtFapiaoOtherReason" runat="server" CssClass="hidden"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="4" style="height: 5px;">
        </td>
    </tr>
    <tr class="tr_information">
        <td>
            Information
        </td>
        <td colspan="3" style="text-align: left;" class="td_information">
            <asp:RadioButtonList ID="rblInformation" runat="server" CssClass="radiobuttonlist radio"
                RepeatDirection="Vertical">
                <asp:ListItem Text="fill-in incorrect cost center" Value="fill-in incorrect cost center" />
                <asp:ListItem Text="fill-in wrong expense type" Value="fill-in wrong expense type" />
                <asp:ListItem Text="print-out form has no amount listed" Value="print-out form has no amount listed" />
                <asp:ListItem Text="other reasons, please state" Value="other reasons, please state" />
            </asp:RadioButtonList>
            <asp:TextBox ID="txtInformationOtherReason" runat="server" CssClass="hidden"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="4" style="height: 5px;">
        </td>
    </tr>
    <tr class="tr_claimedamt">
        <td>
            Claimed Amount
        </td>
        <td colspan="3" style="text-align: left;" class="td_claimedamt">
            <asp:RadioButtonList ID="rblClaimedAmt" runat="server" CssClass="radiobuttonlist radio"
                RepeatDirection="Vertical">
                <asp:ListItem Text="used incorrect exchange rate" Value="used incorrect exchange rate"></asp:ListItem>
                <asp:ListItem Text="other reasons, please state" Value="other reasons, please state" />
            </asp:RadioButtonList>
            <asp:TextBox ID="txtClaimedOtherReason" runat="server" CssClass="hidden"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="4" style="height: 5px;">
        </td>
    </tr>
    <tr class="tr_otherreasons">
        <td>
            Other reasons, please state
        </td>
        <td colspan="3" style="text-align: left;" class="td_otherreasons">
            <asp:TextBox ID="txtOtherReasons" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="4" style="height: 5px;">
        </td>
    </tr>
    <tr>
        <td colspan="4">
            Please contact Poppy Wang at ext. 6148 to resolve the above problem(s) before Finance
            can continue to process your claim form.
        </td>
    </tr>
</table>
<br />
<h3 class="p5">
    For your reference 供参考</h3>
<h3 class="p5">
    According to the Company Business Travel Policy 按照公司商务差旅政策：</h3>
<table class="ca-workflow-form-table">
    <tr>
        <td class="label align-center w15" rowspan="3">
            <h3>
                Location
                <br />
                地方</h3>
        </td>
        <td colspan="3" class="label align-center">
            <h3>
                China</h3>
        </td>
        <td colspan="6" class="value align-center">
            <h3>
                Oversea</h3>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Tier I City
        </td>
        <td class="label align-center">
            Tier II/III City
        </td>
        <td class="label align-center">
            HongKong
        </td>
        <td class="label align-center">
            FarEast
        </td>
        <td class="label align-center">
            USA
        </td>
        <td class="label align-center">
            Switzerland
        </td>
        <td class="label align-center">
            UK
        </td>
        <td class="label align-center">
            Europe & North Africa
        </td>
        <td class="value align-center">
            Central & South America
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            RMB
        </td>
        <td class="label align-center">
            RMB
        </td>
        <td class="label align-center">
            RMB
        </td>
        <td class="label align-center">
            EUR
        </td>
        <td class="label align-center">
            USD
        </td>
        <td class="label align-center">
            CHF
        </td>
        <td class="label align-center">
            GBP
        </td>
        <td class="label align-center">
            EUR
        </td>
        <td class="value align-center">
            USD
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Hotel
        </td>
        <td class="label align-center">
            700
        </td>
        <td class="label align-center">
            400
        </td>
        <td colspan="7" class="value align-center">
            On actual reimbursement
        </td>
    </tr>
    <tr>
        <td class="label align-right">
            Meal Allowance<br />
            Breakfast<br />
            Lunch<br />
            Dinner<br />
        </td>
        <td class="label align-center">
            <br />
            50<br />
            50<br />
            70<br />
        </td>
        <td class="label align-center">
            <br />
            50<br />
            50<br />
            70<br />
        </td>
        <td class="label align-center">
            <br />
            50<br />
            50<br />
            70<br />
        </td>
        <td class="label align-center">
            <br />
            20<br />
            10<br />
            20<br />
        </td>
        <td class="label align-center">
            <br />
            20<br />
            10<br />
            20<br />
        </td>
        <td class="label align-center">
            <br />
            30<br />
            15<br />
            30<br />
        </td>
        <td class="label align-center">
            <br />
            15<br />
            10<br />
            25<br />
        </td>
        <td class="label align-center">
            <br />
            15<br />
            10<br />
            30<br />
        </td>
        <td class="value align-center">
            <br />
            15<br />
            10<br />
            15<br />
        </td>
    </tr>
</table>
<div id="hidFields">
    <asp:HiddenField ID="hidHotelItemValue" runat="server" />
    <asp:HiddenField ID="hidMealItemValue" runat="server" />
    <asp:HiddenField ID="hidTransItemValue" runat="server" />
    <asp:HiddenField ID="hidSampleItemValue" runat="server" />
    <asp:HiddenField ID="hidOthersItemValue" runat="server" />
</div>
<script type="text/javascript">
    var totalCostExceptVehicle = 0;
    var ffPostfix = '_ctl00_ctl00_TextField';
    var radio_fapiao = '';
    var radio_information = '';
    var radio_claimAmt = '';

    $(function () {
        //Get total budget except vehicle cost for GetSummary function
        var compared = Escape($('#<%=this.ffComparedToApproved.ClientID %>' + ffPostfix).val(), /,/);
        var total = Escape($('#<%= this.ffTotalCost.ClientID %>' + ffPostfix).val(), /,/);
        totalCostExceptVehicle = parseFloat(total) + parseFloat(compared);

        RadioButtonListBind();
        SetDataViewStyle();

    });

    function SetDataViewStyle() {
        $('#table_claimsummary input').each(function () {
            $(this).attr('contentEditable', 'false');
        });
        $('#tb_travelexpense tr.tr_subtotal input').each(function () {
            $(this).attr('contentEditable', 'false');
            if ('<%=this.Mode %>' != 'Approve') {
                $(this).attr('style', 'color:black');
            }
        });

        $('#tb_travelexpense td.td_rmbamt').each(function () {

            if ('<%=this.Mode %>' == 'Approve') {
                $(this).find('span').attr('style', 'color:#0066CC');
                $(this).parents('tr').first().next().find('td.td_remark span').attr('style', 'color:#0066CC');
            }
            var $specialApproval = $(this).parents('tr').first().find('td.td_specialapproval select');
            var companyStandsValue = $(this).parents('tr').first().find('td.td_companystd').text();
            var rmbAmt = $(this).text();

            //            $specialApproval.hide();

            if (!ca.util.emptyString(companyStandsValue)
                && !ca.util.emptyString(rmbAmt)
                && !isNaN(companyStandsValue)
                && !isNaN(rmbAmt)
                && (parseFloat(rmbAmt) > parseFloat(companyStandsValue))) {
                $(this).find('span').attr('style', 'color:red');
                //$(this).parents('tr').first().find('td.td_specialapproval input').attr('checked', true);
                //$(this).nextAll('td.td_specialapproval').find('#span_specialapprove').text('Y');
                $specialApproval.show();
                $(this).parents('tr').first().next().find('td.td_remark').show();
            }

        });

        if ('<%=this.Step %>' == 'ConfirmTask') {
            $('#table_pendingform').show();
            $('#tb_travelexpense td.td_rmbamt').find('span').attr('style', 'color:black');
        }
        $('#table_pendingform :radio[checked=true]').click();
    }

    function RadioButtonListBind() {

        $('#<%=this.rblFapiao.ClientID %>').bind('click', function () {
            var value = $(this).find(':radio[checked=true]').val();
            if (value != radio_fapiao) {
                switch (value) {
                    case "other reasons, please state":
                        $('#<%=this.txtFapiaoOtherReason.ClientID %>').show();
                        break;
                    default:
                        $('#<%=this.txtFapiaoOtherReason.ClientID %>').hide();
                        break;
                }
                radio_fapiao = value;
            } else {
                ClearRadioButtonListSelection($(this));
                radio_fapiao = '';
                $('#<%=this.txtFapiaoOtherReason.ClientID %>').hide();
            }



        });
        $('#<%=this.rblInformation.ClientID %>').bind('click', function () {
            var value = $(this).find(':radio[checked=true]').val();
            if (value != radio_information) {
                switch (value) {
                    case "other reasons, please state":
                        $('#<%=this.txtInformationOtherReason.ClientID %>').show();
                        break;
                    default:
                        $('#<%=this.txtInformationOtherReason.ClientID %>').hide();
                        break;
                }
                radio_information = value;
            } else {
                ClearRadioButtonListSelection($(this));
                radio_information = '';
                $('#<%=this.txtInformationOtherReason.ClientID %>').hide();
            }
        });

        $('#<%=this.rblClaimedAmt.ClientID %>').bind('click', function () {
            var value = $(this).find(':radio[checked=true]').val();
            if (value != radio_claimAmt) {
                switch (value) {
                    case "other reasons, please state":
                        $('#<%=this.txtClaimedOtherReason.ClientID %>').show();
                        break;
                    default:
                        $('#<%=this.txtClaimedOtherReason.ClientID %>').hide();
                        break;
                }
                radio_claimAmt = value;
            } else {
                ClearRadioButtonListSelection($(this));
                radio_claimAmt = '';
                $('#<%=this.txtClaimedOtherReason.ClientID %>').hide();
            }

        });

    }

    function ClearRadioButtonListSelection($obj) {
        var options = $obj.find('input:radio');
        options.removeAttr('checked');
    }

    function SelectSpecialApprove(obj) {
        var preId = GetPreId($(obj).attr('id'), 'ddlSpecialApprove');
        CheckSpecialApproved($('#' + preId + 'cbSpecialApproved').get(0));
    }

    function CheckSpecialApproved(obj) {
        var preId = GetPreId($(obj).attr('id'), 'cbSpecialApproved');
        var companyStandsValue = $('#' + preId + 'hidCompanyStandards').val();
        var $rmbAmt = $(obj).parents('tr').first().find('td.td_rmbamt');
        var approvedRmbAmt = $('#' + preId + 'hidRmbAmt');
        var $temp = $('#' + preId + 'lblApprovedRmbAmt');
        //var isCheck = $(obj).attr('checked');

        switch ($('#' + preId + 'ddlSpecialApprove').val()) {
            case "1":
                $(obj).attr('checked', true);
                if ($temp.val() != "") {
                    approvedRmbAmt.val($temp.val());
                    $temp.text($temp.val());
                }
                SetDataViewStyle();
                break;
            case "0":
                $(obj).attr('checked', false);
                $temp.attr('value', $rmbAmt.text());
                approvedRmbAmt.val(companyStandsValue);
                $temp.text(companyStandsValue);
                $(obj).parents('tr').first().find('td.td_rmbamt').attr('style', '');
                SetDataViewStyle();
                break;
            default:
                break;
        }
        GetSummary();

    }

    function GetPreId(clientId, id) {
        return clientId.substring(0, clientId.length - id.length);
    }

    function commafy(num) {
        num = num + "";
        var tmpArr = num.split('.');
        var re = /(-?\d+)(\d{3})/
        while (re.test(tmpArr[0])) {
            tmpArr[0] = tmpArr[0].replace(re, "$1,$2")
        }
        return tmpArr.length >= 2 ? tmpArr[0] + '.' + tmpArr[1] : tmpArr[0];
    }

    function Escape(s, exp) {
        while (exp.test(s)) {
            s = s.replace(exp, "");
        }
        return s;
    }

    function GetSummary() {

        var total = 0;
        var paid = 0;
        //Get SubTotal
        var hotelSubTotal = 0;
        var mealSubTotal = 0;
        var transSubTotal = 0;
        var sampleSubTotal = 0;
        var othersSubTotal = 0;


        $('#tb_travelexpense td.td_hotelrmb').each(function () {
            var rmbamt = Escape($(this).text(), /,/);
            if (!ca.util.emptyString(rmbamt)) {
                hotelSubTotal += !isNaN(rmbamt) && rmbamt.length > 0 ? parseFloat(rmbamt) : 0;
            }
        });
        $('#tb_travelexpense td.td_mealrmb').each(function () {
            var rmbamt = Escape($(this).text(), /,/);
            if (!ca.util.emptyString(rmbamt)) {
                mealSubTotal += !isNaN(rmbamt) && rmbamt.length > 0 ? parseFloat(rmbamt) : 0;
            }
        });
        $('#tb_travelexpense td.td_transrmb').each(function () {
            var rmbamt = Escape($(this).text(), /,/);
            if (!ca.util.emptyString(rmbamt)) {
                transSubTotal += !isNaN(rmbamt) && rmbamt.length > 0 ? parseFloat(rmbamt) : 0;
            }
        });
        $('#tb_travelexpense td.td_samplermb').each(function () {
            var rmbamt = Escape($(this).text(), /,/);
            if (!ca.util.emptyString(rmbamt)) {
                sampleSubTotal += !isNaN(rmbamt) && rmbamt.length > 0 ? parseFloat(rmbamt) : 0;
            }
        });
        $('#tb_travelexpense td.td_othersrmb').each(function () {
            var rmbamt = Escape($(this).text(), /,/);
            if (!ca.util.emptyString(rmbamt)) {
                othersSubTotal += !isNaN(rmbamt) && rmbamt.length > 0 ? parseFloat(rmbamt) : 0;
            }
        });

        $('#<%=this.ffHotelSubTotal.ClientID %>' + ffPostfix).val(commafy(hotelSubTotal.toFixed(2)));
        $('#<%=this.ffMealSubTotal.ClientID %>' + ffPostfix).val(commafy(mealSubTotal.toFixed(2)));
        $('#<%=this.ffTransSubTotal.ClientID %>' + ffPostfix).val(commafy(transSubTotal.toFixed(2)));
        $('#<%=this.ffSampleSubTotal.ClientID %>' + ffPostfix).val(commafy(sampleSubTotal.toFixed(2)));
        $('#<%=this.ffOthersSubTotal.ClientID %>' + ffPostfix).val(commafy(othersSubTotal.toFixed(2)));

        //Get Total
        $('#tb_travelexpense td.td_rmbamt').each(function () {
            var rmbamt = Escape($(this).text(), /,/);
            if (!ca.util.emptyString(rmbamt)) {
                total += !isNaN(rmbamt) && rmbamt.length > 0 ? parseFloat(rmbamt) : 0;
            }
        });
        $('#<%= this.ffTotalCost.ClientID %>' + ffPostfix).val(commafy(parseFloat(total).toFixed(2)));


        //Get Net Payable
        var cashAdvance = Escape($('#<%=this.ffCashAdvanced.ClientID %>' + ffPostfix).val(), /,/);
        var paidByCard = Escape($('#<%=this.ffPaidByCreditCard.ClientID %>' + ffPostfix).val(), /,/);

        if (!isNaN(cashAdvance)
        && cashAdvance.length > 0) {
            paid = total - parseFloat(cashAdvance) - parseFloat(paidByCard);
        }
        $('#<%=this.ffNetPayable .ClientID%>' + ffPostfix).val(commafy(paid.toFixed(2)));
        if (paid < 0) {
            $('#<%=this.ffNetPayable.ClientID %>' + ffPostfix).attr('style', 'color:red');
        }
        else {
            $('#<%=this.ffNetPayable.ClientID %>' + ffPostfix).attr('style', '');
        }

        //Get Total travel expense compared to approved budget value
        var compared = 0;

        compared = totalCostExceptVehicle - total;
        $('#<%=this.ffComparedToApproved.ClientID %>' + ffPostfix).val(commafy((compared).toFixed(2)));
        if (compared < 0) {
            $('#<%=this.ffComparedToApproved.ClientID %>' + ffPostfix).attr('style', 'color:red');
        }
        else {
            $('#<%=this.ffComparedToApproved.ClientID %>' + ffPostfix).attr('style', '');
        }

    }
    function SetBorderWarn(obj) {
        obj.css('border', '1px solid red');
    }
    function ClearBorderWarn(obj) {
        obj.css('border', '');
        obj.css('border-bottom', '#999 1px solid');
    }
</script>
