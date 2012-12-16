<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit2.ascx.cs" Inherits="CA.WorkFlow.UI.EmployeeExpenseClaim2.DataEdit2" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<script type="text/javascript" src="JS/Calendar3.js"></script>
<style type="text/css">
    .ca-workflow-form-table td
    {
        padding: 5px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        text-align: center;
        margin: 0;
        height: 25px;
        line-height: 15px;
    }
    .pftd td
    {
        padding: 0px;
        border: none;
        text-align: left;
        margin: 0;
        height: auto;
        line-height: normal;
    }
    .pftd img
    {
        margin-top: 3px;
    }
    .ca-workflow-form-table
    {
        margin-top: 20px;
    }
    .attachment td
    {
        border-bottom: none;
        border-right: none;
    }
    .cashadvancediv
    {
        padding: 0px;
        margin: 0px;
        width: auto;
        height: auto;
        position: relative;
    }
    .cardiv
    {
        padding: 0px;
        margin: 0px;
        width: 228px;
        height: auto;
        position: absolute;
        left: 0px;
        top: 27px;
        border: 1px solid #999999;
        border-top: none;
        background-color: White;
        display: none;
    }
    .cardivscroll
    {
        height: 400px;
        overflow-x: hidden;
        overflow-y: scroll;
        scrollbar-face-color: #e4e4e5;
    }
    .titlediv
    {
        padding-top: 8px;
        padding-left: 5px;
        margin: 0px;
        width: 230px;
        height: 30px;
        position: absolute;
        left: 0px;
        top: 0px;
        background-image: url(../../../CAResources/themeCA/images/carwf_select.gif);
        background-repeat: no-repeat;
        cursor: pointer;
    }
    .cashadvancediv input
    {
        width: auto;
        border: 1px solid #999999;
        margin-right: 5px;
    }
    .cashadvancediv ul li
    {
        list-style: none;
        width: 218px !important;
        width: 220px;
        padding: 5px;
        cursor: pointer;
    }
    .titledivbg
    {
        background-image: url(../../../CAResources/themeCA/images/carwf_selectup.gif);
    }
    .carli
    {
        background-color: #e4e4e5;
    }
    .ca-workflow-form-table .ms-dtinput input
    {
        width: 60px;
    }
    .ca-workflow-form-table select
    {
        margin-left: 0px;
        width: 100%;
    }
    .ca-workflow-form-table1 input
    {
        padding-left: 0px;
        padding-right: 0px;
    }
    .ca-workflow-form-table1 td
    {
        padding: 5px 2px 5px 2px;
    }
    .w13
    {
        width: 13%;
    }
    .w27
    {
        width: 27%;
    }
    .w12
    {
        width: 12%;
    }
    .wrapdiv
    {
        padding: 2px;
        border: 1px solid red;
    }
</style>
<style type="text/css">
    .summarytype
    {
        display: none;
    }
    #ExpenseTypetable img
    {
        cursor: pointer;
    }
    .Remark
    {
        display: none;
    }
    .Remark input
    {
         width:98%;
    }
    .Insert
    {
        display: none;
    }
    div
    {
        width: auto;
        height: auto;
    }
</style>
<style type="text/css">
    .ExpenseTypeContainer
    {
        position: relative;
        z-index: 1000;
        background-color: White;
    }
    .ExpenseTypeSub
    {
        position: absolute;
        left: 0px !important;
        left: -35px;
        top: 0px;
        z-index: 1001;
    }
    .ExpenseTypeTD
    {
        width: 260px;
    }
    .CostCenterContainer
    {
        position: relative;
        z-index: 1000;
        background-color: White;
    }
    .CostCenterSub
    {
        position: absolute;
        left: 0px !important;
        left: -30px;
        top: 0px;
        z-index: 1001;
    }
    .CostCenterTD
    {
        width: 260px;
    }
    .MobileDate
    {
        position: relative;
        background-color: White;
        z-index: 111;
        width: 90px;
        padding-top: 5px;
        padding-bottom: 5px;
        height: 25px;
        display: block;
    }
    .MobileDate img
    {
        margin-left: 2px;
        cursor: pointer;
        margin-top: 6px;
    }
    .MobileDate input
    {
        margin-left: 0px;
        width: 72%;
        margin-top: 1px;
    }
    .SelectMobileDate
    {
        background-repeat: no-repeat;
        height: 20px;
        padding: 20px;
        position: absolute;
        left: 0px;
        top: 32px;
        width: 290px;
        display: none;
        z-index: 10000;
        text-align: left;
        cursor: pointer;
        border: #9dabb6 2px solid;
        background-color: White;
        font-size: 14px;
        font-weight: bold;
    }
    .year
    {
        width: 60px;
        margin-right: 8px;
    }
    .month
    {
        width: 40px;
        margin-left: 8px;
        margin-right: 11px;
    }
    .dateok
    {
        margin: 0px 0px 0px 15px;
        font-size: 15px;
        cursor: pointer;
        font-weight: bold;
    }
    .SelectMobileDate select
    {
        width: auto;
    }
    .std
    {
        color: Red;
    }
    
    #ErrorDiv
    {
         margin: 0 auto;
        width: 300px;
        position: absolute;
        left: 50%;
        top: 50%;
        display: none;
        border: 1px solid gray;
        background-color: Gray;
        padding: 5px;
        font-size: 14px;
        z-index: 1000001;
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
    #title
    {
        float: left;
        width: 290px;
        border-bottom: 1px solid Gray;
        font-weight: bold;
        cursor: pointer;
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
        display: none;
    }
    #msg
    {
        color:Black;
        float: left;
        width: 290px;
        padding: 5px;
         font-size:14px;
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
    #alertleft
    {
        color: #3d3d3d;
        float: left;
        width: 50%;
        text-align:center;
    }
    #alertright
    {
        color: red;
        float: right;
        width: 50%;
        text-align:center;
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
</style>
<table class="ca-workflow-form-table" id="TitleTable">
    <tr>
        <td colspan="4">
            <h3>
                Employee Expense Claim Form<br />
                员工报销申请表
            </h3>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Choose Employee<br />
            选择员工
            <asp:Button ID="btnPeopleInfo" runat="server" OnClick="btnPeopleInfo_Click" CausesValidation="False"
                CssClass="hidden" />
        </td>
        <td class="pftd value" colspan="3">
            <cc1:CAPeopleFinder ID="cpfUser" runat="server" AllowTypeIn="true" MultiSelect="false"
                CssClass="ca-people-finder" Width="200" />
        </td>
    </tr>
    <tr>
        <td class="w20">
            Dept<br />
            部门
        </td>
        <td class="w30">
            <asp:Label ID="lbDept" runat="server"></asp:Label>
        </td>
        <td class="w20">
            Requested By<br />
            申请人
        </td>
        <td class="w30">
            <asp:Label ID="lbRequestedBy" runat="server" CssClass="lbRequestedBy"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Claims Description<br />
            报销描述
        </td>
        <td colspan="3" class="ExpenseDescription">
            <QFL:FormField ID="ffExpenseDescription" runat="server" FieldName="ExpenseDescription"
                ControlMode="Edit" CssClass="ffExpenseDescription">
            </QFL:FormField>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table ca-workflow-form-table1" id="ExpenseTypetable">
    <tr>
        <td colspan="7">
            <h3>
                Employee expense claim details</h3>
        </td>
    </tr>
    <tr class="ExpenseTypeTitle">
        <td class="w5">
            <img src="../images/pixelicious_001.png" alt="Click to add the information." width="18"
                class="AddItem" />
        </td>
        <td class="w15">
            Expense Type<br />
            费用类别
        </td>
        <td class="w13">
            Date<br />
            日期
        </td>
        <td class="w30">
            Expense Purpose<br />
            费用用途
        </td>
        <td class="w15">
            Cost Center<br />
            成本中心
        </td>
        <td class="w12">
            Amount<br />
            金额(RMB)
        </td>
        <td class="w10">
            Com Std<br />
            公司标准
        </td>
    </tr>
    <tr class="ExpenseTypeItem Items">
        <td class="DelItem">
            <img src="../images/pixelicious_028.png" alt="Remove this information." width="18"
                class="DelItem" />
        </td>
        <td class="ExpenseType">
            <div class="ExpenseTypeContainer">
                <div class="ExpenseTypeSub">
                    <select class="ExpenseType" id="ExpenseType0">
                        <option class="blank" value=""></option>
                    </select></div>
            </div>
        </td>
        <td class="Date">
            <div class="MobileDate">
                <input type="text" class="Date" id="Date0" value="" />
                <img src="../../../CAResources/themeCA/images/selectcalendar.gif" align="absmiddle"
                    alt="Select Date" />
                <div class="SelectMobileDate">
                    Select Date：<select class="year">
                        <option>2010</option>
                        <option>2011</option>
                        <option>2012</option>
                        <option>2013</option>
                        <option>2014</option>
                        <option>2015</option>
                        <option>2016</option>
                        <option>2017</option>
                        <option>2018</option>
                        <option>2019</option>
                        <option>2020</option>
                    </select><select class="month">
                        <option>1</option>
                        <option>2</option>
                        <option>3</option>
                        <option>4</option>
                        <option>5</option>
                        <option>6</option>
                        <option>7</option>
                        <option>8</option>
                        <option>9</option>
                        <option>10</option>
                        <option>11</option>
                        <option>12</option>
                    </select><a class="dateok">[Done]</a>
                </div>
            </div>
        </td>
        <td class="ExpensePurpose">
            <div>
                <input type="text" class="ExpensePurpose" id="ExpensePurpose0" value="" /></div>
        </td>
        <td class="CostCenter">
            <div class="CostCenterContainer" >
                <div class="CostCenterSub">
                    <select class="CostCenter" id="CostCenter0">
                        <option class="blank" value=""></option>
                    </select></div>
            </div>
        </td>
        <td class="Amount">
            <div>
                <input type="text" class="Amount" id="Amount0" value="" /></div>
        </td>
        <td class="ComStd" id="ComStd0">
        </td>
    </tr>
    <tr class="Remark Items">
        <td colspan="2">
            Remark<br />
            备注
        </td>
        <td colspan="5">
            <div>
                <input type="text" class="Remark" id="Remark0" value="" /></div>
        </td>
    </tr>
    <tr class="None Items">
        <td style="height: 20px;" colspan="7">
        </td>
    </tr>
    <tr class="Insert">
        <td colspan="7">
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table summarytypetable">
    <tr>
        <td colspan="3">
            <h3>
                Expense Summary<br />
            </h3>
        </td>
    </tr>
    <tr>
        <td style="width: 295px">
            Expense Type<br />
            费用类别
        </td>
        <td style="width: 200px">
            Cost Center<br />
            成本中心
        </td>
        <td>
            Amount<br />
            金额(RMB)
        </td>
    </tr>
    <tr class="summarytype">
        <td colspan="3">
        </td>
    </tr>
    <tr>
        <td style="border-top: #ccc 2px solid">
            Total Amount<br />
            总金额(RMB)
        </td>
        <td style="border-top: #ccc 2px solid" colspan="2">
            <asp:Label ID="lbTotalAmount" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Cash Advance<br />
            员工借款(RMB)
        </td>
        <td style="height: 40px; text-align: left" align="center" colspan="2">
            <div class="cashadvancediv" style="left: 65px">
                <div class="titlediv" runat="server" id="titlediv">
                    Select Cash Advance to be deducted</div>
                <div class="cardiv" runat="server" id="cardiv">
                    
                </div>
            </div>
        </td>
    </tr>
    <tr>
        <td style="border-top: #ccc 2px solid">
            Payable to Employee/(Refund to Finance)<br />
            应付员工/(财务应收) (RMB)
        </td>
        <td style="border-top: #ccc 2px solid" colspan="2">
            <asp:Label ID="lbAmountDue" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table">
    <tr>
        <td style="width: 295px">
            Original Invoice Attached<br />
            附原发票
        </td>
        <td>
            <ul>
                <li>
                    <asp:RadioButton ID="rbAttInv" GroupName="AttachInvoice" Text="Yes" Checked="true"
                        runat="server" CssClass="radio" /></li>
                <li>
                    <asp:RadioButton ID="rbNonAttInv" GroupName="AttachInvoice" Text="No" runat="server"
                        CssClass="radio" /></li>
            </ul>
        </td>
    </tr>
    <tr>
        <td>
            Remark<br />
            备注
        </td>
        <td>
            <QFL:FormField ID="ffRemark" runat="server" FieldName="Remark" ControlMode="Edit">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Attachment<br />
            附件
        </td>
        <td class="attachment" style="text-align: left;">
            <QFL:FormAttachments runat="server" ID="attacthment">
            </QFL:FormAttachments>
        </td>
    </tr>
</table>
<div id="ErrorDiv">
    <div id="ErrorMsg">
        <div id="title">
            <div id="left">
                Notice</div>
            <div id="right">
                X</div>
        </div>
        <div id="msg">
        </div>
        <div id="alert">
            <div id="alertleft">
                <a onclick="SetAlert();" class="okbtn" style="color: #3d3d3d; font-size: 16px;">
                    Yes</a></div>
            <div id="alertright">
                <a style="color: Red; font-size: 16px;" class="nobtn" onclick="CloseAlert();">No</a></div>
        </div>
    </div>
</div>
<div id="bgDiv">
</div>
<div id="hiddenDIV">
<asp:HiddenField ID="hidSummaryExpenseType" runat="server" Value="" />
<asp:HiddenField ID="hidExpatriateBenefitForm" runat="server" Value="" />
<asp:HiddenField ID="hidCashAdvanceAmount" runat="server" Value="0" />
<asp:HiddenField ID="hidCashAdvanceID" runat="server" Value="" />
<asp:HiddenField ID="hidCashAdvanceIDAndAmount" runat="server" Value="" />
<asp:HiddenField ID="hidTotalAmount" runat="server" Value="0" />

<asp:HiddenField ID="hfCostCenter" runat="server" Value="" />
<asp:HiddenField ID="hfExpenseType" runat="server" Value="" />

<asp:HiddenField ID="hfOTMealStandard" runat="server" Value="0" />
<asp:HiddenField ID="hfMobileStandard" runat="server" Value="0" />

<input id="hfLocationType" type="hidden"  value=""/>

</div>

<script type="text/javascript" src="jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        BindSelectEvent();
        LoadExpenseTypeAndCostCenter();
        BindSelectDateEvent();
        BindRelatedSummaryEvent();
        BindDateEvent();
        BindAmountEvent();
        BindRemarkEvent();
        BindAddAndDelItemEvent();
        DrawExpatriateBenefitForm();
        BindCashAvanceEvent();
        BindPeopleFind();
    });

    function BindPeopleFind() {
        $('#<%=this.cpfUser.ClientID %>' + '_checkNames').click(function () {
            UpdateForm();
            $("#<%=this.btnPeopleInfo.ClientID %>").click();
        });
    }
</script>

<script type="text/javascript">
    var AddItemCount = 1;
    var DelItemCount = 0;
    function BindAddAndDelItemEvent() {
        var $AddItem = $("#ExpenseTypetable tr.ExpenseTypeTitle img.AddItem");
        var $DelItem = $("#ExpenseTypetable tr.ExpenseTypeItem img.DelItem");
        var $Insert = $("#ExpenseTypetable tr.Insert");
        $AddItem.bind("click", function () {
            ++AddItemCount;
            var $InsertItems1 = $Insert.prev();
            var $InsertItems2 = $InsertItems1.prev();
            var $InsertItems3 = $InsertItems2.prev();
            var $InsertItems3Clone = $InsertItems3.clone(true);
            var $InsertItems2Clone = $InsertItems2.clone(true);
            var $InsertItems1Clone = $InsertItems1.clone(true);
            SetExpenseTypeItemID($InsertItems3Clone, $InsertItems2Clone);
            $InsertItems3Clone.insertBefore($Insert);
            $InsertItems2Clone.insertBefore($Insert);
            $InsertItems1Clone.insertBefore($Insert);
            if (AddItemCount - DelItemCount <= 10) {
                DrawSummaryExpenseTable();
            } else {
                setTimeout(DrawSummaryExpenseTable, 0);
            }
        });
        $DelItem.live("click", function () {
            ++DelItemCount;
            if ($("#ExpenseTypetable tr.Items").length - 3 > 0) {
                var $items1 = $(this).parent().parent();
                var $items2 = $items1.next();
                var $items3 = $items2.next();
                $items1.remove();
                $items2.remove();
                $items3.remove();
                if (AddItemCount - DelItemCount <= 10) {
                    DrawSummaryExpenseTable();
                } else {
                    setTimeout(DrawSummaryExpenseTable, 0);
                }
            }
        });
    }

    function SetExpenseTypeItemID(InsertItems3Clone, InsertItems2Clone) {
        var id = AddItemCount - 1;
        InsertItems3Clone.find("select.ExpenseType").attr("id", "ExpenseType" + id);
        InsertItems3Clone.find("input.Date").attr("id", "Date" + id);
        //InsertItems3Clone.find("input.ExpensePurpose").val(AddItemCount);
        InsertItems3Clone.find("input.ExpensePurpose").attr("id", "ExpensePurpose" + id);
        InsertItems3Clone.find("select.CostCenter").attr("id", "CostCenter" + id);
        InsertItems3Clone.find("input.Amount").attr("id", "Amount" + id);
        InsertItems3Clone.find("td.ComStd").attr("id", "ComStd" + id);
        InsertItems2Clone.find("input.Remark").attr("id", "Remark" + id);
    }

    function BindRelatedSummaryEvent() {
        var $ExpenseType = $("#ExpenseTypetable tr.ExpenseTypeItem select.ExpenseType");
        var $CostCenter = $("#ExpenseTypetable tr.ExpenseTypeItem select.CostCenter");
        $ExpenseType.live("change", function () {
            ChangeMobileDate($(this));
            if (AddItemCount - DelItemCount <= 10) {
                DrawSummaryExpenseTable();
            } else {
                setTimeout(DrawSummaryExpenseTable, 0);
            }
        });
        $CostCenter.live("change", function () {
            if (AddItemCount - DelItemCount <= 10) {
                DrawSummaryExpenseTable();
            } else {
                setTimeout(DrawSummaryExpenseTable, 0);
            }
        });
    }

    function BindDateEvent() {
        var $Date = $("#ExpenseTypetable tr.ExpenseTypeItem td.Date");
        $Date.live("focus", function () {
            var $ExpenseType = $(this).prev().find("select.ExpenseType");
            if ($ExpenseType.val() == "") {
                alert("Please select expense type.");
                $(this).find("input").val("");
                $(this).find("input").blur();
                return false;
            }
        });
    }

    function BindAmountEvent() {
        var $Amount = $("#ExpenseTypetable tr.ExpenseTypeItem input.Amount");
        $Amount.live("blur", function () {
            //alert(1);
            if (isNaN($(this).val()) || $(this).val() < 0 || $(this).val() > 100000000) {
                $(this).val("0");
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                }
                alert('Please fill the valid number.');
            } else {
                $(this).parent().removeClass("wrapdiv");
            }
            if ($(this).val() == "" || $(this).val() == "0") {
                $(this).val("0")
            } else {
                var amount = Math.round($(this).val() * Math.pow(10, 2)) / Math.pow(10, 2);
                $(this).val(amount);

                var $parent = $(this).parent().parent().parent();
                var $next = $parent.next();
                var $expenseType = $parent.find("select.ExpenseType");
                var $ComStd = $parent.find("td.ComStd");
                var $remark = $next.find("input.Remark");
                if ($expenseType.val() == "Mobile – local call and others"
                    || $expenseType.val() == "Store mgnt exp - mobile phone local calls and others") {
                    var hfMobileStandard = $("#hiddenDIV input[id$='hfMobileStandard']").val();
                    if (hfMobileStandard != "no limit") {
                        if (amount > parseInt(hfMobileStandard)) {
                            $ComStd.addClass("std");
                            $next.show();
                            $remark.show().val("Why exceeds Company Standard?");
                        } else {
                            $ComStd.removeClass("std");
                            $next.hide();
                            $remark.val("");
                        }
                    } else {
                        $ComStd.removeClass("std");
                        $next.hide();
                        $remark.val("");
                    }
                }
                if ($expenseType.val() == "OT - meal allowance") {
                    if (amount > parseInt($("#hiddenDIV input[id$='hfOTMealStandard']").val())) {
                        $ComStd.addClass("std");
                        $next.show();
                        $remark.val("Why exceeds Company Standard?");
                    } else {
                        $ComStd.removeClass("std");
                        $next.hide();
                        $remark.val("");
                    }
                }
            }
            if (AddItemCount - DelItemCount <= 10) {
                DrawSummaryExpenseTable();
            } else {
                setTimeout(DrawSummaryExpenseTable, 0);
            }
            return false;
        });
        $Amount.live("change", function () {
            //alert(2);
            if (isNaN($(this).val()) || $(this).val() < 0 || $(this).val() > 100000000) {
                $(this).val("0");
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                }
                alert('Please fill the valid number.');
            } else {
                $(this).parent().removeClass("wrapdiv");
            }
            if ($(this).val() == "" || $(this).val() == "0") {
                $(this).val("0")
            } else {
                var amount = Math.round($(this).val() * Math.pow(10, 2)) / Math.pow(10, 2);
                $(this).val(amount);

                var $parent = $(this).parent().parent().parent();
                var $next = $parent.next();
                var $expenseType = $parent.find("select.ExpenseType");
                var $ComStd = $parent.find("td.ComStd");
                var $remark = $next.find("input.Remark");
                if ($expenseType.val() == "Mobile – local call and others"
                    || $expenseType.val() == "Store mgnt exp - mobile phone local calls and others") {
                    var hfMobileStandard = $("#hiddenDIV input[id$='hfMobileStandard']").val();
                    if (hfMobileStandard != "no limit") {
                        if (amount > parseInt(hfMobileStandard)) {
                            $ComStd.addClass("std");
                            $next.show();
                            $remark.show().val("Why exceeds Company Standard?");
                        } else {
                            $ComStd.removeClass("std");
                            $next.hide();
                            $remark.val("");
                        }
                    } else {
                        $ComStd.removeClass("std");
                        $next.hide();
                        $remark.val("");
                    }
                }
                if ($expenseType.val() == "OT - meal allowance") {
                    if (amount > parseInt($("#hiddenDIV input[id$='hfOTMealStandard']").val())) {
                        $ComStd.addClass("std");
                        $next.show();
                        $remark.show().val("Why exceeds Company Standard?");
                    } else {
                        $ComStd.removeClass("std");
                        $next.hide();
                        $remark.val("");
                    }
                }
            }
            if (AddItemCount - DelItemCount <= 10) {
                DrawSummaryExpenseTable();
            } else {
                setTimeout(DrawSummaryExpenseTable, 0);
            }
            return false;
        });
    }

    function BindRemarkEvent() {
        var $Remark = $("#ExpenseTypetable tr.Remark input.Remark");
        $Remark.live("focus", function () {
            var remark = $(this).val();
            if (remark == "Why exceeds Company Standard?"
                 || remark == "Internal function - list names of participants; External function - name of company"
                 || remark == "Internal function - list names of recipient; External function - name of company") {
                $(this).val("");
            }
        });
    }

</script>
<script type="text/javascript">
    function AppendHtml(expenseType, amount, costcenter) {
        var appendHtml = "";
        appendHtml = "<tr class=\"item\" ><td >" + expenseType + "</td><td >" + costcenter + "</td><td >" + amount + "</td></tr>";
        return appendHtml;
    }

    function CheckSummaryTypeHtml(expenseType, costcenter) {
        var result = false;
        var exp = ".summarytypetable tr:contains('" + expenseType + "')";
        var $summarytypehtml = $(exp);
        var $expcostcenter = $summarytypehtml.find("td:contains('" + costcenter + "')");
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
        var amount = 0;
        for (var i = 0; i < AddItemCount; i++) {
            var $expenseType = $("#ExpenseType" + i);
            if ($expenseType.length == 0) {
                continue;
            }
            if (expenseType == "Wireless/Mobile") {
                expenseType = "Mobile";
            }
            if (expenseType == "Store mgnt exp - Wireless/Mobile") {
                expenseType = "Store mgnt exp - mobile";
            }
            var $costCenter = $("#CostCenter" + i);
            var $amount = $("#Amount" + i);
            var et = $("#ExpenseType" + i + " option[value='" + $expenseType.val() + "']").text();
            if (et.indexOf(expenseType) != -1 && $costCenter.val().indexOf(costcenter) != -1 && et != "") {
                if ($amount.val() != "") {
                    amount += parseFloat($amount.val());
                }
            }
        }
        amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
        return amount;
    }

    function DrawSummaryExpenseTable() {
        $(".summarytypetable tr").remove(".item");
        var $summarytype = $(".summarytype");
        for (var i = 0; i < AddItemCount; i++) {
            var $expenseType = $("#ExpenseType" + i);
            if ($expenseType.length == 0) {
                continue;
            }
            if ($expenseType.val() != "") {
                var $amount = $("#Amount" + i);
                var expenseType = $("#ExpenseType" + i + " option[value='" + $expenseType.val() + "']").text();
                if (expenseType.indexOf("Mobile") != -1) {
                    expenseType = "Wireless/Mobile";
                }
                if (expenseType.indexOf("mobile") != -1) {
                    expenseType = "Store mgnt exp - Wireless/Mobile";
                }
                var costcenter = $("#CostCenter" + i).val();
                var result = CheckSummaryTypeHtml(expenseType, costcenter);
                if (isNaN($amount.val()) || $amount.val() < 0 || $amount.val() > 100000000) {
                    alert('Please fill the valid number.');
                    $amount.val("0");
                }
                var txtAmount = $amount.val() == "" ? "0" : $amount.val();
                txtAmount = Math.round(txtAmount * Math.pow(10, 2)) / Math.pow(10, 2);
                if (!result) {
                    var $html = $(AppendHtml(expenseType, txtAmount, costcenter));
                    $summarytype.before($html);
                } else {
                    UpdateSummaryExpenseTypeAmount(expenseType, txtAmount, costcenter);
                }
            }
        }
        CalTotalAmount();
    }

    function UpdateSummaryExpenseType() {
        var $hidSummaryExpenseType = $('#<%= this.hidSummaryExpenseType.ClientID %>');
        $hidSummaryExpenseType.val("");
        var summaryExpenseType = "[";
        var $summarytypetable = $(".summarytypetable tr.item");
        if ($summarytypetable.length > 0) {
            $summarytypetable.each(function () {
                summaryExpenseType += "{name:'" + $(this).find("td").eq(0).text() + "',val:'" + $(this).find("td").eq(2).text() + "',costcenter:'" + $(this).find("td").eq(1).text() + "'},";
            });
            summaryExpenseType += "]";
            $hidSummaryExpenseType.val(summaryExpenseType);
        }
    }

    String.prototype.replaceAll = function (s1, s2) {
        return this.replace(new RegExp(s1, "gm"), s2);
    }

    function UpdateExpatriateBenefitForm() {
        var $hidExpatriateBenefitForm = $("#<%=this.hidExpatriateBenefitForm.ClientID%>");
        $hidExpatriateBenefitForm.val("");
        var expatriateBenefitForm = "[";
        for (var i = 0; i < AddItemCount; i++) {
            var $expenseType = $("#ExpenseType" + i);
            if ($expenseType.length == 0) {
                continue;
            }
            var expensePurpose = $("#ExpensePurpose" + i).val().replaceAll("\'", "\\\'")
                                                                .replaceAll(",", "，")
                                                                .replaceAll("<", "&lt;")
                                                                .replaceAll(">", "&gt;");
            var remark = $("#Remark" + i).val().replaceAll("\'", "\\\'")
                                               .replaceAll(",", "，")
                                               .replaceAll("<", "&lt;")
                                               .replaceAll(">", "&gt;");
            expatriateBenefitForm += "{" +
                                            "BenefitType:'" + $expenseType.val() + "'," +
                                            "Date:'" + $("#Date" + i).val() + "'," +
                                            "ExpensePurpose:'" + expensePurpose + "'," +
                                            "CostCenter:'" + $("#CostCenter" + i).val() + "'," +
                                            "Amount:'" + $("#Amount" + i).val() + "'," +
                                            "ComStd:'" + $("#ComStd" + i).html() + "'," +
                                            "Remark:'" + remark + "'" +
                                     "},";
        }
        expatriateBenefitForm += "]";
        $hidExpatriateBenefitForm.val(expatriateBenefitForm); 
    }

    function DrawExpatriateBenefitForm() {
        var $Insert = $("#ExpenseTypetable tr.Insert");
        var $hidExpatriateBenefitForm = $('#<%= this.hidExpatriateBenefitForm.ClientID %>');
        var json = $hidExpatriateBenefitForm.val();
        if (json != "" && json != "[]") {
            var expatriateBenefitForm = eval("(" + json + ")");
            $.each(expatriateBenefitForm, function (i, item) {
                if (item != undefined) {
                    var $InsertItems1 = $Insert.prev();
                    var $InsertItems2 = $InsertItems1.prev();
                    var $InsertItems3 = $InsertItems2.prev();
                    var $ExpenseTypeItem = $InsertItems3;
                    var $Remark = $InsertItems2;
                    if (i != 0) {
                        ++AddItemCount;
                        var $InsertItems3Clone = $InsertItems3.clone(true);
                        var $InsertItems2Clone = $InsertItems2.clone(true);
                        var $InsertItems1Clone = $InsertItems1.clone(true);
                        SetExpenseTypeItemID($InsertItems3Clone, $InsertItems2Clone);
                        $InsertItems3Clone.insertBefore($Insert);
                        $InsertItems2Clone.insertBefore($Insert);
                        $InsertItems1Clone.insertBefore($Insert);
                        $ExpenseTypeItem = $InsertItems3Clone;
                        $Remark = $InsertItems2Clone;
                    }
                    SetExpenseTypeItemValue($ExpenseTypeItem, item.BenefitType
                                                     , item.Date
                                                     , item.ExpensePurpose
                                                     , item.CostCenter
                                                     , item.Amount
                                                     , item.ComStd);
                    SetRemarkItemsValue($Remark, item.Remark);
                }
            });
            if (AddItemCount - DelItemCount <= 10) {
                DrawSummaryExpenseTable();
            } else {
                setTimeout(DrawSummaryExpenseTable, 0);
            }
        }
    }

    function SetExpenseTypeItemValue(ExpenseTypeItem, BenefitType, Date, ExpensePurpose, CostCenter, Amount, ComStd) {
        var $BenefitType = ExpenseTypeItem.find("select.ExpenseType");
        var $Date = ExpenseTypeItem.find("input.Date");
        var $ExpensePurpose = ExpenseTypeItem.find("input.ExpensePurpose");
        var $CostCenter = ExpenseTypeItem.find("select.CostCenter");
        var $Amount = ExpenseTypeItem.find("input.Amount");
        var $ComStd = ExpenseTypeItem.find("td.ComStd");
        $BenefitType.val(BenefitType);
        $Date.val(Date);
        $ExpensePurpose.val(ExpensePurpose);
        $CostCenter.val(CostCenter);
        $Amount.val(Amount);
        $ComStd.html(ComStd);
        if (ComStd != "no limit" && ComStd != "") {
            if (parseFloat(Amount) > parseFloat(ComStd)) {
                $ComStd.addClass("std");
            }
        }
    }

    function SetRemarkItemsValue(Remark, RemarkText) {
        var $Remark = Remark.find("input.Remark");
        $Remark.val(RemarkText);
        var $parent = $Remark.parent().parent().parent();
        if (RemarkText != "") {
            $parent.show();
            $parent.find("input.Remark").show();
        } else {
            $parent.hide();
        }
    }

</script>
<script type="text/javascript">
    function BindCashAvanceEvent() {
        var $titlediv = $("table.summarytypetable div.titlediv");
        var $cardiv = $("table.summarytypetable div.cardiv");
        $titlediv.click(function () {
            if ($titlediv.html() == "No Cash Advance") {
                return false;
            }
            if ($cardiv.css("display") == "block") {
                $cardiv.slideUp("slow");
                $titlediv.removeClass("titledivbg");
                $cardiv.removeClass("cardivscroll");
            } else {
                var $carul = $("table.summarytypetable div.cardiv ul li");
                if ($carul.length > 12) {
                    $cardiv.height(364);
                    $cardiv.addClass("cardivscroll");
                }
                $cardiv.slideDown("slow");
                $titlediv.addClass("titledivbg");
            }
        });
        var $carli = $("table.summarytypetable div.cardiv ul li");
        $carli.each(function () {
            $(this).mousemove(function () {
                $(this).addClass("carli");
            });
            $(this).mouseout(function () {
                $(this).removeClass("carli");
            });
        });
        var $carliinput = $("table.summarytypetable div.cardiv ul li input");
        $carliinput.each(function () {
            $(this).click(function () {
                CalCashAdvance();
                CalTotalAmount();
            });
        });
    }

    function CalCashAdvance() {
        var $hidCashAdvanceID = $('#<%= this.hidCashAdvanceID.ClientID %>');
        var $hidCashAdvanceAmount = $('#<%= this.hidCashAdvanceAmount.ClientID %>');
        var $hidCashAdvanceIDAndAmount = $('#<%= this.hidCashAdvanceIDAndAmount.ClientID %>');
        var $titlediv = $("table.summarytypetable div.titlediv");
        var $carliinput = $("table.summarytypetable div.cardiv ul li input");
        var num = 0;
        var workflownumber = "";
        var workflownumberandamount = "";
        $carliinput.each(function () {
            if ($(this).attr("checked")) {
                num += parseFloat($(this).val());
                workflownumber += $(this).attr("title") + ";";
                workflownumberandamount += $(this).parent().text() + ";";
            }
        });
        if (num != 0) {
            $titlediv.html(num);
            $titlediv.css("textAlign", "center");
        } else {
            $titlediv.html("Select Cash Advance to be deducted");
            $titlediv.css("textAlign", "left");
        }
        $hidCashAdvanceAmount.val(num);
        $hidCashAdvanceID.val(workflownumber);
        $hidCashAdvanceIDAndAmount.val(workflownumberandamount);
    }

</script>
<script type="text/javascript">
    function CalTotalAmount() {
        var $hidCashAdvanceAmount = $('#<%= this.hidCashAdvanceAmount.ClientID %>');
        var $hidTotalAmount = $('#<%= this.hidTotalAmount.ClientID %>');
        var $lbTotalAmount = $('#<%= this.lbTotalAmount.ClientID %>');
        var $lbAmountDue = $('#<%= this.lbAmountDue.ClientID %>');
        var cashAdvanceAmount = parseFloat($hidCashAdvanceAmount.val());
        //var $Amount = $("#ExpenseTypetable tr.ExpenseTypeItem input.Amount");
        var totalAmount = 0;
        var amountDue = 0;
        //        $Amount.each(function () {
        //            var amount = $(this).val();
        //            if (isNaN(amount) || amount < 0 || amount > 100000000) {
        //                amount = 0;
        //            }
        //            amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
        //            totalAmount += amount;
        //        });

        for (var i = 0; i < AddItemCount; i++) {
            var $amount = $("#Amount" + i);
            if ($amount.length == 0) {
                continue;
            }
            var amount = $amount.val();
            if (isNaN(amount) || amount < 0 || amount > 100000000) {
                amount = 0;
            }
            amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
            totalAmount += amount;
        }

        totalAmount = Math.round(totalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        amountDue = totalAmount - cashAdvanceAmount;
        amountDue = Math.round(amountDue * Math.pow(10, 2)) / Math.pow(10, 2);
        $lbTotalAmount.text(totalAmount);
        $lbAmountDue.text(amountDue);
        $hidTotalAmount.val(totalAmount);
    } 

</script>
<script type="text/javascript">
    function CheckSubmit() {
        CreateForbidDIV();

        $("#TitleTable .wrapdiv").removeClass("wrapdiv");
        $("#ExpenseTypetable div.wrapdiv").removeClass("wrapdiv");

        var result = true;
        var msg = "";

        var $errmsg = $("#TitleTable td.pftd span:contains('No exact match was found.')");
        if ($errmsg.length > 0) {
            msg += "Please choose employee. \n";
            if (!$("td.pftd").find("span.ca-people-finder").hasClass("wrapdiv")) {
                $("td.pftd").find("span.ca-people-finder").addClass("wrapdiv");
            }
            result = false;
        }

        var $ExpenseDescription = $("#TitleTable td.ExpenseDescription input");
        if ($ExpenseDescription.val() == "") {
            msg += "Please fill in the ExpenseDescription.\n";
            if (!$ExpenseDescription.parent().hasClass("wrapdiv")) {
                if ($ExpenseDescription.parent().is("div")) {
                    $ExpenseDescription.parent().addClass("wrapdiv");
                } else {
                    $ExpenseDescription.wrap("<div class=\"wrapdiv\"></div>");
                }
            }
            result = false;
        }

        var $expenseType = $("#ExpenseTypetable tr.ExpenseTypeItem select.ExpenseType");
        $expenseType.each(function () {
            if ($.trim($(this).val()) == "") {
                msg += "Please Select Expense Type.\n";
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                }
                result = false;
            }
            var expense = $(this).val();
            var $costc = $(this).parent().parent().parent().parent().find("select.CostCenter");
            if (expense.indexOf("Store") == 0) {
                if ($costc.val().indexOf("S") != 0) {
                    msg += "Please Select Store CostCenter.\n";
                    if (!$costc.parent().hasClass("wrapdiv")) {
                        $costc.parent().addClass("wrapdiv");
                    }
                    result = false;
                }
            } else {
                if ($costc.val().indexOf("S") == 0) {
                    msg += "The selected expense type and cost center do not match.\n";
                    if (!$costc.parent().hasClass("wrapdiv")) {
                        $costc.parent().addClass("wrapdiv");
                    }
                    result = false;
                }
            }
        });

        var $date = $("#ExpenseTypetable tr.ExpenseTypeItem input.Date");
        $date.each(function () {
            if ($.trim($(this).val()) == "") {
                msg += "Please fill in the Date.\n";
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                }
                result = false;
            }
        });

        var $expensePurpose = $("#ExpenseTypetable tr.ExpenseTypeItem input.ExpensePurpose");
        $expensePurpose.each(function () {
            if ($.trim($(this).val()) == "") {
                msg += "Please fill in the Expense Purpose. \n";
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                }
                result = false;
            }
        });

        var $costCenter = $("#ExpenseTypetable tr.ExpenseTypeItem select.CostCenter");
        $costCenter.each(function () {
            if ($.trim($(this).val()) == "") {
                msg += "Please select CostCenter.\n";
                if (!$(this).parent().parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                }
                result = false;
            }
        });


        var $amount = $("#ExpenseTypetable tr.ExpenseTypeItem input.Amount");
        $amount.each(function () {
            if ($.trim($(this).val()) == "") {
                msg += "Please fill in the Amount.\n";
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                }
                result = false;
            }
        });

        var $remark = $("#ExpenseTypetable tr.Remark input.Remark");
        $remark.each(function () {
            if ($(this).parent().parent().is(":visible")) {
                if ($.trim($(this).val()) == "") {
                    msg += "Please fill in the Remark. \n";
                    if (!$(this).parent().hasClass("wrapdiv")) {
                        $(this).parent().addClass("wrapdiv");
                    }
                    result = false;
                }
            }
        });

        if (msg != "") {
            alert(msg);
        }

        if (!result) {
            ClearForbidDIV();
        }

        if (result) {
            UpdateForm();
        }

        //return false;
        return result;
    }

    function UpdateForm() {
        UpdateExpatriateBenefitForm();
        DrawSummaryExpenseTable();
        UpdateSummaryExpenseType();
    }

</script>

<script type="text/javascript">
    function GetOption(text, val) {
        var option = "";
        option = "<option class=\"item\" value=\"" + text + "\" >" + val + "</option>";
        return option;
    }

    function LoadExpenseType() {
        var $Insert = $("#ExpenseTypetable tr.ExpenseTypeItem select.ExpenseType option.blank");
        var $hfExpenseType = $('#<%= this.hfExpenseType.ClientID %>');
        var json = $hfExpenseType.val();
        if (json != "") {
            var expenseType = eval("(" + json + ")");
            var $blank = $("<option class=\"item\" value=\"\" ></option>");
            $Insert.before($blank);
            $.each(expenseType, function (i, item) {
                if (item != undefined) {
                    $InsertOption = $(GetOption(item.val, item.name));
                    $Insert.before($InsertOption);
                }
            });
            var $option = $("#ExpenseTypetable tr.ExpenseTypeItem select.ExpenseType option").eq(0);
            $("#ExpenseTypetable tr.ExpenseTypeItem select.ExpenseType option").remove(".blank");
        }
    }

    function LoadCostCenter() {
        var $Insert = $("#ExpenseTypetable tr.ExpenseTypeItem select.CostCenter option.blank");
        var $hfCostCenter = $('#<%= this.hfCostCenter.ClientID %>');
        var json = $hfCostCenter.val();
        if (json != "") {
            var costCenter = eval("(" + json + ")");
            var $blank = $("<option class=\"item\" value=\"\" ></option>");
            $Insert.before($blank);
            $.each(costCenter, function (i, item) {
                if (item != undefined) {
                    $InsertOption = $(GetOption(item.val, item.name));
                    $Insert.before($InsertOption);
                }
            });
            var $option = $("#ExpenseTypetable tr.ExpenseTypeItem select.CostCenter option").eq(0);
            $("#ExpenseTypetable tr.ExpenseTypeItem select.CostCenter option").remove(".blank");
        }
    }

    function LoadExpenseTypeAndCostCenter() {
        LoadExpenseType();
        LoadCostCenter();
    }

</script>
<script type="text/javascript">
    function BindSelectEvent() {
        var $ExpenseType = $("#ExpenseTypetable tr.ExpenseTypeItem div.ExpenseTypeSub");
        $ExpenseType.bind("mousemove",function () {
            $(this).addClass("ExpenseTypeTD");
        });
        $ExpenseType.bind("mouseout",function () {
            $(this).removeClass("ExpenseTypeTD");
        });
        var $childrenExpenseType = $ExpenseType.find("select.ExpenseType");
        $childrenExpenseType.bind("click", function () {
            var $parent = $(this).parent();
            $parent.addClass("ExpenseTypeTD");
            $parent.unbind("mouseout");
        });
        $childrenExpenseType.bind("blur", function () {
            var $parent = $(this).parent();
            $parent.removeClass("ExpenseTypeTD");
            $parent.bind("mouseout", function () {
                $(this).removeClass("ExpenseTypeTD"); 
            });
        });

        var $CostCenter = $("#ExpenseTypetable tr.ExpenseTypeItem div.CostCenterSub");
        $CostCenter.bind("mousemove", function () {
            $(this).addClass("CostCenterTD");
        });
        $CostCenter.bind("mouseout", function () {
            $(this).removeClass("CostCenterTD");
        });
        var $childrenCostCenter = $CostCenter.find("select.CostCenter");
        $childrenCostCenter.bind("click", function () {
            var $parent = $(this).parent();
            $parent.addClass("CostCenterTD");
            $parent.unbind("mouseout");
        });
        $childrenCostCenter.bind("blur", function () {
            var $parent = $(this).parent();
            $parent.removeClass("CostCenterTD");
            $parent.bind("mouseout", function () {
                $(this).removeClass("CostCenterTD");
            });
        });
    }

    function BindSelectDateEvent() {
        var $MobileDate = $("#ExpenseTypetable tr.ExpenseTypeItem td.Date div.MobileDate img");
        $MobileDate.bind("click", function () {
            var $expenseType = $(this).parent().parent().parent().find("select.ExpenseType");
            if ($expenseType.val() == "Mobile – local call and others"
           || $expenseType.val() == "Mobile – long distance call") {
                var $SelectMobileDate = $(this).parent().find("div.SelectMobileDate");
                if ($SelectMobileDate.is(":visible")) {
                    $SelectMobileDate.hide();
                } else {
                    $SelectMobileDate.show();
                }
            } else {
                var $dateText = $(this).prev(); 
                new Calendar().show($dateText[0]);
            }
        });
        var $Select = $("#ExpenseTypetable tr.ExpenseTypeItem td.Date div.MobileDate a");
        $Select.bind("click", function () {
            var $DateDiv = $(this).parent();
            var $year = $DateDiv.find(".year");
            var $month = $DateDiv.find(".month");
            var $txtdate = $DateDiv.prev().prev();
            $txtdate.val($month.val() + "/" + $year.val());
            $DateDiv.hide();
        });

        var $year = $("#ExpenseTypetable tr.ExpenseTypeItem td.Date div.MobileDate select.year");
        var $month = $("#ExpenseTypetable tr.ExpenseTypeItem td.Date div.MobileDate select.month");
        var date = new Date();
        $year.val(date.getFullYear());
        $month.val(date.getMonth()+1);
    }
</script>
<script type="text/javascript">
    function ChangeMobileDate(obj) {
        if (ChangeLocation(obj)) {
            return false;
        };
        var $expenseType = obj;
        var $parent = $expenseType.parent().parent().parent().parent();
        var $next = $parent.next();
        var $remark = $next.find("input.Remark");
        var $date = $parent.find("input.Date");
        var $std = $parent.find("td.ComStd");
        var date = $date.val();
        var hfMobileStandard = $("#hiddenDIV input[id$='hfMobileStandard']").val();
        var hfOTMealStandard = $("#hiddenDIV input[id$='hfOTMealStandard']").val();
        if (date != "") {
            if ($expenseType.val() == "Mobile – local call and others"
                || $expenseType.val() == "Mobile – long distance call") {
                var month = date.substring(0, date.indexOf("/"));
                var year = date.substring(date.lastIndexOf("/") + 1);
                $date.val(month + "/" + year);
            } else {
                if (date.indexOf("/") == date.lastIndexOf("/")) {
                    var month = date.substring(0, date.indexOf("/"));
                    var year = date.substring(date.lastIndexOf("/") + 1);
                    var curDate = new Date();
                    $date.val(month + "/" + curDate.getDate() + "/" + year);
                }
            }
        }
        $std.html(""); 
        if ($expenseType.val() == "OT - meal allowance") {
            $std.html(hfOTMealStandard);
        }
        if ($expenseType.val() == "Mobile – local call and others"
            || $expenseType.val() == "Store mgnt exp - mobile phone local calls and others") {
            $std.html(hfMobileStandard);
        }
        $next.hide();
        $remark.val("");
        var $Amount = $parent.find("input.Amount");
        if ($expenseType.val() == "Mobile – local call and others"
                    || $expenseType.val() == "Store mgnt exp - mobile phone local calls and others") {
            if (hfMobileStandard != "no limit") {
                if (parseFloat($Amount.val()) > parseInt(hfMobileStandard)) {
                    $std.addClass("std");
                    $next.show();
                    $remark.show().val("Why exceeds Company Standard?");
                } else {
                    $std.removeClass("std");
                    $remark.val("");
                }
            } else {
                $std.removeClass("std");
                $remark.val("");
            }
        }
        if ($expenseType.val() == "OT - meal allowance") {
            if (parseFloat($Amount.val()) > parseInt(hfOTMealStandard)) {
                $std.addClass("std");
                $next.show();
                $remark.show().val("Why exceeds Company Standard?");
            } else {
                $std.removeClass("std");
                $remark.val("");
            }
        }

        if ($expenseType.val() == "Entertainment - food"
            || $expenseType.val() == "Store exp - entertainment food") {
            $next.show();
            $remark.show().val("Internal function - list names of participants; External function - name of company");
        }
        if ($expenseType.val() == "Entertainment - gift"
            || $expenseType.val() == "Store exp - entertainment gift") {
            $next.show();
            $remark.show().val("Internal function - list names of recipient; External function - name of company");
        }

    }
</script>
<script type="text/javascript">
    function ChangeLocation(obj) {
        var result = false;
        var $obj = $(obj); 
        var $hfLocationType = $("input[id='hfLocationType']");
        if ($hfLocationType.val() == "" && $obj.val().toLowerCase().indexOf("travel") != -1) {
            var alertMsg = "";
            result = true;
            AlertSelect("Is this travel expense related to business trip applied through Travel Request EWF ? ", getScrollTop(), $obj.offset().left);
        }
        return result;
    }

    function getScrollTop() {
        var scrollTop = 0;
        if (document.documentElement && document.documentElement.scrollTop) {
            scrollTop = document.documentElement.scrollTop;
        }
        else if (document.body) {
            scrollTop = document.body.scrollTop;
        }
        return scrollTop;
    } 

    function AlertSelect(msg,top1,left1) {
        $("#bgDiv").css({
            height: function () {
                return $(document).height();
            },
            width: "100%"

        });
        $("#bgDiv").show();
        $("#msg").append(msg);
        $("#ErrorDiv").css({ top: top1+100, left: left1+200 }).fadeIn(1000);
    }

    function CloseAlert() {
        var $hfLocationType = $("input[id='hfLocationType']");
        $hfLocationType.val("Travel");
        $("#ErrorDiv").hide();
        $("#bgDiv").hide();
        return false;
    }

    function SetAlert() {
        var text = $("#msg").html();
        if (text.indexOf("otherwise") == -1) {
            $("a.okbtn").html("OK");
            $("a.nobtn").html("Back");
            $("a.nobtn").hide();
            $("#alertleft").css("width", "88%");
            $("#alertleft").css("textAlign", "right");

            $("#alertright").css("width", "0");
            $("#msg").html("");
            $("#msg").append("Please file the expense using Travel Expense EWF in Task History – Travel Request & Claim, otherwise, Finance will reject your claim.");
        } else {
            CloseAlert();
        }
    }

</script>
