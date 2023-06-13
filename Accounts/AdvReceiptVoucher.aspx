<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="AdvReceiptVoucher.aspx.cs" Inherits="Accounts_AdvReceiptVoucher" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script language="javascript" type="text/javascript">
    function IsValidate() {
        var TransDt = document.getElementById("<%=txtTransactionDate.ClientID %>").value;
        var RcvdFrm = document.getElementById("<%=drpCreditHead.ClientID %>").value;
        var Amnt = document.getElementById("<%=txtAmount.ClientID %>").value;
        var Narra = document.getElementById("<%=txtDesc.ClientID %>").value;

        if (TransDt.trim() == "") {
            alert("Please Enter Transaction Date");
            document.getElementById("<%=txtTransactionDate.ClientID %>").focus();
            return false;
        }
        var dayfield = TransDt.split("-")[0];
        var monthfield = TransDt.split("-")[1];
        var yearfield = TransDt.split("-")[2];
        monthfield = monthfield - 1;
        var myDate = new Date(yearfield, monthfield, dayfield);
        var today = new Date();

        if (myDate > today) {
            if (alert("Transaction date is future date is Not Possible!")) {
                return true;
            }
            else {
                return false;
            }
        }
        if (RcvdFrm == "0") {
            alert("Please Select Receiver");
            document.getElementById("<%=drpCreditHead.ClientID %>").focus();
            return false;
        }
        if (Amnt.trim() == "") {
            alert("Please Enter Amount");
            document.getElementById("<%=txtAmount.ClientID %>").focus();
            return false;
        }
        if (Narra.trim() == "") {
            alert("Please Enter Narration");
            document.getElementById("<%=txtDesc.ClientID %>").focus();
            return false;
        }

        else {
            return true;
        }
    }

    function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
        var key;
        var isCtrl = false;
        var keychar;
        var reg;

        if (window.event) {
            key = e.keyCode;
            isCtrl = window.event.ctrlKey
        }
        else if (e.which) {
            key = e.which;
            isCtrl = e.ctrlKey;
        }

        if (isNaN(key)) return true;

        keychar = String.fromCharCode(key);

        // check for backspace or delete, or if Ctrl was pressed
        if (key == 8 || isCtrl) {
            return true;
        }

        reg = /\d/;
        var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
        var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

        return isFirstN || isFirstD || reg.test(keychar);
    }
         </script>

<table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor">
                            <h2>
                                Receipt Voucher Aginst Advance Paid to Party
                                </h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

<table width="100%" border="0" cellspacing="5px" cellpadding="0">
                <tr>
                    <td style="width: 130px" class="innertbltxt" align="left">
                        Transaction Date<span class="mandatory">*</span>
                    </td>
                    <td style="width: 5px" align="left" class="innertbltxt">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtTransactionDate" TabIndex="1" runat="server"></asp:TextBox>&nbsp;<rjs:PopCalendar
                            ID="dtpTransDt" runat="server" Control="txtTransactionDate"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td style="width: 130px" class="innertbltxt" align="left">
                        &nbsp;</td>
                    <td style="width: 5px" align="left" class="innertbltxt">
                        &nbsp;</td>
                    <td align="left">
                        &nbsp; &nbsp;</td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        Received From<span class="mandatory">*</span>
                    </td>
                    <td class="innertbltxt" align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="drpCreditHead" runat="server" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        &nbsp;</td>
                    <td class="innertbltxt" align="left">
                        &nbsp;</td>
                    <td align="left">
                        &nbsp; &nbsp;</td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        Amount<span class="mandatory">*</span>
                    </td>
                    <td class="innertbltxt" align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtAmount" runat="server" 
                            onkeypress="return blockNonNumbers(this, event, true, true);" TabIndex="3"></asp:TextBox>
                    &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        &nbsp;</td>
                    <td class="innertbltxt" align="left">
                        &nbsp;</td>
                    <td align="left">
                        &nbsp; &nbsp;</td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        Narration<span class="mandatory">*</span>
                    </td>
                    <td class="innertbltxt" align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtDesc" runat="server" TabIndex="4" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        &nbsp;</td>
                    <td class="innertbltxt" align="left">
                        &nbsp;</td>
                    <td align="left">
                        &nbsp; &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                    <td valign="top" align="left">
                        <asp:Button ID="btnSubmit" TabIndex="5" OnClick="btnSubmit_Click" 
                            runat="server" Width="100px"
                            Text="Save" OnClientClick="return IsValidate();"></asp:Button>
                            <asp:Button ID="btnPrint" TabIndex="6" 
                                OnClick="btnPrint_Click" runat="server" Text="Print Receipt" 
                            Width="100px" Enabled="false">
                            </asp:Button>
                        <asp:Button ID="btnList" runat="server" Text="Go To List" OnClick="btnList_Click"
                            Width="120px" TabIndex="8" />
                        <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                            Width="120px" TabIndex="9"></asp:Button>
                    </td>
                </tr>
            </table>
</asp:Content>

