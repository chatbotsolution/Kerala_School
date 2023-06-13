﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptMlyFeeStatusPrint.aspx.cs" Inherits="Reports_rptMlyFeeStatusPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Monthly Fee Status Report</title>
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
       // history.back();
    }			
    </script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblRpt" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>