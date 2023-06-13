<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptBankReconciliation.aspx.cs" Inherits="Accounts_rptBankReconciliation" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function PrintContent() {
            var DocumentContainer = document.getElementById('<%=viewcon.ClientID%>');
            var WindowObject = window.open('', "TrackData", "width=820,height=625,top=20,left=345,toolbars=no,scrollbars=no,status=no,resizable=no");

            WindowObject.document.write(DocumentContainer.innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            WindowObject.close();
        }
    </script>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr style="background-color: #ededed;">
            <td width="350" align="left" valign="middle" style="background-color: ">
                <div>
                    <h1>
                        BANK RECONILLATION
                    </h1>
                    <h2>
                        REPORT
                    </h2>
                </div>
            </td>
            <td height="35" align="left" valign="middle">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                Bank:&nbsp;<asp:DropDownList ID="drpBank" runat="server"> </asp:DropDownList>
                &nbsp;
                <asp:Button ID="btnShow" runat="server" Text="Show" onclick="btnShow_Click" />
                &nbsp;
                <asp:Button ID="btnprnt" runat="server" Text="Print" Width="80px" OnClientClick='javascript:PrintContent()'
                    Enabled="False" TabIndex="5" CausesValidation="False" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width:100%;">
                <div id="viewcon" runat="server">
                    <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

