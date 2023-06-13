<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptStudFineLedgerPrint.aspx.cs" Inherits="Reports_rptStudFineLedgerPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Fine Ledger</title>
<script>
    function Print() { document.body.offsetHeight; window.print(); }
			
</script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
     <table width="80%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Text="Sri Guru Harkrishan Sr.Sec. Public School"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Affiliated to C.B.S.E. New Delhi"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="(Chief Khalsa Diwan), Sector 40-C, Chandigarh"></asp:Label></td>
                        </tr>
                      
                    </table>
    <table width="80%">
    <tr>
    <td align="left">
        <asp:Label ID="Label4" runat="server" Text="Fine Ledger" Font-Bold="True" Font-Underline="True"></asp:Label>
        </td>
        <td align="right">
        <asp:Label ID="lblPrintDate" runat="server" Text="Label"></asp:Label>
        </td>
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
