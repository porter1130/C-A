<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseOrder.DataView" %>

<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>

<style type="text/css">
    .ca-purchaseorder-bigger
    {
       background-color:White;
        background-repeat: repeat-y;
        background-position: right;
        width: 1080px;
        margin-bottom: 0;
        border-bottom: 1px solid #9DABB6;
        border-top: 1px solid #9DABB6;
        
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
    .datetime td
    {
        border-bottom: none;
        border-right: none;
    }
    .list-view td
    {
        padding-left: 2px;
        padding-right: 2px;
    }
    td.left-align
    {
        text-align: left;   
    }
    .left-align div
    {
        padding-top: 1px;
        padding-bottom: 1px;
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
    .IsShowACNumber
    {
       display:none;
        }
</style>

<script type="text/javascript">
    $(document).ready(function () {
        HideAssetinput();
        if ('<%= IsNotReturn%>' == 'True') {
            $(".ca-purchaseorder-bigger").css("width", "1250px");
            /*$(".ca-purchaseorder-bigger").css("width", "1350px");*/
        }
        else {
            $(".AllocatedValue").attr("disabled", "disabled");
        }
        if ('<%= bIsCompex%>' == 'True') {
            GetAmountSummary();
            ChageAllocated();
        }
        else {
            $(".tdAmountSummary").hide();
        }
    });
    function ChangeVaue(object, currentValue) {
        object.each(function () {
            $(this).val(currentValue);
        });
    }

    function HideAssetinput() {
        $("tr[class='POItem']").each(function () {
            var itemcode = $(this).children("td[class='POItemCode']").text();
            if (itemcode.toUpperCase().indexOf("X") == 0) {
                $(this).find(".AssetClass").remove();
            }
        });
    }

    function ChageAllocated() {
        $(".POItem").find(".AllocatedValueTxt > input").change(function () {
            var assetClass = $(this).parent("div").attr("AssetClass");
            var amount = GetAssetallocatedValue(assetClass);
            var divSearch = "div[class='AmountSummary'][AssetClass='" + assetClass + "']";
            $(".POItem").find(divSearch).html(amount);
        });
    }

    ///得到依据AssetClass汇总AllocatedVlaue
    function GetAmountSummary() {
        var assetArray = new Array();
        assetArray = GetAllAsset();
        for (i = 0; i < assetArray.length; i++) {
            var assetClass = assetArray[i];
            var amount = GetAssetallocatedValue(assetClass);
            var divSearch = "div[class='AmountSummary'][AssetClass='" + assetClass + "']";
            $(".POItem").find(divSearch).html(amount);
        }
    }
    ///得到每一个assetClass对Allocated值的汇总。
    function GetAssetallocatedValue(asset) {
        var amount = 0;
        var divSearch = "div[class='AllocatedValueTxt'][AssetClass='" + asset + "']";
        $(".POItem").find(divSearch).each(function () {
            var allocatedValue = $(this).children("input").val();
            if (!isNaN(allocatedValue) && allocatedValue.length>0) {
                amount +=parseFloat(allocatedValue);
            }
        });
        return amount.toFixed(2);
    }

    //得到所有的AssetClass
    function GetAllAsset() {
        var assetArray = new Array();
        $(".POItem").each(function () {
            var currentAsset = $(this).find("div[class='AllocatedValueTxt']").eq(0).attr("AssetClass");
            if (!ExistInArray(assetArray, currentAsset)) {
                assetArray.push(currentAsset)
            }
        });
        return assetArray
    }

    //数组array中是否存存val
    function ExistInArray(array ,val)
    {
	    for(i=0;i<array.length;i++){
		    if(array[i]==val){
			    return true;
			    }
			    return false;
		    }
    }
</script>

<span id="ca_po_isreturn" class="hidden">
    <QFL:FormField ID="ffIsReturn" runat="server" FieldName="IsReturn" ControlMode="Display" ></QFL:FormField>
</span>

<table class="ca-workflow-form-table ca-purchaseorder-bigger ca-first-tb noPrint">
    <tr>
        <td>
            P.O. Number(SAP)
            <br />
            订单号(SAP)
        </td>
        <td>
            <QFL:FormField ID="ffSapNO" runat="server" FieldName="SapNO" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
</table>

<table>
    <thead style="display:table-header-group;">
        <tr>
            <td>
                <table class="ca-workflow-form-table ca-purchaseorder-bigger">
    <tr>
        <td class="bold" colspan="8" id="ca_po_formtitle">
            <div id="FormPrintTitle" runat="server">
                <span>C&A China <asp:Label ID="LabelEnTitle" runat="server" Text="External" ></asp:Label> Order Form</span><span style="display: none;">C&A China Return Form</span>
                <br />
                <span>西雅衣家(中国)商业有限公司<asp:Label ID="LabelCNtitle" runat="server" Text="对外采购" ></asp:Label>订单</span><span style="display: none;">西雅衣家(中国)商业有限公司对外退货单</span>
            </div>
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
            <QFL:FormField ID="ffPONumber" runat="server" FieldName="PONumber" ControlMode="Display" ></QFL:FormField>
        </td>
        <td colspan="3" id="po_number_finance">
            <QFL:FormField ID="ffPONumFinanceDisp" Visible="false" runat="server" FieldName="PONumFinance" ControlMode="Display" ></QFL:FormField>
            <QFL:FormField ID="ffPONumFinance" runat="server" FieldName="PONumFinance" ControlMode="Edit" ></QFL:FormField>
        </td>        
        <td colspan="2" class="datetime">
            <QFL:FormField ID="FormField3" runat="server" FieldName="IssuedDate" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
</table>

<%--<table class="ca-workflow-form-table ca-purchaseorder-bigger list-view" style="border-bottom: none;" id="ca_po_detail_head">
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
                <td style="width: 140px;" id="ca_po_detail_head_desc">
                    Code/Description
                    <br />
                    产品代码/描述
                    <br />
                    140
                </td>
                <td style="width: 60px;">
                    Total QTY
                    <br />
                    <span id="Span1"><span>订购量</span><span style="display: none;">退货量</span></span>
                    <br />
                    60
                </td>
                <td style="width: 65px;" class="TransQuantity">
                    Trans QTY
                    <br />
                    调拨数量
                    <br />
                    65
                </td>
                <td style="width: 60px;" class="RequestQuantity">
                    Purchase QTY
                    <br />
                    购买数量
                    <br />
                    60
                </td>
                <td style="width: 30px;">
                    Unit
                    <br />
                    单位
                    <br />
                    30
                </td>
                <td style="width: 52px;">
                    Currency
                    <br />
                    货币
                    <br />
                    52
                </td>
                <td style="width: 60px;">
                    Unit Price
                    <br />
                    含税单价
                    <br />
                    60
                </td>
                <td style="width: 65px;">
                    Total Price
                    <br />
                    合计
                    <br />
                    65
                </td>
                <td style="width: 55px;">
                    Tax Rate
                    <br />
                    税率
                    <br />
                    55
                </td>
                <td style="width: 60px;">
                    Tax Value
                    <br />
                    税额
                    <br />
                    60
                </td>
                <td style="width: 60px;">
                    Net Price
                    <br />
                    净价
                    <br />
                    60
                </td>
                <td style="width: 60px;" id="ACCode" runat="server" visible="false">
                    AssetClass
                    <br />
                    财务代码
                    <br />
                    60
                </td>
                <td style="width: 80px;" id="ACNumber" runat="server" visible="false">
                    Asset No.
                    <br />
                    80
                </td>
                <td style="width:80px;" id="Allocated" runat="server" visible="false">
                    Allocated Value
                    <br />
                    80
                </td>
                <td style="width:70px;" class="tdAmountSummary">
                   Amount Summary 
                    <br />
                    70
                </td>
               <td style="width:50px;" class="IsShowACNumber">
                   ACNumber
                    <br />
                    50
                </td>
                <td style="width:50px;" class="IsShowACNumber">
                   AllocatedValue 
                    <br />
                    50
                </td>
            </tr>
        </table>--%>
            </td>
        </tr>
    </thead>
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table ca-purchaseorder-bigger list-view" style="border-top: none;table-layout: fixed;" id="ca_po_detail">
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
                <td style="width: 140px;" id="ca_po_detail_head_desc">
                    Code/Description
                    <br />
                    产品代码/描述
                </td>
                <td style="width: 60px;">
                    Total QTY
                    <br />
                    <span id="Span1"><span>订购量</span><span style="display: none;">退货量</span></span>
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
                <td style="width: 60px;" id="ACCode" runat="server" visible="false">
                    AssetClass
                    <br />
                    财务代码
                </td>
                <td style="width: 80px;" id="ACNumber" runat="server" visible="false">
                    Asset No.
                </td>
                <td style="width:80px;" id="Allocated" runat="server" visible="false">
                    Allocated Value
                </td>
                <td style="width:70px;" class="tdAmountSummary">
                   Amount Summary 
                </td>
               <td style="width:70px;" class="IsShowACNumber">
                   ACNumber
                </td>
                <td style="width:70px;" class="IsShowACNumber">
                   AllocatedValue 
                </td>
                <td style="width:70px;" class="IsShowACNumber">
                   AssetClass 
                </td>
            </tr>
            <asp:Repeater ID="rptItem" runat="server" >
                <ItemTemplate>
                    <tr class="POItem">
                        <td>
                            <%# Container.ItemIndex + 1 %> 
                        </td>
                        <td class="POItemCode">
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
                        <td>
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
                        <td runat="server" visible="<%# bIsCompex%>">
                            <%# Eval("AssetClass") %>
                        </td>
                        <td runat="server" visible="<%# bIsCompex%>" >
                           <asp:HiddenField ID="HFID" runat="server" Value='<%# Eval("ID")%>' />
                            <div class='AssetClass'>
                                <asp:TextBox ID="TextBoxACNumber" Width="80" ToolTip='<%# Eval("AssetClass") %>'  CssClass="ACNONotNull" Text='<%# Eval("ACNumber")%>' runat="server"></asp:TextBox> 
                            </div>
                        </td>
                        <td runat="server" visible="<%# IsNotReturn%>" >
                            <div class="AllocatedValueTxt" AssetClass='<%# Eval("AssetClass") %>'>
                                <asp:TextBox ID="TextBoxAllocatedValue" Width="60" Text='<%# GetValue(Convert.ToDouble(Eval("TotalPrice")) - Convert.ToDouble(Eval("TaxValue")),Eval("ItemCode").ToString())%>' CssClass="AllocatedValue" runat="server"></asp:TextBox>
                            </div>
                        </td>
                        <td class="tdAmountSummary"> 
                            <div class="AmountSummary" AssetClass='<%# Eval("AssetClass") %>'></div>
                        </td>
                       <td class="IsShowACNumber"> 
                          <%# Eval("ACNumber")%>
                        </td>
                        <td class="IsShowACNumber"> 
                          <%# Eval("AllocatedValue")%>
                        </td>
                        <td class="IsShowACNumber"> 
                          <%# Eval("AssetClass")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
            
            <table class="ca-workflow-form-table ca-purchaseorder-bigger ca-last-tb">
            <div class="_PageNext"></div><!-- 强制分页打印 //如需强制分布，将_PageNext改为PageNext-->
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
                        <QFL:FormField ID="ffPaymentCondition" runat="server" FieldName="PaymentCondition" ControlMode="Display" ></QFL:FormField>
                    </td>
                    <td class="tb1-dr" style="text-align: left">
                        <QFL:FormField ID="ffDeliveryDirections" runat="server" FieldName="DeliveryDirections" ControlMode="Display" ></QFL:FormField>
                    </td>
                    <td class="tb1-dr" style="text-align: left">
                        <QFL:FormField ID="ffGuarantee" runat="server" FieldName="Guarantee" ControlMode="Display" ></QFL:FormField>
                    </td>
                    <td class="tb1-dr tb1-last" style="text-align: left">
                        <QFL:FormField ID="ffDeliveryDate" runat="server" FieldName="DeliveryDate" ControlMode="Display" ></QFL:FormField>
                    </td>
                </tr>
            </table>
        </td>
        
        <td style="text-align: left;" id="comments1">
            <QFL:FormField ID="FormField4" runat="server" FieldName="OrderComment1" ControlMode="Display" ></QFL:FormField>
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
            <QFL:FormField ID="FormField5" runat="server" FieldName="OrderComment2" ControlMode="Display" ></QFL:FormField>
            <span class="hidden">
            GENERAL CONDITIONS OF <span ID="ContractEN" runat="server">SUPPLY</span> CONTRACT ARE PART OF THIS ORDER. THE VENDOR HAS A COPY OF THE REFERRED CONTRACT AND HAS KNOWLEDGE OF ITS CONTENT AND FULLY AGREED WITH IT.
            <br />
            供应商与西雅衣家(中国)商业有限公司签署的<span ID="ContractCN" runat="server">供货合同</span>应适用此订单，并对双方具有约束力。供应商有一份此合同，明确了解并且完全同意合同内容及条款。
            </span>
        </td>
    </tr>
    
</table>
        </td>
    </tr>
</table>
<script type="text/javascript">
    function Validate() {
        var error = '';

        var flag = 0;
        $('#po_number_finance input').each(function () {
            if (ca.util.emptyString($(this).val())) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please fill the PO Number.\n' : '';

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

            $('#ca_po_detail td.TransQuantity').hide();
            $('#ca_po_detail td.RequestQuantity').hide();
            $('#ca_po_detail_desc').css('width', '285px');
        }
    }

    //当订单中出现外币时，注释处显示输入框
    function SetComments() {
        if ($('#comments1 div').eq(0).text().length === 0) {
            $('#comments1').children().toggle();
            $('#comments1 span').css('display', 'block');
        }
        if ($('#comments2 div').eq(0).text().length === 0) {
            $('#comments2').children().toggle();
            $('#comments2>.hidden').css('display', 'block');
        }
    }

    $(function () {
        SetReturnStyle();
        SetComments();
    });
</script>
