<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptHostFeeToBeReceivedPrtnt.aspx.cs" Inherits="Hostel_rptHostFeeToBeReceivedPrtnt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Fee Component Amount To be Received</title>

    <script>
        function Print() { document.body.offsetHeight; window.print();  }
			
    </script>

    <style media="screen, print">
        @page
        {
            page-break-after: always;
        }
        body
        {
            margin: 1pt;
        }
        table
        {
            /*border:1px solid #CCC;*/
            border-collapse: collapse;
            font-family: Arial;
        }
        td
        {
            border: 1px solid #CCC;
            border-collapse: collapse;
            padding: 3px;
            font-family: Arial;
            page-break-after: right;
        }
        .tblheader
        {
            color: #000;
            font-family: Arial;
            font-size: 12pt;
            font-weight: bold;
            padding: 2px;
            letter-spacing: 1px;
        }
        .tbltd
        {
            color: #000;
            font-family: Arial;
            font-size: 10pt;
            background-color: #FFF;
            letter-spacing: 1px;
        }
    </style>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
