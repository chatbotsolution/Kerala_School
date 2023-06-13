<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="AddFeeDefaulters.aspx.cs" Inherits="Reports_AddFeeDefaulters" %>

<asp:Content ID="contentRemarkReport" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">
    <asp:Panel ID="pnlSummary" runat="server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Additional Fee Defaulters </h2>
    </div>
  <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
  
  
  <table width="100%" cellspacing="2" cellpadding="2">
        <tr>
            <td width="70" class="tbltxt">Select Class</td>
            <td width="5" class="tbltxt">:</td>
            <td width="110" class="tbltxt">
<asp:DropDownList ID="drpclass" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="drpclass_SelectedIndexChanged" CssClass="vsmalltb" 
                    TabIndex="1">
</asp:DropDownList>                            </td>
            <td class="tbltxt" width="80">Select Section</td>
            <td class="tbltxt" width="5">:</td>
            <td class="tbltxt" width="120">
              <asp:DropDownList ID="ddlSection" runat="server" CssClass="smalltb" TabIndex="2"></asp:DropDownList></td>
            <td rowspan="2" valign="bottom" class="tbltxt"><asp:Button ID="btngo" 
                    runat="server" OnClick="btngo_Click" Text="Search" 
                    ToolTip="Click to Search Student Details" TabIndex="3" /> 
                                <asp:Button ID="btnExpExcel" runat="server" 
                    OnClick="btnExpExcel_Click" Text="Export to Excel"
                                    Visible="false" TabIndex="4" />
            <asp:Button ID="btnPrint" Text="Print" runat="server" OnClick="btnPrint_Click1" 
                    TabIndex="5" /></td>
        </tr>
      </table>
      <div style="padding-top:10px;"><asp:Label ID="lblreport" runat="server"></asp:Label> </div>
      <div> <asp:Label ID="lblGrdMsg" runat="server" ForeColor="Red"></asp:Label></div>
  </asp:Panel>
</asp:Content>
