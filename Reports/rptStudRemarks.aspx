<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptStudRemarks.aspx.cs" Inherits="Reports_rptStudRemarks" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script language="javascript" type="text/javascript">
     function isValid() {
         var name = document.getElementById("<%=drpSelectStudent.ClientID %>").value;


         if (name == 0) {
             alert("Please Select Student!");
             document.getElementById("<%=drpSelectStudent.ClientID %>").focus();
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
            Student Remarks</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
        
        
  <table width="100%" cellspacing="2" cellpadding="2" class="cnt-box">
        <tr>
            <td width="90" class="tbltxt">
                Session
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="130" class="tbltxt">
                <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                    OnSelectedIndexChanged="drpSession_SelectedIndexChanged" TabIndex="1">
                </asp:DropDownList>
            </td>
            <td class="tbltxt" width="70">
                Class
            </td>
            <td class="tbltxt" width="5">
                :
            </td>
            <td class="tbltxt" width="120">
                <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                    CssClass="vsmalltb" TabIndex="2">
                </asp:DropDownList>
            </td>
            <td width="50" class="tbltxt">
                Section
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="120" class="tbltxt">
                <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSection_SelectedIndexChanged"
                    CssClass="vsmalltb" TabIndex="3">
                </asp:DropDownList>
            </td>
            <td rowspan="3" valign="bottom" class="tbltxt">
              <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" 
                  OnClientClick="return isValid();"  TabIndex="7" />
                <asp:Button
                    ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                    Visible="False" TabIndex="8" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" 
                    TabIndex="9" />
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                Select Student
            </td>
            <td class="tbltxt">
                :
            </td>
            <td class="tbltxt">
                <asp:DropDownList ID="drpSelectStudent" runat="server" AutoPostBack="True" CssClass="smalltb"
                    OnSelectedIndexChanged="drpSelectStudent_SelectedIndexChanged" 
                    TabIndex="4">
                </asp:DropDownList>
            </td>
            <td class="tbltxt">
                From Date
            </td>
            <td class="tbltxt">
                :
            </td>
            <td class="tbltxt">
                <asp:TextBox ID="txtFromDate" runat="server" ReadOnly="True"  Width="100px"
                     TabIndex="5"></asp:TextBox>
                <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtFromDate" />
            </td>
            <td class="tbltxt">
                To Date
            </td>
            <td class="tbltxt">
                :
            </td>
            <td class="tbltxt">
                <asp:TextBox ID="txtToDate" runat="server" ReadOnly="True" Width="100px"
                    TabIndex="6"></asp:TextBox>
                <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtToDate" />
            </td>
        </tr>
    </table>
       <div style="padding-top:10px; color: #931f1f;font-size:12px;">Note: Leave <b>From Date</b> and <b>To Date</b> as empty to view records for current session. </div> 
    <div style="padding-top: 10px;">
      <asp:Label ID="lblReport" runat="server"></asp:Label>
    </div>
</asp:Content>



