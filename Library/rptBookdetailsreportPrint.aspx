﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptBookdetailsreportPrint.aspx.cs" Inherits="Library_rptBookdetailsreportPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Book Details</title>
    <script language="javascript" type="text/javascript">
        function Print() {
            window.print();
        }			
    </script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div style="width: 100%;">
        <asp:Label ID="lblprint" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
