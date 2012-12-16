<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.CreditCardClaim.DataView" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .ca-workflow-form-table td
    {
       line-height:15px;
    }
    .td_claim input
    {
      border:none;    
    }
     #tbCreditCardClaim tr.tr_details tr.tr_title td
    {
        background-color: #F0EFF5;
    }
    #tbCreditCardClaim tr.tr_details td.td_costcenter select
    {
        width: 250px;
    }
    #tbCreditCardClaim tr.tr_details td.td_expensetype select
    {
        width: 150px;
    }
    #tbCreditCardClaim tr.tr_details td.td_purpose input
    {
        width: 400px;
    }
    
     .summarytypetable
    {
        margin-bottom: 20px;
        display:none;
    }
     .summarytypetable1
    {
        margin-bottom: 20px;
    }
    .summarytypetable td
    {
        padding: 5px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        text-align: center;
        margin: 0;
        height: 25px;
    }
    .summarytypetable1 td
    {
         height: 30px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        text-align: center;
        margin: 0;
    }
    .rbtAttachInvoice td
    {
        border:none
    }
    #part1 td
    {
        border:none
    }
    .AttachInvoice td 
    {
     border:none;    
    }
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4" class="value align-center">
            <h3>
                Company Credit Card Claim Form<br />
                公司信用卡报销清单
            </h3>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Month
            <br />
            月份
        </td>
        <td class="label" colspan="3">
        <asp:Label ID="lblMonth" runat="server" Text=""></asp:Label>
         <%--   <asp:DropDownList ID="dplMonth" runat="server" Width="100px" Enabled="false">
            </asp:DropDownList>--%>
        </td>
        <%--<td class="label align-center">
            文档编号<br />
            Doc No.
        </td>
        <td class="value align-center" colspan="2">
            <asp:TextBox ID="txtNo" runat="server" ReadOnly="true"></asp:TextBox>
        </td>--%>
    </tr>
    <tr>
        <td class="label align-center">
            WorkflowNumber<br />
            工作流ID
        </td>
        <td colspan="3" class="label align-center">
            <qfl:formfield id="ffWorkflowNumber" runat="server" fieldname="WorkflowNumber" controlmode="Display"></qfl:formfield>
        </td>
    </tr>
    <tr>
        <%--<td class="label align-center w10">
            公司<br />
            Company
        </td>
        <td class="label align-center w20">
            <asp:TextBox ID="txtCompany" runat="server" ReadOnly="true"></asp:TextBox>
        </td>--%>
        <td class="label align-center w20">
            Dept<br />
            部门
        </td>
        <td class="label align-center w30">
            <asp:Label ID="txtDepartment" runat="server" Text=""></asp:Label>
        </td>
        <td class="label align-center w20">
            Requested By<br />
            申请人
        </td>
        <td class="label align-center w30">
            <asp:Label ID="txtRequestedBy" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="label align-center">
            Claims Description<br />
            报销描述
        </td>
        <td colspan="3" class="label">
            <QFL:FormField ID="ffExpenseDescription" runat="server" FieldName="ExpenseDescription"
                ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
</table>
<br />
<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="inner-table ca-workflow-form-table" id="tbCreditCardClaim" style="width: 900px; background-color:White">
            <tr style="height: 25px;">
                <td class="value align-center" colspan="9">
                    <h3>
                        Transaction Details
                    </h3>
                </td>
            </tr>
            <tr>
                 <td class="label align-center td_transdate">
                    Transaction Date<br />
                    交易日
                </td>
                <td class="label align-center td_transdesc">
                    Transaction Description<br />
                    交易描述
                </td>
                <td class="label align-center td_cardno">
                    Last 4 digits of Card<br />
                    卡号后四位
                </td>
                <td class="label align-center td_merchantname">
                    Name of Entity/City<br />
                    商户名称/城市
                </td>
                <td class="label align-center td_transamt">
                    Amount/Currency<br />
                    交易金额/币种
                </td>
                <td class="label align-center td_payamt">
                    <table class="inner-table" >
                        <tr>
                            <td colspan="2" class="value align-center">
                                Paid Amount/Currency<br />
                                记帐金额/币种
                            </td>
                        </tr>
                        <tr>
                            <td class="align-center w50">
                                存入
                            </td>
                            <td class="align-center w50">
                                支出
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="label align-center td_personal">
                    Personal Expenses<br />
                    个人消费
                </td>
                <td class="label align-center td_travelrequest">
                    Related to Travel Request<br />
                    差旅申请相关的消费
                </td>
                <td class="label align-center td_rmbamt">
                    Others<br />
                    其它消费
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <asp:Repeater ID="rptTradeInfo" runat="server" OnItemDataBound="rptItem_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td class="label td_transdate">
                                    <asp:Label ID="lblTransDate" runat="server" Text='<%#Eval("TransDate")%>'></asp:Label>
                                </td>
                                <td class="label td_transdesc">
                                    <asp:Label ID="lblTransDesc" runat="server" Text='<%#Eval("TransDesc")%>'></asp:Label>
                                </td>
                                <td class="label td_cardno">
                                    <asp:Label ID="lblCardNo" runat="server" Text='<%#Eval("CardNo")%>'></asp:Label>
                                </td>
                                <td class="label align-center td_merchantname"  style="word-break:break-all; width:165px">
                                    <asp:Label ID="lblMerchantName" runat="server" Text='<%#Eval("MerchantName")%>'></asp:Label>
                                </td>
                                <td class="label align-center td_transamt">
                                    <asp:Label ID="lblTransAmt" runat="server" Text='<%#Eval("TransAmt")%>'></asp:Label>
                                </td>
                                <td class="label align-center td_paidamt">
                                    <table class="inner-table" >
                                        <tr>
                                            <td class="align-center w50 td_depositamt" title="<%#Eval("DepositAmt") %>" >
                                                <asp:Label ID="lblDepositAmt" runat="server" Text='<%#Eval("DepositAmt")%>'></asp:Label>
                                            </td>
                                            <td class="align-center w50 td_payamt" title="<%#Eval("PayAmt") %>" >
                                                <asp:Label ID="lblPayAmt" runat="server" Text='<%#Eval("PayAmt")%>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="label align-center cb td_claim td_personal">
                                    <asp:CheckBox ID="cbPersonal" runat="server" CssClass="claimcheck1" />
                                </td>
                                <td class="label align-center cb td_claim td_travelrequest">
                                    <asp:CheckBox ID="cbTravelRequest" runat="server" CssClass="claimcheck1" />
                                </td>
                                <td class="value align-center td_claim">
                                    <asp:CheckBox ID="cbClaim" runat="server" CssClass="claimcheck" />
                                </td>
                            </tr>
                           <tr class="tr_details">
                                <td class="td_details" colspan="9">
                                   <table class="inner-table">
                                        <tr class="tr_title">
                                            <td class="label align-center td_purpose" colspan="5">
                                                Expense Purpose<br />
                                                费用用途
                                            </td>
                                            <td class="label align-center td_expensetype" colspan="2">
                                                Expense Type<br />
                                                费用种类
                                            </td>
                                            <td class="value align-center td_costcenter" colspan="2">
                                                Cost Center<br />
                                                成本中心
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label align-center td_purpose" colspan="5">
                                                <%#Eval("ExpensePurpose")%>
                                            </td>
                                            <td class="label align-center td_expensetype" colspan="2">
                                                <%#Eval("ExpenseType")%>
                                            </td>
                                            <td class="value align-center td_costcenter" colspan="2">
                                                <%#Eval("CostCenter")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<br />
<table class="ca-workflow-form-table summarytypetable">
    <tr>
        <td colspan="4">
            <h3>
                Expense Summary<br />
            </h3>
        </td>
    </tr>
    <tr>
        <td style="width: 300px">
            ExpenseType<br />
            费用类别
        </td>
        <td style="width: 100px">
            Deal Amount<br />
            交易金额(RMB)
        </td>
        <td style="width: 100px">
            Deposit Amount<br />
            存入金额(RMB)
        </td>
        <td style="width: 100px">
            Pay Amount<br />
            支出金额(RMB)
        </td>
    </tr>
    <tr class="summarytype">
        <td colspan="4" style="border-bottom: none; display: none">
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table summarytypetable1">
    <tr>
        <td>
            Total Amount<br />
            总金额(RMB)
        </td>
        <td class="preTotalAmount" colspan="3">
            <QFL:FormField ID="FormField1" runat="server" FieldName="PreTotalAmount" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Approve Amount<br />
            审批金额(RMB)
        </td>
        <td class="preTotalAmount" colspan="3">
            <QFL:FormField ID="FormField2" runat="server" FieldName="ApproveAmount" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td style="width: 280px">
            Original Invoice Attached<br />
            附原发票
        </td>
        <td class="td_claim" style="width: 300px">
            <asp:RadioButtonList ID="rbtAttachInvoice" runat="server" RepeatDirection="Horizontal"
                Enabled="false" CssClass="AttachInvoice">
                <asp:ListItem Text="Yes" Value="1" />
                <asp:ListItem Text="No" Value="0" Selected="True" />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            Remark<br />
            备注
        </td>
        <td>
            <QFL:FormField ID="ffRemark" runat="server" FieldName="Remark" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Attachment<br />
            附件
        </td>
        <td>
            <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Display">
            </QFL:FormAttachments>
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
            Please contact Amy Zhu at ext.6186 to resolve the above problem(s)
            before Finance can continue to process your claim form.
        </td>
    </tr>
    
</table>
<div id="hidFields">
    <asp:HiddenField ID="hidExcelData" runat="server" />

    <asp:HiddenField ID="hidSummaryExpenseType" runat="server"  Value="" />
</div>
<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    var radio_fapiao = '';
    var radio_information = '';
    var radio_claimAmt = '';

    $(function () {
        //DrawSummaryExpenseTable();
        var $cbClaim = $("span.claimcheck input");
        $cbClaim.each(function () {
            $(this).attr("disabled", "disabled");
            var $trClaim = $(this).parent().parent().parent().next();
            if ($(this).is(":checked")) {
                $trClaim.show();
            } else {
                if ($(this).parent().parent().parent().find("td.td_travelrequest input").attr("checked")
                        || $(this).parent().parent().parent().find("td.td_personal input").attr("checked")) {
                    $trClaim.hide();
                } else {
                    $trClaim.hide();
                    $(this).parent().parent().parent().hide();
                }
            }
        });
        var $cbClaim1 = $("span.claimcheck1 input");
        $cbClaim1.each(function () {
            $(this).attr("disabled", "disabled");
        });


        var step = '<%=this.Step %>';
        if (step == "ConfirmTask" || step == "ConfirmTask1") {
            $("#table_pendingform").show();
            $("#commettable").hide();
            if (step == "ConfirmTask1") {
                $("#displaydiv").show();
                SetReasonStatus();
            }
        }
        var pending = '<%=this.Pending %>';
        if (pending != "") {
            var $pending = $("input[value='Pending']");
            //$pending.hide();
            $pending.attr("disabled", "disabled");
        }
        //ChangeReasonStatus();
        //        var $rblFapiao = $('#<%= this.rblFapiao.ClientID %>');
        //        var $rblInformation = $('#<%= this.rblInformation.ClientID %>');
        //        var $rblClaimedAmt = $('#<%= this.rblClaimedAmt.ClientID %>');

        //        $rblFapiao.click(function () {
        //            var $rblFapiao1 = $('#<%= this.rblFapiao.ClientID %>');
        //            var $txtFapiaoOtherReason = $('#<%= this.txtFapiaoOtherReason.ClientID %>');


        //            if ($rblFapiao1.find(':radio[checked=true]').val() == "other reasons, please state") {
        //                $txtFapiaoOtherReason.removeClass("hidden");
        //            } else {
        //                $txtFapiaoOtherReason.val("");
        //                $txtFapiaoOtherReason.addClass("hidden");
        //            }
        //        });
        //        $rblInformation.click(function () {
        //            var $rblInformation1 = $('#<%= this.rblInformation.ClientID %>');
        //            var $txtInformationOtherReason = $('#<%= this.txtInformationOtherReason.ClientID %>');
        //            if ($rblInformation1.find(':radio[checked=true]').val() == "other reasons, please state") {
        //                $txtInformationOtherReason.removeClass("hidden");
        //            } else {
        //                $txtInformationOtherReason.val("");
        //                $txtInformationOtherReason.addClass("hidden");
        //            }
        //        });
        //        $rblClaimedAmt.click(function () {
        //            var $rblClaimedAmt1 = $('#<%= this.rblClaimedAmt.ClientID %>');
        //            var $txtClaimedOtherReason = $('#<%= this.txtClaimedOtherReason.ClientID %>');
        //            if ($rblClaimedAmt1.find(':radio[checked=true]').val() == "other reasons, please state") {
        //                $txtClaimedOtherReason.removeClass("hidden");
        //            } else {
        //                $txtClaimedOtherReason.val("");
        //                $txtClaimedOtherReason.addClass("hidden");
        //            }
        //        });
        RadioButtonListBind();
        ShowReason();

        HideDepositamt();
    });

    function HideDepositamt() {
        var $td_depositamt = $("td.td_depositamt");
        $td_depositamt.each(function () {
            if ($(this).attr("title") == "0/USD" || $(this).attr("title") == "0/RMB") {
                $(this).html("");
            }
        });
        var $td_payamt = $("td.td_payamt");
        $td_payamt.each(function () {
            if ($(this).attr("title") == "0/USD" || $(this).attr("title") == "0/RMB") {
                $(this).html("");
            }
        });
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
    function GetStep() {
        var step = '<%=this.Step %>';
        return step;
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
                alert("Please fill in or select the invoice pending reasons .");
                result = false;
            }
        }
        if ($td_information_input.eq(3).attr("checked")) {
            $txtInformationOtherReason = $("input[id$='txtInformationOtherReason']");
            if ($txtInformationOtherReason.val() == "") {
                alert("Please fill in or select the information pending reasons .");
                result = false;
            }
        }
        if ($td_claimedamt_input.eq(1).attr("checked")) {
            $txtClaimedOtherReason = $("input[id$='txtClaimedOtherReason']");
            if ($txtClaimedOtherReason.val() == "") {
                alert("Please fill in or select the claimed amount pending reasons .");
                result = false;
            }
        }
        if (td_fapiao_input_count == 0 && td_information_input_count == 0
            && td_claimedamt_input_count == 0 && $td_otherreasons_input.val() == "") {
            alert("Please fill in or select pending reasons .");
            result = false;
        }
        return result;
    }

    function ChangeReasonStatus() {
        var $td_fapiao_input = $("td.td_fapiao input:radio");
        var $td_information_input = $("td.td_information input:radio");
        var $td_claimedamt_input = $("td.td_claimedamt input:radio");
        var checkedStatus_td_fapiao_input = new Array();
        var checkedStatus_td_information_input = new Array();
        var checkedStatus_td_claimedamt_input = new Array();
        $td_fapiao_input.each(function (i) {
            checkedStatus_td_fapiao_input[i] = $(this).attr("checked");
            $($td_fapiao_input[i]).click(function () {
                if (!checkedStatus_td_fapiao_input[i]) {
                    $($td_fapiao_input[i]).attr("checked", "checked");
                    checkedStatus_td_fapiao_input[i] = true;
                } else {
                    $($td_fapiao_input[i]).removeAttr("checked");
                    checkedStatus_td_fapiao_input[i] = false;
                }
            });
        });
        $td_information_input.each(function (i) {
            checkedStatus_td_information_input[i] = $(this).attr("checked");
            $($td_information_input[i]).click(function () {
                if (!checkedStatus_td_information_input[i]) {
                    $($td_information_input[i]).attr("checked", "checked");
                    checkedStatus_td_information_input[i] = true;
                } else {
                    $($td_information_input[i]).removeAttr("checked");
                    checkedStatus_td_information_input[i] = false;
                }
            });
        });
        $td_claimedamt_input.each(function (i) {
            checkedStatus_td_claimedamt_input[i] = $(this).attr("checked");
            $($td_claimedamt_input[i]).click(function () {
                if (!checkedStatus_td_claimedamt_input[i]) {
                    $($td_claimedamt_input[i]).attr("checked", "checked");
                    checkedStatus_td_claimedamt_input[i] = true;
                } else {
                    $($td_claimedamt_input[i]).removeAttr("checked");
                    checkedStatus_td_claimedamt_input[i] = false;
                }
            });
        });
    }
</script>
<script type="text/javascript">
//    function AppendHtml(expenseType, amount, costcenter, depositamt, payamt) {
//        var appendHtml = "";
//        appendHtml = "<tr class=\"item\" ><td >" + expenseType + "</td><td title=\"" + costcenter + "\" >" + amount + "</td><td >" + depositamt + "</td><td >" + payamt + "</td></tr>";
//        return appendHtml;
//    }
    function DrawSummaryExpenseTable() {
//        $(".summarytypetable tr").remove(".item");
//        var $summarytype = $("table.summarytypetable tr.summarytype");
//        var $hidSummaryExpenseType = $('#<%= this.hidSummaryExpenseType.ClientID %>');
//        var summaryExpenseType = $hidSummaryExpenseType.val();
//        if (summaryExpenseType != "") {
//            var summaryExpense = eval("(" + summaryExpenseType + ")");
//            try {
//                $.each(summaryExpense, function (i, item) {
//                    var $html = $(AppendHtml(item.name, item.val, item.costcenter, item.depositamt, item.payamt));
//                    $summarytype.before($html);
//                });
//            } catch (e) { }
//        }
    }
</script>