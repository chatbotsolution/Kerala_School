﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptMlyFeePaidStatus.aspx.cs" Inherits="Reports_rptMlyFeePaidStatus" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script language="javascript">
     function PrintContent() {

         var DocumentContainer = document.getElementById('<%=ViewRpt.ClientID%>');


         var WindowObject = window.open('', "TrackData",
                              "width=420,height=225,top=250,left=345,toolbars=no,scrollbars=no,status=no,resizable=no");

         WindowObject.document.write(DocumentContainer.innerHTML);
         WindowObject.document.close();
         WindowObject.focus();
         WindowObject.print();
         WindowObject.close();
     }
     function IsValid() {
         var Class = document.getElementById("<%=drpclass.ClientID %>").value;
         var Student = document.getElementById("<%=txtadminno.ClientID %>").value;
         if (Student == 0) {
             alert("Please Select Student !");
             document.getElementById("<%=txtadminno.ClientID %>").focus();
             return false;
         }
         else {
             return true;
         }
     }        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
 <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fee Paid Report Card
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="2" cellpadding="2" class="cnt-box">
                <tr>
                    <td width="60" class="tbltxt">
                        Session
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td width="180">
                        <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" TabIndex="1"
                            AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td width="80" class="tbltxt">
                        Class
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td width="100">
                        <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="true" CssClass="vsmalltb"
                            OnSelectedIndexChanged="drpclass_SelectedIndexChanged" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                    <td width="50" class="tbltxt">
                        Section
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpSection" runat="server" CssClass="vsmalltb" AutoPostBack="True"
                            OnSelectedIndexChanged="drpSection_SelectedIndexChanged" TabIndex="3">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Students
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpstudents" runat="server" CssClass="smalltb" AutoPostBack="True"
                            Width="170px" OnSelectedIndexChanged="drpstudents_SelectedIndexChanged" TabIndex="4">
                        </asp:DropDownList>
                    </td>
                    <td class="tbltxt">
                        Admission no.
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtadminno" runat="server" CssClass="vsmalltb" TabIndex="5"></asp:TextBox>
                    </td>
                    <td colspan="3" class="tbltxt">
                        <asp:Button ID="btngo" runat="server" OnClick="btngo_Click" Text="Show" OnClientClick="return IsValid();"
                            ToolTip="Go" TabIndex="6" />&nbsp;
                        <asp:Button ID="btnprint" runat="server" Text="Print" ToolTip="Print" OnClientClick="PrintContent();"
                            TabIndex="7" />
                    </td>
                </tr>
            </table>
            <div id="divreport">
                <table cellspacing="2" cellpadding="2" width="100%">
                    <tr id="trgrd" runat="server" visible="false">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="cnt-box2 tbltxt">
                        <div id="ViewRpt" runat="server">
                            <asp:Label ID="lblReport" runat="server"></asp:Label>
                            </div>
                            <asp:HiddenField ID="hfStudentRoll" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>




