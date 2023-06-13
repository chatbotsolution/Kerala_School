<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptRecieptChequePrint.aspx.cs" Inherits="Reports_rptRecieptChequePrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Fee Receipt</title>
<script>
    function Print() { document.body.offsetHeight; window.print();  }
			
</script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
     <table width="80%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Text="Sri Guru Harkrishan Sr.Sec. Public School"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Affiliated to C.B.S.E. New Delhi"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="(Chief Khalsa Diwan), Sector 40-C, Chandigarh"></asp:Label></td>
                        </tr>
                      
                    </table>
                    <table width="100%">
                                    <tr>
                                        <td style="width: 100%">
                                            <asp:Label ID="literaldata" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 524px">
                                        </td>
                                    </tr>
                                </table>
                    
                                    <table width="70%">
                                        <tr>
                                            <td align="left">
                                                Note: 1.Monthly fee shall be paid by 15th , After that fine Rs 50/- will be charged.
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2.Late fee fine Rs.
                                                50/- will be charged.
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2.Fee collected will
                                                not be refundable.
                                            </td>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <strong>HEAD CLERK</strong>
                                            </td>
                                            <td align="RIGHT">
                                                <strong>PRINCIPAL</strong>
                                            </td>
                                        </tr>
                                    </table>
                                    <%-- <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label4" runat="server" Text="Fee Received" Font-Bold="True" Font-Underline="True"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblPrintDate" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>--%>
                                
    <%--<table width="80%">
    <tr>
    <td align="left">
        <asp:Label ID="Label4" runat="server" Text="Additional Fee Receipt" Font-Bold="True" Font-Underline="True"></asp:Label>
        </td>
        <td align="right">
        <asp:Label ID="lblPrintDate" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label></td>
        </tr>
    </table>--%>
    </div>
    </form>
</body>
</html>
