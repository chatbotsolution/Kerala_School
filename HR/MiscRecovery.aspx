<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="MiscRecovery.aspx.cs" Inherits="HR_MiscRecovery" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function isValid() {
            var emp = document.getElementById("<%=drpEmpName.ClientID %>").value;
            var amt = document.getElementById("<%=txtAmount.ClientID %>").value;
            var reason = document.getElementById("<%=txtReason.ClientID %>").value;
            if (emp.trim() == "0") {
                alert("Select an Employee Name");
                document.getElementById("<%=drpEmpName.ClientID %>").focus();
                return false;
            }
            if (amt.trim() == "" || parseFloat(amt) == 0) {
                alert("Enter Amount to Recover.");
                document.getElementById("<%=txtAmount.ClientID %>").focus();
                return false;
            }
            if (reason.trim() == "") {
                alert("Enter a valid Reason of Recovery.");
                document.getElementById("<%=txtReason.ClientID %>").focus();
                return false;
            }
            else {
                return CnfSubmit();
            }
        }
        function CnfSubmit() {

            if (confirm("Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }
    </script>

    <script language="javascript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function beginRequest(sender, args) {
            // show the popup
            $find('<%=mdlloading.ClientID %>').show();

        }

        function endRequest(sender, args) {
            //  hide the popup
            $find('<%=mdlloading.ClientID %>').hide();

        }
        
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Miscellaneous Recovery from Employee (Against Extra Payment/Damage Recovery)</h2>
    </div>
    <div class="spacer">
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="90%" style="border: solid 1px">
                <tr id="Tr1" style="background-color: #D3E7EE;" runat="server">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;" valign="baseline"
                        colspan="2">
                        Guidelines:-
                        <ol type="1" style="color: Red">
                            <li>Select an Employee, from whom the amount to be recovered.</li>
                            <li>Select type of recovery i.e- recovery against Excess Payment or any Damage.</li>
                            <li>Enter amount to recover.</li>
                            <li>Enter a valid reason of recovery.</li>
                            <li>Select recovery mode i.e- from Salary or Direct Cash Payment</li>
                            <li>In case of Recovery from Salary, the recovered amount will be deducted from the
                                next month Salary of the Employee and it shouldn't be more than the Basic Salary
                                of the Employee.</li>
                            <li>In case of Direct Cash Payment, Payment Date is mandatory.</li>
                        </ol>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="baseline" width="150px">
                        Employee Name<font color="red">*</font>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:DropDownList ID="drpEmpName" runat="server" TabIndex="1" Width="350px"
                            AutoPostBack="True" OnSelectedIndexChanged="drpEmpName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="lblBasic" runat="server" Font-Bold="true" ForeColor="Blue"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        Recovery Type<font color="red">*</font>
                    </td>
                    <td align="left" valign="top">
                        :&nbsp;<asp:RadioButtonList ID="rbRecType" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" TabIndex="2">
                            <asp:ListItem Value="ES" Selected="True" Text="Excess Salary Payment"></asp:ListItem>
                            <asp:ListItem Value="DR" Text="Damage Recovery"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        Amount to Recover<font color="red">*</font>
                    </td>
                    <td align="left" valign="top">
                        :&nbsp;<asp:TextBox ID="txtAmount" runat="server" onblur="if (this.value == '') {this.value = '0';}"
                            onfocus="if(this.value == '0') {this.value = '';}" TabIndex="3" Text="0" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                        <asp:Label ID="lblPending" runat="server" Font-Bold="true" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        Recovery Reason<font color="red">*</font>
                    </td>
                    <td align="left" valign="top">
                        :&nbsp;<asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Height="50px"
                            Width="350px" TabIndex="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        Recovery Mode<font color="red">*</font>
                    </td>
                    <td align="left" valign="top">
                        :&nbsp;<asp:RadioButtonList ID="rbRecMode" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" TabIndex="5" AutoPostBack="True" OnSelectedIndexChanged="rbRecMode_SelectedIndexChanged">
                            <asp:ListItem Value="D" Selected="True" Text="Direct Cash Payment"></asp:ListItem>
                            <asp:ListItem Value="S" Text="Salary of Next Month"></asp:ListItem>
                        </asp:RadioButtonList>
                        &nbsp;&nbsp;||&nbsp;&nbsp;Payment Date &nbsp;:&nbsp;<asp:TextBox ID="txtPmtDate" runat="server"
                            Width="80px" TabIndex="5"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpPmtDate" runat="server" Format="dd mmm yyyy" Control="txtPmtDate" />
                    </td>
                </tr>
                <tr id="trMsg" runat="server">
                    <td colspan="2" style="height: 20px;" align="center">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td align="left">
                        &nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" onfocus="active(this);"
                            onblur="inactive(this);" OnClick="btnSave_Click" TabIndex="6" OnClientClick="return isValid();" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" onfocus="active(this);"
                            onblur="inactive(this);" OnClick="btnCancel_Click" TabIndex="7" />
                             <asp:Button ID="btnList" runat="server" Text="Go to List" Width="100px" onfocus="active(this);"
                            onblur="inactive(this);" TabIndex="8" onclick="btnList_Click" />
                        <asp:HiddenField ID="hfBasic" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <img src="../images/mask.gif" height="8" width="10" />
    <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
        PopupControlID="pnlloading" BackgroundCssClass="Background" />
    <asp:Panel ID="pnlloading" runat="server" Style="display: none">
        <div align="center" style="margin-top: 13px;">
            <img src="../images/loading.gif" />
            <span>Loading...</span>
        </div>
    </asp:Panel>
</asp:Content>

