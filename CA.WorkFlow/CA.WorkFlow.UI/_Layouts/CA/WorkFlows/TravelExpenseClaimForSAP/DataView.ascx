<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="DataView.ascx.cs" Inherits="CA.WorkFlow.UI.TravelExpenseClaimForSAP.DataView" %>
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
        width: 30%;
    }
    #tb_travelexpense td.td_costcenter
    {
        width: 25%;
        text-align: center;
    }
    #tb_travelexpense td.td_rmbamt
    {
        width: 15%;
    }
    #tb_travelexpense td.td_paidbycredit
    {
        width: 10%;
    }
    #tb_travelexpense td.td_companystd
    {
        width: 10%;
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
        width: 150px;
    }
    
    #tb_travelexpense td.td_paidbycredit input
    {
        width: 15px;
    }
    
    #tb_travelexpense td.cc
    {
        padding-top: 6px;
        vertical-align: top;
    }
    #tb_travelexpense td.td_subtotal
    {
        text-align: right;
        padding-right: 10px;
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
    
    #tb_travelexpense td.td_drcr
    {
        padding-left: 10px;
        padding-right: 10px;
    }
    #tb_travelexpense td.td_rmbamt
    {
        padding-left: 10px;
        padding-right: 10px;
    }
    #tb_travelexpense tr.tr_sapsection td.td_summary
    {
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
    <tr class="tr_workflownumber last">
        <td class="label align-center w30 td_wftitle">
            WorkflowNumber<br />
            工作流ID
        </td>
        <td class="value td_wfno">
            <QFL:FormField ID="ffTCWFNumber" runat="server" FieldName="TCWorkflowNumber" ControlMode="Display">
            </QFL:FormField>
        </td>
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
            <tr>
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
            <tr class="tr_sapnosection hidden last">
                <td class="label align-center w30 saptitle">
                    SAP.No<br />
                    SAP编号
                </td>
                <td class="value align-center sapnumber">
                    <asp:Label ID="lblSapNumber" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<table class="inner-table ca-workflow-form-table" id="tb_travelexpense" style="width: 681px">
    <tr>
        <td class="value align-center" colspan="5">
            <h3>
                Travel Expense Claim 出差费用报销
            </h3>
        </td>
    </tr>
    <tr>
        <td colspan="5">
            <table class="inner-table">
                <tr>
                    <td class="label">
                        Purpose of business trip
                    </td>
                    <td class="value td_purpose">
                        <asp:Label ID="lblPurpose" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
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
            Claim Amt
        </td>
    </tr>
    <tr>
        <td colspan="5">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="inner-table">
                        <asp:Repeater ID="rptExpense" runat="server">
                            <ItemTemplate>
                                <tr class="tr_claimedamt">
                                    <td class="label td_drcr">
                                    </td>
                                    <td class="label align-center td_expensetype">
                                        <%#Eval("ExpenseType") %>
                                        <asp:HiddenField ID="hidSAPSection" runat="server" Value='<%#Eval("SAPSection")%>' />
                                    </td>
                                    <td class="label align-center td_glaccount">
                                        <%#Eval("GLAccount") %>
                                    </td>
                                    <td class="label td_costcenter cc">
                                        <%#Eval("CostCenter") %>
                                    </td>
                                    <td class="value align-center td_rmbamt claimedsection">
                                        <%#Eval("ApprovedRmbAmt")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr class="tr_sapsection tr_totalclaimedamt">
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
                            <td class="value td_summary td_totalclaimed">
                            </td>
                        </tr>
                        <tr class="tr_sapsection tr_netbalance">
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
                            <td class="value td_summary td_netbalance">
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
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
        <td class="td_totalcost">
            <QFL:FormField ID="ffTotalCost" runat="server" FieldName="TotalCost" ControlMode="Display">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
        <td style="height: 5px;">
        </td>
    </tr>
</table>
<br />
<div id="hidFields">
    <asp:HiddenField ID="hidTravelDetails" runat="server" />
</div>
<%--Modal Dialog--%>
<!-- #customize your modal window here -->
<div id="dialog" class="hidden">
</div>
<%--End Modal Dialog--%>
<script type="text/javascript">
    var isSAPNoVisible = '<%=this.IsSAPNoVisible %>';
    var hidTravelDetails = '#<%=this.hidTravelDetails.ClientID %>';
    var ffPostfix = '_ctl00_ctl00_TextField';

    var lblApplicant = '#<%=this.lblApplicant.ClientID %>';
    var lblChineseName = '#<%=this.lblChineseName.ClientID %>';
    var lblEnglishName = '#<%=this.lblEnglishName.ClientID %>';
    var lblIDNumber = '#<%=this.lblIDNumber.ClientID %>';
    var lblMobile = '#<%=this.lblMobile.ClientID %>';
    var lblDepartment = '#<%=this.lblDepartment.ClientID %>';
    var lblOfficeExt = '#<%=this.lblOfficeExt.ClientID %>';

    var lblPurpose = '#<%=this.lblPurpose.ClientID %>';

    var tdRmbAmt = '#tb_travelexpense td.td_rmbamt';
    var ffTotalCost = '#table_claimsummary td.td_totalcost';

    //Total claimed amt
    var txtTotalClaimedAmt = '#tb_travelexpense tr.tr_sapsection td.td_totalclaimed';
    //Net Balance
    var txtNetBalance = '#tb_travelexpense tr.tr_sapsection td.td_netbalance';

    $(function () {

        LoadSourceData();

        //CostCenterBind();

        //BindChangeEvent();

        SetControlMode();
        // Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });

    function SetControlMode() {
        var totalAmt = 0;
        debugger;
        $(tdRmbAmt).each(function () {
            var rmbamt = $(this).text();

            if (!ca.util.emptyString(rmbamt)
                && !isNaN(rmbamt)) {
                var isSAPSection = $(this).prevAll('td.td_expensetype').find('input').val();
                if (!parseInt(isSAPSection)) {
                    totalAmt += parseFloat(rmbamt);
                }
                if (parseFloat(rmbamt) >= 0) {
                    $(this).prevAll('td.td_drcr').text('Dr');
                    $(this).prevAll('td.td_drcr').attr('style', 'text-align:left;');

                    $(this).attr('style', 'text-align:left');
                } else {
                    $(this).prevAll('td.td_drcr').text('Cr');
                    $(this).prevAll('td.td_drcr').attr('style', 'text-align:right;');

                    $(this).attr('style', 'text-align:right;');
                }
            }

        });

        var totalCost = Escape($(ffTotalCost).text(), /,/);
        $(txtTotalClaimedAmt).text(totalAmt.toFixed(2));
        $(txtNetBalance).text((totalAmt - parseFloat(totalCost)).toFixed(2));
    }
    function LoadSourceData() {
        var hidTravelDetailsValue = $(hidTravelDetails).val();
        var hidTravelDetailsJson = eval('(' + hidTravelDetailsValue + ')');

        if (hidTravelDetailsJson.length > 0) {

            $(lblApplicant).text(hidTravelDetailsJson[0].Applicant);
            $(lblChineseName).text(hidTravelDetailsJson[0].ChineseName);
            $(lblEnglishName).text(hidTravelDetailsJson[0].EnglishName);
            $(lblIDNumber).text(hidTravelDetailsJson[0].IDNumber);
            $(lblMobile).text(hidTravelDetailsJson[0].Mobile);
            $(lblOfficeExt).text(hidTravelDetailsJson[0].OfficeExt);
            $(lblDepartment).text(hidTravelDetailsJson[0].Department);

            $(lblPurpose).text(hidTravelDetailsJson[0].Purpose);
        }

        if (isSAPNoVisible == 'true') {
            $('.tr_sapnosection').show();
        }
    }
</script>
<script type="text/javascript">
    function DisplayDefaultMessage(obj, msg) {
        if ($(obj).val() == msg) {
            $(obj).val('');
        }
    }
</script>
<script type="text/javascript">
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
</script>
