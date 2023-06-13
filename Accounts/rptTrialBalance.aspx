<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptTrialBalance.aspx.cs" Inherits="Accounts_rptTrialBalance" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
        function PrintContent() {

            var DocumentContainer = document.getElementById('<%=viewcon.ClientID%>');


            var WindowObject = window.open('', "TrackData",
                              "width=420,height=225,top=250,left=345,toolbars=no,scrollbars=no,status=no,resizable=no");

            WindowObject.document.write(DocumentContainer.innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            WindowObject.close();
        }

        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';
        }

        function validate() {
            var dtason = document.getElementById('<%=txtstartdate.ClientID%>').value;

            if (dtason == "") {
                alert("Please enter date");
                document.getElementById('<%=txtstartdate.ClientID%>').focus();
                return false;
            }
            else {
                CheckLoader();
                return true;
            }
        }

    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <div align="center">
                <asp:Panel ID="pnlcat" runat="server" Width="100%">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr style="background-color: #ededed;">
                            <td width="350" align="left" valign="middle" style="background-color: ">
                                <div class="headingcontainor">
                                    <h1>
                                        TRIAL
                                    </h1>
                                    <h2>
                                        BALANCE
                                    </h2>
                                </div>
                            </td>
                            <td height="35" align="left" valign="middle">
                                <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="height: 14px" colspan="2">
                                <asp:Label ID="lblTrialBal" runat="server" Text="Trial Balance as on :"></asp:Label>
                                <asp:TextBox ID="txtstartdate" runat="server" TabIndex="2" Width="160px"></asp:TextBox>
                                <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtstartdate"></rjs:PopCalendar>
                                <asp:LinkButton ID="lnkbtnStartDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtstartdate.value='';return false;"
                Text="Clear" ></asp:LinkButton>
                                &nbsp;
                                <asp:Button ID="btnView" runat="server" Text="Show Report" OnClick="btnView_Click"
                                    TabIndex="4" OnClientClick="return validate();" />
                                <asp:Button ID="btnprnt" runat="server" Text="Print" Width="80px" OnClientClick='javascript:PrintContent()'
                                    Visible="False" TabIndex="5" CausesValidation="False" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <div id="viewcon" runat="server">
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div id="Loader" style="text-align: center; vertical-align: middle; position: absolute;
        top: 0px; left: 0px; z-index: 99; width: 100%; height: 100%; background-color: #ededed;
        -ms-filter: 'progid:DXImageTransform.Microsoft.Alpha(Opacity=60)'; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=60);
        -moz-opacity: 0.8; opacity: 0.8;">
        <div style="width: 48px; height: 48px; margin: 0 auto; margin-top: 275px;">
            <img src="../images/spinner.gif">
        </div>
        <div style="font-family: Trebuchet MS; font-size: 12px; color: Green; text-align: center;">
            Please Wait ...</div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
