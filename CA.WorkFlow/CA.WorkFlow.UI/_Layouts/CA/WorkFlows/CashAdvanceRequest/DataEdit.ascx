<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.CashAdvanceRequest.DataEdit"   %>
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
    #tb_travelexpense td.td_expensetype
    {
        width: 15%;
    }
    #tb_travelexpense td.td_date
    {
        width: 16%;
    }
    #tb_travelexpense td.td_costcenter
    {
        width: 10%;
    }
    #tb_travelexpense td.td_originalamt
    {
        width: 8%;
    }
    #tb_travelexpense td.td_currency
    {
        width: 8%;
    }
    #tb_travelexpense td.td_exchrate
    {
        width: 8%;
    }
    #tb_travelexpense td.td_rmbamt
    {
        width: 8%;
    }
    #tb_travelexpense td.td_paidbycredit
    {
        width: 10%;
    }
    #tb_travelexpense td.td_companystd
    {
        width: 8%;
    }
    #tb_travelexpense td.td_specialapproval
    {
        width: 9%;
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
        width: 72px;
    }
    
    #tb_travelexpense tr.tr_remark td.td_remark input
    {
        width: 500px;
    }
    #tb_travelexpense td.td_remark
    {
        display: none;
    }
    #tb_travelexpense td.td_paidbycredit input
    {
        width: 15px;
    }
    #tb_travelexpense td.td_specialapproval input
    {
        width: 15px;
    }
    #tb_travelexpense td.cc
    {
        padding-top: 6px;
        vertical-align: top;
    }
    
    select.width-fix
    {
        width: 60px;
        z-index: 1000;
    }
    select.expand
    {
        position: absolute;
        width: auto; /* Let the browser handle it. */
    }
    
    #boxes #dialog
    {
        background: url(../images/notice.png) no-repeat 0 0 transparent;
        width: 326px;
        height: 229px;
        padding: 50px 0 20px 25px;
    }
    #dialog table
    {
        margin-top: 70px;
    }
    #dialog select
    {
        margin-top: 20px;
        margin-left: 50px;
    }
    .form-table
    {
        border: none;
        margin: 10px 0px 10px 0px;
    }
    .tdlabel
    {
        border-bottom: none;
        border-right: none;
        padding: 5px;
    }
    .ca-workflow-form-table h3
    {
        padding: 5px;
    }
    .w60
    {
        width: 60%;
    }
    .form-table1
    {
        border-top: none;
    }
    .form-table2
    {
        margin-bottom: 0px;
        border-bottom: #ccc 1px solid;
        border-top: none;
    }
    .form-table3
    {
        margin-top: 15px;
        margin-bottom: 0px;
        border-bottom: none;
    }
    .tdpadding
    {
        height: 40px;
    }
    .lable1
    {
        border-left: none;
        border-bottom: #ccc 1px solid;
        border-top: #ccc 1px solid;
        text-align: center;
    }
    .lable2
    {
        border-left: none;
        border-bottom: #ccc 1px solid;
        border-top: #ccc 1px solid;
        padding: 7px;
    }
    .ca-workflow-form-table input
    {
        border-bottom: #ccc 1px solid;
    }
    .lable2 input
    {
        border: none;
        padding: 0px;
        margin: 0px;
        width: 20%;
        background-color: white;
    }
    .w80
    {
        width: 80%;
    }
    .w80 input
    {
        width: 45%;
    }
    .ContentDiv
    {
        padding: 0px;
        margin: 0px;
        position: relative;
    }
    #ErrorDiv
    {
        margin: 0 auto;
        width: 200px;
        position: absolute;
        left: 50%;
        top: 20%;
        display: none;
        border: 1px solid gray;
        background-color: Gray;
        padding: 5px;
        font-size: 14px;
        z-index: 999;
    }
    #ErrorMsg
    {
        background-color: White;
        color: Red;
        width: 200px;
        float: left;
        line-height: 22px;
    }
    #title
    {
        float: left;
        width: 180px;
        border-bottom: 1px solid Gray;
        padding: 10px;
        font-weight: bold;
        cursor: pointer;
    }
    #left
    {
        float: left;
        color: #3d3d3d;
    }
    #right
    {
        float: right;
    }
    #msg
    {
        float: left;
        width: 180px;
        padding: 10px;
        font-weight: lighter;
    }
    #bgDiv
    {
        position: absolute;
        top: 0px;
        left: 0px;
        right: 0px;
        bottom: 0px;
        background-color: White;
        display: none;
        filter: Alpha(opacity=10);
        z-index: 1;
    }
    .close
    {
        color: Red;
    }
    .ca-workflow-form-table td
    {
        line-height: 15px;
    }
    .txtAmount
    {
        border: 1px solid red;
    }
    .wrapdiv
    {
        padding: 2px;
        border: 1px solid red;
    }
    .label1
    {
        border-right:none;
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
        margin-top:3px;
    }
</style>
<div class="ContentDiv">
    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="ca-workflow-form-table" style=" margin-top:20px">
                <tr>
                    <td colspan="4" class="value align-center">
                        <h3>
                            Cash Advance Request Form<br />
                            借款申请表</h3>
                    </td>
                </tr>
                <tr class="ChooseEmployee">
                    <td class="label align-center">
                        Choose Employee<br />
                        选择员工
                    </td>
                    <td class="pftd value" colspan="3" align="center">
                        <cc1:CAPeopleFinder ID="cpfUser" runat="server" AllowTypeIn="true" MultiSelect="false"
                            CssClass="ca-people-finder" Width="200" />
                    </td>
                </tr>
                <tr class="SAPNo">
                    <td class="label align-center w20">
                        Doc.No<br />
                        文档编号
                    </td>
                    <td class="label w35 docno">
                        <QFL:FormField ID="ffSAPNo" runat="server" FieldName="SAPNo" ControlMode="Display">
                        </QFL:FormField>
                        <asp:TextBox ID="txtCompany" runat="server" Visible="false" Text="CA"></asp:TextBox>
                    </td>
                    <td class="label align-center w15 saptitle">
                        SAP.No<br />
                        SAP编号
                    </td>
                    <td class="label align-center w40 sapnumber">
                        <asp:Label ID="lblSapNumber" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="label align-center w20">
                        Dept<br />
                        部门
                    </td>
                    <td class="label align-center w35 department">
                       <asp:TextBox ID="txtDept" runat="server"></asp:TextBox>
                       <%-- <QFL:FormField ID="ffDepartment" runat="server" FieldName="Department" >
                        </QFL:FormField>--%>
                    </td>
                    <td class="label align-center w15">
                        Requested By<br />
                        申请人
                    </td>
                    <td class="label align-center w40">
                        <asp:Label ID="txtRequestedBy" runat="server" Text="" CssClass="lbRequestedBy"></asp:Label>
                    </td>
                </tr>
            </table>
            <table class="ca-workflow-form-table" style=" margin-top:20px" >
                <tr>
                    <td colspan="3" class="value align-center">
                        <h3>
                            Description</h3>
                    </td>
                </tr>
                <tr>
                    <td class="label w50">
                        Purpose<br />
                        借款详细用途
                    </td>
                    <td class="label align-center w15">
                        Amount<br />
                        金额(RMB)
                    </td>
                    <td class="value align-center w20">
                        Payment Method<br />
                        付款方式
                    </td>
                </tr>
                <tr>
                    <td class="label w50 tdpadding" style=" border-bottom:none">
                        <asp:TextBox ID="txtPurpose" runat="server" CssClass="Purpose"></asp:TextBox>
                    </td>
                    <td class="label align-center w15">
                        <div id="amount">
                            <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox></div>
                    </td>
                    <td class="value align-center w20" id="tdTerm">
                        <asp:DropDownList ID="dplTerm" runat="server" Width="80px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lable2 rbtype">
                        <asp:RadioButtonList ID="rblLevel" runat="server" RepeatDirection="Horizontal" Width="200px">
                            <asp:ListItem Text="Normal" Value="Normal" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Urgent" Value="Urgent"></asp:ListItem>
                        </asp:RadioButtonList>
                        <div style="position: relative">
                            <div style="position: absolute; top: -30px; left: 0px; width: 270px; height: 100px;
                                z-index: 10000; background-color: White; filter: Alpha(opacity=0.5); display: none"
                                id="displaydiv">
                            </div>
                        </div>
                    </td>
                    <td class="lable1" colspan="2">
                    </td>
                </tr>
                <tr class="urgentRemark">
                    <td class="lable" colspan="3" 
                        style="padding: 6px;  ">
                        <div style="float: left">
                            Remark<br />
                            备注</div>
                        <div style="float: left; margin-left:20px; padding-top:8px">
                            <asp:TextBox ID="txtUrgentRemark" runat="server" CssClass="txtUrgentRemark"></asp:TextBox>
                        </div>
                    </td>
                </tr>
            </table>
            <table class="ca-workflow-form-table" style="margin-top: 20px">
                <tr>
                    <td class="lable" colspan="3" style="padding: 6px;border-bottom:#ccc 1px solid;">
                        <div style="float: left">
                            Remark<br />
                            备注</div>
                        <div style="float: left; margin-left: 20px;">
                            <asp:TextBox ID="txtRemark" runat="server"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="lable" colspan="3" style="padding: 6px">
                        <div style="float: left">
                            Attachment<br />
                            附件</div>
                        <div style="float: left; margin-left: 20px;">
                            <QFL:FormAttachments runat="server" ID="attacthment">
                            </QFL:FormAttachments>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="ErrorDiv">
                <div id="ErrorMsg">
                    <div id="title">
                        <div id="left">
                            Error</div>
                        <div id="right">
                            X</div>
                    </div>
                    <br />
                </div>
            </div>
            <div id="bgDiv">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:Button ID="btnPeopleInfo" runat="server" OnClick="btnPeopleInfo_Click" CausesValidation="False"
    CssClass="hidden" />
<input type="hidden" id="termVal" runat="server" />
<script type="text/javascript" src="../../../CAResources/themeCA/js/jquery-1.4.1-vsdoc.js"></script>
<script type="text/javascript">
    var $txtAmount = $('#<%=this.txtAmount.ClientID %>');
    var $dplTerm = $('#<%=this.dplTerm.ClientID %>');
    function ShowError(msg) {
        $("#bgDiv").css({
            height: function () {
                return $(".ContentDiv").height();
            },
            width: "100%"
        });
        $("#bgDiv").show();
        $("#ErrorMsg").append(msg);
        $("#ErrorDiv").fadeIn(1000);
    }
    $(function () {
        var $ffSAPNo = $('tr.SAPNo input.ms-long');
        if ($ffSAPNo.text() == "") {
            $ffSAPNo.parent().parent().parent().hide();
        }

        var mode = '<%=this.mode %>';
        if (mode != "new") {
            $("tr.ChooseEmployee").hide();
        }

        $('#<%=this.cpfUser.ClientID %>' + '_checkNames').click(function () {
            $("#<%=this.btnPeopleInfo.ClientID %>").click();
        });

        var $sapnumber = $("tr.SAPNo td.sapnumber");
        var $saptitle = $("tr.SAPNo td.saptitle");
        var $docno = $("tr.SAPNo td.docno");
        if ($sapnumber.text() == "") {
            $docno.css("borderRight", "none");
            $saptitle.css("borderRight", "none");
            $saptitle.text("");
        }



        var $rblLevel = $('#<%=this.rblLevel.ClientID %>');
        var $rblLevelinput = $rblLevel.find("input:radio");
        $rblLevelinput.each(function () {
            $(this).click(function () {
                if ($(this).val() == "Urgent") {
                    $(".urgentRemark").show();
                    if ($(".urgentRemark input").val() == "") {
                        $(".urgentRemark input").val("Please state reason for urgent request.");
                    }
                    $(".urgentRemark input").focus(function () {
                        if ($(this).val() == "Please state reason for urgent request.") {
                            $(this).val("");
                        }
                    });
                } else {
                    $(".urgentRemark").hide();
                    $(".urgentRemark input").val("");
                }
            });
        });
        if ($rblLevel.find("input:radio:checked").val() == "Urgent") {
            $(".urgentRemark").show();
        } else {
            $(".urgentRemark").hide();
        }




        $txtAmount.blur(function () {
            if (!isNaN($txtAmount.val())) {
                if ($txtAmount.val() > 100000000 || $txtAmount.val() <= 0) {
                    alert("Please fill the valid number.");
                    $txtAmount.val("");
                    return;
                 }

                var $termVal = $('#<%=this.termVal.ClientID %>');
                if (parseInt($txtAmount.val()) > 2000) {
                    $("#tdTerm").empty();
                    $("#tdTerm").append("<span>Transfer</span>");
                    $termVal.val("Transfer");
                } else {
                    $("#tdTerm").empty();
                    $("#tdTerm").append($dplTerm);
                    $termVal.val("");
                }
                var amount = Math.round($txtAmount.val() * Math.pow(10, 0)) / Math.pow(10, 0);
                $txtAmount.val(amount);
            } else {
                $txtAmount.val("");
                //                var msg = "<div id=\"msg\">Format Error!<br />Please Input Number!<br /><br /><br /></div>";
                //                ShowError(msg);
                alert("Please Input Number!");
                return;
            }
        });
        $("#right").click(function () {
            $("#msg").remove();
            $("#ErrorDiv").hide();
            $("#bgDiv").hide();
        }).mousemove(function () {
            $("#right").css("color", "red");
        }).mouseout(function () {
            $("#right").css("color", "#3d3d3d");
        });

        ChangeColor();
        HideTerm();
    });

    function HideTerm() {
        var $termVal = $('#<%=this.termVal.ClientID %>');
        if ($termVal.val() != "") {
            if (parseInt($termVal.val()) > 2000) {
                $("#tdTerm").empty();
                $("#tdTerm").append("<span>Transfer</span>");
            } else {
                $("#tdTerm").empty();
                $("#tdTerm").append("<span>Cash</span>");
            }
        }
    }


    function ChangeColor() {
        var level = '<%=this.Level %>';
        if (level != "") {
            var $rbtAttachInvoice = $(".rbtype input:radio:checked");
            if ($rbtAttachInvoice.val() != "Urgent") {
                $(".rbtype input:radio").each(function () {
                    $(this).attr("disabled", "disabled");
                });
            } else {
                $("#displaydiv").show();
            }
        }
        

    }

    function CheckInfo() {
        CreateForbidDIV();
        $(".wrapdiv").removeClass("wrapdiv");
        var result =  true;
        var company = $('#<%=this.txtCompany.ClientID %>').val();
        var department = $('#<%=this.txtDept.ClientID %>').val();
        var requestBy = $('#<%=this.txtRequestedBy.ClientID %>').val();
        var amount = $('#<%=this.txtAmount.ClientID %>').val();
        var msg = "";

        var mode = '<%=this.mode %>';
        if (mode == "new") {
            var $errmsg = $("td.pftd span:contains('No exact match was found.')");
            if ($errmsg.length > 0) {
                msg += "Please choose employee. \n";
                if (!$("td.pftd").find("span.ca-people-finder").parent().hasClass("wrapdiv")) {
                    $("td.pftd").find("span.ca-people-finder").wrap("<span class=\"wrapdiv\"></span>");
                }
                result = false;
            }
        }



//        if (company == "") {
//            msg = "<div id=\"msg\">Please Input Company Name !<br /><br /><br /></div>";
//            ShowError(msg);
//            return result;
//        }
//        if (department == "") {
//            msg = "<div id=\"msg\">Please Input Department Name !<br /><br /><br /></div>";
//            ShowError(msg);
//            return result;
//        }
//        if (requestBy == "") {
//            msg = "<div id=\"msg\">Please Input Applicant Name !<br /><br /><br /></div>";
//            ShowError(msg);
//            return result;
//        }
        if (amount == "") {
//            msg = "<div id=\"msg\">Please Input Cash Advance Amount !<br /><br /><br /></div>";
            //            ShowError(msg);
           // alert("Please Input Cash Advance Amount !");
            msg += "Please Input Cash Advance Amount.\n";
            var $txtAmount = $("#amount");
            $txtAmount.addClass("txtAmount");
            $('#<%=this.txtAmount.ClientID %>').focus(function () {
                $txtAmount.removeClass("txtAmount");
             });

             result = false;
        }
        var $rbtAttachInvoice = $(".rbtype input:radio:checked");
        if ($rbtAttachInvoice.val() == "Urgent") {
            var $txt = $("input.txtUrgentRemark");
            if ($txt.val() == "" || $txt.val() == "Please state reason for urgent request.") {
                if (!$txt.parent().hasClass("wrapdiv")) {
                    $txt.parent().addClass("wrapdiv");
                }
                msg += "Please Input Urgent Cash Advance Reasons.\n";
                result = false;
            }
        }

       // var $department = $('td.department input.ms-long');
        if (department== "") {
            if (!$('#<%=this.txtDept.ClientID %>').parent().hasClass("wrapdiv")) {
                $('#<%=this.txtDept.ClientID %>').wrap("<span class=\"wrapdiv\"></span>");
                msg += "Please Input Department Name.\n";
                result = false;
            }
        }
        var $Purpose = $("input.Purpose");
        if ($Purpose.val() == "") {
            $Purpose.wrap("<span class=\"wrapdiv\"></span>");
            msg += "Please Input Purpose .";
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

</script>
<script type="text/javascript">
    function CheckUsers() {
        //content
        //var finder = $(".ca-people-finder .ms-input").text(); substring
        var finder = $(".ca-people-finder span[id='content']").text();
        var lbRequestedBy = $("span.lbRequestedBy").text();
        var result = true;
        lbRequestedBy = $.trim(lbRequestedBy).replace(" ", ".").toLowerCase();
        var index = lbRequestedBy.indexOf("(");
        var last = lbRequestedBy.indexOf(")");
        lbRequestedBy = lbRequestedBy.substring(index+1, last);
//                alert(finder.toLowerCase());
//                alert(lbRequestedBy);
//                alert(finder.toLowerCase().indexOf(lbRequestedBy));
        if (finder.toLowerCase().indexOf(lbRequestedBy) == -1) {
            result = false;
        }
        return result;
        //return false;
    }

</script>
