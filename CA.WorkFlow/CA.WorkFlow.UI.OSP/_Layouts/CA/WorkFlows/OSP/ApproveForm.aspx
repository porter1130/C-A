<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="ApproveForm.aspx.cs" Inherits="CA.WorkFlow.UI.OSP.ApproveForm" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %> 

<%@ Register src="Userinfo.ascx" tagname="Userinfo" tagprefix="uc1" %>
<%@ Register src="DataView.ascx" tagname="DataView" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
   OSP Workflow
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
   OSP Workflow
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <QFL:ListFormControl ID="ListFormControl3" runat="server" FormMode="New">
            <div class="ca-workflow-form-buttons">
                <div class="ca-workflow-form-buttons ">
                    <cc1:CAActionsButton ID="Actions" runat="server" CausesValidation="false" />
                    <QFC:MoreApproveButton runat="server" ID="More" />
                    <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx'" />
                </div>
            </div>
        
            <table id="table_comment" class="ca-workflow-form-table full-width">
                <tr>
                    <td class="label align-center w25">
                        Comments<br />
                        批注:
                    </td>
                    <td class="value" id="comment-task">
                        <QFL:TaskPanel runat="server" ID="task1">
                            <QFL:CommentTaskField runat="server" ID="CommentTaskField1" ControlMode="Edit"  />
                        </QFL:TaskPanel>
                    </td>
                </tr>
            </table>
            <br />

      <uc2:DataView ID="DataView1" runat="server"  ControlMode="Display" />
      <%--<uc2:TaskTrace ID="TaskTrace2" runat="server" />--%>
     <%-- <div class="trace-list-grid full-width"> <cc1:TaskTraceList ID="Trace1" runat="server" BorderColor="#999999"  BorderWidth="1px" GridLines="Horizontal" /></div>--%>
      <div class="trace-list-grid full-width">
          <cc1:TaskTraceList ID="Trace1" runat="server" BorderColor="#999999" BorderWidth="1px" GridLines="Horizontal" />
       </div>
       
        <SharePoint:FormDigest ID="FormDigest3" runat="server">
        </SharePoint:FormDigest>
        
    </QFL:ListFormControl>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
