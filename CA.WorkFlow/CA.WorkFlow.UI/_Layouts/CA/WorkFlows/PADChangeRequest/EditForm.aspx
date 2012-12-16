<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" Inherits="CA.WorkFlow.UI._Layouts.CA.WorkFlows.PADChangeRequest.EditForm" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
PAD Change Request
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
 <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
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
            <table class="ca-workflow-form-table" >
              <tr>
              <td class="label align-center" style="width:110px; height:25px;">PO Number</td>
              <td class="label align-left" id="tdinput0"><asp:Label ID="lblPONumber" runat="server"></asp:Label><span style="display:none;"><QFL:FormField ID="PONumberField" runat="server" FieldName="PONumber" ControlMode="Display" >
                        </QFL:FormField></span>
              </td>
              </tr>

              <tr>
              <td class="label align-center" >当前到货日期<br />Current PAD
              </td>
              <td class="label align-left" id="tdinput1"><asp:Label ID="lblCurrentPAD" runat="server"></asp:Label><span style="display:none;"><QFL:FormField ID="CurrentPADField" runat="server" FieldName="CurrentPAD" ControlMode="Display" >
                        </QFL:FormField></span></td>
              </tr>

              <tr>
              <td class="label align-center" >新到货日期<br />Proposed New PAD</td>
                <td class="label align-left" id="tdinput2"><QFL:FormField ID="NewPADField" runat="server" FieldName="NewPAD"  >
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
        <td class="label align-center w25" style="width:120px;">OMU</td>
        <td class="label align-center w20"><QFL:FormField ID="OMUField" runat="server" FieldName="OMU" ControlMode="Display"  >
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
        <td class="label align-center w25" style="width:120px;">OSP</td>
        <td class="label align-center w20"><QFL:FormField ID="OSPField" runat="server" FieldName="OSP" ControlMode="Display"  >
                        </QFL:FormField></td>
        </tr>
       

</table>
            <SharePoint:FormDigest ID="FormDigest3" runat="server">
            </SharePoint:FormDigest>
            </QFL:ListFormControl>

            <br />
           
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
