<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptProfitLoss.aspx.cs" Inherits="Accounts_rptProfitLoss" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr style="background-color: #ededed;">
            <td width="350" align="left" valign="middle" style="background-color: ">
                <div class="headingcontainor">
                    <h2>
                        Profit Loss & Account</h2>
                </div>
            </td>
            <td height="35" align="left" valign="middle" colspan="2">
                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <div>
        Accounting Yr:&nbsp;<asp:DropDownList ID="drpSession" runat="server" 
            AutoPostBack="true" 
            onselectedindexchanged="drpSession_SelectedIndexChanged">
        </asp:DropDownList>&nbsp;&nbsp;
        <asp:Button ID="btnPrint" runat="server" Text="Print" 
            onclick="btnPrint_Click" />
    </div>
    <div class="spacer"></div>
    <div>
        <asp:Label ID="lblReport" runat="server" Text=""></asp:Label></div>
</asp:Content>
