<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="ExportStudentData.aspx.cs" Inherits="Reports_ExportStudentData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Export Student Data
        </h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" cellspacing="2" cellpadding="2">
        <tr>
            <td class="tbltxt" style="width: 95px">
                <asp:Label ID="lblSession" runat="server" Text="Select Session"></asp:Label>&nbsp;
            </td>
            <td class="tbltxt" style="width: 5px">
                :
            </td>
            <td class="tbltxt" style="width: 18px">
                <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                    Width="86px" TabIndex="1">
                </asp:DropDownList>
            </td>
            <td width="70" class="tbltxt">
                <asp:Label ID="lblClasses" runat="server" Text="Select Class"></asp:Label>
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="90" class="tbltxt">
                <asp:DropDownList ID="drpClasses" runat="server" AutoPostBack="True" Width="71px"
                    OnSelectedIndexChanged="drpClasses_SelectedIndexChanged" TabIndex="2">
                </asp:DropDownList>
            </td>
            <td class="tbltxt">
                <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                    Visible="False" TabIndex="8" />
            </td>
        </tr>
    </table>
    <div style="padding-top: 10px;">
        <asp:Label ID="lblReport" runat="server"></asp:Label></div>
    <div>
        <asp:Label ID="lblGrdMsg" runat="server" ForeColor="Red"></asp:Label></div>
</asp:Content>

