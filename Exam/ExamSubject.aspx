<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="ExamSubject.aspx.cs" Inherits="Exam_ExamSubject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function GetTotalMarks() {
            var theory = document.getElementById("<%= txtTheory.ClientID%>").value;
            var practical = document.getElementById("<%= txtPractical.ClientID%>").value;
            var project = document.getElementById("<%= txtProject.ClientID%>").value;
            var maviva = document.getElementById("<%= txtmaviva.ClientID%>").value;
            var total = 0;
            if (theory.trim() == "") {
                theory = 0;
            }
            if (practical.trim() == "") {
                practical = 0;
            }
            if (project.trim() == "") {
                project = 0;
            }
            if (maviva.trim() == "") {
                maviva = 0;
            }
            total = parseFloat(theory) + parseFloat(practical) + parseFloat(project) + parseFloat(maviva);
            document.getElementById("<%= txtFullMarks.ClientID%>").value = total;
            return false;
        }
        function validateCheckBoxList() {
            var isAnyCheckBoxChecked = false;
            var checkBoxes = document.getElementById("ctl00_ContentPlaceHolder1_chkExam").getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox") {
                    if (checkBoxes[i].checked) {
                        isAnyCheckBoxChecked = true;
                        break;
                    }
                }
            }
            if (!isAnyCheckBoxChecked) {
                alert("Select an Exam");
            }

            return isAnyCheckBoxChecked;
        }
        function Isvalid() {
            var Session = document.getElementById("<%=drpSession.ClientID %>").value;
            var Class = document.getElementById("<%=drpClass.ClientID %>").value;
            var ExamSub = document.getElementById("<%=drpSubject.ClientID %>").value;
            var FullMarks = document.getElementById("<%=txtFullMarks.ClientID %>").value;
            var PassMarks = document.getElementById("<%=txtPassMarks.ClientID %>").value;
            if (Session == "0") {
                alert("Select a Session");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            if (Class == "0") {
                alert("Select a Class");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            if (ExamSub.trim() == "0") {
                alert("Select a Subject");
                document.getElementById("<%=drpSubject.ClientID %>").focus();
                return false;
            }
            if (FullMarks.trim() == "" || parseFloat(FullMarks) <= 0) {
                alert("Full Marks shouldn't be Zero.");
                document.getElementById("<%=txtTheory.ClientID %>").focus();
                return false;
            }
            if (PassMarks.trim() == "" || parseFloat(PassMarks) <= 0) {
                alert("Enter Pass Marks");
                document.getElementById("<%=txtPassMarks.ClientID %>").focus();
                return false;
            }
            if (parseFloat(FullMarks) < parseFloat(PassMarks)) {
                alert("Pass Marks shouldn't be more than Full Marks");
                document.getElementById("<%=txtPassMarks.ClientID %>").focus();
                return false;
            }
            else {
                return validateCheckBoxList();
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
    <div style="padding-top: 3px;">
        <h2>
            Define Exams Subjects</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <fieldset style="width: 70%">
                <table width="100%">
                    <tr>
                        <td align="left" valign="baseline">
                            Session Year <span class="mandatory">*</span>
                        </td>
                        <td align="left" valign="top">
                            :&nbsp;<asp:DropDownList runat="server" ID="drpSession" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Class <span class="mandatory">*</span>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:DropDownList ID="drpClass" runat="server" TabIndex="2" AutoPostBack="True"
                                OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Subject <span class="mandatory">*</span>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:DropDownList ID="drpSubject" runat="server" TabIndex="3" OnSelectedIndexChanged="drpSubject_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Theory Marks <span class="mandatory">*</span>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtTheory" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                MaxLength="5" TabIndex="4" onblur="if (this.value == '') {this.value = '0';}"
                                onfocus="if(this.value == '0') {this.value = '';}" onkeyup="return GetTotalMarks();" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                             SE Marks </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtPractical" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                MaxLength="5" TabIndex="5" Text="0" onblur="if (this.value == '') {this.value = '0';}"
                                onfocus="if(this.value == '0') {this.value = '';}" onkeyup="return GetTotalMarks();"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                          Pt-1 Marks</td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtProject" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                MaxLength="5" TabIndex="6" Text="0" onblur="if (this.value == '') {this.value = '0';}"
                                onfocus="if(this.value == '0') {this.value = '';}" onkeyup="return GetTotalMarks();"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            MA/Viva Marks</td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtmaviva" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                MaxLength="5" TabIndex="6" Text="0" onblur="if (this.value == '') {this.value = '0';}"
                                onfocus="if(this.value == '0') {this.value = '';}" onkeyup="return GetTotalMarks();"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Full Marks <span class="mandatory">*</span>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtFullMarks" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                MaxLength="5" TabIndex="7" Enabled="false" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Pass Marks <span class="mandatory">*</span>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtPassMarks" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                MaxLength="5" TabIndex="8" Text="0" onblur="if (this.value == '') {this.value = '0';}"
                                onfocus="if(this.value == '0') {this.value = '';}"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Applicable for Exam<span class="mandatory">*</span>
                        </td>
                        <td align="left" valign="baseline">
                            <asp:CheckBoxList ID="chkExam" RepeatDirection="Vertical" RepeatLayout="Flow" runat="server"
                                TabIndex="9">
                            </asp:CheckBoxList>
                            &nbsp;<label id="lbl" style="color: Red" visible="false" runat="server">Subjects already
                                defined for all Exams.</label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                            &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Save & Add New" TabIndex="10"
                                OnClick="btnSubmit_Click" OnClientClick="return Isvalid();" onfocus="active(this);"
                                onblur="inactive(this);" />
                            <asp:Button ID="btnShow" runat="server" Text="Save & Go To List" TabIndex="11" OnClick="btnShow_Click"
                                OnClientClick="return Isvalid();" onfocus="active(this);" onblur="inactive(this);" />
                            <asp:Button ID="btnCancel" runat="server" Text="Clear" TabIndex="12" OnClick="btnCancel_Click"
                                onfocus="active(this);" onblur="inactive(this);" />
                            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" onfocus="active(this);"
                                onblur="inactive(this);" TabIndex="13" />
                        </td>
                    </tr>
                    <tr id="trMsg" runat="server">
                        <td style="height: 20px;" align="center" colspan="2">
                            <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

