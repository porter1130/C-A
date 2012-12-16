<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.EmployeeExpenseClaim2.DataView" %>
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
        line-height:15px;
    }
    .attachment td
    {
        border-bottom: none;
        border-right: none;
    }
     .ca-workflow-form-table1 td
    {
        padding: 5px;
        text-align: left;
        margin: 0;
         border-bottom:none;
         border-right:none;
         
    }
    .ca-workflow-form-table
    {
        margin-top: 20px;
    }
    .datetime td
    {
        border-bottom: none;
        border-right: none;
    }
    .column-specialapprove input
    {
        border: none;
        cursor: pointer;
    }
  .ddl
  {
   margin:0px;
   cursor:pointer;
  }
  
    .w36{ width:35%}
    .w12{ width:12%}
    .w13{ width:13%}
    .w14{ width:15%}
       .summarytype{display:none; border:none}
        .wrapdiv{ border:1px solid red;  padding:2px}
        .item{ height:30px}
        .OriginalAmount{ display:none}
        .amountcolor{color:#06c;}
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4">
              <h3>Employee Expense Claim Form<br />
                员工报销申请表</h3>
        </td>
    </tr>
    <tr>
        <td>
            WorkflowNumber<br />
            工作流ID
        </td>
        <td colspan="3">
            <qfl:formfield id="ffWorkflowNumber" runat="server" fieldname="WorkflowNumber" controlmode="Display"></qfl:formfield>
        </td>
    </tr>
    <tr>
        <td class="w20">
            Dept<br />
            部门
        </td>
        <td class="w30">
            <QFL:FormField ID="FormField2" runat="server" FieldName="Department" ControlMode="Display" ></QFL:FormField>
        </td>
        <td class="w20">
            Requested By<br />
            申请人
        </td>
        <td class="w30">
            <QFL:FormField ID="FormField1" runat="server" FieldName="RequestedBy" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td>
            Claims Description<br />
            报销描述
        </td>
        <td colspan="3">
            <QFL:FormField ID="ffExpenseDescription" runat="server" FieldName="ExpenseDescription" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
       <table class="ca-workflow-form-table"  id="expenseTypetable">
             <tr >
                <td colspan="9">
                      <h3>Employee expense claim details</h3>
                </td>
            </tr>
            <tr>
                <td class="w5">
                    No
                </td>
                <td class="w10">
                    ExpenseType<br />
                    费用类别
                </td>
                <td class="w5">
                    Date<br />
                    日期
                </td>
                <td class="w36">
                    Expense Purpose<br />
                    费用用途
                </td>
                <td class="w10">
                    CostCenter<br />
                    成本中心
                </td>
                <td class="OriginalAmount">
                    Original<br />
                    Amount(RMB)
                </td>
                <td class="w13">
                    Amount<br />
                    金额(RMB)
                </td>
                <td class="w14">
                    Company Std<br />公司标准
                </td>
                <td class="column-specialapprove w5">
                    Special<br />Approval
                </td>
            </tr>
            <asp:Repeater ID="rptItem" runat="server" OnItemDataBound="rptItem_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 25px;" >
                                <%# Container.ItemIndex + 1 %> 
                                <asp:HiddenField ID="hidItemId" runat="server" Value='<%# Eval("ID")%>' />
                                <asp:HiddenField ID="hidPreAmount" runat="server" Value='<%# Eval("PreAmount")%>' />
                                <asp:HiddenField ID="hidApprovedAmount" runat="server" Value='<%# Eval("Amount")%>' />
                                <asp:HiddenField ID="hidTotalAmount" runat="server" Value='<%# Eval("TotalAmount")%>' />
                            </td>
                            <td class="ExpenseType">
                                <asp:Label ID="lblExpenseType" runat="server" />
                            </td>
                            <td class="datetime">
                                <%# Eval("Dates", "{0:d}")%>
                            </td>
                            <td>
                                <%# Eval("ExpensePurpose")%>
                            </td>
                            <td class="cc">
                                <%# Eval("CostCenter")%>
                            </td>
                            <td class="OriginalAmount">
                                <%# Eval("OriginalAmount")%>
                            </td>
                            <td class="amounttd" style=" color:#06c">
                                <asp:Label ID="lbAmount" runat="server" Text='<%# Eval("Amount")%>' CssClass="Amount"  />
                            </td>
                            <td>
                                <asp:Label ID="lbCompanyStd" runat="server" Text='<%# Eval("CompanyStandard")%>' />
                            </td>
                            <td class="column-specialapprove" >
                                <span class="SpecialApproveResult hidden">
                                    <%# Eval("SpecialApproveResult")%></span><span class="hidden"><asp:CheckBox ID="IsSpecialApprove"
                                        runat="server" onclick="return UpdateSpecialApprove(this);"  Visible="false"/>
                                        <asp:DropDownList ID="ddlSpecialApprove" runat="server"  CssClass="ddl"   Width="100%">
                                            <asp:ListItem Text="" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Approve" Value="1" ></asp:ListItem>
                                            <asp:ListItem Text="Reject" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </span>
                            </td>
                        </tr>
                        <tr>
                             <td colspan="2" style="text-align:center">
                                Remark<br />
                                备注
                            </td>
                            <td colspan="7" align="left" style=" text-align:left; color:#06c; ">
                                <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark")%>'  CssClass="lblRemark"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="9" style=" height:20px;" >
                            </td>
                        </tr>
                    </ItemTemplate>
             </asp:Repeater>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<table class="ca-workflow-form-table summarytypetable"  >
    
    <tr>
       <td colspan="3">
            <h3>
                Expense Summary<br />
            </h3>
        </td>
    </tr>
    <tr>
           <td style="width:295px">
            ExpenseType<br />
            费用类别
        </td>
        <td style="width: 200px">
            CostCenter<br />
            成本中心
        </td>
        <td>
            Amount<br />
            金额(RMB)
        </td>
    </tr>
    <tr class="summarytype">
        <td colspan="3">
        </td>
    </tr>
<%--</table>
<table class="ca-workflow-form-table">
    <tr>
        <td class="w30"></td>
        <td class="w30"></td>
        <td class="w40"></td>
    </tr>--%>
    <tr>
         <td style="border-top: #ccc 2px solid">
            Total Amount<br />
            总金额(RMB)
        </td>
         <td style="border-top: #ccc 2px solid" colspan="2">
            <asp:Label ID="lbTotalAmount" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td >
            Cash Advance<br />
            员工借款(RMB)
        </td>
        <td id="CashAdvance" colspan="2">
            <QFL:FormField ID="FormField3" runat="server" FieldName="CashAdvance" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td style="border-top: #ccc 2px solid">
            Payable to Employee/(Refund to Finance)<br />
           应付员工/(财务应收) (RMB)
        </td>
        <td style="border-top: #ccc 2px solid" colspan="2">
            <asp:Label ID="lbAmountDue" runat="server"></asp:Label>
        </td>
    </tr>
      </table>
    <table class="ca-workflow-form-table" >
    <tr>
           <td style="width:295px">
            Original Invoice Attached<br />
            附原发票
        </td>
        <td colspan="2">
            <QFL:FormField ID="FormField4" runat="server" FieldName="IsAttachInvoice" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td >
            Remark<br />
            备注
        </td>
        <td colspan="2">
            <QFL:FormField ID="ffRemark" runat="server" FieldName="Remark" ControlMode="Display" ></QFL:FormField>
        </td>
    </tr>
    <tr>
        <td >
            Attachment<br />
            附件
        </td>
        <td colspan="2" class="attachment" style="text-align: left; ">
            <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Display" ></QFL:FormAttachments>
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
            Please contact Poppy Wang at ext. 6148 to resolve the above problem(s) before Finance
            can continue to process your claim form.
        </td>
    </tr>
    
</table>
<asp:HiddenField ID="hidDisplayMode" runat="server" />

<asp:HiddenField ID="hidta" runat="server"  Value=""/>
<asp:HiddenField ID="hidca" runat="server"  Value=""/>
<asp:HiddenField ID="hidspecialApproveResult" runat="server"  Value="0"/>

<asp:HiddenField ID="hidSummaryExpenseType" runat="server" Value="" />

<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    var JSExpenseClaim = {};
    var radio_fapiao = '';
    var radio_information = '';
    var radio_claimAmt = '';
    $(function () {

        $('#<%= this.UpdatePanel1.ClientID %> span.SpecialApproveResult').each(function () {
            var step1 = '<%=this.Step %>';
            var satus = '<%=this.Satus %>';
            if ($(this).text() == '11') {
                $(this).next('span').removeClass('hidden');
                //$(this).next('span').children().attr("checked", "checked");
                if (step1 == "ConfirmTask") {
                    $(this).next('span').text("Approve");
//                    var $amount3 = $(this).parent().parent().find("td .Amount");
                    //                    $amount3.css("color", "red");
                    $("td.amounttd").css("color", "black");
                }
                if (step1 == "DisplayStep") {
                    if (satus == "Confirm") {
                        $(this).next('span').text("Approve");
                    } else {
                        $(this).next('span').text("In Progress");
                    }
                    var $amount1 = $(this).parent().parent().find("td .Amount");
                    $amount1.css("color", "red");
                    $(".OriginalAmount").show();
                }

                if (step1 == "ConfirmTask1") {
                    if (satus == "Confirm") {
                        $(this).next('span').text("Approve");
                    } else {
                        $(this).next('span').text("In Progress");
                    }
                    var $amount2 = $(this).parent().parent().find("td .Amount");
                    $amount2.css("color", "red");
                    $(".OriginalAmount").show();
                }


            }
            if ($(this).text() == '00') {

                if (step1 == "ConfirmTask") {
                    $(this).next('span').removeClass('hidden');
                    $(this).next('span').text("");
                }
                if (step1 == "DisplayStep" || step1 == "ConfirmTask1") {
                    $(this).next('span').removeClass('hidden');
                    $(this).next('span').text("Reject");
                    $(".OriginalAmount").show();
                }
                if (step1 == "NextApproveTask") {
                    $(this).next('span').find("select").val("2");
                }
                var $amount2 = $(this).parent().parent().find("td .Amount");
                $amount2.css("color", "black");
            }
            if ($(this).text() == '22') {
                if (step1 == "ConfirmTask") {
                    $(this).next('span').removeClass('hidden');
                    $(this).next('span').text("");
                }
                if (step1 == "DisplayStep") {
                    $(this).next('span').removeClass('hidden');
                    $(this).next('span').text("");
                    $(".OriginalAmount").show();
                }
                var $amount2 = $(this).parent().parent().find("td .Amount");
                $amount2.css("color", "black");
            }
            if ($(this).text() == '1') {
                if (step1 == "DisplayStep") {
                    $(this).next('span').removeClass('hidden');
                    $(this).next('span').text("In Progress");
                    $(".OriginalAmount").show();
                }
                if (step1 == "NextApproveTask") {
                    $(this).next('span').removeClass('hidden');
                }
                var $amount3 = $(this).parent().parent().find("td .Amount");
                $amount3.css("color", "red");
            }

            if (step1 == "DisplayStep" || step1 == "ConfirmTask1" || step1 == "ConfirmTask") {
                $("span.lblRemark").css("color","black");
            }

        });

        JSExpenseClaim.CalcTotal();

        if ($('#<%= this.hidDisplayMode.ClientID %>').val().length > 0) {
            $('#<%= this.UpdatePanel1.ClientID %> td.column-specialapprove .SpecialApproveResult').addClass('hidden');
        }

        var $hidspecialApproveResult = $('#<%= this.hidspecialApproveResult.ClientID %>');
        var $specialapproveinput = $(".column-specialapprove input");
        if ($hidspecialApproveResult.val() == "1") {
            $specialapproveinput.each(function () {
                $(this).attr("disabled", "flase");
            });
        }

        var step = '<%=this.Step %>';
        if (step == "ConfirmTask" || step == "ConfirmTask1") {
            $("#table_pendingform").show();
            $("#commettable").hide();
            if (step == "ConfirmTask1") {
                $("#displaydiv").show();
                SetReasonStatus();
            }
        }


        var $rblFapiao = $('#<%= this.rblFapiao.ClientID %>');
        var $rblInformation = $('#<%= this.rblInformation.ClientID %>');
        var $rblClaimedAmt = $('#<%= this.rblClaimedAmt.ClientID %>');

        $rblFapiao.click(function () {
            var $rblFapiao1 = $('#<%= this.rblFapiao.ClientID %>');
            var $txtFapiaoOtherReason = $('#<%= this.txtFapiaoOtherReason.ClientID %>');


            if ($rblFapiao1.find(':radio[checked=true]').val() == "other reasons, please state") {
                $txtFapiaoOtherReason.removeClass("hidden");
            } else {
                $txtFapiaoOtherReason.val("");
                $txtFapiaoOtherReason.addClass("hidden");
            }
        });
        $rblInformation.click(function () {
            var $rblInformation1 = $('#<%= this.rblInformation.ClientID %>');
            var $txtInformationOtherReason = $('#<%= this.txtInformationOtherReason.ClientID %>');
            if ($rblInformation1.find(':radio[checked=true]').val() == "other reasons, please state") {
                $txtInformationOtherReason.removeClass("hidden");
            } else {
                $txtInformationOtherReason.val("");
                $txtInformationOtherReason.addClass("hidden");
            }
        });
        $rblClaimedAmt.click(function () {
            var $rblClaimedAmt1 = $('#<%= this.rblClaimedAmt.ClientID %>');
            var $txtClaimedOtherReason = $('#<%= this.txtClaimedOtherReason.ClientID %>');
            if ($rblClaimedAmt1.find(':radio[checked=true]').val() == "other reasons, please state") {
                $txtClaimedOtherReason.removeClass("hidden");
            } else {
                $txtClaimedOtherReason.val("");
                $txtClaimedOtherReason.addClass("hidden");
            }
        });
        ShowReason();

        var $ddlSpecialApprove = $('#<%= this.UpdatePanel1.ClientID %> span.SpecialApproveResult').next('span').children();
        $ddlSpecialApprove.each(function () {
            $(this).change(function () {
                var specialApprove = $(this)[0];
                UpdateSpecialApprove(specialApprove);
            });
        });

        DrawSummaryExpenseTable();

        var pending = '<%=this.Pending %>';
        if (pending != "") {
            var $pending = $("input[value='Pending']");
            //$pending.hide();
            $pending.attr("disabled", "disabled");
        }

        CheckRemarkInfo();
        //ChangeReasonStatus();
        RadioButtonListBind();
    });


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

    function CheckRemarkInfo() {
        var $lblRemark = $("span.lblRemark");
        $lblRemark.each(function () {
            if ($(this).html() == "") {
                $(this).parent().parent().hide();
            }
        });
    }

    function checkApprove() {
        var result=true;
        var step1 = '<%=this.Step %>';
        $('#<%= this.UpdatePanel1.ClientID %> span.SpecialApproveResult').each(function () {
            if (step1 == "NextApproveTask") {//option:selected
                var $result = $(this).next("span[class!='hidden']").find("select").find("option:selected");
                if ($result.length > 0) {
                    if ($result.val() == "0") {
                        var $select = $(this).next('span').find("select option:selected");
                        if ($select.val() == "0") {
                            result = false;
                            alert("Please Check Special Approval Items.");
                            if (!$select.parent().parent().hasClass("wrapdiv")) {
                                $select.parent().wrap("<div class=\"wrapdiv\"></div>");
                            }
                            //return false;
                        }
                    }
                }
            }
        });
        return result;
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


    JSExpenseClaim.GetPreId = function (tmpId) {
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_rptItem_ctl00_ddlExpenseType
        return tmpId.substring(0, tmpId.lastIndexOf('_') + 1);
    }

    function UpdateSpecialApprove(sender) {


        JSExpenseClaim.preId = JSExpenseClaim.GetPreId(sender.id);

        var $ddlSpecialApprove = $(sender).find("option:selected");
        //alert($ddlSpecialApprove.val());
        if ($ddlSpecialApprove.val() == "2") {
            JSExpenseClaim.amount = $('#' + JSExpenseClaim.preId + 'lbCompanyStd').text();
            if (JSExpenseClaim.amount != "") {
                $('#' + JSExpenseClaim.preId + 'hidApprovedAmount').val(JSExpenseClaim.amount);
                $('#' + JSExpenseClaim.preId + 'lbAmount').text(JSExpenseClaim.amount);
            } else {
                $('#' + JSExpenseClaim.preId + 'hidApprovedAmount').val("0");
                $('#' + JSExpenseClaim.preId + 'lbAmount').text("0");
            }
            $('#' + JSExpenseClaim.preId + 'lbAmount').css("color","#06c");
        } else {
            JSExpenseClaim.amount = $('#' + JSExpenseClaim.preId + 'hidPreAmount').val();
            $('#' + JSExpenseClaim.preId + 'hidApprovedAmount').val(JSExpenseClaim.amount);
            $('#' + JSExpenseClaim.preId + 'lbAmount').text(JSExpenseClaim.amount);

            var $result = $('#' + JSExpenseClaim.preId + 'lbAmount').parent().next().next().find("span.SpecialApproveResult");
            if ($result.text() == "1") {
                $('#' + JSExpenseClaim.preId + 'lbAmount').css("color", "red");
            } else {
                $('#' + JSExpenseClaim.preId + 'lbAmount').css("color", "#06c");
            }

        } 
        SummaryAmount($('#' + JSExpenseClaim.preId + 'lbAmount'));
        JSExpenseClaim.CalcTotal();
        
    }
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

    JSExpenseClaim.CalcTotal = function () {
        JSExpenseClaim.totalAmount = 0;
        $('#<%= this.UpdatePanel1.ClientID %> span.Amount').each(function () {
            JSExpenseClaim.totalAmount += parseFloat($(this).text());
        });
        $('#<%= this.lbTotalAmount.ClientID %>').text(commafy(JSExpenseClaim.totalAmount));
        var amount = parseFloat($('#<%= this.lbTotalAmount.ClientID %>').text().replace(",", "").replace(",", "").replace(",", "")) - parseFloat($('#CashAdvance').text().replace(",", "").replace(",", "").replace(",", ""));
//        if (amount > 0) {
            $('#<%= this.lbAmountDue.ClientID %>').text(commafy(parseFloat($('#<%= this.lbTotalAmount.ClientID %>').text().replace(",", "").replace(",", "").replace(",", "")) - parseFloat($('#CashAdvance').text().replace(",", "").replace(",", "").replace(",", ""))));
        //} else {
          //  $('#<%= this.lbAmountDue.ClientID %>').text(commafy($('#<%= this.lbTotalAmount.ClientID %>').text().replace(",", "").replace(",", "").replace(",", "")));
       // }
        $('#<%= this.hidta.ClientID %>').val($('#<%= this.lbTotalAmount.ClientID %>').text().replace(",", "").replace(",", "").replace(",", ""));
        $('#<%= this.hidca.ClientID %>').val($.trim($('#CashAdvance').text().replace(",", "").replace(",", "").replace(",", "")));
    }
</script>
<script type="text/javascript">
    function GetStep() {
        var step = '<%=this.Step %>';
        return step;
    }

    function AppendHtml(expenseType, amount, costcenter) {
        var appendHtml = "";
        appendHtml = "<tr class=\"item\" ><td >" + expenseType + "</td><td >" + costcenter + "</td><td >" + amount + "</td></tr>";
       // appendHtml = "<tr class=\"item\" ><td  >" + expenseType + "</td><td title=\"" + costcenter + "\" >" + amount + "</td></tr>";
        return appendHtml;
    }
    function DrawSummaryExpenseTable() {
        $(".summarytypetable tr").remove(".item");
        var $summarytype = $(".summarytype");
        var $hidSummaryExpenseType = $('#<%= this.hidSummaryExpenseType.ClientID %>');
        var summaryExpenseType = $hidSummaryExpenseType.val();
        var summaryExpense = eval("(" + summaryExpenseType + ")");
        try {
            $.each(summaryExpense, function (i, item) {
                var $html = $(AppendHtml(item.name, Math.round(item.val * Math.pow(10, 2)) / Math.pow(10, 2), item.costcenter));
                $summarytype.before($html);
            });
        } catch (e) { }
    }
    function SummaryAmount(amounttobj) {
        DrawSummaryExpenseTable1();return;
        var $item = $(".summarytypetable tr.item");
        if ($item.length > 0) {
            var $summarytype = $(".summarytypetable tr.summarytype");
            var $obj = amounttobj;
            var $expenseType = $obj.parent().parent().find("td.ExpenseType");
            var expenseType = $expenseType.text();
            if (expenseType.indexOf("Mobile") != -1) {
                expenseType = "Mobile";
            }
            //alert("expenseType：" + expenseType + " $obj.text()：" + $obj.text());
            var costcenter = $obj.parent().parent().find("td.cc").text();
            //UpdateSummaryExpenseTypeAmount(expenseType, $obj.text(), costcenter);
            DrawSummaryExpenseTable1();
        }
    }

    function CheckSummaryTypeHtml(expenseType, costcenter) {
        var result = false;
        var exp = ".summarytypetable tr:contains('" + expenseType + "')";
        var $summarytypehtml = $(exp);
        var $expcostcenter = $summarytypehtml.find("td:contains('" + costcenter + "')");
        if ($summarytypehtml.length > 0) {
            if ($expcostcenter.length > 0) {
                result = true;
            }
        }
        return result;
    }

    function CalExpenseTypeAmount(expenseType, costcenter) {
        var $ExpenseType = $("td.ExpenseType");
        var amount = 0;
        var et = expenseType;
        if (et == "Wireless/Mobile") {
            et = "Mobile";
        }
        if (et == "Store mgnt exp - Wireless/Mobile") {
            et = "Store mgnt exp - mobile";
        }
        $ExpenseType.each(function () {
            var costcenterval = $(this).parent().find("td.cc").text();
            if ($(this).text().indexOf(et) != -1 && costcenterval.indexOf(costcenter) != -1) {
                var $amount = $(this).parent().find("td.amounttd");
                if ($amount.text() != "") {
                    amount += parseFloat($amount.text());
                }
            }
        });
        amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
        return amount;
    }
    function UpdateSummaryExpenseTypeAmount(expenseType, amount, costcenter) {
        var exp = ".summarytypetable tr td:contains('" + $.trim(expenseType) + "')";
        var $summarytypehtml = $(exp);
        var $expcostcenter = $summarytypehtml.parent().find("td:contains('" + $.trim(costcenter) + "')");
        if ($expcostcenter.length > 0) {
            var $amount = $expcostcenter.next();
            var totalamount = CalExpenseTypeAmount($.trim(expenseType), $.trim(costcenter));
            $amount.text(totalamount + "");
        }
    }
//    function CheckSummaryTypeHtml(expenseType, costcenter) {
//        var result = false;
//        var exp = ".summarytypetable:contains('" + expenseType + "')";
//        var $summarytypehtml = $(exp);
//        if ($summarytypehtml.length > 0) {
//            result = true;
//        }
//        return result;
//    }
    function DrawSummaryExpenseTable1() {
        $(".summarytypetable tr").remove(".item");
        var $summarytype = $(".summarytype");
        var $expenseType = $("#expenseTypetable tr td.ExpenseType");
        $expenseType.each(function () {
            if ($(this).text() != "") {
                var $amount = $(this).parent().find("td.amounttd");
                var expenseType = $(this).text();
                var costcenter = $(this).parent().find("td.cc").text();

                if (expenseType.indexOf("Mobile") != -1) {
                    expenseType = "Wireless/Mobile";
                }
                if (expenseType.indexOf("mobile") != -1) {
                    expenseType = "Store mgnt exp - Wireless/Mobile";
                }
                var result = CheckSummaryTypeHtml(expenseType, costcenter);

                var txtAmount = $amount.text() == "" ? "0" : $amount.text();
                txtAmount = Math.round(txtAmount * Math.pow(10, 2)) / Math.pow(10, 2);
                if (!result) {
                    var $html = $(AppendHtml(expenseType, txtAmount, costcenter));
                    $summarytype.before($html);
                } else {
                    UpdateSummaryExpenseTypeAmount(expenseType, txtAmount, costcenter);
                }
//                if (!result) {
//                    var $html = $(AppendHtml(expenseType, Math.round(parseFloat($amount.text()) * 100) / 100, costcenter));
//                    $summarytype.before($html);
//                } else {
//                    UpdateSummaryExpenseTypeAmount(expenseType, Math.round(parseFloat($amount.text()) * 100) / 100, costcenter);
//                }
            }
        });
        UpdateSummaryExpenseType();
        
    }
    function UpdateSummaryExpenseType() {
        var $hidSummaryExpenseType = $('#<%= this.hidSummaryExpenseType.ClientID %>');
        $hidSummaryExpenseType.val("");
        var summaryExpenseType = "[";
        var $summarytypetable = $(".summarytypetable tr.item");
        $summarytypetable.each(function () {
            //summaryExpenseType += "{name:'" + $(this).find("td").eq(0).text() + "',val:'" +$(this).find("td").eq(1).text() + "'},";
            summaryExpenseType += "{name:'" + $(this).find("td").eq(0).text() + "',val:'" + $(this).find("td").eq(2).text() + "',costcenter:'" + $(this).find("td").eq(1).text() + "'},";
        });
        summaryExpenseType += "]";
        $hidSummaryExpenseType.val(summaryExpenseType);
        //alert($hidSummaryExpenseType.val());
       
    }
</script>
