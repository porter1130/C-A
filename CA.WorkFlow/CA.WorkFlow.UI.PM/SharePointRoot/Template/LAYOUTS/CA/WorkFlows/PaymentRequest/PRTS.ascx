<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PRTS.ascx.cs" Inherits="CA.WorkFlow.UI.PaymentRequest.PRTS" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .TwoStep
    {
        display: none;
    }
    img
    {
        cursor: pointer;
    }
    .QueryPRHistory
    {
        width: 680px;
        height: 560px;
        border: #9dabb6 2px solid;
        float: left;
        background-color: White;
        z-index: 999999;
        display: none;
    }
    .loading
    {
        width: 680px;
        float: left;
        text-align: center;
        display: none;
        padding: 50px 0px 50px 0px;
    }
    .title
    {
        float: left;
        width: 660px;
        border-bottom: 1px solid #aaaaaa;
        font-weight: bold;
        color: #000000;
        padding: 10px;
    }
    .left
    {
        float: left;
    }
    .right
    {
        float: right;
        cursor: pointer;
    }
    .content
    {
        float: left;
        width: 100%;
        color: #000000;
        padding: 10px 5px 10px 5px;
        height: auto;
    }
    #BGDiv
    {
        position: absolute;
        top: 0px;
        left: 0px;
        right: 0px;
        bottom: 0px;
        display: none;
        background-color: White;
        filter: Alpha(opacity=30);
        z-index: 1;
    }
    .content_ul
    {
        width: 100%;
    }
    .content_ul li
    {
        background-image: url(/_layouts/images/viewheadergrad.gif);
        background-color: #f2f2f2;
        background-repeat: repeat-x;
        color: #b2b2b2;
        border-bottom: #cccccc 1px solid;
        border-right: #cccccc 1px solid;
        padding: 10px 0px 10px 3px;
        font-weight: lighter;
        float: left;
        text-align: center;
    }
    .list_ul
    {
        width: 100%;
    }
    .list_ul li
    {
        color: #000000;
        padding: 10px 0px 10px 3px;
        font-weight: lighter;
        float: left;
        text-align: center;
        border-bottom: #cccccc 1px solid;
        border-right: #cccccc 1px solid;
        cursor: pointer;
    }
    .list_ul a
    {
        color: #29a8df;
        cursor: pointer;
    }
    .WorkFlowNumber
    {
        width: 20%;
        border-left: #cccccc 1px solid;
    }
    .VendorName
    {
        width: 20%;
    }
    .ContractNumber
    {
        width: 20%;
    }
    .PaymentType
    {
        width: 15%;
    }
    .View, .Copy
    {
        width: 10%;
    }
    .paging
    {
        width: 100%;
        margin: 0 auto;
        text-align: center;
        padding-top: 20px;
        display: block;
        padding-bottom: 10px;
    }
    .paging span
    {
        padding: 5px;
        padding-right: 15px;
    }
    .error
    {
        font-weight: bold;
        color: Red;
        width: 680px;
        float: left;
        text-align: center;
        display: none;
        padding: 50px 0px 50px 0px;
    }
    #condition
    {
        float: left;
        width: 645px;
        color: #000000;
        padding: 10px;
        font-family: "Segoe UI" , Arial, "Sans-Serif";
        border: #cccccc 1px solid;
        margin: 15px 0px 7px 5px;
        line-height: 25px;
    }
    #condition input
    {
        border: none;
        border-bottom: #999 1px solid;
        color: #06c;
        margin-right: 20px;
        width: 150px;
    }
    .QueryInfo
    {
        color: #29a8df;
        font-size: 16px;
        cursor: pointer;
        margin-left: 40px;
    }
    .query_left_div
    {
        width: 100%;
        float: left;
    }
</style>
<div class="table">
    <table class="ca-workflow-form-table" id="StepTable">
        <tr class="OneStep">
            <td colspan="2" class="label" style="text-align: center; line-height: 20px; font-size: 13px;
                font-weight: bold">
                Please choose the payment type ICON to enter the payment request page<br />
                请选择付款类型图标以进入支付申请页面
            </td>
        </tr>
        <tr class="OneStep">
            <td class="label w50" align="center" style="border: none; padding: 20px 0px 20px 0px">
                <img src="/_layouts/CAResources/themeCA/images/Opex_1.jpg" width="160" height="130"
                    border="0" mytitle="/WorkFlowCenter/lists/PaymentRequestItems/NewForm.aspx?IsFromIco=1&RequestType=Opex" />
            </td>
            <td class="label w50" align="center" style="border: none; padding: 20px 0px 20px 0px">
                <img src="/_layouts/CAResources/themeCA/images/CapexWithFANO_1.jpg" width="160" height="130"
                    border="0" mytitle="/WorkFlowCenter/lists/PaymentRequestItems/NewForm.aspx?IsFromIco=1&RequestType=Capex_AssetNo" />
            </td>
        </tr>
        <tr class="TwoStep">
            <td colspan="2" class="label" style="text-align: center; line-height: 20px; font-size: 13px;
                font-weight: bold">
                Please choose whether to need to copy the history data for current payment request<br />
                请选择当前支付申请是否需要拷贝历史数据
            </td>
        </tr>
        <tr class="TwoStep">
            <td class="label w50" align="center" style="border: none; padding: 20px 0px 20px 0px">
                <img src="/_layouts/CAResources/themeCA/images/NONeedCopy.jpg" width="160" height="130"
                    border="0" class="NoNeedCopy" />
            </td>
            <td class="label w50" align="center" style="border: none; padding: 20px 0px 20px 0px">
                <img src="/_layouts/CAResources/themeCA/images/NeedCopy.jpg" width="160" height="130"
                    border="0" class="NeedCopy" />
            </td>
        </tr>
    </table>
</div>
<div class="QueryPRHistory">
    <div class="title">
        <div class="left">
            Your's Payment Request History Datas List</div>
        <div class="right" title="Close">
            X</div>
    </div>
    <div id="condition">
        <div>
           <%-- <div class="query_left_div">
                Vendor Code：<input type="text" class="VendorCode" value="" />
                Vendor Name：<input type="text" class="VendorName" value="" />
            </div>--%>
            <div class="query_left_div">
                WorkFlowNumber：<input type="text" class="WorkFlowNumber" value="" />
                ContractNumber：<input type="text" class="ContractNumber" value="" /><a class="QueryInfo">Query</a>
            </div>
        </div>
    </div>
    <div class="content"><div class="loading">
            <img src="../../../CAResources/themeCA/images/loading.gif" alt="" align="absmiddle" />　Loading......
        </div>

        <ul class="content_ul">
            <li class="WorkFlowNumber">WorkFlowNumber</li>
            <li class="VendorName">Vendor Name</li>
            <li class="ContractNumber">Contract Number</li>
            <li class="PaymentType">PaymentType</li>
            <li class="View">View</li>
            <li class="Copy">Copy</li>
        </ul>
        <%--<ul class="list_ul">
            <li class="WorkFlowNumber">PR00000001_1</li>
            <li class="VendorName">优比(中国)有限公司上海分公司</li>
            <li class="ContractNumber">HO10091008</li>
            <li class="View"><a class="ViewDO">View</a></li>
            <li class="Copy"><a class="CopyDO">Copy</a></li>
        </ul>
        --%>
        <div class="paging">
            <span class="up">
                <img src="../../../CAResources/themeCA/images/prup.gif" alt="" align="absmiddle" /></span>
            <span class="down">
                <img src="../../../CAResources/themeCA/images/prdown1.gif" alt="" align="absmiddle" /></span>
        </div>
    </div>
    <div class="error">
        No Any Records!
    </div>
</div>
<input type="hidden" value="" id="hfAddress" />
<asp:HiddenField ID="hfApplicant" runat="server" Value="" />
<script type="text/javascript" src="jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        ShowTwoStep();
        BindCopyEvent();
        BindMouseEvent();
        BindQuery();
    });

    function ShowTwoStep() {
        var $OneStep = $("#StepTable tr.OneStep");
        var $TwoStep = $("#StepTable tr.TwoStep");
        $OneStep.find("img").click(function () {
            $TwoStep.show();
            $OneStep.hide();
            $("#hfAddress").val($(this).attr("mytitle"));
        });
    }

    function BindCopyEvent() {
        var $NoNeedCopy = $("#StepTable tr.TwoStep img.NoNeedCopy");
        $NoNeedCopy.click(function () {
            var rootURL = document.location.protocol + "//" + document.location.host;
            var url = rootURL + $("#hfAddress").val();
            document.location = url;
        });
        var $NeedCopy = $("#StepTable tr.TwoStep img.NeedCopy");
        $NeedCopy.click(function () {
            $("div.table").hide();
            $("div.QueryPRHistory").show();
            $(".error").hide();
            $(".content").find("ul").hide();
            $(".paging").hide();
        });
        var $right = $("div.right");
        $right.click(function () {
            $("div.table").show();
            $("div.QueryPRHistory").hide();
        });
    }

    function BindMouseEvent() {
        var $list_ul = $("div.QueryPRHistory ul.list_ul");
        $list_ul.live("mousemove", function () {
            $(this).find("li").css("backgroundColor", "#f2f2f2");
        });
        $list_ul.live("mouseout", function () {
            $(this).find("li").css("backgroundColor", "white");
        });
    }

</script>
<script type="text/javascript">
    var pageIndex = 0;
    var itemArray = new Array();
    var allArray = new Array();
    function BindQuery() {
        BindQueryAjaxEvent();
        $("span.up").live("click", function () {
            if (pageIndex - 1 < 0) {
                pageIndex = 0;
            } else {
                pageIndex = pageIndex - 1;
            }
            PagingQuery(allArray);
        });
        $("span.down").live("click", function () {
            if (pageIndex + 1 >= allArray.length) {
                pageIndex = pageIndex;
            } else {
                pageIndex = pageIndex + 1;
            }
            PagingQuery(allArray);
        });
        var $QueryInfo = $("a.QueryInfo");
        $QueryInfo.click(function () {
            QueryPRInfo();
        });
    }

    function QueryPRInfo() {
        var paymentRequestType = $("#hfAddress").val();
        if (paymentRequestType.indexOf("Opex") != -1) {
            paymentRequestType = "Opex";
        } else {
            paymentRequestType = "Capex";
        } 
        pageIndex = 0;
        itemArray = new Array();
        allArray = new Array();
        var $hfApplicant = $("input[id$='hfApplicant']");
        //$hfApplicant.val("lj3 (VANCECA\\lj3)");
        var rootURL = document.location.protocol + "//" + document.location.host;
        var r_url = rootURL + "/WorkFlowCenter/_layouts/ca/workflows/PaymentRequest" +
                              "/PRInfo.aspx?Applicant=" + encodeURI($hfApplicant.val()) + "" +
        //"&VendorCode=" +$.trim($("input.VendorCode").val()) + ""+ //"&VendorName=" +$.trim($("input.VendorName").val()) + ""+
                              "&WorkFlowNumber=" + $.trim(encodeURI($("input.WorkFlowNumber").val())) + "" +
                              "&PaymentRequestType=" + paymentRequestType + "" +
                              "&ContractNumber=" + $.trim(encodeURI($("input.ContractNumber").val()));
        $.ajax({
            type: "POST",
            url: r_url,
            dataType: "json",
            timeout: 300000,
            success: function (data) {
                if (data == "") {
                    $(".content").hide();
                    $(".error").show();
                } else {
                    BindPRInfoList(data);
                }
            },
            error: function (msg) {
                $(".content").hide();
                $(".error").show();
            }
        });
    }

    function BindQueryAjaxEvent() {
        var $content = $(".content");
        var $loading = $("div.loading");
        var $QueryInfo = $("a.QueryInfo");
        $content.ajaxStart(function () {
            $(".error").hide();
            $content.find("ul").hide();
            $(".paging").hide();
            $QueryInfo.hide();
            $loading.show();
        });
        $content.ajaxStop(function () {
            $loading.hide();
            $QueryInfo.show();
            $content.find("ul").show();
        });
    }

    function BindPRInfoList(data) {
        var $content = $(".content");
        if (data != null) {
            try {
                $("ul.list_ul").remove();
                $.each(data, function (i, item) {
                   if (item != undefined) {
                        if (itemArray.length < 10) {
                            itemArray.push(item);
                        }
                        else {
                            allArray.push(itemArray);
                            itemArray = new Array();
                            itemArray.push(item);
                        }
                    }
                });
                allArray.push(itemArray);
                PagingQuery(allArray);
                if (allArray.length > 1) {
                    $("div.paging").show();
                } else {
                    $("div.paging").hide();
                }
                $content.show();
            }
            catch (e) {
            }
        } else {
            $content.hide();
        }
    }

    function PagingQuery(allItemArray) {
        var current = allItemArray[pageIndex];
        $("ul.list_ul").remove();
        var $content_ul = $(".content ul.content_ul");
        $.each(current, function (i, item) {
            if (item != undefined) {
                var $html = $(AppendVendorUL(item.WorkFlowNumber, item.VendorName, item.ContractNumber, item.ID, item.RequestType));
                $content_ul.after($html);
            }
        });
    }

    function AppendVendorUL(WorkFlowNumber, VendorName, ContractNumber, ID, RequestType) {
        var PaymentType = RequestType;
        if (PaymentType != "Opex") {
            PaymentType = "Asset";
        } else {
            PaymentType = "Expense";
        }
        var appendHtml = "";
        var vn = VendorName;
        var cn = ContractNumber;
        var rootURL = document.location.protocol + "//" + document.location.host;
        var url = rootURL + "/WorkFlowCenter/Lists/PaymentRequestItems/DispForm.aspx?ID=" + ID;
        var pageURL = rootURL + "/WorkFlowCenter/lists/PaymentRequestItems/NewForm.aspx?IsFromIco=1&RequestType=" + RequestType + "&WorkFlowNumber=" + WorkFlowNumber;
        //var pageURL = rootURL + "/WorkFlowCenter/lists/PaymentRequestItems/NewForm.aspx?WorkFlowNumber=" + WorkFlowNumber;
        VendorName = VendorName.length > 8 ? VendorName.substring(0, 8) + "..." : VendorName;
        ContractNumber = ContractNumber.length > 8 ? ContractNumber.substring(0, 8) + "..." : ContractNumber;
        appendHtml += "<ul class=\"list_ul\">";
        appendHtml += "<li class=\"WorkFlowNumber\">" + WorkFlowNumber + "</li>";
        appendHtml += "<li class=\"VendorName\" title=\"" + vn + "\">" + VendorName + "</li>";
        appendHtml += "<li class=\"ContractNumber\" title=\"" + cn + "\">" + ContractNumber + "</li>";
        appendHtml += "<li class=\"PaymentType\" >" + PaymentType + "</li>";
        appendHtml += "<li class=\"View\"><a class=\"ViewDO\" target=\"_blank\" href=\"" + url + "\" title=\"View\">View</a></li>";
        appendHtml += "<li class=\"Copy\"><a class=\"CopyDO\" href=\"" + pageURL + "\" title=\"Copy\">Copy</a></li>";
        appendHtml += "</ul>";
        return appendHtml;
    }
   
</script>
