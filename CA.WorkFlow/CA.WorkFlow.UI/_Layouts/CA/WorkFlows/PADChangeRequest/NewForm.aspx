<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" Inherits=" CA.WorkFlow.UI.PADChangeRequest.NewForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
PAD Change Request
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
    <style type="text/css">
    
     #tdinput0 input
    {
        width: 200px;
        float: left;
        margin-bottom: -2px;
      
    }  
     #tdinput1 input
    {
        width: 200px;
      
    }  
    #tdinput2 input
    {
        width: 200px;
      
    } 
    #hhh input
    { 
        width: 100px;
        float: left;
        margin-top: -14px;
    }
    #spancacapad input
    {
        width: 50px;
    }
    #spancacasad input
    {
        width: 50px;
    }
</style>
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
                errorMessage += PONO + ":Proposed New PAD can't equal to Current PAD! \n"
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
        var poCount =0;
        poCount= $(".POCount").length;;
        if (poCount > 0) {
            return true;
//            if (poCount >50) {
//                alert("PO Count must less than 50!");
//                isOK = false;
//            }
//            else {
//                isOK = true;
//            }
        }
        else {
            alert("Empty data");
            isOK= false;
        }
        return isOK;
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
PAD Change Request
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
     <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
         <div class="ca-workflow-form-buttons noPrint">
               <QFC:StartWorkflowButton ID="StartWorkflowButton1" WorkflowName="PAD Change Request"
                    runat="server" Text="Submit" />
                <QFC:StartWorkflowButton ID="StartWorkflowButton2" WorkflowName="PAD Change Request"
                    runat="server" Text="Save" />
                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
           </div>
           <uc1:DataForm ID="DataForm1" runat="server"  />
        <br />
            <table class="ca-workflow-form-table">
              <tr>
              <td class="label align-center" style="width:110px; height:26px;">Excel Uplad</td>
              <td class="label align-left">
                <asp:FileUpload ID="FileUploadExcel" runat="server" Width="300px"  />
                  <asp:Button ID="ButtonLoad" CssClass="form-button" Width="80px" runat="server"  Text="UpLoad" onclick="ButtonLoad_Click" />
                  <a href="/tmpfiles/PADChangeRequest/PADChangRequest_Template.xlsx">Excel template download</a>
              </td>
              </tr>
              <tr>
              <td class="label align-center" style="width:110px; height:26px;">PO Number</td>
              <td class="label align-left">
              please input PO NO. and split with ","<br />
                <asp:TextBox ID="TextBoxPONOs" TextMode="MultiLine" Width="300px" Height="60px" runat="server"></asp:TextBox>
                  <asp:Button ID="ButtonBatchSearch" runat="server" Text="Search" CssClass="form-button" onclick="ButtonBatchSearch_Click"/>
              </td>
              </tr>
              
              <tr>
              <td class="label align-center" style="width:110px; height:26px;">Count</td>
              <td class="label align-left">
                    <asp:Label ID="LabelCount" runat="server" Text="0"></asp:Label>
              </td>
              </tr>

            </table>
            <asp:HiddenField ID="HiddenFieldWorkflowNO" runat="server"></asp:HiddenField>
            <asp:Repeater ID="RepeaterPOData" runat="server">
                <ItemTemplate>
                      <table class="ca-workflow-form-table POCount" title='<%# Eval("IsSuccess").ToString()%>'>
                     <tr>
                        <td class="label align-center" colspan="1">PO Number</td>
                        <td class="label align-left" colspan="5"><asp:Label ID="LabelPONO" runat="server" Text='<%# Eval("PONO")%>'></asp:Label></td>
                        <div class="IsNeedApprove" PONO='<%# Eval("PONO")%>'>
                            <asp:HiddenField ID="HiddenFieldstoredelivery" runat="server" Value='<%# Eval("IsNeedApprove")%>' />
                        </div> 
                        <asp:HiddenField ID="HiddenFieldIsSuccess" runat="server" Value='<%# Eval("IsSuccess")%>' />
                     </tr>
                    <tr>
                        <td class="label align-center" colspan="1">当前到货日期<br />Current PAD</td>
                        <td class="label align-left" colspan="5">
                        <asp:Label ID="LabelPAD" runat="server" Text='<%# Eval("PAD")%>'></asp:Label>
                    </td>
                   </tr>
                    <tr>
                        <td class="label align-center" colspan="1">新到货日期<br />Proposed New PAD</td>
                        <td class="label align-left" colspan="5">
                            <div class="DateCompare" CurrentDate='<%# Eval("PAD")%>' PONO='<%# Eval("PONO")%>'>
                                <cc1:CADateTimeControl ID="CADateTimeFrom" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" />
                                 <asp:HiddenField ID="HiddenFieldDate" runat="server" Value='<%# Eval("Date")%>' />
                            </div>
                        </td>
                     </tr>
                    <tr>
                    <td class="label align-center w20">
                        Supplier Name
                     </td>
                    <td class="label align-center w20" >
                             <asp:TextBox ID="TextBoxSupplierName" runat="server" Width="150px" Text='<%# Eval("SupplierName")%>'></asp:TextBox>
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
              <asp:TextBox ID="TextBoxOSP" runat="server" Text='<%# IntFormate(Eval("OSP").ToString())%>'></asp:TextBox>
        </td>
                </tr>
                    <tr>
                    <td class="label align-center w20">Story
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
        <td class="label align-center w20">
             <asp:TextBox ID="TextBoxOMU" runat="server" Text='<%# Eval("OMU")%>'></asp:TextBox>
             </td>
                </tr>
                
                <tr>
                    <td class="label align-center w20">POQty</td>
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

            <SharePoint:FormDigest ID="FormDigest1" runat="server"></SharePoint:FormDigest>
   </QFL:ListFormControl>   
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
<div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
        <div class="middle">
            注：<br />
            1. 一经批准，申请流程会更新SAP系统中的到货日期<br />
            
            <br />
            Note:<br />
           
            <br />
            <br />
            <a href="/WorkFlowCenter/FlowCharts/PADChangeRequest.docx">Click here to view the flowchart of the workflow</a>
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
</asp:Content>
