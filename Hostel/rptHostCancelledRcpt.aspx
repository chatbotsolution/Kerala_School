<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="rptHostCancelledRcpt.aspx.cs" Inherits="Hostel_rptHostCancelledRcpt" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Cancelled Receipts
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" cellspacing="4" cellpadding="0">
        <tr>
            <td style="border: solid 1px black; padding-left: 20px;" class="tbltxt">
                &nbsp; From Date :
                <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" CssClass="tbltxtbox" Width="80px"
                    TabIndex="4"></asp:TextBox>
                <rjs:PopCalendar ID="dtpFrmDt" runat="server" Control="txtFromDt" AutoPostBack="False"
                    Format="dd mmm yyyy"></rjs:PopCalendar>
                &nbsp; To Date :
                <asp:TextBox ID="txtToDt" runat="server" CssClass="tbltxtbox" ReadOnly="true" Width="80px"
                    TabIndex="5"></asp:TextBox>
                <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                    Format="dd mmm yyyy"></rjs:PopCalendar>
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return Isvalid();"
                    TabIndex="6" />&nbsp;
                    <asp:Button ID="btnPrint" runat="server" Text="Print" Enabled="false"
                    TabIndex="7" onclick="btnPrint_Click" />&nbsp;
                    <asp:Button ID="btnExp" runat="server" Text="Export To Excel" Enabled="false"
                    TabIndex="7" onclick="btnExp_Click" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>



