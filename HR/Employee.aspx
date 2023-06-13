<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="Employee.aspx.cs" Inherits="HR_Employee" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript">
        function isValid() {
            var EmpName = document.getElementById("<%=txtEmpName.ClientID %>").value;
            var Dept = document.getElementById("<%=drpDept.ClientID %>").value;
            var Designation = document.getElementById("<%=drpDesignation.ClientID %>").value;
            var ContactNo = document.getElementById("<%=txtContactNo.ClientID %>").value;

            if (EmpName == "") {
                alert("Please Enter Employee Name !");
                document.getElementById("<%=txtEmpName.ClientID %>").focus();
                return false;
            }
            if (Dept == "0") {
                alert("Please Select a Department !");
                document.getElementById("<%=drpDept.ClientID %>").focus();
                return false;
            }
            if (Designation == "0") {
                alert("Please Select a Designation !");
                document.getElementById("<%=drpDesignation.ClientID %>").focus();
                return false;
            }
            if (ContactNo.length > 0 && ContactNo.length < 10) {
                alert("Please Enter a Valid Contact No. !");
                document.getElementById("<%=txtContactNo.ClientID %>").focus();
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

    <div class="bedcromb">
        <asp:Label ID="lblTitle" runat="server" Text="Employee Details"></asp:Label></div>
    <div style="padding: 30px;" align="center">
        <div style="width: 575px; background-color: #666; padding: 2px; margin: 0 auto;">
            <div style="background-color: #FFF; padding: 10px;">
                <center>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="height: 15px;">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Employee Name <font color="red">*</font>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmpName" runat="server" Width="200px" MaxLength="49"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Address :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmpAddress" runat="server" Height="50px" TextMode="MultiLine"
                                    Width="200px" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 7px">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Date Of Birth :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmpDOB" runat="server" Width="100px"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpDOB" runat="server" Control="txtEmpDOB" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 7px">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Department <font color="red">*</font>:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpDept" runat="server" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 7px">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Designation <font color="red">*</font>:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpDesignation" runat="server" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 7px">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Joining Date :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmpJoinDate" runat="server" Width="100px"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpJDate" runat="server" Control="txtEmpJoinDate" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 7px">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Leaving Date :
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtEmpLeaveDate" runat="server" Width="100px"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpLDate" runat="server" Control="txtEmpLeaveDate" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 7px">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Contact Number :
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactNo" runat="server" MaxLength="14" onkeypress="return blockNonNumbers(this, event, false, false);"
                                    Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 7px">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Remarks :
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" Height="50px" TextMode="MultiLine" Width="200px"
                                    MaxLength="199"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 7px">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Qualification :
                            </td>
                            <td>
                                <asp:TextBox ID="txtQualification" runat="server" Width="200px" MaxLength="49"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 7px">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                Status :
                            </td>
                            <td>
                                <asp:RadioButton ID="optStatActive" runat="server" Text="Active" GroupName="ActiveStatus"
                                    Checked="True" />
                                <asp:RadioButton ID="optStatInactive" runat="server" Text="Inactive" GroupName="ActiveStatus" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 7px">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="80px"
                                    OnClick="btnSubmit_Click" OnClientClick="return isValid();" />
                                &nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="Reset" Width="80px"
                                    OnClick="btnReset_Click" />
                                &nbsp;
                                <asp:Button ID="btnShowList" runat="server" Text="Show List" OnClick="btnShowList_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Label runat="server" ID="lblMsg" Font-Bold="True" Font-Italic="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </div>
    </div>
</asp:Content>

