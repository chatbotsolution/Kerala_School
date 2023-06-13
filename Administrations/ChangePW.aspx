<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.master" AutoEventWireup="true"
    CodeFile="ChangePW.aspx.cs" Inherits="Administrations_ChangePW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_admin.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Change Password</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" border="0" cellspacing="2" cellpadding="2">
        <tr>
            <td width="120" class="tbltxt">
                User Name
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtUname" runat="server" CssClass="largetb"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUname" ErrorMessage="*"
                    CssClass="error"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Old Password
            </td>
            <td class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtOldPass" runat="server" TextMode="Password" CssClass="largetb"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtOldPass" ErrorMessage="*"
                    CssClass="error"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                New Password
            </td>
            <td class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password" CssClass="largetb"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNewPass" ErrorMessage="*"
                    CssClass="error"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Confirm Password
            </td>
            <td class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtConPass" runat="server" TextMode="Password" CssClass="largetb"></asp:TextBox><asp:CompareValidator
                    ID="CompareValidator1" runat="server" ControlToValidate="txtConPass" ErrorMessage="Not Matched"
                    CssClass="error" Display="Dynamic" ControlToCompare="txtNewPass" Type="String"
                    SetFocusOnError="True"></asp:CompareValidator><asp:RequiredFieldValidator ID="RequiredFieldValidator4"
                        runat="server" ControlToValidate="txtConPass" CssClass="error" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                &nbsp;
            </td>
            <td class="tbltxt">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn">
                </asp:Button>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" CssClass="btn"
                    OnClick="btnCancel_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
