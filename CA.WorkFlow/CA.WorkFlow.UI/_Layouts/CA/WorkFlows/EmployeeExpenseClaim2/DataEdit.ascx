<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.EmployeeExpenseClaim2.DataEdit" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
   .ContentDiv
    {
        padding: 0px;
        margin: 0px;
        position: relative;
    }
    #ErrorDiv
    {
         margin: 0 auto;
        width: 300px;
        position: absolute;
        left: 30%;
        top: 22%;
        display: none;
        border: 1px solid gray;
        background-color: Gray;
        padding: 5px;
        font-size: 14px;
        z-index: 1000001;
    }
    #ErrorMsg
    {
        background-color: White;
        color: Red;
        width: 290px;
        float: left;
        line-height: 22px;
        padding: 5px;
    }
    #title
    {
        float: left;
        width: 290px;
        border-bottom: 1px solid Gray;
        font-weight: bold;
        cursor: pointer;
    }
    #left
    {
        float: left;
        color: #3d3d3d;
        padding: 5px;
        padding-top: 3px;
    }
    #right
    {
        color: #3d3d3d;
        float: right;
        padding: 5px;
        padding-top: 3px;
        display: none;
    }
    #msg
    {
        color:Black;
        float: left;
        width: 290px;
        padding: 5px;
         font-size:14px;
    }
    #alert
    {
        float: left;
        width: 300px;
        font-weight: bold;
        cursor: pointer;
        text-align: center;
        padding: 20px 0px 20px 0px;
    }
    #alertleft
    {
        color: #3d3d3d;
        float: left;
        width: 50%;
        text-align:center;
    }
    #alertright
    {
        color: red;
        float: right;
        width: 50%;
        text-align:center;
    }
    #bgDiv
    {
        position: absolute;
        top: 0px;
        left: 0px;
        right: 0px;
        bottom: 0px;
        display: none;
        background-color: White;
        filter: Alpha(opacity=30);
        z-index: 1000000;
    }
    .close
    {
        color: Red;
    }
    .ca-workflow-form-table td
    {
        padding: 5px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        text-align: center;
        margin: 0;
        height: 25px;
        line-height: 15px;
    }
    .pftd td
    {
        padding: 0px;
        border: none;
        text-align: left;
        margin: 0;
        height: auto;
        line-height: normal;
    }
    .pftd img
    {
        margin-top: 3px;
    }
    .ca-workflow-form-table
    {
        margin-top: 20px;
    }
    .datetime td, .attachment td
    {
        border-bottom: none;
        border-right: none;
    }
    
    .CostCenterTD
    {
        width: 260px;
    }
    .ExpenseTypeTD
    {
        width: 230px;
    }
    
    .cashadvancediv
    {
        padding: 0px;
        margin: 0px;
        width: auto;
        height: auto;
        position: relative;
    }
    .cardiv
    {
        padding: 0px;
        margin: 0px;
        width: 228px;
        height: auto;
        position: absolute;
        left: 0px;
        top: 27px;
        border: 1px solid #999999;
        border-top: none;
        background-color: White;
        display: none;
    }
    .cardivscroll
    {
        height: 400px;
        overflow-x: hidden;
        overflow-y: scroll;
        scrollbar-face-color: #e4e4e5;
    }
    .titlediv
    {
        padding-top: 8px;
        padding-left: 5px;
        margin: 0px;
        width: 230px;
        height: 20px;
        position: absolute;
        left: 0px;
        top: 0px;
        background-image: url(../../../CAResources/themeCA/images/carwf_select.gif);
        background-repeat: no-repeat;
        cursor: pointer;
    }
    .cashadvancediv input
    {
        width: auto;
        border: 1px solid #999999;
        margin-right: 5px;
    }
    .cashadvancediv ul li
    {
        list-style: none;
        width: 218px !important;
        width: 220px;
        padding: 5px;
        cursor: pointer;
    }
    .titledivbg
    {
        background-image: url(../../../CAResources/themeCA/images/carwf_selectup.gif);
    }
    .carli
    {
        background-color: #e4e4e5;
    }
    .ca-workflow-form-table .ms-dtinput input
    {
        width: 60px;
    }
    .ca-workflow-form-table select
    {
        margin-left: 0px;
        width: 100%;
    }
    .selectDateDiv
    {
        background-repeat: no-repeat;
        height: 20px;
        padding: 20px;
        position: absolute;
        left: 5px;
        top: 32px;
        width: 290px;
        display: none;
        z-index: 10000;
        text-align: left;
        cursor: pointer;
        border: #9dabb6 2px solid;
        background-color: White;
        font-size: 14px;
        font-weight: bold;
    }
    .dateclose
    {
        margin: 0px 0px 0px 15px;
        font-size: 15px;
        cursor: pointer;
        font-weight: bold;
        color: Red;
    }
    .dateok
    {
        margin: 0px 0px 0px 15px;
        font-size: 15px;
        cursor: pointer;
        font-weight: bold;
    }
    .ss
    {
        margin-left: 0 !important;
        margin-left: -50px;
    }
    .ExpenseType1
    {
        position: absolute;
        left: 0px !important;
        left: -35px;
        top: 0px;
        z-index: 100000;
    }
    .CostCenter
    {
        position: absolute;
        left: 0px !important;
        left: -30px;
    }
    .ca-workflow-form-table1 input
    {
        padding-left: 0px;
        padding-right: 0px;
    }
    .ca-workflow-form-table1 td
    {
        padding: 5px 2px 5px 2px;
    }
    .w43
    {
        width: 41%;
    }
    .w12
    {
        width: 14%;
    }
    .summarytype
    {
        display: none;
        border: none;
    }
    .wrapdiv
    {
        padding: 2px;
        border: 1px solid red;
    }
</style>
<table class="ca-workflow-form-table">
    <tr>
        <td colspan="4">
            <h3>
                Employee Expense Claim Form<br />
                员工报销申请表
            </h3>
        </td>
    </tr>
    <tr>
        <td class="label align-center">
            Choose Employee<br />
            选择员工
        </td>
        <td class="pftd value" colspan="3">
            <cc1:CAPeopleFinder ID="cpfUser" runat="server" AllowTypeIn="true" MultiSelect="false"
                CssClass="ca-people-finder" Width="200" />
        </td>
    </tr>
    <tr>
        <td class="w20">
            Dept<br />
            部门
        </td>
        <td class="w30">
            <asp:Label ID="lbDept" runat="server"></asp:Label>
        </td>
        <td class="w20">
            Requested By<br />
            申请人
        </td>
        <td class="w30">
            <asp:Label ID="lbRequestedBy" runat="server" CssClass="lbRequestedBy"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Claims Description<br />
            报销描述
        </td>
        <td colspan="3" class="ExpenseDescription">
            <QFL:FormField ID="ffExpenseDescription" runat="server" FieldName="ExpenseDescription"
                ControlMode="Edit" CssClass="ffExpenseDescription">
            </QFL:FormField>
        </td>
    </tr>
</table>
<asp:Button ID="btnPeopleInfo" runat="server" OnClick="btnPeopleInfo_Click" CausesValidation="False"
    CssClass="hidden" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table class="ca-workflow-form-table ca-workflow-form-table1" id="expenseTypetable">
            <tr>
                <td colspan="7">
                    <h3>
                        Employee expense claim details</h3>
                </td>
            </tr>
            <tr>
                <td class="w5">
                    <asp:ImageButton runat="server" ID="btnAddItem" ToolTip="Click to add the information."
                        ImageUrl="../images/pixelicious_001.png" OnClick="btnAddItem_Click" Width="18"
                        CssClass="img-button" />
                </td>
                <td class="w10">
                    ExpenseType<br />
                    费用类别
                </td>
                <td class="w5">
                    Date<br />
                    日期
                </td>
                <td class="w43">
                    Expense Purpose<br />
                    费用用途
                </td>
                <td class="w10">
                    CostCenter<br />
                    成本中心
                </td>
                <td class="w15">
                    Amount<br />
                    金额(RMB)
                </td>
                <td class="w12">
                    Company Std<br />公司标准
                </td>
            </tr>
            <asp:Repeater ID="rptItem" runat="server" OnItemCommand="rptItem_ItemCommand" OnItemDataBound="rptItem_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="w5">
                            <asp:ImageButton ID="btnDeleteItem" ToolTip="Remove this information." CommandName="delete"
                                runat="server" ImageUrl="../images/pixelicious_028.png" Width="18" CssClass="img-button" />
                        </td>
                        <td>
                            <div style="position: relative; z-index: 100000; background-color: White;">
                                <div class="ExpenseType ExpenseType1">
                                    <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="ExpenseType" onchange="SetRemarkInfo(this);ChangeLocation(this);DrawSummaryExpenseTable();return SelectExpenseType(this);">
                                        <asp:ListItem Text="" Value="0" Selected="True" />
                                        <asp:ListItem Text="Consulting others" Value="Consulting others"></asp:ListItem>
                                        <asp:ListItem Text="Entertainment - food" Value="Entertainment - food"></asp:ListItem>
                                        <asp:ListItem Text="Entertainment - gift" Value="Entertainment - gift"></asp:ListItem>
                                       
                                        <asp:ListItem Text="Government charges - visa application" Value="Government charges - visa application"></asp:ListItem>
                                        <asp:ListItem Text="IT HW/SW maintenance" Value="IT HW/SW maintenance"></asp:ListItem>
                                        <asp:ListItem Text="Internet related" Value="Internet related"></asp:ListItem>
                                        <asp:ListItem Text="Local transportation" Value="Local transportation"></asp:ListItem>
                                        <asp:ListItem Text="Meeting" Value="Meeting"></asp:ListItem>
                                        <asp:ListItem Text="Magazine/newspaper" Value="Magazine/newspaper"></asp:ListItem>
                                        <asp:ListItem Text="Mobile – local call and others" Value="Mobile – local call and others" />
                                        <asp:ListItem Text="Mobile – long distance call" Value="Mobile – long distance call" />
                                        <asp:ListItem Text="Office stationery" Value="Office stationery"></asp:ListItem>
                                        <asp:ListItem Text="Office drink water" Value="Office drink water"></asp:ListItem>
                                        <asp:ListItem Text="Other office supply" Value="Other office supply"></asp:ListItem>
                                        <asp:ListItem Text="OT - meal allowance" Value="OT - meal allowance"></asp:ListItem>
                                        <asp:ListItem Text="OT - local transportation" Value="OT - local transportation"></asp:ListItem>
                                        <asp:ListItem Text="Postage" Value="Postage"></asp:ListItem>
                                        <asp:ListItem Text="PT payroll" Value="PT payroll"></asp:ListItem>
                                        <asp:ListItem Text="Penalty" Value="Penalty"></asp:ListItem>
                                        <asp:ListItem Text="Sample purchase" Value="Sample purchase"></asp:ListItem>
                                        <asp:ListItem Text="Telephone - land line" Value="Telephone - land line"></asp:ListItem>
                                        <asp:ListItem Text="Training" Value="Training"></asp:ListItem>
                                        <asp:ListItem Text="Travel - hotel" Value="Travel - hotel"></asp:ListItem>
                                        <asp:ListItem Text="Travel - local transportation" Value="Travel - local transportation"></asp:ListItem>
                                        <asp:ListItem Text="Travel - meal" Value="Travel - meal"></asp:ListItem>
                                        <asp:ListItem Text="Travel - others" Value="Travel - others"></asp:ListItem>
                                        <asp:ListItem Text="Travel - airticket/train/bus" Value="Travel - airticket/train/bus"></asp:ListItem>
                                        <asp:ListItem Text="Others (specify)" Value="Others (specify)"></asp:ListItem>
                                        
                                        <asp:ListItem Text="Store mgnt exp - local transportation" Value="Store mgnt exp - local transportation"></asp:ListItem>
                                        <asp:ListItem Text="Store staff exp - local transportation" Value="Store staff exp - local transportation"></asp:ListItem>
                                        <asp:ListItem Text="Store exp - travel - transportation" Value="Store exp - travel - transportation"></asp:ListItem>
                                        <asp:ListItem Text="Store exp - travel hotel" Value="Store exp - travel hotel"></asp:ListItem>
                                        <asp:ListItem Text="Store exp - travel food" Value="Store exp - travel food"></asp:ListItem>
                                        <asp:ListItem Text="Store exp - entertainment food" Value="Store exp - entertainment food"></asp:ListItem>
                                        <asp:ListItem Text="Store exp - entertainment gift" Value="Store exp - entertainment gift"></asp:ListItem>
                                        <asp:ListItem Text="Store mgnt exp - training" Value="Store mgnt exp - training"></asp:ListItem>
                                        <asp:ListItem Text="Store staff exp - training" Value="Store staff exp - training"></asp:ListItem>
                                        <asp:ListItem Text="Store mgnt exp - mobile phone local calls and others" Value="Store mgnt exp - mobile phone local calls and others"></asp:ListItem>
                                        <asp:ListItem Text="Store mgnt exp - mobile phone long distance calls" Value="Store mgnt exp - mobile phone long distance calls"></asp:ListItem>
                                        <asp:ListItem Text="Store mgnt exp - internet related" Value="Store mgnt exp - internet related"></asp:ListItem>
                                        <asp:ListItem Text="Store exp - meeting" Value="Store exp - meeting"></asp:ListItem>
                                        <asp:ListItem Text="Store mgnt exp - rental" Value="Store mgnt exp - rental"></asp:ListItem>
                                        <asp:ListItem Text="Store mgnt exp - other benefits" Value="Store mgnt exp - other benefits"></asp:ListItem>
                                        <asp:ListItem Text="Store exp - others" Value="Store exp - others"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hidremarkstatus1" runat="server" Value="0" />
                                </div>
                            </div>
                        </td>
                        <td class="datetime">
                            <div class="selectDate" style="position: relative; background-color: White; z-index: 10000;
                                width: 86px; padding-top: 5px; padding-bottom: 5px; height: 25px">
                                <asp:TextBox ID="txtDate" runat="server" Style="width: 55px;"></asp:TextBox>
                                <img src="../../../CAResources/themeCA/images/selectcalendar.gif" style="margin-left: 5px;
                                    cursor: pointer; margin-top: 6px" align="absmiddle" />
                                <div class="selectDateDiv">
                                    Select Date：<select style="width: 60px; margin-right: 8px;" class="year">
                                        <option>2010</option>
                                        <option>2011</option>
                                        <option>2012</option>
                                        <option>2013</option>
                                        <option>2014</option>
                                        <option>2015</option>
                                        <option>2016</option>
                                        <option>2017</option>
                                        <option>2018</option>
                                        <option>2019</option>
                                        <option>2020</option>
                                    </select><select style="width: 40px; margin-left: 8px; margin-right: 11px;" class="month">
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                        <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                        <option>11</option>
                                        <option>12</option>
                                    </select><a class="dateok">[Done]</a><a class="dateclose" style="display: none">[X]</a>
                                </div>
                            </div>
                            <div class="DateTimeControl" style="display: none">
                                <cc1:CADateTimeControl ID="dtDates" runat="server" DateOnly="true" CssClassTextBox="DateTimeControl" />
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtExpensePurpose" runat="server" CssClass="ExpensePurpose"></asp:TextBox>
                        </td>
                        <td>
                            <div style="position: relative; background-color: White; z-index: 500">
                                <div class="CostCenter">
                                    <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="cc" onchange="DrawSummaryExpenseTable()">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td class="w10">
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="Amount" onchange="DrawSummaryExpenseTable();return CalcTotal();"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbCompanyStandard" runat="server" />
                            <asp:HiddenField ID="hidCompanyStandard" runat="server" Value="-1" />
                            <asp:HiddenField ID="hidddlTotalAmount" runat="server" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td colspan="2" style="text-align: center">
                            Remark<br />
                            备注
                        </td>
                        <td colspan="5" align="left" style="text-align: left">
                            <asp:TextBox ID="txtRemark" runat="server" CssClass="remark"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="height: 20px;">
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td style="height: 20px; border: none">
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<table class="ca-workflow-form-table summarytypetable" >
  
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
        <td class="w30" style="border-right: none">
        </td>
        <td class="w30" style="border-right: none">
        </td>
        <td class="w40">
        </td>
    </tr>--%>
    <tr>
        <td  style=" border-top:#ccc 2px solid">
            Total Amount<br />
            总金额(RMB)
        </td>
          <td  style=" border-top:#ccc 2px solid"  colspan="2">
            <asp:Label ID="lbTotalAmount" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td >
            Cash Advance<br />
            员工借款(RMB)
            <asp:DropDownList ID="ddlTotalAmount" runat="server" CssClass="width-fix" Style="width: 170px;"
                Visible="false">
            </asp:DropDownList>
            <asp:HiddenField ID="hidTotalAmount" runat="server" Value="0" />
        </td>
        <td style=" height:40px; text-align:left"  align="center"  colspan="2">
            <div class="cashadvancediv" style=" left:65px">
                <div class="titlediv" runat="server" id="titlediv">
                    Select Cash Advance to be deducted</div>
                <div class="cardiv" runat="server" id="cardiv">
                </div>
            </div>
        </td>
    </tr>
    <tr>
        <td style="border-top: #ccc 2px solid">
            Payable to Employee/(Refund to Finance)<br />
            应付员工/(财务应收) (RMB)
        </td>
         <td  style=" border-top:#ccc 2px solid"  colspan="2">
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
        <td >
            <ul>
                <li>
                    <asp:RadioButton ID="rbAttInv" GroupName="AttachInvoice" Text="Yes" Checked="true"
                        runat="server" CssClass="radio" /></li>
                <li>
                    <asp:RadioButton ID="rbNonAttInv" GroupName="AttachInvoice" Text="No" runat="server"
                        CssClass="radio" /></li>
            </ul>
        </td>
    </tr>
    <tr>
         <td >
            Remark<br />
            备注
        </td>
       <td >
            <QFL:FormField ID="ffRemark" runat="server" FieldName="Remark" ControlMode="Edit">
            </QFL:FormField>
        </td>
    </tr>
    <tr>
         <td>
            Attachment<br />
            附件
        </td>
        <td  class="attachment" style="text-align: left;">
            <QFL:FormAttachments runat="server" ID="attacthment">
            </QFL:FormAttachments>
        </td>
    </tr>
</table>
<div id="ErrorDiv">
    <div id="ErrorMsg">
        <div id="title">
            <div id="left">
                Notice</div>
            <div id="right">
                X</div>
        </div>
        <div id="msg">
        </div>
        <div id="alert">
            <div id="alertleft">
                <a onclick="SetAlert();" class="okbtn" style="color: #3d3d3d; font-size: 16px;">
                    Yes</a></div>
            <div id="alertright">
                <a style="color: Red; font-size: 16px;" class="nobtn" onclick="CloseAlert();">No</a></div>
        </div>
    </div>
</div>
<div id="bgDiv">
</div>
<asp:HiddenField ID="hidMobileStd" runat="server" Value="" />
<asp:HiddenField ID="hidOTMealStd" runat="server" Value="" />
<asp:HiddenField ID="jobLevelNumber" runat="server" Value="" />
<asp:HiddenField ID="hidcashadvance" runat="server" Value="" />
<asp:HiddenField ID="hidremarkstatus" runat="server" Value="0" />
<asp:HiddenField ID="hidcashadvancewfid" runat="server" Value="" />
<asp:HiddenField ID="hidSummaryExpenseType" runat="server" Value="" />

<input id="hidLocationType" type="hidden"  value=""/>

<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    function ResumeError() { return true; }window.onerror = ResumeError;
</script>
<script type="text/javascript">
    var JSExpenseClaim = {};

    $(function () {
        //BindClick();
        JSExpenseClaim.SetCompanyStd();
        CalcTotal();
        DrawSummaryExpenseTable(); BindPeopleFind()
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //        var $ddlTotalAmount = $('#<%=this.ddlTotalAmount.ClientID %>');
        //        $ddlTotalAmount.change(function () {
        //            if ($ddlTotalAmount.val() != "") {
        //                var $hidTotalAmount = $('#<%=this.hidTotalAmount.ClientID %>');
        //                $hidTotalAmount.val($ddlTotalAmount.val());
        //                GetTotalAmount();
        //            }
        //        });
        BindEvent();
        var $titlediv = $(".titlediv");
        var $cardiv = $(".cardiv");
        $titlediv.click(function () {
            if ($titlediv.html() == "No Cash Advance") {
                return false;
            }
            if ($cardiv.css("display") == "block") {
                $cardiv.slideUp("slow");
                $titlediv.removeClass("titledivbg");
                $cardiv.removeClass("cardivscroll");
            } else {
                var $carul = $(".cardiv ul li");
                if ($carul.length > 12) {
                    $cardiv.height(364);
                    $cardiv.addClass("cardivscroll");
                }
                $cardiv.slideDown("slow");
                $titlediv.addClass("titledivbg");
            }
        });
        var $carli = $(".cardiv ul li");
        $carli.each(function () {
            $(this).mousemove(function () {
                $(this).addClass("carli");
            });
            $(this).mouseout(function () {
                $(this).removeClass("carli");
            });
        });
        var $carliinput = $(".cardiv ul li input");
        $carliinput.each(function () {
            $(this).click(function () {
                CalCashAdvance();
            });
        });

        BindImgDate();

        Selectdate();

        BlurTime();

    });
    
    function BlurTime() {
        var $datetime = $(".datetime");
        $datetime.each(function () {
            var $txttime = $(this).find("input");
            
            $txttime.each(function () {

                $(this).focus(function () {
                    this.blur();

                });
            });

        });
    
    }

    function Selectdate() {
        var $dateclose = $(".dateclose");
        $dateclose.each(function () {
            $(this).click(function () {
                $(this).parent().hide();
            });
        });
        var $dateok = $(".dateok");
        $dateok.each(function () {
            $(this).click(function () {
                var $year = $(this).parent().find(".year");
                var $month = $(this).parent().find(".month");
                var $txtdate = $(this).parent().prev().prev();
                $txtdate.val($month.val() + "/" + $year.val());
                $(this).parent().hide();
            });
        });

    }

    function BindImgDate() {

        var $imgdate = $(".selectDate img");
        $imgdate.each(function () {
            $(this).click(function () {
                var $datediv = $(this).next(".selectDateDiv");
                var $datetext = $(this).prev();
                var date = new Date();
                if ($datediv.is(":visible")) {
                    $datediv.hide();



                } else {
                    $datediv.show();
                }

                var $iframe = $(this).parent().parent().parent().find("iframe");
                $iframe.css("z-index", "10000000");
                

                var $year = $(this).next(".selectDateDiv").find(".year");
                var $month = $(this).next(".selectDateDiv").find(".month");
                $year.val(date.getFullYear());
                $month.val(date.getMonth());
                $year.change(function () {
                    $datetext.val($month.val() + "/" + $year.val());

                });
                $month.change(function () {
                    $datetext.val($month.val() + "/" + $year.val());

                });

            });
        });

        


    }

   function CalCashAdvance() {
       var $titlediv = $(".titlediv");
       var $carliinput = $(".cardiv ul li input");
       var $hidcashadvance = $('#<%= this.hidcashadvance.ClientID %>');
       var $hidTotalAmount = $('#<%=this.hidTotalAmount.ClientID %>');
       var $hidcashadvancewfid = $('#<%=this.hidcashadvancewfid.ClientID %>');
       var num = 0;
       var workflownumber = "";
       var workflownumberandamount = "";
       $carliinput.each(function () {
           if ($(this).attr("checked")) {
               num += parseFloat($(this).val());
               workflownumber += $(this).attr("title") + ";";
               workflownumberandamount += $(this).parent().text() + ";";
           }
       });
       if (num != 0) {
           $titlediv.html(num);
           $titlediv.css("textAlign", "center"); 
       } else {
           $titlediv.html("Select Cash Advance to be deducted");
           $titlediv.css("textAlign", "left"); 
       }
       $hidcashadvance.val(workflownumber);
       $hidTotalAmount.val(num);
       //alert($hidcashadvance.val());
       GetTotalAmount();
       $hidcashadvancewfid.val(workflownumberandamount);
   }


    function BindEvent() {
//        var $CostCenter = $(".CostCenter");
//        $CostCenter.parent().mousemove(function () {
//            // alert($CostCenter.parent().html());
//            $CostCenter.addClass("CostCenterTD");
//        });
//        $CostCenter.parent().mouseout(function () {
//            //alert($CostCenter.parent().html());
//            $CostCenter.removeClass("CostCenterTD");
//        });
//        $CostCenter.children().click(function () {
//            $CostCenter.addClass("CostCenterTD");
//            $CostCenter.parent().unbind("mouseout");
//        });
//        $CostCenter.children().blur(function () {
//            $CostCenter.removeClass("CostCenterTD");
//            $CostCenter.parent().mouseout(function () {
//                //alert($CostCenter.parent().html());
//                $CostCenter.removeClass("CostCenterTD");
//            });
//        });

//        var $ExpenseType = $(".ExpenseType");
//        $ExpenseType.parent().mousemove(function () {
//            // alert($CostCenter.parent().html());
//            $ExpenseType.addClass("ExpenseTypeTD");
//        });
//        $ExpenseType.parent().mouseout(function () {
//            //alert($CostCenter.parent().html());
//            $ExpenseType.removeClass("ExpenseTypeTD");
//        });
//        $ExpenseType.children().click(function () {
//            $ExpenseType.addClass("ExpenseTypeTD");
//            $ExpenseType.parent().unbind("mouseout");
//        });
//        $ExpenseType.children().blur(function () {
//            $ExpenseType.removeClass("ExpenseTypeTD");
//            $ExpenseType.parent().mouseout(function () {
//                //alert($CostCenter.parent().html());
//                $ExpenseType.removeClass("ExpenseTypeTD");
//            });
//        });
        var $ExpenseType = $(".ExpenseType");
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

//        var $CostCenter = $(".CostCenter");
//        $CostCenter.each(function () {
//            $(this).mousemove(function () {
//                $(this).addClass("CostCenterTD");
//            });
//            $(this).mouseout(function () {
//                $(this).removeClass("CostCenterTD");
//            });
//            var $children = $(this).children();
//            $children.click(function () {
//                $(this).addClass("CostCenterTD");
//                $(this).unbind("mouseout");
//            });
//            $children.click(function () {
//                $(this).removeClass("CostCenterTD");
//                $(this).mouseout(function () {
//                    $(this).removeClass("CostCenterTD");
//                });
//            });
//        });


    }


    function EndRequestHandler() {
        //alert("click add_endRequest");
        //CheckPermission();
        //BindClick();
       JSExpenseClaim.SetCompanyStd();
        CalcTotal();
        DisableEnterKey();
        BindEvent();
        BindImgDate();
        Selectdate();
        BlurTime();
        DrawSummaryExpenseTable();BindPeopleFind();
        //SetAutoWidthForDDL();
    }


    JSExpenseClaim.GetPreId = function (tmpId) {
        //ctl00_PlaceHolderMain_ListFormControl1_DataForm1_rptItem_ctl00_ddlExpenseType
        return tmpId.substring(0, tmpId.lastIndexOf('_') + 1);
    }

    function SelectExpenseType(obj) {
        JSExpenseClaim.objId = obj.id;
        JSExpenseClaim.preId = JSExpenseClaim.GetPreId(JSExpenseClaim.objId);
        var $DateTimeControl = $(obj).parent().parent().parent().parent().find("td .DateTimeControl");
        var $selectDate = $(obj).parent().parent().parent().parent().find("td .selectDate");

        var $hidremarkstatus1 = $('#' + JSExpenseClaim.preId + 'hidremarkstatus1');
        //alert("test:" + $(obj).val());
        if ($(obj).val() === 'Mobile – local call and others' || $(obj).val() === 'Store mgnt exp - mobile phone local calls and others') {
            JSExpenseClaim.mobileStd = $('#<%= this.hidMobileStd.ClientID %>').val();
            
            $('#' + JSExpenseClaim.preId + 'lbCompanyStandard').text(JSExpenseClaim.mobileStd == "" ? "0" : JSExpenseClaim.mobileStd);
            //$('#' + JSExpenseClaim.preId + 'hidCompanyStandard').val(isNaN(JSExpenseClaim.mobileStd) ? "0" : JSExpenseClaim.mobileStd);
            $('#' + JSExpenseClaim.preId + 'hidCompanyStandard').val(JSExpenseClaim.mobileStd == "" ? "0" : JSExpenseClaim.mobileStd);
            //alert($('#' + JSExpenseClaim.preId + 'hidCompanyStandard').val());
            $('#<%= this.hidremarkstatus.ClientID %>').val("2");
            $DateTimeControl.hide();
            $selectDate.show();
            $selectDate.find(".selectDateDiv").hide();
            $hidremarkstatus1.val("0");
            
        } else if ($(obj).val() === 'OT - meal allowance') {
            JSExpenseClaim.otMealStd = $('#<%= this.hidOTMealStd.ClientID %>').val();
            $('#' + JSExpenseClaim.preId + 'lbCompanyStandard').text(JSExpenseClaim.otMealStd);
            $('#' + JSExpenseClaim.preId + 'hidCompanyStandard').val(JSExpenseClaim.otMealStd);
            $('#<%= this.hidremarkstatus.ClientID %>').val("2");
            $DateTimeControl.show();
            $selectDate.hide();
            $hidremarkstatus1.val("0");
           
        } else {
            $('#' + JSExpenseClaim.preId + 'lbCompanyStandard').text('');
            $('#' + JSExpenseClaim.preId + 'hidCompanyStandard').val('-1');
            $hidremarkstatus1.val("2");
            //SetSelectExpenseTypeRemark(JSExpenseClaim.preId, $(obj).val());
            $DateTimeControl.show();
            $selectDate.hide();
            if ($(obj).val() === 'Mobile – long distance call' || $(obj).val() === 'Store mgnt exp - mobile phone long distance calls') {
                $DateTimeControl.hide();
                $selectDate.show();
                $selectDate.find(".selectDateDiv").hide();
            }
            if ($(obj).val() == "0") {
                //var $tr = $(obj).parent().parent().parent().prev().find("img"); 
                var $tr = $(obj).parent().parent().parent().next().find("img");
                $tr.each(function () {
                    $(this).click(function () {
                        if ($(obj).val() == "0") {
                            alert("Please Select Expense Type");
                            return false;
                        }
                    });
                });
            }

            var $tr1 = $(obj).parent().parent().parent().next().find("img");
            var $iframe1 = $tr1.parent().parent().parent().find("iframe");
            $iframe1.css("z-index", "10000000");

        }
        SetSelectExpenseTypeRemark(JSExpenseClaim.preId, $(obj).val());
        JSExpenseClaim.SetFontColor($('#' + JSExpenseClaim.preId + 'txtAmount').get(0));
        
    }

    function GetRemarkval(val) {
        var result =  false;
        if (val == "Internal function - list names of participants; External function - name of company") {
            result = true;
        }
        if (val == "Internal function - list names of recipient; External function - name of company") {
            result = true;
        }
//        if (val == "Purpose of Visa") {
//            result = true;
//        }
//        if (val == "Reason for penalty payment") {
//            result = true;
//        }
//        if (val == "Purpose of transportation") {
//            result = true;
//        }
//        if (val == "Purpose of meal") {
//            result = true;
//        }
        if (val == "") {
            result = true;
        }
        return result;
     }

    function SetSelectExpenseTypeRemark(preId, typeValue) {
        var $hidremarkstatus = $('#<%= this.hidremarkstatus.ClientID %>');
        var $hidremarkstatus1 = $('#' + preId + 'hidremarkstatus1');
        //alert($hidremarkstatus1.val());
        //$hidremarkstatus.val("0");
        //$('#' + preId + 'txtRemark').val("");
        JSExpenseClaim.remark = $('#' + preId + 'txtRemark');
        //alert("JSExpenseClaim.remark.val()" + JSExpenseClaim.remark.val())
        var result = GetRemarkval(JSExpenseClaim.remark.val());
        //alert(result);
        JSExpenseClaim.remark.unbind("focus");
        JSExpenseClaim.remark.unbind("blur");
        //Entertainment - food
        if (typeValue == "Entertainment - food" || typeValue == "Store exp - entertainment food") {
            JSExpenseClaim.remark = $('#' + preId + 'txtRemark');
            JSExpenseClaim.remark.css("borderColor", "#999999");
            JSExpenseClaim.remark.parent().parent().show();
//           
            if (result) {
                JSExpenseClaim.remark.val("Internal function - list names of participants; External function - name of company");
                JSExpenseClaim.remark.parent().parent().show();
            }
            JSExpenseClaim.remark.focus(function () {
                if ($(this).val() == "Internal function - list names of participants; External function - name of company") {
                    $(this).val("");
                }
            });
//            JSExpenseClaim.remark.blur(function () {
//                if ($(this).val() == "") {
//                    $(this).val("Internal function - list names of participants; External function - name of company");
//                }
//            });
            $hidremarkstatus.val("1");
            $hidremarkstatus1.val("1");
            return;
        }
        //Entertainment - gift
        if (typeValue == "Entertainment - gift" || typeValue == "Store exp - entertainment gift") {
            JSExpenseClaim.remark = $('#' + preId + 'txtRemark');
            JSExpenseClaim.remark.css("borderColor", "#999999");
            JSExpenseClaim.remark.parent().parent().show();
            if (result) {
                JSExpenseClaim.remark.val("Internal function - list names of recipient; External function - name of company");
            }
            JSExpenseClaim.remark.focus(function () {
                if ($(this).val() == "Internal function - list names of recipient; External function - name of company") {
                    $(this).val("");
                }
            });
//            JSExpenseClaim.remark.blur(function () {
//                if ($(this).val() == "") {
//                    $(this).val("Internal function - list names of recipient; External function - name of company");
//                }
//            });
            $hidremarkstatus.val("1");
            $hidremarkstatus1.val("1");
            return;
        }
        if (typeValue.indexOf("Mobile") != -1) {
            JSExpenseClaim.remark.focus(function () {
                if ($(this).val() == "" || $(this).val() == "Why exceeds Company Standard?") {
                    $(this).val("");
                }
            });
            return;
        }
        if (typeValue.indexOf("meal") != -1) {
            JSExpenseClaim.remark.focus(function () {
                if ($(this).val() == "" || $(this).val() == "Why exceeds Company Standard?") {
                    $(this).val("");
                }
            });
            return;
        }


        //Government charges - visa application
//        if (typeValue == "Government charges - visa application") {
//            JSExpenseClaim.remark = $('#' + preId + 'txtRemark');
//            JSExpenseClaim.remark.css("borderColor", "#999999");
//            JSExpenseClaim.remark.parent().parent().show();
//            if (result) {
//                JSExpenseClaim.remark.val("Purpose of Visa");
//            }
//            JSExpenseClaim.remark.focus(function () {
//                if ($(this).val() == "Purpose of Visa") {
//                    $(this).val("");
//                }
//            });
////            JSExpenseClaim.remark.blur(function () {
////                if ($(this).val() == "") {
////                    $(this).val("Purpose of Visa");
////                }
////            });
//            $hidremarkstatus.val("1");
//            $hidremarkstatus1.val("1");
//            return;
//        }
        //Government charges - penalty charges
//        if (typeValue == "Government charges - penalty charges") {
//            JSExpenseClaim.remark = $('#' + preId + 'txtRemark');
//            JSExpenseClaim.remark.css("borderColor", "#999999");
//            JSExpenseClaim.remark.parent().parent().show();
//            if (result) {
//                JSExpenseClaim.remark.val("Reason for penalty payment");
//            }
//            JSExpenseClaim.remark.focus(function () {
//                if ($(this).val() == "Reason for penalty payment") {
//                    $(this).val("");
//                }
//            });
////            JSExpenseClaim.remark.blur(function () {
////                if ($(this).val() == "") {
////                    $(this).val("Reason for penalty payment");
////                }
////            });
//            $hidremarkstatus.val("1");
//            $hidremarkstatus1.val("1");
//            return;
//        }
        //Travel - transportation
//        if (typeValue == "Travel - transportation") {
//            JSExpenseClaim.remark = $('#' + preId + 'txtRemark');
//            JSExpenseClaim.remark.css("borderColor", "#999999");
//            JSExpenseClaim.remark.parent().parent().show();
//            if (result) {
//                JSExpenseClaim.remark.val("Purpose of transportation");
//            }
//            JSExpenseClaim.remark.focus(function () {
//                if ($(this).val() == "Purpose of transportation") {
//                    $(this).val("");
//                }
//            });
////            JSExpenseClaim.remark.blur(function () {
////                if ($(this).val() == "") {
////                    $(this).val("Purpose of transportation");
////                }
////            });
//            $hidremarkstatus.val("1");
//            $hidremarkstatus1.val("1");
//            return;
//        }
        //Travel - meal
//        if (typeValue == "Travel - meal") {
//            JSExpenseClaim.remark = $('#' + preId + 'txtRemark');
//            JSExpenseClaim.remark.css("borderColor", "#999999");
//            JSExpenseClaim.remark.parent().parent().show();
//            if (result) {
//                JSExpenseClaim.remark.val("Purpose of meal");
//            }
//            JSExpenseClaim.remark.focus(function () {
//                if ($(this).val() == "Purpose of meal") {
//                    $(this).val("");
//                }
//            });
////            JSExpenseClaim.remark.blur(function () {
////                if ($(this).val() == "") {
////                    $(this).val("Purpose of meal");
////                }
////            });
//            $hidremarkstatus.val("1");
//            $hidremarkstatus1.val("1");
//            return;
//        }
        
    }


    JSExpenseClaim.SetCompanyStd = function () {
        JSExpenseClaim.expensetypes = $('#<%= this.UpdatePanel1.ClientID %> select.ExpenseType').toArray();
        for (JSExpenseClaim.i = 0; JSExpenseClaim.i < JSExpenseClaim.expensetypes.length; JSExpenseClaim.i++) {
            SelectExpenseType(JSExpenseClaim.expensetypes[JSExpenseClaim.i]);
        }
    }

    function SetBorderWarn($obj) {
        $obj.css('border', '1px solid red');
    }

    function ClearBorderWarn($obj) {
        $obj.css('border', '');
        $obj.css('border-bottom', '#999 1px solid');
    }

    function CalcTotal() {
        //alert("CalcTotal Start");
        JSExpenseClaim.total = 0;
        JSExpenseClaim.amounts = $('#<%= this.UpdatePanel1.ClientID %> input[class=\'Amount\']').toArray();

        for (JSExpenseClaim.i = 0; JSExpenseClaim.i < JSExpenseClaim.amounts.length; JSExpenseClaim.i++) {
            if (isNaN(JSExpenseClaim.amounts[JSExpenseClaim.i].value)) {
                //alert('Please fill the valid number.');
                JSExpenseClaim.amounts[JSExpenseClaim.i].value = "";
                JSExpenseClaim.SetFontColor(JSExpenseClaim.amounts[JSExpenseClaim.i]);
                //                JSExpenseClaim.total = 0;
                //                GetTotalAmount();
                continue;
            }
            if (JSExpenseClaim.amounts[JSExpenseClaim.i].value != "" && JSExpenseClaim.amounts[JSExpenseClaim.i].value <= 0) {
                //alert('Please fill the valid number.');
                JSExpenseClaim.amounts[JSExpenseClaim.i].value = "";
                JSExpenseClaim.SetFontColor(JSExpenseClaim.amounts[JSExpenseClaim.i]);
                //                JSExpenseClaim.total = 0;
                //                GetTotalAmount();
                continue;
            }
            if (JSExpenseClaim.amounts[JSExpenseClaim.i].value > 100000000) {
                alert('Please fill the number of less than 100000000.');
                JSExpenseClaim.amounts[JSExpenseClaim.i].value = "";
                JSExpenseClaim.SetFontColor(JSExpenseClaim.amounts[JSExpenseClaim.i]);
                continue;
            }
            if (jQuery.trim(JSExpenseClaim.amounts[JSExpenseClaim.i].value) === '') {
                JSExpenseClaim.SetFontColor(JSExpenseClaim.amounts[JSExpenseClaim.i]);
                continue;
            }
            JSExpenseClaim.total += parseFloat(JSExpenseClaim.amounts[JSExpenseClaim.i].value);

            JSExpenseClaim.SetFontColor(JSExpenseClaim.amounts[JSExpenseClaim.i]);
        }
        $('#<%= this.lbTotalAmount.ClientID %>').text(commafy(JSExpenseClaim.total));
        GetTotalAmount();
        JSExpenseClaim.amounts = null;
    }

    JSExpenseClaim.SetFontColor = function (obj) {
        JSExpenseClaim.preId = JSExpenseClaim.GetPreId(obj.id);
        JSExpenseClaim.companyStd = $('#' + JSExpenseClaim.preId + 'hidCompanyStandard').val();
        JSExpenseClaim.remark = $('#' + JSExpenseClaim.preId + 'txtRemark');
        JSExpenseClaim.lbCompanyStandard = $('#' + JSExpenseClaim.preId + 'lbCompanyStandard').val();
        var $hidremarkstatus = $('#<%= this.hidremarkstatus.ClientID %>');
        var $hidremarkstatus1 = $('#' + JSExpenseClaim.preId + 'hidremarkstatus1');
        JSExpenseClaim.ishighlight = true;
        if (JSExpenseClaim.companyStd == '0') {
            JSExpenseClaim.ishighlight = true;
        }
        if (parseFloat(JSExpenseClaim.companyStd) < 0) {
            if (JSExpenseClaim.remark.val() == "") {
                JSExpenseClaim.ishighlight = false;
            }
        }
        if (parseFloat(obj.value) > parseFloat(JSExpenseClaim.companyStd)) {
            if (parseFloat(JSExpenseClaim.companyStd) > 0) {
                JSExpenseClaim.ishighlight = true;
            }
        }
        if (parseFloat(obj.value) <= parseFloat(JSExpenseClaim.companyStd)) {
            JSExpenseClaim.ishighlight = false;
        }
        if ($.trim(obj.value) == "") {
            JSExpenseClaim.ishighlight = false;
            if ($hidremarkstatus1.val() != "0") {
                JSExpenseClaim.ishighlight = true;
            }
        }

        if ($hidremarkstatus1.val() == "1") {
            obj.style.color = '';
            //JSExpenseClaim.remark.css("borderColor", "#999999");
            JSExpenseClaim.remark.parent().parent().show();
            return;
        }
        if ($hidremarkstatus1.val() == "2") {
            obj.style.color = '';
            //JSExpenseClaim.remark.css("borderColor", "#999999");
            JSExpenseClaim.remark.val("");
            JSExpenseClaim.remark.parent().parent().hide();
            return;
        }

        if (JSExpenseClaim.ishighlight) {
            obj.style.color = 'red';
            //JSExpenseClaim.remark.css("borderColor", "red");
            JSExpenseClaim.remark.parent().parent().show();
            if (JSExpenseClaim.remark.val() == "" || JSExpenseClaim.remark.val() == "Why exceeds Company Standard?") {
                JSExpenseClaim.remark.val("Why exceeds Company Standard?");
            }
            JSExpenseClaim.remark.focus(function () {
                if (JSExpenseClaim.remark.val() == "" || JSExpenseClaim.remark.val() == "Why exceeds Company Standard?") {
                    JSExpenseClaim.remark.val("");
                }
            });
        } else {
            obj.style.color = '';
            JSExpenseClaim.remark.parent().parent().hide();
            JSExpenseClaim.remark.val("");

        }


        if ($('#' + JSExpenseClaim.preId + 'lbCompanyStandard').text() == "no limit") {
            obj.style.color = '';
            JSExpenseClaim.remark.parent().parent().hide();
            JSExpenseClaim.remark.val("");
        }


        //        if (JSExpenseClaim.remark.val() != "") {
        //            JSExpenseClaim.remark.parent().parent().show();
        //        }
    }

    function CheckRemarkInfo() {
        CreateForbidDIV();
        $(".wrapdiv").removeClass("wrapdiv");
        var result = true;
        var msg = "";
        var $errmsg = $("td.pftd span:contains('No exact match was found.')");
        if ($errmsg.length > 0) {
            msg += "Please choose employee. \n";
            if (!$("td.pftd").find("span.ca-people-finder").parent().hasClass("wrapdiv")) {
                $("td.pftd").find("span.ca-people-finder").wrap("<span class=\"wrapdiv\"></span>");
            }
            result = false;
        }
        var $remark = $(".remark");
        $remark.each(function () {
            if ($(this).parent().parent().is(":visible")) {
                if ($.trim($(this).val()) == "") {
                    //alert('Please fill the Info of Remark.');
                    msg += "Please fill the Info of Remark. \n";
                    if (!$(this).parent().hasClass("wrapdiv")) {
                        $(this).wrap("<span class=\"wrapdiv\"></span>");
                    }
                    result = false;
                }
            }
        });
        var $ExpensePurpose = $(".ExpensePurpose");
        $ExpensePurpose.each(function () {
            if ($.trim($(this).val()) == "") {
                //alert('Please fill the Expense Purpose.');
                msg += "Please fill the Expense Purpose. \n";
                if (!$(this).parent().hasClass("wrapdiv")) {
                    $(this).wrap("<span class=\"wrapdiv\"></span>");
                }
                result = false;
            }
        });
        var $cc = $(".cc");
        $cc.each(function () {
            if ($.trim($(this).val()) == "0") {
                //alert('Please Select CostCenter.');
                msg += "Please Select CostCenter.\n";
                if (!$(this).parent().parent().hasClass("wrapdiv")) {
                    $(this).parent().addClass("wrapdiv");
                    // $(this).wrap("<span class=\"wrapdiv\"></span>");
                }
                result = false;
            }
        });
        var $et = $("select.ExpenseType");
        $et.each(function () {
            if ($.trim($(this).val()) == "0") {
                //alert('Please Select Expense Type.');
                msg += "Please Select Expense Type.\n";
                if (!$(this).parent().parent().hasClass("wrapdiv")) {
                    // $(this).wrap("<span class=\"wrapdiv\"></span>");
                    $(this).parent().addClass("wrapdiv");
                }
                result = false;
            }
            var expense = $(this).val();
            if (expense.indexOf("Store") != -1) {
                var $costc = $(this).parent().parent().parent().parent().find("select.cc");
                if ($costc.val().indexOf("S") != 0) {
                    msg += "Please Select Store CostCenter.\n";
                    if (!$costc.parent().parent().hasClass("wrapdiv")) {
                        $costc.parent().addClass("wrapdiv");
                    }
                    result = false;
                }
            }
        });
        var $txtAmount = $("input.Amount");
        $txtAmount.each(function () {
            if ($.trim($(this).val()) == "") {
                //alert('Please Select Expense Type.');
                msg += "Please fill the Amount.\n";
                if (!$(this).parent().hasClass("wrapdiv")) {
                    // $(this).wrap("<span class=\"wrapdiv\"></span>");
                    $(this).wrap("<div class=\"wrapdiv\"></div>");
                }
                result = false;
            }
        });

        var $ExpenseDescription = $("td.ExpenseDescription input");
        if ($ExpenseDescription.val() == "") {
            msg += "Please fill the ExpenseDescription.\n";
            if (!$ExpenseDescription.parent().hasClass("wrapdiv")) {
                // $(this).wrap("<span class=\"wrapdiv\"></span>");
                $ExpenseDescription.wrap("<div class=\"wrapdiv\"></div>");
            }
            result = false;
        }

//        var r = CheckUsers();
//        if (!r) {
//            msg += "\nPlease tick √ to choose employee.\n";
//            if (!$("td.pftd").find("span.ca-people-finder").parent().hasClass("wrapdiv")) {
//                $("td.pftd").find("span.ca-people-finder").wrap("<span class=\"wrapdiv\"></span>");
//            }
//            result = false;
//        }

        if (msg != "") {
            alert(msg);
        }
        if (!result) {
            ClearForbidDIV();
        }
        return result;
    }

    function DisableEnterKey() {
        $('#<%=this.UpdatePanel1.ClientID %>' + ' input').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('#<%=this.UpdatePanel1.ClientID %>').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
        $('div.main-body').keypress(function (event) {
            return ca.util.disableEnterKey(event);
        });
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

    function GetTotalAmount() {
        var $lbTotalAmount = $('#<%=this.lbTotalAmount.ClientID %>');
        var $hidTotalAmount = $('#<%=this.hidTotalAmount.ClientID %>');
        var $lbAmountDue = $('#<%=this.lbAmountDue.ClientID %>');
        if ($lbTotalAmount.text() == "0") {
            $lbAmountDue.text("");
        }
        if ($hidTotalAmount.val() != "" && $lbTotalAmount.text() != "0") {
            var amount = parseFloat($lbTotalAmount.text().replace(",", "").replace(",", "").replace(",", "")) - parseFloat($hidTotalAmount.val())

            $lbAmountDue.text(commafy(amount));

//            if (amount > 0) {
//                $lbAmountDue.text(commafy(amount));
//            } else {
//                $lbAmountDue.text(commafy($lbTotalAmount.text().replace(",", "").replace(",", "").replace(",", "")));
//            }
        }
        if ($hidTotalAmount.val() == "" && $lbTotalAmount.text() != "0") {
            $lbAmountDue.text($lbTotalAmount.text());
        }
    }

   

</script>
<script type="text/javascript">
//    function SummaryExpenseType(obj) {
//        var $summarytype = $(".summarytype");
//        var $obj = $(obj);
//        var expenseType = $obj.val();
//        var $amount = $obj.parent().parent().parent().parent().find("input.Amount");
//        if (expenseType == "Mobile – local call and others" || expenseType == "Mobile – long distance call") {
//            expenseType="Mobile";
//        }
//        var result = CheckSummaryTypeHtml(expenseType);
//        if (!result) {
//            var $html = $(AppendHtml(expenseType, $amount.val() == "" ? "0" : $amount.val()));
//            $summarytype.before($html);
//        } else {
//            UpdateSummaryExpenseTypeAmount(expenseType,$amount.val());
//        }
//    }
//    function SummaryAmount(obj) {
//        var $summarytype = $(".summarytype");
//        var $obj = $(obj);
//        var $expenseType = $obj.parent().parent().find("select.ExpenseType");
//        if ($expenseType.val() == "0") {
//            alert("Please Select Expense Type");
//            $obj.val("");
//            return false;
//        }
//        
//        if (isNaN(obj.value)) {
//            alert('Please fill the valid number.');
//            $obj.val("0");
//        }
//        if (obj.value < 0) {
//            alert('Please fill the valid number.');
//            $obj.val("0");
//        }
//        if (obj.value > 100000000) {
//            alert('Please fill the number of less than 100000000.');
//            $obj.val("0");
//        }
//        var expenseType = $expenseType.val();
//        if (expenseType == "Mobile – local call and others" || expenseType == "Mobile – long distance call") {
//            expenseType = "Mobile";
//        }
//        var result = CheckSummaryTypeHtml(expenseType);
//        if (!result) {
//            var $html = $(AppendHtml($expenseType.val(), $obj.val()));
//            $summarytype.before($html);
//        } else {
//            UpdateSummaryExpenseTypeAmount(expenseType, $obj.val());
//        }
//    }
    function AppendHtml(expenseType, amount, costcenter) {
        var appendHtml = "";
        var costcentertd = costcenter;
        if (costcentertd == "0") {
            costcentertd = "";
         }
        appendHtml = "<tr class=\"item\" ><td >" + expenseType + "</td><td >" + costcentertd + "</td><td >" + amount + "</td></tr>";
        return appendHtml;
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
    function UpdateSummaryExpenseTypeAmount(expenseType, amount, costcenter) {
        var exp = ".summarytypetable tr td:contains('" + expenseType + "')";
        var $summarytypehtml = $(exp);
        var $expcostcenter = $summarytypehtml.parent().find("td:contains('" + costcenter + "')");
        if ($expcostcenter.length > 0) {
            var $amount = $expcostcenter.next();
            var totalamount = CalExpenseTypeAmount(expenseType, costcenter); 
            $amount.text(totalamount + "");
        }
    }
    function CalExpenseTypeAmount(expenseType, costcenter) {
        var $ExpenseType = $("select.ExpenseType");
        var amount = 0;
        var et = expenseType;
        if (et == "Wireless/Mobile") {
            et = "Mobile";
        }
        if (et == "Store mgnt exp - Wireless/Mobile") {
            et = "Store mgnt exp - mobile";
        }
        $ExpenseType.each(function () {

            var costcenterval = $(this).parent().parent().parent().parent().find("td select.cc").val();
            if ($(this).val().indexOf(et) != -1 && costcenterval.indexOf(costcenter) != -1) {
                var $amount = $(this).parent().parent().parent().parent().find("input.Amount");
                if ($amount.val() != "") {
                    amount += parseFloat($amount.val());
                }
            }
        });
        amount = Math.round(amount * Math.pow(10, 2)) / Math.pow(10, 2);
        return amount;
    }

   
    function DrawSummaryExpenseTable() {
        $(".summarytypetable tr").remove(".item");
        var $summarytype = $(".summarytype");
        var $expenseType = $("#expenseTypetable select.ExpenseType");
        $expenseType.each(function () {
            if ($(this).val() != "0") {
                var $amount = $(this).parent().parent().parent().parent().find("input.Amount");
                var expenseType = $(this).val();

                var costcenter = $(this).parent().parent().parent().parent().find("td select.cc").val();


                if (expenseType.indexOf("Mobile") != -1) {
                    expenseType = "Wireless/Mobile";
                }
                if (expenseType.indexOf("mobile") != -1) {
                    expenseType = "Store mgnt exp - Wireless/Mobile";
                }
                var result = CheckSummaryTypeHtml(expenseType, costcenter);
                if (isNaN($amount.val())) {
                    alert('Please fill the valid number.');
                    $amount.val("");
                }
                if ($amount.val() < 0) {
                    alert('Please fill the valid number.');
                    $amount.val("");
                }
                if ($amount.val() > 100000000) {
                    alert('Please fill the number of less than 100000000.');
                    $amount.val("");
                }

                var txtAmount = $amount.val() == "" ? "0" : $amount.val();
                txtAmount = Math.round(txtAmount * Math.pow(10, 2)) / Math.pow(10, 2);
                if (!result) {
                    var $html = $(AppendHtml(expenseType, txtAmount, costcenter));
                    $summarytype.before($html);
                } else {
                    UpdateSummaryExpenseTypeAmount(expenseType, txtAmount, costcenter);
                }
                //                if (!result) {
                //                    var $html = $(AppendHtml(expenseType, $amount.val() == "" ? "0" : Math.round(parseFloat($amount.val()) * 100) / 100, costcenter));
                //                    $summarytype.before($html);
                //                } else {
                //                    UpdateSummaryExpenseTypeAmount(expenseType, Math.round(parseFloat($amount.val()) * 100) / 100, costcenter);
                //                }
            }
        });
        UpdateSummaryExpenseType();
    }

    function UpdateSummaryExpenseType() {
        var $hidSummaryExpenseType = $('#<%= this.hidSummaryExpenseType.ClientID %>');
        var summaryExpenseType = "[";
        var $summarytypetable = $(".summarytypetable tr.item");
        $summarytypetable.each(function () {
            //summaryExpenseType += "{name:'" + $(this).find("td").eq(0).text() + "',val:'" + $(this).find("td").eq(1).text() + "'},";
            summaryExpenseType += "{name:'" + $(this).find("td").eq(0).text() + "',val:'" + $(this).find("td").eq(2).text() + "',costcenter:'" + $(this).find("td").eq(1).text() + "'},";
        });
        summaryExpenseType += "]";
        $hidSummaryExpenseType.val(summaryExpenseType);
    }

     
</script>
<script type="text/javascript">
    function BindPeopleFind() {
        $('#<%=this.cpfUser.ClientID %>' + '_checkNames').click(function () {
            $("#<%=this.btnPeopleInfo.ClientID %>").click();
        });
    }
</script>
<script type="text/javascript">
    function ChangeLocation(obj) {
        var $obj = $(obj);
        var $hidLocationType = $("input[id='hidLocationType']");
        if ($hidLocationType.val() == "" && $obj.val().toLowerCase().indexOf("travel") != -1) {
            var alertMsg = "";
            AlertSelect("Is this travel expense related to business trip applied through Travel Request EWF ? ");
        }
    }

    function AlertSelect(msg) {
        $("#bgDiv").css({
            height: function () {
                return $(".ContentDiv").height();
            },
            width: "100%"

        });
        $("#bgDiv").show();
        $("#msg").append(msg);
        $("#ErrorDiv").fadeIn(1000);
    }

    //    function BindClick() {
    //        $("#right").click(function () {
    //            $("#ErrorDiv").hide();
    //            $("#bgDiv").hide();
    //        }).mousemove(function () {
    //            $("#right").css("color", "red");
    //        }).mouseout(function () {
    //            $("#right").css("color", "#3d3d3d");
    //        });
    //    }

    function CloseAlert() {
        var $hidLocationType = $("input[id='hidLocationType']");
        $hidLocationType.val("Travel");
        $("#ErrorDiv").hide();
        $("#bgDiv").hide();
        return false;
    }
    function SetAlert() {
        var text = $("#msg").html();
        if (text.indexOf("otherwise") == -1) {
            $("a.okbtn").html("OK");
            $("a.nobtn").html("Back");
            $("a.nobtn").hide();
            $("#alertleft").css("width", "88%");
            $("#alertleft").css("textAlign", "right");

            $("#alertright").css("width", "0");
            $("#msg").html("");
            $("#msg").append("Please file the expense using Travel Expense EWF in Task History – Travel Request & Claim, otherwise, Finance will reject your claim.");
        } else {
            CloseAlert();
        }
    }
</script>
<script type="text/javascript">
    function SetRemarkInfo(obj) {
        JSExpenseClaim.preId = JSExpenseClaim.GetPreId(obj.id);
        JSExpenseClaim.remark = $('#' + JSExpenseClaim.preId + 'txtRemark');
        JSExpenseClaim.remark.val("");
    }

</script>
