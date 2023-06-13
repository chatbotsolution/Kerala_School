<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="rptHostFeeDefaulters.aspx.cs" Inherits="Hostel_rptHostFeeDefaulters" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <script type="text/javascript" language="javascript">



     function popUp(URL) {
         day = new Date();
         id = day.getTime();
         eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=500,left = 390,top = 184');");
     }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlSummary" runat="server">
                <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                    <img src="../images/icon_rep.jpg" width="29" height="29">
                </div>
                <div style="padding-top: 5px;">
                    <h2>
                        Fee Defaulters Summary
                    </h2>
                </div>
                <table width="100%" cellspacing="2" cellpadding="2">
                    <tr>
                     <td class="tbltxt" style="width: 55px">
                        Session
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td style="width: 100px">
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                           TabIndex="1">
                        </asp:DropDownList>
                    </td>
                        <td width="70" class="tbltxt">
                            Select Class
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="110" class="tbltxt">
                            <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                                CssClass="smalltb" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="80">
                            Select Section
                        </td>
                        <td class="tbltxt" width="5">
                            :
                        </td>
                        <td class="tbltxt" width="90">
                            <asp:DropDownList ID="ddlSection" runat="server" CssClass="vsmalltb" TabIndex="2">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="70" align="right">
                            Fee Type
                        </td>
                        <td class="tbltxt" width="5">
                            :
                        </td>
                        <td class="tbltxt" width="90">
                            <asp:DropDownList ID="drpFeeType" runat="server" CssClass="vsmalltb" TabIndex="3">
                                <asp:ListItem Value="0">-All-</asp:ListItem>
                                <asp:ListItem Value="1">Regular Fee</asp:ListItem>
                                <asp:ListItem Value="2">Admn./Re-admn Fee</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="110" align="right">
                            Due Till Month
                        </td>
                        <td class="tbltxt" width="5">
                            :
                        </td>
                        <td class="tbltxt" width="90">
                            <asp:DropDownList ID="drpDueMnth" runat="server" CssClass="vsmalltb" ToolTip="Select to show the dues Till selected month "
                                TabIndex="3" AutoPostBack="True" 
                                onselectedindexchanged="drpDueMnth_SelectedIndexChanged">
                                <asp:ListItem Value="0">-All-</asp:ListItem>
                                <asp:ListItem Value="4">Apr</asp:ListItem>
                                <asp:ListItem Value="5">May</asp:ListItem>
                                <asp:ListItem Value="6">June</asp:ListItem>
                                <asp:ListItem Value="7">July</asp:ListItem>
                                <asp:ListItem Value="8">Aug</asp:ListItem>
                                <asp:ListItem Value="9">Sep</asp:ListItem>
                                <asp:ListItem Value="10">Oct</asp:ListItem>
                                <asp:ListItem Value="11">Nov</asp:ListItem>
                                <asp:ListItem Value="12">Dec</asp:ListItem>
                                <asp:ListItem Value="1">Jan</asp:ListItem>
                                <asp:ListItem Value="2">Feb</asp:ListItem>
                                <asp:ListItem Value="3">Mar</asp:ListItem>
                            </asp:DropDownList>
                        </td>
</tr>
<tr>
                        <td rowspan="2" colspan="15" valign="bottom" class="tbltxt">
                            <asp:Button ID="btngo" runat="server" OnClick="btngo_Click" Text="Search" ToolTip="Click to Show Defaulter Students"
                                TabIndex="4" /><asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click"
                                    Text="Export to Excel" Visible="false" TabIndex="5" />
                            <asp:Button ID="btnPrint" Text="Print" runat="server" OnClick="btnPrint_Click1" TabIndex="6" />
                        </td>
                    </tr>
                </table>
                <div style="padding-top: 10px;">
                    <div id="divreport">
                        <asp:Label ID="lblReport" runat="server"></asp:Label>
                    </div>
                    <asp:Label ID="lblGrdMsg" runat="server" CssClass="error"></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlDetail" runat="server" Visible="false">
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" TabIndex="7" />
                            <asp:Button ID="btnPrintDetails" runat="server" Visible="true" Text="Print" OnClientClick="return printDetailContent();"
                                TabIndex="8" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btngo"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

