<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="POQuery.aspx.cs" MasterPageFile="~/_Layouts/CA/Layout.Master" Inherits="CA.WorkFlow.UI.PurchaseOrder.POQuery" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    PO Query
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript">
        function CheckPO() {
            var result = false;
            var reg = new RegExp(" ", "g");
            var PO = $("#<%= TextBoxPONO.ClientID%>").val().replace(reg, "");
            if (PO.length == 0) {
                return result;
            }
            else {
                var len = PO.length;
                var lastEle = PO.slice(len - 1, len);
                if (lastEle == "R") {
                    result= true;
                }
            }
            return result;
        }
    </script>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
    PO Search
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="PlaceHolderMain" runat="server">
   <table class="ca-workflow-form-table full-width">
        <tr>
            <td class="label align-center w25">
                PO No.<br />
                PO号
            </td>
            <td class="label align-center w50">
                <asp:TextBox ID="TextBoxPONO" runat="server"></asp:TextBox>
            </td>
            <td class="label align-center w25">
                <div class="ca-workflow-form-buttons noPrint">
                        <asp:Button ID="ButtonQuery" runat="server" Text="Search" OnClientClick="return CheckPO()" onclick="ButtonQuery_Click" />
                </div>
            </td>
        </tr>
  </table>
  <br />
  <br />
   <table class="ca-workflow-form-table full-width">
        <tr>
            <td class="label" colspan="1">PO No.<br />PO号</td>
            <td class="label" colspan="3"><asp:HyperLink ID="HyperLinkPO" Target="_blank" runat="server"></asp:HyperLink></td>
        </tr>
        <tr>
            <td class="label w20">
                SAP Status<br />
                SAP状态
            </td>
            <td class="label w20">
                <asp:Label ID="LabelSAPStatus" runat="server"></asp:Label>
            </td>
            <td class="label w30">
               Is deducted from sap.<br />
               SAP 数量已减
            </td>
            <td class="label w20">
                <asp:Label ID="LabelSAPGRSR" runat="server"></asp:Label>
            </td>
        </tr>
        <asp:Repeater ID="RepeaterRelatedPO" runat="server">
                <ItemTemplate>
         <tr>
                <td class="label">related PO NO.<br />关联PO号</td>
                <td class="label">
                    <asp:HyperLink ID="HyperLinkPO" Target="_blank" Text='<%# Eval("PO")%>' NavigateUrl='<%# sDisplayURL+Eval("ID")%>' runat="server"></asp:HyperLink>
                </td>
                <td class="label w20"> Is related po completed SAP GR/SR<br />
                                        关联PO是否己做完 SAP 收货
                </td>
                <td class="label w20"><asp:Label ID="LabelCompletedGRSR" runat="server" Text='<%# Eval("IsGR")%>'></asp:Label></td>
         </tr>
                </ItemTemplate>
        </asp:Repeater>
  </table>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>