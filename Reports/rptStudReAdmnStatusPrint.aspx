﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptStudReAdmnStatusPrint.aspx.cs" Inherits="Reports_rptStudReAdmnStatusPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Admission & Readmission Status</title>

    <script>
        function Print() { document.body.offsetHeight; window.print(); }
			
    </script>

</head>
<body onload="Print();">
    <form id="form1" runat="server">
     <%-- <div style="padding-top:1%;padding-left:2%;"><asp:Button ID="btnPrev" 
           runat="server"  Text="Back" BackColor="LightCyan" Width="5%" onclick="btnPrev_Click" /></div>
           <br />--%>
    <div>
    <div>
        <table width="80%">
            <tr>
                <td align="left">
                    <asp:Label ID="Label4" runat="server" Text="Student Admission & Readmission Status :-" Font-Bold="True"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="lblPrintDate" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
