<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="Holidaydetail.aspx.cs" Inherits="Masters_Holidaydetail" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_admin.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>Holidays Entry</h2>
    </div>
  
  <div class="spacer"><img src="../images/mask.gif" height="8" width="10"  /></div>
  <table width="100%" border="0" cellspacing="2" cellpadding="2">
    <tr>
      <td width="100" class="tbltxt">Description</td>
      <td width="5" class="tbltxt">:</td>
      <td><asp:TextBox ID="txtholidaydesc" runat="server"
                                CssClass="largetb" TabIndex="1"></asp:TextBox>
      <asp:RequiredFieldValidator ID="reqddesc" runat="server" ControlToValidate="txtholidaydesc"
                                    ErrorMessage="*" CssClass="error"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
      <td class="tbltxt">Date</td>
      <td class="tbltxt">:</td>
      <td><asp:TextBox ID="txtdate" runat="server" CssClass="vsmalltb" TabIndex="2"></asp:TextBox>
                    <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtdate" 
                        meta:resourcekey="PopCalendar2Resource1" /></rjs:PopCalendar><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtdate" ErrorMessage="*" CssClass="error"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
      <td class="tbltxt">&nbsp;</td>
      <td class="tbltxt">&nbsp;</td>
      <td><asp:Button ID="btnsave" runat="server" OnClick="btnsave_Click" CausesValidation="True"
                                Text="Submit" TabIndex="3" /><asp:Button ID="btncancel" 
              runat="server" CausesValidation="False"
                                    Text="Cancel" OnClick="btncancel_Click" 
              TabIndex="4" /></td>
    </tr>
  </table>
  
</asp:Content>
