<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="feercptcash.aspx.cs" Inherits="FeeManagement_feercptcash" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <style type="text/css">
    
    .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
    }
    
</style>

    <script type="text/javascript" language="javascript">

        function valFeePayable() {
            var student = document.getElementById("<%=drpstudent.ClientID%>").value;
            if (student == "" || student == "0") {
                alert("Please select a student !");
                return false;
            }
            else {
                return true;
            }
        }

        function valDetails() {
            var Admno = document.getElementById("<%=txtadmnno.ClientID %>").value;
            if (Admno.trim() == "") {
                alert("Please enter admission number !");
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
                alert("Please Select a Student !");
                document.getElementById("<%=drpstudent.ClientID %>").focus();

            }
            if (new Date(yr, mon - 1, dt, 0, 0, 0) > Currentdate) {
                document.getElementById("<%=txtdate.ClientID %>").focus();
                alert("Future date is not permitted");
                return false;
            }
            if (!isValidDate(RcvDate)) {
                document.getElementById("<%=txtdate.ClientID %>").focus();
                alert("Invalid Date Format. Please correct and submit again. !");
                return false;
            }

            if (PmntMode != "Cash" && bankaccount == 0) {
                alert("Please select Bank A/c");
                document.getElementById("<%=drpBankAc.ClientID %>").focus();
                return false;
            }
            if (PmntMode != "Cash" && ChkDt.trim() == "") {
                alert("Please Enter Check Date !");
                document.getElementById("<%=txtChqDate.ClientID %>").focus();
                return false;
            }
            if (PmntMode != "Cash" && ChkNo.trim() == "") {
                alert("Please Enter Instrument No !");
                document.getElementById("<%=txtChqNo.ClientID %>").focus();
                return false;
            }
            if (PmntMode != "Cash" && Bank.trim() == "") {
                alert("Please Enter Bank Name !");
                document.getElementById("<%=txtBank.ClientID %>").focus();
                return false;
            }
            if (paidamt == "0.00") {
                alert("No pending amount to receive !");
                return false;
            }
            if (paidamt == 0) {
                alert("No pending amount to receive !");
                return false;
            }
            if (Desc.trim() == "") {
                alert("Please enter description !");
                document.getElementById("<%=txtdesc.ClientID %>").focus();
                return false;
            }
            if (parseFloat(SchoolFeeRcvd) > parseFloat(SchoolFeeBal)) {
                alert("SchoolFee Amount Cannot be Greater than Palyble amount !");
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
                alert("Please Select a Student !");
                document.getElementById("<%=drpstudent.ClientID %>").focus();
                return false;
            }
            if (AddFee == 0) {
                alert("Please Select Miscellaneous Fee !");
                document.getElementById("<%=drpAdditionalFee.ClientID %>").focus();
                return false;
            }
            if (MiscDt.trim() == "") {
                alert("Please Select Misc Date !");
                document.getElementById("<%=txtMiscDt.ClientID %>").focus();
                return false;
            }
            if (!isValidDate(RcvDate)) {
                document.getElementById("<%=txtMiscDt.ClientID %>").focus();
                alert("Invalid Date Format. Please correct and submit again. !");
                return false;
            }
            if (Fee.trim() == "") {
                alert("Please Enter Misc Amount !");
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
            var Busfee = document.getElementById("<%=txtBusFee.ClientID %>").value

            //var BookFee = document.getElementById.value;
            var SchoolFee = document.getElementById("<%=txtamt.ClientID %>").value;
            var totPaybl = document.getElementById("<%=lblTotalFee.ClientID %>").innerHTML;
            var totalAmt = 0;
            var invAmt = 0;
            if (MiscFee.trim() == "")
                MiscFee = 0;
            if (Busfee.trim() == "")
                Busfee = 0;
            //if (BookFee.trim() == "")
            //    BookFee = 0;
            if (SchoolFee.trim() == "")
                SchoolFee = 0;

            document.getElementById("<%=lblTotalFee.ClientID %>").innerHTML = parseFloat(MiscFee) + parseFloat(Busfee) + parseFloat(HostelFee) + parseFloat(SchoolFee);
        }



    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">


    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Receive Regular Fee</h2>
    </div>
   
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <center>
                <div>
                    <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="tbltxt" Text=""></asp:Label></div>
            </center>
             <fieldset class="cnt-box">
                <legend   id="lgprchs" class="tbltxt" runat="server"
                    onclick="return lgprchs_onclick()"><b>Selection Criteria :-</b></legend>
                    <asp:Panel ID="Panel1" runat="server" DefaultButton = "btnDetail">
                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                 
                    <tr>
                        <td class="tbltxt" colspan="8">
                            <asp:RadioButton ID="rBtnCurrentSess" GroupName="d" Checked="true" Text="Current Due"
                                runat="server" AutoPostBack="True" 
                                OnCheckedChanged="rBtnCurrentSess_CheckedChanged" TabIndex="1" />
                            &nbsp;
                            <asp:RadioButton ID="rBtnPrevSess" GroupName="d" runat="server" Text="Prev Due" AutoPostBack="True"
                                OnCheckedChanged="rBtnCurrentSess_CheckedChanged" TabIndex="2" />
                           
                            
                        </td>
                        </tr>
                        <tr>
                        <td width="50px" class="tbltxt">
                            Session :
                        </td>
                        <td width="70px" class="tbltxt">
                            <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb"  
                                AutoPostBack="true" TabIndex="3" 
                                OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="40px">
                            Class :
                        </td>
                        <td class="tbltxt" width="70px">
                            <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                                CssClass="vsmalltb"  meta:resourcekey="drpclassResource1" 
                                TabIndex="4">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="55px">
                            Student :
                        </td>
                        <td class="tbltxt">
                            <asp:DropDownList ID="drpstudent" runat="server"   AutoPostBack="True"
                                OnSelectedIndexChanged="drpstudent_SelectedIndexChanged" 
                                CssClass="largetb" TabIndex="5">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="70">
                            Student Id :
                        </td>
                        <td class="tbltxt">
                            <asp:TextBox ID="txtadmnno" runat="server" CssClass="vsmalltb" TabIndex="6" 
                                 ></asp:TextBox><span
                                class="error">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Button ID="btnDetail" runat="server" Text="Show" OnClientClick="return valDetails();"  onfocus="active(this);" onblur="inactive(this);"
                                OnClick="btnDetail_Click" Style=" height: 26px;" 
                                TabIndex="7" />
                        </td>
                    </tr>
                    <tr>
                    <td colspan="9" align="center" class="tbltxt">
                        <asp:Label ID="lblStudDet" runat="server" Text=""></asp:Label>
                    </td>
                    </tr>
                  
                </table>
                 </asp:Panel>
             </fieldset>
            <fieldset id="Fieldset1" runat="server" class="cnt-box2 spaceborder1">
                <legend  id="Legend1" class="tbltxt" runat="server">
                    <b>Fee Details :-</b></legend>
                <table width="100%" border="0" cellpadding="0" cellspacing="2">
                    <tr>
                        <td colspan="4" style="width: 100%">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True"  meta:resourcekey="lblbalanceResource1"
                                CssClass="error" Text="Note: Available Credit Amount:- Rs. "></asp:Label>
                            <asp:Label ID="lblCreditAmt" runat="server" Font-Bold="True" ForeColor="Red" meta:resourcekey="lblbalanceResource1"
                                CssClass="error" Text="0.00"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblOldDues" runat="server" Font-Bold="True" ForeColor="Red" CssClass="error"></asp:Label>
                            <asp:Button ID="btnRcvOldDue" runat="server" OnClientClick="return valDetails();"  onfocus="active(this);" onblur="inactive(this);"
                                OnClick="btnRcvOldDue_Click" Text="Receive Old Dues" TabIndex="8" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblPrevYrBal" runat="server" Font-Bold="True" ForeColor="Red" CssClass="error"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="spacer">
                            <img src="../images/mask.gif" height="8" width="10" />
                        </td>
                    </tr>
                </table>
                <table width="100%" border="0" cellpadding="0" cellspacing="2"  class="cnt-box">
                    <tr>
                        <td colspan="3">
                            <table width="100%" cellpadding="0"  cellspacing="0">
                               
                                <tr>
                                    <td class="tbltxt" colspan="4">
                                        Misc Fee :
                                        <asp:DropDownList ID="drpAdditionalFee" runat="server" ValidationGroup="search" CssClass="vsmalltb" Width="80px" 
                                            TabIndex="11" AutoPostBack="true" 
                                            OnSelectedIndexChanged="drpAdditionalFee_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp; Misc Date :
                                        <asp:TextBox ID="txtMiscDt" runat="server" CssClass="vsmalltb" Width="60px" meta:resourcekey="txtdateResource1"
                                            TabIndex="12"></asp:TextBox>&nbsp;<rjs:PopCalendar ID="dtpMiscDt" 
                                            runat="server" Control="txtMiscDt"
                                                meta:resourcekey="PopCalendar2Resource1" ></rjs:PopCalendar>
                                        &nbsp;&nbsp;Amount:
                                        <asp:TextBox ID="txtRcevMiscFee" runat="server" CssClass="vsmalltb" TabIndex="13"
                                            onkeypress="return blockNonNumbers(this, event, true, false);" 
                                            Width="60px" MaxLength="7"></asp:TextBox>
                                        &nbsp;<asp:Button ID="btnMiscFee" CausesValidation="False" runat="server" OnClick="btnMiscFee_Click"  onfocus="active(this);" onblur="inactive(this);"
                                            Text="Update Misc Fee" meta:resourcekey="btnReverseFineResource1" TabIndex="14" 
                                            OnClientClick="return MiscFee();" />
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table width="100%" class="cnt-box2" cellpadding="2"
                                cellspacing="4">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNote" runat="server" Font-Bold="True" ForeColor="Red" meta:resourcekey="lblbalanceResource1"
                                            CssClass="error" Text="Total Payable Amount :- Rs."></asp:Label>
                                        <asp:Label ID="lblTotalFee" Text="0.00" runat="server" CssClass="error" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" border="0" cellpadding="0" cellspacing="2">
                    <tr>
                        <td align="left" valign="top">
                            <fieldset id="Fieldset3" runat="server" class="cnt-box">
                                <legend class="tbltxt"><b>School Fee :-</b></legend>
                                <table width="280px" style="height: 110px;" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="tbltxt" style="width: 91px" align="left">
                                            <asp:RadioButton ID="rbAll" runat="server" AutoPostBack="True" Checked="True" 
                                                Font-Bold="True" GroupName="1" oncheckedchanged="rbAll_CheckedChanged" 
                                                Text="All" />

                                        
                                            <asp:RadioButton ID="rbOnAdmission" runat="server" AutoPostBack="True" 
                                                GroupName="1" oncheckedchanged="rbOnAdmission_CheckedChanged" 
                                                Text="On Admission" />
                                            <asp:RadioButton ID="rbMonthly" runat="server" AutoPostBack="True" 
                                                Font-Bold="False" GroupName="1" oncheckedchanged="rbMonthly_CheckedChanged" 
                                                Text="Monthly" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="tbltxt" style="width: 91px">
                                             <table>
                                                <tr>
                                                    <td class="tbltxt" style="width: 91px">
                                                        School Fee Till
                                                    </td>
                                                    <td class="tbltxt" style="width: 5px">
                                                        :
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drpMonthPayble" runat="server" AutoPostBack="True" 
                                                            CssClass="smalltb" OnSelectedIndexChanged="drpMonthPayble_SelectedIndexChanged" 
                                                            TabIndex="15" Width="70px">
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:Button ID="btnTillDec" runat="server" onblur="inactive(this);" 
                                                            onclick="btnTillDec_Click" onfocus="active(this);" TabIndex="16" Text="Dec" 
                                                            visible="false" />
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td class="tbltxt" style="width: 91px">
                                                        Payble Amount
                                                    </td>
                                                    <td class="tbltxt" style="width: 5px">
                                                        :
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblbalance" runat="server" CssClass="error" Text="Label" meta:resourcekey="lblbalanceResource1"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  align="left">
                                            <asp:Button ID="btnShowDetails" runat="server" Text="Details" OnClick="btnShowDetails_Click"  onfocus="active(this);" onblur="inactive(this);"
                                                meta:resourcekey="btnShowDetailsResource1" TabIndex="17" ToolTip="Click to view the fee details"
                                                OnClientClick="return valFeePayable();" />
                                            <asp:Button ID="btnFeeFullYr" runat="server" Text="Full Session" OnClientClick="return valDetails();"  onfocus="active(this);" onblur="inactive(this);"
                                                OnClick="btnFeeFullYr_Click" Width="85px" TabIndex="18" />
                                            <asp:Button ID="btnInstDisc" runat="server" OnClick="btnInstDisc_Click" OnClientClick="return valDetails();"  onfocus="active(this);" onblur="inactive(this);"
                                                Text="Give Discount" TabIndex="19" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td align="left" valign="top">
                            <fieldset id="Fieldset4" runat="server" class="cnt-box">
                                <legend class="tbltxt"><b>Misc Fee :-</b></legend>
                                <table width="100%" style="height: 110px;" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="tbltxt" style="width: 95px">
                                            Misc Fee Names
                                        </td>
                                        <td class="tbltxt" style="width: 5px">
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpMiscFee" runat="server" CssClass="smalltb" Width="70px"
                                                AutoPostBack="True" 
                                                OnSelectedIndexChanged="drpMiscFee_SelectedIndexChanged" TabIndex="20">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbltxt" style="width: 100px" align="left">
                                            Payble Amount
                                        </td>
                                        <td class="tbltxt" style="width: 5px">
                                            :
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMiscFee" runat="server" CssClass="error"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right">
                                            <asp:Button ID="btnMiscDtl" runat="server" Text="Details" OnClick="btnMiscDtl_Click"  onfocus="active(this);" onblur="inactive(this);"
                                                meta:resourcekey="btnShowDetailsResource1" TabIndex="21" ToolTip="Click to view the Miscellaneous fee details"
                                                OnClientClick="return valFeePayable();" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td align="left" valign="top">
                            <fieldset id="Fieldset5" runat="server" class="cnt-box">
                                <legend class="tbltxt"><b>Bus Fee :-</b></legend>
                                <table width="100%" style="height: 110px;" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="tbltxt" style="width: 95px">
                                            Bus Fee Till
                                        </td>
                                        <td class="tbltxt" style="width: 5px">
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpBusPayble" runat="server" CssClass="smalltb" Width="70px"
                                                AutoPostBack="True" 
                                                OnSelectedIndexChanged="drpBusPayble_SelectedIndexChanged" TabIndex="22">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbltxt" style="width: 100px" align="left">
                                            Payble Amount
                                        </td>
                                        <td class="tbltxt" style="width: 5px">
                                            :
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBusFee" runat="server" CssClass="error"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right">
                                            <asp:Button ID="btnBusDetails" runat="server" Text="Details" OnClick="btnBusDetails_Click"  onfocus="active(this);" onblur="inactive(this);"
                                                meta:resourcekey="btnShowDetailsResource1" TabIndex="23" ToolTip="Click to view the bus fee details"
                                                OnClientClick="return valFeePayable();" />
                                            <asp:Button ID="btnBusFeeFull" runat="server"  onfocus="active(this);" onblur="inactive(this);"
                                                    Text="Full Session" OnClientClick="return valDetails();" OnClick="btnBusFeeFull_Click"
                                                    Width="90px" TabIndex="24" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td align="left" valign="top">
                            <fieldset id="Fieldset6" runat="server" class="cnt-box">
                                <legend class="tbltxt"><b>Hostel Fee :-</b></legend>
                             <%--   <table style="height: 110px;" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td class="tbltxt" style="width: 105px">
                                            Book Fee Till
                                        </td>
                                        <td class="tbltxt" style="width: 5px">
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpBookPayble" runat="server" CssClass="smalltb" Width="70px"
                                                AutoPostBack="True" 
                                                OnSelectedIndexChanged="drpBookPayble_SelectedIndexChanged" TabIndex="25">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbltxt" align="left">
                                            Payble Amount
                                        </td>
                                        <td class="tbltxt">
                                            :
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBookFee" runat="server" CssClass="error"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right"> 
                                            <asp:Button ID="btnBookDetails" runat="server" Text="Details" OnClick="btnBookDetails_Click"  onfocus="active(this);" onblur="inactive(this);"
                                                meta:resourcekey="btnShowDetailsResource1" TabIndex="26" ToolTip="Click to view the hostel fee details"
                                                OnClientClick="return valFeePayable();" />
                                            <asp:Button ID="btnBookFeeFull" runat="server" Text="Full Session" OnClientClick="return valDetails();"  onfocus="active(this);" onblur="inactive(this);"
                                                OnClick="btnBookFeeFull_Click" Width="90px" TabIndex="27" />
                                        </td>
                                    </tr>
                                </table>--%>
                                <table style="height: 110px;" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td class="tbltxt" style="width: 105px">
                                            Hostel Fee Till
                                        </td>
                                        <td class="tbltxt" style="width: 5px">
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpHosPayble" runat="server" CssClass="smalltb" Width="70px"
                                                AutoPostBack="True" 
                                                OnSelectedIndexChanged="drpHosPayble_SelectedIndexChanged" TabIndex="25">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbltxt" align="left">
                                            Payble Amount
                                        </td>
                                        <td class="tbltxt">
                                            :
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHostelFee" runat="server" CssClass="error"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right"> 
                                            <asp:Button ID="btnHostelDetails" runat="server" Text="Details" OnClick="btnHostelDetails_Click"  onfocus="active(this);" onblur="inactive(this);"
                                                meta:resourcekey="btnShowDetailsResource1" TabIndex="26" ToolTip="Click to view the hostel fee details"
                                                OnClientClick="return valFeePayable();" />
                                            <asp:Button ID="btnHosFeeFull" runat="server" Text="Full Session" OnClientClick="return valDetails();"  onfocus="active(this);" onblur="inactive(this);"
                                                OnClick="btnHosFeeFull_Click" Width="90px" TabIndex="27" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="Fieldset2" runat="server" class="cnt-box2 spaceborder1">
                <legend  id="Legend2" class="tbltxt" runat="server">
                    <b>Received Details :-</b></legend>
                <table width="100%" border="0" cellpadding="2" cellspacing="2">
                    <tr>
                        <td class="tbltxt" style="width: 66px" valign="top">
                            School Fee
                        </td>
                        <td class="tbltxt" style="width: 5px" valign="top">
                            :
                        </td>
                        <td style="width: 142px" valign="top">
                            <asp:TextBox ID="txtamt" runat="server" Enabled="true" CssClass="vsmalltb" meta:resourcekey="txtamtResource1"
                                TabIndex="28" Width="70px" onkeyup="getpayblamt();" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox><span class="error">*</span>
                            <asp:Button ID="btnSFClear" runat="server" Text="Clear" CssClass="error"   onfocus="active(this);" onblur="inactive(this);"
                                OnClick="btnSFClear_Click" TabIndex="29" />
                        </td>
                        <td class="tbltxt" style="width: 48px" valign="top">
                            Misc Fee
                        </td>
                        <td class="tbltxt" style="width: 5px" valign="top">
                            :
                        </td>
                        <td style="width: 143px" valign="top">
                            <asp:TextBox ID="txtMiscFee" runat="server" CssClass="vsmalltb" meta:resourcekey="txtamtResource1"
                                TabIndex="30" Enabled="true" Width="70px"></asp:TextBox><span class="error">*</span>
                            <asp:Button ID="btnMFClear" runat="server" Text="Clear" CssClass="error"  onfocus="active(this);" onblur="inactive(this);"
                                OnClick="btnMFClear_Click" TabIndex="31" />
                        </td>
                        <td class="tbltxt" style="width: 45px" valign="top">
                            Bus Fee
                        </td>
                        <td class="tbltxt" style="width: 5px" valign="top">
                            :
                        </td>
                        <td style="width: 145px" valign="top">
                            <asp:TextBox ID="txtBusFee" runat="server" CssClass="vsmalltb" meta:resourcekey="txtamtResource1"
                                TabIndex="32" Enabled="true" Width="70px"></asp:TextBox><span class="error">*</span>
                            <asp:Button ID="btnBFClear" runat="server" Text="Clear" CssClass="error"  onfocus="active(this);" onblur="inactive(this);"
                                OnClick="btnBFClear_Click" TabIndex="33" />
                        </td>
                  <%--       <td class="tbltxt" style="width: 70px" valign="top">
                            Book Fee
                        </td>
                        <td class="tbltxt" style="width: 5px" valign="top">
                            :
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtBookFee" runat="server" CssClass="vsmalltb" meta:resourcekey="txtamtResource1"
                                TabIndex="34" Width="70px"></asp:TextBox><span class="error">*</span>
                            <asp:Button ID="btnHFClear" runat="server" Text="Clear" CssClass="error"  onfocus="active(this);" onblur="inactive(this);"
                                OnClick="btnHFClear_Click" TabIndex="35" />
                        </td>--%>
                        <td class="tbltxt" style="width: 70px" valign="top">
                            Hostel Fee
                        </td>
                        <td class="tbltxt" style="width: 5px" valign="top">
                            :
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtHostelFee" runat="server" CssClass="vsmalltb" meta:resourcekey="txtamtResource1"
                                TabIndex="34" Enabled="false" Width="70px"></asp:TextBox><span class="error">*</span>
                            <asp:Button ID="btnHFClear" runat="server" Text="Clear" CssClass="error"  onfocus="active(this);" onblur="inactive(this);"
                                OnClick="btnHFClear_Click" TabIndex="35" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" style="width: 66px; height: 36px;">
                            Receive Date
                        </td>
                        <td class="tbltxt" style="height: 36px">
                            :
                        </td>
                        <td style="height: 36px" >
                            <asp:TextBox ID="txtdate" runat="server" CssClass="vsmalltb" Width="80px" TabIndex="36"></asp:TextBox>&nbsp;
                            <rjs:PopCalendar ID="dtpFeeDt" runat="server" Control="txtdate" To-Today="true"></rjs:PopCalendar>
                            <span class="error">*</span>
                        </td>
                        <td class="tbltxt" style="height: 36px">
                            Payment Mode
                        </td>
                        <td class="tbltxt" style="height: 36px">
                            :
                        </td>
                        <td align="left" colspan="5" style="height: 36px">
                            <asp:DropDownList ID="drpPmtMode" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                                TabIndex="37" OnSelectedIndexChanged="drpPmtMode_SelectedIndexChanged">
                                <asp:ListItem>Cash</asp:ListItem>
                                <asp:ListItem>Bank</asp:ListItem>
                            </asp:DropDownList>
                        &nbsp;<asp:DropDownList ID="drpBankAc" runat="server" Width="250px" 
                                ToolTip="The amount will be reflected on this Bank Account">
                        </asp:DropDownList>
                        </td>
                        <td rowspan="4" colspan="2" valign="middle" align="center">
                            <asp:Image ID="imgStud" runat="server" Width="130px" Height="130px" ImageUrl="../Up_Files/Studimage/noimage.jpg" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" style="width: 66px">
                            Instrument Date
                        </td>
                        <td>
                            :
                        </td>
                        <td style="width: 142px">
                            <asp:TextBox ID="txtChqDate" runat="server" CssClass="vsmalltb" TabIndex="38"></asp:TextBox>
                        </td>
                        <td class="tbltxt">
                            Instrument No
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td style="width: 143px">
                            <asp:TextBox ID="txtChqNo" runat="server" CssClass="vsmalltb" TabIndex="39"></asp:TextBox>
                        </td>
                        <td class="tbltxt">
                            Bank Name
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtBank" runat="server" CssClass="largetb" TabIndex="40"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" style="width: 66px">
                            Description
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td colspan="10">
                            <asp:TextBox ID="txtdesc" runat="server" CssClass="largetb" Width="570px" meta:resourcekey="txtdescResource1"
                                TabIndex="41"></asp:TextBox><span class="error">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" style="width: 66px">
                        </td>
                        <td class="tbltxt">
                            &nbsp;
                        </td>
                        <td colspan="3">
                            <asp:Button ID="btnsave" runat="server" OnClick="btnsave_Click" Text="Submit" meta:resourcekey="btnsaveResource1"  onfocus="active(this);" onblur="inactive(this);"
                                TabIndex="42" OnClientClick="return valShow();" />&nbsp;
                            <asp:Button ID="btncancel" runat="server" CausesValidation="False" OnClick="btncancel_Click"  onfocus="active(this);" onblur="inactive(this);"
                                Text="Cancel" meta:resourcekey="btncancelResource1" TabIndex="43" />
                        </td>
                        <td colspan="7" >
                            <asp:Label ID="lblMsg2" runat="server" Font-Bold="true" CssClass="tbltxt" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <%--<ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>--%>
       
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
            <asp:PostBackTrigger ControlID="drpMonthPayble" />
            <asp:PostBackTrigger ControlID="rbAll" />
            <asp:PostBackTrigger ControlID="rbOnAdmission" />
            <asp:PostBackTrigger ControlID="rbMonthly" />
            <%--<asp:PostBackTrigger ControlID="drpstudent" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <%--loader--%>
    <div class="loading" align="center">
        Loading. Please wait.<br />
        <br />
        <img src="../images/loading.gif" alt="" />
    </div>
</asp:Content>


