<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LoanAdvance.aspx.cs" Inherits="HR_LoanAdvance" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .parentDisable
        {
            z-index: 999;
            width: 100%;
            height: 100%;
            display: none;
            position: absolute;
            top: 0;
            left: 0;
        }
        #popup
        {
            width: 300px;
            height: 200px;
            position: relative;
            top: 120px;
            border-style: groove;
            background-color: #F0F0F0;
            border-color: Aqua;
            cursor: auto;
        }
        #close
        {
            position: absolute;
            top: 0;
            right: 0;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function cnf() {

            if (confirm("Are you sure to Save this Loan ?")) {

                return true;
            }
            else {

                return false;
            }
        }
        function pop(div) {
            document.getElementById(div).style.display = 'block';
            var amt = document.getElementById("<%=txtAmount.ClientID %>").value
            document.getElementById("<%=txtPrincipal.ClientID %>").value = amt;
            document.getElementById("<%=txtRI.ClientID %>").focus();
            return false;
        }
        function hide(div) {
            document.getElementById(div).style.display = 'none';
            document.getElementById("<%=txtRI.ClientID %>").value = 0;
            document.getElementById("<%=txtTimePeriod.ClientID %>").value = 0;
            document.getElementById("<%=txtInterest.ClientID %>").focus();
            return false;
        }
        function calInterest() {
            var p = document.getElementById("<%=txtPrincipal.ClientID %>").value;
            var r = document.getElementById("<%=txtRI.ClientID %>").value;
            var t = document.getElementById("<%=txtTimePeriod.ClientID %>").value;
            var mlyInt = 0;
            mlyInt = (parseFloat(p) * parseFloat(r) * ((parseInt(t) / 12) )) / 100;
            document.getElementById("<%=txtInterest.ClientID %>").value = parseFloat(mlyInt).toFixed(2);
            return hide('pop1');
        }
        function valSubmit() {
            var transDt = document.getElementById("<%=txtTransactionDate.ClientID %>").value;
            var Employee = document.getElementById("<%=drpDebtors.ClientID %>").selectedIndex;
            var dedHead = document.getElementById("<%=drpDedHead.ClientID %>").selectedIndex;
            var amt = document.getElementById("<%=txtAmount.ClientID %>").value;
            var month = document.getElementById("<%=drpMonth.ClientID %>").selectedIndex;
            var isDirect = document.getElementById("<%=hfIsDirect.ClientID %>").value;

            if (transDt.trim() == "") {
                alert("Enter Transaction Date");
                document.getElementById("<%=txtTransactionDate.ClientID %>").focus();
                return false;
            }
            else if (Employee == 0) {
                alert("Select a Staff");
                document.getElementById("<%=drpDebtors.ClientID %>").focus();
                return false;
            }
            else if (dedHead == 0) {
                alert("Select Deduction Head");
                document.getElementById("<%=drpDedHead.ClientID %>").focus();
                return false;
            }
            else if (amt.trim() == "" || parseFloat(amt.trim()) == 0) {
                alert("Enter Principal Amount");
                document.getElementById("<%=txtAmount.ClientID %>").focus();
                return false;
            }
            else if (month == 0 && isDirect == "1") {
                alert("Select the Month from which the Deduction will Start From");
                document.getElementById("<%=drpMonth.ClientID %>").focus();
                return false;
            }
            else {
                return cnf();
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

    <asp:UpdatePanel ID="updtEmp" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_fm.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <div style="float: left; width: 250px;">
                    <h2>
                        Loan/Advance</h2>
                </div>
                <div style="float: left; padding-top: 3px;">
                    <strong>
                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></strong>
                </div>
            </div>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="100%" border="0" cellspacing="5px" cellpadding="0">
                <tr>
                    <td style="width: 150px" align="left" valign="baseline">
                        Transaction Date<span class="mandatory">*</span>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:TextBox ID="txtTransactionDate" TabIndex="1" runat="server"></asp:TextBox>&nbsp;
                        <rjs:PopCalendar ID="dtpTransDt" runat="server" Control="txtTransactionDate" 
                            AutoPostBack="True" onselectionchanged="dtpTransDt_SelectionChanged"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="baseline">
                        Staff Name<span class="mandatory">*</span>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:DropDownList ID="drpDebtors" TabIndex="2" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="drpDebtors_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="baseline">
                        Type of Loan / Advance<span class="mandatory">*</span>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:DropDownList ID="drpDedHead" TabIndex="3" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="drpDedHead_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="baseline">
                        Payment Mode<span class="mandatory">*</span>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:RadioButtonList ID="rbtnMode" TabIndex="4" runat="server" AutoPostBack="True"
                            RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0" RepeatLayout="Flow"
                            OnSelectedIndexChanged="rbtnMode_SelectedIndexChanged">
                            <asp:ListItem Value="C">Cash</asp:ListItem>
                            <asp:ListItem Selected="True" Value="B">Bank</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="baseline">
                        Bank Name<span class="mandatory" id="spBankNm" runat="server">*</span>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:DropDownList ID="drpCreditAccount" TabIndex="5" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="baseline">
                        Instrument No.<span class="mandatory" id="spInstNo" runat="server">*</span>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:TextBox ID="txtInstrumentNo" runat="server" TabIndex="6" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="baseline">
                        Instrument Date<span class="mandatory" id="spInstDt" runat="server">*</span>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:TextBox ID="txtInstrumentDt" TabIndex="7" runat="server"></asp:TextBox>&nbsp;
                        <rjs:PopCalendar ID="dtpInstrumentDt" runat="server" Control="txtInstrumentDt"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="baseline">
                        Principal Amount<span class="mandatory">*</span>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:TextBox ID="txtAmount" runat="server" TabIndex="8" onkeypress="return blockNonNumbers(this, event, true, false);"
                            Text="0" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="baseline">
                        Loan Recovery Mode<span class="mandatory">*</span>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:RadioButtonList ID="rdbtnlstRecovery" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnSelectedIndexChanged="rdbtnlstRecovery_SelectedIndexChanged"
                            AutoPostBack="true" TabIndex="9">
                            <asp:ListItem Text="Direct" Value="D"></asp:ListItem>
                            <asp:ListItem Text="From Salary" Value="S" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trInt" runat="server">
                    <td align="left" valign="baseline">
                        &nbsp;
                    </td>
                    <td align="left" valign="baseline" style="padding-left: 10px;">
                        <table style="border: solid 1px; background-color: #D3E7EE;">
                            <tr>
                                <td align="left" valign="baseline">
                                    Total Interest Amount
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtInterest" runat="server" TabIndex="10" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Width="100px" Text="0" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"></asp:TextBox>
                                    <input type="submit" value="Calculate Interest" onclick="return pop('pop1');" onfocus="active(this);"
                                        onblur="inactive(this);" tabindex="10" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    No of EMI for Principal Recovery
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtPrincipalEMI" runat="server" TabIndex="11" MaxLength="2"
                                        onkeypress="return blockNonNumbers(this, event, false, false);" Width="100px"
                                        Text="0" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    No of EMI for Interest Recovery
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtInterestEMI" runat="server" TabIndex="12" MaxLength="2"
                                        onkeypress="return blockNonNumbers(this, event, false, false);" Width="100px"
                                        Text="0" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    Deduction Start From
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:DropDownList ID="drpMonth" runat="server" Width="100px" TabIndex="13">
                                        <asp:ListItem Text="- SELECT -" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="APR" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="MAY" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="JUN" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="JUL" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="AUG" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="SEP" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="OCT" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="NOV" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="DEC" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="JAN" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="FEB" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="MAR" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                    Year :
                                    <asp:DropDownList ID="drpYear" runat="server" TabIndex="13" Width="100px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="baseline">
                        Loan Details
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:TextBox ID="txtDetails" runat="server" TabIndex="14" Width="440px" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="baseline">
                        &nbsp;
                    </td>
                    <td align="left" valign="baseline">
                        &nbsp;&nbsp;<asp:Button ID="btnSubmit" TabIndex="15" runat="server" Text="Save & Add New"
                            OnClientClick="return valSubmit();" onfocus="active(this);" onblur="inactive(this);"
                            OnClick="btnSubmit_Click"></asp:Button>
                        <asp:Button ID="btnClear" TabIndex="16" runat="server" Text="Clear" onfocus="active(this);"
                            onblur="inactive(this);" OnClick="btnClear_Click"></asp:Button>
                        <asp:Button ID="btnList" TabIndex="17" runat="server" Text="Go to List" onfocus="active(this);"
                            onblur="inactive(this);" OnClick="btnList_Click"></asp:Button>
                        <asp:HiddenField ID="hfIsDirect" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
            <div id="pop1" class="parentDisable">
                <center>
                    <div id="popup">
                        <a href="#" onclick="return hide('pop1')">
                            <img id="close" src="../images/refresh_icon.png" alt="Close" />
                        </a>
                        <div align="center">
                            <h2>
                                Loan Calculator</h2>
                        </div>
                        <br />
                        <table>
                            <tr>
                                <td align="left" valign="baseline" width="100px">
                                    Principal Amount
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtPrincipal" runat="server" TabIndex="10" Width="100px"
                                        Text="0" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    Rate of Interest (p/a)
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtRI" runat="server" TabIndex="10" MaxLength="5" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Width="100px" Text="0" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"></asp:TextBox>
                                    %
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    Time Period
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtTimePeriod" runat="server" TabIndex="10" MaxLength="3"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Width="100px"
                                        Text="0" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"></asp:TextBox>
                                    months
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    &nbsp;
                                </td>
                                <td align="left" valign="baseline">
                                    &nbsp;&nbsp;<asp:Button ID="btnCalculate" TabIndex="10" runat="server" Text="Calculate"
                                        onfocus="active(this);" onblur="inactive(this);" OnClientClick="return calInterest();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

