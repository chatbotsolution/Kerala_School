<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptAdvanceLedger.aspx.cs" Inherits="Reports_rptAdvanceLedger" %>

<asp:Content ID="contentRemarkReport" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Advance Fee Ledger</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" cellspacing="2" cellpadding="2">
        <tr>
            <td width="80" class="tbltxt">
                Session
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="140" class="tbltxt">
                <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                    OnSelectedIndexChanged="drpSession_SelectedIndexChanged" TabIndex="1">
                </asp:DropDownList>
            </td>
            <td class="tbltxt" width="90">
                Class
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="100" class="tbltxt">
                <asp:DropDownList ID="drpClasses" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                    OnSelectedIndexChanged="drpClasses_SelectedIndexChanged" TabIndex="2">
                </asp:DropDownList>
            </td>
            <td width="60" class="tbltxt">
                Section
            </td>
            <td class="tbltxt" width="5">
                :
            </td>
            <td class="tbltxt" width="140">
                <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                    OnSelectedIndexChanged="drpSection_SelectedIndexChanged" TabIndex="3">
                </asp:DropDownList>
            </td>
            <td class="tbltxt">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Student
            </td>
            <td class="tbltxt">
                :
            </td>
            <td class="tbltxt">
                <asp:DropDownList ID="drpstudent" runat="server" AutoPostBack="True" CssClass="smalltb"
                    OnSelectedIndexChanged="drpstudent_SelectedIndexChanged" TabIndex="4">
                </asp:DropDownList>
            </td>
            <td width="90" class="tbltxt">
                Addmission No
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="120" class="tbltxt">
                <asp:DropDownList ID="drpadminno" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                    OnSelectedIndexChanged="drpadminno_SelectedIndexChanged" TabIndex="5">
                </asp:DropDownList>
            </td>
            <td colspan="4" class="tbltxt">
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" TabIndex="6" />&nbsp;
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="7" />&nbsp;
                <asp:Button ID="btnExpExcel" runat="server" Text="Export to Excel" Visible="False"
                    Width="106px" TabIndex="8" />
            </td>
        </tr>
    </table>
    <div style="padding-top: 10px;">
        <asp:Label ID="lblReport" runat="server"></asp:Label></div>
</asp:Content>

