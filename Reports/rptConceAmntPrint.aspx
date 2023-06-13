<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptConceAmntPrint.aspx.cs" Inherits="Reports_rptConceAmntPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Concession Report</title>
    <style>
        @media print
        {
            table
            {
                page-break-inside: avoid;
            }
        }
    </style>

    <script language="javascript" type="text/javascript">
        function Print() {
            window.print();
           
        }			
    </script>

</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <center>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Concession Report"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
