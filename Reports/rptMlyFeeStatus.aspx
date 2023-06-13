<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptMlyFeeStatus.aspx.cs" Inherits="Reports_rptMlyFeeStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript">
    function valShow() {

        var Month = document.getElementById("<%=drpMonth.ClientID %>").selectedIndex;

        if (Month == 0) {
            alert("Please select a Month !");
            document.getElementById("<%=drpMonth.ClientID %>").focus;
            return false;
        }
        else {
            return true;
        }
    }
    function popUp(URL) {
        day = new Date();
        id = day.getTime();
        eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=500,left = 390,top = 184');");
    } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Monthly Student Fee Status
        </h2>
    </div>
    <div>
<img src="../images/mask.gif" height="8" width="10" /></div>

 <table width="100%" border="0" cellspacing="2" cellpadding="2" class="cnt-box">
        <tr>
            <td width="50" class="tbltxt">
                Session
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="120">
                <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" Height="16px">
                </asp:DropDownList>
            </td>
            <td width="50" class="tbltxt">
                Class
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="120">
                <asp:DropDownList ID="drpclass" runat="server" CssClass="vsmalltb" Height="16px">
                </asp:DropDownList>
            </td>
            <td width="50" class="tbltxt">
                Month
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="120">
                <asp:DropDownList ID="drpMonth" runat="server" CssClass="vsmalltb" Height="16px">
                    <asp:ListItem Value="--Select--">--Select--</asp:ListItem>
                    <asp:ListItem Value="4">APRIL</asp:ListItem>
                    <asp:ListItem Value="5">MAY</asp:ListItem>
                    <asp:ListItem Value="6">JUNE</asp:ListItem>
                    <asp:ListItem Value="7">JULY</asp:ListItem>
                    <asp:ListItem Value="8">AUGUST</asp:ListItem>
                    <asp:ListItem Value="9">SEPTEMBER</asp:ListItem>
                    <asp:ListItem Value="10">OCTOBER</asp:ListItem>
                    <asp:ListItem Value="11">NOVEMBER</asp:ListItem>
                    <asp:ListItem Value="12">DECEMBER</asp:ListItem>
                    <asp:ListItem Value="1">JANUARY</asp:ListItem>
                    <asp:ListItem Value="2">FEBRUARY</asp:ListItem>
                    <asp:ListItem Value="3">MARCH</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td colspan="3">
                <asp:Button ID="btnShow" runat="server" Text="Show"
                    OnClientClick="return valShow();" onclick="btnShow_Click" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" 
                    onclick="btnPrint_Click"  />
                <asp:Button ID="btnExpExcel" runat="server" Text="Export to Excel"
                    Width="106px" onclick="btnExpExcel_Click" />
            </td>
        </tr>
    </table>
        <table width="100%" class="tbltxt">
        <tr>
            <td valign="top">
                <div id="divreport">
                    <table cellspacing="0" cellpadding="0" width="100%">
                        <tr id="trgrd" runat="server" visible="false">
                            <td valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl2">
                                <asp:Label ID="lblReport" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div style="display: none" id="divhdr">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label4" runat="server" Text="Fee Received" Font-Bold="True" Font-Underline="True"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblPrintDate" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>


