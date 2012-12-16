<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs"MasterPageFile="~/_Layouts/CA/Layout.Master"
Inherits="CA.WorkFlow.UI.PurchaseRequest.EditForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Src="SelectItem.ascx" TagName="DataForm" TagPrefix="uc2" %>
<%@ Register TagPrefix="QFC" Namespace="QuickFlow.UI.Controls" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Purchase Request Form
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/jquery-ui.css" />
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/jquery-ui.custom.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery.bgiframe.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Purchase Request Form
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
            <div class="ca-workflow-form-buttons noPrint">
                <cc1:CAActionsButton ID="Actions" runat="server" CausesValidation="false" />
                <input type="button" value="Back" onclick="window.location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx';" />
            </div>
            <uc1:DataForm ID="DataForm1" runat="server" RequireValidation="true"  />
            <cc1:TaskTraceList ID="TaskTraceList1" runat="server">
            </cc1:TaskTraceList>
            <SharePoint:FormDigest ID="FormDigest2" runat="server">
            </SharePoint:FormDigest>            
        </QFL:ListFormControl>
        <uc2:DataForm ID="dfSelectItem" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
    <div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
        <div class="middle">
            注:<br />
            1. 季度定单只能在二月, 五月, 八月及十一月使用, 并且一年只能下四次单 (每季度一次)。 若需要购买平日营运所需的物品, 请选择”Daily Request”。<br />
            2. 退货申请的相关PO号码会由总部填写, 在提交申请表时把此栏留空即可。<br />
            3. 请选择或填写正确的的产品代码, 所需的数量, 正确的成本中心及购买申请理由, 否则你的申请会被退回。<br />
            4. 请选择或填写正确的产品代码。若有新产品需在系统中增加, 只有工程部采购团队可以建立新产品代码。<br />
            5. 若需询问申请价格和送货日期，请与工程部采购团队联系。<br />

            Note:<br />
            1. Please note that Quarterly Orders can only be placed in February, May, August, and November (4 times per year, ie. 1 time per quarter).<br />
            2. For Return Request, the related PO numbers will be filled-in by Head Office; just leave it blank when submit the request.<br />
            3. Please select or fill-in the correct item code, the required quantity, correct cost center and reasons for the Purchase Request before submission.  Otherwise, the Purchase Request will be rejected and returned to you.<br />
            4. Please select or fill in the correct item code. Only Construction Procurement Team has the right to setup additional items in EWF if required.<br />
            5. Any cost and delivery information please contact with Construction Procurement Team.
            <br />
            <br />
            <a href="/WorkFlowCenter/FlowCharts/PurchaseRequest.doc">Click here to view the flowchart of the workflow</a>
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
</asp:Content>