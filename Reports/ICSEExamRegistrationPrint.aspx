﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ICSEExamRegistrationPrint.aspx.cs" Inherits="Reports_ICSEExamRegistrationPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Notice Print</title>
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
        { document.body.offsetHeight; window.print(); }
			
    </script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblRpt" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>