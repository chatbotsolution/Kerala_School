<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="rptPurchaseDetails.aspx.cs" Inherits="Inventory_rptPurchaseDetails" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            document.getElementById("<%=txtFromDate.ClientID %>").focus();
        });
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=500,left = 390,top = 184');");
        } 
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Purchase Details
        </h2>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblmsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
    </div>
    <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px">
        <table width="100%" cellspacing="2" cellpadding="2">
            <tr>
                <td>
                    From Date:
                    <asp:TextBox ID="txtFromDate" runat="server" Width="100px" TabIndex="4" CssClass="tbltxtbox"></asp:TextBox>&nbsp;
                    <rjs:PopCalendar ID="pcalFromDate" runat="server" Control="txtFromDate" />
                    <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFromDate.value='';return false;"
                        Text="Clear"></asp:LinkButton>&nbsp;&nbsp; To Date:
                    <asp:TextBox ID="txtToDate" runat="server" Width="100px" TabIndex="5" CssClass="tbltxtbox"></asp:TextBox>
                    <rjs:PopCalendar ID="pcalToDate" runat="server" Control="txtToDate" />
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtToDate.value='';return false;"
                        Text="Clear"></asp:LinkButton>&nbsp;&nbsp;
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" TabIndex="6" />&nbsp;
                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="7" />&nbsp;
                    <asp:Button ID="btnExpExcel" runat="server" Text="Export to Excel" Visible="False"
                        Width="106px" TabIndex="8" OnClick="btnExpExcel_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div style="padding-top: 5px;">
        <asp:Label ID="lblReport" runat="server"></asp:Label></div>
</asp:Content>
