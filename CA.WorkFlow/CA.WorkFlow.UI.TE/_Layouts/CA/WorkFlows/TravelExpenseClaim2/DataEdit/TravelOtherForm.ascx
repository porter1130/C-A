<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TravelOtherForm.ascx.cs"
    Inherits="CA.WorkFlow.UI.TE.TravelOtherForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .OtherItem, .OtherRemark
    {
        display: none;
    }
</style>
<div class="Item_UL Other">
    <ul>
        <li class="w104 ImgText">
            <img src="../images/pixelicious_001.png" alt="Click to add the information." width="18"
                class="AddItem" align="absmiddle" />Others</li>
        <li class="w40 TitleText"></li>
        <li class="w20 Sub">Sub Total</li>
        <li class="w30 Total">
            <input type="text" value="" id="Other_SubTotal" readonly="readonly" /></li>
    </ul>
</div>
<div class="Item_UL Other_Items OtherItem">
    <ul>
        <li class="w104 ExpenseType">
            <img src="../images/pixelicious_028.png" alt="Remove this information." width="18"
                class="DelItem" align="absmiddle" /><input type="text" class="Date" id="Other_ExpenseType0"
                    value="Please specify" /></li>
        <li class="w15 Date">
            <input type="text" class="Date" id="Other_Date0" value="" readonly="readonly" />
            <img src="../../../CAResources/themeCA/images/selectcalendar.gif" align="absmiddle"
                alt="Select Date" /></li>
        <li class="w10 CostCenter">
            <div class="CostCenterContainer">
                <div class="CostCenterSub">
                    <select class="CostCenterCurrency CostCenter" id="Other_CostCenter0">
                        <option class="blank" value=""></option>
                    </select></div>
            </div>
        </li>
        <li class="w10 OriginalAmt">
            <input type="text" class="Date" id="Other_OriginalAmt0" value="" /></li>
        <li class="w10 Currency">
            <select class="CostCenterCurrency" id="Other_Currency0">
                <option class="blank" value=""></option>
            </select>
            <div class="CurrencyContainer">
                <div class="CurrencySub">
                    Other Currency<input type="text" class="Date" value="" /><br />
                    <a class="OK">OK</a><a class="Cancel">Select Currency List</a>
                </div>
            </div>
            <input type="text" class="Date OtherCurrency" id="Other_OtherCurrency0"
                value="" readonly="readonly" />
        </li>
        <li class="w10 ExchRate">
            <input type="text" class="Date" id="Other_ExchRate0" value="1.0000" /></li>
        <li class="w10 ClaimAmt">
            <input type="text" class="Date" id="Other_ClaimAmt0" value="" readonly="readonly" /></li>
        <li class="w9 bg ComStd">
            <input type="text" value="" id="Other_ComStd0" readonly="readonly" /></li>
        <li class="w14 bg PaidCredit">
            <input type="checkbox" class="Date" id="Other_PaidCredit0" value="" /></li>
    </ul>
</div>
<div class="Item_Head_Content Other_Items OtherRemark">
    <div class="Item_Head_Left w104">
        Remark</div>
    <div class="Item_Head_Right w774">
        <input id="Other_Remark0" runat="server" /></div>
</div>
<div class="Blank Other_Items">
    <div class="BlankLeft">
    </div>
    <div class="BlankRight">
    </div>
</div>
<div class="OtherInsert">
    <input type="hidden" id="Other_AddItemStatus" value="0" />
    <input type="hidden" id="Other_PaidCredit" value="0" />
</div>
<script type="text/javascript" src="JS/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        LoadOther_CostCenter();
        LoadOther_Currency();
        BindOther_AddAndDelItemEvent();
        BindOther_ExpenseTypeEvent();
        BindOther_DateEvent();
        BindOther_CostCenterEvent();
        BindOther_AmountEvent();
        BindOther_CurrencyEvent();
        BindOther_ExchRateEvent();
        BindOther_RemarkEvent();
        BindOther_PaidCreditEvent();
        DrawOtherForm();
    });

    function GetOther_Currency_Option(text, val) {
        var option = "";
        option = "<option class=\"item\" value=\"" + text + "\" >" + val + "</option>";
        return option;
    }

    function LoadOther_CostCenter() {
        var $CostCenter = $("div.OtherItem ul li.CostCenter select.CostCenter");
        var $Insert = $CostCenter.find("option.blank");
        var $hfCostCenter = $("input[id$='hfCostCenter']");
        var json = $hfCostCenter.val();
        if (json != "") {
            var costCenter = eval("(" + json + ")");
            var $blank = $("<option class=\"item\" value=\"\" ></option>");
            $Insert.before($blank);
            $.each(costCenter, function (i, item) {
                if (item != undefined) {
                    $InsertOption = $(GetOther_Currency_Option(item.val, item.name));
                    $Insert.before($InsertOption);
                }
            });
            $CostCenter.find("option").remove(".blank");
        }
    }

    function LoadOther_Currency() {
        var $Currency = $("div.OtherItem ul li.Currency select");
        var $Insert = $Currency.find("option.blank");
        var currency = ["RMB", "USD", "GBP", "EUR", "AUD", "CHF", "CAD", "JPY", "HKD", "Others"];
        for (var i = 0; i < currency.length; i++) {
            $InsertOption = $(GetOther_Currency_Option(currency[i], currency[i]));
            $Insert.before($InsertOption);
        }
        $Currency.find("option").remove(".blank");
    }
</script>
<script type="text/javascript">
    var Other_AddItemCount = 1;
    var Other_DelItemCount = 0;
    function BindOther_AddAndDelItemEvent() {
        var $AddItemStatus = $("#Other_AddItemStatus");
        var $AddItem = $("div.Other ul li.ImgText img.AddItem");
        var $DelItem = $("div.OtherItem ul li.ExpenseType img.DelItem");
        var $Insert = $("div.OtherInsert");
        $AddItem.bind("click", function () {
            if ($AddItemStatus.val() == "0") {
                $("div.OtherItem").show()
                $AddItemStatus.val("1");
                return;
            }
            ++Other_AddItemCount;
            var $InsertItems1 = $Insert.prev();
            var $InsertItems2 = $InsertItems1.prev();
            var $InsertItems3 = $InsertItems2.prev();
            var $InsertItems3Clone = $InsertItems3.clone(true);
            var $InsertItems2Clone = $InsertItems2.clone(true);
            var $InsertItems1Clone = $InsertItems1.clone(true);
            SetOther_ItemID($InsertItems3Clone, $InsertItems2Clone);
            $InsertItems3Clone.insertBefore($Insert);
            $InsertItems2Clone.insertBefore($Insert);
            $InsertItems1Clone.insertBefore($Insert);
            if (Other_AddItemCount - Other_DelItemCount <= 10) {
                CalOther_SubTotalAmount();
            } else {
                setTimeout(CalOther_SubTotalAmount, 0);
            }
        });
        $DelItem.bind("click", function () {
            ++Other_DelItemCount;
            if ($("div.Other_Items").length - 3 > 0) {
                var $items1 = $(this).parent().parent().parent();
                var $items2 = $items1.next();
                var $items3 = $items2.next();
                $items1.remove();
                $items2.remove();
                $items3.remove();
                if (Other_AddItemCount - Other_DelItemCount <= 10) {
                    CalOther_SubTotalAmount();
                } else {
                    setTimeout(CalOther_SubTotalAmount, 0);
                }
            } else {
                $("div.OtherItem").hide()
                $AddItemStatus.val("0");
                CalOther_SubTotalAmount();
            }
        });
    }

    function SetOther_ItemID(InsertItems3Clone, InsertItems2Clone) {
        var id = Other_AddItemCount - 1;
        InsertItems3Clone.find("li.ExpenseType input").attr("id", "Other_ExpenseType" + id);
        InsertItems3Clone.find("li.Date input").attr("id", "Other_Date" + id);
        InsertItems3Clone.find("li.CostCenter select").attr("id", "Other_CostCenter" + id);
        InsertItems3Clone.find("li.OriginalAmt input").attr("id", "Other_OriginalAmt" + id);
        InsertItems3Clone.find("li.Currency select").attr("id", "Other_Currency" + id);
        InsertItems3Clone.find("li.Currency input.OtherCurrency").attr("id", "Other_OtherCurrency" + id);
        InsertItems3Clone.find("li.ExchRate input").attr("id", "Other_ExchRate" + id);
        InsertItems3Clone.find("li.ClaimAmt input").attr("id", "Other_ClaimAmt" + id);
        InsertItems3Clone.find("li.ComStd input").attr("id", "Other_ComStd" + id);
        InsertItems3Clone.find("li.PaidCredit input").attr("id", "Other_PaidCredit" + id);
        InsertItems2Clone.find("input").attr("id", "Other_Remark" + id);
    }
</script>
<script type="text/javascript">
    function BindOther_ExpenseTypeEvent() {
        var $ExpenseType = $("div.OtherItem li.ExpenseType input");
        $ExpenseType.bind("focus", function () {
            if ($(this).val() == "Please specify") {
                $(this).val("");
            }
        }).bind("blur", function () {
            if ($(this).val() == "") {
                $(this).val("Please specify");
            }
        });
    }

    function BindOther_DateEvent() {
        var $Date = $("div.OtherItem li.Date img");
        $Date.bind("click", function () {
            var $dateText = $(this).prev();
            new Calendar().show($dateText[0]);
        });
    }

    function BindOther_CostCenterEvent() {
        var $CostCenter = $("div.OtherItem li.CostCenter select");
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

    function GetOther_PaidCredit() {
        var $PaidCredit = $("div.OtherItem li.PaidCredit input");
        var $PaidCreditAmount = $("#Other_PaidCredit");
        var TotalAmount = 0;
        $PaidCredit.each(function () {
            var IsPaidCredit = $(this).attr("checked");
            if (IsPaidCredit) {
                var ID = $(this).attr("id").replace("Other_PaidCredit", "");
                var $ClaimAmt = $("#Other_ClaimAmt" + ID);
                TotalAmount += parseFloat($ClaimAmt.val());
            }
        });
        TotalAmount = Math.round(TotalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        if ($("#Other_AddItemStatus").val() == "0") {
            $PaidCreditAmount.val("0");
        } else {
            $PaidCreditAmount.val(TotalAmount.toFixed(2));
        }
    }

    function BindOther_PaidCreditEvent() {
        var $PaidCredit = $("div.OtherItem li.PaidCredit input");
        $PaidCredit.bind("click", function () {
            CalOther_SubTotalAmount();
        });
    }

    function BindOther_AmountEvent() {
        var $Amount = $("div.OtherItem li.OriginalAmt input");
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
                var ID = $(this).attr("id").replace("Other_OriginalAmt", "");
                var $Other_ExchRate = $("#Other_ExchRate" + ID);
                if ($Other_ExchRate.val() == "") {
                    alert("Please Input Exchange Rate !");
                    $Other_ExchRate.css("borderColor", "red");
                    return;
                }
                var excRate = parseFloat($Other_ExchRate.val());
                var ClaimAmt = amount * excRate;
                ClaimAmt = Math.round(ClaimAmt * Math.pow(10, 2)) / Math.pow(10, 2);
                var $Other_ClaimAmt = $("#Other_ClaimAmt" + ID);
                $Other_ClaimAmt.val(ClaimAmt.toFixed(2));

                ShowOrHideOtherRemark(ID);
            }
        });
    }

    function BindOther_CurrencyEvent() {
        var $Currency = $("div.OtherItem li.Currency select");
        $Currency.bind("change", function () {
            var ID = $(this).attr("id").replace("Other_Currency", "");
            var $Other_ExchRate = $("#Other_ExchRate" + ID);
            $Other_ExchRate.val("");
            var $Other_ClaimAmt = $("#Other_ClaimAmt" + ID);
            var curval = $(this).val();
            var $cur = $(this);
            if (curval == "Others") {
                $cur.hide();
                var $next = $cur.next();
                $next.find("div.CurrencySub").show();
                $next.find("input").val("");
                $next.next().show();
                $Other_ClaimAmt.val("");
            } else {
                var $hfExchangeRate = $("input[id$='hfExchangeRate']");
                var json = $hfExchangeRate.val();
                if (json != "") {
                    var exchangeRate = eval("(" + json + ")");
                    if (curval == "RMB") {
                        $Other_ExchRate.val("1.0000");
                    } else {
                        $.each(exchangeRate, function (i, item) {
                            if (item != undefined) {
                                if (item.name == curval) {
                                    $Other_ExchRate.val(item.val);
                                }
                            }
                        });
                    }
                }
            }
            $Other_ExchRate.blur();
        });
        var $OK = $("div.OtherItem li.Currency a.OK");
        var $Cancel = $("div.OtherItem li.Currency a.Cancel");
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
            var ID = $parent.parent().prev().attr("id").replace("Other_Currency", "");
            var $Other_ExchRate = $("#Other_ExchRate" + ID);
            $Other_ExchRate.val("");
            $Other_ExchRate.focus();
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
            var ID = $prev.attr("id").replace("Other_Currency", "");
            var $Other_ExchRate = $("#Other_ExchRate" + ID);
            $Other_ExchRate.val("1.0000");
            var $Other_OriginalAmt = $("#Other_OriginalAmt" + ID);
            $Other_OriginalAmt.blur();
        });
        var $OtherCurrency = $("div.OtherItem li.Currency input.OtherCurrency");
        $OtherCurrency.bind("click", function () {
            $(this).val("");
            var $prev = $(this).prev();
            $prev.find("div.CurrencySub").show();
            $prev.find("input").val("");
        }).bind("focus", function () {
            $(this).blur();
        });
    }

    function BindOther_ExchRateEvent() {
        var $ExchRate = $("div.OtherItem li.ExchRate input");
        $ExchRate.bind("blur", function () {
            var ID = $(this).attr("id").replace("Other_ExchRate", "");
            if ($(this).val() == "") {
                ShowOrHideOtherRemark(ID);
                return;
            }
            var $Other_ClaimAmt = $("#Other_ClaimAmt" + ID);
            if (isNaN($(this).val()) || $(this).val() < 0) {
                $(this).val("");
                alert("Please Input Exch Rate !");
                $(this).css("borderColor", "red");
                $Other_ClaimAmt.val("");
                return;
            } else {
                $(this).css("borderColor", "#999");
            }

            var $Other_OriginalAmt = $("#Other_OriginalAmt" + ID);
            if ($Other_OriginalAmt.val() == "") {
                alert("Please Input Original Amount !");
                $Other_OriginalAmt.css("borderColor", "red");
                return;
            }
            var excRate = parseFloat($(this).val());
            var ClaimAmt = parseFloat($Other_OriginalAmt.val()) * excRate;
            ClaimAmt = Math.round(ClaimAmt * Math.pow(10, 2)) / Math.pow(10, 2);
            $Other_ClaimAmt.val(ClaimAmt.toFixed(2));

            ShowOrHideOtherRemark(ID);
        });
    }

    function BindOther_RemarkEvent() {
        var $OtherRemark = $("div.OtherRemark input");
        $OtherRemark.bind("focus", function () {
            if ($(this).val() == "Why exceeds company standard?") {
                $(this).val("");
            }
        }).bind("blur", function () {
            if ($(this).val() == "") {
                $(this).val("Why exceeds company standard?");
            }
        });
    }

    function ShowOrHideOtherRemark(ID) {
        var $Other_ClaimAmt = $("#Other_ClaimAmt" + ID);
        var $Other_ComStd = $("#Other_ComStd" + ID);
        var $Other_Remark = $("#Other_Remark" + ID);
        $Other_ClaimAmt.css("color", "#06c");
        $Other_Remark.parent().parent().hide();
        $Other_Remark.val("");
        if ($Other_ClaimAmt.val() != "" && $Other_ComStd.val() != "") {
            if (parseFloat($Other_ClaimAmt.val()) > parseFloat($Other_ComStd.val())) {
                $Other_ClaimAmt.css("color", "red");
                $Other_Remark.parent().parent().show();
                $Other_Remark.val("Why exceeds company standard?");
            }
        }
        if (Other_AddItemCount - Other_DelItemCount <= 10) {
            CalOther_SubTotalAmount();
        } else {
            setTimeout(CalOther_SubTotalAmount, 0);
        }
    }

    function CalOther_SubTotalAmount() {
        var $Amount = $("div.OtherItem li.ClaimAmt input");
        var $Other_SubTotal = $("#Other_SubTotal");
        var TotalAmount = 0;
        var res = false;
        $Amount.each(function () {
            if ($(this).val() != "") {
                TotalAmount += parseFloat($(this).val());
            }
        });
        TotalAmount = Math.round(TotalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        if ($("#Other_AddItemStatus").val() == "0") {
            $Other_SubTotal.val("");
        } else {
            $Other_SubTotal.val(TotalAmount.toFixed(2));
        }
        GetOther_PaidCredit();
        SetTotalCost();
    }

</script>
<script type="text/javascript">
    function DrawOtherForm() {
        var $Insert = $("div.OtherInsert");
        var $hfOtherForm = $("#Hidden input[id$='hfOtherForm']");
        var json = $hfOtherForm.val();
        if (json != "" && json != "[]") {
            var OtherForm = eval("(" + json + ")");
            var $AddItemStatus = $("#Other_AddItemStatus");
            $.each(OtherForm, function (i, item) {
                if (item != undefined) {
                    if ($AddItemStatus.val() == "0") {
                        $("div.OtherItem").show()
                        $AddItemStatus.val("1");
                    }
                    var $InsertItems1 = $Insert.prev();
                    var $InsertItems2 = $InsertItems1.prev();
                    var $InsertItems3 = $InsertItems2.prev();
                    var $ClaimItem = $InsertItems3;
                    if (i != 0) {
                        ++Other_AddItemCount;
                        var $InsertItems3Clone = $InsertItems3.clone(true);
                        var $InsertItems2Clone = $InsertItems2.clone(true);
                        var $InsertItems1Clone = $InsertItems1.clone(true);
                        SetOther_ItemID($InsertItems3Clone, $InsertItems2Clone);
                        $InsertItems3Clone.insertBefore($Insert);
                        $InsertItems2Clone.insertBefore($Insert);
                        $InsertItems1Clone.insertBefore($Insert);
                        $ClaimItem = $InsertItems3Clone;
                    }
                    SetClaimItem_TSO($ClaimItem,
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
                }
            });
            if (Other_AddItemCount - Other_DelItemCount <= 10) {
                CalOther_SubTotalAmount();
            } else {
                setTimeout(CalOther_SubTotalAmount, 0);
            }
        }
    }

</script>
