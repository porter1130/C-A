<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.TravelRequest2.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<table class="ca-workflow-form-table">
    <tr>
        <td class="label align-center w25">
            Choose Employee<br />
            选择员工
        </td>
        <td class="value">
            <cc1:CAPeopleFinder ID="cpfUser" runat="server" AllowTypeIn="true" MultiSelect="false"
                CssClass="ca-people-finder" Width="200" />
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
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
                    中文姓名<span class="clr-red icon-point">●</span>
                </td>
                <td class="label align-center w25">
                    English Name<br />
                    英文姓名<span class="clr-red icon-point">●</span>
                </td>
                <td class="label align-center w25">
                    ID/Passport No.<br />
                    身份证/护照号码<span class="clr-red icon-point">●</span>
                </td>
                <td class="value align-center w25">
                    Department
                    <br />
                    部门
                </td>
            </tr>
            <tr>
                <td class="label align-center w25">
                    <QFL:FormField ID="ffChineseName" runat="server" FieldName="ChineseName" ControlMode="Edit">
                    </QFL:FormField>
                </td>
                <td class="label align-center w25">
                    <QFL:FormField ID="ffEnglishName" runat="server" FieldName="EnglishName" ControlMode="Edit">
                    </QFL:FormField>
                </td>
                <td class="label align-center w25">
                    <QFL:FormField ID="ffIDNumber" runat="server" FieldName="IDNumber" ControlMode="Edit">
                    </QFL:FormField>
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
                                手机号码<span class="clr-red icon-point">●</span>
                            </td>
                            <td class="label align-center w15">
                                <QFL:FormField ID="ffMobile" runat="server" FieldName="Mobile">
                                </QFL:FormField>
                            </td>
                            <td class="label align-center">
                                Office Ext. No.<br />
                                分机号码<span class="clr-red icon-point">●</span>
                            </td>
                            <td class="value align-center w15">
                                <QFL:FormField ID="ffOfficeExt" runat="server" FieldName="OfficeExt">
                                </QFL:FormField>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnPeopleInfo" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
<table class="ca-workflow-form-table">
    <tr>
        <td class="value align-center" colspan="3">
            <h3>
                Travel Information & Budget 出差信息及预算</h3>
        </td>
    </tr>
    <tr>
        <td class="value w15 align-right">
            Travel Purpose
        </td>
        <td class="value w35">
            <QFL:FormField ID="ffPurpose" runat="server" FieldName="TravelPurpose">
            </QFL:FormField>
        </td>
        <td class="value">
            <div class="hidden" id="ca-other-purpose">
                Please state travel purpose <span class="clr-red icon-point">●</span>
                <asp:TextBox ID="txtOtherPurpose" runat="server" Width="170" />
            </div>
        </td>
    </tr>
    <tr>
        <td class="value align-right w15">
            Remark, if any:
        </td>
        <td class="value" colspan="2">
            <QFL:FormField ID="ffNote" runat="server" FieldName="TravelRemark" />
        </td>
    </tr>
    <tr>
        <td class="value align-right w15">
            Attachment:
        </td>
        <td class="value" colspan="2">
            <QFL:FormAttachments runat="server" ID="attacthment">
            </QFL:FormAttachments>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="inner-table">
                        <tr>
                            <td class="cell align-center" width="36">
                                <asp:ImageButton runat="server" ID="btnAddTravel" ToolTip="Click to add the travel information."
                                    ImageUrl="../images/pixelicious_001.png" OnClick="btnAddTravel_Click" Width="18"
                                    CssClass="img-button" />
                            </td>
                            <td class="label align-center" width="25%">
                                Travel Period<span class="clr-red icon-point">●</span>
                            </td>
                            <td class="label align-center" width="20%">
                                Travel Location<span class="clr-red icon-point">●</span>
                            </td>
                            <td class="label align-center" width="15%">
                                Cost Center<span class="clr-red icon-point">●</span>
                            </td>
                            <td class="label align-center" width="25%">
                                Cost Item
                            </td>
                            <td class="value align-center" width="13%">
                                Estimated Cost<span class="clr-red icon-point">●</span>
                            </td>
                        </tr>
                        <asp:Repeater ID="rptTravel" runat="server" OnItemCommand="rptTravel_ItemCommand"
                            OnItemDataBound="rptTravel_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td class="cell align-center" width="36">
                                        <asp:ImageButton ID="btnDeleteTravel" CommandName="delete" ToolTip="Delete this travel information."
                                            runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                                    </td>
                                    <td class="cell align-center" valign="top">
                                        <table class="inner-table">
                                            <tr>
                                                <td class="label align-center w50">
                                                    From
                                                </td>
                                                <td class="value align-center">
                                                    To
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="align-center">
                                                    <cc1:CADateTimeControl ID="dtTravelDateFrom" runat="server" DateOnly="true" CssClassTextBox="TravelInformation w60 DateTimeControl" />
                                                </td>
                                                <td class="align-center">
                                                    <cc1:CADateTimeControl ID="dtTravelDateTo" runat="server" DateOnly="true" CssClassTextBox="TravelInformation w60 DateTimeControl" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <asp:HiddenField ID="hidCost" runat="server" />
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="cell align-center" valign="top">
                                        <table class="inner-table">
                                            <tr>
                                                <td class="label align-center w50">
                                                    From
                                                </td>
                                                <td class="value align-center">
                                                    To
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="align-center">
                                                    <asp:TextBox ID="txtTravelFrom" runat="server" onfocus="Block(this)" onblur="ReturnCost(this)"
                                                        CssClass="TravelInformation" />
                                                </td>
                                                <td class="align-center">
                                                    <asp:TextBox ID="txtTravelTo" runat="server" onfocus="Block(this)" onblur="ReturnCost(this)"
                                                        CssClass="TravelInformation" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" class="blank-line">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" class="align-center">
                                                    <p>
                                                        Area Option:</p>
                                                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="TravelInformation ca-travel-area"
                                                        onchange="return ReturnCost(this)">
                                                        <asp:ListItem Text="China" Value="China" Selected="True" />
                                                        <asp:ListItem Text="FarEast" Value="FarEast" />
                                                        <asp:ListItem Text="USA" Value="USA" />
                                                        <asp:ListItem Text="Switzerland" Value="Switzerland" />
                                                        <asp:ListItem Text="UK" Value="UK" />
                                                        <asp:ListItem Text="Europe & North Africa" Value="Europe & North Africa" />
                                                        <asp:ListItem Text="Central & South America" Value="Central & South America" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="cell w15 costcenter">
                                        <asp:DropDownList ID="ddlCostCenter" CssClass="width-fix" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="cell w15">
                                        <table class="inner-table">
                                            <tr>
                                                <td class="last-cell h30 p5 ca-travel-vehicle">
                                                    <asp:DropDownList ID="ddlVehicle" runat="server" Width="60%" onchange="OnChangeVehicle(this)"
                                                        CssClass="VehicleInfomation">
                                                        <asp:ListItem Text="Flight" Value="Flight" Selected="True" />
                                                        <asp:ListItem Text="Rail" Value="Rail" />
                                                        <asp:ListItem Text="Bus" Value="Bus" />
                                                    </asp:DropDownList>
                                                    Return<asp:CheckBox ID="returnVehicle" runat="server" onclick="CheckReturn(this)"
                                                        Width="13" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="last-cell h30 p5">
                                                    Hotel
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="last-cell h30 p5">
                                                    Meal Allowance
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="last-cell h30 p5">
                                                    Local Transportation
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="last-cell h30 p5">
                                                    Sample Purchase
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="last-cell h30 p5">
                                                    Others
                                                    <asp:TextBox ID="txtOthersCostItem" runat="server" Width="100" />
                                                </td>
                                            </tr>
                                            <tr class="last">
                                                <td class="last-cell h30 p5">
                                                    Total
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="cell align-center w15">
                                        <table class="inner-table">
                                            <tr>
                                                <td class="last-cell h30 p5">
                                                    <asp:TextBox ID="txtVehicleCost" runat="server" CssClass="TravelInformation_Number VehicleCostValidate" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="last-cell h30 p5">
                                                    <asp:TextBox ID="txtHotelCost" runat="server" CssClass="TravelInformation_Number" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="last-cell h30 p5">
                                                    <asp:TextBox ID="txtMealCost" runat="server" CssClass="TravelInformation_Number MealCostValidate" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="last-cell h30 p5">
                                                    <asp:TextBox ID="txtTransportationCost" runat="server" CssClass="TravelInformation_Number" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="last-cell h30 p5">
                                                    <asp:TextBox ID="txtSample" runat="server" CssClass="TravelInformation_Number" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="last-cell h30 p5">
                                                    <asp:TextBox ID="txtOtherCost" runat="server" CssClass="TravelInformation_Number" />
                                                </td>
                                            </tr>
                                            <tr class="last">
                                                <td class="last-cell h30 p5 subtotal">
                                                    <asp:Label ID="lblTotalCost" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="btnReloadTravelPolicy" EventName="Click" />--%>
                    <asp:AsyncPostBackTrigger ControlID="btnReloadCostPolicy" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <table class="inner-table">
                <tr>
                    <td class="align-center" width="36">
                    </td>
                    <td class="align-center" width="25%">
                    </td>
                    <td class="align-center" width="30%">
                    </td>
                    <td class="align-center" width="25%">
                    </td>
                    <td class="align-center" width="13%">
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="p5">
                        Total Budget of the business trip
                    </td>
                    <td class="totalbudget">
                        <asp:Label ID="lblTotalBudget" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="p5">
                        Cash Advance required?
                        <br />
                        <span class="zh-height">需要预借现金 </span>
                    </td>
                    <td id="ca-travel-iscashadvance" colspan="2" class="p5">
                        <QFL:FormField ID="ffIsCashAdvanced" runat="server" FieldName="IsCashAdvanced" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="p5">
                        If choose Yes above, please state the amount of Cash Advance required
                        <br />
                        <span class="zh-height">如果选择YES，请注明预借金额 <span class="clr-red icon-point">●</span></span>
                    </td>
                    <td id="ca-travel-cashadvance" class="p5">
                        <QFL:FormField ID="ffCashAdvanced" runat="server" FieldName="CashAdvanced" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="p5">
                        Note: Cash Advance requested will be transferred to your bank account in two weeks
                        after the Travel Request is approved by your line manager.
                        <br />
                        <span class="zh-height">预借现金将会在出差申请被批准后的两周内汇入你的账户 </span>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="7" class="label align-center">
            <h3>
                Vehicle Information 交通工具信息 (for booking purpose)<span class="clr-red icon-point">●</span></h3>
        </td>
    </tr>
    <tr>
        <td colspan="7">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="inner-table">
                        <tr>
                            <td class="cell align-center" width="36">
                                <asp:ImageButton runat="server" ID="btnAddVehicle" ToolTip="Click to add the vehicle information."
                                    ImageUrl="../images/pixelicious_001.png" OnClick="btnAddVehicle_Click" Width="18"
                                    CssClass="img-button" />
                            </td>
                            <td class="label align-center w15">
                                Vehicle
                            </td>
                            <td class="label align-center w15">
                                Vehicle No.
                                <br />
                                (if known)
                            </td>
                            <td class="label align-center w15">
                                Departure Date
                            </td>
                            <td class="label align-center w15">
                                From
                            </td>
                            <td class="label align-center w15">
                                To
                            </td>
                            <td class="value align-center">
                                Departure Time
                                <br />
                                (am/pm)
                            </td>
                        </tr>
                        <asp:Repeater ID="rptVehicle" runat="server" OnItemCommand="rptVehicle_ItemCommand"
                            OnItemDataBound="rptVehicle_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td class="cell align-center">
                                        <asp:ImageButton ID="btnDeleteVehicle" ToolTip="Remove this vehicle information."
                                            CommandName="delete" runat="server" ImageUrl="../images/pixelicious_028.png"
                                            Width="18" CssClass="img-button" />
                                    </td>
                                    <td class="label align-center">
                                        <asp:DropDownList ID="ddlVehicle" runat="server" Width="90%">
                                            <asp:ListItem Text="Flight" Value="Flight" Selected="True" />
                                            <asp:ListItem Text="Rail" Value="Rail" />
                                            <asp:ListItem Text="Bus" Value="Bus" />
                                        </asp:DropDownList>
                                    </td>
                                    <td class="label align-center">
                                        <asp:TextBox ID="txtVehicleNum" runat="server" CssClass="VehicleInfomation" />
                                    </td>
                                    <td class="label align-center">
                                        <cc1:CADateTimeControl ID="CADateTimeVehicleDate" runat="server" DateOnly="true"
                                            CssClassTextBox="VehicleInfomation w60 DateTimeControl" />
                                    </td>
                                    <td class="label align-center">
                                        <asp:TextBox ID="txtFrom" runat="server" CssClass="VehicleInfomation" />
                                    </td>
                                    <td class="label align-center">
                                        <asp:TextBox ID="txtTo" runat="server" CssClass="VehicleInfomation" />
                                    </td>
                                    <td class="value align-center">
                                        <asp:TextBox ID="txtTime" runat="server" CssClass="VehicleInfomation" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td colspan="7" class="p5">
            For Flight, if Economy class of chosen flight is not available, you opt for:
            <br />
            <table>
                <tr>
                    <td class="ca-travel-flight">
                        Business class (商务舱) of chosen flight
                        <asp:CheckBox ID="cbChosenFlight" runat="server" Width="32" />
                        &nbsp;or Other available flight
                        <asp:CheckBox ID="cbNextFlight" runat="server" Width="32" />
                    </td>
                    <td class="ca-available-flight">
                        <span id="ddlAvailableFlight" class="hidden">
                            <QFL:FormField ID="NextAvailableFlight" runat="server" FieldName="NextAvailableFlight">
                            </QFL:FormField>
                        </span>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="7" class="p5">
            Note: If according to Company Travel Policy, you are not entitled to Business Class,
            but you opt for Business Class above, then your request will be routed to CEO for
            approval.
            <br />
            <span class="zh-height">如果你无法根据公司的差旅政策选择经济舱，你的商务舱申请将会被自动发送至CEO审批 </span>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table ca-travel-hoteltable">
    <tr>
        <td colspan="6" class="value align-center">
            <h3>
                Hotel Information 酒店信息 (for booking purpose)<span class="clr-red icon-point">●</span></h3>
        </td>
    </tr>
    <tr>
        <td colspan="6" class="value p5">
            Note: If you join a meeting or function which hotel is arranged by the organizer,
            ie. Company agent need not book hotel for you, please tick here
            <asp:CheckBox ID="cbIsBookHotel" runat="server" Width="32" />
            and you do not have to fill-in the information requested below.
            <br />
            <span class="zh-height">如果有邀请方安排酒店事宜，不需要公司帮助预订请在方框中打钩，无须填写以下信息 </span>
        </td>
    </tr>
    <tr>
        <td colspan="6">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="inner-table">
                        <tr>
                            <td class="cell align-center" width="36">
                                <asp:ImageButton runat="server" ID="btnAddHotel" ToolTip="Click to add the hotel information."
                                    ImageUrl="../images/pixelicious_001.png" OnClick="btnAddHotel_Click" Width="18"
                                    CssClass="img-button" />
                            </td>
                            <td class="label align-center w20">
                                City
                            </td>
                            <td class="label align-center w20">
                                Name of Hotel
                                <br />
                                (if known)
                            </td>
                            <td class="label align-center w20">
                                Check-in Date
                            </td>
                            <td class="label align-center w20">
                                Check-out Date
                            </td>
                            <td class="value align-center">
                                No. of nights
                            </td>
                        </tr>
                        <asp:Repeater ID="rptHotel" runat="server" OnItemCommand="rptHotel_ItemCommand" OnItemDataBound="rptHotel_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td class="cell align-center">
                                        <asp:ImageButton ID="btnDeleteHotel" ToolTip="Remove this hotel information" CommandName="delete"
                                            runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                                    </td>
                                    <td class="label align-center">
                                        <asp:TextBox ID="txtCity" runat="server" CssClass="HotelInfomation" />
                                    </td>
                                    <td class="label align-center">
                                        <asp:TextBox ID="txtHotelName" runat="server" CssClass="HotelInfomation" />
                                    </td>
                                    <td class="label align-center">
                                        <cc1:CADateTimeControl ID="dtCheckInDate" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" />
                                    </td>
                                    <td class="label align-center">
                                        <cc1:CADateTimeControl ID="dtCheckOutDate" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" />
                                    </td>
                                    <td class="value align-center">
                                        <asp:TextBox ID="txtTotalNights" runat="server" CssClass="HotelInfomation HotelInfomation_Number" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnClearHotelInfo" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td colspan="6" class="p5">
            <table>
                <tr>
                    <td class="p200">
                        Please state special requests, if any:
                    </td>
                    <td class="p450">
                        <QFL:FormField ID="ffHotelRemark" runat="server" FieldName="HotelRemark" />
                    </td>
                </tr>
            </table>
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
<asp:Button ID="btnPeopleInfo" runat="server" OnClick="btnPeopleInfo_Click" CausesValidation="False"
    CssClass="hidden" />
<asp:Button ID="btnClearHotelInfo" runat="server" OnClick="btnClearHotelInfo_Click"
    CausesValidation="False" CssClass="hidden" />
<%--<asp:Button runat="server" ID="btnReloadTravelPolicy" OnClick="btnReloadTravelPolicy_Click"
    Text="Reload" CausesValidation="False" CssClass="hidden" />--%>
<asp:Button runat="server" ID="btnReloadCostPolicy" OnClick="btnReloadCostPolicy_Click"
    Text="Reload" CausesValidation="False" CssClass="hidden" />
<asp:HiddenField runat="server" ID="hidMixedLocation" />
<script type="text/javascript">
    var travelPeriod;
    var travelLocation;
    var travelLimit;
    var caTravel = {};

    function DisableEnterKey() {
        $('#<%=this.UpdatePanel1.ClientID %>' + ' input').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('#<%=this.UpdatePanel1.ClientID %>').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('#<%=this.UpdatePanel2.ClientID %>' + ' input').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('#<%=this.UpdatePanel2.ClientID %>').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('#<%=this.UpdatePanel3.ClientID %>' + ' input').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('#<%=this.UpdatePanel3.ClientID %>').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('#<%=this.UpdatePanel4.ClientID %>' + ' input').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('#<%=this.UpdatePanel4.ClientID %>').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('div.main-body').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
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

    function commafy(num) {
        num = num + "";
        var tmpArr = num.split('.');
        var re = /(-?\d+)(\d{3})/
        while (re.test(tmpArr[0])) {
            tmpArr[0] = tmpArr[0].replace(re, "$1,$2")
        }
        return tmpArr.length >= 2 ? tmpArr[0] + '.' + tmpArr[1] : tmpArr[0];
    }

    function Escape(s) {
        var re = /'/
        while (re.test(s)) {
            s = s.replace(re, "");
        }
        return s;
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
    if (!Number.prototype.toFixed) {
        Number.prototype.toFixed = function (n) {
            with (Math) return round(Number(this) * pow(10, n)) / pow(10, n)
        }
    }

    $(function () {

        SetTravel();
        $('.DateTimeControl').attr('contentEditable', 'false'); //There are 5 date time controls
        if ($('#<%= this.ffPurpose.ClientID %>_ctl00_DropDownChoice').val().indexOf('Others') === 0) {
            $('#ca-other-purpose').removeClass('hidden');
        }
        if ($('#ca-travel-iscashadvance :radio').eq(1).is(':checked')) {
            $('#ca-travel-cashadvance').addClass('hidden');
            $('#ca-travel-cashadvance').find('input').val(0);
        }
        if ($('#<%= this.cbNextFlight.ClientID %>').is(':checked')) {
            $('#ddlAvailableFlight').removeClass('hidden');
        }

        TravelDataBind();

        HotelDataBind();

        CostCenterBund();

        DisableEnterKey();
    });

    function CostCenterBund() {
        $('#<%=this.UpdatePanel1.ClientID %> select.width-fix')
            .bind('focus mouseover', function () { $(this).addClass('expand').removeClass('clicked'); })
            .bind('click', function () { if ($(this).hasClass('clicked')) { $(this).blur(); } $(this).toggleClass('clicked'); })
            .bind('mouseout', function () { if (!$(this).hasClass('clicked')) { $(this).removeClass('expand'); } })
            .bind('blur', function () { $(this).removeClass('expand clicked'); });
    }


    $('#<%= this.ffPurpose.ClientID %>_ctl00_DropDownChoice').change(function () {
        var otherPurpose = $('#ca-other-purpose')
        if ($(this).val().indexOf('Others') === 0) {
            otherPurpose.removeClass('hidden');
        }
        else {
            otherPurpose.addClass('hidden');
        }
    });

    $('#<%= this.cbChosenFlight.ClientID %>').click(function () {
        if ($(this).is(':checked')) {
            $('#<%= this.cbNextFlight.ClientID %>').removeAttr('checked', 'checked');
            $('#ddlAvailableFlight').addClass('hidden');
        }
    });

    $('#<%= this.cbNextFlight.ClientID %>').click(function () {
        var ddl = $('#ddlAvailableFlight');
        if ($(this).is(':checked')) {
            ddl.removeClass('hidden');
            $('#<%= this.cbChosenFlight.ClientID %>').removeAttr('checked', 'checked');
        }
        else {
            ddl.addClass('hidden');
        }
    });

    $('#ca-travel-iscashadvance :radio').change(function () {
        var cashadvance = $('#ca-travel-cashadvance');
        if ($(this).val() === 'ctl00') {
            cashadvance.removeClass('hidden');
            cashadvance.find('input').val('');
        } else {
            cashadvance.addClass('hidden');
            cashadvance.find('input').val(0);
        }
    });

    //    $('.ca-travel-vehicle select').live('change', function () {
    //        //        var preId = $(this).attr('id').substring(0, 65);
    //        //        var txtVehicleCost = $('#' + preId + 'txtVehicleCost');
    //        //        if ($(this).val() != 'Flight') {
    //        //            txtVehicleCost.attr('preVal', txtVehicleCost.val());
    //        //            txtVehicleCost.val('0');            
    //        //        } else if (!ca.util.emptyString(txtVehicleCost.attr('preVal'))) {
    //        //            txtVehicleCost.val(txtVehicleCost.attr('preVal'));
    //        //        }
    //        //        txtVehicleCost.change();
    //        var preId = $(this).attr('id').substring(0, 65);
    //        var txtVehicleCost = $('#' + preId + 'txtVehicleCost');
    //        txtVehicleCost.val('0');
    //        txtVehicleCost.change();
    //    });
    function OnChangeVehicle(obj) {
        var preId = obj.id.substring(0, 65);
        var $txtVehicleCost = $('#' + preId + 'txtVehicleCost');

        if ($('#' + preId + 'hidCost').val() == "") { return; }
        var hidValue = eval('(' + $('#' + preId + 'hidCost').val() + ')');

        //debugger
        $txtVehicleCost.val($('#' + preId + 'ddlVehicle').val() == "Flight" ? hidValue.Flight : '0');
        $txtVehicleCost.change();
    }
    $('#<%=this.UpdatePanel1.ClientID %>' + ' input:hidden').live('change', function () {
        var preId = $(this).attr('id').substring(0, 65);
        $('#' + preId + 'txtVehicleCost').change();
    });

    $('#<%=this.UpdatePanel1.ClientID %>' + ' input[class^=\'TravelInformation_Number\']').live('change', function () {

        var preId = $(this).attr('id').substring(0, 65);
        var txtVehicleCostId = '#' + preId + 'txtVehicleCost';
        var txtHotelCostId = '#' + preId + 'txtHotelCost';
        var txtMealCostId = '#' + preId + 'txtMealCost';
        var txtTransportationCostId = '#' + preId + 'txtTransportationCost';
        var txtSampleId = '#' + preId + 'txtSample';
        var txtOtherCostId = '#' + preId + 'txtOtherCost';
        var subTotal = '#' + preId + 'lblTotalCost';

        var temp = 0;
        var txtVehicleCostVal = $(txtVehicleCostId).val();
        var txtHotelCostVal = $(txtHotelCostId).val();
        var txtMealCostVal = $(txtMealCostId).val();
        var txtTransportationCostVal = $(txtTransportationCostId).val();
        var txtSampleVal = $(txtSampleId).val();
        var txtOtherCostVal = $(txtOtherCostId).val();
        temp += !isNaN(txtVehicleCostVal) && txtVehicleCostVal.length > 0 ? parseFloat(txtVehicleCostVal) : 0;
        temp += !isNaN(txtHotelCostVal) && txtHotelCostVal.length > 0 ? parseFloat(txtHotelCostVal) : 0;
        temp += !isNaN(txtMealCostVal) && txtMealCostVal.length > 0 ? parseFloat(txtMealCostVal) : 0;
        temp += !isNaN(txtTransportationCostVal) && txtTransportationCostVal.length > 0 ? parseFloat(txtTransportationCostVal) : 0;
        temp += !isNaN(txtSampleVal) && txtSampleVal.length > 0 ? parseFloat(txtSampleVal) : 0;
        temp += !isNaN(txtOtherCostVal) && txtOtherCostVal.length > 0 ? parseFloat(txtOtherCostVal) : 0;
        $(subTotal).text(commafy(parseFloat(temp).toFixed(0)));

        CalcSubTotal();
    });

    function CalcSubTotal() {
        var allTotal = '#<%= this.lblTotalBudget.ClientID %>';
        var temp = 0;
        $('#<%=this.UpdatePanel1.ClientID %>' + ' input[class^=\'TravelInformation_Number\']').each(function () {
            temp += !isNaN($(this).val()) && $(this).val().length > 0 ? parseFloat($(this).val()) : 0;
        });
        $(allTotal).text(commafy(parseFloat(temp).toFixed(0)));
    }

    $('#<%= this.cbIsBookHotel.ClientID %>').click(function () {
        if ($(this).is(':checked')) {
            $('#<%= this.btnClearHotelInfo.ClientID %>').click();
        }
    });

    $('#<%=this.btnAddHotel.ClientID %>').live('click', function () {
        if ($('#<%= this.cbIsBookHotel.ClientID %>').is(':checked')) {
            $('#<%= this.cbIsBookHotel.ClientID %>').attr('checked', false);
        }
    });

    function CheckReturn(obj) {
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_rptTravel_ctl00_txtTravelTo
        var preId = obj.id.substring(0, 65);
        caTravel.travelPreId = preId;
        var txtVehicleCost = $('#' + preId + 'txtVehicleCost');
        if (ca.util.emptyString(txtVehicleCost.val())) {
            return;
        }
        //
        if ($(obj).is(':checked')) {
            txtVehicleCost.val(txtVehicleCost.val() * 2);
        } else {
            txtVehicleCost.val(txtVehicleCost.val() / 2);
        }
        txtVehicleCost.change();
    }
    function Block(obj) {
        var objId = obj.id;
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_rptTravel_ctl00_txtTravelTo
        var temp = objId.substring(0, 65);
        var id = "0";

        var fromId = '#' + temp + "txtTravelFrom";
        var toId = '#' + temp + "txtTravelTo";

        var from = $(fromId).val();
        var to = $(toId).val();

        if (!ca.util.emptyString(to) || !ca.util.emptyString(from)) {
            $('#ctl00_PlaceHolderMain_ListFormControl1 input[value=\'Submit\']').attr('disabled', true);
            $('#ctl00_PlaceHolderMain_ListFormControl1 input[value=\'Save\']').attr('disabled', true);
        }
    }
    function UnBlock() {
        $('#ctl00_PlaceHolderMain_ListFormControl1 input[value=\'Submit\']').attr('disabled', false);
        $('#ctl00_PlaceHolderMain_ListFormControl1 input[value=\'Save\']').attr('disabled', false);
    }

    function ReturnCost(obj) {
        var objId = obj.id;
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_rptTravel_ctl00_txtTravelTo
        var temp = objId.substring(0, 65);
        var id = "0";

        var fromId = '#' + temp + "txtTravelFrom";
        var toId = '#' + temp + "txtTravelTo";
        var txtHotelCostId = "txtHotelCost";
        var txtMealCostId = "txtMealCost";
        var txtVehicleCostId = "txtVehicleCost";
        var ddlVehicleId = '#' + temp + "ddlVehicle";
        var returnVehicleId = '#' + temp + 'returnVehicle';
        var fromDateId = temp + 'dtTravelDateFrom_dtTravelDateFromDate';
        var toDateId = temp + 'dtTravelDateTo_dtTravelDateToDate';
        var areaId = '#' + temp + 'ddlArea';

        
        var from = Escape($(fromId).val());
        var to = Escape($(toId).val());
        var hidCost = $('#' + temp + 'hidCost').val();

        if (!(ca.util.emptyString(to) || ca.util.emptyString(from))
        && $('#' + temp + 'ddlArea').val() === '') {
            return alert('Please select area option.');
        }

        if (!(ca.util.emptyString(to) || ca.util.emptyString(from))) {
            //SetTravel();

            var $curDateFrom = $('#' + temp + 'dtTravelDateFrom_dtTravelDateFromDate');
            var $curDateTo = $('#' + temp + 'dtTravelDateTo_dtTravelDateToDate');
            var mealDays = "1";
            var travelDays = "1";

            if (hidCost != "") {
                var hidValue = eval('(' + hidCost + ')');
                id = hidValue.ID;
            } else {
                travelPeriod.push({ "DateFrom": $curDateFrom.val(), "DateTo": $curDateTo.val() });
                travelLocation.push({ "LocationFrom": from, "LocationTo": to });
                id = travelPeriod.length - 1;
            }
            var isMarkReturn = $(returnVehicleId).is(':checked');
            var area = $(areaId).val();


            if (!(ca.util.emptyString($curDateFrom.val()) || ca.util.emptyString($curDateTo.val()))) {
                if (id == "0") {
                    mealDays = (new Date($curDateTo.val()) - new Date($curDateFrom.val())) / (24 * 60 * 60 * 1000) + 1;
                    travelDays = (new Date($curDateTo.val()) - new Date($curDateFrom.val())) / (24 * 60 * 60 * 1000);
                } else {
                    mealDays = GetMealDays(parseInt(id));
                    travelDays = GetTravelDays(parseInt(id));
                }
            }


            var parmas = from + '|' + to + '|' + temp + '|' + txtHotelCostId + '|' + txtMealCostId + '|' + txtVehicleCostId + '|' + isMarkReturn + '|' + area + '|' + mealDays + '|' + travelDays;
            //When the FROM and TO fields are all filled and the VEHICLE item is Flight, the 3 places should be updated.
            //Otherwise when the TO field is filled, only 2 places should be updated
            $('#<%= this.hidMixedLocation.ClientID %>').val(parmas);

            $('#<%= this.btnReloadCostPolicy.ClientID %>').click();

        }
    }

    function SetBorderWarn(obj) {
        obj.css('border', '1px solid red');
    }
    function ClearBorderWarn(obj) {
        obj.css('border', '');
        obj.css('border-bottom', '#999 1px solid');
    }

    function SetBorderWarnByJS(obj) {
        obj.style.border = '1px solid red';
    }
    function ClearBorderWarnByJS(obj) {
        obj.style.border = '';
        obj.style.border = '#999 1px solid';
    }

    function EmptyString(fieldName, clientId, addition) {
        var msg = '';
        if (ca.util.emptyString($('#' + clientId + addition).val())) {
            msg = 'Please fill in the ' + fieldName + ' field.\n';
            SetBorderWarn($('#' + clientId + addition));
        } else {
            ClearBorderWarn($('#' + clientId + addition));
        }
        return msg;
    }
    function Validate() {
        $('#<%=this.UpdatePanel1.ClientID %>' + ' .DateTimeControl').unbind();
        /*******************************************/
        var startDate;
        var timediff = '';
        var endDate;
        var df;
        /*******************************************/
        var error = '';
        var fields =
        [
            { "fieldName": "Choose Employee", "clientId": "<%=this.cpfUser.ClientID%>", "addition": "" },
            { "fieldName": "ID/Passport No.", "clientId": "<%=this.ffIDNumber.ClientID%>", "addition": "_ctl00_ctl00_TextField" },
            { "fieldName": "Mobile Phone No.", "clientId": "<%=this.ffMobile.ClientID%>", "addition": "_ctl00_ctl00_TextField" },
            { "fieldName": "Office Ext. No.", "clientId": "<%=this.ffOfficeExt.ClientID%>", "addition": "_ctl00_ctl00_TextField" },
            { "fieldName": "The amount of Cash Advance required", "clientId": "<%=this.ffCashAdvanced.ClientID%>", "addition": "_ctl00_ctl00_TextField" }
         ];
        for (var index in fields) {
            error += EmptyString(fields[index]["fieldName"], fields[index]["clientId"], fields[index]["addition"]);
        }

        caTravel.FlightCount = 0;
        $('#<%=this.UpdatePanel1.ClientID %>' + ' select[class=\'VehicleInfomation\']').each(function () {
            if ($(this).val() === 'Flight') {
                caTravel.FlightCount++;
            }
        });
        if ($('#<%=this.cbChosenFlight.ClientID %>').is(':checked') == false
            && $('#<%=this.cbNextFlight.ClientID %>').is(':checked') == false
            && caTravel.FlightCount > 0) {
            error += "Please choose Business class or Others.\n";
            SetBorderWarn($('#<%=this.cbChosenFlight.ClientID %>'));
            SetBorderWarn($('#<%=this.cbNextFlight.ClientID %>'));
        } else {
            ClearBorderWarn($('#<%=this.cbChosenFlight.ClientID %>'));
            ClearBorderWarn($('#<%=this.cbNextFlight.ClientID %>'));
        }

        /*******************************************/
        startDate = new Date();
        /*******************************************/
        /*******************************************/
        endDate = new Date();
        df = (endDate.getTime() - startDate.getTime()) / 1000;
        timediff += 'start:' + df + '.\n';
        /*******************************************/
        var flag = 0;
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_UpdatePanel1		
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_rptTravel_ctl00_dtTravelDateFrom_dtTravelDateFromDate
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_rptTravel_ctl00_dtTravelDateTo_dtTravelDateToDate
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_rptTravel_ctl00_txtTravelFrom
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_rptTravel_ctl00_txtTravelTo	
        /*******************************************/
        startDate = new Date();
        /*******************************************/
        caTravel.panelId = '<%=this.UpdatePanel1.ClientID %>';
        caTravel.preTmpId = caTravel.panelId.substring(0, 49) + 'rptTravel_ctl';
        caTravel.lines = $('#<%=this.UpdatePanel1.ClientID %>' + ' input:hidden').length;
        caTravel.arr = new Array();
        /*******************************************/
        endDate = new Date();
        df = (endDate.getTime() - startDate.getTime()) / 1000;
        timediff += 'before for:' + df + '.\n';
        /*******************************************/
        for (caTravel.i = 0; caTravel.i < caTravel.lines; caTravel.i++) {
            /*******************************************/
            startDate = new Date();
            /*******************************************/
            caTravel.preControlId = caTravel.preTmpId + (100 + caTravel.i + '').substring(1, 3) + '_';
            caTravel.dateFromId = caTravel.preControlId + 'dtTravelDateFrom_dtTravelDateFromDate';
            caTravel.dateToId = caTravel.preControlId + 'dtTravelDateTo_dtTravelDateToDate';
            caTravel.locFromId = caTravel.preControlId + 'txtTravelFrom';
            caTravel.locToId = caTravel.preControlId + 'txtTravelTo';
            caTravel.arr.push(document.getElementById(caTravel.dateFromId));
            caTravel.arr.push(document.getElementById(caTravel.dateToId));
            caTravel.arr.push(document.getElementById(caTravel.locFromId));
            caTravel.arr.push(document.getElementById(caTravel.locToId));
            while (caTravel.arr.length > 0) {
                caTravel.obj = caTravel.arr.pop();
                if (ca.util.emptyString(caTravel.obj.value)) {
                    flag++;
                    SetBorderWarnByJS(caTravel.obj);
                } else {
                    ClearBorderWarnByJS(caTravel.obj);
                }
            }
            /*******************************************/
            endDate = new Date();
            df = (endDate.getTime() - startDate.getTime()) / 1000;
            timediff += 'for[' + caTravel.i + ']:' + df + '.\n';
            /*******************************************/
        }
        error += flag > 0 ? 'Please fill in the TravelInformation details.\n' : '';

        var flag = 0;
        $('#<%=this.UpdatePanel1.ClientID %>' + ' select[class^=\'TravelInformation\']').each(function () {
            if (ca.util.emptyString($(this).val()) || $(this).val().length === 0) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please fill in the TravelInformation details.\n' : '';

        flag = 0;
        $('#<%=this.UpdatePanel1.ClientID %>' + ' input[class^=\'TravelInformation_Number\']').each(function () {
            if (ca.util.emptyString($(this).val()) || isNaN($(this).val()) || $(this).val() < 0) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please fill the valid number in TravelInformation Cost details.\n' : '';

        //validate vehicle cost that must be more than 0
        flag = 0;
        $('#<%=this.UpdatePanel1.ClientID %>' + ' input[class$=\'VehicleCostValidate\']').each(function () {
            var vehicleCost = $(this).val();
            if (!(!isNaN(vehicleCost) && vehicleCost.length > 0 && parseFloat(vehicleCost) > 0)) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please supply the Vehicle Cost which is more than 0.\n' : '';

        flag = 0;
        $('#<%=this.UpdatePanel3.ClientID %>' + ' input[class^=\'VehicleInfomation\']').each(function () {
            if (ca.util.emptyString($(this).val())) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please fill in the VehicleInfomation details.\n' : '';

        flag = 0;
        $('#<%=this.UpdatePanel2.ClientID %>' + ' input[class^=\'HotelInfomation\']').each(function () {
            if (ca.util.emptyString($(this).val())) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please fill in the HotelInfomation details.\n' : '';

        flag = 0;
        $('#<%=this.UpdatePanel2.ClientID %>' + ' input[class$=\'HotelInfomation_Number\']').each(function () {
            if (ca.util.emptyString($(this).val())) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please fill in the valid number HotelInfomation details.\n' : '';

        if ($('#<%= this.ffPurpose.ClientID %>_ctl00_DropDownChoice').val().indexOf('Others') === 0) {
            if (ca.util.emptyString($('#ca-other-purpose input').val())) {
                error += 'Please fill in the Other Purpose.\n';
                SetBorderWarn($('#ca-other-purpose input'));
            } else {
                ClearBorderWarn($('#ca-other-purpose input'));
            }
        }

        //alert(timediff);
        if (error) {
            alert(error);
            TravelDataBind();
        }

        //return error.length === 0;

        var returnResult=error.length === 0
        if (returnResult) {
            CreateForbidDIV(); //单击生成弹出层，防止重复提交。
        }
        return returnResult;

    }

    function beforeSubmit() {
        return Validate();
    }

    $(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });


    function EndRequestHandler() {

        $('.DateTimeControl').attr('contentEditable', 'false'); //There are 5 date time controls

        TravelDataBind();

        HotelDataBind();

        CostCenterBund();

        DisableEnterKey();

        $('#<%=this.UpdatePanel1.ClientID %>' + ' input:hidden').change();

        setTimeout('UnBlock()', 500);

    }

    function HotelDataBind() {
        $('#<%=this.UpdatePanel2.ClientID %>' + ' .DateTimeControl').bind('propertychange', function () {
            if (event.propertyName != 'value') {
                return;
            }
            var preId = $(this).attr('id').substring(0, 64);
            var checkOut = $('#' + preId + 'dtCheckOutDate_dtCheckOutDateDate').val();
            var checkIn = $('#' + preId + 'dtCheckInDate_dtCheckInDateDate').val();

            if (ca.util.emptyString(checkOut)
            || ca.util.emptyString(checkIn)) {
                return;
            }

            if ((new Date(checkOut) - new Date(checkIn)) < 0) {
                $('#' + preId + 'dtCheckOutDate_dtCheckOutDateDate').val("");
                return alert("Check-Out Date must be more than Check-In Date.");
            }

            var days = (new Date(checkOut) - new Date(checkIn)) / (24 * 60 * 60 * 1000);
            $('#' + preId + 'txtTotalNights').val(days >= 0 ? days : 0);
        });
    }

    function TravelDataBind() {
        $('#<%=this.UpdatePanel1.ClientID %>' + ' .DateTimeControl').bind('propertychange', function () {
            if (event.propertyName != 'value') {
                return;
            }

            var preId = $(this).attr('id').substring(0, 65);
            var curDateFrom = $('#' + preId + 'dtTravelDateFrom_dtTravelDateFromDate').val();
            var curDateTo = $('#' + preId + 'dtTravelDateTo_dtTravelDateToDate').val();
            var curLocationFrom = Escape($('#' + preId + 'txtTravelFrom').val());
            var curLocationTo = Escape($('#' + preId + 'txtTravelTo').val());
            if (ca.util.emptyString(curDateFrom)
            || ca.util.emptyString(curDateTo)) {
                return;
            }
            
            if ((new Date(curDateTo) - new Date(curDateFrom)) < 0) {
                $('#' + preId + 'dtTravelDateTo_dtTravelDateToDate').val("");
                return alert("End Date must be more than Start Date.");
            }

            if ($('#' + preId + 'hidCost').val() == "") { return; }

            var hidValue = eval('(' + $('#' + preId + 'hidCost').val() + ')');
            var id = hidValue.ID;
            if (isNaN(id) || id == null) { return; }

            if (travelLimit.length == 0) {
                SetTravel();
            }
            var sourceLimit = eval(travelLimit);
            var sourceHotelLimit = parseFloat(sourceLimit[id].HotelLimit);
            var sourceMealLimit = parseFloat(sourceLimit[id].MealLimit);
            //SetTravel();

            var hotelLimit = parseFloat(hidValue.HotelLimit);
            var mealLimit = parseFloat(hidValue.MealLimit);
            var vehicleCost = $('#' + preId + 'ddlVehicle').val() == "Flight" ? parseFloat(hidValue.Flight) : 0;

            var sourceDateFrom = hidValue.DateFrom != curDateFrom ? hidValue.DateFrom : curDateFrom;
            var sourceDateTo = hidValue.DateTo != curDateTo ? hidValue.DateTo : curDateTo;
            var sourceDays = (new Date(FormatDate(sourceDateTo)) - new Date(FormatDate(sourceDateFrom))) / (24 * 60 * 60 * 1000);

            hidValue.DateFrom = curDateFrom;
            hidValue.DateTo = curDateTo;

            $('#' + preId + 'hidCost').val(JsonToStr(hidValue));

            SetTravel();

            var mealDays = GetMealDays(parseInt(id));
            var travelDays = GetTravelDays(parseInt(id));
            var $txtMealCost = $('#' + preId + 'txtMealCost');
            var $txtHotelCost = $('#' + preId + 'txtHotelCost');

            if (sourceMealLimit != mealLimit || FormatDate(sourceDateFrom) != curDateFrom || FormatDate(sourceDateTo) != curDateTo
            //            mealLimit != sourceMealLimit
            //            || mealLimit * (sourceDays + 1) == parseFloat($txtMealCost.val()) 
            //            || $txtMealCost.val() == mealLimit
                ) {
                $txtMealCost.val(mealDays > 0 ? mealDays * mealLimit : 0);
            }
            if (sourceHotelLimit != hotelLimit || FormatDate(sourceDateFrom) != curDateFrom || FormatDate(sourceDateTo) != curDateTo
            //            hotelLimit != sourceHotelLimit
            //            || hotelLimit * sourceDays == parseFloat($txtHotelCost.val()) 
            //            || $txtHotelCost.val() == hotelLimit
                ) {
                $txtHotelCost.val(travelDays > 0 ? travelDays * hotelLimit : 0);
            }
            if (hidValue.LocationFrom != curLocationFrom || hidValue.LocationTo != curLocationTo) {
                $('#' + preId + 'txtVehicleCost').val(vehicleCost);
            }


            $('#<%=this.UpdatePanel1.ClientID %>' + ' input:hidden').change();


        });
    }
    function GetMealDays(id) {

        var json = eval(travelPeriod);
        var curDateFrom = json[id].DateFrom;
        var curDateTo = json[id].DateTo;
        var json2 = eval(travelLocation);
        var curLocationTo = json2[id].LocationTo;
        var LocationFrom = json2[0].LocationFrom;

        if (curLocationTo == LocationFrom) {
            return 0;
        }

        if (id != 0) {
            var prevDateFrom = json[id - 1].DateFrom;
            var prevDateTo = json[id - 1].DateTo;
            if (prevDateTo == curDateFrom) {
                return (new Date(curDateTo) - new Date(curDateFrom)) / (24 * 60 * 60 * 1000);
            }
        }

        return (new Date(curDateTo) - new Date(curDateFrom)) / (24 * 60 * 60 * 1000) + 1;
    }

    function GetTravelDays(id) {

        var json = eval(travelLocation);
        var json2 = eval(travelPeriod);
        var curLocationTo = json[id].LocationTo;
        var curDateFrom = json2[id].DateFrom;
        var curDateTo = json2[id].DateTo;
        var days = ((new Date(curDateTo) - new Date(curDateFrom)) / (24 * 60 * 60 * 1000)) > 0 ? (new Date(curDateTo) - new Date(curDateFrom)) / (24 * 60 * 60 * 1000) : 0;

        var LocationFrom = json[0].LocationFrom;

        if (curLocationTo == LocationFrom) {
            return 0;
        }

        return days;
    }

    $('#<%=this.cpfUser.ClientID %>' + '_checkNames').click(function () {
        setTimeout('$("#<%=this.btnPeopleInfo.ClientID %>").click()', 2000);
    });

    function SetTravel() {

        var $hidCostItems = $('#<%=this.UpdatePanel1.ClientID %>' + ' input:hidden');
        travelPeriod = new Array();
        travelLocation = new Array();
        travelLimit = new Array();

        for (var i = 0; i < $hidCostItems.length; i++) {
            if (ca.util.emptyString($hidCostItems.eq(i).val())) { continue; }
            var tmp = eval('(' + $hidCostItems.eq(i).val() + ')');
            travelPeriod.push({ "DateFrom": tmp.DateFrom, "DateTo": tmp.DateTo });
            travelLocation.push({ "LocationFrom": tmp.LocationFrom, "LocationTo": tmp.LocationTo });
            travelLimit.push({ "HotelLimit": tmp.HotelLimit, "MealLimit": tmp.MealLimit });
        }
        $hidCostItems.change();
    }


    //    $('#<%=this.cpfUser.ClientID %>' + '_checkNames').click(function () {
    //        setTimeout('$("#<%=this.btnPeopleInfo.ClientID %>").click()', 2000);
    //    });
    
</script>
