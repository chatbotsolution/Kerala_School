<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptStudStatusPrint.aspx.cs" Inherits="Reports_rptStudStatusPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <title>Student Status</title>
    <script>
        function Print() { document.body.offsetHeight; window.print(); }
			
</script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
 
    <div>
    <table width="100%">
    <tr>
    <td>
    <strong>LOYOLA SCHOOL,BHUBANESWAR</strong> <span style="margin-left:100px"> <asp:Label ID="lblDate" runat="server"></asp:Label></span>
    
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
