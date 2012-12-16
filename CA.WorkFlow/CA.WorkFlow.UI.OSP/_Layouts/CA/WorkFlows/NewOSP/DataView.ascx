<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.NewOSP.DataView" %>

<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>

   <script type="text/javascript">
       $(document).ready(function () {
           if ("<%= isDisplayStep%>" == "False")//是审批流程
           {
               $(".IsSuccess").each(function () {
                   if ($(this).children("input").val() == "1") {//已经修改过的就不显示
                       $(this).parents(".ca-workflow-form-table").hide();
                   }
               });
           }
       });

       function beforeSubmit(obj) {
           if (!CheckRejectComments(obj.value)) {
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
      <td class="label align-center w25" style="width:200px;">申请人<br />
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
        <td class="label align-center w25" style="width:120px;">姓名<br />Name</td>
        <td class="label align-center w20">
            <QFL:FormField ID="ChineseNameField" runat="server" FieldName="ChineseName" ControlMode="Display"  ></QFL:FormField>
        </td>
      </tr>
           <tr>
            <td class="label align-center w25">
                Workflow NO.
            </td>
            <td class="value" id="Td2" colspan="5">
                <QFL:FormField ID="FormFieldWorkflowNo" runat="server" FieldName="Title" ControlMode="Display"  ></QFL:FormField>
            </td>
        </tr>
           <tr>
            <td class="label align-center w25">
                Count
            </td>
            <td class="value" id="Td2" colspan="5">
                <asp:Label ID="LabelCount" runat="server" Text="0"></asp:Label>
            </td>
        </tr>
</table>
<br/>
            <asp:Repeater ID="RepeaterPOData" runat="server">
                <ItemTemplate>
                      <table class="ca-workflow-form-table POCount">
                         <tr>
                            <td class="label align-center" colspan="1">Style No.</td>
                            <td class="label align-left" colspan="5">
                                <asp:Label ID="LabelStyleNO" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                <asp:HiddenField ID="HiddenFieldID" runat="server" Value='<%# Eval("ID")%>'></asp:HiddenField>
                                 <div class="IsSuccess" isApprove='<%# Eval("IsApproved").ToString()%>'>
                                        <asp:HiddenField ID="HiddenFieldISSuccess" runat="server" Value='<%# Eval("IsSuccess")%>'></asp:HiddenField>
                                 </div>
                            </td>
                         </tr>
                        <tr>
                            <td class="label align-center" colspan="1">Original Osp</td>
                            <td class="label align-left" colspan="5">
                            <%# Eval("OriginalOsp")%>
                        </td>
                       </tr>
                        <tr>
                            <td class="label align-center" colspan="1">New OSP</td>
                            <td class="label align-left" colspan="5"> <asp:Label ID="LabelNewOSP" runat="server" Text='<%# Eval("NewOSP")%>'></asp:Label>
                                <asp:DropDownList ID="DropDownListApprove" Visible='<%# isApproveStep%>' Enabled='<%# Eval("IsSuccess").ToString()=="1"?false:true%>' Width="80px" runat="server" >
                                    <asp:ListItem Text="Approve" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Reject" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                         </tr>
                        <tr>
                            <td class="label align-center" colspan="1">PO</td>
                            <td class="label align-left" colspan="5"><%# Eval("PONO")%></td>
                         </tr>

                         
                        <tr id="isUpdated" runat="server" visible='<%# isDisplayStep%>' >
                            <td class="label align-center" colspan="1">Approved Satus</td>
                            <td class="label align-left" colspan="5"> <%# GetApprovedSatus(Eval("IsApproved ").ToString())%>
                            </td>
                         </tr>

                        <tr id="isDisplayStep" runat="server" visible='<%# isDisplayStep%>' >
                            <td class="label align-center" colspan="1">Is updated to SAP</td>
                            <td class="label align-left" colspan="5" style="color:Red"><%# Eval("IsSuccess").ToString()=="1"?"YES":"NO"%></td>
                         </tr>
                         <tr>
                            <td class="label align-center w20">Sub Div </td>
                            <td class="label align-center w20" ><%# Eval("SubDiv")%></td>
                            <td class="label align-center w22" style="width:120px;">Class</td>
                            <td class="label align-center w20" id="Td4"><%# Eval("Class")%></td>
                            <td class="label align-center w10" style="width:80px;">Qty</td>
                            <td class="label align-center w20"><%# Eval("Qty")%></td>
                        </tr>
                         <tr>
                            <td class="label align-center">Current OMU</td>
                            <td class="label align-center" ><%# Eval("CurrentOMU")%></td>
                            <td class="label align-center" style="width:120px;">Created by</td>
                            <td class="label align-center" id="Td5"><%# Eval("CreatedBy")%></td>
                            <td class="label align-center" style="width:80px;">PAD</td>
                            <td class="label align-center"><%# Eval("PAD")%></td>
                       </tr>
                         <tr>
                       <td class="label align-center">SAD</td>
                       <td class="label align-center" ><%# Eval("SAD")%></td>
                       <td class="label align-center">GR </td>
                       <td class="label align-center" id="Td1"><%# Eval("GR")%></td>
                       <td class="label align-center">Allocated date</td>
                       <td class="label align-center"><%# Eval("AllocatedDate")%></td>
                    </tr>
                    <tr>
                        <td class="label align-center">New OMU</td>
                        <td class="label align-center"><%# Eval("NewOMU")%></td>
                        <td class="label align-center">OMU Reduction</td>
                        <td class="label align-center"><%# Eval("OMUReduction")%></td>
                        <td class="label align-center"></td>
                        <td class="label align-center"></td>
                    </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>