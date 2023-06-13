<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="OpenFY.aspx.cs" Inherits="Accounts_OpenFY" %>

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
        function Cnf() {

            if (confirm("You are going to open a financial year for transaction. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }

    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="250" align="left" valign="middle" height="35">
                        <div>
                            <h1>
                                Open 
                            </h1>
                            <h2>
                                Financial Year</h2>
                        </div>
                    </td>
                    <td align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-top: 10px;">
                        <table width="100%" border="0" cellspacing="1" cellpadding="2">
                            <tr>
                                <td align="left" valign="top">
                                    <table style="width: 70%">
                                        <tr align="left">
                                            <td>
                                                Financial Year Start Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtYearStartDate" runat="server"></asp:TextBox>
                                                <rjs:PopCalendar ID="calYearStartDate" runat="server" Control="txtYearStartDate">
                                                </rjs:PopCalendar>
                                            </td>
                                            <td>
                                                Financial Year End Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtYearEndDate" runat="server"></asp:TextBox>
                                                <rjs:PopCalendar ID="calYearEndDate" runat="server" Control="txtYearEndDate"></rjs:PopCalendar>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="height: 25px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:PlaceHolder ID="phControls" runat="server"></asp:PlaceHolder>
                                    <asp:HiddenField ID="hidAccountCount" runat="server" />
                                    <asp:Label ID="lblAccountCode" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px">
                                    <span style="color:Red;">
                                                1. Set Account Head as <b>DEBIT</b> if balance is in positive or is to be Received
                                                </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table cellpadding="0" cellspacing="0" border="0" width="62%">
                                        <tr>
                                            <td>
                                                
                                                <span style="color:Red;">
                                                2. Account Head as <b>CREDIT</b> if balance is in negative or is to be Paid
                                                </span>
                                            </td>
                                            <td align="right" style="padding-bottom: 10px;">
                                                <asp:Button ID="btnCalculateTotals" runat="server" Text="Calculate Total Amount"
                                                    CausesValidation="false" OnClick="btnCalculateTotals_Click" onfocus="active(this);" onblur="inactive(this);"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="left">
                                                Total Debit Amount&nbsp;&nbsp;:&nbsp;<asp:Label ID="lblDebitAmount" runat="server"
                                                    Text="0"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="left">
                                                Total Credit Amount&nbsp;&nbsp;:&nbsp;<asp:Label ID="lblCreditAmount" runat="server"
                                                    Text="0"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%;">
                                                &nbsp;
                                            </td>
                                            <td align="left">
                                                Closing Balance&nbsp; &nbsp; :&nbsp;<asp:TextBox ID="txtClosingBal" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 15px; padding-left: 270px;" align="left">
                                    <asp:Label ID="lblBottomMsg" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <table cellpadding="0" cellspacing="0" border="0" width="90%">
                                        <tr>
                                            <td align="left">
                                                <asp:Button ID="btnInitialYear" runat="server" Text="Save" OnClick="btnInitialYear_Click"
                                                    Width="130px" OnClientClick="return Cnf();" Enabled="true" onfocus="active(this);" onblur="inactive(this);" />
                                                <asp:Button ID="btnGoToList" Text="Go To List" runat="server" OnClick="btnGoToList_Click"
                                                    Width="130px" onfocus="active(this);" onblur="inactive(this);"/>&nbsp;
                                                <asp:Button ID="btnClear" Text="Clear" runat="server" Width="120px"
                                                        OnClick="btnClear_Click" onfocus="active(this);" onblur="inactive(this);"/>
                                                <asp:Button ID="btnCancel" runat="server" Width="120px" Text="Cancel" OnClick="btnCancel_Click" onfocus="active(this);" onblur="inactive(this);"/>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                </td>
                            </tr>
                        </table>
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

