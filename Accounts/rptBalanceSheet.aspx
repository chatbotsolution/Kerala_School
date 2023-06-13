<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptBalanceSheet.aspx.cs" Inherits="Accounts_rptBalanceSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr style="background-color: #ededed;">
            <td width="350" align="left" valign="middle" style="background-color: ">
                <div class="headingcontainor">
                    <h2>
                        Balance Sheet Statement</h2>
                </div>
            </td>
            <td height="35" align="left" valign="middle" colspan="2">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text="Msg"></asp:Label>
            </td>
        </tr>
    </table>
    <div>
        Accounting Yr:&nbsp;<asp:DropDownList ID="drpSession" runat="server" AutoPostBack="true"
            OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
        </asp:DropDownList>
        &nbsp;&nbsp;
        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
    </div>
    <div class="spacer">
    </div>
    <div>
        <asp:Label ID="lblReport" runat="server" Text=""></asp:Label></div>
</asp:Content>
