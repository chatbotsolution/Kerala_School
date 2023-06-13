<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptFeeReceiptPrint.aspx.cs" Inherits="Reports_rptFeeReceiptPrint" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Fee Receipt</title>

    <script>
        function Print()
        { document.body.offsetHeight; window.print();  }
			
    </script>

</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
        <table width="80%">
            <tr>
                <td align="left">
                    <asp:Label ID="lblDup" runat="server" Font-Bold="False" Font-Size="Medium" Font-Underline="True"
                        ForeColor="Gray">DUPLICATE</asp:Label>
                </td>
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
