<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit2.ascx.cs" Inherits="CA.WorkFlow.UI.TE.DataEdit2" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register Src="~/_Layouts/CA/WorkFlows/TravelExpenseClaim2/DataEdit/TravelHotelForm.ascx"
    TagName="TravelHotelForm" TagPrefix="TravelHotelForm" %>
<%@ Register Src="~/_Layouts/CA/WorkFlows/TravelExpenseClaim2/DataEdit/TravelMealForm.ascx"
    TagName="TravelMealForm" TagPrefix="TravelMealForm" %>
<%@ Register Src="~/_Layouts/CA/WorkFlows/TravelExpenseClaim2/DataEdit/TravelTransportationForm.ascx"
    TagName="TravelTransportationForm" TagPrefix="TravelTransportationForm" %>
<%@ Register Src="~/_Layouts/CA/WorkFlows/TravelExpenseClaim2/DataEdit/TravelSamplePurchaseForm.ascx"
    TagName="TravelSamplePurchaseForm" TagPrefix="TravelSamplePurchaseForm" %>
<%@ Register Src="~/_Layouts/CA/WorkFlows/TravelExpenseClaim2/DataEdit/TravelOtherForm.ascx"
    TagName="TravelOtherForm" TagPrefix="TravelOtherForm" %>
<script type="text/javascript" src="DataEdit/JS/Calendar3.js"></script>
<style type="text/css">
    .Content
    {
        float: left;
        margin: 0 auto;
    }
    .Title
    {
        margin-top: 20px;
        float: left;
        width: 680px;
        border: #9dabb6 2px solid;
        border-bottom: #9dabb6 1px solid;
    }
    .Title_Head
    {
        float: left;
        width: 100%;
        text-align: center;
        padding: 5px 0px 5px 0px;
        border-bottom: #ccc 1px solid;
    }
    .Title_Cotent
    {
        float: left;
        width: 100%;
        padding: 0px;
    }
    .Title_Cotent ul
    {
        width: 100%;
        float: left;
    }
    .Title_Cotent ul li
    {
        padding: 0px;
        margin: 0px;
        float: left;
        text-align: center;
        border-bottom: #ccc 1px solid;
        padding: 5px 0px 5px 0px;
    }
    .w25
    {
        border-right: #ccc 1px solid;
        width: 25%;
    }
    .w24
    {
        width: 24.5%;
    }
    .w74
    {
        width: 74.5%;
    }
    .Info li
    {
        line-height: 20px;
        color: #06c;
    }
    .no
    {
        line-height: 24px;
        color: #06c;
    }
</style>
<style type="text/css">
    .Item
    {
        float: left;
        width: 900px;
        border: #9dabb6 2px solid;
        border-bottom: #9dabb6 2px solid;
        margin-top: 20px;
        height: auto;
    }
    .Item_Head_Left
    {
        width: 170px;
        float: left;
        padding: 5px 0px 5px 0px;
        text-align: center;
        line-height: 25px;
        border-bottom: #ccc 1px solid;
    }
    .Item_Head_Right
    {
        width: 730px;
        float: left;
        padding: 5px 0px 5px 0px;
        background-color: White;
        line-height: 25px;
        border-bottom: #ccc 1px solid;
    }
    .Applicant  input
    {
        border: none;
        color: #06c;
        padding: 7px 0px 0px 0px;
        margin-left: 5px;
        width: 80%;
        background-color:transparent;
        text-align:center;
    }
    .Item_Head_Right input
    {
        border: none;
        border-bottom: #999 1px solid;
        color: #06c;
        padding: 7px 0px 0px 0px;
        margin-left: 5px;
        width: 500px;
    }
    .Item_Title
    {
        width: 100%;
        float: left;
    }
    .Item_Title_Left
    {
        float: left;
        width: 680px;
        text-align: center;
        padding: 5px 0px 5px 0px;
        border-bottom: #ccc 1px solid;
    }
    .Item_Title_Right
    {
        width: 220px;
        float: left;
        padding: 5px 0px 5px 0px;
        background-color: White;
        border-bottom: #ccc 1px solid;
    }
    .Item_UL
    {
        width: 100%;
        float: left;
    }
    .Item_UL ul
    {
        width: 100%;
        float: left;
    }
    .Item_UL ul li
    {
        padding: 0px;
        margin: 0px;
        float: left;
        text-align: center;
        border-bottom: #ccc 1px solid;
        padding: 5px 0px 5px 0px;
        line-height: 25px;
    }
    .w10
    {
        border-right: #ccc 1px solid;
        width: 10%;
        background-color: White;
    }
    .w104
    {
        border-right: #ccc 1px solid;
        width: 14%;
    }
    .w15
    {
        border-right: #ccc 1px solid;
        width: 15%;
    }
    .w14
    {
        width: 14.1%;
    }
    .w9
    {
        border-right: #ccc 1px solid;
        width: 6%;
    }
    .w40
    {
        border-right: #ccc 1px solid;
        width: 389px;
    }
    .w20
    {
        width: 90px;
        border-right: #ccc 1px solid;
        background-color: White;
    }
    .w30
    {
        background-color: White;
        width: 263px;
    }
    .bg
    {
        background-color: White;
    }
    .DataTitle
    {
        color: Black;
        font-weight: 700;
    }
    .BlankLeft
    {
        float: left;
        width: 40%;
        height: 20px;
        border-bottom: #ccc 1px solid;
    }
    .BlankRight
    {
        float: left;
        width: 60%;
        height: 20px;
        background-color: White;
        border-bottom: #ccc 1px solid;
    }
    
    .Item_UL img
    {
        cursor: pointer;
        margin-left: 5px;
        margin-right: 5px;
    }
    .Item_UL ul li.TitleText
    {
        padding-left: 20px;
    }
    .Item_UL ul li.Sub
    {
        text-align: center;
    }
    .Item_UL ul li.Total
    {
        color: #06c;
        padding-left: 10px;
    }
    
    .Hotel ul li
    {
        text-align: left;
    }
    .Item_UL ul li.ExpenseType
    {
        text-align: left;
    }
    .MealAllowance ul li
    {
        text-align: left;
    }
    .Transportation ul li
    {
        text-align: left;
    }
    .SamplePurchase ul li
    {
        text-align: left;
    }
    .Other ul li
    {
        text-align: left;
    }
    .Item_UL li input
    {
        border: none;
        border-bottom: #999 1px solid;
        color: #06c;
        width: 70px;
    }
    .Item_UL li.ExpenseType input
    {
        border: none;
        border-bottom: #999 1px solid;
        color: #06c;
        width: 70px;
        background-color: transparent;
    }
    .HotelItem li.Date input
    {
        border: none;
        border-bottom: #999 1px solid;
        color: #06c;
        width: 35px;
    }
    .HotelItem li.Date img
    {
        margin: 0px;
    }
    .ItemSelect
    {
        width: 92px;
    }
    .CostCenterCurrency
    {
        width: 80px;
    }
    li.PaidCredit input
    {
        border: none;
    }
    li.ImgText
    {
        border-right: none;
    }
    .w774
    {
        width: 773px;
    }
    .Title_Cotent ul li.Summary_Left
    {
        text-align: right;
        border: none;
        width: 80%;
        line-height: 20px;
    }
    .Title_Cotent ul li.Summary_Right
    {
        text-align: left;
        border: none;
        width: 10%;
        line-height: 20px;
        margin-left: 20px;
    }
    .Title_Cotent ul li.Summary_Right input
    {
        text-align: left;
        border: none;
        color: #06c;
        width: 100px;
        background-color: transparent;
        padding-top:2px;
    }
    .Title_Cotent ul li.Summary_Left input
    {
        text-align: left;
        border: none;
        border-bottom: #999 1px solid;
        color: #06c;
        width: 390px;
        background-color: transparent;
    }
    .w24
    {
        width: 24.5%;
    }
    .w74
    {
        width: 74.5%;
    }
    .FinanceSection ul li
    {
        line-height: 25px;
    }
    .FinanceSection ul li input
    {
        text-align: left;
        border: none;
        border-bottom: #999 1px solid;
        color: #06c;
        width: 120px;
        background-color: transparent;
    }
    .FinanceSection ul li.NoBorder input
    {
        text-align: left;
        border: none;
        color: #06c;
        width: 120px;
        background-color: transparent;
    }
    .Bottom table tr
    {
        height: 25px;
    }
    .Meal
    {
        line-height: 20px;
    }
    .NoBorder
    {
        border: none;
    }
    .Border
    {
        border-bottom: #9dabb6 2px solid;
    }
    #partAttachment
    {
        line-height: normal;
        float:left;
        background-color:White;
    }
    #onetidIOFile
    {
        width: 300px;
    }
    #attachOKbutton
    {
        width: 100px;
    }
    #attachCancelButton
    {
        width: 100px;
    }
</style>
<div class="Content">
    <div class="Title">
        <div class="Title_Head">
            <h3>
                Employee Information<br />
                员工信息</h3>
        </div>
        <div class="Title_Cotent Applicant">
            <ul>
                <li class="w25">Applicant<br />
                    申请人</li>
                <li class="w74 no">
                    <div id="Applicant" runat="server">
                        <input type="text" id="txtApplicant" readonly="readonly" runat="server" /></div>
                </li>
            </ul>
            <ul>
                <li class="w25">Chinese Name<br />
                    中文姓名</li>
                <li class="w25">English Name<br />
                    英文姓名</li>
                <li class="w25">ID/Passport No.<br />
                    身份证/护照号码</li>
                <li class="w24">Department<br />
                    部门</li>
            </ul>
            <ul class="Info">
                <li class="w25">
                    <div id="ChineseName" runat="server">
                        <input type="text" id="txtChineseName" readonly="readonly" runat="server" /></div>
                </li>
                <li class="w25">
                    <div id="EnglishName" runat="server">
                        <input type="text" id="txtEnglishName" readonly="readonly" runat="server" /></div>
                </li>
                <li class="w25">
                    <div id="IDPassportNo" runat="server">
                        <input type="text" id="txtIDPassportNo" readonly="readonly" runat="server" /></div>
                </li>
                <li class="w24">
                    <div id="Department" runat="server">
                        <input type="text" id="txtDepartment" readonly="readonly" runat="server" /></div>
                </li>
            </ul>
            <ul>
                <li class="w25">Mobile Phone No.<br />
                    手机号码</li>
                <li class="w25 no">
                    <div id="MobilePhone" runat="server" class="Applicant">
                        <input type="text" id="txtMobile" readonly="readonly" runat="server" /></div>
                </li>
                <li class="w25">Office Ext. No.<br />
                    分机号码 </li>
                <li class="w24 no">
                    <div id="OfficeExtNo" runat="server" class="Applicant">
                        <input type="text" id="txtOffice" readonly="readonly" runat="server" /></div>
                </li>
            </ul>
        </div>
    </div>
    <div class="Item">
        <div class="Item_Head">
            <div class="Item_Title">
                <div class="Item_Title_Left">
                    <h3>
                        Travel Expense Claim<br />
                        出差费用报销</h3>
                </div>
                <div class="Item_Title_Right">
                    　<br />　
                </div>
            </div>
            <div class="Item_Head_Content">
                <div class="Item_Head_Left">
                    Purpose of business trip</div>
                <div class="Item_Head_Right">
                    <input id="txtTravelPurpose" runat="server" /></div>
            </div>
            <div class="Item_UL">
                <ul class="DataTitle">
                    <li class="w104">Expense Type</li>
                    <li class="w15">Date</li>
                    <li class="w10">Cost Center</li>
                    <li class="w10">Original Amt</li>
                    <li class="w10">Currency</li>
                    <li class="w10">Exch Rate</li>
                    <li class="w10">Claim Amt</li>
                    <li class="w9 bg">Com Std</li>
                    <li class="w14 bg">Paid by Credit Card</li>
                </ul>
            </div>
            <div class="Blank">
                <div class="BlankLeft">
                </div>
                <div class="BlankRight">
                </div>
            </div>
            <!--Hotel Item-->
            <TravelHotelForm:TravelHotelForm id="TravelHotelForm" runat="server" />
            <!--Meal Allowance Item-->
            <TravelMealForm:TravelMealForm id="TravelMealForm" runat="server" />
            <!--Transportation Item-->
            <TravelTransportationForm:TravelTransportationForm id="TravelTransportationForm"
                runat="server" />
            <!--Sample Purchase Item-->
            <TravelSamplePurchaseForm:TravelSamplePurchaseForm id="TravelSamplePurchaseForm"
                runat="server" />
            <!--Others Item-->
            <TravelOtherForm:TravelOtherForm id="TravelOtherForm" runat="server" />
        </div>
    </div>
    <div class="Title Border">
        <div class="Title_Head">
            <h3 style="line-height: 25px">
                Claim Summary</h3>
        </div>
        <div class="Title_Cotent" id="Summary">
            <ul>
                <li class="Summary_Left">Total</li>
                <li class="Summary_Right Summary_Total">
                    <input type="text" id="txtTotalCost" readonly="readonly" runat="server" />
                </li>
            </ul>
            <ul>
                <li class="Summary_Left">Cash Advance</li>
                <li class="Summary_Right Summary_CashAdvance">
                    <input type="text" id="txtCashAdvanced" readonly="readonly" runat="server"  value="0"/>
                </li>
            </ul>
            <ul>
                <li class="Summary_Left">Paid by Co Credit Card</li>
                <li class="Summary_Right Summary_PaidCreditCard">
                    <input type="text" id="txtPaidByCreditCard" readonly="readonly" runat="server" />
                </li>
            </ul>
            <ul>
                <li class="Summary_Left">Payable to Employee/(Refund to Finance)</li>
                <li class="Summary_Right Summary_Payable">
                    <input type="text" id="txtNetPayable" readonly="readonly" runat="server" />
                </li>
            </ul>
            <ul>
                <li class="Summary_Left">Approved Budget less Flight Charges</li>
                <li class="Summary_Right Summary_Approved">
                    <input type="text" id="txtTotalExceptFlight" readonly="readonly" runat="server"  value=""/>
                </li>
            </ul>
            <ul>
                <li class="Summary_Left">Lower/(Higher) than Budget</li>
                <li class="Summary_Right Summary_Lower SummaryClaim">
                    <input type="text" id="txtComparedToApproved" readonly="readonly" runat="server" />
                </li>
            </ul>
            <ul>
                <li class="Summary_Left" style="text-align: left; padding-left: 20px">Reasons for why
                    travel expenses exceeded the travel budget (if applicable)</li>
                <li class="Summary_Right"></li>
            </ul>
            <ul>
                <li class="Summary_Left" style="text-align: left; padding-left: 20px">
                    <input type="text" id="txtReasons"  runat="server" />
                </li>
                <li class="Summary_Right"></li>
            </ul>
        </div>
    </div>
    <div class="Title">
        <div class="Title_Head">
            <h3>
                Finance Section</h3>
        </div>
        <div class="Title_Cotent FinanceSection">
            <ul>
                <li class="w25">Supporting submitted</li>
                <li class="w25 no">
                    <input type="text" id="txtSupportingSubmitted" runat="server" />
                </li>
                <li class="w25">Date of submission</li>
                <li class="w24 no NoBorder">
                    <input type="text" id="txtSubmissionDate" readonly="readonly" runat="server" />
                </li>
            </ul>
            <ul>
                <li class="w25">Attachment</li>
                <li class="w74 no" style="line-height: 17px">
                    <QFL:FormAttachments runat="server" ID="attacthment">
                    </QFL:FormAttachments>
                </li>
            </ul>
            <ul>
                <li class="w25">Finance Remark</li>
                <li class="w74 no NoBorder">
                    <input type="text" id="txtFinanceRemark" readonly="readonly" runat="server" />
                </li>
            </ul>
        </div>
    </div>
    <div class="Bottom">
        <table class="ca-workflow-form-table" style="width: 685px; margin-top: 20px;">
            <tr>
                <td colspan="10" class="label align-center">
                    <h3 class="p5">
                        For your reference 供参考<br />
                        According to the Company Business Travel Policy 按照公司商务差旅政策</h3>
                </td>
            </tr>
            <tr>
                <td class="label align-center w15" rowspan="3">
                    <h3>
                        Location
                        <br />
                        地方</h3>
                </td>
                <td colspan="3" class="label align-center">
                    <h3>
                        China</h3>
                </td>
                <td colspan="6" class="value align-center">
                    <h3>
                        Oversea</h3>
                </td>
            </tr>
            <tr>
                <td class="label align-center">
                    Tier I City
                </td>
                <td class="label align-center">
                    Tier II/III City
                </td>
                <td class="label align-center">
                    HongKong
                </td>
                <td class="label align-center">
                    FarEast
                </td>
                <td class="label align-center">
                    USA
                </td>
                <td class="label align-center">
                    Switzerland
                </td>
                <td class="label align-center">
                    UK
                </td>
                <td class="label align-center">
                    Europe & North Africa
                </td>
                <td class="value align-center">
                    Central & South America
                </td>
            </tr>
            <tr>
                <td class="label align-center">
                    RMB
                </td>
                <td class="label align-center">
                    RMB
                </td>
                <td class="label align-center">
                    RMB
                </td>
                <td class="label align-center">
                    EUR
                </td>
                <td class="label align-center">
                    USD
                </td>
                <td class="label align-center">
                    CHF
                </td>
                <td class="label align-center">
                    GBP
                </td>
                <td class="label align-center">
                    EUR
                </td>
                <td class="value align-center">
                    USD
                </td>
            </tr>
            <tr>
                <td class="label align-center">
                    Hotel
                </td>
                <td class="label align-center">
                    700
                </td>
                <td class="label align-center">
                    400
                </td>
                <td colspan="7" class="value align-center">
                    On actual reimbursement
                </td>
            </tr>
            <tr class="Meal">
                <td class="label align-right">
                    Meal Allowance<br />
                    Breakfast<br />
                    Lunch<br />
                    Dinner<br />
                </td>
                <td class="label align-center">
                    <br />
                    50<br />
                    50<br />
                    70<br />
                </td>
                <td class="label align-center">
                    <br />
                    50<br />
                    50<br />
                    70<br />
                </td>
                <td class="label align-center">
                    <br />
                    50<br />
                    50<br />
                    70<br />
                </td>
                <td class="label align-center">
                    <br />
                    20<br />
                    10<br />
                    20<br />
                </td>
                <td class="label align-center">
                    <br />
                    20<br />
                    10<br />
                    20<br />
                </td>
                <td class="label align-center">
                    <br />
                    30<br />
                    15<br />
                    30<br />
                </td>
                <td class="label align-center">
                    <br />
                    15<br />
                    10<br />
                    25<br />
                </td>
                <td class="label align-center">
                    <br />
                    15<br />
                    10<br />
                    30<br />
                </td>
                <td class="value align-center">
                    <br />
                    15<br />
                    10<br />
                    15<br />
                </td>
            </tr>
        </table>
    </div>
</div>
<div id="Hidden">
    <asp:HiddenField ID="hfCostCenter" runat="server"  Value=""/>
    <asp:HiddenField ID="hfExchangeRate" runat="server"  Value=""/>
    <asp:HiddenField ID="hfTravelPolicy" runat="server"  Value=""/>

    <asp:HiddenField ID="hfHotelForm" runat="server" Value="" />
    <asp:HiddenField ID="hfMealAllowanceForm" runat="server" Value="" />
    <asp:HiddenField ID="hfTransportationForm" runat="server" Value="" />
    <asp:HiddenField ID="hfSamplePurchaseForm" runat="server" Value="" />
    <asp:HiddenField ID="hfOtherForm" runat="server" Value="" />

    <asp:HiddenField ID="hfHotelSubTotal" runat="server" Value="" />
    <asp:HiddenField ID="hfMealSubTotal" runat="server" Value="" />
    <asp:HiddenField ID="hfTransSubTotal" runat="server" Value="" />
    <asp:HiddenField ID="hfSampleSubTotal" runat="server" Value="" />
    <asp:HiddenField ID="hfOthersSubTotal" runat="server" Value="" />
</div>
<script type="text/javascript" src="DataEdit/JS/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    function CheckSubmit() {
        CreateForbidDIV();
        var result = true;
        result = CheckHotel();
        if (!result) {
            ClearForbidDIV();
            return result;
        }
        result = CheckMealAllowance();
        if (!result) {
            return result;
        }
        result = CheckTransportation();
        if (!result) {
            ClearForbidDIV();
            return result;
        }
        result = CheckSamplePurchase();
        if (!result) {
            ClearForbidDIV();
            return result;
        }
        result = CheckOther();
        if (!result) {
            ClearForbidDIV();
            return result;
        }
        if ($("#Hotel_AddItemStatus").val() == "0"
           && $("#MealAllowanceItemStatus").val() == "0"
           && $("#Transportation_AddItemStatus").val() == "0"
           && $("#SamplePurchase_AddItemStatus").val() == "0"
           && $("#Other_AddItemStatus").val() == "0") {
            result = false;
            alert("You have unclaimed items. Please make sure they are related to trips applied through Travel Request. If not, please claim them here.");
        }
        return result;
    }

    function SetTotalCost() {
        var Hotel_SubTotal = $("#Hotel_SubTotal").val();
        var MealAllowance_SubTotal = $("#MealAllowance_SubTotal").val();
        var Transportation_SubTotal = $("#Transportation_SubTotal").val();
        var SamplePurchase_SubTotal = $("#SamplePurchase_SubTotal").val();
        var Other_SubTotal = $("#Other_SubTotal").val();

        var Hotel_PaidCredit = $("#Hotel_PaidCredit").val();
        var MealAllowance_PaidCredit = $("#MealAllowance_PaidCredit").val();
        var Transportation_PaidCredit = $("#Transportation_PaidCredit").val();
        var SamplePurchase_PaidCredit = $("#SamplePurchase_PaidCredit").val();
        var Other_PaidCredit = $("#Other_PaidCredit").val();

        var $txtTotalCost = $("#Summary input[id$='txtTotalCost']");
        var $txtCashAdvanced = $("#Summary input[id$='txtCashAdvanced']");
        var $txtPaidByCreditCard = $("#Summary input[id$='txtPaidByCreditCard']");
        var $txtNetPayable = $("#Summary input[id$='txtNetPayable']");
        var $txtTotalExceptFlight = $("#Summary input[id$='txtTotalExceptFlight']");
        var $txtComparedToApproved = $("#Summary input[id$='txtComparedToApproved']");

        var Total = 0;
        var TotalPaidCredit = 0;
        if (Hotel_SubTotal != "" && Hotel_SubTotal != "0.0") {
            Total += parseFloat(Hotel_SubTotal);
            TotalPaidCredit += parseFloat(Hotel_PaidCredit);
        }
        if (MealAllowance_SubTotal != "" && MealAllowance_SubTotal != "0.0") {
            Total += parseFloat(MealAllowance_SubTotal);
            TotalPaidCredit += parseFloat(MealAllowance_PaidCredit);
        }
        if (Transportation_SubTotal != "" && Transportation_SubTotal != "0.0") {
            Total += parseFloat(Transportation_SubTotal);
            TotalPaidCredit += parseFloat(Transportation_PaidCredit);
        }
        if (SamplePurchase_SubTotal != "" && SamplePurchase_SubTotal != "0.0") {
            Total += parseFloat(SamplePurchase_SubTotal);
            TotalPaidCredit += parseFloat(SamplePurchase_PaidCredit);
        }
        if (Other_SubTotal != "" && Other_SubTotal != "0.0") {
            Total += parseFloat(Other_SubTotal);
            TotalPaidCredit += parseFloat(Other_PaidCredit);
        }
        Total = Math.round(Total * Math.pow(10, 2)) / Math.pow(10, 2);
        TotalPaidCredit = Math.round(TotalPaidCredit * Math.pow(10, 2)) / Math.pow(10, 2);
        $txtTotalCost.val(Total.toFixed(2));
        $txtPaidByCreditCard.val(TotalPaidCredit.toFixed(2));

        var txtNetPayable = Total - parseInt($txtCashAdvanced.val()) - TotalPaidCredit;
        txtNetPayable = Math.round(txtNetPayable * Math.pow(10, 2)) / Math.pow(10, 2);
        $txtNetPayable.val(txtNetPayable.toFixed(2));

        if ($txtTotalExceptFlight.val() != "") {
            var ComparedToApproved = parseFloat($txtTotalExceptFlight.val()) - txtNetPayable;
            ComparedToApproved = Math.round(ComparedToApproved * Math.pow(10, 2)) / Math.pow(10, 2);
            if (ComparedToApproved < 0) {
                $txtComparedToApproved.css("color", "red");
            } else {
                $txtComparedToApproved.css("color", "#06c");
            }
            $txtComparedToApproved.val(ComparedToApproved.toFixed(2));
        }
    }

    String.prototype.replaceAll = function (s1, s2) {
        return this.replace(new RegExp(s1, "gm"), s2);
    }
</script>
<script type="text/javascript">
    function CheckHotel() {
        var result = true;
        var msg = "";
        var $AddItemStatus = $("#Hotel_AddItemStatus");
        if ($AddItemStatus.val() != "0") {
            var $ExpenseType = $("div.HotelItem li.ExpenseType select");
            $ExpenseType.each(function () {
                var ID = $(this).attr("id").replace("Hotel_ExpenseType", "");
                var ExpenseType = $(this).val();
                var FromDate = $("#Hotel_FromDate" + ID).val();
                var ToDate = $("#Hotel_ToDate" + ID).val();
                var CostCenter = $("#Hotel_CostCenter" + ID).val();
                var OriginalAmt = $("#Hotel_OriginalAmt" + ID).val();
                var ExchRate = $("#Hotel_ExchRate" + ID).val();
                var ClaimAmt = $("#Hotel_ClaimAmt" + ID).val();
                if (ExpenseType == "Select City") {
                    msg += "Please Select City In The Hotel Area.\n";
                    result = false;
                }
                if (FromDate == "" || ToDate == "") {
                    msg += "Please Select From Date Or To Date In The Hotel Area.\n";
                    result = false;
                }
                var StartDate = new Date(Date.parse(FromDate));
                var EndDate = new Date(Date.parse(ToDate));
                if (EndDate < StartDate) {
                    msg += "The To Date be less than the From Date In The Hotel Area.\n";
                    result = true;
                }
                if (CostCenter == "") {
                    msg += "Please Select Cost Center In The Hotel Area.\n";
                    result = false;
                }
                if (OriginalAmt == "") {
                    msg += "Please Input Original Amount In The Hotel Area.\n";
                    result = false;
                }
                if (ExchRate == "") {
                    msg += "Please Input Exchange Rate In The Hotel Area.\n";
                    result = false;
                }
                if (ClaimAmt == "") {
                    msg += "Please Input Original Amount In The Hotel Area.\n";
                    result = false;
                }
            });
            if (result) {
                UpdateHotelForm();
            }
        }
        if (msg != "") {
            alert(msg);
        }
        return result;
    }

    function UpdateHotelForm() {
        var $HotelForm = $("#Hidden input[id$='hfHotelForm']");
        $HotelForm.val("");
        var HotelForm = "[";
        var HotelSubTotal = 0;
        for (var i = 0; i < Hotel_AddItemCount; i++) {
            var $ExpenseType = $("#Hotel_ExpenseType" + i);
            if ($ExpenseType.length == 0) {
                continue;
            }
            var ID = i;
            var ExpenseType = $ExpenseType.val();
            var FromDate = $("#Hotel_FromDate" + ID).val();
            var ToDate = $("#Hotel_ToDate" + ID).val();
            var CostCenter = $("#Hotel_CostCenter" + ID).val();
            var OriginalAmt = $("#Hotel_OriginalAmt" + ID).val();
            var Currency = $("#Hotel_Currency" + ID).val();
            var OtherCurrency = $("#Hotel_OtherCurrency" + ID).val();
            var ExchRate = $("#Hotel_ExchRate" + ID).val();
            var ClaimAmt = $("#Hotel_ClaimAmt" + ID).val();
            HotelSubTotal += parseFloat(ClaimAmt);
            var ComStd = $("#Hotel_ComStd" + ID).val();
            var PaidCredit = $("#Hotel_PaidCredit" + ID).attr("checked") == true ? "1" : "0";
            var Remark = $("#Hotel_HotelRemark" + ID).val();
            Remark = Remark.replaceAll("\'", "\\\'")
                           .replaceAll(",", "，")
                           .replaceAll("<", "&lt;")
                           .replaceAll(">", "&gt;");
            HotelForm += "{" +
                               "ExpenseType:'" + ExpenseType + "'," +
                               "FromDate:'" + FromDate + "'," +
                               "ToDate:'" + ToDate + "'," +
                               "CostCenter:'" + CostCenter + "'," +
                               "OriginalAmt:'" + OriginalAmt + "'," +
                               "Currency:'" + Currency + "'," +
                               "OtherCurrency:'" + OtherCurrency + "'," +
                               "ExchRate:'" + ExchRate + "'," +
                               "ClaimAmt:'" + ClaimAmt + "'," +
                               "ComStd:'" + ComStd + "'," +
                               "PaidCredit:'" + PaidCredit + "'," +
                               "Remark:'" + Remark + "'" +
                          "},";
        }
        HotelForm += "]";
        $HotelForm.val(HotelForm);
        HotelSubTotal = Math.round(HotelSubTotal * Math.pow(10, 2)) / Math.pow(10, 2);
        $("#Hidden input[id$='hfHotelSubTotal']").val(HotelSubTotal.toFixed(2));
   }
</script>
<script type="text/javascript">
    function CheckMealAllowance() {
        var result = true;
        var msg = "";
        var $AddItemStatus = $("#MealAllowanceItemStatus");
        if ($AddItemStatus.val() != "0") {
            var $ExpenseType = $("div.MealAllowanceItem li.ExpenseType select");
            $ExpenseType.each(function () {
                var ID = $(this).attr("id").replace("MealAllowance_ExpenseType", "");
                var Date = $("#MealAllowance_Date" + ID).val();
                var CostCenter = $("#MealAllowance_CostCenter" + ID).val();
                var OriginalAmt = $("#MealAllowance_OriginalAmt" + ID).val();
                var ExchRate = $("#MealAllowance_ExchRate" + ID).val();
                var ClaimAmt = $("#MealAllowance_ClaimAmt" + ID).val();
                if (Date == "" ) {
                    msg += "Please Select Date In The Meal Allowance Area.\n";
                    result = false;
                }
                if (CostCenter == "") {
                    msg += "Please Select Cost Center In The Meal Allowance Area.\n";
                    result = false;
                }
                if (OriginalAmt == "") {
                    msg += "Please Input Original Amount In The Meal Allowance Area.\n";
                    result = false;
                }
                if (ExchRate == "") {
                    msg += "Please Input Exchange Rate In The Meal Allowance Area.\n";
                    result = false;
                }
                if (ClaimAmt == "") {
                    msg += "Please Input Original Amount In The Meal Allowance Area.\n";
                    result = false;
                }
            });
            if (result) {
                UpdateMealAllowanceForm();
            }
        }
        if (msg != "") {
            alert(msg);
        }
        return result;
    }

    function UpdateMealAllowanceForm() {
        var $MealAllowanceForm = $("#Hidden input[id$='hfMealAllowanceForm']");
        $MealAllowanceForm.val("");
        var MealAllowanceForm = "[";
        var MealSubTotal = 0;
        for (var i = 0; i < MealAllowance_AddItemCount; i++) {
            var $ExpenseType = $("#MealAllowance_ExpenseType" + i);
            if ($ExpenseType.length == 0) {
                continue;
            }
            var ID = i;
            var ExpenseType = $ExpenseType.val();
            var Date = $("#MealAllowance_Date" + ID).val();
            var CostCenter = $("#MealAllowance_CostCenter" + ID).val();
            var OriginalAmt = $("#MealAllowance_OriginalAmt" + ID).val();
            var Currency = $("#MealAllowance_Currency" + ID).val();
            var OtherCurrency = $("#MealAllowance_OtherCurrency" + ID).val();
            var ExchRate = $("#MealAllowance_ExchRate" + ID).val();
            var ClaimAmt = $("#MealAllowance_ClaimAmt" + ID).val();
            MealSubTotal += parseFloat(ClaimAmt);
            var ComStd = $("#MealAllowance_ComStd" + ID).val();
            var PaidCredit = $("#MealAllowance_PaidCredit" + ID).attr("checked") == true ? "1" : "0";
            var Remark = $("#MealAllowance_MealAllowanceRemark" + ID).val();
            Remark = Remark.replaceAll("\'", "\\\'")
                           .replaceAll(",", "，")
                           .replaceAll("<", "&lt;")
                           .replaceAll(">", "&gt;");
            MealAllowanceForm += "{" +
                               "ExpenseType:'" + ExpenseType + "'," +
                               "Date:'" + Date + "'," +
                               "CostCenter:'" + CostCenter + "'," +
                               "OriginalAmt:'" + OriginalAmt + "'," +
                               "Currency:'" + Currency + "'," +
                               "OtherCurrency:'" + OtherCurrency + "'," +
                               "ExchRate:'" + ExchRate + "'," +
                               "ClaimAmt:'" + ClaimAmt + "'," +
                               "ComStd:'" + ComStd + "'," +
                               "PaidCredit:'" + PaidCredit + "'," +
                               "Remark:'" + Remark + "'" +
                          "},";
        }
        MealAllowanceForm += "]";
        $MealAllowanceForm.val(MealAllowanceForm);
        MealSubTotal = Math.round(MealSubTotal * Math.pow(10, 2)) / Math.pow(10, 2);
        $("#Hidden input[id$='hfMealSubTotal']").val(MealSubTotal.toFixed(2));
    }
</script>
<script type="text/javascript">
    function CheckTransportation() {
        var result = true;
        var msg = "";
        var $AddItemStatus = $("#Transportation_AddItemStatus");
        if ($AddItemStatus.val() != "0") {
            var $ExpenseType = $("div.TransportationItem li.ExpenseType input");
            $ExpenseType.each(function () {
                var ID = $(this).attr("id").replace("Transportation_ExpenseType", "");
                var Date = $("#Transportation_Date" + ID).val();
                var CostCenter = $("#Transportation_CostCenter" + ID).val();
                var OriginalAmt = $("#Transportation_OriginalAmt" + ID).val();
                var ExchRate = $("#Transportation_ExchRate" + ID).val();
                var ClaimAmt = $("#Transportation_ClaimAmt" + ID).val();
                if (Date == "") {
                    msg += "Please Select Date In The Transportation Area.\n";
                    result = false;
                }
                if (CostCenter == "") {
                    msg += "Please Select Cost Center In The Transportation Area.\n";
                    result = false;
                }
                if (OriginalAmt == "") {
                    msg += "Please Input Original Amount In The Transportation Area.\n";
                    result = false;
                }
                if (ExchRate == "") {
                    msg += "Please Input Exchange Rate In The Transportation Area.\n";
                    result = false;
                }
                if (ClaimAmt == "") {
                    msg += "Please Input Original Amount In The Transportation Area.\n";
                    result = false;
                }
            });
            if (result) {
                UpdateTransportationForm();
            }
        }
        if (msg != "") {
            alert(msg);
        }
        return result;
    }

    function UpdateTransportationForm() {
        var $TransportationForm = $("#Hidden input[id$='hfTransportationForm']");
        $TransportationForm.val("");
        var TransportationForm = "[";
        var TransSubTotal = 0;
        for (var i = 0; i < Transportation_AddItemCount; i++) {
            var $ExpenseType = $("#Transportation_ExpenseType" + i);
            if ($ExpenseType.length == 0) {
                continue;
            }
            var ID = i;
            var ExpenseType = $ExpenseType.val();
            ExpenseType = ExpenseType.replaceAll("\'", "\\\'")
                                     .replaceAll(",", "，")
                                     .replaceAll("<", "&lt;")
                                     .replaceAll(">", "&gt;");
            var Date = $("#Transportation_Date" + ID).val();
            var CostCenter = $("#Transportation_CostCenter" + ID).val();
            var OriginalAmt = $("#Transportation_OriginalAmt" + ID).val();
            var Currency = $("#Transportation_Currency" + ID).val();
            var OtherCurrency = $("#Transportation_OtherCurrency" + ID).val();
            var ExchRate = $("#Transportation_ExchRate" + ID).val();
            var ClaimAmt = $("#Transportation_ClaimAmt" + ID).val();
            TransSubTotal += parseFloat(ClaimAmt);
            var ComStd = $("#Transportation_ComStd" + ID).val();
            var PaidCredit = $("#Transportation_PaidCredit" + ID).attr("checked") == true ? "1" : "0";
            TransportationForm += "{" +
                               "ExpenseType:'" + ExpenseType + "'," +
                               "Date:'" + Date + "'," +
                               "CostCenter:'" + CostCenter + "'," +
                               "OriginalAmt:'" + OriginalAmt + "'," +
                               "Currency:'" + Currency + "'," +
                               "OtherCurrency:'" + OtherCurrency + "'," +
                               "ExchRate:'" + ExchRate + "'," +
                               "ClaimAmt:'" + ClaimAmt + "'," +
                               "ComStd:'" + ComStd + "'," +
                               "PaidCredit:'" + PaidCredit + "'" +
                         "},";
        }
        TransportationForm += "]";
        $TransportationForm.val(TransportationForm);
        TransSubTotal = Math.round(TransSubTotal * Math.pow(10, 2)) / Math.pow(10, 2);
        $("#Hidden input[id$='hfTransSubTotal']").val(TransSubTotal.toFixed(2));
    }
</script>
<script type="text/javascript">
    function CheckSamplePurchase() {
        var result = true;
        var msg = "";
        var $AddItemStatus = $("#SamplePurchase_AddItemStatus");
        if ($AddItemStatus.val() != "0") {
            var $ExpenseType = $("div.SamplePurchaseItem li.ExpenseType input");
            $ExpenseType.each(function () {
                var ID = $(this).attr("id").replace("SamplePurchase_ExpenseType", "");
                var Date = $("#SamplePurchase_Date" + ID).val();
                var CostCenter = $("#SamplePurchase_CostCenter" + ID).val();
                var OriginalAmt = $("#SamplePurchase_OriginalAmt" + ID).val();
                var ExchRate = $("#SamplePurchase_ExchRate" + ID).val();
                var ClaimAmt = $("#SamplePurchase_ClaimAmt" + ID).val();
                if (Date == "") {
                    msg += "Please Select Date In The Sample Purchase Area.\n";
                    result = false;
                }
                if (CostCenter == "") {
                    msg += "Please Select Cost Center In The Sample Purchase Area.\n";
                    result = false;
                }
                if (OriginalAmt == "") {
                    msg += "Please Input Original Amount In The Sample Purchase Area.\n";
                    result = false;
                }
                if (ExchRate == "") {
                    msg += "Please Input Exchange Rate In The Sample Purchase Area.\n";
                    result = false;
                }
                if (ClaimAmt == "") {
                    msg += "Please Input Original Amount In The Sample Purchase Area.\n";
                    result = false;
                }
            });
            if (result) {
                UpdateSamplePurchaseForm();
            }
        }
        if (msg != "") {
            alert(msg);
        }
        return result;
    }

    function UpdateSamplePurchaseForm() {
        var $SamplePurchaseForm = $("#Hidden input[id$='hfSamplePurchaseForm']");
        $SamplePurchaseForm.val("");
        var SamplePurchaseForm = "[";
        var SampleSubTotal = 0;
        for (var i = 0; i < SamplePurchase_AddItemCount; i++) {
            var $ExpenseType = $("#SamplePurchase_ExpenseType" + i);
            if ($ExpenseType.length == 0) {
                continue;
            }
            var ID = i;
            var ExpenseType = $ExpenseType.val();
            ExpenseType = ExpenseType.replaceAll("\'", "\\\'")
                                     .replaceAll(",", "，")
                                     .replaceAll("<", "&lt;")
                                     .replaceAll(">", "&gt;");
            var Date = $("#SamplePurchase_Date" + ID).val();
            var CostCenter = $("#SamplePurchase_CostCenter" + ID).val();
            var OriginalAmt = $("#SamplePurchase_OriginalAmt" + ID).val();
            var Currency = $("#SamplePurchase_Currency" + ID).val();
            var OtherCurrency = $("#SamplePurchase_OtherCurrency" + ID).val();
            var ExchRate = $("#SamplePurchase_ExchRate" + ID).val();
            var ClaimAmt = $("#SamplePurchase_ClaimAmt" + ID).val();
            SampleSubTotal += parseFloat(ClaimAmt);
            var ComStd = $("#SamplePurchase_ComStd" + ID).val();
            var PaidCredit = $("#SamplePurchase_PaidCredit" + ID).attr("checked") == true ? "1" : "0";
            SamplePurchaseForm += "{" +
                               "ExpenseType:'" + ExpenseType + "'," +
                               "Date:'" + Date + "'," +
                               "CostCenter:'" + CostCenter + "'," +
                               "OriginalAmt:'" + OriginalAmt + "'," +
                               "Currency:'" + Currency + "'," +
                               "OtherCurrency:'" + OtherCurrency + "'," +
                               "ExchRate:'" + ExchRate + "'," +
                               "ClaimAmt:'" + ClaimAmt + "'," +
                               "ComStd:'" + ComStd + "'," +
                               "PaidCredit:'" + PaidCredit + "'" +
                         "},";
        }
        SamplePurchaseForm += "]";
        $SamplePurchaseForm.val(SamplePurchaseForm);
        SampleSubTotal = Math.round(SampleSubTotal * Math.pow(10, 2)) / Math.pow(10, 2);
        $("#Hidden input[id$='hfSampleSubTotal']").val(SampleSubTotal.toFixed(2));
    }
</script>
<script type="text/javascript">
    function CheckOther() {
        var result = true;
        var msg = "";
        var $AddItemStatus = $("#Other_AddItemStatus");
        if ($AddItemStatus.val() != "0") {
            var $ExpenseType = $("div.OtherItem li.ExpenseType input");
            $ExpenseType.each(function () {
                var ID = $(this).attr("id").replace("Other_ExpenseType", "");
                var Date = $("#Other_Date" + ID).val();
                var CostCenter = $("#Other_CostCenter" + ID).val();
                var OriginalAmt = $("#Other_OriginalAmt" + ID).val();
                var ExchRate = $("#Other_ExchRate" + ID).val();
                var ClaimAmt = $("#Other_ClaimAmt" + ID).val();
                if (Date == "") {
                    msg += "Please Select Date In The Others Area.\n";
                    result = false;
                }
                if (CostCenter == "") {
                    msg += "Please Select Cost Center In The Others Area.\n";
                    result = false;
                }
                if (OriginalAmt == "") {
                    msg += "Please Input Original Amount In The Others Area.\n";
                    result = false;
                }
                if (ExchRate == "") {
                    msg += "Please Input Exchange Rate In The Others Area.\n";
                    result = false;
                }
                if (ClaimAmt == "") {
                    msg += "Please Input Original Amount In The Others Area.\n";
                    result = false;
                }
            });
            if (result) {
                UpdateOtherForm();
            }
        }
        if (msg != "") {
            alert(msg);
        }
        return result;
    }

    function UpdateOtherForm() {
        var $OtherForm = $("#Hidden input[id$='hfOtherForm']");
        $OtherForm.val("");
        var OtherForm = "[";
        var OthersSubTotal = 0;
        for (var i = 0; i < Other_AddItemCount; i++) {
            var $ExpenseType = $("#Other_ExpenseType" + i);
            if ($ExpenseType.length == 0) {
                continue;
            }
            var ID = i;
            var ExpenseType = $ExpenseType.val();
            ExpenseType = ExpenseType.replaceAll("\'", "\\\'")
                                     .replaceAll(",", "，")
                                     .replaceAll("<", "&lt;")
                                     .replaceAll(">", "&gt;");
            var Date = $("#Other_Date" + ID).val();
            var CostCenter = $("#Other_CostCenter" + ID).val();
            var OriginalAmt = $("#Other_OriginalAmt" + ID).val();
            var Currency = $("#Other_Currency" + ID).val();
            var OtherCurrency = $("#Other_OtherCurrency" + ID).val();
            var ExchRate = $("#Other_ExchRate" + ID).val();
            var ClaimAmt = $("#Other_ClaimAmt" + ID).val();
            OthersSubTotal += parseFloat(ClaimAmt);
            var ComStd = $("#Other_ComStd" + ID).val();
            var PaidCredit = $("#Other_PaidCredit" + ID).attr("checked") == true ? "1" : "0";
            OtherForm += "{" +
                               "ExpenseType:'" + ExpenseType + "'," +
                               "Date:'" + Date + "'," +
                               "CostCenter:'" + CostCenter + "'," +
                               "OriginalAmt:'" + OriginalAmt + "'," +
                               "Currency:'" + Currency + "'," +
                               "OtherCurrency:'" + OtherCurrency + "'," +
                               "ExchRate:'" + ExchRate + "'," +
                               "ClaimAmt:'" + ClaimAmt + "'," +
                               "ComStd:'" + ComStd + "'," +
                               "PaidCredit:'" + PaidCredit + "'" +
                         "},";
        }
        OtherForm += "]";
        $OtherForm.val(OtherForm);
        OthersSubTotal = Math.round(OthersSubTotal * Math.pow(10, 2)) / Math.pow(10, 2);
        $("#Hidden input[id$='hfOthersSubTotal']").val(OthersSubTotal.toFixed(2));
    }
</script>