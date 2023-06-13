<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="HostRcptPrevDue.aspx.cs" Inherits="Hostel_HostRcptPrevDue" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript" language="javascript">

    function valFeePayable() {
        var student = document.getElementById("<%=drpstudent.ClientID %>").value;
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
        var Amnt = document.getElementById("<%=txtamt.ClientID %>").value;
        var student = document.getElementById("<%=drpstudent.ClientID %>").value;
        if (student == "" || student == "0") {
            alert("Please select a student !");
            return false;
        }
        if (Amnt.trim() == "") {
            alert("Please enter receive amount !");
            document.getElementById("<%=txtamt.ClientID %>").focus();
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
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Receive Old Dues Before Computerisation</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="2" cellspacing="2">
                <tr>
                    <td width="100" class="tbltxt">
                        Session
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" AutoPostBack="true"
                            TabIndex="1" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Select Class
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                            CssClass="vsmalltb" meta:resourcekey="drpclassResource1" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Student
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpstudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpstudent_SelectedIndexChanged"
                            CssClass="largetb" meta:resourcekey="drpstudentResource1" TabIndex="3">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Student Id
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtadmnno" runat="server" CssClass="vsmalltb" AutoPostBack="True"
                            OnTextChanged="txtadminno_TextChanged" TabIndex="4"></asp:TextBox><span class="error">*</span>
                        &nbsp;
                        <asp:Button ID="btnDetail" runat="server" Text="Show" OnClientClick="return valDetails();"
                            OnClick="btnDetail_Click" />
                        &nbsp; &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Previous Due
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblbalance" runat="server" CssClass="error" Text="Label" meta:resourcekey="lblbalanceResource1"></asp:Label>&nbsp;<asp:Button
                            ID="btnShowDetails" runat="server" Text="Show Details" OnClick="btnShowDetails_Click" Visible="false"
                            Width="86px" meta:resourcekey="btnShowDetailsResource1" TabIndex="5" ToolTip="Click to view the fee details"
                            OnClientClick="return valFeePayable();" />
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Receive Date
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtdate" runat="server" CssClass="vsmalltb" meta:resourcekey="txtdateResource1" ReadOnly="true"
                            TabIndex="9"></asp:TextBox>&nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server"
                                Control="txtdate" meta:resourcekey="PopCalendar2Resource1" To-Today="true">
                        </rjs:PopCalendar>
                        <span class="error">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Received Amount
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtamt" onkeypress="return blockNonNumbers(this, event, true, false);"  runat="server" CssClass="vsmalltb" MaxLength="7" meta:resourcekey="txtamtResource1"
                            TabIndex="10"></asp:TextBox><span class="error">*</span>
                    </td>
                </tr>
                <%--<tr>
                    <td class="tbltxt">
                        Payment Mode
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpPmtMode" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                            TabIndex="12" OnSelectedIndexChanged="drpPmtMode_SelectedIndexChanged">
                            <asp:ListItem>Cash</asp:ListItem>
                            <asp:ListItem>Bank</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>--%>
                <%--<tr>
                    <td class="tbltxt">
                        Instrument Date
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtChqDate" runat="server" CssClass="vsmalltb" TabIndex="13"></asp:TextBox>
                        <span class="error">*</span> &nbsp;<span class="tbltxt">Instrument No : </span>
                        <asp:TextBox ID="txtChqNo" runat="server" CssClass="vsmalltb" TabIndex="14"></asp:TextBox>
                        &nbsp; <span class="tbltxt"><span class="error">*</span> Drawn on Bank :</span>
                        <asp:TextBox ID="txtBank" runat="server" CssClass="largetb" TabIndex="15"></asp:TextBox><span
                            class="error">*</span>
                    </td>
                </tr>--%>
                <tr>
                    <td class="tbltxt" colspan="2">
                    </td>
                    <td class="tbltxt" style="color:Red;">
                        **Only Cash Receipt Allowed
                    </td>
                </tr>
                <tr><td colspan="3"><asp:Label ID="lblerr" runat="server" Font-Bold="true" CssClass="tbltxt"></asp:Label></td></tr>
                <tr>
                    <td class="tbltxt">
                    </td>
                    <td class="tbltxt">
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnsave" runat="server" OnClick="btnsave_Click" Text="Submit" meta:resourcekey="btnsaveResource1"
                            TabIndex="16" OnClientClick="return valShow();" />&nbsp;
                        <asp:Button ID="btncancel" runat="server" CausesValidation="False" OnClick="btncancel_Click"
                            Text="Cancel" meta:resourcekey="btncancelResource1" TabIndex="17" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
