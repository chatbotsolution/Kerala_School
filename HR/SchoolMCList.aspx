<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchoolMCList.aspx.cs" Inherits="HR_SchoolMCList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Managing Committee List</title>

    <script type="text/javascript">
        function Print() {
            document.getElementById("btnPrint").style.display = "None";
            document.body.offsetHeight; window.print(); history.back();
        }			
    </script>

</head>
<body>
    <div align="right">
        <input type="button" id="btnPrint" name="btnPrint" value="Print" onclick="Print();" /></div>
    <center>
        <form id="form1" runat="server">
        <asp:Label ID="lblReport" runat="server"></asp:Label>
        </form>
    </center>
</body>
</html>
