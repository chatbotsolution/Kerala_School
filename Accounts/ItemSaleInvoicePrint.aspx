<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="ItemSaleInvoicePrint.aspx.cs" Inherits="Accounts_ItemSaleInvoicePrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Receipt Print</title>
    <style>
        @media print
        {
            table
            {
                page-break-inside: avoid;
            }
        }
    </style>

    <script language="javascript" type="text/javascript">
        function Print() {
            window.print();
            history.back();

        }			
    </script>

</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <center>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
