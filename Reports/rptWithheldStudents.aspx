<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptWithheldStudents.aspx.cs" Inherits="Reports_rptWithheldStudents" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>
    
<asp:Content ID="contentRemarkReport" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
            <img src="../images/icon_rep.jpg" width="29" height="29"></div>
        <div style="padding-top: 5px;">
            <h2>
            Withheld Students</h2>
        </div>
 <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>


<table width="100%" border="0" cellspacing="2" cellpadding="2">
  <tr>
    <td class="tbltxt"><asp:Label ID="lblSession" runat="server" Text="Session"></asp:Label> 
    : 
                    <asp:DropDownList ID="drpSession" runat="server" 
            AutoPostBack="True"   CssClass="vsmalltb" 
            OnSelectedIndexChanged="drpSession_SelectedIndexChanged" TabIndex="1"></asp:DropDownList>
                     <asp:Label ID="lblClasses" runat="server" Text="Class" ></asp:Label> 
                     : 
                     <asp:DropDownList ID="drpClasses" runat="server" 
            AutoPostBack="True" CssClass="vsmalltb" 
            OnSelectedIndexChanged="drpClasses_SelectedIndexChanged" TabIndex="2"></asp:DropDownList>
                    <asp:Label ID="lblSection" runat="server" Text="Section" ></asp:Label> 
                    : 
                    <asp:DropDownList ID="drpSection" runat="server" 
            AutoPostBack="True"  CssClass="vsmalltb" 
            OnSelectedIndexChanged="drpSection_SelectedIndexChanged" TabIndex="3"></asp:DropDownList>
                    <asp:Button ID="btnShow" runat="server" Text="Show" 
            OnClick="btnShow_Click" TabIndex="4" />
                    <asp:Button ID="btnExpExcel" runat="server" 
            OnClick="btnExpExcel_Click" Text="Export to Excel" Visible="False" 
            TabIndex="5" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" 
            OnClick="btnPrint_Click" TabIndex="6" /></td>
  </tr>
  <tr>
    <td><asp:Label ID="lblReport" runat="server"></asp:Label></td>
  </tr>
</table>
</asp:Content>
