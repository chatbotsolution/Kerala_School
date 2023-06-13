<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>eVidyalaya Login</title>
    <link rel="stylesheet" href="css/layout.css" type="text/css">

    <script type="text/javascript" src="Scripts/time.js"></script>

</head>
<body onload="startTime()">
    <form id="form1" runat="server">
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
         <tr>
            <td height="60" valign="middle" class="topbar">
                 
                <div style="padding-top: 5px;">
                    <img src="images/logo-new.png" width="100" /><br />Kerala English Medium School</div>
            </td>
        </tr>
    </table>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="outertable" style="margin-bottom:66px;">
       
        <tr>
            <td align="center" valign="middle">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 100%; padding:60px 0 0;">
                    <tr>
                         
                        <td width="550" align="center" valign="middle">
                            <table width="500" border="0" cellspacing="0" cellpadding="0">
                                
                                
                                <tr>
                                    
                                    <td align="center" valign="middle" class="loginbox">
                                        <table width="330" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <div class="logintxt">
                                                        User ID</div>
                                                    <div>
                                                        <asp:TextBox ID="txtUID" runat="server" Width="300" CssClass="login-txt"></asp:TextBox>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div class="logintxt">
                                                        Password</div>
                                                    <div>
                                                        <asp:TextBox ID="txtPassword" runat="server" Width="300" TextMode="Password" CssClass="login-txt"></asp:TextBox>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" style="padding: 10px 30px 0px 0px;">
                                                    <%--<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="" AlternateText="Submit" OnClick="ImageButton1_Click" CssClass="btn" style="border:1px solid #84cff7; background:#2092d0;" />--%>
                                                    
                                                    <asp:Button ID="Button1" runat="server" Width="100%" Text="Submit"  CssClass="space" style=" border-width:5px !important;"
                                                        onclick="Button1_Click"/>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <br />
                                                    <asp:Label ID="lblMsg" runat="server" CssClass="alert" style="color:White" Text=" "
                                                       ><p style="color:White">Before Submit, please check the correctness of Date & Time and change accordingly.</p></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    
                                </tr>
                               
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
        <td align="center" class="whitetxtmedium">
            &nbsp;</td>
        </tr>
        
    </table>
    <table width="100%"  border="0" align="center" cellpadding="0" cellspacing="0" >
        <tr>
            <td valign="middle" class="footer">
            <div style="padding:10px;">
                <div style="width: 40%; float:left; padding-bottom:10px;">Vidyalaya 1.0 &copy; 2018 | All Rights Reserved</div>
                <div style="width: 40%; float:right; padding-bottom:10px; text-align:right">Powered by : <a href="http://www.xprosolutions.co.in/" target="_blank">XPRO</a>
                </div>
            </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
