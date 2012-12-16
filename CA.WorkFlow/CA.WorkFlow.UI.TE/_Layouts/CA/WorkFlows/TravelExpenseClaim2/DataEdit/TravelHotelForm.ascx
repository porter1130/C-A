<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TravelHotelForm.ascx.cs"
    Inherits="CA.WorkFlow.UI.TE.TravelHotelForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .HotelItem, .HotelRemark
    {
        display: none;
    }
    .HotelInsert
    {
        display: none;
    }
    li.Total input
    {
        border: none;
        color: #06c;
    }
    li.ComStd input
    {
        border: none;
        color: #06c;
        text-align: center;
        width: 90%;
    }
    .CostCenterContainer
    {
        height: 25px;
        position: relative;
        z-index: 1;
        background-color: White;
    }
    .CostCenterSub
    {
        position: absolute;
        left: 5px;
        top: 3px;
        z-index: 2;
    }
    .CostCenterTD
    {
        width: 270px;
    }
    .CurrencyContainer
    {
        position: relative;
        z-index: 10;
    }
    .CurrencySub
    {
        display: none;
        position: absolute;
        left: -1px;
        top: -6px;
        border: #999 1px solid;
        width: 180px;
        z-index: 10;
        background-color: White;
        line-height: 30px;
        padding: 20px;
    }
    .OtherCurrency
    {
        display: none;
    }
    .CurrencySub input
    {
        margin: 0px 0px 0px 10px;
        cursor: pointer;
    }
    .CurrencySub a
    {
        margin: 0px 15px 0px 10px;
        cursor: pointer;
        color: #06c;
        font-weight:700;
    }
    .CurrencySub a.Cancel
    {
        color: Red;
    }
</style>
<div class="Item_UL Hotel">
    <ul>
        <li class="w104 ImgText">
            <img src="../images/pixelicious_001.png" alt="Click to add the information." width="18"
                class="AddItem" align="absmiddle" />Hotel</li>
        <li class="w40 TitleText"></li>
        <li class="w20 Sub">Sub Total</li>
        <li class="w30 Total">
            <input type="text" value="" id="Hotel_SubTotal"  readonly="readonly"/></li>
    </ul>
</div>
<div class="Item_UL Hotel_Items HotelItem">
    <ul>
        <li class="w104 ExpenseType">
            <img src="../images/pixelicious_028.png" alt="Remove this information." width="18"
                class="DelItem" align="absmiddle" /><select class="ItemSelect" id="Hotel_ExpenseType0">
                    <option value="Select City">Select City</option>
                    <option value="Tier I City">Tier I City</option>
                    <option value="Tier II/III City">Tier II/III City</option>
                    <option value="HK/Oversea">HK/Oversea</option>
                </select></li>
        <li class="w15 Date" title="">
            <input type="text" class="Date FromDate" id="Hotel_FromDate0" value="" title="" readonly="readonly" />
            <img src="../../../CAResources/themeCA/images/selectcalendar.gif" align="absmiddle"
                alt="Select Date" />
            —
            <input type="text" class="Date ToDate" id="Hotel_ToDate0" value="" title="" readonly="readonly" />
            <img src="../../../CAResources/themeCA/images/selectcalendar.gif" align="absmiddle"
                alt="Select Date" />
        </li>
        <li class="w10 CostCenter">
            <div class="CostCenterContainer">
                <div class="CostCenterSub">
                    <select class="CostCenterCurrency CostCenter" id="Hotel_CostCenter0">
                        <option class="blank" value=""></option>
                    </select></div>
            </div>
        </li>
        <li class="w10 OriginalAmt">
            <input type="text" class="Date" value="" id="Hotel_OriginalAmt0" /></li>
        <li class="w10 Currency">
            <select class="CostCenterCurrency" id="Hotel_Currency0">
                <option class="blank" value=""></option>
            </select>
            <div class="CurrencyContainer">
                <div class="CurrencySub">
                    Other Currency<input type="text" class="Date" value="" /><br />
                    <a class="OK">OK</a><a class="Cancel">Select Currency List</a>
                </div>
            </div>
            <input type="text" class="Date OtherCurrency" id="Hotel_OtherCurrency0" value=""  readonly="readonly"/>
        </li>
        <li class="w10 ExchRate">
            <input type="text" class="Date" id="Hotel_ExchRate0" value="1.0000" /></li>
        <li class="w10 ClaimAmt">
            <input type="text" class="Date" id="Hotel_ClaimAmt0" value="" readonly="readonly"/></li>
        <li class="w9 bg ComStd">
            <input type="text" value="" id="Hotel_ComStd0"  readonly="readonly"/></li>
        <li class="w14 bg PaidCredit">
            <input type="checkbox" class="Date" id="Hotel_PaidCredit0" value="" /></li>
    </ul>
</div>
<div class="Item_Head_Content Hotel_Items HotelRemark">
    <div class="Item_Head_Left w104">
        Remark</div>
    <div class="Item_Head_Right w774">
        <input type="text" id="Hotel_HotelRemark0" value="" /></div>
</div>
<div class="Blank Hotel_Items">
    <div class="BlankLeft">
    </div>
    <div class="BlankRight">
    </div>
</div>
<div class="HotelInsert">
    <input type="hidden" id="Hotel_AddItemStatus" value="0" />
    <input type="hidden" id="Hotel_PaidCredit" value="0" />
</div>
<script type="text/javascript" src="JS/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        LoadHotel_CostCenter();
        LoadHotel_Currency();
        BindHotel_AddAndDelItemEvent();
        BindHotel_ExpenseTypeEvent();
        LoadHotel_InputDateEvent();
        BindHotel_DateEvent();
        BindHotel_CostCenterEvent();
        BindHotel_AmountEvent();
        BindHotel_CurrencyEvent();
        BindHotel_ExchRateEvent();
        BindHotel_HotelRemarkEvent();
        BindHotel_PaidCreditEvent();
        DrawHotelForm(); 
    });

    function LoadHotel_InputDateEvent() {
        var $Date = $("div.HotelItem ul li.Date input");
        $Date.bind("blur", function () {
            var ID = $(this).attr("id").replace("Hotel_FromDate", "").replace("Hotel_ToDate", "");
            var $Hotel_ComStd = $("#Hotel_ComStd" + ID);
            var $Hotel_FromDate = $("#Hotel_FromDate" + ID);
            var $Hotel_ToDate = $("#Hotel_ToDate" + ID);
            var $Hotel_ExpenseType = $("#Hotel_ExpenseType" + ID);
            var expenseType = $Hotel_ExpenseType.val();
            if ($Hotel_FromDate.val() != "" && $Hotel_ToDate.val() != "") {
                var StartDate = new Date(Date.parse($Hotel_FromDate.val())).getTime();
                var EndDate = new Date(Date.parse($Hotel_ToDate.val())).getTime();
                if (EndDate < StartDate) {
                    alert("The To Date be less than the From Date In The Hotel Area.");
                    $Hotel_FromDate.val("");
                    $Hotel_ToDate.val("");
                    $Hotel_ExpenseType.val("Select City");
                    $Hotel_ComStd.val("");
                    ShowOrHideHotelRemark(ID);
                    return;
                }
                var DateCount = Math.abs((StartDate - EndDate)) / (1000 * 60 * 60 * 24);
                if (DateCount == 0) {
                    DateCount = 1;
                }
                var $hfTravelPolicy = $("input[id$='hfTravelPolicy']");
                var json = $hfTravelPolicy.val();
                var travelPolicy = eval("(" + json + ")");
                if (expenseType != "HK/Oversea" && expenseType != "Select City") {
                    $.each(travelPolicy, function (i, item) {
                        if (item != undefined) {
                            if (item.Location == expenseType) {
                                $Hotel_ComStd.val(item.HotelLimit * DateCount);
                            }
                        }
                    });
                } else {
                    $Hotel_ComStd.val("");
                }
                ShowOrHideHotelRemark(ID);
            }
        });
    }

    function GetHotel_Currency_Option(text, val) {
        var option = "";
        option = "<option class=\"item\" value=\"" + text + "\" >" + val + "</option>";
        return option;
    }

    function LoadHotel_CostCenter() {
        var $CostCenter = $("div.HotelItem ul li.CostCenter select.CostCenter");
        var $Insert = $CostCenter.find("option.blank");
        var $hfCostCenter = $("input[id$='hfCostCenter']");
        var json = $hfCostCenter.val();
        if (json != "") {
            var costCenter = eval("(" + json + ")");
            var $blank = $("<option class=\"item\" value=\"\" ></option>");
            $Insert.before($blank);
            $.each(costCenter, function (i, item) {
                if (item != undefined) {
                    $InsertOption = $(GetHotel_Currency_Option(item.val, item.name));
                    $Insert.before($InsertOption);
                }
            });
            $CostCenter.find("option").remove(".blank");
        }
    }

    function LoadHotel_Currency() {
        var $Currency = $("div.HotelItem ul li.Currency select");
        var $Insert = $Currency.find("option.blank");
        var currency = ["RMB", "USD", "GBP", "EUR", "AUD", "CHF", "CAD", "JPY", "HKD", "Others"];
        for (var i = 0; i < currency.length; i++) {
            $InsertOption = $(GetHotel_Currency_Option(currency[i], currency[i]));
            $Insert.before($InsertOption);
        }
        $Currency.find("option").remove(".blank");
    }
</script>
<script type="text/javascript">
    var Hotel_AddItemCount = 1;
    var Hotel_DelItemCount = 0;
    function BindHotel_AddAndDelItemEvent() {
        var $AddItemStatus = $("#Hotel_AddItemStatus");
        var $AddItem = $("div.Hotel ul li.ImgText img.AddItem");
        var $DelItem = $("div.HotelItem ul li.ExpenseType img.DelItem");
        var $Insert = $("div.HotelInsert");
        $AddItem.bind("click", function () {
            if ($AddItemStatus.val() == "0") {
                $("div.HotelItem").show()
                $AddItemStatus.val("1");
                return;
            }
            ++Hotel_AddItemCount;
            var $InsertItems1 = $Insert.prev();
            var $InsertItems2 = $InsertItems1.prev();
            var $InsertItems3 = $InsertItems2.prev();
            var $InsertItems3Clone = $InsertItems3.clone(true);
            var $InsertItems2Clone = $InsertItems2.clone(true);
            var $InsertItems1Clone = $InsertItems1.clone(true);
            SetHotel_ItemID($InsertItems3Clone, $InsertItems2Clone);
            $InsertItems3Clone.insertBefore($Insert);
            $InsertItems2Clone.insertBefore($Insert);
            $InsertItems1Clone.insertBefore($Insert);
            if (Hotel_AddItemCount - Hotel_DelItemCount <= 10) {
                CalHotel_SubTotalAmount();
            } else {
                setTimeout(CalHotel_SubTotalAmount, 0);
            }
        });
        $DelItem.bind("click", function () {
            ++Hotel_DelItemCount;
            if ($("div.Hotel_Items").length - 3 > 0) {
                var $items1 = $(this).parent().parent().parent();
                var $items2 = $items1.next();
                var $items3 = $items2.next();
                $items1.remove();
                $items2.remove();
                $items3.remove();
                if (Hotel_AddItemCount - Hotel_DelItemCount <= 10) {
                    CalHotel_SubTotalAmount();
                } else {
                    setTimeout(CalHotel_SubTotalAmount, 0);
                }
            } else {
                $("div.HotelItem").hide()
                $AddItemStatus.val("0");
                CalHotel_SubTotalAmount();
            }
        });
    }

    function SetHotel_ItemID(InsertItems3Clone, InsertItems2Clone) {
        var id = Hotel_AddItemCount - 1;
        InsertItems3Clone.find("li.ExpenseType select").attr("id", "Hotel_ExpenseType" + id);
        InsertItems3Clone.find("li.Date input").eq(0).attr("id", "Hotel_FromDate" + id);
        InsertItems3Clone.find("li.Date input").eq(1).attr("id", "Hotel_ToDate" + id);
        InsertItems3Clone.find("li.CostCenter select").attr("id", "Hotel_CostCenter" + id);
        InsertItems3Clone.find("li.OriginalAmt input").attr("id", "Hotel_OriginalAmt" + id);
        InsertItems3Clone.find("li.Currency select").attr("id", "Hotel_Currency" + id);
        InsertItems3Clone.find("li.Currency input.OtherCurrency").attr("id", "Hotel_OtherCurrency" + id);
        InsertItems3Clone.find("li.ExchRate input").attr("id", "Hotel_ExchRate" + id);
        //InsertItems3Clone.find("li.ExchRate input").val(id+1);
        InsertItems3Clone.find("li.ClaimAmt input").attr("id", "Hotel_ClaimAmt" + id);
        InsertItems3Clone.find("li.ComStd input").attr("id", "Hotel_ComStd" + id);
        InsertItems3Clone.find("li.PaidCredit input").attr("id", "Hotel_PaidCredit" + id);
        InsertItems2Clone.find("input").attr("id", "Hotel_HotelRemark" + id);
    }
</script>
<script type="text/javascript">
    function BindHotel_ExpenseTypeEvent() {
        var $ExpenseType = $("div.HotelItem li.ExpenseType select");
        $ExpenseType.bind("click change", function () {
            var ID = $(this).attr("id").replace("Hotel_ExpenseType", "");
            var $Hotel_ComStd = $("#Hotel_ComStd" + ID);

            var $Hotel_FromDate = $("#Hotel_FromDate" + ID);
            var $Hotel_ToDate = $("#Hotel_ToDate" + ID);
            if ($Hotel_FromDate.val() == "" || $Hotel_ToDate.val() == "") {
                alert("Please Select From Date Or To Date.");
                $(this).val("Select City");
                return;
            }
            var StartDate = new Date(Date.parse($Hotel_FromDate.val())).getTime();
            var EndDate = new Date(Date.parse($Hotel_ToDate.val())).getTime();
            if (EndDate < StartDate) {
                alert("The To Date be less than the From Date In The Hotel Area.");
                $(this).val("Select City");
                return;
            }
            var DateCount = Math.abs((StartDate - EndDate)) / (1000 * 60 * 60 * 24);
            if (DateCount == 0) {
                DateCount = 1;
            }
            var $hfTravelPolicy = $("input[id$='hfTravelPolicy']");
            var json = $hfTravelPolicy.val();
            var travelPolicy = eval("(" + json + ")");
            var expenseType = $(this).val();
            if (expenseType != "HK/Oversea" && expenseType != "Select City") {
                $.each(travelPolicy, function (i, item) {
                    if (item != undefined) {
                        if (item.Location == expenseType) {
                            $Hotel_ComStd.val(item.HotelLimit * DateCount);
                        }
                    }
                });
            } else {
                $Hotel_ComStd.val("");
            }
            ShowOrHideHotelRemark(ID);
        });
    }

    function BindHotel_DateEvent() {
        var $Date = $("div.HotelItem li.Date img");
        $Date.bind("click", function () {
            var $dateText = $(this).prev();
            new Calendar().show($dateText[0]);
        });
    }

    function BindHotel_CostCenterEvent() {
        var $CostCenter = $("div.HotelItem li.CostCenter select");
        $CostCenter.bind("mousemove", function () {
            $(this).addClass("CostCenterTD");
        }).bind("blur mouseout", function () {
            $(this).removeClass("CostCenterTD");
        }).bind("click", function () {
            $(this).addClass("CostCenterTD");
            $(this).unbind("mouseout");
        }).bind("change", function () {
            $(this).removeClass("CostCenterTD");
            $(this).bind("mouseout", function () {
                $(this).removeClass("CostCenterTD");
            });
        });
    }

    function GetHotel_PaidCredit() {
        var $PaidCredit = $("div.HotelItem li.PaidCredit input");
        var $PaidCreditAmount = $("#Hotel_PaidCredit");
        var TotalAmount = 0;
        $PaidCredit.each(function () {
            var IsPaidCredit = $(this).attr("checked");
            if (IsPaidCredit) {
                var ID = $(this).attr("id").replace("Hotel_PaidCredit", "");
                var $ClaimAmt = $("#Hotel_ClaimAmt" + ID);
                TotalAmount += parseFloat($ClaimAmt.val());
            }
        });
        TotalAmount = Math.round(TotalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        if ($("#Hotel_AddItemStatus").val() == "0") {
            $PaidCreditAmount.val("0");
        } else {
            $PaidCreditAmount.val(TotalAmount.toFixed(2));
        }
    }

    function BindHotel_PaidCreditEvent() {
        var $PaidCredit = $("div.HotelItem li.PaidCredit input");
        $PaidCredit.bind("click", function () {
            CalHotel_SubTotalAmount();
        });
    }

    function BindHotel_AmountEvent() {
        var $Amount = $("div.HotelItem li.OriginalAmt input");
        $Amount.bind("blur", function () {
            if (isNaN($(this).val()) || $(this).val() < 0 || $(this).val() > 100000000) {
                $(this).val("0");
                $(this).css("borderColor", "red");
                alert('Please fill the valid number.');
            } else {
                $(this).css("borderColor", "#999");
            }
            if ($(this).val() == "") {
                $(this).val("0")
            } else {
                var amount = Math.round($(this).val() * Math.pow(10, 2)) / Math.pow(10, 2);
                $(this).val(amount);
                var ID = $(this).attr("id").replace("Hotel_OriginalAmt", "");
                var $Hotel_ExchRate = $("#Hotel_ExchRate" + ID);
                if ($Hotel_ExchRate.val() == "") {
                    alert("Please Input Exchange Rate !");
                    $Hotel_ExchRate.css("borderColor", "red");
                    return;
                }
                var excRate = parseFloat($Hotel_ExchRate.val());
                var ClaimAmt = amount * excRate;
                ClaimAmt = Math.round(ClaimAmt * Math.pow(10, 2)) / Math.pow(10, 2);
                var $Hotel_ClaimAmt = $("#Hotel_ClaimAmt" + ID);
                $Hotel_ClaimAmt.val(ClaimAmt.toFixed(2));

                ShowOrHideHotelRemark(ID);
            }
        });
    }

    function BindHotel_CurrencyEvent() {
        var $Currency = $("div.HotelItem li.Currency select");
        $Currency.bind("change", function () {
            var ID = $(this).attr("id").replace("Hotel_Currency", "");
            var $Hotel_ExchRate = $("#Hotel_ExchRate" + ID);
            $Hotel_ExchRate.val("");
            var $Hotel_ClaimAmt = $("#Hotel_ClaimAmt" + ID);
            var curval = $(this).val();
            var $cur = $(this);
            if (curval == "Others") {
                $cur.hide();
                var $next = $cur.next();
                $next.find("div.CurrencySub").show();
                $next.find("input").val("");
                $next.next().show();
                $Hotel_ClaimAmt.val("");
            } else {
                var $hfExchangeRate = $("input[id$='hfExchangeRate']");
                var json = $hfExchangeRate.val();
                if (json != "") {
                    var exchangeRate = eval("(" + json + ")");
                    if (curval == "RMB") {
                        $Hotel_ExchRate.val("1.0000");
                    } else {
                        $.each(exchangeRate, function (i, item) {
                            if (item != undefined) {
                                if (item.name == curval) {
                                    $Hotel_ExchRate.val(item.val);
                                }
                            }
                        });
                    }
                }
            }
            $Hotel_ExchRate.blur();
        });
        var $OK = $("div.HotelItem li.Currency a.OK");
        var $Cancel = $("div.HotelItem li.Currency a.Cancel");
        $OK.bind("click", function () {
            var $parent = $(this).parent();
            var curval = $parent.find("input").val();
            if (curval == "") {
                alert("Please Input Other Currency !");
                return;
            }
            $parent.parent().next().val(curval);
            $parent.hide();
            $parent.parent().parent().next().find("input").val("");
            var ID = $parent.parent().prev().attr("id").replace("Hotel_Currency", "");
            var $Hotel_ExchRate = $("#Hotel_ExchRate" + ID);
            $Hotel_ExchRate.val("");
            $Hotel_ExchRate.focus();
        });
        $Cancel.bind("click", function () {
            var $parent = $(this).parent();
            $parent.hide();
            var $prev = $parent.parent().prev();
            var $next = $parent.parent().next();
            $prev.show();
            $prev.val("RMB");
            $next.hide();
            $next.val("");
            var ID = $prev.attr("id").replace("Hotel_Currency", "");
            var $Hotel_ExchRate = $("#Hotel_ExchRate" + ID);
            $Hotel_ExchRate.val("1.0000");
            var $Hotel_OriginalAmt = $("#Hotel_OriginalAmt" + ID);
            $Hotel_OriginalAmt.blur();
        });
        var $OtherCurrency = $("div.HotelItem li.Currency input.OtherCurrency");
        $OtherCurrency.bind("click", function () {
            $(this).val("");
            var $prev = $(this).prev();
            $prev.find("div.CurrencySub").show();
            $prev.find("input").val("");
        }).bind("focus", function () {
            $(this).blur();
        });
    }

    function BindHotel_ExchRateEvent() {
        var $ExchRate = $("div.HotelItem li.ExchRate input");
        $ExchRate.bind("blur", function () {
            var ID = $(this).attr("id").replace("Hotel_ExchRate", "");
            if ($(this).val() == "") {
                ShowOrHideHotelRemark(ID);
                return;
            }
            var $Hotel_ClaimAmt = $("#Hotel_ClaimAmt" + ID);
            if (isNaN($(this).val()) || $(this).val() < 0) {
                $(this).val("");
                alert("Please Input Exch Rate !");
                $(this).css("borderColor", "red");
                $Hotel_ClaimAmt.val("");
                return;
            } else {
                $(this).css("borderColor", "#999");
            }

            var $Hotel_OriginalAmt = $("#Hotel_OriginalAmt" + ID);
            if ($Hotel_OriginalAmt.val() == "") {
                alert("Please Input Original Amount !");
                $Hotel_OriginalAmt.css("borderColor", "red");
                return;
            }
            var excRate = parseFloat($(this).val());
            var ClaimAmt = parseFloat($Hotel_OriginalAmt.val()) * excRate;
            ClaimAmt = Math.round(ClaimAmt * Math.pow(10, 2)) / Math.pow(10, 2);
            $Hotel_ClaimAmt.val(ClaimAmt.toFixed(2));

            ShowOrHideHotelRemark(ID);
        });
    }

    function BindHotel_HotelRemarkEvent() {
        var $HotelRemark = $("div.HotelRemark input");
        $HotelRemark.bind("focus", function () {
            if ($(this).val() == "Why exceeds company standard?") {
                $(this).val("");
            }
        }).bind("blur", function () {
            if ($(this).val() == "") {
                $(this).val("Why exceeds company standard?");
            }
        });
    }

    function ShowOrHideHotelRemark(ID) {
        var $Hotel_ClaimAmt = $("#Hotel_ClaimAmt" + ID);
        var $Hotel_ComStd = $("#Hotel_ComStd" + ID);
        var $Hotel_HotelRemark = $("#Hotel_HotelRemark" + ID);
        $Hotel_ClaimAmt.css("color", "#06c");
        $Hotel_HotelRemark.parent().parent().hide();
        $Hotel_HotelRemark.val("");
        if ($Hotel_ClaimAmt.val() != "" && $Hotel_ComStd.val() != "") {
            if (parseFloat($Hotel_ClaimAmt.val()) > parseFloat($Hotel_ComStd.val())) {
                $Hotel_ClaimAmt.css("color", "red");
                $Hotel_HotelRemark.parent().parent().show();
                $Hotel_HotelRemark.val("Why exceeds company standard?");
            }
        }
        if (Hotel_AddItemCount - Hotel_DelItemCount <= 10) {
            CalHotel_SubTotalAmount();
        } else {
            setTimeout(CalHotel_SubTotalAmount, 0);
        }
    }

    function CalHotel_SubTotalAmount() {
        var $Amount = $("div.HotelItem li.ClaimAmt input");
        var $Hotel_SubTotal = $("#Hotel_SubTotal");
        var TotalAmount = 0;
        var res = false;
        $Amount.each(function () {
            if ($(this).val() != "") {
                TotalAmount += parseFloat($(this).val());
            }
        });
        TotalAmount = Math.round(TotalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        if ($("#Hotel_AddItemStatus").val() == "0") {
            $Hotel_SubTotal.val("");
        } else {
            $Hotel_SubTotal.val(TotalAmount.toFixed(2));
        }
        GetHotel_PaidCredit();
        SetTotalCost();
    }

</script>
<script type="text/javascript">
    function DrawHotelForm() {
        var $Insert = $("div.HotelInsert");
        var $hfHotelForm = $("#Hidden input[id$='hfHotelForm']");
        var json = $hfHotelForm.val();
        if (json != "" && json != "[]") {
            var HotelForm = eval("(" + json + ")");
            var $AddItemStatus = $("#Hotel_AddItemStatus");
            $.each(HotelForm, function (i, item) {
                if (item != undefined) {
                    if ($AddItemStatus.val() == "0") {
                        $("div.HotelItem").show()
                        $AddItemStatus.val("1");
                    }
                    var $InsertItems1 = $Insert.prev();
                    var $InsertItems2 = $InsertItems1.prev();
                    var $InsertItems3 = $InsertItems2.prev();
                    var $ClaimItem = $InsertItems3;
                    var $Reamrk = $InsertItems2;
                    if (i != 0) {
                        ++Hotel_AddItemCount;
                        var $InsertItems3Clone = $InsertItems3.clone(true);
                        var $InsertItems2Clone = $InsertItems2.clone(true);
                        var $InsertItems1Clone = $InsertItems1.clone(true);
                        SetHotel_ItemID($InsertItems3Clone, $InsertItems2Clone);
                        $InsertItems3Clone.insertBefore($Insert);
                        $InsertItems2Clone.insertBefore($Insert);
                        $InsertItems1Clone.insertBefore($Insert);
                        $ClaimItem = $InsertItems3Clone;
                        $Reamrk = $InsertItems2Clone;
                    }
                    SetClaimItem($ClaimItem,
                                    item.ExpenseType,
                                    item.FromDate,
                                    item.ToDate,
                                    item.CostCenter,
                                    item.OriginalAmt,
                                    item.Currency,
                                    item.OtherCurrency,
                                    item.ExchRate,
                                    item.ClaimAmt,
                                    item.ComStd,
                                    item.PaidCredit);
                    SetHotelRemarkItem($Reamrk, item.Remark);
                }
            });
            if (Hotel_AddItemCount - Hotel_DelItemCount <= 10) {
                CalHotel_SubTotalAmount();
            } else {
                setTimeout(CalHotel_SubTotalAmount, 0);
            }
        }
    }

    function SetClaimItem(ClaimItem,
                          ExpenseType,
                          FromDate,
                          ToDate,
                          CostCenter,
                          OriginalAmt,
                          Currency,
                          OtherCurrency,
                          ExchRate,
                          ClaimAmt,
                          ComStd,
                          PaidCredit) {
        ClaimItem.find("li.ExpenseType select").val(ExpenseType);
        ClaimItem.find("li.Date input").eq(0).val(FromDate);
        ClaimItem.find("li.Date input").eq(1).val(ToDate);
        ClaimItem.find("li.CostCenter select").val(CostCenter);
        ClaimItem.find("li.OriginalAmt input").val(OriginalAmt);
        var $Currency = ClaimItem.find("li.Currency select");
        var $OtherCurrency = ClaimItem.find("li.Currency input.OtherCurrency");
        $Currency.val(Currency);
        $OtherCurrency.val(OtherCurrency);
        if (OtherCurrency != "") {
            $Currency.hide();
            $OtherCurrency.show();
        } else {
            $Currency.show();
            $OtherCurrency.hide();
        }
        ClaimItem.find("li.ExchRate input").val(ExchRate);
        var $ClaimAmt = ClaimItem.find("li.ClaimAmt input");
        $ClaimAmt.val(ClaimAmt);
        if (ComStd != "") {
            if (parseFloat(ClaimAmt) > parseFloat(ComStd)) {
                $ClaimAmt.css("color", "red");
            } else {
                $ClaimAmt.css("color", "#06c");
            }
        } else {
            $ClaimAmt.css("color", "#06c");
        }
        ClaimItem.find("li.ComStd input").val(ComStd);
        var $PaidCredit = ClaimItem.find("li.PaidCredit input");
        if (PaidCredit == "1") {
            $PaidCredit.attr("checked", true);
        } else {
            $PaidCredit.attr("checked", false);
        }
    }

    function SetHotelRemarkItem(Reamrk, RemarkValue) {
        if (RemarkValue != "") {
            var $input = Reamrk.find("input");
            var ID = $input.attr("id").replace("Hotel_HotelRemark", "");
            var $Hotel_ClaimAmt = $("#Hotel_ClaimAmt" + ID);
            var $Hotel_ComStd = $("#Hotel_ComStd" + ID);
            if ($Hotel_ComStd.val() != "") {
                if (parseFloat($Hotel_ClaimAmt.val()) > parseFloat($Hotel_ComStd.val())) {
                    Reamrk.find("input").val(RemarkValue);
                    Reamrk.show();
                }
            } else {
                Reamrk.hide();
            }
        } else {
            Reamrk.hide();
        }
    }
</script>
