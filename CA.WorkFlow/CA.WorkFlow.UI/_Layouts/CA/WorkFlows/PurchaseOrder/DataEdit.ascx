<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseOrder.DataEdit" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register src="Installment.ascx" tagname="Installment" tagprefix="uc1" %>
<style type="text/css">
    .ca-purchaseorder-bigger
    {
        background-image: url("../images/background_white.JPG");
        background-repeat: repeat-y;
        background-position: right;
        width: 950px;
        margin-bottom: 0;
        border-bottom: 1px solid #9DABB6;
        border-top: 1px solid #9DABB6;
        table-layout: fixed;
    }
    .ca-first-tb
    {
        border-top: 2px solid #9DABB6;
    }
    .ca-last-tb
    {
        margin-bottom: 3px;
        border-bottom: 2px solid #9DABB6;
    }
    .ca-purchaseorder-bigger td
    {
        padding: 5px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        text-align: center;
        margin: 0;
        
    }
    .special-tb td
    {
        border-bottom: none;
        border-right: none;
    }
    .list-view td
    {
        padding-left: 2px;
        padding-right: 2px;
    }
    .ca-po-tb1
    {
        padding: 0 !important;   
    }
    .ca-po-tb1 table,.ca-po-tb1 tr
    {
        width: 100%;   
    }
    .ca-po-tb1 .tb1-dr
    {
        border-bottom: none;   
    }
    .ca-po-tb1 .tb1-last
    {
        border-right: none;   
    }
    .ca-purchaseorder-bigger td.align-left
    {
        text-align: left;           
    }
</style>

<script type="text/javascript">
    $(document).ready(function () {
        var ffPaymentCondition = $('#ctl00_PlaceHolderMain_ListFormControl3_DataForm1_ffPaymentCondition_ctl00_ctl00_TextField')
        ffPaymentCondition.attr("readonly", "readonly");
        ffPaymentCondition.blur();
        ffPaymentCondition.focus(function () {
            $(".InstallmentDiv").fadeIn();
            var position = ffPaymentCondition.offset();
            $(".InstallmentDiv").css("left", position.left + 50);
            $(".InstallmentDiv").css("top", position.top - 300);
            CreateForbidDIV();
        });
    });
</script>
<span id="ca_po_isreturn" class="hidden"><QFL:FormField ID="ffIsReturn" runat="server" FieldName="IsReturn" ControlMode="Display" ></QFL:FormField></span>

<table class="ca-workflow-form-table ca-purchaseorder-bigger ca-first-tb noPrint">
    <tr>
        <td>
            P.O. Number(SAP)
            <br />
            订单号(SAP)
        </td>
        <td>
            <QFL:FormField ID="ffPONumSAP" runat="server" FieldName="SapNO" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
</table>

<table class="ca-workflow-form-table ca-purchaseorder-bigger">
    <tr>
        <td class="bold" colspan="8" id="ca_po_formtitle">
            <span>C&A China External Order Form</span><span style="display: none;">C&A China External Return Form</span>
            <br />
            <span>西雅衣家(中国)商业有限公司对外采购订单</span><span style="display: none;">西雅衣家(中国)商业有限公司对外退货单</span>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            C&A(China) Co., Ltd.
            <br />
            西雅衣家(中国)商业有限公司
        </td>
        <td>
            Vendor
            <br />
            供应商
        </td>
        <td colspan="3">
            <QFL:FormField ID="ffVendor" runat="server" FieldName="Vendor" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Address
            <br />
            地址
        </td>
        <td colspan="3">
            3A88 Shanghai Mart, 2299 Yan'an Road West
            <br />
            上海市延安西路2299号上海世贸商城3A88
        </td>
        <td>
            Address
            <br />
            地址
        </td>
        <td colspan="3">
            <QFL:FormField ID="ffVendorAddress" runat="server" FieldName="VendorAddress" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Code
            <br />
            邮编
        </td>
        <td>
            200336
        </td>
        <td>
            City
            <br />
            城市
        </td>
        <td>
            Shanghai
            <br />
            上海
        </td>
        <td>
            Code
            <br />
            邮编
        </td>
        <td>
            <QFL:FormField ID="ffVendorCode" runat="server" FieldName="VendorCode" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            City
            <br />
            城市
        </td>
        <td>
            <QFL:FormField ID="ffVendorCity" runat="server" FieldName="VendorCity" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Phone
            <br />
            电话
        </td>
        <td>
            <QFL:FormField ID="FormField2" runat="server" FieldName="Phone" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            Fax
            <br />
            传真
        </td>
        <td>
            <QFL:FormField ID="FormField1" runat="server" FieldName="Fax" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            Phone
            <br />
            电话
        </td>
        <td>
            <QFL:FormField ID="ffVendorPhone" runat="server" FieldName="VendorPhone" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            Fax
            <br />
            传真
        </td>
        <td>
            <QFL:FormField ID="ffVendorFax" runat="server" FieldName="VendorFax" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            E-mail
        </td>
        <td colspan="3">
            <QFL:FormField ID="ffEmail" runat="server" FieldName="Email" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            E-mail
        </td>
        <td colspan="3">
            <QFL:FormField ID="ffVendorMail" runat="server" FieldName="VendorMail" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Buyer
            <br />
            采购员
        </td>
        <td>
            <QFL:FormField ID="ffBuyer" runat="server" FieldName="Buyer" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            Department
            <br />
            部门
        </td>
        <td>
            <QFL:FormField ID="ffDepartment" runat="server" FieldName="Department" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            Vendor No.
            <br />
            供应商编号
        </td>
        <td>
            <QFL:FormField ID="ffVendorNo" runat="server" FieldName="VendorNo" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            Contact
            <br />
            联系人
        </td>
        <td>
            <QFL:FormField ID="ffVendorContact" runat="server" FieldName="VendorContact" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>

    <tr>
        <td colspan="3">
            P.O. Number
            <br />
            订单号
        </td>
        <td colspan="3">
            P.O. Number (Finance)
            <br />
            订单号(财务)
        </td>        
        <td colspan="2">
            Issued
            <br />
            日期
        </td>
    </tr>
    <tr>
        <td colspan="3">
           <span class="PONO"> <QFL:FormField ID="ffPONumber" runat="server" FieldName="PONumber" ControlMode="Display" ></QFL:FormField></span>
        </td>
        <td colspan="3">
            <QFL:FormField ID="ffPONumFinance" runat="server" FieldName="PONumFinance" ControlMode="Display" ></QFL:FormField>
        </td>        
        <td colspan="2" class="datetime">
            <QFL:FormField ID="FormField3" runat="server" FieldName="IssuedDate" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
</table>


<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table ca-purchaseorder-bigger list-view" id="ca_po_detail">
            <tr>            
                <td style="width: 30px;">
                    No.
                    <br />
                    序号
                </td>
                <td style="width: 60px;">
                    Item Code
                    <br />
                    编号
                </td>
                <td style="width: 70px;">
                    Cost Center
                    <br />
                    成本中心
                </td>
                <td style="width: 140px;" id="ca_po_detail_desc">
                    Code/Description
                    <br />
                    产品代码/描述
                </td>
                <td style="width: 60px;">
                    Total QTY
                    <br />
                    <span id="label_total_qty"><span>订购量</span><span style="display: none;">退货量</span></span>
                </td>
                <td style="width: 65px;" class="TransQuantity">
                    Trans QTY
                    <br />
                    调拨数量
                </td>
                <td style="width: 60px;" class="RequestQuantity">
                    Purchase QTY
                    <br />
                    购买数量
                </td>
                
                
                <td style="width: 30px;">
                    Unit
                    <br />
                    单位
                </td>
                <td style="width: 52px;">
                    Currency
                    <br />
                    货币
                </td>
                <td style="width: 60px;">
                    Unit Price
                    <br />
                    含税单价
                </td>
                <td style="width: 65px;">
                    Total Price
                    <br />
                    合计
                </td>
                <td style="width: 55px;">
                    Tax Rate
                    <br />
                    税率
                </td>
                <td style="width: 60px;">
                    Tax Value
                    <br />
                    税额
                </td>
                <td style="width: 60px;">
                    Net Price
                    <br />
                    净价
                </td>
            </tr>
            <asp:Repeater ID="rptItem" runat="server" >
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Container.ItemIndex + 1 %> 
                        </td>
                        <td>
                            <%# Eval("ItemCode")%>
                        </td>
                        <td>
                            <%# Eval("CostCenter")%>
                        </td>
                        <td style="text-align: left;">
                            <%# Eval("Description")%>
                        </td>
                        <td>
                            <%# Eval("TotalQuantity")%>
                        </td>
                        <td class="TransQuantity">
                            <%# Eval("TransQuantity")%>
                        </td>
                        <td class="RequestQuantity">
                            <%# Eval("RequestQuantity")%>
                        </td>

                        <td>
                            <%# Eval("Unit")%>
                        </td>
                        <td class="Currency">
                            <%# Eval("Currency")%>
                        </td>
                        <td>
                            <%# Eval("UnitPrice")%>
                        </td>
                        <td>
                            <%# Eval("TotalPrice")%>
                        </td>
                        <td>
                            <%# Eval("TaxRate")%>
                        </td>
                        <td>
                            <%# Eval("TaxValue")%>
                        </td>
                        <td>
                            <%# Convert.ToDouble(Eval("TotalPrice")) - Convert.ToDouble(Eval("TaxValue"))%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

<table class="ca-workflow-form-table ca-purchaseorder-bigger ca-last-tb">
    <tr>
        <td style="width: 90px;">
            Total Net Price
            <br />
            总净价
        </td>
        <td style="width: 80px;">
            Site Install Fee
            <br />
            安装费
        </td>
        <td style="width: 90px;">
            Package Charge
            <br />
            包装费
        </td>
        <td style="width: 80px;">
            Freight Cost
            <br />
            运输费
        </td>
        <td style="width: 80px;">
            Discount
            <br />
            折扣
        </td>
        <td style="width: 80px;">
            Tax Value
            <br />
            税额
        </td>
        <td>
            Grand Total Value
            <br />
            总订单额
        </td>
    </tr>
    <tr>
        <td id="order_total">
            <QFL:FormField ID="ffTotal" runat="server" FieldName="Total" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            <QFL:FormField ID="ffSiteInstallFee" runat="server" FieldName="SiteInstallFee" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            <QFL:FormField ID="ffPackageCharge" runat="server" FieldName="PackageCharge" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            <QFL:FormField ID="ffFreightCost" runat="server" FieldName="FreightCost" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            <QFL:FormField ID="ffDiscount" runat="server" FieldName="Discount" ControlMode="Display" ></QFL:FormField>
        </td>
        <td>
            <QFL:FormField ID="ffTaxValue" runat="server" FieldName="TaxValue" ControlMode="Display" ></QFL:FormField>
        </td>
        <td id="grand_total">
            <QFL:FormField ID="ffGrandTotal" runat="server" FieldName="GrandTotal" ControlMode="Display" ></QFL:FormField>
        </td>
        <asp:HiddenField ID="hidGrandTotal" runat="server" />
    </tr>
    <tr>
        <td colspan="6" class="ca-po-tb1">
            <table>
                <tr>
                    <td class="w25" id="ca_po_pcondition">
                        <span>Payment Conditions</span><span style="display: none;">Refund Conditions</span>
                        <br />
                        <span>付款条件</span><span style="display: none;">退款条件</span>
                    </td>
                    <td class="w25" id="ca_po_ddirection">
                        <span>Delivery Direction</span><span style="display: none;">Return Location</span>
                        <br />
                        <span>送货目的地</span><span style="display: none;">退货地点</span>
                    </td>
                    <td class="w25">
                        Guarantee Period
                        <br />
                        质保期
                    </td>
                    <td class="w25 tb1-last" id="ca_po_deliverydate">
                        <span>Delivery Date</span><span style="display: none;">Return Date</span>
                        <br />
                        <span>交货日</span><span style="display: none;">退货日期</span>
                    </td>
                </tr>
                <tr>
                    <td class="tb1-dr" style="text-align: left">
                        <QFL:FormField ID="ffPaymentCondition" runat="server" FieldName="PaymentCondition" ControlMode="Edit" ></QFL:FormField>
                       <%-- <asp:HiddenField ID="HFPaymentInfo" runat="server" />--%>
                        <span class="HFPaymentInfo"></span>
                    </td>
                    <td class="tb1-dr" style="text-align: left">
                        <QFL:FormField ID="ffDeliveryDirections" runat="server" FieldName="DeliveryDirections" ControlMode="Edit" ></QFL:FormField>
                    </td>
                    <td class="tb1-dr" style="text-align: left">
                        <QFL:FormField ID="ffGuarantee" runat="server" FieldName="Guarantee" ControlMode="Edit" ></QFL:FormField>
                    </td>
                    <td class="tb1-dr tb1-last" style="text-align: left">
                        <QFL:FormField ID="ffDeliveryDate" runat="server" FieldName="DeliveryDate" ControlMode="Edit" ></QFL:FormField>
                    </td>
                </tr>
            </table>
        </td>
        
        <td style="text-align: left;" id="comments1">
            <QFL:FormField ID="FormField4" runat="server" FieldName="OrderComment1" ControlMode="Edit" ></QFL:FormField>
            <span class="hidden">
            NOTE: THE P.O. NUMBER MUST BE SPECIFIED IN THE INVOICES RELATED TO
            <br />
            备注：供应商必须在相应发票上注明此订单号
            </span>
        </td>
    </tr>
    <tr>
        <td colspan="6" class="ca-po-tb1">
            <table>
                <tr>
                    <td class="w50" style="height: 30px;">
                        <!--empty-->
                    </td>
                    <td class="w50 tb1-last">
                        <!--empty-->
                    </td>
                </tr>
                <tr>
                    <td class="align-left">
                        VENDOR CHOP
                        <br />
                        供应商签字盖章
                    </td>
                    <td class="tb1-last align-left">
                        VENDEE CHOP
                        <br />
                        采购方盖章
                    </td>
                </tr>
                <tr>
                    <td class="tb1-last" colspan="2">
                        ***FOR ORDERS WITH MORE THAN ONE PAGE, CONSIDER PRICES AND TAXES AT LAST PAGE***
                        <br />
                        ***此订单如果有多页，价格和税率打印在订单最后一页***
                    </td>
                </tr>
            </table>
        </td>

        <td class="tb1-dr" style="text-align: left;" id="comments2">
            <QFL:FormField ID="FormField5" runat="server" FieldName="OrderComment2" ControlMode="Edit" ></QFL:FormField>
            <span class="hidden">
            GENERAL CONDITIONS OF SUPPLY CONTRACT ARE PART OF THIS ORDER. THE VENDOR HAS A COPY OF THE REFERRED CONTRACT AND HAS KNOWLEDGE OF ITS CONTENT AND FULLY AGREED WITH IT.
            <br />
            供应商与西雅衣家(中国)商业有限公司签署的供货合同应适用此订单，并对双方具有约束力。供应商有一份此合同，明确了解并且完全同意合同内容及条款。
            </span>
        </td>
    </tr>
</table>


<script type="text/javascript">
    var JSPurchaseOrder = {};

    function Validate() {
        var error = '';

        var flag = 0;
        $('.ca-po-tb1 textarea').each(function () {
            if (ca.util.emptyString($(this).val())) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please fill the all fields in the form.\n' : '';

        if (error) {
            alert(error);
        }

        return error.length === 0;
    }

    function SetBorderWarn($obj) {
        $obj.css('border', '1px solid red');
    }

    function ClearBorderWarn($obj) {
        $obj.css('border', '');
        $obj.css('border-bottom', '#999 1px solid');
    }

    //设置退货界面，若退货被选中，则显示后面的退货编号输入框，否则则隐藏
    function SetReturnStyle() {
        var is_return = jQuery.trim($('#ca_po_isreturn').text()) === 'Yes';
        if (is_return) {
            $('#label_total_qty span').toggle();
            $('#ca_po_pcondition span').toggle();
            $('#ca_po_ddirection span').toggle();
            //$('#ca_po_gperiod span').toggle();
            $('#ca_po_deliverydate span').toggle();
            $('#ca_po_formtitle span').toggle();
            $('#<%=this.ffGuarantee.ClientID%>_ctl00_ctl00_TextField').val('N/A不适用');

            $('#ca_po_detail td.TransQuantity').hide();
            $('#ca_po_detail td.RequestQuantity').hide();
            $('#ca_po_detail_desc').css('width', '285px');
        }
    }

    //当订单中出现外币时，注释处显示输入框
    function SetComments() {
        var count = 0;
        $('#ca_po_detail td.Currency').each(function () {
            if (jQuery.trim($(this).text()) != 'RMB') {
                count++;
            }
        });
        if (count === 0) {
            $spans = $('#comments1 span');
            $spans.toggle();
            $spans.eq(1).css('display', 'block');

            $spans = $('#comments2 span');
            $spans.toggle();
            $spans.eq(1).css('display', 'block');
        }
    }

    $(function () {
        SetReturnStyle();
        SetComments();

    });
</script>
<uc1:Installment ID="Installment1" runat="server" />
