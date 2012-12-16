<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="DisplayForm.aspx.cs" Inherits="CA.WorkFlow.UI._Layouts.CA.WorkFlows.PADChangeRequest.DisplayForm" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"  Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
PAD Change Request
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <style type="text/css">
        .SADLeft
        {
            float:left;
            width:30px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
PAD Change Request
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="Display">
<div class="ca-workflow-form-buttons noPrint">
            <input type="button" value="Back" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
        </div>
    <table id="table_comment" class="ca-workflow-form-table full-width">
                <tr>
                    <td class="label align-center w25">
                        Remarks<br />
                        备注:
                    </td>
                    <td class="value" id="comment-task">
                    <QFL:FormField ID="RemarksField" runat="server" FieldName="Remarks"  ControlMode="Display" ></QFL:FormField>
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
         <tr> <td class="label align-center w25" style="width:200px;">Count</td>
        <td class="label align-left w20" colspan="5" >
            <asp:Label ID="LabelCount" runat="server" Text="0"></asp:Label>
        </td>
        </tr>
       </table>
            <br />
            
            <asp:Panel ID="PanelNew" Visible="false" runat="server">
            <asp:Repeater ID="RepeaterPOData" runat="server">
                <ItemTemplate>
                      <table class="ca-workflow-form-table">
                     <tr>
                        <td class="label align-center" colspan="1">PO Number</td>
                        <td class="label align-left" colspan="5"><%# Eval("PONumber")%></td>
                     </tr>
                    <tr>
                        <td class="label align-center" colspan="1">当前到货日期<br />Current PAD</td>
                        <td class="label align-left" colspan="5">
                       <%# DateFormate(Eval("CurrentPAD").ToString())%>
                    </td>
                   </tr>
                    <tr>
                        <td class="label align-center" colspan="1">新到货日期<br />Proposed New PAD</td>
                        <td class="label align-left" colspan="5">
                        <%# DateFormate(Eval("NewPAD").ToString())%>
                        
                        </td>
                     </tr>
                    <tr>
                        <td class="label align-center" colspan="1">审批状态<br />Approve Status</td>
                        <td class="label align-left" colspan="5">
                            <%# SetItemApproveState(Eval("IsApprove").ToString())%>
                        </td>
                     </tr>
                    <tr>
                        <td class="label align-center" colspan="1">是否己更新至SAP<br />Is updated to SAP</td>
                        <td class="label align-left" colspan="5" style="color:Red">
                            <%# SetPostStatus(Eval("IsSuccess").ToString())%>
                        </td>
                     </tr>
                    <tr>
                        <td class="label align-center w20">
                        Supplier Name
                     </td>
                    <td class="label align-center w20">
                            <%# Eval("SupplierName")%>
                    </td>
                        <td class="label align-center w22" style="width:120px;">PAD year\week</td>
                        <td class="label align-center w20" id="Td4">
                        <div class="SADLeft">
                            <%# Eval("PADyear")%>
                        </div>
                         <div class="SADLeft">\</div>
                        <div class="SADLeft">
                           <%# Eval("PADweek")%>
                        </div>
                   </td>
                        <td class="label align-center w10" style="width:80px;">OSP</td>
                        <td class="label align-center w20">
              <%# Eval("OSP")%>
        </td>
                    </tr>
                    <tr>
                        <td class="label align-center w20">Story</td>
                        <td class="label align-center w20" ><%# Eval("ValueForStory")%></td>
                        <td class="label align-center w22" style="width:120px;">SAD year\week</td>
                        <td class="label align-center w20" id="Td5">
                            <div class="SADLeft">
                                <%# Eval("SADyear")%>
                            </div>
                            <div class="SADLeft">\</div>
                             <div class="SADLeft">
                                <%# Eval("SADweek")%>
                             </div>
                         </td>
                        <td class="label align-center w10" style="width:80px;">OMU</td>
                        <td class="label align-center w20"><%# Eval("OMU")%></td>
                    </tr>
                    
                <tr>
                    <td class="label align-center w20">POQty</td>
                    <td class="label align-center w20" ><%# Eval("PoQty")%></td>
                    <td class="label align-center w22" style="width:120px;">Style Number</td>
                    <td class="label align-center w20" id="Td1"><%# Eval("StyleNumber")%></td>
                    <td class="label align-center w10" style="width:80px;"></td>
                    <td class="label align-center w20"></td>
                </tr>
                    <tr>
                        <td class="label align-center" colspan="6"></td>
                    </tr>
            </table>
                </ItemTemplate>
            </asp:Repeater>
            </asp:Panel>
            
            <asp:Panel ID="PanelOldWorkFlow" Visible="false" runat="server">
            <!----OldData---->
            <table class="ca-workflow-form-table" >
              <tr>
              <td class="label align-center" style="width:110px; height:25px;">PO Number</td>
              <td class="label align-left" id="tdinput0"><QFL:FormField ID="PONumberField" runat="server" FieldName="PONumber" ControlMode="Display" >
                        </QFL:FormField>
              </td>
              </tr>

              <tr>
              <td class="label align-center" >当前到货日期<br />Current PAD
              </td>
              <td class="label align-left" id="tdinput1"><QFL:FormField ID="CurrentPADField" runat="server" FieldName="CurrentPAD" ControlMode="Display" >
                        </QFL:FormField></td>
              </tr>

              <tr>
              <td class="label align-center" >新到货日期<br />Proposed New PAD</td>
                <td class="label align-left" id="tdinput2"><QFL:FormField ID="NewPADField" runat="server" FieldName="NewPAD" ControlMode="Display"  >
                        </QFL:FormField></td>
              </tr>
            </table>
            <br />
            <table class="ca-workflow-form-table">
<tr> <td class="label align-center w20" style="width:120px;">Supplier Name
        </td>
        <td class="label align-center w20" ><QFL:FormField ID="SupplierNameField" runat="server" FieldName="SupplierName" ControlMode="Display"  >
                        </QFL:FormField>
        </td>
        <td class="label align-center w22" style="width:120px;">PAD year\week
        </td>
        <td class="label align-center w20"><QFL:FormField ID="PADyearField" runat="server" FieldName="PADyear" ControlMode="Display"  >
                        </QFL:FormField>\<QFL:FormField ID="PADweekField" runat="server" FieldName="PADweek" ControlMode="Display"  >
                        </QFL:FormField></td>
        <td class="label align-center w25" style="width:120px;">OSP</td>
        <td class="label align-center w20"><QFL:FormField ID="OSPField" runat="server" FieldName="OSP" ControlMode="Display"  >
                        </QFL:FormField></td>
        </tr>
        <tr> <td class="label align-center w20" style="width:120px;">Story
        </td>
        <td class="label align-center w20" ><QFL:FormField ID="ValueforStoryField" runat="server" FieldName="ValueForStory" ControlMode="Display"  >
                        </QFL:FormField>
        </td>
        <td class="label align-center w22" style="width:120px;">SAD year\week
        </td>
        <td class="label align-center w20"><QFL:FormField ID="SADyearField" runat="server" FieldName="SADyear" ControlMode="Display"  >
                        </QFL:FormField>\<QFL:FormField ID="SADweekField" runat="server" FieldName="SADweek" ControlMode="Display"  >
                        </QFL:FormField></td>
        <td class="label align-center w25" style="width:120px;">OMU</td>
        <td class="label align-center w20"><QFL:FormField ID="OMUField" runat="server" FieldName="OMU" ControlMode="Display"  >
                        </QFL:FormField></td>
        </tr>
        

</table>
            <!-------->
            </asp:Panel>
<br />
<br />
  <div class="trace-list-grid full-width"> <cc1:TaskTraceList ID="Trace1" runat="server" BorderColor="#999999" BorderWidth="1px" GridLines="Horizontal" /></div>
<br />
<br />
</QFL:ListFormControl>   
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
