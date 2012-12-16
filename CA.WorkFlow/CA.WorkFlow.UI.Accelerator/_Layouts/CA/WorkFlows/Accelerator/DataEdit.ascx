<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.Accelerator.DataEdit" %>

<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<script type="text/javascript">
    $(document).ready(function () {
        SetDateReadonly();
        $("#Test").click(function () {
            beforeSubmit(this);
        });
    });

    function beforeSubmit(obj) {
        CreateForbidDIV();
        if (!CheckPeriod() || !CheckEmpty() || !CheckDateSpan() || !CheckAttach()) {
            ClearForbidDIV();
            return false;
        }
        else {
            return true;
        }
    }

    //验证日期不为空
    function CheckPeriod() {
        var IsOK = true;
        var fromDateObj = $("#FromDate").find("input");
        var toDateObj = $("#ToDate").find("input");

        if (fromDateObj.val() == "") {
            SetBorderWarn(fromDateObj);
            IsOK = false;
        }
        else {
            ClearInputBorderWarn(fromDateObj);
        }

        if (toDateObj.val() == "") {
            SetBorderWarn(toDateObj);
            IsOK = false;
        }
        else {
            ClearInputBorderWarn(toDateObj);
        }
        return IsOK;
    }
    /*///验证Acceleratortye不能为空 
    function CheckAcceleratortye() {
        var isOK = true;
        if (acceleratortye.find("input[CHECKED]").length == 0) {
            isOK=false;
            SetBorderWarn(acceleratortye);
        }
        else
        {
            ClearInputBorderWarn(acceleratortye);
        }
        return isOK;
    }*/

    ///验证时间
    function CheckDateSpan() {
        var isOK = true;
        var errorMsg = "";

        var fromDateObj = $("#FromDate").find("input");
        var toDateObj = $("#ToDate").find("input");
        var fromDate = fromDateObj.val();
        var toDate = toDateObj.val();

        if (Date.parse(fromDate) > Date.parse(toDate)) {
            errorMsg += "FromDate must be less than ToDate!\n"
            isOK = false;
        }

        var myDate = new Date();
        var nowDate = myDate.getYear() + "-" + parseInt(myDate.getMonth() + 1) + "-" + myDate.getDate();
        var timeSpan = DateCompare(nowDate, fromDate);
        if (timeSpan < 7) {
            isOK = false;
            errorMsg += "FromDate must be greater than current date at least 7 days!"
        }
        else {
        }
        if (!isOK) {
            alert(errorMsg);
        }
        return isOK;
    }

    //设置日期控件只读
    function SetDateReadonly() {
        var fromDateObj = $("#FromDate").find("input");
        var toDateObj = $("#ToDate").find("input");
        fromDateObj.attr("readonly", "readonly");
        toDateObj.attr("readonly", "readonly");
    }

    //得到日期的天数差
    function DateCompare(asStartDate, asEndDate) {
        var miStart = Date.parse(asStartDate.replace(/\-/g, '/ '));
        var miEnd = Date.parse(asEndDate.replace(/\-/g, '/ '));
        return (miEnd - miStart) / (1000 * 24 * 3600);
    }
    function CheckAttach() {
        var attacthment=$("#idAttachmentsTable").text().length; ///.find("input"));
        var isOK = true;
        if (attacthment==0) {
            isOK = false;
            alert("Please add attacthment!");
        }
        return isOK;
    }

    function CheckEmpty()
    {
        var isOK=true;

        //验证商品类别不为空
        var classType=$("#ClassType").find("input")
        if ($.trim(classType.val()) == "") {
            SetBorderWarn(classType);
            classType.focus();
            isOK = false;
        }
        else {
            ClearInputBorderWarn(classType);
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
    function ClearInputBorderWarn($obj) {
        $obj.css('border', '');
        $obj.css('border-bottom', '#999 1px solid');
    }
</script>
<table class="ca-workflow-form-table">
<tr id="WorkflowNO" runat="server">
    <td class="label align-center w20">Workflow NO.</td>
    <td class="label align-left w80" colspan="3">
    <QFL:FormField ID="FormFieldTitle" runat="server" FieldName="Title" ControlMode="Display"  ></QFL:FormField>
    </td>
</tr>
<tr>
    <td class="label align-left w20"><span id="Test">Class<br/>商品类别</span></td>
    <td class="label align-left w80" colspan="3">
       <span id="ClassType"><QFL:FormField ID="FormFieldClass" runat="server" FieldName="Class" ControlMode="Edit"  ></QFL:FormField></span></td>
</tr>
<tr>
    <td class="label align-left">FromDate<br/>开始日期</td>
    <td class="label align-left"><span id="FromDate"><cc1:CADateTimeControl ID="CADateTimeFrom" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></span></td>
    <td class="label align-left">ToDate<br/>结束日期</td>
    <td class="label align-left"><span id="ToDate"><cc1:CADateTimeControl ID="CADateTimeTo" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></span></td>
</tr>
<tr>
    <td class="label align-left">Accelerator Type<br/>促销活动类别</td>
    <td class="label align-left" colspan="3">
       <%--<asp:RadioButtonList ID="RadioButtonListAccelerator" runat="server"></asp:RadioButtonList>--%>
       <asp:DropDownList ID="DropDownListAccelerator" runat="server"></asp:DropDownList>
    </td>
</tr>
<tr>
    <td class="label align-left">Description<br/>描述</td>
    <td class="label align-left" colspan="3"><span id="Description"><QFL:FormField ID="ApplicantFieldContent" runat="server" FieldName="Description" ControlMode="Edit"  ></QFL:FormField></span></td>
</tr>


<tr>
    <td class="label align-left">Accelerator simulation template download</td>
    <td class="label align-left" colspan="3">
       <a href="/tmpfiles/Accelerator/Accelerator Simulation Template.xls">template download</a>
    </td>
</tr>
<tr>
    <td class="label align-left">attacthment</td>
    <td class="label align-left" colspan="3">
      <QFL:FormAttachments runat="server" ID="attacthment"></QFL:FormAttachments>
    </td>
</tr>
</table>
