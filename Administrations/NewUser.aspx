<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.master" AutoEventWireup="true"
    CodeFile="NewUser.aspx.cs" Inherits="Administrations_NewUser" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function check(chk) {

            chk.checked = false;
        }
        function checkall(chkad, chk1, chk2, chk3, chk4, chk5, chk6, chk7, chk8) {
            if (chkad.checked == true) {
                chk1.checked = true;
                chk2.checked = true;
                chk3.checked = true;
                chk4.checked = true;
                chk5.checked = true;
                chk6.checked = true;
                chk7.checked = true;
                chk8.checked = true;
            }
            else {
                chk1.checked = false;
                chk2.checked = false;
                chk3.checked = false;
                chk4.checked = false;
                chk5.checked = false;
                chk6.checked = false;
                chk7.checked = false;
                chk8.checked = false;
            }
        }
        function focus() {
            var txt = document.getElementById("txtUserName");
            txt.focus();
        }

        function checkblank(src, args) {
            //args.IsValid = true;
            if (document.getElementById("txtRePw").value == "" && document.getElementById("txtPw").value != "")
                args.IsValid = false;
            else
                args.IsValid = true;
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
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_admin.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Create New User</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" border="0" cellspacing="2" cellpadding="2">
        <tr>
            <td width="120" class="tbltxt">
                Full Name
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="largetb" ></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator4" runat="server" BackColor="Transparent" ControlToValidate="txtUserName"
                    ErrorMessage="*" CssClass="error">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Designation
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtDesignation" runat="server" CssClass="largetb"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator5" runat="server" BackColor="Transparent" ControlToValidate="txtUserName"
                    ErrorMessage="*" CssClass="error">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Contact No.
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtContactNo" runat="server" CssClass="largetb"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Is Cashier
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td>
                <asp:RadioButton ID="rBtnYes" runat="server" GroupName="Chk1" Text="Yes" CssClass="tbltxt" />
                &nbsp;&nbsp;
                <asp:RadioButton ID="rBtnNo" runat="server" GroupName="Chk1" Text="No" CssClass="tbltxt" />
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Work Station Id
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtWSId" runat="server" CssClass="largetb" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator6" runat="server" BackColor="Transparent" ControlToValidate="txtUserName"
                    ErrorMessage="*" CssClass="error">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                User Name
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtUserName" runat="server" CssClass="largetb"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator3" runat="server" BackColor="Transparent" ControlToValidate="txtUserName"
                    ErrorMessage="*" CssClass="error">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Password
            </td>
            <td class="tbltxt">
                :
            </td>
            <td class="tbltxt">
                <asp:TextBox ID="txtPW" runat="server" CssClass="largetb" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Confirm Password
            </td>
            <td class="tbltxt">
                :
            </td>
            <td class="gridtxt">
                <asp:TextBox ID="txtRePw" runat="server" TextMode="Password" CssClass="largetb"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtRePw"
                    ErrorMessage="Not Matched" Display="Dynamic" ControlToCompare="txtPW" Type="String"
                    CssClass="error"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Permission End Date
            </td>
            <td class="tbltxt">
                :
            </td>
            <td class="dt">
                <asp:TextBox ID="txtPED" runat="server" CssClass="vsmalltb"></asp:TextBox>
                <rjs:PopCalendar ID="dtpPED" runat="server" Control="txtPED" meta:resourcekey="PopCalendar2Resource1" />
                <asp:LinkButton ID="LinkButton1" CssClass="txtlink" runat="server" CausesValidation="false"
                    meta:resourcekey="lblClearResource1" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtPED.value='';return false;"
                    Text="Clear Date"></asp:LinkButton>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                
            </td>
        </tr>
        <tr>
            <td class="dt" colspan="2"></td>
            <td ><asp:CheckBox ID="chkAdminRight" CssClass="dt" Checked="false" runat="server" Text="Allow Admin Rights"  /></td>
        </tr>
        <tr>

            <td align="center" colspan="3">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnsubmit"  runat="server" Text="Submit" OnClick="btnsubmit_Click" CssClass="btn">
                </asp:Button>
                <asp:Button ID="btnpermission" runat="server" Text="Assign Permissions" CausesValidation="False"
                    Enabled="False" OnClick="btnpermission_Click" CssClass="btn"></asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div class="error">
                    Note : For default password leave the password and confirm password fields blank</div>
            </td>
        </tr>
    </table>
</asp:Content>
