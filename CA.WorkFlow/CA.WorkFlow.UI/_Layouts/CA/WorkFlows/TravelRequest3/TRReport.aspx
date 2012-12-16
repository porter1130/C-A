<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/_Layouts/CA/Layout.Master"
    CodeBehind="TRReport.aspx.cs" Inherits="CA.WorkFlow.UI._Layouts.CA.WorkFlows.TravelRequest3.TRReport" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    TR Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript">
    </script>
    <style type="text/css">
        .ReaportWhiteColor
        {
            background-color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    TR Report
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="div_table">
        <div class="div_tr">
            <div class="div_td">
                <span>From:<SharePoint:DateTimeControl ID="dtPeriodFrom" runat="server" DateOnly="true" />
                </span>
            </div>
            <div class="div_td">
                <span>To:<SharePoint:DateTimeControl ID="dtPeriodTo" runat="server" DateOnly="true" />
                </span>
            </div>
        </div>
        <div class="div_tr">
            <div class="div_td">
                <asp:DropDownList ID="ddlCostCenter" Visible="false" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div class="div_tr">
            <div class="div_td">
                <asp:Button ID="btnQuery" runat="server" Text="Query" OnClick="btnQuery_Click" />
            </div>
            <div class="div_td">
                <asp:Button ID="btnReport" runat="server" Text="Export Report" OnClick="btnReport_Click" />
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="uplCustomer" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <SharePoint:SPGridView ID="SPGridView1" runat="server" AutoGenerateColumns="False"
                AllowPaging="True" PageSize="100" OnPageIndexChanging="SPGridView1_PageIndexChanging"
                BorderColor="#9dabb6" BorderStyle="Solid" BorderWidth="1px" CssClass="ms-listviewtable inner-table"
                EnableTheming="False" GridLines="Horizontal">
                <AlternatingRowStyle CssClass="each-row ms-alternating" />
                <RowStyle CssClass="each-row ReaportWhiteColor" />
                <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
                <Columns>
                    <SharePoint:SPBoundField HeaderText="Chinese Name" DataField="ChineseName" />
                    <SharePoint:SPBoundField HeaderText="Department" DataField="Department" />
                    <SharePoint:SPBoundField HeaderText="Cost Center" DataField="CostCenter" />
                    <SharePoint:SPBoundField HeaderText="Travel period_from" DataField="TravelDateFrom" />
                    <SharePoint:SPBoundField HeaderText="Travel period_to" DataField="TravelDateTo" />
                    <SharePoint:SPBoundField HeaderText="Travel location_from" DataField="TravelLocationFrom" />
                    <SharePoint:SPBoundField HeaderText="Travel location_to" DataField="TravelLocationTo" />
                </Columns>
            </SharePoint:SPGridView>
            <div class="align-center">
                <SharePoint:SPGridViewPager ID="SPGridViewPager1" runat="server" GridViewId="SPGridView1">
                </SharePoint:SPGridViewPager>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickNext" />
            <asp:AsyncPostBackTrigger ControlID="SPGridViewPager1" EventName="ClickPrevious" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
