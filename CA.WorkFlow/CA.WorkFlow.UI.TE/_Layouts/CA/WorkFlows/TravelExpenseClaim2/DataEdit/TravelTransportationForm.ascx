<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TravelTransportationForm.ascx.cs"
    Inherits="CA.WorkFlow.UI.TE.TravelTransportationForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .TransportationItem, .TransportationRemark
    {
        display: none;
    }
</style>
<div class="Item_UL Transportation">
    <ul>
        <li class="w104 ImgText">
            <img src="../images/pixelicious_001.png" alt="Click to add the information." width="18"
                class="AddItem" align="absmiddle" />Transportation</li>
        <li class="w40 TitleText"></li>
        <li class="w20 Sub">Sub Total</li>
        <li class="w30 Total">
            <input type="text" value="" id="Transportation_SubTotal" readonly="readonly" /></li>
    </ul>
</div>
<div class="Item_UL Transportation_Items TransportationItem">
    <ul>
        <li class="w104 ExpenseType">
            <img src="../images/pixelicious_028.png" alt="Remove this information." width="18"
                class="DelItem" align="absmiddle" />
            <input type="text" class="Date" id="Transportation_ExpenseType0" value="Purpose of local transportation" /></li>
        <li class="w15 Date">
            <input type="text" class="Date" id="Transportation_Date0" value="" readonly="readonly" />
            <img src="../../../CAResources/themeCA/images/selectcalendar.gif" align="absmiddle"
                alt="Select Date" /></li>
        <li class="w10 CostCenter">
            <div class="CostCenterContainer">
                <div class="CostCenterSub">
                    <select class="CostCenterCurrency CostCenter" id="Transportation_CostCenter0">
                        <option class="blank" value=""></option>
                    </select></div>
            </div>
        </li>
        <li class="w10 OriginalAmt">
            <input type="text" class="Date" id="Transportation_OriginalAmt0" value="" /></li>
        <li class="w10 Currency">
            <select class="CostCenterCurrency" id="Transportation_Currency0">
                <option class="blank" value=""></option>
            </select>
            <div class="CurrencyContainer">
                <div class="CurrencySub">
                    Other Currency<input type="text" class="Date" value="" /><br />
                    <a class="OK">OK</a><a class="Cancel">Select Currency List</a>
                </div>
            </div>
            <input type="text" class="Date OtherCurrency" id="Transportation_OtherCurrency0"
                value="" readonly="readonly" />
        </li>
        <li class="w10 ExchRate">
            <input type="text" class="Date" id="Transportation_ExchRate0" value="1.0000" /></li>
        <li class="w10 ClaimAmt">
            <input type="text" class="Date" id="Transportation_ClaimAmt0" value="" readonly="readonly" /></li>
        <li class="w9 bg ComStd">
            <input type="text" value="" id="Transportation_ComStd0" readonly="readonly" /></li>
        <li class="w14 bg PaidCredit">
            <input type="checkbox" class="Date" id="Transportation_PaidCredit0" value="" /></li>
    </ul>
</div>
<div class="Item_Head_Content Transportation_Items TransportationRemark">
    <div class="Item_Head_Left w104">
        Remark</div>
    <div class="Item_Head_Right w774">
        <input id="Transportation_Remark0" runat="server" /></div>
</div>
<div class="Blank Transportation_Items">
    <div class="BlankLeft">
    </div>
    <div class="BlankRight">
    </div>
</div>
<div class="TransportationInsert">
    <input type="hidden" id="Transportation_AddItemStatus" value="0" />
    <input type="hidden" id="Transportation_PaidCredit" value="0" />
</div>
<script type="text/javascript" src="JS/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        LoadTransportation_CostCenter();
        LoadTransportation_Currency();
        BindTransportation_AddAndDelItemEvent();
        BindTransportation_ExpenseTypeEvent();
        BindTransportation_DateEvent();
        BindTransportation_CostCenterEvent();
        BindTransportation_AmountEvent();
        BindTransportation_CurrencyEvent();
        BindTransportation_ExchRateEvent();
        BindTransportation_RemarkEvent();
        BindTransportation_PaidCreditEvent();
        DrawTransportationForm();
    });

    function GetTransportation_Currency_Option(text, val) {
        var option = "";
        option = "<option class=\"item\" value=\"" + text + "\" >" + val + "</option>";
        return option;
    }

    function LoadTransportation_CostCenter() {
        var $CostCenter = $("div.TransportationItem ul li.CostCenter select.CostCenter");
        var $Insert = $CostCenter.find("option.blank");
        var $hfCostCenter = $("input[id$='hfCostCenter']");
        var json = $hfCostCenter.val();
        if (json != "") {
            var costCenter = eval("(" + json + ")");
            var $blank = $("<option class=\"item\" value=\"\" ></option>");
            $Insert.before($blank);
            $.each(costCenter, function (i, item) {
                if (item != undefined) {
                    $InsertOption = $(GetTransportation_Currency_Option(item.val, item.name));
                    $Insert.before($InsertOption);
                }
            });
            $CostCenter.find("option").remove(".blank");
        }
    }

    function LoadTransportation_Currency() {
        var $Currency = $("div.TransportationItem ul li.Currency select");
        var $Insert = $Currency.find("option.blank");
        var currency = ["RMB", "USD", "GBP", "EUR", "AUD", "CHF", "CAD", "JPY", "HKD", "Others"];
        for (var i = 0; i < currency.length; i++) {
            $InsertOption = $(GetTransportation_Currency_Option(currency[i], currency[i]));
            $Insert.before($InsertOption);
        }
        $Currency.find("option").remove(".blank");
    }
</script>
<script type="text/javascript">
    var Transportation_AddItemCount = 1;
    var Transportation_DelItemCount = 0;
    function BindTransportation_AddAndDelItemEvent() {
        var $AddItemStatus = $("#Transportation_AddItemStatus");
        var $AddItem = $("div.Transportation ul li.ImgText img.AddItem");
        var $DelItem = $("div.TransportationItem ul li.ExpenseType img.DelItem");
        var $Insert = $("div.TransportationInsert");
        $AddItem.bind("click", function () {
            if ($AddItemStatus.val() == "0") {
                $("div.TransportationItem").show()
                $AddItemStatus.val("1");
                return;
            }
            ++Transportation_AddItemCount;
            var $InsertItems1 = $Insert.prev();
            var $InsertItems2 = $InsertItems1.prev();
            var $InsertItems3 = $InsertItems2.prev();
            var $InsertItems3Clone = $InsertItems3.clone(true);
            var $InsertItems2Clone = $InsertItems2.clone(true);
            var $InsertItems1Clone = $InsertItems1.clone(true);
            SetTransportation_ItemID($InsertItems3Clone, $InsertItems2Clone);
            $InsertItems3Clone.insertBefore($Insert);
            $InsertItems2Clone.insertBefore($Insert);
            $InsertItems1Clone.insertBefore($Insert);
            if (Transportation_AddItemCount - Transportation_DelItemCount <= 10) {
                CalTransportation_SubTotalAmount();
            } else {
                setTimeout(CalTransportation_SubTotalAmount, 0);
            }
        });
        $DelItem.bind("click", function () {
            ++Transportation_DelItemCount;
            if ($("div.Transportation_Items").length - 3 > 0) {
                var $items1 = $(this).parent().parent().parent();
                var $items2 = $items1.next();
                var $items3 = $items2.next();
                $items1.remove();
                $items2.remove();
                $items3.remove();
                if (Transportation_AddItemCount - Transportation_DelItemCount <= 10) {
                    CalTransportation_SubTotalAmount();
                } else {
                    setTimeout(CalTransportation_SubTotalAmount, 0);
                }
            } else {
                $("div.TransportationItem").hide()
                $AddItemStatus.val("0");
                CalTransportation_SubTotalAmount();
            }
        });
    }

    function SetTransportation_ItemID(InsertItems3Clone, InsertItems2Clone) {
        var id = Transportation_AddItemCount - 1;
        InsertItems3Clone.find("li.ExpenseType input").attr("id", "Transportation_ExpenseType" + id);
        InsertItems3Clone.find("li.Date input").attr("id", "Transportation_Date" + id);
        InsertItems3Clone.find("li.CostCenter select").attr("id", "Transportation_CostCenter" + id);
        InsertItems3Clone.find("li.OriginalAmt input").attr("id", "Transportation_OriginalAmt" + id);
        InsertItems3Clone.find("li.Currency select").attr("id", "Transportation_Currency" + id);
        InsertItems3Clone.find("li.Currency input.OtherCurrency").attr("id", "Transportation_OtherCurrency" + id);
        InsertItems3Clone.find("li.ExchRate input").attr("id", "Transportation_ExchRate" + id);
        InsertItems3Clone.find("li.ClaimAmt input").attr("id", "Transportation_ClaimAmt" + id);
        InsertItems3Clone.find("li.ComStd input").attr("id", "Transportation_ComStd" + id);
        InsertItems3Clone.find("li.PaidCredit input").attr("id", "Transportation_PaidCredit" + id);
        InsertItems2Clone.find("input").attr("id", "Transportation_Remark" + id);
    }
</script>
<script type="text/javascript">
    function BindTransportation_ExpenseTypeEvent() {
        var $ExpenseType = $("div.TransportationItem li.ExpenseType input");
        $ExpenseType.bind("focus", function () {
            if ($(this).val() == "Purpose of local transportation") {
                $(this).val("");
            }
        }).bind("blur", function () {
            if ($(this).val() == "") {
                $(this).val("Purpose of local transportation");
            }
        });
    }

    function BindTransportation_DateEvent() {
        var $Date = $("div.TransportationItem li.Date img");
        $Date.bind("click", function () {
            var $dateText = $(this).prev();
            new Calendar().show($dateText[0]);
        });
    }

    function BindTransportation_CostCenterEvent() {
        var $CostCenter = $("div.TransportationItem li.CostCenter select");
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

    function GetTransportation_PaidCredit() {
        var $PaidCredit = $("div.TransportationItem li.PaidCredit input");
        var $PaidCreditAmount = $("#Transportation_PaidCredit");
        var TotalAmount = 0;
        $PaidCredit.each(function () {
            var IsPaidCredit = $(this).attr("checked");
            if (IsPaidCredit) {
                var ID = $(this).attr("id").replace("Transportation_PaidCredit", "");
                var $ClaimAmt = $("#Transportation_ClaimAmt" + ID);
                TotalAmount += parseFloat($ClaimAmt.val());
            }
        });
        TotalAmount = Math.round(TotalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        if ($("#Transportation_AddItemStatus").val() == "0") {
            $PaidCreditAmount.val("0");
        } else {
            $PaidCreditAmount.val(TotalAmount.toFixed(2));
        }
    }

    function BindTransportation_PaidCreditEvent() {
        var $PaidCredit = $("div.TransportationItem li.PaidCredit input");
        $PaidCredit.bind("click", function () {
            CalTransportation_SubTotalAmount();
        });
    }

    function BindTransportation_AmountEvent() {
        var $Amount = $("div.TransportationItem li.OriginalAmt input");
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
                var ID = $(this).attr("id").replace("Transportation_OriginalAmt", "");
                var $Transportation_ExchRate = $("#Transportation_ExchRate" + ID);
                if ($Transportation_ExchRate.val() == "") {
                    alert("Please Input Exchange Rate !");
                    $Transportation_ExchRate.css("borderColor", "red");
                    return;
                }
                var excRate = parseFloat($Transportation_ExchRate.val());
                var ClaimAmt = amount * excRate;
                ClaimAmt = Math.round(ClaimAmt * Math.pow(10, 2)) / Math.pow(10, 2);
                var $Transportation_ClaimAmt = $("#Transportation_ClaimAmt" + ID);
                $Transportation_ClaimAmt.val(ClaimAmt.toFixed(2));

                ShowOrHideTransportationRemark(ID);
            }
        });
    }

    function BindTransportation_CurrencyEvent() {
        var $Currency = $("div.TransportationItem li.Currency select");
        $Currency.bind("change", function () {
            var ID = $(this).attr("id").replace("Transportation_Currency", "");
            var $Transportation_ExchRate = $("#Transportation_ExchRate" + ID);
            $Transportation_ExchRate.val("");
            var $Transportation_ClaimAmt = $("#Transportation_ClaimAmt" + ID);
            var curval = $(this).val();
            var $cur = $(this);
            if (curval == "Others") {
                $cur.hide();
                var $next = $cur.next();
                $next.find("div.CurrencySub").show();
                $next.find("input").val("");
                $next.next().show();
                $Transportation_ClaimAmt.val("");
            } else {
                var $hfExchangeRate = $("input[id$='hfExchangeRate']");
                var json = $hfExchangeRate.val();
                if (json != "") {
                    var exchangeRate = eval("(" + json + ")");
                    if (curval == "RMB") {
                        $Transportation_ExchRate.val("1.0000");
                    } else {
                        $.each(exchangeRate, function (i, item) {
                            if (item != undefined) {
                                if (item.name == curval) {
                                    $Transportation_ExchRate.val(item.val);
                                }
                            }
                        });
                    }
                }
            }
            $Transportation_ExchRate.blur();
        });
        var $OK = $("div.TransportationItem li.Currency a.OK");
        var $Cancel = $("div.TransportationItem li.Currency a.Cancel");
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
            var ID = $parent.parent().prev().attr("id").replace("Transportation_Currency", "");
            var $Transportation_ExchRate = $("#Transportation_ExchRate" + ID);
            $Transportation_ExchRate.val("");
            $Transportation_ExchRate.focus();
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
            var ID = $prev.attr("id").replace("Transportation_Currency", "");
            var $Transportation_ExchRate = $("#Transportation_ExchRate" + ID);
            $Transportation_ExchRate.val("1.0000");
            var $Transportation_OriginalAmt = $("#Transportation_OriginalAmt" + ID);
            $Transportation_OriginalAmt.blur();
        });
        var $OtherCurrency = $("div.TransportationItem li.Currency input.OtherCurrency");
        $OtherCurrency.bind("click", function () {
            $(this).val("");
            var $prev = $(this).prev();
            $prev.find("div.CurrencySub").show();
            $prev.find("input").val("");
        }).bind("focus", function () {
            $(this).blur();
        });
    }

    function BindTransportation_ExchRateEvent() {
        var $ExchRate = $("div.TransportationItem li.ExchRate input");
        $ExchRate.bind("blur", function () {
            var ID = $(this).attr("id").replace("Transportation_ExchRate", "");
            if ($(this).val() == "") {
                ShowOrHideTransportationRemark(ID);
                return;
            }
            var $Transportation_ClaimAmt = $("#Transportation_ClaimAmt" + ID);
            if (isNaN($(this).val()) || $(this).val() < 0) {
                $(this).val("");
                alert("Please Input Exch Rate !");
                $(this).css("borderColor", "red");
                $Transportation_ClaimAmt.val("");
                return;
            } else {
                $(this).css("borderColor", "#999");
            }

            var $Transportation_OriginalAmt = $("#Transportation_OriginalAmt" + ID);
            if ($Transportation_OriginalAmt.val() == "") {
                alert("Please Input Original Amount !");
                $Transportation_OriginalAmt.css("borderColor", "red");
                return;
            }
            var excRate = parseFloat($(this).val());
            var ClaimAmt = parseFloat($Transportation_OriginalAmt.val()) * excRate;
            ClaimAmt = Math.round(ClaimAmt * Math.pow(10, 2)) / Math.pow(10, 2);
            $Transportation_ClaimAmt.val(ClaimAmt.toFixed(2));

            ShowOrHideTransportationRemark(ID);
        });
    }

    function BindTransportation_RemarkEvent() {
        var $TransportationRemark = $("div.TransportationRemark input");
        $TransportationRemark.bind("focus", function () {
            if ($(this).val() == "Why exceeds company standard?") {
                $(this).val("");
            }
        }).bind("blur", function () {
            if ($(this).val() == "") {
                $(this).val("Why exceeds company standard?");
            }
        });
    }

    function ShowOrHideTransportationRemark(ID) {
        var $Transportation_ClaimAmt = $("#Transportation_ClaimAmt" + ID);
        var $Transportation_ComStd = $("#Transportation_ComStd" + ID);
        var $Transportation_Remark = $("#Transportation_Remark" + ID);
        $Transportation_ClaimAmt.css("color", "#06c");
        $Transportation_Remark.parent().parent().hide();
        $Transportation_Remark.val("");
        if ($Transportation_ClaimAmt.val() != "" && $Transportation_ComStd.val() != "") {
            if (parseFloat($Transportation_ClaimAmt.val()) > parseFloat($Transportation_ComStd.val())) {
                $Transportation_ClaimAmt.css("color", "red");
                $Transportation_Remark.parent().parent().show();
                $Transportation_Remark.val("Why exceeds company standard?");
            }
        }
        if (Transportation_AddItemCount - Transportation_DelItemCount <= 10) {
            CalTransportation_SubTotalAmount();
        } else {
            setTimeout(CalTransportation_SubTotalAmount, 0);
        }
    }

    function CalTransportation_SubTotalAmount() {
        var $Amount = $("div.TransportationItem li.ClaimAmt input");
        var $Transportation_SubTotal = $("#Transportation_SubTotal");
        var TotalAmount = 0;
        var res = false;
        $Amount.each(function () {
            if ($(this).val() != "") {
                TotalAmount += parseFloat($(this).val());
            }
        });
        TotalAmount = Math.round(TotalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        if ($("#Transportation_AddItemStatus").val() == "0") {
            $Transportation_SubTotal.val("");
        } else {
            $Transportation_SubTotal.val(TotalAmount.toFixed(2));
        }
        GetTransportation_PaidCredit();
        SetTotalCost();
    }

</script>
<script type="text/javascript">
    function DrawTransportationForm() {
        var $Insert = $("div.TransportationInsert");
        var $hfTransportationForm = $("#Hidden input[id$='hfTransportationForm']");
        var json = $hfTransportationForm.val();
        if (json != "" && json != "[]") {
            var TransportationForm = eval("(" + json + ")");
            var $AddItemStatus = $("#Transportation_AddItemStatus");
            $.each(TransportationForm, function (i, item) {
                if (item != undefined) {
                    if ($AddItemStatus.val() == "0") {
                        $("div.TransportationItem").show()
                        $AddItemStatus.val("1");
                    }
                    var $InsertItems1 = $Insert.prev();
                    var $InsertItems2 = $InsertItems1.prev();
                    var $InsertItems3 = $InsertItems2.prev();
                    var $ClaimItem = $InsertItems3;
                    if (i != 0) {
                        ++Transportation_AddItemCount;
                        var $InsertItems3Clone = $InsertItems3.clone(true);
                        var $InsertItems2Clone = $InsertItems2.clone(true);
                        var $InsertItems1Clone = $InsertItems1.clone(true);
                        SetTransportation_ItemID($InsertItems3Clone, $InsertItems2Clone);
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
            if (Transportation_AddItemCount - Transportation_DelItemCount <= 10) {
                CalTransportation_SubTotalAmount();
            } else {
                setTimeout(CalTransportation_SubTotalAmount, 0);
            }
        }
    }

    function SetClaimItem_TSO(ClaimItem,
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
        ClaimItem.find("li.ExpenseType input").val(ExpenseType);
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
        ClaimItem.find("li.ClaimAmt input").val(ClaimAmt);
        ClaimItem.find("li.ComStd input").val(ComStd);
        var $PaidCredit = ClaimItem.find("li.PaidCredit input");
        if (PaidCredit == "1") {
            $PaidCredit.attr("checked", true);
        } else {
            $PaidCredit.attr("checked", false);
        }
    }
        
</script>