<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.TravelRequest2.DataView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<table class="ca-workflow-form-table">
    <tr>
        <td class="label align-center w25">
            Employee<br />
        </td>
        <td class="value">
            <QFL:FormField ID="Applicant" runat="server" FieldName="Applicant" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
</table>
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
            <QFL:FormField ID="ChineseName" runat="server" FieldName="ChineseName" ControlMode="Display">
            </QFL:FormField>
        </td>
        <td class="label align-center w25">
            <QFL:FormField ID="EnglishName" runat="server" FieldName="EnglishName" ControlMode="Display">
            </QFL:FormField>
        </td>
        <td class="label align-center w25">
            <QFL:FormField ID="IDNumber" runat="server" FieldName="IDNumber" ControlMode="Display">
            </QFL:FormField>
        </td>
        <td class="label align-center w25">
            <QFL:FormField ID="Department" runat="server" FieldName="Department" ControlMode="Display">
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
                        <QFL:FormField ID="Mobile" runat="server" FieldName="Mobile" ControlMode="Display">
                        </QFL:FormField>
                    </td>
                    <td class="label align-center">
                        Office Ext. No.<br />
                        分机号码
                    </td>
                    <td class="value align-center w15">
                        <QFL:FormField ID="OfficeExt" runat="server" FieldName="OfficeExt" ControlMode="Display">
                        </QFL:FormField>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table">
    <tr>
        <td class="value align-center" colspan="3">
            <h3>
                Travel Information & Budget 出差信息及预算</h3>
        </td>
    </tr>
    <tr>
        <td class="value w15 align-right">
            Travel Purpose:
        </td>
        <td class="value w35" id="ca-travelpurpose">
            <QFL:FormField ID="ffPurpose" runat="server" FieldName="TravelPurpose" ControlMode="Display">
            </QFL:FormField>
        </td>
        <td class="value">
            <div class="hidden" id="ca-other-purpose">
                <QFL:FormField ID="TravelOtherPurpose" runat="server" FieldName="TravelOtherPurpose"
                    ControlMode="Display">
                </QFL:FormField>
            </div>
        </td>
    </tr>
    <tr>
        <td class="value align-right w15">
            Remark:
        </td>
        <td class="value" colspan="2">
            <QFL:FormField ID="TravelRemark" runat="server" FieldName="TravelRemark" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td class="value align-right w15">
            Attachment:
        </td>
        <td class="value" colspan="2">
            <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Display">
            </QFL:FormAttachments>
        </td>
    </tr>
    <tr>
        <td class="value align-right" colspan="3">
            <!-- empty -->
        </td>
    </tr>
    <tr>
        <td colspan="3">
            

            <table id="ca-travel-detail" class="inner-table">
                <tr>
                    <td class="cell align-center" width="36">
                    </td>
                    <td class="label align-center" width="25%">
                        Travel Period
                    </td>
                    <td class="label align-center" width="20%">
                        Travel Location
                    </td>
                    <td class="label align-center" width="15%">
                        Cost Center
                    </td>
                    <td class="label align-center" width="25%">
                        Cost Item
                    </td>
                    <td class="value align-center" width="13%">
                        Estimated Cost
                    </td>
                </tr>
                <asp:Repeater ID="rptTravel" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td class="cell align-center" width="36">
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
                                            <%# Eval("TravelDateFrom","{0:d}") %>
                                        </td>
                                        <td class="align-center">
                                            <%# Eval("TravelDateTo","{0:d}")%>
                                        </td>
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
                                            <%# Eval("TravelLocationFrom")%>
                                        </td>
                                        <td class="align-center">
                                            <%# Eval("TravelLocationTo")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="cell align-center costcenter">
                                <%# Eval("CostCenter")%>
                            </td>
                            <td class="cell w15">
                                <table class="inner-table">
                                    <tr>
                                        <td class="last-cell h30 p5">
                                            <%# Eval("VehicleCostItem")%>
                                            <span class="ca-travel-returnVehicle">
                                                <%# Eval("ReturnVehicleCostItem")%>
                                            </span>
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
                                            <%# Eval("OthersCostItem") %>
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
                                <table class="cost-table inner-table">
                                    <tr>
                                        <td class="last-cell h30 p5 value-view">
                                            <%# string.Format("{0:N0}", Eval("VehicleEstimatedCost"))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="last-cell h30 p5 value-view">
                                            <%# string.Format("{0:N0}",Eval("HotelEstimatedCost"))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="last-cell h30 p5 value-view">
                                            <%# string.Format("{0:N0}",Eval("MealEstimatedCost"))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="last-cell h30 p5 value-view">
                                            <%# string.Format("{0:N0}",Eval("LocalTransportationEstimatedCost"))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="last-cell h30 p5 value-view">
                                            <%# string.Format("{0:N0}",Eval("SamplePurchaseCost"))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="last-cell h30 p5 value-view">
                                            <%# string.Format("{0:N0}",Eval("OtherEstimatedCost"))%>
                                        </td>
                                    </tr>
                                    <tr class="last">
                                        <td class="last-cell h30 p5 subtotal">
                                            <%# string.Format("{0:N0}",Eval("TotalEstimatedCost"))%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td colspan="5" class="p5">
                        Total Budget of the business trip
                    </td>
                    <td class="totalbudget-view">
                        <QFL:FormField ID="TravelTotalCost" runat="server" FieldName="TravelTotalCost" ControlMode="Display">
                        </QFL:FormField>
                    </td>
                </tr>


                <tr>                
                    <td colspan="6">
                        <table class="inner-table">
                            <tr>
                                <td colspan="7" class="value align-right"></td>
                            </tr>
                            <tr>
                                <td class="cell align-center" width="14%">
                                    Vehicle
                                </td>
                                <td class="label align-center" width="14%">
                                    Hotel
                                </td>
                                <td class="label align-center" width="14%">
                                    Meal Allowance
                                </td>
                                <td class="label align-center" width="14%">
                                    Local Transportation
                                </td>
                                <td class="label align-center" width="14%">
                                    Sample Purchase
                                </td>
                                <td class="label align-center" width="14%">
                                    Others
                                </td>
                                <td id="ca-travel-summarytotal" class="value align-center" width="16%">
                                    Total
                                </td>
                            </tr>
                            <tr>
                                <td id="vehicle" class="cell align-center">
                                </td>
                                <td id="hotel" class="label align-center">
                                </td>
                                <td id="meal" class="label align-center">
                                </td>
                                <td id="transportation" class="label align-center">
                                </td>
                                <td id="sample" class="label align-center">
                                </td>
                                <td id="others" class="label align-center">
                                </td>
                                <td id="total" class="value align-center">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7" class="value align-right"></td>
                            </tr>
                        </table>    
                    </td>
                </tr>

                <tr>
                    <td colspan="5" class="p5">
                        Cash Advance:
                        <br />
                        预借现金
                    </td>
                    <td class="cashadvance">
                        <div id="ca-travel-iscashadvance" class="hidden">
                            <QFL:FormField ID="IsCashAdvanced" runat="server" FieldName="IsCashAdvanced" ControlMode="Display">
                            </QFL:FormField>
                        </div>
                        <div id="ca-travel-cashadvance">
                            <span style="display: none;">N/A</span>
                            <span>
                                <QFL:FormField ID="CashAdvanced" runat="server" FieldName="CashAdvanced" ControlMode="Display">
                                </QFL:FormField>
                            </span>
                        </div>
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
                Vehicle Information 交通工具信息 (for booking purpose)</h3>
        </td>
    </tr>
    <tr>
        <td class="cell align-center" width="36">
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
    <asp:Repeater ID="rptVehicle" runat="server">
        <ItemTemplate>
            <tr>
                <td class="cell align-center">
                </td>
                <td class="label align-center">
                    <%#Eval("VehicleCostItem")%>
                </td>
                <td class="label align-center">
                    <%#Eval("VehicleNumber")%>
                </td>
                <td class="label align-center">
                    <%#Eval("Date","{0:d}")%>
                </td>
                <td class="label align-center">
                    <%#Eval("VehicleFrom")%>
                </td>
                <td class="label align-center">
                    <%#Eval("VehicleTo")%>
                </td>
                <td class="value align-center">
                    <%#Eval("Time")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <tr>
        <td colspan="7" class="p5">
            <table>
                <tr id="ca-travel-flight">
                    <td>
                        The chosen flight:&nbsp;
                    </td>
                    <td>
                        <QFL:FormField ID="FormField1" runat="server" FieldName="FlightClass" ControlMode="Display">
                        </QFL:FormField>
                    </td>
                    <td class="hidden">
                        (
                        <QFL:FormField ID="FormField2" runat="server" FieldName="NextAvailableFlight" ControlMode="Display">
                        </QFL:FormField>
                        )
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="6" class="value align-center">
            <h3>
                Hotel Information 酒店信息 (for booking purpose)</h3>
        </td>
    </tr>
    <tr>
        <td class="cell align-center" width="36">
        </td>
        <td class="label align-center w20">
            City
        </td>
        <td class="label align-center w20">
             Name of Hotel <br />
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
    <asp:Repeater ID="rptHotel" runat="server">
        <ItemTemplate>
            <tr>
                <td class="cell align-center">
                </td>
                <td class="label align-center">
                    <%#Eval("City")%>
                </td>
                <td class="label align-center">
                    <%#Eval("HotelName")%>
                </td>
                <td class="label align-center">
                    <%#Eval("CheckInDate", "{0:d}")%>
                </td>
                <td class="label align-center">
                    <%#Eval("CheckOutDate", "{0:d}")%>
                </td>
                <td class="value align-center">
                    <%#Eval("TotalNights")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <tr>
        <td colspan="6" class="p5">
            Special requests:
            <QFL:FormField ID="HotelRemark" runat="server" FieldName="HotelRemark" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td colspan="6" class="p5">
            <div id="ca-hotel-bookresult">
                <span style="display: none;">Note: Company agent has to book hotel for you</span>
                <span>Note: Company agent won't book hotel for you</span>
            </div>
            <div id="ca-hotel-isbook" class="hidden">
                <QFL:FormField ID="IsBookHotel" runat="server" FieldName="IsBookHotel" ControlMode="Display">
                </QFL:FormField>
            </div>
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

    function decommafy(num) {
        num = num + '';
        return num.replace(/,/g, '');
    }

    function costcalc() {
        $costTable = $('.cost-table');
        var vehicle = 0;
        var hotel = 0;
        var meal = 0;
        var transportation = 0;
        var sample = 0;
        var others = 0;
        var total = 0;
        $costTable.each(function () {
            $tds = $(this).find('td');
            vehicle += parseFloat(decommafy(jQuery.trim($tds.eq(0).text())));
            hotel += parseFloat(decommafy(jQuery.trim($tds.eq(1).text())));
            meal += parseFloat(decommafy(jQuery.trim($tds.eq(2).text())));
            transportation += parseFloat(decommafy(jQuery.trim($tds.eq(3).text())));
            sample += parseFloat(decommafy(jQuery.trim($tds.eq(4).text())));
            others += parseFloat(decommafy(jQuery.trim($tds.eq(5).text())));
            total += parseFloat(decommafy(jQuery.trim($tds.eq(6).text())));
        });
        $('#vehicle').text(commafy(vehicle));
        $('#hotel').text(commafy(hotel));
        $('#meal').text(commafy(meal));
        $('#transportation').text(commafy(transportation));
        $('#sample').text(commafy(sample));
        $('#others').text(commafy(others));
        $('#total').text(commafy(total));
    }

    $(function () {
        if (jQuery.trim($('#ca-travelpurpose').text()).indexOf('Others') === 0) {
            $('#ca-travelpurpose').append('- ' + $('#ca-other-purpose').text());
        }

        if (jQuery.trim($('#ca-travel-iscashadvance').text()) === 'No') {
            $('#ca-travel-cashadvance span').toggle();
        }
        $('.inner-table .ca-travel-returnVehicle').each(function () {
            if (jQuery.trim($(this).text()) === '1') {
                $(this).text('(Round Trip)');
            } else {
                $(this).text('');
            }
        });
        if (jQuery.trim($('#ca-hotel-isbook').text()) === 'Yes') {
            $('#ca-hotel-bookresult span').toggle();
        }
        var travelFlight = $('#ca-travel-flight td');
        if (jQuery.trim(travelFlight.eq(1).text()).indexOf('Other') === 0) {
            travelFlight.eq(2).removeClass("hidden");
        }
        var $totalBudget = $('.totalbudget-view');
        $totalBudget.text(commafy(Math.round(decommafy(jQuery.trim($totalBudget.text())))));
        var $cashadvance = $('#ca-travel-cashadvance span').eq(1);
        $cashadvance.text(commafy(jQuery.trim($cashadvance.text())));

        costcalc();
    });

</script>
