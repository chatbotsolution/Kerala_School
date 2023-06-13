<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="NewsMagazineMaster.aspx.cs" Inherits="Library_NewsMagazineMaster" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var MagazineName = document.getElementById("<%=txtMagName.ClientID %>").value;
            var MagazineCode = document.getElementById("<%=txtMagCode.ClientID %>").value;
            var SubscribeDt = document.getElementById("<%=txtSubscribeDt.ClientID %>").value;
            var ExpireDt = document.getElementById("<%=txtExpDt.ClientID %>").value;
            var Periodicity = document.getElementById("<%=ddlPeriodicity.ClientID %>").selectedIndex;
            var Amount = document.getElementById("<%=txtAmount.ClientID %>").value;

            if (MagazineCode.trim() == "") {
                alert("Please Fill Magazine Code !");
                document.getElementById("<%=txtMagCode.ClientID %>").focus();
                document.getElementById("<%=txtMagCode.ClientID %>").select();
                return false;
            }
            if (MagazineName.trim() == "") {
                alert("Please Fill Magazine Name !");
                document.getElementById("<%=txtMagName.ClientID %>").focus();
                document.getElementById("<%=txtMagName.ClientID %>").select();
                return false;
            }
            if (SubscribeDt.trim() == "") {
                alert("Please Fill Subscription Date !");
                document.getElementById("<%=txtSubscribeDt.ClientID %>").focus();
                document.getElementById("<%=txtSubscribeDt.ClientID %>").select();
                return false;
            }
            if (ExpireDt.trim() == "") {
                alert("Please Fill Subscription Expire Date !");
                document.getElementById("<%=txtExpDt.ClientID %>").focus();
                document.getElementById("<%=txtExpDt.ClientID %>").select();
                return false;
            }
            if (Date.parse(SubscribeDt.trim()) > Date.parse(ExpireDt.trim())) {
                alert("Expire Date Must be Greater than Subscription Date!");
                document.getElementById("<%=txtExpDt.ClientID %>").focus();
                return false;
            }
            if (Periodicity == 0) {
                alert("Please Select Periodicity !");
                document.getElementById("<%=ddlPeriodicity.ClientID %>").focus();
                return false;
            }
            if (Amount == "") {
                alert("Please Enter Price");
                document.getElementById("<%=txtAmount.ClientID %>").focus();
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
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Add/Modify NewsPaper/Magazine</h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="40" width="10" /></div>
    <div style="width: 470px; background-color: #666; padding: 1px; margin: 0 auto;">
        <div style="background-color: #FFF; padding: 10px;">
            <table cellpadding="0px" cellspacing="0px" align="center" width="100%" class="tbltxt">
                <tr>
                    <td>
                        NewsPaper/Magazine Code :&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtMagCode" runat="server" MaxLength="50" CssClass="largetb"></asp:TextBox>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        NewsPaper/Magazine Name :&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtMagName" runat="server" Width="200px" MaxLength="100" CssClass="largetb"></asp:TextBox>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Subscription Date :&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubscribeDt" runat="server" MaxLength="100" CssClass="largetb"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpSubscibeDt" runat="server" Control="txtSubscribeDt" AutoPostBack="False"
                            Format="dd mmm yyyy"></rjs:PopCalendar>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Subscription Expiry Date :&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtExpDt" runat="server" MaxLength="100" CssClass="largetb"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpExpireDt" runat="server" Control="txtExpDt" AutoPostBack="False"
                            Format="dd mmm yyyy"></rjs:PopCalendar>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Periodicity :&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPeriodicity" runat="server" CssClass="largetb">
                            <asp:ListItem Text="--Select--" Value="--Select--" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                            <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                            <asp:ListItem Text="ByMonthly" Value="ByMonthly"></asp:ListItem>
                            <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                            <asp:ListItem Text="HalfYearly" Value="HalfYearly"></asp:ListItem>
                            <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                        </asp:DropDownList>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Total Amount Paid :&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtAmount" runat="server" MaxLength="10" CssClass="largetb" onkeypress="return blockNonNumbers(this, event,true, false);"></asp:TextBox>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 260px" colspan="2" align="center">
                        <asp:Button ID="btnSaveAddNew" runat="server" Text="Save & AddNew" Font-Bold="True"
                            OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveAddNew_Click" />&nbsp;
                        <asp:Button ID="btnSaveGotoList" runat="server" Text="Save & GotoList" Font-Bold="True"
                            OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveGotoList_Click" />&nbsp;
                        <asp:Button ID="Button1" runat="server" Text="Clear" Font-Bold="True" Font-Size="8pt"
                            Width="60px" OnClick="btnCancel_Click" />&nbsp;
                        <asp:Button ID="Button2" runat="server" Text="Back" Font-Bold="True" Font-Size="8pt"
                            Width="70px" OnClick="btnShow_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="hfUserId" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
