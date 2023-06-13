<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentTCPrint.aspx.cs" Inherits="Admissions_StudentTCPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student TC</title>
    <script>
        function Print() { document.body.offsetHeight; window.print();  }
			
</script>
<%--<style type="text/css">
@page
{
 size: landscape;
 margin: 2% 2% 2% 2%; 
}
.UlSpan
{
	border-bottom-width:1px;
	border-bottom-style:dotted;
}
</style>--%>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div style=" margin-top:0px;"></div>
    <div>
        <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
