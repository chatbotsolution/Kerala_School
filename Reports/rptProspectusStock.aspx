<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptProspectusStock.aspx.cs" Inherits="Reports_rptProspectusStock" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Prospectus Stock Report
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" class="cnt-box">
        <tr>
            <td class="tbltxt">
                Session Year :
                <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" 
                    TabIndex="1">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;From Date :
                <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" CssClass="tbltxtbox" 
                    TabIndex="2"></asp:TextBox>
                <rjs:PopCalendar ID="dtpPros" runat="server" Control="txtFromDt" AutoPostBack="False"
                    Format="dd mmm yyyy"></rjs:PopCalendar>
                &nbsp; To Date :
                <asp:TextBox ID="txtToDt" runat="server" CssClass="tbltxtbox" ReadOnly="true" 
                    TabIndex="3"></asp:TextBox>
                <rjs:PopCalendar ID="dtpPros1" runat="server" Control="txtToDt" AutoPostBack="False"
                    Format="dd mmm yyyy"></rjs:PopCalendar><br><br />
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click"
                    OnClientClick="return Isvalid();" TabIndex="4" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="5" />&nbsp;&nbsp;<asp:Button
                    ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click"
                    TabIndex="6" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="center" class="tbltxt">
                <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

