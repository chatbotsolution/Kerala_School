<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptEstimation.aspx.cs" Inherits="Reports_rptEstimation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
 <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Estimation Report</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" border="0" cellspacing="2" cellpadding="2" class="cnt-box">
        <tr>
            <td width="100" class="tbltxt">
                Session Year
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="100">
                <asp:DropDownList ID="drpSessionYr" runat="server" CssClass="vsmalltb" OnSelectedIndexChanged="drpSessionYr_SelectedIndexChanged"
                    TabIndex="1">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" TabIndex="4" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="5" />
                <asp:Button ID="btnExpExcel" runat="server" Text="Export to Excel" TabIndex="6" 
                    onclick="btnExpExcel_Click" />
            </td>
        </tr>
    </table>
    <div class="tbltxt " style="box-shadow: 0px 0px 5px #888;  height: 15px; background:#ccc; color:white ">
             <span class="error">***</span> Selection of Class only effects on Fee Components, Fine, Bus, Hostel & Prospectus
        Sale.
    </div>
    <div class="spacer"></div>
    <div class="cnt-box2 lbl2 tbltxt">
        <div style="padding-top: 10px;">
            <asp:Label ID="lblReport" runat="server"></asp:Label></div>
        <div style="padding-top: 5px;">
            <asp:Label ID="lblFine" runat="server"></asp:Label></div>
        <div style="padding-top: 5px;">
            <asp:Label ID="lblBus" runat="server"></asp:Label></div>
        <div style="padding-top: 5px;">
            <asp:Label ID="lblHostel" runat="server"></asp:Label></div>
        <div style="padding-top: 5px;">
            <asp:Label ID="lblProsFee" runat="server"></asp:Label></div>
        <div style="padding-top: 5px;">
            <asp:Label ID="lblBookMaterial" runat="server"></asp:Label></div>
        <div style="padding-top: 5px;">
            <asp:Label ID="lblAnudan" runat="server"></asp:Label></div>
        <div style="padding-top: 5px;">
            <asp:Label ID="lblMiscFee" runat="server"></asp:Label></div>
            <div style="padding-top: 5px;">
            <asp:Label ID="lblPrev" runat="server"></asp:Label></div>
        <div style="padding-top: 5px;">
            <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
        <div style="padding-top: 5px;">
            <asp:Label ID="lblAdj" runat="server"></asp:Label></div>
        <div style="padding-top: 5px;">
            <asp:Label ID="lblGTotal" runat="server"></asp:Label></div>
    </div>
</asp:Content>


