<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="ICSEExamRegistration.aspx.cs" Inherits="Reports_ICSEExamRegistration" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type = "text/javascript">
     function PrintPanel() {
         debugger;
         var panel = document.getElementById("<%=lblReport.ClientID %>");
         var printWindow = window.open('', '', 'height=650,width=1000');
         printWindow.document.write('<html><head><title>DIV Contents</title>');
         printWindow.document.write('</head><body >');
         printWindow.document.write(panel.innerHTML);
         printWindow.document.write('</body></html>');
         printWindow.document.close();
         setTimeout(function () {
             printWindow.print();
         }, 500);
         return false;
     }
    </script>
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
           ICSE Exam Notice
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
        <table width="100%" border="0" cellspacing="2" cellpadding="2">
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
                        OnSelectedIndexChanged="drpclass_SelectedIndexChanged" TabIndex="2"  Enabled="False">
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
                 <td class="tbltxt">
                    Students
                </td>
                <td class="tbltxt">
                    :
                </td>
                <td>
                    <asp:DropDownList ID="drpstudents" runat="server" CssClass="smalltb" AutoPostBack="True"
                        Width="137px" OnSelectedIndexChanged="drpstudents_SelectedIndexChanged" TabIndex="4">
                    </asp:DropDownList>
                </td>
              
            </tr>
            
            <tr>
            <td width="60" class="tbltxt">
                   Submission Date
                </td>
                <td width="5" class="tbltxt">
                    :
                </td>
                <td>
                <asp:TextBox ID="txtDt" runat="server"  ></asp:TextBox>
                <rjs:PopCalendar ID="dtpDt" runat="server" Control="txtDt" />
                </td>
                
            <td colspan="6" class="tbltxt">
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" TabIndex="4"
                        OnClientClick="return isValid();" />
                    &nbsp;<asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick=" return PrintPanel();" />
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                 </td>
            </tr>
        </table>

</asp:Content>


