<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="MiscExpensesEntry.aspx.cs" Inherits="Accounts_MiscExpensesEntry" %>

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
            var transDt = document.getElementById("<%=txtExpDate.ClientID %>").value;
            var Amount = document.getElementById("<%=txtAmount.ClientID %>").value;
            var TransAmt = document.getElementById("<%=txtTransCharge.ClientID %>").value;
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
            if (transDt.trim() == "") {
                alert("Please Enter Transaction Date");
                document.getElementById("<%=txtAmount.ClientID %>").focus();
                return false;
            }
            if (Amount.trim() == "") {
                alert("Please Enter Amount");
                document.getElementById("<%=txtExpDate.ClientID %>").focus();
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


        function ToggleAll(e) {
            if (e.checked) {
                CheckAll();
            }
            else {
                ClearAll();
            }
        }

        function CheckAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = true;
                }
            }
            ml.toggleAll.checked = true;
        }
        function ClearAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = false;
                }
            }
            ml.toggleAll.checked = false;
        }

        function Cal() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            var totalcol1 = 0;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    if (e.checked = true) {
                        if (e.id == "lblAmt") {
                            totalcol1 += parseFloat(e.value);
                        }
                    }
                }

            }
            document.getElementById('<%= txtAmount.ClientID %>').innerHTML = totalcol1.toFixed(2).toString();
        }
        
        
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Miscellaneous Expenditure</h2>
            &nbsp;<asp:Label ID="lblMsg1" runat="server" Font-Bold="true"></asp:Label>
    </div>
    <div class="linegap">
        <img src="../images/mask.gif" height="5" width="10" />
    </div>
    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="90%" cellpadding="2px" cellspacing="4px">
                <%--<tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="3">
                        <asp:RadioButton ID="optCashBank" runat="server" GroupName="CashCredit" Text="Cash/Bank"
                            AutoPostBack="true" Checked="True" Font-Bold="true" OnCheckedChanged="optCashBank_CheckedChanged"
                            TabIndex="1" />&nbsp;&nbsp;
                        <asp:RadioButton ID="optCredit" runat="server" GroupName="CashCredit" Font-Bold="true"
                            Text="Credit" OnCheckedChanged="optCashBank_CheckedChanged" AutoPostBack="true"
                            TabIndex="1" />
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
                        <asp:DropDownList ID="drpAcHead" runat="server" Width="250px" AutoPostBack="True"
                            OnSelectedIndexChanged="drpAcHead_SelectedIndexChanged" TabIndex="1">
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
                        <asp:DropDownList ID="drpCreditHead" runat="server" Width="250px" TabIndex="3" >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Instrument No.
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtInstrumentNo" runat="server" TabIndex="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 130px" align="left">
                        Instrument Date
                    </td>
                    <td style="width: 5px" align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtInstrumentDt" TabIndex="5" runat="server"></asp:TextBox>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Receipt/Voucher No.<span class="mandatory" id="Span6" runat="server">*</span>
                    </td>
                    <td align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtRcptVoucherNo" runat="server" TabIndex="6"></asp:TextBox>
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
                        <asp:TextBox ID="txtExpDate" runat="server" ValidationGroup="vgSubmit" Width="165px" ReadOnly="true"
                            TabIndex="7"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpExpenseDt" runat="server" Control="txtExpDate" Format="dd mmm yyyy" To-Today="true">
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
                            onkeypress="return blockNonNumbers(this, event, true, true);" TabIndex="8"></asp:TextBox>
                        <asp:RadioButtonList ID="rbtnRound" runat="server" AutoPostBack="True" 
                            CellPadding="0" CellSpacing="0" 
                            OnSelectedIndexChanged="rbtnRound_SelectedIndexChanged" 
                            RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Selected="True" Value="E">Exact Amount</asp:ListItem>
                            <asp:ListItem Value="R">Round Off Amount</asp:ListItem>
                        </asp:RadioButtonList>
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
                    <td>
                        Transaction Details<span class="mandatory" id="Span4" runat="server">*</span>
                    </td>
                    <td style="width: 5px;">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtExpDetails" runat="server" Height="50px" TextMode="MultiLine"
                            Width="250px" MaxLength="99" TabIndex="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td colspan="3">
                <asp:Panel ID="pnlEmpDed" runat="server" Visible="false">
                <table width="70%">
                    <tr>
                           <td>
                               
                               &nbsp;
                               Month Upto : 
                               <asp:DropDownList ID="drpMonth" runat="server" AutoPostBack="true" 
                                   onselectedindexchanged="drpMonth_SelectedIndexChanged">
                                        <asp:ListItem Text="--Prev FY--" Value="0"></asp:ListItem>
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
                               &nbsp;
                               <asp:Button ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click"/>
                               </td>
                    </tr>
                    <tr>
                        <td>
                        
                                <asp:GridView ID="grdEmpDed" runat="server" AutoGenerateColumns="False" Width="100%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <%--<HeaderTemplate>
                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox"
                                                    value="ON" />
                                                </HeaderTemplate>--%>
                                                
                                                <HeaderTemplate>
                                                        <asp:CheckBox Text="" runat="server" ID="optEmpAll" AutoPostBack="true" 
                                                           oncheckedchanged="optEmpAll_CheckedChanged" />
                                                </HeaderTemplate>
                                                
                                                <%--<ItemTemplate>
                                                <input name="Checkb" type="checkbox" value='<%# Eval("EmpId") %>-<%# Eval("Amount") %>' />
                                                </ItemTemplate>--%>
                                                
                                                <ItemTemplate>
                                                        <asp:CheckBox Text="" runat="server" ID="optEmp" AutoPostBack="true" 
                                                        TabIndex="-1" oncheckedchanged="optEmp_CheckedChanged" />
                                                </ItemTemplate>
                                                
                                                <HeaderStyle HorizontalAlign="Left" Width="5px" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="5px"/>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Employee Name">
                                                <ItemTemplate>
                                                    <%#Eval("SevName")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pending Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmt" runat="server" Text='<%#Eval("Amount")%>'></asp:Label>
                                                    <asp:Label ID="lblEmpId" runat="server" Text='<%#Eval("EmpId")%>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="50px" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

                </td>
                </tr>
                <tr>
                    <td style="width: 5px;">
                        &nbsp; <asp:HiddenField ID="hfDedDesc" runat="server" Value="" />
                        <asp:HiddenField ID="hfExpId" runat="server" Value="" />
                    </td>
                    <td valign="top" colspan="2">
                        &nbsp;&nbsp;
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                            OnClientClick="return IsValid();" TabIndex="11" Width="80px" onfocus="active(this);"
                            onblur="inactive(this);" />
                        &nbsp;<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click"
                            TabIndex="12" Width="80px" onfocus="active(this);" onblur="inactive(this);" />
                        &nbsp;<asp:Button ID="btnShowList" runat="server" Text="Show List" OnClick="btnShowList_Click"
                            TabIndex="13" Width="80px" onfocus="active(this);" onblur="inactive(this);" />
                            <br />
                            <br />
                        &nbsp;<asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnShowList" EventName="Click" />
            <asp:PostBackTrigger ControlID="drpAcHead" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

