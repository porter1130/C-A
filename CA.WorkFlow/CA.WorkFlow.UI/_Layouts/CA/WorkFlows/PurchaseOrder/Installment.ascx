<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Installment.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseOrder.Installment" %>
<style type="text/css">
    .ClearBoth
    {
        clear: both;
    }
    .PaymentTitle
    {
        border-bottom: 1px solid #aaa;
        margin-bottom: 20px;
        padding-bottom: 10px;
    }
    .PaymentPaymentTitleItem
    {
        float: left;
    }
    
    
    
    .InstallButton
    {
        border: 1px solid #9dabb6;
        background: url(../images/button/btnout_bg_center.gif) repeat-x;
    }
    .InstallmentDiv
    {
        display: none;
        position: absolute;
        z-index: 5;
        background-color: #ffffff;
        border: 2px solid #000000;
        padding: 10px;
    }
    .Installmetn
    {
        display: none;
    }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        $("#<%= RadioListPaymentType.ClientID%>").change(function () {
            var selectedVal = $('input[name=ctl00$PlaceHolderMain$ListFormControl3$DataForm1$Installment1$RadioListPaymentType]:checked').val(); //.index();
            if (selectedVal == "Once") {
                $(".PaymentOnce").show()
                $(".Installmetn").hide();
            }
            else {
                $(".PaymentOnce").hide();
                $(".Installmetn").show();
            }
            $('#ctl00_PlaceHolderMain_ListFormControl3_DataForm1_ffPaymentCondition_ctl00_ctl00_TextField').val("");
        });

        //点击确认
        $("#PaymentCofirm").click(function () {
            var isCheckOk = CheckInstallment();
            if (isCheckOk) {
                ClosePaymentType();
            }
        });
        //点击关闭
        $("#PaymentClose").click(function () {
            ClosePaymentType();
        });
        //选 择 到货后付款
        $(".PaymentGRSR").live("click", function () {
            if ($(this).children("input").attr("checked")) {
                $(".PaymentGRSR").children("input").removeAttr("checked");
                $(this).children("input").attr("checked", "checked");
            }
        });

        InitePaymentInfo();
    });

    //显示分期付款的数据 。
    function InitePaymentInfo() {
        var PaymentType = $('input[name=ctl00$PlaceHolderMain$ListFormControl3$DataForm1$Installment1$RadioListPaymentType]:checked').val();
        if (PaymentType == "Once") {
            $(".PaymentOnce").show();
            $(".Installmetn").hide();
        }
        else {
            $(".PaymentOnce").hide();
            $(".Installmetn").show();
        }
    } 
     

    //关闭弹出层及遮罩
    function ClosePaymentType() {
        ClearForbidDIV();
        $(".InstallmentDiv").fadeOut();
    }

    //验证能过
    function CheckInstallment() {
        var result = true;
        var selectedVal = $('input[name=ctl00$PlaceHolderMain$ListFormControl3$DataForm1$Installment1$RadioListPaymentType]:checked').val();
        if (selectedVal == "Once") {
            var PONO = $(".PONO").val();
            var PaymnetOnceCommets = $(".PaymnetOnceCommets").val();
            $('#ctl00_PlaceHolderMain_ListFormControl3_DataForm1_ffPaymentCondition_ctl00_ctl00_TextField').val(PaymnetOnceCommets)
        }
        else {

            var payAfterGRSRLength = $(".PaymentGRSR > input[type='checkbox']:checked").length;
            if (payAfterGRSRLength == 0) {
                result = false;
                alert("Please Select a item for Payment after GR/SR");
                return result;
            }
            var totalPercent = CheckTotalPercent();
            if (!totalPercent) {//验证不通过
                alert("Total Percent must be 100%");
                result = false;
            }

            else { //验证通过
                $('#ctl00_PlaceHolderMain_ListFormControl3_DataForm1_ffPaymentCondition_ctl00_ctl00_TextField').val("");
                var index = 0;
                var paymentWords = "";
                $(".PaymentItem").each(function () {
                    index++;
                    var PaymentPercent = $(this).find(".PaymentPercent").val();
                    var PaymentComments = $(this).find(".PaymentComments").val();
                    var PaymentGRSR = $(this).find(".PaymentGRSR").children("input").attr("checked") == true ? " . " : "";
                    paymentWords += index
                    paymentWords += ".  ";
                    paymentWords += PaymentPercent + "%";
                    if (PaymentComments != "") {
                        paymentWords += "   "
                        paymentWords += PaymentComments;
                    }
                    // paymentWords += "   ,Payment ater GR/SR? "
                    paymentWords += PaymentGRSR
                    paymentWords += "\r\n\r\n";
                });
                $('#ctl00_PlaceHolderMain_ListFormControl3_DataForm1_ffPaymentCondition_ctl00_ctl00_TextField').val(paymentWords)
                /* $(".HFPaymentInfo").val(""); ///清空值。
                var PONO = $(".PONO").val();
                var index=1;
                $(".PaymentItem").each(function () {
                alert("0");
                var PaymentPercent = $(this).find(".PaymentPercent").val();
                var PaymentComments = $(this).find(".PaymentComments").val();
                var PaymentGRSR = $(this).find(".PaymentGRSR").attr("checked") == true ? 1 : 0;
                SaveData(PONO, index, PaymentPercent, PaymentGRSR, PaymentComments);
                index++;
                });
                alert($(".HFPaymentInfo").text());*/
            }

        }
        return result;
    }

    //验证百分比是否符合100%
    function CheckTotalPercent() {
        //  var regFloat = /^(\d)|(([1-9]+)(\.)(\d+))$/;
        var result = true;
        var regFloat = /^([1-9][0-9]*)(\.?)(\d*)$/; /// /^(\d+)(\.?)(\d*)$/;
        var regInt = /^\d$/;
        var totalPercent = 0;
        $(".PaymentPercent").each(function () {
            var percent = $(this).val();
            if (percent !== "") {
                if (regFloat.test(percent)) {
                    totalPercent += parseFloat($(this).val()); /// $(this).val();
                }
                else {//数据不合法
                    $(this).focus();
                    $(this).val("");
                    result = false;
                    return false;
                }
            }
            else {//为空
                $(this).focus();

                result = false;
                return false;
            }
        });
        totalPercent = totalPercent.toFixed(0);
        if (totalPercent != 100) {
            result = false;
        }
        return result;
    }
</script>
<div class="InstallmentDiv">
    <div class="PaymentTitle">
        <table cellpadding="10" cellspacing="10">
            <tr>
                <td style="font-weight: bold">
                    Payment Type:
                </td>
                <td>
                    <asp:RadioButtonList ID="RadioListPaymentType" runat="server" CellPadding="5" RepeatDirection="Horizontal">
                        <asp:ListItem Value="InstallMent">Installment</asp:ListItem>
                        <asp:ListItem Value="Once" Selected="True">Payment Once</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </div>
    <div class="PaymentOnce">
        <table class="ca-workflow-form-table" cellpadding="10">
            <tr>
                <td class="label align-center" style="width: 20%">
                    备注<br />
                    Comments
                </td>
                <td class="label align-center" style="width: 80%">
                    <%--<textarea name="textarea" class="PaymnetOnceCommets" cols="50" rows="5"></textarea>--%>
                    <asp:TextBox ID="TextBoxOnceComments" TextMode="MultiLine" class="PaymnetOnceCommets"
                        Rows="5" Columns="50" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="Installmetn">
        <div class="PaymentTitle">
            NO of installment:
            <asp:DropDownList ID="DDLPaymentCount" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DDLPaymentCount_SelectedIndexChanged">
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
                <asp:ListItem>13</asp:ListItem>
                <asp:ListItem>14</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
            </asp:DropDownList>
        </div>
        <%--
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Repeater ID="ReapterInstallment" runat="server">
                    <HeaderTemplate>
                        <table class="ca-workflow-form-table" cellpadding="10">
                            <tr>
                                <td class="label align-center" style="width: 20%">
                                    付款百分比<br />
                                    Percent
                                </td>
                                <td class="label align-center" style="width: 60%">
                                    备注<br />
                                    Comments
                                </td>
                                <td class="label align-center" style="width: 20%">
                                    系统收货后付款<br />
                                    Payment after GR/SR
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="PaymentItem">
                            <td class="label align-center">
                                <asp:TextBox ID="TextBoxPercent" Width="40px" class="PaymentPercent" runat="server"
                                    Text='<%# Eval("Paid")%>'></asp:TextBox>
                                %
                            </td>
                            <td class="label align-center">
                                <asp:TextBox ID="TextBoxCommens" runat="server" TextMode="MultiLine" Rows="3" Columns="20"
                                    CssClass="PaymentComments" Text='<%# Eval("Comments")%>'></asp:TextBox>
                            </td>
                            <td class="label align-center">
                                <asp:CheckBox ID="CheckBoxIsGRSR" class="PaymentGRSR" runat="server" Style="border: none;
                                    width: 20px" Checked='<%# Eval("IsNeedGR").ToString()=="1"?true:false%>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DDLPaymentCount" EventName="SelectedIndexChanged">
                </asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="PaymentBottom">
        <div class="ca-workflow-form-buttons">
            <input type="button" class="InstallButton" id="PaymentCofirm" value="Confirm" />
            <input type="button" class="InstallButton" id="PaymentClose" value="Close" />
        </div>
    </div>
</div>
