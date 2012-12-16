<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" 
Inherits="CA.WorkFlow.UI.PurchaseRequest.DataEdit" %>
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
    select.width-fix
    {
        width: 70px;
        z-index: 1000;
    }
    td.ddl
    {
        padding-top: 12px; 
        vertical-align: top;
    }
    select.expand
    {
        position: absolute;
        width: 180px; /* Let the browser handle it. */
    }
    .ca-workflow-form-table td.label, .ca-workflow-form-table td.value {
        padding: 2px;
    }
    .tmpvalue
    {
        padding: 5px;
    }
    .list-view input
    {
        width: 48px;   
    }
    input.smallinput
    {
        width: 35px;   
    }
    input.ca-button
    {
        border: 1px solid #9DABB6;
        color: #000000;
        margin: 0;
        margin-left: 5px;
        cursor: pointer;
        width: 40px;   
    }
    .upload-pane
    {
        width: 280px !important;
        
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
        var dynamic_style = '#tb_purchaserequest{width: 1095px;}.PermissionDisplay{display: inline;}.DescWidth{width: 175px;}.PermissionLabel{border-bottom: 1px solid #CCCCCC;padding-left:2px;}';
        addCssByStyle(dynamic_style);
    } else {
        var dynamic_style = '.PermissionDisplay{display: none;}.DescWidth{width: 257px;}.PermissionLabel{border-bottom: none}';
        addCssByStyle(dynamic_style);
    }
</script>
<table class="ca-workflow-form-table" style="border-bottom: none; margin-bottom: 0;">
    <tr>
        <td class="label align-center" style="width: 137px; border-bottom: none;">
            Form Type
            <br />
            页面类型
        </td>
        <td class="PermissionLabel">
            <asp:RadioButtonList ID="rbFormType" runat="server" CssClass="radio" RepeatDirection="Horizontal" onclick="return SelectFormType(this, true);">
                <asp:ListItem Value="Store" Text="Store Request" Selected="True" />
                <asp:ListItem Value="HO" Text="HO Request" />
            </asp:RadioButtonList>
        </td>
    </tr>
    
    <tr class="hidden" id="pr_store_purpose">
        <td class="label-right"></td>
        <td style="padding: 2px;">
            <asp:RadioButtonList ID="rbPRStorePurpose" runat="server" CssClass="radio" RepeatDirection="Horizontal" onclick="return SwitchStorePurpose(this);">
                <asp:ListItem Value="Daily" Text="Daily Request" Selected="True"/>
                <asp:ListItem Value="QuarterlyOrder" Text="Quarterly Order Request" />
                <asp:ListItem Value="PaperBag" Text="Paper Bag Request" />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr class="hidden" id="pr_ho_purpose">
        <td class="label-right"></td>
        <td style="padding: 2px;">
            <asp:RadioButtonList ID="rbPRHOPurpose" runat="server" CssClass="radio" RepeatDirection="Horizontal" >
                <asp:ListItem Value="Construction" Text="For Construction" />
                <asp:ListItem Value="Store" Text="For Store" Selected="True"/>
                <asp:ListItem Value="Department" Text="For Other Departments" />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td class="label-right"></td>
        <td style="padding: 2px;">
            <ul>
                <li><asp:FileUpload ID="uploadAttachment" CssClass="upload-pane" runat="server" /></li>
                <li class="ca-workflow-form-buttons"><asp:Button ID="btnUpload" Text="上传" runat="server" CssClass="ca-button" OnClick="btnUpload_Click" /></li>
                <li class="ca-workflow-form-buttons"> <a href="/tmpfiles/PurchaseRequest/Purchase Request_201112.xlsx">Excel template download</a></li>
            </ul>
        </td>
    </tr>
</table>

<table class="ca-workflow-form-table" id="return-table" style="border-top: 1px solid #CCCCCC; margin-top: 0;">
    <tr>
        <td class="label" style="width: 119px; padding-left: 20px; height: 30px;">
            <table>
                <tr>
                    <td class="box">
                        <QFL:FormField ID="ffIsReturn" runat="server" FieldName="IsReturn" ControlMode="Edit" ></QFL:FormField>
                    </td>
                    <td>
                        Return
                    </td>
                </tr>
            </table>
        </td>
        <td class="label" >
            <span id="return_pr_no"><QFL:FormField ID="ffPRNumber" runat="server" FieldName="ReturnNumber" ControlMode="Edit" ></QFL:FormField></span>
        </td>
        <td class="label" style="width: 120px;">
            
        </td>
    </tr>
</table>

<table class="ca-workflow-form-table">
    <tr>
        <td rowspan="2" class="label align-center" style="width: 137px;">
            Type
            <br />
            类型
        </td>
        <td class="tmpvalue" id="request_type">
            <asp:RadioButtonList ID="rbRequestType" runat="server" CssClass="radio" RepeatDirection="Horizontal" onclick="return SwitchRequestType(this);">
                <asp:ListItem Value="Opex" Text="Opex 易耗品" Selected="True" />
                <asp:ListItem Value="Capex" Text="Capex 固定资产" />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td class="value" id="capex_type">
            <asp:RadioButtonList ID="rbCapexType" runat="server" CssClass="radio" RepeatDirection="Horizontal" >
                <asp:ListItem Value="NewProject" Text="New Project 新店" Selected="True" />
                <asp:ListItem Value="Refurbishment" Text="Refurbishment 改造店" />
                
                <asp:ListItem Value="Exist" Text="Existing Store 旧店" />
            </asp:RadioButtonList>
        </td>
    </tr>
</table>

<table class="ca-workflow-form-table"  id="tb_purchaserequest">
    <tr>
        <td colspan="4" class="label align-center">
            <h3>
                <span id="pr_detail_title">Purchase Request Details</span> 明细
            </h3>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="inner-table list-view" id="ca_pr_details">
                        <tr>
                            <td class="cell align-center" style="width: 25px;">
                                <asp:ImageButton runat="server" ID="btnAddItem" ToolTip="Click to add the PR information."
                                    ImageUrl="../images/pixelicious_001.png" OnClick="btnAddItem_Click" Width="18"
                                    CssClass="img-button" />
                            </td>
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
                            
                            <td class="label align-center" style="width: 80px;">
                                Cost Center
                                <br />
                                成本中心
                            </td>
                            <td class="label align-center" style="width: 80px;">
                                Item Type
                                <br />
                                产品类型
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
                            <td class="hidden">
                                IsAccpetDecimal
                            </td>
                        </tr>
                        <asp:Repeater ID="rptItem" runat="server" OnItemCommand="rptItem_ItemCommand"
                            OnItemDataBound="rptItem_ItemDataBound">
                            <ItemTemplate>
                                <tr class="dr-row">
                                    <td class="cell align-center">
                                        <asp:ImageButton ID="btnDeleteItem" ToolTip="Remove this PR information."
                                            CommandName="delete" runat="server" ImageUrl="../images/pixelicious_028.png"
                                            Width="18" CssClass="img-button delete-button" />
                                    </td>
                                    <td class="label align-center">
                                        <asp:Label ID="lbItem" runat="server" />
                                    </td>
                                    <td class="label ItemCode">
                                        <asp:Label ID="lbItemCode" CssClass="SelectItemCode" runat="server" />
                                        <img width="18px" alt="Select" onclick="return OpenDialog(this);" src="/_layouts/CAResources/themeCA/images/load.gif">
                                        <asp:HiddenField ID="hidItemCode" runat="server" />
                                    </td>
                                    <td class="label Desc">
                                        <asp:Label ID="lbDesc" runat="server" />
                                        <asp:HiddenField ID="hidDesc" runat="server" />
                                        <asp:HiddenField ID="HFPackagedRegulation" runat="server" />
                                    </td>
                                    <td class="label align-center TotalQuantity">
                                        <asp:TextBox ID="txtTotalQuantity" runat="server" onchange="return UpdateTotal(this);" />
                                    </td>
                                    <td class="label align-center PermissionDisplay TransQuantity">
                                        <asp:Label ID="lbTransQuantity" runat="server" Text="0" CssClass="hidden" />
                                        <asp:TextBox ID="txtTransQuantity" runat="server" Text="0" onchange="return UpdateTotal(this);" />
                                    </td>
                                    <td class="label align-center PermissionDisplay RequestQuantity">
                                        <asp:Label ID="lbRequestQuantity" runat="server" />
                                        <asp:HiddenField ID="hidRequestQuantity" runat="server" />
                                    </td>
                                    <td class="label align-center">
                                        <asp:Label ID="lbUnit" runat="server" />
                                        <asp:HiddenField ID="hidUnit" runat="server" />
                                    </td>
                                    <td class="label PermissionDisplay">
                                        <asp:Label ID="lbVendor" runat="server" />
                                        <asp:HiddenField ID="hidVendor" runat="server" />
                                        <asp:HiddenField ID="hidVendorId" runat="server" />
                                    </td>
                                    <td class="label align-center PermissionDisplay">
                                        <asp:Label ID="lbCurrency" runat="server" />
                                        <asp:HiddenField ID="hidCurrency" runat="server" />
                                    </td>
                                    <td class="label align-center PermissionDisplay ExchangeRate">
                                        <asp:TextBox ID="txtExchangeRate" runat="server" Text="1" CssClass="smallinput" onchange="return UpdateTotal(this);" />
                                    </td>
                                    <td class="label align-center PermissionDisplay UnitPrice">
                                        <asp:Label ID="lbUnitPrice" runat="server" />
                                        <asp:TextBox ID="txtUnitPrice" runat="server" Text="0" CssClass="hidden" onchange="return UpdateTotal(this);" />
                                    </td>
                                    <td class="label align-center PermissionDisplay TotalPrice">
                                        <asp:Label ID="lbTotalPrice" runat="server" Text="0" />
                                    </td>
                                    <td class="label align-center hidden DeliveryPeriod">
                                        <asp:Label ID="lbDeliveryPeriod" runat="server" />
                                        <asp:HiddenField ID="hidDeliveryPeriod" runat="server" />
                                    </td>
                                    <td class="label align-center hidden">
                                        <asp:Label ID="lbTaxValue" runat="server" />
                                        <asp:HiddenField ID="hidTaxValue" runat="server" />
                                    </td>
                                    
                                    <td class="label ddl CostCenter">
                                        <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="width-fix" >                                            
                                        </asp:DropDownList>
                                    </td>
                                    <td class="label ItemType">
                                        <asp:Label ID="lbItemType" runat="server" />
                                        <asp:HiddenField ID="hidItemType" runat="server" />
                                    </td>
                                    <td class="label align-center">
                                        <asp:HyperLink ID="hlPhoto" runat="server" NavigateUrl="#" Target="_blank">Link</asp:HyperLink>
                                    </td>
                                    <td class="label">
                                        <asp:Label ID="lbAssetClass" runat="server" />
                                        <asp:HiddenField ID="hidAssetClass" runat="server" />
                                    </td>
                                    <td class="IsAccpetDecimal">
                                        <asp:HiddenField ID="hdIsAccpetDecimal" runat="server" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQueryItemDetail" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnChangeType" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr class="bold-bottom-line PermissionDisplay" id="request_total">
        <td class="label align-center w20">
            Total (RMB):
            <br />
            总计
        </td>
        <td class="label w30">
            <asp:Label ID="lbTotal" runat="server" Text="0" />
        </td>
        
        <td class="label align-center w20">
            Approval Total (RMB):
            <br />
            工作流审批金额
        </td>
        <td class="label w30">
            <asp:Label ID="lbApproveTotal" runat="server" Text="0" />
        </td>

        
    </tr>
</table>

<table class="ca-workflow-form-table">
    <tr>
        <td class="label align-center" id="request_reason" style="width: 143px;">
            Reason of Request
            <br />
            申请理由
        </td>
        <td class="value">
            <QFL:FormField ID="ffReason" runat="server" FieldName="RequestReason" ControlMode="Edit" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Applicant
            <br />
            申请人
        </td>
        <td class="value">
            <asp:Label ID="lbApplicant" runat="server" />
        </td>
    </tr>
</table>

<asp:HiddenField ID="hidSelectedItem" runat="server" />
<asp:HiddenField ID="hidTotal" runat="server" />

<asp:HiddenField ID="hidIsShowCache" runat="server" Value="Y" />
<asp:HiddenField ID="hidVendors" runat="server" />
<asp:Button runat="server" ID="btnQueryItemDetail" OnClick="btnQueryItemDetail_Click"
    Text="Query" CausesValidation="False" CssClass="hidden" />
<asp:Button runat="server" ID="btnChangeType" OnClick="btnChangeType_Click"
    Text="Query" CausesValidation="False" CssClass="hidden" />
<asp:Button runat="server" ID="btnCheckReturnNo" OnClick="btnCheckReturnNo_Click"
    Text="Query" CausesValidation="False" CssClass="hidden" />

<script type="text/javascript">
    var JSPurchaseRequest = {};//命名空间

    //获取Common Pre Id
    JSPurchaseRequest.GetPreId = function (tmpId) {
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_rptItem_ctl00_ddlItemCode
        return tmpId.substring(0, tmpId.lastIndexOf('_') + 1);
    }

    //根据ItemCode进行服务端查询，已不再使用
    function QueryItemInfo(obj) {
        var objId = obj.id;
        var preId = JSPurchaseRequest.GetPreId(objId);

        var parmas = $(obj).val() + ';' + preId;
        $('#<%= this.hidSelectedItem.ClientID %>').val(parmas);
        $('#<%= this.btnQueryItemDetail.ClientID %>').click();
    }

    function SetBorderWarn($obj) {
        $obj.css('border', '1px solid red');
    }

    function ClearBorderWarn($obj) {
        $obj.css('border', '');
        $obj.css('border-bottom', '#999 1px solid');
    }

    //点击提交事保存时，进行本地合法性检查。如是拒绝，则必须输入Comment
    function beforeSubmit(sender) {
        var result = false;
        var isExistSameItem = CheckSameItemCode(); //验证的costcenter下只能选择一次Itemcode
        if (isExistSameItem) {
            return result;
        }

        if (sender.value == "Submit") {
            ///验证是纸袋是的话如果有数量超过50会计算总量，提示用户是否保存。并验证包装规则数量是否合法。
            var checkPaperBag = CheckPaperBageData(50);
            if (!checkPaperBag) {
                return false
            }
        }

        if (sender.value === 'Submit' || sender.value === 'Confirm') {
            result = Validate();
            if (!result) {
                if (jQuery.browser.msie) {
                    event.cancelBubble = true;
                }
            }
        } else if (sender.value === 'Save') {
            result = ValidateForSave();
            if (!result) {
                if (jQuery.browser.msie) {
                    event.cancelBubble = true;
                }
            }
        } else if (sender.value === 'Reject') {
            result = !ca.util.emptyString($('#comment-task textarea').val());
            if (!result) {
                if (jQuery.browser.msie) {
                    event.cancelBubble = true;
                }
                alert('Please fill in the Reject Comments.');
                return false;
            }
        }

        //验证购买数量不能为零
        if (result) {
            var checkZero = CheckRequestIsZero();
            if (!checkZero) {
                alert('Purchase QTY (购买数量) inavailable!');
                return false;
            }
        }

        //验证数据中是否有ItemType为空的数据。
        if (result) {
            var emptyItemtype = IsEmptyItemType();
            if (emptyItemtype) {
                return false;
            }
        }
        
        if (result) {//验证通过 生成弹出层，防止重复提交。
            CreateForbidDIV(); //单击生成弹出层，防止重复提交。
        }
        return result;
    }

    //提交请求时进行合法性验证
    function Validate() {
        var error = '';

        var flag = 0;
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_ffIsReturn_ctl00_ctl00_BooleanField
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_ffPRNumber_ctl00_ctl00_TextField
        //门店不需要输入退货编号，HO必须帮助门店输入。因此前面判断是否为HO
        //退货申请只与一个PO号进行绑定
        var return_po = jQuery.trim($('#<%=this.ffPRNumber.ClientID%>_ctl00_ctl00_TextField').val());
        if ($('#<%= this.hidDisplayMode.ClientID %>').val() === 'Display'
            && $('#<%=this.ffIsReturn.ClientID%>_ctl00_ctl00_BooleanField').is(':checked')
            && (ca.util.emptyString(return_po) || return_po === 'Please input PO numbers of goods to be returned')) {
            error += 'Please input PO numbers of goods to be returned.\n';
            SetBorderWarn($('#<%=this.ffPRNumber.ClientID%>_ctl00_ctl00_TextField'));
        } else if (return_po.replace(/[^;]/g, '').length != 0
                    || return_po.replace(/[^,]/g, '').length != 0) {
            error += 'Please do not input multi PO numbers.\n';
            SetBorderWarn($('#<%=this.ffPRNumber.ClientID%>_ctl00_ctl00_TextField'));
        } else {
            ClearBorderWarn($('#<%=this.ffPRNumber.ClientID%>_ctl00_ctl00_TextField'));
        }

        if (ca.util.emptyString($('#<%=this.ffReason.ClientID%>_ctl00_ctl00_TextField').val())) {
            error += 'Please fill in the Reason for Request field.\n';
            SetBorderWarn($('#<%=this.ffReason.ClientID%>_ctl00_ctl00_TextField'));
        } else {
            ClearBorderWarn($('#<%=this.ffReason.ClientID%>_ctl00_ctl00_TextField'));
        }

        error += ValidateNumber();

        if (error) {
            alert(error);
        }

        return error.length === 0;
    }

    //保存事件时执行合法性检查
    function ValidateForSave() { //检查repeat控件中数量输入框为数字且非空
        var error = '';

        error += ValidateNumber();

        if (error) {
            alert(error);
        }

        return error.length === 0;
    }

    function ValidateNumber() {
        var error = '';

        var flag = 0;
        //Class以Quantity结尾的输入框中要求为数字
        $('#ca_pr_details td[class$=\'Quantity\']').find('input').each(function () {
            if (ca.util.emptyString($(this).val()) || $(this).val().length === 0 || isNaN($(this).val())) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please fill the valid quantity.\n' : '';

        flag = 0;
        //Class是UnitPrice的输入框中要求为数字
        $('#ca_pr_details td.UnitPrice').find('input').each(function () {
            if (ca.util.emptyString($(this).val()) || $(this).val().length === 0 || isNaN($(this).val())) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please fill the valid unit price.\n' : '';

        flag = 0;
        //Class是ExchangeRate的输入框中要求为数字
        $('#ca_pr_details td.ExchangeRate').find('input').each(function () {
            if (ca.util.emptyString($(this).val()) || $(this).val().length === 0 || isNaN($(this).val())) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please fill the valid exchange rate.\n' : '';

        flag = 0;
        //Class是CostCenter的下拉框中必须选中非空项
        $('#ca_pr_details td.CostCenter').find('select').each(function () {
            if ($(this).val() == '-1') {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please select cost center for item.\n' : '';

        flag = 0;
        //验证每行中Item Code必须有值
        $('#ca_pr_details td.ItemCode').find('input').each(function () {
            if (ca.util.emptyString($(this).val())) {
                flag++;
                SetBorderWarn($(this));
            } else {
                ClearBorderWarn($(this));
            }
        });
        error += flag > 0 ? 'Please select item code first.\n' : '';

        flag = 0;
        var maintenace_count = 0;
        var non_maintenace_count = 0;
        if ($('#<%= this.hidDisplayMode.ClientID %>').val() != 'Display') {
            $('#ca_pr_details td.ItemType').find('span').each(function () {
                if ($(this).text() === 'Maintenance') {
                    maintenace_count++;
                } else {
                    non_maintenace_count++;
                }
            });
            error += (maintenace_count != 0 && non_maintenace_count != 0) ? 'The request contains the items with maintenance and non-maintenance type.\n' : '';
        }

        //如果前面验证均通过，则下段代码将进行是否有小数位的验证。其中是否有小数位则由Item Code在基础表中进行维护。若表中值为0则表示不允许为小数，反之亦然
        var $tmp_obj = null;
        var is_decimal;
        if (error.length === 0) {
            flag = 0;
            $('#ca_pr_details td[class$=\'Quantity\']').each(function () {
                $tmp_obj = $(this).find('input').eq(0);
                is_decimal = $(this).siblings('td.IsAccpetDecimal').children('input').eq(0).val();
                if (is_decimal === '0') {
                    if ($tmp_obj.val().indexOf('.') != -1) {
                        flag++;
                        SetBorderWarn($tmp_obj);
                    } else {
                        ClearBorderWarn($tmp_obj);
                    }
                }
            });
            error += flag > 0 ? 'The system can not accept decimal for this item. Please fill the valid number.' : '';
        }

        return error;
    }

    //供Change事件调用
    function UpdateTotal(sender) {
        JSPurchaseRequest.pre_id = JSPurchaseRequest.GetPreId($(sender).attr('id'));
        

        JSPurchaseRequest.total_quantity = $('#' + JSPurchaseRequest.pre_id + 'txtTotalQuantity').val();
        JSPurchaseRequest.trans_quantity = $('#' + JSPurchaseRequest.pre_id + 'txtTransQuantity').val();
        JSPurchaseRequest.unit_price = $('#' + JSPurchaseRequest.pre_id + 'txtUnitPrice').val();
        JSPurchaseRequest.exchange_rate = $('#' + JSPurchaseRequest.pre_id + 'txtExchangeRate').val();

        //验证订购量，调拨量，单位和汇率是否合法
        if (isNaN(JSPurchaseRequest.total_quantity)
            || isNaN(JSPurchaseRequest.trans_quantity)
            || isNaN(JSPurchaseRequest.unit_price)
            || isNaN(JSPurchaseRequest.exchange_rate)) {
            alert('Please fill the valid number.');
            $('#' + JSPurchaseRequest.pre_id + 'lbTotalPrice').text(0);//非法输入时设置当前行总价为0
            return false;
        }

        var $ic = $('#' + JSPurchaseRequest.pre_id + 'hidItemCode');
        if ($ic.val().length === 0) {
            return false;
        }

        //验证是否允许输入小数位
        JSPurchaseRequest.is_accept_decimal = $('#' + JSPurchaseRequest.pre_id + 'hdIsAccpetDecimal').val();
        if (!ca.util.emptyString(JSPurchaseRequest.is_accept_decimal) && JSPurchaseRequest.is_accept_decimal === '0') {
            if (JSPurchaseRequest.total_quantity.indexOf('.') != -1
                || JSPurchaseRequest.trans_quantity.indexOf('.') != -1) {
                alert('The system can not accept decimal for this item. Please fill the valid number.');
                $('#' + JSPurchaseRequest.pre_id + 'lbTotalPrice').text(0); //非法输入时设置当前行总价为0
                return false;
            }
        }

        JSPurchaseRequest.itemCode = $('#' + JSPurchaseRequest.pre_id + 'hidItemCode').val();
        if (JSPurchaseRequest.itemCode.indexOf('X') === 0) {
            $('#' + JSPurchaseRequest.pre_Id + 'lbTransQuantity').show(); //X开头的服务费不能有调拨费用，默认为0
            $('#' + JSPurchaseRequest.pre_id + 'txtTransQuantity').val(0).hide();
            $('#' + JSPurchaseRequest.pre_id + 'txtTransQuantity').hide();
            JSPurchaseRequest.trans_quantity = '0';
        }

        //自动去除数字前面的0和前后的空格
        JSPurchaseRequest.total_quantity = AutoCorrectData(JSPurchaseRequest.total_quantity);
        JSPurchaseRequest.trans_quantity = AutoCorrectData(JSPurchaseRequest.trans_quantity);
        JSPurchaseRequest.unit_price = AutoCorrectData(JSPurchaseRequest.unit_price);
        JSPurchaseRequest.exchange_rate = AutoCorrectData(JSPurchaseRequest.exchange_rate);
        $('#' + JSPurchaseRequest.pre_id + 'txtTotalQuantity').val(JSPurchaseRequest.total_quantity);
        $('#' + JSPurchaseRequest.pre_id + 'txtTransQuantity').val(JSPurchaseRequest.trans_quantity);
        $('#' + JSPurchaseRequest.pre_id + 'txtUnitPrice').val(JSPurchaseRequest.unit_price);
        $('#' + JSPurchaseRequest.pre_id + 'txtExchangeRate').val(JSPurchaseRequest.exchange_rate);

        //当数据不完整时，当前行总价置为0，同时不再继续计算总价
        if (ca.util.emptyString(JSPurchaseRequest.total_quantity)
            || ca.util.emptyString(JSPurchaseRequest.trans_quantity)
            || ca.util.emptyString(JSPurchaseRequest.unit_price)
            || ca.util.emptyString(JSPurchaseRequest.exchange_rate)) {
            $('#' + JSPurchaseRequest.pre_id + 'lbTotalPrice').text(0); //输入不完整时设置当前行总价为0
            return false;
        }

        

        //计算需购买总数
        JSPurchaseRequest.request_quantity = parseFloat(JSPurchaseRequest.total_quantity) - parseFloat(JSPurchaseRequest.trans_quantity);
        $('#' + JSPurchaseRequest.pre_id + 'lbRequestQuantity').text(JSPurchaseRequest.request_quantity);
        $('#' + JSPurchaseRequest.pre_id + 'hidRequestQuantity').val(JSPurchaseRequest.request_quantity);

        //计算当前行总价，并进行千分号显示
        JSPurchaseRequest.total_price = parseFloat(JSPurchaseRequest.total_quantity) * parseFloat(JSPurchaseRequest.unit_price);
        $('#' + JSPurchaseRequest.pre_id + 'lbTotalPrice').text(commafy(JSPurchaseRequest.total_price));

        CalcTotal();//更新当前PR总价
    }

    //根据Repeater控件中控件值进行总数的计算，同时调用千分号函数进行显示。若一行中有计算值处于空白状态，则该行略过
    function CalcTotal() {
        JSPurchaseRequest.total = 0;
        JSPurchaseRequest.total_return = 0;
        JSPurchaseRequest.total_approval = 0;
        var tmpTotal = 0;
        var is_discount = false;
        var is_return = $('#<%= this.ffIsReturn.ClientID %>_ctl00_ctl00_BooleanField').is(':checked');
        $('#ca_pr_details td.TotalPrice').each(function () {
            JSPurchaseRequest.curr_span = $(this).children('span').eq(0);
            JSPurchaseRequest.curr_totalprice = JSPurchaseRequest.curr_span.text().replace(/,/g, '');
            JSPurchaseRequest.curr_exch = $(this).siblings('td.ExchangeRate').children('input').eq(0).val();
            JSPurchaseRequest.itemcode = $(this).siblings('td.ItemCode').children('input').eq(0).val();
            JSPurchaseRequest.itemdesc = $(this).siblings('td.Desc').children('input').eq(0).val();
            is_discount = (JSPurchaseRequest.itemcode.indexOf('X') === 0) && contains(JSPurchaseRequest.itemdesc, 'discount', true);

            if (isNaN(JSPurchaseRequest.curr_totalprice) || JSPurchaseRequest.curr_totalprice.length === 0) return true; //如果当前行总价非数字，则跳过
            if (isNaN(JSPurchaseRequest.curr_exch) || JSPurchaseRequest.curr_exch.length === 0) return true; //如果汇率为非数字，则跳过
            tmpTotal = parseFloat(JSPurchaseRequest.curr_totalprice) * parseFloat(JSPurchaseRequest.curr_exch); //总价，汇率转换
            JSPurchaseRequest.total += (!is_discount) ? tmpTotal : (-tmpTotal); ; //累加总价
            JSPurchaseRequest.total_approval += (is_discount && !is_return) ? -tmpTotal : tmpTotal; //审批金额，绝对值相加;审批金额正常时需减去折扣
            JSPurchaseRequest.total_return += (JSPurchaseRequest.itemcode.indexOf('X') === 0) ? tmpTotal : (-tmpTotal); //计算当为退货时的总价
            JSPurchaseRequest.curr_span.text(commafy(JSPurchaseRequest.curr_totalprice)); //千分号显示当前行总价
        });

        $('#<%= this.lbTotal.ClientID %>').attr('amount', commafy(JSPurchaseRequest.total)).attr('returnamount', commafy(JSPurchaseRequest.total_return)); //将总价格保存在属性中

        if (is_return) {
            $('#<%= this.lbTotal.ClientID %>').text(commafy(JSPurchaseRequest.total_return));
            $('#<%= this.lbApproveTotal.ClientID %>').text(commafy(JSPurchaseRequest.total_approval));
        } else {
            $('#<%= this.lbTotal.ClientID %>').text(commafy(JSPurchaseRequest.total));
            $('#<%= this.lbApproveTotal.ClientID %>').text(commafy(JSPurchaseRequest.total_approval));
        }
        return false;
    }

    function ClickReturn() {
        //选择退货时只允许是Capex类型
        if ($('#<%= this.ffIsReturn.ClientID %>_ctl00_ctl00_BooleanField').is(':checked')) {
            var requestType = $('#<%= this.rbRequestType.ClientID %> input:checked').eq(0).val();
            if (requestType != 'Capex') {
                $('#<%= this.rbRequestType.ClientID %> input').eq(1).attr('checked', true);
                $('#<%= this.rbRequestType.ClientID %> input').eq(0).attr('checked', false);

                SwitchRequestType($('#<%= this.rbRequestType.ClientID %>').get(0));
            } else {
                //显示上一次存储在属性中的值
                $lbtotal = $('#<%= this.lbTotal.ClientID %>');
                $lbtotal.text($lbtotal.attr('returnamount'));
            }
            $('#<%= this.rbRequestType.ClientID %> input').eq(0).attr('disabled', true);
        } else {
            $('#<%= this.rbRequestType.ClientID %> input').eq(0).attr('disabled', false);
            //存储当前值到属性中
            $lbtotal = $('#<%= this.lbTotal.ClientID %>');
            $lbtotal.text($lbtotal.attr('amount'));
        }
        //$('#ctl00_PlaceHolderMain_dfSelectItem_btnClose').click();
        SetReturnStyle();
    }

    //设置退货界面，若退货被选中，则显示后面的退货编号输入框，否则则隐藏
    function SetReturnStyle() {
        var is_display = false;
        is_display = $('#<%= this.hidDisplayMode.ClientID %>').val() === 'Display';

        if ($('#<%= this.ffIsReturn.ClientID %>_ctl00_ctl00_BooleanField').is(':checked')) {
            $('#label_total_qty').text('退货量');
            $('#pr_detail_title').text('Return Request Details');

            if (is_display) {
                $('#return_pr_no').removeClass('hidden');
            }

            if (ca.util.emptyString($('#return_pr_no input').val())) {
                $('#return_pr_no input').val('Please input PO numbers of goods to be returned');
            }
            
        } else {
            $('#label_total_qty').text('订货量');
            $('#return_pr_no').addClass('hidden');
            $('#pr_detail_title').text('Purchase Request Details');

            $('#return_pr_no input').val('');
        }
        if (is_display) {
            if ($('#<%= this.ffIsReturn.ClientID %>_ctl00_ctl00_BooleanField').is(':checked')) {
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
    }

    $(function () {
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_ffIsReturn_ctl00_ctl00_BooleanField
        $('#<%= this.ffIsReturn.ClientID %>_ctl00_ctl00_BooleanField').click(ClickReturn);
        SetReturnStyle();

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler); //注册服务器返回事件后执行函数
        //$('#<%= this.UpdatePanel1.ClientID %> :text').live('change', CalcTotal); //每次有修改则重新进行总价计算

        DisableEnterKey();
        CheckPermission();
        SetRequestType($('#request_type input:checked').val());

        SetAutoWidthForDDL();
        SetXTypeStyle();
        SelectFormType($('#<%= this.rbFormType.ClientID %>').get(0), false);

        CalcTotal(); //计算PR总价

        //给退货输入框加事件
        $('#return_pr_no input').click(function () {
            if ($(this).val() === 'Please input PO numbers of goods to be returned') {
                $(this).val('');
            }
        });
        RequestPBAction(); ///纸袋订单必须是OPEX， CAPEX按钮需要变灰色
    });

    function SwitchStorePurpose(sender) {
        //$('#<%= this.btnChangeType.ClientID %>').click();
        SwitchRequestType($('#<%= this.rbRequestType.ClientID %>').get(0));
    }

    function SwitchRequestType(sender) {
        var requestType = $(sender).find('input:checked').eq(0).val();
        SetRequestType(requestType);
        $('#<%= this.hidIsShowCache.ClientID %>').val('N');
        $('#<%= this.btnChangeType.ClientID %>').click();
    }

    //设置RequestType显示样式，若选择Capex，则显示Capex二级选项
    function SetRequestType(requestType) {
        if (requestType === 'Capex') {
            $('#capex_type').removeClass('hidden');
        } else {
            $('#capex_type').addClass('hidden');
        }
    }

    //服务器返回后执行函数
    function EndRequestHandler() {
        CheckPermission();
        CalcTotal();
        DisableEnterKey();
        SetAutoWidthForDDL();
        SetXTypeStyle();
        SetReturnStyle();//设置退货状态下页面显示样式
        SetClickEventForDDL(); //回传中防止click事件消失

    }

    //禁用页面Enter事件，防止repeat控件自动添加新行
    function DisableEnterKey() {
        $('#<%=this.UpdatePanel1.ClientID %>' + ' input').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('#<%=this.UpdatePanel1.ClientID %>').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('div.main-body').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
    }

    //检查用户权限，不同权限用户不同显示界面
    function CheckPermission() {
        //当StoreRequest时，在Check页面，隐藏Skip Approve选项
        if ($('#<%= this.rbFormType.ClientID %> input:checked').eq(0).val() === 'Store') {
            $('#ca_pr_labal_skipapprove').hide();
        }
    }

    //设置页面中下拉列表控件的显示样式
    function SetAutoWidthForDDL() {
        $('#<%= this.UpdatePanel1.ClientID %> select.width-fix')
            .bind('focus mouseover', function () { $(this).addClass('expand').removeClass('clicked'); })
            .bind('click', function () { if ($(this).hasClass('clicked')) { $(this).blur(); } $(this).toggleClass('clicked'); })
            .bind('mouseout', function () { if (!$(this).hasClass('clicked')) { $(this).removeClass('expand'); } })
            .bind('blur', function () { $(this).removeClass('expand clicked'); });
    }

    //重新设置下拉列表点击事件
    function SetClickEventForDDL() {
        $('#<%= this.UpdatePanel1.ClientID %> select.width-fix')
            .unbind('click')
            .bind('click', function () { if ($(this).hasClass('clicked')) { $(this).blur(); } $(this).toggleClass('clicked'); })
    }

    //设置单价样式，若ITEM CODE为X开头为Service，则单价可修改。另外纸袋开头为E0000，也需要价格可修改。隐藏Label控件，显示编辑控件
    function SetXTypeStyle() {
        JSPurchaseRequest.preId = '';
        $('#ca_pr_details td.UnitPrice').find('input').each(function () {
            JSPurchaseRequest.preId = JSPurchaseRequest.GetPreId($(this).attr('id'));

            JSPurchaseRequest.itemCode = $('#' + JSPurchaseRequest.preId + 'hidItemCode').val();
            if (ca.util.emptyString(JSPurchaseRequest.itemCode)) {
                return true; //ItemCode未选择，为空
            }

            if (JSPurchaseRequest.itemCode.indexOf('X') === 0) {
                $('#' + JSPurchaseRequest.preId + 'lbTransQuantity').show(); //X开头的服务费不能有调拨费用，默认为0
                $('#' + JSPurchaseRequest.preId + 'txtTransQuantity').val(0);
                $('#' + JSPurchaseRequest.preId + 'txtTransQuantity').hide();

                $(this).prev('span').addClass('hidden');
                $(this).removeClass('hidden');
            } else if (JSPurchaseRequest.itemCode.indexOf('E0000') === 0) {
                $(this).prev('span').addClass('hidden');
                $(this).removeClass('hidden');
            }
        });
    }

    //格式化日期
    function FormatDate(s) {
        var tmp = s.split('-');
        if (tmp.length > 1) {
            if (tmp[0].length == 4) {
                return tmp[1] + '/' + tmp[2] + '/' + tmp[0];
            }
            return tmp[0] + '/' + tmp[1] + '/' + tmp[2];
        }
        return s;
    }

    //对数字进行2位小数的Round，同时使用千分号格式化
    function commafy(num) {
        num = Math.round(num * 100) / 100;
        num = num + '';
        var tmpArr = num.split('.');
        var re = /(-?\d+)(\d{3})/
        while (re.test(tmpArr[0])) {
            tmpArr[0] = tmpArr[0].replace(re, "$1,$2")
        }
        return tmpArr.length >= 2 ? tmpArr[0] + '.' + tmpArr[1] : tmpArr[0];
    }

    ///验证购买数量不能为零
    function CheckRequestIsZero() {
        var result = true;
        $('#ca_pr_details td.RequestQuantity').each(function () {
            if ($(this).children("input").val() == 0) {
                $(this).removeClass("label");
                $(this).css('border', '1px solid red');
                result = false;
            }
            else {
                $(this).css('border', '1px solid #cccccc');
                $(this).addClass("label");
            }
        });
        return result;
    }
</script>

<script type="text/javascript">
    function CheckReturnPRNo() {
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_ffPRNumber_ctl00_ctl00_TextField
        var PRNumber = $('#<%= this.ffPRNumber.ClientID %>_ctl00_ctl00_TextField').val();
        if (ca.util.emptyString(PRNumber)) {
            alert('Please fill the return PR Number first.');
        } else {
            $('#<%= this.btnCheckReturnNo.ClientID %>').click();
        }
    }

    //设置当FormType改变时，页面显示状态
    function SelectFormType(sender, isclear) {
        var is_show_upload = false;
        var is_show_hopurpose = false;
        var is_show_storepurpose = false;
        switch ($(sender).find('input:checked').eq(0).val()) {
            case 'Store': //StoreRequest
                is_show_storepurpose = true;
                break;
            case 'HO': //HORequest
                is_show_hopurpose = true;
                is_show_upload = true;
                break;
//            case 'PaperBag': //PaperBagRequest by xu 
//                is_show_upload = true; by xu 
//                break; by xu 
            default:
                break;
        }

        //设置上传样式
        /*if (is_show_upload) {
            $('#upload-row').show();
        } else {
            $('#upload-row').hide();
        }*/

        //设置PR单目的样式
        if (is_show_hopurpose) {
            $('#pr_ho_purpose').show();
        } else {
            $('#pr_ho_purpose').hide();
        }

        //设置PR单目的样式
        if (is_show_storepurpose) {
            $('#pr_store_purpose').show();
        } else {
            $('#pr_store_purpose').hide();
        }

        if (isclear) {
            $('#<%= this.hidIsShowCache.ClientID %>').val('N');
            $('#<%= this.btnChangeType.ClientID %>').click(); //清空已选ITEM
        }
    }

    function AutoCorrectData(num) {
        var n = ca.util.trim(num);
        if (n.length === 0) {
            return n;//空
        }
        return n * 1 + '';
    }

    /*
    *string:原始字符串
    *substr:子字符串
    *isIgnoreCase:忽略大小写
    */
    function contains(string,substr,isIgnoreCase)
    {
        if(isIgnoreCase)
        {
         string=string.toLowerCase();
         substr=substr.toLowerCase();
        }
         var startChar=substr.substring(0,1);
         var strLen=substr.length;
             for(var j=0;j<string.length-strLen+1;j++)
             {
                 if(string.charAt(j)==startChar)//如果匹配起始字符,开始查找
                 {
                       if(string.substring(j,j+strLen)==substr)//如果从j开始的字符与str匹配，那ok
                       {
                             return true;
                       }  
                 }
             }
             return false;
         }

         function CheckSameItemCode() {
             var result = false;
             var arrList = new Array();
             var i = 0;
             $(".dr-row").each(function () {
                 var itemCode_td = $(this).children("td").eq(2);
                 var costCenter_td = $(this).children("td[class='label ddl CostCenter']");

                 var itemCode = itemCode_td.children("span").text();
                 var CostCenter = costCenter_td.children("select").children("option:selected").text();
                 var item = itemCode + "_" + CostCenter;
                 if (ArrayExist(arrList, item)) {///数据中是否存在相同的itemCode和costcenter
                     result = true;
                     return result;
                 }
                 else {
                     arrList[i] = item; 
                     i++;
                 }
             });
             return result;
         }

         //数据中是否存在相同的itemCode和costcenter
         function ArrayExist(arr, item) {
             var result = false;
             if (arr.length == 0) {
                 return result;
             }
             for (var i = 0; i < arr.length; i++) {
                 if (arr[i] == item) {
                     var arrItem = item.split("_")
                     alert(arrItem[1] + "  has already selected  " + arrItem[0]);
                     result = true;
                 }
             }
             return result;
         }

         ///验证是纸袋是的话如果有数量超过50会计算总量，提示用户是否保存。并验证包装规则数量是否合法。
         function CheckPaperBageData(count) {
             var result = true;
             //var largeCount = false;
             var isCheckCount = true; //是否要验证数字
             var reg = /^\d+/;
             var errorFormate = "";
             var countMessage = "";
             $(".dr-row").each(function () {
                 var packagedRegulation = $(this).children("td").eq(3).children("input[id*='HFPackagedRegulation']").val(); //.children(".HFPackagedRegulation");
                 if (packagedRegulation.length > 1) {//是纸袋数据
                     var itemCode = $(this).children("td").eq(2).text();
                     var itemCount = $(this).children("td").eq(4).children("input").val();
                     var packagedQuantity = packagedRegulation.match(reg); ///取得包装规则里的单位里的数量如：100只/箱  里的100
                     var packagedUnit = packagedRegulation.split("/")[0].replace(reg, ""); //取得包装规则里单位里如：100只/箱  里的只
                     if (null == packagedQuantity || packagedRegulation.indexOf("/") <= 2) {//纸袋包装规则 格式不正确
                         errorFormate += "Error packagedRegulation formate for " + itemCode + "\n";
                         isCheckCount = false;
                     }
                     else {
                         if (itemCount > count) {
                             var totalQuantity = packagedQuantity * itemCount;
                             countMessage += totalQuantity + " " + packagedUnit + " " + itemCode + "\n";
                            // largeCount = true;
                         }
                     }
                 }
             });

             if ((errorFormate + countMessage) != "") {
                 if (isCheckCount) {
                     if (confirm("Are you sure want to submit：\n " + countMessage)) {
                         result = true;
                     }
                     else {
                         result = false;
                     }
                 }
                 else {
                     alert(errorFormate);
                     result = false;
                 }
             }
             return result;
         }

         //纸袋订单必须是OPEX， CAPEX按钮需要变灰色
         function RequestPBAction() {
             $('#<%= rbPRStorePurpose.ClientID%>').change(function () {
                 var capexRadio = $('#<%= rbRequestType.ClientID%>').find("input[value='Capex']");
                 var OpexRadio = $('#<%= rbRequestType.ClientID%>').find("input[value='Opex']");
                 if ($(this).find("input:checked").val() == "PaperBag") {
                     capexRadio.attr("disabled", true);
                     OpexRadio.click();
                 }
                 else {
                     capexRadio.removeAttr("disabled");
                 }
             });
         }

         ///验证数据中是否用ItemType为空的数据。
         function IsEmptyItemType() {
             var isEmptItemType = false;
             $("#ca_pr_details td.ItemType").each(function () {
                 var spanItemtype = jQuery.trim($(this).children("span").text()); //.replace(/(^\s*)|(\s*$)/g, ""); //.Trim(); //.replace(/(^\s*)|(\s*$)/g, "");
                 var inputItemtype = jQuery.trim($(this).children("input").val()); //.replace(/(^\s*)|(\s*$)/g, ""); //.Trim()(); //.replace(/\s+/g, ""); //.replace(/(^\s*)|(\s*$)/g, "");
                 if (spanItemtype == "" || inputItemtype == "") {
                     SetBorderWarn($(this));
                     isEmptItemType = true;
                 }
                 else {
                     ClearBorderWarn($(this));
                     //$(this).css("border-right","
                     $(this).css('border', '1px solid #cccccc');
                 }
             });
             if (isEmptItemType) {
                 alert("ItemType can not be  empty!Please check data");
             }
             return isEmptItemType
         }
</script>
