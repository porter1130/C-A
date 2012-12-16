<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequestGeneral.DataEdit" %>

<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .TextBoxWidth
    {
            width:300px;
        }
    .Isincurred
    {
            display:none;
        }
   #AmountInput input
   {
        width:100px;
       }
   .MustFiled
   {
        color:Red;
        font-size:14px;
        font-weight:bold;
        width:10px;
        margin-left:10px;
       }
</style>
<script type="text/javascript">
    var regFloat = /^[0-9]+(\.?)(\d*)$/;
    var re=/,/;
    $(document).ready(function () {
        $("#<%= RadioListIsAnnualBudget.ClientID%>").find("input").css({ "border-bottom": "none", "width": "30px" });
        $("#<%= RadioListIsNeedBid.ClientID%>").find("input").css({ "border-bottom": "none", "width": "30px" });
        $("#<%= RadioButtonListIncurred.ClientID%>").find("input").css({ "border-bottom": "none", "width": "30px" });
        $("#RemainingAmount").find("input").attr("readonly", true);

        var cost = $("#PRGCost").find("input");
        cost.val(cost.val().replace(/,/g, ""));
        IncourredEvent();
        AmountChangeEvent();
        MoneyFormate();
    });
    function MoneyFormate() {
        $("#LatestAmount").find("input").change(function () {
            var va = $(this).val().replace(/,/g, "");
            if (regFloat.test(va))
            $(this).val(fmoney(va,2))
        });

        $("#PRGCost").find("input").change(function () {
            var vaCost = $(this).val().replace(/,/g, "");
            if (regFloat.test(vaCost))
                $(this).val(fmoney(vaCost, 2))
        });

        
    }

    function beforeSubmit(obj) {
        CreateForbidDIV();
        var isOK = true;
        if (!CheckUnEmpty($("#CostCenter").find("input"))) {
            isOK = false;
        }

        if (!CheckUnEmpty($("#Content").find("textarea"))) {
            isOK = false;
        }

        if (!CheckUnEmpty($("#Reasons").find("textarea"))) {
            isOK = false;
        }

        var PRGCostobj = $("#PRGCost").find("input");

        ///验证金额
        var PRGCostVal = PRGCostobj.val().replace(/,/g, "");
        if (!regFloat.test(PRGCostVal)) {
            SetBorderWarn(PRGCostobj);
            PRGCostobj.focus();
            isOK = false;
        }
        else {
            ClearInputBorderWarn(PRGCostobj);
        }

       //验证备注不为空
        if ($("#<%= RadioListIsAnnualBudget.ClientID%>").find("input[CHECKED]").next("label").text() == "NO") {
            var AnnualBudgetobj = $("#AnnualBudgetComm").find("textarea"); //FormFieldAnnualBudgetComm
            var AnnualBudgetComm = AnnualBudgetobj.text();
            if (jQuery.trim(AnnualBudgetComm) == "") {
                SetBorderWarn(AnnualBudgetobj);
                isOK = false;
                AnnualBudgetobj.focus();
            }
            else {
                ClearBorderWarn(AnnualBudgetobj);
            }
        }
        else {
            var AnnualBudgetobj = $("#AnnualBudgetComm").find("textarea");
            ClearBorderWarn(AnnualBudgetobj);
        }
        //验证备注不为空
        if ($("#<%= RadioListIsNeedBid.ClientID%>").find("input[CHECKED]").next("label").text() == "NO") {
            var BidObj = $("#BidComm").find("textarea");
            var BidComm = BidObj.text();
            if (jQuery.trim(BidComm)=="") {
                SetBorderWarn(BidObj);
                BidObj.focus();
                isOK = false;
            }
            else {
                ClearBorderWarn(BidObj);
            }
        }
        else {
            var BidObj = $("#BidComm").find("textarea");
            ClearBorderWarn(BidObj);
        }
        var IsIncourredOK = CheckIsIncourred();
        if (!IsIncourredOK) {
            isOK = false;
        }

        var isAmountOK = CheckAmount();
        if (!isAmountOK) {
            isOK = false;
        }

        var isPeriodOK = CheckPeriod();
        if (!isPeriodOK) {
            isOK = false;
        }
        if (!isOK) {
            ClearForbidDIV();
        }
        return isOK;
    }

    //Purchase incurred 
    function IncourredEvent() {
        $("#<%= RadioButtonListIncurred.ClientID%>").click(function () {
            if ($(this).find("input[CHECKED]").next("label").text() == "Yes") {
                $(".Isincurred").show();
            }
            else {
                $(".Isincurred").hide();
                ClearIncurred();
            }
        });
        if ($("#<%= RadioButtonListIncurred.ClientID%>").find("input[CHECKED]").next("label").text() == "Yes") {
            $(".Isincurred").show();
        }
    }

    function CheckIsIncourred() {
        var isOK = true;
        if($("#<%= RadioButtonListIncurred.ClientID%>").find("input[CHECKED]").next("label").text() == "Yes")
        {
           var latestAmountObj = $("#LatestAmount").find("input");
           var latestAmount = latestAmountObj.val().replace(/,/g, "");
           if (!regFloat.test(latestAmount)) {
               isOK = false;
               SetBorderWarn(latestAmountObj);
               latestAmountObj.focus();
           }
           else {
               ClearInputBorderWarn(latestAmountObj);
           }

           var fromDateobj = $("#CADateTimeincurredFromVal").find("input");
           if (fromDateobj.val()=="") {
               isOK = false;
               SetBorderWarn(fromDateobj);
           }
           else {
               ClearInputBorderWarn(fromDateobj);
           }

           var toDateobj = $("#CADateTimeincurredToVal").find("input"); 
           if (toDateobj.val() == "") {
               isOK = false;
               SetBorderWarn(toDateobj);
           }
           else {
               ClearInputBorderWarn(toDateobj);
           }
       }
       return isOK;
    }

    function CheckAmount() {
        var IsOK = true;
        var BudgetAmountObj = $("#BudgetAmount").find("input");
        var UsedAmountObj = $("#UsedAmount").find("input");
        var RemainingAmountObj = $("#RemainingAmount").find("input");

        if (!regFloat.test(BudgetAmountObj.val().replace(/,/g, ""))) {
            SetBorderWarn(BudgetAmountObj);
            IsOK = false;
        }
        else {
            ClearInputBorderWarn(BudgetAmountObj);
        }


        if (!regFloat.test(UsedAmountObj.val().replace(/,/g, ""))) {
            SetBorderWarn(UsedAmountObj);
            IsOK = false;
        }
        else {
            ClearInputBorderWarn(UsedAmountObj);
        }

        /*
        if (!regFloat.test(RemainingAmountObj.val().replace(/,/g, ""))) {
            SetBorderWarn(RemainingAmountObj);
            IsOK = false;
        }
        else {
            ClearInputBorderWarn(RemainingAmountObj);
        }*/
        return IsOK;
    }

    function CheckPeriod() {
        var IsOK = true;
        var periodFromObj = $("#CADateTimeFromVal").find("input");
        var periodToObj = $("#CADateTimeToVal").find("input");

        if (periodFromObj.val() == "") {
            SetBorderWarn(periodFromObj);
            IsOK = false;
        }
        else {
            ClearInputBorderWarn(periodFromObj);
        }

        if (periodToObj.val() == "") {
            SetBorderWarn(periodToObj);
            IsOK = false;
        }
        else {
            ClearInputBorderWarn(periodToObj);
        }
        return IsOK;
    }

    ///Amount改变时发生的事件
    function AmountChangeEvent() {
        $("#BudgetAmount").find("input").change(function () {
            $(this).val(fmoney($(this).val(), 2))
            CalculateAmount();
        });
        $("#UsedAmount").find("input").change(function () {
            $(this).val(fmoney($(this).val(),2))
            CalculateAmount();
        });
    }

    //统计金额
    function CalculateAmount() {
        var budgetAmount = $("#BudgetAmount").find("input").val().replace(/,/g, "");
        var usedAmount = $("#UsedAmount").find("input").val().replace(/,/g, "");
        if (!regFloat.test(budgetAmount) || !regFloat.test(usedAmount)) {
            return false;
        }
        var remainingAmount = parseFloat(budgetAmount) - parseFloat(usedAmount);
        $("#RemainingAmount").find("input").val(fmoney(remainingAmount.toFixed(2), 2));
    }

    function ClearIncurred() {
        $("#CADateTimeincurredFromVal").find("input").val("");
        $("#CADateTimeincurredToVal").find("input").val("");
        $("#LatestAmount").find("input").val("");
    }

    function SetBorderWarn($obj) {
        $obj.css('border', '2px solid red');
    }

    function ClearBorderWarn($obj) {
        $obj.css('border', '#999 1px solid');
        $obj.css('border-bottom', '#999 1px solid');
    }
    function ClearInputBorderWarn($obj) {
        $obj.css('border', '');
        $obj.css('border-bottom', '#999 1px solid');
    }
    ///验证不为空
    function CheckUnEmpty(obj) {
        var isOK = true;
        if (obj.val() == "") {
            isOK = false;
            SetBorderWarn(obj);
        }
        else {
            ClearBorderWarn(obj);
        }
        return isOK;
    }

    function fmoney(s, n) {
        n = n > 0 && n <= 20 ? n : 2;
        s = parseFloat((s + "").replace(/[^\d\.-]/g, "")).toFixed(n) + "";
        var l = s.split(".")[0].split("").reverse(),
        r = s.split(".")[1];
        t = "";
        for (i = 0; i < l.length; i++) {
            t += l[i] + ((i + 1) % 3 == 0 && (i + 1) != l.length ? "," : "");
        }
        return t.split("").reverse().join("") + "." + r;
    } 

</script>


<table class="ca-workflow-form-table">
<tr runat="server" id="Title" visible="false">
    <td class="label align-left w20">Workflow No.</td>
    <td class="label align-left w80" colspan="2"><QFL:FormField ID="FormFieldTitle" runat="server" FieldName="Title" ControlMode="Display"  ></QFL:FormField></td>
</tr>
<tr>
    <td class="label align-left w20">cost center<span class="MustFiled">*</span></td>
    <td class="label align-left w80" colspan="2"><span id="CostCenter"><QFL:FormField ID="FormFieldCostCenter" runat="server" FieldName="CostCenter" ControlMode="Edit"  ></QFL:FormField></span></td>
</tr>
<tr>
    <td class="label align-left w20">Goods/Services to be purchased<span class="MustFiled">*</span></td>
    <td class="label align-left w80" colspan="2"><span id="Content"><QFL:FormField ID="ApplicantFieldContent" runat="server" FieldName="Content" ControlMode="Edit"  ></QFL:FormField></span></td>
</tr>
<tr>
    <td class="label align-left">Reasons for the purchase/business case<span class="MustFiled">*</span></td>
    <td class="label align-left" colspan="2"><span id="Reasons"><QFL:FormField ID="FormFieldReasons" runat="server" FieldName="Reasons" ControlMode="Edit"  ></QFL:FormField></span></td>
</tr>
<tr>
    <td class="label align-left">Period<span class="MustFiled">*</span></td>
    <td class="label align-left" colspan="2">
        <table>
            <tr>
                <td>From</td>
                <td><span id="CADateTimeFromVal"><cc1:CADateTimeControl ID="CADateTimeFrom" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></span></td>
                <td>To</td>
                <td><span id="CADateTimeToVal"><cc1:CADateTimeControl ID="CADateTimeTo" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></span></td>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td class="label align-left">Total cost <span class="MustFiled">*</span></td>
    <td class="label align-left w10">
        <asp:DropDownList ID="DropDownListCurrency" Width="100px" runat="server">
        </asp:DropDownList>
    </td>
    <td class="label align-left">
        <span id="PRGCost"><QFL:FormField ID="FormFieldCost" runat="server" FieldName="Cost" ControlMode="Edit" CssClass="TextBoxWidth"></QFL:FormField></span>
    </td>
</tr>
<tr>
    <td class="label align-left">Purchase incurred before or not</td>
    <td class="label align-left" colspan="2">
      <asp:RadioButtonList ID="RadioButtonListIncurred" runat="server" RepeatDirection="Horizontal" style="float:left" >
            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
            <asp:ListItem Text="NO" Selected="True" Value="0"></asp:ListItem>
        </asp:RadioButtonList>
    </td>
</tr>
<tr class="Isincurred">
    <td class="label align-left">Latest Purchase Lasting Period<span class="MustFiled">*</span></td>
    <td class="label align-left" colspan="2">
        <table>
            <tr>
                <td>From</td>
                <td><span id="CADateTimeincurredFromVal"><cc1:CADateTimeControl ID="CADateTimeincurredFrom" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></span></td>
                <td>To</td>
                <td><span id="CADateTimeincurredToVal"><cc1:CADateTimeControl ID="CADateTimeincurredTo" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></span></td>
            </tr>
        </table>
    </td>
</tr>
<tr class="Isincurred">
    <td class="label align-left">Latest Purchase Amount<span class="MustFiled">*</span></td>
    <td class="label align-left" colspan="2">
        <span id="LatestAmount"><QFL:FormField ID="FormFieldLatestAmount" runat="server" FieldName="LatestAmount" ControlMode="Edit"  ></QFL:FormField></span>
    </td>
</tr>
<tr>
    <td class="label align-left">Amount info<span class="MustFiled">*</span></td>
    <td class="label align-left" colspan="2">
       <table id="AmountInput">
        <tr>
            <td>Budget amount</td>
            <td><span id="BudgetAmount"><QFL:FormField ID="FormFieldBudgetAmount" runat="server" FieldName="BudgetAmount" ControlMode="Edit" CssClass="TextBoxWidth"></QFL:FormField></span></td>
            <td>Used amount</td>
            <td><span id="UsedAmount"><QFL:FormField ID="FormFieldUsedAmount" runat="server" FieldName="UsedAmount" ControlMode="Edit" CssClass="TextBoxWidth"></QFL:FormField></span></td>
            <td>Remaining amount</td>
            <td><span id="RemainingAmount"><QFL:FormField ID="FormFieldRemainingAmount" runat="server" FieldName="RemainingAmount" ControlMode="Edit" CssClass="TextBoxWidth"></QFL:FormField></span></td>
        </tr>
       </table>
    </td>
</tr>


<tr>
    <td class="label align-left">Cost is included in the approved annual budget of your department</td>
    <td class="label align-left" colspan="2">
        <asp:RadioButtonList ID="RadioListIsAnnualBudget" runat="server" RepeatDirection="Horizontal" >
            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
            <asp:ListItem Text="NO" Value="0" Selected="True"></asp:ListItem>
        </asp:RadioButtonList>
    </td>
</tr>
<tr>
    <td class="label align-left">Reason</td>
    <td class="label align-left" colspan="2"><span id="AnnualBudgetComm"><QFL:FormField ID="FormFieldAnnualBudgetComm" runat="server" FieldName="AnnualBudgetComm" ControlMode="Edit"  ></QFL:FormField></span></td>
</tr>
<tr>
    <td class="label align-left">If cost exceeds rmb100,000, will bidding be organized to select supplier</td>
    <td class="label align-left" colspan="2">
        <asp:RadioButtonList ID="RadioListIsNeedBid" runat="server" RepeatDirection="Horizontal" style="float:left" >
            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
            <asp:ListItem Text="NO" Selected="True" Value="0"></asp:ListItem>
        </asp:RadioButtonList>
    </td>
</tr>
<tr>
    <td class="label align-left">Reason</td>
    <td class="label align-left" colspan="2"><span id="BidComm"><QFL:FormField ID="FormFieldBidComm" runat="server" FieldName="BidComm" ControlMode="Edit"  ></QFL:FormField></span></td>
</tr>

<tr>
    <td class="label align-left">attacthment</td>
    <td class="label align-left" colspan="2">
      <QFL:FormAttachments runat="server" ID="attacthment"></QFL:FormAttachments>
    </td>
</tr>
</table>