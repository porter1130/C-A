<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.PaymentRequest.DataView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<style type="text/css">
    .CurrencyTR
    {
      display:none;  
      font-weight:bold;
      text-align:center; 
     }
    .FA
    {
      display:none;   
     }
     .label1
  {
       border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
         border-top: 1px solid #CCCCCC;
         padding:5px;
  }
    *
    {
        line-height: 14px;
    }
    .wrapdiv
    {
        padding: 2px;
        border: 1px solid red;
    }
    .out_table input[type='radio']
    {
        border: 0 none red;
        float: left;
        width: auto;
        margin-left: 5px;
        padding: 0;
    }
    
    .out_table label
    {
        border: 0 none red;
        float: left;
        width: auto;
        margin-top: 2px;
    }
    
    .ClearBoth
    {
        clear: both;
    }
    
    .h20
    {
        height: 20px;
    }
    
    .h25
    {
        height: 25px;
    }
    
    .h30
    {
        height: 30px;
    }
    
    .h40
    {
        height: 40px;
    }
    
    .h50
    {
        height: 50px;
    }
    
    .col1
    {
        width: 160px;
    }
    .col2
    {
        width: 160px;
    }
    .col3
    {
        width: 160px;
    }
    .col4
    {
        width: 160px;
    }
    
    .colspan3
    {
        width: 480px;
    }
    
    .right_border
    {
        border: 0 none;
        border-right-style: solid;
        border-right-width: 1px;
    }
    .txtPaymentReason
    {
    }
    #table_pendingform
    {
     margin-top:20px;
    }
    #table_pendingform td
    {
        padding:2px;  
    }
</style>
<div class="ContentDiv">
    <table class="ca-workflow-form-table form-table3 out_table" id="out_table">
        <tr>
            <td class="value align-center" colspan="4">
                Payment Request付款申请单
            </td>
        </tr>
        <tr>
            <td class="label">
                WorkflowNumber<br />
                工作流ID
            </td>
            <td colspan="3" class="label align-center">
                <QFL:FormField ID="ffWorkflowNumber" runat="server" FieldName="SubPRNo" ControlMode="Display">
                </QFL:FormField>
            </td>
        </tr>
        <tr>
            <td class="label col1">
                部门<br />
                Dept
            </td>
            <td class="label col2">
                <asp:TextBox ID="txtDept" runat="server" ReadOnly="True"></asp:TextBox>
            </td>
            <td class="label col3">
                申请人<br />
                Request by
            </td>
            <td class="label col4">
                <asp:TextBox ID="txtApplicant" runat="server" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="label col1">
                供应商编号<br />
                Vendor Code
            </td>
            <td class="label col2">
                <asp:TextBox ID="txtVenderCode" runat="server" ReadOnly="True"></asp:TextBox>
            </td>
            <td class="label col3">
                供应商名称<br />
                Vendor Name
            </td>
            <td class="label col4">
                <asp:TextBox ID="txtVenderName" runat="server" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="4">
                <div class="ContentPadding">
                    若是一次性的供应商，请填写以下银行信息<br />
                    If new one-off vendor,please enter the following bank infomation</div>
            </td>
        </tr>



                <tr>
            <td class="label">
                供应商所在城市<br />
                Vendor City
            </td>
            <td class="label">
                <asp:TextBox ID="txtVendorCity" runat="server" CssClass="txtVendorCity" 
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td class="label">
                供应商所在国家<br />
                Vendor Country
            </td>
            <td class="label">
                <asp:TextBox ID="txtVendorCountry" runat="server" CssClass="txtVendorCountry" 
                    ToolTip="只能英文字母组成" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
     <tr>
            <td class="label">
                银行名称<br />
                Bank Name
            </td>
            <td class="label">
                <asp:TextBox ID="txtBankName" runat="server" CssClass="txtBankName" 
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td class="label">
                银行账号<br />
                Bank A/C
            </td>
            <td class="label">
                <asp:TextBox ID="txtBankAC" runat="server" CssClass="txtBankAC" 
                    ToolTip="银行账号只能[0-9]数字组成" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="label" style="border-bottom:2px solid #9dabb6">
                银行所在城市国家<br />
                Bank Country
            </td>
            <td class="label" style="border-bottom:2px solid #9dabb6">
                <asp:TextBox ID="txtBankCity" runat="server" CssClass="txtBankCity" 
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td class="label" style="border-bottom:2px solid #9dabb6">
                银行代码<br />
                Bank Key
            </td>
            <td class="label" style="border-bottom:2px solid #9dabb6">
                <asp:TextBox ID="txtSwiftCode" runat="server" CssClass="txtSwiftCode" 
                    ReadOnly="True"></asp:TextBox>
            </td>
        </tr>






        <tr>
            <td class="label">
                单据描述<br />
                Payment<br />
                Descriptions
            </td>
            <td class="label colspan3" colspan="3">
                <asp:TextBox ID="txtPaymentDesc" runat="server" TextMode="MultiLine" Width="490px"
                    Height="36px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr style=" display:none">
            <td class="label col1">
                成本中心<br />
                Cost Center
            </td>
            <td class="label" colspan="3">
                <asp:TextBox ID="txtCostCenter" runat="server" CssClass="txtCostCenter"
                    Width="490px"></asp:TextBox>
            </td>
        </tr>
        <tr class="CurrencyTR">
            <td class="label" colspan="2" style=" border-bottom:none">
                Currency
            </td>
            <td class="label" colspan="2" style=" border-bottom:none">
                <asp:Label ID="lblCurrency" runat="server" Text="" CssClass="lblCurrency"></asp:Label>
                <asp:Label ID="lblExchangeRate" runat="server" Text="" CssClass="lblExchangeRate"
                    Visible="false"></asp:Label>
            </td>
        </tr>
        
        <tr>
            <td class="label" colspan="4" style="padding: 0px;text-align:center;">
                <table class="ca-workflow-form-table ca-workflow-form-table1" id="CostCenterTable"
                    style="width: 100%; border: none">
                    <tr style="font-weight: bold; text-align: center">
                        <td class="label1 w5">
                            No
                        </td>
                        <td class="label1 w15 FA">
                            Asset No
                        </td>
                        <td class="label1 w25">
                            <asp:Label ID="lblExpenseType" runat="server" Text=""></asp:Label>
                        </td>
                        <td class="label1 w15">
                            <asp:Label ID="lblTDStatus" runat="server" Text=""></asp:Label>
                            <asp:HiddenField ID="FAStatus" runat="server" Value="0" />
                            <asp:HiddenField ID="hfFAList" runat="server" Value="" />
                        </td>
                        <td class="label1 w25">
                            Cost Center
                        </td>
                        <td class="label1 w15">
                            Amount
                        </td>
                    </tr>
                    <asp:Repeater ID="rptItem" runat="server" OnItemDataBound="rptItem_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td class="label1">
                                    <%# Container.ItemIndex + 1 %>
                                </td>
                                <td class="label1 FA">
                                    <asp:TextBox ID="txtFANO" runat="server" CssClass="FANO" Text='<%# Eval("FANO")%>'
                                        ToolTip='<%# Eval("ID")%>'></asp:TextBox>
                                </td>
                                <td class="label1 ExpenseType">
                                    <asp:Label ID="lblExpenseType" runat="server" />
                                </td>
                                <td class="label1">
                                    <%# Eval("GLAccount")%>
                                </td>
                                <td class="label1">
                                    <%# Eval("CostCenter")%>
                                </td>
                                <td class="label1">
                                    <%# Eval("ItemInstallmentAmount").ToString() == "" ? Eval("ItemAmount") : Eval("ItemInstallmentAmount")%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="height: 15px;">
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>

        <tr>
            <td class="label col1">
                分期付款<br />
                Installment Payment
            </td>
            <td class="label" colspan="3">
                <div class="RadioWidth">
                    <asp:RadioButtonList ID="radioInstallment" runat="server" RepeatDirection="Horizontal"
                        CssClass="ms-RadioText radioInstallment" Enabled="False">
                        <asp:ListItem Selected="True">Yes</asp:ListItem>
                        <asp:ListItem Value="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </td>
        </tr>
        <tr class="trNoInstallment" style="height: 50px;">
            <td class="label col1" style="padding: 0 0 0 5px; margin: 0;">
                总金额<br />
                TotalAmount
            </td>
            <td class="label" colspan="3" style="padding: 0 0 0 5px; margin: 0;">
                <asp:TextBox ID="txtTotalAmount1" CssClass="txtTotalAmount1" runat="server" Width="490px"></asp:TextBox>
            </td>
        </tr>
        <tr class="h50 trInstallment" style="display: none;">
            <td class="h50 label col1" style="padding: 0 0 0 5px; margin: 0;">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 100%">
                    <tr>
                        <td style="width: 50%">
                            总金额<br />
                            TotalAmount
                        </td>
                        <td style="width: 50%">
                            <asp:TextBox ID="txtTotalAmount" CssClass="txtTotalAmount" runat="server" Width="55px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="label" style="padding: 0 0 0 5px; margin: 0;">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 100%">
                    <tr>
                        <td style="width: 50%">
                            己付<br />
                            Paid Before
                        </td>
                        <td style="width: 50%">
                            <asp:TextBox ID="txtPaidBefore" runat="server" CssClass="txtPaidBefore" ReadOnly="True"
                                Width="55px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="label" style="padding: 0 0 0 5px; margin: 0;">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 100%">
                    <tr>
                        <td style="width: 50%">
                            本次付<br />
                            Paid this time
                        </td>
                        <td style="width: 50%">
                            <asp:TextBox ID="txtPaidThisTime" runat="server" CssClass="txtPaidThisTime" Width="55px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="label" style="padding: 0 0 0 5px; margin: 0;">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 100%">
                    <tr>
                        <td class="h36" style="width: 50%">
                            余额<br />
                            Balance
                        </td>
                        <td style="width: 50%">
                            <asp:TextBox ID="txtBlance" runat="server" CssClass="txtBlance" ReadOnly="True" Width="55px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="h50">
            <td class="label col1">
                己签署有付款金额的合同<br />
                Contract/PO with paid amount signed</td>
            <td class="label col2">
                <asp:RadioButtonList ID="radioContractPO" runat="server" RepeatDirection="Horizontal"
                    CssClass="ms-RadioText radioContractPO" Enabled="False">
                    <asp:ListItem Selected="True"> Yes</asp:ListItem>
                    <asp:ListItem> No</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="label" colspan="2">
                <div class="divContractPO" style="vertical-align: middle;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="30%">
                                <div id="divLabContractPO" style="float: left;">
                                    合同编号<br />
                                    Contract/PO No
                                </div>
                            </td>
                            <td width="70%">
                                <div id="divTxtContractPO">
                                    <asp:TextBox ID="txtContractPO" CssClass="txtContractPO" runat="server" Width="220px"
                                        ReadOnly="True"></asp:TextBox></div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr class="h50">
            <td class="label col1">
                己做系统订单<br />
                System PO has been done
            </td>
            <td class="label col2">
                <asp:RadioButtonList ID="radioSystemPO" runat="server" RepeatDirection="Horizontal"
                    CssClass="ms-RadioText radioSystemPO" Enabled="False">
                    <asp:ListItem Selected="True"> Yes</asp:ListItem>
                    <asp:ListItem> No</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="label" colspan="2">
                <div class="divSystemPO" style="vertical-align: middle;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="30%">
                                <div id="divLabSystemPO" style="float: left;">
                                    系统订单编号<br />
                                    System PO No</div>
                            </td>
                            <td width="70%">
                                <div id="divTxtSystemPO">
                                    <asp:TextBox ID="txtSystemPO" CssClass="txtSystemPO" runat="server" Width="220px"
                                        ReadOnly="True"></asp:TextBox></div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr class="h50">
            <td class="label col1" rowspan="2">
                已收合同描述的商品/服务<br />
                Goods/Services received as agreed
            </td>
            <td class="label col2" style="border-bottom-style: none;">
                <asp:RadioButtonList ID="radioNeedGR" runat="server" RepeatDirection="Horizontal"
                    CssClass="ms-RadioText radioContractGR" Enabled="False">
                    <asp:ListItem Selected="True"> Yes </asp:ListItem>
                    <asp:ListItem> No</asp:ListItem>
                </asp:RadioButtonList>
                如没有，为什么要求付款：<br />
                If Not,why request to make payment:
                <br />
            </td>
            <td class="label" colspan="2">
                <div class="divSystemGR" style="vertical-align: middle;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="30%">
                                <div id="div1" style="float: left;">
                                    己做系统收货<br />
                                    System GR 
                                    done
                                </div>
                            </td>
                            <td width="70%">
                                <div id="div2">
                                    <asp:RadioButtonList ID="radioSystemGR" runat="server" CssClass="ms-RadioText radioSystemGR"
                                        RepeatDirection="Horizontal" Enabled="False">
                                        <asp:ListItem Selected="True"> Yes</asp:ListItem>
                                        <asp:ListItem> No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="3">
                <asp:TextBox ID="txtPaymentReason" CssClass="txtPaymentReason" runat="server" TextMode="MultiLine"
                    Width="490px" Height="36px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr class="h50">
            <td class="label col1">
                附原发票<br />
                Original Invoice Attached
            </td>
            <td class="label" colspan="3">
                <asp:RadioButtonList ID="radioInvoice" runat="server" RepeatDirection="Horizontal"
                    CssClass="ms-RadioText radioInvoicew" Enabled="False">
                    <asp:ListItem Selected="True"> Yes</asp:ListItem>
                    <asp:ListItem> No</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="label col1">
                备注Remark<br />
            </td>
            <td class="label" colspan="3">
                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Height="36px" Width="490px"
                    ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="label col1">
                附件 Attachment
            </td>
            <td class="label" colspan="3">
                <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Display">
                </QFL:FormAttachments>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="4">
            </td>
        </tr>
    </table>

    <table id="table_pendingform" class="ca-workflow-form-table ca-workflow-form-table1 hidden" style=" position:relative; ">
    <tr ><td colspan="4" style=" position:relative"><div style=" position:absolute; left:0px; top:0px; width:679px; height:448px; z-index:10000; background-color:White;filter: Alpha(opacity=0.5); display:none" id="displaydiv"></div></td></tr>
    <tr>
        <td colspan="4" class="value align-center" style=" text-align:center">
            <h3>
                Your claim is pending/rejected due to :</h3>
        </td>
    </tr>
    <tr class="tr_fapiao">
        <td  style="width:150px">
            Invoice
        </td>
        <td colspan="3" style="text-align: left;" class="td_fapiao">
            <asp:RadioButtonList ID="rblFapiao" runat="server" CssClass="radiobuttonlist radio"
                RepeatDirection="Vertical">
                <asp:ListItem Text="Invoice not attached" Value="Invoice not attached" />
                <asp:ListItem Text="Invoice amount not match/insufficient" Value="Invoice amount not match/insufficient" />
                <asp:ListItem Text="other reasons, please state" Value="other reasons, please state"  />
            </asp:RadioButtonList>
            <asp:TextBox ID="txtFapiaoOtherReason" runat="server" CssClass="hidden"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="4" style="height: 5px;">
        </td>
    </tr>
    <tr class="tr_information">
        <td>
            Information
        </td>
        <td colspan="3" style="text-align: left;" class="td_information">
            <asp:RadioButtonList ID="rblInformation" runat="server" CssClass="radiobuttonlist radio"
                RepeatDirection="Vertical">
                <asp:ListItem Text="fill-in incorrect cost center" Value="fill-in incorrect cost center" />
                <asp:ListItem Text="fill-in wrong expense type" Value="fill-in wrong expense type" />
                <asp:ListItem Text="print-out form has no amount listed" Value="print-out form has no amount listed" />
                <asp:ListItem Text="other reasons, please state" Value="other reasons, please state" />
            </asp:RadioButtonList>
            <asp:TextBox ID="txtInformationOtherReason" runat="server" CssClass="hidden"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="4" style="height: 5px;">
        </td>
    </tr>
    <tr class="tr_claimedamt">
        <td>
            Claimed Amount
        </td>
        <td colspan="3" style="text-align: left;" class="td_claimedamt">
            <asp:RadioButtonList ID="rblClaimedAmt" runat="server" CssClass="radiobuttonlist radio"
                RepeatDirection="Vertical">
                <asp:ListItem Text="used incorrect exchange rate" Value="used incorrect exchange rate"></asp:ListItem>
                <asp:ListItem Text="other reasons, please state" Value="other reasons, please state" />
            </asp:RadioButtonList>
            <asp:TextBox ID="txtClaimedOtherReason" runat="server" CssClass="hidden"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="4" style="height: 5px;">
        </td>
    </tr>
    <tr class="tr_otherreasons">
        <td>
            Other reasons, please state
        </td>
        <td colspan="3" style="text-align: left;" class="td_otherreasons">
            <asp:TextBox ID="txtOtherReasons" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="label" colspan="4" style="height: 5px;">
        </td>
    </tr>
    <tr>
        <td colspan="4">
            Please contact Sara Wang at ext. 6514 to resolve the above problem(s) before Finance
            can continue to process your payment request form.
        </td>
    </tr>
    
</table>
</div>
<script type="text/javascript" language="javascript">

    if ($.trim($(".radioInstallment input[type=radio]:checked").val()) == "Yes") {
        $(".trNoInstallment").css("display", "none");
        $(".trInstallment").css("display", "");
    }
    else {
        $(".trNoInstallment").css("display", "");
        $(".trInstallment").css("display", "none");
    }

</script>
<script type="text/javascript">
    function GetStep() {
        var step = '<%=this.Step %>';
        return step;
    }
    var radio_fapiao = '';
    var radio_information = '';
    var radio_claimAmt = '';
    function ShowPendForm() {
        var step = '<%=this.Step %>';
        if (step == "ConfirmTask" || step == "Display") {
            $("#commettable").hide();
            $("#table_pendingform").show();
            if (step == "Display") {
                $("#displaydiv").show();
                SetReasonStatus();
            }
        }
        if (step == "ConfirmTask") {
            var pending = '<%=this.Pending %>';
            if (pending != "") {
                var $pending = $("input[value='Pending']");
                $pending.attr("disabled", "disabled");
            }
        }
    }

    function RadioButtonListBind() {
        $('#<%=this.rblFapiao.ClientID %>').bind('click', function () {
            var value = $(this).find(':radio[checked=true]').val();
            if (value != radio_fapiao) {
                switch (value) {
                    case "other reasons, please state":
                        $('#<%=this.txtFapiaoOtherReason.ClientID %>').show();
                        break;
                    default:
                        $('#<%=this.txtFapiaoOtherReason.ClientID %>').hide();
                        break;
                }
                radio_fapiao = value;
            } else {
                ClearRadioButtonListSelection($(this));
                radio_fapiao = '';
                $('#<%=this.txtFapiaoOtherReason.ClientID %>').hide();
            }
        });
        $('#<%=this.rblInformation.ClientID %>').bind('click', function () {
            var value = $(this).find(':radio[checked=true]').val();
            if (value != radio_information) {
                switch (value) {
                    case "other reasons, please state":
                        $('#<%=this.txtInformationOtherReason.ClientID %>').show();
                        break;
                    default:
                        $('#<%=this.txtInformationOtherReason.ClientID %>').hide();
                        break;
                }
                radio_information = value;
            } else {
                ClearRadioButtonListSelection($(this));
                radio_information = '';
                $('#<%=this.txtInformationOtherReason.ClientID %>').hide();
            }
        });
        $('#<%=this.rblClaimedAmt.ClientID %>').bind('click', function () {
            var value = $(this).find(':radio[checked=true]').val();
            if (value != radio_claimAmt) {
                switch (value) {
                    case "other reasons, please state":
                        $('#<%=this.txtClaimedOtherReason.ClientID %>').show();
                        break;
                    default:
                        $('#<%=this.txtClaimedOtherReason.ClientID %>').hide();
                        break;
                }
                radio_claimAmt = value;
            } else {
                ClearRadioButtonListSelection($(this));
                radio_claimAmt = '';
                $('#<%=this.txtClaimedOtherReason.ClientID %>').hide();
            }
        });
    }
    function ClearRadioButtonListSelection($obj) {
        var options = $obj.find('input:radio');
        options.removeAttr('checked');
    }
    function ShowReason() {
        var $txtFapiaoOtherReason = $('#<%= this.txtFapiaoOtherReason.ClientID %>');
        var $txtInformationOtherReason = $('#<%= this.txtInformationOtherReason.ClientID %>');
        var $txtClaimedOtherReason = $('#<%= this.txtClaimedOtherReason.ClientID %>');
        if ($txtFapiaoOtherReason.val() != "") {
            $txtFapiaoOtherReason.removeClass("hidden");
        }
        if ($txtInformationOtherReason.val() != "") {
            $txtInformationOtherReason.removeClass("hidden");
        }
        if ($txtClaimedOtherReason.val() != "") {
            $txtClaimedOtherReason.removeClass("hidden");
        }
    }
    $(function () {
        DisplayOrHideFATD1();
        RadioButtonListBind();
        ShowPendForm();
        ShowReason();
    });
    function DisplayOrHideFATD() {
        var $FAStatus = $('#<%=this.FAStatus.ClientID %>');
        if ($FAStatus.val() == "1") {
            $("#CostCenterTable td.FA").show();
            $("#CostCenterTable input.FANO").each(function () {
                $(this).css("border", "none");
                $(this).css("color", "#000000");
                $(this).css("backgroundColor", "transparent");
                $(this).css("textAlign", "center");
                $(this).attr("readonly", "readonly");
            });
        }
        if ($FAStatus.val() == "2") {
            $("#CostCenterTable td.FA").show();
            var $ExpenseType = $("#CostCenterTable td.ExpenseType");
            $ExpenseType.each(function () {
                if ($(this).html().indexOf("Tax")!=-1) {
                    $(this).prev().find("input").hide();
                }
            });
            
        }
    }
    function DisplayOrHideFATD1() {
        var $FAStatus = $('#<%=this.FAStatus.ClientID %>');
        var $FANO = $("input.FANO");
        var result = false;
        //alert($FAStatus.val());
        if ($FAStatus.val() == "ApproveTask_Capex_FANO" || $FAStatus.val() == "DisplayStep_Capex_FANO") {
            result = true;
            $FANO.each(function () {
                $(this).css("border", "none");
                $(this).css("color", "#000000");
                $(this).css("backgroundColor", "transparent");
                $(this).css("textAlign", "center");
                $(this).attr("readonly", "readonly");
            });
        }
        if ($FAStatus.val() == "ConfirmTask_Capex_FANO") {
            $FANO.each(function () {
                result = true;
                if ($(this).val() != "") {
                    $(this).css("border", "none");
                    $(this).css("color", "#000000");
                    $(this).css("backgroundColor", "transparent");
                    $(this).css("textAlign", "center");
                    $(this).attr("readonly", "readonly");
                } else {
                    var et = $(this).parent().next().html();
                    if (et.indexOf("Tax") != -1) {
                        $(this).hide();
                    }
                }
            });
        }
        if (result) {
            $("#CostCenterTable td.FA").show();
        }

        var $CurrencyTR = $("tr.CurrencyTR");
        var $lblCurrency = $("span.lblCurrency");
        if ($lblCurrency.html() != "") {
            $CurrencyTR.show();
        }

    }
    function SetHfFAList() {
        var falist = "";
        var result = true;
        var msg = "";
        var $hfFAList = $('#<%=this.hfFAList.ClientID %>');
        $hfFAList.val("");
        var $FAStatus = $('#<%=this.FAStatus.ClientID %>');
        if ($FAStatus.val() == "ConfirmTask_Capex_FANO") {
            $("#CostCenterTable input.FANO").each(function () {
                var val = $(this).val();
                var et = $(this).parent().next().html();
                if (val == "" && et.indexOf("Tax") == -1) {
                    if (!$(this).parent().parent().hasClass("wrapdiv")) {
                        $(this).parent().addClass("wrapdiv");
                    }
                    result = false;
                    msg += "Please fill the FANO.\n";
                } else {
                    var title = $(this).attr("title");
                    falist += title + "," + val + ";";
                }
            });
        }
        $hfFAList.val(falist);
        if (msg != "") {
            alert(msg);
        }
        return result;
    }
    function SetReasonStatus() {
        var $td_fapiao_input = $("td.td_fapiao input:radio");
        var $td_information_input = $("td.td_information input:radio");
        var $td_claimedamt_input = $("td.td_claimedamt input:radio");
        var $td_otherreasons_input = $("td.td_otherreasons input:text");
        var td_fapiao_input_count = 0;
        var td_information_input_count = 0;
        var td_claimedamt_input_count = 0;
        $td_fapiao_input.each(function () {
            if ($(this).attr("checked")) {
                td_fapiao_input_count += 1;
            }
        });
        $td_information_input.each(function () {
            if ($(this).attr("checked")) {
                td_information_input_count += 1;
            }
        });
        $td_claimedamt_input.each(function () {
            if ($(this).attr("checked")) {
                td_claimedamt_input_count += 1;
            }
        });
        if (td_fapiao_input_count == 0) {
            $("tr.tr_fapiao").hide();
        }
        if (td_information_input_count == 0) {
            $("tr.tr_information").hide();
            $("tr.tr_information").next().hide();
        }
        if (td_claimedamt_input_count == 0) {
            $("tr.tr_claimedamt").hide();
            $("tr.tr_claimedamt").next().hide();
        }
        if ($td_otherreasons_input.val() == "") {
            $("tr.tr_otherreasons").hide();
            $("tr.tr_otherreasons").next().hide();
        }
        if (td_fapiao_input_count == 0 && td_information_input_count == 0
            && td_claimedamt_input_count == 0 && $td_otherreasons_input.val() == "") {
            $("#table_pendingform").hide();
        }
    }

    function CheckPending() {
        var result = true;
        var $td_fapiao_input = $("td.td_fapiao input:radio");
        var $td_information_input = $("td.td_information input:radio");
        var $td_claimedamt_input = $("td.td_claimedamt input:radio");
        var $td_otherreasons_input = $("td.td_otherreasons input:text");
        var td_fapiao_input_count = 0;
        var td_information_input_count = 0;
        var td_claimedamt_input_count = 0;
        $td_fapiao_input.each(function () {
            if ($(this).attr("checked")) {
                td_fapiao_input_count += 1;
            }
        });
        $td_information_input.each(function () {
            if ($(this).attr("checked")) {
                td_information_input_count += 1;
            }
        });
        $td_claimedamt_input.each(function () {
            if ($(this).attr("checked")) {
                td_claimedamt_input_count += 1;
            }
        });

        if ($td_fapiao_input.eq(2).attr("checked")) {
            $txtFapiaoOtherReason = $("input[id$='txtFapiaoOtherReason']");
            if ($txtFapiaoOtherReason.val() == "") {
                alert("Please fill in or select the invoice pending/reject reasons .");
                result = false;
             }
        }
        if ($td_information_input.eq(3).attr("checked")) {
            $txtInformationOtherReason = $("input[id$='txtInformationOtherReason']");
            if ($txtInformationOtherReason.val() == "") {
                alert("Please fill in or select the information pending/reject reasons .");
                result = false;
            }
        }
        if ($td_claimedamt_input.eq(1).attr("checked")) {
            $txtClaimedOtherReason = $("input[id$='txtClaimedOtherReason']");
            if ($txtClaimedOtherReason.val() == "") {
                alert("Please fill in or select the claimed amount pending/reject reasons .");
                result = false;
            }
        }
      
        if (td_fapiao_input_count == 0 && td_information_input_count == 0
            && td_claimedamt_input_count == 0 && $td_otherreasons_input.val() == "") {
            alert("Please fill in or select pending/reject reasons .");
            result = false;
        }
        return result;
    }
</script>
