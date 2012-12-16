<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectItem.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequest.SelectItem" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<div id="ca-select-itemcode" class="hidden">
    <div class="ca-workflow-form-buttons">
        <button id="ca-button-select">Select</button>
        <button id="ca-close-dialog">Close</button>
        <asp:Button id="btnHiddenCopyItem" runat="server" OnClick="btnCopyItem_Click" CssClass="hidden" />
        <asp:Button ID="btnClose" runat="server" CssClass="hidden" OnClick="btnClose_Click" />
    </div>
    <table class="ca-workflow-form-table">
        <tr>
            <td class="label align-center w25">
                Item Code:
            </td>
            <td class="label align-center w25">
                <asp:TextBox ID="txtItemCode" runat="server"></asp:TextBox>
            </td>
            <td class="label w25 align-center">
                Description:
            </td>
            <td class="value align-center">
                <asp:TextBox ID="txtDesc" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="last">
            <td colspan="4" class="value">
                <div class="ca-workflow-form-buttons">
                    <asp:Button ID="btnQuery" runat="server" Text="Query" OnClick="btnQuery_Click" CssClass="button" />
                </div>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="uplCustomer" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <SharePoint:SPGridView ID="SPGridView1" runat="server" autogeneratecolumns="False" allowpaging="True" pagesize="10" onpageindexchanging="SPGridView1_PageIndexChanging" BorderColor="#9dabb6" BorderStyle="Solid" BorderWidth="1px" CssClass="ms-listviewtable inner-table"
                EnableTheming="False" GridLines="Horizontal">
                <alternatingrowstyle cssclass="each-row ms-alternating" />
                <rowstyle cssclass="each-row" />
                <selectedrowstyle cssclass="ms-selectednav" font-bold="True" />
                <columns>
                    <asp:BoundField DataField="Title" HeaderText="Item Code"/>
                    <asp:BoundField DataField="ItemType" HeaderText="Item Type" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="Unit" HeaderText="Unit" />
                    <asp:BoundField DataField="AssetClass" HeaderText="Asset Class" />
                    <asp:BoundField DataField="VendorId" HeaderText="VendorId" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  />
                    <asp:BoundField DataField="DeliveryPeriod" HeaderText="Delivery Period" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  />
                    <asp:BoundField DataField="TaxValue" HeaderText="Tax Value" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  />
                    <asp:BoundField DataField="IsAccpetDecimal" HeaderText="IsAccpetDecimal" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  />
                    <asp:BoundField DataField="Currency" HeaderText="Currency" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  />
                    <asp:BoundField DataField="ItemScope" HeaderText="ItemScope" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  />
                    <asp:BoundField DataField="PackagedRegulation" HeaderText="PackagedRegulation" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  />
                </columns>
            </SharePoint:SPGridView>
            <div class="align-center">
                <SharePoint:SPGridViewPager id="SPGridViewPager1" runat="server" gridviewid="SPGridView1">
                </SharePoint:SPGridViewPager>
            </div>
            <asp:HiddenField ID="hidSelectedItemCode" runat="server" />
            <asp:HiddenField ID="hidSelectSampleId" runat="server" />

            <script type="text/javascript">
                $(function () {
                    $('#ca-select-itemcode').dialog({
                        title: "Select a item code",
                        autoOpen: false,
                        width: 706,
                        height: 550,
                        modal: true,
                        resizable: false,
                        draggable: false,
                        bigiframe: true,
                        open: function (type, data) {
                            $(this).parent().appendTo('form');

                            if ($('#ctl00_PlaceHolderMain_ListFormControl1_DataForm1_hidIsShowCache').val() === 'N') {
                                //alert('into');
                                //alert($('#ctl00_PlaceHolderMain_dfSelectItem_uplCustomer').length);
                                //alert($('#ctl00_PlaceHolderMain_dfSelectItem_uplCustomer').html()); ;
                                $('#ctl00_PlaceHolderMain_dfSelectItem_uplCustomer div').hide();
                                $('#ctl00_PlaceHolderMain_dfSelectItem_uplCustomer').append('<div>There are no items to show in this view.</div>');
                                //$('#ca-select-itemcode').children('div:last').text('aa');
                                $('#ctl00_PlaceHolderMain_ListFormControl1_DataForm1_hidIsShowCache').val('Y');
                            }
                        },
                        close: function () {
                            //$('#<%= this.btnClose.ClientID %>').click();
                        }
                    });
                    $('#ca-supplier-form .ca-copy-button').click(function () {
                        $('#ca-select-itemcode').dialog('open');
                        return false;
                    });

                    $('#ca-button-select').unbind('click').click(function () {
                        var result = setVal();
                        if (result) {
                            $('#ca-select-itemcode').dialog('close');
                            //$('#<%= this.btnHiddenCopyItem.ClientID %>').click();
                        }

                        return false;
                    });

                    $('#ca-close-dialog').unbind('click').click(function () {
                        $('#ca-select-itemcode').dialog('close');
                        return false;
                    });

                    $('#<%= this.SPGridView1.ClientID %> tr.each-row').die('click hover').live({
                        click: function () {
                            $(this).siblings().removeClass('selected');
                            $(this).addClass('selected');

                            var $tds = $(this).children('td');
                            var tmpInfo = '';
                            for (i = 0; i < $tds.length; i++) {
                                tmpInfo += $tds.eq(i).text() + 'л';
                            }
                            $('#<%= this.hidSelectedItemCode.ClientID %>').val(tmpInfo);
                        },
                        hover: function () {
                            $(this).toggleClass('hover');
                        }
                    });

                    //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_hidVendors
                    //alert(ca.util.emptyString(JSPurchaseRequest.ItemStrs));
                    if (ca.util.emptyString(JSPurchaseRequest.VendorStrs)) {
                        JSPurchaseRequest.VendorStrs = $('#ctl00_PlaceHolderMain_ListFormControl1_DataForm1_hidVendors').val();
                        JSPurchaseRequest.Vendors = jQuery.parseJSON(JSPurchaseRequest.VendorStrs);
                        $('#ctl00_PlaceHolderMain_ListFormControl1_DataForm1_hidVendors').val('');
                    }

                });

                function OpenDialog(sender) {
                    $('#<%= this.hidSelectSampleId.ClientID %>').val($(sender).next('input').attr('id'));
                    $('#ca-select-itemcode').dialog('open');
                    return false;
                }

                function setVal() {
                    if (ca.util.emptyString($('#<%= this.hidSelectedItemCode.ClientID %>').val())) {
                        alert('Please select one record first.');
                        return false;
                    }

                    JSPurchaseRequest.selectItemInfo = $('#<%= this.hidSelectedItemCode.ClientID %>').val();
                    JSPurchaseRequest.itemInfo = JSPurchaseRequest.selectItemInfo.split('л');
                    JSPurchaseRequest.preId = JSPurchaseRequest.GetPreId($('#<%= this.hidSelectSampleId.ClientID %>').val());
                    $('#' + JSPurchaseRequest.preId + 'lbItemCode').text(JSPurchaseRequest.itemInfo[0]);
                    $('#' + JSPurchaseRequest.preId + 'hidItemCode').val(JSPurchaseRequest.itemInfo[0]);
                    $('#' + JSPurchaseRequest.preId + 'hlPhoto').attr('href', '/WorkFlowCenter/ItemPic/' + JSPurchaseRequest.itemInfo[0] + '.jpg');


                    $('#' + JSPurchaseRequest.preId + 'lbItemType').text(JSPurchaseRequest.itemInfo[1]);
                    $('#' + JSPurchaseRequest.preId + 'hidItemType').val(JSPurchaseRequest.itemInfo[1]);

                    $('#' + JSPurchaseRequest.preId + 'lbDesc').text(JSPurchaseRequest.itemInfo[2]);
                    $('#' + JSPurchaseRequest.preId + 'hidDesc').val(JSPurchaseRequest.itemInfo[2]);

                    $('#' + JSPurchaseRequest.preId + 'lbUnit').text(JSPurchaseRequest.itemInfo[3]);
                    $('#' + JSPurchaseRequest.preId + 'hidUnit').val(JSPurchaseRequest.itemInfo[3]);

                    $('#' + JSPurchaseRequest.preId + 'lbAssetClass').text(JSPurchaseRequest.itemInfo[4]);
                    $('#' + JSPurchaseRequest.preId + 'hidAssetClass').val(JSPurchaseRequest.itemInfo[4]);


                    $('#' + JSPurchaseRequest.preId + 'lbDeliveryPeriod').text(JSPurchaseRequest.itemInfo[6]);
                    $('#' + JSPurchaseRequest.preId + 'hidDeliveryPeriod').val(JSPurchaseRequest.itemInfo[6]);
                    
                    var PackagedRegulation = JSPurchaseRequest.itemInfo[12]; //包装规则
                    $('#' + JSPurchaseRequest.preId + 'HFPackagedRegulation').val(PackagedRegulation); 

                    $('#' + JSPurchaseRequest.preId + 'lbUnitPrice').text(JSPurchaseRequest.itemInfo[7]);
                    $('#' + JSPurchaseRequest.preId + 'txtUnitPrice').val(JSPurchaseRequest.itemInfo[7]);
                    JSPurchaseRequest.itemCode = JSPurchaseRequest.itemInfo[0];
                    if (JSPurchaseRequest.itemCode.indexOf('X') === 0) {
                        $('#' + JSPurchaseRequest.preId + 'lbTransQuantity').show(); //X开头的服务费不能有调拨费用，默认为0
                        $('#' + JSPurchaseRequest.preId + 'txtTransQuantity').val(0);
                        $('#' + JSPurchaseRequest.preId + 'txtTransQuantity').hide();

                        $('#' + JSPurchaseRequest.preId + 'lbUnitPrice').addClass('hidden');//X服务费可以修改单价
                        $('#' + JSPurchaseRequest.preId + 'txtUnitPrice').removeClass('hidden');
                    } else if (JSPurchaseRequest.itemCode.indexOf('E0000') === 0) {
                        $('#' + JSPurchaseRequest.preId + 'lbTransQuantity').hide();
                        $('#' + JSPurchaseRequest.preId + 'txtTransQuantity').show();

                        $('#' + JSPurchaseRequest.preId + 'lbUnitPrice').addClass('hidden');//纸袋可以修改单价
                        $('#' + JSPurchaseRequest.preId + 'txtUnitPrice').removeClass('hidden');
                    } else {
                        $('#' + JSPurchaseRequest.preId + 'lbTransQuantity').hide();
                        $('#' + JSPurchaseRequest.preId + 'txtTransQuantity').show();

                        $('#' + JSPurchaseRequest.preId + 'lbUnitPrice').removeClass('hidden');//无法修改单价
                        $('#' + JSPurchaseRequest.preId + 'txtUnitPrice').addClass('hidden');
                    }

                    $('#' + JSPurchaseRequest.preId + 'lbTaxValue').text(JSPurchaseRequest.itemInfo[8]);
                    $('#' + JSPurchaseRequest.preId + 'hidTaxValue').val(JSPurchaseRequest.itemInfo[8]);

                    $('#' + JSPurchaseRequest.preId + 'hdIsAccpetDecimal').val(JSPurchaseRequest.itemInfo[9]);

                    $('#' + JSPurchaseRequest.preId + 'lbCurrency').text(JSPurchaseRequest.itemInfo[10]);
                    $('#' + JSPurchaseRequest.preId + 'hidCurrency').val(JSPurchaseRequest.itemInfo[10]);
                    
                    JSPurchaseRequest.vendorId = JSPurchaseRequest.itemInfo[5];

                    for (JSPurchaseRequest.i = 0; JSPurchaseRequest.i < JSPurchaseRequest.Vendors.length; JSPurchaseRequest.i++) {
                        if (JSPurchaseRequest.vendorId === JSPurchaseRequest.Vendors[JSPurchaseRequest.i].VendorId) {
                            $('#' + JSPurchaseRequest.preId + 'lbVendor').text(JSPurchaseRequest.Vendors[JSPurchaseRequest.i].Name);
                            $('#' + JSPurchaseRequest.preId + 'hidVendor').val(JSPurchaseRequest.Vendors[JSPurchaseRequest.i].Name);
                            $('#' + JSPurchaseRequest.preId + 'hidVendorId').val(JSPurchaseRequest.vendorId);
                            break;
                        }
                    }
                    $('#<%= this.hidSelectedItemCode.ClientID %>').val('');

                    UpdateTotal($('#' + JSPurchaseRequest.preId + 'txtUnitPrice'));
                    return true;
                }

                //当前选中的Item是否己经选过
                function IsItemSelected(ItemCode) {
                    var isSelected = false;
                    $(".SelectItemCode").each(function () {
                        if ($(this).text() == ItemCode) {
                            isSelected = true;
                            return;
                        }
                    });
                    return isSelected;
                }
            </script>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnHiddenCopyItem" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickNext" />
            <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickPrevious" />
        </Triggers>
    </asp:UpdatePanel>
</div>

