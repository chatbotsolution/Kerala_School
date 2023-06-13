<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptPrevDueReceipt.aspx.cs" Inherits="Reports_rptPrevDueReceipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function printcontent() {

            var DocumentContainer = document.getElementById('<%=literaldata.ClientID%>');

            var documentheader = document.getElementById('divhdr');
            var documentfooter = document.getElementById('divfooter');
            var WindowObject = window.open('', "TrackData",
                             "width=420,height=225,top=250,left=345,toolbars=no,scrollbars=no,status=no,resizable=yes");

            WindowObject.document.write(documentheader.innerHTML + "\n" + DocumentContainer.innerHTML + "\n" + documentfooter.innerHTML);
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
                        <td valign="top" align="left" style="width: 500px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnBack" Text="Back" runat="server" onclick="btnBack_Click" /> &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnPrint" runat="server" Text="Print" Width="47px" OnClientClick="return printcontent();"
                                            TabIndex="2" />
                                        <asp:Button ID="btnContinueCashReceipt" runat="server" OnClick="btnContinueCashReceipt_Click"
                                            Text="Continue Cash Receipt" TabIndex="3" />
                                        <asp:Button ID="btnMainMenu" runat="server" OnClick="btnMainMenu_Click" Text="Main Menu"
                                            TabIndex="4" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 100%">
                            <div id="divreport">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%">
                                            <asp:Label ID="literaldata" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 524px">
                                        </td>
                                    </tr>
                                </table>
                                <div style="display: none" id="divhdr">
                                </div>
                                <div style="display: none" id="divfooter">
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

