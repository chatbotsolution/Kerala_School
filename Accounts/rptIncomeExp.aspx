<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptIncomeExp.aspx.cs" Inherits="Accounts_rptIncomeExp" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr style="background-color: #ededed;">
            <td width="350" align="left" valign="middle" style="background-color: ">
                <div class="headingcontainor">
                    <h2>
                        Income & Exenditure Account</h2>
                </div>
            </td>
            <td height="35" align="left" valign="middle" colspan="2">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <div>
        Accounting Yr:&nbsp;<asp:DropDownList ID="drpSession" runat="server" 
            onselectedindexchanged="drpSession_SelectedIndexChanged" AutoPostBack="true">
        </asp:DropDownList>&nbsp;&nbsp;
        
        From Date :
        <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" Width="80px" TabIndex="1"></asp:TextBox>
        <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
        </rjs:PopCalendar>
        &nbsp;
        To Date :
        <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" Width="80px" TabIndex="2"></asp:TextBox>
        <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" Format="dd mmm yyyy">
        </rjs:PopCalendar>
        &nbsp;&nbsp;
        
        <asp:Button ID="btnShow" runat="server" Text="Show" onclick="btnShow_Click" />
            &nbsp;
        
        <asp:Button ID="btnPrint" runat="server" Text="Print" 
            onclick="btnPrint_Click" />
    </div>
    <div class="spacer"></div>
    <div>
        <asp:Label ID="lblReport" runat="server" Text=""></asp:Label></div>
</asp:Content>


