<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptFeeReceived.aspx.cs" Inherits="Reports_rptFeeReceived" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var Session = document.getElementById("<%=drpSession.ClientID %>").value;
            var Class = document.getElementById("<%=drpclass.ClientID %>").value;
            var Student = document.getElementById("<%=drpstudents.ClientID %>").value;

            var FrDt = document.getElementById("<%=txtFmdate.ClientID%>").value;
            var ToDt = document.getElementById("<%=txtToDate.ClientID%>").value;

            var currentdate = new Date();
            var currday = currentdate.getDate();

            if (Session == "0") {
                alert("Please Select Session !");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            if (Class == "0") {
                alert("Please Select Class !");
                document.getElementById("<%=drpclass.ClientID %>").focus();
                return false;
            }
            if (Student == "0") {
                alert("Please Select Student !");
                document.getElementById("<%=drpstudents.ClientID %>").focus();
                return false;
            }
            if (Date.parse(FrDt.trim()) > Date.parse(ToDt.trim())) {
                alert("Please check date range! From Date can't be greater than To date!")
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fee Received
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
            <td width="130">
                <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" AutoPostBack="true"
                    TabIndex="1" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td width="80" class="tbltxt">
                Class
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="120">
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
                    OnSelectedIndexChanged="drpstudents_SelectedIndexChanged" TabIndex="4">
                </asp:DropDownList>
            </td>
            <td class="tbltxt">
                Admission no.
            </td>
            <td class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtadminno" runat="server" CssClass="vsmalltb" AutoPostBack="True"
                    OnTextChanged="txtadminno_TextChanged" TabIndex="5"></asp:TextBox>
            </td>
            <td colspan="3" class="tbltxt">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="tbltxt">
                From Date
            </td>
            <td class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtFmdate" runat="server" AutoPostBack="True" onblur="getduedates();"
                    onchange="getduedates(this);" CssClass="vsmalltb" TabIndex="6"></asp:TextBox>&nbsp;
                <rjs:PopCalendar ID="dtpFromDate" runat="server" Control="txtFmdate" />
            </td>
            <td class="tbltxt">
                To Date
            </td>
            <td class="tbltxt">
                :
            </td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="vsmalltb" TabIndex="7"></asp:TextBox>&nbsp;
                <rjs:PopCalendar ID="dtpToDate" runat="server" Control="txtToDate" />
            </td>
            <td colspan="3" class="tbltxt">
                <asp:Button ID="btngo" runat="server" OnClick="btngo_Click" Text="Show" TabIndex="8"
                    OnClientClick="return isValid();" />&nbsp;
                <asp:Button ID="btnprint" runat="server" Text="Print" TabIndex="9" OnClick="btnprint_Click" />&nbsp;
                <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                    Width="106px" TabIndex="10" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="9">
                <asp:Label ID="lblReport" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
