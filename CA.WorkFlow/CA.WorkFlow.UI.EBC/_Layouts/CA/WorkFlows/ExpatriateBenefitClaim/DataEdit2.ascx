<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit2.ascx.cs" Inherits="CA.WorkFlow.UI.EBC.DataEdit2" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
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
    .w43
    {
        width: 43%;
    }
    .w22
    {
        width: 22%;
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
    .Insert
    {
        display: none;
    }
    div
    {
        width: auto;
        height: auto;
    }
    td.Date
    {
        display:none;    
    }
</style>
<table class="ca-workflow-form-table" id="TitleTable">
    <tr>
        <td colspan="4">
            <h3>
                Expatriate Benefit Claim Form<br />
                外籍员工福利报销申请单
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
            Benefit Claim Description<br />
            福利报销描述
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
        <td colspan="6">
            <h3>
                Expatriate benefit claim details</h3>
        </td>
    </tr>
    <tr class="ExpenseTypeTitle">
        <td class="w5">
            <img src="../images/pixelicious_001.png" alt="Click to add the information." width="18"
                class="AddItem" />
        </td>
        <td class="w20">
            Benefit Type<br />
            福利类别
        </td>
        <td class="w15 Date">
            Date<br />
            日期
        </td>
        <td class="w30">
            Expense Detail Purpose<br />
            费用详细用途
        </td>
        <td class="w20">
            CostCenter<br />
            成本中心
        </td>
        <td class="w10">
            Amount<br />
            金额(RMB)
        </td>
    </tr>
    <tr class="ExpenseTypeItem Items">
        <td class="DelItem">
            <img src="../images/pixelicious_028.png" alt="Remove this information." width="18"
                class="DelItem" />
        </td>
        <td class="ExpenseType">
            <div>
                <select class="ExpenseType" id="ExpenseType0">
                    <option value=""></option>
                    <option value="Child education">Child education</option>
                    <option value="House rental">House rental</option>
                    <option value="Car related">Car related</option>
                    <option value="Medical assistance">Medical assistance</option>
                    <option value="Others (please specify)">Others (please specify)</option>
                </select></div>
        </td>
        <td class="Date">
            <div>
                <input type="text" class="Date" id="Date0" value="0" /></div>
        </td>
        <td class="ExpensePurpose">
            <div>
                <input type="text" class="ExpensePurpose" id="ExpensePurpose0" value="" /></div>
        </td>
        <td class="CostCenter">
            <div>
                <select class="CostCenter" id="CostCenter0">
                    <option value=""></option>
                    <option value="H1013215">H1013215 - Central Services</option>
                    <option value="H1012219">H1012219 - BMM Expats Homeleave</option>
                </select></div>
        </td>
        <td class="Amount">
            <div>
                <input type="text" class="Amount" id="Amount0" value="" /></div>
        </td>
    </tr>
    <tr class="Remark Items">
        <td colspan="2">
            Remark<br />
            备注
        </td>
        <td colspan="4">
            <div>
                <input type="text" class="Remark" id="Remark0" value="0" /></div>
        </td>
    </tr>
    <tr class="None Items">
        <td style="height: 20px;" colspan="6">
        </td>
    </tr>
    <tr class="Insert">
        <td colspan="6">
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table summarytypetable">
    <tr>
        <td colspan="3">
            <h3>
                Benefit Summary<br />
            </h3>
        </td>
    </tr>
    <tr>
        <td style="width: 295px">
            Benefit Type<br />
            福利类别
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
<asp:HiddenField ID="hidSummaryExpenseType" runat="server" Value="" />
<asp:HiddenField ID="hidExpatriateBenefitForm" runat="server" Value="" />
<asp:HiddenField ID="hidCashAdvanceAmount" runat="server" Value="0" />
<asp:HiddenField ID="hidCashAdvanceID" runat="server" Value="" />
<asp:HiddenField ID="hidCashAdvanceIDAndAmount" runat="server" Value="" />
<asp:HiddenField ID="hidTotalAmount" runat="server" Value="0" />
<script type="text/javascript" src="jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        BindRelatedSummaryEvent();
        BindDateEvent();
        BindAmountEvent();
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
        InsertItems2Clone.find("input.Remark").attr("id", "Remark" + id);
    }

    function BindRelatedSummaryEvent() {
        var $ExpenseType = $("#ExpenseTypetable tr.ExpenseTypeItem select.ExpenseType");
        var $CostCenter = $("#ExpenseTypetable tr.ExpenseTypeItem select.CostCenter");
        $ExpenseType.live("change", function () {
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
        var $Date = $("#ExpenseTypetable tr.ExpenseTypeItem input.Date");
        $Date.live("focus", function () {
            var $ExpenseType = $(this).parent().parent().prev().find("select.ExpenseType");
            if ($ExpenseType.val() == "") {
                alert("Please select benefit type.");
                $(this).val("");
                $(this).blur();
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
            }
            if (AddItemCount - DelItemCount <= 10) {
                DrawSummaryExpenseTable();
            } else {
                setTimeout(DrawSummaryExpenseTable, 0);
            }
            return false;
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
        //var $ExpenseType = $("#ExpenseTypetable tr.ExpenseTypeItem select.ExpenseType");
        var amount = 0;
        //var et = expenseType;
//        $ExpenseType.each(function () {
//            var costcenterval = $(this).parent().parent().parent().find("select.CostCenter").val();
//            if ($(this).val().indexOf(et) != -1 && costcenterval.indexOf(costcenter) != -1 && $(this).val() != "") {
//                var $amount = $(this).parent().parent().parent().find("input.Amount");
//                if ($amount.val() != "") {
//                    amount += parseFloat($amount.val());
//                }
//            }
//        });

        for (var i = 0; i < AddItemCount; i++) {
            var $expenseType = $("#ExpenseType" + i);
            if ($expenseType.length == 0) {
                continue;
            }
            var $costCenter = $("#CostCenter" + i);
            var $amount = $("#Amount" + i);
            if ($expenseType.val().indexOf(expenseType) != -1 && $costCenter.val().indexOf(costcenter) != -1 && $expenseType.val() != "") {
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
//        var $expenseType = $("#ExpenseTypetable tr.ExpenseTypeItem select.ExpenseType");
//        $expenseType.each(function () {
//            if ($(this).val() != "") {
//                var $amount = $(this).parent().parent().parent().find("input.Amount");
//                var expenseType = $(this).val();
//                var costcenter = $(this).parent().parent().parent().find("select.CostCenter").val();
//                var result = CheckSummaryTypeHtml(expenseType, costcenter);
//                if (isNaN($amount.val())) {
//                    alert('Please fill the valid number.');
//                    $amount.val("0");
//                }
//                if ($amount.val() < 0) {
//                    alert('Please fill the valid number.');
//                    $amount.val("0");
//                }
//                if ($amount.val() > 100000000) {
//                    alert('Please fill the number of less than 100000000.');
//                    $amount.val("0");
//                }
//                var txtAmount = $amount.val() == "" ? "0" : $amount.val();
//                txtAmount = Math.round(txtAmount * Math.pow(10, 2)) / Math.pow(10, 2);
//                if (!result) {
//                    var $html = $(AppendHtml(expenseType, txtAmount, costcenter));
//                    $summarytype.before($html);
//                } else {
//                    UpdateSummaryExpenseTypeAmount(expenseType, txtAmount, costcenter);
//                }
//            }
        //        });
        for (var i = 0; i < AddItemCount; i++) {
            var $expenseType = $("#ExpenseType" + i);
            if ($expenseType.length == 0) {
                continue;
            }
            if ($expenseType.val() != "") {
                var $amount = $("#Amount" + i);
                var expenseType = $expenseType.val();
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
        //$("td.temp1").html($hidSummaryExpenseType.val());
    }

    String.prototype.replaceAll = function (s1, s2) {
        return this.replace(new RegExp(s1, "gm"), s2);
    }

    function UpdateExpatriateBenefitForm() {
        //var $ExpenseTypeItem = $("#ExpenseTypetable tr.ExpenseTypeItem select.ExpenseType");
        var $hidExpatriateBenefitForm = $("#<%=this.hidExpatriateBenefitForm.ClientID%>");
        $hidExpatriateBenefitForm.val("");
        var expatriateBenefitForm = "[";
//        $ExpenseTypeItem.each(function () {
//            expatriateBenefitForm += "{" +
//                                            "BenefitType:'" + $(this).val() + "'," +
//                                            "Date:'" + $(this).parent().parent().next().find("input.Date").val() + "'," +
//                                            "ExpensePurpose:'" + $(this).parent().parent().parent().find("input.ExpensePurpose").val() + "'," +
//                                            "CostCenter:'" + $(this).parent().parent().parent().find("select.CostCenter").val() + "'," +
//                                            "Amount:'" + $(this).parent().parent().parent().find("input.Amount").val() + "'," +
//                                            "Remark:'" + $(this).parent().parent().parent().next().find("input.Remark").val() + "'" +
//                                     "},";
        //        });

        for (var i = 0; i < AddItemCount; i++) {
            var $expenseType = $("#ExpenseType" + i);
            if ($expenseType.length == 0) {
                continue;
            }
            var expensePurpose = $("#ExpensePurpose" + i).val().replaceAll("\'", "\\\'").replaceAll(",", "，")
                                                               .replaceAll("<", "&lt;")
                                                               .replaceAll(">", "&gt;");
            expatriateBenefitForm += "{" +
                                            "BenefitType:'" + $expenseType.val() + "'," +
                                            "Date:'" + $("#Date" + i).val() + "'," +
                                            "ExpensePurpose:'" + expensePurpose + "'," +
                                            "CostCenter:'" + $("#CostCenter" + i).val() + "'," +
                                            "Amount:'" + $("#Amount" + i).val() + "'," +
                                            "Remark:'" + $("#Remark" + i).val() + "'" +
                                     "},";
        }



        expatriateBenefitForm += "]";
        $hidExpatriateBenefitForm.val(expatriateBenefitForm);
        //$("td.temp2").html($hidExpatriateBenefitForm.val()); 
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
                                                     , item.Amount);
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

    function SetExpenseTypeItemValue(ExpenseTypeItem, BenefitType, Date, ExpensePurpose, CostCenter, Amount) {
        var $BenefitType = ExpenseTypeItem.find("select.ExpenseType");
        var $Date = ExpenseTypeItem.find("input.Date");
        var $ExpensePurpose = ExpenseTypeItem.find("input.ExpensePurpose");
        var $CostCenter = ExpenseTypeItem.find("select.CostCenter");
        var $Amount = ExpenseTypeItem.find("input.Amount");
        $BenefitType.val(BenefitType);
        $Date.val(Date); 
        $ExpensePurpose.val(ExpensePurpose);
        $CostCenter.val(CostCenter);
        $Amount.val(Amount);
    }

    function SetRemarkItemsValue(Remark, RemarkText) {
        var $Remark = Remark.find("input.Remark");
        $Remark.val(RemarkText);
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
                msg += "Please Select Benefit Type.\n";
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                }
                result = false;
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
