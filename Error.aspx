<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Software Renewal</title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <p class="style1">
            &nbsp;</p>
        <p class="style1">
            &nbsp;</p>
        <p class="style1">
            &nbsp;</p>
        <p class="style1">
            &nbsp;</p>
        <p class="style2">
           <h1><strong>Oops!</strong></h1> </p>
        <p class="style2">
            <strong>SORRY&nbsp; FOR&nbsp; THE&nbsp; INCONVENIENCE </strong>
        </p>
        <p class="style2">
            <strong>License Of This Software Is Expired.</strong></p>
        <p class="style2">
            <strong>Please contact:Xpro Solutions Pvt.Ltd for Renewal of the 
            Software</strong></p>
            <p>
            <strong>Email ID: </strong>nayanlogin@gmail.com</p>
        <p>
            &nbsp;</p>
            <p>
            <strong>Enter Key: </strong> 
                <asp:TextBox ID="txtActKey" Width="500px"  runat="server"></asp:TextBox>
                <br />
             <br />
                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
             <br />
                <asp:Button ID="btnActivate" runat="server" Text="Activate New Key" 
                    onclick="btnActivate_Click" />
            </p>
            
    </div>
    </form>
</body>
</html>
