<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptDefaulters.aspx.cs" Inherits="Reports_rptDefaulters" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" language="javascript">



    function popUp(URL) {
        day = new Date();
        id = day.getTime();
        eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=500,left = 390,top = 184');");
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
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
                <div class="spacer" style="height:20px; ">
                    <img src="../images/mask.gif" height="8" width="10" />
                   
                </div> 
                <div class="cnt-box tbltxt">
                <asp:RadioButton ID="rbDetails" runat="server" Checked="True" GroupName="1" 
                        Text="Details" />
                    <asp:RadioButton ID="rbConsolidated" runat="server" GroupName="1" 
                        Text="Consolidated" />
                </div>
                <table width="100%" cellspacing="2" cellpadding="2" class="cnt-box2">
                    <tr>
                        <td class="tbltxt">
                            Session
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True"
                                CssClass="vsmalltb" TabIndex="1" 
                                onselectedindexchanged="drpSession_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td  class="tbltxt">
                            Class
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td  class="tbltxt">
                            <asp:DropDownList ID="drpclass" Width="70px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                                CssClass="vsmalltb" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt">
                            Section
                        </td>
                        <td class="tbltxt" width="5">
                            :
                        </td>
                        <td class="tbltxt">
                            <asp:DropDownList ID="ddlSection" runat="server" Width="40px" CssClass="vsmalltb" TabIndex="2">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="110" align="right">
                            Due Till Month
                        </td>
                        <td class="tbltxt" width="5">
                            :
                        </td>
                        <td class="tbltxt">
                            <asp:DropDownList ID="drpDueMnth" runat="server" Width="60px" CssClass="vsmalltb" ToolTip="Select to show the dues Till selected month "
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
                        <td class="tbltxt" align="right">
                         <%--   Fee Type--%>
                        </td>
                        <td class="tbltxt" width="5">
                          <%--  :--%>
                        </td>
                        <td class="tbltxt" width="90">
                            <asp:DropDownList ID="drpFeeType" runat="server" CssClass="vsmalltb" TabIndex="3" Visible="false">
                                <asp:ListItem Value="0">-All-</asp:ListItem>
                                <asp:ListItem Value="1">Regular Fee</asp:ListItem>
                                <asp:ListItem Value="2">Admn./Re-admn Fee</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        
                        </tr>
                        <tr>
                        <td rowspan="2" colspan="15" valign="bottom" class="tbltxt">
                            <asp:Button ID="btngo" runat="server" OnClick="btngo_Click" Text="Search" ToolTip="Click to Show Defaulter Students"
                                TabIndex="4" />&nbsp;<asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click"
                                    Text="Export to Excel" Visible="true" TabIndex="5" />
                            <asp:Button ID="btnPrint" Text="Print" runat="server" OnClick="btnPrint_Click1" TabIndex="6" />
                        </td>
                    </tr>
                </table>
                <div style="padding-top: 10px;">
                    <div id="divreport" style="height: 450px; overflow: scroll;" class="lbl2 cnt-box2">
                    <asp:Label ID="lblTotRec" runat="server"></asp:Label><br />
                        <asp:Label ID="lblReport" runat="server" ></asp:Label>
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
              <asp:PostBackTrigger ControlID="btnExpExcel"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


