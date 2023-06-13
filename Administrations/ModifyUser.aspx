<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.master" AutoEventWireup="true"
    CodeFile="ModifyUser.aspx.cs" Inherits="Administrations_ModifyUser" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_admin.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Modify User</h2>
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
                <asp:DropDownList ID="drpUName" runat="server" Width="130px" AutoPostBack="True"
                    OnSelectedIndexChanged="drpUName_SelectedIndexChanged" CssClass="largetb1">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Permission Start Date
            </td>
            <td class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtPSD" runat="server" ReadOnly="True" CssClass="vsmalltb"></asp:TextBox>
                <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtPSD" meta:resourcekey="PopCalendar2Resource1"
                    To-Date="2110-12-31" Visible="False" />
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
                <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtPED" meta:resourcekey="PopCalendar2Resource1"
                    To-Date="2110-12-31"></rjs:PopCalendar>
                <asp:LinkButton ID="lblClear" runat="server" meta:resourcekey="lblClearResource1"
                    CssClass="txtlink" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtPED.value='';return false;"
                    Text="Clear Date"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Select User Privileges
            </td>
            <td class="tbltxt">
                :
            </td>
            <td>
                <asp:CheckBox ID="chkAdmin" runat="server" Width="128px" Text="Admin Rights" BorderColor="Transparent"
                    CssClass="tbltxt" AutoPostBack="True" OnCheckedChanged="chkAdmin_CheckedChanged">
                </asp:CheckBox>
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
                <div style="padding-top: 5px; padding-bottom: 5px;">
                    <asp:Label ID="lblerr" runat="server" CssClass="error"></asp:Label></div>
                <asp:Button ID="btnModify" runat="server" Text="Submit" Width="88px" OnClick="btnsubmit_Click" CssClass="btn">
                </asp:Button>&nbsp;<asp:Button ID="btnResetPW" runat="server" Text="Reset Password"
                    Width="120px" CausesValidation="False" OnClick="btnreset_Click" CssClass="btn"></asp:Button>&nbsp;<asp:Button
                        ID="btnModPerm" runat="server" Text="Set Permission" Width="112px" CausesValidation="False"
                        OnClick="btnPerm_Click" CssClass="btn"></asp:Button>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        function calendarPicker(strField) {
            window.open('Datepicker.aspx?field=' + strField, 'CalenderPopUp', 'width=235,height=220, screenX=100,screenY=200,left=200,top=375,titlebar=no,toolbar=no,resizable=no');
        }
    </script>
</asp:Content>
