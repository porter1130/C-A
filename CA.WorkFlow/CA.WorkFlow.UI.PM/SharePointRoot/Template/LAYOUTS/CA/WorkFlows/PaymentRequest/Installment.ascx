<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Installment.ascx.cs"
    Inherits="CA.WorkFlow.UI.PaymentRequest.Installment" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<style type="text/css">
    .ContentDiv
    {
        padding: 0px;
        margin: 0px;
        position: relative;
    }
    #taxDiv
    {
        margin: 0 auto;
        width: 300px;
        position: absolute;
        left: 30%;
        top: 22%;
        display: none;
        border: 1px solid gray;
        background-color: Gray;
        padding: 5px;
        font-size: 14px;
        z-index: 1000001;
        color:Black;
        font-family:"Segoe UI" , Arial, "Sans-Serif";
    }
    #ErrorDiv
    {
        margin: 0 auto;
        width: 300px;
        position: absolute;
        left: 30%;
        top: 22%;
        display: none;
        border: 1px solid gray;
        background-color: Gray;
        padding: 5px;
        font-size: 14px;
        z-index: 1000001;
        color:Black;
        font-family:"Segoe UI" , Arial, "Sans-Serif";
    }
    #taxMsg
    {
        background-color: White;
        color: Red;
        width: 290px;
        float: left;
        line-height: 22px;
        padding: 5px;
    }
    #ErrorMsg
    {
        background-color: White;
        color: Red;
        width: 290px;
        float: left;
        line-height: 22px;
        padding: 5px;
    }
    #taxTitle
    {
        float: left;
        width: 290px;
        border-bottom: 1px solid Gray;
        font-weight: bold;
        cursor: pointer;
    }
    #title
    {
        float: left;
        width: 290px;
        border-bottom: 1px solid Gray;
        font-weight: bold;
        cursor: pointer;
    } 
    #taxLeft
    {
        float: left;
        color: #3d3d3d;
        padding: 5px;
        padding-top: 3px;
    }
    #taxRight
    {
        color: #3d3d3d;
        float: right;
        padding: 5px;
        padding-top: 3px;
        display: block;
    }
    #left
    {
        float: left;
        color: #3d3d3d;
        padding: 5px;
        padding-top: 3px;
    }
    #right
    {
        color: #3d3d3d;
        float: right;
        padding: 5px;
        padding-top: 3px;
        display: block;
    }
    #taxMsgInfo
    {
        color:Black;
        float: left;
        width: 290px;
        padding: 5px;
        font-weight:lighter;
        line-height:20px;
         font-family:"Segoe UI" , Arial, "Sans-Serif";
         font-size:14px;
        
    }
    #msg
    {
        color:Black;
        float: left;
        width: 290px;
        padding: 5px;
        font-weight:lighter;
        line-height:20px;
         font-family:"Segoe UI" , Arial, "Sans-Serif";
         font-size:14px;
    }
    #taxAlert
    {
        float: left;
        width: 300px;
        font-weight: bold;
        cursor: pointer;
        text-align:right;
        padding: 20px 0px 20px 0px;
    }
    .taxselect
    {
     margin-top:20px;    
    }
    #alert
    {
        float: left;
        width: 300px;
        font-weight: bold;
        cursor: pointer;
        text-align: center;
        padding: 20px 0px 20px 0px;
    }
    
     #taxOK
    {
        color: Black;
        float: left;
        width: 85%;
        text-align:right;
        padding:0px 20px 0px 20px;
    }
    #alertleft
    {
        color: Black;
        float: left;
        width: 50%;
    }
    #alertright
    {
        color: red;
        float: right;
        width: 50%;
    }
    #bgDiv
    {
        position: absolute;
        top: 0px;
        left: 0px;
        right: 0px;
        bottom: 0px;
        display: none;
        background-color: White;
        filter: Alpha(opacity=30);
        z-index: 1000000;
    }
    .close
    {
        color: Red;
    }
    .ExpenseType11
    {
        position: absolute;
        left: 0px !important;
        left: -95px;
        top: 0px;
        z-index: 2;
        cursor: pointer;
        display: block;
    }
    .CostCenter1
    {
        position: absolute;
        left: 0px !important;
        left: -95px;
        top: -15px;
        cursor: pointer;
        display: block;
        z-index: 1;
    }
    .CostCenterTD1
    {
        width: 350px;
    }
    .ExpenseTypeTD1
    {
        width: 330px;
    }
    .padd_bom10
    {
        padding-bottom: 10px;
        width: 582px;
        font-weight: bold;
    }
    
    .mag_top5
    {
        margin-top: 5px;
    }
    
    .flt_left
    {
        float: left;
    }
    .ca-workflow-form-buttons
    {
        width: 680px;
    }
    .txtTotalAmount2
    {
    }
    
    .out_table
    {
        margin-bottom: 0px;
        border-bottom: none;
    }
    #InstallmentDiv
    {
      font-size:12px;
      font-weight:lighter;    
    }
</style>
<%--<script type="text/javascript" src="jquery-1.4.1-vsdoc.js"></script>--%>
<div id="InstallmentDiv" class="hidden ContentDiv" >
    <div class="ca-workflow-form-buttons">
        <input type="button" class="InstallButton" id="PaymentCofirm" value="Confirm" onclick="ConfirmDialog();" />
        <input type="button" class="InstallButton" id="PaymentClose" value="Close" onclick="CloseDialog();" />
    </div>
    <div id="ErrorDiv">
        <div id="ErrorMsg">
            <div id="title">
                <div id="left">
                    Notice</div>
                <div id="right">
                    <a style="color: Red; font-size: 16px;" onclick="CloseAlert();"></a></div>
            </div>
            <div id="msg">
            </div>
            <div id="alert">
                <div id="alertleft">
                    <a onclick="OKAlert();" class="okbtn" style="color: #3d3d3d; font-size: 16px;">Yes</a></div>
                <div id="alertright">
                    <a style="color: Red; font-size: 16px;" onclick="CloseAlert();">No</a></div>
            </div>
        </div>
    </div>
    <div id="taxDiv">
        <div id="taxMsg">
            <div id="taxTitle">
                <div id="taxLeft">
                    Notice</div>
                <div id="taxRight">
                </div>
            </div>
            <div id="taxMsgInfo">
                <div class="taxselect">
                    请选择税率：<select id="taxSelect" style="width: 70px"><option value="0.17">0.17</option>
                    </select></div>
            </div>
            <div id="taxAlert">
                <div id="taxOK">
                    <a onclick="UpdateTax();" class="oktaxbtn" style="color: #3d3d3d; font-size: 16px;">
                        OK</a>
                    <asp:HiddenField ID="hfTaxPrice" runat="server" Value="0" />
                    <asp:HiddenField ID="hfTaxStatus" runat="server" Value="0" />
                     <asp:HiddenField ID="hfNoTaxTotalAmount" runat="server" Value="0" />
                </div>
            </div>
        </div>
    </div>
    <div id="bgDiv">
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="ca-workflow-form-table ca-workflow-form-table1" style="width: 710px;
                margin: 0px 0px 15px 0px; font-weight: bold; display:none">
                <tr>
                    <td class="label1 w25" align="center">
                        Currency：
                    </td>
                    <td class="label1 w25" align="center">
                       
                    </td>
                    <td class="label1 w25" align="center">
                        Exchange Rate：
                    </td>
                    <td class="label1 w25" align="center">
                        <asp:TextBox ID="txtExchangeRate" runat="server" CssClass="txtExchangeRate" Text="1"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table class="ca-workflow-form-table ca-workflow-form-table1" id="CostCenterTable1"
                style="width: 710px; border: #9dabb6 2px solid;">
                <tr style="font-weight: bold; text-align: center">
                    <td class="label1 w5" style="border-left: 1px solid #CCCCCC;">
                        <asp:ImageButton runat="server" ID="btnAddItem" ToolTip="Click to add the information."
                            ImageUrl="../images/pixelicious_001.png" OnClick="btnAddItem_Click" Width="18"
                            CssClass="img-button" />
                        <asp:HiddenField ID="hfTax" runat="server" Value="" />
                        <asp:HiddenField ID="hfGLAccount1" runat="server" Value="" />
                        <asp:HiddenField ID="FAStatus2" runat="server" Value="0" />
                        <asp:HiddenField ID="hidSummaryExpenseType" runat="server" Value="" />
                    </td>
                    <td class="label1 w20 FA">
                        Asset No<span style="font-weight:lighter" >(if known)</span>
                    </td>
                    <td class="label1 w20">
                        <asp:Label ID="lblExpenseType1" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="label1 w15">
                        <asp:Label ID="lblTDStatus1" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="label1 w25">
                        Cost Center
                    </td>
                    <td class="label1 w15">
                        Amount
                    </td>
                </tr>
                <asp:Repeater ID="rptItem" runat="server" OnItemCommand="rptItem_ItemCommand" OnItemDataBound="rptItem_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="label1" style="border-left: 1px solid #CCCCCC;">
                                <asp:ImageButton ID="btnDeleteItem" ToolTip="Remove this information." CommandName="delete"
                                    runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                            </td>
                            <td class="label1 FA">
                                <asp:TextBox ID="txtFANO" runat="server" CssClass="FANO1"></asp:TextBox>
                            </td>
                            <td class="label1">
                                <div style="position: relative; background-color: White; z-index: 2">
                                    <div class="ExpenseType11">
                                        <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="ExpenseType1" onchange="ChangeGLAccount1(this);DrawSummaryExpenseTable()">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                            <td class="label1">
                                <asp:TextBox ID="lblGLAccount" runat="server" CssClass="GLAccount"></asp:TextBox>
                            </td>
                            <td class="label1">
                                <div style="position: relative; background-color: White; z-index: 1">
                                    <div class="CostCenter1">
                                        <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="cc1" onchange="DrawSummaryExpenseTable()">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                            <td class="label1">
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="Amount1"  onchange="DrawSummaryExpenseTable()"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="height: 15px; border-left: 1px solid #CCCCCC; border-right: 1px solid #CCCCCC;
                                border-bottom: 1px solid #CCCCCC;">
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hfSummaryAmount" runat="server"  Value="0"/>
    <asp:HiddenField ID="hf_I_RequestType" runat="server" Value="" />
    <script type="text/javascript">
        $(function () {
            DisplayOrHideFATD1();
            BindAmountBlurEvent1();
            BindExpenseTypeEvent1();
            DrawSummaryExpenseTable();
            GetCalAmount1();
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        });
        function DisplayOrHideFATD1() {
            var $FAStatus2 = $('#<%=this.FAStatus2.ClientID %>');
            if ($FAStatus2.val() == "0") {
                $("#CostCenterTable1 td.FA").hide();
            }
            
        }
        //        function EndRequestHandler() {
        //            BindAmountBlurEvent1();
        //            BindExpenseTypeEvent1();
        //            //取得Items总金额
        //            GetCalAmount1();
        //            ChangeInstallmentInfo();
        //            //alert("totalAmount：" + totalAmount);
        //        }
        function BindAmountBlurEvent1() {
            var $inputAmount = $("#CostCenterTable1 td input.Amount1");
            $inputAmount.each(function () {
                $(this).blur(function () {
                    if (isNaN($(this).val()) || $(this).val() < 0 || $(this).val() > 100000000) {
                        $(this).val("0");
                        if (!$(this).parent().hasClass("wrapdiv")) {
                            $(this).wrap("<span class=\"wrapdiv\"></span>");
                        }
                        alert('Please fill the valid number.');
                    } else {
                        $(this).parent().removeClass("wrapdiv");
                    }
                    if ($(this).val() == "" || $(this).val() == "0") {
                        $(this).val("0")
                    } else {
                        //四舍五入取整
                        var amount = Math.round($(this).val() * Math.pow(10, 2)) / Math.pow(10, 2);
                        $(this).val(amount);
                    }
                    //取得Items总金额
                    var totalAmount = GetCalAmount1();
                    ChangeInstallmentInfo();
                    //                    var $txtTotalAmount = $("input.txtTotalAmount2");
                    //                    $txtTotalAmount.val(totalAmount);
                    //alert("totalAmount：" + totalAmount);
                });
            });
        }
        function GetCalAmount1() {
            var result = false;
            var $inputAmount = $("#CostCenterTable1 td input.Amount1");
            var totalAmount = 0;
            $inputAmount.each(function () {
                var inputAmount = $(this).val();
                if (inputAmount == "") {
                    inputAmount = "0";
                }
                totalAmount += parseFloat(inputAmount);
            });
            var amount = Math.round(totalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
            var $txtTotalAmount = $("input.txtTotalAmount2");
            $txtTotalAmount.val(amount);
            return amount;
        }
        function BindExpenseTypeEvent1() {
            var $ExpenseType = $("#CostCenterTable1 .ExpenseType11");
            $ExpenseType.each(function () {
                $(this).mousemove(function () {
                    $(this).addClass("ExpenseTypeTD1");
                });
                $(this).mouseout(function () {
                    $(this).removeClass("ExpenseTypeTD1");
                });
                var $children = $(this).children();
                $children.click(function () {
                    $children.parent().addClass("ExpenseTypeTD1");
                    $children.parent().unbind("mouseout");
                });
                $children.blur(function () {
                    $children.parent().removeClass("ExpenseTypeTD1");
                    $children.parent().mouseout(function () {
                        $children.parent().removeClass("ExpenseTypeTD1");
                    });
                });
            });

            var $CostCenter = $("#CostCenterTable1 .CostCenter1");
            $CostCenter.each(function () {
                $(this).mousemove(function () {
                    $(this).addClass("CostCenterTD1");
                });
                $(this).mouseout(function () {
                    $(this).removeClass("CostCenterTD1");
                });
                var $children = $(this).children();
                $children.click(function () {
                    $children.parent().addClass("CostCenterTD1");
                    $children.parent().unbind("mouseout");
                });
                $children.blur(function () {
                    $children.parent().removeClass("CostCenterTD1");
                    $children.parent().mouseout(function () {
                        $children.parent().removeClass("CostCenterTD1");
                    });
                });
            });
        }
        function CheckSubmit1() {
            $("#CostCenterTable1 .wrapdiv").removeClass("wrapdiv");
            var result = true;
            var msg = "";
            var $et = $("#CostCenterTable1 select.ExpenseType1");
            $et.each(function () {
                if ($.trim($(this).val()) == "") {
                    msg += "Please Select Expense Type.\n";
                    //                    if (!$(this).parent().parent().hasClass("wrapdiv")) {
                    //                        $(this).parent().addClass("wrapdiv");
                    //                    }
                    result = false;
                }
//                var hf_I_RequestType = $("input[id$='hf_I_RequestType']").val();
//                if (hf_I_RequestType == "Opex") {
//                    var expense = $(this).val();
//                    var $costc = $(this).parent().parent().parent().parent().find("select.cc1");
//                    if (expense.indexOf("Store") == 0) {
//                        if ($costc.val().indexOf("S") != 0) {
//                            msg += "Please Select Store CostCenter.\n";
//                            if (!$costc.parent().hasClass("wrapdiv")) {
//                                $costc.parent().addClass("wrapdiv");
//                            }
//                            result = false;
//                        }
//                    }
//                    if (expense.indexOf("HO") == 0) {
//                        //var $costc = $(this).parent().parent().parent().parent().find("select.cc1");
//                        if ($costc.val().indexOf("H") != 0) {
//                            msg += "Please Select Head Office CostCenter.\n";
//                            if (!$costc.parent().hasClass("wrapdiv")) {
//                                $costc.parent().addClass("wrapdiv");
//                            }
//                            result = false;
//                        }
//                    }
//                    if (expense.indexOf("Store") != 0 && expense.indexOf("HO") != 0) {
//                        if ($costc.val().indexOf("S") == 0) {
//                            msg += "Please Select Not Start With 'S' CostCenter.\n";
//                            if (!$costc.parent().hasClass("wrapdiv")) {
//                                $costc.parent().addClass("wrapdiv");
//                            }
//                            result = false;
//                        }
//                    }
//                }
            });
            var $cc = $("#CostCenterTable1 select.cc1");
            $cc.each(function () {
                var $expenseType = $(this).parent().parent().parent().prev().prev().find("select.ExpenseType1");
                if ($.trim($(this).val()) == "" && $expenseType.val() != "Tax payable - VAT input"
                    && $expenseType.val().indexOf("Accrual") != 0
                    && $expenseType.val().indexOf("Accrued") != 0) {
                    //&& $expenseType.val().indexOf("Prepaid") != 0) {
                    msg += "Please Select CostCenter.\n";
                    if (!$(this).parent().parent().hasClass("wrapdiv")) {
                        $(this).parent().addClass("wrapdiv");
                    }
                    result = false;
                }
            });
            var $txtAmount = $("#CostCenterTable1 input.Amount1");
            $txtAmount.each(function () {
                if ($.trim($(this).val()) == "" || $.trim($(this).val()) == "0") {
                    msg += "Please fill the Amount.\n";
                    if (!$(this).parent().hasClass("wrapdiv")) {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                    result = false;
                }
            });
//            var $FAStatus2 = $('#<%=this.FAStatus2.ClientID %>'); 
//            if ($FAStatus2.val() == "1") {
//                var $txtFANO = $("#CostCenterTable1 input.FANO1");
//                $txtFANO.each(function () {
//                    var $et11 = $(this).parent().parent().find("select.ExpenseType1"); 
//                    if ($.trim($(this).val()) == "" && $et11.val() != "Tax payable - VAT input") {
//                        msg += "Please fill the FANO.\n";
//                        if (!$(this).parent().hasClass("wrapdiv")) {
//                            $(this).wrap("<span class=\"wrapdiv\"></span>");
//                        }
//                        result = false;
//                    }
//                });
//            }
            if (msg != "") {
                alert(msg);
                return false;
            }
            var r = CheckTax();
            if (!r) {
                return false;
            }
            return result;
        }

        function GetTmpId1(tmpId) {
            return tmpId.substring(0, tmpId.lastIndexOf('_') + 1);
        }
        function ChangeGLAccount1(obj) {
            var $cc = $(obj).parent().parent().parent().parent().find("select.cc1");
            var $fano = $(obj).parent().parent().parent().parent().find("input.FANO1");
            //var $amount = $(obj).parent().parent().parent().parent().find("input.Amount1"); 
            if ($(obj).val() != "Tax payable - VAT input") {
                //&& $(obj).val().indexOf("Prepaid") == -1) {
                $cc.parent().show();
                $fano.show();
            } else {
                //$amount.val("");
                $cc.val("");
                $fano.val("");
                $cc.parent().hide();
                $fano.hide();
            }
            var $expenseType = $(obj);
            var $glAccount = $expenseType.parent().parent().parent().parent().find("td input.GLAccount");
            var $hfGLAccount = $('#<%=this.hfGLAccount1.ClientID %>');
            var $lblGLAccount = $('#' + GetTmpId1(obj.id) + 'lblGLAccount');
            var glaccountTable = eval("(" + $hfGLAccount.val() + ")");
            try {
                $.each(glaccountTable, function (i, item) {
                    if ($expenseType.val() == "0") {
                        $glAccount.val("");
                        $lblGLAccount.val("");
                        return;
                    } else {
                        if ($expenseType.val() == item.name) {
                            $glAccount.val(item.val);
                            $lblGLAccount.val(item.val);
                            return;
                        }
                    }
                });
            }
            catch (e) {

            }

        }
    </script>
    <script type="text/javascript">
        function AppendHtml(expenseType, amount, costcenter) {
            var appendHtml = "";
            var costcentertd = costcenter;
            appendHtml = "<tr class=\"item\" ><td class=\"label1\" >" + expenseType + "</td><td class=\"label1\" >" + costcentertd + "</td><td class=\"label1\" ><input class=\"SummaryAmount\"  type=\"text\" value=\"" + amount + "\" /></td></tr>";
            return appendHtml;
        }
        function CheckSummaryTypeHtml(expenseType, costcenter) {
            var result = false;
            var exp = ".summarytypetable tr:contains('" + expenseType + "')";
            //alert(exp);
            var $summarytypehtml = $(exp);
            var $expcostcenter = $summarytypehtml.find("td:contains('" + costcenter + "')");
            //alert("$summarytypehtml.length" + $summarytypehtml.length);
            //alert("$expcostcenter.length" + $expcostcenter.length);
            if ($summarytypehtml.length > 0) {
                if ($expcostcenter.length > 0) {
                    result = true;
                }
            }
            return result;
        }
        function UpdateSummaryExpenseTypeAmount(expenseType, amount, costcenter) {
            var exp = ".summarytypetable tr td:contains('" + expenseType + "')";
            var $summarytypehtml = $(exp);
            var $expcostcenter = $summarytypehtml.parent().find("td:contains('" + costcenter + "')");
            if ($expcostcenter.length > 0) {
                var $amount = $expcostcenter.next();
                var totalamount = CalExpenseTypeAmount(expenseType, costcenter);
                $amount.text(totalamount + "");
            }
        }
        function CalExpenseTypeAmount(expenseType, costcenter) {
            var $ExpenseType = $("select.ExpenseType1");
            var amount = 0;
            var et = expenseType;
            $ExpenseType.each(function () {
                var costcenterval = $(this).parent().parent().parent().parent().find("td select.cc1").val();
                if ($(this).val().indexOf(et) != -1 && costcenterval.indexOf(costcenter) != -1) {
                    var $amount = $(this).parent().parent().parent().parent().find("input.Amount1");
                    if ($amount.val() != "") {
                        amount += parseFloat($amount.val());
                    }
                }
            });
            amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
            return amount;
        }
        function DrawSummaryExpenseTable() {
            $(".summarytypetable tr").remove(".item");
            var $summarytype = $(".summarytype");
            var $expenseType = $("#CostCenterTable1 td select.ExpenseType1");
            $expenseType.each(function () {
                if ($(this).val() != "") {
                    var $amount = $(this).parent().parent().parent().parent().find("input.Amount1");
                    var expenseType = $(this).val();
                    var costcenter = $(this).parent().parent().parent().parent().find("td select.cc1").val();
                    var result = CheckSummaryTypeHtml(expenseType, costcenter);
                    if (isNaN($amount.val())) {
                        alert('Please fill the valid number.');
                        $amount.val("0");
                    }
                    if ($amount.val() < 0) {
                        alert('Please fill the valid number.');
                        $amount.val("0");
                    }
                    if ($amount.val() > 100000000) {
                        alert('Please fill the number of less than 100000000.');
                        $amount.val("0");
                    }
                    var txtAmount = $amount.val() == "" ? "0" : $amount.val();
                    txtAmount = Math.round(txtAmount * Math.pow(10, 2)) / Math.pow(10, 2);
                    var $html = $(AppendHtml(expenseType, txtAmount, costcenter));
                    $summarytype.before($html);
                    //                    if (!result) {
                    //                        var $html = $(AppendHtml(expenseType, txtAmount, costcenter));
                    //                        $summarytype.before($html);
                    //                    } else {
                    //                        UpdateSummaryExpenseTypeAmount(expenseType, txtAmount, costcenter);
                    //                    }
                }
            });
            UpdateSummaryAmount();
            UpdateSummaryExpenseType();
        }

        function UpdateSummaryExpenseType() {
            var $hidSummaryExpenseType = $('#<%= this.hidSummaryExpenseType.ClientID %>');
            var summaryExpenseType = "[";
            var $summarytypetable = $(".summarytypetable tr.item");
            $summarytypetable.each(function () {
                //summaryExpenseType += "{name:'" + $(this).find("td").eq(0).text() + "',val:'" + $(this).find("td").eq(1).text() + "'},";
                summaryExpenseType += "{name:'" + $(this).find("td").eq(0).text() + "',val:'" + $(this).find("td").eq(2).find("input").val() + "',costcenter:'" + $(this).find("td").eq(1).text() + "'},";
            });
            summaryExpenseType += "]";
            $hidSummaryExpenseType.val(summaryExpenseType);
        }

        function UpdateSummaryAmount() {
            var $SummaryAmount = $("#out_table table.summarytypetable tr td input.SummaryAmount");
            var $radioInstallment = $("#out_table .radioInstallment input[type=radio]");
            var $txtTotalAmount2 = $("#Table1 tr td input.txtTotalAmount2");
            var $PaymentPercent = $("#InstallmentDiv tr.PaymentItem td input.PaymentPercent");
            var $PaymentAmount = $("#InstallmentDiv tr.PaymentItem td input.PaymentAmount");
            var totalAmount = 0;
            var hfSummaryAmount = "";

            

            $SummaryAmount.each(function (i) {
                var amount = $(this).val();
                if ($radioInstallment.eq(1).attr("checked")) {
                    if ($SummaryAmount.length == 1) {
                        var tempAmount = Math.round(parseFloat($txtTotalAmount2.val()) * Math.pow(10, 2)) / Math.pow(10, 2);
                        $(this).val(tempAmount);
                        hfSummaryAmount += tempAmount + ";";
                    } else {
                        var tempAmount1 = Math.round(parseFloat($(this).val()) * Math.pow(10, 2)) / Math.pow(10, 2);
                        hfSummaryAmount += tempAmount1 + ";";
                    }
                    $(this).css("border", "none");
                    $(this).css("color", "#000000");
                    $(this).css("backgroundColor", "transparent");
                    $(this).css("textAlign", "center");
                    $(this).attr("readonly", "readonly");
                } else {
                    if ($SummaryAmount.length == 1) {
                        var paymentAmount = $PaymentAmount.eq(0).val();
                        var tempAmount2 = Math.round(parseFloat(paymentAmount) * Math.pow(10, 2)) / Math.pow(10, 2);
                        $(this).val(tempAmount2);
                        hfSummaryAmount += tempAmount2 + ";";
                        $(this).css("border", "none");
                        $(this).css("color", "#000000");
                        $(this).css("backgroundColor", "transparent");
                        $(this).css("textAlign", "center");
                        $(this).attr("readonly", "readonly");
                    } else {
                        if (i + 1 == $SummaryAmount.length) {
                            var ta = parseFloat($PaymentAmount.eq(0).val()) - totalAmount;
                            $(this).val(Math.round(ta * Math.pow(10, 2)) / Math.pow(10, 2));
                            hfSummaryAmount += $(this).val() + ";";
                        } else {
                            var currentAmount = Math.round(parseFloat($(this).val()) * (parseFloat($PaymentPercent.eq(0).val()) / 100) * Math.pow(10, 2)) / Math.pow(10, 2);
                            totalAmount += currentAmount;
                            hfSummaryAmount += currentAmount + ";";
                            $(this).val(currentAmount);
                        }
                    }
                }
            });
            var $hfSummaryAmount = $("#InstallmentDiv input[id$='hfSummaryAmount']");
            $hfSummaryAmount.val(hfSummaryAmount);
        }
    </script>
    <script type="text/javascript">
        function CheckTax() {
            var result = false;
            var $et = $("#CostCenterTable1 select.ExpenseType1");
            $et.each(function () {
                var $glaccount = $(this).parent().parent().parent().parent().find("input.GLAccount");
                var gl = $glaccount.val();
                if ($.trim($(this).val()) == "Tax payable - VAT input") {
                    result = true;
                }
//                if (gl.indexOf("A") != -1) {
//                    result = true;
//                }
            });
            var $hfTax = $("#CostCenterTable1 input:hidden[id$='hfTax']"); 
            if (!result && $hfTax.val() != "1" && $hfTax.val() != "2" && $hfTax.val() != "") {
                var msg = "<br/>Is it a VAT invoice ? If yes, please split the tax and net invoice amount.<br/><br/>是否为增值税专用发票 ? 若是，请分开填写不含税金额和税额.";
                AlertSelect(msg);
                result = false;
            } else {
                result = true;
            }
            return result;
        }
        function AlertSelect(msg) {
            $("#bgDiv").css({
                height: function () {
                    return $(".ContentDiv").height()/2;
                },
                width: "100%"
            });
            $("#bgDiv").show();
            $("#msg").html("");
            $("#msg").append(msg);
            $("#ErrorDiv").fadeIn(1000);
        }
        function CloseAlert() {
            var $hfTax = $("#CostCenterTable1 input:hidden[id$='hfTax']");
            $hfTax.val("2");
            $("#ErrorDiv").hide();
            $("#bgDiv").hide();
            //return false;

            DrawSummaryExpenseTable();
            if ($(".radioInstallment input[type=radio]:checked").val() == "Yes" && CheckTotalPercent() == false) {
                alert(' 分期付款百分比累计必须等于100% ');
                return false
            }
            else {
                CloseDialog();
            }
            var $summaryTR = $("#out_table tr.summaryTR");
            $summaryTR.css("display", "block");
        }
        function OKAlert() {
            $("#ErrorDiv").hide();
            $("#bgDiv").hide();
            //加行
            var $hfTax = $("#CostCenterTable1 input:hidden[id$='hfTax']");
            $hfTax.val("1");
            var $btnAddItem = $("#CostCenterTable1 input[id$='btnAddItem']");
            $btnAddItem.click();

//            var $hfTaxStatus = $("input[id$='hfTaxStatus']");
//            $hfTaxStatus.val("2"); 
//            ShowTaxBox();
        }
    </script>
    <script type="text/javascript">
        var txtAmountObj;
        function ShowTaxBox() {
            TaxSelect();
        }
        function TaxSelect() {
            $("#bgDiv").css({
                height: function () {
                    return $(".ContentDiv").height() / 2;
                },
                width: "100%"
            });
            $("#bgDiv").show();
            $("#taxDiv").fadeIn(1000);
        }
        function UpdateTax() {
            $("#taxDiv").hide();
            $("#bgDiv").hide();
            var $hfTaxPrice = $("input[id$='hfTaxPrice']");
            $hfTaxPrice.val($("#taxSelect").val());
            var $hfNoTaxTotalAmount = $("input[id$='hfNoTaxTotalAmount']");
            $hfNoTaxTotalAmount.val($("input.txtTotalAmount2").val());

            var $hfTaxStatus = $("input[id$='hfTaxStatus']");
            
            if ($hfTaxStatus.val() == "0") {

                var total = (parseFloat($hfNoTaxTotalAmount.val()) - parseFloat($(txtAmountObj).val())) * parseFloat($hfTaxPrice.val());
                var amount = Math.round(total * Math.pow(10, 2)) / Math.pow(10, 2);
                $(txtAmountObj).val(amount);
                GetCalAmount1();
            } else {
                var $btnAddItem = $("#CostCenterTable1 input[id$='btnAddItem']");
                $btnAddItem.click();
                $hfTaxStatus.val("0"); 
            }
        }
        function CheckAmountChange(obj) {
            var $amount = $(obj);
            var $et = $amount.parent().parent().find("select.ExpenseType1");

            if ($et.val() == "Tax payable - VAT input") {
                $amount.blur();
                txtAmountObj = obj;
                TaxSelect();
            }
        }
    </script>
    <div style="float: right;">
        <table id="Table1" class="ca-workflow-form-table" style="border: none;" cellspacing="0"
            border="0" cellpadding="0">
            <tr>
                <td class="label" align="right" style="border: 0 none; font-weight: bold;">
                    Currency：<asp:DropDownList ID="ddlCurrency" runat="server" CssClass="ddlCurrency"
                        Width="60px">
                    </asp:DropDownList>　　　
                    Total Amount:
                    <asp:TextBox ID="txtTotalAmount2" runat="server" CssClass="txtTotalAmount2" Width="145px"
                        ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="InstallmentItemsDiv">
        <div>
            <br />
            <table id="InstallmentItem" class="ca-workflow-form-table" style="border-bottom: none;
                margin-bottom: 0px; font-weight: bold;" cellpadding="10">
                <tr>
                    <td class="label1" style="height: 35px;">
                        <span class="mag_top5 flt_left">No of Installment:</span>&nbsp;&nbsp;
                        <asp:DropDownList ID="DDLPaymentCount" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DDLPaymentCount_SelectedIndexChanged"
                            Height="20px" Width="62px">
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                            <asp:ListItem>24</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 582px">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Repeater ID="ReapterInstallment" runat="server">
                        <HeaderTemplate>
                            <table id="InstallmentItem" class="ca-workflow-form-table" style="border-top: 0 none;
                                font-weight: bold;" cellpadding="10">
                                <tr>
                                    <td class="label align-center" style="width: 20%">
                                        付款百分比<br />
                                        Percent
                                    </td>
                                    <td class="label align-center" style="width: 20%">
                                        付款金额<br />
                                        Amount
                                    </td>
                                    <td class="label align-center" style='width: 60%'>
                                        备注<br />
                                        Comments
                                    </td>
                                    <td class="label align-center" style='display: none'>
                                        收货后付款<br />
                                        Payment after GR/SR
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="PaymentItem">
                                <td class="label align-center">
                                    <asp:TextBox ID="txtPaid" Width="60px" class="PaymentPercent" runat="server" Text='<%# Eval("Paid")%>'></asp:TextBox>
                                    %
                                </td>
                                <td class="label align-center">
                                    <asp:TextBox ID="txtPaidAmount" Width="80px" class="PaymentAmount" runat="server"
                                        Text='<%# Eval("PaymentAmount")%>'></asp:TextBox>
                                </td>
                                <td class="label align-center">
                                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="3" Columns="20"
                                        CssClass="PaymentComments" Text='<%# Eval("Comments")%>'></asp:TextBox>
                                </td>
                                <td class="label align-center" style='display: none'>
                                    <asp:CheckBox ID="txtIsNeedGR" class="PaymentGRSR" runat="server" Style="border: none;
                                        width: 20px" Checked='<%# Eval("IsNeedGR").ToString()=="1"?true:false%>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DDLPaymentCount" EventName="SelectedIndexChanged">
                    </asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<script type="text/javascript">

    //验证百分比是否符合100%
    function CheckTotalPercent() {
        var result = true;
        var regFloat = /^([1-9][0-9]*)(\.?)(\d*)$/;
        var regInt = /^\d$/;
        var totalPercent = 0;
        $(".PaymentItem .PaymentPercent").each(function () {
            var percent = $(this).val();
            if (percent != "") {
                if (regFloat.test(percent)) {
                    totalPercent += parseFloat($(this).val());
                }
                else {//数据不合法
                    $(this).focus();
                    $(this).val("");
                    result = false;
                    return false;
                }
            }
        });

        if (totalPercent.toFixed(0) != 100) {
            result = false;
            return false;
        }
        return result;
    }

    function BindEvent() {
        $(".PaymentAmount").each(function (i) {
            var ind = i;
            $(this).bind({
                change: function (ind) {
                    var totalAmount = $(".txtTotalAmount2").val();
                    if (totalAmount > 0) {
                        var amount = $(this).val();
                        var percent = Math.round(amount * 100000 / totalAmount) / 1000;
                        $(".PaymentPercent").each(function (j) {
                            if (i == j) {
                                $(this).val(percent);
                            }
                        });
                    }
                }
            });
        });

        $(".PaymentPercent").each(function (i) {
            var ind = i;
            $(this).bind({
                change: function (ind) {
                    var totalAmount = $(".txtTotalAmount2").val();
                    if (totalAmount > 0) {
                        var percent = $(this).val();
                        var amount = Math.round(percent * totalAmount * 10) / 1000;
                        $(".PaymentAmount").each(function (j) {
                            if (i == j) {
                                $(this).val(amount);
                            }
                        });
                    }
                }
            });
        });
    } BindEvent();

    function ChangeInstallmentInfo() {
        if ($(".txtTotalAmount2").val() > 0) {
            $(".PaymentItem .PaymentPercent").each(function (i) {
                var percent = $(this).val();
                var totalAmount = $(".txtTotalAmount2").val();
                if ($.trim(percent) != "" && percent != 0) {
                    var amount = Math.round(percent * totalAmount * 10) / 1000;
                    $(".PaymentItem .PaymentAmount").each(function (j) {
                        if (i == j) {
                            $(this).val(amount);
                        }
                    });
                }
                else {
                    var amount = $(this).val();
                    var percent = Math.round(amount * 100000 / totalAmount) / 1000;
                    $(".PaymentItem .PaymentPercent").each(function (j) {
                        if (i == j) {
                            $(this).val(percent);
                        }
                    });
                }
            });
        }
    }

    $('#InstallmentDiv').dialog({
        title: "Please input the Payment Info",
        width: 750,
        height: 466,
        autoOpen: false,
        modal: true,
        resizable: false,
        draggable: true,
        bigiframe: true,
        open: function (type, data) {
            if ($(".radioInstallment input[type=radio]:checked").val() == "No") {
                $("#InstallmentItemsDiv").css("display", "none");
            }
            else {
                $("#InstallmentItemsDiv").css("display", "");
            }
        },
        create: function (type, data) {
            $(this).parent().appendTo('form');

        },
        beforeClose: function () {
            var $txtTotalAmount2 = $("#Table1 tr td input.txtTotalAmount2");
            var $PaymentAmount = $("#InstallmentDiv tr.PaymentItem td input.PaymentAmount");
            var totalPaymentAmount = 0;
            $PaymentAmount.each(function () {
                if ($(this).val() != "") {
                    totalPaymentAmount += Math.round(parseFloat($(this).val()) * Math.pow(10, 2)) / Math.pow(10, 2);
                }
            });
            var totalAmount = Math.round(parseFloat($txtTotalAmount2.val()) * Math.pow(10, 2)) / Math.pow(10, 2);
            totalPaymentAmount = Math.round(parseFloat(totalPaymentAmount) * Math.pow(10, 2)) / Math.pow(10, 2);
            if (totalAmount != totalPaymentAmount && totalPaymentAmount != 0) {
                alert("分期付款金额之和不等于总金额！");
                return false;
            }


            GetCalAmount1();
            var $hfTax = $("#CostCenterTable1 input:hidden[id$='hfTax']");
            $hfTax.val("2");
            $(".txtTotalAmount").val($(".txtTotalAmount2").val());
            $(".txtTotalAmount1").val($(".txtTotalAmount2").val());
            SetInstallmentInfo();
            if ($(".radioInstallment input[type=radio]:checked") == "Yes") {
                if (CheckTotalPercent() == false) {
                    return confirm('您输入的分期付款百分比数据存在错误，确定继续关闭窗口');
                }
            }
        }
    });

    function OpenDialog() {
        $('#InstallmentDiv').dialog('open');
        var $hfTax = $("#CostCenterTable1 input:hidden[id$='hfTax']");
        $hfTax.val("0");
        var $hfTaxStatus = $("#CostCenterTable1 input:hidden[id$='hfTaxStatus']");
        $hfTaxStatus.val("0");
        return false;
    }

    function CloseDialog() {
        GetCalAmount1();
        $('#InstallmentDiv').dialog('close');
        return false;
    }

    function ConfirmDialog(sender) {
        SetMainPageCurrency($('#<%=this.ddlCurrency.ClientID %>').val());
        var result = CheckSubmit1();
        if (!result) {
            return false;
        }
        DrawSummaryExpenseTable();



        if ($(".radioInstallment input[type=radio]:checked").val() == "Yes" && CheckTotalPercent() == false) {
            alert(' 分期付款百分比累计必须等于100% ');
            return false
        }
        else {
            CloseDialog();
        }
        var $summaryTR = $("#out_table tr.summaryTR");
        $summaryTR.css("display", "block");
    }

</script>
