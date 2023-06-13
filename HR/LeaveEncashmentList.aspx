<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveEncashmentList.aspx.cs" Inherits="HR_LeaveEncashmentList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function CnfSubmit() {

            if (confirm("Do you want to continue ?\nNote :- Please verify the details before Save.")) {

                return true;
            }
            else {

                return false;
            }
        }
        function CnfDelete() {

            if (confirm("Are you sure to Delete the Record ?")) {

                return true;
            }
            else {

                return false;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Manage Leave Encashment</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="mv1" runat="server" ActiveViewIndex="0">
                <asp:View ID="view1" runat="server">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                Encashment Status
                                <asp:DropDownList ID="drpStatus" runat="server" TabIndex="1" AutoPostBack="True"
                                    OnSelectedIndexChanged="drpStatus_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="Pending Approval" Value="P"></asp:ListItem>
                                    <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Encashed" Value="E"></asp:ListItem>
                                    <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Button ID="btnNew" runat="server" Text="New Leave Encashment" onfocus="active(this);"
                                    onblur="inactive(this);" onclick="btnNew_Click" TabIndex="2" />
                            </td>
                        </tr>
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
                                <asp:GridView ID="grdLeave" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record"
                                    Width="100%" OnRowCommand="grdLeave_RowCommand" AllowPaging="True" OnPageIndexChanging="grdLeave_PageIndexChanging"
                                    PageSize="25">
                                    <PagerSettings PageButtonCount="20" />
                                    <Columns>
                                        <asp:BoundField DataField="EmpName" HeaderText="Employee Name">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AppliedDate" HeaderText="Applied Date" DataFormatString="{0:dd-MMM-yyyy}">
                                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Leave Type">
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                            <ItemTemplate>
                                                <%#Eval("LeaveCode")%>
                                                (<%#Eval("LeaveDesc")%>)</ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AppliedDays" HeaderText="Applied Days">
                                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AppRejDate" HeaderText="Approved Date" DataFormatString="{0:dd-MMM-yyyy}">
                                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ApprovedDays" HeaderText="Approved Days">
                                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PaidAmount" HeaderText="Amount" DataFormatString="{0:F2}">
                                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Payable In">
                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                                            <ItemTemplate>
                                                <%#Eval("PaidMonth")%>-<%#Eval("PaidYear")%></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PaidDate" HeaderText="Paid Date">
                                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Approve">
                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                            <ItemTemplate>
                                                <asp:Button ID="btnApprove" runat="server" Text="Approve" onfocus="active(this);"
                                                    onblur="inactive(this);" CommandName="Approve" CommandArgument='<%#Eval("Id")%>' /></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reject">
                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                            <ItemTemplate>
                                                <asp:Button ID="btnReject" runat="server" Text="Reject" onfocus="active(this);" onblur="inactive(this);"
                                                    CommandName="Reject" CommandArgument='<%#Eval("Id")%>' /></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                            <ItemTemplate>
                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" onfocus="active(this);" onblur="inactive(this);"
                                                    CommandName="Remove" CommandArgument='<%#Eval("Id")%>' OnClientClick="return CnfDelete();" /></ItemTemplate>
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
                                Applied Date
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
                                Applied Days
                            </td>
                            <td align="left">
                                :&nbsp;<asp:Label ID="lblAppliedDays" runat="server"></asp:Label>
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
                                :&nbsp;<asp:RadioButtonList ID="rbAppRej" runat="server" TabIndex="1" AutoPostBack="True"
                                    BorderStyle="Solid" BorderWidth="1px" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                    OnSelectedIndexChanged="rbAppRej_SelectedIndexChanged" Enabled="False">
                                    <asp:ListItem Text="Approve" Value="A" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Reject" Value="R"></asp:ListItem>
                                </asp:RadioButtonList>
                                &nbsp;<b>||</b>&nbsp;Aproved/Rejected Date<font color="red">*</font> &nbsp;:&nbsp;<asp:TextBox
                                    ID="txtDate" runat="server" Width="80px" TabIndex="2" MaxLength="10" AutoPostBack="True"
                                    OnTextChanged="txtDate_TextChanged"></asp:TextBox>
                                &nbsp;<span style="color: #FF0000">(DD-MM-YYYY)</span>
                            </td>
                        </tr>
                        <tr id="tr1" runat="server">
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr2" runat="server">
                            <td align="left">
                                Days Approved
                            </td>
                            <td align="left">
                                :&nbsp;<asp:TextBox ID="txtDaysApproved" runat="server" Width="50px" TabIndex="3"
                                    MaxLength="5" onkeypress="return blockNonNumbers(this, event, true, false);"
                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                    Text="0" OnTextChanged="txtDaysApproved_TextChanged" AutoPostBack="True"></asp:TextBox>
                                &nbsp;<b>||</b>&nbsp;Current Salary&nbsp;:&nbsp;<asp:TextBox ID="txtSalary" runat="server"
                                    Width="100px" TabIndex="4" Enabled="false" Text="0"></asp:TextBox>
                                &nbsp;<b>||</b>&nbsp;Payable Encashed Amount&nbsp;:&nbsp;<asp:TextBox ID="txtPayableAmt"
                                    runat="server" Width="100px" TabIndex="5" Text="0" onkeypress="return blockNonNumbers(this, event, true, false);"
                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"></asp:TextBox>
                                &nbsp;<b>||</b>&nbsp;<asp:DropDownList ID="drpPmtMode" runat="server" TabIndex="6"
                                    AutoPostBack="true" OnSelectedIndexChanged="drpPmtMode_SelectedIndexChanged">
                                    <asp:ListItem Text="Pay in Next Salary" Value="S" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Pay Instantly" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="tr3" runat="server">
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr4" runat="server">
                            <td align="left" valign="baseline">
                                Payment Mode
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:RadioButtonList ID="rbPmtMode" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" TabIndex="7" OnSelectedIndexChanged="rbPmtMode_SelectedIndexChanged"
                                    AutoPostBack="true" BorderStyle="Solid" BorderWidth="1px">
                                    <asp:ListItem Text="Cash" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Bank" Value="B" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                                &nbsp;<b>||</b> Payment Date :&nbsp;<asp:TextBox ID="txtPaymentDate" runat="server"
                                    Width="80px" TabIndex="8"></asp:TextBox>
                                &nbsp;<span style="color: #FF0000">(DD-MM-YYYY)</span> &nbsp;<b>||</b> Bank Name&nbsp;:
                                <asp:DropDownList ID="drpBank" runat="server" TabIndex="9">
                                </asp:DropDownList>
                                &nbsp;<b>||</b> Instrument No&nbsp;:&nbsp;<asp:TextBox ID="txtInstrNo" TabIndex="10"
                                    runat="server" Width="100px"></asp:TextBox>
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
                                :&nbsp;<asp:TextBox ID="txtRemarks" runat="server" Width="300px" TabIndex="11" Height="50px"
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
                                    OnClientClick="return CnfSubmit();" TabIndex="12" onfocus="active(this);" onblur="inactive(this);"
                                    Width="70px" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" TabIndex="13" onfocus="active(this);"
                                    onblur="inactive(this);" Width="70px" OnClick="btnCancel_Click" /><asp:HiddenField
                                        ID="hfId" runat="server" Value="0" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

