<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryReport.ascx.cs"
    Inherits="CA.WorkFlow.UI.PaymentRequest.QueryReport" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
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
    .paiddate td
    {
        border: none;
        padding: 0px;
    }
    .create td
    {
        border: none;
        padding: 0px;
    }
    .ca-workflow-form-table
    {
        margin-top: 25px;
    }
    .ca-workflow-form-table input
    {
        width: 60px;
    }
    .Report
    {
        width: 950px;
        background-color: White;
    }
    .Report
    {
        cursor: pointer;
        margin: 0px;
    }
    .Report input
    {
        border: none;
        width: 30px;
    }
    .Report .title
    {
        font-weight: bold;
    }
    .Report tr.item td
    {
        text-align: left;
    }
    .condtion
    {
        margin-bottom: 20px;
    }
    .wrapdiv
    {
        border-bottom: 1px solid red;
    }
    .ca-workflow-form-table .ms-dtinput input
    {
        width: 122px;
    }
    .create .ms-dtinput input
    {
        width: 65px;
    }
    .create table
    {
        float: left;
    }
</style>
<style type="text/css">
    .ca-workflow-form-table td.currency
    {
        text-align: left;
    }
    .currency input
    {
        cursor: pointer;
        border: none;
        width: auto;
        margin-left: 20px;
    }
</style>
<table class="ca-workflow-form-table condtion">
    <tr>
        <td class="value align-center" colspan="4">
            <h3>
                Payment Request Report</h3>
        </td>
    </tr>
    <tr>
        <td class="label">
            单据类型
        </td>
        <td class="label1">
            <select id="dpQueryType" runat="server">
                <option value="Opex">Opex</option>
                <option value="Capex">Capex</option>
                <option value="TravelExpenseClaim">TravelExpenseClaim</option>
                <option value="CreditCardClaim">CreditCardClaim</option>
                <option value="EmployeeExpenseClaim" >EmployeeExpenseClaim</option>
                <option value="ExpatriateBenefitClaim">ExpatriateBenefitClaim</option>
                <option value="CashAdvanceRequest" >CashAdvanceRequest</option>
            </select>
        </td>
        <td class="label">
            单据状态
        </td>
        <td class="label1">
            <select id="dpStatus" runat="server">
                <option value=""></option>
                <option value="In Progress">In Progress</option>
                <option value="Rejected">Rejected</option>
                <option value="Pending">Pending</option>
                <option value="Complete">Complete</option>
                <option value="Posted to SAP">Posted to SAP</option>
                <option value="Paid">Paid</option>
            </select>
        </td>
    </tr>
    <tr>
        <td class="label">
            部门
        </td>
        <td>
            <input type="text" value="" id="txtDepartment" runat="server" style="width: 70%;" />
        </td>
        <td class="label">
            付款日
        </td>
        <td class="paiddate">
            <cc1:CADateTimeControl ID="txtPaidDate" runat="server" DateOnly="true"  CssClassTextBox="DateTimeControl"  />
        </td>
    </tr>
    <tr>
        <td class="label">
            申请人
        </td>
        <td>
            <input type="text" value="" id="txtApplicant" runat="server" style="width: 70%" />
        </td>
        <td class="label VendorNameText">
            收款用户名称
        </td>
        <td class="VendorNameInput">
            <input type="text" value="" id="txtVendorName" runat="server" style="width: 70%" />
        </td>
    </tr>
    <tr>
        <td class="label w20">
            单据编号
        </td>
        <td class="label w30">
            <input type="text" value="" id="txtStartNO" runat="server" />到
            <input type="text" value="" id="txtEndNO" runat="server" />
        </td>
        <td class="label w20">
            制单日
        </td>
        <td class="label w30 create">
            <cc1:CADateTimeControl ID="txtStartCreate" runat="server" DateOnly="true" CssClassTextBox="DateTimeControl" />
            <table>
                <tr>
                    <td>
                        到
                    </td>
                </tr>
            </table>
            <cc1:CADateTimeControl ID="txtEndCreate" runat="server" DateOnly="true" CssClassTextBox="DateTimeControl" />
        </td>
    </tr>
    <tr>
        <td class="label">
            金额
        </td>
        <td class="label">
            <input type="text" value="" id="txtStartAmount" runat="server" />
            到
            <input type="text" value="" id="txtEndAmount" runat="server" />
        </td>
        <td class="label VendorNOText">
            供应商编号
        </td>
        <td class="label VendorNOInput">
            <input type="text" value="" id="txtStartVendorNO" runat="server" /><span>到</span>
            <input type="text" value="" id="txtEndVendorNO" runat="server" />
        </td>
    </tr>
    <tr id="currency">
        <td class="label">
            PONO
        </td>
        <td class="label">
            <input type="text" value="" id="txtSystemPONO" runat="server" style=" width:150px"/>
        </td>
        <td class="label">
            币种
        </td>
        <td class="label currency">
            <input type="radio" value="0" name="currency" id="raRMBCurrency" runat="server" checked="true" />RMB
            <input type="radio" value="1" name="currency" id="raNORMBCurrency" runat="server" />非RMB
        </td>
    </tr>
    <tr>
        <td colspan="2" style="border: none">
        </td>
        <td class="label" style="border: none">
            <asp:Button ID="btnExportExcel" runat="server" OnClick="btnExportExcel_Click" Text="Export Excel"
                OnClientClick="return CheckExport()" Style="border: #06c 1px solid; cursor: pointer;
                padding: 3px; width: 100px; border-color: transparent;" />
        </td>
        <td class="label">
            <asp:Button ID="btnQueryReport" runat="server" OnClick="btnQueryReport_Click" Text="Query Report"
                OnClientClick="return CheckQuery()" Style="border: #06c 1px solid; cursor: pointer;
                padding: 3px; width: 100px; border-color: transparent" />
        </td>
    </tr>
</table>
<table class="ca-workflow-form-table Report" style="width:1300px">
    <asp:Repeater ID="PaymentRequestReportData" runat="server">
        <HeaderTemplate>
            <tr class="title">
                <td class="value align-center w5">
                    <input type="checkbox" class="AllItems" style="width: 20px" />
                </td>
                <td class="value align-center w5">
                    申请人
                </td>
                <td class="value align-center w5">
                    部门
                </td>
                <td class="value align-center w5">
                    SAPNO
                </td>
                <td class="value align-center w10">
                    单据编号
                </td>
                <td class="value align-center w5">
                    PONO
                </td>
                <td class="value align-center w10">
                    创建时间
                </td>
                <td class="value align-center w10">
                    供应商号码
                </td>
                <td class="value align-center w10">
                    收款用户名称
                </td>
                <td class="value align-center w10">
                    描述
                </td>
                <td class="value align-center w5">
                    金额
                </td>
                <td class="value align-center w5">
                    币种
                </td>
                <td class="value align-center w5">
                    状态
                </td>
                <td class="value align-center w10">
                    付款日
                </td>
                <td class="value align-center w10">
                    发票
                </td>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="item">
                <td class="value align-center">
                    <asp:CheckBox ID="ckbItems" runat="server" CssClass="Items" ToolTip='<%#Eval("Title") %>' />
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblApplicant" runat="server" Text='<%#Eval("Applicant") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("Dept") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblSAPNO" runat="server" Text='<%#Eval("SAPNumber")%>'></asp:Label>
                </td>
                <td class="value align-center">
                    <a href='http://cnashsptest.cnaidc.cn:91/WorkFlowCenter/Lists/PaymentRequestItems/DispForm.aspx?ID=<%#Eval("ID")%>'
                        target="_blank">
                        <asp:Label ID="lblSubPRNo" runat="server" Text='<%#Eval("SubPRNo") %>'></asp:Label></a>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblPONO" runat="server" Text='<%#Eval("SystemPONo") %>'></asp:Label>
                </td>
                 <td class="value align-center">
                    <asp:Label ID="lblNewPONO" runat="server" Text='<%#Eval("PONO") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblCreated" runat="server" Text='<%#Eval("Created") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblVendorNo" runat="server" Text='<%#Eval("VendorNo") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblVendorName" runat="server" Text='<%#Eval("VendorName") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblPaymentDesc" runat="server" Text='<%#Eval("PaymentDesc") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%#Eval("TotalAmount") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("Currency") %>'></asp:Label>
                </td>
                <td class="value align-center ItemStatus">
                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PaidDate") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblIsAttachedInvoice" runat="server" Text='<%#Eval("IsAttachedInvoice").ToString()=="0"?"NO":"YES"%>'></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<table class="ca-workflow-form-table Report" style="width:1200px">
    <asp:Repeater ID="TravelExpenseClaimData" runat="server">
        <HeaderTemplate>
            <tr class="title">
                <td class="value align-center w5">
                    <input type="checkbox" class="AllItems" style="width: 20px" />
                </td>
                <td class="value align-center w10">
                    申请人
                </td>
                <td class="value align-center w5">
                    部门
                </td>
                <td class="value align-center w5">
                    SAPNO
                </td>
                <td class="value align-center w10">
                    单据编号
                </td>
                <td class="value align-center w10">
                    创建时间
                </td>
                <td class="value align-center w10">
                    员工ID
                </td>
                <td class="value align-center w10">
                    员工名称
                </td>
                <td class="value align-center w15">
                    描述
                </td>
                <td class="value align-center w5">
                    金额
                </td>
                <td class="value align-center w5">
                    币种
                </td>
                <td class="value align-center w5">
                    状态
                </td>
                <td class="value align-center w10">
                    付款日
                </td>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="item">
                <td class="value align-center">
                    <asp:CheckBox ID="ckbItems" runat="server" CssClass="Items" ToolTip='<%#Eval("Title") %>' />
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblApplicant" runat="server" Text='<%#Eval("Applicant") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblSAPNO" runat="server" Text='<%#Eval("SAPNo") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <a href='http://cnashsptest.cnaidc.cn:91/WorkFlowCenter/Lists/TravelExpenseClaim/DispForm.aspx?ID=<%#Eval("ID")%>'
                        target="_blank">
                        <asp:Label ID="lblSubPRNo" runat="server" Text='<%#Eval("Title") %>'></asp:Label></a>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblCreated" runat="server" Text='<%#Eval("Created") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblVendorNo" runat="server" Text=<%#GetEmployeeIDAndName(Eval("Applicant").ToString()).Split(';')[0]%> ></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblVendorName" runat="server" Text=<%#GetEmployeeIDAndName(Eval("Applicant").ToString()).Split(';')[1]%> ></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblPaymentDesc" runat="server" Text='<%#Eval("Purpose") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%#Eval("TotalCost") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblCurrency" runat="server" Text="RMB"></asp:Label>
                </td>
                <td class="value align-center ItemStatus">
                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PaidDate") %>'></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<table class="ca-workflow-form-table Report" style="width:1200px">
    <asp:Repeater ID="CreditCardClaimData" runat="server">
        <HeaderTemplate>
            <tr class="title">
                <td class="value align-center w5">
                    <input type="checkbox" class="AllItems" style="width: 20px" />
                </td>
                <td class="value align-center w10">
                    申请人
                </td>
                <td class="value align-center w5">
                    部门
                </td>
                <td class="value align-center w5">
                    SAPNO
                </td>
                <td class="value align-center w10">
                    单据编号
                </td>
                <td class="value align-center w10">
                    创建时间
                </td>
                <td class="value align-center w10">
                    员工ID
                </td>
                <td class="value align-center w10">
                    员工名称
                </td>
                <td class="value align-center w15">
                    描述
                </td>
                <td class="value align-center w5">
                    金额
                </td>
                <td class="value align-center w5">
                    币种
                </td>
                <td class="value align-center w5">
                    状态
                </td>
                <td class="value align-center w10">
                    付款日
                </td>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="item">
                <td class="value align-center">
                     <asp:CheckBox ID="ckbItems" runat="server" CssClass="Items" ToolTip='<%#Eval("Title") %>' />
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblApplicant" runat="server" Text='<%#Eval("Applicant") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblSAPNO" runat="server" Text='<%#Eval("SAPNo")+";"+Eval("SAPUSDNo") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <a href='http://cnashsptest.cnaidc.cn:91/WorkFlowCenter/Lists/CreditCardClaimWorkflow/DispForm.aspx?ID=<%#Eval("ID")%>'
                        target="_blank">
                        <asp:Label ID="lblSubPRNo" runat="server" Text='<%#Eval("Title") %>'></asp:Label></a>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblCreated" runat="server" Text='<%#Eval("Created") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblVendorNo" runat="server" Text=<%#GetEmployeeIDAndName(Eval("Applicant").ToString()).Split(';')[0]%> ></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblVendorName" runat="server" Text=<%#GetEmployeeIDAndName(Eval("Applicant").ToString()).Split(';')[1]%> ></asp:Label>
                </td>
                <td class="value align-center">
                  <asp:Label ID="lblPaymentDesc" runat="server" Text='<%#Eval("ExpenseDescription") %>'></asp:Label>
                </td>
                <td class="value align-center">
                 <asp:Label ID="lblTotalAmount" runat="server" Text='<%#Eval("ApproveAmount") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblCurrency" runat="server" Text="RMB"></asp:Label>
                </td>
                <td class="value align-center ItemStatus">
                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PaidDate") %>'></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<table class="ca-workflow-form-table Report" style="width:1200px">
    <asp:Repeater ID="EmployeeExpenseClaimData" runat="server">
        <HeaderTemplate>
            <tr class="title">
                <td class="value align-center w5">
                    <input type="checkbox" class="AllItems" style="width: 20px" />
                </td>
                <td class="value align-center w10">
                    申请人
                </td>
                <td class="value align-center w5">
                    部门
                </td>
                <td class="value align-center w5">
                    SAPNO
                </td>
                <td class="value align-center w10">
                    单据编号
                </td>
                <td class="value align-center w10">
                    创建时间
                </td>
                <td class="value align-center w10">
                    员工ID
                </td>
                <td class="value align-center w10">
                    员工名称
                </td>
                <td class="value align-center w15">
                    描述
                </td>
                <td class="value align-center w5">
                    金额
                </td>
                <td class="value align-center w5">
                    币种
                </td>
                <td class="value align-center w5">
                    状态
                </td>
                <td class="value align-center w10">
                    付款日
                </td>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="item">
                <td class="value align-center">
                     <asp:CheckBox ID="ckbItems" runat="server" CssClass="Items" ToolTip='<%#Eval("Title") %>' />
                </td>
                <td class="value align-center">
                     <asp:Label ID="lblApplicant" runat="server" Text='<%#Eval("RequestedBy") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblSAPNO" runat="server" Text='<%#Eval("SAPNumber") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <a href='http://cnashsptest.cnaidc.cn:91/WorkFlowCenter/Lists/EmployeeExpenseClaimWorkflow/DispForm.aspx?ID=<%#Eval("ID")%>'
                        target="_blank">
                      <asp:Label ID="lblSubPRNo" runat="server" Text='<%#Eval("Title") %>'></asp:Label></a>
                </td>
                <td class="value align-center">
                     <asp:Label ID="lblCreated" runat="server" Text='<%#Eval("Created") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblVendorNo" runat="server" Text=<%#GetEmployeeIDAndName(Eval("RequestedBy").ToString()).Split(';')[0]%> ></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblVendorName" runat="server" Text=<%#GetEmployeeIDAndName(Eval("RequestedBy").ToString()).Split(';')[1]%> ></asp:Label>
                </td>
                <td class="value align-center">
                     <asp:Label ID="lblPaymentDesc" runat="server" Text='<%#Eval("ExpenseDescription") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%#Eval("TotalAmount") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblCurrency" runat="server" Text="RMB"></asp:Label>
                </td>
                 <td class="value align-center ItemStatus">
                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PaidDate") %>'></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<table class="ca-workflow-form-table Report" style="width:1200px">
    <asp:Repeater ID="ExpatriateBenefitClaimData" runat="server">
        <HeaderTemplate>
            <tr class="title">
                <td class="value align-center w5">
                    <input type="checkbox" class="AllItems" style="width: 20px" />
                </td>
                <td class="value align-center w10">
                    申请人
                </td>
                <td class="value align-center w5">
                    部门
                </td>
                <td class="value align-center w5">
                    SAPNO
                </td>
                <td class="value align-center w10">
                    单据编号
                </td>
                <td class="value align-center w10">
                    创建时间
                </td>
                <td class="value align-center w10">
                    员工ID
                </td>
                <td class="value align-center w10">
                    员工名称
                </td>
                <td class="value align-center w15">
                    描述
                </td>
                <td class="value align-center w5">
                    金额
                </td>
                <td class="value align-center w5">
                    币种
                </td>
                <td class="value align-center w5">
                    状态
                </td>
                <td class="value align-center w10">
                    付款日
                </td>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="item">
                <td class="value align-center">
                    <asp:CheckBox ID="ckbItems" runat="server" CssClass="Items" ToolTip='<%#Eval("Title") %>' />
                </td>
                <td class="value align-center">
                  <asp:Label ID="lblApplicant" runat="server" Text='<%#Eval("Applicant") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblSAPNO" runat="server" Text='<%#Eval("SAPNo") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <a href='http://cnashsptest.cnaidc.cn:91/WorkFlowCenter/Lists/ExpatriateBenefitClaimWorkflow/DispForm.aspx?ID=<%#Eval("ID")%>'
                        target="_blank">
                          <asp:Label ID="lblSubPRNo" runat="server" Text='<%#Eval("Title") %>'></asp:Label></a>
                </td>
                <td class="value align-center">
                      <asp:Label ID="lblCreated" runat="server" Text='<%#Eval("Created") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblVendorNo" runat="server" Text=<%#GetEmployeeIDAndName(Eval("Applicant").ToString()).Split(';')[0]%> ></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblVendorName" runat="server" Text=<%#GetEmployeeIDAndName(Eval("Applicant").ToString()).Split(';')[1]%> ></asp:Label>
                </td>
                <td class="value align-center">
                      <asp:Label ID="lblPaymentDesc" runat="server" Text='<%#Eval("ExpenseDescription") %>'></asp:Label>
                </td>
                <td class="value align-center">
                     <asp:Label ID="lblTotalAmount" runat="server" Text='<%#Eval("TotalAmount") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblCurrency" runat="server" Text="RMB"></asp:Label>
                </td>
                 <td class="value align-center ItemStatus">
                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PaidDate") %>'></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<table class="ca-workflow-form-table Report" style="width:1200px">
    <asp:Repeater ID="CashAdvanceReportData" runat="server">
        <HeaderTemplate>
            <tr class="title">
                <td class="value align-center w5">
                    <input type="checkbox" class="AllItems" style="width: 20px" />
                </td>
                <td class="value align-center w10">
                    申请人
                </td>
                <td class="value align-center w5">
                    部门
                </td>
                <td class="value align-center w5">
                    SAPNO
                </td>
                <td class="value align-center w10">
                    单据编号
                </td>
                <td class="value align-center w10">
                    创建时间
                </td>
                <td class="value align-center w10">
                    员工ID
                </td>
                <td class="value align-center w10">
                    员工名称
                </td>
                <td class="value align-center w15">
                    描述
                </td>
                <td class="value align-center w5">
                    金额
                </td>
                <td class="value align-center w5">
                    币种
                </td>
                <td class="value align-center w5">
                    状态
                </td>
                <td class="value align-center w10">
                    付款日
                </td>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="item">
                <td class="value align-center">
                    <asp:CheckBox ID="ckbItems" runat="server" CssClass="Items" ToolTip='<%#Eval("Title") %>' />
                </td>
                <td class="value align-center">
                     <asp:Label ID="lblApplicant" runat="server" Text='<%#Eval("Applicant") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblSAPNO" runat="server" Text='<%#Eval("SAPNumber") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <a href='http://cnashsptest.cnaidc.cn:91/WorkFlowCenter/Lists/CashAdvanceRequest/DispForm.aspx?ID=<%#Eval("ID")%>'
                        target="_blank">
                         <asp:Label ID="lblSubPRNo" runat="server" Text='<%#Eval("Title") %>'></asp:Label></a>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblCreated" runat="server" Text='<%#Eval("Created") %>'></asp:Label>
                </td>
                 <td class="value align-center">
                    <asp:Label ID="lblVendorNo" runat="server" Text=<%#GetEmployeeIDAndName(Eval("Applicant").ToString()).Split(';')[0]%> ></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblVendorName" runat="server" Text=<%#GetEmployeeIDAndName(Eval("Applicant").ToString()).Split(';')[1]%> ></asp:Label>
                </td>
                <td class="value align-center">
                  <asp:Label ID="lblPaymentDesc" runat="server" Text='<%#Eval("Purpose") %>'></asp:Label>
                </td>
                <td class="value align-center">
                 <asp:Label ID="lblTotalAmount" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblCurrency" runat="server" Text="RMB"></asp:Label>
                </td>
                 <td class="value align-center ItemStatus">
                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                </td>
                <td class="value align-center">
                    <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PaidDate") %>'></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<div id="div">
    <input id="hfpaidstatus" type="hidden" value="0" runat="server" />
    <input id="hfclaimstatus" type="hidden" value="0" runat="server" />
</div>
<script type="text/javascript" src="jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    $(function () {
        BindCheck();
        ShowOrHideTD();
        SetItemStatus();
    });

    function BindCheck() {
        var $AllItems = $("table.Report .AllItems");
        var $Items = $("table.Report span.Items input");
        $AllItems.live("click", function () {
            var result = $(this).attr("checked");
            $Items.attr("checked", result);
        });
    }

    function ShowOrHideTD() {
        var $dpQueryType = $("#ctl00_PlaceHolderMain_qr_dpQueryType");
        var $VendorNameText = $("table.condtion td.VendorNameText");
        var $VendorNOText = $("table.condtion td.VendorNOText");
        var $VendorNameInput = $("table.condtion td.VendorNameInput");
        var $VendorNOInput = $("table.condtion td.VendorNOInput");
        $dpQueryType.live("change", function () {
            if ($(this).val() != "Opex" &&
                $(this).val() != "Capex") {
                $VendorNameText.html("");
                $VendorNOText.html("");
                $VendorNameInput.find("input").hide();
                $VendorNOInput.find("input").hide();
                $VendorNOInput.find("span").hide();
                $("#div input[id$='hfclaimstatus']").val("1");
                $("#currency").hide();
            } else {
                $VendorNameText.html("收款用户名称");
                $VendorNOText.html("供应商编号");
                $VendorNameInput.find("input").show();
                $VendorNOInput.find("input").show();
                $VendorNOInput.find("span").show();
                $("#div input[id$='hfclaimstatus']").val("0");
                $("#currency").show();
            }
        });
        if ($("#div input[id$='hfclaimstatus']").val() == "1") {
            $VendorNameText.html("");
            $VendorNOText.html("");
            $VendorNameInput.find("input").hide();
            $VendorNOInput.find("input").hide();
            $VendorNOInput.find("span").hide();
        } else {
            $VendorNameText.html("收款用户名称");
            $VendorNOText.html("供应商编号");
            $VendorNameInput.find("input").show();
            $VendorNOInput.find("input").show();
            $VendorNOInput.find("span").show();
        }
    }

    function SetItemStatus() {
        var $Status = $("#ctl00_PlaceHolderMain_qr_dpStatus");
        var $ItemStatus = $(".Report .ItemStatus"); 
        if ($Status.val() != "") {
            $ItemStatus.html($Status.val());
        }
    }

    function CheckQuery() {
        $("#div input[id$='hfpaidstatus']").val("0");
        $(".condtion").find("input").css("borderBottom", "#999 1px solid");
        var msg = "";
        var result = true;
        var $txtPaidDate = $("input[id$='txtPaidDateDate']"); 
        if ($txtPaidDate.val() != "") {
            var txtStartCreate = new Date(Date.parse($txtPaidDate.val().replace(/-/g, "/")));
            if (txtStartCreate == "NaN") {
                msg += "请检查并输入正确的付款日！\n如：2012-12-12\n";
                $txtPaidDate.css("borderBottom", "1px solid red");
                result = false;
            }
            $("#div input[id$='hfpaidstatus']").val("1");
        }
        var $txtStartNO = $("input[id$='txtStartNO']");
        var $txtEndNO = $("input[id$='txtEndNO']");
        var $txtStartCreate = $("input[id$='txtStartCreateDate']");
        var $txtEndCreate = $("input[id$='txtEndCreateDate']");
        var $txtStartAmount = $("input[id$='txtStartAmount']");
        var $txtEndAmount = $("input[id$='txtEndAmount']");
        var $txtStartVendorNO = $("input[id$='txtStartVendorNO']");
        var $txtEndVendorNO = $("input[id$='txtEndVendorNO']");
        if ($txtStartNO.val() != "" && $txtEndNO.val() == ""
         || $txtStartNO.val() == "" && $txtEndNO.val() != "") {
            msg += "请输入起始单据编号！\n";
            $txtStartNO.css("borderBottom", "1px solid red");
            $txtEndNO.css("borderBottom", "1px solid red");
            result = false;
        }
        if ($txtStartNO.val().indexOf("_") == -1 && $txtEndNO.val().indexOf("_") > 0
        || $txtStartNO.val().indexOf("_") > 0 && $txtEndNO.val().indexOf("_") == -1) {
            msg += "请检查并输入正确的起始单据编号，必须保持2边风格一致！\n";
            $txtStartNO.css("borderBottom", "1px solid red");
            $txtEndNO.css("borderBottom", "1px solid red");
            result = false;
        }

        if ($txtStartNO.val() != "" && $txtEndNO.val() != "") {
            var $dpQueryType = $("#ctl00_PlaceHolderMain_qr_dpQueryType");
            switch ($dpQueryType.val()) {
                case "Opex":
                    if ($txtStartNO.val().toLowerCase().indexOf("pr") == -1 ||
                        $txtEndNO.val().toLowerCase().indexOf("pr") == -1) {
                        msg += "请检查并输入正确的Payment Request起始单据编号，必须保持2边风格一致！\n";
                        $txtStartNO.css("borderBottom", "1px solid red");
                        $txtEndNO.css("borderBottom", "1px solid red");
                        result = false;
                    }
                    break;
                case "Capex":
                    if ($txtStartNO.val().toLowerCase().indexOf("pr") == -1 ||
                        $txtEndNO.val().toLowerCase().indexOf("pr") == -1) {
                        msg += "请检查并输入正确的Payment Request起始单据编号，必须保持2边风格一致！\n";
                        $txtStartNO.css("borderBottom", "1px solid red");
                        $txtEndNO.css("borderBottom", "1px solid red");
                        result = false;
                    }
                    break;
                case "TravelExpenseClaim":
                    if ($txtStartNO.val().toLowerCase().indexOf("te") == -1 ||
                        $txtEndNO.val().toLowerCase().indexOf("te") == -1) {
                        msg += "请检查并输入正确的Travel Expense Claim起始单据编号，必须保持2边风格一致！\n";
                        $txtStartNO.css("borderBottom", "1px solid red");
                        $txtEndNO.css("borderBottom", "1px solid red");
                        result = false;
                    }
                    break;
                case "CreditCardClaim":
                    if ($txtStartNO.val().toLowerCase().indexOf("ccc") == -1 ||
                        $txtEndNO.val().toLowerCase().indexOf("ccc") == -1) {
                        msg += "请检查并输入正确的Credit Card Claim起始单据编号，必须保持2边风格一致！\n";
                        $txtStartNO.css("borderBottom", "1px solid red");
                        $txtEndNO.css("borderBottom", "1px solid red");
                        result = false;
                    }
                    break;
                case "EmployeeExpenseClaim":
                    if ($txtStartNO.val().toLowerCase().indexOf("eec") == -1 ||
                        $txtEndNO.val().toLowerCase().indexOf("eec") == -1) {
                        msg += "请检查并输入正确的Employee Expense Claim起始单据编号，必须保持2边风格一致！\n";
                        $txtStartNO.css("borderBottom", "1px solid red");
                        $txtEndNO.css("borderBottom", "1px solid red");
                        result = false;
                    }
                    break;
                case "ExpatriateBenefitClaim":
                    if ($txtStartNO.val().toLowerCase().indexOf("ebc") == -1 ||
                        $txtEndNO.val().toLowerCase().indexOf("ebc") == -1) {
                        msg += "请检查并输入正确的Expatriate Benefit Claimm起始单据编号，必须保持2边风格一致！\n";
                        $txtStartNO.css("borderBottom", "1px solid red");
                        $txtEndNO.css("borderBottom", "1px solid red");
                        result = false;
                    }
                    break;
                case "CashAdvanceRequest":
                    if ($txtStartNO.val().toLowerCase().indexOf("ca_") == -1 ||
                        $txtEndNO.val().toLowerCase().indexOf("ca_") == -1) {
                        msg += "请检查并输入正确的Cash Advance Request起始单据编号，必须保持2边风格一致！\n";
                        $txtStartNO.css("borderBottom", "1px solid red");
                        $txtEndNO.css("borderBottom", "1px solid red");
                        result = false;
                    }
                    break;
            }
        }

        if ($txtStartCreate.val() == "" || $txtEndCreate.val() == "") {
            msg += "请输入起始制单日！\n";
            $txtStartCreate.css("borderBottom", "1px solid red");
            $txtEndCreate.css("borderBottom", "1px solid red");
            result = false;
        }
        if ($txtStartAmount.val() != "" && $txtEndAmount.val() == ""
         || $txtStartAmount.val() == "" && $txtEndAmount.val() != "") {
            msg += "请输入起始金额！\n";
            $txtStartAmount.css("borderBottom", "1px solid red");
            $txtEndAmount.css("borderBottom", "1px solid red");
            result = false;
        }
        if ($txtStartVendorNO.val() != "" && $txtEndVendorNO.val() == ""
         || $txtStartVendorNO.val() == "" && $txtEndVendorNO.val() != "") {
            msg += "请输入起始供应商编号！\n";
            $txtStartVendorNO.css("borderBottom", "1px solid red");
            $txtEndVendorNO.css("borderBottom", "1px solid red");
            result = false;
        }
        if ($txtStartNO.val() != "" && $txtEndNO.val() != "") {
            var txtStartNO = $.trim($txtStartNO.val()).toLowerCase().replace("te", "00")
                                                      .replace("ccc", "00")
                                                      .replace("eec", "00")
                                                      .replace("ebc", "00")
                                                      .replace("ca", "00")
                                                      .replace("pr", "00")
                                                      .replace("_", "");
            var txtEndNO = $.trim($txtEndNO.val()).toLowerCase().replace("te", "00")
                                                      .replace("ccc", "00")
                                                      .replace("eec", "00")
                                                      .replace("ebc", "00")
                                                      .replace("ca", "00")
                                                      .replace("pr", "00")
                                                      .replace("_", "");
            if ((isNaN(txtStartNO) || isNaN(txtEndNO)) || txtEndNO < txtStartNO) {
                msg += "请检查并输入正确的起始单据编号！\n";
                $txtStartNO.css("borderBottom", "1px solid red");
                $txtEndNO.css("borderBottom", "1px solid red");
                result = false;
            }
        }
        if ($txtStartCreate.val() != "" && $txtEndCreate.val() != "") {
            var txtStartCreate = new Date(Date.parse($txtStartCreate.val().replace(/-/g, "/")));
            var txtEndCreate = new Date(Date.parse($txtEndCreate.val().replace(/-/g, "/")));
            if (txtStartCreate == "NaN" || txtEndCreate == "NaN") {
                msg += "请检查并输入正确的起始制单日！\n如：2012-12-12 ";
                $txtStartCreate.css("borderBottom", "1px solid red");
                $txtEndCreate.css("borderBottom", "1px solid red");
                result = false;
            } else {
                if (txtEndCreate < txtStartCreate ||
                    txtEndCreate.getYear() - txtStartCreate.getYear() > 1) {
                    msg += "请检查并输入正确的起始制单日！\n起始制单日的年月不应该超过一年以上！\n";
                    $txtStartCreate.css("borderBottom", "1px solid red");
                    $txtEndCreate.css("borderBottom", "1px solid red");
                    result = false;
                } else {
                    if (txtEndCreate.getYear() - txtStartCreate.getYear() == 1) {
                        if (txtEndCreate.getMonth() - txtStartCreate.getMonth() > 0) {
                            msg += "请检查并输入正确的起始制单日！\n起始制单日的年月不应该超过一年以上！\n";
                            $txtStartCreate.css("borderBottom", "1px solid red");
                            $txtEndCreate.css("borderBottom", "1px solid red");
                            result = false;
                        }
                    }
                }
            }
        }
        if ($txtStartAmount.val() != "" && $txtEndAmount.val() != "") {
            if (isNaN($txtStartAmount.val()) || isNaN($txtEndAmount.val())) {
                msg += "请检查并输入正确的起始金额！\n";
                $txtStartAmount.css("borderBottom", "1px solid red");
                $txtEndAmount.css("borderBottom", "1px solid red");
                result = false;
            } else {
                if (parseFloat($txtEndAmount.val()) < parseFloat($txtStartAmount.val())) {
                    msg += "请检查并输入正确的起始金额！\n";
                    $txtStartAmount.css("borderBottom", "1px solid red");
                    $txtEndAmount.css("borderBottom", "1px solid red");
                    result = false;
                }
            }
        }
        if ($txtStartVendorNO.val() != "" && $txtEndVendorNO.val() != "") {
            if (isNaN($txtStartVendorNO.val()) || isNaN($txtEndVendorNO.val())) {
                msg += "请检查并输入正确的起始供应商编号！\n";
                $txtStartVendorNO.css("borderBottom", "1px solid red");
                $txtEndVendorNO.css("borderBottom", "1px solid red");
                result = false;
            } else {
                if (parseFloat($txtEndVendorNO.val()) < parseFloat($txtStartVendorNO.val())) {
                    msg += "请检查并输入正确的起始供应商编号！\n";
                    $txtStartVendorNO.css("borderBottom", "1px solid red");
                    $txtEndVendorNO.css("borderBottom", "1px solid red");
                    result = false;
                }
            }
        }
        if (msg != "") {
            alert(msg);
        }
        return result;
    }

    function CheckExport() {
        var $Items = $("table.Report span.Items input");
        var result = false;
        $Items.each(function () {
            if ($(this).attr("checked")) {
                result = true;
            }
        });
        if (!result) {
            alert("请先选择要导出Excel文件的项！");
        }
        return result;
    }
</script>
