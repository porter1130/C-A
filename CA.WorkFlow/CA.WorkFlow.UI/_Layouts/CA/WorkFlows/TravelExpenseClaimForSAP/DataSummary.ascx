<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataSummary.ascx.cs"
    Inherits="CA.WorkFlow.UI.TravelExpenseClaimForSAP.DataSummary" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="System.Data" %>
<style type="text/css">
    .ms-vb2
    {
        padding: 5px;
    }
    .ms-vh2-gridview
    {
        border: #cccccc 1px solid;
        border-top: none;
        padding: 5px;
    }
    #tb_summary
    {
        width: 680px;
        border: #9dabb6 2px solid;
        margin-top: 25px;
        cursor: pointer;
    }
    #tb_summary td
    {
        padding: 7px;
        line-height: 15px;
        border-bottom: #cccccc 1px solid;
    }
    .ms-listviewtable td
    {
        border: #cccccc 1px solid;
        padding: 5px;
        height: auto;
    }
</style>
<asp:Repeater ID="rptWFItemCollection" OnItemDataBound="rptWFItemCollection_ItemDataBound"
    runat="server">
    <HeaderTemplate>
        <table border="0" cellpadding="0" cellspacing="0" id="tb_summary">
            <tr>
                <td colspan="3" class="align-center">
                    <h3>
                        Travel Expense Claim For SAP Details
                    </h3>
                </td>
            </tr>
            <tr>
                <td class="w30 align-left" style="font-weight: bold">
                    <asp:CheckBox ID="cbAllItems" runat="server" onclick="CheckAllItems(this)" />
                    <span style="padding-left: 30px;">SAPNo</span>
                </td>
                <td class="w40 align-left" style="font-weight: bold">
                    <span style="padding-left: 30px;">Travel Expense Claim WorkflowNumber</span>
                </td>
                <td class="align-center" style="font-weight: bold">
                    Post Count
                </td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td class="td_check td_checkitem w30">
                <img src="../images/pixelicious_001.png" alt="Expand Detail Items" align="absmiddle"
                    onclick="DisplayDetails(this)" />
                <asp:CheckBox ID="cbWorkflowItem" runat="server" />
                <span class="span_wftitle">
                    <%#Eval("Title") %> </span>
                <asp:HiddenField ID="hidWorkflowID" runat="server" Value='<%#Eval("Title") %>' />
            </td>
            <td class="align-center w40">
                <asp:Label ID="Label1" runat="server" Text='<%#Eval("TCWorkflowNumber")%>'></asp:Label>
            </td>
            <td class="align-center">
                <asp:Label ID="lblPostItemsCount" runat="server" Text='<%#Eval("PostCount") %>'></asp:Label>
            </td>
        </tr>
        <tr class="tr_details hidden">
            <td colspan="3" style="padding-left: 38px">
                <asp:UpdatePanel ID="upDetails" runat="server">
                    <ContentTemplate>
                        <SharePoint:SPGridView ID="spgvWFItem" runat="server" AutoGenerateColumns="false">
                        </SharePoint:SPGridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td height="5px" colspan='3'>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<br />
<script type="text/javascript">
    var checkItems = '#tb_summary td.td_checkitem';
    $(function () {
    });
</script>
<script type="text/javascript">
    function DisplayDetails(obj) {
        $(obj).parents('tr').first().next().toggle();
        //$(obj).next().toggle();
        if ($(obj).parents('tr').first().next().is(":visible")) {
            $(obj).parents('tr').first().find("td.td_check img").attr("src", "../images/pixelicious_028.png");
            $(obj).parents('tr').first().find("td.td_check img").attr("alt", "Collapse Detail Items");
        } else {
            $(obj).parents('tr').first().find("td.td_check img").attr("src", "../images/pixelicious_001.png");
            $(obj).parents('tr').first().find("td.td_check img").attr("alt", "Expand Detail Items");
        }
    }

    function CheckAllItems(obj) {
        if ($(obj).attr('checked')) {
            $(checkItems + ' :checkbox').each(function () {
                $(this).attr('checked', true);
            });
        } else {
            $(checkItems + ' :checkbox').each(function () {
                $(this).attr('checked', false);
            });
        }
    }
</script>
