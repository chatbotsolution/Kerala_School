<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptFeeReceivedPrint.aspx.cs" Inherits="Reports_rptFeeReceivedPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Wise Fee Received</title>

    <script>
        function Print() { document.body.offsetHeight; window.print(); }
			
    </script>

</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
        <table width="80%">
            <tr>
                <td align="left">
                    <asp:Label ID="Label4" runat="server" Text="Student Wise Fee Received :-" Font-Bold="True"></asp:Label>
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
