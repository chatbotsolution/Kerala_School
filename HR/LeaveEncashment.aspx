<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveEncashment.aspx.cs" Inherits="HR_LeaveEncashment" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function CnfSubmit() {

            if (confirm("Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }

        function valSave() {
            var emp = document.getElementById("<%=drpEmpName.ClientID %>").value;
            var leave = document.getElementById("<%=drpLeave.ClientID %>").value;
            var encash = document.getElementById("<%=txtEncash.ClientID %>").value;
            var appliedDt = document.getElementById("<%=txtAppliedDt.ClientID %>").value;
            var balance = document.getElementById("<%=txtLeaveAvail.ClientID %>").value;
            var maxEncash = document.getElementById("<%=txtMaxEncashment.ClientID %>").value;

            if (emp == "0") {
                alert("Select an Employee");
                document.getElementById("<%=drpEmpName.ClientID %>").focus();
                return false;
            }
            if (leave == "0") {
                alert("Select a Leave");
                document.getElementById("<%=drpLeave.ClientID %>").focus();
                return false;
            }
            if (encash.trim() == "" || parseFloat(encash) == 0) {
                alert("Enter Total Days to be Encashed.");
                document.getElementById("<%=txtEncash.ClientID %>").focus();
                return false;
            }
            if (appliedDt.trim() == "") {
                alert("Applied Date shouldn't be left blank.");
                document.getElementById("<%=txtAppliedDt.ClientID %>").focus();
                return false;
            }
            if (parseFloat(encash) > parseFloat(balance)) {
                alert("Days to be Encashed should not more than the Balance Days.");
                document.getElementById("<%=txtEncash.ClientID %>").focus();
                return false;
            }
            if (parseFloat(encash) > parseFloat(maxEncash)) {
                alert("Days to be Encashed should not more than the Max Encashment Allowed.");
                document.getElementById("<%=txtEncash.ClientID %>").focus();
                return false;
            }
            else {
                return CnfSubmit();
            }
        }
    </script>

    <%--<script language="javascript" type="text/javascript">

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
        
    </script>--%>
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Apply for Leave Encashment</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="width: 600px">
                <table width="100%">
                    <tr>
                        <td align="left" valign="baseline" width="100px">
                            Employee Name<font color="red">*</font>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:DropDownList ID="drpEmpName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpEmpName_SelectedIndexChanged"
                                TabIndex="1" Width="350px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Leave Type<font color="red">*</font>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:DropDownList ID="drpLeave" runat="server" TabIndex="3" AutoPostBack="True"
                                OnSelectedIndexChanged="drpLeave_SelectedIndexChanged" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Balance Leave
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtLeaveAvail" runat="server" Text="0" Enabled="False" MaxLength="3"
                                Width="50px"></asp:TextBox>
                            &nbsp;Pending Encashment&nbsp;:&nbsp;<asp:TextBox ID="txtPending" runat="server"
                                Text="0" Enabled="False" MaxLength="3" Width="50px"></asp:TextBox>
                            &nbsp;Max Encashment Allowed&nbsp;:&nbsp;<asp:TextBox ID="txtMaxEncashment" runat="server"
                                Text="0" Enabled="False" MaxLength="3" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            To be Encashed<font color="red">*</font>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtEncash" runat="server" Text="0" MaxLength="3" Width="50px"
                                onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                TabIndex="4"></asp:TextBox>
                            &nbsp;Total Encashed (in the service Period)&nbsp;:&nbsp;<asp:TextBox ID="txtTotEncashed"
                                runat="server" Text="0" Enabled="False" MaxLength="3" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Applied Date<font color="red">*</font>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtAppliedDt" runat="server" Width="80px" TabIndex="5"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpAppliedDt" runat="server" Format="dd mmm yyyy" Control="txtAppliedDt" />
                        </td>
                    </tr>
                    <tr id="trMsg" runat="server">
                        <td colspan="2" style="height: 20px;" align="center">
                            <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="baseline">
                            &nbsp;
                        </td>
                        <td align="left" valign="baseline">
                            &nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                                OnClientClick="return valSave();" TabIndex="7" Width="100px" onfocus="active(this);"
                                onblur="inactive(this);" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" TabIndex="8"
                                Width="100px" onfocus="active(this);" onblur="inactive(this);" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <%--<ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />--%>
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
