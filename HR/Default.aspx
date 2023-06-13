<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="HR_Default" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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

    <script language="javascript" type="text/javascript">
        function CnfSubmit() {

            if (confirm("Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }
        function checkAdjLeave(balance, txtAdj) {
            var adj = document.getElementById(txtAdj);
            if (parseFloat(adj.value) > parseFloat(balance)) {
                document.getElementById(txtAdj).value = "";
                alert("Days Adjusted should not be more than the Balance Leave");
                document.getElementById(txtAdj).focus();
                return false;
            }
        }
        function isValid() {
            var sal = document.getElementById("<%=drpSalary.ClientID %>").selectedIndex;
            var appDate = document.getElementById("<%=txtAppDate.ClientID %>").value;
            if (sal == 0) {
                alert("Select Salary from the List");
                document.getElementById("<%=drpSalary.ClientID %>").focus();
                return false;
            }
            if (appDate.trim() == "") {
                alert("Enter Approved Date");
                document.getElementById("<%=txtAppDate.ClientID %>").focus();
                return false;
            }
            else {
                return CnfSubmit();
            }
        }
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=400,height=400,left = 300,top = 100');");
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Approve Claim Against</h2>
        <h3>
            <asp:RadioButtonList ID="rbClaim" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                AutoPostBack="True" OnSelectedIndexChanged="rbClaim_SelectedIndexChanged">
                <asp:ListItem Text="Excess Leave" Value="L" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Salary Withheld" Value="S"></asp:ListItem>
            </asp:RadioButtonList>
        </h3>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="mv1" runat="server" ActiveViewIndex="0">
                <asp:View ID="view1" runat="server">
                    <table width="100%">
                        <tr id="trMessage" runat="server">
                            <td align="center">
                                <asp:Label runat="server" ID="lblMessage" ForeColor="White" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline">
                                <asp:Label ID="lblRecords" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="baseline">
                                <asp:GridView ID="grdLeave" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                    EmptyDataText="No Record" Width="100%" PageSize="20" OnRowCommand="grdLeave_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="EmpName" HeaderText="Employee Name">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ClaimDate" HeaderText="Claim Date" DataFormatString="{0:dd-MMM-yyyy}">
                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Leave Type">
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                            <ItemTemplate>
                                                <%#Eval("LeaveCode")%>
                                                (<%#Eval("LeaveDesc")%>)</ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LeaveStartDt" HeaderText="Leave Start Date" DataFormatString="{0:dd-MMM-yyyy}">
                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LeaveEndDt" HeaderText="Leave End Date" DataFormatString="{0:dd-MMM-yyyy}">
                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ExtraDaysAvailed" HeaderText="Extra Days Availed">
                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WPL" HeaderText="WPL">
                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Reason" HeaderText="Reason">
                                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Related File">
                                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                            <ItemTemplate>
                                                <a href="../Up_Files/Claim_Doc/<%#Eval("RelatedFile")%>">Download</a></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approve/Reject">
                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                            <ItemTemplate>
                                                <asp:Button ID="btnApprove" runat="server" Text="Select" onfocus="active(this);"
                                                    onblur="inactive(this);" CommandName="Approve" CommandArgument='<%#Eval("ClaimId")%>' /></ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="view2" runat="server">
                    <table width="100%" align="center" style="border: 1px solid">
                        <tr>
                            <td align="center" width="50%">
                                <table width="100%" align="center" style="border: 1px solid">
                                    <tr>
                                        <td align="left" width="150px">
                                            Employee Name
                                        </td>
                                        <td align="left">
                                            :&nbsp;<asp:Label ID="lblName" runat="server" Width="300px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Claim Date
                                        </td>
                                        <td align="left">
                                            :&nbsp;<asp:Label ID="lblDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Leave Type
                                        </td>
                                        <td align="left">
                                            :&nbsp;<asp:Label ID="lblLeaveType" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Extra Days Availed <span style="color: #FF0000">(WPL)</span>
                                        </td>
                                        <td align="left">
                                            :&nbsp;<asp:Label ID="lblExtraDays" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Reason
                                        </td>
                                        <td align="left">
                                            :&nbsp;<asp:Label ID="lblReason" runat="server" Width="300px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="baseline">
                                            Approve/Reject
                                        </td>
                                        <td align="left" valign="baseline">
                                            :&nbsp;<asp:RadioButton ID="rbApproved" runat="server" Text="Approve" Checked="True"
                                                GroupName="claim" TabIndex="1" AutoPostBack="True" OnCheckedChanged="rbApproved_CheckedChanged" />
                                            <asp:RadioButton ID="rbReject" runat="server" Text="Reject" GroupName="claim" TabIndex="2"
                                                AutoPostBack="True" OnCheckedChanged="rbReject_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Days Approved
                                        </td>
                                        <td align="left">
                                            :&nbsp;<asp:TextBox ID="txtDaysApproved" runat="server" Width="100px" TabIndex="3"
                                                MaxLength="10" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="baseline">
                                            Aproved/Rejected Date<font color="red">*</font>
                                        </td>
                                        <td align="left" valign="baseline">
                                            :&nbsp;<asp:TextBox ID="txtDate" runat="server" Width="100px" TabIndex="4" MaxLength="10"></asp:TextBox>
                                            &nbsp;<span style="color: #FF0000">(DD-MM-YYYY)</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            Remarks<font color="red">*</font>
                                        </td>
                                        <td align="left" valign="top">
                                            :&nbsp;<asp:TextBox ID="txtRemarks" runat="server" Width="300px" TabIndex="5" Height="50px"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trMsg" runat="server">
                                        <td align="center" colspan="2">
                                            <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="baseline">
                                            &nbsp;
                                        </td>
                                        <td align="left" valign="baseline">
                                            &nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                                                OnClientClick="return CnfSubmit();" TabIndex="6" onfocus="active(this);" onblur="inactive(this);"
                                                Width="70px" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" TabIndex="7" onfocus="active(this);"
                                                onblur="inactive(this);" Width="70px" OnClick="btnCancel_Click" /><asp:HiddenField
                                                    ID="hfId" runat="server" Value="0" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="center" width="50%" valign="top">
                                <b>Available Leave for Adjustment</b>
                                <table width="100%" align="center" style="border: 1px solid">
                                    <tr>
                                        <td align="center" width="100%">
                                            <asp:GridView ID="grdAvlLeave" runat="server" AutoGenerateColumns="false" EmptyDataText="No Leave Available"
                                                DataKeyNames="LeaveId" Width="100%" OnRowDataBound="grdAvlLeave_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Leave Type">
                                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeaveCode" runat="server" Text='<%#Eval("LeaveCode") %>'></asp:Label>(<%#Eval("LeaveDesc") %>)
                                                            <asp:HiddenField ID="hfLeaveId" runat="server" Value='<%#Eval("LeaveId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance Leave">
                                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBalance" runat="server" Text='<%#Eval("BalanceLeave") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="No of Days Adjusted">
                                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAdjLeave" runat="server" Text="0" onblur="if (this.value == '') {this.value = '0';}"
                                                                onfocus="if(this.value == '0') {this.value = '';}" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                Width="80px" TabIndex="7"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="view3" runat="server">
                    <table width="50%" style="border: solid 1px">
                        <tr>
                            <td align="left" valign="baseline" width="100px">
                                Employee Name<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:DropDownList ID="drpEmp" runat="server" AutoPostBack="True" TabIndex="1"
                                    OnSelectedIndexChanged="drpEmp_SelectedIndexChanged" Width="300px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline" width="100px">
                                Withheld Salary<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:DropDownList ID="drpSalary" runat="server" TabIndex="2" Width="300px">
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
                                Approved Amount<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtAppAmt" runat="server" Width="100px" TabIndex="3" MaxLength="10"
                                    Text="0" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                    onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline">
                                Approved Date<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtAppDate" runat="server" Width="100px" TabIndex="4" MaxLength="10"></asp:TextBox>
                                &nbsp;<span style="color: #FF0000">(DD-MM-YYYY)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td align="left">
                                &nbsp;&nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" TabIndex="4"
                                    onfocus="active(this);" onblur="inactive(this);" Width="70px" Enabled="False"
                                    OnClick="btnSubmit_Click" OnClientClick="return isValid();" />
                                <asp:Button ID="btnReset" runat="server" Text="Cancel" TabIndex="5" onfocus="active(this);"
                                    onblur="inactive(this);" Width="70px" OnClick="btnReset_Click" />
                                <asp:HiddenField ID="hfLeaveApplyId" runat="server" Value="0" />
                                <asp:HiddenField ID="hfEmpId" runat="server" Value="0" />
                            </td>
                        </tr>
                        <tr id="trMsg1" runat="server">
                            <td align="center" colspan="2">
                                <asp:Label runat="server" ID="lblMsg1" ForeColor="White" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="drpEmp" />
        </Triggers>
    </asp:UpdatePanel>
    <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
        PopupControlID="pnlloading" BackgroundCssClass="Background" />
    <asp:Panel ID="pnlloading" runat="server" Style="display: none">
        <div align="center" style="margin-top: 13px;">
            <img src="../images/loading.gif" />
            <span>Loading...</span>
        </div>
    </asp:Panel>
</asp:Content>

