<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="PartyMaster.aspx.cs" Inherits="Accounts_PartyMaster" %>

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
            var Party = document.getElementById("<%=txtPartyNm.ClientID %>").value;
            var ContactPers = document.getElementById("<%=txtContactPrsn.ClientID %>").value;
            var ConGroup = document.getElementById("<%=drpAccGroups.ClientID %>").value;
            var Type = document.getElementById("<%=drpPartyType.ClientID %>").value;
            var CusType = document.getElementById("<%=drpCustomerType.ClientID %>").value;
            var EmailId = document.getElementById("<%=txtEmail.ClientID %>").value;

            if (Party.trim() == "") {
                alert("Please Enter Party Name !");
                document.getElementById("<%=txtPartyNm.ClientID %>").focus();
                return false;
            }
            if (ContactPers.trim() == "") {
                alert("Please Enter Contact Person Name !");
                document.getElementById("<%=txtContactPrsn.ClientID %>").focus();
                return false;
            }
            if (ConGroup == "0") {
                alert("Please Select Account Groups");
                document.getElementById("<%=drpAccGroups.ClientID %>").focus();
                return false;
            }
            if (Type == "0") {
                alert("Please Select Party Type");
                document.getElementById("<%=drpPartyType.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=drpCustomerType.ClientID %>").disabled == false && CusType == "0") {
                alert("Please Select Customer Type");
                document.getElementById("<%=drpCustomerType.ClientID %>").focus();
                return false;
            }
            if (EmailId.trim() != "") {
                var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
                if (reg.test(EmailId) == false) {
                    alert('Invalid Email Address');
                    return false;
                }
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

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnback" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="250" align="left" valign="middle" height="35">
                        <div>
                            <h1>
                                Party
                            </h1>
                            <h2>
                                Details</h2>
                        </div>
                    </td>
                    <td align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-top: 10px;">
                        <div>
                            <h3>
                                Party Information</h3>
                        </div>
                        <table width="100%" border="0" cellspacing="1" cellpadding="2">
                            <tr>
                                <td colspan="6" id="tdMsg" runat="server" align="center">
                                </td>
                            </tr>
                            <tr>
                                <td width="140">
                                    Party Name<span class="mandatory">*</span>
                                </td>
                                <td width="5">
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtPartyNm" runat="server" Width="90%" MaxLength="100" TabIndex="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="140">
                                    Contact Person<span class="mandatory">*</span>
                                </td>
                                <td width="5">
                                    :
                                </td>
                                <td width="280">
                                    <asp:TextBox ID="txtContactPrsn" runat="server" Width="90%" MaxLength="50" TabIndex="2"></asp:TextBox>
                                </td>
                                <td width="140">
                                    PAN Number
                                </td>
                                <td width="5">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPanNo" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Account Groups<span class="mandatory">*</span>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpAccGroups" runat="server" TabIndex="3" AutoPostBack="true"
                                        onselectedindexchanged="drpAccGroups_SelectedIndexChanged">
                                        <%--<asp:ListItem Value="0">--SELECT--</asp:ListItem>
                                        <asp:ListItem Value="11">Sundry Creditors</asp:ListItem>
                                        <asp:ListItem Value="12">Sundry Debtors</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    TIN Number
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTinNo" runat="server" MaxLength="20" TabIndex="8"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Party Type<span class="mandatory">*</span>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpPartyType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPartyType_SelectedIndexChanged"
                                        TabIndex="4">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    CST Number
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCstNo" runat="server" MaxLength="20" TabIndex="9"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Customer Type
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpCustomerType" runat="server" TabIndex="5">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Business Start Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStartDt" runat="server" Width="100px" TabIndex="10"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpStartDate" runat="server" Control="txtStartDt" AutoPostBack="False"
                                        Format="dd mmm yyyy"></rjs:PopCalendar>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Credit Limit
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCrLimit" runat="server" onkeypress="return blockNonNumbers(this, event, true, true);"
                                        MaxLength="10" TabIndex="6"></asp:TextBox>
                                </td>
                                <td>
                                    Status
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbtnActive" runat="server" Text="Active" AutoPostBack="True"
                                        GroupName="s" Checked="True" TabIndex="11"></asp:RadioButton>
                                    <asp:RadioButton ID="rbtnInactive" runat="server" Text="Inactive" GroupName="s" TabIndex="12">
                                    </asp:RadioButton>
                                </td>
                            </tr>
                        </table>
                        <div style="padding-top: 20px;">
                            <h3>
                                Address Details</h3>
                        </div>
                        <table width="100%" border="0" cellspacing="1" cellpadding="2">
                            <tr>
                                <td width="140">
                                    Address
                                </td>
                                <td width="5">
                                    :
                                </td>
                                <td width="280">
                                    <asp:TextBox ID="txtAddress" runat="server" Width="90%" MaxLength="200" TabIndex="13"></asp:TextBox>
                                </td>
                                <td width="140">
                                    City
                                </td>
                                <td width="5px">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCity" runat="server" Width="75%" TabIndex="17"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    State
                                </td>
                                <td width="5">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtState" runat="server" Width="75%" TabIndex="14"></asp:TextBox>
                                </td>
                                <td>
                                    PinCode
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPinNo" runat="server" Width="75%" TabIndex="18" onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Phone
                                </td>
                                <td width="5">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPhone" runat="server" Width="75%" TabIndex="15"></asp:TextBox>
                                </td>
                                <td>
                                    Mobile
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMobile" runat="server" Width="75%" TabIndex="19"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    FAX
                                </td>
                                <td width="5">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFax" runat="server" Width="75%" TabIndex="16"></asp:TextBox>
                                </td>
                                <td>
                                    Email
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" Width="75%" TabIndex="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px;">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="left" style="padding-top: 10px;">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Save & Add New" OnClick="btnSubmit_Click"
                                        OnClientClick="return isValid();" TabIndex="21" Width="150px" onfocus="active(this);" onblur="inactive(this);"/>
                                    <asp:Button ID="btnShow" runat="server" Text="Go To List" OnClick="btnShow_Click"
                                        TabIndex="22" Width="150px" onfocus="active(this);" onblur="inactive(this);" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Clear" OnClick="btnCancel_Click1"
                                        TabIndex="23" Width="150px" onfocus="active(this);" onblur="inactive(this);"/>
                                    <asp:Button ID="btnback" runat="server" Text="Cancel" OnClick="btnback_Click" TabIndex="24"
                                        Width="150px" onfocus="active(this);" onblur="inactive(this);" />
                                </td>
                            </tr>
                        </table>
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
</asp:Content>


