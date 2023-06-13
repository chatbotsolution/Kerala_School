<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptAdmnSumPrint.aspx.cs" Inherits="Reports_rptAdmnSumPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
<head id="Head1" runat="server">
    <title>Admission Summary Print</title>
    <script type="text/javascript" language="javascript">

        function Print() {
            document.body.offsetHeight;
            window.print();
        }
			
    </script>
    <style media="screen, print" type="text/css">
        @page
        {
            page-break-after:always;
        }

        body {
                margin:1pt;
            }
        table
        {
            /*border:1px solid #CCC;*/
            border-collapse:collapse;
            font-family:Arial;
        }

        td
        {
            border:1px solid #CCC;
            border-collapse:collapse;
            padding:3px;
            font-family:Arial;
            page-break-after:right;
        }

        .tblheader
        {
	        color:#000;
	        font-family:Arial;
	        font-size:12pt;
	        font-weight:bold;
	        padding:2px;
	        letter-spacing:1px;
        }

        .tbltd
        {
	        color:#000;
	        font-family:Arial;
	        font-size:10pt;
	        background-color:#FFF;
	        letter-spacing:1px;
        }

    </style>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
        <table width="100%">
            <tr>
                <td align="left">
                    <div style="width:250px; float:left;">
                        <asp:Label ID="Label4" runat="server" Text="Admission Summary Report" 
                            Font-Bold="True"></asp:Label>
                    </div>
                    <div style="width:200px; float:right; text-align:right;">
                        <asp:Label ID="lblPrintDate" runat="server" Text=""></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

