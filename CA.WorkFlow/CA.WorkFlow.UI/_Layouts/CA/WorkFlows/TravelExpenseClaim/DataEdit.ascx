<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.TravelExpenseClaim.DataEdit" %>
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
    <contenttemplate>
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
    </contenttemplate>
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
            <qfl:formfield id="ffPurpose" runat="server" fieldname="Purpose">
            </qfl:formfield>
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
                <contenttemplate>
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
                </contenttemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr class="tr_subtotal">
        <td colspan="6" class="td_subtotal">
            Sub Total:
        </td>
        <td class="td_hotelsubtotal" colspan="4">
            <qfl:formfield id="ffHotelSubTotal" runat="server" fieldname="HotelSubTotal">
            </qfl:formfield>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="10" style="height: 5px;">
        </td>
    </tr>
    <tr>
        <td colspan="10">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <contenttemplate>
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
                </contenttemplate>
                <triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSelect" EventName="Click" />
                </triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr class="tr_subtotal">
        <td colspan="6" class="td_subtotal">
            Sub Total:
        </td>
        <td class="td_mealsubtotal" colspan="4">
            <qfl:formfield id="ffMealSubTotal" runat="server" fieldname="MealSubTotal">
            </qfl:formfield>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="10" style="height: 5px;">
        </td>
    </tr>
    <tr>
        <td colspan="10">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <contenttemplate>
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
                </contenttemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr class="tr_subtotal">
        <td colspan="6" class="td_subtotal">
            Sub Total:
        </td>
        <td class="td_transsubtotal" colspan="4">
            <qfl:formfield id="ffTransSubTotal" runat="server" fieldname="TransSubTotal">
            </qfl:formfield>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="10" style="height: 5px;">
        </td>
    </tr>
    <tr>
        <td colspan="10">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <contenttemplate>
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
                </contenttemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr class="tr_subtotal">
        <td colspan="6" class="td_subtotal">
            Sub Total:
        </td>
        <td class="td_samplesubtotal" colspan="4">
            <qfl:formfield id="ffSampleSubTotal" runat="server" fieldname="SampleSubTotal">
            </qfl:formfield>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="10" style="height: 5px;">
        </td>
    </tr>
    <tr>
        <td colspan="10">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <contenttemplate>
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
                </contenttemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr class="tr_subtotal">
        <td colspan="6" class="td_subtotal">
            Sub Total:
        </td>
        <td class="td_otherssubtotal" colspan="4">
            <qfl:formfield id="ffOthersSubTotal" runat="server" fieldname="OthersSubTotal">
            </qfl:formfield>
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
                        <qfl:formfield id="ffTotalCost" runat="server" fieldname="TotalCost">
                        </qfl:formfield>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right; padding-right: 10px; width: 600px;">
                        Cash Advance
                    </td>
                    <td>
                        <qfl:formfield id="ffCashAdvanced" runat="server" fieldname="CashAdvanced">
                        </qfl:formfield>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right; padding-right: 10px; width: 600px;">
                        Paid by Co Credit Card
                    </td>
                    <td>
                        <qfl:formfield id="ffPaidByCreditCard" runat="server" fieldname="PaidByCreditCard">
                        </qfl:formfield>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right; padding-right: 10px; width: 600px;">
                        Payable to Employee/(Refund to Finance)
                    </td>
                    <td>
                        <qfl:formfield id="ffNetPayable" runat="server" fieldname="NetPayable">
                        </qfl:formfield>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right; padding-right: 10px; width: 600px;">
                        Approved Budget less Flight Charges
                    </td>
                    <td>
                        <qfl:formfield id="ffTotalExceptFlight" runat="server" fieldname="TotalExceptFlight">
                        </qfl:formfield>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right; padding-right: 10px; width: 600px;">
                        Lower/(Higher) than Budget
                    </td>
                    <td style="width: 20%;">
                        <qfl:formfield id="ffComparedToApproved" runat="server" fieldname="ComparedToApproved">
                        </qfl:formfield>
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
            <qfl:formfield id="ffReasons" runat="server" fieldname="Reasons">
            </qfl:formfield>
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
            <qfl:formfield id="ffSupportingSubmitted" runat="server" fieldname="SupportingSubmitted">
            </qfl:formfield>
        </td>
        <td class="w25">
            Date of submission
        </td>
        <td class="w25">
            <qfl:formfield id="ffSubmissionDate" runat="server" fieldname="SubmissionDate" controlmode="Display">
            </qfl:formfield>
        </td>
    </tr>
    <tr>
        <td>
            Attachment:
        </td>
        <td colspan="3" style="text-align: left;">
            <qfl:formattachments runat="server" id="attacthment">
            </qfl:formattachments>
        </td>
    </tr>
    <tr>
        <td>
            Finance Remark
        </td>
        <td colspan="3" style="text-align: left;">
            <qfl:formfield id="ffFinanceRemark" runat="server" fieldname="FinanceRemark" controlmode="Display">
            </qfl:formfield>
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
                            <cc1:cadatetimecontrol id="dtMealFrom" runat="server" dateonly="true" cssclasstextbox="meal_date" />
                        </td>
                    </tr>
                    <tr>
                        <th class="align-left">
                            To
                        </th>
                        <td class="align-left">
                            <cc1:cadatetimecontrol id="dtMealTo" runat="server" dateonly="true" cssclasstextbox="meal_date" />
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
<%--End Modal Dialog--%>
<script type="text/javascript">
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

    function FormatDate(s) {
        var tmp = s.split('-');
        if (tmp.length > 1) {
            if (tmp[0].length == 4) {
                return tmp[1] + '/' + tmp[2] + '/' + tmp[0];
            }
            return tmp[0] + '/' + tmp[1] + '/' + tmp[2];
        }
        return s;
    }
</script>
<script type="text/javascript">
    var travelTotalBudget;
    var vehicleCost = 0;
    var ffPostfix = '_ctl00_ctl00_TextField';
    var datePickerId;
    var ddlArea = '#<%=this.ddlArea.ClientID %>';
    var txtCurrency = '#<%=this.txtCurrency.ClientID %>';
    var txtExchRate = '#<%=this.txtExchRate.ClientID %>';
    var hidExchRate = '#<%=this.hidExchRate.ClientID %>';

    //page_load

    $(function () {

        LoadSourceData();

        CostCenterBind();

        LaunchModalDialog();

        BindChangeEvent();

        $('#tb_travelexpense td.td_date .DateTimeControl').attr('contentEditable', 'false');

        SetExchRate();

        SetControlMode();

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });

    function LoadSourceData() {

        var hidTravelRequestValue = $('#<%=this.hidTravelRequest.ClientID %>').val();
        //var hidTravelDetailsVaue = $('#<%=this.hidTravelDetails.ClientID %>').val();
        var hidTravelRequestJson = eval('(' + hidTravelRequestValue + ')');
        //var hidTravelDetailsJson = eval('(' + hidTravelDetailsVaue + ')');
        var isEmpty = hidTravelRequestJson.length > 0;
        $('#<%=this.lblApplicant.ClientID %>').text(isEmpty ? hidTravelRequestJson[0].Applicant : "");
        $('#<%=this.lblChineseName.ClientID %>').text(isEmpty ? hidTravelRequestJson[0].ChineseName : "");
        $('#<%=this.lblEnglishName.ClientID %>').text(isEmpty ? hidTravelRequestJson[0].EnglishName : "");
        $('#<%=this.lblIDNumber.ClientID %>').text(isEmpty ? hidTravelRequestJson[0].IDNumber : "");
        $('#<%=this.lblDepartment.ClientID %>').text(isEmpty ? hidTravelRequestJson[0].Department : "");
        $('#<%=this.lblMobile.ClientID %>').text(isEmpty ? hidTravelRequestJson[0].Mobile : "");
        $('#<%=this.lblOfficeExt.ClientID %>').text(isEmpty ? hidTravelRequestJson[0].OfficeExt : "");
        $('#<%=this.ffCashAdvanced.ClientID %>' + ffPostfix).val(isEmpty ? hidTravelRequestJson[0].CashAdvanced : "0");
        if (ca.util.emptyString($('#<%=this.ffPurpose.ClientID %>' + ffPostfix).val())) {
            if (ca.util.emptyString(hidTravelRequestJson[0].TravelOtherPurpose)) {
                $('#<%=this.ffPurpose.ClientID %>' + ffPostfix).val(isEmpty ? hidTravelRequestJson[0].TravelPurpose : "");
            } else {
                $('#<%=this.ffPurpose.ClientID %>' + ffPostfix).val(isEmpty ? hidTravelRequestJson[0].TravelOtherPurpose : "");
            }
        }
        //        $('#<%=this.ffSubmissionDate.ClientID %>' + ffPostfix).val(isEmpty ? hidTravelRequestJson[0].Modified : "");

        travelTotalBudget = isEmpty ? hidTravelRequestJson[0].TravelTotalCost : 0;
        var hidTravelDetailsValue = $('#<%=this.hidTravelDetails.ClientID %>').val();
        var json = eval('(' + hidTravelDetailsValue + ')');
        if (json.length > 0) {
            for (var i = 0; i < json.length; i++) {
                vehicleCost += parseFloat(json[i].VehicleEstimatedCost);
            }
        }
        $('#<%=this.ffTotalExceptFlight.ClientID %>' + ffPostfix).val(isEmpty ? travelTotalBudget - vehicleCost : "0");
    }

    function CostCenterBind() {
        $('#tb_travelexpense select.width-fix')
            .bind('focus mouseover', function () { $(this).addClass('expand').removeClass('clicked'); })
            .bind('click', function () { if ($(this).hasClass('clicked')) { $(this).blur(); } $(this).toggleClass('clicked'); })
            .bind('mouseout', function () { if (!$(this).hasClass('clicked')) { $(this).removeClass('expand'); } })
            .bind('blur', function () { $(this).removeClass('expand clicked'); });
    }

    function LaunchModalDialog() {
        $('#dialog').dialog({
            title: "Select Meal Info",
            autoOpen: false,
            width: 600,
            height: 270,
            modal: true,
            resizable: false,
            draggable: false,
            bigiframe: true,
            open: function (type, data) {
                $(this).parent().appendTo('form');
            }
        });

        $(ddlArea).change(function () {
            var hidTravelPolicyValue = $('#<%=this.hidTravelPolicy.ClientID %>').val();
            var json = eval('(' + hidTravelPolicyValue + ')');
            var currency = 'RMB';

            var $area = $('#<%=this.ddlArea.ClientID %>');
            for (var i = 0; i < json.length; i++) {
                if (json[i].Title == $area.val()) {
                    currency = json[i].Currency;
                    break;
                }
            }

            $(txtCurrency).val(currency);
            //$(txtCurrency).attr('contentEditable', 'false');

            $(txtExchRate).val(GetExchRate(currency));
            $(hidExchRate).val(GetExchRate(currency));
            //$(txtExchRate).attr('contentEditable', 'false');

        });

        $('#<%=this.btnSelect.ClientID %>').click(function () {

            var hidTravelPolicyValue = $('#<%=this.hidTravelPolicy.ClientID %>').val();
            var json = eval('(' + hidTravelPolicyValue + ')');

            var $area = $('#<%=this.ddlArea.ClientID %>');
            var fromDate = FormatDate($('#<%=this.dtMealFrom.ClientID %>' + '_dtMealFromDate').val());
            var toDate = FormatDate($('#<%=this.dtMealTo.ClientID %>' + '_dtMealToDate').val());
            var days = ca.util.emptyString(fromDate) || ca.util.emptyString(toDate) ? 1 : (new Date(toDate) - new Date(fromDate)) / (24 * 60 * 60 * 1000) + 1;

            var breakfast = 50;
            var lunch = 50;
            var dinner = 70;

            var costCenter = $('#<%=this.ddlCostCenter.ClientID %>').val();
            for (var i = 0; i < json.length; i++) {
                if (json[i].Title == $area.val()) {
                    currency = json[i].Currency;
                    breakfast = json[i].BreakfastLimit;
                    lunch = json[i].LunchLimit;
                    dinner = json[i].DinnerLimit;
                    break;
                }
            }
            var currency = $(txtCurrency).val();

            $area.attr('selectedIndex', 0);
            $('#dialog input.meal_date').val('');


            var mealItemValue = "{'Currency':'" + currency
                              + "','BreakfastLimit':'" + breakfast
                              + "','LunchLimit':'" + lunch
                              + "','DinnerLimit':'" + dinner
                              + "','Days':'" + days
                              + "','DateFrom':'" + fromDate
                              + "','DateTo':'" + toDate
                              + "','CostCenter':'" + costCenter + "'}";
            $('#<%=this.hidMealItemValue.ClientID %>').val(mealItemValue);

            $('#dialog').dialog('close');
        });

    }
    function JsonToStr(o) {
        var arr = [];
        var fmt = function (s) {
            if (typeof s == 'object' && s != null) return JsonToStr(s);
            return /^(string)$/.test(typeof s) ? "'" + s + "'" : s;
        }
        for (var i in o) arr.push("'" + i + "':" + fmt(o[i]));
        return '{' + arr.join(',') + '}';
    }

    function BindChangeEvent() {
        $('#tb_travelexpense td.td_currency input').live('change', function () {
            var preId = GetPreId($(this).attr('id'), 'txtOtherCurrency');

            var hidMealItemValue = $('#' + preId + 'hidMealItem').val();
            if (!ca.util.emptyString(hidMealItemValue)) {
                var json = eval('(' + hidMealItemValue + ')');
                json.Currency = $(this).val();
                $('#' + preId + 'hidMealItem').val(JsonToStr(json));
            }

        });

        $('#tb_travelexpense td.td_originalamt input').live('change', function () {
            var preId = GetPreId($(this).attr('id'), 'txtOriginalAmt');

            var originalAmt = Escape($(this).val(), /,/);
            var exchRate = Escape($('#' + preId + 'txtExchRate').val(), /,/);
            var companyStandsValue = $('#' + preId + 'lblCompanyStandards').text() == "N/A" ? Infinity : $('#' + preId + 'lblCompanyStandards').text();

            if (!ca.util.emptyString(originalAmt)
                && !ca.util.emptyString(exchRate)
                && !isNaN(originalAmt)
                && !isNaN(exchRate)) {
                $('#' + preId + 'txtRmbAmt').val((parseFloat(originalAmt) * parseFloat(exchRate)).toFixed(2));
                if (!ca.util.emptyString(companyStandsValue)
                && !isNaN(companyStandsValue)) {
                    if ((parseFloat(originalAmt) * parseFloat(exchRate)).toFixed(2) > parseFloat(companyStandsValue)) {
                        $('#' + preId + 'txtRmbAmt').attr('style', 'color:red');
                        $('#' + preId + 'cbSpecialApprove').attr('checked', true);
                        $('#' + preId + 'lblSpecialApprove').text('Yes');

                        $(this).parents('tr').first().next().find('td.td_remark').show();


                    }
                    else {

                        $('#' + preId + 'txtRmbAmt').attr('style', '');
                        $('#' + preId + 'cbSpecialApprove').attr('checked', false);
                        $('#' + preId + 'lblSpecialApprove').text('');

                        //$(this).parents('table').first().find('td.td_remark input').val('');
                        $('#' + preId + 'txtRemark').val('Why exceeds company standard?');
                        $(this).parents('tr').first().next().find('td.td_remark').hide();

                    }
                }

            }
            else {
                $('#' + preId + 'txtRmbAmt').val('');
            }

            GetSummary();

        });

        $('#tb_travelexpense td.td_exchrate input').live('change', function () {
            var preId = GetPreId($(this).attr('id'), 'txtExchRate');

            var $mealType = $('#' + preId + 'ddlMealType');
            if ($mealType.length != 0) {
                var exchRate = $(this).val();
                var hidMealItemValue = $('#' + preId + 'hidMealItem').val();
                var mealLimit = GetMealLimit($mealType.val(), hidMealItemValue);
                if (!ca.util.emptyString(exchRate)
                && !isNaN(exchRate)) {
                    //$('#' + preId + 'lblCompanyStandards').text((parseFloat(mealLimit) * parseFloat(exchRate)).toFixed(2));
                    //$('#'+preId+'hidCompanyStandards').val((parseFloat(mealLimit) * parseFloat(exchRate)).toFixed(2));
                } else {
                    //$('#' + preId + 'lblCompanyStandards').text('N/A');
                    //$('#' + preId + 'hidCompanyStandards').val('N/A');
                }
            }

            $('#' + preId + 'txtOriginalAmt').change();
        });

        $('#tb_travelexpense td.td_expensetype select.select_mealtype').live('change', function () {
            var preId = GetPreId($(this).attr('id'), 'ddlMealType');
            $('#' + preId + 'txtExchRate').change();
        });
    }

    function GetSummary() {

        var total = 0;
        var paidByCard = 0;
        var paid = 0;
        //Get SubTotal
        var hotelSubTotal = 0;
        var mealSubTotal = 0;
        var transSubTotal = 0;
        var sampleSubTotal = 0;
        var othersSubTotal = 0;


        $('#tb_travelexpense td.td_hotelrmb input').each(function () {
            hotelSubTotal += !isNaN($(this).val()) && $(this).val().length > 0 ? parseFloat($(this).val()) : 0;
        });
        $('#tb_travelexpense td.td_mealrmb input').each(function () {
            mealSubTotal += !isNaN($(this).val()) && $(this).val().length > 0 ? parseFloat($(this).val()) : 0;
        });
        $('#tb_travelexpense td.td_transrmb input').each(function () {
            transSubTotal += !isNaN($(this).val()) && $(this).val().length > 0 ? parseFloat($(this).val()) : 0;
        });
        $('#tb_travelexpense td.td_samplermb input').each(function () {
            sampleSubTotal += !isNaN($(this).val()) && $(this).val().length > 0 ? parseFloat($(this).val()) : 0;
        });
        $('#tb_travelexpense td.td_othersrmb input').each(function () {
            othersSubTotal += !isNaN($(this).val()) && $(this).val().length > 0 ? parseFloat($(this).val()) : 0;
        });

        $('#<%=this.ffHotelSubTotal.ClientID %>' + ffPostfix).val(hotelSubTotal.toFixed(2));
        $('#<%=this.ffMealSubTotal.ClientID %>' + ffPostfix).val(mealSubTotal.toFixed(2));
        $('#<%=this.ffTransSubTotal.ClientID %>' + ffPostfix).val(transSubTotal.toFixed(2));
        $('#<%=this.ffSampleSubTotal.ClientID %>' + ffPostfix).val(sampleSubTotal.toFixed(2));
        $('#<%=this.ffOthersSubTotal.ClientID %>' + ffPostfix).val(othersSubTotal.toFixed(2));
        //Get Total
        $('#tb_travelexpense td.td_rmbamt input').each(function () {
            total += !isNaN($(this).val()) && $(this).val().length > 0 ? parseFloat($(this).val()) : 0;
        });
        $('#<%= this.ffTotalCost.ClientID %>' + ffPostfix).val(commafy(parseFloat(total).toFixed(2)));

        //Get Paid By Card
        $('#tb_travelexpense td.td_paidbycredit input:checkbox').each(function () {
            if ($(this).attr('checked')) {
                var $rmbAmt = $(this).parents('tr').first().find('td.td_rmbamt input');
                paidByCard += !isNaN($rmbAmt.val()) && $rmbAmt.val().length > 0 ? parseFloat($rmbAmt.val()) : 0;
            }
        });
        $('#<%=this.ffPaidByCreditCard.ClientID %>' + ffPostfix).val(commafy(paidByCard.toFixed(2)));

        //Get Net Payable
        var cashAdvance = $('#<%=this.ffCashAdvanced.ClientID %>' + ffPostfix).val();
        if (!isNaN(cashAdvance)
        && cashAdvance.length > 0) {
            paid = total - parseFloat(cashAdvance) - paidByCard;
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

        compared = travelTotalBudget - vehicleCost - total;
        $('#<%=this.ffComparedToApproved.ClientID %>' + ffPostfix).val(commafy((compared).toFixed(2)));
        if (compared < 0) {
            $('#<%=this.ffComparedToApproved.ClientID %>' + ffPostfix).attr('style', 'color:red');
        }
        else {
            $('#<%=this.ffComparedToApproved.ClientID %>' + ffPostfix).attr('style', '');
        }

    }
    function SetExchRate() {
        $('#tb_travelexpense td.td_currency select').each(function () {
            var preId = GetPreId($(this).attr('id'), 'ddlCurrency');

            switch ($(this).val()) {
                case "RMB":
                    $('#' + preId + 'txtExchRate').val("1.0000");
                    break;
                case "Others":
                    $('#' + preId + 'txtOtherCurrency').show();
                    var hidMealItemValue = $('#' + preId + 'hidMealItem').val();
                    if (!ca.util.emptyString(hidMealItemValue)) {
                        var json = eval('(' + hidMealItemValue + ')');
                        $('#' + preId + 'txtOtherCurrency').val(json.Currency);
                    }
                    break;
                default:
                    break;
            }

        });
    }


    function SetControlMode() {
        $('#<%=this.ffSubmissionDate.ClientID %>' + ffPostfix).attr('disabled', 'true');
        $('#<%=this.ffFinanceRemark.ClientID %>' + ffPostfix).attr('disabled', 'true');
        $('#tb_travelexpense td.td_specialapproval').hide();

        $('#table_claimsummary input').attr('contentEditable', 'false');
        $('#<%=this.ffReasons.ClientID %>' + ffPostfix).attr('contentEditable', 'true');
        $('#tb_travelexpense tr.tr_subtotal input').attr('contentEditable', 'false');
        GetSummary();
    }

    function EndRequestHandler() {

        SetAsynViewStyle();

        $('#tb_travelexpense td.td_date .DateTimeControl').attr('contentEditable', 'false');

        $('#tb_travelexpense td.td_specialapproval input').each(function () {
            var preId = GetPreId($(this).attr('id'), 'cbSpecialApprove');

            if ($(this).attr('checked')) {

                $('#' + preId + 'txtRmbAmt').attr('style', 'color:red');

                $(this).parents('tr').first().next().find('td.td_remark').show();

            }

        });

        $('#tb_travelexpense td.td_specialapproval').hide();

        SetExchRate();

        $('#tb_travelexpense select.width-fix').unbind();
        CostCenterBind();

        GetSummary();

    }

    function SetAsynViewStyle() {
        $('#<%=this.ddlCostCenter.ClientID %>').val('-1');
        $('#<%=this.rblFillAllowance.ClientID %>').val('0');
        $('#<%=this.txtCurrency.ClientID %>').val('RMB');
        $('#<%=this.txtExchRate.ClientID %>').val('1.0000');
    }
</script>
<script type="text/javascript">
    //    function ShowModal() {

    //        //Get the A tag
    //        var id = "#dialog";

    //        //Get the screen height and width
    //        var maskHeight = $(document).height();
    //        var maskWidth = $(window).width();

    //        //Set height and width to mask to fill up the whole screen
    //        $('#mask').css({ 'width': maskWidth, 'height': maskHeight });

    //        //transition effect    
    //        $('#mask').fadeIn(500);
    //        $('#mask').fadeTo("slow", 0.8);

    //        //Get the window height and width
    //        var winH = $(document).height();
    //        var winW = $(window).width();

    //        //Set the popup window to center
    //        $(id).css('top', winH / 2 - $(id).height() / 2);
    //        $(id).css('left', winW / 2 - $(id).width() / 2);

    //        //transition effect
    //        $(id).fadeIn(2000);

    //    }
    function DisplayDefaultMessage(obj, msg) {
        if ($(obj).val() == msg) {
            $(obj).val('');
        }
    }

    function SelectLocation(obj) {
        var preId = obj.id.substring(0, 64);

        $('#' + preId + 'lblCompanyStandards').text(GetHotelLimit($(obj).val()));
        $('#' + preId + 'hidCompanyStandards').val(GetHotelLimit($(obj).val()));
        DateChange(preId);

    }
    function SelectCurrency(obj) {

        var preId = GetPreId($(obj).attr('id'), 'ddlCurrency');
        if ($('#' + preId + 'ddlMealType').length == 0) {
            var mealType = $('#' + preId + 'ddlMealType').val();
            var hidMealItemValue = $('#' + preId + 'hidMealItem').val();
            var mealLimit = GetMealLimit(mealType, hidMealItemValue);
        }
        var exchRate = GetExchRate($(obj).val());

        $('#' + preId + 'txtExchRate').val(exchRate);
        $('#' + preId + 'txtExchRate').change();

        if ($(obj).val() == "Others") {
            $('#' + preId + 'txtOtherCurrency').show();
        } else {
            $('#' + preId + 'txtOtherCurrency').hide();
        }

        //        if (!ca.util.emptyString($('#' + preId + 'txtExchRate').val())) {
        //            $('#' + preId + 'txtRmbAmt').val('');
        //        }        

        $('#' + preId + 'txtOriginalAmt').change();

    }

    function GetMealLimit(mealType, hidMealItemValue) {
        var mealLimit;
        var json = eval('(' + hidMealItemValue + ')');
        switch (mealType) {
            case "Breakfast":
                mealLimit = json.BreakfastLimit;
                break;
            case "Lunch":
                mealLimit = json.LunchLimit;
                break;
            case "Dinner":
                mealLimit = json.DinnerLimit;
                break;
            default:
                break;
        }
        return mealLimit;
    }

    function GetExchRate(currency) {

        var exchRate = "";
        switch (currency) {
            case "RMB":
                exchRate = "1.0000";
                break;
            case "USD":
                exchRate = '<%=GetExchRate("USD") %>';
                break;
            case "GBP":
                exchRate = '<%=GetExchRate("GBP") %>';
                break;
            case "EUR":
                exchRate = '<%=GetExchRate("EUR") %>';
                break;
            case "AUD":
                exchRate = '<%=GetExchRate("AUD") %>';
                break;
            case "CHF":
                exchRate = '<%=GetExchRate("CHF") %>';
                break;
            case "CAD":
                exchRate = '<%=GetExchRate("CAD") %>';
                break;
            case "JPY":
                exchRate = '<%=GetExchRate("JPY") %>';
                break;
            case "HKD":
                exchRate = '<%=GetExchRate("HKD") %>';
                break;
            default:
                break;
        }
        return exchRate;
    }

    function CheckBoxPaidByCredit(obj) {
        var preId = GetPreId($(obj).attr('id'), 'cbPaidByCredit');

        //$('#' + preId + 'txtOriginalAmt').change();
        GetSummary();

    }

    function DateValueChange(obj) {

        if (obj.Picker != null) {
            datePickerId = obj.Picker.id.substring(0, 64);
            DateChange(datePickerId);
        } else {
            $('#' + datePickerId + 'dtTo_dtToDate').val('');
        }
    }

    function DateChange(preId) {
        var curDateFrom = FormatDate($('#' + preId + 'dtFrom_dtFromDate').val());
        var curDateTo = FormatDate($('#' + preId + 'dtTo_dtToDate').val());

        if (!ca.util.emptyString(curDateFrom)
                && !ca.util.emptyString(curDateTo)) {
            if ((new Date(curDateTo) - new Date(curDateFrom)) < 0) {
                $('#' + preId + 'dtTo_dtToDate').val('');
                return alert("End Date must be more than Start Date.");

            } else {

                var days = (new Date(curDateTo) - new Date(curDateFrom)) / (24 * 60 * 60 * 1000);
                var hotelLimit = GetHotelLimit($('#' + preId + 'ddlLocation').val());
                if (!isNaN(days) && !isNaN(hotelLimit)) {
                    $('#' + preId + 'lblCompanyStandards').text(parseFloat(hotelLimit) * parseFloat(days));
                    $('#' + preId + 'hidCompanyStandards').val(parseFloat(hotelLimit) * parseFloat(days));
                } else {
                    $('#' + preId + 'lblCompanyStandards').text('N/A');
                    $('#' + preId + 'hidCompanyStandards').val('N/A');
                }
            }
        }
        $('#' + preId + 'txtOriginalAmt').change();

    }

    function GetPreId(clientId, id) {
        return clientId.substring(0, clientId.length - id.length);
    }

    function GetHotelLimit(location) {
        var hidTravelPolicyValue = $('#<%=this.hidTravelPolicy.ClientID %>').val();
        var json = eval('(' + hidTravelPolicyValue + ')');
        var hotelLimit = "N/A";
        for (var i = 0; i < json.length; i++) {
            if (json[i].Location == location) {
                hotelLimit = json[i].HotelLimit;
                break;
            }
        }
        return hotelLimit;
    }

    function IsValidHotel(from, to) {
        var hidTravelDetailsValue = $('#<%=this.hidTravelDetails.ClientID %>').val();
        var json = eval('(' + hidTravelDetailsValue + ')');
        var travelDateFrom, travelDateTo;
        for (var i = 0; i < json.length; i++) {
            travelDateFrom = FormatDate(json[i].TravelDateFrom);
            travelDateTo = FormatDate(json[i].TravelDateTo);
            if (new Date(from) >= new Date(travelDateFrom)
                && new Date(to) <= new Date(travelDateTo)) {
                return true;
            }
        }
        return false;
    }


</script>
<script type="text/javascript">

    function beforeSubmit() {
        var isValidate = Validate();
        if (isValidate) {
            alert('Please print out this claim form,attach all invoices(include those items paid by Company credit card),and pass to Finance.');
            CreateForbidDIV(); //单击生成弹出层，防止重复提交。
        }
        return isValidate;
    }
    function Validate() {

        var error = '';
        var fields =
        [
            { "fieldName": "Total", "clientId": "<%=this.ffTotalCost.ClientID%>", "addition": "_ctl00_ctl00_TextField" },
            { "fieldName": "Cash Advance", "clientId": "<%=this.ffCashAdvanced.ClientID%>", "addition": "_ctl00_ctl00_TextField" },
            { "fieldName": "Paid by Company Credit Card", "clientId": "<%=this.ffPaidByCreditCard.ClientID%>", "addition": "_ctl00_ctl00_TextField" },
            { "fieldName": "Net Payable to Employee/(Refund to Finance)", "clientId": "<%=this.ffNetPayable.ClientID%>", "addition": "_ctl00_ctl00_TextField" },
            { "fieldName": "Lower(Higher) than Budget", "clientId": "<%=this.ffComparedToApproved.ClientID%>", "addition": "_ctl00_ctl00_TextField" }
         ];
        for (var index in fields) {
            error += ValidateEmptyString(fields[index]["fieldName"], fields[index]["clientId"], fields[index]["addition"]);
            error += ValidateNumber(fields[index]["fieldName"], fields[index]["clientId"], fields[index]["addition"]);
        }

        error += ValidateRepeaterTable();

        if (error) {
            alert(error);
        }
        return error.length === 0;
    }

    function ValidateEmptyString(fieldName, clientId, addition) {
        var msg = '';
        if (ca.util.emptyString($('#' + clientId + addition).val())) {
            msg = 'Please fill in the ' + fieldName + ' field.\n';
            SetBorderWarn($('#' + clientId + addition));
        } else {
            ClearBorderWarn($('#' + clientId + addition));
        }
        return msg;
    }
    function ValidateNumber(fieldName, clientId, addition) {
        var msg = '';
        var value = Escape($('#' + clientId + addition).val(), /,/);
        if (isNaN(value)) {
            msg = 'The ' + fieldName + ' field is not a valid number.\n';
            SetBorderWarn($('#' + clientId + addition));
        } else {
            ClearBorderWarn($('#' + clientId + addition));
        }
        return msg;
    }
    function ValidateRepeaterTable() {

        var errorMessage = "Please fill the valid travel expense claim details info.\n";
        var msg = '';
        $('#tb_travelexpense td.td_date .DateTimeControl').each(function () {
            msg += ValidateEmptyString('Date', $(this).attr('id'), '');
        });
        $('#tb_travelexpense td.td_originalamt input').each(function () {
            if (ca.util.emptyString($(this).val())) {
                msg += ValidateEmptyString('Claim Amount', $(this).attr('id'), '');
            } else {
                msg += ValidateNumber('Claim Amount', $(this).attr('id'), '');
            }
        });
        $("#tb_travelexpense td.td_expensetype input[type='text']").each(function () {
            msg += ValidateEmptyString('Date', $(this).attr('id'), '');
        });
        $('#tb_travelexpense td.td_expensetype select').each(function () {
            var preId = GetPreId($(this).attr('id'), 'ddlLocation');
            if ($(this).val() == "Select City") {
                msg += "Please Select the City\n";
                SetBorderWarn($(this).parent());
            } else {
                ClearBorderWarn($(this).parent());
            }
        });
        $('#tb_travelexpense td.td_costcenter select').each(function () {
            if ($(this).val() == "-1") {
                msg += "Please Select CostCenter\n";
                SetBorderWarn($(this).parent());
            } else {
                ClearBorderWarn($(this).parent());
            }
        });
        if (msg.length > 0) { return errorMessage; }
        return msg;
    }
    function SetBorderWarn(obj) {
        obj.css('border', '1px solid red');
    }
    function ClearBorderWarn(obj) {
        obj.css('border', '');
        obj.css('border-bottom', '#999 1px solid');
    }
        
</script>
