<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" EnableEventValidation="false" 
MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.TR.NewForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>

<%@ Register Src="DataForm.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Travel Request Form
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Travel Request Form
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <qfl:listformcontrol id="ListFormControl1" runat="server" formmode="New">
            <div class="ca-workflow-form-buttons noPrint">
                <QFC:StartWorkflowButton ID="StartWorkflowButton1" WorkflowName="New Travel Request Workflow2" runat="server" Text="Submit" />
                <QFC:StartWorkflowButton ID="StartWorkflowButton2" WorkflowName="New Travel Request Workflow2" runat="server" Text="Save" CausesValidation="false"/>
                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
                <input type="button" value="Claim" onclick="location.href = '/WorkFlowCenter/default.aspx'" class="hidden" />
            </div>
            <uc1:DataForm ID="DataForm1" runat="server" RequireValidation="true" />
            <SharePoint:FormDigest ID="FormDigest1" runat="server">
            </SharePoint:FormDigest>
        </qfl:listformcontrol>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
    <div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
        <div class="middle">
            注：<br />
            1. 一经批准，出差申请表会直接转到前台进行火车票及携程驻公司员工进行飞机票和酒店的订位，订位完成后他们会和你确认详细行程。<br />
            2. 为证明员工在外出差的天数及申请出差用餐补贴，员工出差回来后请将本表、来回机票、登机牌，住宿发票及酒店入住登记记录等原件附在报销申请表后一并提交。
            <br />
            Note:<br />
            1. Once approved, the Travel Request Form will be forwarded to Receptionist for booking of train and C-trip Onsite for booking of flight and hotel. Detail itinerary will be sent to you once bookings are done.<br />
            2. In order to evidence the number of days spent out of Shanghai for reimbursement of meal allowances, the employee should attach this form together with the air-ticket, boarding pass, hotel invoice, check-in and check-out record, etc to the Expense Claim Form and submit all of them after returning from the business trip.
            <br />
            <br />
            <br />
            <a href="/WorkFlowCenter/FlowCharts/TravelRequest.doc">Click here to view the flowchart of the workflow</a>
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
</asp:Content>