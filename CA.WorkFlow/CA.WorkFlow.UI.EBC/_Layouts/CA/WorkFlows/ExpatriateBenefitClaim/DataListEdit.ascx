<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataListEdit.ascx.cs"
    Inherits="CA.WorkFlow.UI.EBC.DataListEdit" %>
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
        left: -95px;
        top: -15px;
        z-index: 1000;
    }
    .CostCenter
    {
        position: absolute;
        left: 0px !important;
        left: -95px;
        top: -15px;
    }
    .CostCenterTD
    {
        width: 260px;
    }
    .ExpenseTypeTD
    {
        width: 230px;
    }
    .wrapdiv
    {
        padding: 2px;
        border: 1px solid red;
    }
    .totalamount
    {
        color: Red;
    }
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4">
            <h3>
                Expatriate Benefit Claim Form<br />
                外籍员工福利报销申请单
            </h3>
        </td>
    </tr>
    <tr>
        <td class="w20">
            WorkFlowNumber<br />
            工作流ID
        </td>
        <td colspan="3">
            <asp:Label ID="lblWorkFlowNumber" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="w20">
            Requested ID<br />
            申请人ID
        </td>
        <td class="w30">
            <asp:Label ID="lblRequestedID" runat="server"></asp:Label>
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
        <td colspan="3">
            <asp:TextBox ID="txtExpenseDescription" runat="server" class="ExpenseDescription"></asp:TextBox>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table" id="expenseTypetable">
            <tr style="height: 20px">
                <td colspan="6">
                    <h3>
                        Expatriate benefit claim details</h3>
                </td>
            </tr>
            <tr style="font-weight: bold;">
                <td class="w5">
                    <asp:ImageButton runat="server" ID="btnAddItem" ToolTip="Click to add the information."
                        ImageUrl="../images/pixelicious_001.png" OnClick="btnAddItem_Click" Width="18"
                        CssClass="img-button" />
                </td>
                <td class="w5">
                    Dr/Cr
                </td>
                <td class="w30">
                    BenefitType
                </td>
                <td class="w15">
                    GL Account
                </td>
                <td class="w30">
                    CostCenter
                </td>
                <td class="w15">
                    Amount
                </td>
            </tr>
            <asp:Repeater ID="rptItem" runat="server" OnItemCommand="rptItem_ItemCommand" OnItemDataBound="rptItem_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnDeleteItem" ToolTip="Remove this information." CommandName="delete"
                                runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                        </td>
                        <td style="text-align: left">
                            Dr
                        </td>
                        <td>
                            <div style="position: relative; z-index: 1000; background-color: White;">
                                <div class="ExpenseType1">
                                    <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="ExpenseType" onchange="ChangeGLAccount(this)">
                                        <asp:ListItem Text="" Value="" Selected="True" />
                                        <asp:ListItem Text="Child education" Value="Child education"></asp:ListItem>
                                        <asp:ListItem Text="House rental" Value="House rental"></asp:ListItem>
                                        <asp:ListItem Text="Car related" Value="Car related"></asp:ListItem>
                                        <asp:ListItem Text="Medical assistance" Value="Medical assistance"></asp:ListItem>
                                        <asp:ListItem Text="Others (please specify)" Value="Others (please specify)"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hidremarkstatus1" runat="server" Value="0" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="lblGLAccount" runat="server" CssClass="GLAccount"></asp:TextBox>
                            <%--<asp:Label ID="lblGLAccount" runat="server"  CssClass="GLAccount"/>--%>
                        </td>
                        <td>
                            <div style="position: relative; background-color: White; z-index: 500">
                                <div class="CostCenter">
                                    <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="cc">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="Amount"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="height: 20px;">
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="evo">
                <td>
                </td>
                <td style="padding: 10px; text-align: right">
                    Cr
                </td>
                <td style="text-align: right">
                    OP - employee vendor
                </td>
                <td class="EmployeeID">
                    <asp:Label ID="lblEmployeeID1" runat="server" />
                </td>
                <td>
                </td>
                <td>
                    <asp:TextBox ID="txtEmployeeVendor" runat="server" Style="border: none; text-align: right"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 20px;" colspan="6">
                </td>
            </tr>
            <%-- <tr class="CashAdvanceORtr">
                <td style="padding: 10px">
                    CR
                </td>
                <td style="text-align: left">
                    Cash Advance - OR
                </td>
                <td>
                   
                </td>
                <td class="CashAdvanceORtd">
                    <asp:TextBox ID="txtCashAdvance" runat="server" style="border:none" ></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblEmployeeID2" runat="server" />
                </td>
            </tr>--%>
            <%-- <tr class="catr" style="display: none">
                <td style="height: 20px;" colspan="6">
                </td>
            </tr>--%>
            <tr class="catr">
                <td>
                </td>
                <td style="padding: 10px; text-align: left" colspan="4">
                    Total claimed amount
                </td>
                <td>
                    <asp:TextBox ID="txtTotalClaimedAmount" runat="server" Style="border: none; text-align: right"
                        ReadOnly="true" CssClass="TotalAmount"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 20px;" colspan="6">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="padding: 10px; text-align: left" colspan="4">
                    Net balance
                </td>
                <td>
                    <asp:TextBox ID="txtNetbalance" runat="server" Style="border: none; text-align: right"
                        ReadOnly="true" CssClass="Netbalance">0</asp:TextBox>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<table class="ca-workflow-form-table">
    <tr>
        <td style="width: 565px">
            Total Amount<br />
            总金额(RMB)
        </td>
        <td style="text-align: right">
            <asp:Label ID="lbTotalAmount" runat="server" CssClass="TotalAmount"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Cash Advance<br />
            员工借款(RMB)
        </td>
        <td style="text-align: right;">
            <asp:Label ID="lblCashAdvanceAmount" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Payable to Employee/(Refund to Finance)<br />
            应付员工/(财务应收) (RMB)
        </td>
        <td style="text-align: right">
            <asp:Label ID="lblPreTotalAmount" runat="server"></asp:Label>
            <asp:HiddenField ID="hfGLAccount" runat="server" Value="" />
            <asp:HiddenField ID="hfCashAdvanceWorkFlowNumber" runat="server" Value="" />
        </td>
    </tr>
</table>
<script type="text/javascript" src="jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">

    $(function () {
        BindEvent();
        BindAmountBlurEvent();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        HideEVO();
        AppendCashAdvance();
        GetCalAmount();
    });


    function AppendHtml(amount, employeeID) {
        var appendHtml = "";
        appendHtml = "<tr><td></td><td style=\"padding: 10px; text-align:right\">Cr</td><td style=\"text-align: right\">OR - cash advance</td><td>" + employeeID + "</td><td></td><td><input type=\"text\" value=" + amount + " style=\"border:none; text-align:right\" readonly=\"readonly\" /></td></tr><tr><td style=\"height: 20px;\" colspan=\"6\"></td></tr>";
        return appendHtml;
    }

    function AppendCashAdvance() {
        var $hfCashAdvanceWorkFlowNumber = $('#<%=this.hfCashAdvanceWorkFlowNumber.ClientID %>');
        var $employeeID = $("td.EmployeeID");
        var cawfid = $hfCashAdvanceWorkFlowNumber.val();
        var $catr = $("tr.catr");
        if (cawfid != "") {
            var strlist = cawfid.split(";");
            if (strlist.length > 0) {
                for (var i = 0; i < strlist.length; i++) {
                    if (strlist[i] != "") {
                        var canumber = strlist[i].substring(strlist[i].indexOf("-") + 1);
                        var $html = $(AppendHtml("-" + canumber, $employeeID.text()));
                        $catr.before($html);
                    }
                }
            }
        }
    }

    function HideEVO() {
        var $evo = $("tr.evo input");
        if ($evo.val() == "0") {
            $evo.parent().parent().hide();
            $evo.parent().parent().next().hide();
        }
    }

    function BindAmountBlurEvent() {
        var $inputAmount = $("td input.Amount");
        $inputAmount.each(function () {
            $(this).blur(function () {
                if (isNaN($(this).val()) || $(this).val() < 0 || $(this).val() > 100000000) {
                    $(this).val("0");
                    if (!$(this).parent().hasClass("wrapdiv")) {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                    alert('Please fill the valid number.');
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
                GetCalAmount();
            });
        });
    }

    function EndRequestHandler() {
        BindEvent();
        BindAmountBlurEvent();
        AppendCashAdvance();
        HideEVO();
        GetCalAmount();
    }

    function CalAmount() {
        var result = false;
        var $inputAmount = $("td input.Amount");
        var $TotalAmount = $("span.TotalAmount");
        var totalAmount = 0;
        $inputAmount.each(function () {
            totalAmount += parseFloat($(this).val());
        });
        totalAmount = Math.round(totalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        var amount = parseFloat($TotalAmount.text());
        amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
        //if (totalAmount != parseFloat($TotalAmount.text())) {
        if (totalAmount != amount) {
            //alert('Not equal to the total amount!\nPlease fill the valid number.');
            result = true;
        }
        return result;
    }

    function GetCalAmount() {
        var result = false;
        var $inputAmount = $("td input.Amount");
        var $TotalAmount = $("span.TotalAmount");
        var totalAmount = 0;
        $inputAmount.each(function () {
            var inputAmount = $(this).val();
            if (inputAmount == "") {
                inputAmount = "0";
            }
            totalAmount += parseFloat(inputAmount);
        });
        var amount = totalAmount - parseFloat($TotalAmount.text());
        totalAmount = Math.round(totalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
        var ta = totalAmount + ";" + amount;
        if (totalAmount != parseFloat($TotalAmount.text())) {
            $("span.TotalAmount").addClass("totalamount");
            $("input.TotalAmount").css("color", "red");
            $("input.Netbalance").css("color", "red");
            $("input.TotalAmount").val(ta.split(';')[0]);
            $("input.Netbalance").val(ta.split(';')[1]);
        } else {
            $("span.TotalAmount").removeClass("totalamount");
            $("input.TotalAmount").css("color", "#06c");
            $("input.Netbalance").css("color", "#06c");
            $("input.TotalAmount").val(ta.split(';')[0]);
            $("input.Netbalance").val(ta.split(';')[1]);
        }
    }

    function GetTmpId(tmpId) {
        return tmpId.substring(0, tmpId.lastIndexOf('_') + 1);
    }

    function ChangeGLAccount(obj) {
        var $expenseType = $(obj);
        var $glAccount = $expenseType.parent().parent().parent().parent().find("td input.GLAccount");
        var $hfGLAccount = $('#<%=this.hfGLAccount.ClientID %>');
        var $lblGLAccount = $('#' + GetTmpId(obj.id) + 'lblGLAccount');
        var glaccountTable = eval("(" + $hfGLAccount.val() + ")");
        try {
            $.each(glaccountTable, function (i, item) {
                if ($expenseType.val() == "0") {
                    $glAccount.val("");
                    $lblGLAccount.val("");
                    return;
                } else {
                    if ($expenseType.val() == item.name) {
                        $glAccount.val(item.val);
                        $lblGLAccount.val(item.val);
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
            if ($.trim($(this).val()) == "0") {
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
        if (msg != "") {
            alert(msg);
        }
        // return false
        return result;
    }

</script>
