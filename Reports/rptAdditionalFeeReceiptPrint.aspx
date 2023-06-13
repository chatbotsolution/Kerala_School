<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptAdditionalFeeReceiptPrint.aspx.cs" Inherits="Reports_rptAdditionalFeeReceiptPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Additional Fee Receipt</title>
<script>
    function Print() { document.body.offsetHeight; window.print(); }
			
</script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
     <table width="70%">
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblDup" runat="server" Font-Bold="False" Font-Size="Medium" Font-Underline="True" ForeColor="Gray"></asp:Label></td>
                        </tr>                      
                    </table>
    <table width="100%">
    <tr>
    </tr>
        <tr>
            <td colspan="2" align="left">
                <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label></td>
        </tr>
    </table>
        
                <table width="70%">
                                      
                                     
                                    </table>
            
        
    </div>
    </form>
</body>
</html>


