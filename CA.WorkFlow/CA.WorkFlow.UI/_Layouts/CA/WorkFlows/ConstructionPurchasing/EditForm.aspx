<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Application.Master"
    AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" Inherits="CA.WorkFlow.UI.ConstructionPurchasing.EditForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<%@ Register Src="DataForm.ascx" TagName="DataForm" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    Construction purchasing Request workflow-非商品采购流程
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <script language="javascript" type="text/javascript">
        function strUrl(obj) {
            var url = document.getElementById("<%=hdUrl.ClientID %>").value;
            obj.href = url;
        }
        function CheckComments() {
            var obj = "<%=body.ClientID %>";
            obj = obj + "_ctl00_ctl00_TextField";
            if (Trim(document.getElementById(obj).value) == "") {
                alert("Please supply comments when reject a request.");
                document.getElementById(obj).focus();
                return false;
            }
        }
        function Trim(s) {
            var ss = s.replace(/^[\s]+/g, "");
            ss = ss.replace(/[\s]+$/g, "");
            return ss;
        }
        $().ready(function() {
            if ($(".ms-ButtonHeightWidth").length > 1) {

                $($(".ms-ButtonHeightWidth")[1]).bind("click", function() {
                    var obj = "<%=body.ClientID %>";
                    obj = obj + "_ctl00_ctl00_TextField";
                    if (Trim(document.getElementById(obj).value) == "") {
                        alert("Please supply comments when reject a request.");
                        document.getElementById(obj).focus();
                        return false;
                    }
                });
            }
        })
    </script>

    <br />
    <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <table width="680" border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="80%">
                                <div class="workflow_table_button" style="padding-bottom: 10px">
                                    <QFC:ActionsButton ID="actions" runat="server" />
                                    <asp:Button runat="server" Text="Save" ID="btnSave" OnClick="btnSave_Click" />
                                    <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                            </td>
                            <td width="20%" align="right">
                                <a id="hrefPDF" onclick="strUrl(this)" style="cursor: hand" runat="server">ExportExcel</a><input
                                    id="hdUrl" type="hidden" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="workflow_table3">
                                    <tr>
                                        <th>
                                            Comments:
                                        </th>
                                        <td>
                                            <QFL:TaskPanel runat="server" ID="task1">
                                                <QFL:CommentTaskField runat="server" ID="body" ControlMode="Edit" />
                                            </QFL:TaskPanel>
                                            <input id="hdBody" runat="server" type="hidden" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <uc1:DataForm ID="DataForm1" runat="server" ControlMode="Edit" />
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <cc1:TaskTraceList ID="TaskTraceList1" runat="server">
                                            </cc1:TaskTraceList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="596" border="0" align="center" cellpadding="0" cellspacing="0" class="note">
                                    <tr>
                                        <th valign="top" class="top">
                                            &nbsp;
                                        </th>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <a href="/WorkFlowCenter/FlowCharts/ConstructionPurchasing.doc" style="cursor: hand;
                                                color: Blue; font-size: 14px">Click here to view the flowchart of the workflow</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th valign="top" class="foot">
                                            &nbsp;
                                        </th>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <SharePoint:FormDigest ID="FormDigest1" runat="server">
        </SharePoint:FormDigest>
    </QFL:ListFormControl>
</asp:Content>
