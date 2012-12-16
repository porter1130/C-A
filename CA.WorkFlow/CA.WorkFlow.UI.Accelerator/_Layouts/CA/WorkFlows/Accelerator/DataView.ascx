<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.Accelerator.DataView" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<script type="text/javascript">

    function beforeSubmit(obj) {
        CreateForbidDIV();
        if (!CheckRejectComments(obj.value)) {
            ClearForbidDIV();
            return false;
        }
        else {
            return true;
        }
    }

    function CheckRejectComments(action) {
        var isOK = true;
        if (action == 'Reject') {
            var commentobj = $("#comment-task").find("textarea");
            if ($.trim(commentobj.val()) == "") {
                isOK = false;
                SetBorderWarn(commentobj);
                commentobj.focus();
            }
            else {
                ClearBorderWarn(commentobj);
            }
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
                <span id="PRGComments"><QFL:FormField ID="CommentsField" runat="server" FieldName="Remarks" ControlMode="Display" ></QFL:FormField></span>
            </td>
        </tr>
</table>
<br />
<table class="ca-workflow-form-table">
<tr>
    <td class="label align-center w20">Workflow NO.</td>
    <td class="label align-left w80" colspan="3">
    <QFL:FormField ID="FormFieldTitle" runat="server" FieldName="Title" ControlMode="Display"  ></QFL:FormField>
    </td>
</tr>
<tr>
    <td class="label align-left w20">Class<br/>商品类别</td>
    <td class="label align-left w80" colspan="3"><span id="Span1"><QFL:FormField ID="FormField2" runat="server" FieldName="Class" ControlMode="Display"  ></QFL:FormField></span></td>
</tr>
<tr>
    <td class="label align-left">FromDate<br/>开始日期</td>
    <td class="label align-left"><QFL:FormField ID="FormFieldFromDate" runat="server" FieldName="FromDate" ControlMode="Display"  ></QFL:FormField></td>
    <td class="label align-left">ToDate<br/>结束日期</td>
    <td class="label align-left"><QFL:FormField ID="FormField1" runat="server" FieldName="ToDate" ControlMode="Display"  ></QFL:FormField></td>
</tr>
<tr>
    <td class="label align-left">Accelerator Type<br/>促销活动类别</td>
    <td class="label align-left" colspan="3">
            <QFL:FormField ID="FormFieldAcceleratorContent" runat="server" FieldName="AcceleratorContent" ControlMode="Display"  ></QFL:FormField>
    </td>
</tr>
<tr>
    <td class="label align-left">Description<br/>描述</td>
    <td class="label align-left" colspan="3"><span id="Content"><QFL:FormField ID="ApplicantFieldContent" runat="server" FieldName="Description" ControlMode="Display"  ></QFL:FormField></span></td>
</tr>
<tr>
    <td class="label align-left">Attacthment</td>
    <td class="label align-left" colspan="2"><QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Display" ></QFL:FormAttachments></td>
</tr>
</table>
<br />
<table class="ca-workflow-form-table">
<tr>
    <td class="label align-left w20"> 
         Applicant
         <br />
         申请人
     </td>
    <td class="label align-left w80"><QFL:FormField ID="FormFieldApplicant" runat="server" FieldName="Applicant" ControlMode="Display"  ></QFL:FormField></td>
</tr>
</table>




            