<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptMiscFeeList.aspx.cs" Inherits="Reports_rptMiscFeeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/sms.css" rel="stylesheet" type="text/css" />
    <title>Bus & Hostel Fee</title>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <table width="75%" cellpadding="0" cellspacing="0">
            <tr>
                <td class="tbltxt cnt-box" style="padding: 15px; background-color: #666; color: White;">
                    <div>
                        <div style="width: 50%; float: left;">
                            <b>Student Name :</b>
                            <asp:Label ID="lblName" runat="server"></asp:Label></div>
                        <div style="width: 35%; float: right;">
                            <b>Admission No :</b>
                            <asp:Label ID="lblRegd" runat="server"></asp:Label></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset1" runat="server" class="cnt-box">
                        <legend><b>
                            <asp:Label ID="lblHeader" runat="server"></asp:Label></b></legend>
                        <asp:Label ID="lblMiscFee" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-left: 5px;">
                    <div class="tbltxt cnt-box" style="background-color: #666; color: White;">
                        <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
