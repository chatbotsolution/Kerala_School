<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Kerala English Medium School Dash Board</title>
    <link rel="stylesheet" href="css/layout.css" type="text/css">
    <link href="font-awesome-4.7.0/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="font-awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/time.js"></script>  
    
    <link href="date-time/datetime.css" rel="stylesheet" type="text/css" />
    <script src="date-time/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // Making 2 variable month and day
            var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            var dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

            // make single object
            var newDate = new Date();
            // make current time
            newDate.setDate(newDate.getDate());
            // setting date and time
            $('#Date').html(dayNames[newDate.getDay()] + " " + newDate.getDate() + ' ' + monthNames[newDate.getMonth()] + ' ' + newDate.getFullYear());

            setInterval(function () {
                // Create a newDate() object and extract the seconds of the current time on the visitor's
                var seconds = new Date().getSeconds();
                // Add a leading zero to seconds value
                $("#sec").html((seconds < 10 ? "0" : "") + seconds);
            }, 1000);

            setInterval(function () {
                // Create a newDate() object and extract the minutes of the current time on the visitor's
                var minutes = new Date().getMinutes();
                // Add a leading zero to the minutes value
                $("#min").html((minutes < 10 ? "0" : "") + minutes);
            }, 1000);

            setInterval(function () {
                // Create a newDate() object and extract the hours of the current time on the visitor's
                var hours = new Date().getHours();
                // Add a leading zero to the hours value
                $("#hours").html((hours < 10 ? "0" : "") + hours);
            }, 1000);
        });
</script>

</head>
<body onload="startTime()">
    <form id="form1" runat="server">
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" >
         <tr>
            <td height="60" valign="middle" class="topbar">
                 
                <div style="padding-top: 5px;">
                    <img src="images/logo-new.png" width="60" /><br />
                   <span id="kerala">Kerala English Medium School</span>
                    
                    
                    </div>
                    <div class="rght-side" style="position:absolute; top:16px; left:1015px">
                    <div class="clock">
                        <div id="Date"></div>
                        <ul>
                        <li id="hours"></li>
                        <li id="point">:</li>
                        <li id="min"></li>
                        <li id="point">:</li>
                        <li id="sec"></li>
                        </ul>
                    </div>

                </div>
                      
               
            </td>
        </tr>
    </table>
    <table width="90%" border="0" align="center" cellpadding="0" cellspacing="0" class="outertable outer-btm">
        
        <tr>
            <td class="innersecbar" valign="middle">

                <div class="innersecbar_lft">
                    <%--<asp:ImageButton ID="imgBtnDashboard" runat="server" 
                        ImageUrl="" CausesValidation="false"
                        OnClick="imgBtnDashboard_Click" AlternateText=" Dashboard"  
                        CssClass="fa fa-dashboard space" ImageAlign="TextTop"/>--%>
                    <asp:LinkButton ID="lbDashboard" runat="server" Font-Underline="false" 
                        onclick="lbDashboard_Click"  CssClass="fa fa-dashboard space"> Dashboard</asp:LinkButton>

                </div>
                <div class="innersecbar_mdl">
                    <div style="text-align: center; margin-top: 15px;" class="logininf">
                        Welcome  <span class="logininfhl">
                            <asp:Label ID="lblUser" runat="server"></asp:Label></span></div>
                </div>
                <div class="innersecbar_lft">
                        
                    <div style=" float: right; margin-left: 10px;">
                     <%--<asp:ImageButton ID="imgBtnChangePW" runat="server" AlternateText=" Change Password" ImageUrl="" CausesValidation="false"
                        OnClick="imgBtnChangePW_Click"  CssClass="fa fa-key space" />
                        <asp:ImageButton ID="imgBtnLogout" runat="server" ImageUrl="" AlternateText=" Logout" OnClick="imgBtnLogout_Click" CssClass="fa fa-sign-out space" />--%>
                        <asp:LinkButton ID="lbChangePw" runat="server" CssClass="fa fa-key space" 
                            Font-Underline="false" onclick="lbChangePw_Click">Change Password</asp:LinkButton>
                        <asp:LinkButton ID="lbLogOut" runat="server" CssClass="fa fa-sign-out space" 
                            Font-Underline="false" onclick="lbLogOut_Click"> Logout</asp:LinkButton>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
        <td class="content">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="height: 100%;">
                <tr>
                    <td valign="top">
                       <%-- <div style="display: inline;">
                            <div style="width: 42px; height: 41px; float: left; margin-right: 5px;">
                                <img src="images/icon.png" width="42" height="42"></div>
                            <div style="padding-top: 10px;">
                                <h1>
                                    Dashboard</h1>
                            </div>
                        </div>--%>
                         
                        <div id="triggers" align="left">
                            <table width="100%" border="0" cellpadding="20" cellspacing="20">
                                <tr>
                                    <td width="125" align="center" class="cpiconimg">
                                        <a href="Administrations/AdminHome.aspx">
                                        <div class="cpicon">                                            
                                                <img src="images/administration.png" width="65" height="65">
                                            
                                        </div>
                                        <div class="cpicontxt">Administration</div>
                                        </a>
                                    </td>
                                    <td width="125" align="center" class="cpiconimg">
                                        
                                            <a href="Masters/MastersHome.aspx">
                                            <div class="cpicon">
                                                <img src="images/mastent.png" width="65" height="65"></div>
                                        <div class="cpicontxt">
                                            Master Entry</div></a>
                                    </td>
                                    <td width="125" align="center" class="cpiconimg">
                                        <a href="Admissions/Home.aspx">
                                        <div class="cpicon">
                                            <img src="images/addmission.png" width="65" height="65"></div>
                                        <div class="cpicontxt">
                                            Admission </div></a>
                                    </td>
                                    <td width="125" align="center" class="cpiconimg">
                                        <a href="FeeManagement/FeeHome.aspx">
                                        <div class="cpicon">
                                           <img src="images/accounts.png" width="65" height="65"></div>
                                        <div class="cpicontxt">
                                            Fee Management<div></a>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td align="center" class="cpiconimg">
                                          <a href="Attendance/AttendHome.aspx">
                                          <div class="cpicon">
                                              <img src="images/classsheduling.png" width="65" height="65"></div>
                                        <div class="cpicontxt">
                                            Attendance</div></a>
                                    </td>
                                    <td align="center" class="cpiconimg">
                                        <a href="Library/Home.aspx"><div class="cpicon">
                                            
                                                <img src="images/library.png" width="65" height="65"></div>
                                        <div class="cpicontxt">
                                            Library</div></a>
                                    </td>
                                      <td align="center" class="cpiconimg">
                                         <a href="Administrations/BackUp.aspx"> <div class="cpicon">
                                           
                                             <img src="images/DB_Backup.png" width="65" height="65">
                                            
                                            
                                        </div>
                                         <div class="cpicontxt">
                                              Data Backup </div></a>
                                    </td>
                                     <td align="center" class="cpiconimg">

                                      <a href="Accounts/SalesAndAccountHome.aspx"><div class="cpicon">
                                            
                                                <img src="images/hostel.png" width="65" height="65"></div>
                                        <div class="cpicontxt">
                                             Accounts</div></a>
                                       <%-- <a href="Hostel/HostelHome.aspx"><div class="cpicon">
                                            
                                                <img src="images/hostel.png" width="65" height="65"></div>
                                        <div class="cpicontxt">
                                             Hostel</div></a>--%>
                                    </td>
                                   <%-- <td align="center" class="cpiconimg">
                                         <a href="#"><div onmouseover="reveal('ClassSheduling');"  class="cpicon">
                                           
                                                    <img src="images/hr-payroll.PNG" width="65" height="65"></div>
                                            <div class="cpicontxt">
                                                HR & Payroll</div></a>
                                    </td>--%>
                                 <%--  <td align="center" class="cpiconimg">
                                     <a href="#"><div class="cpicon"> 
                                    <%-- <a href="Inventory/Home.aspx"><div class="cpicon">
                                            
                                                <img src="images/invt.png" width="65" height="65"></div>
                                        <div class="cpicontxt">
                                            Inventory</div></a>
                                            
                                       
                                    </td>--%>
                                    
                                   
                                </tr>
                                <tr>
                                                                        
                                    <td align="center" class="cpiconimg">
                                        <a href="Exam/ExamHome.aspx"><div onmouseover="reveal('Scholarship');" class="cpicon">
                                            
                                                <img src="images/exam.png" width="65" height="65"></div>
                                        <div class="cpicontxt">
                                            Exam & Result</div></a>
                                    </td>
                                    <td align="center" class="cpiconimg">
                                            <a href="#"><div  class="cpicon">
                                               
                                               <%-- <a href="#">--%>
                                                    <%--<img src="images/settings.png" width="65" height="65">--%>
                                                    <img src="images/sms2.png" width="65" height="65">
                                                    
                                                    </div>
                                            <div class="cpicontxt">
                                                Send SMS</div></a>
                                  </td>
                                   <%-- <td align="center" class="cpiconimg">
                                         <a href="Administrations/BackUp.aspx"> <div class="cpicon">
                                           
                                             <img src="images/DB_Backup.png" width="65" height="65">
                                            
                                            
                                        </div>
                                         <div class="cpicontxt">
                                              Data Backup </div></a>
                                    </td>--%>
                                    <td align="center" class="cpiconimg">

                                    <%--  <a href="Accounts/OpenFYList.aspx"><div class="cpicon">
                                            
                                                <img src="images/hostel.png" width="65" height="65"></div>
                                        <div class="cpicontxt">
                                             Accounts</div></a>--%>
                                    <a href="Hostel/HostelHome.aspx"><div class="cpicon">
                                            
                                                <img src="images/hostel.png" width="65" height="65"></div>
                                        <div class="cpicontxt">
                                             Hostel</div></a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    
                </tr>
            </table>
        </td>
        </tr>
    </table>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" style="height: 100%;">
         <tr>
            <td valign="middle" class="footer">
            <div style="padding:10px;">
                <div style="width: 40%; float:left; padding-bottom:10px;">Vidyalaya 1.0 &copy; 2020 | All Rights Reserved</div>
                <div style="width: 40%; float:right; padding-bottom:10px; text-align:right">Powered by : <a href="http://www.xprosolutions.co.in/" target="_blank">XPRO</a>
                </div>
            </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
