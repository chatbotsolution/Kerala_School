<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="SupplierInvoice.aspx.cs" Inherits="Accounts_SupplierInvoice" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table style="width: 100%;" width="50%" cellpadding="0" cellspacing="0">
        <tr>
            <td height="20" valign="top">
                <div class="bedcromb">
                    Payment
                </div>
            </td>
        </tr>
    </table>
    <br />
    <center>
        <div align="center">
            <div style="width: 850px; background-color: #666; padding: 2px; margin: 0 auto;">
                <div style="background-color: #FFF; padding: 0px 10px 10px 10px;">
                    <table width="100%">
                        <tr style="background-color: #026302; color: White;">
                            <td align="left" colspan="2" style="padding-left: 10px; font-weight: bold;">SUPPLIER DETAILS
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" class="tbltxt">
                                <div>
                                    Creditor&nbsp;:&nbsp;<asp:DropDownList ID="drpSupplier" runat="server" ValidationGroup="vgPay" Width="230px"
                                        AutoPostBack="True" OnSelectedIndexChanged="drpSupplier_SelectedIndexChanged"
                                        TabIndex="1">
                                    </asp:DropDownList><asp:RequiredFieldValidator ID="rfvSupplier" runat="server" ControlToValidate="drpSupplier"
                                        Display="Dynamic" ErrorMessage="Select A Supplier" InitialValue="0" ValidationGroup="vgPay">*</asp:RequiredFieldValidator>
                                    <div id="AcHead" style="float: right; width: 50%; text-align: left;" visible="false" runat="server">
                                        &nbsp;&nbsp;&nbsp;Account Head&nbsp;:&nbsp;
                                <asp:DropDownList ID="drpAcHead" runat="server" Width="250px">
                                </asp:DropDownList>
                                        <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server"
                                            TargetControlID="rfvAcHead">
                                        </ajaxToolkit:ValidatorCalloutExtender>
                                        <asp:RequiredFieldValidator ID="rfvAcHead" runat="server" ControlToValidate="drpAcHead"
                                            Display="Dynamic" ErrorMessage="Please select an A/C Head !" InitialValue="0" ValidationGroup="vgPay">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tbltxt" colspan="2" align="left">Transaction Details&nbsp;:&nbsp;<asp:TextBox ID="txtPmtDetails" runat="server" Width="350px" TabIndex="2" MaxLength="100"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td align="left" class="tbltxt">
                                <asp:Label ID="lblBal" runat="server" ForeColor="#FF3300"></asp:Label>
                            </td>
                            <td class="tbltxt">&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <br />
        <div align="center">
            <div style="width: 850px; background-color: #666; padding: 2px; margin: 0 auto;">
                <div style="background-color: #FFF; padding: 0px 10px 10px 10px;">
                    <table width="100%">
                        <tr style="background-color: #026302; color: White; font-weight: bold;">
                            <td align="left" colspan="5" style="padding-left: 10px;">PAYMENT DETAILS
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 12%" class="tbltxt">MR No :
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="txtBillNo" runat="server" ValidationGroup="vgPay" Width="170px"
                                    TabIndex="3" MaxLength="20"></asp:TextBox>&nbsp;<asp:Label ID="lblPrevMRNo" ForeColor="White" runat="server" BackColor="Gray" />
                            </td>
                            <td align="left" class="tbltxt">Paid Date :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtBillDate" runat="server" ValidationGroup="vgPay" Width="150px"
                                    TabIndex="4"></asp:TextBox>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/PopCalendar/Calendar.gif" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd MMM yyyy"
                                    PopupButtonID="Image1" TargetControlID="txtBillDate">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender11" runat="server"
                                    TargetControlID="rfvMRDt">
                                </ajaxToolkit:ValidatorCalloutExtender>
                                <asp:RequiredFieldValidator ID="rfvMRDt" runat="server" ControlToValidate="txtBillDate"
                                    Display="Dynamic" ErrorMessage="Please provide paid date !" ValidationGroup="vgPay">*</asp:RequiredFieldValidator>
                            </td>

                        </tr>
                        <tr>
                            <td align="left" class="tbltxt">Pay Order No:
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtPayOrderNo" runat="server" Width="170px" TabIndex="5" MaxLength="20"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 15%" class="tbltxt">Paid Amount :
                            </td>
                            <td style="width: 26%">
                                <asp:TextBox ID="txtPaidAmt" runat="server" ValidationGroup="vgPay" Width="170px"
                                    TabIndex="8" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPaidAmt" runat="server" ControlToValidate="txtPaidAmt"
                                    Display="Dynamic" ErrorMessage="Enter Paid Amount" ValidationGroup="vgPay">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>

                            <td align="left" class="tbltxt">Pay Slip No:
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtPaySlipNo" runat="server" Width="170px" TabIndex="6" MaxLength="20"></asp:TextBox>
                            </td>
                            <td align="left" class="tbltxt">Payment Mode :
                            </td>
                            <td>
                                <asp:DropDownList ID="drpPmtMode" runat="server" ValidationGroup="vgPay" Width="170px" OnSelectedIndexChanged="drpPmtMode_SelectedIndexChanged"
                                    TabIndex="9" AutoPostBack="true">
                                    <asp:ListItem Value="0">---Select---</asp:ListItem>
                                    <asp:ListItem>Cash</asp:ListItem>
                                    <asp:ListItem>Draft</asp:ListItem>
                                    <asp:ListItem>Cheque</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPmtMode" runat="server" ControlToValidate="drpPmtMode"
                                    Display="Dynamic" ErrorMessage="Select Mode Of Payment" ValidationGroup="vgPay"
                                    InitialValue="0">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="tbltxt">Received By :
                            </td>
                            <td>
                                <asp:TextBox ID="txtRcvdBy" runat="server" ValidationGroup="vgPay" Width="170px"
                                    MaxLength="50" TabIndex="7"></asp:TextBox>
                                <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender12" runat="server"
                                    TargetControlID="rfvRcvdBy">
                                </ajaxToolkit:ValidatorCalloutExtender>
                                <asp:RequiredFieldValidator ID="rfvRcvdBy" runat="server" ControlToValidate="txtRcvdBy"
                                    Display="Dynamic" ErrorMessage="Please provide name of the person who received the amount !" ValidationGroup="vgPay">*</asp:RequiredFieldValidator>
                            </td>

                            <td align="left" class="tbltxt">Cheque/Draft No :
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo" runat="server" Width="170px" MaxLength="20" TabIndex="10"></asp:TextBox>
                                <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server"
                                    TargetControlID="rfvDDNo">
                                </ajaxToolkit:ValidatorCalloutExtender>
                                <asp:RequiredFieldValidator ID="rfvDDNo" runat="server" ControlToValidate="txtChequeNo"
                                    Display="Dynamic" ErrorMessage="Please provide Cheque/Draft No !" ValidationGroup="vgPay">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>

                            <td align="left" class="tbltxt">Drawn On Bank :
                            </td>
                            <td>
                                <asp:TextBox ID="txtBank" runat="server" Width="170px" MaxLength="50" TabIndex="11"></asp:TextBox>
                                <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server"
                                    TargetControlID="rfvBank">
                                </ajaxToolkit:ValidatorCalloutExtender>
                                <asp:RequiredFieldValidator ID="rfvBank" runat="server" ControlToValidate="txtBank"
                                    Display="Dynamic" ErrorMessage="Please provide bank name !" ValidationGroup="vgPay">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">&nbsp;
                            </td>
                            <td align="left">&nbsp;
                            </td>
                            <td align="left">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <hr />
                                <asp:Button ID="btnPay" runat="server" Text="   Pay   " ValidationGroup="vgPay" OnClick="btnPay_Click"
                                    TabIndex="13" />&nbsp;&nbsp;
                                <asp:Button ID="btnClear" runat="server" Text=" Clear " OnClick="btnClear_Click"
                                    TabIndex="14" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnList" runat="server" Text=" Go to List " TabIndex="15"
                                    OnClick="btnList_Click" />
                                &nbsp;<asp:HyperLink ID="hlPrint" NavigateUrl="#" runat="server" Text="Print Voucher" Target="_blank" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <br />
        <br />
    </center>
</asp:Content>


