<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequestGeneral.NewForm" %>

<%@ Register src="UserInfo.ascx" tagname="UserInfo" tagprefix="uc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %> 
<%@ Register src="DataEdit.ascx" tagname="DataEdit" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Purchase Request - General
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
Purchase Request - General
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <qfl:listformcontrol id="ListFormControl1" runat="server" formmode="New">
    <asp:HiddenField ID="HiddenFieldWorkflowNumber" runat="server" />
        <div id="ca-pr-form">
            <div class="ca-workflow-form-buttons">
                <QFC:StartWorkflowButton ID="StartWorkflowButtonSubmit" WorkflowName="PRGWorkflow" runat="server" Text="Submit" CausesValidation="false"/>
                <QFC:StartWorkflowButton ID="StartWorkflowButtonSave" WorkflowName="PRGWorkflow" runat="server" Text="Save" CausesValidation="false"/>
                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
            </div>
            <uc1:Userinfo ID="Userinfo1" runat="server" />
            <uc2:DataEdit ID="DataEdit1" runat="server" />
            <SharePoint:FormDigest ID="FormDigest1" runat="server"></SharePoint:FormDigest>
        </div>
    </qfl:listformcontrol>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
 <div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
            <div class="middle">
                1.	If exact time of purchase/project to occur is not known yet, please put down your best estimate on the request.  However, please note that the approval obtained here is only valid for 3 months from the approval given date.
                如未能确定购买/项目发生的时间，请填写你估计最准确的发生时间。但请注意这审批申请里所授与的批准，有效期为发出批准当日起的三个月内。
                <br />2.	If the exact cost of purchase/project is not known yet, please state your best estimate on the request.  However, please note that if the final cost > 5% of the approved amount, the related contract has to go through full approval process again.
                如未能确定购买/项目的所需费用，请填写你估计最准确的费用。但请注意如最终的所需费用高于这申请表里所批准的5%，相关的合同需重新走一遍审批流程。
                <br />3.	If the purchase/project involves signing of main-frame contract which total amount of payment will not be stated in the contract, please put down your best estimate of the annual cost.
                如这购买/项目需签署框架合同，即总付款金额不能体现在合同里；请填写你估计最准确的一年总费用。
                <br />4.	Please note that according to Company policy, if cost of purchase/project > rmb100,000, supplier has to be selected through a bidding process.  If bidding is not going to be used, please state your reason on the request.
                跟据公司政策，如购买/项目的总费用超过人民币十万元，供应商必须通过投标的方式挑选。如打算不用投标的方法，请在这申请表里填写原因。
                <br />5.	Please print out the approved request for record.  When sign the contract for the purchase/project approved above; if the final cost is not greater than 5% of the approved cost on the request, the contract need not go through approval process again (please refer to Section II of “Contract Obligation & Agreement Approval Tracking Form”).
                请打印经审批后的申请表来作被案。当签署购买/项目合同时，若合同金额不高于这申请表里批准的5%，该合同不需再通过审批流程（请参看“合同审批跟踪表”里的第二项）。
                <br />6.	The cost center is the one which will bear the cost. For example the cost is incurred in marketing dept. but should be borne by store; the cost center input should be related store.
                成本中心为费用承担部门而非费用发生部门.例如,费用发生在市场部,但需由门店承担,则成本中心需填写为门店.
                <br />7.	The item marked with “*” is the compulsory one which must be filled in.
                标注为”*”的项目为必填项.
            </div>
</div>
</asp:Content>
