<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="BankTransactions.aspx.cs" Inherits="Accounts_BankTransactions" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
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
        function isValid() {
            var TranDt = document.getElementById("<%=txtTranDt.ClientID %>").value;
            var Bankhd = document.getElementById("<%=drpBank.ClientID %>").value;
            var transtype = document.getElementById("<%=drpTransType.ClientID %>").value;
            var Amount = document.getElementById("<%=txtAmount.ClientID %>").value;
            var remarks = document.getElementById("<%=txtRemarks.ClientID %>").value;
            if (TranDt.trim() == "") {
                alert("Please Enter Date");
                document.getElementById("<%=txtTranDt.ClientID %>").focus();
                return false;
            }
            if (Bankhd == "0") {
                alert("Please Select Bank");
                document.getElementById("<%=drpBank.ClientID %>").focus();
                return false;
            }
            if (transtype == "0") {
                alert("Please Transaction Type");
                document.getElementById("<%=drpTransType.ClientID %>").focus();
                return false;
            }
            if (Amount.trim() == "") {
                alert("Please Enter Amount");
                document.getElementById("<%=txtAmount.ClientID %>").focus();
                return false;
            }
            if (remarks.trim() == "") {
                alert("Please Enter Remarks");
                document.getElementById("<%=txtRemarks.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    
    </script>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr style="background-color: #ededed;">
            <td width="350" align="left" valign="middle" style="background-color: ">
                <div class="headingcontainor">
                    <h2>
                        Bank Transactions</h2>
                </div>
            </td>
            <td height="35" align="left" valign="middle" colspan="2">
                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <table width="100%" style="padding-left: 10px">
        <tr>
            <td colspan="3" style="height: 15px">
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 120px">
                Transaction Date
            </td>
            <td style="width: 5px;">
                :
            </td>
            <td>
                <asp:TextBox ID="txtTranDt" runat="server" Width="80px" TabIndex="1" ReadOnly="true"></asp:TextBox>
                <rjs:PopCalendar ID="dtpTranDt" runat="server" Control="txtTranDt" To-Today="true"></rjs:PopCalendar>
                &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3" style="height: 15px">
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                Bank Account
            </td>
            <td>
                :
            </td>
            <td>
                <asp:DropDownList ID="drpBank" runat="server" TabIndex="2" OnSelectedIndexChanged="drpBank_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3" style="height: 15px">
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                Transaction Type
            </td>
            <td>
                :
            </td>
            <td>
                <asp:DropDownList ID="drpTransType" runat="server" TabIndex="3">
                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                    <asp:ListItem Value="d">Deposit</asp:ListItem>
                    <asp:ListItem Value="w">Withdrawl</asp:ListItem>
                    <asp:ListItem Value="i">Bank Interest</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="height: 15px">
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                Amount
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="txtAmount" runat="server" TabIndex="4" onkeypress="return blockNonNumbers(this, event, true, true);"
                    Width="100px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="height: 15px">
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                Remarks
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="txtRemarks" Width="300px" runat="server" TabIndex="5"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="height: 15px">
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 51px">
            </td>
            <td>
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" TabIndex="6" Text="Save" OnClientClick="return isValid();"
                    OnClick="btnSave_Click" onfocus="active(this);" onblur="inactive(this);"/>&nbsp;
                <asp:Button ID="btnCancel" runat="server" TabIndex="7" Text="Cancel" OnClick="btnCancel_Click" onfocus="active(this);" onblur="inactive(this);"/>
                &nbsp;
                <asp:Button ID="btnViewList" runat="server" TabIndex="8" Text="View List" OnClick="btnViewList_Click" onfocus="active(this);" onblur="inactive(this);"/>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td colspan="2" align="left">
                <asp:Label ID="lblMsg" runat="server" Visible="False" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

