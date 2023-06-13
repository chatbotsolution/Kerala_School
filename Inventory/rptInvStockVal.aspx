<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="rptInvStockVal.aspx.cs" Inherits="Inventory_rptInvStockVal" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Stock Value</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" cellspacing="2" cellpadding="2">
        <tr>
            <td width="60" class="tbltxt">
                Location
            </td>
            <td class="tbltxt" width="5">
                :
            </td>
            <td class="tbltxt" width="140">
                <asp:DropDownList ID="drpLoc" runat="server" CssClass="vsmalltb">
                </asp:DropDownList>
            </td>
            <td colspan="4" class="tbltxt">
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" TabIndex="6" />&nbsp;
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="7" />&nbsp;
                <asp:Button ID="btnExpExcel" runat="server" Text="Export to Excel" Visible="False"
                    Width="106px" TabIndex="8" OnClick="btnExpExcel_Click" />
            </td>
        </tr>
    </table>
    <div style="padding-top: 10px;">
        <asp:Label ID="lblReport" runat="server"></asp:Label></div>
</asp:Content>

