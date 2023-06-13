<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptFeeReceivedAll.aspx.cs" Inherits="Reports_rptFeeReceivedAll" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fee Received
        Details</h2>
    </div>
   <div><img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>


<table width="100%" border="0" cellspacing="2" cellpadding="2">
  <tr>
    <td width="130" class="tbltxt">Fee Collection Counter</td>
    <td width="5" class="tbltxt">:</td>
    <td width="130"><asp:DropDownList ID="drpFeeCounter" runat="server" 
            CssClass="smalltb" AutoPostBack="True"
                        OnSelectedIndexChanged="drpFeeCounter_SelectedIndexChanged" 
            TabIndex="1">
        </asp:DropDownList></td>
    <td width="60" class="tbltxt">From Date</td>
    <td width="5" class="tbltxt">:</td>
    <td width="120"><asp:TextBox
                        ID="txtFromDate" runat="server" CssClass="vsmalltb" 
            TabIndex="2"></asp:TextBox>&nbsp;<rjs:PopCalendar
                            ID="dtpFromDate" runat="server" Control="txtFromDate" /></td>
    <td width="50" class="tbltxt">To Date</td>
    <td width="5" class="tbltxt">:</td>
    <td width="120">
        <asp:TextBox
                        ID="txtToDate" runat="server" CssClass="vsmalltb" TabIndex="3"></asp:TextBox>
        <rjs:PopCalendar ID="dtpToDate" runat="server" Control="txtToDate" /></td>
    <td>Payment Mode :
        <asp:DropDownList ID="drpPmtMode" runat="server" CssClass="vsmalltb" 
            onclick="return ViewChequeDetails();" TabIndex="4">
            <asp:ListItem>Cash</asp:ListItem>
            <asp:ListItem>Bank</asp:ListItem>
            <asp:ListItem>Cheque</asp:ListItem>
        </asp:DropDownList>
      </td>
    </tr>
  <tr>
    <td class="tbltxt">Class</td>
    <td class="tbltxt">:</td>
    <td>
        <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" 
            CssClass="vsmalltb" OnSelectedIndexChanged="drpClass_SelectedIndexChanged" 
            TabIndex="5">
        </asp:DropDownList>
      </td>
    <td class="tbltxt">Section</td>
    <td class="tbltxt">:</td>
    <td>
        <asp:DropDownList ID="drpSection" runat="server" CssClass="vsmalltb" 
            OnSelectedIndexChanged="drpSection_SelectedIndexChanged1" TabIndex="6" 
            AutoPostBack="True">
        </asp:DropDownList>
      </td>
    <td class="tbltxt">Student</td>
    <td class="tbltxt">:</td>
    <td class="tbltxt">
        <asp:DropDownList ID="drpstudents" runat="server" AutoPostBack="True" 
            CssClass="smalltb" OnSelectedIndexChanged="drpstudents_SelectedIndexChanged" 
            TabIndex="4">
        </asp:DropDownList>
      </td>
    <td class="tbltxt">
        <asp:TextBox ID="txtadminno" runat="server" AutoPostBack="True" 
            CssClass="vsmalltb" OnTextChanged="txtadminno_TextChanged" TabIndex="5"></asp:TextBox>
        &nbsp;<asp:Button ID="btnShow" runat="server" Text="Show" 
            OnClick="btnShow_Click" TabIndex="7" /> </td>
    </tr>
    <tr>
        <td colspan="8" align="left">
            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="8" />
            <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel" TabIndex="9" />
        </td>
        <td  colspan="2" >
        
        </td>
    </tr>
 </table>
</div>
<asp:Label ID="lblReport" runat="server"></asp:Label>
  </ContentTemplate>
    <Triggers>
    	<asp:PostBackTrigger ControlID="btnExpExcel" />
    </Triggers>
    </asp:UpdatePanel>
</asp:Content>
