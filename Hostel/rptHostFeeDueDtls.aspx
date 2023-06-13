<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptHostFeeDueDtls.aspx.cs" Inherits="Hostel_rptHostFeeDueDtls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Hostel Fee Details</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <center>
        <table width="75%" cellpadding="0" cellspacing="0">
            <tr>
                <td class="tbltxt" style="padding-left: 5px; background-color: #666; color: White;">
                    <div>
                        <div style="width: 50%; float: left;">
                            <b>Student Name :</b>
                            <asp:Label ID="lblName" runat="server"></asp:Label></div>
                        <div style="width: 40%; float: right;">
                            <b>Admission No :</b>
                            <asp:Label ID="lblRegd" runat="server"></asp:Label></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset1" runat="server" style="width: 96%">
                        <legend><b>Balance in Regular Hostel Fees :</b></legend>
                        <asp:Label ID="lblBalReg" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
        </table>
    </center>

    </div>
    </form>
</body>
</html>
