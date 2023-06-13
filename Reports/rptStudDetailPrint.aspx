<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptStudDetailPrint.aspx.cs" Inherits="Admissions_rptStudDetailPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Report</title>
    <script>
        function Print() { document.body.offsetHeight; window.print();}
			
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%">
    <tr>
    <td>
    </td>
     <td >
    </td>
     <td width="20%">
     <div>
     Session : <%#Eval("SessionYear")%>
     </div>
      <div>
     Class : <%#Eval("ClassName")%>
     </div>
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
