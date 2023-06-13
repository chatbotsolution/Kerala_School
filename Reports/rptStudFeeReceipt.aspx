<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptStudFeeReceipt.aspx.cs" Inherits="Reports_rptStudFeeReceipt" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table width="100%" style="height:340px;">
        <TR>
            <TD vAlign="top" align="center" bgColor=#dfdfdf colspan="3" style="width:100%;">
            <STRONG>Fee Receipt</STRONG> 
            </TD>
        </TR>
       
        
        <tr>
        <td valign="top" colspan="5">
        <table width="100%" >
       
                <tr>
                <td colspan="4" align="left">
                    <asp:Label ID="lblClasses" runat="server" Text="Student Admission No:"></asp:Label>
                   <asp:DropDownList ID="drpStudent" runat="server" AutoPostBack="True"  
                        Width="181px" TabIndex="1">
                    </asp:DropDownList>
                    &nbsp; &nbsp; &nbsp;
                    <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
                    &nbsp; &nbsp; &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    
                </td>
                <td align="right">
                    &nbsp; &nbsp;
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" 
                        TabIndex="2" />
                    <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" 
                        Text="Export to Excel" TabIndex="3" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" 
                        TabIndex="4" />
                </td>
            </tr>
            
        </table>
        </td>
        </tr>
        <tr>
        <td >
                <asp:Label ID="lblReport" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td style="height:65%;" align="center" valign="top">
                &nbsp;</td>
        </tr>
    </table>

</asp:Content>


