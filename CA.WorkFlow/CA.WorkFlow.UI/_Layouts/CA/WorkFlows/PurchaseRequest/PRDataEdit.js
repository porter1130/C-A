    var CurrentPageIndex = 1; //当前页索引 by xu 
    var PageSize = 10; //每页显示条数 by xu 
    var PreId = ""; //选中行的控件的前缀ID by xu 

    var JSPurchaseRequest = {}; //命名空间

    //获取Common Pre Id
    JSPurchaseRequest.GetPreId = function (tmpId) {
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_rptItem_ctl00_ddlItemCode
        return tmpId.substring(0, tmpId.lastIndexOf('_') + 1);
    }
    function SetBorderWarn($obj) {
        $obj.css('border', '1px solid red');
    }

    function ClearBorderWarn($obj) {
        $obj.css('border', '');
        $obj.css('border-bottom', '#999 1px solid');
    }

    //点击提交事保存时，进行本地合法性检查。如是拒绝，则必须输入Comment
    function beforeSubmit(sender) {
        CreateForbidDIV();
        var result = false;
        if (sender.value === 'Submit' || sender.value === 'Confirm') {
            result = Validate();
            if (!result) {
                if (jQuery.browser.msie) {
                    event.cancelBubble = true;
                }
                ClearForbidDIV();
            }
        } else if (sender.value === 'Save') {
            result = ValidateForSave();
            if (!result) {
                if (jQuery.browser.msie) {
                    event.cancelBubble = true;
                }
                ClearForbidDIV();
            }
        } else if (sender.value === 'Reject') {
            result = !ca.util.emptyString($('#comment-task textarea').val());
            if (!result) {
                if (jQuery.browser.msie) {
                    event.cancelBubble = true;
                }
                alert('Please fill in the Reject Comments.');
                ClearForbidDIV();
                return false;
            }
        }

        var isExistSameItem = CheckSameItemCode(); //验证的costcenter下只能选择一次Itemcode
        if (isExistSameItem) {
            ClearForbidDIV();
            return false;
        }

        //验证购买数量不能为零
        if (result) {
            var checkZero = CheckRequestIsZero();
            if (!checkZero) {
                alert('Purchase QTY (购买数量) inavailable!');
                ClearForbidDIV();
                return false;
            }
        }

        //验证数据中是否有ItemType为空的数据。
        if (result) {
            var emptyItemtype = IsEmptyItemType();
            if (emptyItemtype) {
                ClearForbidDIV();
                return false;
            }
        }
        
        if (sender.value == "Submit") {
            ///验证是纸袋是的话如果有数量超过50会计算总量，提示用户是否保存。并验证包装规则数量是否合法。
            var checkPaperBag = CheckPaperBageData(50);
            if (!checkPaperBag) {
                ClearForbidDIV();
                return false
            }
        }

        if (result) {//验证通过 生成弹出层，防止重复提交。
            CreateForbidDIV(); //单击生成弹出层，防止重复提交。
        }
        return result;
    }


    //保存事件时执行合法性检查
    function ValidateForSave() { //检查repeat控件中数量输入框为数字且非空
        var error = '';

        error += ValidateNumber();

        if (error) {
            alert(error);
        }

        return error.length === 0;
    }

    function ValidateNumber() {
        var error = '';

        var flag = 0;
        //Class以Quantity结尾的输入框中要求为数字
        $('#ca_pr_details td[class$=\'Quantity\']').find('input').each(function () {
            if (!$(this).is(":hidden")) {///对可见的行进行验证
                if (ca.util.emptyString($(this).val()) || $(this).val().length === 0 || isNaN($(this).val())) {
                    flag++;
                    SetBorderWarn($(this));
                } else {
                    ClearBorderWarn($(this));
                }
            }
        });
        error += flag > 0 ? 'Please fill the valid quantity.\n' : '';

        flag = 0;
        //Class是UnitPrice的输入框中要求为数字
        $('#ca_pr_details td.UnitPrice').find('input').each(function () {
            if (!$(this).is(":hidden")) {///对可见的行进行验证
                if (ca.util.emptyString($(this).val()) || $(this).val().length === 0 || isNaN($(this).val())) {
                    flag++;
                    SetBorderWarn($(this));
                } else {
                    ClearBorderWarn($(this));
                }
            }
        });
        error += flag > 0 ? 'Please fill the valid unit price.\n' : '';

        flag = 0;
        //Class是ExchangeRate的输入框中要求为数字
        $('#ca_pr_details td.ExchangeRate').find('input').each(function () {
            if (!$(this).is(":hidden")) {///对可见的行进行验证

                if (ca.util.emptyString($(this).val()) || $(this).val().length === 0 || isNaN($(this).val())) {
                    flag++;
                    SetBorderWarn($(this));
                } else {
                    ClearBorderWarn($(this));
                }
            }
        });
        error += flag > 0 ? 'Please fill the valid exchange rate.\n' : '';

        flag = 0;
        //Class是CostCenter的下拉框中必须选中非空项
        $('#ca_pr_details td.CostCenter').find('select').each(function () {
            if (!$(this).is(":hidden")) {///对可见的行进行验证
                var id = $(this).prev("input").val();
                var text = $(this).next("input").val();
                if ($(this).val() == '-1' || jQuery.trim(id) == "" || jQuery.trim(text) == "") {
                    flag++;
                    SetBorderWarn($(this).parent("td"));
                } else {
                    ClearBorderWarn($(this).parent("td"));
                }
            }
        });
        error += flag > 0 ? 'Please select cost center for item.\n' : '';

        flag = 0;
        //验证每行中Item Code必须有值
        $('#ca_pr_details td.ItemCode').find('input').each(function () {
            if (!$(this).is(":hidden")) {///对可见的行进行验证
                if (ca.util.emptyString($(this).val())) {
                    flag++;
                    SetBorderWarn($(this));
                } else {
                    ClearBorderWarn($(this));
                }
            }
        });
        error += flag > 0 ? 'Please select item code first.\n' : '';

        flag = 0;
        var maintenace_count = 0;
        var non_maintenace_count = 0;
        //if ($('#<%= this.hidDisplayMode.ClientID %>').val() != 'Display') {
        var displayMode = DisplayMod();
        if (displayMode != 'Display') {
            $('#ca_pr_details td.ItemType').find('span').each(function () {
                if (!$(this).is(":hidden")) {
                    if ($(this).text() === 'Maintenance') {
                        maintenace_count++;
                    } else {
                        non_maintenace_count++;
                    }
                }
            });
            error += (maintenace_count != 0 && non_maintenace_count != 0) ? 'The request contains the items with maintenance and non-maintenance type.\n' : '';
        }

        //如果前面验证均通过，则下段代码将进行是否有小数位的验证。其中是否有小数位则由Item Code在基础表中进行维护。若表中值为0则表示不允许为小数，反之亦然
        var $tmp_obj = null;
        var is_decimal;
        if (error.length === 0) {
            flag = 0;
            $('#ca_pr_details td[class$=\'Quantity\']').each(function () {
                if (!$(this).is(":hidden")) {///对可见的行进行验证
                    $tmp_obj = $(this).find('input').eq(0);
                    is_decimal = $(this).siblings('td.IsAccpetDecimal').children('input').eq(0).val();
                    if (is_decimal === '0') {
                        if ($tmp_obj.val().indexOf('.') != -1) {
                            flag++;
                            SetBorderWarn($tmp_obj);
                        } else {
                            ClearBorderWarn($tmp_obj);
                        }
                    }
                }
            });
            error += flag > 0 ? 'The system can not accept decimal for this item. Please fill the valid number.' : '';
        }

        return error;
    }


    //供Change事件调用
    function UpdateTotal(sender) {
        JSPurchaseRequest.pre_id = JSPurchaseRequest.GetPreId($(sender).attr('id'));


        JSPurchaseRequest.total_quantity = $('#' + JSPurchaseRequest.pre_id + 'txtTotalQuantity').val();
        JSPurchaseRequest.trans_quantity = $('#' + JSPurchaseRequest.pre_id + 'txtTransQuantity').val();
        JSPurchaseRequest.unit_price = $('#' + JSPurchaseRequest.pre_id + 'txtUnitPrice').val();
        JSPurchaseRequest.exchange_rate = $('#' + JSPurchaseRequest.pre_id + 'txtExchangeRate').val();

        //验证订购量，调拨量，单位和汇率是否合法
        if (isNaN(JSPurchaseRequest.total_quantity)
            || isNaN(JSPurchaseRequest.trans_quantity)
            || isNaN(JSPurchaseRequest.unit_price)
            || isNaN(JSPurchaseRequest.exchange_rate)) {
            alert('Please fill the valid number.');
            $('#' + JSPurchaseRequest.pre_id + 'lbTotalPrice').text(0); //非法输入时设置当前行总价为0
            return false;
        }

        var $ic = $('#' + JSPurchaseRequest.pre_id + 'hidItemCode');
        if ($ic.val().length === 0) {
            return false;
        }

        //验证是否允许输入小数位
        JSPurchaseRequest.is_accept_decimal = $('#' + JSPurchaseRequest.pre_id + 'hdIsAccpetDecimal').val();
        if (!ca.util.emptyString(JSPurchaseRequest.is_accept_decimal) && JSPurchaseRequest.is_accept_decimal === '0') {
            if (JSPurchaseRequest.total_quantity.indexOf('.') != -1
                || JSPurchaseRequest.trans_quantity.indexOf('.') != -1) {
                alert('The system can not accept decimal for this item. Please fill the valid number.');
                $('#' + JSPurchaseRequest.pre_id + 'lbTotalPrice').text(0); //非法输入时设置当前行总价为0
                return false;
            }
        }

        JSPurchaseRequest.itemCode = $('#' + JSPurchaseRequest.pre_id + 'hidItemCode').val();
        if (JSPurchaseRequest.itemCode.indexOf('X') === 0) {
            $('#' + JSPurchaseRequest.pre_Id + 'lbTransQuantity').show(); //X开头的服务费不能有调拨费用，默认为0
            $('#' + JSPurchaseRequest.pre_id + 'txtTransQuantity').val(0).hide();
            $('#' + JSPurchaseRequest.pre_id + 'txtTransQuantity').hide();
            JSPurchaseRequest.trans_quantity = '0';
        }

        //自动去除数字前面的0和前后的空格
        JSPurchaseRequest.total_quantity = AutoCorrectData(JSPurchaseRequest.total_quantity);
        JSPurchaseRequest.trans_quantity = AutoCorrectData(JSPurchaseRequest.trans_quantity);
        JSPurchaseRequest.unit_price = AutoCorrectData(JSPurchaseRequest.unit_price);
        JSPurchaseRequest.exchange_rate = AutoCorrectData(JSPurchaseRequest.exchange_rate);
        $('#' + JSPurchaseRequest.pre_id + 'txtTotalQuantity').val(JSPurchaseRequest.total_quantity);
        $('#' + JSPurchaseRequest.pre_id + 'txtTransQuantity').val(JSPurchaseRequest.trans_quantity);
        $('#' + JSPurchaseRequest.pre_id + 'txtUnitPrice').val(JSPurchaseRequest.unit_price);
        $('#' + JSPurchaseRequest.pre_id + 'txtExchangeRate').val(JSPurchaseRequest.exchange_rate);

        //当数据不完整时，当前行总价置为0，同时不再继续计算总价
        if (ca.util.emptyString(JSPurchaseRequest.total_quantity)
            || ca.util.emptyString(JSPurchaseRequest.trans_quantity)
            || ca.util.emptyString(JSPurchaseRequest.unit_price)
            || ca.util.emptyString(JSPurchaseRequest.exchange_rate)) {
            $('#' + JSPurchaseRequest.pre_id + 'lbTotalPrice').text(0); //输入不完整时设置当前行总价为0
            return false;
        }



        //计算需购买总数
        JSPurchaseRequest.request_quantity = parseFloat(JSPurchaseRequest.total_quantity) - parseFloat(JSPurchaseRequest.trans_quantity);
        $('#' + JSPurchaseRequest.pre_id + 'lbRequestQuantity').text(JSPurchaseRequest.request_quantity);
        $('#' + JSPurchaseRequest.pre_id + 'hidRequestQuantity').val(JSPurchaseRequest.request_quantity);

        //计算当前行总价，并进行千分号显示
        JSPurchaseRequest.total_price = parseFloat(JSPurchaseRequest.total_quantity) * parseFloat(JSPurchaseRequest.unit_price);
        $('#' + JSPurchaseRequest.pre_id + 'lbTotalPrice').text(commafy(JSPurchaseRequest.total_price));

        CalcTotal(); //更新当前PR总价
    }

    //设置RequestType显示样式，若选择Capex，则显示Capex二级选项
    function SetRequestType(requestType) {
        if (requestType === 'Capex') {
            $('#capex_type').removeClass('hidden');
        } else {
            $('#capex_type').addClass('hidden');
        }
    }

    //服务器返回后执行函数
    function EndRequestHandler() {
        HideEmptyTR();
        CheckPermission();
        CalcTotal();
        DisableEnterKey();
        SetAutoWidthForDDL();
        SetXTypeStyle();
        SetReturnStyle(); //设置退货状态下页面显示样式
        SetClickEventForDDL(); //回传中防止click事件消失
        ShowFistHide(); /////显示第一个隐藏的Item行
        //ClearForbidDIV();
        $("#LoadMoreItem").hide();
    }
    //设置单价样式，若ITEM CODE为X开头为Service，则单价可修改。另外纸袋开头为E0000，也需要价格可修改。隐藏Label控件，显示编辑控件
    function SetXTypeStyle() {
        JSPurchaseRequest.preId = '';
        $('#ca_pr_details td.UnitPrice').find('input').each(function () {
            if (!$(this).is(":hidden")) {
                JSPurchaseRequest.preId = JSPurchaseRequest.GetPreId($(this).attr('id'));

                JSPurchaseRequest.itemCode = $('#' + JSPurchaseRequest.preId + 'hidItemCode').val();
                if (ca.util.emptyString(JSPurchaseRequest.itemCode)) {
                    return true; //ItemCode未选择，为空
                }

                if (JSPurchaseRequest.itemCode.indexOf('X') === 0) {
                    $('#' + JSPurchaseRequest.preId + 'lbTransQuantity').show(); //X开头的服务费不能有调拨费用，默认为0
                    $('#' + JSPurchaseRequest.preId + 'txtTransQuantity').val(0);
                    $('#' + JSPurchaseRequest.preId + 'txtTransQuantity').hide();

                    $(this).prev('span').addClass('hidden');
                    $(this).removeClass('hidden');
                } else if (JSPurchaseRequest.itemCode.indexOf('E0000') === 0) {
                    $(this).prev('span').addClass('hidden');
                    $(this).removeClass('hidden');
                }
            }
        });
    }

    //格式化日期
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

    //对数字进行2位小数的Round，同时使用千分号格式化
    function commafy(num) {
        num = Math.round(num * 100) / 100;
        num = num + '';
        var tmpArr = num.split('.');
        var re = /(-?\d+)(\d{3})/
        while (re.test(tmpArr[0])) {
            tmpArr[0] = tmpArr[0].replace(re, "$1,$2")
        }
        return tmpArr.length >= 2 ? tmpArr[0] + '.' + tmpArr[1] : tmpArr[0];
    }

    ///验证购买数量不能为零
    function CheckRequestIsZero() {
        var result = true;
        $('#ca_pr_details td.RequestQuantity').each(function () {
            if (!$(this).is(":hidden")) {//只对可见的进行验证
                if ($(this).children("input").val() == 0) {
                    $(this).removeClass("label");
                    $(this).css('border', '1px solid red');
                    result = false;
                }
                else {
                    $(this).css('border', '1px solid #cccccc');
                    $(this).addClass("label");
                }
            }
        });
        return result;
    }

    //设置页面中下拉列表控件的显示样式
    function SetAutoWidthForDDL() {
        $('#ca_pr_details select.width-fix')
            .bind('focus mouseover', function () { $(this).addClass('expand').removeClass('clicked'); })
            .bind('click', function () { if ($(this).hasClass('clicked')) { $(this).blur(); } $(this).toggleClass('clicked'); })
            .bind('mouseout', function () { if (!$(this).hasClass('clicked')) { $(this).removeClass('expand'); } })
            .bind('blur', function () { $(this).removeClass('expand clicked'); });
        //.change(function () { SetCostCenterVal($(this)); });
        //alert("执行了SetAutoWidthForDDL");
    }

    //重新设置下拉列表点击事件
    function SetClickEventForDDL() {
        $('#ca_pr_details select.width-fix')
            .unbind('click')
            .bind('click', function () { if ($(this).hasClass('clicked')) { $(this).blur(); } $(this).toggleClass('clicked'); })
    }

    //禁用页面Enter事件，防止repeat控件自动添加新行
    function DisableEnterKey() {
        $('#ca_pr_details' + ' input').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('#ca_pr_details').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('div.main-body').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
    }

    //动态增加Style样式
    function addCssByStyle(cssString) {
        var doc = document;
        var style = doc.createElement("style");
        style.setAttribute("type", "text/css");

        if (style.styleSheet) {// IE
            style.styleSheet.cssText = cssString;
        } else {// w3c	        
            var cssText = doc.createTextNode(cssString);
            style.appendChild(cssText);
        }

        var heads = doc.getElementsByTagName("head");
        if (heads.length)
            heads[0].appendChild(style);
        else
            doc.documentElement.appendChild(style);
    }
    /****************Begin 原三个<script>里的js**********************************************************************************************/

    function AutoCorrectData(num) {
        var n = ca.util.trim(num);
        if (n.length === 0) {
            return n; //空
        }
        return n * 1 + '';
    }

    /*
    *string:原始字符串
    *substr:子字符串
    *isIgnoreCase:忽略大小写
    */
    function contains(string, substr, isIgnoreCase) {
        if (isIgnoreCase) {
            string = string.toLowerCase();
            substr = substr.toLowerCase();
        }
        var startChar = substr.substring(0, 1);
        var strLen = substr.length;
        for (var j = 0; j < string.length - strLen + 1; j++) {
            if (string.charAt(j) == startChar)//如果匹配起始字符,开始查找
            {
                if (string.substring(j, j + strLen) == substr)//如果从j开始的字符与str匹配，那ok
                {
                    return true;
                }
            }
        }
        return false;
    }

    //动态增加Style样式
    function addCssByStyle(cssString) {
        var doc = document;
        var style = doc.createElement("style");
        style.setAttribute("type", "text/css");

        if (style.styleSheet) {// IE
            style.styleSheet.cssText = cssString;
        } else {// w3c	        
            var cssText = doc.createTextNode(cssString);
            style.appendChild(cssText);
        }

        var heads = doc.getElementsByTagName("head");
        if (heads.length)
            heads[0].appendChild(style);
        else
            doc.documentElement.appendChild(style);
    }
    /*******************************by xu************************************************************/

    function CheckSameItemCode() {
        var result = false;
        var arrList = new Array();
        var i = 0;
        $(".dr-row").each(function () {
            if (!$(this).is(":hidden")) {//只对可见的进行验证
                var itemCode_td = $(this).children("td").eq(2);
                var costCenter_td = $(this).children("td[class='label ddl CostCenter']");

                var itemCode = itemCode_td.children("span").text();
                if (!jQuery.trim(itemCode)) {
                    var CostCenter = costCenter_td.children("select").children("option:selected").text();
                    var item = itemCode + "_" + CostCenter;
                    if (ArrayExist(arrList, item)) {///数据中是否存在相同的itemCode和costcenter
                        result = true;
                        return result;
                    }
                    else {
                        arrList[i] = item;
                        i++;
                    }
                }
            }
        });
        return result;
    }

    //数据中是否存在相同的itemCode和costcenter
    function ArrayExist(arr, item) {
        var result = false;
        if (arr.length == 0) {
            return result;
        }
        var message = "";
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == item) {
                var arrItem = item.split("_")
                message += arrItem[1] + "  has already selected  " + arrItem[0];
                result = true;
            }
        }
        if (message != "") {
            alert(message);
        }
        return result;
    }

    ///验证是纸袋是的话如果有数量超过50会计算总量，提示用户是否保存。并验证包装规则数量是否合法。
    function CheckPaperBageData(count) {
        var result = true;
        var isCheckCount = true; //是否要验证数字
        var reg = /^\d+/;
        var errorFormate = "";
        var countMessage = "";

        $("#ca_pr_details").find(".dr-row").each(function () {
            if (!$(this).is(":hidden")) {//只对可见的进行验证
                var packagedRegulation = $(this).children("td").eq(3).children("input[id*='HFPackagedRegulation']").val(); //.children(".HFPackagedRegulation");
                if (packagedRegulation.length > 1) {//是纸袋数据
                    var itemCode = $(this).children("td").eq(2).text();
                    var itemCount = $(this).children("td").eq(4).children("input").val();
                    var packagedQuantity = packagedRegulation.match(reg); ///取得包装规则里的单位里的数量如：100只/箱  里的100
                    var packagedUnit = packagedRegulation.split("/")[0].replace(reg, ""); //取得包装规则里单位里如：100只/箱  里的只
                    if (null == packagedQuantity || packagedRegulation.indexOf("/") <= 2) {//纸袋包装规则 格式不正确
                        errorFormate += "Error packagedRegulation formate for " + itemCode + "\n";
                        isCheckCount = false;
                    }
                    else {
                        if (itemCount > count) {
                            var totalQuantity = packagedQuantity * itemCount;
                            countMessage += totalQuantity + " " + packagedUnit + " " + itemCode + "\n";
                            // largeCount = true;
                        }
                    }
                }
            }
        });

        if ((errorFormate + countMessage) != "") {
            if (isCheckCount) {
                if (confirm("Are you sure want to submit：\n " + countMessage)) {
                    result = true;
                }
                else {
                    result = false;
                }
            }
            else {
                alert(errorFormate);
                result = false;
            }
        }
        return result;
    }

    ///验证数据中是否用ItemType为空的数据。
    function IsEmptyItemType() {
        var isEmptItemType = false;
        $("#ca_pr_details td.ItemType").each(function () {
            if (!$(this).is(":hidden")) {//只对可见的进行验证
                var spanItemtype = jQuery.trim($(this).children("span").text()); //.replace(/(^\s*)|(\s*$)/g, ""); //.Trim(); //.replace(/(^\s*)|(\s*$)/g, "");
                var inputItemtype = jQuery.trim($(this).children("input").val()); //.replace(/(^\s*)|(\s*$)/g, ""); //.Trim()(); //.replace(/\s+/g, ""); //.replace(/(^\s*)|(\s*$)/g, "");
                if (spanItemtype == "" || inputItemtype == "") {
                    SetBorderWarn($(this));
                    isEmptItemType = true;
                }
                else {
                    ClearBorderWarn($(this));
                    //$(this).css("border-right","
                    $(this).css('border', '1px solid #cccccc');
                }
            }
        });
        if (isEmptItemType) {
            alert("ItemType can not be  empty!Please check data");
        }
        return isEmptItemType
    }
    /****************End 原三个<script>里的js**********************************************************************************************/


    /****************Begin ItemCode查询全用js去处理*******************************************************************************************************/
    function ItemCodeQuery() {
        $('#SelectedItem').dialog({
            modal: true,
            autoOpen: false,
            width: 750,
            //height:550,
            buttons: {
                "Ok": function () {
                    SetSelectedVal();
                },
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });
        ///绑定查询ItemCode的查询事件。
        $("#buttonQuery").click(function () {
            CurrentPageIndex = 1;
            GetDataFromServer(CurrentPageIndex);
        });

        //点击上一页
        $("#Prev").click(function () {
            CurrentPageIndex = CurrentPageIndex - 1;
            GetDataFromServer(CurrentPageIndex);
        });
        //点击上下一页
        $("#Next").click(function () {
            CurrentPageIndex=CurrentPageIndex + 1;
            GetDataFromServer(CurrentPageIndex);
        });
    }
    //弹出层
    function OpenDialog(sender) { // by xu
        $('#SelectedItem').dialog('open');

        PreId = GetTRPreId($(sender).next('input').attr('id'));
        $(".each-row").live("click", function () {
            BindClickFun($(this));
        });
        $(".ms-alternating").live("click", function () {
            BindClickFun($(this));
        });
    }

    //读取从服务器端返回的json并将数据拼成html格式。
    function ReadJSON(jsonList) { // by xu
        var ItemHTML = "";
        for (var i in jsonList.ItemCode) {
            var TRHTML = "<tr>";
            if (i % 2 == 0) {
                TRHTML = "<tr class='each-row'>";
            }
            else {
                TRHTML = "<tr class='ms-alternating'>";
            }
            TRHTML += CreateTD("TDData", jsonList.ItemCode[i]["Title"]);
            TRHTML += CreateTD("TDData", jsonList.ItemCode[i]["ItemType"]);
            TRHTML += CreateTD("TDData", jsonList.ItemCode[i]["Description"]);
            TRHTML += CreateTD("TDData", jsonList.ItemCode[i]["Unit"]);
            TRHTML += CreateTD("TDData", jsonList.ItemCode[i]["AssetClass"]);
            TRHTML += CreateTD("hidden", jsonList.ItemCode[i]["VendorID"]);
            TRHTML += CreateTD("hidden", jsonList.ItemCode[i]["DeliveryPeriod"]);
            TRHTML += CreateTD("hidden", jsonList.ItemCode[i]["UnitPrice"]);
            TRHTML += CreateTD("hidden", jsonList.ItemCode[i]["TaxValue"]);
            TRHTML += CreateTD("hidden", jsonList.ItemCode[i]["IsAccpetDecimal"]);
            TRHTML += CreateTD("hidden", jsonList.ItemCode[i]["Currency"]);
            TRHTML += CreateTD("hidden", jsonList.ItemCode[i]["ItemScope"]);
            TRHTML += CreateTD("hidden", jsonList.ItemCode[i]["PackagedRegulation"]);
            TRHTML += "</tr>";
            ItemHTML += TRHTML;

        }
        ClearItemData();
        $(ItemHTML).insertAfter("#TRTitle");
        SetPrveNextStatus(jsonList.TotalCount[0]["Count"]);
        
        BindHoverFun($(".ms-alternating"));
        BindHoverFun($(".each-row"));
    }

    //构造td列的数值
    function CreateTD(tdClass, val) {
        return "<td class='" + tdClass + "'>" + val + "</td>";
    }
    //绑定hover事件。
    function BindHoverFun(obj) {
        obj.hover(function () {
            $(this).addClass("YellowTR");
        }, function () {
            $(this).removeClass("YellowTR");
        });
    }
    ///Item的选定事件
    function BindClickFun(obj) {
        $("#SelectedItem").find(".ms-alternating").removeClass("ItemSelected"); //ms-selectednav");
        $("#SelectedItem").find(".each-row").removeClass("ItemSelected");
        obj.addClass("ItemSelected");
    }

    //设置下一页和上一页的显示状态 
    function SetPrveNextStatus(totalCount) {
        if (CurrentPageIndex * PageSize < totalCount) {
            $("#Next").show();
        }
        else {
            $("#Next").hide();
        }

        if (CurrentPageIndex > 1) {
            $("#Prev").show();
        }
        else {
            $("#Prev").hide();
        }
       // alert("TotalCount:" + totalCount + "CurrentIndex:" + CurrentPageIndex);
    }
    //得到选中行的控件的前缀ID
    function GetTRPreId(tmpId) {
        return tmpId.substring(0, tmpId.lastIndexOf('_') + 1);
    }

    ///设置选定的行为一行新的信息
    function SetSelectedVal() {

        //没有选中行
        if ($("#SelectedItem").find(".ItemSelected").length == 0) {
            alert('Please select one record first.');
            return false;
        }

        var vendorID = $("#SelectedItem").find(".ItemSelected").eq(0).children('td').eq(5).text(); //选中的行的Vendor
        if (typeof (vendorID) == "undefined" || vendorID == "undefined"||jQuery.trim(vendorID) == "") {
            alert("Vendor is empty for current item");
            return false;
        }
        $.ajax({
            type: "Get",
            dataType: "json",
            url: "GetVendor.aspx",
            data: { VendorID: vendorID },
            beforeSend: function () {
                $("#loading").show();
            },
            success: function (jsonList) {
                if (jsonList) {
                    var jsonVendor = jsonList.Vendor;
                    var vendorName = jsonVendor[0]["Title"];
                    $('#' + PreId + 'lbVendor').text(vendorName);
                    $('#' + PreId + 'hidVendor').val(vendorName);
                    $('#' + PreId + 'hidVendorId').val(jsonVendor[0]["VendorId"]);
                    SetSelectdItemInfo();
                }
                else {
                    alert("Can not find Vendor for VendorId:" + vendorID);
                }
            },
            error: function (message) {
                alert("Loading vendor failed:" + message);
            },
            complete: function () {
                $("#loading").hide();
            }
        });

        return true;
    }
    //设置选定的行为一行新的Item的信息
    function SetSelectdItemInfo() {
        var selectTRTD = $("#SelectedItem").find(".ItemSelected").eq(0).children('td');  //选中的行的列的集合
        var selectedItem = new Array();  ///保存选中的行的Item信息
        for (var i = 0; i < selectTRTD.length; i++) {
            selectedItem.push(selectTRTD.eq(i).text());
        }
        var vendorID = selectedItem[5];

        var itemCode = selectedItem[0];
        $('#' + PreId + 'lbItemCode').text(itemCode);
        $('#' + PreId + 'hidItemCode').val(itemCode);
        $('#' + PreId + 'hlPhoto').attr('href', '/WorkFlowCenter/ItemPic/' + itemCode + '.jpg');

        $('#' + PreId + 'lbItemType').text(selectedItem[1]);
        $('#' + PreId + 'hidItemType').val(selectedItem[1]);

        $('#' + PreId + 'lbDesc').text(selectedItem[2]);
        $('#' + PreId + 'hidDesc').val(selectedItem[2]);

        $('#' + PreId + 'lbUnit').text(selectedItem[3]);
        $('#' + PreId + 'hidUnit').val(selectedItem[3]);

        $('#' + PreId + 'lbAssetClass').text(selectedItem[4]);
        $('#' + PreId + 'hidAssetClass').val(selectedItem[4]);

        $('#' + PreId + 'lbDeliveryPeriod').text(selectedItem[6]);
        $('#' + PreId + 'hidDeliveryPeriod').val(selectedItem[6]);

        var PackagedRegulation = selectedItem[12]; //包装规则
        $('#' + PreId + 'HFPackagedRegulation').val(PackagedRegulation);

        $('#' + PreId + 'lbUnitPrice').text(selectedItem[7]);
        $('#' + PreId + 'txtUnitPrice').val(selectedItem[7]);

        if (itemCode.indexOf('X') === 0) {
            $('#' + PreId + 'lbTransQuantity').show(); //X开头的服务费不能有调拨费用，默认为0
            $('#' + PreId + 'txtTransQuantity').val(0);
            $('#' + PreId + 'txtTransQuantity').hide();
            $('#' + PreId + 'lbUnitPrice').addClass('hidden'); //X服务费可以修改单价
            $('#' + PreId + 'txtUnitPrice').removeClass('hidden');
        } else if (itemCode.indexOf('E0000') === 0) {
            $('#' + PreId + 'lbTransQuantity').hide();
            $('#' + PreId + 'txtTransQuantity').show();
            $('#' + PreId + 'lbUnitPrice').addClass('hidden'); //纸袋可以修改单价
            $('#' + PreId + 'txtUnitPrice').removeClass('hidden');
        } else {
            $('#' + PreId + 'lbTransQuantity').hide();
            $('#' + PreId + 'txtTransQuantity').show();
            $('#' + PreId + 'lbUnitPrice').removeClass('hidden'); //无法修改单价
            $('#' + PreId + 'txtUnitPrice').addClass('hidden');
        }

        $('#' + PreId + 'lbTaxValue').text(selectedItem[8]);
        $('#' + PreId + 'hidTaxValue').val(selectedItem[8]);

        $('#' + PreId + 'hdIsAccpetDecimal').val(selectedItem[9]);

        $('#' + PreId + 'lbCurrency').text(selectedItem[10]);
        $('#' + PreId + 'hidCurrency').val(selectedItem[10]);

        UpdateTotal($('#' + PreId + 'txtUnitPrice'));
        $('#SelectedItem').dialog("close");
    }


    //清除老数据。
    function ClearItemData() {
        $("#SelectedItem").find(".ms-alternating").remove();
        $("#SelectedItem").find(".each-row").remove();
    }


    ///显示非空的数据行。
    function HideEmptyTR() {
        var iShowCount = 0;
        $("#ca_pr_details").find(".dr-row").each(function () {
            var itemCode = $(this).find("td[class='label ItemCode']").children("span").text();
            if (itemCode == "") {//隐藏
                $(this).hide();
            }
            else {//显示
                var costCenterTD = $(this).find("td[class='label ddl CostCenter']");
                iShowCount++;
                SetDDLSelectValue(costCenterTD.children("select"));
                SetCostCenterSelected(costCenterTD);
                $(this).show();
            }
        });
        if (iShowCount == 0) {//若都没有数据，则第一条显示。
            var firstRow = $("#ca_pr_details").find(".dr-row");
            firstRow.eq(0).show();
            SetDDLSelectValue(firstRow.find("select"));
        }
    }

    //显示一行
    function ShowNextTR() {
        var isExistTr = false; //是否存在隐藏的行
        var hiddenRows = $("#ca_pr_details").find(".dr-row:hidden");
        if (hiddenRows.length==0) {
            AddItem();
        }
        else {
            var firstRow= hiddenRows.eq(0)
            firstRow.show();
            SetDDLSelectValue(firstRow.find("select"));
            return false;
        }
    }
    //删除当前行。
    function DeletCurrentRow(obj) {
        var currentRow = $(obj).parent("td").parent("tr");
        DelteRow(currentRow);
        CalcTotal();
    }

    //删除行(隐藏)主方法体
    function DelteRow(currentRow) {
        currentRow.hide();
        currentRow.children("td:gt(1)").each(function () {
            if ($(this).hasClass("TransQuantity")) {
                $(this).children("span").text("0");
                $(this).children("input").val("0");
            }
            else if ($(this).hasClass("ExchangeRate")) {
                $(this).children("span").text("1");
                $(this).children("input").val("1");
            }
            else if ($(this).hasClass("TotalPrice")) {
                $(this).children("span").text("0");
                $(this).children("input").val("0");
            }
            else {
                $(this).children("span").text("");
                $(this).children("input").val("");
            }

            $(this).children("select").val("-1");
        });
    }
    //页面类型发生转换时，清空所有的数据。
    function ClearAll() {
        $("#ca_pr_details").find(".dr-row").each(function () {
            if (!$(this).is(":hidden"))
                DelteRow($(this));
        });
        $("#ca_pr_details").find(".dr-row").eq(0).show(); //显示第一行
    }

    ///设置选中的costCenter的值和ID
    function SetCostCenterVal(obj) {
        var selectText = $(obj).children("option:selected").text();
        var selectVal = $(obj).children("option:selected").val();
        $(obj).prev("input").val(selectVal);
        $(obj).next("input").val(selectText);
    }

    //设置选中项的值
    function SetCostCenterSelected(costCenterTD) {
        var ddlCostCenter = costCenterTD.find("select");
        var costCenter = ddlCostCenter.next("input").val();
        var costCenterID = ddlCostCenter.prev("input").val();
        if ($.trim(costCenter) == "-" ||$.trim(costCenter) == "") {
            return false;
        }
        ddlCostCenter.val(costCenterID);
        if (ddlCostCenter.children("option:selected").val() != costCenterID) {// CostCenter找不到的
            var unknowOption = "<option value='" + costCenterID + "'>" + costCenter + "</option>";
            ddlCostCenter.append(unknowOption);
            ddlCostCenter.val(costCenterID);
        }
    }

    ///显示第一个隐藏的Item行
    function ShowFistHide() {
        var firstRow = $("#ca_pr_details").find(".dr-row:hidden");
        firstRow.eq(0).show();
        SetDDLSelectValue(firstRow.find("select"));
    }
/*****************End ItemCode查询全用js去处理*******************************************************************************************************/