<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpSalStructure.aspx.cs" Inherits="HR_EmpSalStructure" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="bedcromb">
        <asp:Label ID="lblTitle" runat="server" Text="Employee Salary Structure"></asp:Label></div>
    <table style="height: 450px;" width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="padding: 30px;" align="center">
                            <div style="width: 680px; background-color: #666; padding: 2px; margin: 0 auto;">
                                <div style="background-color: #FFF; padding: 10px;">
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                Dept Name
                                            </td>
                                            <td align="left">
                                                :&nbsp;<asp:DropDownList ID="drpDeptName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpDeptName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left">
                                                Employe Name
                                            </td>
                                            <td align="left">
                                                :&nbsp;<asp:DropDownList ID="drpEmpName" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Effective Date
                                            </td>
                                            <td>
                                                :&nbsp;<asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                                                <rjs:PopCalendar ID="dob" runat="server" Control="txtFrom" AutoPostBack="False" ShowMessageBox="True"
                                                    TextMessage="Enter date"></rjs:PopCalendar>
                                            </td>
                                            <td>
                                                Pay
                                            </td>
                                            <td>
                                                :&nbsp;<asp:TextBox ID="txtPay" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                GP
                                            </td>
                                            <td align="left">
                                                :&nbsp;<asp:TextBox ID="txtGp" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                DA
                                            </td>
                                            <td align="left">
                                                :&nbsp;<asp:TextBox ID="txtDa" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                HR
                                            </td>
                                            <td align="left">
                                                :&nbsp;<asp:TextBox ID="txtHr" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                Medicine
                                            </td>
                                            <td align="left">
                                                :&nbsp;<asp:TextBox ID="txtMedicine" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                EPF
                                            </td>
                                            <td align="left">
                                                :&nbsp;<asp:TextBox ID="txtEpf" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" >
                                                Remark 
                                            </td>
                                            <td align="left" colspan="3">:&nbsp;<asp:TextBox ID="txtRemark" runat="server" Width="96%"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="4">
                                                &nbsp;<strong>Deductions:-</strong> <script language="javascript">

                                                                                        function isValid() {
                                                                                            var name = document.getElementById("<%=drpDeptName.ClientID %>").value;
                                                                                            var empname = document.getElementById("<%=drpEmpName.ClientID %>").value;
                                                                                            var from = document.getElementById("<%=txtFrom.ClientID %>").value;
                                                                                            var pay = document.getElementById("<%=txtPay.ClientID %>").value;
                                                                                            var gp = document.getElementById("<%=txtGp.ClientID %>").value;
                                                                                            var da = document.getElementById("<%=txtDa.ClientID %>").value;
                                                                                            var hr = document.getElementById("<%=txtHr.ClientID %>").value;
                                                                                            var medicine = document.getElementById("<%=txtMedicine.ClientID %>").value;
                                                                                            var epf = document.getElementById("<%=txtEpf.ClientID %>").value;

                                                                                            if (name == "0") {
                                                                                                alert("Select Dept Name !");
                                                                                                document.getElementById("<%=drpDeptName.ClientID %>").focus();
                                                                                                return false;
                                                                                            }
                                                                                            if (empname == "0") {
                                                                                                alert("Select Emp Name !");
                                                                                                document.getElementById("<%=drpEmpName.ClientID %>").focus();
                                                                                                return false;
                                                                                            }
                                                                                            if (from.trim() == "") {
                                                                                                alert("Enter Date !");
                                                                                                document.getElementById("<%=txtFrom.ClientID %>").focus();
                                                                                                return false;
                                                                                            }
                                                                                            if (pay.trim() == "") {
                                                                                                alert("Enter Pay !");
                                                                                                document.getElementById("<%=txtPay.ClientID %>").focus();
                                                                                                return false;
                                                                                            }
                                                                                            if (gp.trim() == "") {
                                                                                                alert("Enter GP !");
                                                                                                document.getElementById("<%=txtGp.ClientID %>").focus();
                                                                                                return false;
                                                                                            }
                                                                                            if (da.trim() == "") {
                                                                                                alert("Enter DA !");
                                                                                                document.getElementById("<%=txtDa.ClientID %>").focus();
                                                                                                return false;
                                                                                            }
                                                                                            if (hr.trim() == "") {
                                                                                                alert("Enter HR !");
                                                                                                document.getElementById("<%=txtHr.ClientID %>").focus();
                                                                                                return false;
                                                                                            }
                                                                                            if (medicine.trim() == "") {
                                                                                                alert("Enter Medicine !");
                                                                                                document.getElementById("<%=txtMedicine.ClientID %>").focus();
                                                                                                return false;
                                                                                            }
                                                                                            if (epf.trim() == "") {
                                                                                                alert("Enter EPF !");
                                                                                                document.getElementById("<%=txtEpf.ClientID %>").focus();
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
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="4">
                                                Deduction Type:<asp:DropDownList ID="drpDeductionType" runat="server">
                                                </asp:DropDownList> 
                                                <asp:Button ID="btnAdd" runat="server" OnClick="btnSubmit_Click" 
                                                    OnClientClick="return isValid();" Text="Add" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                                    OnClientClick="return isValid();" />
                                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                                                <asp:Button ID="btnDetaails" runat="server" Text="Details" OnClick="btnDetaails_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnsubmit" />
                    </Triggers>
                </asp:UpdatePanel>
                <br />
            </td>
        </tr>
    </table>
</asp:Content>

