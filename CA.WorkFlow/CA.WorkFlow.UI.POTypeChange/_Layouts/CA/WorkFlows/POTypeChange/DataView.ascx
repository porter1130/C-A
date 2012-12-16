<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.POTypeChange.DataView" %>

<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>

   <script type="text/javascript">
       $(document).ready(function () {
           if ("<%= isDisplayStep%>" == "False")//是审批流程
           {
                $(".IsSuccess").each(function () {
                    if ($(this).children("input").val() == "1" && $(this).next("input").val()=="1") {//已经修改过的就不显示
                        $(this).parents(".ca-workflow-form-table").hide();
                    }
                });
           }
       });

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
            <td class="value" id="comment-task" colspan="5">
                <QFL:FormField ID="RemarksField" runat="server" FieldName="Remarks" ControlMode="Display" ></QFL:FormField>

            </td>
        </tr>
        <tr>
        <td class="label align-center w25">
            Workflow NO.
        </td>
        <td class="value" id="Td2" colspan="5">
            <QFL:FormField ID="FormFieldTitle" runat="server" FieldName="Title" ControlMode="Display"  ></QFL:FormField>
        </td>
        </tr>
           <tr>
            <td class="label align-center w25">
                Count
            </td>
            <td class="value" id="Td1" colspan="5">
                <asp:Label ID="LabelCount" runat="server" Text="0"></asp:Label>
            </td>
        </tr>
</table>
<br/>
            <asp:Repeater ID="RepeaterPOData" runat="server">
                <ItemTemplate>
                      <table class="ca-workflow-form-table POCount"> 
                         <tr>
                            <td class="label align-center" colspan="1">PO No.</td>
                            <td class="label align-left" colspan="5">
                                <asp:Label ID="LabelPONO" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                <asp:HiddenField ID="HiddenFieldID" runat="server" Value='<%# Eval("ID")%>'></asp:HiddenField>
                                 <div class="IsSuccess" isApprove='<%# Eval("IsApproved").ToString()%>'>
                                        <asp:HiddenField ID="HiddenFieldISSuccess" runat="server" Value='<%# Eval("IsSuccess")%>'></asp:HiddenField>
                                 </div>
                                 <asp:HiddenField ID="HiddenFieldIsPADSuccess" runat="server" Value='<%# Eval("IsPADSuccess")%>'></asp:HiddenField>
                            </td>
                         </tr>
                        <tr>
                            <td class="label align-center" colspan="1">New PO Type</td>
                            <td class="label align-left" colspan="3">
                             <asp:HiddenField ID="HiddenFieldNewTypeValue" runat="server" Value='<%# Eval("NewTypeValue")%>'></asp:HiddenField>
                                <asp:Label ID="LabelNewType" runat="server" Text='<%# Eval("NewType")%>'></asp:Label>
                                <asp:DropDownList ID="DropDownListApprove" Visible='<%# isApproveStep%>' Enabled='<%# Eval("IsSuccess").ToString()=="1"?false:true%>' Width="80px" runat="server" >
                                    <asp:ListItem Text="Approve" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Reject" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="HiddenFieldIsApproved" runat="server" Value='<%# Eval("IsApproved")%>'></asp:HiddenField>
                            </td>
                         </tr>
                       
                         <tr id="isUpdated" runat="server" visible='<%# isDisplayStep%>' >
                            <td class="label align-center" colspan="1">Approved Satus</td>
                            <td class="label align-left" colspan="3"> <%# GetApprovedSatus(Eval("IsApproved").ToString())%>
                            </td>
                         </tr>
                         
                        <tr id="isDisplayStep" runat="server" visible='<%# isDisplayStep%>' >
                            <td class="label align-center" colspan="1">Is po type updated to SAP</td>
                            <td class="label align-left" colspan="3" style="color:Red"><%# Eval("IsSuccess").ToString()=="1"?"YES":"NO"%></td>
                         </tr>
                         
                        <tr id="Tr1" runat="server" visible='<%# isDisplayStep%>' >
                            <td class="label align-center" colspan="1">Is PAD updated to SAP</td>
                            <td class="label align-left" colspan="3" style="color:Red"><%# Eval("IsPADSuccess").ToString() == "1" ? "YES" : "NO"%></td>
                         </tr>

                        <tr>
                            <td class="label align-center">New PAD</td>
                            <td class="label align-center">
                                <asp:Label ID="LabelNewPAD" runat="server" Text='<%# Eval("NewPAD")%>' ></asp:Label>
                            </td>
                            <td class="label align-center" >Is Allocated </td>
                            <td class="label align-center">
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("IsAllocated ")%>' ></asp:Label>
                            </td>
                         </tr>

                         <tr>
                            <td class="label align-center w20">PAD</td>
                            <td class="label align-center w20"><asp:Label ID="LabePAD" runat="server" Text='<%# Eval("PAD")%>'></asp:Label></td>
                            <td class="label align-center w20">SAD</td>
                            <td class="label align-center w20"><asp:Label ID="LabelSAD" runat="server" Text='<%# Eval("SAD")%>'></asp:Label></td>
                        </tr>
                         <tr>
                            <td class="label align-center w20">OMU</td>
                            <td class="label align-center w20"><asp:Label ID="LabelOMU" runat="server" Text='<%# Eval("OMU")%>'></asp:Label></td>
                            <td class="label align-center w20">Qty</td>
                            <td class="label align-center w20"><asp:Label ID="LabelQty" runat="server" Text='<%# Eval("Qty")%>'></asp:Label></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
            
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