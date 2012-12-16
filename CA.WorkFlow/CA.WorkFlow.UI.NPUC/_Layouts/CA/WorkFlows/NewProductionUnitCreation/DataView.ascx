<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.NPUC.DataView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .ca-workflow-form-table td
    {
        padding: 5px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        text-align: center;
        margin: 0;
        height: 25px;
        line-height: 15px;
    }
    .pftd td
    {
        padding: 0px;
        border: none;
        text-align: left;
        margin: 0;
        height: auto;
        line-height: normal;
    }
    .pftd img
    {
        margin-top: 2px;
    }
    .ca-workflow-form-table
    {
        margin-top: 20px;
    }
    .ca-workflow-form-table td input
    {
        width: 80%;
    }
    .wrapdiv
    {
        padding: 2px;
        border: 1px solid red;
    }
</style>
<table class="ca-workflow-form-table" id="TitleTable">
    <tr>
        <td colspan="4">
            <h3>
                New Production Unit Creation Form
            </h3>
        </td>
    </tr>
    <tr>
        <td class="label align-center w20">
            Dept
        </td>
        <td class="label align-center w30">
            <asp:Label ID="lbDepartment" runat="server"></asp:Label>
        </td>
        <td class="label align-center w20">
            Applicant
        </td>
        <td class="label align-center w30">
            <asp:Label ID="lbApplicant" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Supplier Name
        </td>
        <td class="label align-center" colspan="3">
            <asp:Label ID="lblSupplierName" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Supplier No
        </td>
        <td class="label align-center">
            <asp:Label ID="lblSupplierNo" runat="server" Text=""></asp:Label>
        </td>
        <td class="label align-center">
            No.of Existing PU
        </td>
        <td class="label align-center">
            <asp:Label ID="lblPUNO" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            New Production Unit Name
        </td>
        <td class="label align-center">
            <asp:Label ID="lblProductionUnitName" runat="server" Text=""></asp:Label>
        </td>
        <td class="label align-center">
            Is Mondial Factory
        </td>
        <td class="label align-center">
            <asp:Label ID="lblIsMondial" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Reason
        </td>
        <td class="pftd value" colspan="3">
            <asp:Label ID="lblReason" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            upload supplier profile&Audit Assessment form
        </td>
        <td class="pftd value" colspan="3">
            <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Display">
            </QFL:FormAttachments>
        </td>
    </tr>
</table>
