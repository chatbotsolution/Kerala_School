<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveApprove.aspx.cs" Inherits="HR_LeaveApprove" %>

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
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Approve Leave</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 500px; float: left;" align="left">
                <fieldset>
                    <legend style="color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana">
                        Applied Leave Details</legend>
                    <table width="100%">
                        <tr>
                            <td align="left" valign="baseline" width="120px">
                                Employee Name
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:Label ID="lblEmpName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline" width="120px">
                                Leave Type
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:Label ID="lblLeaveType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="120px" valign="baseline">
                                Total Days Applied
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:Label ID="lblDaysApplied" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="120px" valign="baseline">
                                From Date
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:Label ID="lblFromDt" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="120px" valign="baseline">
                                To Date
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:Label ID="lblToDt" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="120px" valign="baseline">
                                Reason
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:Label ID="lblReason" runat="server" Width="300px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="120px" valign="baseline">
                                Status
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:Label ID="lblStatus" runat="server" Width="300px" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="120px" valign="baseline">
                                Approve/Reject
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:RadioButton ID="rbApproved" runat="server" Text="Approve" AutoPostBack="true"
                                    Checked="True" GroupName="leave" OnCheckedChanged="rbApproved_CheckedChanged" />
                                <asp:RadioButton ID="rbReject" runat="server" Text="Reject" AutoPostBack="true" GroupName="leave"
                                    OnCheckedChanged="rbReject_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="120px" valign="baseline">
                                Days Approved<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtDaysApproved" runat="server" MaxLength="3" Width="100px"
                                    onkeypress="return blockNonNumbers(this, event, true, false);" TabIndex="1" onblur="if (this.value == '') {this.value = '0';}"
                                    onfocus="if(this.value == '0') {this.value = '';}" Text="0"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline" width="120px">
                                Remarks<font color="red">*</font>
                            </td>
                            <td align="left" valign="top">
                                :&nbsp;<asp:TextBox ID="txtRemarks" runat="server" Width="250px" TabIndex="2" Height="50px"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline" colspan="2">
                                &nbsp;
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
                                &nbsp;
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return CnfSubmit();"
                                    TabIndex="3" Width="70px" onfocus="active(this);" onblur="inactive(this);" />
                                <asp:Button ID="btnList" runat="server" Text="Go to List" TabIndex="5" OnClick="btnList_Click"
                                    onfocus="active(this);" onblur="inactive(this);" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div style="width: 520px; float: right" align="left">
                <fieldset>
                    <legend style="color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana">
                        Employee Leave History (Current Year)</legend>
                    <asp:GridView ID="grdLeave" runat="server" AutoGenerateColumns="False" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="No">
                                <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                <ItemStyle HorizontalAlign="Left" Width="20px" />
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="LeaveStartDt" HeaderText="From Date" DataFormatString="{0:dd-MMM-yyyy}">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LeaveEndDt" HeaderText="To Date" DataFormatString="{0:dd-MMM-yyyy}">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DaysAvailed" HeaderText="Days Availed">
                                <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LeaveCode" HeaderText="Leave Type">
                                <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Reason" HeaderText="Reason">
                                <HeaderStyle HorizontalAlign="Left" Width="130px" />
                                <ItemStyle HorizontalAlign="Left" Width="130px" />
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Record(s)
                        </EmptyDataTemplate>
                    </asp:GridView>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

