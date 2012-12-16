<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataGRSR.ascx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequest.DataGRSR" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    .ca-grsr-table
    {
        margin-bottom: 0;
        border-top: 1px solid #9DABB6;
    }
    .ca-grsr-table td
    {
        padding: 5px;
        border-bottom: 1px solid #CCCCCC;
        border-right: 1px solid #CCCCCC;
        text-align: center;
        margin: 0;
    }
    .inner-div
    {
        border: 2px solid #9DABB6;
        margin: 5px;
        display:none;
    }
    .po-title
    {
        padding: 5px 5px 5px 10px;
    }
    .RedBorder
    {
            border:2px solid #FF0000;
        }
   .FloatRight
   {
       float:right;
       width:100%;
       }
   .ClearBoth
   {
        clear:both;
       }
   .FeedBackItem
   {
       padding-left:20px;
   }
   .FeedBackContent ul
   {
       margin:10px;
       }
   .FeedBackContent ul li
   {
       margin-top:10px;
       list-style:none;
       margin-bottom:5px;
       border-bottom-color:#c2c2c2;
       border-bottom-style:solid;
       border-bottom-width:1px;
       }
    .BigFont
    {   
        color:#383838;
        font-weight:bold;
        }
    .PONumberClick{
	    cursor:pointer;
	    background-image:url(../images/up.gif);  
	    background-repeat: no-repeat;
	    padding-left: 30px;
	    height:20px;
    }
    .EveryGRSR
    {
        width:150%;
        padding:5px;
        background-color:White;
        border:2px solid #383838;
        }
</style>
<script type="text/javascript">
    $(document).ready(function () {

        $('#<%= btnGRSR.ClientID%>').click(function () {
            var Result = CheckReceived();
            return Result;
        });
        $("input[type='radio']").click(function () {
            $(this).parents("table").removeClass("RedBorder");
        });

        $(".PONumberClick").toggle(function () {
            $(this).css("background-image", "url(../images/down.gif)");
            $(this).next(".inner-div").slideDown();
        }, function () {
            $(this).css("background-image", "url(../images/up.gif)");
            $(this).next(".inner-div").slideUp();
        });
    });

    ////收货的PO是否填写了反馈信息
    function CheckReceived() {
        var Result = false;
        $("input[type='checkbox'][checked]").each(function () {//遍历每一个选中的checkbox
            var feedBackClass = $(this).parent("span").attr("class");
            var selectedCount = checkFeedBackSelected($(".F_" + feedBackClass));
            if (selectedCount == 5) {
                Result = true;
            }
            else {
                return;
            }
        });
        return Result;
    }
    //每个反馈信息是否都填写完
    function checkFeedBackSelected(obj) {
        var selectedCount = 0;
        obj.each(function () {
            if ($(this).find("input[type='radio'][checked]").length == 1) {
                selectedCount++;
            }
            else {
                $(this).addClass("RedBorder");
            }
        });
        return selectedCount;
    }
</script>

<asp:HiddenField ID="HFIsHO" runat="server" />
<table class="FloatRight" >
    <tr>
        <td >PR Number(号):&nbsp;<asp:TextBox ID="TextBoxPR"  Width="100px" runat="server"></asp:TextBox>    &nbsp;&nbsp; PO Number(号):&nbsp;<asp:TextBox ID="TextBoxPO"  Width="100px" runat="server"></asp:TextBox></td>
        <td ><div class="ca-workflow-form-buttons"><asp:Button ID="ButPOQuery" style="width:80px"   runat="server" Text="Query(查询)" onclick="ButPOQuery_Click" /></div></td>
        <td>
            <div class="ca-workflow-form-buttons"><asp:Button ID="btnGRSR" style="width:80px"  runat="server" Text="GR(收货)" OnClick="btnGRSR_Click" /></div>
        </td>
        <td>
            <div class="ca-workflow-form-buttons"><input id="GoBack" style="width:50px" type="button" value="Back" onclick="location.href('/WorkFlowCenter/Lists/PurchaseRequestWorkflow/MyApply.aspx')" /></div>
        </td>
    </tr>
</table>
<div class="ClearBoth"></div>
<div class="EveryGRSR">
    <ul>
    <asp:Repeater ID="rptPOs" runat="server" OnItemDataBound="rptPOs_ItemDataBound">
            <ItemTemplate>
                <li style="margin-top:20px;">
                <div class="PONumberClick">
                    <table style="width:100%;background-color:White" cellpadding="5" cellpadding="5">
                        <tr>
                            <td>PO：<%# Eval("PONumber")%></td>
                            <td>Item Count(申请条数):<asp:Label ID="LabelCount" runat="server"></asp:Label></td>
                            <td>Requested by(申请者): <asp:Label ID="LabelPRBy" runat="server"></asp:Label></td>
                            <td>PO Created(创建日期): <asp:Label ID="LabelCreated" runat="server"></asp:Label></td>
                            <td><asp:Label ID="LabelVendor" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </div>
                    <div class="inner-div">
                        <div class="po-title">
                            <table>
                                <tr>
                                    <td style="width:50px;">
                                        <span class="BigFont">PO No:</span>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="LabelPOnumber" runat="server" Text='<%# ((DataRowView)Container.DataItem)["PONumber"]%>'></asp:Label>
                                        <asp:HiddenField ID="HFVendorIDName" runat="server" />
                                    </td>
                                    <td style="width: 430px;"></td>
                                    <td align="right" style="width:200px;" >
                                        <span class="BigFont">Is Received (是否收货)?&nbsp;
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hidPONumber" runat="server" Value='<%# ((DataRowView)Container.DataItem)["PONumber"]%>' />
                                        <asp:CheckBox ID="cbIsReceived" CssClass='<%#"CheckDataFull"+ Container.ItemIndex%>' runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table class="ca-grsr-table">
                            <tr>
                                <td style="width: 50px;">
                                    <span class="BigFont">No
                                    <br />
                                    序号</span>
                                </td>
                                <td style="width: 50px;">
                                    <span class="BigFont">Cost&nbsp;Center<br/> 门店</span>
                                </td>
                                <td style="width: 100px;">
                                    <span class="BigFont">Item Code
                                    <br />
                                    产品代码</span>
                                </td>
                                <td style="width: 453px;">
                                    <span class="BigFont">Description
                                    <br />
                                    描述</span>
                                </td>
                                <td style="width: 80px;">
                                    <span class="BigFont">Total Quantity
                                    <br />
                                    订购量</span>
                                </td>
                            </tr>
                            <asp:Repeater ID="rptItems" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%# Container.ItemIndex + 1 %>
                                        </td>
                                        <td>
                                            <%# Eval("CostCenter")%>
                                        </td>
                                        <td>
                                            <%# ((DataRowView)Container.DataItem)["ItemCode"]%>
                                        </td>
                                        <td style="text-align: left;">
                                            <%# ((DataRowView)Container.DataItem)["Description"]%>
                                        </td>
                                        <td>
                                            <%# ((DataRowView)Container.DataItem)["TotalQuantity"]%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <div class="FeedBackContent">
                            <ul>
                                <li><span class="BigFont">Standhard&quality标准及服务质量：</span>
                                    <br />
                                    <div class="FeedBackItem">
                                        <asp:RadioButtonList ID="RadioListStandhard" CellSpacing="10" CssClass='<%#"F_CheckDataFull"+ Container.ItemIndex%>' runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Good" Text="Good好" />
                                            <asp:ListItem Value="Acceptable" Text="Acceptable合格" />
                                            <asp:ListItem Value="Poor" Text="Poor不合格" />
                                        </asp:RadioButtonList>
                                    </div>
                                </li>
                                <li><span class="BigFont">Productqty产品数量：</span>
                                    <br />
                                    <div class="FeedBackItem">
                                        <asp:RadioButtonList ID="RadioListQuantity" CellSpacing="10" runat="server" CssClass='<%#"F_CheckDataFull"+ Container.ItemIndex%>' RepeatDirection="Horizontal">
                                            <asp:ListItem Value="MatchOrdering" Text="Match Ordering正确" />
                                            <asp:ListItem Value="NotMatchOrdering" Text="Not Match Ordering不正确" />
                                        </asp:RadioButtonList>
                                    </div>
                                </li>
                                <li><span class="BigFont">Delivery Time 送货时间：</span>
                                    <br />
                                    <div class="FeedBackItem">
                                        <asp:RadioButtonList ID="RadioListDelivery" CellSpacing="10" runat="server" CssClass='<%#"F_CheckDataFull"+ Container.ItemIndex%>' RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Early" Text="Early提前到" />
                                            <asp:ListItem Value="OnTime" Text="On Time准时到" />
                                            <asp:ListItem Value="Delay" Text="Delay超时到" />
                                        </asp:RadioButtonList>
                                    </div>
                                </li>
                                <li><span class="BigFont">Service Manner服务态度：</span>
                                    <br />
                                    <div class="FeedBackItem">
                                        <asp:RadioButtonList ID="RadioListManner" CellSpacing="10" runat="server" CssClass='<%#"F_CheckDataFull"+ Container.ItemIndex%>' RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Satisfied" Text="Satisfied满意" />
                                            <asp:ListItem Value="Acceptable" Text="Acceptable一般" />
                                            <asp:ListItem Value="Unacceptable" Text="Unacceptable不满意" />
                                        </asp:RadioButtonList>
                                    </div>
                                </li>
                                <li><span class="BigFont">Response响应速度：</span>
                                    <br />
                                    <div class="FeedBackItem">
                                        <asp:RadioButtonList ID="RadioListResponse" CellSpacing="10" runat="server" CssClass='<%#"F_CheckDataFull"+ Container.ItemIndex%>' RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Fast" Text="Fast快" />
                                            <asp:ListItem Value="Acceptable" Text="Acceptable一般" />
                                            <asp:ListItem Value="Slow" Text="Slow慢" />
                                        </asp:RadioButtonList>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>

<script type="text/javascript">


</script>