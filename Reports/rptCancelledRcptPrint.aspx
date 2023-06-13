<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptCancelledRcptPrint.aspx.cs" Inherits="Reports_rptCancelledRcptPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Cancelled Receipt </title>
    <style type="text/css">
        @media print
        {
            table
            {
                page-break-inside: avoid;
            }
        }
         .tbltxt
        {
            font-family: Tahoma, Geneva, sans-serif;
            font-size: 12px;
            color: #000;
            padding: 3px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function Print() {
            window.print();
           // history.back();
            document.body.offsetHeight;
        }			
    </script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
        <asp:Label runat="server" ID="lblRpt" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>

