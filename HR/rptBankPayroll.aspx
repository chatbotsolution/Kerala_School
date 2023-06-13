<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="rptBankPayroll.aspx.cs" Inherits="HR_rptBankPayroll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function valShow() {
            var Mth = document.getElementById("<%=drpMonth.ClientID %>").value;
            var Yr = document.getElementById("<%=drpYear.ClientID %>").value;
            var to = document.getElementById("<%=txtTo.ClientID %>").value;
            var sub = document.getElementById("<%=txtSubject.ClientID %>").value;
            var desc = document.getElementById("<%=txtDesc.ClientID %>").value;

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
            if (to.trim() == "") {
                alert("To field shouldn't be left blank.");
                document.getElementById("<%=txtTo.ClientID %>").focus();
                return false;
            }
            if (sub.trim() == "") {
                alert("Subject field shouldn't be left blank.");
                document.getElementById("<%=txtSubject.ClientID %>").focus();
                return false;
            }
            if (desc.trim() == "") {
                alert("Description field shouldn't be left blank.");
                document.getElementById("<%=txtDesc.ClientID %>").focus();
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
            Payroll for Bank</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table width="100%" style="border: ridge 2px black;">
                                        <tr style="background-color: Black;">
                                            <td align="center" style="color: White;" colspan="2">
                                                <strong>FILTER CRITERIA</strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <strong>Month&nbsp;:&nbsp;</strong>
                                                <asp:DropDownList ID="drpMonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpMonth_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">- SELECT -</asp:ListItem>
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
                                                <asp:DropDownList ID="drpYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpYear_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <strong>&nbsp;Bill No : </strong>
                                                <asp:DropDownList ID="drpBillNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpBillNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="100%" colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="font-size: 12px;">
                                                <b>To</b>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtTo" runat="server" Width="700px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="font-size: 12px;">
                                                <b>Subject</b>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtSubject" runat="server" Width="700px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" style="font-size: 12px;">
                                                <b>Madam/Sir</b>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtDesc" runat="server" Width="700px" TextMode="MultiLine" Height="70px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnGenerate" Text="Generate Payroll" runat="server" OnClientClick="return valShow();"
                                                    Width="120px" onfocus="active(this);" onblur="inactive(this);" Enabled="false"
                                                    OnClick="btnGenerate_Click" />
                                                <asp:Button ID="btnPrint" runat="server" Text="Print" Width="120px" onfocus="active(this);"
                                                    onblur="inactive(this);" Enabled="false" onclick="btnPrint_Click" />
                                                <asp:Button ID="btnExport" runat="server" Text="Export to Excel" Width="120px" onfocus="active(this);"
                                                    onblur="inactive(this);" Enabled="false" OnClick="btnExport_Click" Visible="false" />
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
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblReport" runat="server"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnGenerate" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
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
    </table>
</asp:Content>

