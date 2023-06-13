<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="FineMaster.aspx.cs" Inherits="Library_FineMaster" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function isValid() {
            var Date = document.getElementById("<%=txtEffDate.ClientID %>").value;
            var FixA = document.getElementById("<%=txtFixedAmnt.ClientID %>").value;
            var DailyR = document.getElementById("<%=txtDailyRate.ClientID %>").value;
            var WeekR = document.getElementById("<%=txtWeeklyRate.ClientID %>").value;
            var FortnR = document.getElementById("<%=txtFortNRate.ClientID %>").value;
            var MlyR = document.getElementById("<%=txtMlyrate.ClientID %>").value;
            var YrR = document.getElementById("<%=txtYrlyrate.ClientID %>").value;

            if (Date.trim() == "") {
                alert("Please Select Effective Date !");
                document.getElementById("<%=txtEffDate.ClientID %>").focus();
                return false;
            }
            if (FixA.trim() == "" && document.getElementById("<%=txtFixedAmnt.ClientID %>").disabled == false) {
                alert("Please Enter Fixed Amount !");
                document.getElementById("<%=txtFixedAmnt.ClientID %>").focus();
                return false;
            }
            if (DailyR.trim() == "" && document.getElementById("<%=txtDailyRate.ClientID %>").disabled == false) {
                alert("Please Enter Daily Rate !");
                document.getElementById("<%=txtDailyRate.ClientID %>").focus();
                return false;
            }
            if (WeekR.trim() == "" && document.getElementById("<%=txtWeeklyRate.ClientID %>").disabled == false) {
                alert("Please Enter Weekly Rate !");
                document.getElementById("<%=txtWeeklyRate.ClientID %>").focus();
                return false;
            }
//            if (FortnR.trim() == "" && document.getElementById("<%=txtFortNRate.ClientID %>").disabled == false) {
//                alert("Please Enter Fortnightly Rate !");
//                document.getElementById("<%=txtFortNRate.ClientID %>").focus();
//                return false;
//            }
//            if (MlyR.trim() == "" && document.getElementById("<%=txtMlyrate.ClientID %>").disabled == false) {
//                alert("Please Enter Monthly Rate !");
//                document.getElementById("<%=txtMlyrate.ClientID %>").focus();
//                return false;
//            }
//            if (YrR.trim() == "" && document.getElementById("<%=txtYrlyrate.ClientID %>").disabled == false) {
//                alert("Please Enter Yearly Rate !");
//                document.getElementById("<%=txtYrlyrate.ClientID %>").focus();
//                return false;
//            }
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


    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Add/Modify Fine</h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="40" width="10" /></div>
    <div style="width: 400px; background-color: #666; padding: 1px; margin: 0 auto;">
        <div style="background-color: #FFF; padding: 10px;">
            <table cellpadding="3" cellspacing="0" align="center" width="100%" class="tbltxt">
                <tr>
                    <td style="width: 130px;">
                        Effective Date:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEffDate" runat="server" Width="100px" CssClass="smalltb"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpEffDt" runat="server" Control="txtEffDate" AutoPostBack="False"
                            Format="dd mmm yyyy"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fine Type :
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtnFixed" runat="server" Text="Fixed" GroupName="Ft" OnCheckedChanged="rbtnFixed_CheckedChanged"
                            AutoPostBack="True" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="rbtnVariable" runat="server" Text="Variable" GroupName="Ft"
                            OnCheckedChanged="rbtnFixed_CheckedChanged" AutoPostBack="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Fixed Amount :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFixedAmnt" runat="server" Width="200px" MaxLength="6" CssClass="smalltb"
                            onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Daily Rate :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDailyRate" runat="server" Width="200px" MaxLength="4" CssClass="smalltb"
                            onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Weekly Rate :
                    </td>
                    <td>
                        <asp:TextBox ID="txtWeeklyRate" runat="server" Width="200px" MaxLength="6" CssClass="smalltb"
                            onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fortnightly Rate :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFortNRate" runat="server" Width="200px" MaxLength="6" CssClass="smalltb"
                            onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Monthly Rate :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMlyrate" runat="server" Width="200px" MaxLength="6" CssClass="smalltb"
                            onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        YearlyRate :
                    </td>
                    <td>
                        <asp:TextBox ID="txtYrlyrate" runat="server" Width="200px" MaxLength="8" CssClass="smalltb"
                            onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Panel ID="pnl1" runat="server" Visible="true">
                            <asp:Button ID="btnSave" runat="server" Text="Save" Font-Bold="True" OnClientClick="return isValid(); "
                                Font-Size="8pt" Width="60px" OnClick="btnSave_Click" />&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Font-Bold="True" Font-Size="8pt"
                                Width="60px" OnClick="btnCancel_Click" />&nbsp;
                            <asp:Button ID="btnShow" runat="server" Text="Show List" Font-Bold="True" Font-Size="8pt"
                                Width="70px" OnClick="btnShow_Click" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <span style="color: Red; font-size: small;">[N.B. - All Fields are Mandatory ]</span>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
