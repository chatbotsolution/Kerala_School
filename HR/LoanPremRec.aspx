<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LoanPremRec.aspx.cs" Inherits="HR_LoanPremRec" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function cnf() {
            if (confirm("Are you sure to save the data ?\n\nNote:- Verify the details before save.")) {
                return true;
            }
            else {

                return false;
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
        function validatePayment() {
            var payOptn = document.getElementById("<%=rbtnPayment.ClientID%>").value;
            var totAmount = document.getElementById("<%=txtTotAmount.ClientID%>").value;
            var Emp = document.getElementById("<%=drpEmp.ClientID%>").value;
            var Loan = document.getElementById("<%=drpLoan.ClientID%>").value;
            if (payOptn == "1") {
                var PayDate = document.getElementById("<%=txtPayDt.ClientID%>").value;
                var pmtCash = document.getElementById("<%=rbtnPmtCash.ClientID%>").checked;
                var bankName = document.getElementById("<%=txtBankName.ClientID%>").value;
                var instrNo = document.getElementById("<%=txtInstrNo.ClientID%>").value;
                var instrDate = document.getElementById("<%=txtInstrDate.ClientID%>").value;
                var bankHd = document.getElementById("<%=drpBank.ClientID%>").value;
                if (Emp == "0") {
                    alert("Please Select an Employee !");
                    document.getElementById("<%=drpEmp.ClientID%>").focus();
                    return false;
                }
                if (Loan == "0") {
                    alert("Please Select a Loan !");
                    document.getElementById("<%=drpLoan.ClientID%>").focus();
                    return false;
                }
                if (totAmount.trim() == "" || parseFloat(totAmount) == 0) {
                    alert("Please Provide Recovery Amount !");
                    document.getElementById("<%=txtTotAmount.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtPayDt.ClientID %>").disabled == false && PayDate.trim() == "") {
                    alert("Please Provide Received Date !");
                    document.getElementById("<%=txtPayDt.ClientID%>").focus();
                    return false;
                }

                if (document.getElementById("<%=drpBank.ClientID %>").disabled == false && bankHd == "0") {
                    alert("Please Select Bank Head !");
                    document.getElementById("<%=drpBank.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtBankName.ClientID %>").disabled == false && bankName.trim() == "") {
                    alert("Please provide Drawn On Bank name !");
                    document.getElementById("<%=txtBankName.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtInstrNo.ClientID %>").disabled == false && instrNo.trim() == "") {
                    alert("Please provide instrument no. !");
                    document.getElementById("<%=txtInstrNo.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtInstrDate.ClientID %>").disabled == false && instrDate.trim() == "") {
                    alert("Please provide instrument date !");
                    document.getElementById("<%=txtInstrDate.ClientID%>").focus();
                    return false;
                }

                else {
                    return cnf();
                }
            }
            else {
                if (Emp == "0") {
                    alert("Please Select an Employee !");
                    document.getElementById("<%=drpEmp.ClientID%>").focus();
                    return false;
                }
                if (Loan == "0") {
                    alert("Please Select a Loan !");
                    document.getElementById("<%=drpLoan.ClientID%>").focus();
                    return false;
                }
                if (totAmount.trim() == "" || parseFloat(totAmount) == 0) {
                    alert("Please Provide Recovery Amount !");
                    document.getElementById("<%=txtTotAmount.ClientID%>").focus();
                    return false;
                }
                else {
                    return cnf();
                }
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <div style="float: left;">
            <h2>
                Loan Recovery</h2>
            <h3>
                <asp:RadioButtonList ID="rbRecType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                    AutoPostBack="True" OnSelectedIndexChanged="rbRecType_SelectedIndexChanged">
                    <asp:ListItem Text="Premature Recovery" Value="P" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Direct Recovery" Value="D"></asp:ListItem>
                </asp:RadioButtonList>
            </h3>
        </div>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="90%">
                <tr style="background-color: #D3E7EE;">
                    <td colspan="2" align="left" style="padding: 5px; font-weight: bold; height: 3px;
                        font-family: Tahoma, Geneva, sans-serif; color: #000; border: 1px solid #333;
                        background-color: Transparent;">
                        Select an Employee&nbsp;:&nbsp;<asp:DropDownList ID="drpEmp" runat="server" TabIndex="1"
                            AutoPostBack="true" OnSelectedIndexChanged="drpEmp_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;Select a Loan&nbsp;:&nbsp;<asp:DropDownList ID="drpLoan" runat="server" TabIndex="2"
                            AutoPostBack="True" OnSelectedIndexChanged="drpLoan_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:Label ID="lblRecovered" runat="server" Text=""></asp:Label>
                        &nbsp;&nbsp;
                        <asp:Label ID="lblSalary" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr id="row1" runat="server">
                    <td colspan="2">
                        <span style="color: Red;">*&nbsp;NOTE:- Please Enter final recovery amount (Principal
                            + Interest) in the given box.<br />
                        </span><span style="color: Red;">*&nbsp;Select "Debit from salary" for loan deduction
                            from salary or "Cash and Bank Receipt" for Direct Receipt</span><br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <strong>
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" ForeColor="White"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="color: white; background-color: gray; font-size: 14px; width: 50%;">
                        PAYMENT DETAILS
                    </td>
                    <td id="col1" runat="server" align="left" style="color: white; background-color: gray;
                        font-size: 14px; width: 50%;">
                        RECOVERY SCHEDULE (EXISTING)
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rbtnPayment" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" OnSelectedIndexChanged="rbtnPayment_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0" Selected="True">Debit From Salary</asp:ListItem>
                                        <asp:ListItem Value="1">Cash/Bank Receipt</asp:ListItem>
                                        <asp:ListItem Value="2">Debit From Salary (Partly)</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Amount To Be Recovered&nbsp;:&nbsp;<asp:TextBox ID="txtTotAmount" runat="server"
                                        TabIndex="3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel runat="server" ID="pnlPayment" Enabled="false">
                                        <table width="100%" style="border: solid 1px gray;">
                                            <tr>
                                                <td align="left" style="padding-left: 5px; width: 98px;">
                                                    Received Date
                                                </td>
                                                <td>
                                                    :&nbsp;<asp:TextBox ID="txtPayDt" runat="server" Width="90px" TabIndex="4"></asp:TextBox>
                                                    <rjs:PopCalendar ID="dtpPayDt" runat="server" Control="txtPayDt"></rjs:PopCalendar>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="padding-left: 5px; width: 98px">
                                                    Payment Mode
                                                </td>
                                                <td>
                                                    :&nbsp;
                                                    <asp:RadioButton ID="rbtnPmtCash" runat="server" AutoPostBack="true" Checked="true"
                                                        GroupName="pmtmode" OnCheckedChanged="rbtnPmtCash_CheckedChanged" TabIndex="5"
                                                        Text="Cash" />
                                                    <asp:RadioButton ID="rbtnPmtBank" runat="server" AutoPostBack="true" GroupName="pmtmode"
                                                        OnCheckedChanged="rbtnPmtCash_CheckedChanged" TabIndex="6" Text="Bank" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="padding-left: 5px; padding-top: 5px; width: 98px;">
                                                    Bank A/c Head
                                                </td>
                                                <td>
                                                    :&nbsp;<asp:DropDownList runat="server" ID="drpBank" TabIndex="7">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="padding-left: 5px; padding-top: 5px; width: 98px;">
                                                    Drawn On Bank
                                                </td>
                                                <td>
                                                    :&nbsp;<asp:TextBox runat="server" ID="txtBankName" MaxLength="50" TabIndex="8" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="padding-left: 5px; padding-top: 5px; width: 98px;">
                                                    Instrument No
                                                </td>
                                                <td>
                                                    :&nbsp;<asp:TextBox runat="server" ID="txtInstrNo" Width="150px" TabIndex="9" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="padding-left: 5px; padding-top: 5px; width: 98px;">
                                                    Instrument Date
                                                </td>
                                                <td>
                                                    :&nbsp;<asp:TextBox runat="server" ID="txtInstrDate" Width="90px" ReadOnly="true"
                                                        TabIndex="10" />
                                                    <rjs:PopCalendar ID="dtpInstDate" runat="server" Control="txtInstrDate"></rjs:PopCalendar>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" onfocus="active(this);"
                                        onblur="inactive(this);" OnClientClick="return validatePayment();" TabIndex="11"
                                        Width="100px" />
                                    &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                        Width="100px" TabIndex="12" onfocus="active(this);" onblur="inactive(this);" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <asp:GridView ID="grdLoan" runat="server" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Month">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonth" runat="server" Text='<%#Eval("CalMonth")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Year">
                                    <ItemTemplate>
                                        <asp:Label ID="lblYear" runat="server" Text='<%#Eval("CalYear")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount to be Deducted">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("RecAmt")%>'></asp:Label>
                                        <asp:HiddenField ID="hfLoanId" runat="server" Value='<%#Eval("LoanRecId")%>' />
                                        <asp:HiddenField ID="hfGenledgerId" runat="server" Value='<%#Eval("GenLedgerId")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" colspan="2">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

