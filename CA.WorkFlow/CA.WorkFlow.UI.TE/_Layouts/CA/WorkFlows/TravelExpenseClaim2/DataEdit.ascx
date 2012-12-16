<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.TE.DataEdit" %>
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
    #tb_travelexpense div.div_fuckie6
    {
        position: relative;
        z-index: 8999;
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
    #tb_travelexpense td.td_subtotal
    {
        text-align: right;
        padding-right: 10px;
    }
    
    select.width-fix
    {
        width: 60px;
        z-index: 1000;
    }
    select.expand
    {
        position: absolute;
        width: 270px; /* Let the browser handle it. */
    }
    #dialog input
    {
        font-size: 12px;
    }
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td class="label align-center w25">
            Applicant
        </td>
        <td class="value">
            <asp:Label ID="lblApplicant" runat="server"></asp:Label>
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
                    <asp:Label ID="lblChineseName" runat="server" />
                </td>
                <td class="label align-center w25">
                    <asp:Label ID="lblEnglishName" runat="server" />
                </td>
                <td class="label align-center w25">
                    <asp:Label ID="lblIDNumber" runat="server" />
                </td>
                <td class="label align-center w25">
                    <asp:Label ID="lblDepartment" runat="server" />
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
                                <asp:Label ID="lblMobile" runat="server" />
                            </td>
                            <td class="label align-center">
                                Office Ext. No.<br />
                                分机号码
                            </td>
                            <td class="value align-center w15">
                                <asp:Label ID="lblOfficeExt" runat="server" />
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
            <QFL:FormField ID="ffPurpose" runat="server" FieldName="Purpose">
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
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="inner-table">
                        <tr style="width: inherit;">
                            <td class="value align-center" colspan="10" style="padding-left: 0; padding-right: 0;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: left; width: 36px;">
                                            <asp:ImageButton runat="server" ID="btnAddHotel" OnClick="btnAddHotel_Click" ToolTip="Click to add the hotel information."
                                                ImageUrl="../images/pixelicious_001.png" Width="18" CssClass="img-button" />
                                        </td>
                                        <td style="text-align: left;">
                                            Hotel
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <asp:Repeater ID="rptHotel" runat="server" OnItemCommand="rptHotel_ItemCommand" OnItemDataBound="rptHotel_ItemDataBound">
                            <ItemTemplate>
                                <tr style="width: inherit;">
                                    <td class="label align-center td_expensetype" style="padding-left: 0; padding-right: 0;">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: left; width: 30px;">
                                                    <asp:ImageButton ID="btnDeleteHotel" CommandName="delete" ToolTip="Delete this hotel information."
                                                        runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="select_location" onchange="SelectLocation(this)">
                                                        <asp:ListItem Text="Select City" Selected="True" />
                                                        <asp:ListItem Text="Tier I City" Value="Tier I City" />
                                                        <asp:ListItem Text="Tier II/III City" Value="Tier II/III City" />
                                                        <asp:ListItem Text="HK/Oversea" Value="HK/Oversea" />
                                                    </asp:DropDownList>
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
                                                    <cc1:CADateTimeControl ID="dtFrom" runat="server" DateOnly="true" CssClassTextBox="w60 DateTimeControl"
                                                        OnValueChangeClientScript="DateValueChange(this)" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="value align-center w5">
                                                    To
                                                </td>
                                                <td class="align-center">
                                                    <cc1:CADateTimeControl ID="dtTo" runat="server" DateOnly="true" CssClassTextBox="w60 DateTimeControl"
                                                        OnValueChangeClientScript="DateValueChange(this)" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="label td_costcenter cc" style="padding-top: 12px; vertical-align: top;">
                                        <asp:DropDownList ID="ddlCostCenter" CssClass="width-fix" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="label align-center td_originalamt">
                                        <asp:TextBox ID="txtOriginalAmt" runat="server" />
                                    </td>
                                    <td class="label align-center td_currency">
                                        <asp:DropDownList ID="ddlCurrency" runat="server" onchange="SelectCurrency(this)">
                                            <asp:ListItem Text="RMB" Value="RMB" Selected="True" />
                                            <asp:ListItem Text="USD" Value="USD" />
                                            <asp:ListItem Text="GBP" Value="GBP" />
                                            <asp:ListItem Text="EUR" Value="EUR" />
                                            <asp:ListItem Text="AUD" Value="AUD" />
                                            <asp:ListItem Text="CHF" Value="CHF" />
                                            <asp:ListItem Text="CAD" Value="CAD" />
                                            <asp:ListItem Text="JPY" Value="JPY" />
                                            <asp:ListItem Text="HKD" Value="HKD" />
                                            <asp:ListItem Text="Others" Value="Others" />
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtOtherCurrency" runat="server" CssClass="hidden" />
                                    </td>
                                    <td class="label align-center td_exchrate">
                                        <asp:TextBox ID="txtExchRate" runat="server" />
                                    </td>
                                    <td class="label align-center td_rmbamt td_hotelrmb">
                                        <asp:TextBox ID="txtRmbAmt" runat="server" contentEditable="false" />
                                    </td>
                                    <td class="label align-center td_paidbycredit">
                                        <asp:CheckBox ID="cbPaidByCredit" runat="server" onclick="CheckBoxPaidByCredit(this)" />
                                    </td>
                                    <td class="label align-center td_companystd">
                                        <asp:Label ID="lblCompanyStandards" runat="server" />
                                        <asp:HiddenField ID="hidCompanyStandards" runat="server" />
                                    </td>
                                    <td class="value align-center td_specialapproval">
                                        <asp:Label ID="lblSpecialApprove" runat="server"></asp:Label>
                                        <asp:CheckBox ID="cbSpecialApprove" runat="server" CssClass="hidden" />
                                    </td>
                                </tr>
                                <tr class="tr_remark">
                                    <td class="value align-center td_remark">
                                        Remark:
                                    </td>
                                    <td class="value td_remark" colspan="9">
                                        <asp:TextBox ID="txtRemark" runat="server" value="Why exceeds company standard?"
                                            onclick="DisplayDefaultMessage(this,'Why exceeds company standard?')" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
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
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table id="tb_mealallowance" class="inner-table" style="width: 100%;">
                        <tr style="width: inherit;">
                            <td class="value align-center w15" colspan="10" style="padding-left: 0; padding-right: 0;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: left; width: 36px;">
                                            <asp:ImageButton runat="server" ID="btnAddMeal" OnClientClick="$('#dialog').dialog('open');return false;"
                                                ToolTip="Click to add the meal allowance information." ImageUrl="../images/pixelicious_001.png"
                                                Width="18" CssClass="img-button" />
                                        </td>
                                        <td style="text-align: left;">
                                            Meal Allowance
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <asp:Repeater ID="rptMeal" runat="server" OnItemCommand="rptMeal_ItemCommand" OnItemDataBound="rptMeal_ItemDataBound">
                            <ItemTemplate>
                                <tr style="width: inherit;">
                                    <td class="label align-center td_expensetype" style="padding-left: 0; padding-right: 0;">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: left; width: 30px;">
                                                    <asp:ImageButton ID="btnDeleteMeal" CommandName="delete" ToolTip="Delete this meal information."
                                                        runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:DropDownList ID="ddlMealType" runat="server" Width="90%" CssClass="select_mealtype">
                                                        <asp:ListItem Text="Breakfast" Value="Breakfast" Selected="True" />
                                                        <asp:ListItem Text="Lunch" Value="Lunch" />
                                                        <asp:ListItem Text="Dinner" Value="Dinner" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="label align-center td_date" style="padding: 0;">
                                        <cc1:CADateTimeControl ID="dtDate" runat="server" DateOnly="true" CssClassTextBox="w60 DateTimeControl" />
                                    </td>
                                    <td class="label td_costcenter cc">
                                        <asp:DropDownList ID="ddlCostCenter" CssClass="width-fix" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="label align-center td_originalamt">
                                        <asp:TextBox ID="txtOriginalAmt" runat="server" />
                                    </td>
                                    <td class="label align-center td_currency">
                                        <asp:DropDownList ID="ddlCurrency" runat="server" onchange="SelectCurrency(this)">
                                            <asp:ListItem Text="RMB" Value="RMB" Selected="True" />
                                            <asp:ListItem Text="USD" Value="USD" />
                                            <asp:ListItem Text="GBP" Value="GBP" />
                                            <asp:ListItem Text="EUR" Value="EUR" />
                                            <asp:ListItem Text="AUD" Value="AUD" />
                                            <asp:ListItem Text="CHF" Value="CHF" />
                                            <asp:ListItem Text="CAD" Value="CAD" />
                                            <asp:ListItem Text="JPY" Value="JPY" />
                                            <asp:ListItem Text="HKD" Value="HKD" />
                                            <asp:ListItem Text="Others" Value="Others" />
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtOtherCurrency" runat="server" CssClass="hidden" />
                                    </td>
                                    <td class="label align-center td_exchrate">
                                        <asp:TextBox ID="txtExchRate" runat="server" />
                                        <asp:HiddenField ID="hidMealItem" runat="server" />
                                    </td>
                                    <td class="label align-center td_rmbamt td_mealrmb">
                                        <asp:TextBox ID="txtRmbAmt" runat="server" contentEditable="false" />
                                    </td>
                                    <td class="label align-center td_paidbycredit">
                                        <asp:CheckBox ID="cbPaidByCredit" runat="server" onclick="CheckBoxPaidByCredit(this)" />
                                    </td>
                                    <td class="label align-center td_companystd">
                                        <asp:Label ID="lblCompanyStandards" runat="server" />
                                        <asp:HiddenField ID="hidCompanyStandards" runat="server" />
                                    </td>
                                    <td class="value align-center td_specialapproval">
                                        <asp:Label ID="lblSpecialApprove" runat="server"></asp:Label>
                                        <asp:CheckBox ID="cbSpecialApprove" runat="server" CssClass="hidden" />
                                    </td>
                                </tr>
                                <tr class="tr_remark">
                                    <td class="value align-center td_remark">
                                        Remark:
                                    </td>
                                    <td class="value td_remark" colspan="9">
                                        <asp:TextBox ID="txtRemark" runat="server" value="Why exceeds company standard?"
                                            onclick="DisplayDefaultMessage(this,'Why exceeds company standard?')" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSelect" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
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
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="inner-table">
                        <tr style="width: inherit;">
                            <td class="value align-center w15" colspan="10" style="padding-left: 0; padding-right: 0;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: left; width: 36px;">
                                            <asp:ImageButton runat="server" ID="btnAddTrans" OnClick="btnAddTrans_Click" ToolTip="Click to add the local transportation information."
                                                ImageUrl="../images/pixelicious_001.png" Width="18" CssClass="img-button" />
                                        </td>
                                        <td style="text-align: left;">
                                            Transportation
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <asp:Repeater ID="rptTrans" runat="server" OnItemCommand="rptTrans_ItemCommand" OnItemDataBound="rptTrans_ItemDataBound">
                            <ItemTemplate>
                                <tr style="width: inherit;">
                                    <td class="label align-center td_expensetype" style="padding-left: 0; padding-right: 0;">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <asp:ImageButton ID="btnDeleteTrans" CommandName="delete" ToolTip="Delete this local transportation information."
                                                        runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtExpenseDetail" runat="server" value="Purpose of local transportation"
                                                        onclick="this.value=''" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="label align-center td_date" style="padding: 0;">
                                        <cc1:CADateTimeControl ID="dtDate" runat="server" DateOnly="true" CssClassTextBox="w60 DateTimeControl" />
                                    </td>
                                    <td class="label td_costcenter cc">
                                        <asp:DropDownList ID="ddlCostCenter" CssClass="width-fix" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="label align-center td_originalamt">
                                        <asp:TextBox ID="txtOriginalAmt" runat="server" />
                                    </td>
                                    <td class="label align-center td_currency">
                                        <asp:DropDownList ID="ddlCurrency" runat="server" onchange="SelectCurrency(this)">
                                            <asp:ListItem Text="RMB" Value="RMB" Selected="True" />
                                            <asp:ListItem Text="USD" Value="USD" />
                                            <asp:ListItem Text="GBP" Value="GBP" />
                                            <asp:ListItem Text="EUR" Value="EUR" />
                                            <asp:ListItem Text="AUD" Value="AUD" />
                                            <asp:ListItem Text="CHF" Value="CHF" />
                                            <asp:ListItem Text="CAD" Value="CAD" />
                                            <asp:ListItem Text="JPY" Value="JPY" />
                                            <asp:ListItem Text="HKD" Value="HKD" />
                                            <asp:ListItem Text="Others" Value="Others" />
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtOtherCurrency" runat="server" CssClass="hidden" />
                                    </td>
                                    <td class="label align-center td_exchrate">
                                        <asp:TextBox ID="txtExchRate" runat="server" />
                                    </td>
                                    <td class="label align-center td_rmbamt td_transrmb">
                                        <asp:TextBox ID="txtRmbAmt" runat="server" contentEditable="false" />
                                    </td>
                                    <td class="label align-center td_paidbycredit">
                                        <asp:CheckBox ID="cbPaidByCredit" runat="server" onclick="CheckBoxPaidByCredit(this)" />
                                    </td>
                                    <td class="label align-center td_companystd">
                                        <asp:Label ID="lblCompanyStandards" runat="server" />
                                        <asp:HiddenField ID="hidCompanyStandards" runat="server" />
                                    </td>
                                    <td class="value align-center td_specialapproval">
                                        <asp:Label ID="lblSpecialApprove" runat="server"></asp:Label>
                                        <asp:CheckBox ID="cbSpecialApprove" runat="server" CssClass="hidden" />
                                    </td>
                                </tr>
                                <tr class="tr_remark">
                                    <td class="value align-center td_remark">
                                        Remark:
                                    </td>
                                    <td class="value td_remark" colspan="9">
                                        <asp:TextBox ID="txtRemark" runat="server" value="Why exceeds company standard?"
                                            onclick="DisplayDefaultMessage(this,'Why exceeds company standard?')" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
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
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="inner-table">
                        <tr style="width: inherit;">
                            <td class="value align-center w15" colspan="10" style="padding-left: 0; padding-right: 0;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: left; width: 36px;">
                                            <asp:ImageButton runat="server" ID="btnAddSample" OnClick="btnAddSample_Click" ToolTip="Click to add the sample purchase information."
                                                ImageUrl="../images/pixelicious_001.png" Width="18" CssClass="img-button" />
                                        </td>
                                        <td style="text-align: left;">
                                            Sample Purchase
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <asp:Repeater ID="rptSample" runat="server" OnItemCommand="rptSample_ItemCommand"
                            OnItemDataBound="rptSample_ItemDataBound">
                            <ItemTemplate>
                                <tr style="width: inherit;">
                                    <td class="label align-center td_expensetype" style="padding-left: 0; padding-right: 0;">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <asp:ImageButton ID="btnDeleteSample" CommandName="delete" ToolTip="Delete this sample information."
                                                        runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtExpenseDetail" runat="server" value="City" onclick="this.value=''" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="label align-center td_date" style="padding: 0;">
                                        <cc1:CADateTimeControl ID="dtDate" runat="server" DateOnly="true" CssClassTextBox="w60 DateTimeControl" />
                                    </td>
                                    <td class="label td_costcenter cc">
                                        <asp:DropDownList ID="ddlCostCenter" CssClass="width-fix" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="label align-center td_originalamt">
                                        <asp:TextBox ID="txtOriginalAmt" runat="server" />
                                    </td>
                                    <td class="label align-center td_currency">
                                        <asp:DropDownList ID="ddlCurrency" runat="server" onchange="SelectCurrency(this)">
                                            <asp:ListItem Text="RMB" Value="RMB" Selected="True" />
                                            <asp:ListItem Text="USD" Value="USD" />
                                            <asp:ListItem Text="GBP" Value="GBP" />
                                            <asp:ListItem Text="EUR" Value="EUR" />
                                            <asp:ListItem Text="AUD" Value="AUD" />
                                            <asp:ListItem Text="CHF" Value="CHF" />
                                            <asp:ListItem Text="CAD" Value="CAD" />
                                            <asp:ListItem Text="JPY" Value="JPY" />
                                            <asp:ListItem Text="HKD" Value="HKD" />
                                            <asp:ListItem Text="Others" Value="Others" />
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtOtherCurrency" runat="server" CssClass="hidden" />
                                    </td>
                                    <td class="label align-center td_exchrate">
                                        <asp:TextBox ID="txtExchRate" runat="server" />
                                    </td>
                                    <td class="label align-center td_rmbamt td_samplermb">
                                        <asp:TextBox ID="txtRmbAmt" runat="server" contentEditable="false" />
                                    </td>
                                    <td class="label align-center td_paidbycredit">
                                        <asp:CheckBox ID="cbPaidByCredit" runat="server" onclick="CheckBoxPaidByCredit(this)" />
                                    </td>
                                    <td class="label align-center td_companystd">
                                        <asp:Label ID="lblCompanyStandards" runat="server" />
                                        <asp:HiddenField ID="hidCompanyStandards" runat="server" />
                                    </td>
                                    <td class="value align-center td_specialapproval">
                                        <asp:Label ID="lblSpecialApprove" runat="server"></asp:Label>
                                        <asp:CheckBox ID="cbSpecialApprove" runat="server" CssClass="hidden" />
                                    </td>
                                </tr>
                                <tr class="tr_remark">
                                    <td class="value align-center td_remark">
                                        Remark:
                                    </td>
                                    <td class="value td_remark" colspan="9">
                                        <asp:TextBox ID="txtRemark" runat="server" value="Why exceeds company standard?"
                                            onclick="DisplayDefaultMessage(this,'Why exceeds company standard?')" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
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
            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="inner-table">
                        <tr style="width: inherit;">
                            <td class="value align-center w15" colspan="10" style="padding-left: 0; padding-right: 0;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: left; width: 36px;">
                                            <asp:ImageButton runat="server" ID="btnAddOthers" OnClick="btnAddOthers_Click" ToolTip="Click to add the others information."
                                                ImageUrl="../images/pixelicious_001.png" Width="18" CssClass="img-button" />
                                        </td>
                                        <td style="text-align: left;">
                                            Others
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <asp:Repeater ID="rptOthers" runat="server" OnItemCommand="rptOthers_ItemCommand"
                            OnItemDataBound="rptOthers_ItemDataBound">
                            <ItemTemplate>
                                <tr style="width: inherit;">
                                    <td class="label align-center td_expensetype" style="padding-left: 0; padding-right: 0;">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <asp:ImageButton ID="btnDeleteOthers" CommandName="delete" ToolTip="Delete this others information."
                                                        runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:TextBox ID="txtExpenseDetail" runat="server" value="Please specify" onclick="this.value=''" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="label align-center td_date" style="padding: 0;">
                                        <cc1:CADateTimeControl ID="dtDate" runat="server" DateOnly="true" CssClassTextBox="w60 DateTimeControl" />
                                    </td>
                                    <td class="label td_costcenter cc">
                                        <asp:DropDownList ID="ddlCostCenter" CssClass="width-fix" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="label align-center td_originalamt">
                                        <asp:TextBox ID="txtOriginalAmt" runat="server" />
                                    </td>
                                    <td class="label align-center td_currency">
                                        <asp:DropDownList ID="ddlCurrency" runat="server" onchange="SelectCurrency(this)">
                                            <asp:ListItem Text="RMB" Value="RMB" Selected="True" />
                                            <asp:ListItem Text="USD" Value="USD" />
                                            <asp:ListItem Text="GBP" Value="GBP" />
                                            <asp:ListItem Text="EUR" Value="EUR" />
                                            <asp:ListItem Text="AUD" Value="AUD" />
                                            <asp:ListItem Text="CHF" Value="CHF" />
                                            <asp:ListItem Text="CAD" Value="CAD" />
                                            <asp:ListItem Text="JPY" Value="JPY" />
                                            <asp:ListItem Text="HKD" Value="HKD" />
                                            <asp:ListItem Text="Others" Value="Others" />
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtOtherCurrency" runat="server" CssClass="hidden" />
                                    </td>
                                    <td class="label align-center td_exchrate">
                                        <asp:TextBox ID="txtExchRate" runat="server" />
                                    </td>
                                    <td class="label align-center td_rmbamt td_othersrmb">
                                        <asp:TextBox ID="txtRmbAmt" runat="server" contentEditable="false" />
                                    </td>
                                    <td class="label align-center td_paidbycredit">
                                        <asp:CheckBox ID="cbPaidByCredit" runat="server" onclick="CheckBoxPaidByCredit(this)" />
                                    </td>
                                    <td class="label align-center td_companystd">
                                        <asp:Label ID="lblCompanyStandards" runat="server" />
                                        <asp:HiddenField ID="hidCompanyStandards" runat="server" />
                                    </td>
                                    <td class="value align-center td_specialapproval">
                                        <asp:Label ID="lblSpecialApprove" runat="server"></asp:Label>
                                        <asp:CheckBox ID="cbSpecialApprove" runat="server" CssClass="hidden" />
                                    </td>
                                </tr>
                                <tr class="tr_remark">
                                    <td class="value align-center td_remark">
                                        Remark:
                                    </td>
                                    <td class="value td_remark" colspan="9">
                                        <asp:TextBox ID="txtRemark" runat="server" value="Why exceeds company standard?"
                                            onclick="DisplayDefaultMessage(this,'Why exceeds company standard?')" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
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
            <QFL:FormField ID="ffReasons" runat="server" FieldName="Reasons">
            </QFL:FormField>
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
            <QFL:FormField ID="ffSupportingSubmitted" runat="server" FieldName="SupportingSubmitted">
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
            <QFL:FormAttachments runat="server" ID="attacthment">
            </QFL:FormAttachments>
        </td>
    </tr>
    <tr>
        <td>
            Finance Remark
        </td>
        <td colspan="3" style="text-align: left;">
            <QFL:FormField ID="ffFinanceRemark" runat="server" FieldName="FinanceRemark" ControlMode="Display">
            </QFL:FormField>
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
    <asp:HiddenField ID="hidTravelRequest" runat="server" />
    <asp:HiddenField ID="hidTravelPolicy" runat="server" />
    <asp:HiddenField ID="hidTravelDetails" runat="server" />
    <asp:HiddenField ID="hidMealItemValue" runat="server" />
</div>
<%--Modal Dialog--%>
<!-- #customize your modal window here -->
<div id="dialog" class="hidden">
    <table>
        <tr>
            <td colspan="2">
                <table class="inner-table">
                    <tr>
                        <th class="align-left">
                            From
                        </th>
                        <td class="align-left">
                            <cc1:CADateTimeControl ID="dtMealFrom" runat="server" DateOnly="true" CssClassTextBox="meal_date" />
                        </td>
                    </tr>
                    <tr>
                        <th class="align-left">
                            To
                        </th>
                        <td class="align-left">
                            <cc1:CADateTimeControl ID="dtMealTo" runat="server" DateOnly="true" CssClassTextBox="meal_date" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="2" style="height: 5px;">
            </td>
        </tr>
        <tr>
            <th class="align-left">
                Please Select Area Option:
                <!-- close button is defined as close class -->
                <%--<a href="#" class="close">Close it</a>--%>
            </th>
            <th class="align-left">
                Currency:
            </th>
        </tr>
        <tr>
            <td class="label" colspan="2" style="height: 5px;">
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlArea" runat="server" CssClass="select_area">
                    <asp:ListItem Text="China" Value="China" />
                    <asp:ListItem Text="FarEast" Value="FarEast" />
                    <asp:ListItem Text="USA" Value="USA" />
                    <asp:ListItem Text="Switzerland" Value="Switzerland" />
                    <asp:ListItem Text="UK" Value="UK" />
                    <asp:ListItem Text="Europe & North Africa" Value="Europe & North Africa" />
                    <asp:ListItem Text="Central & South America" Value="Central & South America" />
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtCurrency" runat="server" Width="70px" Text="RMB"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="2" style="height: 5px;">
            </td>
        </tr>
        <tr>
            <th class="align-left">
                Exchange Rate:
            </th>
            <td>
                <asp:TextBox ID="txtExchRate" runat="server" Width="70px" Text="1.0000"></asp:TextBox>(or
                fill-in your own rate)
                <asp:HiddenField ID="hidExchRate" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="label" colspan="2" style="height: 5px;">
            </td>
        </tr>
        <tr>
            <th class="align-left">
                Need to fill-in the standard meal allowance?
            </th>
            <td>
                <asp:RadioButtonList ID="rblFillAllowance" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="2" style="height: 5px;">
            </td>
        </tr>
        <tr>
            <th colspan="2" class="align-left">
                Please Select Cost Center:
            </th>
        </tr>
        <tr>
            <td class="label" colspan="2" style="height: 5px;">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:DropDownList ID="ddlCostCenter" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <div class="ca-workflow-form-buttons">
        <asp:Button ID="btnSelect" runat="server" Text="Select" OnClick="btnAddMealItems_Click"
            CausesValidation="False" />
    </div>
</div>
