<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI._Layouts.CA.WorkFlows.PADChangeRequest.DataEdit" %>
<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628"
    Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<style type="text/css">
    select.width-fix
    {
        width: 60px;
        z-index: 1000;
    }
    select.expand
    {
        position: absolute;
        width: 270px; /* Let the browser handle it. */
    }
    #dialog input
    {
        font-size: 12px;
    }
    #DataEditUserInfotb td input
    {
        width: 100px;
        border: 0;
    }
    
</style>
<table id="table_comment" class="ca-workflow-form-table full-width">
                <tr>
                    <td class="label align-center w25">
                        Remarks<br />
                        备注:
                    </td>
                    <td class="value" id="comment-task">
                    <QFL:FormField ID="RemarksField" runat="server" FieldName="Remarks">
                        </QFL:FormField>
                    </td>
                </tr>
            </table>
<br />
<table id="DataEditUserInfotb" class="ca-workflow-form-table">

    <tr>
        <td class="label align-center w25" style="width:200px">
            选择员工<br />Choose Employee
            
        </td>
        <td class="value" colspan="5">
            <cc1:CAPeopleFinder ID="cpfUser" runat="server" AllowTypeIn="true" MultiSelect="false" 
                CssClass="ca-people-finder" Width="350"   />
        </td>
    </tr>
    <tr> <td class="label align-center w25" style="width:200px;">申请人<br />
            Requested By
        </td>
        <td class="label align-center w20" ><QFL:FormField ID="ApplicantField" runat="server" FieldName="Applicant">
                        </QFL:FormField>
        </td>
        <td class="label align-center w22" style="width:120px;">部门<br />Dept
        </td>
        <td class="label align-center w20"><QFL:FormField ID="DepartmentField" runat="server" FieldName="Department">
                        </QFL:FormField></td>
        <td class="label align-center w25" style="width:120px;">姓名<br />Name</td>
        <td class="label align-center w20"><QFL:FormField ID="ChineseNameField" runat="server" FieldName="ChineseName">
                        </QFL:FormField></td>
        </tr>
</table>


<asp:Button ID="btnPeopleInfo" runat="server" OnClick="btnPeopleInfo_Click" CausesValidation="False"
   CssClass="hidden" />
<script type="text/javascript">
    $('#<%=this.cpfUser.ClientID %>' + '_checkNames').click(function () {
        // alert("11");
        ttclick();
        //setTimeout('$("#<%=this.btnPeopleInfo.ClientID %>").click()', 2000);
    });
    function ttclick() {
        $("#<%=this.btnPeopleInfo.ClientID %>").click();
  }
</script>