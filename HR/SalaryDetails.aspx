<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="SalaryDetails.aspx.cs" Inherits="HR_SalaryDetails" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript">
        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';

        }
        function clearText(btn) {
            switch (btn) {
                case '1':
                    {
                        document.getElementById("<%=txtFromDt.ClientID %>").value = "";
                        return false;
                    }
                default:
                    {
                        break;
                    }
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

        function validateform() {
            var FromDt = document.getElementById("<%=txtFromDt.ClientID %>").value;

            var Basic = document.getElementById("<%=txtBasic.ClientID %>").value;

            if (FromDt.trim() == "") {
                alert("Enter From Date !");
                document.getElementById("<%=txtFromDt.ClientID %>").focus();
                return false;
            }

            if (Basic.trim() == "") {
                alert("Enter Basic Salary !");
                document.getElementById("<%=txtBasic.ClientID %>").focus();
                return false;
            }
            else {
                CheckLoader();
                return true;
            }

        }
    </script>

    <div align="center">
        <br />
        <div class="innerdiv" style="width: 700px;">
            <div class="linegap">
                <img src="../images/mask.gif" width="10" height="10" /></div>
            <div style="padding: 8px;">
                <table>
                    <tr>
                        <td align="left" valign="top" class="tbltxt">
                            Employee Name
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td colspan="2" >
                            <asp:Label ID="lblEmp" runat="server" CssClass="totalrec"></asp:Label>
                        </td>
                        <td colspan="3" >
                            Effective Date:&nbsp; <asp:TextBox ID="txtFromDt" runat="server" CssClass="tbltxtbox_mid" TabIndex="1"></asp:TextBox><span class="mandatory">*</span><rjs:PopCalendar ID="dtpFrom" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
                            </rjs:PopCalendar><asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                OnClientClick="return clearText('1');" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt" colspan="3">
                           
                        </td>
                       
                        <td align="left" valign="top" class="mandatory">
                        </td>
                        <td align="center" valign="middle" class="totalrec" style="background-color:#D3DECA;border:solid 1px gray;" colspan="4">
                            DEDUCTIONS
                        </td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            Basic Salary
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtBasic" runat="server" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="2"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        *
                        </td>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            GSLI Deductor
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtGSLI" runat="server" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="7"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            DA
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtDA" runat="server" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="3"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            EPF Deductor
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtEPF" runat="server" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="8"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            HRA
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtHR" runat="server" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="4"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            Insurance
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtInsurance" runat="server" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="9"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            Allowance
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtAllowance" runat="server" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="5"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            Any Advance
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtAnyAdv" runat="server" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="10"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            Special Allowance
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtSplAllowance" runat="server" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="6"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            Others
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtOther" runat="server" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="11"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top" class="tbltxt" colspan="8">
                            <asp:Button ID="btnSave" runat="server" Text="Save & Add New" OnClientClick="return validateform();"
                                OnClick="btnSave_Click" TabIndex="12" />&nbsp;<asp:Button ID="btnShow" 
                                runat="server" Text="Save & Go to List" OnClick="btnShow_Click"
                                TabIndex="13" />&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Clear" OnClick="btnCancel_Click"
                                TabIndex="14" />&nbsp;<asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click"
                                TabIndex="15" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
<%--    <div id="Loader" style="text-align: center; vertical-align: middle; position: absolute;
        top: 0px; left: 0px; z-index: 99; width: 100%; height: 100%; background-color: #ededed;
        -ms-filter: 'progid:DXImageTransform.Microsoft.Alpha(Opacity=60)'; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=60);
        -moz-opacity: 0.8; opacity: 0.8;">
        <div style="width: 48px; height: 48px; margin: 0 auto; margin-top: 275px;">
            <img src="../images/spinner.gif">
        </div>
        <div style="font-family: Trebuchet MS; font-size: 12px; color: Green; text-align: center;">
            Please Wait ...</div>
    </div>--%>
</asp:Content>


