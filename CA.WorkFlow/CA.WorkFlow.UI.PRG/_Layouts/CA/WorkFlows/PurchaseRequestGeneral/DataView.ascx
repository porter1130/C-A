<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequestGeneral.DataView" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>

<script type="text/javascript">
    function beforeSubmit(obj) {
        CreateForbidDIV();
        var isOK = true;
        if (obj.value === 'Reject') {
            var comm = $("#comment-task").find("textarea");
            if (jQuery.trim(comm.val()) == "") {
                isOK = false;
                alert("Please fill remarks!");
                SetBorderWarn(comm);
                comm.focus();
            }
            else {
                ClearBorderWarn(comm);
            }
        }
        if (!isOK) {
            ClearForbidDIV();
        }
        return isOK;
    }
    function SetBorderWarn($obj) {
        $obj.css('border', '2px solid red');
    }

    function ClearBorderWarn($obj) {
        $obj.css('border', '#999 1px solid');
        $obj.css('border-bottom', '#999 1px solid');
    }
</script>

<table id="table_comment" class="ca-workflow-form-table full-width">
        <tr>
            <td class="label align-center w25">
                Remarks<br />
                备注:
            </td>
            <td class="value" id="comment-task">
                <span id="PRGComments"><QFL:FormField ID="CommentsField" runat="server" FieldName="Comments" ControlMode="Display" ></QFL:FormField></span>
            </td>
        </tr>
</table>
<br />
<table id="DataEditUserInfotb" class="ca-workflow-form-table">
    <tr> <td class="label align-center w25" style="width:200px;">申请人<br />
            Requested By
        </td>
        <td class="label align-center w20" >
            <QFL:FormField ID="ApplicantField" runat="server" FieldName="Applicant" ControlMode="Display"  ></QFL:FormField>
        </td>
        <td class="label align-center w22" style="width:120px;">部门<br />Dept
        </td>
        <td class="label align-center w20">
            <QFL:FormField ID="DepartmentField" runat="server" FieldName="Department" ControlMode="Display"  ></QFL:FormField>
        </td>
        </tr>
</table>
<br />
<table class="ca-workflow-form-table">

<tr>
    <td class="label align-left w20">Workflow No.</td>
    <td class="label align-left w80" colspan="2"><QFL:FormField ID="FormField1" runat="server" FieldName="Title" ControlMode="Display"  ></QFL:FormField></td>
</tr>
<tr id="InernalNO" runat="server" visible="false">
    <td class="label align-left w20">Inernal order No.</td>
    <td class="label align-left w80" colspan="2"><QFL:FormField ID="FormFieldInernalNO" runat="server" FieldName="InernalNO" ControlMode="Edit"  ></QFL:FormField></td>
</tr>
<tr>
    <td class="label align-left w20">cost center</td>
    <td class="label align-left w80" colspan="2"><span id="Span1"><QFL:FormField ID="FormFieldCostCenter" runat="server" FieldName="CostCenter" ControlMode="Display"  ></QFL:FormField></span></td>
</tr>
<tr>
    <td class="label align-left w20">Goods/Services to be purchased</td>
    <td class="label align-left w80" colspan="2"><QFL:FormField ID="ApplicantFieldContent" runat="server" FieldName="Content" ControlMode="Display"  ></QFL:FormField></td>
</tr>
<tr>
    <td class="label align-left">Reasons for the purchase/business case</td>
    <td class="label align-left" colspan="2"><QFL:FormField ID="FormFieldReasons" runat="server" FieldName="Reasons" ControlMode="Display"  ></QFL:FormField></td>
</tr>
<tr>
    <td class="label align-left">Period</td>
    <td class="label align-left" colspan="2">
      <table>
            <tr>
                <td><QFL:FormField ID="FormFieldPeriod" runat="server" FieldName="PeriodFrom" ControlMode="Display"  ></QFL:FormField> </td>
                <td>-</td>
                <td><QFL:FormField ID="FormField2" runat="server" FieldName="PeriodTo" ControlMode="Display"  ></QFL:FormField> </td>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td class="label align-left">Total cost </td>
    <td class="label align-left" style="width:50px">
        <QFL:FormField ID="FormFieldCurrency" runat="server" FieldName="Currency" ControlMode="Display" CssClass="TextBoxWidth"></QFL:FormField>
    </td>
    <td class="label align-left">
        <QFL:FormField ID="FormFieldCost" runat="server" FieldName="Cost" ControlMode="Display" CssClass="TextBoxWidth"></QFL:FormField>
    </td>
</tr>

<tr>
    <td class="label align-left">Purchase incurred before or not</td>
    <td class="label align-left" colspan="2">
     <QFL:FormField ID="FormField3" runat="server" FieldName="IsIncurred" ControlMode="Display" CssClass="TextBoxWidth"></QFL:FormField>
    </td>
</tr>
<asp:Panel ID="PanelIncurred" runat="server">
<tr class="Isincurred">
    <td class="label align-left">Latest Purchase Lasting Period</td>
    <td class="label align-left" colspan="2">
        <table>
            <tr>
                <td><QFL:FormField ID="FormFieldIncurredFrom" runat="server" FieldName="IncurredFrom" ControlMode="Display" CssClass="TextBoxWidth"></QFL:FormField></td>
                <td>-</td>
                <td><QFL:FormField ID="FormFieldIncurredTo" runat="server" FieldName="IncurredTo" ControlMode="Display" CssClass="TextBoxWidth"></QFL:FormField></td>
            </tr>
        </table>
    </td>
</tr>
<tr class="Isincurred">
    <td class="label align-left">Latest Purchase Amount</td>
    <td class="label align-left" colspan="2"> 
        <span id="LatestAmount"><QFL:FormField ID="FormFieldLatestAmount" runat="server" FieldName="LatestAmount" ControlMode="Display"  ></QFL:FormField></span>
    </td>
</tr>

</asp:Panel>
<tr>
    <td class="label align-left">Amount info</td>
    <td class="label align-left" colspan="2">
       <table id="AmountInput">
        <tr>
            <td>Budget amount：</td>
            <td><span id="BudgetAmount"><QFL:FormField ID="FormFieldBudgetAmount" runat="server" FieldName="BudgetAmount" ControlMode="Display"></QFL:FormField></span></td>
            <td>; Used amount：</td>
            <td><span id="UsedAmount"><QFL:FormField ID="FormFieldUsedAmount" runat="server" FieldName="UsedAmount" ControlMode="Display"></QFL:FormField></span></td>
            <td>; Remaining amount：</td>
            <td><span id="RemainingAmount"><QFL:FormField ID="FormFieldRemainingAmount" runat="server" FieldName="RemainingAmount" ControlMode="Display"></QFL:FormField></span></td>
        </tr>
       </table>
    </td>
</tr>

<tr>
    <td class="label align-left">Cost is included in the approved annual budget of your department</td>
    <td class="label align-left" colspan="2">
        <QFL:FormField ID="FormFieldIsAnnualBudget" runat="server" FieldName="IsAnnualBudget" ControlMode="Display" CssClass="TextBoxWidth"></QFL:FormField>
    </td>
</tr>
<tr>
    <td class="label align-left">Reason</td>
    <td class="label align-left" colspan="2"><QFL:FormField ID="FormFieldAnnualBudgetComm" runat="server" FieldName="AnnualBudgetComm" ControlMode="Display"  ></QFL:FormField></td>
</tr>
<tr>
    <td class="label align-left">If cost exceeds rmb100,000, will bidding be organized to select supplier</td>
    <td class="label align-left" colspan="2">
         <QFL:FormField ID="FormFieldIsNeedBid" runat="server" FieldName="IsNeedBid" ControlMode="Display" CssClass="TextBoxWidth"></QFL:FormField>
    
    </td>
</tr>
<tr>
    <td class="label align-left">Reason</td>
    <td class="label align-left" colspan="2"><QFL:FormField ID="FormFieldBidComm" runat="server" FieldName="BidComm" ControlMode="Display"  ></QFL:FormField></td>
</tr>
<tr>
    <td class="label align-left">Attacthment</td>
    <td class="label align-left" colspan="2"><QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Display" ></QFL:FormAttachments></td>
</tr>
</table>


            