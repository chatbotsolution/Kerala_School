<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptPartyLedger.aspx.cs" Inherits="Accounts_rptPartyLedger" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function beginRequest(sender, args) {
            // show the popup
            $find('<%=mdlloading.ClientID %>').show();

        }

        function endRequest(sender, args) {
            //  hide the popup
            $find('<%=mdlloading.ClientID %>').hide();

        }

        function isValid() {

            var PartyType = document.getElementById("<%=drpPartyType.ClientID %>").value;
            var Party = document.getElementById("<%=drpParty.ClientID %>").value;


            if (PartyType == 0) {
                alert("Please Select Party Type !");
                document.getElementById("<%=drpPartyType.ClientID %>").focus();
                return false;
            }
            if (Party == 0) {
                alert("Please Select Party !");
                document.getElementById("<%=drpParty.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }

    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnPrint" EventName="Click" />
            <asp:PostBackTrigger ControlID="BtnExcel" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                                Party Ledger
                            </h1>
                            <h2>
                                List</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" style="width: 220px;">
                        <strong>
                            <asp:Label ID="lblPartyType" runat="server" Text="Party Type : "></asp:Label><span
                                class="mandatory">*</span>
                            <asp:DropDownList ID="drpPartyType" runat="server" AutoPostBack="True" TabIndex="1"
                                OnSelectedIndexChanged="drpPartyType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </strong>
                    </td>
                    <td align="left" colspan="4">
                        <strong>
                            <asp:Label ID="lblParty" runat="server" Text="Party : "></asp:Label><span class="mandatory">*</span>
                            <asp:DropDownList ID="drpParty" runat="server" TabIndex="2">
                            </asp:DropDownList>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <strong>
                            <asp:Label ID="lblFromdt" runat="server" Text="From Date : "></asp:Label>
                            <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" CssClass="tbltxtbox" Width="80px"
                                TabIndex="3"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFromDt" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="lnkbtnClearFromDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFromDt.value='';return false;"
                Text="Clear" ></asp:LinkButton>
                        </strong>
                    </td>
                    <td style="width: 220px;">
                        <strong>
                            <asp:Label ID="lblToDt" runat="server" Text="To Date : "></asp:Label>
                            <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" Width="80px" TabIndex="4"></asp:TextBox>
                            <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtToDt.value='';return false;"
                Text="Clear" ></asp:LinkButton>
                        </strong>
                    </td>
                    <td>
                        <strong>
                            <asp:RadioButtonList ID="rbnAmount" runat="server" Font-Bold="true" Font-Size="16px"
                                RepeatDirection="Horizontal" TabIndex="5">
                                <asp:ListItem>Credit</asp:ListItem>
                                <asp:ListItem>Debit</asp:ListItem>
                            </asp:RadioButtonList>
                        </strong>
                    </td>
                    <td align="left" colspan="4">
                        <asp:Button ID="btnView" runat="server" OnClick="btnView_Click" OnClientClick="return isValid();"
                            TabIndex="6" Text="View List" />
                        &nbsp;
                        <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" TabIndex="7" Text="Print" />
                        &nbsp;
                        <asp:Button ID="BtnExcel" runat="server" OnClick="btnExpExcel_Click" TabIndex="8"
                            Text="Export to Excel" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblReport" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
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

