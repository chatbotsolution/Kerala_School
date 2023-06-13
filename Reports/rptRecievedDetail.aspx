<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptRecievedDetail.aspx.cs" Inherits="Reports_rptRecievedDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cash Received Details</title>
    <link href="../css/sms.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="cnt-box" style="margin:50px auto; width:90%">
    <div >
        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
        <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel" />
    </div>
    <div class="spacer"></div>
    <div style="width:100%">
        <asp:Label ID="lblReport" runat="server"></asp:Label>
    </div>
    
    </fieldset>
    </form>
</body>
</html>