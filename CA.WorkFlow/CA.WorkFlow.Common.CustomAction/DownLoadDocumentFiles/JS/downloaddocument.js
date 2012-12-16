var second = 0; //计录秒数
var secondsInterval; //秒数计数器
var url = "/_LAYOUTS/CA/DownloadFiles/DownloadZipRequest.aspx"; //Ajax请求页面
var request;
function CheckFiles() {
    second = 0;
    var result = GetCheckCount();
    if (result == "") {
        return;
    }
    ShowForbid();
    var date = new Date;
    var parServerUrl = GetServerURL();
    if (request != null) {
        request.abort();
    }

    $.get(url, { FilesID: result, Datetime: date, serverUrl: parServerUrl, ActionType: 'GetTotalSize' }, function (callBackResult) {
        if (callBackResult == "1") {
            if (confirm("所选文件太大！确定下载会有长时间的等待，确定要继续吗？")) {
                DownLoad(url, result, parServerUrl);
            }
            else {
                hideForbid();
                clearInterval(secondsInterval);
            }
        }
        else {
            DownLoad(url, result, parServerUrl);
        }
    });


}
function DownLoad(url, result, parServerUrl) {

    var date = new Date;
    request = $.get(url, { FilesID: result, Datetime: date, serverUrl: parServerUrl }, function (callBackResult) {
        if (callBackResult == "0") {
            alert("文件名异常，请联系管理员！");
        }
        else {
            GetFile(callBackResult);
        }
        hideForbid();
    });

}

//计算所选中的item的个数
function GetCheckCount() {
    var result = "";
    $("input[LItemId]").each(function () {
        if ($(this).attr("checked") == true) {
            if (result.length > 0) {
                result += "-";
            }
            result += $(this).attr("LItemId");
        }
    });
    return result;
}

///得到item的ServerURl
function GetServerURL() {
    var serverurl;
    serverurl = $("input[ServerURl]").attr("ServerURl");
    return serverurl;

}

//进度条动画
function setanimate() {
    $("#animateBG").width(0);
    $("#animateBG").animate({ width: 138 }, "slow");
}
//显示弹出层
function ShowForbid() {
    CreatDIV();
    // var setanimate = setInterval("setanimate()", 2000);
    /// secondsInterval = setInterval("getTotalSeconds()", 1000);计算
    Cancel();
    $(".Forbid").css("opacity", 0.3);
    $(".Forbid").fadeIn();
    var width = $("body").width();
    var height = document.body.scrollHeight; //$("html").height(); //document.documentElement.clientHeight; // $("body").height()
    $(".Forbid").width("100%");
    $(".Forbid").height(height);
    var left = (parseInt(document.body.scrollWidth) - 100) / 2;
    var top = (parseInt(document.body.scrollHeight) - 100) / 2;
    $("#Waite").css("left", left);
    $("#Waite").css("top", top);
    $("#Waite").fadeIn();
}
//取消弹出层
function hideForbid() {
    clearInterval(secondsInterval);
    $(".Forbid").remove(); //.hide();
    $("#Waite").remove(); //.hide();
}
//计算时间
function getTotalSeconds() {
    second++;
    $("#TiemSeconds").text(second)
}
//下载资料
function GetFile(sFileName) {
    //alert(sFileName);
    window.location.href(sFileName);
}

//取消的事件和样式
function Cancel() {
    $('.cancel').click(function () {
        if (confirm("确定要取消下载么?")) {
            hideForbid();
            if (null != request) {
                request.abort();
            }
        }
    });

    $('.cancel').hover(function () {
        $(this).css('background-color', '#9b9999');
    }, function () {
        $(this).css('background-color', '#CCCCCC');
    });
}

//生成弹出层
function CreatDIV() {

    var div = "<div class='Forbid'></div>"; // style = 'background-color:Black;z-index:2;position:absolute;display:none;'
    div += "<table id='Waite' bgcolor='#f1f1f1'>"; // style='width:140px;height:130px;z-index:5;position:absolute;display:none;'loading.gif
    div += "<tr>";
    div += "<td align='left'><img src='/_LAYOUTS/CAResources/themeCA/images/loading.gif' alt='加载中。。。' /></td>";
    //div += "<td align='left'><div class='StateBorder'><div id='animateBG' ></div></div></td>"; // style='border: thin solid #333333; width:140px;height:20px;'      style='width:100px;background-color:Blue;display:inline;height:20px;'
    div += "</tr>";
    /*div += "<tr>";
    div += "<td align='center'>正在处理，请稍侯!</td>";
    div += "</tr>";*/
    div += "<tr>";
    //div += "<td align='center'><span id='TiemSeconds'></span> S</td>";
    div += "<td align='center'><div class='cancel'>取     消</a></td>";
    div += "</tr>";
    div += "</table>";
    $("form").prepend(div);
}