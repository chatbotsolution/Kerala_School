<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="rptFeeReceivedComponent.aspx.cs" Inherits="Hostel_rptFeeReceivedComponent" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Componentwise Collection Report</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" border="0" cellspacing="2" cellpadding="2">
        <tr>
            <td width="100" class="tbltxt">
                Mode Of Payment
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="100">
                <asp:DropDownList ID="drpModeOfPayment" runat="server" CssClass="vsmalltb" OnSelectedIndexChanged="drpModeOfPayment_SelectedIndexChanged"
                    TabIndex="1">
                    <asp:ListItem>All</asp:ListItem>
                    <asp:ListItem>Cash</asp:ListItem>
                    <asp:ListItem>Cheque</asp:ListItem>
                    <asp:ListItem>Bank</asp:ListItem>
                </asp:DropDownList>
            </td>
           <%-- <td class="tbltxt">
                Session Yr :
            </td>
            <td>
                <asp:DropDownList ID="drpSessionYr" runat="server"
                    onselectedindexchanged="drpSessionYr_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </td>--%>
            <td width="65" class="tbltxt">
                From Date
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="100">
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="vsmalltb" Width="70px" ReadOnly="True"
                    TabIndex="2"></asp:TextBox>
                <rjs:PopCalendar ID="pcalFromDate" runat="server" Control="txtFromDate" />
            </td>
            <td width="50" class="tbltxt">
                To Date
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="100">
                <asp:TextBox ID="txtToDate" runat="server" CssClass="vsmalltb" Width="70px" ReadOnly="True"
                    TabIndex="3"></asp:TextBox>
                <rjs:PopCalendar ID="pcalToDate" runat="server" Control="txtToDate" />
            </td>
            <td>
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" TabIndex="4" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="5" />
                <asp:Button ID="btnExpExcel" runat="server" Text="Export to Excel" Visible="False"
                    TabIndex="6" />
                <asp:Button ID="btnOutstandingFee" runat="server" Text="Show Outstanding Amount"
                    TabIndex="6" OnClick="btnOutstandingFee_Click" />
            </td>
        </tr>
    </table>
    <div style="padding-top: 10px;">
        <asp:Label ID="lblReport" runat="server"></asp:Label></div>
    <div style="padding-top: 5px;">
        <asp:Label ID="lblAdv" runat="server"></asp:Label></div>
    <div style="padding-top: 5px;">
        <asp:Label ID="lblPrev" runat="server"></asp:Label></div>
    <div style="padding-top: 5px;">
        <asp:Label ID="lblPrevDue" runat="server"></asp:Label></div>
    <div style="padding-top: 5px;">
        <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
    <div style="padding-top: 5px;">
        <asp:Label ID="lblGTotal" runat="server"></asp:Label>
    </div>
</asp:Content>


