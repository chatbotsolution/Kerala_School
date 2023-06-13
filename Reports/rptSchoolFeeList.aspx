<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptSchoolFeeList.aspx.cs" Inherits="Reports_rptSchoolFeeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/sms.css" rel="stylesheet" type="text/css" />
    <title>Student School Fee</title>
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
                        <legend><b>Regular Fee :</b></legend>
                        <asp:Label ID="lblSchoolFee" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset2" runat="server" class="cnt-box">
                        <legend><b>Fine :</b></legend>
                        <asp:Label ID="lblFine" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td align="right" >
                    <div class="tbltxt cnt-box lbl2" style="background-color: #666;padding: 5px 10px; color: White;">
                        <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
