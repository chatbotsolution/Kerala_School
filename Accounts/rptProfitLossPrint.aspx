<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptProfitLossPrint.aspx.cs" Inherits="Accounts_rptProfitLossPrint" %>

<head id="Head1" runat="server">
    <title>Profit & Loss Account</title>
    <script language="javascript" type="text/javascript">
        function Print() {
            window.print();
           
        }			
    </script>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>

