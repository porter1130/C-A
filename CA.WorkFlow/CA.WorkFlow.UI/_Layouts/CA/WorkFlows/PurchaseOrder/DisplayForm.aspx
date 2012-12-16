<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayForm.aspx.cs" MasterPageFile="~/_Layouts/CA/Layout.Master" 
Inherits="CA.WorkFlow.UI.PurchaseOrder.DisplayForm" %>

<%@ Register Src="DataView.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Purchase Order Form
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
    <script type="text/javascript">
        function popexcel(url) {
            var w = window.open(url, '_blank');
            w.location.href = url;
        }
        $(document).ready(function () {
            $('#<%= DDLIsShowACNumber.ClientID %>').change(function () {
                if ($(this).children("option:selected").val() == "1") {
                    $("#ca_po_detail").find(".IsShowACNumber").show();
                    $(".ca-purchaseorder-bigger").css("width", "1290px");
                }
                else {
                    $("#ca_po_detail").find(".IsShowACNumber").hide();
                    $(".ca-purchaseorder-bigger").css("width", "1080px");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    Purchase Order Form
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="PlaceHolderMain" runat="server">
 <QFL:ListFormControl ID="ListFormControl3" runat="server" FormMode="Display">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <div class="ca-workflow-form-buttons noPrint">
            <table cellpadding="10" cellspacing="10">
                <tr>
                    <td>
                        Show ACNumber?&nbsp;&nbsp; <asp:DropDownList ID="DDLIsShowACNumber" runat="server">
                        <asp:ListItem Value="0">No</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td id="ExportType" runat="server">Export type:&nbsp;&nbsp; <asp:DropDownList 
                            ID="DDLPoType" AutoPostBack="true" runat="server" 
                            onselectedindexchanged="DDLPoType_SelectedIndexChanged"><asp:ListItem Text="General PO"></asp:ListItem><asp:ListItem Text="Maintenance PO"></asp:ListItem></asp:DropDownList></td>
                    <td><asp:Button ID="ButtonExport" runat="server" Text="PO export" onclick="ButtonExport_Click"></asp:Button></td>
                    <td><asp:Button ID="ButtonBack" runat="server" Text="Back" onclick="ButtonBack_Click" /></td>
                </tr>
            </table>
        </div>
        <uc1:DataForm ID="DataForm1" runat="server" />
        <div class="noPrint">
            <uc2:TaskTrace ID="TaskTrace1" runat="server" />
        </div>
        <SharePoint:FormDigest ID="FormDigest3" runat="server">
        </SharePoint:FormDigest>
    </qfl:listformcontrol>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
