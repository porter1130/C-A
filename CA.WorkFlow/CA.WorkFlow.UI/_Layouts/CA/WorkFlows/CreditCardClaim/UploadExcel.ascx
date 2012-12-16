<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadExcel.ascx.cs"
    Inherits="CA.WorkFlow.UI.CreditCardClaim.UploadExcel" %>
<style type="text/css">
    #uploadLog span.span_fontcolor
    {
        color: #0066CC;
    }
</style>
<script type="text/javascript" src="/_layouts/CAResources/themeCA/js/jquery-1.4.4.min.js"></script>
<div>
    <table id="tb_uploadsection">
        <tr>
            <td class="td_radiobuttons">
                <asp:RadioButtonList ID="rblUploadYear" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Last Year" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Current Year" Value="1" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="ca-workflow-form-buttons">
                <asp:FileUpload ID="uploadAttachment" runat="server" Style="width: 350px; border: 1px solid gray;" />
                <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClientClick="return IsExistMonth()"
                    OnClick="btnUpload_Click" Style="width: auto; border: 1px solid gray;" />
            </td>
        </tr>
    </table>
</div>
<div id="uploadLog">
    <table class="table_uploadLog" cellspacing="5px">
        <tr>
            <th>
                上传详细信息
            </th>
        </tr>
        <tr>
            <td class="w50">
                Excel文档中总记录条数:
            </td>
            <td class="align-left">
                <span class="span_fontcolor">
                    <asp:Label ID="lblDistinctRecords" runat="server"></asp:Label></span>
            </td>
        </tr>
        <tr>
            <td class="w50">
                其中工资记录条数:
            </td>
            <td class="align-left">
                <span class="span_fontcolor">
                    <asp:Label ID="lblSalary" runat="server"></asp:Label></span>
            </td>
        </tr>
        <tr>
            <td class="w50">
                已导入的总记录条数:
            </td>
            <td class="align-left">
                <span class="span_fontcolor">
                    <asp:Label ID="lblRecordsCount" runat="server"></asp:Label></span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblCardInfoTitle" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <span class="span_fontcolor">
                    <asp:Label ID="lblCardInfo" runat="server">
                    </asp:Label></span>
            </td>
        </tr>
        <!--failed to start workflow user list-->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblApplicantsTitle" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <span class="span_fontcolor">
                    <asp:Label ID="lblApplicantsInfo" runat="server">
                    </asp:Label></span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblSAPNoTitle" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <span class="span_fontcolor">
                    <asp:Label ID="lblSAPNo" runat="server">
                    </asp:Label></span>
            </td>
        </tr>
    </table>
</div>
<script type="text/javascript">
    var uploadAttachment = '#<%=uploadAttachment.ClientID %>';


    function IsExistMonth() {
        var isExistMonth = true;
        var fileName = $(uploadAttachment).val();
        $.ajax({
            type: 'POST',
            url: 'Handler.ashx',
            async: false,
            data: {
                fileFullPath: $(uploadAttachment).val(),
                isCurrentYear: $('#tb_uploadsection td.td_radiobuttons :radio:checked').val()
            },
            success: function (data) {
                if (data == 'True') {
                    if (!confirm('The data of current month already exists, if you want to continue?')) {
                        isExistMonth = false;
                    }
                }
            }
        });
        return isExistMonth;
    }

</script>
