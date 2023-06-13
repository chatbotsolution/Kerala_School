<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="FeeComponents.aspx.cs" Inherits="Masters_FeeComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
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
                    if (e.checked)
                        flag = true;
                    else
                        flag = false;
                }
            }
            if (flag == true)
                return true;
            else {
                alert("Please select any record");
                return false;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fee Components</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="3" width="50%" border="0" class="tbltxt">
                <tbody>
                    <tr>
                        <td valign="top" align="center" colspan="2">
                            <asp:Label ID="lblerr" runat="server" ForeColor="Red">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Fee Description:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtfeedesc" runat="server" CssClass="tbltxtbox" Width="250px" TabIndex="1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvfeedesc" runat="server" ControlToValidate="txtfeedesc"
                                ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Periodicity:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drlPeriodicity" runat="server" TabIndex="2">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Fine Applicable:
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkFineApplicable" runat="server" TabIndex="3"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Concession Applicable:
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkConcessionApplicable" runat="server" TabIndex="4"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Refundable:
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkRefundable" runat="server" TabIndex="5" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Account Head:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpAccountHead" runat="server" TabIndex="6">
                            </asp:DropDownList>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpAccountHead"
                                ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Active Status:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpActiveSt" runat="server" TabIndex="6">
                                <asp:ListItem Value="1">True</asp:ListItem>
                                <asp:ListItem Value="0">False</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                            &nbsp;&nbsp;&nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="center" colspan="2">
                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Submit" Width="64px"
                                TabIndex="7"></asp:Button>&nbsp;
                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                CausesValidation="False" TabIndex="8"></asp:Button>&nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:DropDownList ID="drpPriority" runat="server" Visible="False">
            </asp:DropDownList>
            <input id="hdnsts" type="hidden" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
