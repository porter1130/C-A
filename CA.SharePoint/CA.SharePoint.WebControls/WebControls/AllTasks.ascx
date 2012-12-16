<%@ Register TagPrefix="sp" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllTasks.ascx.cs" Inherits="CA.SharePoint.WebControls.AllTasks" %>
<%@ OutputCache VaryByParam="TaskId" Duration="5" %>
<sp:SPGridView ID="gvList" runat="server" AutoGenerateColumns="False" AllowPaging="False"
    EnableViewState="false" AllowGrouping="true" AllowGroupCollapse="true" PageSize="3"
    GroupField="ModuleTitle" EmptyDataText="you don't have any task.">
    <AlternatingRowStyle CssClass="ms-alternating" />
    <SelectedRowStyle CssClass="ms-selectednav" Font-Bold="True" />
    <Columns>
        <sp:SPMenuField HeaderText="Task" MenuTemplateIdField="MenuTemplateIdField" TokenNameAndValueFields="WorkflowUrl=WorkflowUrl,Title=TaskTitle,Id=TaskId"
            TextFields="TaskTitle" NavigateUrlFields="WorkflowUrl" NavigateUrlFormat="{0}" />
        <asp:BoundField HeaderText="Status" DataField="Status" />
        <asp:BoundField HeaderText="Start Date" DataField="StartTime" />
    </Columns>
</sp:SPGridView>
<sp:MenuTemplate ID="MenuList" runat="server">
    <%--<sp:MenuItemTemplate ID="viewFormMenu" runat="server" Text="Open Task" ImageUrl="/_layouts/images/ApViewItem.gif" ClientOnClickNavigateUrl="%WorkflowUrl%" />--%>
    <sp:MenuItemTemplate ID="deleteForMenu" runat="server" Text="Delete Task" ClientOnClickScript="performPostBack('DELETE,%Id%');" />
</sp:MenuTemplate>
<sp:MenuTemplate ID="MenuTemplate1" runat="server">
    <sp:MenuItemTemplate ID="viewFormMenu" runat="server" Text="Open Task" ImageUrl="/_layouts/images/ApViewItem.gif"
        ClientOnClickNavigateUrl="%WorkflowUrl%" />
</sp:MenuTemplate>
<script type="text/javascript">
    function performPostBack(param) {
        var rs = confirm("Are you sure you want to delete this item?");
        if (rs = true) {
            alert(param);
            __doPostBack("<%=this.UniqueID %>", param);
        } else {
            return;
        }
    }
</script>
