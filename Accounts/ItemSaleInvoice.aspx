<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="ItemSaleInvoice.aspx.cs" Inherits="Accounts_ItemSaleInvoice" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function printDetail() {

            var DocumentContainer = document.getElementById('<%=literaldata.ClientID%>');
            var WindowObject = window.open('', "TrackData",
                             "width=420,height=225,top=250,left=345,toolbars=no,scrollbars=no,status=no,resizable=yes");

            WindowObject.document.write(DocumentContainer.innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            WindowObject.close();
            return false;
        }
       

    </script>

    <table style="height: 420px">
        <tr>
            <td valign="top" style="width: 794px">
                <table width="100%">
                    <tr visible="false">
                        <td valign="top" align="left" style="width: 800px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnConsolidat" runat="server" Text="Print Consolidated Invoice" OnClientClick="return printConsolidated();"
                                            TabIndex="1" OnClick="btnConsolidat_Click" />
                                        <asp:Button ID="btnDetail" runat="server" Text="Print Detailed Invoice"
                                            TabIndex="2" onclick="btnDetail_Click" />
                                        <asp:Button ID="btnBack" Text="Back" runat="server" OnClick="btnBack_Click" TabIndex="3" />
                                        <asp:Button ID="btnMainMenu" runat="server" OnClick="btnMainMenu_Click" Text="Main Menu"
                                            TabIndex="3" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 100%">
                            <table width="100%">
                                <tr>
                                    <td style="width: 100%" align="center">
                                        <asp:Label ID="literaldata" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 524px">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 100%">
                            <table width="100%">
                                <tr>
                                    <td style="width: 100%" align="center">
                                        <asp:Label ID="lblConsolidated" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

