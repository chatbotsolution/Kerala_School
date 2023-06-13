<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="Examination.aspx.cs" Inherits="Exam_Examination" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function validateCheckBoxList() {
            var isAnyCheckBoxChecked = false;
            var checkBoxes = document.getElementById("ctl00_ContentPlaceHolder1_chklClass").getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox") {
                    if (checkBoxes[i].checked) {
                        isAnyCheckBoxChecked = true;
                        break;
                    }
                }
            }
            if (!isAnyCheckBoxChecked) {
                alert("Select a Class");
            }

            return isAnyCheckBoxChecked;
        }
        function validateform() {
            var Session = document.getElementById("<%=drpSession.ClientID %>").value;
            var ExamName = document.getElementById("<%=txtExamName.ClientID %>").value;
            var ExamType = document.getElementById("<%=drpExamTyp.ClientID %>").value;
            var fromDate = document.getElementById("<%=txtFromDate.ClientID %>").value;
            var ToDate = document.getElementById("<%=txtToDate.ClientID %>").value;
            var PassPer = document.getElementById("<%=txtPassPercent.ClientID %>").value;
            if (Session == "0") {
                alert("Select a Session");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            if (ExamName.trim() == "") {
                alert("Enter Examination Name");
                document.getElementById("<%=txtExamName.ClientID %>").focus();
                return false;
            }
            if (ExamType == "0") {
                alert("Select an Exam Type");
                document.getElementById("<%=drpExamTyp.ClientID %>").focus();
                return false;
            }
            if (fromDate == "") {
                alert("Enter Exam Start Date");
                document.getElementById("<%=txtFromDate.ClientID %>").focus();
                return false;
            }
            if (ToDate == "") {
                alert("Enter Exam End Date");
                document.getElementById("<%=txtToDate.ClientID %>").focus();
                return false;
            }
            if (Date.parse(fromDate.trim()) > Date.parse(ToDate.trim())) {

                alert("Check date range.\n\nStart Date cannot be greater than End Date.");
                return false;

            }
            if (PassPer.trim() == "") {
                alert("Enter Pass Percentage");
                document.getElementById("<%=txtPassPercent.ClientID %>").focus();
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
            Define Exams</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="height: 100%;">
                <tr>
                    <td align="center" valign="top">
                        <br />
                        <div align="left" style="padding-left: 10px">
                            <div style="padding: 8px;">
                                <table width="100%">
                                    <tr>
                                        <td width="200px" align="left" valign="top">
                                            Session Year
                                        </td>
                                        <td align="left" valign="top">
                                            :&nbsp;<asp:DropDownList runat="server" ID="drpSession" TabIndex="1">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" style="height: 23px">
                                            Examination Name
                                        </td>
                                        <td align="left" valign="top" style="height: 23px">
                                            :&nbsp;<asp:TextBox ID="txtExamName" runat="server" TabIndex="2" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            Applicable for Class
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:CheckBoxList ID="chklClass" RepeatDirection="Vertical" runat="server" TabIndex="3"
                                                AutoPostBack="True" OnSelectedIndexChanged="chklClass_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            Exam Type
                                        </td>
                                        <td align="left" valign="top">
                                            :&nbsp;<asp:DropDownList runat="server" ID="drpExamTyp" TabIndex="4">
                                            <asp:ListItem Value="0" Text="- Select -"></asp:ListItem>
                                            <asp:ListItem Value="UT" Text="Unit Test"></asp:ListItem>
                                            <asp:ListItem Value="HF" Text="Half Yearly"></asp:ListItem>
                                            <asp:ListItem Value="PT" Text="Pre-Test"></asp:ListItem>
                                            <asp:ListItem Value="TE" Text="Test"></asp:ListItem>
                                            <asp:ListItem Value="PB" Text="Pre-Board"></asp:ListItem>
                                            <asp:ListItem Value="AN" Text="Annual"></asp:ListItem>
                                            <asp:ListItem Value="OT" Text="Others"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            Exam Start Date
                                        </td>
                                        <td align="left" valign="top">
                                            :&nbsp;<asp:TextBox ID="txtFromDate" runat="server" Height="22px" Width="90px" ReadOnly="True"></asp:TextBox>
                                            <rjs:PopCalendar ID="dtpFromDate" runat="server" Control="txtFromDate" AutoPostBack="False"
                                                Format="dd mmm yyyy"></rjs:PopCalendar>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            Exam End Date
                                        </td>
                                        <td align="left" valign="top">
                                            :&nbsp;<asp:TextBox ID="txtToDate" runat="server" Height="22px" Width="90px" ReadOnly="True"></asp:TextBox>
                                            <rjs:PopCalendar ID="dtptoDate" runat="server" Control="txtToDate" AutoPostBack="False"
                                                Format="dd mmm yyyy"></rjs:PopCalendar>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            Pass Percent
                                        </td>
                                        <td align="left" valign="top">
                                            :&nbsp;<asp:TextBox ID="txtPassPercent" runat="server" Height="22px" Width="90px"
                                                onkeypress="return blockNonNumbers(this, event, false, false);" MaxLength="3"
                                                TabIndex="5" Text="0" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100" align="left" valign="top">
                                            All Subject Required To Pass
                                        </td>
                                        <td align="left" valign="top">
                                            :&nbsp;<asp:RadioButton ID="rbnYes" runat="server" Text="Yes" GroupName="b" Checked="True"
                                                TabIndex="6" />
                                            <asp:RadioButton ID="rbnNo" runat="server" Text="No" GroupName="b" TabIndex="5" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100" align="left" valign="top">
                                            Status
                                        </td>
                                        <td align="left" valign="top">
                                            :&nbsp;<asp:RadioButton ID="rbnActive" runat="server" Text="Active" GroupName="a"
                                                Checked="True" TabIndex="7" />
                                            <asp:RadioButton ID="rbnInAct" runat="server" Text="InActive" GroupName="a" TabIndex="6" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Save & Add New" TabIndex="8" OnClick="btnSubmit_Click"
                                                onfocus="active(this);" onblur="inactive(this);" OnClientClick="return validateform();" />
                                            <asp:Button ID="btnShow" runat="server" Text="Save & Go To List" OnClick="btnShow_Click"
                                                TabIndex="9" onfocus="active(this);" onblur="inactive(this);" OnClientClick="return validateform();" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Clear" OnClick="btnCancel_Click"
                                                TabIndex="10" onfocus="active(this);" onblur="inactive(this);" />
                                            <asp:Button ID="btnBack" runat="server" Text="Back" TabIndex="11" OnClick="btnBack_Click"
                                                onfocus="active(this);" onblur="inactive(this);" />
                                        </td>
                                    </tr>
                                    <tr id="trMsg" runat="server">
                                        <td style="height: 20px;" align="center" colspan="2">
                                            <asp:Label ID="lblMsg" ForeColor="White" Font-Bold="true" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

