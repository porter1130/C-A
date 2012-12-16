<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TravelMealForm.ascx.cs"
    Inherits="CA.WorkFlow.UI.TE.TravelMealForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .MealAllowanceItem, .MealAllowanceRemark
    {
        display: none;
    }
    .MealAllowanceInsert
    {
        display: none;
    }
    .MealAllowance
    {
        position: relative;
        float: left;z-index: 5;
    }
    .SelectMealAllowance
    {
        position: absolute;
        width: 462px;
        height: auto;
        left: -2px;
        top: 35px;
        z-index: 5;
        background-color: White;
        float: left;
        border: #999 1px solid;
        display:none;
    }
    .SelectMealAllowance input
    {
        width: 90px;
    }
    .M_Head
    {
        width: 100%;
        float: left;
        padding: 13px 0px 13px 0px;
        border-bottom: #999 1px solid;
        font-weight: 700;
    }
    .M_Head_Left
    {
        width: 40%;
        float: left;
        padding-left: 28px;
    }
    .M_Head_Right
    {
        width: 40%;
        float: right;
        padding-right: 20px;
        color: Red;
        text-align: right;
        cursor: pointer;
        font-size: 15px;
    }
    .M_Content
    {
        width: 100%;
        float: left;
        line-height: 35px;
    }
    .M_C_Left
    {
        width: 119px;
        float: left;
        padding-right: 28px;
        text-align: right;
    }
    .M_C_right
    {
        width: 315px;
        float: left;
    }
    .S_Date
    {
        border: none;
        border-bottom: #999 1px solid;
        color: #06c;
    }
    #SMA_Currency
    {
        border: none;
    }
    #SMA_ExchangeRate
    {
        margin-right: 20px;
    }
    .Select
    {
        color: #06c;
        cursor: pointer;
        text-decoration: underline;
    }
</style>
<div class="Item_UL MealAllowance">
    <div>
        <ul>
            <li class="w104 ImgText">
                <img src="../images/pixelicious_001.png" alt="Click to add the information." width="18"
                    class="AddItem" align="absmiddle" />Meal Allowance</li>
            <li class="w40 TitleText"></li>
            <li class="w20 Sub">Sub Total</li>
            <li class="w30 Total">
                <input type="text" value="" id="MealAllowance_SubTotal" readonly="readonly" /></li>
        </ul>
    </div>
    <div class="SelectMealAllowance" id="SelectMealAllowanceInfo">
        <div class="M_Head">
            <div class="M_Head_Left">
                Select Meal Info
            </div>
            <div class="M_Head_Right">
                <a class="CloseMealAllowance" title="Close">X</a>
            </div>
        </div>
        <div class="M_Content">
            <div class="M_C_Left">
                Select Date：
            </div>
            <div class="M_C_right" id="SelectDate">
                <input type="text" class="S_Date" id="SMA_FromDate" value="" readonly="readonly"/>
                <img src="../../../CAResources/themeCA/images/selectcalendar.gif" align="absmiddle"
                    alt="Select Date" />—
                <input type="text" class="S_Date" id="SMA_TODate" value="" readonly="readonly"/>
                <img src="../../../CAResources/themeCA/images/selectcalendar.gif" align="absmiddle"
                    alt="Select Date" />
            </div>
            <div class="M_C_Left">
                Select Area：
            </div>
            <div class="M_C_right">
                <select id="SelectArea">
                    <option value="China">China</option>
                    <option value="FarEast">FarEast</option>
                    <option value="USA">USA</option>
                    <option value="Switzerland">Switzerland</option>
                    <option value="UK">UK</option>
                    <option value="Europe & North Africa">Europe & North Africa</option>
                    <option value="central & south america">central & south america</option>
                </select>
            </div>
            <div class="M_C_Left">
                Currency：
            </div>
            <div class="M_C_right">
                <input type="text" class="S_Date" id="SMA_Currency" value="RMB" readonly="readonly" />
            </div>
            <div class="M_C_Left">
                Exchange Rate：
            </div>
            <div class="M_C_right">
                <input type="text" class="S_Date" id="SMA_ExchangeRate" value="1.0000"  />
                (or fill-in your own rate)
            </div>
            <div class="M_C_Left">
                Cost Center：
            </div>
            <div class="M_C_right">
                <select id="SMA_CostCenter">
                    <option class="blank" value=""></option>
                </select>
            </div>
            <div class="M_C_Left">
            </div>
            <div class="M_C_right">
                <a class="Select">Select Meal Allowance</a>
            </div>
        </div>
        
    </div>
</div>
<div class="Item_UL MealAllowance_Items MealAllowanceItem">
    <ul>
        <li class="w104 ExpenseType">
            <img src="../images/pixelicious_028.png" alt="Remove this information." width="18"
                class="DelItem" align="absmiddle" /><select class="ItemSelect" id="MealAllowance_ExpenseType0">
                    <option>Breakfast</option>
                    <option>Lunch</option>
                    <option>Dinner</option>
                </select></li>
        <li class="w15 Date">
            <input type="text" class="Date" id="MealAllowance_Date0" value="" readonly="readonly" />
            <img src="../../../CAResources/themeCA/images/selectcalendar.gif" align="absmiddle"
                alt="Select Date" /></li>
        <li class="w10 CostCenter">
             <div class="CostCenterContainer">
                <div class="CostCenterSub">
                    <select class="CostCenterCurrency CostCenter" id="MealAllowance_CostCenter0">
                        <option class="blank" value=""></option>
                    </select></div>
            </div></li>
        <li class="w10 OriginalAmt">
            <input type="text" class="Date" id="MealAllowance_OriginalAmt0" value="" /></li>
        <li class="w10 Currency">
            <select class="CostCenterCurrency" id="MealAllowance_Currency0">
                <option class="blank" value=""></option>
            </select>
            <div class="CurrencyContainer">
                <div class="CurrencySub">
                    Other Currency<input type="text" class="Date" value="" /><br />
                    <a class="OK">OK</a><a class="Cancel">Select Currency List</a>
                </div>
            </div>
            <input type="text" class="Date OtherCurrency" id="MealAllowance_OtherCurrency0" value=""
                readonly="readonly" />
        </li>
        <li class="w10 ExchRate">
            <input type="text" class="Date" id="MealAllowance_ExchRate0" value="1.0000" /></li>
        <li class="w10 ClaimAmt">
            <input type="text" class="Date" id="MealAllowance_ClaimAmt0" value="" readonly="readonly" /></li>
        <li class="w9 bg ComStd"><input type="text" value="" id="MealAllowance_ComStd0"  readonly="readonly"/></li>
        <li class="w14 bg PaidCredit">
            <input type="checkbox" class="Date" id="MealAllowance_PaidCredit0" value="" /></li>
    </ul>
</div>
<div class="Item_Head_Content MealAllowance_Items MealAllowanceRemark">
    <div class="Item_Head_Left w104">
        Remark</div>
    <div class="Item_Head_Right w774">
        <input type="text" id="MealAllowance_MealAllowanceRemark0" value="" /></div>
</div>
<div class="Blank MealAllowance_Items">
    <div class="BlankLeft">
    </div>
    <div class="BlankRight">
    </div>
</div>
<div class="MealAllowanceInsert">
    <input type="hidden" id="MealAllowanceItemStatus" value="0" />
    <input type="hidden" id="MealAllowance_ComStd" value="50;50;70" />
    <input type="hidden" id="MealAllowance_PaidCredit" value="0" />
</div>
<script type="text/javascript" src="JS/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        LoadMealAllowance_CostCenter();
        LoadMealAllowance_Currency();
        BindMealAllowance_AddAndDelItemEvent();
        BindMealAllowance_ExpenseTypeEvent();
        BindMealAllowance_DateEvent();
        BindMealAllowance_CostCenterEvent();
        BindMealAllowance_AmountEvent();
        BindMealAllowance_CurrencyEvent();
        BindMealAllowance_ExchRateEvent();
        BindMealAllowance_MealAllowanceRemarkEvent();
        BindMealAllowance_SelectMealInfoEvent();
        BindMealAllowance_PaidCreditEvent();
        DrawMealAllowanceForm();
    });

    function GetMealAllowance_Currency_Option(text, val) {
        var option = "";
        option = "<option class=\"item\" value=\"" + text + "\" >" + val + "</option>";
        return option;
    }

    function LoadMealAllowance_CostCenter() {
        var $CostCenter = $("div.MealAllowanceItem ul li.CostCenter select.CostCenter"); 
        var $Insert = $CostCenter.find("option.blank");

        var $SMA_CostCenter = $("#SMA_CostCenter");
        var $SMA_Insert = $SMA_CostCenter.find("option.blank");

        var $hfCostCenter = $("input[id$='hfCostCenter']");
        var json = $hfCostCenter.val();
        if (json != "") {
            var costCenter = eval("(" + json + ")");
            var $blank1 = $("<option class=\"item\" value=\"\" ></option>");
            var $blank2 = $("<option class=\"item\" value=\"\" ></option>");
            $Insert.before($blank1);
            $SMA_Insert.before($blank2);
            $.each(costCenter, function (i, item) {
                if (item != undefined) {
                    $InsertOption1 = $(GetMealAllowance_Currency_Option(item.val, item.name));
                    $InsertOption2 = $(GetMealAllowance_Currency_Option(item.val, item.name));
                    $Insert.before($InsertOption1);
                    $SMA_Insert.before($InsertOption2);
                }
            });
            $CostCenter.find("option").remove(".blank");
            $SMA_CostCenter.find("option").remove(".blank");
        }
    }

    function LoadMealAllowance_Currency() {
        var $Currency = $("div.MealAllowanceItem ul li.Currency select");
        var $Insert = $Currency.find("option.blank");
        var currency = ["RMB", "USD", "GBP", "EUR", "AUD", "CHF", "CAD", "JPY", "HKD", "Others"];
        for (var i = 0; i < currency.length; i++) {
            $InsertOption = $(GetMealAllowance_Currency_Option(currency[i], currency[i]));
            $Insert.before($InsertOption);
        }
        $Currency.find("option").remove(".blank");
    }
</script>
<script type="text/javascript">
    var MealAllowance_AddItemCount = 1;
    var MealAllowance_DelItemCount = 0;
    function BindMealAllowance_AddAndDelItemEvent() {
        var $AddItem = $("div.MealAllowance ul li.ImgText img.AddItem");
        var $DelItem = $("div.MealAllowanceItem ul li.ExpenseType img.DelItem");
        $AddItem.bind("click", function () {
            ShowOrHideMealAllowanceInfo();
        });
        $DelItem.bind("click", function () {
            ++MealAllowance_DelItemCount;
            if ($("div.MealAllowance_Items").length - 3 > 0) {
                var $items1 = $(this).parent().parent().parent();
                var $items2 = $items1.next();
                var $items3 = $items2.next();
                $items1.remove();
                $items2.remove();
                $items3.remove();
                if (MealAllowance_AddItemCount - MealAllowance_DelItemCount <= 10) {
                    CalMealAllowance_SubTotalAmount();
                } else {
                    setTimeout(CalMealAllowance_SubTotalAmount, 0);
                }
            } else {
                $("div.MealAllowanceItem").hide();
                $("#MealAllowanceItemStatus").val("0");
                CalMealAllowance_SubTotalAmount();
            }
        });
    }

    function SetMealAllowance_ItemID(InsertItems3Clone, InsertItems2Clone) {
        var id = MealAllowance_AddItemCount - 1;
        InsertItems3Clone.find("li.ExpenseType select").attr("id", "MealAllowance_ExpenseType" + id);
        InsertItems3Clone.find("li.Date input").attr("id", "MealAllowance_Date" + id);
        InsertItems3Clone.find("li.CostCenter select").attr("id", "MealAllowance_CostCenter" + id);
        InsertItems3Clone.find("li.OriginalAmt input").attr("id", "MealAllowance_OriginalAmt" + id);
        InsertItems3Clone.find("li.Currency select").attr("id", "MealAllowance_Currency" + id);
        InsertItems3Clone.find("li.Currency input.OtherCurrency").attr("id", "MealAllowance_OtherCurrency" + id);
        InsertItems3Clone.find("li.ExchRate input").attr("id", "MealAllowance_ExchRate" + id);
        //InsertItems3Clone.find("li.ExchRate input").val(id + 1);
        InsertItems3Clone.find("li.ClaimAmt input").attr("id", "MealAllowance_ClaimAmt" + id);
        InsertItems3Clone.find("li.ComStd input").attr("id", "MealAllowance_ComStd" + id);
        InsertItems3Clone.find("li.PaidCredit input").attr("id", "MealAllowance_PaidCredit" + id);
        InsertItems2Clone.find("input").attr("id", "MealAllowance_MealAllowanceRemark" + id);
    }

    function SetMealAllowanceValue(InsertItems3Clone, index, date, staus) {
        var SMA_FromDate = $("#SMA_FromDate").val();
        var SMA_TODate = $("#SMA_TODate").val();
        var SMA_Currency = $("#SMA_Currency").val();
        var SMA_ExchangeRate = $("#SMA_ExchangeRate").val();
        var SMA_CostCenter = $("#SMA_CostCenter").val();
        var $MealAllowance_ComStd = $("#MealAllowance_ComStd");
        var c_Date = new Date();
        if (SMA_FromDate == "" || SMA_TODate == "" || staus == false) {
            var m_Date = (c_Date.getMonth() + 1) + "/" + c_Date.getDate() + "/" + c_Date.getYear();
            InsertItems3Clone.find("li.Date input").val(m_Date);
        }
        if (staus) {
            var StartDate = new Date(Date.parse(SMA_FromDate));
            var Cur_Date = StartDate;
            var timer = Cur_Date.getTime() + (date * 24 * 60 * 60 * 1000);
            Cur_Date.setTime(timer);
            var m_Date = (Cur_Date.getMonth() + 1) + "/" + Cur_Date.getDate() + "/" + Cur_Date.getYear();
            InsertItems3Clone.find("li.Date input").val(m_Date);
        }
        InsertItems3Clone.find("li.CostCenter select").val(SMA_CostCenter);
        InsertItems3Clone.find("li.Currency select").val(SMA_Currency);
        InsertItems3Clone.find("li.ExchRate input").val(SMA_ExchangeRate);
        var MealAllowance_ComStd = $MealAllowance_ComStd.val();
        var $ExpenseType = InsertItems3Clone.find("li.ExpenseType select");
        var $ComStd = InsertItems3Clone.find("li.ComStd input");
        switch (index) {
            case 1:
                $ExpenseType.val("Breakfast");
                var amt1 = parseFloat(MealAllowance_ComStd.split(';')[0]) * parseFloat(SMA_ExchangeRate);
                amt1 = Math.round(amt1 * Math.pow(10, 2)) / Math.pow(10, 2);
                amt1 = amt1.toFixed(2);
                InsertItems3Clone.find("li.OriginalAmt input").val(MealAllowance_ComStd.split(';')[0]);
                InsertItems3Clone.find("li.ClaimAmt input").val(amt1);
                $ComStd.val(amt1);
                break;
            case 2:
                $ExpenseType.val("Lunch");
                var amt2 = parseFloat(MealAllowance_ComStd.split(';')[1]) * parseFloat(SMA_ExchangeRate);
                amt2 = Math.round(amt2 * Math.pow(10, 2)) / Math.pow(10, 2);
                amt2 = amt2.toFixed(2);
                InsertItems3Clone.find("li.OriginalAmt input").val(MealAllowance_ComStd.split(';')[1]);
                InsertItems3Clone.find("li.ClaimAmt input").val(amt2);
                $ComStd.val(amt2);
                break;
            case 3:
                $ExpenseType.val("Dinner");
                var amt3 = parseFloat(MealAllowance_ComStd.split(';')[2]) * parseFloat(SMA_ExchangeRate);
                amt3 = Math.round(amt3 * Math.pow(10, 2)) / Math.pow(10, 2);
                amt3 = amt3.toFixed(2);
                InsertItems3Clone.find("li.OriginalAmt input").val(MealAllowance_ComStd.split(';')[2]);
                InsertItems3Clone.find("li.ClaimAmt input").val(amt3);
                $ComStd.val(amt3);
                break;
        }

        var ComStd = InsertItems3Clone.find("li.ComStd input").val();
        var $ClaimAmt = InsertItems3Clone.find("li.ClaimAmt input");
        var $Remark = InsertItems3Clone.next();
        if (parseFloat($ClaimAmt.val()) > parseFloat(ComStd)) {
            $ClaimAmt.css("color", "red");
            $Remark.show();
        } else {
            $ClaimAmt.css("color", "#06c");
            $Remark.hide();
        }
    }

    function ValidateFromToDate() {
        var msg = "";
        var result = false;
        var $SMA_FromDate = $("#SMA_FromDate");
        var $SMA_TODate = $("#SMA_TODate");
        if ($SMA_FromDate.val() != "" && $SMA_TODate.val() != "") {
            var StartDate = new Date(Date.parse($SMA_FromDate.val()));
            var EndDate = new Date(Date.parse($SMA_TODate.val()));
            if (EndDate < StartDate) {
                msg += "The To Date be less than the From Date .";
                result = true;
            }
        }
        if (msg != "") {
            alert(msg);
        }
        return result;
    }
</script>
<script type="text/javascript">
    function BindMealAllowance_ExpenseTypeEvent() {
//        var $ExpenseType = $("div.MealAllowanceItem li.ExpenseType select");
//        $ExpenseType.bind("change", function () {
//            var ID = $(this).attr("id").replace("MealAllowance_ExpenseType", "");
//            var $Hotel_ComStd = $("#MealAllowance_ComStd" + ID);
//            var $hfTravelPolicy = $("input[id$='hfTravelPolicy']");
//            var json = $hfTravelPolicy.val();
//            var travelPolicy = eval("(" + json + ")");
//            var expenseType = $(this).val();
//            if (expenseType != "HK/Oversea") {
//                $.each(travelPolicy, function (i, item) {
//                    if (item != undefined) {
//                        if (item.Location == expenseType) {
//                            $Hotel_ComStd.val(item.HotelLimit);
//                        }
//                    }
//                });
//            } else {
//                $Hotel_ComStd.val("");
//            }
//            ShowOrHideMealAllowanceRemark(ID);
//        });
    }

    function BindMealAllowance_DateEvent() {
        var $Date = $("div.MealAllowanceItem li.Date img");
        $Date.bind("click", function () {
            var $dateText = $(this).prev();
            new Calendar().show($dateText[0]);
        });
    }

    function BindMealAllowance_CostCenterEvent() {
        var $CostCenter = $("div.MealAllowanceItem li.CostCenter select");
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

    function GetMealAllowance_PaidCredit() {
        var $PaidCredit = $("div.MealAllowanceItem li.PaidCredit input");
        var $PaidCreditAmount = $("#MealAllowance_PaidCredit");
        var TotalAmount = 0;
        $PaidCredit.each(function () {
            var IsPaidCredit = $(this).attr("checked");
            if (IsPaidCredit) {
                var ID = $(this).attr("id").replace("MealAllowance_PaidCredit", "");
                var $ClaimAmt = $("#MealAllowance_ClaimAmt" + ID);
                TotalAmount += parseFloat($ClaimAmt.val());
            }
        });
        TotalAmount = Math.round(TotalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        if ($("#MealAllowanceItemStatus").val() == "0") {
            $PaidCreditAmount.val("0");
        } else {
            $PaidCreditAmount.val(TotalAmount.toFixed(2));
        }
    }

    function BindMealAllowance_PaidCreditEvent() {
        var $PaidCredit = $("div.MealAllowanceItem li.PaidCredit input");
        $PaidCredit.bind("click", function () {
            CalMealAllowance_SubTotalAmount();
        });
    }

    function BindMealAllowance_AmountEvent() {
        var $Amount = $("div.MealAllowanceItem li.OriginalAmt input");
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
                var ID = $(this).attr("id").replace("MealAllowance_OriginalAmt", "");
                var $MealAllowance_ExchRate = $("#MealAllowance_ExchRate" + ID);
                if ($MealAllowance_ExchRate.val() == "") {
                    alert("Please Input Exchange Rate !");
                    $MealAllowance_ExchRate.css("borderColor", "red");
                    return;
                }
                var excRate = parseFloat($MealAllowance_ExchRate.val());
                var ClaimAmt = amount * excRate;
                ClaimAmt = Math.round(ClaimAmt * Math.pow(10, 2)) / Math.pow(10, 2);
                var $MealAllowance_ClaimAmt = $("#MealAllowance_ClaimAmt" + ID);
                $MealAllowance_ClaimAmt.val(ClaimAmt.toFixed(2));

                ShowOrHideMealAllowanceRemark(ID);
            }
        });
    }

    function BindMealAllowance_CurrencyEvent() {
        var $Currency = $("div.MealAllowanceItem li.Currency select");
        $Currency.bind("change", function () {
            var ID = $(this).attr("id").replace("MealAllowance_Currency", "");
            var $MealAllowance_ExchRate = $("#MealAllowance_ExchRate" + ID);
            $MealAllowance_ExchRate.val("");
            var $MealAllowance_ClaimAmt = $("#MealAllowance_ClaimAmt" + ID);
            var curval = $(this).val();
            var $cur = $(this);
            if (curval == "Others") {
                $cur.hide();
                var $next = $cur.next();
                $next.find("div.CurrencySub").show();
                $next.find("input").val("");
                $next.next().show();
                $MealAllowance_ClaimAmt.val("");
            } else {
                var $hfExchangeRate = $("input[id$='hfExchangeRate']");
                var json = $hfExchangeRate.val();
                if (json != "") {
                    var exchangeRate = eval("(" + json + ")");
                    if (curval == "RMB") {
                        $MealAllowance_ExchRate.val("1.0000");
                    } else {
                        $.each(exchangeRate, function (i, item) {
                            if (item != undefined) {
                                if (item.name == curval) {
                                    $MealAllowance_ExchRate.val(item.val);
                                }
                            }
                        });
                    }
                }
            }
            $MealAllowance_ExchRate.blur();
        });
        var $OK = $("div.MealAllowanceItem li.Currency a.OK");
        var $Cancel = $("div.MealAllowanceItem li.Currency a.Cancel");
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
            var ID = $parent.parent().prev().attr("id").replace("MealAllowance_Currency", "");
            var $MealAllowance_ExchRate = $("#MealAllowance_ExchRate" + ID);
            $MealAllowance_ExchRate.val("");
            $MealAllowance_ExchRate.focus();
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
            var ID = $prev.attr("id").replace("MealAllowance_Currency", "");
            var $MealAllowance_ExchRate = $("#MealAllowance_ExchRate" + ID);
            $MealAllowance_ExchRate.val("1.0000");
            var $MealAllowance_OriginalAmt = $("#MealAllowance_OriginalAmt" + ID);
            $MealAllowance_OriginalAmt.blur();
        });
        var $OtherCurrency = $("div.MealAllowanceItem li.Currency input.OtherCurrency");
        $OtherCurrency.bind("click", function () {
            $(this).val("");
            var $prev = $(this).prev();
            $prev.find("div.CurrencySub").show();
            $prev.find("input").val("");
        }).bind("focus", function () {
            $(this).blur();
        });
    }

    function BindMealAllowance_ExchRateEvent() {
        var $ExchRate = $("div.MealAllowanceItem li.ExchRate input");
        $ExchRate.bind("blur", function () {
            var ID = $(this).attr("id").replace("MealAllowance_ExchRate", "");
            if ($(this).val() == "") {
                ShowOrHideMealAllowanceRemark(ID);
                return;
            }
            var $MealAllowance_ClaimAmt = $("#MealAllowance_ClaimAmt" + ID);
            if (isNaN($(this).val()) || $(this).val() < 0) {
                $(this).val("");
                alert("Please Input Exch Rate !");
                $(this).css("borderColor", "red");
                $MealAllowance_ClaimAmt.val("");
                return;
            } else {
                $(this).css("borderColor", "#999");
            }

            var $MealAllowance_OriginalAmt = $("#MealAllowance_OriginalAmt" + ID);
            if ($MealAllowance_OriginalAmt.val() == "") {
                alert("Please Input Original Amount !");
                $MealAllowance_OriginalAmt.css("borderColor", "red");
                return;
            }
            var excRate = parseFloat($(this).val());
            var ClaimAmt = parseFloat($MealAllowance_OriginalAmt.val()) * excRate;
            ClaimAmt = Math.round(ClaimAmt * Math.pow(10, 2)) / Math.pow(10, 2);
            $MealAllowance_ClaimAmt.val(ClaimAmt.toFixed(2));

            ShowOrHideMealAllowanceRemark(ID);
        });
    }

    function BindMealAllowance_MealAllowanceRemarkEvent() {
        var $MealAllowanceRemark = $("div.MealAllowanceRemark input");
        $MealAllowanceRemark.bind("focus", function () {
            if ($(this).val() == "Why exceeds company standard?") {
                $(this).val("");
            }
        }).bind("blur", function () {
            if ($(this).val() == "") {
                $(this).val("Why exceeds company standard?");
            }
        });
    }

    function ShowOrHideMealAllowanceRemark(ID) {
        var $MealAllowance_ClaimAmt = $("#MealAllowance_ClaimAmt" + ID);
        var $MealAllowance_ComStd = $("#MealAllowance_ComStd" + ID);
        var $MealAllowance_MealAllowanceRemark = $("#MealAllowance_MealAllowanceRemark" + ID);
        $MealAllowance_ClaimAmt.css("color", "#06c");
        $MealAllowance_MealAllowanceRemark.parent().parent().hide();
        $MealAllowance_MealAllowanceRemark.val("");
        if ($MealAllowance_ClaimAmt.val() != "" && $MealAllowance_ComStd.val() != "") {
            if (parseFloat($MealAllowance_ClaimAmt.val()) > parseFloat($MealAllowance_ComStd.val())) {
                $MealAllowance_ClaimAmt.css("color", "red");
                $MealAllowance_MealAllowanceRemark.parent().parent().show();
                $MealAllowance_MealAllowanceRemark.val("Why exceeds company standard?");
            }
        }
        CalMealAllowance_SubTotalAmount();
    }

    function CalMealAllowance_SubTotalAmount() {
        var $Amount = $("div.MealAllowanceItem li.ClaimAmt input");
        var $MealAllowance_SubTotal = $("#MealAllowance_SubTotal");
        var TotalAmount = 0;
        var res = false;
        $Amount.each(function () {
            if ($(this).val() != "") {
                TotalAmount += parseFloat($(this).val());
            }
        });
        TotalAmount = Math.round(TotalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        if ($("#MealAllowanceItemStatus").val() == "0") {
            $MealAllowance_SubTotal.val("");
        } else {
            $MealAllowance_SubTotal.val(TotalAmount.toFixed(2));
        }
        GetMealAllowance_PaidCredit();
        SetTotalCost();
    }

</script>
<script type="text/javascript">
    function BindMealAllowance_SelectMealInfoEvent() {
        var $SelectDate = $("#SelectDate img");
        $SelectDate.bind("click", function () {
            var $dateText = $(this).prev();
            new Calendar().show($dateText[0]);
        });

        var $SelectArea = $("#SelectArea");
        var $SMA_ExchangeRate = $("#SMA_ExchangeRate");
        var $SMA_Currency = $("#SMA_Currency");
        $SelectArea.bind("change", function () {
            var $MealAllowance_ComStd = $("#MealAllowance_ComStd");
            var $hfTravelPolicy = $("input[id$='hfTravelPolicy']");
            var json = $hfTravelPolicy.val();
            if (json != "") {
                var travelPolicy = eval("(" + json + ")");
                var area = $(this).val();
                $.each(travelPolicy, function (i, item) {
                    if (item != undefined) {
                        if (item.Location == area.toLowerCase()) {
                            $SMA_Currency.val(item.Currency);
                            $MealAllowance_ComStd.val(item.BreakfastLimit + ";" + item.LunchLimit + ";" + item.DinnerLimit);
                        }
                    }
                });
                GetMealAllowance_ExchangeRate();
            }
        });

        $SMA_ExchangeRate.bind("blur", function () {
            if (isNaN($(this).val()) || $(this).val() < 0 || $(this).val() == "") {
                $(this).val("");
                $(this).css("borderColor", "red");
                alert("Please Input Exch Rate !");
            } else {
                $(this).css("borderColor", "#999");
            }
        });

        var $CloseMealAllowance = $("#SelectMealAllowanceInfo a.CloseMealAllowance");
        $CloseMealAllowance.bind("click", function () {
            $("#SelectMealAllowanceInfo").fadeOut(500);
            SetSelectMealAllowanceInfoVal();
        });

        var $Select = $("#SelectMealAllowanceInfo a.Select");
        $Select.bind("click", function () {
            var result = ValidateFromToDate();
            if (result) {
                return;
            }
            $("#SelectMealAllowanceInfo").hide();
            var $MealAllowanceItemStatus = $("#MealAllowanceItemStatus");
            var $Insert = $("div.MealAllowanceInsert");

            var $SMA_FromDate = $("#SMA_FromDate");
            var $SMA_TODate = $("#SMA_TODate");
            var DateCount = 1;
            var staus = false;
            if ($SMA_FromDate.val() != "" && $SMA_TODate.val() != "") {
                var StartDate = new Date(Date.parse($SMA_FromDate.val())).getTime();
                var EndDate = new Date(Date.parse($SMA_TODate.val())).getTime();
                DateCount = Math.abs((StartDate - EndDate)) / (1000 * 60 * 60 * 24);
                ++DateCount;
                staus = true;
            }
            for (var date = 0; date < DateCount; date++) {
                var AddCount = 3;
                if ($MealAllowanceItemStatus.val() == "0") {
                    $("div.MealAllowanceItem").show();
                    $MealAllowanceItemStatus.val("1");
                    AddCount = 2;
                }
                var $InsertItems1 = $Insert.prev();
                var $InsertItems2 = $InsertItems1.prev();
                var $InsertItems3 = $InsertItems2.prev();
                if (AddCount == 2) {
                    SetMealAllowanceValue($InsertItems3, 1, date, staus);
                }
                for (var i = 1; i <= AddCount; i++) {
                    ++MealAllowance_AddItemCount;
                    var $InsertItems3Clone = $InsertItems3.clone(true);
                    var $InsertItems2Clone = $InsertItems2.clone(true);
                    var $InsertItems1Clone = $InsertItems1.clone(true);
                    SetMealAllowance_ItemID($InsertItems3Clone, $InsertItems2Clone);
                    if (AddCount == 2) {
                        SetMealAllowanceValue($InsertItems3Clone, i + 1, date, staus);
                    } else {
                        SetMealAllowanceValue($InsertItems3Clone, i, date, staus);
                    }
                    $InsertItems3Clone.insertBefore($Insert);
                    $InsertItems2Clone.insertBefore($Insert);
                    $InsertItems1Clone.insertBefore($Insert);
                }
            }

            SetSelectMealAllowanceInfoVal();
            if (MealAllowance_AddItemCount - MealAllowance_DelItemCount <= 10) {
                CalMealAllowance_SubTotalAmount();
            } else {
                setTimeout(CalMealAllowance_SubTotalAmount, 0);
            }
        });
    }

    function GetMealAllowance_ExchangeRate() {
        var $SMA_ExchangeRate = $("#SMA_ExchangeRate");
        var $SMA_Currency = $("#SMA_Currency")
        var $hfExchangeRate = $("input[id$='hfExchangeRate']");
        var json = $hfExchangeRate.val();
        if (json != "") {
            var exchangeRate = eval("(" + json + ")");
            if ($SMA_Currency.val() == "RMB") {
                $SMA_ExchangeRate.val("1.0000");
            } else {
                $.each(exchangeRate, function (i, item) {
                    if (item != undefined) {
                        if (item.name == $SMA_Currency.val()) {
                            $SMA_ExchangeRate.val(item.val);
                        }
                    }
                });
            }
        }
    }

    function ShowOrHideMealAllowanceInfo() {
        var $SelectMealAllowanceInfo = $("#SelectMealAllowanceInfo");
        if ($SelectMealAllowanceInfo.css("display") == "block") {
            $SelectMealAllowanceInfo.fadeOut(500);
            SetSelectMealAllowanceInfoVal();
        } else {
            $SelectMealAllowanceInfo.fadeIn(1000);
        }
    }

    function SetSelectMealAllowanceInfoVal() {
        $("#SMA_FromDate").val("");
        $("#SMA_TODate").val("");
        $("#SelectArea").val("China");
        $("#SMA_Currency").val("RMB");
        $("#SMA_ExchangeRate").val("1.0000");
        $("#SMA_CostCenter").val("");
        $("#MealAllowance_ComStd").val("50;50;70");
    }
</script>
<script type="text/javascript">
    function DrawMealAllowanceForm() {
        var $Insert = $("div.MealAllowanceInsert");
        var $hfMealAllowanceForm = $("#Hidden input[id$='hfMealAllowanceForm']");
        var json = $hfMealAllowanceForm.val();
        if (json != "" && json != "[]") {
            var MealAllowanceForm = eval("(" + json + ")");
            var $AddItemStatus = $("#MealAllowanceItemStatus");
            $.each(MealAllowanceForm, function (i, item) {
                if (item != undefined) {
                    if ($AddItemStatus.val() == "0") {
                        $("div.MealAllowanceItem").show()
                        $AddItemStatus.val("1");
                    }
                    var $InsertItems1 = $Insert.prev();
                    var $InsertItems2 = $InsertItems1.prev();
                    var $InsertItems3 = $InsertItems2.prev();
                    var $ClaimItem = $InsertItems3;
                    var $Reamrk = $InsertItems2;
                    if (i != 0) {
                        ++MealAllowance_AddItemCount;
                        var $InsertItems3Clone = $InsertItems3.clone(true);
                        var $InsertItems2Clone = $InsertItems2.clone(true);
                        var $InsertItems1Clone = $InsertItems1.clone(true);
                        SetMealAllowance_ItemID($InsertItems3Clone, $InsertItems2Clone);
                        $InsertItems3Clone.insertBefore($Insert);
                        $InsertItems2Clone.insertBefore($Insert);
                        $InsertItems1Clone.insertBefore($Insert);
                        $ClaimItem = $InsertItems3Clone;
                        $Reamrk = $InsertItems2Clone;
                    }
                    SetClaimItem_Meal($ClaimItem,
                                    item.ExpenseType,
                                    item.Date,
                                    item.CostCenter,
                                    item.OriginalAmt,
                                    item.Currency,
                                    item.OtherCurrency,
                                    item.ExchRate,
                                    item.ClaimAmt,
                                    item.ComStd,
                                    item.PaidCredit);
                    SetMealRemarkItem($Reamrk, item.Remark);
                }
            });
            if (MealAllowance_AddItemCount - MealAllowance_DelItemCount <= 10) {
                CalMealAllowance_SubTotalAmount();
            } else {
                setTimeout(CalMealAllowance_SubTotalAmount, 0);
            }
        }
    }

    function SetClaimItem_Meal(ClaimItem,
                          ExpenseType,
                          Date,
                          CostCenter,
                          OriginalAmt,
                          Currency,
                          OtherCurrency,
                          ExchRate,
                          ClaimAmt,
                          ComStd,
                          PaidCredit) {
        ClaimItem.find("li.ExpenseType select").val(ExpenseType);
        ClaimItem.find("li.Date input").val(Date);
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

    function SetMealRemarkItem(Reamrk, RemarkValue) {
        if (RemarkValue != "") {
            var $input = Reamrk.find("input");
            var ID = $input.attr("id").replace("MealAllowance_MealAllowanceRemark", "");
            var $MealAllowance_ClaimAmt = $("#MealAllowance_ClaimAmt" + ID);
            var $MealAllowance_ComStd = $("#MealAllowance_ComStd" + ID);
            if ($MealAllowance_ComStd.val() != "") {
                if (parseFloat($MealAllowance_ClaimAmt.val()) > parseFloat($MealAllowance_ComStd.val())) {
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
