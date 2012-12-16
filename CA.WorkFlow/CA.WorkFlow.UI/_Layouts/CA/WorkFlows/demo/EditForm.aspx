<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" Inherits="CA.WorkFlow.UI.demo.EditForm" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.Controls" TagPrefix="QFC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <QFL:ListFormControl ID="ListFormControl1" runat="server" FormMode="Edit">
    <h2>
        招聘申请
    </h2>
    <hr />
    
     <p>
    单号：<asp:Label runat="server" ID="lblSn"></asp:Label>
    <asp:Label runat="server" ID="lblFlowStatus"></asp:Label>
    </p>
    <p> Employee Name
            <br />
            员工姓名
           <asp:Label runat="server" ID="lblEmployeeName" ></asp:Label>
   

            <p>  On-board Date
            <br />
            入职时间
             <asp:Label runat="server" ID="lblOnBoardDate"></asp:Label>
          <p>
    
    Management审批状态：<asp:Label runat="server" ID="lblManagementStatus"></asp:Label><br/>
    Hr审批状态  <asp:Label runat="server" ID="lblHrStatus"></asp:Label><br/>
    It审批状态：<asp:Label runat="server" ID="lblItStatus"></asp:Label><br/>
    <p><QFC:ActionsButton ID="actions" runat="server" /></p>
    </QFL:ListFormControl>
    </div>
    </form>
</body>
</html>
