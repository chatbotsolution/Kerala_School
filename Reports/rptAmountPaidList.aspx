﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptAmountPaidList.aspx.cs" Inherits="Reports_rptAmountPaidList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/sms.css" rel="stylesheet" type="text/css" />
    <title>Student Fee Paid Details</title>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <table width="75%" cellpadding="0" cellspacing="0">
            <tr>
                <td class="tbltxt cnt-box" style="padding-left: 5px; background-color: #666; color: White;">
                    <div>
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
                    <fieldset id="Fieldset1" runat="server"  class="cnt-box lbl2">
                        <legend><b>School Fee Paid :</b></legend>
                        <asp:Label ID="lblSchoolFee" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset4" runat="server"  class="cnt-box lbl2">
                        <legend><b>Previous Year Due Paid :</b></legend>
                        <asp:Label ID="lblPrevDue" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
         <%--   <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset2" runat="server" class="cnt-box lbl2">
                        <legend><b>Fine Amount Paid :</b></legend>
                        <asp:Label ID="lblFine" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset3" runat="server" class="cnt-box lbl2">
                        <legend><b>Bus Fee Paid :</b></legend>
                        <asp:Label ID="lblBus" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset5" runat="server" class="cnt-box lbl2">
                        <legend><b>Book Fee Paid :</b></legend>
                        <asp:Label ID="lblHostel" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>--%>
            <tr>
                <td align="right" style="padding:0px 5px;">
                    <div class="tbltxt cnt-box lbl2" style="background-color: #666; color: White;">
                        <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>

