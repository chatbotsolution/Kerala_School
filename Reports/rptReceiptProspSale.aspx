<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptReceiptProspSale.aspx.cs" Inherits="Reports_rptReceiptProspSale" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Prospectus Sale</title>
    <style type="text/css">
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
    <asp:Label ID="lbldetail" runat="server"></asp:Label>
    </form>
</body>
</html>
