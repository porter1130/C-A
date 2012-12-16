<%@ Page Title="" Language="C#" MasterPageFile="~/_Layouts/CA/Layout.Master" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" Inherits="CA.WorkFlow.UI.POTypeChange.EditForm" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>
<%@ Assembly Name="QuickFlow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<%@ Register src="DataEdit.ascx" tagname="DataEdit" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">  
PO Type Change
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
   <script type="text/javascript">
       function beforeSubmit(obj) {
           var isCheckOK = check();
           return isCheckOK;
       }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="WSSDesignConsole" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderWorkFlowName" runat="server">
PO Type Change
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <qfl:listformcontrol id="ListFormControl1" runat="server" formmode="Edit">
            <div class="ca-workflow-form-buttons">
                <div class="ca-workflow-form-buttons ">
                    <cc1:CAActionsButton ID="Actions" runat="server" CausesValidation="false" />
                    <QFC:MoreApproveButton runat="server" ID="More" />
                    <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/Lists/Tasks/MyItems.aspx'" />
                </div>
            </div>
            <uc2:DataEdit ID="DataEdit1" runat="server" ControlMode="Edit"  />
          <div class="trace-list-grid full-width">
             <cc1:TaskTraceList ID="Trace1" runat="server" BorderColor="#999999" BorderWidth="1px" GridLines="Horizontal" />
         </div>
        <SharePoint:FormDigest ID="FormDigest1" runat="server"></SharePoint:FormDigest>
    </qfl:listformcontrol>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderNotes" runat="server">
</asp:Content>
