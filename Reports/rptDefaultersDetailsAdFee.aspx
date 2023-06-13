<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDefaultersDetailsAdFee.aspx.cs" Inherits="Reports_rptDefaultersDetailsAdFee" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Additional Fee Defaulters Details</title>
    <link href="../css/sms.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <table width="75%" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <fieldset id="Fieldset1" runat="server" class="cnt-box lbl2 tbltxt">
                        <legend class="tbltxt">Fee Details:</legend>
                        <asp:Label ID="lblDetails" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>

