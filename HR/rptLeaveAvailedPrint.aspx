<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptLeaveAvailedPrint.aspx.cs" Inherits="HR_rptLeaveAvailedPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Leave Availed Report</title>

    <script language="javascript" type="text/javascript">
        function Print() {
            window.print();
           // history.back();
        }
    </script>

</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <asp:Label ID="lblPrint" runat="server"></asp:Label>
    </form>
</body>
</html>
