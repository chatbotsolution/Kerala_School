﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptReceiptPaymentPrint.aspx.cs" Inherits="Accounts_rptReceiptPaymentPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Receipt Payment Report</title>
    <script language="javascript" type="text/javascript">
        function Print() {
            window.print();
       
        }			
    </script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
