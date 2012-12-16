<%@ Page Title="New Trade Supplier Creation" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master"
    AutoEventWireup="true" CodeBehind="ApproveForm.aspx.cs" Inherits="CA.WorkFlow.UI.NTSC.ApproveForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<%@ Register Src="DataView.ascx" TagName="DataForm" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/TaskTrace.ascx" TagName="TaskTrace" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    New Trade Supplier Creation Form
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link type="text/css" rel="stylesheet" href="/_layouts/CAResources/themeCA/css/sp-override.css" />
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
    <script type="text/javascript" src="/_layouts/CAResources/themeCA/js/ca.site.js"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName"
    runat="server">
    New Trade Supplier Creation Form
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="ca-supplier-form">
        <asp:Label runat="server" CssClass="clr-red" ID="lblError" />
        <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="New">
            <div class="ca-workflow-form-buttons noPrint">
                <cc1:CAActionsButton ID="Actions" runat="server" CausesValidation="false" />
                <QFC:MoreApproveButton runat="server" ID="More" />
                <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx'" />
            </div>
            <table class="ca-workflow-form-table full-width" id="commettable">
                <tr>
                    <td class="label align-center w25">
                        Comments<br />
                        批注:
                    </td>
                    <td class="value" id="comment-task">
                        <QFL:TaskPanel runat="server" ID="task1">
                            <QFL:CommentTaskField runat="server" ID="CommentTaskField1" ControlMode="Edit"/>
                        </QFL:TaskPanel>
                    </td>
                </tr>
            </table>
            <uc1:DataForm ID="DataForm" runat="server">
            </uc1:DataForm>
            <uc2:TaskTrace ID="TaskTrace" runat="server" />
            <SharePoint:FormDigest ID="FormDigest3" runat="server">
            </SharePoint:FormDigest>
        </QFL:ListFormControl>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
    <div class="ca-workflow-form-note noPrint">
        <div class="top">
            &nbsp;</div>
        <div class="middle">
            Please write down your business plan under <b>Reason Box</b> .<br />
            Don’t forget to upload the supplier profile and factory audit assessment form with
            <b>completed information</b> .
        </div>
        <div class="foot">
            &nbsp;</div>
    </div>
    <script type="text/javascript">
        function dispatchAction(sender) {
            if (sender.value === 'Reject') {
                if (ca.util.emptyString($('#comment-task textarea').val())) {
                    if (jQuery.browser.msie) {
                        event.cancelBubble = true;
                    }
                    alert('Please fill in the Reject Comments.');
                    return false;
                }
            }
            return true;
        }
    </script>
</asp:Content>
