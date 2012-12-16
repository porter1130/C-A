<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.EBC.DataView" %>
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
    .radiobuttonlist td
    {
        border: none;
        padding: 2px;
        text-align: left;
        vertical-align: middle;
    }
    #table_pendingform td
    {
       text-align: left; 
       padding: 0px;
    }
    td.Date
    {
        display:none;    
    }
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4">
            <h3>
                Expatriate Benefit Claim Form<br />
                外籍员工福利报销申请单</h3>
        </td>
    </tr>
    <tr>
        <td>
            WorkflowNumber<br />
            工作流ID
        </td>
        <td colspan="3">
            <QFL:FormField ID="ffWorkflowNumber" runat="server" FieldName="WorkflowNumber" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td class="w20">
            Dept<br />
            部门
        </td>
        <td class="w30">
            <QFL:FormField ID="ffDepartment" runat="server" FieldName="Department" ControlMode="Display">
            </QFL:FormField>
        </td>
        <td class="w20">
            Requested By<br />
            申请人
        </td>
        <td class="w30">
            <QFL:FormField ID="ffApplicant" runat="server" FieldName="Applicant" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Benefit Claim Description<br />
            福利报销描述
        </td>
        <td colspan="3">
            <QFL:FormField ID="ffExpenseDescription" runat="server" FieldName="ExpenseDescription"
                ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table ca-workflow-form-table1" id="ExpenseTypetable">
    <tr>
        <td colspan="5">
            <h3>
                Expatriate benefit claim details</h3>
        </td>
    </tr>
    <tr class="ExpenseTypeTitle">
        <td class="w25">
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
        <td class="ExpenseType">
        </td>
        <td class="Date">
        </td>
        <td class="ExpensePurpose">
        </td>
        <td class="CostCenter">
        </td>
        <td class="Amount">
        </td>
    </tr>
    <tr class="Remark Items">
        <td colspan="1">
            Remark<br />
            备注
        </td>
        <td colspan="4" class="Remark">
        </td>
    </tr>
    <tr class="None Items">
        <td style="height: 20px;" colspan="5">
        </td>
    </tr>
    <tr class="Insert">
        <td colspan="5">
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
            CostCenter<br />
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
            <QFL:FormField ID="FormField6" runat="server" FieldName="TotalAmount" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Cash Advance<br />
            员工借款(RMB)
        </td>
        <td id="CashAdvance" colspan="2">
            <QFL:FormField ID="FormField3" runat="server" FieldName="CashAdvanceAmount" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td style="border-top: #ccc 2px solid">
            Payable to Employee/(Refund to Finance)<br />
            应付员工/(财务应收) (RMB)
        </td>
        <td style="border-top: #ccc 2px solid" colspan="2">
            <QFL:FormField ID="FormField5" runat="server" FieldName="AmountDue" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table">
    <tr>
        <td style="width: 295px">
            Original Invoice Attached<br />
            附原发票
        </td>
        <td colspan="2">
            <QFL:FormField ID="FormField4" runat="server" FieldName="IsAttachInvoice" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Remark<br />
            备注
        </td>
        <td colspan="2">
            <QFL:FormField ID="ffRemark" runat="server" FieldName="Remark" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Attachment<br />
            附件
        </td>
        <td colspan="2" class="attachment" style="text-align: left;">
            <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Display">
            </QFL:FormAttachments>
        </td>
    </tr>
</table>
<table id="table_pendingform" class="ca-workflow-form-table ca-workflow-form-table1 hidden"
    style="position: relative;">
    <tr>
        <td colspan="4" class="value align-center" style="text-align: center; position: relative">
            <div style="position: absolute; left: 0px; top: 0px; width: 679px; height: 448px;
                z-index: 10000; background-color: White; filter: Alpha(opacity=0.5); display: none"
                id="displaydiv">
            </div>
            <h3>
                Your claim is pending/rejected due to :</h3>
        </td>
    </tr>
    <tr class="tr_fapiao">
        <td style="width: 150px">
            Invoice
        </td>
        <td colspan="3" style="text-align: left;" class="td_fapiao">
            <asp:RadioButtonList ID="rblFapiao" runat="server" CssClass="radiobuttonlist radio"
                RepeatDirection="Vertical">
                <asp:ListItem Text="Invoice not attached" Value="Invoice not attached" />
                <asp:ListItem Text="Invoice amount not match/insufficient" Value="Invoice amount not match/insufficient" />
                <asp:ListItem Text="other reasons, please state" Value="other reasons, please state" />
            </asp:RadioButtonList>
            <asp:TextBox ID="txtFapiaoOtherReason" runat="server" CssClass="hidden"></asp:TextBox>
        </td>
    </tr>
    <tr class="tr_information">
        <td>
            Information
        </td>
        <td colspan="3" style="text-align: left;" class="td_information">
            <asp:RadioButtonList ID="rblInformation" runat="server" CssClass="radiobuttonlist radio"
                RepeatDirection="Vertical">
                <asp:ListItem Text="fill-in incorrect cost center" Value="fill-in incorrect cost center" />
                <asp:ListItem Text="fill-in wrong expense type" Value="fill-in wrong expense type" />
                <asp:ListItem Text="print-out form has no amount listed" Value="print-out form has no amount listed" />
                <asp:ListItem Text="other reasons, please state" Value="other reasons, please state" />
            </asp:RadioButtonList>
            <asp:TextBox ID="txtInformationOtherReason" runat="server" CssClass="hidden"></asp:TextBox>
        </td>
    </tr>
    <tr class="tr_claimedamt">
        <td>
            Claimed Amount
        </td>
        <td colspan="3" style="text-align: left;" class="td_claimedamt">
            <asp:RadioButtonList ID="rblClaimedAmt" runat="server" CssClass="radiobuttonlist radio"
                RepeatDirection="Vertical">
                <asp:ListItem Text="used incorrect exchange rate" Value="used incorrect exchange rate"></asp:ListItem>
                <asp:ListItem Text="other reasons, please state" Value="other reasons, please state" />
            </asp:RadioButtonList>
            <asp:TextBox ID="txtClaimedOtherReason" runat="server" CssClass="hidden"></asp:TextBox>
        </td>
    </tr>
    <tr class="tr_otherreasons">
        <td>
            Other reasons, please state
        </td>
        <td colspan="3" style="text-align: left;" class="td_otherreasons">
            <asp:TextBox ID="txtOtherReasons" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="4" style="text-align: left">
            Please contact Poppy Wang at ext. 6148 to resolve the above problem(s) before Finance
            can continue to process your claim form.
        </td>
    </tr>
</table>
<asp:HiddenField ID="hidSummaryExpenseType" runat="server" Value="" />
<asp:HiddenField ID="hidExpatriateBenefitForm" runat="server" Value="" />
<script type="text/javascript" src="jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        DrawExpatriateBenefitForm();
        ShowPendingForm();
        RadioButtonListBind();
        ShowReason();
    });
</script>
<script type="text/javascript">
    function AppendHtml(expenseType, amount, costcenter) {
        var appendHtml = "";
        appendHtml = "<tr class=\"item\" ><td >" + expenseType + "</td><td >" + costcenter + "</td><td >" + amount + "</td></tr>";
        return appendHtml;
    }

    function DrawSummaryExpenseTable() {
        $(".summarytypetable tr").remove(".item");
        var $summarytype = $(".summarytype");
        var $hidSummaryExpenseType = $('#<%= this.hidSummaryExpenseType.ClientID %>');
        var summaryExpenseType = $hidSummaryExpenseType.val();
        var summaryExpense = eval("(" + summaryExpenseType + ")");
        $.each(summaryExpense, function (i, item) {
            if (item != undefined) {
                var $html = $(AppendHtml(item.name, Math.round(item.val * Math.pow(10, 2)) / Math.pow(10, 2), item.costcenter));
                $summarytype.before($html);
            }
        });
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
                        var $InsertItems3Clone = $InsertItems3.clone(true);
                        var $InsertItems2Clone = $InsertItems2.clone(true);
                        var $InsertItems1Clone = $InsertItems1.clone(true);
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
            DrawSummaryExpenseTable();
        }
    }

    function SetExpenseTypeItemValue(ExpenseTypeItem, BenefitType, Date, ExpensePurpose, CostCenter, Amount) {
        var $BenefitType = ExpenseTypeItem.find("td.ExpenseType");
        var $Date = ExpenseTypeItem.find("td.Date");
        var $ExpensePurpose = ExpenseTypeItem.find("td.ExpensePurpose");
        var $CostCenter = ExpenseTypeItem.find("td.CostCenter");
        var $Amount = ExpenseTypeItem.find("td.Amount");
        $BenefitType.text(BenefitType);
        $Date.text(Date);
        $ExpensePurpose.text(ExpensePurpose);
        $CostCenter.text(CostCenter);
        $Amount.text(Amount);
    }

    function SetRemarkItemsValue(Remark, RemarkText) {
        var $Remark = Remark.find("td.Remark");
        $Remark.text(RemarkText);
    }

</script>
<script type="text/javascript">
    var radio_fapiao = '';
    var radio_information = '';
    var radio_claimAmt = '';
    function RadioButtonListBind() {
        $('#<%=this.rblFapiao.ClientID %>').bind('click', function () {
            var value = $(this).find(':radio[checked=true]').val();
            if (value != radio_fapiao) {
                switch (value) {
                    case "other reasons, please state":
                        $('#<%=this.txtFapiaoOtherReason.ClientID %>').show();
                        break;
                    default:
                        $('#<%=this.txtFapiaoOtherReason.ClientID %>').hide();
                        break;
                }
                radio_fapiao = value;
            } else {
                ClearRadioButtonListSelection($(this));
                radio_fapiao = '';
                $('#<%=this.txtFapiaoOtherReason.ClientID %>').hide();
            }
        });

        $('#<%=this.rblInformation.ClientID %>').bind('click', function () {
            var value = $(this).find(':radio[checked=true]').val();
            if (value != radio_information) {
                switch (value) {
                    case "other reasons, please state":
                        $('#<%=this.txtInformationOtherReason.ClientID %>').show();
                        break;
                    default:
                        $('#<%=this.txtInformationOtherReason.ClientID %>').hide();
                        break;
                }
                radio_information = value;
            } else {
                ClearRadioButtonListSelection($(this));
                radio_information = '';
                $('#<%=this.txtInformationOtherReason.ClientID %>').hide();
            }
        });

        $('#<%=this.rblClaimedAmt.ClientID %>').bind('click', function () {
            var value = $(this).find(':radio[checked=true]').val();
            if (value != radio_claimAmt) {
                switch (value) {
                    case "other reasons, please state":
                        $('#<%=this.txtClaimedOtherReason.ClientID %>').show();
                        break;
                    default:
                        $('#<%=this.txtClaimedOtherReason.ClientID %>').hide();
                        break;
                }
                radio_claimAmt = value;
            } else {
                ClearRadioButtonListSelection($(this));
                radio_claimAmt = '';
                $('#<%=this.txtClaimedOtherReason.ClientID %>').hide();
            }
        });
    }

    function ClearRadioButtonListSelection($obj) {
        var options = $obj.find('input:radio');
        options.removeAttr('checked');
    }

    function ShowReason() {
        var $txtFapiaoOtherReason = $('#<%= this.txtFapiaoOtherReason.ClientID %>');
        var $txtInformationOtherReason = $('#<%= this.txtInformationOtherReason.ClientID %>');
        var $txtClaimedOtherReason = $('#<%= this.txtClaimedOtherReason.ClientID %>');
        if ($txtFapiaoOtherReason.val() != "") {
            $txtFapiaoOtherReason.removeClass("hidden");
        }
        if ($txtInformationOtherReason.val() != "") {
            $txtInformationOtherReason.removeClass("hidden");
        }
        if ($txtClaimedOtherReason.val() != "") {
            $txtClaimedOtherReason.removeClass("hidden");
        }
    }

    function CheckPending() {
        var result = true;
        var $td_fapiao_input = $("td.td_fapiao input:radio");
        var $td_information_input = $("td.td_information input:radio");
        var $td_claimedamt_input = $("td.td_claimedamt input:radio");
        var $td_otherreasons_input = $("td.td_otherreasons input:text");
        var td_fapiao_input_count = 0;
        var td_information_input_count = 0;
        var td_claimedamt_input_count = 0;
        $td_fapiao_input.each(function () {
            if ($(this).attr("checked")) {
                td_fapiao_input_count += 1;
            }
        });
        $td_information_input.each(function () {
            if ($(this).attr("checked")) {
                td_information_input_count += 1;
            }
        });
        $td_claimedamt_input.each(function () {
            if ($(this).attr("checked")) {
                td_claimedamt_input_count += 1;
            }
        });
        if ($td_fapiao_input.eq(2).attr("checked")) {
            $txtFapiaoOtherReason = $("input[id$='txtFapiaoOtherReason']");
            if ($txtFapiaoOtherReason.val() == "") {
                alert("Please fill in or select the invoice pending reasons .");
                result = false;
            }
        }
        if ($td_information_input.eq(3).attr("checked")) {
            $txtInformationOtherReason = $("input[id$='txtInformationOtherReason']");
            if ($txtInformationOtherReason.val() == "") {
                alert("Please fill in or select the information pending reasons .");
                result = false;
            }
        }
        if ($td_claimedamt_input.eq(1).attr("checked")) {
            $txtClaimedOtherReason = $("input[id$='txtClaimedOtherReason']");
            if ($txtClaimedOtherReason.val() == "") {
                alert("Please fill in or select the claimed amount pending reasons .");
                result = false;
            }
        }
        if (td_fapiao_input_count == 0 && td_information_input_count == 0
            && td_claimedamt_input_count == 0 && $td_otherreasons_input.val() == "") {
            alert("Please fill in or select pending reasons .");
            result = false;
        }
        return result;
    }

    function GetStep() {
        var step = '<%=this.Step %>';
        return step;
    }

    function ShowPendingForm() {
        var step = '<%=this.Step %>';
        if (step == "DisplayStep" || step == "ConfirmTask") {
            $("#table_pendingform").show();
            $("#commettable").hide();
            if (step == "DisplayStep") {
                $("#displaydiv").show();
                SetReasonStatus();
            }
        }
        var pending = '<%=this.Pending %>';
        if (pending != "") {
            var $pending = $("input[value='Pending']");
            $pending.attr("disabled", "disabled");
        }
    }

    function SetReasonStatus() {
        var $td_fapiao_input = $("td.td_fapiao input:radio");
        var $td_information_input = $("td.td_information input:radio");
        var $td_claimedamt_input = $("td.td_claimedamt input:radio");
        var $td_otherreasons_input = $("td.td_otherreasons input:text");
        var td_fapiao_input_count = 0;
        var td_information_input_count = 0;
        var td_claimedamt_input_count = 0;
        $td_fapiao_input.each(function () {
            if ($(this).attr("checked")) {
                td_fapiao_input_count += 1;
            }
        });
        $td_information_input.each(function () {
            if ($(this).attr("checked")) {
                td_information_input_count += 1;
            }
        });
        $td_claimedamt_input.each(function () {
            if ($(this).attr("checked")) {
                td_claimedamt_input_count += 1;
            }
        });
        if (td_fapiao_input_count == 0) {
            $("tr.tr_fapiao").hide();
        }
        if (td_information_input_count == 0) {
            $("tr.tr_information").hide();
            $("tr.tr_information").next().hide();
        }
        if (td_claimedamt_input_count == 0) {
            $("tr.tr_claimedamt").hide();
            $("tr.tr_claimedamt").next().hide();
        }
        if ($td_otherreasons_input.val() == "") {
            $("tr.tr_otherreasons").hide();
            $("tr.tr_otherreasons").next().hide();
        }
        if (td_fapiao_input_count == 0 && td_information_input_count == 0
            && td_claimedamt_input_count == 0 && $td_otherreasons_input.val() == "") {
            $("#table_pendingform").hide();
        }
    }

</script>
