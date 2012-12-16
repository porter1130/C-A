<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataForm.ascx.cs" Inherits="CA.WorkFlow.UI.PaymentRequest.DataForm" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .query_left_div
    {
        width: 80%;
        float: left;
        margin: 0 auto;
    }
    .query_right_div
    {
        float: right;
        width: 20%;
        padding-top: 3px;
        margin: 0 auto;
        font-weight: bold;
        text-align: center;
    }
    .QueryVendorInfo
    {
        color: #29a8df;
        font-size: 16px;
        cursor: pointer;
    }
    .CloseQuery
    {
        color: #000000;
        font-size: 12px;
        cursor: pointer;
    }
    .V_ContentDiv
    {
        padding: 0px;
        margin: 0px;
        position: relative;
    }
    #V_ErrorDiv
    {
        margin: 0 auto;
        width: 105%;
        height: 550px;
        position: absolute;
        left: 0px;
        top: 0px;
        display: none;
        border: #aaaaaa 1px solid;
        background-color: White;
        padding: 4px 6px 6px 4px;
        font-size: 12px;
        z-index: 10001;
        color: #000000;
        font-family: "Segoe UI" , Arial, "Sans-Serif";
    }
    .V_ErrorDiv_Scroll
    {
        overflow-x: hidden;
        overflow-y: scroll;
        scrollbar-face-color: #e4e4e5;
    }
    #V_ErrorMsg
    {
        background-color: White;
        float: left;
        line-height: 22px;
        padding: 4px;
        width: 99%;
        height: 99%;
    }
    #V_title
    {
        float: left;
        width: 100%;
        border-bottom: 1px solid #aaaaaa;
        font-weight: bold;
        color: #000000;
        padding-bottom: 5px;
    }
    #V_left
    {
        float: left;
        color: #000000;
        padding: 5px;
        padding-top: 3px;
    }
    #V_right
    {
        color: #000000;
        float: right;
        padding: 5px;
        padding-top: 3px;
        display: block;
        padding-right: 10px;
    }
    #V_msg
    {
        font-weight: bold;
        color: #000000;
        float: left;
        width: 98%;
        padding: 10px;
        padding-right:2px;
        margin-top: 20px;
        line-height: 20px;
        font-family: "Segoe UI" , Arial, "Sans-Serif";
        font-size: 12px;
        margin-bottom: 10px;
        border:#9dabb6 2px solid;
    }
    #V_alert
    {
        float: left;
        width: 100%;
        font-weight: bold;
        cursor: pointer;
        text-align: center;
        padding: 20px 0px 20px 0px;
    }
    #V_alertleft
    {
        color: #000000;
        float: left;
        width: 50%;
    }
    #V_alertright
    {
        color: red;
        float: right;
        width: 50%;
    }
    #V_bgDiv
    {
        position: absolute;
        top: 0px;
        left: 0px;
        right: 0px;
        bottom: 0px;
        display: none;
        background-color: White;
        filter: Alpha(opacity=30);
        z-index: 10;
    }
    #V1_bgDiv
    {
        position: absolute;
        top: 0px;
        left: 0px;
        right: 0px;
        bottom: 0px;
        display: none;
        background-color: White;
        filter: Alpha(opacity=30);
        z-index: 10;
    }
    
    .summarytypetable
    {
        text-align: center;
        width: 100%;
        border: none;
    }
    .summarytype
    {
        display: none;
        border: none;
    }
    .summaryTR
    {
        display: none;
    }
    .label1
    {
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        border-top: 1px solid #CCCCCC;
        padding: 5px;
    }
    .label1 h3
    {
        text-align: center;
        padding: 5px;
    }
    .ExpenseType1
    {
        position: absolute;
        left: 0px !important;
        left: -95px;
        top: -15px;
        z-index: 1000;
        cursor: pointer;
    }
    .CostCenter
    {
        position: absolute;
        left: 0px !important;
        left: -95px;
        top: -15px;
        cursor: pointer;
    }
    .CostCenterTD
    {
        width: 300px;
    }
    .ExpenseTypeTD
    {
        width: 280px;
    }
    .wrapdiv
    {
        padding: 2px;
        border: 1px solid red;
    }
    .amount
    {
        color: Red;
    }
    *
    {
        line-height: 14px;
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
    
    .h36
    {
        height: 40px;
    }
    
    .h40
    {
        height: 40px;
    }
    
    .h50
    {
        height: 50px;
    }
    
    .col1, .col2, .col3, .col4
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
    .w490
    {
        width: 490px;
    }
    .label11
    {
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        border-top: 1px solid #CCCCCC;
        padding: 5px;
    }
</style>
<style type="text/css">
    
    #QueryVendorCode
    {
        
        margin-left: 10px;
        cursor: pointer;
    }
    #Vendor_Info
    {
        width: 100%;
        float: left;
        color: #000000;
        cursor: pointer;
        display: none;
        font-weight: bold;
       padding:0px;
       margin-top:10px;
    }
    #errordiv
    {
        width: 100%;
        float: left;
        color: red;
        font-weight: bold;
        cursor: pointer;
        display: none;
        text-align: center;
    }
    .loading
    {
        width: 100%;
        margin: 0 auto;
        text-align: center;
        padding-top: 40px;
        display: none;
    }
    .paging
    {
        width: 100%;
        margin: 0 auto;
        text-align: center;
        padding-top: 20px;
        display: none;
        padding-bottom: 20px;
    }
    .paging span
    {
        padding: 5px;
        padding-right:15px;
    }
    .Vendor_Info ul li
    {
        list-style-type: none;
        white-space: normal;
    }
    .verdor_ul li
    {
        background-image: url(/_layouts/images/viewheadergrad.gif);
        background-color: #f2f2f2;
        background-repeat: repeat-x;
        color: #b2b2b2;
        border-bottom: #cccccc 1px solid;
        border-right: #cccccc 1px solid;
        padding: 10px 0px 10px 3px;
        font-weight: lighter;
    }
    li
    {
        line-height: 14px;
        padding: 10px 0px 10px 3px;
        border-bottom: #cccccc 1px solid;
        border-right: #cccccc 1px solid;
    }
    .borderleft
    {
        border-left: #cccccc 1px solid;
        }
    
    .Select
    {
        width: 6%;
    }
    
    .VendorCode
    {
        width: 10%;
    }
    .VendorName
    {
        width: 35%;
    }
    .BankName
    {
        width: 24%;
    }
    .BankAC
    {
        width: 22%;
    }
    .SelectVendor
    {
        color: #29a8df;
        font-weight: bold;
    }
    .Select div
    {
        display: none;
    }
</style>
<div class="ContentDiv">
    <asp:Button ID="btnPeopleInfo" runat="server" OnClick="btnPeopleInfo_Click" CausesValidation="False"
        CssClass="hidden" />
    <table class="ca-workflow-form-table form-table3 out_table" id="out_table">
        <tr>
            <td class="value align-center" colspan="4">
                Payment Request付款申请单
            </td>
        </tr>
        <tr id="ChooseEmployee" runat="server">
            <td class="label">
                Choose Employee<br />
                选择员工
            </td>
            <td class="label" colspan="3">
                <cc1:capeoplefinder id="cpfUser" runat="server" allowtypein="true" multiselect="false"
                    cssclass="ca-people-finder" width="210" />
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
            <td class="label col1" style="border-bottom: 2px solid #9dabb6">
                供应商编号<br />
                Vendor Code
            </td>
            <td class="label col2" style="border-bottom: 2px solid #9dabb6">
                <asp:TextBox ID="txtVenderCode" runat="server" CssClass="txtVenderCode" Width="100px"></asp:TextBox>
                <asp:HiddenField ID="hfVendorInfoList" runat="server" Value="" />
                <img id="QueryVendorCode" src="../../../CAResources/themeCA/images/PRQuery.png" alt="Query And Select Vendor Info"
                    align="absmiddle" />
                <div id="V_ErrorDiv">
                    <div id="V_ErrorMsg">
                        <div id="V_title">
                            <div id="V_left">
                                Query And Select Vendor Info</div>
                            <div id="V_right">
                                <a class="CloseQuery" title="Close">X</a>
                            </div>
                        </div>
                        <div id="V_msg">
                            <div>
                                <div class="query_left_div">
                                    Vendor Code：<input type="text" class="V_Code" style="width: 120px; margin-right: 10px;" />
                                    Vendor Name：<input type="text" class="V_Name" style="width: 170px;" /></div>
                                <div class="query_right_div">
                                    <a class="QueryVendorInfo" title="Query And Select Vendor Info">Query</a></div>
                            </div>
                        </div>
                        <div id="Vendor_Info">
                            <div class="loading">
                                <img src="../../../CAResources/themeCA/images/loading.gif" alt="" align="absmiddle" />
                            </div>
                            <ul class="verdor_ul">
                                <li class="VendorCode borderleft">Vendor Code</li>
                                <li class="VendorName">Vendor Name</li>
                                <li class="BankName">Bank Name</li>
                                <li class="BankAC">Bank A/C</li>
                                <li class="Select">Select</li>
                            </ul>
                            <%-- <ul class="v_ul">
                                <li class="VendorCode">8008</li>
                                <li class="VendorName">An Shi Electromechanical engineerin</li>
                                <li class="BankName">上海银行</li>
                                <li class="BankAC">310015588000500031</li>
                                <li class="Select"><a class="SelectVendor">Select</a></li>
                            </ul>
                            <ul class="v_ul">
                                <li class="VendorCode">8008</li>
                                <li class="VendorName">An Shi Electromechanical engineerin</li>
                                <li class="BankName">上海银行</li>
                                <li class="BankAC">310015588000500031</li>
                               <li class="Select"><a class="SelectVendor">Select</a></li>
                            </ul>
                            <ul class="v_ul">
                                <li class="VendorCode">8008</li>
                                <li class="VendorName">An Shi Electromechanical engineerin</li>
                                <li class="BankName">上海银行</li>
                                <li class="BankAC">310015588000500031</li>
                                <li class="Select"><a class="SelectVendor">Select</a>
                                <div class="VendorCity"></div>
                                <div class="VendorCountry"></div>
                                <div class="BankCountry"></div>
                                <div class="BankKey"></div>
                                </li>
                            </ul>--%>
                            <div class="paging">
                                <span class="up">
                                    <img src="../../../CAResources/themeCA/images/prup.gif" alt="" align="absmiddle" /></span>
                                <%--<span class="start">1</span><span class="start">10</span>--%>
                                <span class="down">
                                    <img src="../../../CAResources/themeCA/images/prdown1.gif" alt="" align="absmiddle" /></span>
                            </div>
                        </div>
                        <div id="errordiv">
                            No Match Items!
                        </div>
                    </div>
                </div>
                <div id="V_bgDiv">
                </div>
            </td>
            <td class="label col3" style="border-bottom: 2px solid #9dabb6">
                供应商名称<br />
                Vendor Name
            </td>
            <td class="label col4" style="border-bottom: 2px solid #9dabb6">
                <asp:TextBox ID="txtVenderName" runat="server" CssClass="txtVenderName"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="4">
                <div class="ContentPadding">
                    若是一次性的供应商，请填写以下信息<br />
                    If new one-off vendor,please enter the following infomation</div>
                <asp:HiddenField ID="hfgr" runat="server" Value="0" />
            </td>
        </tr>
        <tr>
            <td class="label">
                供应商所在城市<br />
                Vendor City
            </td>
            <td class="label">
                <asp:TextBox ID="txtVendorCity" runat="server" CssClass="txtVendorCity"></asp:TextBox>
            </td>
            <td class="label">
                供应商所在国家<br />
                Vendor Country
            </td>
            <td class="label">
                <div class="hidden">
                    <asp:TextBox ID="txtVendorCountry" runat="server" Text="CN" CssClass="txtVendorCountry"
                        ToolTip="只能英文字母组成"></asp:TextBox>
                </div>
                <asp:DropDownList ID="ddlVendorCountry" runat="server" CssClass="ddlVendorCountry"
                    Width="60px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="label">
                银行名称<br />
                Bank Name
            </td>
            <td class="label">
                <asp:TextBox ID="txtBankName" runat="server" CssClass="txtBankName"></asp:TextBox>
            </td>
            <td class="label">
                银行账号<br />
                Bank A/C
            </td>
            <td class="label">
                <asp:TextBox ID="txtBankAC" runat="server" CssClass="txtBankAC" ToolTip="银行账号只能[0-9]数字组成"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="label" style="border-bottom: 2px solid #9dabb6">
                银行所在国家<br />
                Bank Country
            </td>
            <td class="label" style="border-bottom: 2px solid #9dabb6">
                <div class="hidden">
                    <asp:TextBox ID="txtBankCity" runat="server" CssClass="txtBankCity" Text="CN"></asp:TextBox>
                    <asp:HiddenField ID="hfCountryCurrency" runat="server" Value="" />
                </div>
                <asp:DropDownList ID="ddlBankCountry" runat="server" CssClass="ddlBankCountry" Width="60px">
                </asp:DropDownList>
            </td>
            <td class="label" style="border-bottom: 2px solid #9dabb6">
                银行代码<br />
                Bank Key
            </td>
            <td class="label" style="border-bottom: 2px solid #9dabb6">
                <asp:TextBox ID="txtSwiftCode" runat="server" CssClass="txtSwiftCode"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="label">
                单据描述<br />
                Payment Descriptions
            </td>
            <td class="label" colspan="3">
                <asp:TextBox ID="txtPaymentDesc" runat="server" TextMode="MultiLine" Width="490px"
                    CssClass="txtPaymentDesc" Height="36px"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none">
            <td class="label col1">
                成本中心<br />
                Cost Center
            </td>
            <td class="label" colspan="3">
                <span class="spanCostCenter">
                    <asp:DropDownList ID="dropCostCenter" runat="server" CssClass="dropCostCenter" Width="490px"
                        Font-Bold="False">
                    </asp:DropDownList>
                </span>
            </td>
        </tr>
        <tr class="FromPOTR">
            <td colspan="4" style="height: 18px;">
            </td>
        </tr>
        <tr class="FromPOTR">
            <td class="label" colspan="4" style="padding: 0px; cursor: pointer">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="ca-workflow-form-table ca-workflow-form-table1" id="CostCenterTable"
                            style="width: 100%; border: none">
                            <tr style="font-weight: bold; text-align: center">
                                <td class="label1 w5">
                                    <asp:ImageButton runat="server" ID="btnAddItem" ToolTip="Click to add the information."
                                        ImageUrl="../images/pixelicious_001.png" OnClick="btnAddItem_Click" Width="18"
                                        CssClass="img-button" />
                                    <asp:HiddenField ID="hfGLAccount" runat="server" Value="" />
                                    <asp:HiddenField ID="FAStatus1" runat="server" Value="0" />
                                </td>
                                <td class="label1 w15 FA">
                                    Asset No
                                </td>
                                <td class="label1 w25">
                                    <asp:Label ID="lblExpenseType" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="label1 w15">
                                    <asp:Label ID="lblTDStatus" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="label1 w25">
                                    Cost Center
                                </td>
                                <td class="label1 w15">
                                    Amount
                                </td>
                            </tr>
                            <asp:Repeater ID="rptItem" runat="server" OnItemCommand="rptItem_ItemCommand" OnItemDataBound="rptItem_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td class="label1">
                                            <asp:ImageButton ID="btnDeleteItem" ToolTip="Remove this information." CommandName="delete"
                                                runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                                        </td>
                                        <td class="label1 FA">
                                            <asp:TextBox ID="txtFANO" runat="server" CssClass="FANO"></asp:TextBox>
                                        </td>
                                        <td class="label1">
                                            <div style="position: relative; z-index: 1000; background-color: White;">
                                                <div class="ExpenseType1">
                                                    <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="ExpenseType" onchange="ChangeGLAccount(this)">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </td>
                                        <td class="label1">
                                            <asp:TextBox ID="lblGLAccount" runat="server" CssClass="GLAccount"></asp:TextBox>
                                        </td>
                                        <td class="label1">
                                            <div style="position: relative; background-color: White; z-index: 500">
                                                <div class="CostCenter">
                                                    <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="cc">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </td>
                                        <td class="label1">
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="Amount"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="height: 15px;">
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="label col1 h40">
                分期付款<br />
                Installment Payment
            </td>
            <td class="label" colspan="3">
                <div class="RadioWidth">
                    <asp:RadioButtonList ID="radioInstallment" runat="server" RepeatDirection="Horizontal"
                        CssClass="ms-RadioText  radioInstallment">
                        <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem Value="No" Selected="True"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </td>
        </tr>
        <tr class="CurrencyTR NoFromPOTR" style="display: none; font-weight: bold">
            <td class="label" colspan="2" align="center" style="border-bottom: none">
                Currency
            </td>
            <td class="label" colspan="2" align="center" style="border-bottom: none">
                <asp:Label ID="lblCurrency" runat="server" Text="" CssClass="lblCurrency"></asp:Label>
            </td>
        </tr>
        <tr class="summaryTR NoFromPOTR">
            <td class="label" colspan="4" style="padding: 0px 0px 10px 0px; cursor: pointer">
                <table class="ca-workflow-form-table ca-workflow-form-table1 summarytypetable">
                    <tr>
                        <td colspan="3" class="label1">
                            <h3>
                                Expense Summary<br />
                                <asp:HiddenField ID="hidSummaryExpenseType1" runat="server" Value="" />
                            </h3>
                        </td>
                    </tr>
                    <tr style="font-weight: bold">
                        <td class="label1 w50">
                            <asp:Label ID="lblSummaryType" runat="server" Text=""></asp:Label>
                        </td>
                        <td class="label1 w30">
                            CostCenter
                        </td>
                        <td class="label1 w20">
                            Amount
                        </td>
                    </tr>
                    <tr class="summarytype">
                        <td colspan="3">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="trNoInstallment" style="height: 50px;">
            <td class="label col1" style="padding: 0 0 0 5px; margin: 0;">
                总金额<br />
                TotalAmount
            </td>
            <td class="label" colspan="3" style="padding: 0 0 0 5px; margin: 0;">
                <asp:TextBox ID="txtTotalAmount1" CssClass="txtTotalAmount1" runat="server" Width="490px"
                    Style="cursor: pointer;"></asp:TextBox>
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
                        <td style="width: 50%;">
                            <asp:TextBox ID="txtTotalAmount" CssClass="txtTotalAmount" runat="server" Width="55px"
                                Style="cursor: pointer;"></asp:TextBox>
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
                        <td style="width: 50%;">
                            <asp:TextBox ID="txtPaidThisTime" runat="server" CssClass="txtPaidThisTime" Width="55px"
                                Style="cursor: pointer"></asp:TextBox>
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
                Contract/PO with paid amount signed
            </td>
            <td class="label col2">
                <asp:RadioButtonList ID="radioContractPO" runat="server" RepeatDirection="Horizontal"
                    CssClass="ms-RadioText  radioContractPO">
                    <asp:ListItem> Yes</asp:ListItem>
                    <asp:ListItem Selected="True"> No</asp:ListItem>
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
                                    <asp:TextBox ID="txtContractPO" CssClass="txtContractPO" runat="server" Width="220px"></asp:TextBox></div>
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
                    CssClass="ms-RadioText  radioSystemPO">
                    <asp:ListItem> Yes</asp:ListItem>
                    <asp:ListItem Selected="True"> No</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="label" colspan="2">
                <div class="divSystemPO" style="vertical-align: middle;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="30%">
                                <div id="divLabSystemPO" style="float: left;">
                                    系统订单编号<br />
                                    System PO No
                                </div>
                            </td>
                            <td width="70%">
                                <div id="divTxtSystemPO">
                                    <asp:TextBox ID="txtSystemPO" CssClass="txtSystemPO" runat="server" Width="220px"></asp:TextBox></div>
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
                    CssClass="ms-RadioText radioContractGR">
                    <asp:ListItem> Yes </asp:ListItem>
                    <asp:ListItem Selected="True"> No</asp:ListItem>
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
                                    System GR done
                                </div>
                            </td>
                            <td width="70%">
                                <div id="div2">
                                    <asp:RadioButtonList ID="radioSystemGR" runat="server" CssClass="ms-RadioText  radioSystemGR"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem> Yes</asp:ListItem>
                                        <asp:ListItem Selected="True"> No</asp:ListItem>
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
                    Width="490px" Height="36px"></asp:TextBox>
            </td>
        </tr>
        <tr class="h50">
            <td class="label col1">
                附原发票<br />
                Original Invoice Attached
            </td>
            <td class="label" colspan="3">
                <asp:RadioButtonList ID="radioInvoice" runat="server" RepeatDirection="Horizontal"
                    CssClass="ms-RadioText  radioInvoice">
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
                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Height="36px" Width="490px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="label col1">
                附件 Attachment
            </td>
            <td class="label" colspan="3">
                <QFL:FormAttachments runat="server" ID="attacthment" ControlMode="Edit">
                </QFL:FormAttachments>
            </td>
        </tr>
        <tr>
            <td class="label" colspan="4">
            </td>
        </tr>
    </table>
</div>
<asp:HiddenField ID="hfpostatus" runat="server" Value="" />
<asp:HiddenField ID="hfFromPO" runat="server" Value="" />
<asp:HiddenField ID="hf_D_RequestType" runat="server" Value="" />
<script type="text/javascript" language="javascript">
    (function ($) {
        $.fn.extend({
            SetBgColorWhite: function () {
                return $(this).css({ "background-color": "white" });
            },
            SetBgColorGrey: function () {
                return $(this).css({ "background-color": "#F1F0ED" });
            },
            Hidden: function () {
                return $(this).css("display", "none");
            },
            Show: function () {
                return $(this).css("display", "");
            },
            Disable: function () {
                return $(obj).attr("disable", "true");
            },
            UnDisable: function () {
                return $(obj).attr("disable", "false");
            },
            SetBorderWarn: function () {
                return $(this).css("border", "1px solid red");
            },
            ClearBorderWarn: function () {
                return $(this).css("border", "").css("border-bottom", "#999 1px solid");
            },
            ClearBorderWarn1: function () {
                return $(this).css("border", "#999 1px solid");
            },
            ClearBorderWarn2: function () {
                return $(this).css("border", "");
            },
            SetRadioChecked: function (value) {
                return $(this).each(function () {
                    var $radioObj = $(this);
                    $radioObj.attr("checked", ($.trim($radioObj.val()) == value ? true : false));
                });
            },
            GetRadioChecked: function () {
                return $("#" + $(this).parents("table").attr("id") + " input[type=radio]:checked");
            },
            IsEmpty: function (isSetWar) {
                var $obj = $(this);
                if ($.trim($obj.val()) == "" || $obj.val() == "undefined") {
                    if (isSetWar) {
                        $obj.SetBorderWarn();
                    }
                    return true;
                }
                return false;
            },
            IsZeroOrStr: function (isSetWar) {
                var $obj = $(this);
                if ($.trim($obj.val()) == "0" || $obj.IsAmount(false) == false) {
                    if (isSetWar) {
                        $obj.SetBorderWarn();
                    }
                    return true;
                }
                return false;
            },
            IsAmount: function (isSetWar) {
                var $obj = $(this);
                var reg = /^([1-9][0-9]*)(\.*)(\d*)$/;
                if (reg.test($obj.val()) == true) {
                    if (isSetWar) {
                        $obj.SetBorderWarn();
                    }
                    return true;
                }
                return false;
            },
            IsNumber: function (isSetWar) {
                 var $obj = $(this);
                 var reg = /^\d\d*$/;
                if (reg.test($obj.val()) == true) {
                    if (isSetWar) {
                        $obj.SetBorderWarn();
                    }
                    return true;
                }
                return false;
            }
        });
    })(jQuery);

    //从后台获取数据值 
    var isFromPO = false;
    var paidIndex = <%= PaidIndex.ToString() %>;
    if ("<%= IsFromPO.ToString() %>" == "True") {
        isFromPO = true;
    }

    var $Attachments = $("#part1");
    var $TotalAmount = $(".txtTotalAmount");
    var $TotalAmount1 = $(".txtTotalAmount1");
    var $TotalAmount2 = $(".txtTotalAmount2");
    var $PaidThisTime = $(".txtPaidThisTime");
    var $Blance = $(".txtBlance");
    var $PaidBefore = $(".txtPaidBefore");
    var $ContractPO = $(".txtContractPO");
    var $SystemPO = $(".txtSystemPO");
    var $PaidThisTime = $(".txtPaidThisTime");
    var $PaymentReason = $(".txtPaymentReason");
    var $BankName = $(".txtBankName");
    var $BankAC = $(".txtBankAC");
    var $SwiftCode = $(".txtSwiftCode");
    var $PaymentDesc = $(".txtPaymentDesc");
    var $VenderCode = $(".txtVenderCode");
    var $VenderName = $(".txtVenderName");
    var $BankCity = $(".txtBankCity");
    var $VendorCity = $(".txtVendorCity");
    var $VendorCountry = $(".txtVendorCountry");
    var $CostCenter = $(".dropCostCenter");
    var $SpanCostCenter = $(".spanCostCenter");
    var $RadioInstallment = $(".radioInstallment input[type=radio]");
    var $RadioSystemPO = $(".radioSystemPO input[type=radio]");
    var $RadioSystemGR = $(".radioSystemGR input[type=radio]");
    var $RadioContractPO = $(".radioContractPO input[type=radio]");
    var $RadioContractGR = $(".radioContractGR input[type=radio]");
    var $RadioInvoice = $(".radioInvoice input[type=radio]");

    //当用户SAVE、SUBMIT时，设置页面上控件为可编辑状态
    function SetControlUnDisable() {
        $RadioInstallment.attr("disabled", false);
        $RadioSystemPO.attr("disabled", false);
        $RadioSystemGR.attr("disabled", false);
        $RadioContractPO.attr("disabled", false);
        $RadioContractGR.attr("disabled", false);
        $PaidThisTime.attr("readonly", false);
        $SystemPO.attr("readonly", false);
        $ContractPO.attr("readonly", false);
        $TotalAmount.attr("readonly", false);
        $TotalAmount1.attr("readonly", false);
    }

    //设置分期付款信息
    function SetInstallmentInfo(){
        if ($.trim($RadioInstallment.val()) == "Yes") {
            var percent = 100;
            var $PaymentPercent = $(".PaymentItem .PaymentPercent");
            $PaymentPercent.each(function (index){
                if (index == 0) {
                    percent = $(this).val();
                }
            });
            var totalAmount = $TotalAmount.val();
            var paidThisTime = Math.round(totalAmount * percent) / 100;
            var blance = Math.round((totalAmount - paidThisTime) * 100) / 100;
           
            var $PaymentAmount=$(".PaymentItem .PaymentAmount");
            var ptt=0;
            $PaymentAmount.each(function (i){
                if (i == 0) {
                    ptt = $(this).val();
                }
            });
            ptt=Math.round(ptt * Math.pow(10, 2)) / Math.pow(10, 2);
           
            var blanceAmount=totalAmount-ptt;
            blanceAmount=Math.round(blanceAmount * Math.pow(10, 2)) / Math.pow(10, 2);


            $PaidThisTime.val(ptt);
            $Blance.val(blanceAmount).attr("readonly", "false");
            $PaidBefore.val("0").attr("readonly", "false");
        }
    }

    function CheckCityInfo(){
        var $txtVenderCode=$("input[id$='txtVenderCode']");

        var $txtVenderName=$("input[id$='txtVenderName']");
        var $txtVendorCity=$("input[id$='txtVendorCity']");
        var $txtVendorCountry=$("input[id$='txtVendorCountry']");
        var $txtBankName=$("input[id$='txtBankName']");
        var $txtBankAC=$("input[id$='txtBankAC']");
        var $txtBankCity=$("input[id$='txtBankCity']");
        var $txtSwiftCode=$("input[id$='txtSwiftCode']");
        $("div.wrapdiv").removeClass("wrapdiv");
        var msg="";
        var result=true;
        if($.trim($txtVenderCode.val())==""){
            if($.trim($txtVenderName.val())==""){
                msg += "Please input Vender Name.\n";
                if(!$txtVenderName.parent().is("div")){
                    $txtVenderName.wrap("<div class='wrapdiv'></div>");
                }else{
                    $txtVenderName.parent().addClass("wrapdiv");
                }
                result=false;
            }
            if($.trim($txtVendorCity.val())==""){
                msg += "Please input Vender City.\n";
                if(!$txtVendorCity.parent().is("div")){
                    $txtVendorCity.wrap("<div class='wrapdiv'></div>");
                }else{
                    $txtVendorCity.parent().addClass("wrapdiv");
                }
                result=false;
            }
            if($.trim($txtVendorCountry.val())==""){
                msg += "Please input Vendor Country.\n";
                if(!$txtVendorCountry.parent().is("div")){
                    $txtVendorCountry.wrap("<div class='wrapdiv'></div>");
                }else{
                    $txtVendorCountry.parent().addClass("wrapdiv");
                }
                result=false;
            }
            if($.trim($txtBankName.val())==""){
                msg += "Please input Bank Name.\n";
                if(!$txtBankName.parent().is("div")){
                    $txtBankName.wrap("<div class='wrapdiv'></div>");
                }else{
                    $txtBankName.parent().addClass("wrapdiv");
                }
                result=false;
            }
            if($.trim($txtBankAC.val())==""){
               msg += "Please input Bank AC.\n";
               if(!$txtBankAC.parent().is("div")){
                    $txtBankAC.wrap("<div class='wrapdiv'></div>");
                }else{
                    $txtBankAC.parent().addClass("wrapdiv");
                }
                result=false;
            }
            if($.trim($txtBankCity.val())==""){
                 msg += "Please input Bank City.\n";
                 if(!$txtBankCity.parent().is("div")){
                        $txtBankCity.wrap("<div class='wrapdiv'></div>");
                    }else{
                        $txtBankCity.parent().addClass("wrapdiv");
                    }
               result=false;
            }
            if($.trim($txtSwiftCode.val())==""){
                msg += "Please input Swift Code.\n";
                if(!$txtSwiftCode.parent().is("div")){
                        $txtSwiftCode.wrap("<div class='wrapdiv'></div>");
                    }else{
                        $txtSwiftCode.parent().addClass("wrapdiv");
                    }
                result=false;
            }
        }
        if(msg!=""){
            alert(msg);
        }
        return result;
    }

    function CheckSummaryAmount(){
        var result=true;
        var $hfSummaryAmount = $("#InstallmentDiv input[id$='hfSummaryAmount']");
        if($hfSummaryAmount.val()!="0"&&$hfSummaryAmount.val()!=""){
             var $SummaryAmount = $("#out_table table.summarytypetable tr td input.SummaryAmount");
            var $radioInstallment = $("#out_table .radioInstallment input[type=radio]");
            var $txtTotalAmount2 = $("#Table1 tr td input.txtTotalAmount2");
            var $PaymentAmount = $("#InstallmentDiv tr.PaymentItem td input.PaymentAmount");
            var totalAmount = 0;
            $SummaryAmount.each(function () {
                totalAmount+=Math.round(parseFloat($(this).val()) * Math.pow(10, 2)) / Math.pow(10, 2);
            });
            totalAmount=Math.round(totalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
            if ($radioInstallment.eq(1).attr("checked")) {
                var txtTotalAmount2=  Math.round(parseFloat($txtTotalAmount2.val()) * Math.pow(10, 2)) / Math.pow(10, 2);
                if(totalAmount!=txtTotalAmount2){
                    result=false;
                }
            }else{
                var paymentAmount = $PaymentAmount.eq(0).val();
                paymentAmount=Math.round(parseFloat(paymentAmount) * Math.pow(10, 2)) / Math.pow(10, 2);
                if(totalAmount!=paymentAmount){
                    result=false;
                }
            }
            var hfSummaryAmount = "";
            $SummaryAmount.each(function () {
                if(!result){
                    if (!$(this).parent().hasClass("wrapdiv")) {
                         $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                }
                hfSummaryAmount+=$(this).val()+";";
            });
            $hfSummaryAmount.val(hfSummaryAmount);
        }
        return result; 
    }
    //检测用户输入的数据
    function CheckInstallmentInfo() 
    {
        $("#out_table span.wrapdiv").removeClass("wrapdiv");
        CreateForbidDIV();
        var result=CheckCityInfo();
        if(!result){
            ClearForbidDIV();
            return false;
        }
        var resultSummaryAmount=CheckSummaryAmount();
        if(!resultSummaryAmount){
            alert("费用汇总金额不等于当期总金额");
            ClearForbidDIV();
            return false;
        }
        UpdateSummaryExpenseType();
        $SystemPO.ClearBorderWarn();
        $BankName.ClearBorderWarn();
        $BankAC.ClearBorderWarn();
        $VenderCode.ClearBorderWarn();
        $VenderName.ClearBorderWarn();
        $ContractPO.ClearBorderWarn();
        $TotalAmount.ClearBorderWarn();
        $TotalAmount1.ClearBorderWarn();
        $PaidThisTime.ClearBorderWarn();
        $PaymentReason.ClearBorderWarn1();
        $Attachments.ClearBorderWarn2();
        $SpanCostCenter.ClearBorderWarn2(); 
        $PaymentDesc.ClearBorderWarn1();

        var isAllCorrect = true;
        if ($RadioInstallment.GetRadioChecked().val() == "Yes") {
            isAllCorrect = $TotalAmount.IsEmpty(true) == true ? false : isAllCorrect;
            isAllCorrect = $PaidThisTime.IsEmpty(true) == true ? false : isAllCorrect;
            isAllCorrect = $TotalAmount.IsZeroOrStr(true) == true ? false : isAllCorrect;
            isAllCorrect = $PaidThisTime.IsZeroOrStr(true) == true ? false : isAllCorrect;
        }
        else {
            isAllCorrect = $TotalAmount1.IsEmpty(true) == true ? false : isAllCorrect;
            isAllCorrect = $TotalAmount1.IsZeroOrStr(true) == true ? false : isAllCorrect;
        }

        isAllCorrect = $PaymentDesc.IsEmpty(true) == true ? false : isAllCorrect;
//        if ($VenderCode.IsEmpty(false) && $VenderName.IsEmpty(false)) {
//            isAllCorrect = $BankAC.IsNumber(true) == true ? isAllCorrect : false;
//            isAllCorrect = $BankAC.IsEmpty(true) == true ? false : isAllCorrect;
//            isAllCorrect = $BankName.IsEmpty(true) == true ? false : isAllCorrect;
//        }

        if ($.trim($RadioSystemPO.GetRadioChecked().val()) == "Yes") {
            isAllCorrect = $SystemPO.IsEmpty(true) == true ? false : isAllCorrect;
        }

        if ($.trim($RadioContractPO.GetRadioChecked().val()) == "Yes") {
                isAllCorrect = $ContractPO.IsEmpty(true) == true ? false : isAllCorrect; 
        }

        if ($.trim($RadioContractGR.GetRadioChecked().val()) == "No") {
            isAllCorrect = $PaymentReason.IsEmpty(true) == true ? false : isAllCorrect;
        }

        if (isAllCorrect == false) 
        {
            alert(" 您输入的信息存在错误，请重新输入 ");
            ClearForbidDIV();
            return false;
        }

        if (isFromPO == false && $.trim($RadioInstallment.GetRadioChecked().val()) == "Yes" && paidIndex <= 1) 
        { 
            if (CheckTotalPercent() == false) 
            {
                $PaidThisTime.SetBorderWarn();
                alert(' 您输入的分期付款百分比数据存在错误，请重新输入 ');
                ClearForbidDIV();
                return false;
            }
        }

        if($("input[id$='hfpostatus']").val() == "1")
        {
            var ccTotalAmount = GetCalAmount();
            var paidAmount = ($RadioInstallment.GetRadioChecked().val() == "Yes") ? 
                                $PaidThisTime.val() : $TotalAmount1.val();
            var  amount=Math.round(paidAmount) - Math.round(ccTotalAmount);
            if(Math.round(paidAmount) != Math.round(ccTotalAmount) && amount!=1 && amount!=-1 ) 
            {
                $RadioInstallment.GetRadioChecked().val() == "Yes" ? $PaidThisTime.SetBorderWarn() : 
                $TotalAmount1.SetBorderWarn();
                alert(" 本次付款金额和Cost Center总额不相等,请重新输入 ");
                ClearForbidDIV();
                return false;
            }

            if (CheckSubmit() == false){
                ClearForbidDIV();
                return false;
            }
        }
        else
        {
            if (CheckSubmit1() == false) {
                ClearForbidDIV();
                return false;
            }
        }

        SetControlUnDisable();
        return true;
    }

    function GetVendorInfo(){
        var rootURL = document.location.protocol + "//" + document.location.host; 
        jQuery.getJSON(rootURL + "/WorkFlowCenter/_layouts/ca/workflows/PaymentRequest" +
                                    "/vendorinfo.aspx?vid=" + $VenderCode.val(), 
        function (data) { 
            if(data.Result=="0"){
                var $txtVenderCode=$("#out_table input[id$='txtVenderCode']");
                $txtVenderCode.val("");
                alert("供应商编号无效！\n请重新输入供应商编号！");
                return;
            }
            $BankName.val(data.BankName);
            $BankAC.val(data.BankAccount);
            $SwiftCode.val(data.SwiftCode);
            $VenderName.val(data.VendorName);

            //$BankCity.val(data.BankCity);
            $("select.ddlBankCountry").val(data.BankCity);
            $VendorCity.val(data.VendorCity);
           // $VendorCountry.val(data.VendorCountry);
            $("select.ddlVendorCountry").val(data.VendorCountry);
        });
    }

    function ChangeVenderStatus()
    {
        if($VenderCode.IsEmpty(false) && $VenderName.IsEmpty(false))
        {
            $SwiftCode.SetBgColorWhite();
            $BankName.SetBgColorWhite();
            $BankAC.SetBgColorWhite();
            
            $BankCity.SetBgColorWhite();
            $VendorCity.SetBgColorWhite();
            $VendorCountry.SetBgColorWhite();
        }
        else
        {
            $SwiftCode.SetBgColorGrey();
            $BankName.SetBgColorGrey();
            $BankAC.SetBgColorGrey();
            
            $BankCity.SetBgColorGrey();
            $VendorCity.SetBgColorGrey();
            $VendorCountry.SetBgColorGrey();
        }
    }

    //当页面载入，根据条件去设置各控件的状态：可编辑与否
    function CheckInstallmentStatus(){
        if ($.trim($RadioInstallment.GetRadioChecked().val()) == "Yes") {
            $TotalAmount2.val($TotalAmount.val());
            $TotalAmount1.val($TotalAmount.val());
            $(".trNoInstallment").css("display", "none");
            $(".trInstallment").css("display", "");
        }
        else {
            $(".trNoInstallment").css("display", "");
            $(".trInstallment").css("display", "none");
            $TotalAmount.val($TotalAmount1.val());
            $TotalAmount2.val($TotalAmount1.val());
        }

        if(isFromPO == false){
            if($.trim($RadioContractPO.GetRadioChecked().val()) == "Yes"){
                $RadioSystemPO.attr("disabled", false);
                if(paidIndex > 1 && $.trim($ContractPO.val()) == "")
                    $ContractPO.attr("readonly", false);
                $SystemPO.attr("readonly", false).SetBgColorGrey();
                $ContractPO.SetBgColorWhite();
            }
            else{
                $RadioSystemPO.attr("disabled", true);
                $ContractPO.attr("readonly", true).SetBgColorGrey();
                $SystemPO.attr("readonly", true).SetBgColorGrey();
            }

            if($.trim($RadioSystemPO.GetRadioChecked().val()) == "Yes"){
                $SystemPO.attr("readonly", false).SetBgColorWhite();
            }
            else{
                $SystemPO.attr("readonly", true).SetBgColorGrey();
            }

            if($.trim($RadioContractGR.GetRadioChecked().val()) == "Yes"){
                 
                     $RadioSystemGR.attr("disabled", false);
                 
            }
            else{
                $RadioSystemGR.attr("disabled", true);
            }
        }
    }
    CheckInstallmentStatus();
    ChangeVenderStatus(); 

    function OpenInstallmentDialog() {
        OpenDialog();
    }

    ///事件绑定
    $VenderCode.bind({
        change: function () {
            $BankName.val("");
            $BankAC.val("");
            $SwiftCode.val("");
            $VenderName.val("");

            $VendorCity.val("");
            $("#out_table select[id$='ddlVendorCountry']").val("CN");
            $("#out_table select[id$='ddlBankCountry']").val("CN");
            GetVendorInfo();
        }
    });

    $VenderName.bind({
        change: function () {
            ChangeVenderStatus();
        }
    });

    $RadioInstallment.each(function () {
        $(this).bind({
            click: function () {
                SetInstallmentInfo();
                CheckInstallmentStatus();
            }
        });
    });

    $TotalAmount1.bind({
        change: function () {
            CheckInstallmentStatus();
        }
    });

    $TotalAmount.bind({
        change: function () {
            SetInstallmentInfo();
            CheckInstallmentStatus();
        }
    });

    var systemPO = $SystemPO.val();
    var contractPO = $ContractPO.val();
    $RadioContractPO.each(function () {
        $(this).bind({
            change: function () {
                if ($.trim($(this).val()) == "Yes") { 
                    $ContractPO.attr("readonly", false).val(contractPO).SetBgColorWhite();
                    $RadioSystemPO.attr("disabled", false);
                }
                else { 
                    systemPO = $SystemPO.val();
                    contractPO = $ContractPO.val();
                    $ContractPO.attr("readonly", true).val("").SetBgColorGrey();
                    $SystemPO.attr("readonly", true).val("").SetBgColorGrey();
                    $RadioSystemPO.attr("disabled", true).SetRadioChecked("No");
                }
            }
        });
    });

    $RadioSystemPO.each(function () {
        $(this).bind({
            change: function () {
                if ($.trim($(this).val()) == "Yes") {
                    $SystemPO.val(systemPO).attr("readonly", false).SetBgColorWhite();
                }
                else {
                    systemPO = $SystemPO.val();
                    $SystemPO.val("").attr("readonly", true).SetBgColorGrey();
                }
            }
        });
    });

    $RadioSystemGR.each(function () {
        $(this).bind({
            change: function () {
                if ($.trim($(this).val()) == "Yes") {
                    $RadioContractGR.SetRadioChecked("Yes");
                }
            }
        });
    });

    $RadioContractGR.each(function () {
        $(this).bind({
            change: function () {
                if ($.trim($(this).val()) == "Yes") {
                    var $hfgr = $("input[id$='hfgr']");
                    if ($hfgr.val() == "0") {
                        $RadioSystemGR.attr("disabled",  false);
                    }
                }
                else {
                    $RadioSystemGR.attr("disabled", true).SetRadioChecked("No");
                }
            }
        });
    });

</script>
<script type="text/javascript">
    $(function () {
        Hidehfgr();
        DisableCC();
        DisplayOrHideFATD();
        BindAmountBlurEvent();
        BindExpenseTypeEvent();
        DrawSummaryExpenseTable1();
        BindPeopleFind();
        BindQueryVendorCodeClick(); ShowOrDisplayTR();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });

    function ShowOrDisplayTR() {
        var $hfFromPO = $("input[id$='hfFromPO']");
        if ($hfFromPO.val() == "") {
            $("#out_table tr.FromPOTR").hide();
            //var $lblCurrency = $("#out_table span.lblCurrency");
            var $hfSummaryAmount = $("#InstallmentDiv input[id$='hfSummaryAmount']");
            if ($hfSummaryAmount.val() != "0" && $hfSummaryAmount.val() != "") {
                $("#out_table tr.NoFromPOTR").show(); 
            }
        } else {
            $("#out_table tr.FromPOTR").show();
            $("#out_table tr.NoFromPOTR").hide();
        }
    }

    function BindPeopleFind() {
        $('#<%=this.cpfUser.ClientID %>' + '_checkNames').click(function () {
            $("#<%=this.btnPeopleInfo.ClientID %>").click();
        });
        var $input = $("#ctl00_PlaceHolderMain_ListFormControl1_PaymentDataForm_cpfUser_HiddenEntityKey");
        var $div = $("#ctl00_PlaceHolderMain_ListFormControl1_PaymentDataForm_cpfUser_upLevelDiv");
        $div.html($input.val());
        $div.css("textDecoration", "underline");
        $div.focus(function () {
            $div.css("textDecoration", "none");
        });
    }
    function CheckAmountChange1(obj) {
        var $amount = $(obj);
        var $et = $amount.parent().parent().find("select.ExpenseType");

        if ($et.val() == "Tax payable - VAT input") {
            $amount.blur();
           
        }
    }
    function Hidehfgr() {
        var $hfgr = $("input[id$='hfgr']");
        var $radioSystemGR = $("table[id$='radioSystemGR']").find("input");
        if ($hfgr.val() == "1") {
            $radioSystemGR.each(function () {
                $(this).attr("disabled", true);
            });
         }
    }

    function AppendHtml(expenseType, amount, costcenter) {
        var appendHtml = "";
        appendHtml = "<tr class=\"item\" ><td class=\"label1\" >" + expenseType + "</td><td class=\"label1\" >" + costcentertd + "</td><td class=\"label1\" ><input class=\"SummaryAmount\"  type=\"text\" value=\"" + amount + "\" /></td></tr>";
        return appendHtml;
    }
    function DrawSummaryExpenseTable1() {
        $(".summarytypetable tr").remove(".item");
        var $summarytype = $(".summarytype");
        var $hidSummaryExpenseType = $('#<%= this.hidSummaryExpenseType1.ClientID %>');
        var summaryExpenseType = $hidSummaryExpenseType.val(); 
        if ($hidSummaryExpenseType.val() != "") {
            $("tr.summaryTR").show();
            var summaryExpense = eval("(" + summaryExpenseType + ")");
            try {
                $.each(summaryExpense, function (i, item) {
                    var $html = $(AppendHtml(item.name, Math.round(item.val * Math.pow(10, 2)) / Math.pow(10, 2), item.costcenter));
                    $summarytype.before($html);
                });
            } catch (e) { }
        }
    }

    function DisplayOrHideFATD() {
        var $FAStatus1 = $('#<%=this.FAStatus1.ClientID %>');
        if ($FAStatus1.val() == "0") {
            $("#CostCenterTable td.FA").hide();
        }
    }

    function EndRequestHandler() {
        Hidehfgr();
        DisableCC();
        DisplayOrHideFATD();
        DisplayOrHideFATD1();
        BindEvent();
        BindAmountBlurEvent();
        BindExpenseTypeEvent();
        BindAmountBlurEvent1();
        BindExpenseTypeEvent1();
        GetCalAmount1();
        ChangeInstallmentInfo();
        DrawSummaryExpenseTable();
        BindPeopleFind();ShowOrDisplayTR();
       // BindQueryVendorCodeClick();
        //取得Items总金额 -- GetCalAmount1();
        //var totalAmount = GetCalAmount();
        //alert("totalAmount：" + totalAmount);
    }

    function DisableCC() {
        var $et = $("#CostCenterTable1 select.ExpenseType1");
        $et.each(function () {
            if ($.trim($(this).val()) == "Tax payable - VAT input") {
                //|| $(this).val().indexOf("Prepaid") == 0) {
                var $cc = $(this).parent().parent().parent().parent().find("select.cc1");
                var $fano = $(this).parent().parent().parent().parent().find("input.FANO1");
                $cc.val("");
                $cc.parent().hide();
                $fano.val("");
                $fano.hide();
            }
        });
        var $et1 = $("#CostCenterTable select.ExpenseType"); 
        $et1.each(function () {
            if ($.trim($(this).val()) == "Tax payable - VAT input") {
                //|| $(this).val().indexOf("Prepaid") == 0) {
                var $cc = $(this).parent().parent().parent().parent().find("select.cc");
                var $fano = $(this).parent().parent().parent().parent().find("input.FANO");
                $cc.val("");
                $cc.parent().hide();
                $fano.val("");
                $fano.hide();
            }
        });
        var $lblCurrency = $("span.lblCurrency");
        if ($lblCurrency.text() != "") {
            $("tr.CurrencyTR").show();
         }
    }

    function BindAmountBlurEvent() {
        var $inputAmount = $("#CostCenterTable td input.Amount");
        $inputAmount.each(function () {
            $(this).blur(function () {
                if (isNaN($(this).val()) || $(this).val() < 0 || $(this).val() > 100000000) {
                    $(this).val("0");
                    if (!$(this).parent().hasClass("wrapdiv")) {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                    alert('Please fill the valid number.');
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
                //取得Items总金额
                var totalAmount = GetCalAmount();
                //alert("totalAmount：" + totalAmount);
            });
        });
    }
    function GetCalAmount() {
        var result = false;
        var $inputAmount = $("#CostCenterTable td input.Amount");
        var totalAmount = 0;
        $inputAmount.each(function () {
            var inputAmount = $(this).val();
            if (inputAmount == "") {
                inputAmount = "0";
            }
            totalAmount += parseFloat(inputAmount);
        });
        totalAmount = Math.round(totalAmount * Math.pow(10, 2)) / Math.pow(10, 2);
        return totalAmount;
    }
    function BindExpenseTypeEvent() {
        var $ExpenseType = $("#CostCenterTable .ExpenseType1");
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

        var $CostCenter = $("#CostCenterTable .CostCenter");
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
        $("#CostCenterTable .wrapdiv").removeClass("wrapdiv");
        var result = true;
        var msg = "";
        var $et = $("#CostCenterTable select.ExpenseType");
        $et.each(function () {
            if ($.trim($(this).val()) == "") {
                msg += "Please Select Expense Type.\n";
                if (!$(this).parent().parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                }
                result = false;
            }
//            var hf_D_RequestType = $("input[id$='hf_D_RequestType']").val();
//            if (hf_D_RequestType=="Opex") {
//                var expense = $(this).val();
//                var $costc = $(this).parent().parent().parent().parent().find("select.cc");
//                if (expense.indexOf("Store") == 0) {
//                    if ($costc.val().indexOf("S") != 0) {
//                        msg += "Please Select Store CostCenter.\n";
//                        if (!$costc.parent().hasClass("wrapdiv")) {
//                            $costc.parent().addClass("wrapdiv");
//                        }
//                        result = false;
//                    }
//                }
//                if (expense.indexOf("HO") == 0) {
//                    //var $costc = $(this).parent().parent().parent().parent().find("select.cc");
//                    if ($costc.val().indexOf("H") != 0) {
//                        msg += "Please Select Head Office CostCenter.\n";
//                        if (!$costc.parent().hasClass("wrapdiv")) {
//                            $costc.parent().addClass("wrapdiv");
//                        }
//                        result = false;
//                    }
//                }
//                if (expense.indexOf("Store") != 0 && expense.indexOf("HO") != 0) {
//                    if ($costc.val().indexOf("S") == 0) {
//                        msg += "Please Select Not Start With 'S' CostCenter.\n";
//                        if (!$costc.parent().hasClass("wrapdiv")) {
//                            $costc.parent().addClass("wrapdiv");
//                        }
//                        result = false;
//                    }
//                }
//            }
        });
        var $cc = $("#CostCenterTable select.cc");
        $cc.each(function () {
            var $expenseType = $(this).parent().parent().parent().prev().prev().find("select.ExpenseType");
            if ($.trim($(this).val()) == "" && $expenseType.val() != "Tax payable - VAT input"
                    && $expenseType.val().indexOf("Accrual") != 0
                    && $expenseType.val().indexOf("Accrued") != 0) {
                    //&& $expenseType.val().indexOf("Prepaid") != 0) {
//            ) {
                msg += "Please Select CostCenter.\n";
                if (!$(this).parent().parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                }
                result = false;
            }
        });
        var $txtAmount = $("#CostCenterTable input.Amount");
        $txtAmount.each(function () {
            if ($.trim($(this).val()) == "") {
                msg += "Please fill the Amount.\n";
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).wrap("<span class=\"wrapdiv\"></span>");
                }
                result = false;
            }
        });
        var $FAStatus1 = $('#<%=this.FAStatus1.ClientID %>');
        if ($FAStatus1.val() == "1") {
            var $txtFANO = $("#CostCenterTable input.FANO");
            $txtFANO.each(function () {
                var $et11 = $(this).parent().parent().find("select.ExpenseType")
                if ($.trim($(this).val()) == "" && $et11.val() != "Tax payable - VAT input") {
                    msg += "Please fill the FANO.\n";
                    if (!$(this).parent().hasClass("wrapdiv")) {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                    result = false;
                }
            });
        }
        if (msg != "") {
            alert(msg);
        }
        return result;
    }
    function GetTmpId(tmpId) {
        return tmpId.substring(0, tmpId.lastIndexOf('_') + 1);
    }
    function ChangeGLAccount(obj) {
        var $cc = $(obj).parent().parent().parent().parent().find("select.cc");
        var $fano = $(obj).parent().parent().parent().parent().find("input.FANO");
        //var $amount = $(obj).parent().parent().parent().parent().find("input.Amount"); 
        if ($(obj).val() != "Tax payable - VAT input") {
           // && $(obj).val().indexOf("Prepaid") == -1) {
            $cc.parent().show();
            $fano.show();
        } else {
            //$amount.val("");
            $cc.val("");
            $fano.val("");
            $cc.parent().hide();
            $fano.hide();
        }
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

    function SetMainPageCurrency(value) {
        $('tr.CurrencyTR').show();
        $('#<%=this.lblCurrency.ClientID %>').text(value);
    }
</script>
<script type="text/javascript">
    var pageIndex = 0;
    var itemArray = new Array();
    var allArray = new Array();
    function BindQueryVendorCodeClick() {
        var $QueryVendorCode = $("#QueryVendorCode");
        var $CloseQuery = $("a.CloseQuery");
        var $QueryVendorInfo = $("a.QueryVendorInfo");
        var $Vendor_Info_ul = $("#Vendor_Info ul.v_ul");
        var $SelectVendor = $("a.SelectVendor");
        $QueryVendorCode.click(function () {
            DisplayQueryVendorCode();
        });
        $CloseQuery.click(function () {
            HideQueryVendorCode();
        });
        $QueryVendorInfo.click(function () {
            $("#Vendor_Info ul.v_ul").remove();
            QueryVendorInfo();
        });
        $Vendor_Info_ul.live("mousemove", function () {
            $(this).find("li").css("backgroundColor", "#f2f2f2");
        });
        $Vendor_Info_ul.live("mouseout", function () {
            $(this).find("li").css("backgroundColor", "white");
        });
        $SelectVendor.live("click", function () {
            SetVerdorInfo($(this));
            HideQueryVendorCode();
        });
        BindQueryAjaxEvent();
        $("span.up").live("click",function () {
            if (pageIndex - 1 < 0) {
                pageIndex = 0;
            } else {
                pageIndex = pageIndex - 1;
            }
            PagingQuery(allArray);
        });
        $("span.down").live("click", function () {
            if (pageIndex + 1 >= allArray.length) {
                pageIndex = pageIndex;
            } else {
                pageIndex = pageIndex + 1;
            }
            PagingQuery(allArray);
        });
        BindSummaryAmountEvent();
    }
    function SetVerdorInfo(vendor) {
        var $vendorCode = vendor.parent().parent().find(".VendorCode");
        var $vendorName = vendor.parent().parent().find(".VendorName");
        var $bankName = vendor.parent().parent().find(".BankName");
        var $bankAC = vendor.parent().parent().find(".BankAC");
        var $vendorCity = vendor.parent().parent().find(".VendorCity");
        var $vendorCountry = vendor.parent().parent().find(".VendorCountry");
        var $bankCountry = vendor.parent().parent().find(".BankCountry");
        var $bankKey = vendor.parent().parent().find(".BankKey");
        $VenderCode.val($vendorCode.html());
        $VenderName.val($vendorName.attr("title"));
        $VendorCity.val($vendorCity.html());
        $("select.ddlVendorCountry").val($vendorCountry.html());
        $BankName.val($bankName.attr("title"));
        $BankAC.val($bankAC.html());
        $("select.ddlBankCountry").val($bankCountry.html());
        $SwiftCode.val($bankKey.html());
    }
    function DisplayQueryVendorCode() {
        $("#V_bgDiv").css({
            height: function () {
                return $(".V_ContentDiv").height()-50;
            },
            width: "100%"
        });
        $("#V1_bgDiv").css({
            height: "30",
            width: "100%"
        });
        $("#V_bgDiv").show();
        $("#V1_bgDiv").show();
        $("#V_ErrorDiv").show(300);
        $("#V_ErrorDiv").removeClass("V_ErrorDiv_Scroll");
        $("#V_ErrorDiv").css("paddingRight", "6px");
        $("div.paging").hide();
        $("ul.v_ul").remove();
        $("#Vendor_Info").find("ul").hide();
        $("input.V_Code").val("");
        $("input.V_Name").val("");
    }
    function HideQueryVendorCode() {
        $("#V_bgDiv").hide();
        $("#V1_bgDiv").hide();
        $("#V_ErrorDiv").hide(300);
        $("#Vendor_Info ul.v_ul").remove();
    }
    function QueryVendorInfo() {
        pageIndex = 0;
        itemArray = new Array();
        allArray = new Array();
        var $Vendor_Info = $("#Vendor_Info");
        var $V_Code = $("input.V_Code");
        var $V_Name = $("input.V_Name");
        var rootURL = document.location.protocol + "//" + document.location.host;
        var r_url = rootURL + "/WorkFlowCenter/_layouts/ca/workflows/PaymentRequest" +
                              "/VendorInfo.aspx?vendorCode=" + $V_Code.val() + "&vendorName=" + encodeURIComponent($V_Name.val());
        $.ajax({
            type: "POST",
            url: r_url,
            data: "?vendorCode=" + $V_Code.val() + "&vendorName=" + $V_Name.val(),
            dataType: "json",
            timeout: 300000,
            success: function (data) {
                BindVendorInfoList(data);
            },
            error: function (msg) {
                $("#errordiv").html(msg);
                $("#errordiv").show();
                $Vendor_Info.hide();
            }
        });
    }
    function BindQueryAjaxEvent() {
        var $Vendor_Info = $("#Vendor_Info");
        var $loading = $("div.loading");
        $Vendor_Info.ajaxStart(function () {
            $Vendor_Info.find("ul").hide();
            $("#errordiv").hide();
            $("div.paging").hide();
            $Vendor_Info.show();
            $loading.show();
            $("a.QueryVendorInfo").hide();
        });
        $Vendor_Info.ajaxStop(function () {
            $loading.hide();
            $Vendor_Info.find("ul").show();
            $("a.QueryVendorInfo").show();
        });
    }
    function BindVendorInfoList(data) {
        var $Vendor_Info = $("#Vendor_Info");
        if (data != null) {
            try {
                $("ul.v_ul").remove();
                $.each(data, function (i, item) {
                    if (item != undefined) {
                        if (itemArray.length < 10) {
                            itemArray.push(item);
                        }
                        else {
                            allArray.push(itemArray);
                            itemArray = new Array();
                            itemArray.push(item);
                        }
                    }
                });
                allArray.push(itemArray);
                PagingQuery(allArray);
                if (allArray.length > 1) {
                    $("#V_ErrorDiv").addClass("V_ErrorDiv_Scroll");
                    $("#V_ErrorDiv").css("paddingRight", "20px");
                    $("div.paging").show();
                } else {
                    $("#V_ErrorDiv").removeClass("V_ErrorDiv_Scroll");
                    $("#V_ErrorDiv").css("paddingRight", "6px");
                    $("div.paging").hide();
                }
                $Vendor_Info.show();
            }
            catch (e) {
            }
        } else {
            $("#V_ErrorDiv").removeClass("V_ErrorDiv_Scroll");
            $("#V_ErrorDiv").css("paddingRight", "6px");
            $("#errordiv").show();
            $Vendor_Info.hide();
        }
    }
    function PagingQuery(allItemArray) {
        var current = allItemArray[pageIndex];
        $("ul.v_ul").remove();
        var $verdor_ul = $("#Vendor_Info ul.verdor_ul");
        $.each(current, function (i, item) {
            if (item != undefined) {
                var $html = $(AppendVendorUL(item.VendorID, item.VendorName, item.BankName, item.BankAccount, item.VendorCity, item.VendorCountry, item.BankCity, item.SwiftCode));
                $verdor_ul.after($html);
            }
        });
    }
    function AppendVendorUL(VendorCode, VendorName, BankName, BankAC, VendorCity, VendorCountry, BankCountry, BankKey) {
        var appendHtml = "";
        var vn = VendorName;
        var bn = BankName;
        VendorName = VendorName.length > 15 ? VendorName.substring(0, 15) + "..." : VendorName;
        BankName = BankName.length > 10 ? BankName.substring(0, 10) + "..." : BankName;
        appendHtml += "<ul class=\"v_ul\">";
        appendHtml += "<li class=\"VendorCode borderleft\">" + VendorCode + "</li>";
        appendHtml += "<li class=\"VendorName\" title=\"" + vn + "\">" + VendorName + "</li>";
        appendHtml += "<li class=\"BankName\" title=\"" + bn + "\">" + BankName + "</li>";
        appendHtml += "<li class=\"BankAC\">" + BankAC + "</li>";
        appendHtml += "<li class=\"Select\"><a class=\"SelectVendor\" title=\"Select Vendor Info\">Select</a>";
        appendHtml += "<div class=\"VendorCity\">" + VendorCity + "</div>";
        appendHtml += "<div class=\"VendorCountry\">" + VendorCountry + "</div>";
        appendHtml += "<div class=\"BankCountry\">" + BankCountry + "</div>";
        appendHtml += "<div class=\"BankKey\">" + BankKey + "</div>";
        appendHtml += "</li>";
        appendHtml += "</ul>";
        return appendHtml;

    }
    
</script>
<script type="text/javascript">
    function BindSummaryAmountEvent() {
        var $SummaryAmount = $("table.summarytypetable tr td input.SummaryAmount");
        $SummaryAmount.live("blur", function () {
            if (isNaN($(this).val()) || $(this).val() < 0 || $(this).val() > 100000000) {
                $(this).val("0");
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).wrap("<span class=\"wrapdiv\"></span>");
                }
                alert('Please fill the valid number.');
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
        });
    }

</script>