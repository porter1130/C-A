<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="BatchApproveForm.aspx.cs" Inherits=" CA.WorkFlow.UI.PADChangeRequest.BatchApproveForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<%@ Register Src="DataView.ascx" TagName="DataViewForm" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
PAD Change Request
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server"> 
    <script type="text/javascript">
        $(document).ready(function () {
            $(".IsSuccess").each(function () {
                if ($(this).children("input").val() == "1") {//已经修改过的就不显示
                    $(this).parents(".ca-workflow-form-table").hide();
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="Display">
            <div class="ca-workflow-form-buttons noPrint">
                <cc1:CAActionsButton ID="Actions" runat="server"  />
                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx'" />
            </div>
            <table id="table_comment" class="ca-workflow-form-table full-width">
                <tr>
                    <td class="label align-center w25">
                        Comments<br />
                        批注:
                    </td>
                    <td class="value" id="comment-task">
                        <QFL:TaskPanel runat="server" ID="task1">
                            <QFL:CommentTaskField runat="server" ID="CommentTaskField1" ControlMode="Edit"/>
                        </QFL:TaskPanel>
                    </td>
                </tr>
            </table>
            <br />
         
         <!--begin Body-->

            <table id="table1" class="ca-workflow-form-table full-width">
                <tr>
                    <td class="label align-center w25">
                        Remarks<br />
                        备注:
                    </td>
                    <td class="value" id="Td1">
                    <QFL:FormField ID="RemarksField" runat="server" FieldName="Remarks" ControlMode="Display">
                        </QFL:FormField>
                    </td>
                </tr>
            </table>
            <br />
            <table class="ca-workflow-form-table">
                <tr> <td class="label align-center w25" style="width:200px;">申请人<br />
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
                <tr> <td class="label align-center w25" style="width:200px;">Count</td>
        <td class="label align-left w20" colspan="5" >
            <asp:Label ID="LabelCount" runat="server" Text="0"></asp:Label>
        </td>
        </tr>
            </table>
            <br />
                 <asp:Repeater ID="RepeaterPOData" runat="server">
                <ItemTemplate>
                      <table class="ca-workflow-form-table">
                     <tr>
                        <td class="label align-center" colspan="1">PO Number</td>
                        <td class="label align-left" colspan="5">
                            <asp:Label ID="LabelPONO" runat="server" Text='<%# Eval("PONumber")%>'></asp:Label>
                            <div class="IsSuccess" isApprove='<%# Eval("IsApprove").ToString()%>'>
                                <asp:HiddenField ID="HiddenFieldIsSuccess" runat="server" Value='<%# Eval("IsSuccess")%>' />
                            </div>
                        </td>
                     </tr>
                    <tr>
                        <td class="label align-center" colspan="1">当前到货日期<br />Current PAD</td>
                        <td class="label align-left" colspan="5">
                        <asp:Label ID="LabelCurrentPAD" runat="server" Text='<%# DateFormate(Eval("CurrentPAD").ToString())%>'></asp:Label>
                    </td>
                   </tr>
                    <tr>
                        <td class="label align-center" colspan="1">新到货日期<br />Proposed New PAD</td>
                        <td class="label align-left" colspan="5">
                            <span style="float:left;">
                                <asp:Label ID="LabelNewPAD" runat="server" Text='<%# DateFormate(Eval("NewPAD").ToString())%>'></asp:Label>
                            </span>
                            <span style="float:left;">&nbsp;&nbsp;&nbsp; 
                                <asp:DropDownList ID="DDLApproveStatus" Width="100px" runat="server">
                                    <asp:ListItem Value="1" Text="Approve"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Reject"></asp:ListItem>
                                </asp:DropDownList>
                             </span>
                          <asp:HiddenField ID="HiddenFieldID" runat="server" Value='<%# Eval("ID")%>' />
                          <asp:HiddenField ID="HiddenFieldIsNeedApprove" runat="server" Value='<%# Eval("IsApprove").ToString()%>' />
                        </td>
                     </tr>
                    <tr>
                    <td class="label align-center w20">
                        Supplier Name
                     </td>
                    <td class="label align-center w20" >
                             <%# Eval("SupplierName")%>
                    </td>
                    <td class="label align-center w22" style="width:120px;">PAD year\week
                    </td>
        <td class="label align-center w20" id="Td4">
            <span style="float:left;">
                <%# Eval("PADyear")%>
            </span>
            <span style="float:left;">
                  <%# Eval("PADweek")%>
            </span>
       </td>
        <td class="label align-center w10" style="width:80px;">OSP</td>
        <td class="label align-center w20">
              <%# Eval("OSP")%>
        </td>
                </tr>
                    <tr>
                    <td class="label align-center w20">Story
        </td>
        <td class="label align-center w20" >
            <%# Eval("ValueForStory")%>
        </td>
        <td class="label align-center w22" style="width:120px;">SAD year\week
        </td>
        <td class="label align-center w20" id="Td5">
            <span style="float:left;">
             <%# Eval("SADyear")%>
            </span>
             <span style="float:left;">
             <%# Eval("SADweek")%>
             </span>
         </td>
        <td class="label align-center w10" style="width:80px;">OMU</td>
        <td class="label align-center w20">
             <%# Eval("OMU")%>
             </td>
                </tr>
      
                <tr>
                    <td class="label align-center w20">PoQty</td>
                    <td class="label align-center w20" ><%# Eval("PoQty").ToString()%></td>
                    <td class="label align-center w22" style="width:120px;">Style Number</td>
                    <td class="label align-center w20" id="Td1"><%# Eval("StyleNumber")%></td>
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

         <!--end Body-->
         
            <br />
  <div class="trace-list-grid full-width"> <cc1:TaskTraceList ID="Trace1" runat="server" BorderColor="#999999"  BorderWidth="1px" GridLines="Horizontal" /></div>

            <SharePoint:FormDigest ID="FormDigest3" runat="server">
            </SharePoint:FormDigest>
        
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