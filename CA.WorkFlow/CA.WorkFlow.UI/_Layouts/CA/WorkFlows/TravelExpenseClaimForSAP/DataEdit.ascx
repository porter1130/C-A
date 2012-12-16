<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.TravelExpenseClaimForSAP.DataEdit" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    #tb_travelexpense
    {
        background-image: url("../images/background_white.JPG");
        background-repeat: repeat-y;
        background-position: right;
    }
    #tb_travelexpense td.td_drcr
    {
        width: 10%;
    }
    #tb_travelexpense td.td_expensetype
    {
        width: 25%;
    }
    #tb_travelexpense td.td_costcenter
    {
        width: 35%;
    }
    #tb_travelexpense td.td_rmbamt
    {
        width: 10%;
    }
    #tb_travelexpense td.td_glaccount
    {
        width: 15%;
    }
    
    #tb_travelexpense input
    {
        width: 50px;
    }
    #tb_travelexpense td.td_purpose input
    {
        width: 500px;
    }
    #tb_travelexpense td.td_expensetype input
    {
        width: 145px;
    }
    #tb_travelexpense td.td_rmbamt input
    {
        width: 60px;
    }
    #tb_travelexpense td.td_drcr
    {
        padding-left: 10px;
        padding-right: 10px;
    }
    
    
    #tb_travelexpense td.cc
    {
        padding-top: 6px;
        vertical-align: top;
    }
    
    select.width-fix
    {
        width: 250px;
        z-index: 1000;
    }
    select.expand
    {
        position: absolute;
        width: 270px; /* Let the browser handle it. */
    }
    #dialog input
    {
        font-size: 12px;
    }
    #tb_travelexpense tr.tr_sapsection td.td_summary input
    {
        width: 60px;
        text-align: right;
    }
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td class="label align-center w25">
            Applicant
        </td>
        <td class="value">
            <asp:Label ID="lblApplicant" runat="server"></asp:Label>
        </td>
        <%--<td class="value">
            <asp:Button ID="btnFindTR" runat="server" OnClick="btnFindTR_Click" Text="Find" CssClass="form-button" />
        </td>--%>
    </tr>
</table>
<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table">
            <tr>
                <td colspan="4" class="value align-center">
                    <h3>
                        Employee Information 员工信息</h3>
                </td>
            </tr>
            <tr>
                <td class="label align-center w25">
                    Chinese Name
                    <br />
                    中文姓名
                </td>
                <td class="label align-center w25">
                    English Name<br />
                    英文姓名
                </td>
                <td class="label align-center w25">
                    ID/Passport No.<br />
                    身份证/护照号码
                </td>
                <td class="value align-center w25">
                    Department
                    <br />
                    部门
                </td>
            </tr>
            <tr>
                <td class="label align-center w25">
                    <asp:Label ID="lblChineseName" runat="server" />
                </td>
                <td class="label align-center w25">
                    <asp:Label ID="lblEnglishName" runat="server" />
                </td>
                <td class="label align-center w25">
                    <asp:Label ID="lblIDNumber" runat="server" />
                </td>
                <td class="label align-center w25">
                    <asp:Label ID="lblDepartment" runat="server" />
                </td>
            </tr>
            <tr class="last">
                <td colspan="4">
                    <table class="inner-table">
                        <tr>
                            <td class="label align-center">
                                Mobile Phone No.<br />
                                手机号码
                            </td>
                            <td class="label align-center w15">
                                <asp:Label ID="lblMobile" runat="server" />
                            </td>
                            <td class="label align-center">
                                Office Ext. No.<br />
                                分机号码
                            </td>
                            <td class="value align-center w15">
                                <asp:Label ID="lblOfficeExt" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<table class="inner-table ca-workflow-form-table" id="tb_travelexpense" style="width: 820px">
    <tr>
        <td class="value align-center" colspan="6">
            <h3>
                Travel Expense Claim 出差费用报销
            </h3>
        </td>
    </tr>
    <tr>
        <td colspan="6">
            <table class="inner-table">
                <tr>
                    <td class="label">
                        Purpose of business trip
                    </td>
                    <td class="value td_purpose">
                        <asp:TextBox ID="txtPurpose" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="label" style="text-align: left; width: 30px;">
            <asp:ImageButton runat="server" ID="ImageButton1" OnClick="btnAddExpense_Click" ToolTip="Click to add the hotel information."
                ImageUrl="../images/pixelicious_001.png" Width="18" CssClass="img-button" />
        </td>
        <td class="label align-center td_drcr">
            Dr/Cr
        </td>
        <td class="label align-center td_expensetype">
            Expense Type
        </td>
        <td class="label align-center td_glaccount">
            GL
            <br />
            Account
        </td>
        <td class="label align-center td_costcenter">
            Cost Center
        </td>
        <td class="value align-center td_rmbamt">
            Claim
            <br />
            Amt
        </td>
    </tr>
    <tr>
        <td colspan="6">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="inner-table">
                        <asp:Repeater ID="rptExpense" runat="server" OnItemCommand="rptExpense_ItemCommand"
                            OnItemDataBound="rptExpense_ItemDataBound">
                            <ItemTemplate>
                                <tr class="tr_claimedsection">
                                    <td class="label" style="text-align: left; width: 30px;">
                                        <asp:ImageButton ID="btnDeleteExpense" CommandName="delete" ToolTip="Delete this expense information."
                                            runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                                    </td>
                                    <td class="label td_drcr">
                                    </td>
                                    <td class="label td_selecttype" style="text-align: left;">
                                        <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="select_location" onchange="SelectExpenseType(this)">
                                            <asp:ListItem Text="" Value="" Selected="True" />
                                            <asp:ListItem Text="Travel - hotel" Value="Travel - hotel" />
                                            <asp:ListItem Text="Travel - meal" Value="Travel - meal" />
                                            <asp:ListItem Text="Travel - local transportation" Value="Travel - local transportation" />
                                            <asp:ListItem Text="Travel - sample purchase" Value="Travel - sample purchase" />
                                            <asp:ListItem Text="Travel - airticket/train/bus" Value="Travel - airticket/train/bus" />
                                            <asp:ListItem Text="Travel - others" Value="Travel - others" />
                                            <asp:ListItem Text="Store mgnt exp - local transportation" Value="Store mgnt exp - local transportation" />
                                            <asp:ListItem Text="Store exp - travel hotel" Value="Store exp - travel hotel" />
                                            <asp:ListItem Text="Store exp - travel food" Value="Store exp - travel food" />
                                            <asp:ListItem Text="Store exp - travel - transportation" Value="Store exp - travel - transportation" />
                                        </asp:DropDownList>
                                    </td>
                                    <td class="label align-center td_glaccount">
                                        <asp:TextBox ID="txtGLAccount" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="label td_costcenter cc">
                                        <asp:DropDownList ID="ddlCostCenter" CssClass="width-fix" runat="server" onchange="SelectCostCenter(this)">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="value td_rmbamt">
                                        <asp:TextBox ID="txtRmbAmt" runat="server" CssClass="claimedsection" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr class="tr_sapsection tr_ca hidden">
                            <td class="label" style="text-align: left; width: 30px;">
                            </td>
                            <td class="label td_drcr">
                            </td>
                            <td class="label align-center td_expensetype">
                                OR - Cash Advance
                            </td>
                            <td class="label align-center td_glaccount">
                                <asp:Label ID="lblCAGLAccount" runat="server" />
                                <asp:HiddenField ID="hidCAGLAccount" runat="server" />
                            </td>
                            <td class="label td_costcenter cc">
                            </td>
                            <td class="value td_rmbamt">
                                <asp:TextBox ID="txtCARmbAmt" runat="server" />
                            </td>
                        </tr>
                        <tr class="tr_sapsection tr_cc hidden">
                            <td class="label" style="text-align: left; width: 30px;">
                            </td>
                            <td class="label td_drcr">
                            </td>
                            <td class="label align-center td_expensetype">
                                OR - Credit Card
                            </td>
                            <td class="label align-center td_glaccount">
                                <asp:Label ID="lblCCGLAccount" runat="server" />
                                <asp:HiddenField ID="hidCCGLAccount" runat="server" />
                            </td>
                            <td class="label td_costcenter cc">
                            </td>
                            <td class="value td_rmbamt">
                                <asp:TextBox ID="txtCCRmbAmt" runat="server" />
                            </td>
                        </tr>
                        <tr class="tr_sapsection tr_ccbalance hidden">
                            <td class="label" style="text-align: left; width: 30px;">
                            </td>
                            <td class="label td_drcr">
                            </td>
                            <td class="label align-center td_expensetype">
                                OR - Credit Card
                            </td>
                            <td class="label align-center td_glaccount">
                                <asp:Label ID="lblCCBalanceGLAccount" runat="server" />
                                <asp:HiddenField ID="hidCCBalanceGLAccount" runat="server" />
                            </td>
                            <td class="label td_costcenter cc">
                            </td>
                            <td class="value td_rmbamt">
                                <asp:TextBox ID="txtCCBalanceRmbAmt" runat="server" />
                            </td>
                        </tr>
                        <tr class="tr_sapsection tr_ev hidden">
                            <td class="label" style="text-align: left; width: 30px;">
                            </td>
                            <td class="label td_drcr">
                            </td>
                            <td class="label align-center td_expensetype">
                                OP - Employee Vendor
                            </td>
                            <td class="label align-center td_glaccount">
                                <asp:Label ID="lblEVGLAccount" runat="server" />
                                <asp:HiddenField ID="hidEVGLAccount" runat="server" />
                            </td>
                            <td class="label td_costcenter cc">
                            </td>
                            <td class="value td_rmbamt">
                                <asp:TextBox ID="txtEVRmbAmt" runat="server" />
                            </td>
                        </tr>
                        <tr class="tr_sapsection tr_cabalance hidden">
                            <td class="label" style="text-align: left; width: 30px;">
                            </td>
                            <td class="label td_drcr">
                            </td>
                            <td class="label align-center td_expensetype">
                                OR - Cash Advance
                            </td>
                            <td class="label align-center td_glaccount">
                                <asp:Label ID="lblCABalanceGLAccount" runat="server" />
                                <asp:HiddenField ID="hidCABalanceGLAccount" runat="server" />
                            </td>
                            <td class="label td_costcenter cc">
                            </td>
                            <td class="value td_rmbamt">
                                <asp:TextBox ID="txtCABalanceRmbAmt" runat="server" />
                            </td>
                        </tr>
                        <tr class="tr_sapsection tr_totalclaimedamt">
                            <td class="label" style="text-align: left; width: 30px;">
                            </td>
                            <td class="label td_drcr">
                            </td>
                            <td class="label align-center td_expensetype">
                                Total claimed amount
                            </td>
                            <td class="label align-center td_glaccount">
                                <asp:Label ID="lblTotalClaimedAmt" runat="server" />
                                <asp:HiddenField ID="hidTotalClaimedAmt" runat="server" />
                            </td>
                            <td class="label td_costcenter cc">
                            </td>
                            <td class="value td_summary">
                                <asp:TextBox ID="txtTotalClaimedAmt" runat="server" />
                            </td>
                        </tr>
                        <tr class="tr_sapsection tr_netbalance">
                            <td class="label" style="text-align: left; width: 30px;">
                            </td>
                            <td class="label td_drcr">
                            </td>
                            <td class="label align-center td_expensetype">
                                Net balance
                            </td>
                            <td class="label align-center td_glaccount">
                                <asp:Label ID="lblNetBalance" runat="server" />
                                <asp:HiddenField ID="hidNetBalance" runat="server" />
                            </td>
                            <td class="label td_costcenter cc">
                            </td>
                            <td class="value td_summary">
                                <asp:TextBox ID="txtNetBalance" runat="server" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ImageButton1" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table" id="table_claimsummary">
    <tr>
        <td colspan="2" class="value align-center">
            <h3>
                Claim Summary</h3>
        </td>
    </tr>
    <tr>
        <td style="text-align: right; padding-right: 10px; width: 80%">
            Total
        </td>
        <td>
            <QFL:FormField ID="ffTotalCost" runat="server" FieldName="TotalCost">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="height: 5px;">
        </td>
    </tr>
</table>
<br />
<div id="hidFields">
    <asp:HiddenField ID="hidTravelDetails" runat="server" />
    <asp:HiddenField ID="hidSAPGLAccount" runat="server" />
</div>
<%--Modal Dialog--%>
<!-- #customize your modal window here -->
<div id="dialog" class="hidden">
</div>
<%--End Modal Dialog--%>
<script type="text/javascript">

    function GetPreId(clientId, id) {
        return clientId.substring(0, clientId.length - id.length);
    }

    function commafy(num) {
        num = num + "";
        var tmpArr = num.split('.');
        var re = /(-?\d+)(\d{3})/
        while (re.test(tmpArr[0])) {
            tmpArr[0] = tmpArr[0].replace(re, "$1,$2")
        }
        return tmpArr.length >= 2 ? tmpArr[0] + '.' + tmpArr[1] : tmpArr[0];
    }

    function Escape(s, exp) {
        while (exp.test(s)) {
            s = s.replace(exp, "");
        }
        return s;
    }

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

    function ReturnJsonValue(id) {
        var value = $(id).val();
        return eval('(' + value + ')');
    }
</script>
<script type="text/javascript">
    var mode = '<%=this.Mode %>';

    //Cash Advance-OR
    var lblCAGLAccount = '#<%=this.lblCAGLAccount.ClientID %>';
    var hidCAGLAccount = '#<%=this.hidCAGLAccount.ClientID %>';
    var txtCARmbAmt = '#<%=this.txtCARmbAmt.ClientID %>';

    //Credit Card - OR
    var lblCCGLAccount = '#<%=this.lblCCGLAccount.ClientID %>';
    var hidCCGLAccount = '#<%=this.hidCCGLAccount.ClientID %>';
    var txtCCRmbAmt = '#<%=this.txtCCRmbAmt.ClientID %>';

    //Credit Card - OR (Balance)
    var lblCCBalanceGLAccount = '#<%=this.lblCCBalanceGLAccount.ClientID %>';
    var hidCCBalanceGLAccount = '#<%=this.hidCCBalanceGLAccount.ClientID %>';
    var txtCCBalanceRmbAmt = '#<%=this.txtCCBalanceRmbAmt.ClientID %>';

    //Employee Vendor - OP
    var lblEVGLAccount = '#<%=this.lblEVGLAccount.ClientID %>';
    var hidEVGLAccount = '#<%=this.hidEVGLAccount.ClientID %>';
    var txtEVRmbAmt = '#<%=this.txtEVRmbAmt.ClientID %>';

    //Cash Advance - OR (Balance)
    var lblCABalanceGLAccount = '#<%=this.lblCABalanceGLAccount.ClientID %>';
    var hidCABalanceGLAccount = '#<%=this.hidCABalanceGLAccount.ClientID %>';
    var txtCABalanceRmbAmt = '#<%=this.txtCABalanceRmbAmt.ClientID %>';

    //Global Variable
    var hidTravelDetails = '#<%=this.hidTravelDetails.ClientID %>';
    var hidSAPGLAccount = '#<%=this.hidSAPGLAccount.ClientID %>';
    var ffPostfix = '_ctl00_ctl00_TextField';

    var lblApplicant = '#<%=this.lblApplicant.ClientID %>';
    var lblChineseName = '#<%=this.lblChineseName.ClientID %>';
    var lblEnglishName = '#<%=this.lblEnglishName.ClientID %>';
    var lblIDNumber = '#<%=this.lblIDNumber.ClientID %>';
    var lblMobile = '#<%=this.lblMobile.ClientID %>';
    var lblDepartment = '#<%=this.lblDepartment.ClientID %>';
    var lblOfficeExt = '#<%=this.lblOfficeExt.ClientID %>';

    var txtPurpose = '#<%=this.txtPurpose.ClientID %>';

    var ffTotalCost = '#<%=this.ffTotalCost.ClientID %>' + ffPostfix;

    //EmployeeId
    var employeeId = '<%=this.ApplicantEmployeeId %>';

    var tdRmbAmt = '#tb_travelexpense td.td_rmbamt';
    var sapSection = '#tb_travelexpense tr.tr_sapsection';

    //Total claimed amt
    var txtTotalClaimedAmt = '#<%=this.txtTotalClaimedAmt.ClientID %>';
    //Net Balance
    var txtNetBalance = '#<%=this.txtNetBalance.ClientID %>';

    $(function () {
        LoadSourceData();

        BindChangeEvent();

        SetControlMode();

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });


    function EndRequestHandler() {
        LoadSAPSummary();
        LoadGLAccount();
        SetControlMode();
    }

    function SetControlMode() {
        $(sapSection).find('input').attr('contentEditable', false);
        $(ffTotalCost).attr('contentEditable', false);

        $(tdRmbAmt + ' input').each(function () {
            var rmbamt = $(this).val();
            if (!ca.util.emptyString(rmbamt)
                && !isNaN(rmbamt)) {
                if (parseFloat(rmbamt) >= 0) {
                    $(this).parent().prevAll('td.td_drcr').text('Dr');
                    $(this).parent().prevAll('td.td_drcr').attr('style', 'text-align:left;');

                    $(this).attr('text-align', 'left');
                } else {
                    $(this).parent().prevAll('td.td_drcr').text('Cr');
                    $(this).parent().prevAll('td.td_drcr').attr('style', 'text-align:right;');

                    $(this).attr('style', 'text-align:right;');
                }
            }
        });

        GetTotal();
    }

    function LoadSourceData() {
        var hidTravelDetailsJson = ReturnJsonValue(hidTravelDetails);

        if (hidTravelDetailsJson.length > 0) {

            $(lblApplicant).text(hidTravelDetailsJson[0].Applicant);
            $(lblChineseName).text(hidTravelDetailsJson[0].ChineseName);
            $(lblEnglishName).text(hidTravelDetailsJson[0].EnglishName);
            $(lblIDNumber).text(hidTravelDetailsJson[0].IDNumber);
            $(lblMobile).text(hidTravelDetailsJson[0].Mobile);
            $(lblOfficeExt).text(hidTravelDetailsJson[0].OfficeExt);
            $(lblDepartment).text(hidTravelDetailsJson[0].Department);
            $(txtPurpose).val(hidTravelDetailsJson[0].Purpose);

            $(ffTotalCost).val(hidTravelDetailsJson[0].TotalCost);

        }

        LoadSAPSummary();

        if (mode != 'Edit') {
            LoadGLAccount();
        }
    }

    function LoadSAPSummary() {

        //load GLAccount
        $('#tb_travelexpense tr.tr_sapsection td.td_expensetype').each(function () {
            $(this).next().find('span').text(employeeId);
            $(this).next().find('input').val(employeeId);
        });

        var hidTravelDetailsJson = ReturnJsonValue(hidTravelDetails);


        $(txtCARmbAmt).val(hidTravelDetailsJson[0].CashAdvanced != '0' ? '-' + hidTravelDetailsJson[0].CashAdvanced : '0');

        $(txtCCRmbAmt).val(hidTravelDetailsJson[0].PaidByCreditCard != '0' ? '-' + hidTravelDetailsJson[0].PaidByCreditCard : '0');

        if ($(txtCARmbAmt).val() != "0") {
            $('#tb_travelexpense tr.tr_ca').show();
        }

        if ($(txtCCRmbAmt).val() != "0") {
            $('#tb_travelexpense tr.tr_cc').show();
        }

        if ($(txtCCBalanceRmbAmt).val() != "0") {
            $('#tb_travelexpense tr.tr_ccbalance').show();
        }

        if ($(txtEVRmbAmt).val() != "0") {
            $('#tb_travelexpense tr.tr_ev').show();
        }

        if ($(txtCABalanceRmbAmt).val() != "0") {
            $('#tb_travelexpense tr.tr_cabalance').show();
        }
    }

    function LoadGLAccount() {

        $('#tb_travelexpense td.td_selecttype select').each(function () {
            $(this).change();
        });

        $('#tb_travelexpense td.td_costcenter select').each(function () {
            $(this).change();
        });

    }

    function BindChangeEvent() {
        $('#tb_travelexpense td.td_rmbamt input').live('change', function () {
            GetTotal();
        });
    }

    function GetTotal() {
        var currTotal = 0;
        var totalAmt = 0;

        var isValid = false;
        $('#tb_travelexpense td.td_rmbamt input').each(function () {
            var rmbamt = $(this).val();
            if (!ca.util.emptyString(rmbamt)
                && !isNaN(rmbamt)) {
                if ($(this).hasClass('claimedsection')) {
                    totalAmt += parseFloat(rmbamt);
                }
            }
        });

        var totalCost = $(ffTotalCost).val();

        if (totalAmt.toFixed(2) != parseFloat(totalCost)) {
            $(ffTotalCost).attr('style', 'color:red');
        } else {
            $(ffTotalCost).attr('style', 'color:#0066CC');
            isValid = true;
        }

        $(txtTotalClaimedAmt).val(totalAmt.toFixed(2));
        $(txtNetBalance).val((totalAmt - parseFloat(totalCost)).toFixed(2));

        return isValid;
    }
</script>
<script type="text/javascript">
    function DisplayDefaultMessage(obj, msg) {
        if ($(obj).val() == msg) {
            $(obj).val('');
        }
    }

    function SelectExpenseType(obj) {

        var expenseType = $(obj).val();
        var preId = GetPreId($(obj).attr('id'), 'ddlExpenseType');
        var costCenter = $('#' + preId + 'ddlCostCenter').val();

        var hidSAPGLAccountValue = $(hidSAPGLAccount).val();
        var json = eval('(' + hidSAPGLAccountValue + ')');

        for (var i = 0; i < json.length; i++) {
            if (json[i].ExpenseType == expenseType) {
                //$('#' + preId + 'lblGLAccount').text(json[i].GLAccount);

                var glAccount = json[i].GLAccount;
                if (costCenter.toLowerCase().indexOf('s') == 0) {
                    glAccount.replace('1551', '1517');
                }
                $('#' + preId + 'txtGLAccount').val(glAccount);
                break;
            }
        }
    }

    function SelectCostCenter(obj) {
        var costCenter = $(obj).val();
        var preId = GetPreId($(obj).attr('id'), 'ddlCostCenter');
        var glAccount = $('#' + preId + 'txtGLAccount').val().trim();

        if (costCenter.toLowerCase().indexOf('s') == 0) {
            $('#' + preId + 'txtGLAccount').val(glAccount.replace('1551', '1517'));
        } else {
            $('#' + preId + 'txtGLAccount').val(glAccount.replace('1517', '1551'));
        }
    }

    function beforeSubmit() {
        var isValidate = Validate();
        if (isValidate) {
            CreateForbidDIV(); //单击生成弹出层，防止重复提交。
        }
        return isValidate;
    }

    function Validate() {
        var isValid = true;
        var errorMsg = '';
        if (!GetTotal()) {
            errorMsg = 'Please check expense detail items because the sum of detail items is not equal to original total cost.';
        }

        if (errorMsg.length > 0) {
            alert(errorMsg);
            isValid = false;
        }
        return isValid;
    }
</script>
