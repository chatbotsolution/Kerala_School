<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="protected void Page_Load(object sender, EventArgs e).aspx.cs" Inherits="Reports_protected_void_Page_Load_object_sender__EventArgs_e_" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function valShow() {
            var session = document.getElementById("<%=drpclass.ClientID %>").value;
            if (session == "0") {
                alert("Please select a class !");
                document.getElementById("<%=drpclass.ClientID %>").focus;
                return false;
            }
            else {
                return true;
            }
        }
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=500,left = 390,top = 184');");
        } 
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Student Fee Status
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" border="0" cellspacing="2" cellpadding="2">
        <tr>
            <td width="180" class="tbltxt">
                <asp:RadioButton ID="rbtnTillDt" CssClass="tbltxt" runat="server" Text="Till Date"
                    GroupName="s" AutoPostBack="True" Checked="true" OnCheckedChanged="rbtnTillDt_CheckedChanged">
                </asp:RadioButton>
                <asp:RadioButton ID="rbtnFullSess" CssClass="tbltxt" runat="server" Text="Full Session"
                    AutoPostBack="True" GroupName="s" OnCheckedChanged="rbtnTillDt_CheckedChanged">
                </asp:RadioButton>
            </td>
            <td width="50" class="tbltxt">
                Class
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="120">
                <asp:DropDownList ID="drpclass" runat="server" CssClass="vsmalltb" Height="16px">
                </asp:DropDownList>
            </td>
            <td colspan="3">
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                    OnClientClick="return valShow();" style="height: 26px" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
                <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                    Width="106px" />
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td valign="top">
                <div id="divreport">
                    <table cellspacing="2" cellpadding="2" width="100%">
                        <tr id="trgrd" runat="server" visible="false">
                            <td valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblReport" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div style="display: none" id="divhdr">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label4" runat="server" Text="Fee Received" Font-Bold="True" Font-Underline="True"></asp:Label>
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
</asp:Content>

