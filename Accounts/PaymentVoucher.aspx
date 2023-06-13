<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="PaymentVoucher.aspx.cs" Inherits="Accounts_PaymentVoucher" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function IsValidate() {
            var transdate = document.getElementById("<%=txtTransactionDate.ClientID %>").value;
            var CrAcc = document.getElementById("<%=drpCreditAccount.ClientID %>").value;
            var Receiver = document.getElementById("<%=drpDebtors.ClientID %>").value;
            var InstNo = document.getElementById("<%=txtInstrumentNo.ClientID %>").value;
            var InstDt = document.getElementById("<%=txtInstrumentDt.ClientID %>").value;
            var Amount = document.getElementById("<%=txtAmount.ClientID %>").value;
            var chkTds = document.getElementById("<%=chkTds.ClientID %>");


            var Details = document.getElementById("<%=txtDetails.ClientID %>").value;

            if (transdate.trim() == "") {
                alert("Please Select Transaction date");
                document.getElementById("<%=txtTransactionDate.ClientID %>").focus();
                return false;
            }
            if (Receiver == "0") {
                alert("Please Select Reciever");
                document.getElementById("<%=drpDebtors.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=drpCreditAccount.ClientID %>").disabled == false && CrAcc == "0") {
                alert("Please select bank name");
                document.getElementById("<%=drpCreditAccount.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtInstrumentNo.ClientID %>").disabled == false && InstNo.trim() == "") {
                alert("Please Enter Instrument Number");
                document.getElementById("<%=txtInstrumentNo.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtInstrumentDt.ClientID %>").disabled == false && InstDt.trim() == "") {
                alert("Please select Instrument Date");
                document.getElementById("<%=txtInstrumentDt.ClientID %>").focus();
                return false;
            }
            if (Amount.trim() == "" || parseFloat(Amount) <= 0) {
                alert("Please Enter Amount");
                document.getElementById("<%=txtAmount.ClientID %>").focus();
                return false;
            }
            if (chkTds.checked) {
                var tdsAmount = document.getElementById("<%=txtTdsAmt.ClientID %>").value;
                if (tdsAmount.trim() == "" || parseFloat(tdsAmount) <= 0) {
                    alert("Please Enter TDS Amount");
                    document.getElementById("<%=txtTdsAmt.ClientID %>").focus();
                    return false;
                }
            }

            if (Details.trim() == "") {
                alert("Please Enter Payment Details");
                document.getElementById("<%=txtDetails.ClientID %>").focus();
                return false;
            }

            var trdate = document.getElementById('<%=txtTransactionDate.ClientID%>').value;
            var dayfield = trdate.split("-")[0];
            var monthfield = trdate.split("-")[1];
            var yearfield = trdate.split("-")[2];
            monthfield = monthfield - 1;
            var myDate = new Date(yearfield, monthfield, dayfield);
            var today = new Date();

            if (myDate > today) {
                if (confirm("Transaction date is future date!Do you want to continue?")) {
                    return true;
                }
                else {
                    return false;
                }
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

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
            <%--   <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="grdAccountGroup" EventName="RowEditing" />--%>
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="200px" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor" style="width: 200px;">
                            <h1>
                                Payment
                            </h1>
                            <h2>
                                Voucher</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="5px" cellpadding="0">
                <tr >
                    <td align="left" style="width: 172px">
                        Transaction Date<span class="mandatory">*</span>
                    </td>
                    <td style="width: 5px"  align="left">
                        :
                    </td>
                    <td align="left" >
                        <asp:TextBox ID="txtTransactionDate" TabIndex="1" runat="server" ReadOnly="true"></asp:TextBox>&nbsp;<rjs:PopCalendar
                            ID="PopCalendar2" runat="server" Control="txtTransactionDate" To-Today="true"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr >
                    <td align="left">
                        Paid To(Account Head)<span class="mandatory">*</span>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="drpDebtors" TabIndex="2" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Payment Mode<span class="mandatory">*</span>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:RadioButtonList ID="rbtnMode" TabIndex="3" runat="server" OnSelectedIndexChanged="rbtnMode_SelectedIndexChanged"
                            AutoPostBack="True" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="C">Cash</asp:ListItem>
                            <asp:ListItem Selected="True" Value="B">Bank</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Bank Name<span class="mandatory" id="spBankNm" runat="server">*</span>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="drpCreditAccount" TabIndex="4" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Instrument No.<span class="mandatory" id="spInstNo" runat="server">*</span>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtInstrumentNo" runat="server" TabIndex="5"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Instrument Date<span class="mandatory" id="spInstDt" runat="server">*</span>
                    </td>
                    <td style="width: 5px" align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtInstrumentDt" TabIndex="6" runat="server"></asp:TextBox>&nbsp;<rjs:PopCalendar
                            ID="PopCalendar1" runat="server" Control="txtInstrumentDt"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Receipt/Voucher No.
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtRcptVoucherNo" runat="server" TabIndex="7"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Amount<span class="mandatory">*</span>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtAmount" runat="server" TabIndex="8" onkeypress="return blockNonNumbers(this, event, true, true);"></asp:TextBox>
                        <asp:CheckBox ID="chkTds" runat="server" Text="TDS Applicable" AutoPostBack="true"
                            oncheckedchanged="chkTds_CheckedChanged" />
                        <asp:TextBox Visible="false" ID="txtTdsAmt" runat="server" Width="100px" onkeypress="return blockNonNumbers(this, event, true, true);"></asp:TextBox>
                        <asp:Label ForeColor="Red" Visible="false" ID="lblTds" runat="server" Text="Please enter actual paid amount(without TDS) in the amount box and TDS amount in the TDS box "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Bank Charges (If any)<%--<span class="mandatory">*</span>--%>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtTransCharge" runat="server" TabIndex="9" onkeypress="return blockNonNumbers(this, event, true, true);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Transaction Details<span class="mandatory">*</span>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtDetails" runat="server" TabIndex="10" Width="440px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                    <td valign="top" align="left">
                        <asp:Button ID="btnSubmit" TabIndex="11" OnClick="btnSubmit_Click" runat="server"
                            Text="Save & Add New" OnClientClick="return IsValidate();" onfocus="active(this);"
                            onblur="inactive(this);"></asp:Button>
                        <asp:Button ID="btnClear" TabIndex="12" OnClick="btnClear_Click" runat="server" Text="Clear"
                            CausesValidation="false" Width="120px" onfocus="active(this);" onblur="inactive(this);">
                        </asp:Button>
                        <asp:Button ID="btnList" TabIndex="13" runat="server" Text="Go To List" OnClick="btnList_Click"
                            Width="120px" onfocus="active(this);" onblur="inactive(this);" />
                        <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                            Width="120px" TabIndex="14" onfocus="active(this);" onblur="inactive(this);">
                        </asp:Button>
                        <asp:Button ID="btnPrint" runat="server" onblur="inactive(this);" 
                            OnClick="btnPrint_Click" onfocus="active(this);" TabIndex="14" Text="Print" 
                            Width="120px" Enabled="False" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

