<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptEmpSalStructurePrint.aspx.cs" Inherits="HR_rptEmpSalStructurePrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Current Salary Structure</title>

    <script language="javascript" type="text/javascript">
        function Print() { document.body.offsetHeight; window.print(); }			
    </script>

</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
