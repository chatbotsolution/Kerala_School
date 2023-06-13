<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptstudentdetailPrint.aspx.cs" Inherits="Reports_rptstudentdetailPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Detail</title>

    <script>
        function Print() { document.body.offsetHeight; window.print();  }
			
    </script>

<%--    <style media="screen, print">
        @page
        {
            page-break-after: always;
        }
        body
        {
            margin: 1pt;
        }
        table
        {
            /*border:1px solid #CCC;*/
            border-collapse: collapse;
            font-family: Arial;
        }
        td
        {
            border: 1px solid #CCC;
            border-collapse: collapse;
            padding: 3px;
            font-family: Arial;
            page-break-after: right;
        }
        .tblheader
        {
            color: #000;
            font-family: Arial;
            font-size: 12pt;
            font-weight: bold;
            padding: 2px;
            letter-spacing: 1px;
        }
        .tbltd
        {
            color: #000;
            font-family: Arial;
            font-size: 10pt;
            background-color: #FFF;
            letter-spacing: 1px;
        }
    </style>--%>
</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <div style="width: 250px; float: left;">
                    <asp:Label ID="Label4" runat="server" Text="Student Detail" Font-Bold="True"></asp:Label></div>
                <div style="width: 200px; float: right; text-align: right;">
                    <%--<asp:Label ID="lblPrintDate" runat="server" Text="Label"></asp:Label>--%></div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblReport" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:DataList ID="liststudent" runat="server" BorderWidth="0" Width="100%">
        <ItemTemplate>
             <table width="100%" border="0" cellspacing="1" cellpadding="1" bgcolor="#CCC">
                            <tr>
                               <td colspan="2" width="150" class="tblheader">
                                    Personal Details
                                </td>
                                <td colspan="3" width="150" class="tblheader">
                                    <b>To whomsoever it may concern</b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" width="150" class="tbltd">
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Name :
                                </td>
                                <td class="tbltd" width="150">
                                    <%#Eval("fullname")%>
                                </td>
                                <td  class="tbltd">
                                    Nick Name:
                                </td>
                                <td class="tbltd" width="150">
                                    <%#Eval("nickname")%>
                                </td>
                                <td class="tbltd" rowspan="5" align="left" valign="top">
                                    <img src='../Up_Files/Studimage/<%#Eval("StudentPhoto")%>' alt='<%#Eval("FullName")%>'
                                        width="100px" height="130px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Father's Name :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("fathername")%>
                                </td>
                                <td class="tbltd">
                                    Mother's Name:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("mothername")%>
                                </td>
                                
                               
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Father's Occupation :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("fatheroccupation")%>
                                </td>
                                <td class="tbltd">
                                    Mother's Occupation:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("motheroccupation")%>
                                </td>
                                 
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Local Guardian Name :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("LocalGuardianName")%>
                                </td>
                                <td class="tbltd">
                                    Relation With Local Guardian :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("RelationWithLG")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Present Address:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PresAddr1")%>
                                </td>
                                <td class="tbltd">
                                    Permanent Address :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PermAddr1")%>
                                </td>
                                
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Present Dist :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PresAddrDist")%>
                                </td>
                                <td class="tbltd">
                                    Permanent Dist:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PermAddrDist")%>
                                </td>
                                
                                <td class="tbltd">
                                    Admn No :
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Pin :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PresAddrPin")%>
                                </td>
                                <td class="tbltd">
                                    Pin :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PermAddrPin")%>
                                </td>
                                
                                 <td class="tbltd">
                                    <%#Eval("AdmnNo")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Category :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("CatName")%>
                                </td>
                                <td class="tbltd">
                                    Nationality:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("nationality")%>
                                </td>
                                 <td class="tbltd">
                                    Admn No as per Register:
                                </td>
                                
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Home Phone :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("telnoresidence")%>
                                </td>
                                <td class="tbltd">
                                    Work Phone :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("telenooffice")%>
                                </td>
                                 <td class="tbltd">
                                    <%#Eval("OldAdmnNo")%>
                                </td>
                            </tr>
           
                <tr>
                    <td class="tbltd">
                        Date Of Birth :
                    </td>
                    <td class="tbltd">
                        <%#Eval("dateofbirth")%>
                    </td>
                    <td class="tbltd">
                        Gender :
                    </td>
                    <td class="tbltd">
                        <%#Eval("sex")%>
                    </td>
                </tr>
                <tr>
                    <td class="tbltd">
                        Religion :
                    </td>
                    <td class="tbltd">
                        <%#Eval("religion")%>
                    </td>
                    <td class="tbltd">
                        Nationality :
                    </td>
                    <td class="tbltd">
                        <%#Eval("Nationality")%>
                    </td>
                </tr>
                <tr>
                    <td class="tbltd">
                        Locality :
                    </td>
                    <td class="tbltd">
                        <%#Eval("Locality")%>
                    </td>
                    <td class="tbltd">
                        Mother Tongue :
                    </td>
                    <td class="tbltd">
                        <%#Eval("MotherTongue")%>
                    </td>
                </tr>
                <tr>
                    <td height="10" colspan="5" bgcolor="#FFFFFF">
                        <img src="../images/mask.gif" height="8" width="10" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6" class="tblheader">
                        Previous school Details
                    </td>
                </tr>
                <tr>
                    <td class="tbltd">
                        Previous School Name :
                    </td>
                    <td class="tbltd">
                        <%#Eval("prevschoolname")%>
                    </td>
                    <td class="tbltd">
                        Previous Class:
                    </td>
                    <td class="tbltd">
                        <%#Eval("prevclassname")%>
                    </td>
                    <td class="tbltd">
                    </td>
                </tr>
                <tr>
                    <td height="10" colspan="5" bgcolor="#FFFFFF">
                        <img src="../images/mask.gif" height="8" width="10" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="tblheader">
                        Class Details
                    </td>
                </tr>
                <tr>
                    <td class="tbltd">
                        Session :
                    </td>
                    <td class="tbltd">
                        <%#Eval("sessionyear")%>
                    </td>
                    <td class="tbltd">
                        Class:
                    </td>
                    <td class="tbltd">
                        <%#Eval("currentclass")%>
                    </td>
                    <td class="tbltd">
                    </td>
                </tr>
                <tr>
                    <td class="tbltd">
                        Join Date :
                    </td>
                    <td class="tbltd">
                        <%#Eval("joindate")%>
                    </td>
                    <td class="tbltd">
                        Section:
                    </td>
                    <td class="tbltd">
                        <%#Eval("section")%>
                    </td>
                    <td class="tbltd">
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:DataList>
    </form>
</body>
</html>
