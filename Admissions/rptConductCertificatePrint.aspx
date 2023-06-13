<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptConductCertificatePrint.aspx.cs" Inherits="Admissions_rptConductCertificatePrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Conduct Certificate</title>
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
    <div></div>
    <div style="width: 100%;">
        <asp:Label ID="lblprint" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
