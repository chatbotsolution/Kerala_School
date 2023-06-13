<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="rptHostFeeToBeReceivedComponent.aspx.cs" Inherits="Hostel_rptHostFeeToBeReceivedComponent" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Componentwise Outstanding Amount</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" border="0" cellspacing="2" cellpadding="2">
        <tr>
            <td width="65" class="tbltxt">
                Till Date
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="120">
                <asp:TextBox ID="txtDate" runat="server" CssClass="vsmalltb" ReadOnly="True" TabIndex="2"></asp:TextBox>
                <rjs:PopCalendar ID="dtpDate" runat="server" Control="txtDate" />
            </td>
            <td>
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" TabIndex="4" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="5" />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" TabIndex="5" />
            </td>
        </tr>
    </table>
    <div style="padding-top: 10px;">
        <asp:Label ID="lblReport" runat="server"></asp:Label></div>
    <div style="padding-top: 5px;">
        <asp:Label ID="lblPrevDue" runat="server"></asp:Label></div>
    <div style="padding-top: 5px;">
        <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
</asp:Content>

