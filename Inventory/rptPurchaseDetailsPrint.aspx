<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptPurchaseDetailsPrint.aspx.cs" Inherits="Inventory_rptPurchaseDetailsPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>

<script>
    function Print() { document.body.offsetHeight; window.print();  }
			
</script>

<body onload="Print();">
    <form id="form1" runat="server">
    <%--<div>
        <asp:Label ID="Label1" runat="server" Text="Purchase List" Font-Bold="true"></asp:Label>
    </div>--%>
    <div>
        <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>
</html>
