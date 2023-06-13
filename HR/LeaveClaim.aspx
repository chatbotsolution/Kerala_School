<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveClaim.aspx.cs" Inherits="HR_LeaveClaim" %>

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
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Salary Claim Against Excess Leave</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="left" valign="top" width="400px">
                        <table width="100%" style="border: solid 1px">
                            <tr>
                                <td colspan="2" align="left">
                                    <span style="color: #FF0000"><b>Note:- Salary can be Claimed only for the WPL</b></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline" width="100px">
                                    Employee Name<font color="red">*</font>
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:DropDownList ID="drpEmpName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpEmpName_SelectedIndexChanged"
                                        TabIndex="1" Width="250px">
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
                                    Claim Date</td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtClaimDt" runat="server" Width="80px" TabIndex="3"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpClaimDt" runat="server" Format="dd mmm yyyy" Control="txtClaimDt" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    Days Approved
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtDaysApproved" runat="server" Width="40px" Text="0" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    Days Availed
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:TextBox ID="txtDaysAvailed" runat="server" Width="40px" Text="0" Enabled="false"></asp:TextBox>
                                    &nbsp;Extra Days :&nbsp;<asp:TextBox ID="txtExtraDays" runat="server" Width="40px"
                                        Text="0" Enabled="false"></asp:TextBox>
                                    <span style="color: #FF0000">WPL :&nbsp;</span><asp:TextBox ID="txtWPL" runat="server"
                                        Width="40px" Text="0" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    Reason<font color="red">*</font>
                                </td>
                                <td align="left" valign="top">
                                    :&nbsp;<asp:TextBox ID="txtReason" runat="server" Height="50px" TextMode="MultiLine"
                                        Width="250px" TabIndex="4"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="baseline">
                                    Related Documents<font color="red">*</font>
                                </td>
                                <td align="left" valign="baseline">
                                    :&nbsp;<asp:FileUpload ID="fuDoc" runat="server" TabIndex="5" onfocus="active(this);"
                                        onblur="inactive(this);" />
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
                                <td align="left" valign="baseline" colspan="2">
                                    &nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                                        OnClientClick="return CnfSubmit();" TabIndex="6" onfocus="active(this);" onblur="inactive(this);"
                                        Width="70px" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" TabIndex="7"
                                        onfocus="active(this);" onblur="inactive(this);" Width="70px" /><asp:HiddenField
                                            ID="hfId" runat="server" Value="0" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="center" valign="top" width="600px" style="border: solid 1px">
                        <b>Available Leave for Claim</b>
                        <asp:GridView ID="grdLeave" runat="server" AutoGenerateColumns="False" OnRowCommand="grdLeave_RowCommand"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Leave Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLeaveCode" runat="server" Text='<%#Eval("LeaveCode")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Reason" HeaderText="Reason">
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Days Approved">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApproved" runat="server" Text='<%#Eval("DaysApproved")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="LeaveStartDt" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Leave Start Date">
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LeaveEndDt" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Leave End Date">
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Days Availed">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAvailed" runat="server" Text='<%#Eval("DaysAvailed")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Claim">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfLeaveStartDt" runat="server" Value='<%#Eval("LeaveStartDt")%>' />
                                        <asp:HiddenField ID="hfLeaveEndDt" runat="server" Value='<%#Eval("LeaveEndDt")%>' />
                                        <asp:HiddenField ID="hfLeaveApplyId" runat="server" Value='<%#Eval("LeaveApplyId")%>' />
                                        <asp:Button ID="btnClaim" runat="server" CommandArgument='<%#Container.DataItemIndex %>'
                                            CommandName="Claim" onblur="inactive(this);" onfocus="active(this);" Text="Select" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Record(s)
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
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

