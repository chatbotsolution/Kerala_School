<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="HostExpensesEntry.aspx.cs" Inherits="Hostel_HostExpensesEntry" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function IsValid() {
            var AcHead = document.getElementById("<%=drpAcHead.ClientID %>").value;
            var CrHead = document.getElementById("<%=drpCreditHead.ClientID %>").value;
            var InstNo = document.getElementById("<%=txtInstrumentNo.ClientID %>").value;
            var InstDt = document.getElementById("<%=txtInstrumentDt.ClientID %>").value;
            var RVNo = document.getElementById("<%=txtRcptVoucherNo.ClientID %>").value;
            var Amount = document.getElementById("<%=txtAmount.ClientID %>").value;
            var Desc = document.getElementById("<%=txtExpDetails.ClientID %>").value;

            if (AcHead == "0") {
                alert("Please Select Expense Account Head ");
                document.getElementById("<%=drpAcHead.ClientID %>").focus();
                return false;
            }

            if (CrHead == "0") {
                alert("Please Select Credit Account Head ");
                document.getElementById("<%=drpCreditHead.ClientID %>").focus();
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
            if (RVNo.trim() == "") {
                alert("Please Enter Receipt/Voucher No ");
                document.getElementById("<%=txtRcptVoucherNo.ClientID %>").focus();
                return false;
            }
            if (Amount.trim() == "") {
                alert("Please Enter Amount");
                document.getElementById("<%=txtAmount.ClientID %>").focus();
                return false;
            }
            if (Desc.trim() == "") {
                alert("Please Enter Transaction Details ");
                document.getElementById("<%=txtExpDetails.ClientID %>").focus();
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

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Hostel Expenditure</h2>
    </div>
    <div class="linegap">
        <img src="../images/mask.gif" height="5" width="10" />
    </div>
    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
       
        <ContentTemplate>
    <table width="90%" cellpadding="2px" cellspacing="4px">
        <tr>
            <td>
            </td>
            <td colspan="2">
                <asp:Label ID="lblMsg" runat="server" CssClass="tbltxt" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:RadioButton ID="optCashBank" runat="server" GroupName="CashCredit" Text="Cash/Bank"
                    AutoPostBack="true" Checked="True" Font-Bold="true" OnCheckedChanged="optCashBank_CheckedChanged" />&nbsp;&nbsp;
                <asp:RadioButton ID="optCredit" runat="server" GroupName="CashCredit" Font-Bold="true"
                    Text="Credit" OnCheckedChanged="optCashBank_CheckedChanged" AutoPostBack="true" />
            </td>
        </tr>
        <tr>
            <td style="width: 140px;">
                Expense Account Head<span class="mandatory" id="Span1" runat="server">*</span>
            </td>
            <td style="width: 5px;">
                :
            </td>
            <td align="left">
                <asp:DropDownList ID="drpAcHead" runat="server" Width="250px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="innertbltxt">
                Payment Mode<span class="mandatory">*</span>
            </td>
            <td align="left" class="innertbltxt">
                :
            </td>
            <td class="pageLabel" align="left">
                <asp:RadioButtonList ID="rbtnMode" TabIndex="2" runat="server" OnSelectedIndexChanged="rbtnMode_SelectedIndexChanged"
                    AutoPostBack="True" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0"
                    RepeatLayout="Flow">
                    <asp:ListItem Value="C">Cash</asp:ListItem>
                    <asp:ListItem Selected="True" Value="B">Bank</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                Credit Account Head<span class="mandatory" id="Span2" runat="server">*</span>
            </td>
            <td style="width: 5px;">
                :
            </td>
            <td>
                <asp:DropDownList ID="drpCreditHead" runat="server" Width="250px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="innertbltxt" align="left">
                Instrument No.
            </td>
            <td class="innertbltxt" align="left">
                :
            </td>
            <td align="left">
                <asp:TextBox ID="txtInstrumentNo" runat="server" TabIndex="6"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 130px" class="innertbltxt" align="left">
                Instrument Date
            </td>
            <td style="width: 5px" align="left" class="innertbltxt">
                :
            </td>
            <td align="left">
                <asp:TextBox ID="txtInstrumentDt" TabIndex="1" runat="server"></asp:TextBox>&nbsp;
                <rjs:PopCalendar ID="dtpInstrumentDt" runat="server" Control="txtInstrumentDt" Format="dd mmm yyyy">
                </rjs:PopCalendar>
            </td>
        </tr>
        <tr>
            <td class="innertbltxt" align="left">
                Receipt/Voucher No.<span class="mandatory" id="Span6" runat="server">*</span>
            </td>
            <td class="innertbltxt" align="left">
                :
            </td>
            <td align="left">
                <asp:TextBox ID="txtRcptVoucherNo" runat="server" TabIndex="7"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Transaction Date<span class="mandatory" id="Span3" runat="server">*</span>
            </td>
            <td style="width: 5px;">
                :
            </td>
            <td>
                <asp:TextBox ID="txtExpDate" runat="server" ValidationGroup="vgSubmit" Width="165px"></asp:TextBox>
                <rjs:PopCalendar ID="dtpExpenseDt" runat="server" Control="txtExpDate" Format="dd mmm yyyy">
                </rjs:PopCalendar>
            </td>
        </tr>
        <tr>
            <td>
                Amount<span class="mandatory" id="Span5" runat="server">*</span>
            </td>
            <td style="width: 5px;">
                :
            </td>
            <td>
                <asp:TextBox ID="txtAmount" runat="server" Width="165px" ValidationGroup="vgSubmit"
                    onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Transaction Details<span class="mandatory" id="Span4" runat="server">*</span>
            </td>
            <td style="width: 5px;">
                :
            </td>
            <td>
                <asp:TextBox ID="txtExpDetails" runat="server" Height="50px" TextMode="MultiLine"
                    Width="250px" MaxLength="99"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td valign="top" colspan="2">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                    OnClientClick="return IsValid();" />
                &nbsp;
                <asp:Button ID="btnReset" runat="server" Text=" Reset  " OnClick="btnReset_Click" />
                &nbsp;
                <asp:Button ID="btnShowList" runat="server" Text="Show List" OnClick="btnShowList_Click" />
            </td>
        </tr>
    </table>
    </ContentTemplate> 
     <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnShowList" EventName="Click" />
        
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
