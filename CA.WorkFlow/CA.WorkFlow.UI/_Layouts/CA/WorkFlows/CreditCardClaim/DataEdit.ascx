<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.CreditCardClaim.DataEdit"   %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .ca-workflow-form-table
    {
        margin-top: 15px;
    }
    .ca-workflow-form-table td
    {
        line-height: 15px;
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
    .cb input
    {
        border: none;
    }
    .td_claim input
    {
        border: none;
    }
    .wrapdiv
    {
        padding: 2px;
        border: 1px solid red;
    }
    
    .summarytypetable
    {
        margin-bottom: 20px;
        display: none;
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
        border: none;
    }
    #part1 td
    {
        border: none;
    }
    .CreditCardBillID
    {
        display: none;
    }
</style>
<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
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
                    Choose CardHolder<br />
                    选择持卡人
                </td>
                <td class="pftd value" colspan="3">
                    <asp:DropDownList ID="ddlEmployeeList" runat="server" Width="200px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="label align-center">
                    Month
                    <br />
                    月份
                </td>
                <td class="value" colspan="3">
                    <asp:DropDownList ID="ddlMonth" runat="server" Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr >
                <td class="label align-center w20">
                    Dept<br />
                    部门
                </td>
                <td class="label align-center w30">
                    <asp:Label ID="lblDepartment" runat="server" ReadOnly="true"></asp:Label>
                </td>
                <td class="label align-center w20">
                    Requested By<br />
                    申请人
                </td>
                <td class="label value align-center w30">
                    <asp:Label ID="lblRequestedBy" runat="server" ReadOnly="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="label align-center">
                    Claims Description<br />
                    报销描述
                </td>
                <td colspan="3" class="label ExpenseDescription">
                    <QFL:FormField ID="ffExpenseDescription" runat="server" FieldName="ExpenseDescription"
                        ControlMode="Edit" CssClass="ffExpenseDescription">
                    </QFL:FormField>
                </td>
            </tr>
        </table>
        <table class="ca-workflow-form-table" id="tbCreditCardClaim" style="width: 900px;
            background-color: White">
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
                <td class="label align-center td_claim">
                    Others<br />
                    其它消费
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <asp:Repeater ID="rptTradeInfo" runat="server">
                        <ItemTemplate>
                            <tr class="tr_index">
                                <td class="label align-center td_transdate">
                                    <asp:Label ID="lblTransDate" runat="server" Text='<%#Eval("TransDate")%>'></asp:Label>
                                </td>
                                <td class="label align-center td_transdesc">
                                    <asp:Label ID="lblTransDesc" runat="server" Text='<%#Eval("TransDesc")%>'></asp:Label>
                                </td>
                                <td class="label align-center td_cardno">
                                    <asp:Label ID="lblCardNo" runat="server" Text='<%#Eval("Title").ToString().Substring(Eval("Title").ToString().Length-4,4)%>'></asp:Label>
                                </td>
                                <td class="label align-center td_merchantname" style="word-break:break-all; width:165px">
                                    <asp:Label ID="lblMerchantName" runat="server" Text='<%#Eval("MerchantName")%>'></asp:Label>
                                </td>
                                <td class="label align-center td_transamt" >
                                    <asp:Label ID="lblTransAmt" runat="server" Text='<%#string.Format("{0}/{1}",Eval("TransAmt"),Eval("Currency"))%>'></asp:Label>
                                </td>
                                <td class="label align-center">
                                    <table class="inner-table" >
                                        <tr>
                                            <td class="align-center w50 td_depositamt" title="<%#Eval("DepositAmt") %>" >
                                                <asp:Label ID="lblDepositAmt" runat="server" Text='<%#string.Format("{0}/{1}",Eval("DepositAmt"),Eval("rCurrency"))%>'></asp:Label>
                                            </td>
                                            <td class="align-center w50 td_payamt" title="<%#Eval("PayAmt") %>" >
                                                <asp:Label ID="lblPayAmt" runat="server" Text='<%#string.Format("{0}/{1}",Eval("PayAmt"),Eval("rCurrency"))%>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                 <td class="label align-center cb td_personal">
                                   <asp:CheckBox ID="cbPersonal" runat="server" CssClass="personalcheck" Checked='<%#Eval("IsPersonal")=="1"?true:false %>' />
                                </td>
                                <td class="label align-center cb td_travelrequest">
                                   <asp:CheckBox ID="cbTravelRequest"  runat="server" CssClass="travelrequestcheck" Checked='<%#Eval("IsTravelRequest")=="1"?true:false %>'  />
                                </td>
                               <td class="value align-center cb td_claim">
                                    <asp:CheckBox ID="cbClaim" runat="server" CssClass="claimcheck" Checked='<%#Eval("IsClaim")=="1"?true:false %>' />
                                </td>
                            </tr>
                            <tr class="tr_details hidden">
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
                                                <asp:TextBox ID="txtExpensePurpose" runat="server" Text='<%#Eval("ExpensePurpose") %>'></asp:TextBox>
                                                <div class="CreditCardBillID"><%#Eval("ID") %></div>
                                            </td>
                                            <td class="label align-center td_expensetype" colspan="2">
                                                <asp:DropDownList ID="ddlExpenseType" runat="server" SelectedValue='<%#Eval("ExpenseType") %>'  onchange="SetExpenseSummary(this)">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Travel - hotel" Value="Travel - hotel"></asp:ListItem>
                                                    <asp:ListItem Text="Travel - transportation" Value="Travel - transportation"></asp:ListItem>
                                                    <asp:ListItem Text="Travel - meal" Value="Travel - meal"></asp:ListItem>
                                                    <asp:ListItem Text="Entertainment - food" Value="Entertainment - food"></asp:ListItem>
                                                    <asp:ListItem Text="Entertainment - gift" Value="Entertainment - gift"></asp:ListItem>
                                                    <asp:ListItem Text="Sample purchase" Value="Sample purchase"></asp:ListItem>
                                                    <asp:ListItem Text="Others (specify)" Value="Others (specify)"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="value align-center td_costcenter" colspan="2" title='<%#Eval("CostCenter") %>'>
                                                <asp:DropDownList ID="ddlCostCenter" runat="server" onchange="SetExpenseSummary(this)" >
                                                </asp:DropDownList>
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
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnBillInfoBind" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
<table class="ca-workflow-form-table summarytypetable" >
    <tr>
        <td colspan="5">
            <h3>
                Expense Summary<br />
            </h3>
        </td>
    </tr>
    <tr>
        <td style="width: 200px">
            ExpenseType<br />
            费用类别
        </td>
        <td style="width: 100px">
            Cist Center<br />
            成本中心
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
        <td colspan="5" style="border-bottom: none; display: none">
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
            <QFL:FormField ID="ffPreTotalAmount" runat="server" FieldName="PreTotalAmount">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Approve Amount<br />
            审批金额(RMB)
        </td>
        <td class="preTotalAmount" colspan="3">
            <QFL:FormField ID="ffApproveAmount" runat="server" FieldName="ApproveAmount">
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
                CssClass="rbtAttachInvoice">
                <asp:ListItem Text="Yes" Value="1" Selected="True" />
                <asp:ListItem Text="No" Value="0" />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            Remark<br />
            备注
        </td>
        <td>
            <QFL:FormField ID="ffRemark" runat="server" FieldName="Remark">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Attachment<br />
            附件
        </td>
        <td>
            <QFL:FormAttachments runat="server" ID="attacthment">
            </QFL:FormAttachments>
        </td>
    </tr>
</table>
<div id="hidFields">
    <asp:Button ID="btnBillInfoBind" runat="server" OnClick="btnBillInfoBind_Click" CssClass="hidden" />
    <asp:HiddenField ID="hidExcelData" runat="server" />
    <asp:HiddenField ID="hidCreditCardBillInfo" runat="server" />
    <asp:HiddenField ID="hidCostCenterInfo" runat="server" />
    <asp:HiddenField ID="hidTravelRequest" runat="server"  Value="0"/>

    <asp:HiddenField ID="hidRMBSummaryExpenseType" runat="server"  Value="" />
    <asp:HiddenField ID="hidUSDSummaryExpenseType" runat="server"  Value="" />
</div>
<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
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
</script>
<script type="text/javascript">
    //server control clientid
    var hidCreditCardBillInfoClientId = '#<%=this.hidCreditCardBillInfo.ClientID %>'
    var ddlMonthClientId = '#<%=this.ddlMonth.ClientID %>';
    var ddlEmployeeList = '#<%=this.ddlEmployeeList.ClientID %>';
    var btnBillInfoBindClientId = '#<%=this.btnBillInfoBind.ClientID %>';
    var hidCostCenterInfoClientId = '#<%=this.hidCostCenterInfo.ClientID %>';
    var ffPreTotalAmountClientId = '#<%=this.ffPreTotalAmount.ClientID %>' + '_ctl00_ctl00_TextField';

    var ffApproveAmountClientId = '#<%=this.ffApproveAmount.ClientID %>' + '_ctl00_ctl00_TextField';
    var ffExpenseDescription = '#<%=this.ffExpenseDescription.ClientID %>' + '_ctl00_ctl00_TextField';


    function CheckClaim() {
        CreateForbidDIV();
        $(".wrapdiv").removeClass("wrapdiv");
        $("#tbCreditCardClaim tr.tr_index").find("td input").css("border", "none");
        var $cbClaim = $("span.claimcheck input:checked");
        var $travelrequestcheck = $("span.travelrequestcheck input:checked");
        var $personalcheck = $("span.personalcheck input:checked");
        var $hidTravelRequest = $('#<%=this.hidTravelRequest.ClientID %>');
        var errorMsg = "";
        var result = true;
        var $tr_index = $("#tbCreditCardClaim tr.tr_index");
        var checkResult = false;
        $tr_index.each(function () {
            var $check = $(this).find("td input:checked");
            if ($check.length == 0) {
                var $uncheck = $(this).find("td input:not(:checked)");
                $uncheck.each(function () {
                    $(this).css("border", "1px solid red");
                });
                checkResult = true;
            }
        });
        if (($cbClaim.length == 0 && $travelrequestcheck.length == 0 && $personalcheck.length == 0) || checkResult) {
            errorMsg += "You have unclaimed items\nPlease make sure they are related to trips applied through Travel Request.\nIf not,please claim them here.\n";
            result = false;
        }
        
        $cbClaim.each(function () {
            var $td_purpose = $(this).parent().parent().parent().next().find("td.td_purpose input");
            $td_purpose.each(function () {
                if ($(this).val() == "") {
                    errorMsg += "Please fill the Info of purpose. \n";
                    //alert('Please fill the Info of purpose.');
                    if (!$(this).parent().hasClass("wrapdiv")) {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                    result = false;
                }
            });
            var $td_expensetype = $(this).parent().parent().parent().next().find("td.td_expensetype select");
            $td_expensetype.each(function () {
                if ($(this).val() == "") {
                    errorMsg += "Please select expensetype type. \n";
                    //alert('Please select expensetype type.');
                    if (!$(this).parent().hasClass("wrapdiv")) {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                    result = false;
                }
            });
            var $td_costcenter = $(this).parent().parent().parent().next().find("td.td_costcenter select");
            $td_costcenter.each(function () {
                if ($(this).val() == "-1") {
                    errorMsg += "Please select costcenter type. \n";
                    //alert('Please select costcenter type.');
                    if (!$(this).parent().hasClass("wrapdiv")) {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                    result = false;
                }
            });
        });

        if ($(ffPreTotalAmountClientId).val() == "0" && $cbClaim.length == 0
                && $travelrequestcheck.length == 0 && $personalcheck.length==0) {
            if (!$(ffPreTotalAmountClientId).parent().hasClass("wrapdiv")) {
                $(ffPreTotalAmountClientId).wrap("<span class=\"wrapdiv\"></span>");
            }
            result = false;
        }

        if ($(ffExpenseDescription).val() == "") {
            errorMsg += "Please input Claims Description. \n";
            if (!$(ffExpenseDescription).parent().hasClass("wrapdiv")) {
                $(ffExpenseDescription).wrap("<span class=\"wrapdiv\"></span>");
            }
            result = false;
        };

        

        if ($cbClaim.length == 0 && $(ffPreTotalAmountClientId).val() == "0") {
            $hidTravelRequest.val("1");
            //alert($hidTravelRequest.val());
        }
        //return false;
        //        var $rbtAttachInvoice = $(".rbtAttachInvoice input:radio:checked");
        //        if ($rbtAttachInvoice.val() == "1") {
        //            var $del = $("td.ms-propertysheet");
        //            if ($del.length == 0) {
        //                alert("Please upload attachment! \n");
        //                result = false;
        //             }
        //        }
        if (errorMsg != "") {
            alert(errorMsg);
        } else {
            AddSummaryExpenseType();
        }
        if (!result) {
            ClearForbidDIV();
        }
        //return false;
        return result;
    }

    $(function () {
        //alert("For expenses related to trips applied through Travel Request.\nplease like the claim through Travel Expense claim.");
        BindEvent();
        //FormatCurrency();
        GetTotalAmount();

        SetControlMode();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        HideDepositamt();
        DisabledPreTotalAmount();
        ChangeCheckedStatus();
        //DrawSummaryExpenseTable();
    });

    function EndRequestHandler() {
        BindEvent();
        GetTotalAmount();
        SetControlMode();
        HideDepositamt();
        DisabledPreTotalAmount();
        ChangeCheckedStatus();
    }

    function DisabledPreTotalAmount() {
        var $preTotalAmount = $("td.preTotalAmount input");
        $preTotalAmount.focus(function () {
            $(this).blur();
        });
    }


    function HideDepositamt() {
        var $td_depositamt = $("td.td_depositamt");
        $td_depositamt.each(function () {
            if ($(this).attr("title") == "0") {
                $(this).html("");
            }
        });
        var $td_payamt = $("td.td_payamt");
        $td_payamt.each(function () {
            if ($(this).attr("title") == "0") {
                $(this).html("");
            }
        }); 
    }

</script>
<script type="text/javascript">
    function BindEvent() {

        $(ddlMonthClientId).change(function () {
            $(btnBillInfoBindClientId).click();
        });

        $(ddlEmployeeList).change(function () {
            $(btnBillInfoBindClientId).click();
        });

//        $('#tbCreditCardClaim td.td_claim :checkbox').live('click', function () {
//            $(this).parents('tr').first().next().toggle();

//            if ($(this).attr('checked')) {
//                //CostCenterDataBind($(this).parents('tr').first().next().find('td.td_costcenter select'));
//            }
//            GetTotalAmount();

//            
//        });

    }

    function SetControlMode() {
        $('#tbCreditCardClaim td.td_costcenter select').each(function () {
            var title = $(this).parent().attr('title');
            if (!ca.util.emptyString(title)) {
                $(this).val(title);
            }
        });
    }

    function CostCenterDataBind($obj) {

        var hidCostCenterValue = $(hidCostCenterInfoClientId).val();
        var json = eval('(' + hidCostCenterValue + ')');

        for (var i = 0; i < json.length; i++) {
            $obj.append($('<option></option>').val(json[i].Title).html(json[i].Display));
        }
    }

    function GetTotalAmount() {
        var payAmtTotal = 0;
        var depositAmtTotal = 0;
        var totalAmt = 0;

        var am = 0;

        $('#tbCreditCardClaim tr.tr_index').each(function () {

            if ($(this).find('td.td_claim :checkbox').attr('checked')) {
                $(this).next().show();
                var payAmt = FormatCurrency($(this).find('td.td_payamt').text());
                var depositAmt = FormatCurrency($(this).find('td.td_depositamt').text());

                payAmtTotal += parseFloat(payAmt);
                depositAmtTotal += parseFloat(depositAmt);
            }

        });
        totalAmt = payAmtTotal - depositAmtTotal;

        am = payAmtTotal + depositAmtTotal;

        $(ffPreTotalAmountClientId).val(totalAmt.toFixed(0));

        $(ffApproveAmountClientId).val(am.toFixed(0));
    }

    function FormatCurrency(amtStr) {

        var tmpArr = amtStr.split('/');
        var amt = tmpArr[0];
        var currency = tmpArr[1];
        var exchRate = GetExchRate($.trim(currency));
        var returnAmt = 0;

        if (!ca.util.emptyString(amt)
                && !ca.util.emptyString(exchRate)
                && !isNaN(amt)
                && !isNaN(exchRate)) {
            returnAmt = parseFloat(amt) * parseFloat(exchRate);
        }

        return returnAmt;
    }

    function IsUnique(month, array) {
        var isUnique = true;
        for (var i = 0; i < array.length; i++) {
            if (array[i] == month) {
                isUnique = false;
            }
        }
        return isUnique;
    }

    function GetExchRate(currency) {

        var exchRate = "";
        switch (currency) {
            case "RMB":
                exchRate = "1.0000";
                break;
            case "USD":
                exchRate = '<%=GetExchRate("USD") %>';
                break;
            case "GBP":
                exchRate = '<%=GetExchRate("GBP") %>';
                break;
            case "EUR":
                exchRate = '<%=GetExchRate("EUR") %>';
                break;
            case "AUD":
                exchRate = '<%=GetExchRate("AUD") %>';
                break;
            case "CHF":
                exchRate = '<%=GetExchRate("CHF") %>';
                break;
            case "CAD":
                exchRate = '<%=GetExchRate("CAD") %>';
                break;
            case "JPY":
                exchRate = '<%=GetExchRate("JPY") %>';
                break;
            case "HKD":
                exchRate = '<%=GetExchRate("HKD") %>';
                break;
            default:
                break;
        }
        return exchRate;
    }
</script>
<script type="text/javascript">
    function ChangeCheckBoxStatus(obj) {
        var $item = $(obj);
        alert($item.attr("checked"));
    }

    //$(function () {
    function ChangeCheckedStatus() {
        var $td_travelrequestitem = $("td.td_travelrequest input");
        var $td_personalitem = $("td.td_personal input");
        var $td_claimitem = $("td.td_claim input");
        $td_travelrequestitem.each(function () {
            $(this).click(function () {
                $(this).css("border", "none");
                $(this).parent().parent().parent().find("td.td_personal input").css("border", "none");
                $(this).parent().parent().parent().find("td.td_claim input").css("border", "none");
                if ($(this).attr("checked")) {
                    $(this).parent().parent().parent().find("td.td_personal input").removeAttr("checked");
                    $(this).parent().parent().parent().find("td.td_claim input").removeAttr("checked");
                    $(this).parent().parent().parent().next().hide();
                    var $cbClaim = $("span.claimcheck input:checked");
                    if ($cbClaim.length == 0) {
                        $(ffPreTotalAmountClientId).val("0");
                    }
                    GetTotalAmount();
                }
            });
        });
        $td_personalitem.each(function () {
            $(this).click(function () {
                $(this).css("border", "none");
                $(this).parent().parent().parent().find("td.td_travelrequest input").css("border", "none");
                $(this).parent().parent().parent().find("td.td_claim input").css("border", "none");
                $(this).parent().parent().parent().find("td.td_travelrequest input").removeAttr("checked");
                $(this).parent().parent().parent().find("td.td_claim input").removeAttr("checked");
                $(this).parent().parent().parent().next().hide();
                var $cbClaim = $("span.claimcheck input:checked");
                if ($cbClaim.length == 0) {
                    $(ffPreTotalAmountClientId).val("0");
                }
                GetTotalAmount();
            });
        });
        $td_claimitem.each(function () {
            $(this).click(function () {
                $(this).css("border", "none");
                $(this).parent().parent().parent().find("td.td_travelrequest input").css("border", "none");
                $(this).parent().parent().parent().find("td.td_personal input").css("border", "none");
                if ($(this).attr("checked")) {
                    $(this).parent().parent().parent().find("td.td_travelrequest input").removeAttr("checked");
                    $(this).parent().parent().parent().find("td.td_personal input").removeAttr("checked");
                    $(this).parent().parent().parent().next().removeClass("hidden");
                    GetTotalAmount();
                } else {
                    $(this).parent().parent().parent().next().hide();
                    var $cbClaim = $("span.claimcheck input:checked");
                    if ($cbClaim.length == 0) {
                        $(ffPreTotalAmountClientId).val("0");
                    }
                    GetTotalAmount();
                }
            });
        });
    }
    //});
</script>
<script type="text/javascript">
    function SetExpenseSummary(obj) {
        AddSummaryExpenseType();
    }

    function CheckSummaryTypeHtml(expenseType) {
        var result = false;
        var exp = ".summarytypetable:contains('" + expenseType + "')";
        var $summarytypehtml = $(exp);
        if ($summarytypehtml.length > 0) {
            result = true;
        }
        return result;
    }

    function UpdateSummaryExpenseTypeAmount(expenseType) {
        var exp = ".summarytypetable tr td:contains('" + expenseType + "')";
        var $summarytypehtml = $(exp);
        var $amount = $summarytypehtml.next();
        var $depositamt = $summarytypehtml.next().next();
        var $payamt = $summarytypehtml.next().next().next();

        var totalamount = CalExpenseTypeAmount(expenseType);
        var totalamountitem = totalamount.split("/");
        $amount.text(totalamountitem[0]);
        $depositamt.text(totalamountitem[1]);
        $payamt.text(totalamountitem[2]);
    }

    function CalExpenseTypeAmount(expenseType) {
        var $tr_details = $("tr.tr_details:visible");
        var amount = 0;
        var depositamt = 0;
        var payamt = 0;
        var totalamount = "";
        if ($tr_details.length > 0) {
            $tr_details.each(function () {
                var expenseTypeitem = $(this).find("td.td_expensetype select").val();
                if (expenseTypeitem == expenseType) {
                    var $amount = $(this).prev().find("td.td_transamt span");
                    var $depositamt = $(this).prev().find("td.td_depositamt span");
                    var $payamt = $(this).prev().find("td.td_payamt span");

                    var amountitem = FormatCurrency($amount.text()).toFixed(0);
                    var depositamtitem = FormatCurrency($depositamt.text()).toFixed(0);
                    var payamtitem = FormatCurrency($payamt.text()).toFixed(0);

                    amount +=  parseInt(amountitem);
                    depositamt += parseInt(depositamtitem);
                    payamt += parseInt(payamtitem);
                }
            });
        }
        totalamount = amount + "/" + depositamt + "/" + payamt;
        return totalamount;
    }

    function AppendHtml(amounttype, expenseType, amount, costcenter, depositamt, payamt, transdesc, creditCardBillID) {
        var appendHtml = "";
        appendHtml = "<tr class=\"item\" ><td title=\"" + amounttype + "\" >" + expenseType + "</td><td title=\"" + transdesc + "\" >" + costcenter + "</td><td title=\"" + creditCardBillID + "\">" + amount + "</td><td >" + depositamt + "</td><td >" + payamt + "</td></tr>";
        return appendHtml;
    }

    function AddSummaryExpenseType() {
        $(".summarytypetable tr").remove(".item");
        var $hidRMBSummaryExpenseType = $('#<%= this.hidRMBSummaryExpenseType.ClientID %>');
        var $summarytype = $("table.summarytypetable tr.summarytype");
        var $tr_details = $("tr.tr_details:visible");
        if ($tr_details.length > 0) {
            $tr_details.each(function () {
                var expenseType = $(this).find("td.td_expensetype select").val();
                if (expenseType != "") {
                    var costcenter = $(this).find("td.td_costcenter select").val();
                    var $amount = $(this).prev().find("td.td_transamt span");
                    var $depositamt = $(this).prev().find("td.td_depositamt span");
                    var $payamt = $(this).prev().find("td.td_payamt span");

                    var transdesc = $(this).prev().find("td.td_transdesc span").text();
                    //var amount = FormatCurrency($amount.text()).toFixed(0);
                    //var depositamt = FormatCurrency($depositamt.text()).toFixed(0);
                    //var payamt = FormatCurrency($payamt.text()).toFixed(0);
                    var depositamt = $depositamt.text().split("/")[0] == "" ? "0" : $depositamt.text().split("/")[0];
                    var payamt = $payamt.text().split("/")[0] == "" ? "0" : $payamt.text().split("/")[0];
                    var amount = parseFloat(payamt - depositamt);
                    amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
                    var amounttype = "";
                    if ($depositamt.text().indexOf("RMB") != -1 || $payamt.text().indexOf("RMB") != -1) {
                        amounttype = "RMB";
                    } else {
                        amounttype = "USD";
                    }

                    var creditCardBillID = $(this).find("div.CreditCardBillID").text(); 
                    //var result = CheckSummaryTypeHtml(expenseType);
                    //if (!result) {
                    var $html = $(AppendHtml(amounttype, expenseType, $amount.text() == "" ? "0" : amount, costcenter, depositamt, payamt, transdesc, creditCardBillID));
                    $summarytype.before($html);
                    //} else {
                    //UpdateSummaryExpenseTypeAmount(expenseType);
                    //}
                }
            });
        }
        UpdateSummaryExpenseType();
    }

    function UpdateSummaryExpenseType() {
        var $hidRMBSummaryExpenseType = $('#<%= this.hidRMBSummaryExpenseType.ClientID %>');
        var $hidUSDSummaryExpenseType = $('#<%= this.hidUSDSummaryExpenseType.ClientID %>');
        $hidRMBSummaryExpenseType.val("");
        $hidUSDSummaryExpenseType.val("");
        var rmbSummaryExpenseType = "[";
        var usdSummaryExpenseType = "[";
        var $summarytypetable = $(".summarytypetable tr.item");
        $summarytypetable.each(function () {
            //summaryExpenseType += "{name:'" + $(this).find("td").eq(0).text() + "',val:'" + $(this).find("td").eq(1).text() + "'},";
            if ($(this).find("td").eq(0).attr("title") == "RMB") {
                rmbSummaryExpenseType += "{name:'" + $(this).find("td").eq(0).text() + "',val:'" + $(this).find("td").eq(2).text() + "',costcenter:'" + $(this).find("td").eq(1).text() + "',depositamt:'" + $(this).find("td").eq(3).text() + "',payamt:'" + $(this).find("td").eq(4).text() + "',transdesc:'" + $(this).find("td").eq(1).attr("title") + "',creditCardBillID:'" + $(this).find("td").eq(2).attr("title") + "'},";
            } else {
                usdSummaryExpenseType += "{name:'" + $(this).find("td").eq(0).text() + "',val:'" + $(this).find("td").eq(2).text() + "',costcenter:'" + $(this).find("td").eq(1).text() + "',depositamt:'" + $(this).find("td").eq(3).text() + "',payamt:'" + $(this).find("td").eq(4).text() + "',transdesc:'" + $(this).find("td").eq(1).attr("title") + "',creditCardBillID:'" + $(this).find("td").eq(2).attr("title") + "'},";
            }

        });
        rmbSummaryExpenseType += "]";
        usdSummaryExpenseType += "]";
        $hidRMBSummaryExpenseType.val(rmbSummaryExpenseType);
        $hidUSDSummaryExpenseType.val(usdSummaryExpenseType);
    }

</script>
<script type="text/javascript">
    //    function DrawSummaryExpenseTable() {
    //        $(".summarytypetable tr").remove(".item");
    //        var $summarytype = $("table.summarytypetable tr.summarytype");
    //        var $hidRMBSummaryExpenseType = $('#<%= this.hidRMBSummaryExpenseType.ClientID %>');
    //        var summaryExpenseType = $hidRMBSummaryExpenseType.val();
    //        if (summaryExpenseType != "") {
    //            var summaryExpense = eval("(" + summaryExpenseType + ")");
    //            try {
    //                $.each(summaryExpense, function (i, item) {
    //                    var $html = $(AppendHtml(item.name, item.val, item.costcenter, item.depositamt, item.payamt));
    //                    $summarytype.before($html);
    //                });
    //            } catch (e) { }
    //        }
    //    }
</script>
