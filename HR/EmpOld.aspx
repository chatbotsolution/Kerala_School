<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpOld.aspx.cs" Inherits="HR_EmpOld" %>

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
            width: 400px;
            height: 170px;
            position: relative;
            top: 120px;
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
    <script language="javascript">
        function ReplaceEmptyFields(orig, repl) {
            if (orig == "") {
                document.write(repl);
            }
            else {
                document.write(orig);
            }
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
                alert("Please select any record");
                return false;
            }
        }

        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';

        }
        function clearText(btn) {
            switch (btn) {
                case '1':
                    {
                        document.getElementById("<%=txtDOB.ClientID %>").value = "";
                        return false;
                    }

                case '3':
                    {
                        document.getElementById("<%=txtFirstJoinDt.ClientID %>").value = "";
                        return false;
                    }
                case '4':
                    {
                        document.getElementById("<%=txtTrainedDate.ClientID %>").value = "";
                        return false;
                    }
                case '5':
                    {
                        document.getElementById("<%=txtEPFDate.ClientID %>").value = "";
                        return false;
                    }
                case '6':
                    {
                        document.getElementById("<%=txtGLSIDate.ClientID %>").value = "";
                        return false;
                    }
                case '7':
                    {
                        document.getElementById("<%=txtLeavingDate.ClientID %>").value = "";
                        document.getElementById("<%=drpStatus.ClientID %>").selectedIndex = 0;
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

        function DateOfBirth() {

            var start_date = document.getElementById('<%=txtDOB.ClientID%>').value;
            start_date = start_date.split("-")
            year = start_date[2];
            day = start_date[1];
            month = start_date[0];
            var currentdate = new Date();
            var myDate = new Date(year, month, day);
            var currday = currentdate.getDate();
            if (myDate > currentdate) {
                alert("Date of Birth can't be greater than Current Date");
                return false;
            }
        }

        function validateform() {
            var SevabratiId = document.getElementById("<%=txtSevabratiId.ClientID %>").value;
            var SevabratiName = document.getElementById("<%=txtSevabratiName.ClientID %>").value;
            var EduQual = document.getElementById("<%=drpEduQual.ClientID %>").value;
            var dob = document.getElementById("<%=txtDOB.ClientID %>").value;
            var Designation = document.getElementById("<%=drpDesignation.ClientID %>").value;
            var AcharyaType = document.getElementById("<%=drpAcharyaType.ClientID %>").value;
            var EmailId = document.getElementById("<%=txtEmail.ClientID %>").value;
            var fdoj = document.getElementById("<%=txtFirstJoinDt.ClientID %>").value;
            var apNo = document.getElementById("<%=txtAppOrderNo.ClientID %>").value;
            var gender = document.getElementById("<%=drpGender.ClientID %>").value;


            if (AcharyaType == "0") {
                alert("Select Appointment Type");
                document.getElementById("<%=drpAcharyaType.ClientID %>").focus();
                return false;
            }
            if (fdoj.trim() == "") {
                alert("Provide Date of Joining");
                document.getElementById("<%=txtFirstJoinDt.ClientID %>").focus();
                return false;
            }

            if (Designation == "0") {
                alert("Select Designation");
                document.getElementById("<%=drpDesignation.ClientID %>").focus();
                return false;
            }
            if (SevabratiName.trim() == "") {
                alert("Enter Employee Name");
                document.getElementById("<%=txtSevabratiName.ClientID %>").focus();
                return false;
            }
            if (EduQual == "0") {
                alert("Select Educational Qualification");
                document.getElementById("<%=drpEduQual.ClientID %>").focus();
                return false;
            }
            if (dob.trim() == "") {
                alert("Provide Date of Birth");
                document.getElementById("<%=txtDOB.ClientID %>").focus();
                return false;
            }
            if (gender.trim() == "0") {
                alert("Select a Gender");
                document.getElementById("<%=drpGender.ClientID %>").focus();
                return false;
            }
            var status = document.getElementById("<%=drpStatus.ClientID %>");
            var leaveDt = document.getElementById("<%=txtLeavingDate.ClientID %>");
            if (status.value == "0" && leaveDt.value.trim() != "") {
                alert("Provide Leaving Date only when Employee is not Active");
                leaveDt.focus();
                return false;
            }
            else {
                CheckLoader();
                return true;
            }

        }

        function CopyAddress() {
            var PresAddress = document.getElementById("<%=txtPresAddress.ClientID %>").value;
            var PresDist = document.getElementById("<%=drpPresAddDistId.ClientID %>").value;
            var PresPin = document.getElementById("<%=txtPresPin.ClientID %>").value;

            document.getElementById("<%=txtPermAddress.ClientID %>").value = PresAddress;
            document.getElementById("<%=drpPermAddDistId.ClientID %>").value = PresDist;
            document.getElementById("<%=txtPermPin.ClientID %>").value = PresPin;
        }


        function CnfDelete() {

            if (confirm("You are going to Assign Shift To selected Employee(s). Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }

        function pop(div) {
            document.getElementById(div).style.display = 'block';
            return false;
        }
        function hide(div) {
            document.getElementById(div).style.display = 'none';
            return false;
        }

    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Employee Details</h2>
        <div style="float: right;">
            <asp:Label ID="lblMsgTop" Font-Bold="true" ForeColor="Green" runat="server" Text=""
                CssClass="tbltxt"></asp:Label>
        </div>
    </div>
    <div align="left">
        <div class="innerdiv" style="width: 990px">
            <div style="padding: 8px;">
                <table align="center" cellpadding="2" cellspacing="2">
                    <tr>
                        <td colspan="2" valign="top">
                            <table id="tblInsAppointments" runat="server" width="100%" style="border: solid 1px gray;
                                height: 140px;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" colspan="8">
                                        <strong>Initial Appointment Details</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        Appointment Order No.
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <span class="mandatory">
                                            <asp:TextBox ID="txtAppOrderNo" runat="server" CssClass="tbltxtbox" MaxLength="20"
                                                TabIndex="1"></asp:TextBox>
                                        </span>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        Employee Type
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:RadioButton ID="optTeach" runat="server" AutoPostBack="True" Checked="true"
                                            CssClass="tbltxt" GroupName="EmpType" OnCheckedChanged="optTeach_CheckedChanged"
                                            TabIndex="6" Text="Teaching" />
                                        <asp:RadioButton ID="optNonTeach" runat="server" AutoPostBack="True" CssClass="tbltxt"
                                            GroupName="EmpType" OnCheckedChanged="optNonTeach_CheckedChanged" TabIndex="5"
                                            Text="Non-Teaching" />
                                        <span class="mandatory">*</span>
                                    </td>
                                    <%--<td align="left" valign="top" class="tbltxt">Engagement Date
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">:
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                        <asp:TextBox ID="txtAppDate" runat="server" CssClass="tbltxtbox_mid" 
                                            TabIndex="2"></asp:TextBox>*<rjs:popcalendar
                                            ID="dtpAppoint" runat="server" AutoPostBack="False" Control="txtAppDate" 
                                            Format="dd mmm yyyy" />
                                        &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                            OnClientClick="return clearText('2');" />
                                    </td>
                                    <td align="left" valign="top" class="mandatory"></td>--%>
                                </tr>
                                <tr>
                                    <td align="left" class="tbltxt" valign="top">
                                        Penal Year&nbsp;
                                    </td>
                                    <td align="left" class="tbltxt" valign="top">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="drpSessionYr" CssClass="tbltxtbox_mid" runat="server" TabIndex="4">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" class="mandatory" valign="top">
                                    </td>
                                    <td align="left" class="tbltxt" valign="top">
                                        Appointment Type
                                    </td>
                                    <td align="left" class="tbltxt" valign="top">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="drpAcharyaType" runat="server" CssClass="tbltxtbox_mid" TabIndex="9">
                                            <asp:ListItem Value="0">- Select -</asp:ListItem>
                                            <asp:ListItem>Permanent</asp:ListItem>
                                            <asp:ListItem>Temporary</asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="mandatory">*</span>
                                    </td>
                                    <%--<td align="left" class="tbltxt" valign="top">
                                                Acharya Type
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:DropDownList ID="drpAcharyaType" runat="server" CssClass="tbltxtbox_mid" TabIndex="3">
                                                    <asp:ListItem Value="0">- Select -</asp:ListItem>
                                                    <asp:ListItem>Permanent</asp:ListItem>
                                                    <asp:ListItem>Temporary</asp:ListItem>
                                                </asp:DropDownList>
                                                <span class="mandatory">*</span>
                                            </td>
                                            <td align="left" class="mandatory" valign="top">
                                            </td>--%>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        Employee ID
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                        <asp:TextBox ID="txtSevabratiId" runat="server" CssClass="tbltxtbox_mid" MaxLength="20"
                                            TabIndex="63"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        Designation
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="drpDesignation" runat="server" CssClass="tbltxtbox" TabIndex="9">
                                        </asp:DropDownList>
                                        <span class="mandatory">*</span>
                                    </td>
                                    <%-- <td align="left" valign="top" class="tbltxt">
                                                Designation
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:DropDownList ID="drpDesignation" runat="server" CssClass="tbltxtbox" TabIndex="5">
                                                </asp:DropDownList>
                                                <span class="mandatory">*</span>
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>--%>
                                </tr>
                                <tr>
                                    <td align="left" class="tbltxt" valign="top">
                                        Date of Joinining
                                    </td>
                                    <td align="left" class="tbltxt" valign="top">
                                        :
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                        <asp:TextBox ID="txtFirstJoinDt" runat="server" CssClass="tbltxtbox_mid" TabIndex="4"></asp:TextBox>*
                                        <rjs:PopCalendar ID="dtpFirstJoinDt" runat="server" AutoPostBack="False" Control="txtFirstJoinDt"
                                            Format="dd mmm yyyy" />
                                        <asp:ImageButton ID="ImageButton3" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                            OnClientClick="return clearText('3');" />
                                    </td>
                                    <td align="left" class="mandatory" valign="top">
                                        &nbsp;
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                            </table>
                            <asp:FormView Visible="false" ID="fvAppointments" runat="server" Width="100%">
                                <ItemTemplate>
                                    <table width="100%" style="border: solid 1px gray; height: 120px;" cellpadding="0"
                                        cellspacing="0">
                                        <tr>
                                            <td align="left" class="headerrow" colspan="8">
                                                <strong>Initial Appointment Details<strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 5px;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt" style="width: 155px;">
                                                Appointment Order No.
                                            </td>
                                            <td align="left" valign="top" class="tbltxt" style="width: 5px;">
                                                :
                                            </td>
                                            <td class="tbltxt" align="left" valign="top" style="width: 140px;">

                                                <script language="javascript">                                                    ReplaceEmptyFields('<%#Eval("AppointmentOrderNoD")%>', "<font color='gray'>N/A</font>")</script>

                                            </td>
                                            <td class="tbltxt" align="left" valign="top">
                                            </td>
                                            <td align="left" valign="top" class="tbltxt" style="width: 200px;">
                                                Appointment Date
                                            </td>
                                            <td align="left" valign="top" class="tbltxt" style="width: 5px;">
                                                :
                                            </td>
                                            <td class="tbltxt" align="left" valign="top" style="width: 200px;">

                                                <script language="javascript">                                                    ReplaceEmptyFields('<%#Eval("AppointmentDtD")%>', "<font color='gray'>N/A</font>")</script>

                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt">
                                                Designation
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td class="tbltxt" align="left" valign="top">

                                                <script language="javascript">                                                    ReplaceEmptyFields('<%#Eval("Designation")%>', "<font color='gray'>N/A</font>")</script>

                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                Appointment Type
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td class="tbltxt" align="left" valign="top">

                                                <script language="javascript">                                                    ReplaceEmptyFields('<%#Eval("AcharyaType")%>', "<font color='gray'>N/A</font>")</script>

                                            </td>
                                            <td align="left" class="mandatory" valign="top">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt">
                                                Employee ID
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td class="tbltxt" align="left" valign="top">
                                                <%#Eval("SevabratiId")%>
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                            <td class="tbltxt" align="left" class="tbltxt" valign="top">
                                                Date of First Joinining
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td class="tbltxt" align="left" valign="top">

                                                <script language="javascript">                                                    ReplaceEmptyFields('<%#Eval("DOJD")%>', "<font color='gray'>N/A</font>")</script>

                                            </td>
                                            <td align="left" class="mandatory" valign="top">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:FormView>
                        </td>
                        <td align="center" style="width: 100px;" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="100%" colspan="2">
                            <table style="border: solid 1px gray; width: 100%; height: 180px;" cellpadding="0"
                                cellspacing="0">
                                <tr>
                                    <td align="left" valign="top" colspan="8" class="headerrow" style="height: 25px;">
                                        <strong>Personal Information</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" colspan="8" style="width: 10; height: 10;">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt" style="width: 113px">
                                        Name of Employee
                                    </td>
                                    <td align="left" valign="top" class="tbltxt" width="5px">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 237px">
                                        <asp:TextBox ID="txtSevabratiName" runat="server" CssClass="tbltxtbox" MaxLength="30"
                                            TabIndex="10"></asp:TextBox>
                                        <span class="mandatory">*</span>
                                    </td>
                                    <td align="left" valign="top" class="mandatory" style="width: 13px">
                                    </td>
                                    <td align="left" valign="top" class="tbltxt" style="width: 159px">
                                        Religion
                                    </td>
                                    <td align="left" valign="top" class="tbltxt" width="5px">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 218px">
                                        <asp:DropDownList ID="drpReligion" runat="server" TabIndex="17" CssClass="tbltxtbox_mid">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" valign="top" class="mandatory" rowspan="6">
                                        <asp:Image ID="imgEmployee" Width="116px" runat="server" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="1px" ImageUrl="~/images/noimage.jpg" Height="121px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt" style="width: 113px">
                                        Father&#39;s Name
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 237px">
                                        <asp:TextBox ID="txtFathName" runat="server" CssClass="tbltxtbox" MaxLength="30"
                                            TabIndex="11"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory" style="width: 13px">
                                    </td>
                                    <td align="left" valign="top" class="tbltxt" style="width: 159px">
                                        Category
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 218px">
                                        <asp:DropDownList ID="drpCategory" runat="server" TabIndex="18" CssClass="tbltxtbox_mid">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt" style="width: 113px">
                                        Mother&#39;s name
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 237px">
                                        <asp:TextBox ID="txtMothName" runat="server" CssClass="tbltxtbox" MaxLength="30"
                                            TabIndex="12"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory" style="width: 13px">
                                    </td>
                                    <td align="left" valign="top" class="tbltxt" style="width: 159px">
                                        Educational Qualification
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 218px">
                                        <asp:DropDownList ID="drpEduQual" runat="server" CssClass="tbltxtbox" TabIndex="19">
                                        </asp:DropDownList>
                                        <span class="mandatory">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt" style="width: 113px">
                                        Spouse&#39;s Name
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 237px">
                                        <asp:TextBox ID="txtSposeName" runat="server" CssClass="tbltxtbox" MaxLength="30"
                                            TabIndex="13"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory" style="width: 13px">
                                    </td>
                                    <td align="left" valign="top" class="tbltxt" style="width: 159px">
                                        Extra Qualification
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 218px">
                                        <asp:TextBox ID="txtExtraQual" MaxLength="30" runat="server" CssClass="tbltxtbox"
                                            TabIndex="20"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt" style="width: 113px">
                                        Date of Birth
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 237px">
                                        <asp:TextBox ID="txtDOB" runat="server" CssClass="tbltxtbox_mid" TabIndex="14"></asp:TextBox>
                                        <rjs:PopCalendar ID="dtpDOB" runat="server" AutoPostBack="False" Control="txtDOB"
                                            Format="dd mmm yyyy" />
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                            OnClientClick="return clearText('1');" /><span class="mandatory">*</span>
                                    </td>
                                    <td align="left" valign="top" class="mandatory" style="width: 13px">
                                    </td>
                                    <td align="left" valign="top" class="tbltxt" style="width: 159px">
                                        Upload Employee Photo
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 218px">
                                        <asp:FileUpload ID="fuEmpImage" runat="server" TabIndex="21" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt" style="width: 113px">
                                        Gender
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 237px">
                                        <asp:DropDownList ID="drpGender" runat="server" CssClass="tbltxtbox" Width="100px">
                                            <asp:ListItem Selected="true" Value="0">- Select -</asp:ListItem>
                                            <asp:ListItem Text="Male" Value="M"></asp:ListItem>
                                            <asp:ListItem Text="Female" Value="F"></asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="mandatory">*</span>
                                    </td>
                                    <td align="left" valign="top" class="mandatory" style="width: 13px">
                                        &nbsp;
                                    </td>
                                    <td align="left" valign="top" class="tbltxt" style="width: 159px">
                                        Blood Group
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top" style="width: 218px">
                                        <asp:TextBox ID="txtBloodGrp" runat="server" TabIndex="22"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            <table width="100%" style="border: solid 1px gray; width: 100%; height: 210px;" cellpadding="0"
                                cellspacing="0">
                                <tr>
                                    <td align="left" valign="top" colspan="4" class="headerrow" style="height: 25px;">
                                        <strong>Present Address</strong>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt" width="120px">
                                        Present Address
                                    </td>
                                    <td align="left" valign="top" class="tbltxt" width="5px">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtPresAddress" runat="server" MaxLength="60" Height="50px" Width="200px"
                                            TextMode="MultiLine" TabIndex="23" CssClass="txtarea"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        District
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="drpPresAddDistId" runat="server" CssClass="tbltxtbox" TabIndex="24">
                                        </asp:DropDownList>
                                         <asp:Button ID="btnReassign" Text="Create District Name" runat="server" OnClick="btnReassign_Click" />
                                        <div id="pop1" class="parentDisable">
                                            <center>
                                                <div id="popup">
                                                    <a href="#" onclick="return hide('pop1')">
                                                        <img id="close" src="../images/refresh_icon.png" alt="Close" />
                                                    </a>
                                                    <div align="center">
                                                        <h2>
                                                            Create District Name</h2>
                                                        <table>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label runat="server" Text="" ID="lblMsg2" Width="390px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    District Name:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtdistric" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                
                                                                <td colspan="3" align="center">
                                                                    <asp:Button ID="btnSaveShift" runat="server" Text="Save" OnClick="btnSaveShift_Click"
                                                                        OnClientClick="return ReValid();" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </center>
                                        </div>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        Pincode
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtPresPin" runat="server" CssClass="tbltxtbox" MaxLength="6" TabIndex="25"
                                            onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        Phone No
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="tbltxtbox" TabIndex="26" onkeypress="return blockNonNumbers(this, event, true, false);"
                                            MaxLength="14"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        Mobile No
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="tbltxtbox" TabIndex="27" onkeypress="return blockNonNumbers(this, event, true, false);"
                                            MaxLength="14"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        Email ID
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="tbltxtbox" TabIndex="28" MaxLength="50"
                                            ValidationGroup="vgSubmit"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 50%">
                            <table width="100%" style="border: solid 1px gray; width: 100%; height: 210px;" cellpadding="0"
                                cellspacing="0">
                                <tr>
                                    <td align="left" valign="top" style="height: 25px;" class="headerrow" colspan="4">
                                        <strong>Permanent Address</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left" valign="middle" class="tbltxt">
                                        <input type="button" id="btnSameAsPre" value="Same as Present Addres" tabindex="29"
                                            onclick="CopyAddress();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        Permanent Address
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtPermAddress" runat="server" TextMode="MultiLine" CssClass="txtarea"
                                            Height="50px" Width="200px" TabIndex="30" MaxLength="60"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        District
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="drpPermAddDistId" runat="server" CssClass="tbltxtbox" TabIndex="31">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        Pincode
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtPermPin" runat="server" CssClass="tbltxtbox" MaxLength="6" TabIndex="32"
                                            onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="border: solid 1px gray; width: 100%;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="top" colspan="8" class="headerrow" style="height: 25px;">
                                        <strong>Bank Account Details</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        Bank Name
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtBank" runat="server" TabIndex="33"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        Bank A/c No.
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtEmpBankAcNo" runat="server" CssClass="tbltxtbox" TabIndex="35"
                                            MaxLength="20"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tbltxt">
                                        Branch</td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtBranch" runat="server" CssClass="tbltxtbox" TabIndex="34"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                     <td align="left" valign="top" class="tbltxt">
                                        IFSC Code.
                                    </td>
                                    <td align="left" valign="top" class="tbltxt">
                                        :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtIFSC" runat="server" CssClass="tbltxtbox" TabIndex="35"
                                            MaxLength="20"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="mandatory">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel ID="updtpnl1" runat="server">
                                <ContentTemplate>
                                    <table style="border: solid 1px gray; width: 100%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top" colspan="8" class="headerrow" style="height: 25px;">
                                                <strong>Accounts Details</strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt">
                                                EPF A/c No
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtEPFAcNo" runat="server" CssClass="tbltxtbox" Width="200px" TabIndex="36"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                EPF Effect Date
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtEPFDate" runat="server" CssClass="tbltxtbox_mid" TabIndex="37"></asp:TextBox>
                                                <rjs:PopCalendar ID="dtpEPFDate" runat="server" AutoPostBack="False" Control="txtEPFDate"
                                                    Format="dd mmm yyyy" />
                                                <asp:ImageButton ID="ImageButton5" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                                    OnClientClick="return clearText('5');" />
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt">
                                                Is GSLI Done
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:RadioButton ID="optGLSIDoneY" runat="server" Text="Yes" AutoPostBack="True"
                                                    OnCheckedChanged="optGLSIDoneY_CheckedChanged" GroupName="GSLI" Checked="true"
                                                    TabIndex="38" />&nbsp;<asp:RadioButton ID="optGLSIDoneN" runat="server" Checked="false"
                                                        Text="No" AutoPostBack="True" OnCheckedChanged="optGLSIDoneY_CheckedChanged"
                                                        GroupName="GSLI" TabIndex="39" />
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                GSLI Date
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox Enabled="true" ID="txtGLSIDate" runat="server" CssClass="tbltxtbox_mid"
                                                    TabIndex="40"></asp:TextBox>
                                                <rjs:PopCalendar ID="dtpGLSIDate" Enabled="true" runat="server" AutoPostBack="False"
                                                    Control="txtGLSIDate" Format="dd mmm yyyy" />
                                                <asp:ImageButton ID="ImageButton6" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                                    OnClientClick="return clearText('6');" />
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt">
                                                GSLI Diposite Amount
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtGSLIAmt" Enabled="true" runat="server" CssClass="tbltxtbox_mid"
                                                    Width="150px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                    TabIndex="41"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                            </td>
                                            <td align="left" valign="top">
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" colspan="8" class="headerrow" style="height: 25px;">
                                                <strong>Other Details</strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" colspan="8" style="width: 10; height: 10;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt">
                                                Trained
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:RadioButton ID="optTrained" runat="server" Checked="true" GroupName="trained"
                                                    Text="Trained" AutoPostBack="True" OnCheckedChanged="optTrained_CheckedChanged"
                                                    TabIndex="42" />&nbsp;<asp:RadioButton ID="optUntrained" Text="Untrained" runat="server"
                                                        Checked="false" GroupName="trained" AutoPostBack="True" OnCheckedChanged="optTrained_CheckedChanged"
                                                        TabIndex="43" />
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                Trained Date
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox Enabled="true" ID="txtTrainedDate" runat="server" CssClass="tbltxtbox_mid"
                                                    TabIndex="44"></asp:TextBox>
                                                <rjs:PopCalendar ID="dtpTrainedDate" Enabled="true" runat="server" AutoPostBack="False"
                                                    Control="txtTrainedDate" Format="dd mmm yyyy" />
                                                <asp:ImageButton ID="ImageButton4" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                                    OnClientClick="return clearText('4');" />
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt">
                                                Marital Status
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:DropDownList ID="drpMaritalStat" CssClass="tbltxtbox" Width="150px" runat="server"
                                                    TabIndex="45">
                                                    <asp:ListItem>Married</asp:ListItem>
                                                    <asp:ListItem>Unmarried</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                Remarks
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="tbltxtbox" MaxLength="200"
                                                    Width="250px" TabIndex="46"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt">
                                                Service
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:DropDownList ID="drpStatus" CssClass="tbltxtbox_mid" runat="server" AutoPostBack="true"
                                                    TabIndex="47" OnSelectedIndexChanged="drpStatus_SelectedIndexChanged">
                                                    <asp:ListItem>In Service</asp:ListItem>
                                                    <asp:ListItem>Out Of Service</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                Leaving Date(if other than <b>Active</b>)
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox Enabled="true" ID="txtLeavingDate" runat="server" CssClass="tbltxtbox_mid"
                                                    TabIndex="48"></asp:TextBox>
                                                <rjs:PopCalendar ControlFocusOnError="true" ID="dtpLeavingDate" Enabled="true" runat="server"
                                                    AutoPostBack="True" Control="txtLeavingDate" Format="dd mmm yyyy" OnSelectionChanged="dtpLeavingDate_SelectionChanged" />
                                                <asp:ImageButton ID="ImageButton7" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                                    OnClientClick="return clearText('7');" />
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" colspan="8" class="headerrow" style="height: 25px;">
                                                <strong>Educational Qualification</strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" colspan="8" style="width: 10; height: 10;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt">
                                                Qualification
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:DropDownList ID="drpQual" runat="server" CssClass="tbltxtbox" Width="150px"
                                                    TabIndex="49">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                Board/University Name
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtBoardUnivers" runat="server" CssClass="tbltxtbox" MaxLength="200"
                                                    Width="250px" TabIndex="50"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt">
                                                Subject
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtSubject" runat="server" CssClass="tbltxtbox" MaxLength="200"
                                                    Width="250px" TabIndex="51"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                Mark Percentage
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                                <asp:TextBox ID="txtMarksPer" runat="server" CssClass="tbltxtbox-no-rb" MaxLength="3"
                                                    onkeypress="return blockNonNumbers(this, event, true, false);" Width="50px" TabIndex="52"></asp:TextBox><asp:TextBox
                                                        ID="TextBox1" runat="server" ReadOnly="true" Text="%" Width="20px" CssClass="tbltxtbox-no-lb"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top" class="mandatory">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td-all" colspan="3" align="center">
                                                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" OnClientClick="return valAdd();"
                                                    TabIndex="53" />&nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click"
                                                        TabIndex="54" />&nbsp;<asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick="return valid();"
                                                            OnClick="btnDelete_Click" TabIndex="55" />
                                            </td>
                                            <td colspan="5" align="left" class="td-help tbltxt">
                                                ** To modify an added education details, delete it by selecting the corresponding
                                                checkbox and click on <b>Delete</b> then <b>Add</b> it again with correct information.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td-bottom" colspan="8">
                                                <asp:HiddenField runat="server" ID="hfEduQual" Value="0" />
                                                <asp:GridView ID="gvEduction" runat="server" AutoGenerateColumns="False" CssClass="gridtext"
                                                    Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                            <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                            <ItemTemplate>
                                                                <input name="Checkb" type="checkbox" value='<%#Eval("QualId")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" Course">
                                                            <ItemTemplate>
                                                                <%#Eval("QualName")%>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" Board/University Name">
                                                            <ItemTemplate>
                                                                <%#Eval("BoardUnivName")%>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Subjects">
                                                            <ItemTemplate>
                                                                <%#Eval("Subjects")%>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Marks(%)">
                                                            <ItemTemplate>
                                                                <%#Eval("MarkPercent")%>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="100px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        No Qualification added !
                                                    </EmptyDataTemplate>
                                                    <PagerStyle CssClass="headergrid" HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="headergrid" />
                                                    <AlternatingRowStyle CssClass="gridtext" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" Font-Bold="true" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnAdd" />
                                    <asp:PostBackTrigger ControlID="optGLSIDoneY" />
                                    <asp:PostBackTrigger ControlID="optTrained" />
                                    <asp:PostBackTrigger ControlID="drpStatus" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 13%; height: 17px;" valign="top" colspan="2">
                            <div style="width: 100%;" align="center">
                                <asp:Button ID="btnSave" Font-Size="11px" runat="server" Text="Save & Continue" OnClientClick="return validateform();"
                                    OnClick="btnSave_Click" ValidationGroup="vgSubmit" TabIndex="56" />&nbsp;<asp:Button
                                        ID="btnShow" runat="server" Text="Save & Go to List" OnClientClick="return validateform();"
                                        OnClick="btnShow_Click" TabIndex="57" Font-Size="11px" />&nbsp;<asp:Button ID="btnReset"
                                            Font-Size="11px" runat="server" Text="Reset" OnClick="btnReset_Click" TabIndex="58" />&nbsp;<asp:Button
                                                ID="btnBack" runat="server" Text="Back" Font-Size="11px" OnClick="btnBack_Click"
                                                TabIndex="59" />
                                &nbsp;
                                <%--<asp:Button ID="btnAppDet" runat="server" Text="Appointment/Transfer Details"
                                                    OnClick="btnAppDet_Click" TabIndex="60" Font-Size="11px" />&nbsp;<asp:Button
                                                                ID="btnTrainingDet" runat="server" 
                                    Text="Training Details" OnClick="btnTrainingDet_Click"
                                                                TabIndex="61" Font-Size="11px" />&nbsp;--%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" class="td-help">
                            ** Above Buttons (Past Appointments,Training Details) will navigate to the respective
                            page where you can add corresponding information for the last created or, modified
                            Sevabrati.
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <%--<div id="Loader" style="text-align: center; vertical-align: middle; position: absolute;
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


