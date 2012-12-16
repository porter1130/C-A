<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="GRSRReport.aspx.cs" Inherits="CA.WorkFlow.UI.PurchaseRequest.GRSRReport" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"  Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<style type="text/css">
    .w14
    {
            width:14%;
        }
    .w10
    {
        width:10%;
        }
    .w8
    {
            width:8%;
        }
</style>
<script type="text/javascript">
//    $(document).ready(function () {
//        $("#<%= ButtonQuery.ClientID%>").click(function () {
//            var IsCheck = Check();
//            return IsCheck;
//        });

//        $("#<%= ButtonExportExcel.ClientID%>").click(function () {
//            var IsCheck = Check();
//            return IsCheck;
//        });

//        function Check() {
//            var isCheck = true;
//            var selectIndex = $("#<%= DropdownVendor.ClientID%>").get(0).selectedIndex;
//            if (selectIndex == 0) {
//                isCheck = false;
//            }
//            return isCheck;
//        }

//    });

        function popexcel(url) {
            var w = window.open(url, '_blank');
            w.location.href = url;
        }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    GR/SR Feedback Report
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<table>
    <tr>
        <td>
            <cc1:CADateTimeControl ID="CADateTimeFrom" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" />
        </td>
        <td>
          <cc1:CADateTimeControl ID="CADateTimeTo" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" />
        </td>
        <td>
            <asp:DropDownList ID="DropdownVendor" runat="server"></asp:DropDownList>
        </td>
        <td>
            <div class="ca-workflow-form-buttons">
                <asp:Button ID="ButtonQuery" Text="Query" runat="server" onclick="ButtonQuery_Click" />
            </div>
        </td>
        <td>
            <div class="ca-workflow-form-buttons">
                <asp:Button ID="ButtonExportExcel" Text="Export" runat="server" onclick="ButtonExportExcel_Click" />
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="5">
            <table style="width:900px;background-color:White" class="ca-workflow-form-table">
                <tr>
                    <td class="label w14">Assessement Item<br />评估项目</td>
                    <td class="label w10">Total Qty<br />每项总和</td>
                    <td class="label w8">Level One<br />一级</td>
                    <td class="label w8">Sum Qty.<br />汇总评次</td>
                    <td class="label w8">Ratio<br />占比</td>
                    <td class="label w10">Level Two<br />二级</td>
                    <td class="label w8">Sum Qty<br />.汇总评次</td>
                    <td class="label w8">Ratio<br />占比</td>
                    <td class="label w10">Level Three<br />三级</td>
                    <td class="label w8">Sum Qty.<br />汇总评次</td>
                    <td class="label w8">Ratio<br />占比</td>
                </tr>
                <tr>
                    <td class="label">Standard & quantity<br />标准及服务质量：</td>
                    <td class="label"><asp:Label ID="LabelStandardTotalQty" Text="0" runat="server" ></asp:Label></td>
                    <td class="label">Good<br />好</td>
                    <td class="label"><asp:Label ID="LabelStandardOneCount" Text="0" runat="server"></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelStandardOneRatio" Text="0%"  runat="server"></asp:Label></td>
                    <td class="label">Acceptable<br />合格</td>
                    <td class="label"><asp:Label ID="LabelStandardTwoCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelStandardTwoRatio" runat="server" Text="0%" ></asp:Label></td>
                    <td class="label">Poor<br />不合格</td>
                    <td class="label"><asp:Label ID="LabelStandardThreeCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelStandardThreeRatio" runat="server" Text="0%" ></asp:Label></td>
                </tr>
                <tr>
                    <td class="label">Product qty.<br />产品数量：</td>
                    <td class="label"><asp:Label ID="LabelProductQtyTotalQty" runat="server"  Text="0" ></asp:Label></td>
                    <td class="label">Match Ordering<br />正确</td>
                    <td class="label"><asp:Label ID="LabelProductQtyOneCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelProductQtyOneRatio" runat="server" Text="0%" ></asp:Label></td>
                    <td class="label">N/A</td>
                    <td class="label"></td>
                    <td class="label"></td>
                    <td class="label">Not Match Ordering<br />不正确</td>
                    <td class="label"><asp:Label ID="LabelProductQtyThreeCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelProductQtyThreeRatio" runat="server" Text="0%" ></asp:Label></td>
                </tr>
                <tr>
                    <td class="label">Delivery Time.<br />送货时间：</td>
                    <td class="label"><asp:Label ID="LabelDeliveryTotal" runat="server"  Text="0" ></asp:Label></td>
                    <td class="label">Early<br />提前到</td>
                    <td class="label"><asp:Label ID="LabelDeliveryOneCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelDeliveryOneRatio" runat="server" Text="0%" ></asp:Label></td>
                    <td class="label">On Time<br />准时到</td>
                    <td class="label"><asp:Label ID="LabelDeliveryTwoCount" runat="server"  Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelDeliveryTwoRatio" runat="server"  Text="0%" ></asp:Label></td>
                    <td class="label">Delay<br />超时到</td>
                    <td class="label"><asp:Label ID="LabelDeliveryThreeCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelDeliveryThreeRatio" runat="server" Text="0%" ></asp:Label></td>
                </tr>
                <tr>
                    <td class="label">Service Manner<br />服务态度：</td>
                    <td class="label"><asp:Label ID="LabelServiceTotal" runat="server"  Text="0" ></asp:Label></td>
                    <td class="label">Satisfied<br />满意</td>
                    <td class="label"><asp:Label ID="LabelServiceOneCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelServiceOneRatio" runat="server" Text="0%" ></asp:Label></td>
                    <td class="label">Acceptable<br />一般</td>
                    <td class="label"><asp:Label ID="LabelServiceTwoCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelServiceTwoRatio" runat="server" Text="0%" ></asp:Label></td>
                    <td class="label">Unsatisfied<br />不满意</td>
                    <td class="label"><asp:Label ID="LabelServiceThreeCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelServiceThreeRatio" runat="server" Text="0%" ></asp:Label></td>
                </tr>
                <tr>
                    <td class="label">Respond<br />响应速度：</td>
                    <td class="label"><asp:Label ID="LabelRespondCount" runat="server" Text="0"  ></asp:Label></td>
                    <td class="label">Fast<br />快</td>
                    <td class="label"><asp:Label ID="LabelRespondOneCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelRespondOneRatio" runat="server" Text="0%" ></asp:Label></td>
                    <td class="label">Acceptable<br />一般</td>
                    <td class="label"><asp:Label ID="LabelRespondTwoCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelRespondTwoRatio" runat="server" Text="0%" ></asp:Label></td>
                    <td class="label">Slow<br />慢</td>
                    <td class="label"><asp:Label ID="LabelRespondThreeCount" runat="server" Text="0" ></asp:Label></td>
                    <td class="label"><asp:Label ID="LabelRespondThreeRatio" runat="server" Text="0%" ></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
</table>

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
