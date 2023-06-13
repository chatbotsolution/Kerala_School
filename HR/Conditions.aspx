<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="Conditions.aspx.cs" Inherits="HR_Conditions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" >
    function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
        var key;
        var isCtrl = false;
        var keychar;
        var reg;

        if (window.event) {
            key = e.keyCode;
            isCtrl = window.event.ctrlKey
        }
        else if (e.which) {
            key = e.which;
            isCtrl = e.ctrlKey;
        }

        if (isNaN(key)) return true;

        keychar = String.fromCharCode(key);

        // check for backspace or delete, or if Ctrl was pressed
        if (key == 8 || isCtrl) {
            return true;
        }

        reg = /\d/;
        var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
        var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

        return isFirstN || reg.test(keychar);
    }
</script>
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Set HR Conditions</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    
<table width="50%" border="1" style="border-collapse:collapse;">
<tr><td align="center" colspan="2">
    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label></td></tr>
    <tr><td><b>Condition</b></td><td><b>Value</b></td></tr>
     <tr> <td align="left" colspan="2">&nbsp;</td></tr>
<tr><td align="left"><b>No. of Extra working days for one EL to be Credited</b></td><td align="center">
    <asp:TextBox ID="txtEL" Width="50px" runat="server" 
        onkeypress="return blockNonNumbers(this, event, true, true);" TabIndex="1"></asp:TextBox></td></tr>
    <tr> <td align="left" colspan="2">&nbsp;</td></tr>
   <tr> <td align="left" colspan="2">
        <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" 
            TabIndex="2" /></tr>
</table>
</asp:Content>
