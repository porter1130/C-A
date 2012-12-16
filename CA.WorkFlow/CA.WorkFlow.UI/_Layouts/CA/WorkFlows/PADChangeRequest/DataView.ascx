<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI._Layouts.CA.WorkFlows.PADChangeRequest.DataView" %>

<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<table id="table_comment" class="ca-workflow-form-table full-width">
                <tr>
                    <td class="label align-center w25">
                        Remarks<br />
                        备注:
                    </td>
                    <td class="value" id="comment-task">
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
</table>
<br />
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
 <br />
  <div class="trace-list-grid full-width"> <cc1:TaskTraceList ID="Trace1" runat="server" BorderColor="#999999" 
         BorderWidth="1px" GridLines="Horizontal" /></div>