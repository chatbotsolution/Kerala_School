<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveApply.aspx.cs" Inherits="HR_LeaveApply" %>

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
            Apply for Leave</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 100%; float: left;" align="left">
                <fieldset>
                    <table width="100%">
                        <tr>
                            <td align="right" valign="top" colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline" width="170px">
                                Employee Name<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:DropDownList ID="drpEmpName" runat="server" TabIndex="2" AutoPostBack="True"
                                    OnSelectedIndexChanged="drpEmpName_SelectedIndexChanged" Width="200px">
                                </asp:DropDownList>
                            </td>
                            <td align="left" valign="top" rowspan="12">
                                <asp:GridView ID="grdLeave" runat="server" AutoGenerateColumns="false" 
                                    EmptyDataText="No Leave Available" DataKeyNames="LeaveId" 
                                    onrowdatabound="grdLeave_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Button ID="btnSelect" runat="server" ToolTip="Click to Select" Text="Select"
                                                    onblur="inactive(this);" onfocus="active(this);" OnClick="btnSelect_Click" CommandArgument='<%#Eval("LeaveId") %>' />
                                                    <asp:HiddenField ID="hfBalance" runat="server" Value='<%#Eval("BalanceLeave") %>' />                                       
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Leave Type">
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                            <ItemTemplate>
                                                <%#Eval("LeaveCode") %>(<%#Eval("LeaveDesc") %>)                                        
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                        <asp:TemplateField HeaderText="Days Availed (Current Year)">
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAvlDays" runat="server" Text='<%#Eval("AvlDays") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                       
                                        <asp:TemplateField HeaderText="Balance (Previous Year + Current Year)">
                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" Text='<%#Eval("BalanceLeave") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="AvlDays" HeaderText="Days Availed (Current FY)">
                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>--%>
                                        <%--<asp:BoundField DataField="BalanceLeave" HeaderText="Balance (Previous FY + Current FY)">
                                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                        </asp:BoundField>--%>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline" width="100px">
                                Leave Type
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtLeave" runat="server" Enabled="False" Width="200px" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="100px" valign="baseline">
                                From Date<font color="red">*</font>&nbsp;
                            </td>
                            <td align="left" valign="baseline" colspan="2">
                                :&nbsp;<asp:TextBox ID="txtDate" runat="server" TabIndex="4" Width="100px"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpDate" runat="server" Control="txtDate" AutoPostBack="True"
                                    ShowMessageBox="True" TextMessage="Enter date" Format="dd mmm yyyy" 
                                    OnSelectionChanged="dtpDate_SelectionChanged" EnableTheming="False">
                                </rjs:PopCalendar>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="100px" valign="baseline">
                                Total Days<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtDaysApplied" runat="server" MaxLength="3" Width="40px"
                                    onkeypress="return blockNonNumbers(this, event, true, false);" TabIndex="5" onblur="if (this.value == '') {this.value = '0';}"
                                    onfocus="if(this.value == '0') {this.value = '';}" Text="0"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="100px" valign="baseline">
                                Authorized Leave in Current Year</td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtAuth" runat="server" Text="0" Enabled="false" Width="40px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="100px" valign="baseline">
                                Balance Leave&nbsp;(in
                                <asp:Label ID="lblYear" runat="server"></asp:Label>)&nbsp;
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtBalLeave" runat="server" Text="0" Enabled="false" Width="40px"></asp:TextBox>
                                 &nbsp;Applied but Pending for Approval&nbsp;:&nbsp;<asp:TextBox ID="txtApplied" runat="server"
                                    Text="0" Enabled="false" Width="40px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="100px" valign="baseline">
                                Max. Allowed
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtMaxDays" runat="server" Text="0" Enabled="false" Width="40px"></asp:TextBox>
                                &nbsp;Approved but not Availed&nbsp;:&nbsp;<asp:TextBox ID="txtNotAvailed" runat="server"
                                    Text="0" Enabled="false" Width="40px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                Reason<font color="red">*</font>
                            </td>
                            <td align="left" valign="top" colspan="2">
                                :&nbsp;<asp:TextBox ID="txtReason" runat="server" Width="400px" TabIndex="6" Height="50px"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trMsg" runat="server">
                            <td colspan="3" style="height: 20px;" align="center">
                                <asp:Label ID="lblMsg" Font-Bold="true" runat="server" Text=""></asp:Label>
                              
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="baseline">
                                &nbsp;
                            </td>
                            <td align="left" valign="baseline" colspan="2">
                                &nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                                    OnClientClick="return CnfSubmit();" TabIndex="7" Width="100px" onfocus="active(this);"
                                    onblur="inactive(this);" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" TabIndex="8"
                                    Width="100px" onfocus="active(this);" onblur="inactive(this);" />
                                    <asp:HiddenField ID="hfLeaveId" runat="server" />
                                    <asp:HiddenField ID="hfBalLeave" runat="server" Value="0" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="dtpDate" EventName="SelectionChanged" />
           <%-- <asp:AsyncPostBackTrigger ControlID="drpEmpName" EventName="SelectedIndexChanged" />--%>
            <asp:PostBackTrigger ControlID="btnSave"  />
            <asp:PostBackTrigger ControlID="drpEmpName"  />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

