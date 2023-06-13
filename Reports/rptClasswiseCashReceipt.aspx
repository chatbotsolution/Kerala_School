<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptClasswiseCashReceipt.aspx.cs" Inherits="Reports_rptClasswiseCashReceipt" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>
    
<asp:Content ID="contentRemarkReport" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <table width="100%" style="height:410px;">
        <TR>
            <TD vAlign="top" align="center" bgColor=#dfdfdf colspan="3" style="width:100%;">
            <STRONG>Fee Receipt</STRONG> 
            </TD>
        </TR>
       
        
        <tr>
        <td valign="top" colspan="5">
        <table width="100%" >
       
                <tr>
                <td colspan="4" align="left" style="height: 56px">
                    &nbsp;<asp:Label ID="Label2" runat="server" Text="Payment Mode" ></asp:Label>
                    <asp:DropDownList ID="drpPmtMode" runat="server" onclick="return ViewChequeDetails();"
                        TabIndex="1" Width="73px">
                        <asp:ListItem>Cash</asp:ListItem>
                        <asp:ListItem>Bank</asp:ListItem>
                        <asp:ListItem>Cheque</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                    <asp:Label ID="lblSession" runat="server" Text="Session"></asp:Label>&nbsp;<asp:DropDownList
                        ID="drpSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                        Width="86px" TabIndex="2">
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                    <asp:Label ID="lblClasses" runat="server" Text="Class" ></asp:Label>
                    <asp:DropDownList ID="drpClasses" runat="server" AutoPostBack="True"  
                        Width="71px" OnSelectedIndexChanged="drpClasses_SelectedIndexChanged" 
                        TabIndex="3">
                    </asp:DropDownList>
                    &nbsp; &nbsp; 
                    <asp:Label ID="Label1" runat="server" Text="Section" ></asp:Label>&nbsp;<asp:DropDownList
                        ID="drpSection" runat="server" 
                        Width="55px" OnSelectedIndexChanged="drpSection_SelectedIndexChanged1" 
                        TabIndex="4">
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                    <asp:Label ID="lblFromDate" runat="server" Text="From Date" ></asp:Label>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="70px" TabIndex="5"></asp:TextBox>&nbsp;<rjs:PopCalendar ID="pcalFromDate" runat="server" Control="txtFromDate" />
                    &nbsp;&nbsp;
                    <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="False"></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="70px" TabIndex="6"></asp:TextBox>
                    <rjs:PopCalendar ID="pcalToDate" runat="server" Control="txtToDate" />
                    &nbsp; &nbsp;&nbsp;<asp:Button ID="btnShow" runat="server" Text="Show" 
                        OnClick="btnShow_Click" TabIndex="7" /><asp:Button ID="btnPrint" 
                        runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="8" /></td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    
                </td>
                <td align="right">
                    &nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                </td>
            </tr>
            
        </table>
        <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" 
                Text="Export to Excel" Visible="False" TabIndex="9" /></td>
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


