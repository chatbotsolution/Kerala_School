<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptRecieptCashNo.aspx.cs" Inherits="Reports_rptRecieptCashNo" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" language="javascript">

    function isValid() {

        var session = document.getElementById("<%=drpSession.ClientID%>").value;
        var Classes = document.getElementById("<%=drpClasses.ClientID%>").value;
        var section = document.getElementById("<%=ddlSection.ClientID%>").value;
        var student = document.getElementById("<%=drpstudent.ClientID%>").value;
        var quarter = document.getElementById("<%=drpQuarter.ClientID%>").value;
        if (session == "" || session == "0") {
            alert("Please select a Session !");
            document.getElementById("<%=drpSession.ClientID%>").focus();
            return false;
        }
        if (Classes == "" || Classes == "0") {
            alert("Please select a Class !");
            document.getElementById("<%=drpClasses.ClientID%>").focus();
            return false;
        }
        if (section == "" || section == "0") {
            alert("Please select a Section !");
            document.getElementById("<%=ddlSection.ClientID%>").focus();
            return false;
        }
        if (student == "" || student == "0") {
           alert("Please select a student !");
           document.getElementById("<%=drpstudent.ClientID%>").focus();
           return false;
        }
       if (quarter == "" || quarter == "--Select--") {
           alert("Please select a Quarter !");
           document.getElementById("<%=drpQuarter.ClientID%>").focus();
           return false;
       }
   }

   function printcontent() {

       var DocumentContainer = document.getElementById('divreport');

       var documentheader = document.getElementById('divhdr');
       var WindowObject = window.open('', "TrackData",
                             "width=800,height=600,top=20,left=20,toolbars=no,scrollbars=no,status=no,resizable=yes");
       WindowObject.document.write(documentheader.innerHTML + "\n" + DocumentContainer.innerHTML);
       WindowObject.document.close();
       WindowObject.focus();
       WindowObject.print();
       WindowObject.close();
       return false;
   }
   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Duplicate Receipt </h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <fieldset id="fsStudDetails" runat="server" class="cnt-box2">
                <legend  id="lgprchs" class="tbltxt" runat="server">
                    Student Details :-</legend>
                <table border="0" cellspacing="2" cellpadding="2" width="100%">
                    <tr>
                        <td width="60" class="tbltxt">
                            Session
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td style="width: 100px">
                            <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" 
                                CssClass="vsmalltb" TabIndex="1"  
                                onselectedindexchanged="drpSession_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" style="width: 34px">
                            Class
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="120">
                            <asp:DropDownList ID="drpClasses" runat="server" AutoPostBack="True"  CssClass="vsmalltb"
                                 TabIndex="2" onselectedindexchanged="drpClasses_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>

                           <td class="tbltxt" style="width: 34px">
                            Section
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="120">
                            <asp:DropDownList ID="ddlSection" runat="server" AutoPostBack="True" CssClass="vsmalltb" 
                                 TabIndex="3" onselectedindexchanged="ddlSection_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                       
                        <td class="tbltxt" style="width: 44px">
                            Student
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpstudent" runat="server"  CssClass="largetb"
                                 TabIndex="4">
                            </asp:DropDownList>
                        </td>
                        
                    </tr>
                    <tr>
                    <td class="tbltxt" style="width: 69px">
                             Quarter
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td>
                           <asp:DropDownList ID="drpQuarter" runat="server" CssClass="vsmalltb" 
                                Height="16px"  TabIndex="5" >
                    <asp:ListItem Value="--Select--">--Select--</asp:ListItem>
                    <asp:ListItem Value="APR-JUN">FIRST</asp:ListItem>
                    <asp:ListItem Value="JUL-SEP">SECOND</asp:ListItem>
                    <asp:ListItem Value="OCT-DEC">THIRD</asp:ListItem>
                    <asp:ListItem Value="JAN-MAR">FOURTH</asp:ListItem>
                  
                </asp:DropDownList>
                        </td>
                        
                          <td class="tbltxt" colspan="9" >
                           
                        <asp:Button ID="btnShow" runat="server" Text="Show"  OnClientClick="return isValid();"
                                TabIndex="6" onclick="btnShow_Click" />&nbsp;
                                    <asp:Button ID="btnPrint" runat="server" Text="Print" TabIndex="7" Visible="false"
                                style="height: 26px" onclick="btnPrint_Click" OnClientClick="printcontent();"/>
                        </td>
                     

                    </tr>
                </table>
            </fieldset>

            <div align="center" class="cnt-box2"  >
                <asp:Label ID="lbldetail" runat="server"></asp:Label></div>
                <table width="100%" class="tbltxt">
        <tr>
            <td valign="top">
                <div id="divreport">
                    <table cellspacing="0" cellpadding="0" width="100%">
                        <tr id="trgrd" runat="server" visible="false">
                            <td valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl2">
                                <asp:Label ID="lblReport" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div style="display: none" id="divhdr">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label4" runat="server" Text="Fee Receipt" Font-Bold="True" Font-Underline="True"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblPrintDate" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="drpstudent" EventName="SelectedIndexChanged">
            </asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

