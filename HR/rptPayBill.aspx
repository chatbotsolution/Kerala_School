<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="rptPayBill.aspx.cs" Inherits="HR_rptPayBill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function valShow() {
            var Mth = document.getElementById("<%=drpMonth.ClientID %>").value;
            var Yr = document.getElementById("<%=drpYear.ClientID %>").value;

            if (Mth == "0") {
                alert("Select a Month");
                document.getElementById("<%=drpMonth.ClientID %>").focus();
                return false;
            }
            if (Yr == "0") {
                alert("Select a Year");
                document.getElementById("<%=drpYear.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Pay Bill Report</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:UpdatePanel ID="updtpnlStock" runat="server">
                    <ContentTemplate>
                        <table width="800px" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table width="100%" style="border: ridge 2px black;">
                                        <tr style="background-color: Black;">
                                            <td align="center" style="color: White;">
                                                <strong>FILTER CRITERIA</strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <strong>Month&nbsp;:&nbsp;</strong>
                                                <asp:DropDownList ID="drpMonth" runat="server">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                    <asp:ListItem Value="JAN">January</asp:ListItem>
                                                    <asp:ListItem Value="FEB">February</asp:ListItem>
                                                    <asp:ListItem Value="MAR">March</asp:ListItem>
                                                    <asp:ListItem Value="APR">April</asp:ListItem>
                                                    <asp:ListItem Value="MAY">May</asp:ListItem>
                                                    <asp:ListItem Value="JUN">June</asp:ListItem>
                                                    <asp:ListItem Value="JUL">July</asp:ListItem>
                                                    <asp:ListItem Value="AUG">August</asp:ListItem>
                                                    <asp:ListItem Value="SEP">September</asp:ListItem>
                                                    <asp:ListItem Value="OCT">October</asp:ListItem>
                                                    <asp:ListItem Value="NOV">November</asp:ListItem>
                                                    <asp:ListItem Value="DEC">December</asp:ListItem>
                                                </asp:DropDownList>
                                                <strong>&nbsp;Year : </strong>
                                                <asp:DropDownList ID="drpYear" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnShow" Text="Show Report" runat="server" OnClick="btnShow_Click"
                                                    OnClientClick="return valShow();" />&nbsp;<asp:Button ID="btnPrint" runat="server"
                                                        Text="Print" OnClick="btnPrint_Click" />&nbsp;<asp:Button ID="btnExport" runat="server"
                                                            Text="Export To Excel" OnClick="btnExport_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="rowdata" runat="server" visible="false">
                                <td align="center">
                                    <table width="100%" style="border: solid 2px black;">
                                        <tr>
                                            <td align="center">
                                                <asp:UpdatePanel ID="updtpnl1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblReport" runat="server"></asp:Label>
                                                        <asp:Label Visible="false" ID="lblNoData" ForeColor="Black" Font-Bold="true" Font-Size="20px"
                                                            Text="No record found !" runat="server"></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExport" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="height: 20px;">
            </td>
        </tr>
    </table>
</asp:Content>
