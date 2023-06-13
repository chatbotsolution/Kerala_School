<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptMemwiseFineRecievedPrint.aspx.cs" Inherits="Library_rptMemwiseFineRecievedPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Fee Received Details</title>
</head>
<script>
    function Print() { document.body.offsetHeight; window.print();  }
			
</script>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
    <table width="100%">
    <tr>
    <td align="left">
        <asp:Label ID="Label4" runat="server" Text="Fee Received Details" Font-Bold="True" Font-Underline="True"></asp:Label>
        </td>
        <td align="right">
            &nbsp;</td>
    </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label></td>
        </tr>
    </table>  
    </div>
    </form>
</body>
</html>
