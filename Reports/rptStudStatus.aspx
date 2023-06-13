<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptStudStatus.aspx.cs" Inherits="Reports_rptStudStatus" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Student Promotion Status
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" >
                <tr>
                    <td class="tbltxt cnt-box">
                        Session :
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                            TabIndex="1">
                        </asp:DropDownList>
                        &nbsp; Class :
                        <asp:DropDownList ID="drpClass" runat="server" CssClass="vsmalltb" TabIndex="2">
                        </asp:DropDownList>
                        &nbsp; Status :
                        <asp:DropDownList ID="drpStatus" runat="server" CssClass="vsmalltb" TabIndex="3">
                            <asp:ListItem Value="P">Pending</asp:ListItem>
                            <asp:ListItem Value="PR">Promoted</asp:ListItem>
                            <asp:ListItem Value="D">Detained</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnShow" CausesValidation="false" runat="server"
                            Text="Show" ToolTip="Click to show student list." TabIndex="4" 
                            onclick="btnShow_Click"></asp:Button>
                        <asp:Button ID="btnPrint" runat="server" Text="Print" TabIndex="5" 
                            onclick="btnPrint_Click" />
                        <asp:Button ID="btnExpExcel" runat="server" Text="Export to Excel"
                            TabIndex="6" onclick="btnExpExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt" align="center">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="error"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt">
                        &nbsp;<asp:Label ID="lblReport" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
</asp:Content>
