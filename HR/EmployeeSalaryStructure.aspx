<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmployeeSalaryStructure.aspx.cs" Inherits="HR_EmployeeSalaryStructure" %>

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
            width: 350px;
            height: 450px;
            position: relative;
            top: 100px;
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
        function GetSelectedRow(e) {
            var row = e.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var amt = row.cells[2].innerHTML;
            var basic = document.getElementById('<%=txtBasic.ClientID%>').value;
            var gross = document.getElementById('<%=txtGross.ClientID%>').value;
            var dedPerEmp = document.getElementById('<%=hfDedPerEmp.ClientID%>').value;
            var dedPerOrg = document.getElementById('<%=hfDedPerOrg.ClientID%>').value;
            if (e.checked) {
                gross = parseFloat(gross) + parseFloat(amt);
            }
            else {
                gross = parseFloat(gross) - parseFloat(amt);
            }
            var dedEmp = parseFloat(dedPerEmp) * parseFloat(gross) / 100;
            var dedOrg = parseFloat(dedPerOrg) * parseFloat(gross) / 100;
            document.getElementById('<%=txtGross.ClientID%>').value = gross;
            document.getElementById('<%=txtDedEmp.ClientID%>').value = dedEmp;
            document.getElementById('<%=txtDedOrg.ClientID%>').value = dedOrg;
            return false;
        }
        function CalculateDed() {
            var dedEmp = document.getElementById('<%=txtDedEmp.ClientID%>').value;
            var dedOrg = document.getElementById('<%=txtDedOrg.ClientID%>').value;
            document.getElementById('<%=txtAmtEmp.ClientID%>').value = dedEmp;
            document.getElementById('<%=txtAmtOrg.ClientID%>').value = dedOrg;
            return true;
        }
        function pop(div) {
            document.getElementById(div).style.display = 'block';
            var amt = document.getElementById("<%=txtPay.ClientID %>").value;
            var dedPerEmp = document.getElementById('<%=hfDedPerEmp.ClientID%>').value;
            var dedPerOrg = document.getElementById('<%=hfDedPerOrg.ClientID%>').value;
            var emp = parseFloat(dedPerEmp) * parseFloat(amt) / 100;
            var org = parseFloat(dedPerOrg) * parseFloat(amt) / 100;
            document.getElementById("<%=txtBasic.ClientID %>").value = amt;
            document.getElementById("<%=txtGross.ClientID %>").value = amt;
            document.getElementById('<%=txtDedEmp.ClientID%>').value = emp;
            document.getElementById('<%=txtDedOrg.ClientID%>').value = org;
            return false;
        }
        function hide(div) {
            document.getElementById(div).style.display = 'none';
            document.getElementById("<%=txtBasic.ClientID %>").value = 0;
            document.getElementById("<%=txtGross.ClientID %>").value = 0;
            document.getElementById("<%=btnCalculate.ClientID %>").focus();
            return false;
        }
        function valAdd() {
            var DeductionType = document.getElementById("<%=drpDeductionType.ClientID %>").value;
            if (DeductionType.trim() == "0") {
                alert("Select a Deduction type to Add");
                document.getElementById("<%=drpDeductionType.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
        function valSubmit() {
            var Employee = document.getElementById("<%=drpEmployee.ClientID %>").value;
            var FromDt = document.getElementById("<%=txtFromDt.ClientID %>").value;
            var Pay = document.getElementById("<%=txtPay.ClientID %>").value;
            var Employee = document.getElementById("<%=drpEmployee.ClientID %>").value;
            var Employee = document.getElementById("<%=drpEmployee.ClientID %>").value;
            var Employee = document.getElementById("<%=drpEmployee.ClientID %>").value;
            var Employee = document.getElementById("<%=drpEmployee.ClientID %>").value;
            if (Employee.trim() <= 0) {
                alert("Select an Employee");
                document.getElementById("<%=drpEmployee.ClientID %>").focus();
                return false;
            }
            if (FromDt.trim() == "") {
                alert("Enter Date");
                document.getElementById("<%=txtFromDt.ClientID %>").focus();
                return false;
            }
            if (Pay.trim() == "") {
                alert("Enter Basic Pay of the Employee");
                document.getElementById("<%=txtPay.ClientID %>").focus();
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
        function valid() {
            var flag;
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {

                    if (e.checked) {
                        flag = true;
                        break;
                    }
                    else
                        flag = false;
                }
            }
            //alert(flag);
            if (flag == true)
                return true;
            else {
                alert("Select any Record");
                return false;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Employee Salary Structure</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div style="padding: 20px; font-size: 10px; width: 98%;">
                <div style="width: 97%; background-color: #666; padding: 2px;">
                    <div style="background-color: #FFF; padding: 10px;">
                        <table style="width: 100%;">
                            <tr>
                                <td style="height: 10px;">
                                    <strong>Designation&nbsp;:</strong>&nbsp;<asp:DropDownList AutoPostBack="true" ID="drpDesignation"
                                        runat="server" OnSelectedIndexChanged="drpDesignation_SelectedIndexChanged" TabIndex="1">
                                    </asp:DropDownList>
                                    <strong>Employee&nbsp;:</strong>&nbsp;<asp:DropDownList ID="drpEmployee" runat="server"
                                        TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="drpEmployee_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:RadioButton ID="rbtnNewSal" Text="New" runat="server" Checked="true" AutoPostBack="True" 
                                        GroupName="Sal" oncheckedchanged="rbtnNewSal_CheckedChanged" />
                                    <asp:RadioButton ID="rbtnModSal" Text="Existing" runat="server" GroupName="Sal" AutoPostBack="True"
                                    oncheckedchanged="rbtnNewSal_CheckedChanged" />
                                    <br />
                                    ** Choose the "NEW" option to set new salary structure for the selected employee (will generate arrer if the 
                                    Effective Date is prior to the salary paid month)
                                    <br />
                                    ** Choose the "Existing" option to modify the existing salary structure(if the salary has not been generated)
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="height: 5px;">
                </div>
                <div style="width: 97%; background-color: #666; padding: 2px;">
                    <div style="background-color: #FFF; padding: 10px;">
                        <table width="100%" cellpadding="0" cellspacing="0" style="font-size: 12px;">
                            <tr style="background-color: #00628c; color: White; font-size: 17px; font-weight: bold;
                                height: 20px;">
                                <td align="center" style="color: White;">
                                    SALARY STRUCTURE
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;">
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <strong>Effective Date</strong>&nbsp;:&nbsp;<asp:TextBox ID="txtFromDt" Font-Size="11px"
                                        runat="server" ReadOnly="true" TabIndex="3"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpFromDate" runat="server" Control="txtFromDt" AutoPostBack="False"
                                        Format="dd mmm yyyy" TabIndex="3"></rjs:PopCalendar>
                                    <strong>Basic</strong>&nbsp;:&nbsp;<asp:TextBox ID="txtPay" runat="server" TabIndex="4"
                                        AutoPostBack="true" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        OnTextChanged="txtPay_TextChanged" onblur="if (this.value == '') {this.value = '0';}"
                                        onfocus="if(this.value == '0') {this.value = '';}" Text="0"></asp:TextBox>
                                    <strong>DA</strong>&nbsp;:&nbsp;<asp:TextBox ID="txtDA" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        TabIndex="5" Text="0" OnTextChanged="txtDA_TextChanged" AutoPostBack="true" onblur="if (this.value == '') {this.value = '0';}"
                                        onfocus="if(this.value == '0') {this.value = '';}"></asp:TextBox>
                                    <asp:HiddenField ID="hfDA" runat="server" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;">
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" style="font-size: 12px;">
                            <tr>
                                <td>
                                    <div style="width: 42%; float: left;">
                                        <table width="100%" cellpadding="1" cellspacing="1" style="font-size: 12px;">
                                            <tr style="background-color: #00628c; color: White; font-size: 17px; font-weight: bold;
                                                height: 20px;">
                                                <td align="center" style="color: White;">
                                                    ALLOWANCE DETAILS
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 5px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Allowance</strong>&nbsp;:&nbsp;<asp:DropDownList ID="drpAllowance" runat="server"
                                                        AutoPostBack="True" Width="150px" TabIndex="6" OnSelectedIndexChanged="drpAllowance_SelectedIndexChanged"
                                                        Enabled="False">
                                                    </asp:DropDownList>
                                                    &nbsp;<strong>Amount</strong>&nbsp;:&nbsp;<asp:TextBox ID="txtAllowance" Width="60px"
                                                        TabIndex="7" runat="server" onblur="if (this.value == '') {this.value = '0';}"
                                                        onfocus="if(this.value == '0') {this.value = '';}" Text="0" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        Enabled="False"></asp:TextBox>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnAddAllowance" runat="server" Text="Add" TabIndex="8" OnClick="btnAddAllowance_Click"
                                                        Enabled="False" onfocus="active(this);" onblur="inactive(this);" Width="80px" />
                                                    <asp:Button ID="btnClearAllowance" runat="server" Text="Clear" TabIndex="8" onfocus="active(this);"
                                                        onblur="inactive(this);" Width="80px" Enabled="False" OnClick="btnClearAllowance_Click" />
                                                    <asp:HiddenField ID="hfAllwPer" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hfPerAmt" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="grdAllowance" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        DataKeyNames="AllowanceId" OnRowDeleting="grdAllowance_RowDeleting">
                                                        <Columns>
                                                            <asp:BoundField DataField="AllowanceId" Visible="false" />
                                                            <asp:TemplateField HeaderText="Allowance Name">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEditAllowance" runat="server" CommandArgument='<%#Eval("AllowanceId")%>'
                                                                        Text='<%#Eval("Allowance") %>' Font-Underline="true" OnClick="btnEditAllowance_Click"
                                                                        CausesValidation="false" ToolTip='<%#"Click to Edit" +" "+ Eval("Allowance").ToString()+" "+"Allowance" %>'></asp:LinkButton>
                                                                    <asp:HiddenField ID="hdnAllowanceId" runat="server" Value='<%#Eval("AllowanceId") %>' />
                                                                    <asp:HiddenField ID="hfAmt" runat="server" Value='<%#Eval("Amount") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Amount" DataField="Allw" HeaderStyle-HorizontalAlign="Right"
                                                                DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Middle"
                                                                HeaderStyle-VerticalAlign="Middle" />
                                                            <asp:CommandField ButtonType="Button" ShowDeleteButton="true" HeaderText="Delete"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            Add Allowance to the List
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="width: 1%; float: left;">
                                        &nbsp;
                                    </div>
                                    <div style="width: 57%; float: left">
                                        <table width="100%" cellpadding="1" cellspacing="1" style="font-size: 12px;">
                                            <tr style="background-color: #00628c; color: White; font-size: 17px; font-weight: bold;
                                                height: 20px;">
                                                <td align="center" style="color: White;">
                                                    DEDUCTION DETAILS
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 5px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <strong>Deduction</strong>&nbsp;:&nbsp;<asp:DropDownList ID="drpDeductionType" runat="server"
                                                        AutoPostBack="True" Width="150px" TabIndex="9" OnSelectedIndexChanged="drpDeductionType_SelectedIndexChanged"
                                                        Enabled="False">
                                                    </asp:DropDownList>
                                                    <strong>Empolyee</strong>&nbsp;:&nbsp;<asp:TextBox ID="txtAmtEmp" Width="70px" TabIndex="10"
                                                        runat="server" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                        Text="0" onkeypress="return blockNonNumbers(this, event, true, false);" Enabled="False"></asp:TextBox>
                                                    <strong>Employer</strong>&nbsp;:&nbsp;<asp:TextBox ID="txtAmtOrg" Width="70px" TabIndex="11"
                                                        runat="server" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                        Text="0" onkeypress="return blockNonNumbers(this, event, true, false);" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add" TabIndex="12" OnClick="btnAdd_Click"
                                                        OnClientClick="return valAdd();" Enabled="False" onfocus="active(this);" onblur="inactive(this);"
                                                        Width="80px" />
                                                    <asp:Button ID="btnClearDeduction" runat="server" Text="Clear" TabIndex="12" onfocus="active(this);"
                                                        onblur="inactive(this);" Width="80px" Enabled="False" OnClick="btnClearDeduction_Click" />
                                                    <asp:Button ID="btnCal" runat="server" Text="Calculator" TabIndex="12" Visible="False"
                                                        onfocus="active(this);" onblur="inactive(this);" OnClientClick="return pop('pop1');"
                                                        Width="80px" />
                                                    <asp:HiddenField ID="hfDedPerEmp" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hfDedPerOrg" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="grdDeductions" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        DataKeyNames="DedTypeId" OnRowDeleting="grdDeductions_RowDeleting">
                                                        <Columns>
                                                            <asp:BoundField DataField="DedTypeId" Visible="false" />
                                                            <asp:TemplateField HeaderText="Deduction Type">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEditDeduction" runat="server" CommandArgument='<%#Eval("DedTypeId")%>'
                                                                        Text='<%#Eval("DedDetails")%>' OnClick="btnEditDeduction_Click" Font-Underline="true"
                                                                        CausesValidation="false" ToolTip='<%#"Click to Edit" +" "+ Eval("DedDetails").ToString()+" "+"Deduction" %>'></asp:LinkButton>
                                                                    <asp:HiddenField ID="hdnDedTypeId" runat="server" Value='<%#Eval("DedTypeId") %>' />
                                                                    <asp:HiddenField ID="hfPerAmt" runat="server" Value='<%#Eval("PerAmt") %>' />
                                                                    <asp:HiddenField ID="hfAmtEmp" runat="server" Value='<%#Eval("AmtFromEmp") %>' />
                                                                    <asp:HiddenField ID="hfAmtOrg" runat="server" Value='<%#Eval("AmtFromOrg") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Employee Share" DataField="EmpShare" HeaderStyle-HorizontalAlign="Right"
                                                                DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Middle"
                                                                HeaderStyle-VerticalAlign="Middle">
                                                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Employer Share" DataField="OrgShare" HeaderStyle-HorizontalAlign="Right"
                                                                DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Middle"
                                                                HeaderStyle-VerticalAlign="Middle">
                                                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </asp:BoundField>
                                                            <asp:CommandField ButtonType="Button" ShowDeleteButton="true" HeaderText="Delete"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:CommandField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            Add Deduction to the List
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" style="font-size: 12px;">
                            <tr>
                                <td style="height: 5px;">
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <strong>Gross Total</strong>&nbsp;:&nbsp;
                                    <asp:TextBox ID="txtGrossTot" ReadOnly="true" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Width="90px" TabIndex="14" Text="0"></asp:TextBox>
                                    <strong>Total Deduction</strong>&nbsp;:&nbsp;
                                    <asp:TextBox ID="txtTotalDed" ReadOnly="true" runat="server" Width="90px" TabIndex="15"
                                        Text="0"></asp:TextBox>
                                    <strong>Net Payable</strong>&nbsp;:&nbsp;
                                    <asp:TextBox ID="txtNetPayable" ReadOnly="true" runat="server" Width="90px" TabIndex="15"
                                        Text="0"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="height: 5px;">
                </div>
                <div style="width: 97%; background-color: #666; padding: 2px;" align="left">
                    <div style="background-color: #FFF; padding: 10px;">
                        <div style="color: Red; font-size: 12px; font-weight: bold; background-color: Yellow;"
                            align="center">
                            Before Submitting please Recheck the Correctness
                        </div>
                        <br />
                        <div style="width: 100%; font-size: 12px;">
                            <strong>Remarks&nbsp;:&nbsp;</strong><asp:TextBox ID="txtRemarks" runat="server"
                                TabIndex="16" Width="400px"></asp:TextBox>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                TabIndex="17" OnClientClick="return valSubmit();" onfocus="active(this);" onblur="inactive(this);" />
                            <asp:Button ID="btnCancel" runat="server" TabIndex="18" Text="Cancel" OnClick="btnCancel_Click"
                                onfocus="active(this);" onblur="inactive(this);" />
                                <asp:HiddenField ID="hfLastSalDt" runat="server" />
                                <asp:HiddenField ID="hfPrevSal" runat="server" />
                        </div>
                        <br />
                        <div id="divMsg" runat="server" style="width: 100%; height: 20px; text-align: center;">
                            <asp:Label runat="server" ID="lblMsg" Font-Size="12px" ForeColor="White" Font-Bold="true"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div id="pop1" class="parentDisable">
                <center>
                    <div id="popup">
                        <%--<a href="#" onclick="return hide('pop1')">
                            <img id="close" src="../images/refresh_icon.png" alt="Close" />
                        </a>--%>
                        <div align="center">
                            <h2>
                                Deduction Calculator</h2>
                        </div>
                        <br />
                        <table>
                            <tr>
                                <td align="left" valign="baseline" colspan="2" style="color: Red">
                                    **Deduction will be calculated from the Gross.
                                    <ol type="1">
                                        <li>Select required Allowances to be added in Gross.</li>
                                        <li>Click on Submit Button</li>
                                    </ol>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline" width="100px">
                                    Basic
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtBasic" runat="server" Width="100px" Text="0" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline" colspan="2">
                                    <asp:GridView ID="grdAllw" runat="server" Width="100%" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <input name="Checkb" type="checkbox" value='<%# Eval("Amount") %>' onchange="return GetSelectedRow(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <%--<HeaderTemplate>
                                                    <input name="toggleAll" onclick="ToggleAll(this)" type="checkbox" value="ON" />
                                                </HeaderTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Allowance Name" DataField="Allowance" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Amount" DataField="Amount" HeaderStyle-HorizontalAlign="Right"
                                                DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Middle"
                                                HeaderStyle-VerticalAlign="Middle" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    Gross
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtGross" runat="server" Width="100px" Text="0" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    Employee Share
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtDedEmp" runat="server" Width="100px" Text="0" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    Employer Share
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtDedOrg" runat="server" Width="100px" Text="0" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    &nbsp;
                                </td>
                                <td align="left" valign="baseline">
                                    &nbsp;<asp:Button ID="btnCalculate" runat="server" Text="Submit" onfocus="active(this);"
                                        onblur="inactive(this);" OnClientClick="return CalculateDed();" OnClick="btnCalculate_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

