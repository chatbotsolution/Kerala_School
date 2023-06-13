<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="FeeReceiptHostel.aspx.cs" Inherits="Hostel_FeeReceiptHostel" %>


<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function valFeePayable() {
            var student = document.getElementById("<%=drpstudent.ClientID%>").value;
            if (student.trim() == "" || student.trim() == "0") {
                alert("Please Select a Student");
                return false;
            }
            else {
                return true;
            }
        }

        function valDetails() {
            var Admno = document.getElementById("<%=txtadmnno.ClientID %>").value;
            if (Admno.trim() == "") {
                alert("Please Enter Admission Number");
                document.getElementById("<%=txtadmnno.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }

        function valShow() {
            var Stud = document.getElementById("<%=drpstudent.ClientID %>").value;
            var Desc = document.getElementById("<%=txtdesc.ClientID %>").value;
            var PmntMode = document.getElementById("<%=drpPmtMode.ClientID %>").value;
            var ChkDt = document.getElementById("<%=txtChqDate.ClientID %>").value;
            var ChkNo = document.getElementById("<%=txtChqNo.ClientID %>").value;
            var Bank = document.getElementById("<%=txtBank.ClientID %>").value;
            var paidamt = document.getElementById("<%=lblTotalFee.ClientID %>").innerHTML;

            var SchoolFeeRcvd = document.getElementById("<%=txtamt.ClientID %>").value;
            var SchoolFeeBal = document.getElementById("<%=lblbalance.ClientID %>").innerHTML;

            if (SchoolFeeRcvd.trim() == "") {
                SchoolFeeRcvd = 0;
                document.getElementById("<%=txtamt.ClientID %>").value = "0";
            }

            if (PmntMode != "Cash") {
                var bankaccount = document.getElementById("<%=drpBankAc.ClientID %>").value;
            }

            var strDate = document.getElementById("<%=txtdate.ClientID %>").value;   // Add any of the date from any control
            var dt = strDate.substring(0, 2);
            var mon = strDate.substring(3, 5);
            var yr = strDate.substring(6);
            var RcvDate = mon + '/' + dt + '/' + yr

            var Currentdate = new Date();

            if (Stud == 0) {
                alert("Please Select a Student");
                document.getElementById("<%=drpstudent.ClientID %>").focus();

            }
            if (new Date(yr, mon - 1, dt, 0, 0, 0) > Currentdate) {
                document.getElementById("<%=txtdate.ClientID %>").focus();
                alert("Future date is not permitted");
                return false;
            }
            if (!isValidDate(RcvDate)) {
                document.getElementById("<%=txtdate.ClientID %>").focus();
                alert("Invalid Date Format. Please Correct and Submit again.");
                return false;
            }

            if (PmntMode != "Cash" && bankaccount == 0) {
                alert("Please Select Bank A/c");
                document.getElementById("<%=drpBankAc.ClientID %>").focus();
                return false;
            }
            if (PmntMode != "Cash" && ChkDt.trim() == "") {
                alert("Please Enter Check Date");
                document.getElementById("<%=txtChqDate.ClientID %>").focus();
                return false;
            }
            if (PmntMode != "Cash" && ChkNo.trim() == "") {
                alert("Please Enter Instrument No");
                document.getElementById("<%=txtChqNo.ClientID %>").focus();
                return false;
            }
            if (PmntMode != "Cash" && Bank.trim() == "") {
                alert("Please Enter Bank Name");
                document.getElementById("<%=txtBank.ClientID %>").focus();
                return false;
            }
            if (paidamt == "0.00") {
                alert("No pending Amount to Receive");
                return false;
            }
            if (paidamt == 0) {
                alert("No Pending Amount to Receive");
                return false;
            }
            if (Desc.trim() == "") {
                alert("Please Enter Description");
                document.getElementById("<%=txtdesc.ClientID %>").focus();
                return false;
            }
            if (parseFloat(SchoolFeeRcvd) > parseFloat(SchoolFeeBal)) {
                alert("Hostel Fee Amount can't be greater than Payable Amount");
                document.getElementById("<%=txtamt.ClientID %>").focus();
                return false;
            }
            else {
                return true;

            }
        }

        function isValidDate(subject) {
            if (subject.match(/^(?:(0[1-9]|1[012])[\- \/.](0[1-9]|[12][0-9]|3[01])[\- \/.](19|20)[0-9]{2})$/)) {
                return true;
            } else {

                return false;
            }
        }

        function MiscFee() {
            var Stud = document.getElementById("<%=drpstudent.ClientID %>").value;
            var AddFee = document.getElementById("<%=drpAdditionalFee.ClientID %>").value;
            var Fee = document.getElementById("<%=txtRcevMiscFee.ClientID %>").value;
            var MiscDt = document.getElementById("<%=txtMiscDt.ClientID %>").value;
            var dt = MiscDt.substring(0, 2);
            var mon = MiscDt.substring(3, 5);
            var yr = MiscDt.substring(6);
            var RcvDate = mon + '/' + dt + '/' + yr
            if (Stud == 0) {
                alert("Please Select a Student");
                document.getElementById("<%=drpstudent.ClientID %>").focus();
                return false;
            }
            if (AddFee == 0) {
                alert("Please Select Miscellaneous Fee");
                document.getElementById("<%=drpAdditionalFee.ClientID %>").focus();
                return false;
            }
            if (MiscDt.trim() == "") {
                alert("Please Select Misc Date");
                document.getElementById("<%=txtMiscDt.ClientID %>").focus();
                return false;
            }
            if (!isValidDate(RcvDate)) {
                document.getElementById("<%=txtMiscDt.ClientID %>").focus();
                alert("Invalid Date Format");
                return false;
            }
            if (Fee.trim() == "") {
                alert("Please Enter Misc Amount");
                document.getElementById("<%=txtRcevMiscFee.ClientID %>").focus();
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
        function lgprchs_onclick() {

        }

        function getpayblamt() {
            var MiscFee = document.getElementById("<%= txtMiscFee.ClientID%>").value;
            var SchoolFee = document.getElementById("<%=txtamt.ClientID %>").value;
            var totPaybl = document.getElementById("<%=lblTotalFee.ClientID %>").innerHTML;
            var totalAmt = 0;
            var invAmt = 0;
            if (MiscFee.trim() == "")
                MiscFee = 0;
            if (SchoolFee.trim() == "")
                SchoolFee = 0;

            document.getElementById("<%=lblTotalFee.ClientID %>").innerHTML = parseFloat(MiscFee) + parseFloat(Busfee) + parseFloat(HostelFee) + parseFloat(SchoolFee);
        }

    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Receive Hostel Fee</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <center>
                <div>
                    <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="tbltxt" Text=""></asp:Label></div>
            </center>
            <fieldset id="fsselection" runat="server">
                <legend style="background-color: window" id="lgprchs" class="tbltxt" runat="server"
                    onclick="return lgprchs_onclick()"><b>Selection Criteria</b></legend>
                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <td class="tbltxt">
                            <asp:RadioButton ID="rBtnCurrentSess" GroupName="d" Checked="true" Text="Current Due"
                                runat="server" AutoPostBack="True" OnCheckedChanged="rBtnCurrentSess_CheckedChanged"
                                TabIndex="1" />
                            &nbsp;
                            <asp:RadioButton ID="rBtnPrevSess" GroupName="d" runat="server" Text="Prev Due" AutoPostBack="True"
                                OnCheckedChanged="rBtnCurrentSess_CheckedChanged" TabIndex="2" />
                        </td>
                        <td width="50px" class="tbltxt">
                            Session&nbsp;:
                        </td>
                        <td width="70px" class="tbltxt">
                            <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" Width="70px"
                                AutoPostBack="true" TabIndex="3" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="40px">
                            Class&nbsp;:
                        </td>
                        <td class="tbltxt" width="70px">
                            <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                                CssClass="vsmalltb" Width="70px" meta:resourcekey="drpclassResource1" TabIndex="4">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="55px">
                            Student&nbsp;:
                        </td>
                        <td class="tbltxt">
                            <asp:DropDownList ID="drpstudent" runat="server" Width="100px" AutoPostBack="True"
                                OnSelectedIndexChanged="drpstudent_SelectedIndexChanged" 
                                CssClass="largetb" TabIndex="5" Height="18px">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="70">
                            Student Id&nbsp;:
                        </td>
                        <td class="tbltxt">
                            <asp:TextBox ID="txtadmnno" runat="server" CssClass="vsmalltb" TabIndex="6" Width="70px"></asp:TextBox><span
                                class="error">*</span>
                            <asp:Button ID="btnDetail" runat="server" Text="Show" OnClientClick="return valDetails();"
                                onfocus="active(this);" onblur="inactive(this);" OnClick="btnDetail_Click" Width="70px"
                                Height="25px" TabIndex="7" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="Fieldset1" runat="server">
                <legend style="background-color: window" id="Legend1" class="tbltxt" runat="server">
                    <b>Fee Details :-</b></legend>
                <table width="100%" border="0" cellpadding="0" cellspacing="2">
                    <tr>
                        <td colspan="4" style="width: 100%" valign="baseline">
                            <asp:Label ID="lblOldDues" runat="server" Font-Bold="True" ForeColor="Red" CssClass="error"></asp:Label>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnRcvOldDue" runat="server" OnClientClick="return valDetails();"
                                onfocus="active(this);" onblur="inactive(this);" OnClick="btnRcvOldDue_Click"
                                Text="Receive Old Dues" TabIndex="8" />&nbsp;&nbsp;&nbsp;<asp:Label ID="lblPrevYrBal"
                                    runat="server" Font-Bold="True" ForeColor="Red" CssClass="error"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="spacer">
                            <img src="../images/mask.gif" height="8" width="10" />
                        </td>
                    </tr>
                </table>
                <table width="100%" border="0" cellpadding="0" cellspacing="2">
                    <tr>
                        <td align="left" valign="top">
                            <fieldset id="Fieldset5" runat="server">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="tbltxt" colspan="4">
                                            Misc Fee :
                                            <asp:DropDownList ID="drpAdditionalFee" runat="server" ValidationGroup="search" CssClass="vsmalltb"
                                                Width="80px" TabIndex="9" AutoPostBack="true" OnSelectedIndexChanged="drpAdditionalFee_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            &nbsp;&nbsp; Misc Date :
                                            <asp:TextBox ID="txtMiscDt" runat="server" CssClass="vsmalltb" Width="60px" meta:resourcekey="txtdateResource1"
                                                TabIndex="10"></asp:TextBox>&nbsp;<rjs:PopCalendar ID="dtpMiscDt" runat="server"
                                                    Control="txtMiscDt" meta:resourcekey="PopCalendar2Resource1"></rjs:PopCalendar>
                                            &nbsp;&nbsp;Amount :
                                            <asp:TextBox ID="txtRcevMiscFee" runat="server" CssClass="vsmalltb" TabIndex="11"
                                                onkeypress="return blockNonNumbers(this, event, true, false);" Width="60px" MaxLength="7"></asp:TextBox>
                                            &nbsp;<asp:Button ID="btnMiscFee" CausesValidation="False" runat="server" OnClick="btnMiscFee_Click"
                                                onfocus="active(this);" onblur="inactive(this);" Text="Update Misc Fee" meta:resourcekey="btnReverseFineResource1"
                                                TabIndex="12" OnClientClick="return MiscFee();" />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblNote" runat="server" Font-Bold="True" ForeColor="Red" meta:resourcekey="lblbalanceResource1"
                                                CssClass="error" Text="Total Payable Amount :- Rs."></asp:Label>
                                            <asp:Label ID="lblTotalFee" Text="0.00" runat="server" CssClass="error" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <table width="100%" border="0" cellpadding="0" cellspacing="2">
                        <tr>
                            <td align="left" valign="top">
                                <fieldset id="Fieldset3" runat="server">
                                    <legend class="tbltxt"><b>Hostel Fee</b></legend>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="tbltxt" align="left">
                                                Hostel Fee Till :&nbsp;<asp:DropDownList ID="drpMonthPayble" runat="server" CssClass="smalltb"
                                                    Width="70px" AutoPostBack="True" OnSelectedIndexChanged="drpMonthPayble_SelectedIndexChanged"
                                                    TabIndex="13">
                                                </asp:DropDownList>
                                                &nbsp;Payble Amt :&nbsp;<asp:Label ID="lblbalance" runat="server" CssClass="error"
                                                    Text="Label" meta:resourcekey="lblbalanceResource1"></asp:Label>
                                                &nbsp;
                                                <asp:Button ID="btnShowDetails" runat="server" Text="Details" OnClick="btnShowDetails_Click"
                                                    onfocus="active(this);" onblur="inactive(this);" meta:resourcekey="btnShowDetailsResource1"
                                                    TabIndex="14" ToolTip="Click to view the fee details" OnClientClick="return valFeePayable();" />
                                                <asp:Button ID="btnFeeFullYr" runat="server" Text="Full Session" OnClientClick="return valDetails();"
                                                    onfocus="active(this);" onblur="inactive(this);" OnClick="btnFeeFullYr_Click"
                                                    Width="85px" TabIndex="15" />
                                                <asp:Button ID="btnInstDisc" runat="server" OnClick="btnInstDisc_Click" OnClientClick="return valDetails();"
                                                    onfocus="active(this);" onblur="inactive(this);" Text="Give Discount" TabIndex="16" Visible="false"/>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td align="left" valign="top">
                                <fieldset id="Fieldset4" runat="server">
                                    <legend class="tbltxt"><b>Misc Fee</b></legend>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="tbltxt">
                                                Misc Fee Names&nbsp;:&nbsp;<asp:DropDownList ID="drpMiscFee" runat="server" CssClass="smalltb"
                                                    Width="70px" AutoPostBack="True" OnSelectedIndexChanged="drpMiscFee_SelectedIndexChanged"
                                                    TabIndex="17">
                                                </asp:DropDownList>
                                                &nbsp;Payble Amt&nbsp;:&nbsp;<asp:Label ID="lblMiscFee" runat="server" CssClass="error"></asp:Label>
                                                &nbsp;<asp:Button ID="btnMiscDtl" runat="server" Text="Details" OnClick="btnMiscDtl_Click"
                                                    onfocus="active(this);" onblur="inactive(this);" meta:resourcekey="btnShowDetailsResource1"
                                                    TabIndex="18" ToolTip="Click to view the Miscellaneous fee details" OnClientClick="return valFeePayable();" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
            </fieldset>
            <fieldset id="Fieldset2" runat="server">
                <legend style="background-color: window" id="Legend2" class="tbltxt" runat="server">
                    <b>Received Details</b></legend>
                <table width="100%" border="0" cellpadding="2" cellspacing="2">
                    <tr>
                        <td class="tbltxt" style="width: 100px" valign="top">
                            Hostel Fee
                        </td>
                        <td valign="top">
                            :&nbsp;<asp:TextBox ID="txtamt" runat="server" Enabled="true" CssClass="vsmalltb"
                                meta:resourcekey="txtamtResource1" TabIndex="19" Width="80px" onkeyup="getpayblamt();"
                                onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox><span
                                    class="error">*</span>
                            <asp:Button ID="btnSFClear" runat="server" Text="Clear" CssClass="error" onfocus="active(this);"
                                onblur="inactive(this);" OnClick="btnSFClear_Click" TabIndex="20" />
                        </td>
                        <td class="tbltxt" style="width: 100px" valign="top">
                            Misc Fee
                        </td>
                        <td valign="top" colspan="5">
                            :&nbsp;<asp:TextBox ID="txtMiscFee" runat="server" CssClass="vsmalltb" meta:resourcekey="txtamtResource1"
                                TabIndex="21" Enabled="false" Width="80px"></asp:TextBox><span class="error">*</span>
                            <asp:Button ID="btnMFClear" runat="server" Text="Clear" CssClass="error" onfocus="active(this);"
                                onblur="inactive(this);" OnClick="btnMFClear_Click" TabIndex="22" />
                        </td>
                        <td rowspan="4" colspan="3" valign="top" align="center">
                            <asp:Image ID="imgStud" runat="server" Width="130px" Height="130px" ImageUrl="../Up_Files/Studimage/noimage.jpg" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" valign="top">
                            Receive Date
                        </td>
                        <td align="left" valign="top">
                            :&nbsp;<asp:TextBox ID="txtdate" runat="server" CssClass="vsmalltb" Width="80px"
                                TabIndex="23"></asp:TextBox>&nbsp;
                            <rjs:PopCalendar ID="dtpFeeDt" runat="server" Control="txtdate" To-Today="true">
                            </rjs:PopCalendar>
                            <span class="error">*</span>
                        </td>
                        <td class="tbltxt" valign="top">
                            Payment Mode
                        </td>
                        <td align="left" valign="top" colspan="2">
                            :&nbsp;<asp:DropDownList ID="drpPmtMode" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                                TabIndex="24" OnSelectedIndexChanged="drpPmtMode_SelectedIndexChanged">
                                <asp:ListItem>Cash</asp:ListItem>
                                <asp:ListItem>Bank</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;<asp:DropDownList ID="drpBankAc" runat="server" Width="250px" CssClass="vsmalltb"
                                ToolTip="The amount will be reflected on this Bank Account" TabIndex="25">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" valign="top">
                            Instrument Date
                        </td>
                        <td align="left" valign="top">
                            :&nbsp;<asp:TextBox ID="txtChqDate" runat="server" CssClass="vsmalltb" TabIndex="26"></asp:TextBox>
                        </td>
                        <td class="tbltxt" valign="top">
                            Instrument No
                        </td>
                        <td class="tbltxt" valign="top">
                            :
                            <asp:TextBox ID="txtChqNo" runat="server" CssClass="vsmalltb" TabIndex="27"></asp:TextBox>
                        </td>
                        <td class="tbltxt" valign="top">
                            Bank Name&nbsp;:&nbsp;<asp:TextBox ID="txtBank" runat="server" CssClass="largetb"
                                TabIndex="28"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" valign="top">
                            Description
                        </td>
                        <td align="left" valign="top" colspan="5">
                            :&nbsp;<asp:TextBox ID="txtdesc" runat="server" CssClass="largetb" Width="447px"
                                meta:resourcekey="txtdescResource1" TabIndex="29"></asp:TextBox><span class="error">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt">
                            &nbsp;
                        </td>
                        <td align="left" colspan="2">
                            &nbsp;&nbsp;<asp:Button ID="btnsave" runat="server" OnClick="btnsave_Click" Text="Submit"
                                meta:resourcekey="btnsaveResource1" onfocus="active(this);" onblur="inactive(this);"
                                TabIndex="30" OnClientClick="return valShow();" />
                            <asp:Button ID="btncancel" runat="server" CausesValidation="False" OnClick="btncancel_Click"
                                onfocus="active(this);" onblur="inactive(this);" Text="Cancel" meta:resourcekey="btncancelResource1"
                                TabIndex="31" />
                        </td>
                        <td colspan="6">
                            <asp:Label ID="lblMsg2" runat="server" Font-Bold="true" CssClass="tbltxt" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
            <asp:PostBackTrigger ControlID="drpMonthPayble" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
