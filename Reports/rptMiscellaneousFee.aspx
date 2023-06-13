<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptMiscellaneousFee.aspx.cs" Inherits="Reports_rptMiscellaneousFee" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/sms.css" rel="stylesheet" type="text/css" />
    <title>Miscellaneous Fee</title>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <table width="75%" cellpadding="0" cellspacing="0">
            <tr>
                <td class="tbltxt" style="padding-left: 5px;background-color: #666; color: White;">
                    <div >
                        <div style="width: 50%; float: left;">
                            <b>Student Name :</b>
                            <asp:Label ID="lblName" runat="server"></asp:Label></div>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        <div style="width: 35%; float: right;">
                            <b>Admission No :</b>
                            <asp:Label ID="lblRegd" runat="server"></asp:Label></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset1" runat="server" style="width: 96%">
                        <legend><b>Miscellaneous Fee :</b></legend>
                        <asp:Label ID="lblMiscFee" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-left: 5px;">
                    <div class="tbltxt" style="background-color: #666; color: White;">
                        <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>

