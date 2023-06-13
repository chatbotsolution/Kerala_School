<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptAdditionalFeeReceipt.aspx.cs" Inherits="Reports_rptAdditionalFeeReceipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Additional Fee Receipt</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" border="0" cellspacing="2" cellpadding="2">
        <tr>
            <td>
                <%--<input type="button" value="Back" title="Back" 
            onclick="javascript:history.back();" id="Button1" tabindex="1" />--%>
                <asp:Button Text="Back" runat="server" ID="btnBack" OnClick="btnBack_Click" />&nbsp;
                <asp:Button ID="btnprint" runat="server" Text="Print" OnClick="btnprint_Click" TabIndex="2" />
                <div>
                    <asp:Label ID="lblReport" runat="server" CssClass="error"></asp:Label></div>
                <div>
                    <asp:Label ID="lblDetails" runat="server"></asp:Label></div>
                <div>
                    <asp:Label ID="literaldata" runat="server" CssClass="error"></asp:Label></div>
            </td>
        </tr>
        <tr>
            <td id="tdreport" runat="server">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>

