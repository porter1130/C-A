<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true"
    CodeBehind="DisplayForm.aspx.cs" Inherits="CA.WorkFlow.UI.CashAdvanceRequest.DisplayForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Register Src="DataEdit.ascx" TagName="DataForm" TagPrefix="uc1" %>
    <%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Cash Advance Request Form
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Cash Advance Request Form
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
            <div class="ca-workflow-form-buttons noPrint">
                <input type="button" value="Back" onclick="window.history.go(-1)" />
            </div>
            <uc1:DataForm ID="DataForm1" runat="server" ControlMode="Display" />
            <uc2:TaskTrace id="TaskTrace1" runat="server" />
            <SharePoint:FormDigest ID="FormDigest3" runat="server">
            </SharePoint:FormDigest>
        </QFL:ListFormControl>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
    <div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
        <div class="middle">
            Note:<br />
            1. Only cash advance below RMB 2,000 can be paid out in cash. Cash advance above
            RMB 2,000 will be transferred to your bank account in two weeks after Finance received
            the approved request.
            <br />
            预借现金在2000元人民币以下的，可以领取现金。超出2000元的申请，财务会在收到获批申请的两周内安排转帐到申请人的本人工资账户。
            <br />
            2. For urgent cash advance, additional approval is required from CFO. Valid reasons
            must be stated for urgent request. If not, the request will be treated as normal.
            Once urgent request is approved, cash advance will be paid out in three working
            days.<br />
            紧急借款申请，需要提供充分的理由，并需获得首席财务官的加批，否则作为一般申请处理。获得加批后，紧急借款申请将在三个工作日内安排付款。
            <br />
            3. Finance handles only RMB cash advance.
            <br />
            财务仅提供人民币借款。<br />
            4.Attached the purpose or quotation of cash advance is required.<br />
            申请借款时必须上传阐述借款预算和用途的文件作为附件。<br />
            5. Cash advance must be used for the purpose(s) stated on the Cash Advance Application
            Form.
            <br />
            员工借款只能用于借款申请表中所列的用途。<br />
            6. According to Company Policy, cash advance must be cleared one month after the
            cash advance is released to you. If you have unclear cash advance, Finance has the
            right to reject your cash advance application.
            <br />
            依照公司相关政策，员工向公司预借的现金，必须在拿到后的一个月内报销结清。若员工有未结清的借 款，财务将拒绝新的借款申请。<br />
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
</asp:Content>
