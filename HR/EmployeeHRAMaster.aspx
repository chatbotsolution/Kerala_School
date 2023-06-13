<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmployeeHRAMaster.aspx.cs" Inherits="HR_EmployeeHRAMaster" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function CnfSubmit() {
            var msg = "You are going to Modify the HRA Rate. Do you want to continue ?";
            if (confirm(msg)) {

                return true;
            }
            else {

                return false;
            }
        }
        function valSubmit() {
            var tDate = document.getElementById("<%=txtDate.ClientID %>").value;
            var HRARate = document.getElementById("<%=txtHRARate.ClientID %>").value;


            if (HRARate.trim() == "") {
                alert("Enter HRA Rate");
                document.getElementById("<%=txtHRARate.ClientID %>").focus();
                document.getElementById("<%=txtHRARate.ClientID %>").select();
                return false;
            }
            if (tDate.trim() == "") {
                alert("Enter Date");
                document.getElementById("<%=txtDate.ClientID %>").focus();
                return false;
            }
            else {
                CnfSubmit();
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


        function extractNumber(obj, decimalPlaces, allowNegative) {
            var temp = obj.value;

            // avoid changing things if already formatted correctly
            var reg0Str = '[0-9]*';
            if (decimalPlaces > 0) {
                reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
            } else if (decimalPlaces < 0) {
                reg0Str += '\\.?[0-9]*';
            }
            reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
            reg0Str = reg0Str + '$';
            var reg0 = new RegExp(reg0Str);
            if (reg0.test(temp)) return true;

            // first replace all non numbers
            var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
            var reg1 = new RegExp(reg1Str, 'g');
            temp = temp.replace(reg1, '');

            if (allowNegative) {
                // replace extra negative
                var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
                var reg2 = /-/g;
                temp = temp.replace(reg2, '');
                if (hasNegative) temp = '-' + temp;
            }

            if (decimalPlaces != 0) {
                var reg3 = /\./g;
                var reg3Array = reg3.exec(temp);
                if (reg3Array != null) {
                    // keep only first occurrence of .
                    //  and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
                    var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
                    reg3Right = reg3Right.replace(reg3, '');
                    reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;
                    temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;
                }
            }

            obj.value = temp;
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            HRA Master</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <div style="width: 500px; background-color: #666; padding: 2px;" align="left">
        <div style="background-color: #FFF;" align="left">
            <table style="width: 100%;">
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right" width="150px">
                        Current HRA Rate
                    </td>
                    <td align="left">
                        :&nbsp;<asp:Label ID="lblCurrRate" runat="server" Font-Bold="True" ForeColor="#FF3300"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right" width="150px">
                        Enter New HRA Rate
                    </td>
                    <td align="left">
                        :&nbsp;<asp:TextBox ID="txtHRARate" runat="server" TabIndex="1" Width="100px" onkeypress="return blockNonNumbers(this, event, true, false);"
                            Text="0.00"></asp:TextBox>%
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right" width="150px">
                        Effective Date
                    </td>
                    <td align="left">
                        :&nbsp;<asp:TextBox ID="txtDate" runat="server" TabIndex="2" Width="100px"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpDate" runat="server" Control="txtDate" AutoPostBack="False"
                            ShowMessageBox="True" TextMessage="Enter date" Format="dd mmm yyyy"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right" width="150px">
                        &nbsp;
                    </td>
                    <td align="left">
                        &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                            OnClientClick="return valSubmit();" TabIndex="3" />
                        <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" TabIndex="4" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr id="trMsg" runat="server">
                    <td colspan="2" style="height: 20px;" align="center">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

