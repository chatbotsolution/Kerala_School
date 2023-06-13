<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptSundryCreditors.aspx.cs" Inherits="Accounts_rptSundryCreditors" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function PrintContent() {
            var DocumentContainer = document.getElementById('<%=viewcon.ClientID%>');
            var WindowObject = window.open('', "TrackData", "width=420,height=225,top=250,left=345,toolbars=no,scrollbars=no,status=no,resizable=no");

            WindowObject.document.write(DocumentContainer.innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            WindowObject.close();
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
                                <div>
                                    <h1>
                                        Sundry
                                    </h1>
                                    <h2>
                                        Creditors
                                    </h2>
                                </div>
                            </td>
                            <td height="35" align="left" valign="middle">
                                <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="height: 14px" colspan="2">
                                <asp:Label ID="lblCreditor" runat="server" Text="Creditors list as on :"></asp:Label>
                                <asp:TextBox ID="txtstartdate" runat="server" TabIndex="2" Width="80px"></asp:TextBox>
                                <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtstartdate"></rjs:PopCalendar>
                                <asp:LinkButton ID="lnkDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtstartdate.value='';return false;"
                Text="Clear" ></asp:LinkButton>
                                &nbsp;
                                <asp:Button ID="btnView" runat="server" Text="Show Report" OnClick="btnView_Click"
                                    TabIndex="4" />
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
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../Images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

