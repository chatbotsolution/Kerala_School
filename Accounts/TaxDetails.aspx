<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="TaxDetails.aspx.cs" Inherits="Accounts_TaxDetails" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">


        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function beginRequest(sender, args) {
            // show the popup
            $find('<%=mdlloading.ClientID %>').show();

        }

        function endRequest(sender, args) {
            //  hide the popup
            $find('<%=mdlloading.ClientID %>').hide();

        }
        function isValid() {

            var Code = document.getElementById("<%=txtTaxCode.ClientID %>").value;
            var Desc = document.getElementById("<%=txtTaxDesc.ClientID %>").value;
            var Rate = document.getElementById("<%=txtTaxRate.ClientID %>").value;
            var Date = document.getElementById("<%=txtDt.ClientID %>").value;


            if (Code.trim() == "") {
                alert("Enter Tax Code");
                document.getElementById("<%=txtTaxCode.ClientID %>").focus();
                return false;
            }
            if (Desc.trim() == "") {
                alert("Enter Tax Description");
                document.getElementById("<%=txtTaxDesc.ClientID %>").focus();
                return false;
            }
            if (Rate.trim() == "") {
                alert("Enter Tax Rate");
                document.getElementById("<%=txtTaxRate.ClientID %>").focus();
                return false;
            }
            if (Date.trim() == "") {
                alert("Enter Effective Date");
                document.getElementById("<%=txtDt.ClientID %>").focus();
                return false;
            }


            else {
                return true;
            }
        }

        function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
            var key;
            var isCtrl = false;
            var keychar;
            var reg;

            if (window.event) {
                key = e.keyCode;
                isCtrl = window.event.ctrlKey
            }
            else if (e.which) {
                key = e.which;
                isCtrl = e.ctrlKey;
            }

            if (isNaN(key)) return true;

            keychar = String.fromCharCode(key);

            // check for backspace or delete, or if Ctrl was pressed
            if (key == 8 || isCtrl) {
                return true;
            }

            reg = /\d/;
            var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
            var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

            return isFirstN || isFirstD || reg.test(keychar);
        }

    </script>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr style="background-color: #ededed;">
            <td width="250" align="left" valign="middle" height="35">
                <div class="headingcontainor">
                    <h1>
                        Tax
                    </h1>
                    <h2>
                        Type</h2>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-top: 10px;">
                <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <table width="100%" border="0" cellspacing="1" cellpadding="2">
                            <tr>
                                <td colspan="6" id="tdMsg" runat="server" align="center">
                                    <asp:Label ID="lblShow" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="120" class="innertbltxt">
                                    Tax Code<span class="mandatory">*</span>
                                </td>
                                <td width="5" class="innertbltxt">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTaxCode" runat="server" MaxLength="30" TabIndex="1"></asp:TextBox>
                                    <%--<asp:CheckBox ID="chkIsVAT" runat="server" Text="Is VAT" TabIndex="2" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px; width: 110px;">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="innertbltxt">
                                    Tax Description<span class="mandatory">*</span>
                                </td>
                                <td width="5" class="innertbltxt">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTaxDesc" runat="server" MaxLength="50" TabIndex="3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px; width: 110px;">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="innertbltxt">
                                    Tax Rate<span class="mandatory">*</span>
                                </td>
                                <td width="5" class="innertbltxt">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTaxRate" runat="server" MaxLength="20" TabIndex="4" onkeypress="return blockNonNumbers(this, event, true, true);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px; width: 110px;">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="innertbltxt">
                                    Effective Date<span class="mandatory">*</span>
                                </td>
                                <td class="innertbltxt">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDt" runat="server" ReadOnly="True" TabIndex="5"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpdt" runat="server" Control="txtDt" AutoPostBack="False" Format="dd mmm yyyy">
                                    </rjs:PopCalendar>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                        OnClientClick="return isValid();" TabIndex="6" onfocus="active(this);" onblur="inactive(this);" />
                                    <asp:Button ID="btnView" runat="server" Text="View List" OnClick="btnView_Click"
                                        TabIndex="7" onfocus="active(this);" onblur="inactive(this);" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" TabIndex="7"
                                        onfocus="active(this);" onblur="inactive(this);" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                        TabIndex="8" onfocus="active(this);" onblur="inactive(this);" />
                                </td>
                            </tr>
                        </table>
                        <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                            PopupControlID="pnlloading" BackgroundCssClass="Background" />
                        <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                            <div align="center" style="margin-top: 13px;">
                                <img src="../Images/loading.gif" />
                                <span>Loading ...</span>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>


