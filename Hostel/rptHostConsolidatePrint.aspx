<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptHostConsolidatePrint.aspx.cs" Inherits="Hostel_rptHostConsolidatePrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Consolidate Receipt</title>
    <style>
        @media print
        {
            table
            {
                page-break-inside: avoid;
            }
        }
    </style>

   
    <script>
        function Print()
        { document.body.offsetHeight; window.print();  }
			
    </script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
    <center>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
    </center>
    </div>
    </form>
</body>
</html>
