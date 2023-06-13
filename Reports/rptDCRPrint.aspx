<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDCRPrint.aspx.cs" Inherits="Reports_rptDCRPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Fee Collection Report</title>
    <link href="../css/sms.css" rel="stylesheet" type="text/css" />
    <script>
        function Print() { document.body.offsetHeight; window.print(); history.back(); }
			
    </script>

</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td align="left">
                    <div style="width: 250px; float: left;">
                        <asp:Label ID="Label4" runat="server" Text=" Fee Collection Report" Font-Bold="True"></asp:Label></div>
                    <div style="width: 200px; float: right; text-align: right;">
                        <asp:Label ID="lblPrintDate" runat="server" Text="Label"></asp:Label></div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
