<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="feenoms.aspx.cs" Inherits="Masters_feenoms" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
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

        function checkSession() {
            var txt = document.getElementById("<%=txtsession.ClientID%>").value;
            var str1 = txt.substr(0, 4);
            var str2 = txt.charAt(4);
            var str3 = txt.substr(5, 2);
            if (txt.length < 7) {
                alert("Please Enter Full Session Year");
                return false;
            }
            if (isNaN(str1) || str2 != '-' || isNaN(str3)) {
                alert("Please Enter Valid Session Year");
                return false;
            }
            else {
                return true;
            }

        }

        function getduedates(txt) {
            start_date = txt.value.split("-")
            year = start_date[2];
            day = start_date[1];
            month = start_date[0];
            var mydate = new Date(year, month, day);
            var duedate1 = setdate(year, month, day);
            start_date = duedate1.split("-")
            var duedate2 = setdate(start_date[2], start_date[0], day);
            start_date = duedate2.split("-");
            var duedate3 = setdate(start_date[2], start_date[0], day);
            start_date = duedate3.split("-");
            var duedate4 = setdate(start_date[2], start_date[0], day);
            start_date = duedate4.split("-");
            var duedate5 = setdate(start_date[2], start_date[0], day);
            start_date = duedate5.split("-");
            var duedate6 = setdate(start_date[2], start_date[0], day);
            start_date = duedate6.split("-");
            var duedate7 = setdate(start_date[2], start_date[0], day);
            start_date = duedate7.split("-");
            var duedate8 = setdate(start_date[2], start_date[0], day);
            start_date = duedate8.split("-");
            var duedate9 = setdate(start_date[2], start_date[0], day);
            start_date = duedate9.split("-");
            var duedate10 = setdate(start_date[2], start_date[0], day);
            start_date = duedate10.split("-");
            var duedate11 = setdate(start_date[2], start_date[0], day);

            document.getElementById('<%=txtduedate1.ClientID%>').value = duedate1
            document.getElementById('<%=txtduedate2.ClientID%>').value = duedate2
            document.getElementById('<%=txtduedate3.ClientID%>').value = duedate3
            document.getElementById('<%=txtduedate4.ClientID%>').value = duedate4
            document.getElementById('<%=txtduedate5.ClientID%>').value = duedate5
            document.getElementById('<%=txtduedate6.ClientID%>').value = duedate6
            document.getElementById('<%=txtduedate7.ClientID%>').value = duedate7
            document.getElementById('<%=txtduedate8.ClientID%>').value = duedate8
            document.getElementById('<%=txtduedate9.ClientID%>').value = duedate9
            document.getElementById('<%=txtduedate10.ClientID%>').value = duedate10
            document.getElementById('<%=txtduedate11.ClientID%>').value = duedate11

        }

        function setdate(year, month, day) {
            month = parseInt(month) + 2;
            if (parseInt(month) > 12) {
                month = month - 12; year = parseInt(year) + 1;
            }
            if (day > 30 && month == 9 || month == 11 || month == 5 || month == 4) {
                day = 30;
                month = parseInt(month) + 1
            }
            else if (day > 28 && month == 2) {
                day = 28;
                month = parseInt(month) + 1
            }
            if (parseInt(month) > 12) {
                month = month - 12; year = parseInt(year) + 1;
            }
            var date = month + "-" + day + "-" + year;
            return date;
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fee Norms</h2>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="border-right: #000000 1px solid; padding-right: 1px; border-top: #000000 1px solid;
                padding-left: 1px; padding-bottom: 1px; border-left: #000000 1px solid; padding-top: 1px;
                border-bottom: #000000 1px solid; border-collapse: collapse" cellspacing="1"
                cellpadding="3" width="50%" align="left" border="0" class="tbltxt">
                <tbody>
                    <tr>
                        <td align="right" style="width: 32%" valign="top">
                            Fee Collection Period<span class="error">*</span>:
                        </td>
                        <td align="left" valign="top" width="50%">
                            <asp:DropDownList ID="drpFeeCollPeriod" runat="server" OnSelectedIndexChanged="drpFeeCollPeriod_SelectedIndexChanged"
                                Width="154px" AutoPostBack="True" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 32%" valign="top" align="right">
                            Session<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%">
                            <asp:TextBox ID="txtsession" AutoPostBack="True" runat="server" TabIndex="2" MaxLength="7"
                                Width="60px" ></asp:TextBox>
                            <span class="error"><b>Ex : YYYY-YY</b></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 32%; height: 6px;" valign="top" align="right">
                            Start Date<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%" style="height: 6px">
                            <asp:TextBox ID="txtdate" runat="server" AutoPostBack="True" CausesValidation="true"
                                OnTextChanged="txtdate_TextChanged" ReadOnly="True" TabIndex="3"></asp:TextBox><rjs:PopCalendar
                                    ID="PopCalendarSt" runat="server" Control="txtdate" AutoPostBack="True" OnSelectionChanged="PopCalendarSt_SelectionChanged">
                                </rjs:PopCalendar>
                            <asp:Label ID="lblDay" runat="server" Text="Label" Visible="False"></asp:Label>
                            <asp:Label ID="lblMonth" runat="server" Text="Label" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="RowDt1">
                        <td style="width: 32%; height: 9px;" valign="top" align="right">
                            Due Date1<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%" style="height: 9px">
                            <asp:TextBox ID="txtduedate1" runat="server" TabIndex="4"></asp:TextBox><rjs:PopCalendar
                                ID="Popcalendar1" runat="server" Control="txtduedate1" />
                        </td>
                    </tr>
                    <tr runat="server" id="RowDt2">
                        <td style="width: 32%" valign="top" align="right">
                            Due Date2<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%">
                            <asp:TextBox ID="txtduedate2" runat="server" TabIndex="5"></asp:TextBox><rjs:PopCalendar
                                ID="Popcalendar2" runat="server" Control="txtduedate2" />
                        </td>
                    </tr>
                    <tr runat="server" id="RowDt3">
                        <td style="width: 32%; height: 5px;" valign="top" align="right">
                            Due Date3<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%" style="height: 5px">
                            <asp:TextBox ID="txtduedate3" runat="server" TabIndex="6"></asp:TextBox><rjs:PopCalendar
                                ID="Popcalendar3" runat="server" Control="txtduedate3" />
                        </td>
                    </tr>
                    <tr runat="server" id="RowDt4">
                        <td style="width: 32%; height: 7px;" valign="top" align="right">
                            Due Date4<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%" style="height: 7px">
                            <asp:TextBox ID="txtduedate4" runat="server" TabIndex="7"></asp:TextBox><rjs:PopCalendar
                                ID="Popcalendar4" runat="server" Control="txtduedate4" />
                        </td>
                    </tr>
                    <tr runat="server" id="RowDt5">
                        <td style="width: 32%; height: 12px;" valign="top" align="right">
                            Due Date5<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%" style="height: 12px">
                            <asp:TextBox ID="txtduedate5" runat="server" TabIndex="8"></asp:TextBox><rjs:PopCalendar
                                ID="Popcalendar5" runat="server" Control="txtduedate5" />
                        </td>
                    </tr>
                    <tr runat="server" id="RowDt6">
                        <td style="width: 32%; height: 12px;" valign="top" align="right">
                            Due Date6<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%" style="height: 12px">
                            <asp:TextBox ID="txtduedate6" runat="server" TabIndex="9"></asp:TextBox><rjs:PopCalendar
                                ID="Popcalendar6" runat="server" Control="txtduedate6" />
                        </td>
                    </tr>
                    <tr runat="server" id="RowDt7">
                        <td style="width: 32%; height: 12px;" valign="top" align="right">
                            Due Date7<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="100%" style="height: 12px">
                            <asp:TextBox ID="txtduedate7" runat="server" TabIndex="10"></asp:TextBox><rjs:PopCalendar
                                ID="Popcalendar7" runat="server" Control="txtduedate7" />
                        </td>
                    </tr>
                    <tr runat="server" id="RowDt8">
                        <td style="width: 32%; height: 16px;" valign="top" align="right">
                            Due Date8<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%" style="height: 16px">
                            <asp:TextBox ID="txtduedate8" runat="server" TabIndex="11"></asp:TextBox><rjs:PopCalendar
                                ID="Popcalendar8" runat="server" Control="txtduedate8" />
                        </td>
                    </tr>
                    <tr runat="server" id="RowDt9">
                        <td style="width: 32%; height: 12px;" valign="top" align="right">
                            Due Date9<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%" style="height: 12px">
                            <asp:TextBox ID="txtduedate9" runat="server" TabIndex="12"></asp:TextBox><rjs:PopCalendar
                                ID="Popcalendar9" runat="server" Control="txtduedate9" />
                        </td>
                    </tr>
                    <tr runat="server" id="RowDt10">
                        <td style="width: 32%; height: 12px;" valign="top" align="right">
                            Due Date10<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%" style="height: 12px">
                            <asp:TextBox ID="txtduedate10" runat="server" TabIndex="13"></asp:TextBox><rjs:PopCalendar
                                ID="Popcalendar10" runat="server" Control="txtduedate10" />
                        </td>
                    </tr>
                    <tr runat="server" id="RowDt11">
                        <td style="width: 32%; height: 12px;" valign="top" align="right">
                            Due Date11<span class="error">*</span>:
                        </td>
                        <td valign="top" align="left" width="50%" style="height: 12px">
                            <asp:TextBox ID="txtduedate11" runat="server" TabIndex="14"></asp:TextBox><rjs:PopCalendar
                                ID="Popcalendar11" runat="server" Control="txtduedate11" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 32%; height: 4px" valign="top">
                            Fine Amount<span class="error">*</span>:
                        </td>
                        <td align="left" style="height: 4px" valign="top" width="50%">
                            <asp:TextBox ID="txtfineamt" runat="server" TabIndex="15" onkeypress="return blockNonNumbers(this, event, true, true);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 32%; height: 4px" valign="top">
                            Fine Due Period<span class="error">*</span>:
                        </td>
                        <td align="left" class="DefaultFont10" style="height: 4px" valign="top" width="50%">
                            <asp:TextBox ID="txtFineDays" runat="server" Width="72px" TabIndex="16" onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
                            Days
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 32%; height: 4px" valign="top">
                            Finalized:
                        </td>
                        <td align="left" style="height: 4px" valign="top" width="50%">
                            <asp:DropDownList ID="drpFinal" runat="server" TabIndex="17">
                                <asp:ListItem>No</asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 16px" valign="top" align="center" colspan="2">
                            <asp:Label ID="lblMsg" Font-Bold="true" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 16px" valign="top" colspan="2">
                            <div align="center">
                                <font face="Verdana">
                                    <asp:Button ID="btnSave" runat="server" Text="Submit" Width="64px" OnClick="btnSave_Click"
                                        TabIndex="18" OnClientClick="return checkSession();"></asp:Button>
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                                        OnClick="btnCancel_Click" TabIndex="19"></asp:Button>
                                    &nbsp; &nbsp; &nbsp; &nbsp; </font>
                            </div>
                        </td>
                        <input id="hdnsts" type="hidden" runat="server" />
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
