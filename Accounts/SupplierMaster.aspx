<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="SupplierMaster.aspx.cs" Inherits="Accounts_SupplierMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function edRfv() {
            var bal = document.getElementById("<%=txtAcBalance.ClientID %>").value;
            if (bal.trim() == '') {
                document.getElementById("ctl00_ContentPlaceHolder1_rfvPmtType").enabled = false;
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_rfvPmtType").enabled = true;
            }
        }
    </script>

    <table style="width: 100%;" cellspacing="0" cellpadding="0">
        <tr>
            <td height="20" valign="top">
                <div class="bedcromb">
                    Expenses &raquo; Add Creditor Details
                </div>
                <br />
            </td>
        </tr>
    </table>
    <div align="center">
        <div style="width: 500px; background-color: #666; padding: 2px; margin: 0 auto;">
            <div style="background-color: #FFF; padding: 10px;">
                <div class="linegap">
                    <img src="../images/mask.gif" height="5" width="10" />
                </div>
                <table style="width: 100%;" cellspacing="0" cellpadding="0">
                    <tr>
                        <td style="width: 40%;" valign="top">&nbsp;
                        </td>
                        <td style="width: 60%">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">Name of the Creditor :
                        </td>
                        <td align="left" style="padding-left: 10px;">
                            <asp:TextBox ID="txtSupplierName" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                            <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server"
                                TargetControlID="rfvSupplierName">
                            </ajaxToolkit:ValidatorCalloutExtender>
                            <asp:RequiredFieldValidator ID="rfvSupplierName" runat="server" ControlToValidate="txtSupplierName"
                                Display="Dynamic" ErrorMessage="*Enter Supplier Name" ValidationGroup="grpSuppliers">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">Address :
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtAddress" runat="server" Width="200px" MaxLength="80"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">Telephone No :
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtTelNo" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                FilterType="Custom, Numbers" TargetControlID="txtTelNo">
                            </ajaxToolkit:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">FAX No :
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtFAXNo" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">Email Id :
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtEmailId" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmailId"
                                Display="Dynamic" ErrorMessage="* Invalid Format" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ValidationGroup="grpSuppliers">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">TIN No :
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtTIN" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">CST No :
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtCST" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">A/c Balance :
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtAcBalance" runat="server" Width="120px" onchange="edRfv();"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                FilterType="Custom, Numbers" TargetControlID="txtAcBalance" ValidChars=".">
                            </ajaxToolkit:FilteredTextBoxExtender>
                            <asp:DropDownList ID="drpPayMethod" runat="server" Width="80px">
                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                <asp:ListItem>Cr</asp:ListItem>
                                <asp:ListItem>Dr</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvPmtType" runat="server" ErrorMessage="Please select balance type !"
                                ControlToValidate="drpPayMethod" InitialValue="0" ValidationGroup="grpSuppliers"
                                Display="Dynamic" Enabled="false">*</asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server"
                                TargetControlID="rfvPmtType">
                            </ajaxToolkit:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">A/c Balance As On Date :
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtAcOnDate" runat="server" Width="200px"></asp:TextBox>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/PopCalendar/Calendar.gif" />
                            <asp:RequiredFieldValidator ID="rfvTranDate" runat="server" ControlToValidate="txtAcOnDate"
                                ErrorMessage="Enter Transaction Date" ValidationGroup="vgReceive" Display="Dynamic">*</asp:RequiredFieldValidator>
                            <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server"
                                TargetControlID="rfvTranDate">
                            </ajaxToolkit:ValidatorCalloutExtender>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd MMM yyyy"
                                PopupButtonID="Image1" TargetControlID="txtAcOnDate">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="Button1" runat="server" Text="Submit" ValidationGroup="grpSuppliers"
                                OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click" />
                            &nbsp;
                            <asp:Button ID="btnShow" runat="server" Text="Show List" OnClick="btnShow_Click" />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px"></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div style="height: 30px;"></div>
</asp:Content>
