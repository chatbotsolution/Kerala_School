<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptMemwiseFineRecieved.aspx.cs" Inherits="Library_rptMemwiseFineRecieved" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Memberwise Fine Recieved</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 35px;">
        <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="Print" />
        &nbsp;&nbsp;
        <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export To Excel" />
        &nbsp;</div>
    <center>
        <div>
            <asp:Label ID="lblReport" runat="server"></asp:Label></div>
    </center>
    </form>
</body>
</html>
