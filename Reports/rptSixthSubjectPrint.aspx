<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptSixthSubjectPrint.aspx.cs" Inherits="Reports_rptSixthSubjectPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Sixth Subject List</title>
    <script>
        function Print() { document.body.offsetHeight; window.print();  }
			
</script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
    <table width="100%">
    <tr>
    <td><center>Sixth subject Student List</center>
    </td>
    </tr>
    <tr>
    
    <td>
    <strong>LOYOLA SCHOOL,BHUBANESWAR</strong> <span style="float:right"> <asp:Label ID="lblDate" runat="server"></asp:Label></span>
    
    </td>
       </tr>
    <tr>
    <td>
     <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
    </td>
    </tr>
    </table>
    
    </div>
    </form>
</body>
</html>