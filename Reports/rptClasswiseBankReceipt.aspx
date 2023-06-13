<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptClasswiseBankReceipt.aspx.cs" Inherits="Reports_rptClasswiseBankReceipt" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>
    
<asp:Content ID="contentRemarkReport" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<asp:UpdatePanel id="UpdatePanel1" runat="server">--%>
<ContentTemplate></ContentTemplate>
    <table width="100%" style="height:410px;">
      <TR>
                <TD vAlign="top" align="center" bgColor=#dfdfdf colspan="3" style="width:100%;">
                <STRONG>Bank Receipt</STRONG> 
                </TD>
            </TR>
       
        
        <tr>
        <td valign="top" colspan="5">
        <table width="100%" >
       
                <tr>
                <td colspan="4" align="left">
                    <asp:Label ID="lblSession" runat="server" Text="Session" ></asp:Label>
                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True"  
                        Width="89px" OnSelectedIndexChanged="drpSession_SelectedIndexChanged" 
                        TabIndex="1"></asp:DropDownList>
                    
                    <asp:Label ID="lblClasses" runat="server" Text="Class" ></asp:Label>
                    <asp:DropDownList ID="drpClasses" runat="server" AutoPostBack="True"  
                        Width="83px" OnSelectedIndexChanged="drpClasses_SelectedIndexChanged" 
                        TabIndex="2"></asp:DropDownList>
                    <asp:Label ID="lblSection" runat="server" Text="Section" ></asp:Label>
                    <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True"  
                        Width="74px" OnSelectedIndexChanged="drpSection_SelectedIndexChanged" 
                        TabIndex="3"></asp:DropDownList>
                     
                    
                    <asp:Label ID="lblFromDate" runat="server" Text="From Date" ></asp:Label>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="100px" TabIndex="4"></asp:TextBox>&nbsp;
                    <rjs:PopCalendar ID="pcalFromDate" runat="server" Control="txtFromDate" />
                    <asp:Label ID="lblToDate" runat="server" Text="To Date" ></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="100px" TabIndex="5"></asp:TextBox>
                    <rjs:PopCalendar ID="pcalToDate" runat="server" Control="txtToDate" />
             
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" 
                        TabIndex="6" />&nbsp;
                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" 
                        TabIndex="7" /></td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" 
                        Text="Export to Excel" Visible="False" Width="106px" TabIndex="8" /></td>
                <td align="right">
                    &nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                </td>
            </tr>
            
        </table>
        </td>
        </tr>
        <tr>
        <td ></td>
        </tr>
        
        <tr>
            <td style="height:65%;" align="center" valign="top">
                <asp:Label ID="lblReport" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
<%--</asp:UpdatePanel>--%>
</asp:Content>


