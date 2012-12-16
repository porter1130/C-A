<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="CA.WorkFlow.UI.POTypeChange.Report" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
POTypeChange Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
 <script type="text/javascript">
     function popexcel(url) {
         var w = window.open(url, '_blank');
         w.location.href = url;
     }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
POTypeChange Report
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
     <table style="background-color:White;padding:10px;margin:10px;">
        <tr>
            <td ><cc1:CADateTimeControl ID="CADateTimeFrom" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></td>
            <td><cc1:CADateTimeControl ID="CADateTimeTo" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></td>
            <td>Type:</td>
            <td>
               <asp:DropDownList ID="DDLType" runat="server">
                    <asp:ListItem Value="NORM">CS==>NORM</asp:ListItem>
                    <asp:ListItem Value="CS">NORM==>CS</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <div class="ca-workflow-form-buttons">
                    <asp:Button ID="ButtonQuery" Text="Query" runat="server" onclick="ButtonQuery_Click" />
                </div>
            </td>
            <td><div class="ca-workflow-form-buttons"><asp:Button ID="ButtonExport" Text="Export" runat="server" onclick="ButtonExport_Click" /></div></td>
        </tr>
        </table>
        
    <table style="background-color:White">
        <tr>
            <td colspan="5" >
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="uplCustomer" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <SharePoint:SPGridView ID="SPGridViewReport" runat="server" AutoGenerateColumns="False"
                                    AllowPaging="True" PageSize="100" OnPageIndexChanging="SPGridViewReport_PageIndexChanging"
                                    BorderColor="#9dabb6" BorderStyle="Solid" BorderWidth="1px" CssClass="ms-listviewtable inner-table"
                                    EnableTheming="False" GridLines="Horizontal">
                                    <AlternatingRowStyle CssClass="each-row ms-alternating" />
                                    <RowStyle CssClass="each-row ReaportWhiteColor" />
                                    <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
                                </SharePoint:SPGridView>
                                <div class="align-center">
                                    <SharePoint:SPGridViewPager ID="SPGridViewPager1" runat="server" GridViewId="SPGridViewReport">
                                    </SharePoint:SPGridViewPager>
                                </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickNext" />
                                    <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickPrevious" />
                                </Triggers>
                        </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>

