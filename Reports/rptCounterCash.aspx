﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptCounterCash.aspx.cs" Inherits="Reports_rptCounterCash" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
 <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Datewise Collection Report
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table cellspacing="0"  class="cnt-box" width="100%">
        <tr>
            <td class="tbltxt">
                Fee Collection Counter&nbsp;<asp:DropDownList ID="drpFeeCounter" runat="server" CssClass="vsmalltb"
                    AutoPostBack="True" TabIndex="1">
                </asp:DropDownList>
                &nbsp; Payment Mode :
                <asp:DropDownList ID="drpModeOfPayment" runat="server" CssClass="vsmalltb"  
                    TabIndex="2">
                    <asp:ListItem>All</asp:ListItem>
                    <asp:ListItem>Cash</asp:ListItem>
                    <asp:ListItem>Bank</asp:ListItem>
                </asp:DropDownList>
               
                From Date :
                <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" CssClass="tbltxtbox" Width="80px"
                    TabIndex="4"></asp:TextBox>
                <rjs:PopCalendar ID="dtpPros" runat="server" Control="txtFromDt" AutoPostBack="False"
                    Format="dd mmm yyyy"></rjs:PopCalendar>
                &nbsp; To Date :
                <asp:TextBox ID="txtToDt" runat="server" CssClass="tbltxtbox" ReadOnly="true"  Width="80px"
                    TabIndex="5"></asp:TextBox>
                <rjs:PopCalendar ID="dtpPros1" runat="server" Control="txtToDt" AutoPostBack="False"
                    Format="dd mmm yyyy"></rjs:PopCalendar>
                <br /><br />
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return Isvalid();"
                    TabIndex="6" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center" valign="top" class="tbltxt" 
                style="background-color: #2092d0;
                padding: 2px; height: 3px; color: #fff; border: 2px solid #333; padding: 5px 10px 5px 20px; text-align: left;">
                <div style="text-align: left; width: 500px;">
                    *** Click on -&gt; <b>Total Amount</b> to view the Details<br />
                </div>
            </td>
        </tr>
        <tr>
            <td align="center" style="text-align: left">
                <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>




