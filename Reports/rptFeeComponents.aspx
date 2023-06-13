<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptFeeComponents.aspx.cs" Inherits="Reports_rptFeeComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
 <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
        Fee Components</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
  <table width="100%" border="0" cellspacing="2" cellpadding="2" class="cnt-box">
        <tr>
            <td align="right" class="tbltxt">
                <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                    Visible="False" TabIndex="1" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" 
                    TabIndex="2" />
            </td>
        </tr>
        <tr>
            <td class="cnt-box2 lbl2 tbltxt">
                <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

