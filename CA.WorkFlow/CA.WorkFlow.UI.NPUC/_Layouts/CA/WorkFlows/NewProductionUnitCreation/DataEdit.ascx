<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.NPUC.DataEdit" %>
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
    <tr style="display:none">
        <td class="label align-center">
            Choose Employee
            <asp:Button ID="btnPeopleInfo" runat="server" OnClick="btnPeopleInfo_Click" CausesValidation="False"
                CssClass="hidden" />
        </td>
        <td class="pftd value" colspan="3">
            <cc1:CAPeopleFinder ID="cpfUser" runat="server" AllowTypeIn="true" MultiSelect="false"
                CssClass="ca-people-finder" Width="200" />
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
            <div>
                <input id="txtSupplierName" runat="server" style=" width:200px"/></div>
            <div style="display: none">
                <asp:DropDownList ID="dpSubDivision" runat="server" Visible="false">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblSubDivision" runat="server"></asp:Label>
            </div>
        </td>
       <%-- <td class="label align-center">
            Sub Division<br />
            子部门
        </td>
        <td class="label align-center">
            
        </td>--%>
    </tr>
    <tr>
        <td class="label align-center">
            Supplier No
        </td>
        <td class="label align-center">
            <div>
                <input id="txtSupplierNo" runat="server" /></div>
        </td>
        <td class="label align-center">
            No.of Existing PU
        </td>
        <td class="label align-center">
            <div>
                <input id="txtPUNO" runat="server" /></div>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            New Production Unit Name
        </td>
        <td class="label align-center">
            <div>
                <input id="txtProductionUnitName" runat="server" /></div>
        </td>
        <td class="label align-center">
            Is Mondial Factory
        </td>
        <td class="label align-center">
            <asp:DropDownList ID="ddlIsMondial" runat="server" Width="60px">
                <asp:ListItem Text="" Value=""></asp:ListItem>
                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                <asp:ListItem Text="No" Value="No"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Reason
        </td>
        <td class="pftd value" colspan="3">
            <div>
                <textarea id="txtReason" runat="server" rows="4" cols="4"></textarea></div>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Upload Supplier Profile&Audit Assessment Form
        </td>
        <td class="pftd value" colspan="3">
            <QFL:FormAttachments runat="server" ID="attacthment">
            </QFL:FormAttachments>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Template
        </td>
        <td class="pftd value" colspan="3">
            <a href="Tem/Blank Supplier profile - 2011 9 8.xls" target="_blank">Blank Supplier profile
                - 2011 9 8.xls</a><br />
            <br />
            <a href="Tem/Garment Supplier assessment audit form 5 Dec 11-.doc" target="_blank">Garment
                Supplier assessment audit form 5 Dec 11-.doc</a>
        </td>
    </tr>
</table>
<script type="text/javascript" src="jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        BindPeopleFind();
    });
    function BindPeopleFind() {
        $('#<%=this.cpfUser.ClientID %>' + '_checkNames').click(function () {
            $("#<%=this.btnPeopleInfo.ClientID %>").click();
        });
    }
    function CheckSubmit() {
        $("#TitleTable .wrapdiv").removeClass("wrapdiv");
        CreateForbidDIV();
        var result = true;
        var msg = "";
        var $txtSupplierName = $("input[id$='txtSupplierName']");
        if ($txtSupplierName.val() == "") {
            if (!$txtSupplierName.parent().hasClass("wrapdiv")) {
                $txtSupplierName.parent().addClass("wrapdiv");
            }
            msg += "Please input the Supplier Name .\n";
            result = false;
        }
        var $txtSupplierNo = $("input[id$='txtSupplierNo']");
        if ($txtSupplierNo.val() == "") {
            if (!$txtSupplierNo.parent().hasClass("wrapdiv")) {
                $txtSupplierNo.parent().addClass("wrapdiv");
            }
            msg += "Please input the Supplier No .\n";
            result = false;
        }
        var $txtPUNO = $("input[id$='txtPUNO']");
        if ($txtPUNO.val() == "") {
            if (!$txtPUNO.parent().hasClass("wrapdiv")) {
                $txtPUNO.parent().addClass("wrapdiv");
            }
            msg += "Please input the No.of Existing PU .\n";
            result = false;
        }
        var $txtProductionUnitName = $("input[id$='txtProductionUnitName']");
        if ($txtProductionUnitName.val() == "") {
            if (!$txtProductionUnitName.parent().hasClass("wrapdiv")) {
                $txtProductionUnitName.parent().addClass("wrapdiv");
            }
            msg += "Please input the Production Unit Name .\n";
            result = false;
        }
        var $ddlIsMondial = $("#ctl00_PlaceHolderMain_ListFormControl1_DataForm_ddlIsMondial");
        if ($ddlIsMondial.val() == "") {
            msg += "Please select the Mondial Supplier .\n";
            result = false;
        }
        var $txtReason = $("#ctl00_PlaceHolderMain_ListFormControl1_DataForm_txtReason");
        if ($txtReason.val() == "") {
            if (!$txtReason.parent().hasClass("wrapdiv")) {
                $txtReason.parent().addClass("wrapdiv");
            }
            msg += "Please input the Reason .\n";
            result = false;
        }
        var $AttachmentsTable = $("#idAttachmentsTable");
        if ($AttachmentsTable.find("td").length == 0) {
            msg += "Please upload supplier profile .";
            result = false;
        }
        if (msg != "") {
            alert(msg);
        }
        if (!result) {
            ClearForbidDIV();
        }
        return result;
    }
</script>
