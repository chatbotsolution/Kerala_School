<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDupRcptConsPrint.aspx.cs" Inherits="Accounts_rptDupRcptConsPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        function Print() {
            window.print();
           //history.back();
        }
    </script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div style="text-align:center;">
         <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
