<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequest.DataView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>

<style type="text/css">
    #tb_purchaserequest
    {
        background-image: url("../images/background_white.JPG");
        background-repeat: repeat-y;
        background-position: right;
    }
    .return-hide
    {
        display: none;   
    }
    .return-width-vendor
    {
        width: 235px;
    }
    .vendor-width
    {
        width: 130px;   
    }
</style>

<asp:HiddenField ID="hidDisplayMode" runat="server" />
<script type="text/javascript">
    //动态增加Style样式
    function addCssByStyle(cssString) {
        var doc = document;
        var style = doc.createElement("style");
        style.setAttribute("type", "text/css");

        if (style.styleSheet) {// IE
            style.styleSheet.cssText = cssString;
        } else {// w3c	        
            var cssText = doc.createTextNode(cssString);
            style.appendChild(cssText);
        }

        var heads = doc.getElementsByTagName("head");
        if (heads.length)
            heads[0].appendChild(style);
        else
            doc.documentElement.appendChild(style);
    }

    if ($('#<%= this.hidDisplayMode.ClientID %>').val() === 'Display') {
        var dynamic_style = '#tb_purchaserequest{width: 1080px;}.PermissionDisplay{display: inline;}.DescWidth{width: 175px;}';
        addCssByStyle(dynamic_style);
    } else {
        var dynamic_style = '.PermissionDisplay{display: none;}.DescWidth{width: 257px;}';
        addCssByStyle(dynamic_style);
    }
</script>

<table class="ca-workflow-form-table">
    <tr>
        <td class="label align-center" style="width: 119px;">
            Request No
            <br />
            申请编号
        </td>
        <td class="value">
            <QFL:FormField ID="FormField1" runat="server" FieldName="WorkflowNumber" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Form Type
            <br />
            页面类型
        </td>
        <td class="value">
            <span id="ca_pr_formtype"><QFL:FormField ID="FormField5" runat="server" FieldName="FormType" ControlMode="Display" ></QFL:FormField></span>
            <span class="hidden"><QFL:FormField ID="FormField6" runat="server" FieldName="PRHOPurpose" ControlMode="Display" ></QFL:FormField></span>
            <span class="hidden"><QFL:FormField ID="FormField2" runat="server" FieldName="PRStorePurpose" ControlMode="Display" ></QFL:FormField></span>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            PO to be Returned
            <br />
            被退货单编号
        </td>
        <td class="value">
            <span id="ca_pr_isreturn"><QFL:FormField ID="FormField7" runat="server" FieldName="IsReturn" ControlMode="Display" ></QFL:FormField></span>
            <span class="hidden"><QFL:FormField ID="FormField9" runat="server" FieldName="ReturnNumber" ControlMode="Display" ></QFL:FormField></span></td>
    </tr>
</table>

<table class="ca-workflow-form-table">
    <tr>
        <td class="label align-center" style="width: 119px;">
            Type
            <br />
            类型
        </td>
        <td class="value" id="request_type">
            <QFL:FormField ID="ffRequestType" runat="server" FieldName="RequestType" ControlMode="Display" ></QFL:FormField>
        </td>
        <td class="value align-center hidden" id="capex_type">
            <QFL:FormField ID="ffCapexType" runat="server" FieldName="CapexType" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
</table>

<table class="ca-workflow-form-table"  id="tb_purchaserequest">
    <tr>
        <td colspan="4" class="label align-center">
            <h3>
                Purchase Request Details 明细
            </h3>
        </td>
    </tr>
    <tr>
        <td colspan="4">
                    <table class="inner-table" id="ca_pr_details">
                        <tr>
                            <td class="label align-center" style="width: 30px;">
                                No
                                <br />
                                编号
                            </td>
                            <td class="label align-center" style="width: 80px;">
                                Item Code
                                <br />
                                产品代码
                            </td>
                            <td class="label align-center DescWidth">
                                Description
                                <br />
                                产品描述
                            </td>
                            
                            <td class="label align-center" style="width: 55px;">
                                Total QTY
                                <br />
                                <span id="label_total_qty">订购量</span>
                            </td>
                            <td class="label align-center PermissionDisplay TransQTY" style="width: 55px;">
                                Trans QTY
                                <br />
                                调拨数量
                            </td>
                            <td class="label align-center PermissionDisplay PurQTY" style="width: 50px;">
                                Purchase QTY
                                <br />
                                购买数量
                            </td>

                            <td class="label align-center" style="width: 30px;">
                                Unit
                                <br />
                                单位
                            </td>
                            <td class="label align-center PermissionDisplay vendor-width" id="ca_pr_col_vendor">
                                Vendor
                                <br />
                                供应商
                            </td>
                            <td class="label align-center PermissionDisplay" style="width: 48px;">
                                Currency
                                <br />
                                货币
                            </td>
                            <td class="label align-center PermissionDisplay" style="width: 44px;">
                                Rate
                                <br />
                                汇率
                            </td>
                            <td class="label align-center PermissionDisplay" style="width: 55px;">
                                Unit Price
                                <br />
                                单价
                            </td>
                            <td class="label align-center PermissionDisplay" style="width: 60px;">
                                Total Price
                                <br />
                                总价
                            </td>
                            <td class="label align-center hidden">
                                Delivery Period
                            </td>
                            <td class="label align-center hidden">
                                Tax Value
                            </td>
                            
                            <td class="label align-center" style="width: 65px;">
                                Cost Center
                                <br />
                                成本中心
                            </td>
                            <td class="label align-center" style="width: 35px;">
                                Photo
                                <br />
                                图片
                            </td>
                            <td class="label align-center" style="width: 60px;">
                                A/C Code
                                <br />
                                财务代码
                            </td>
                        </tr>
                        <asp:Repeater ID="rptItem" runat="server" >
                            <ItemTemplate>
                                <tr>
                                    <td class="label align-center">
                                        <%# Container.ItemIndex + 1 %>
                                    </td>
                                    <td class="label align-center ddl">
                                        <%# Eval("ItemCode")%>
                                    </td>
                                    <td class="label">
                                        <%# Eval("Description")%>
                                    </td>                             
                                    
                                    <td class="label align-center">
                                        <%# Eval("TotalQuantity")%>
                                    </td>
                                    <td class="label align-center PermissionDisplay TransQuantity">
                                        <%# Eval("TransQuantity")%>
                                    </td>
                                    <td class="label align-center PermissionDisplay RequestQuantity">
                                        <%# Eval("RequestQuantity")%>
                                    </td>
                                    <td class="label align-center">
                                        <%# Eval("Unit")%>
                                    </td>
                                    <td class="label PermissionDisplay">
                                        <%# Eval("VendorName")%>
                                    </td>
                                    <td class="label align-center PermissionDisplay">
                                        <%# Eval("Currency")%>
                                    </td>
                                    <td class="label align-center PermissionDisplay">
                                        <%# Eval("ExchangeRate")%>
                                    </td>
                                    <td class="label align-center PermissionDisplay">
                                        <%# Eval("UnitPrice")%>
                                    </td>
                                    <td class="label align-center PermissionDisplay">
                                        <%# Math.Round(Convert.ToDouble(Eval("TotalPrice")), 2)%>
                                    </td>
                                    <td class="label align-center ddl">
                                        <%# Eval("CostCenter")%>&nbsp;-&nbsp;<%# Eval("CostCenterName")%>
                                    </td>
                                    <td class="label align-center">
                                        <a target="_blank" href='<%# string.Format("/WorkFlowCenter/ItemPic/{0}.jpg", Eval("ItemCode")) %>'>Link</a>
                                    </td>
                                    <td class="label align-center ddl">
                                        <%# Eval("AssetClass")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>                        
                    </table>
        </td>
    </tr>
    <tr class="bold-bottom-line PermissionDisplay" id="request_total">
        <td class="label align-center w20">
            Total (RMB):
            <br />
            总计
        </td>
        <td class="label w30">
            <QFL:FormField ID="ffTotalRMB" runat="server" FieldName="TotalRMB" ControlMode="Display" ></QFL:FormField>
        </td>
        
        <td class="label align-center w20">
            Approval Total (RMB):
            <br />
            工作流审批金额
        </td>
        <td class="label w30">
            <QFL:FormField ID="ffApprovalTotal" runat="server" FieldName="ApprovalTotalRMB" ControlMode="Display" ></QFL:FormField>
        </td>

    </tr>
</table>

<table class="ca-workflow-form-table">
    <tr>
        <td class="label align-center" id="request_reason">
            Reason of Request
            <br />
            申请理由
        </td>
        <td class="value" colspan="3">
            <QFL:FormField ID="ffReason" runat="server" FieldName="RequestReason" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Applicant
            <br />
            申请人
        </td>
        <td class="value" colspan="3">
            <QFL:FormField ID="ffBuyer" runat="server" FieldName="Applicant" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
</table>



<script type="text/javascript">
    var JSPurchaseRequest = {};

    $(function () {
        var requestType = jQuery.trim($('#request_type').text());
        if (requestType === 'Capex') {
            $('#request_type').text(requestType + ' - ' + $('#capex_type').text());
        }

        SetReturnStyle(); //当PR单为退货时设置显示样式
        SetFormStyle(); //设置FormType内容显示
    });

    function SetFormStyle() {
        var $form_type = $('#ca_pr_formtype');
        var formType = jQuery.trim($form_type.text());
        if (formType === 'HO') {
            $form_type.next('span').removeClass('hidden');
            $form_type.append(' - ');
        } else if (formType === 'Store') {
            $form_type.next('span').next('span').removeClass('hidden');
            $form_type.append(' - ');
        }
    }

    //设置退货界面，若退货被选中，则显示后面的退货编号输入框，否则则隐藏
    function SetReturnStyle() {
        $isreturn = $('#ca_pr_isreturn');
        var is_return = jQuery.trim($isreturn.text()) === 'Yes';
        if (is_return) {
            $('#label_total_qty').text('退货量');
            $isreturn.text($isreturn.next('span').text()); //设置退货单编号
        } else {
            $isreturn.parent().parent().hide();
        }

        if ($('#<%= this.hidDisplayMode.ClientID %>').val() === 'Display') {
            if (is_return) {
                $('#ca_pr_col_vendor').removeClass('vendor-width');
                $('#ca_pr_col_vendor').addClass('return-width-vendor');
                $('#ca_pr_details td.PurQTY').addClass('return-hide');
                $('#ca_pr_details td.TransQTY').addClass('return-hide');
                $('#ca_pr_details td.RequestQuantity').addClass('return-hide');
                $('#ca_pr_details td.TransQuantity').addClass('return-hide');
            } else {
                $('#ca_pr_col_vendor').addClass('vendor-width');
                $('#ca_pr_col_vendor').removeClass('return-width-vendor');
                $('#ca_pr_details td.PurQTY').removeClass('return-hide');
                $('#ca_pr_details td.TransQTY').removeClass('return-hide');
                $('#ca_pr_details td.RequestQuantity').removeClass('return-hide');
                $('#ca_pr_details td.TransQuantity').removeClass('return-hide');
            }
        }

        //设置退货编号的显示
        if (is_return) {
            $('#ca-isreturn').text('Return PO No.');
            $('#return-table').removeClass('hidden');
            $('#ca-pr-isreturn').removeClass('hidden');
        }
    }
</script>


