<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="rptHostStudInvStatus.aspx.cs" Inherits="Hostel_rptHostStudInvStatus" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Pending Details</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblmsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
    </div>
    <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px">
        <table width="100%" cellspacing="2" cellpadding="2">
            <tr>
                <td width="80px" class="tbltxt">
                    Pending For :
                    <asp:DropDownList ID="drpStudent" runat="server" CssClass="tbltxtbox" Width="150px">                        
                    </asp:DropDownList>
                    <rjs:PopCalendar ID="pcalFromDate" runat="server" Control="txtFromDate" />
                    <asp:Button ID="btnShow" runat="server" Text="Show" onclick="btnShow_Click" />
                    <%--<asp:Button ID="btnPrint" runat="server" Text="Print" onclick="btnPrint_Click" />--%>                        
                    <asp:Button ID="btnExpExcel" runat="server" Text="Export to Excel" 
                        Visible="False" onclick="btnExpExcel_Click" />                        
                </td>
            </tr>
        </table>
    </div>
    <div style="padding-top: 5px;">
        <asp:Label ID="lblReport" runat="server"></asp:Label>
    </div>
</asp:Content>

