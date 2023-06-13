<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptFeeBalance.aspx.cs" Inherits="Reports_rptFeeBalance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/sms.css" rel="stylesheet" type="text/css" />
    <title>Balance Amount</title>
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
                        <div style="width: 40%; float: right;">
                            <b>Admission No :</b>
                            <asp:Label ID="lblRegd" runat="server"></asp:Label></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset1" runat="server" class="cnt-box lbl2">
                        <legend><b>Balance in Regular Fees :</b></legend>
                        <asp:Label ID="lblBalReg" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset6" runat="server" class="cnt-box lbl2">
                        <legend><b>Balance in Previous Year Fees :</b></legend>
                        <asp:Label ID="lblPreviousFee" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
           <%-- <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset2" runat="server" class="cnt-box lbl2">
                        <legend><b>Balance in Misc Fees :</b></legend>
                        <asp:Label ID="lblBalMisc" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset3" runat="server" class="cnt-box lbl2">
                        <legend><b>Balance in Bus Fees :</b></legend>
                        <asp:Label ID="lblBalBus" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset8" runat="server" class="cnt-box lbl2">
                        <legend><b>Balance in Book Fees :</b></legend>
                        <asp:Label ID="lblBalBook" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset5" runat="server" class="cnt-box lbl2">
                        <legend><b>Fine Balance :</b></legend>
                        <asp:Label ID="lblFine" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset7" runat="server" class="cnt-box lbl2">
                        <legend><b>Advance Paid :</b></legend>
                        <asp:Label ID="lblAdvancePaid" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    <fieldset id="Fieldset4" runat="server" class="cnt-box lbl2">
                        <legend><b>Advance Fees :</b></legend>
                        <asp:Label ID="lblAdvance" runat="server"></asp:Label>
                    </fieldset>
                </td>
            </tr>--%>
            <tr>
                <td align="right" style="padding:0px 5px;">
                    <div class="tbltxt cnt-box" style="background-color: #666; color: White;">
                        <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
