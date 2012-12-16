<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataListSAPView.ascx.cs"
    Inherits="CA.WorkFlow.UI.PaymentRequestSAP.DataListSAPView" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<style type="text/css">
    .ca-workflow-form-table td
    {
        padding: 5px;
        border-bottom: 1px solid #CCCCCC;
        text-align: left;
        margin: 0;
        line-height: 15px;
        cursor: pointer;
    }
    .ca-workflow-form-table
    {
        margin-top: 25px;
    }
    .ca-workflow-form-table input
    {
        border: none;
        background-color: transparent;
    }
    .spgvSAPtr
    {
        background-color: transparent;
    }
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
    .spgvSAPtrbg
    {
        background-color: #f2f2f2;
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
    img
    {
        margin: 0px 0px 0px 5px;
    }
    input
    {
        border: #cccccc 1px solid;
        margin: 0px 5px 0px 5px;
    }
    #tb_summary h3
    {
        text-align: center;
    }
</style>
<table border="0" cellpadding="0" cellspacing="0" id="tb_summary">
    <tr>
        <td colspan="3">
            <h3>
                Payment Request SAP Details</h3>
        </td>
    </tr>
    <tr>
        <td style="font-weight: bold; width: 250px">
            <input type="checkbox" class="ckAll" title="Select All Items" />　　Payment Request Form ID
        </td>
        <td style="padding-left: 26px; font-weight: bold; width: 300px">
            Payment Request WorkFlowNumber
        </td>
        <td style="text-align: left; font-weight: bold;" colspan="">
            Post Count
        </td>
    </tr>
    <asp:Repeater ID="rptWFItemCollection" OnItemDataBound="rptWFItemCollection_ItemDataBound"
        runat="server">
        <ItemTemplate>
            <tr class="wftr">
                <td class="td_check" style="width: 250px;">
                    <img src="../images/pixelicious_001.png" alt="Expand Detail Items" align="absmiddle"
                        onclick="DisplayDetails(this)" />
                    <input type="checkbox" name="chkitem" title='<%#Eval("PRWorkflowNumber") %>' id='<%#Eval("Title") %>' class="itemCK" />
                    <span class="span_wftitle">
                        <%#Eval("Title") %></span>
                </td>
                <td style="padding-left: 26px">
                    <%# Eval("PRWorkflowNumber")%>
                </td>
                <td class="pc" style="text-align: left">
                    <%#Eval("PostCount") %>
                </td>
            </tr>
            <tr class="tr_details hidden">
                <td style="padding-left: 38px" colspan="3" class="spgvtd" >
                    <asp:UpdatePanel ID="upDetails" runat="server">
                        <contenttemplate>
                            <SharePoint:SPGridView ID="spgvWFItem" runat="server" AutoGenerateColumns="false"
                                AlternatingRowStyle-CssClass="spgvSAPtr" HeaderStyle-CssClass="spgvSAPtr" RowStyle-CssClass="spgvSAPtr" OnRowDataBound="spgvWFItem_RowDataBound">
                                <Columns>
                                </Columns>
                            </SharePoint:SPGridView>
                        </contenttemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<asp:HiddenField ID="hidWorkflowID" runat="server" Value="" />
<asp:HiddenField ID="hidSAPWorkflowID" runat="server" Value="" />
<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">

    function CheckSubmit() {
        CreateForbidDIV();
        var result = false;
        var $chkitem = $("input[name$='chkitem']");
        var $hidWorkflowID = $('#<%= this.hidWorkflowID.ClientID %>');
        var $hidSAPWorkflowID = $('#<%= this.hidSAPWorkflowID.ClientID %>');
        var wfid = "";
        var sapwfid = "";
        $chkitem.each(function () {
            if ($(this).attr("checked")) {
                result = true;
                wfid += $(this).attr("title") + ";";
                sapwfid += $(this).attr("id") + ";";
            }
        });
        $hidWorkflowID.val(wfid);
        $hidSAPWorkflowID.val(sapwfid);
        if (!result) {
            alert("Please Select SAP Claim Items.");
            ClearForbidDIV();
        }
        
        return result;
    }

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
    $(function () {
        SetGVTitle();
        var $ckAll = $("input.ckAll");
        var $item = $("input.itemCK");
        $ckAll.click(function () {
            var result = $(this).attr("checked");
            $item.each(function () {
                $(this).attr("checked", result);
            });
        });
    });

    function SetGVTitle() {
        var $msviewheadertr = $("td.spgvtd tr.ms-viewheadertr");
        $msviewheadertr.each(function () {
            var $tr = $(this).nextAll();
            var result = false;
            $tr.each(function () {
                var $td1 = $(this).find("td").eq(0).find("span");
                var $td2 = $(this).find("td").eq(1).find("span");
                if ($td1.text() == ""
                     && $td2.text() != "Tax payable - VAT input"
                     && $td2.text() != "OP-Non-trade vendor"
                     && $td2.text() != "GRIR vendor code") {
                    result = true;
                }
            });
            if (result) {
                $(this).find("th").eq(0).hide();
                $tr.each(function () {
                    $(this).find("td").eq(0).hide();
                });
            } else {
                $(this).find("th").eq(1).html("AssetType");
                $(this).find("th").eq(4).html("AssetClass");
            }
        });
    }

</script>
