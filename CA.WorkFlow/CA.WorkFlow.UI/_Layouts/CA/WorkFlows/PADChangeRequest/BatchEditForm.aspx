<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="BatchEditForm.aspx.cs" Inherits="CA.WorkFlow.UI.PADChangeRequest.BatchEditForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/jquery-ui.css" />
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/jquery-ui.custom.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery.bgiframe.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
    
<script type="text/javascript">


    function beforeSubmit(sender) {
        if (!CheackCount()) {
            return false;
        }
        if (!CheckDateIsEmpty()) {
            return false;
        }
        if (!CheckPODate()) {
            return false;
        }
        if (!CheckDelivered()) {
            return false
        }
        return true;
    }
    ///验证每条PO的日期
    function CheckPODate() {
        var isOK = true;
        var errorMessage = "";
        $(".DateCompare").each(function () {
            var CurrentDate = $(this).attr("CurrentDate");
            var PONO = $(this).attr("PONO");
            var newDate = $(this).find("input").val();
            if (Date.parse(CurrentDate) == Date.parse(newDate)) {
                errorMessage += PONO + ":Proposed New PAD can't equal Current PAD! \n"
            }
        });
        if (errorMessage.length > 0) {
            isOK = false;
            alert(errorMessage);
        }
        return isOK;
    }

    ///验证PO数不为空
    function CheackCount() {
        var isOK = true;
        var poCount = 0;
        poCount = $(".POCount").length; ;
        if (poCount > 0) {
            return true;
        }
        else {
            alert("Empty data");
            return false;
        }
    }

    //验证日期不为空 
    function CheckDateIsEmpty() {
        var errorMessage = "";
        var isOK = true;
        $(".DateTimeControl").each(function () {
            if ($(this).val().length == 0) {
                errorMessage += "Proposed New PAD is neccessary!"
                SetBorderWarn($(this));
                isOK = false;
            }
            else {
                ClearBorderWarn($(this))
            }
        });
        if (errorMessage.length > 0) {
            alert("Proposed New PAD is neccessary!");
        }
        return isOK;
    }

    //验证PO中的Delivered
    function CheckDelivered() {
        var isOK = true;
        var errorMessage = "";
        $(".IsNeedApprove").each(function () {
            if ($(this).children("input").val() != "False") {
                errorMessage += $(this).attr("PONO") + " is Delivered,It will not be save.\n"
            }
        });
        if (errorMessage.length > 0) {
            errorMessage += "\nAre you sure continue?";
            if (!confirm(errorMessage)) {
                isOK = false;
            }
        }
        return isOK
    }

    function SetBorderWarn($obj) {
        $obj.css('border', '2px solid red');
    }

    function ClearBorderWarn($obj) {
        $obj.css('border', '');
        $obj.css('border-bottom', '#999 1px solid');
    }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="Edit">
           <div class="ca-workflow-form-buttons noPrint">
                <cc1:CAActionsButton ID="Actions" runat="server"  />
                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx'" />
           </div>
            <table id="table_comment" class="ca-workflow-form-table full-width">
                <tr>
                    <td class="label align-center w25">
                        Remarks<br />
                        备注:
                    </td>
                    <td class="value" id="comment-task">
                    <QFL:FormField ID="RemarksField" runat="server" FieldName="Remarks"  >
                        </QFL:FormField>
                    </td>
                </tr>
            </table>
            <br />
              <table class="ca-workflow-form-table" >
              <tr>
              <td class="label align-center w25" style="width:200px;">申请人<br />
            Requested By
        </td>
        <td class="label align-center w20" ><QFL:FormField ID="ApplicantField" runat="server" FieldName="Applicant" ControlMode="Display"  >
                        </QFL:FormField>
        </td>
        <td class="label align-center w22" style="width:120px;">部门<br />Dept
        </td>
        <td class="label align-center w20"><QFL:FormField ID="DepartmentField" runat="server" FieldName="Department" ControlMode="Display"  >
                        </QFL:FormField></td>
        <td class="label align-center w25" style="width:120px;">姓名<br />Name</td>
        <td class="label align-center w20"><QFL:FormField ID="ChineseNameField" runat="server" FieldName="ChineseName" ControlMode="Display"  >
                        </QFL:FormField></td>
        </tr>
       </table>
            <br />
              <table class="ca-workflow-form-table">
              <tr>
              <td class="label align-center" style="width:110px; height:26px;">Excel Uplad</td>
              <td class="label align-left">
                <asp:FileUpload ID="FileUploadExcel" runat="server" Width="300px"  />
                  <asp:Button ID="ButtonLoad" CssClass="form-button" Width="80px" runat="server" Text="UpLoad" onclick="ButtonLoad_Click" />
              </td>
              </tr>
              <tr>
              <td class="label align-center" style="width:110px; height:26px;">PO Number</td>
              <td class="label align-left">
                <asp:TextBox ID="TextBoxPONOs" TextMode="MultiLine" Width="300px" runat="server"></asp:TextBox>
                  <asp:Button ID="ButtonBatchSearch" runat="server" Text="Search" 
                      CssClass="form-button" onclick="ButtonBatchSearch_Click"/>
              </td>
              </tr>
              
              <tr>
              <td class="label align-center" style="width:110px; height:26px;">Count</td>
              <td class="label align-left">
                    <asp:Label ID="LabelCount" runat="server" Text="Label"></asp:Label>
              </td>
              </tr>
            </table>

              <asp:Repeater ID="RepeaterPOData" runat="server">
                <ItemTemplate>
                      <table class="ca-workflow-form-table POCount">
                     <tr>
                        <td class="label align-center" colspan="1">PO Number</td>
                        <td class="label align-left" colspan="5"><asp:Label ID="LabelPONO" runat="server" Text='<%# Eval("PONumber")%>'></asp:Label></td>
                        <asp:HiddenField ID="HiddenFieldstoredelivery" runat="server" Value='<%# IsDelivery(Eval("IsNeedApprove").ToString())%>' />
                        <asp:HiddenField ID="HiddenFieldIsSuccess" runat="server" Value='<%# Eval("IsSuccess")%>' />
                     </tr>
                    <tr>
                        <td class="label align-center" colspan="1">当前到货日期<br />Current PAD</td>
                        <td class="label align-left" colspan="5">
                        <asp:Label ID="LabelPAD" runat="server" Text='<%# DateFormate(Eval("CurrentPAD").ToString())%>'></asp:Label>
                    </td>
                   </tr>
                    <tr>
                        <td class="label align-center" colspan="1">新到货日期<br />Proposed New PAD</td>
                        <td class="label align-left" colspan="5">
                            <div class="DateCompare" CurrentDate='<%# Eval("CurrentPAD")%>' PONO='<%# Eval("PONumber")%>'>
                                <cc1:CADateTimeControl ID="CADateTimeFrom" runat="server"  DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" />
                                <asp:HiddenField ID="HiddenFieldDate" runat="server" Value='<%# Eval("NewPAD")%>' />
                            </div>
                        </td>
                     </tr>
                    <tr>
                    <td class="label align-center w20">
                        Supplier Name
                     </td>
                    <td class="label align-center w20" >
                             <asp:TextBox ID="TextBoxSupplierName" runat="server" Text='<%# Eval("SupplierName")%>'></asp:TextBox>
                    </td>
                    <td class="label align-center w22" style="width:120px;">PAD year\week
                    </td>
        <td class="label align-center w20" id="Td4">
            <span style="float:left;">
                <asp:TextBox ID="TextBoxPADYear" Width="50px" runat="server" Text='<%# Eval("PADyear")%>'></asp:TextBox>
            </span>
            <span style="float:left;">
                  <asp:TextBox ID="TextBoxPADweek" Width="50px" runat="server" Text='<%# Eval("PADweek")%>'></asp:TextBox>
            </span>
       </td>
        <td class="label align-center w10" style="width:80px;">OSP</td>
        <td class="label align-center w20">
              <asp:TextBox ID="TextBoxOSP" runat="server" Text='<%# Eval("OSP")%>'></asp:TextBox>
        </td>
                </tr>
                    <tr><td class="label align-center w20">Story
        </td>
        <td class="label align-center w20" >
            <asp:TextBox ID="TextBoxValueForStory" runat="server" Text='<%# Eval("ValueForStory")%>'></asp:TextBox>
        </td>
        <td class="label align-center w22" style="width:120px;">SAD year\week
        </td>
        <td class="label align-center w20" id="Td5">
            <span style="float:left;">
             <asp:TextBox ID="TextBoxSADyear" Width="50px" runat="server" Text='<%# Eval("SADyear")%>'></asp:TextBox>
            </span>
             <span style="float:left;">
             <asp:TextBox ID="TextBoxSADweek" Width="50px" runat="server" Text='<%# Eval("SADweek")%>'></asp:TextBox>
             </span>
         </td>
        <td class="label align-center w10" style="width:80px;">OMU</td>
        <td class="label align-center w20"><asp:TextBox ID="TextBoxOMU" runat="server" Text='<%# Eval("OMU")%>'></asp:TextBox>
             </td>
                </tr>
      
                <tr>
                    <td class="label align-center w20">PoQty</td>
                    <td class="label align-center w20" ><asp:TextBox ID="TextBoxPOQTY" runat="server" Text='<%# IntFormate(Eval("PoQty").ToString())%>'></asp:TextBox></td>
                    <td class="label align-center w22" style="width:120px;">Style Number</td>
                    <td class="label align-center w20" id="Td1"><asp:TextBox ID="TextBoxStyleNO" runat="server" Text='<%# Eval("StyleNumber")%>'></asp:TextBox></td>
                    <td class="label align-center w10" style="width:80px;"></td>
                    <td class="label align-center w20"></td>
                </tr>

                    <tr>
                    <td class="label align-center" colspan="6">
                        
                    </td>
                </tr>
            </table>
                </ItemTemplate>
            </asp:Repeater>
            <cc1:TaskTraceList ID="Trace1" runat="server" BorderColor="#999999" BorderWidth="1px" GridLines="Horizontal" />
            <SharePoint:FormDigest ID="FormDigest3" runat="server"></SharePoint:FormDigest>
            <br />
</QFL:ListFormControl>   
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
<div class="ca-workflow-form-note noPrint">
        <div class="top"></div>
        <div class="middle">
            注：<br />
            1. 一经批准，申请流程会更新SAP系统中的到货日期<br />
            <br />
            Note:<br />
            <br />
            <br />
            <a href="/WorkFlowCenter/FlowCharts/PADChangeRequest.docx">Click here to view the flowchart of the workflow</a>
        </div>
        <div class="foot"></div>
</div>
</asp:Content>
