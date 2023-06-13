<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewPurchaseDetails.aspx.cs" Inherits="Inventory_ViewPurchaseDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>purchase Details</title>
    <link href="../css/sms.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="float: left; width: 300px;" class="tbltxt">
            <b>Purchase Details</b>
        </div>
        <div style="float: right;">
            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
            <asp:Button ID="btnExpExcel" runat="server" Text="Export to Excel" OnClick="btnExpExcel_Click" />
            <asp:Button ID="Close" runat="server" Text="Close" OnClientClick="self.close();" />
        </div>
    </div>
    <div style="padding-top: 5px;">
        <asp:Label ID="lblReport" runat="server"></asp:Label></div>
    </form>
</body>
</html>
