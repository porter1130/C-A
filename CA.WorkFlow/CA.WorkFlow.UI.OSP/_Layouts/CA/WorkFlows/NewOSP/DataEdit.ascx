<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.NewOSP.DataEdit" %>

<%@ Register Assembly="QuickFlow.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ec1e0fe6e1745628" Namespace="QuickFlow.UI.ListForm" TagPrefix="QFL" %>

<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="cc1" %>
<script type="text/javascript">

    var regFloat = /^[0-9]+(\.?)(\d*)$/;
    $(document).ready(function () {
        OSPChange();
        InteNewOMU();
    });

    ///页面提交前进行各项验证。
    function check() {
        var isCountOK = CheackCount();
        if (!isCountOK) {
            return false;
        }

        var isPriceOK = CheckPrice();
        if (!isPriceOK) {
            alert("New OSP Error!");
            return false;
        }

        var checkAllocatedMSG = CheckAllocatedDate();
        if (checkAllocatedMSG.length>0) {
            checkAllocatedMSG += "\nAre you sure want to continue?";
            if (confirm(checkAllocatedMSG)) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return true;
        }
    }

    ///验证PO数不为空
    function CheackCount() {
        var isOK = true;
        var poCount = 0;
        poCount = $(".POCount").length; ;
        if (poCount > 0) {
            return true;
        }
        else {
            alert("Empty data");
            isOK = false;
        }
        return isOK;
    }
    ///验证价格格式。
    function CheckPrice() {
        var isCheck = true;
        $(".NewOSP").each(function () {
            var newOsp = $(this).val();
            if (!regFloat.test(newOsp)) {
                SetBorderWarn($(this));
                isCheck = false;
            }
            else {
                ClearBorderWarn($(this));
            }
        });
        return isCheck;
    }

    //设置不合法的价格的边框
    function SetBorderWarn($obj) {
        $obj.css('border', '2px solid red');
    }

    ///取消验证价格时产生的边框 。
    function ClearBorderWarn($obj) {
        $obj.css('border', '');
        $obj.css('border-bottom', '#999 1px solid');
    }

    //验证style No. 的AllocatedDate
    function CheckAllocatedDate() {
        var errorMessage = "";
        $(".POCount").each(function () {
            var allocatedDate = $(this).attr("allocatedDate");
            if (allocatedDate!="") {
                var styleNO = $(this).find("tr:first").find("span").text();
                errorMessage += styleNO;
                errorMessage += " has been allocated already, you are not allowed to change ASP.";
            }
        });
        return errorMessage;
    }

    //OSP的值改变时
    function OSPChange() {
        $(".POCount").find("input[class='NewOSP']").change(function () {
            NewOMUValue($(this));
        });
    }
    //统计页面上的NewOMU值
    function InteNewOMU() {
        $(".POCount").find("input[class='NewOSP']").each(function () {
        if($.trim($(this).val())!="")
            NewOMUValue($(this));
        });
    }
    //计算OMUNewOMUValue
    function NewOMUValue(obj) {
        var osp = obj.val();
        if (regFloat.test(osp)) {
            
            var NewOMUID = obj.prev("span").attr("class");
            var NewOMUI = obj.parentsUntil(".POCount").find("#" + NewOMUID);

            var cost = NewOMUI.prev("input").val();
            if (regFloat.test(cost) && parseFloat(osp) != 0) {
                var newVal = ((parseFloat(osp) - parseFloat(cost)) / parseFloat(osp))*100;
                NewOMUI.children("span").text(newVal.toFixed(2)+"%");
                NewOMUI.next("span").hide();
            }

            //OriginalOsp
            var index = obj.prev("span").attr("index"); 
            var OMUReduction = obj.parentsUntil(".POCount").find("#OMUReduction" + index);
            var OriginalOsp = OMUReduction.prev("input").val();
            var Qty = OMUReduction.attr("Qty");
            if (regFloat.test(OriginalOsp) && parseFloat(osp) != 0 && parseFloat(Qty)) {

                var newVal = ((parseFloat(OriginalOsp) - parseFloat(osp)) * parseFloat(Qty));
                OMUReduction.children("span").text(newVal.toFixed(0));
                OMUReduction.next("span").hide();
                //
                //Qty
            }
        }
    }
</script>
            <table class="ca-workflow-form-table">
              <tr>
              <td class="label align-center" style="width:110px; height:26px;">Excel Uplad</td>
              <td class="label align-left">
                <asp:FileUpload ID="FileUploadExcel" runat="server" Width="300px"  />
                  <asp:Button ID="ButtonLoad" CssClass="form-button" Width="80px" runat="server"  Text="UpLoad" onclick="ButtonLoad_Click" />
                  <a href="/tmpfiles/OSP/OSP_Template.xlsx">Excel template download</a>
              </td>
              </tr>
              <tr>
              <td class="label align-center" style="width:110px; height:26px;">Style Number</td>
              <td class="label align-left">
                please input style number split with ,<br />
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

            <asp:Repeater ID="RepeaterPOData" runat="server">
                <ItemTemplate>
                      <table class="ca-workflow-form-table POCount" allocatedDate='<%# Eval("AllocatedDate")%>'>
                         <tr>
                            <td class="label align-center" colspan="1">Style No.</td>
                            <td class="label align-left" colspan="5">
                                <asp:Label ID="LabelStyleNO" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                <asp:HiddenField ID="HiddenFieldISSuccess" Value='<%# Eval("IsSuccess")%>' runat="server" />
                            </td>
                         </tr>
                        <tr>
                            <td class="label align-center" colspan="1">Original Osp</td>
                            <td class="label align-left" colspan="5">
                                <asp:Label ID="LabelOriginalOsp" runat="server" Text='<%# Eval("OriginalOsp")%>'></asp:Label>
                        </td>
                       </tr>
                        <tr>
                            <td class="label align-center" colspan="1">New OSP</td>
                            <td class="label align-left" colspan="5"><span index='<%#+ Container.ItemIndex + 1%>' class='Cost<%#+ Container.ItemIndex + 1%>'></span><asp:TextBox ID="TextBoxNewOSP" CssClass="NewOSP" runat="server" Text='<%# Eval("NewOSP")%>'></asp:TextBox></td>
                         </tr>
                        <tr>
                            <td class="label align-center" colspan="1">PO</td>
                            <td class="label align-left" colspan="5"><asp:Label ID="LabelPONO" runat="server" Text='<%# Eval("PONO")%>'></asp:Label></td>
                         </tr>
                         <tr>
                            <td class="label align-center w20">Sub Div </td>
                            <td class="label align-center w20" ><asp:Label ID="LabelSubDiv" runat="server" Text='<%# Eval("SubDiv")%>'></asp:Label></td>
                            <td class="label align-center w22" style="width:120px;">Class</td>
                            <td class="label align-center w20" id="Td4"><asp:Label ID="LabelClass" runat="server" Text='<%# Eval("Class")%>'></asp:Label></td>
                            <td class="label align-center w10" style="width:80px;">Qty</td>
                            <td class="label align-center w20"><asp:Label ID="LabelQty" runat="server" Text='<%# IntFormate(Eval("Qty").ToString())%>'></asp:Label></td>
                        </tr>
                         <tr>
                            <td class="label align-center">Current OMU</td>
                            <td class="label align-center" ><asp:Label ID="LabelCurrentOMU" runat="server" Text='<%# Eval("CurrentOMU")%>'></asp:Label></td>
                            <td class="label align-center" >Created by</td>
                            <td class="label align-center" id="Td5"><asp:Label ID="LabelCreatedBy" runat="server" Text='<%# Eval("CreatedBy")%>'></asp:Label></td>
                            <td class="label align-center" style="width:80px;">PAD</td>
                            <td class="label align-center"><asp:Label ID="LabelPAD" runat="server" Text='<%# Eval("PAD")%>'></asp:Label></td>
                       </tr>
                         <tr>
                           <td class="label align-center">SAD</td>
                           <td class="label align-center"><asp:Label ID="LabelSAD" runat="server" Text='<%# Eval("SAD")%>'></asp:Label></td>
                           <td class="label align-center">GR </td>
                           <td class="label align-center" id="Td1"><asp:Label ID="LabelGR" runat="server" Text='<%# Eval("GR")%>'></asp:Label></td>
                           <td class="label align-center">Allocated date</td>
                           <td class="label align-center"><asp:Label ID="LabelAllocatedDate" runat="server" Text='<%# Eval("AllocatedDate")%>'></asp:Label></td>
                        </tr>
                         <tr>
                           <td class="label align-center">New OMU</td>
                           <td class="label align-center">
                               <asp:HiddenField ID="HiddenFieldCost" Value='<%# Eval("Cost")%>' runat="server" />
                               <span id='Cost<%#+ Container.ItemIndex + 1%>'>
                                    <asp:Label ID="LabelNewOMU" runat="server"></asp:Label>
                               </span>
                               <asp:Label ID="LabelOMUValue" visible="<%# bIsEdt%>"  runat="server" Text='<%# Eval("NewOMU")%>'></asp:Label>
                           </td>
                           <td class="label align-center">OMU Reduction</td>
                           <td class="label align-center">
                              <asp:HiddenField ID="HiddenFieldOMUReduction" Value='<%# Eval("OriginalOsp")%>' runat="server" />
                               <span id='OMUReduction<%#+ Container.ItemIndex + 1%>' Qty='<%# Eval("Qty")%>'>
                                    <asp:Label ID="LabelOMUReduction" runat="server"></asp:Label>
                               </span>
                               <asp:Label ID="LabelOMUReductionVal" visible="<%# bIsEdt%>"  runat="server" Text='<%# Eval("OMUReduction")%>'></asp:Label>
                           </td>
                           <td class="label align-center"></td>
                           <td class="label align-center"></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>