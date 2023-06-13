<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptLoanRecDetails.aspx.cs" Inherits="HR_rptLoanRecDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Loan Recovery Report</title>

  <script language="javascript" type="text/javascript">
      function Print() {
          document.getElementById('btnPrint').style.display = 'none';
          window.print();
          history.back();
          document.getElementById('btnPrint').style.display = 'inherit';
      }			
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" cellspacing="2" cellpadding="2">
        <tr>
            <td align="right">
                <asp:Button ID="btnPrint" runat="server" Text="Print" TabIndex="7" OnClientClick="Print();" />&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblReport" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
