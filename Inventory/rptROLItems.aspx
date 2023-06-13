<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="rptROLItems.aspx.cs" Inherits="Inventory_rptROLItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Item List on ROL</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
    </div>
    <div align="right" style="width:100%;">
        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="6" />&nbsp;
        <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click" TabIndex="7" /> 
    </div>
    <div style="width:100%;">
        <asp:Label ID="lblReport" runat="server" Text="" CssClass="tbltxt"></asp:Label></div>
    <%--<table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td align="right">
                                   
            </td>
        </tr>
        <tr>
            <td>
                
            </td>
        </tr>
    </table>--%>
</asp:Content>

