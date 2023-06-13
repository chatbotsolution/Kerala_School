<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LoanRecovery.aspx.cs" Inherits="HR_LoanRecovery" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function valSubmit() {
            var transDt = document.getElementById("<%=txtTransactionDate.ClientID %>").value;
            var Employee = document.getElementById("<%=drpEmployee.ClientID %>").selectedIndex;
            var dedHead = document.getElementById("<%=drpDedHead.ClientID %>").selectedIndex;
            var amt = document.getElementById("<%=txtAmount.ClientID %>").value;

            if (transDt.trim() == "") {
                alert("Please provide date !");
                document.getElementById("<%=txtTransactionDate.ClientID %>").focus();
                return false;
            }
            if (Employee == 0) {
                alert("Please select Staff !");
                document.getElementById("<%=drpEmployee.ClientID %>").focus();
                return false;
            }
            if (dedHead == 0) {
                alert("Please select Deduction Head !");
                document.getElementById("<%=drpDedHead.ClientID %>").focus();
                return false;
            }
            if (amt.trim() == "") {
                alert("Please Enter Amount !");
                document.getElementById("<%=txtAmount.ClientID %>").focus();
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

    <asp:UpdatePanel ID="updtEmp" runat="server">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_fm.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <div style="float: left; width: 250px;">
                    <h2>
                        Loan Recovery</h2>
                </div>
                <div style="float: left; padding-top: 3px;">
                    <strong>
                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></strong>
                </div>
            </div>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <%--<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Load/Advance</h2>
        <div style="float: right;">
            <asp:Label ID="lblMsgTop" Font-Bold="true" ForeColor="Green" runat="server" Text=""
                CssClass="tbltxt"></asp:Label>
        </div>
    </div>--%>
            <table width="100%" border="0" cellspacing="5px" cellpadding="0">
                <tr>
                    <td style="width: 150px" align="left">
                        Recovery Date<span class="mandatory">*</span>
                    </td>
                    <td style="width: 5px" align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtTransactionDate" TabIndex="1" runat="server"></asp:TextBox>&nbsp;
                        <rjs:PopCalendar ID="dtpTransDt" runat="server" Control="txtTransactionDate"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Staff Name<span class="mandatory">*</span>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="drpEmployee" TabIndex="2" runat="server" AutoPostBack="true"
                            onselectedindexchanged="drpEmployee_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Type of loan / Advance<span class="mandatory">*</span>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="drpDedHead" TabIndex="3" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="drpDedHead_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                       Total Pending Amount<span class="mandatory">*</span>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtAmount" runat="server" TabIndex="8" MaxLength="9" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
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
                        <asp:RadioButtonList ID="rbtnMode" TabIndex="4" runat="server" AutoPostBack="True"
                            RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0" RepeatLayout="Flow"
                            OnSelectedIndexChanged="rbtnMode_SelectedIndexChanged">
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
                        <asp:DropDownList ID="drpCreditAccount" TabIndex="5" runat="server">
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
                        <asp:TextBox ID="txtInstrumentNo" runat="server" TabIndex="6" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 130px" align="left">
                        Instrument Date<span class="mandatory" id="spInstDt" runat="server">*</span>
                    </td>
                    <td style="width: 5px" align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtInstrumentDt" TabIndex="7" runat="server"></asp:TextBox>&nbsp;
                        <rjs:PopCalendar ID="dtpInstrumentDt" runat="server" Control="txtInstrumentDt"></rjs:PopCalendar>
                    </td>
                </tr>                
                <%--<tr>
                    <td align="left">
                        Loan Recovery Mode<span class="mandatory">*</span>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <div>
                            <div style="float: left;">
                                <asp:RadioButtonList ID="rdbtnlstRecovery" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" OnSelectedIndexChanged="rdbtnlstRecovery_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Text="Direct" Value="D"></asp:ListItem>
                                    <asp:ListItem Text="From Salary" Value="S" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                                &nbsp;&nbsp
                            </div>
                            <div style="float: left; padding-top: 2px;" id="dvInstallment" runat="server">
                                No of Installment :<asp:TextBox ID="txtInstallment" runat="server" TabIndex="8" MaxLength="2"
                                    onkeypress="return blockNonNumbers(this, event, false, true);"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                </tr>--%>
                <tr>
                    <td align="left">
                        Payment Details
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtDetails" runat="server" TabIndex="9" Width="440px" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                    <td valign="top" align="left">
                        <asp:Button ID="btnSubmit" TabIndex="10" runat="server" Text="Save & Add New" OnClientClick="return valSubmit();"
                            onfocus="active(this);" onblur="inactive(this);" OnClick="btnSubmit_Click"></asp:Button>
                        <asp:Button ID="btnClear" TabIndex="11" runat="server" Text="Clear" CausesValidation="false"
                            Width="120px" onfocus="active(this);" onblur="inactive(this);" OnClick="btnClear_Click">
                        </asp:Button>
                        <%--<asp:Button ID="btnList" TabIndex="12" runat="server" Text="Go To List" OnClick="btnList_Click"
                            Width="120px" onfocus="active(this);" onblur="inactive(this);" />--%>
                        <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="120px" TabIndex="13"
                            onfocus="active(this);" onblur="inactive(this);" OnClick="btnCancel_Click"></asp:Button>--%>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
