<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptFeeBalancePrint.aspx.cs" Inherits="Reports_rptFeeBalancePrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/sms.css" rel="stylesheet" type="text/css" />
    <title>Total Balance For Individual Students</title>

    <script>
        function Print() { document.body.offsetHeight; window.print();  }
			
    </script>

</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <asp:Label ID="lblReport" runat="server"></asp:Label>
    </form>
</body>
</html>
