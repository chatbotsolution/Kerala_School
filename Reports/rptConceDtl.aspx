<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptConceDtl.aspx.cs" Inherits="Reports_rptConceDtl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/sms.css" rel="stylesheet" type="text/css" />
    <title>Fee Concession</title>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <table width="85%" cellpadding="0" cellspacing="0">
            <tr>
                <td class="tbltxt cnt-box" style="padding-left: 5px; background-color: #ccc; color: White;">
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
                    <fieldset id="Fieldset1" runat="server" style="width: 96%" class="cnt-box lbl2">
                        <legend><b>
                            <asp:Label ID="lblHeader" runat="server"></asp:Label></b></legend>
                        <asp:Label ID="lblConcessFee" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>

