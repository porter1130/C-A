<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.POTypeChange.DataEdit" %>

<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<script type="text/javascript">
    var regFloat = /^[0-9]+(\.?)(\d*)$/;
    $(document).ready(function () {
        TypeChange();

        $(".POCount").each(function () {
            var ddlType = $(this).find(".DDLType");
            var newPADObje = $(this).find(".DateTimePAD").find("input");
            newPADObje.attr("readonly", true);

            if (ddlType.children("input").val() == "1" && ddlType.next("input").val() == "1") {//已经修改过的就不显示
                ddlType.parents(".ca-workflow-form-table").hide();
            }
            else if (ddlType.children("input").val() == "1") {//PO type己经成功更新到SAP
                ddlType.find("select").hide(); //.attr("disabled", "disabled");
                var tye = ddlType.prev("input").val();
                ddlType.append(tye + " (Updated to SAP Success!)")//.find("select").attr("title","Updated to SAP Success!")
            }
            else if (ddlType.next("input").val() == "1") {//PAD成功更新到SAP
                $(this).find(".DateTimePAD").find("a").hide();
                $(this).find(".DateTimePAD").find("input").attr("title", "Updated to SAP Success!")
            }

            /******************设置数量的格式**************************************************/
            var qtyObj = $(this).find(".Qty");
            var qty = qtyObj.text();
            if (regFloat.test(qty)) {
                qtyObj.text(parseFloat(qty).toFixed(0));
            }
        });


        $("#Test").click(function () {
            if (beforeSubmit(this)) {
                ClearForbidDIV();
            }
        });
    });
    function beforeSubmit(obj) {
        CreateForbidDIV();
        if (!CheckCount() || !CheckType()|| !CheckIsAllocated()|| !CheckDateSpan()){
            ClearForbidDIV();
            return false;
        }
        else {
            return true;
        }
    }
    ///验证Item的个数
    function CheckCount() {
        var isOK = true;
        isOK = $(".POCount").length > 0;
        return isOK;
    }
    ///dropdownlist里的Type值 发生连动时的事件。
    function TypeChange() {
        $(".POCount").find(".NewType").find(".DDLType>select").each(function () {
            $(this).change(function () {
                SetValue($(this).children("option:selected").val())
            });
        });
    }

    ///设置dropdownlist的值连动
    function SetValue(val) {
        $(".POCount").find(".NewType").find(".DDLType>select").each(function () {
            if ($(this).children("option:selected").text() == "") {
                $(this).val(val)
            }
        });
    }

    ///验证DDLType不能有为空的
    function CheckType() {
        var isOK = true;
        $(".POCount").find(".NewType").find(".DDLType>select").each(function () {
            var parenttd = $(this).parents("td");
            if ($(this).children("option:selected").text() == "") {
                SetBorderWarn(parenttd);
                isOK = false;
            }
            else {
                ClearBorderWarn(parenttd);
            }
        });
        return isOK
    }

    ////设置数量的格式
    function SetQtyFormate() {
        $(".POCount").find(".Qty").each(function () {
            var qty = $(this).text();
            if (regFloat.test(qty)) {
                $(this).text(parseFloat(qty).toFixed(0));
            }
        });
    }

    //得到日期的天数差
    function DateCompare(asStartDate, asEndDate) {
        var miStart = Date.parse(asStartDate.replace(/\-/g, '/ '));
        var miEnd = Date.parse(asEndDate.replace(/\-/g, '/ '));
        return (miEnd - miStart) / (1000 * 24 * 3600);
    }

    ///验证时间差
    function CheckDateSpan() {
        var isOK = true;
        var isDateSpanOK = true;
        var errorMsg = "";
        if("<%= IsAllocated%>"=="True")
        {
            return isOK;
        }

        $(".POCount").each(function () {
            var newPADObj = $(this).find(".NewPAD").find(".DateTimePAD").find("input");
            var currentPAD = $(this).find(".CurrentPAD");

            if ($.trim(newPADObj.val()).length == 0) {
                isOK = false;
                SetBorderWarn(newPADObj);
            }
            else {
                ClearBorderWarn(newPADObj);
            }

            //当前PAD-当前时间）>7
            var myDate = new Date();
            var nowDate = myDate.getYear() + "-" + parseInt(myDate.getMonth() + 1) + "-" + myDate.getDay();
            var timeSpan = DateCompare(nowDate, currentPAD.text());
            if (timeSpan < 7) {
                isDateSpanOK = false;
                SetBorderWarn(currentPAD.parents("td"));
            }
            else {
                ClearBorderWarn(currentPAD.parents("td"));
            }
        });

        if (!isOK) {
           errorMsg+="New PAD can not be empty!\n";
        }
        if (!isDateSpanOK) {
            errorMsg += "PO type change is only allowed 7 days before the PAD!";
        }
        if (errorMsg.length > 0) {
            alert(errorMsg);
        }

        return isOK && isDateSpanOK;
    }
    //验证Allocated
    function CheckIsAllocated() {
        var isOK = true;
        $(".POCount").find(".NewPAD").find(".IsAllocated").each(function () {
            if ($(this).text() == "Y") {
                SetBorderWarn($(this).parent("td"));
                isOK = false;
            }
            else {
                ClearBorderWarn($(this).parent("td"));
            }
        });
        if (!isOK) {
            alert("There are one or more po is allocated,can't submit or save!");
        }

        return isOK;
    }

    function SetBorderWarn($obj) {
        $obj.css('border', '2px solid red');
    }

    function ClearBorderWarn($obj) {
        $obj.css('border', '#999 1px solid');
        $obj.css('border-bottom', '#999 1px solid');
    }
</script>
            <table class="ca-workflow-form-table">
            <tr>
                <td class="label align-center w25">
                    <span id="Test">Remarks<br />
                    备注:</span>
                </td>
                <td class="value" id="comment-task">
                    <QFL:FormField ID="RemarksField" runat="server" FieldName="Remarks"></QFL:FormField>
                </td>
            </tr>
              <tr>
              <td class="label align-center" style="width:110px; height:26px;">Excel Uplad</td>
              <td class="label align-left">
                <asp:FileUpload ID="FileUploadExcel" runat="server" Width="300px"  />
                  <asp:Button ID="ButtonLoad" CssClass="form-button" Width="80px" runat="server"  Text="UpLoad" onclick="ButtonLoad_Click" />
                  <a href="/tmpfiles/POTypeChangeWorkflow/POTypeChange_Template.xlsx">Excel template download</a>
              </td>
              </tr>
              <tr>
              <td class="label align-center" style="width:110px; height:26px;">PO Number</td>
              <td class="label align-left">
                please input PO number split with ,<br />
                <asp:TextBox ID="TextBoxPONOs" TextMode="MultiLine" Height="60px" Width="300px" runat="server"></asp:TextBox>
                <asp:Button ID="ButtonBatchSearch" runat="server" Text="Search" CssClass="form-button" onclick="ButtonBatchSearch_Click"/>
              </td>
              </tr>
              <tr>
              <td class="label align-center" style="width:110px; height:26px;">Count</td>
              <td class="label align-left">
                <asp:Label ID="LabelCount" runat="server" Text="0"></asp:Label>
              </td>
              </tr>
            </table>
            <br/>
             
            <asp:Repeater ID="RepeaterPOData" runat="server">
                <ItemTemplate>
                       <table class="ca-workflow-form-table POCount">
                         <tr>
                            <td class="label align-center w20">PO No.</td>
                            <td class="label align-center w20" colspan="3" ><asp:Label ID="LabelTitle" runat="server" Text='<%# Eval("title")%>'></asp:Label></td>
                        </tr>
                         <tr class="NewType">
                            <td class="label align-center w20">New PO Type</td>
                            <td class="label align-center w20" colspan="3">
                                <asp:HiddenField ID="HiddenFieldNewTypeValue" Value='<%# Eval("NewTypeValue")%>' runat="server" />
                                <span class="DDLType">
                                    <asp:HiddenField ID="HiddenFieldISSuccess" Value='<%# Eval("IsSuccess")%>' runat="server" />
                                    <asp:DropDownList ID="DDLType" runat="server"></asp:DropDownList>
                                </span>
                                <asp:HiddenField ID="HiddenFieldIsPADSuccess" Value='<%# Eval("IsPADSuccess")%>' runat="server" />
                            </td>
                        </tr>
                         <tr class="NewPAD">
                            <td class="label align-center w20">New PAD</td>
                            <td class="label align-left w20">
                                <asp:HiddenField ID="HiddenFieldNewPAD" Value='<%# Eval("NewPAD")%>' runat="server" />
                                <span class="DateTimePAD"><cc1:CADateTimeControl ID="CADateTimePAD" runat="server" DateOnly="true" CssClassTextBox="HotelInfomation DateTimeControl" /></span>
                            </td>
                            <td class="label align-center w20">IsAllocated</td>
                            <td class="label align-center w20"><span class="IsAllocated"><asp:Label ID="LabelIsAllocated" CssClass="IsAllocated" runat="server" Text='<%# Eval("IsAllocated")%>'></asp:Label></span></td>
                        </tr>
                         <tr>
                            <td class="label align-center w20">PAD</td>
                            <td class="label align-center w20"><asp:Label ID="LabePAD" CssClass="CurrentPAD" runat="server" Text='<%# ConvertDateStr(Eval("PAD"))%>'></asp:Label></td>
                            <td class="label align-center w20">SAD</td>
                            <td class="label align-center w20"><asp:Label ID="LabelSAD" runat="server" Text='<%# ConvertDateStr(Eval("SAD"))%>'></asp:Label></td>
                        </tr>
                         <tr>
                            <td class="label align-center w20">OMU</td>
                            <td class="label align-center w20"><asp:Label ID="LabelOMU" runat="server" Text='<%# Eval("OMU")%>'></asp:Label></td>
                            <td class="label align-center w20">Qty</td>
                            <td class="label align-center w20"><asp:Label ID="LabelQty" CssClass="Qty" runat="server" Text='<%# Eval("Qty")%>'></asp:Label></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
              