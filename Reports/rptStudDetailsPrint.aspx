<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptStudDetailsPrint.aspx.cs" Inherits="Reports_rptStudDetailsPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Report</title>
    <script>
        function Print() { document.body.offsetHeight; window.print();  }
			
</script>
<style type="text/css">
    .textstyle
    {
        font-size:12px;
        font-family:Calibri;
    }
</style>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    
    <div>
    <div>
    <table width="100%">
    <tr>
    <td class="tbltxt" style=" text-align:center"><b>Kerala School, Keonjhar</b>
    <div><span class="tbltxt">Class: &nbsp <asp:Label ID="lblCls" runat="server"></asp:Label>&nbsp &nbsp Section: &nbsp<asp:Label ID="lblSec" runat="server"></asp:Label></span> 
    </div>
       </td>
     <td width="30%" class="textstyle">
   <div>
     Session :
     <asp:Label ID="lblSes" runat="server"></asp:Label> 
    </div>
    <div>
    <asp:Label ID="lblDate" runat="server"></asp:Label>
   </div>
    
     
    </td>
    </tr>
    <tr>
    <td colspan="2">
     <asp:Label ID="lblReport" runat="server" Text="" CssClass="textstyle"></asp:Label>
    </td>
    </tr>
    </table>
    
    </div>
    </form>
</body>
</html>
