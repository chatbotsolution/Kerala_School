<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptFeeReceiptCheque.aspx.cs" Inherits="Reports_rptFeeReceiptCheque" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
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


        function Button1_onclick() {

        }

    </script>

    <table style="height: 360px">
        <tr>
            <td valign="top" style="width: 794px">
                <table width="100%">
                    <tr visible="false">
                        <td valign="top" align="left" style="width: 500px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <input type="button" value="Back" title="Back" 
                                            onclick="javascript:history.back();" id="Button1" tabindex="1" onclick="return Button1_onclick()" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnPrint" runat="server" Text="Print" Width="47px" 
                                            OnClientClick="return printcontent();" OnClick="btnPrint_Click1" 
                                            TabIndex="2"  />
                                        <asp:Button ID="btnContinueChequeReceipt" runat="server" OnClick="btnContinueCashReceipt_Click"
                                            Text="Continue Cheque Receipt" TabIndex="3" />
                                        <asp:Button ID="btnMainMenu" runat="server" OnClick="btnMainMenu_Click" 
                                            Text="Main Menu" TabIndex="4" /></td>
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
                                    <table width="70%">
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Text="Sri Guru Harkrishan Sr.Sec. Public School"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="Affiliated to C.B.S.E. New Delhi"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="(Chief Khalsa Diwan),Sector 40-C, Chandigarh"></asp:Label>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                    <%-- <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label4" runat="server" Text="Fee Received" Font-Bold="True" Font-Underline="True"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblPrintDate" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>--%>
                                </div>
                                <div id="divfooter">
                                    <table width="80%">
                                        <tr>
                                            <td align="left">
                                                Note: 1.Monthly fee shall be paid by 15th , After that fine Rs 50/- will be charged.
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2.Late fee fine Rs.
                                                50/- will be charged.
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2.Fee collected will
                                                not be refundable.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3.Cheque payment is
                                                subject to realization.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>
                                                    Cheque Details:
                                                </strong>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                    Cheque No:
                                                    <asp:Label ID="lblchequeno" runat="server" ></asp:Label>,
                                                &nbsp;
                                                    Cheque Date:
                                                
                                                <asp:Label ID="lblchequedate" runat="server" ></asp:Label>,
                                                 &nbsp;
                                                    Drawn on Bank:
                                                
                                                <asp:Label ID="lblbank" runat="server" ></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <strong>HEAD CLERK</strong>
                                            </td>
                                            <td align="RIGHT">
                                                <strong>PRINCIPAL</strong>
                                            </td>
                                        </tr>
                                    </table>
                                    <%-- <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label4" runat="server" Text="Fee Received" Font-Bold="True" Font-Underline="True"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblPrintDate" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>--%>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>


