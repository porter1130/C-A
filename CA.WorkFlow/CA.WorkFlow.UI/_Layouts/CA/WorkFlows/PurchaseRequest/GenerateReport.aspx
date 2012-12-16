﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateReport.aspx.cs" 
MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.PurchaseRequest.GenerateReport" %>

<%@ Register Src="DataReport.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
   
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var title = $('#<%= HFTitle.ClientID%>').val();
            document.title = title;
            $('.PageTitle').text(title);
        });
    </script>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
     <div class="PageTitle"></div>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="HFTitle" runat="server" />
    <uc1:DataForm ID="DataForm1" runat="server" />
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>