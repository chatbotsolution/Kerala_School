﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptFeeAdjustment.aspx.cs" Inherits="Reports_rptFeeAdjustment" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script language="javascript" type="text/javascript">

     function openwindow() {
         var pageurl = "rptFeeAdjustmentPrint.aspx";
         window.open(pageurl, 'true', 'true');
     }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            List of Student with Concession</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" cellspacing="2" cellpadding="2" class="cnt-box">
        <tr>
            <td width="30" class="tbltxt">
                Session
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="80" class="tbltxt">
                <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" Width="70px"
                    CssClass="vsmalltb" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                    TabIndex="1">
                </asp:DropDownList>
            </td>
            <td class="tbltxt" width="30">
                Class
            </td>
            <td class="tbltxt" width="5">
                :
            </td>
            <td class="tbltxt" width="80">
                <asp:DropDownList ID="drpClasses" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                    OnSelectedIndexChanged="drpClasses_SelectedIndexChanged" TabIndex="4">
                </asp:DropDownList>
            </td>
            <td class="tbltxt" width="50">
                Student
            </td>
            <td class="tbltxt" width="5">
                :
            </td>
            <td class="tbltxt" width="120">
                <asp:DropDownList ID="drpstudent" runat="server" CssClass="smalltb" AutoPostBack="True"
                    OnSelectedIndexChanged="drpstudent_SelectedIndexChanged" TabIndex="2">
                </asp:DropDownList>
            </td>
            <td class="tbltxt" width="75">
                Admission No
            </td>
            <td class="tbltxt" width="5">
                :
            </td>
            <td class="tbltxt" width="50">
                <asp:TextBox ID="txtadmnno" runat="server" CssClass="vsmalltb" TabIndex="5"></asp:TextBox>
            </td>
            </tr>
            <tr>
            <td align="left" class="tbltxt" colspan="12">
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return isValid();"
                    TabIndex="7" />&nbsp;
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="openwindow();"
                    TabIndex="8" />
                <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                    Width="106px" TabIndex="9" />
            </td>
        </tr>
    </table>
    <div style="padding-top: 10px; color: #931f1f; font-size: 14px; text-align: center;">
        <asp:Label ID="lblMsg" Font-Bold="true" runat="server" Text=""></asp:Label>
    </div>
    <div style="padding-top: 10px;">
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <div style="padding-top: 10px;">
        <asp:Label ID="lblReport" runat="server"></asp:Label></div>
</asp:Content>



