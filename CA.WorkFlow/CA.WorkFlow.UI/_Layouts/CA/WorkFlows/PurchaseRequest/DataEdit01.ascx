<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit01.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequest.DataEdit01" %>

<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
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
    
    /*****************Begin add by xu*********************************************************************/
    .TDData
    {
        border-top: 1px solid #999;
        padding:5px;
        }
        
    .ms-alternating,.each-row
    {
        cursor:pointer;
        line-height:20px;
        height:20px;
        }
   .ItemW15
   {
       width:15%;
       }
   .ItemW40
   {
       width:40%;
       }
   .YellowTR
   {
       background-color:Yellow;
       }
   .ItemSelected
   {
       background-color:Yellow;
       }
   #loading
   {
       width:20px;
       margin-top:20px;
       margin-bottom:20px;
       margin-left:auto;
       margin-right:auto;
       display:none;
       }
    #PagIndex
    {
       width:100px;
       margin-left:auto;
       margin-right:auto;
       margin-top:10px;
       margin-bottom:10px;
    }
    #PagIndex #Prev, #PagIndex #Next
    {
       cursor:pointer;
       display:none;
    }
    #TRTitle
    {
        line-height:40px;
        font-weight:bolder ;
        background-color: #f2f2f2;
    }
    .dr-row
    {
        display:none;
        }
    .img-button
    {
        cursor:pointer;
    }
    #LoadMoreItem
    {
        position:absolute;
        z-index:5;
        left:300px;
        top:600px;
        display:none;
        }
    /*****************End add by xu*********************************************************************/
</style>
<script src="PRDataEdit.js" type="text/javascript"></script>
<asp:HiddenField ID="hidDisplayMode" runat="server" />
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
            <asp:RadioButtonList ID="rbPRStorePurpose" runat="server" CssClass="radio" RepeatDirection="Horizontal">
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
            <asp:RadioButtonList ID="rbRequestType" runat="server" CssClass="radio" RepeatDirection="Horizontal">
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
                               <img src="../images/pixelicious_001.png" width="18" class="img-button"  id="btnAddItem" alt="Click to add the PR information." onclick="ShowNextTR()" />
                               <asp:Button ID="ButtonAddItem" runat="server" CssClass="hidden" Text="AddItem" onclick="ButtonAddItem_Click" />
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
                            <td class="label align-center" style="width: 55px;">
                                Unit Price
                                <br />
                                单价
                            </td>
                            <td class="label align-center" style="width: 60px;">
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
                        <asp:Repeater ID="rptItem" runat="server" OnItemDataBound="rptItem_ItemDataBound">
                                            <ItemTemplate>
                                                <tr class="dr-row">
                                                    <td class="cell align-center">
                                                            <img src="../images/pixelicious_028.png" width="18" class="img-button"  id="btnDeleteItem" alt="Remove this PR information." onclick="DeletCurrentRow(this)" />
                                                    </td>
                                                    <td class="label align-center">
                                                        <asp:Label ID="lbItem" runat="server" />
                                                    </td>
                                                    <td class="label ItemCode">
                                                        <asp:Label ID="lbItemCode"  CssClass="SelectItemCode" runat="server" />
                                                        <img width="18px" class="img-button" alt="Select" onclick="OpenDialog(this);" src="/_layouts/CAResources/themeCA/images/load.gif">
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
                                                    <td class="label align-center UnitPrice">
                                                        <asp:Label ID="lbUnitPrice" runat="server" />
                                                        <asp:TextBox ID="txtUnitPrice" runat="server" Text="0" CssClass="hidden" onchange="return UpdateTotal(this);" />
                                                    </td>
                                                    <td class="label align-center TotalPrice">
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
                                                        <asp:HiddenField ID="HiddenFieldCostCenterID" runat="server" />
                                                        <select class="width-fix" onchange="SetCostCenterVal(this)">
                                                        </select>
                                                        <asp:HiddenField ID="HiddenFieldCostCenter" runat="server" />
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
                        <asp:AsyncPostBackTrigger ControlID="ButtonAddItem" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
        </td>
    </tr>
    <tr class="bold-bottom-line" id="request_total">
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

<div id="SelectedItem" title="Itemcode Search">
    <table style="margin-left:auto;margin-right:auto" class="ca-workflow-form-table">
        <tr>
            <td class="label">
                Item Code:
            </td>
            <td class="label">
                <input id="txtItemCode" style="width:120px" class="SearchCoditon" type="text" />
            </td>
            <td class="label">
                Description:
            </td>
            <td class="label">
                <input id="txtDesc" style="width:120px" class="SearchCoditon" type="text" />
            </td>
            <td class="label">
                <div class="ca-workflow-form-buttons">
                    <input type="button" id="buttonQuery" value="Query" />
                </div>
            </td>
        </tr>
</table>
    <table style="margin-left:auto;margin-right:auto" class="ca-workflow-form-table">
           <tr id="TRTitle">
                <td class="ItemW15">Item Code</td>
                <td class="ItemW15">Item Type</td>
                <td class="ItemW40">Description</td>
                <td class="ItemW15">Unit</td>
                <td class="ItemW15">Asset Class</td>
                <td class="hidden">VendorId</td>
                <td class="hidden">Delivery Period</td>
                <td class="hidden">Unit Price</td>
                <td class="hidden">Tax Value</td>
                <td class="hidden">IsAccpetDecimal</td>
                <td class="hidden">Currency</td>
                <td class="hidden">ItemScope</td>
                <td class="hidden">PackagedRegulation</td>
           </tr>
    </table>
    <div id="loading"><img id="imgloading" src="/_layouts/CAResources/themeCA/images/loading.gif" alt="loading" /></div>
    <div id="PagIndex">
        <span id="Prev"> Prev  </span>
        <span id="Currentpage">1</span>
        <span id="Next">  Next </span>
    </div>
</div>
<div id="LoadMoreItem">
    <img src="/_layouts/CAResources/themeCA/images/loading.gif" alt="loading" />
</div>
<asp:DropDownList ID="DropDownListCostCenter" runat="server"></asp:DropDownList>
<asp:HiddenField ID="hidSelectedItem" runat="server" />
<asp:HiddenField ID="hidTotal" runat="server" />

<asp:HiddenField ID="hidIsShowCache" runat="server" Value="Y" />
    <script type="text/javascript">
        $(function () {
            $("#<%= DropDownListCostCenter.ClientID%>").hide();
            $('#<%= this.ffIsReturn.ClientID %>_ctl00_ctl00_BooleanField').click(ClickReturn);
            SelectFormType($('#<%= this.rbFormType.ClientID %>').get(0), false);
            HideEmptyTR();
            BindFuction();
            /***************************/
            ItemCodeQuery(); //ItemCode查询全用js去处理

            $('#<%= this.ffIsReturn.ClientID %>_ctl00_ctl00_BooleanField').click(ClickReturn);
            SetReturnStyle();
            SetStyle();

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
            RequestPBAction(); ///纸袋订单必须是OPEX， CAPEX按钮需要变灰色 by xu
            /***************************/
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler); //注册服务器返回事件后执行函数

            var offset = $("#btnAddItem").offset();
            $("#LoadMoreItem").css({ left: offset.left, top: offset.top });
        });

        //提交请求时进行合法性验证
        function Validate() {
            var error = '';

            var flag = 0;
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

        //根据Repeater控件中控件值进行总数的计算，同时调用千分号函数进行显示。若一行中有计算值处于空白状态，则该行略过
        function CalcTotal() {
            JSPurchaseRequest.total = 0;
            JSPurchaseRequest.total_return = 0;
            JSPurchaseRequest.total_approval = 0;
            var tmpTotal = 0;
            var is_discount = false;
            var is_return = $('#<%= this.ffIsReturn.ClientID %>_ctl00_ctl00_BooleanField').is(':checked');
            $('#ca_pr_details td.TotalPrice').each(function () {
                if (!$(this).is(":hidden")) {
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
                }
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

        //检查用户权限，不同权限用户不同显示界面
        function CheckPermission() {
            //当StoreRequest时，在Check页面，隐藏Skip Approve选项
            if ($('#<%= this.rbFormType.ClientID %> input:checked').eq(0).val() === 'Store') {
                $('#ca_pr_labal_skipapprove').hide();
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
                default:
                    break;
            }

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
            }
        }

        function SwitchStorePurpose(sender) {
            SwitchRequestType($('#<%= this.rbRequestType.ClientID %>').get(0));
        }

        function SwitchRequestType(sender) {
            var requestType = $(sender).find('input:checked').eq(0).val();
            SetRequestType(requestType);
            $('#<%= this.hidIsShowCache.ClientID %>').val('N');
            ClearAll();
            ClearItemData();
            CalcTotal();
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
       
        /************by xu*************************************/
        function SetStyle() {
            if ($('#<%= this.hidDisplayMode.ClientID %>').val() === 'Display') {
                var dynamic_style = '#tb_purchaserequest{width: 1095px;}.PermissionDisplay{display: inline;}.DescWidth{width: 175px;}.PermissionLabel{border-bottom: 1px solid #CCCCCC;padding-left:2px;}';
                addCssByStyle(dynamic_style);
            } else {
                var dynamic_style = '#tb_purchaserequest{width: 900px;}.PermissionDisplay{display: none;}.DescWidth{width: 257px;}.PermissionLabel{border-bottom: none}';
                addCssByStyle(dynamic_style);
            }
        }
        ///通过AJAX得到数据
        function GetDataFromServer(pageIndex) { // by xu
            var ItemCode = $("#txtItemCode").val();
            var ItemDesc = $("#txtDesc").val();
            var PageType = ""; //E,C
            var ItemType = ""; //QO,PB......
            var isStore = 1;   //1?
            if ($("#<%= rbRequestType.ClientID%>").find("input[CHECKED]").val() == "Opex") {
                PageType = "E";
            }
            else {
                PageType = "C";
            }

            if ($("#<%= rbFormType.ClientID%>").find("input[CHECKED]").val() == "Store") {
                isStore = 1;
            }
            else {
                isStore = 0;
            }

            if ($("#<%= rbPRStorePurpose.ClientID%>").find("input[CHECKED]").val() == "Daily") {
                ItemType = "";
            }
            else if ($("#<%= rbPRStorePurpose.ClientID%>").find("input[CHECKED]").val() == "QuarterlyOrder") {
                ItemType = "QO";
            }
            else if ($("#<%= rbPRStorePurpose.ClientID%>").find("input[CHECKED]").val() == "PaperBag") {
                ItemType = "PB";
            }
            $.ajax({
                type: "Get",
                dataType: "json",
                url: "GetItemCode.aspx",
                data: { ItemCode: ItemCode, Desc: ItemDesc, ItemType: ItemType, sItemStart: PageType, pageIndex: pageIndex, isStore: isStore },
                beforeSend: function () {
                    //清除老数据。
                    
                    $("#loading").show();
                },
                success: function (jsonList) {
                    if (!jsonList) {
                        alert("There are no avaliable item data");
                    }
                    else {
                        ReadJSON(eval(jsonList));
                    }
                },
                error: function (message) {
                    alert("Loading item failed:" + message);
                },
                complete: function () {
                    $("#loading").hide();
                    $("#Currentpage").text(pageIndex);
                }

            });
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

        //给行里的dropdownlist添加值。
        function SetDDLSelectValue(obj) {
            if (obj.children().length == 0) {
                var option = $("#<%= DropDownListCostCenter.ClientID%>").children().clone();
                obj.append(option);
            }
        }
        //默认加载的行数太少了，触发加载Item行数的事件
        function AddItem() {
            $('#<%= this.ButtonAddItem.ClientID %>').click();
            $("#LoadMoreItem").show();
        }
        function BindFuction() {
            $("#<%= rbRequestType.ClientID%>").change(function () {
                return SwitchRequestType(this);
            });

            $("#<%= rbPRStorePurpose.ClientID%>").change(function () {
                return SwitchStorePurpose(this)
            });
        }

        function DisplayMod() {
            var displayMod = $('#<%= this.hidDisplayMode.ClientID %>').val();
            return displayMod;
        }
    </script>