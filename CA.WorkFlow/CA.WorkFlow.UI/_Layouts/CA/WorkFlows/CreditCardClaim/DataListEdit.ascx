<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataListEdit.ascx.cs"
    Inherits="CA.WorkFlow.UI.CreditCardClaim.DataListEdit" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .ca-workflow-form-table td
    {
        padding: 5px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        text-align: center;
        margin: 0;
        line-height: 15px;
    }
    .ca-workflow-form-table
    {
        margin-top: 25px;
    }
    .ca-workflow-form-table input
    {
        width: 80%;
    }
    .ca-workflow-form-table select
    {
        margin-left: 0px;
        width: 100%;
    }
    .ExpenseType1
    {
        position: absolute;
        left: 0px !important;
        left: -45px;
        top: -15px;
        z-index: 1000;
    }
    .CostCenter
    {
        position: absolute;
        left: 0px !important;
        left: -62px;
        top: -15px;
    }
    .CostCenterTD
    {
        width: 260px;
    }
    .ExpenseTypeTD
    {
        width: 215px;
    }
    .wrapdiv
    {
        padding: 2px;
        border: 1px solid red;
    }
    .dealamount
    {
        color: #06c;
    }
    .RMBTotalAmount
    {
        color: #06c;
    }
    .USDTotalAmount
    {
        color: #06c;
    }
    .vendor
    {
        color: #06c;
    }
    .totalamount
    {
        color: Red;
    }
    .rmbevo
    {
        display: none;
    }
    .usdevo
    {
        display: none;
    }
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4">
            <h3>
                Credit Card Claim Form<br />
                员工报销申请表
            </h3>
        </td>
    </tr>
    <tr>
        <td class="w20">
            WorkFlowNumber<br />
            工作流ID
        </td>
        <td colspan="3" >
            <asp:Label ID="lblWorkFlowNumber" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="w20">
            Requested ID<br />
            申请人ID
        </td>
        <td class="w30" >
            <asp:Label ID="lblRequestedID" runat="server" CssClass="RequestedID"></asp:Label>
        </td>
        <td class="w20">
            Requested Name<br />
            申请人姓名
        </td>
        <td class="w30">
            <asp:Label ID="lblRequestedBy" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Claims Description<br />
            描述
        </td>
        <td colspan="3" >
            <asp:TextBox ID="txtExpenseDescription" runat="server" class="ExpenseDescription"></asp:TextBox>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table" id="RMBExpenseTypetable">
            <tr style="height: 20px">
                <td colspan="7">
                    <h3>
                        Credit Card Claim RMB details</h3>
                </td>
            </tr>
            <tr style="font-weight: bold;">
                <td class="w5">
                    <asp:ImageButton runat="server" ID="btnAddItem" ToolTip="Click to add the information."
                        ImageUrl="../images/pixelicious_001.png" OnClick="btnAddItem_Click" Width="18"
                        CssClass="img-button" />
                </td>
                <%--<td class="w15">
                    ExpenseType<br />
                    费用类别
                </td>
                <td class="w20">
                    CostCenter<br />
                    成本中心
                </td>
                <td class="w15">
                    Deal Amount<br />
                    交易金额(RMB)
                </td>
                <td class="w15">
                    Deposit Amount<br />
                    存入金额(RMB)
                </td>
                <td class="w15">
                    Pay Amount<br />
                    支出金额(RMB)
                </td>
                <td class="w15">
                    GL Account<br />
                    总帐会计
                </td>--%>
                <td class="w5">
                    Dr/Cr
                </td>
                <td class="w15">
                    ExpenseType
                </td>
                <td class="w15">
                    GL Account
                </td>
                <td class="w20">
                    CostCenter
                </td>
                <td class="w25">
                    Transaction Description
                </td>
                <td class="w15">
                    Amount(RMB)
                </td>
            </tr>
            <asp:Repeater ID="rptItem" runat="server" OnItemCommand="rptItem_ItemCommand" OnItemDataBound="rptItem_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnDeleteItem" ToolTip="Remove this information." CommandName="delete"
                                runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                        </td>
                        <td style="text-align:left">
                            Dr
                        </td>
                        <td>
                            <div style="position: relative; z-index: 1000; background-color: White;">
                                <div class="ExpenseType1">
                                    <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="ExpenseType" onchange="ChangeGLAccount(this)">
                                        <asp:ListItem Text="" Value="0" Selected="True" />
                                        <asp:ListItem Text="Travel - hotel" Value="Travel - hotel"></asp:ListItem>
                                        <asp:ListItem Text="Travel - transportation" Value="Travel - transportation"></asp:ListItem>
                                        <asp:ListItem Text="Travel - meal" Value="Travel - meal"></asp:ListItem>
                                        <asp:ListItem Text="Entertainment - food" Value="Entertainment - food"></asp:ListItem>
                                        <asp:ListItem Text="Entertainment - gift" Value="Entertainment - gift"></asp:ListItem>
                                        <asp:ListItem Text="Sample purchase" Value="Sample purchase"></asp:ListItem>
                                        <asp:ListItem Text="Others (specify)" Value="Others (specify)"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hidremarkstatus1" runat="server" Value="0" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="lblGLAccount" runat="server" CssClass="GLAccount"></asp:TextBox>
                            <%--  <asp:Label ID="lblGLAccount" runat="server" CssClass="GLAccount" />--%>
                        </td>
                        <td>
                            <div style="position: relative; background-color: White; z-index: 500">
                                <div class="CostCenter">
                                    <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="cc">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <%--<td class="dealamount">
                            <asp:Label ID="txtAmount" runat="server"></asp:Label>
                        </td>
                        <td class="DepositAmount">
                            <asp:TextBox ID="txtDepositAmount" runat="server" CssClass="Amount"></asp:TextBox>
                        </td>
                        <td class="PayAmount">
                            <asp:TextBox ID="txtPayAmount" runat="server" CssClass="Amount"></asp:TextBox>
                        </td>--%>
                        <td>
                            <asp:Label ID="lblTransactionDescription" runat="server" />
                            <asp:HiddenField ID="hfRMBCreditCardBillID" runat="server" Value="" />
                        </td>
                        <td class="Amount">
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="Amount" onchange="AppendRMBEmployeeVendor()"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="height: 20px;">
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="rmbevo">
                <td colspan="7" style="height: 20px;">
                </td>
            </tr>
            <tr class="tcarmb">
                <td>
                </td>
                <td style="padding: 10px; text-align: left" colspan="5">
                    Total claimed amount
                </td>
                <td>
                    <asp:TextBox ID="txtRMBTotalClaimedAmount" runat="server" Style="border: none; text-align: right"
                        ReadOnly="true" CssClass="RMBTotalClaimedAmount"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 20px;" colspan="7">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="padding: 10px; text-align: left" colspan="5">
                    Net balance
                </td>
                <td>
                    <asp:TextBox ID="txtRMBNetbalance" runat="server" Style="border: none; text-align: right"
                        ReadOnly="true" CssClass="RMBNetbalance">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 20px;" colspan="7">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="5" style="text-align:left">
                    Total Amount<br />
                    总金额(RMB)
                </td>
                <td style="border-right: none;text-align:right">
                    <asp:Label ID="lblRMBTotalAmount" runat="server" Text="" CssClass="RMBTotalAmount"></asp:Label>
                </td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnAddItem" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table" id="USDExpenseTypetable">
            <tr style="height: 20px">
                <td colspan="7">
                    <h3>
                        Credit Card Claim USD details</h3>
                </td>
            </tr>
            <tr style="font-weight: bold;">
                <td class="w5">
                    <asp:ImageButton runat="server" ID="btnAddUSDItem" ToolTip="Click to add the information."
                        ImageUrl="../images/pixelicious_001.png" OnClick="btnAddUSDItem_Click" Width="18"
                        CssClass="img-button" />
                </td>
                <%--<td class="w15">
                    ExpenseType<br />
                    费用类别
                </td>
                <td class="w20">
                    CostCenter<br />
                    成本中心
                </td>
                <td class="w15">
                    Deal Amount<br />
                    交易金额(USD)
                </td>
                <td class="w15">
                    Deposit Amount<br />
                    存入金额(USD)
                </td>
                <td class="w15">
                    Pay Amount<br />
                    支出金额(USD)
                </td>
                <td class="w15">
                    GL Account<br />
                    总帐会计
                </td>--%>
               <td class="w5">
                    Dr/Cr
                </td>
                <td class="w15">
                    ExpenseType
                </td>
                <td class="w15">
                    GL Account
                </td>
                <td class="w20">
                    CostCenter
                </td>
                <td class="w25">
                    Transaction Description
                </td>
                <td class="w15">
                    Amount(USD)
                </td>
            </tr>
            <asp:Repeater ID="rptUSDItem" runat="server" OnItemCommand="rptUSDItem_ItemCommand"
                OnItemDataBound="rptUSDItem_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnDeleteUSDItem" ToolTip="Remove this information." CommandName="delete"
                                runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                        </td>
                        <td style="text-align:left">
                            Dr
                        </td>
                        <td>
                            <div style="position: relative; z-index: 1000; background-color: White;">
                                <div class="ExpenseType1">
                                    <asp:DropDownList ID="ddlUSDExpenseType" runat="server" CssClass="ExpenseType" onchange="ChangeGLAccount(this)">
                                        <asp:ListItem Text="" Value="0" Selected="True" />
                                        <asp:ListItem Text="Travel - hotel" Value="Travel - hotel"></asp:ListItem>
                                        <asp:ListItem Text="Travel - transportation" Value="Travel - transportation"></asp:ListItem>
                                        <asp:ListItem Text="Travel - meal" Value="Travel - meal"></asp:ListItem>
                                        <asp:ListItem Text="Entertainment - food" Value="Entertainment - food"></asp:ListItem>
                                        <asp:ListItem Text="Entertainment - gift" Value="Entertainment - gift"></asp:ListItem>
                                        <asp:ListItem Text="Sample purchase" Value="Sample purchase"></asp:ListItem>
                                        <asp:ListItem Text="Others (specify)" Value="Others (specify)"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hidremarkstatus1" runat="server" Value="0" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="lblUSDGLAccount" runat="server" CssClass="GLAccount"></asp:TextBox>
                            <%--  <asp:Label ID="lblUSDGLAccount" runat="server" CssClass="GLAccount" />--%>
                        </td>
                        <td>
                            <div style="position: relative; background-color: White; z-index: 500">
                                <div class="CostCenter">
                                    <asp:DropDownList ID="ddlUSDCostCenter" runat="server" CssClass="cc">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <%-- <td class="dealamount">
                            <asp:Label ID="txtUSDAmount" runat="server"></asp:Label>
                        </td>
                        <td class="DepositAmount">
                            <asp:TextBox ID="txtUSDDepositAmount" runat="server" CssClass="USDAmount"></asp:TextBox>
                        </td>
                        <td class="PayAmount">
                            <asp:TextBox ID="txtUSDPayAmount" runat="server" CssClass="USDAmount"></asp:TextBox>
                        </td>--%>
                        <td>
                            <asp:Label ID="lblUSDTransactionDescription" runat="server" />
                            <asp:HiddenField ID="hfUSDCreditCardBillID" runat="server" Value="" />
                        </td>
                        <td class="USDAmount">
                            <asp:TextBox ID="txtUSDAmount" runat="server" CssClass="USDAmount" onchange="AppendUSDEmployeeVendor()"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="height: 20px;">
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="usdevo">
                <td colspan="7" style="height: 20px;">
                </td>
            </tr>
             <tr class="tcausd">
                <td>
                </td>
                <td style="padding: 10px; text-align: left" colspan="5">
                    Total claimed amount
                </td>
                <td>
                    <asp:TextBox ID="txtUSDTotalClaimedAmount" runat="server" Style="border: none; text-align: right"
                        ReadOnly="true" CssClass="USDTotalClaimedAmount"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 20px;" colspan="7">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="padding: 10px; text-align: left" colspan="5">
                    Net balance
                </td>
                <td>
                    <asp:TextBox ID="txtUSDNetbalance" runat="server" Style="border: none; text-align: right"
                        ReadOnly="true" CssClass="USDNetbalance">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 20px;" colspan="7">
                </td>
            </tr>
            <tr>
               <td>
                </td>
                <td colspan="5" style="text-align:left">
                    Total Amount<br />
                    总金额(USD)
                </td>
                <td style="border-right: none;text-align:right">
                    <asp:Label ID="lblUSDTotalAmount" runat="server" Text="" CssClass="USDTotalAmount"></asp:Label>
                </td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnAddUSDItem" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
<table class="ca-workflow-form-table" style="display: none">
    <tr>
        <td style="width: 430px">
            Total Amount<br />
            总金额(RMB)
        </td>
        <td>
            <asp:Label ID="lbTotalAmount" runat="server" CssClass="TotalAmount"></asp:Label>
            <asp:HiddenField ID="hfGLAccount" runat="server" Value="" />
            <asp:HiddenField ID="hfTableStatus" runat="server" Value="" />
            <asp:HiddenField ID="hfRMBEmployeeVendor" runat="server" Value="" />
            <asp:HiddenField ID="hfUSDEmployeeVendor" runat="server" Value="" />
         </td>
    </tr>
</table>
<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">

    $(function () {
        BindEvent();
        BindAmountBlurEvent();
        BindUSDAmountBlurEvent();
        DisplayOrHideTable();
        GetRMBCalAmount();
        GetUSDCalAmount();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });

    function AppendHtml(trclass,amount, employeeID,amounttype) {
        var appendHtml = "";
        //appendHtml = "<tr class=\"" + trclass + "\" ><td></td><td style=\"padding: 10px\">CR</td><td style=\"text-align: left\">Employee Vendor - OP</td><td>" + employeeID + "</td><td></td><td></td><td>" + amounttype + "</td><td><input type=\"text\" value=\"" + amount + "\" style=\"border:none\"  readonly=\"readonly\"/></td></tr><tr class=\"" + trclass + "\"><td style=\"height: 20px;\" colspan=\"8\"></td></tr>";
        appendHtml = "<tr class=\"" + trclass + "\" ><td></td><td style=\"padding: 10px;text-align:right\">Cr</td><td style=\"text-align: right\">OR - employee vendor</td><td>" + employeeID + "</td><td></td><td></td><td><input type=\"text\" value=\"" + amount + "\" style=\"border:none;text-align: right\"  readonly=\"readonly\"/></td></tr><tr class=\"" + trclass + "\"><td style=\"height: 20px;\" colspan=\"7\"></td></tr>";
        return appendHtml;
    }

    function AppendRMBEmployeeVendor() {
        $("#RMBExpenseTypetable tr").remove(".rmb");
        var $hfRMBEmployeeVendor = $('#<%=this.hfRMBEmployeeVendor.ClientID %>');
        var rmbEmployeeVendor = "";
        var $inputAmount = $("td input.Amount");
        var $rmbevo = $("tr.rmbevo");
        $inputAmount.each(function () {
            var amount = $(this).val();
            if (isNaN(amount) || amount > 100000000) {
                amount = 0;
            }
            amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
            var employeeID = $("span.RequestedID").text();
            var amounttype = "RMB";
            if (amount < 0) {
                amount = 0 - amount;
            } else {
                amount = 0 - amount;
            }
            rmbEmployeeVendor += amount + ";";
            var $html = $(AppendHtml("rmb", amount, employeeID, amounttype));
            $rmbevo.before($html);
        });
        $hfRMBEmployeeVendor.val(rmbEmployeeVendor);
   }

    function AppendUSDEmployeeVendor() {
        $("#USDExpenseTypetable tr").remove(".usd");
        var $hfUSDEmployeeVendor = $('#<%=this.hfUSDEmployeeVendor.ClientID %>');
        var usdEmployeeVendor ="";
        var $inputAmount = $("td input.USDAmount");
        var $usdevo = $("tr.usdevo");
        $inputAmount.each(function () {
            var amount = $(this).val();
            if (isNaN(amount) || amount > 100000000) {
                amount = 0;
            }
            amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
            var employeeID = $("span.RequestedID").text();
            var amounttype = "USD";
            if (amount < 0) {
                amount = 0 - amount;
            } else {
                amount = 0 - amount;
            }
            usdEmployeeVendor += amount + ";";
            var $html = $(AppendHtml("usd", amount, employeeID, amounttype));
            $usdevo.before($html);
        });
        $hfUSDEmployeeVendor.val(usdEmployeeVendor);
    }

    function DisplayOrHideTable() {
        var $hfTableStatus = $('#<%=this.hfTableStatus.ClientID %>'); 
        if ($hfTableStatus.val() == "RMB") {
            $("#RMBExpenseTypetable").hide();
            AppendUSDEmployeeVendor();
        }
        if ($hfTableStatus.val() == "USD") {
            $("#USDExpenseTypetable").hide();
            AppendRMBEmployeeVendor(); 
        }
        if ($hfTableStatus.val() == "") {
            AppendRMBEmployeeVendor();
            AppendUSDEmployeeVendor();
        }
    }

    function GetRMBCalAmount() {
        var result = false; //USDExpenseTypetable USDTotalAmount
        var $inputAmount = $("#RMBExpenseTypetable td input.Amount");
        var $TotalAmount = $("#RMBExpenseTypetable span.RMBTotalAmount");
        var totalAmount = 0;
        $inputAmount.each(function () {
            var inputAmount = $(this).val();
            if (inputAmount == "") {
                inputAmount = "0";
            }
            totalAmount += parseFloat(inputAmount);
        });
        totalAmount = Math.round(totalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        if (totalAmount != parseFloat($TotalAmount.text())) {
            $("span.RMBTotalAmount").addClass("totalamount");
            $("input.RMBTotalClaimedAmount").css("color", "red");
            $("input.RMBTotalClaimedAmount").val(totalAmount);
        } else {
            $("span.RMBTotalAmount").removeClass("totalamount");
            $("input.RMBTotalClaimedAmount").css("color", "#06c");
            $("input.RMBTotalClaimedAmount").val(totalAmount);
        }
    }

    function GetUSDCalAmount() {
        var result = false;
        var $inputAmount = $("#USDExpenseTypetable td input.USDAmount");
        var $TotalAmount = $("#USDExpenseTypetable span.USDTotalAmount");
        var totalAmount = 0;
        $inputAmount.each(function () {
            var inputAmount = $(this).val();
            if (inputAmount == "") {
                inputAmount = "0";
            }
            totalAmount += parseFloat(inputAmount);
        });
        totalAmount = Math.round(totalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        if (totalAmount != parseFloat($TotalAmount.text())) {
            $("span.USDTotalAmount").addClass("totalamount");
            $("input.USDTotalClaimedAmount").css("color", "red");
            $("input.USDTotalClaimedAmount").val(totalAmount);
        } else {
            $("span.USDTotalAmount").removeClass("totalamount");
            $("input.USDTotalClaimedAmount").css("color", "#06c");
            $("input.USDTotalClaimedAmount").val(totalAmount);
        }
    }

    function BindAmountBlurEvent() {
        var $inputAmount = $("td input.Amount");
        $inputAmount.each(function () {
            $(this).blur(function () {
                if (isNaN($(this).val()) || $(this).val() > 100000000) {
                    if ($(this).parent().is("span")) {
                        if (!$(this).parent().hasClass("wrapdiv")) {
                            $(this).parent().addClass("wrapdiv");
                        }
                    } else {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                    alert('Please fill the valid number.');
                    $(this).val("0");
                    //                    var $dealamount1 = $(this).parent().parent().parent().find("td.dealamount");
                    //                    var PayAmount1 = 0;
                    //                    var DepositAmount1 = 0;
                    //                    if ($(this).parent().parent().attr("class") == "DepositAmount") {
                    //                        var $pay1 = $(this).parent().parent().next().find("input.Amount");
                    //                        DepositAmount1 = parseInt(0);
                    //                        PayAmount1 = parseInt($pay1.val() == "" ? "0" : $pay1.val());
                    //                    } else {
                    //                        var $deposit1 = $(this).parent().parent().prev().find("input.Amount");
                    //                        DepositAmount1 = parseInt($deposit1.val() == "" ? "0" : $deposit1.val());
                    //                        PayAmount1 = parseInt(0);
                    //                    }
                    //                    var totalamount1 = DepositAmount1 + PayAmount1;
                    //                    $dealamount1.text(totalamount1);
                    //return;
                } else {
                    $(this).parent().removeClass("wrapdiv");
                }

                if ($(this).val() == "" || $(this).val() == "0") {
                    $(this).val("0")
                } else {
                    //四舍五入取整
                    var amount = Math.round($(this).val() * Math.pow(10, 2)) / Math.pow(10, 2);
                    $(this).val(amount);
                }

                GetRMBCalAmount();
                //return;
                //                if (!$(this).parent().is("span")) {
                //                    var $dealamount = $(this).parent().parent().find("td.dealamount");
                //                    var PayAmount = 0;
                //                    var DepositAmount = 0;
                //                    if ($(this).parent().attr("class") == "DepositAmount") {
                //                        var $pay = $(this).parent().next().find("input.Amount");
                //                        DepositAmount = parseInt($(this).val());
                //                        PayAmount = parseInt($pay.val() == "" ? "0" : $pay.val());
                //                    } else {
                //                        var $deposit = $(this).parent().prev().find("input.Amount");
                //                        DepositAmount = parseInt($deposit.val() == "" ? "0" : $deposit.val());
                //                        PayAmount = parseInt($(this).val());
                //                    }
                //                    var totalamount = DepositAmount + PayAmount;
                //                    $dealamount.text(totalamount);
                //                } else {
                //                    var $dealamount2 = $(this).parent().parent().parent().find("td.dealamount");
                //                    var PayAmount2 = 0;
                //                    var DepositAmount2 = 0;
                //                    if ($(this).parent().parent().attr("class") == "DepositAmount") {
                //                        var $pay2 = $(this).parent().parent().next().find("input.Amount");
                //                        DepositAmount2 = parseInt($(this).val());
                //                        PayAmount2 = parseInt($pay2.val() == "" ? "0" : $pay2.val());
                //                    } else {
                //                        var $deposit2 = $(this).parent().parent().prev().find("input.Amount");
                //                        DepositAmount2 = parseInt($deposit2.val() == "" ? "0" : $deposit2.val());
                //                        PayAmount2 = parseInt($(this).val());
                //                    }
                //                    var totalamount2 = DepositAmount2 + PayAmount2;
                //                    $dealamount2.text(totalamount2);
                //                }
            });
        });
    }

    function BindUSDAmountBlurEvent() {
        var $inputAmount = $("td input.USDAmount");
        $inputAmount.each(function () {
            $(this).blur(function () {
                if (isNaN($(this).val()) || $(this).val() > 100000000) {
                    if ($(this).parent().is("span")) {
                        if (!$(this).parent().hasClass("wrapdiv")) {
                            $(this).parent().addClass("wrapdiv");
                        }
                    } else {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                    alert('Please fill the valid number.');
                    $(this).val("0");
                    //                    var $dealamount1 = $(this).parent().parent().parent().find("td.dealamount");
                    //                    var PayAmount1 = 0;
                    //                    var DepositAmount1 = 0;
                    //                    if ($(this).parent().parent().attr("class") == "DepositAmount") {
                    //                        var $pay1 = $(this).parent().parent().next().find("input.USDAmount");
                    //                        DepositAmount1 = parseFloat(0);
                    //                        PayAmount1 = parseFloat($pay1.val() == "" ? "0" : $pay1.val());
                    //                    } else {
                    //                        var $deposit1 = $(this).parent().parent().prev().find("input.USDAmount");
                    //                        DepositAmount1 = parseFloat($deposit1.val() == "" ? "0" : $deposit1.val());
                    //                        PayAmount1 = parseFloat(0);
                    //                    }
                    //                    var totalamount1 = DepositAmount1 + PayAmount1;
                    //                    $dealamount1.text(Math.round(totalamount1 * Math.pow(10, 2)) / Math.pow(10, 2));
                    //                    return;
                } else {
                    $(this).parent().removeClass("wrapdiv");
                }

                if ($(this).val() == "" || $(this).val() == "0") {
                    $(this).val("0")
                } else {
                    //四舍五入取整
                    var amount = Math.round($(this).val() * Math.pow(10, 2)) / Math.pow(10, 2);
                    $(this).val(amount);
                }
                GetUSDCalAmount();
                //                if (!$(this).parent().is("span")) {
                //                    var $dealamount = $(this).parent().parent().find("td.dealamount");
                //                    var PayAmount = 0;
                //                    var DepositAmount = 0;
                //                    if ($(this).parent().attr("class") == "DepositAmount") {
                //                        var $pay = $(this).parent().next().find("input.USDAmount");
                //                        DepositAmount = parseFloat($(this).val());
                //                        PayAmount = parseFloat($pay.val() == "" ? "0" : $pay.val());
                //                    } else {
                //                        var $deposit = $(this).parent().prev().find("input.USDAmount");
                //                        DepositAmount = parseFloat($deposit.val() == "" ? "0" : $deposit.val());
                //                        PayAmount = parseFloat($(this).val());
                //                    }
                //                    var totalamount = DepositAmount + PayAmount;
                //                    $dealamount.text(Math.round(totalamount * Math.pow(10, 2)) / Math.pow(10, 2));
                //                } else {
                //                    var $dealamount2 = $(this).parent().parent().parent().find("td.dealamount");
                //                    var PayAmount2 = 0;
                //                    var DepositAmount2 = 0;
                //                    if ($(this).parent().parent().attr("class") == "DepositAmount") {
                //                        var $pay2 = $(this).parent().parent().next().find("input.USDAmount");
                //                        DepositAmount2 = parseFloat($(this).val());
                //                        PayAmount2 = parseFloat($pay2.val() == "" ? "0" : $pay2.val());
                //                    } else {
                //                        var $deposit2 = $(this).parent().parent().prev().find("input.USDAmount");
                //                        DepositAmount2 = parseFloat($deposit2.val() == "" ? "0" : $deposit2.val());
                //                        PayAmount2 = parseFloat($(this).val());
                //                    }
                //                    var totalamount2 = DepositAmount2 + PayAmount2;
                //                    $dealamount2.text(Math.round(totalamount2 * Math.pow(10, 2)) / Math.pow(10, 2));
                //                }
            });
        });
    }

    function EndRequestHandler() {
        BindEvent();
        BindAmountBlurEvent();
        BindUSDAmountBlurEvent();
        DisplayOrHideTable();
        GetRMBCalAmount();
        GetUSDCalAmount();
    }

    function CalAmount() {
        var result = false;
        var $TotalAmount = $("span.RMBTotalAmount");
//        var $DepositAmount = $("td.DepositAmount input.Amount");
//        var $PayAmount = $("td.PayAmount input.Amount");

//        var deposittotalAmount = 0;
//        var paytotalAmount = 0;
        var totalAmount = 0;

//        $DepositAmount.each(function () {
//            deposittotalAmount += parseInt($(this).val());
//        });
//        $PayAmount.each(function () {
//            paytotalAmount += parseInt($(this).val());
//        });
        //totalAmount = paytotalAmount - deposittotalAmount;
        //alert(totalAmount);
        var $Amount = $("td.Amount input.Amount");
        $Amount.each(function () {
            totalAmount += parseFloat($(this).val());
        });
        totalAmount = Math.round(totalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        var ta = Math.round(parseFloat($TotalAmount.text()) * Math.pow(10, 2)) / Math.pow(10, 2);
        if (totalAmount != ta) {
            //alert('Not equal to the total amount!\nPlease fill the valid number.');
            result = true;
        }
        return result;
    }

    function CalUSDAmount() {
        var result = false;
        var $TotalAmount = $("span.USDTotalAmount");
//        var $DepositAmount = $("td.DepositAmount input.USDAmount");
//        var $PayAmount = $("td.PayAmount input.USDAmount");

//        var deposittotalAmount = 0;
//        var paytotalAmount = 0;
        var totalAmount = 0;

//        $DepositAmount.each(function () {
//            deposittotalAmount += parseFloat($(this).val());
//        });
//        $PayAmount.each(function () {
//            paytotalAmount += parseFloat($(this).val());
//        });
        //totalAmount = paytotalAmount - deposittotalAmount; 
        //alert(totalAmount);
        var $Amount = $("td.USDAmount input.USDAmount");
        $Amount.each(function () {
            totalAmount += parseFloat($(this).val());
        });
        totalAmount = Math.round(totalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        var ta = Math.round(parseFloat($TotalAmount.text()) * Math.pow(10, 2)) / Math.pow(10, 2);
        if (totalAmount != ta) {
            //alert('Not equal to the total amount!\nPlease fill the valid number.');
            result = true;
        }
        return result;
    }

    function ChangeGLAccount(obj) {
        var $expenseType = $(obj);
        var $glAccount = $expenseType.parent().parent().parent().parent().find("td input.GLAccount");
        var $hfGLAccount = $('#<%=this.hfGLAccount.ClientID %>');
        var glaccountTable = eval("(" + $hfGLAccount.val() + ")");
        try {
            $.each(glaccountTable, function (i, item) {
                if ($expenseType.val() == "0") {
                    $glAccount.val("");
                    return;
                } else {
                    if ($expenseType.val() == item.name) {
                        $glAccount.val(item.val);
                        return;
                    }
                }
            });
        } catch (e) { }
    }

    function BindEvent() {
        var $ExpenseType = $(".ExpenseType1");
        $ExpenseType.each(function () {
            $(this).mousemove(function () {
                $(this).addClass("ExpenseTypeTD");
            });
            $(this).mouseout(function () {
                $(this).removeClass("ExpenseTypeTD");
            });
            var $children = $(this).children();
            $children.click(function () {
                $children.parent().addClass("ExpenseTypeTD");
                $children.parent().unbind("mouseout");
            });
            $children.blur(function () {
                $children.parent().removeClass("ExpenseTypeTD");
                $children.parent().mouseout(function () {
                    $children.parent().removeClass("ExpenseTypeTD");
                });
            });
        });

        var $CostCenter = $(".CostCenter");
        $CostCenter.each(function () {
            $(this).mousemove(function () {
                $(this).addClass("CostCenterTD");
            });
            $(this).mouseout(function () {
                $(this).removeClass("CostCenterTD");
            });
            var $children = $(this).children();
            $children.click(function () {
                $children.parent().addClass("CostCenterTD");
                $children.parent().unbind("mouseout");
            });
            $children.blur(function () {
                $children.parent().removeClass("CostCenterTD");
                $children.parent().mouseout(function () {
                    $children.parent().removeClass("CostCenterTD");
                });
            });
        });
    }

    function CheckSubmit() {
        $(".wrapdiv").removeClass("wrapdiv");
        var result = true;
        var msg = "";
        var $et = $("select.ExpenseType");
        $et.each(function () {
            if ($.trim($(this).val()) == "0" ) {
                msg += "Please Select Expense Type.\n";
                if (!$(this).parent().parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                }
                result = false;
            }
            if ($(this).val().indexOf("Others") == 0) {
                var $input = $(this).parent().parent().parent().next().find("input");
                if ($input.val() == "") {
                    msg += "Please Select Expense Type.\n";
                    if (!$(this).parent().parent().hasClass("wrapdiv")) {
                        $(this).parent().addClass("wrapdiv");
                    }
                    result = false;
                }
            }
        });
//        var $cc = $("select.cc");
//        $cc.each(function () {
//            if ($.trim($(this).val()) == "0") {
//                msg += "Please Select CostCenter.\n";
//                if (!$(this).parent().parent().hasClass("wrapdiv")) {
//                    $(this).parent().addClass("wrapdiv");
//                }
//                result = false;
//            }
//        });
        var $txtAmount = $("input.Amount");
        $txtAmount.each(function () {
            if ($.trim($(this).val()) == "") {
                msg += "Please fill the Amount.\n";
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).wrap("<span class=\"wrapdiv\"></span>");
                }
                result = false;
            }
        });
        var $txtUSDAmount = $("input.USDAmount");
        $txtUSDAmount.each(function () {
            if ($.trim($(this).val()) == "") {
                msg += "Please fill the Amount.\n";
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).wrap("<span class=\"wrapdiv\"></span>");
                }
                result = false;
            }
        });

        var $txtGLAccount = $("input.GLAccount");
        $txtGLAccount.each(function () {
            if ($.trim($(this).val()) == "") {
                msg += "Please fill GL Account.\n";
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).wrap("<span class=\"wrapdiv\"></span>");
                }
                result = false;
            }
        });

        if ($("#RMBExpenseTypetable").is(":visible")) {
            var calAmountresult = CalAmount();
            if (calAmountresult) {
                msg += "Please check expense detail items because the sum of detail items is not equal to original total cost.\nPlease fill the valid number.";
                var $inputAmount = $("td input.Amount");
                $inputAmount.each(function () {
                    if (!$(this).parent().hasClass("wrapdiv")) {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                });
                result = false;
            }
        }
        if ($("#USDExpenseTypetable").is(":visible")) {
            var calUSDAmountresult = CalUSDAmount();
            if (calUSDAmountresult) {
                msg += "Please check expense detail items because the sum of detail items is not equal to original total cost.\nPlease fill the valid number.";
                var $inputAmount = $("td input.USDAmount");
                $inputAmount.each(function () {
                    if (!$(this).parent().hasClass("wrapdiv")) {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                });
                result = false;
            }
        }
        if (msg != "") {
            alert(msg);
        }
        AppendRMBEmployeeVendor();
        AppendUSDEmployeeVendor();
        return result;
    }

</script>

