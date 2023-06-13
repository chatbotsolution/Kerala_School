<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveStatus.aspx.cs" Inherits="HR_LeaveStatus" %>

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
        function clearText(btn) {
            switch (btn) {
                case '1':
                    {
                        document.getElementById("<%=txtStartDt.ClientID %>").value = "";
                        return false;
                    }
                case '2':
                    {
                        document.getElementById("<%=txtEndDt.ClientID %>").value = "";
                        return false;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Set Leave Status</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr id="trMsg" runat="server">
                    <td align="center" colspan="2">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="baseline">
                        <asp:Panel ID="pnlStatus" runat="server" Enabled="false">
                            <table width="100%" border="1px">
                                <tr>
                                    <td align="left" valign="baseline" width="250px">
                                        <b>Name :</b>&nbsp;<asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td align="center" valign="baseline" width="240px">
                                        Leave Start Date :&nbsp;<asp:TextBox ID="txtStartDt" runat="server" Width="80px"></asp:TextBox>
                                        <rjs:PopCalendar ID="dtpStartDt" runat="server" Control="txtStartDt" AutoPostBack="False"
                                            ShowMessageBox="True" TextMessage="Enter Leave Start date" Format="dd mmm yyyy">
                                        </rjs:PopCalendar>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                            OnClientClick="return clearText('1');" />
                                    </td>
                                    <td align="center" valign="baseline" width="240px">
                                        Leave End Date :&nbsp;<asp:TextBox ID="txtEndDt" runat="server" Width="80px"></asp:TextBox>
                                        <rjs:PopCalendar ID="dtpEndDt" runat="server" Control="txtEndDt" AutoPostBack="True"
                                            ShowMessageBox="True" TextMessage="Enter Leave End date" Format="dd mmm yyyy"
                                            OnSelectionChanged="dtpEndDt_SelectionChanged"></rjs:PopCalendar>
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                            OnClientClick="return clearText('2');" />
                                    </td>
                                    <td align="center" valign="baseline" width="120px">
                                        Days Availed :&nbsp;<asp:TextBox ID="txtDaysAvailed" runat="server" MaxLength="3"
                                            Width="40px" onkeypress="return blockNonNumbers(this, event, true, false);" onblur="if (this.value == '') {this.value = '0';}"
                                            onfocus="if(this.value == '0') {this.value = '';}" Text="0"></asp:TextBox>
                                    </td>
                                    <td align="center" valign="baseline" width="120px">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return CnfSubmit();"
                                            onfocus="active(this);" onblur="inactive(this);" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                            onfocus="active(this);" onblur="inactive(this);" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <div style="margin: 5px; padding: 5px; background-color: #E9FFDA; border: solid 1px #C0EDDA;
                font-weight: bold; text-align: left; color: #00514D; box-shadow: 3px 3px 5px #888888;">
                **NOTE:-&nbsp;Select a Leave from the Approved Leave List below to Set Status.</div>
            <fieldset>
                <legend style="color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana">
                    All Approved Leave</legend>
                <table width="100%">
                    <tr>
                        <td align="left" valign="baseline">
                            Set Status for&nbsp;<asp:DropDownList ID="drpStatusFor" runat="server" TabIndex="1"
                                AutoPostBack="true" OnSelectedIndexChanged="drpStatusFor_SelectedIndexChanged">
                                <asp:ListItem Text="Starting Leave" Value="S" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Reporting Back" Value="R"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="drpReporting" runat="server" TabIndex="2" Visible="False" AutoPostBack="True"
                                OnSelectedIndexChanged="drpReporting_SelectedIndexChanged">
                                <asp:ListItem Text="Today Reporting" Value="T" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="All Reporting" Value="A"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right" valign="baseline">
                            <asp:Label ID="lblRecCount" runat="server" Font-Bold="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grdLeave" runat="server" AutoGenerateColumns="False" Width="100%"
                                OnRowCommand="grdLeave_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="No">
                                        <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                        <ItemStyle HorizontalAlign="Left" Width="20px" />
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Employee Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpName" runat="server" Text='<%#Eval("EmpName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Leave Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeaveCode" runat="server" Text='<%#Eval("LeaveCode")%>'></asp:Label>
                                            (<%#Eval("LeaveDesc")%>)
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Applied Date">
                                        <ItemTemplate>
                                            <%#Eval("strLeaveAppliedDt")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Days Approved">
                                        <ItemTemplate>
                                            <%#Eval("DaysApproved")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From Date">
                                        <ItemTemplate>
                                            <%#Eval("LeaveFrom")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Date">
                                        <ItemTemplate>
                                            <%#Eval("LeaveTo")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reason">
                                        <ItemTemplate>
                                            <%#Eval("Reason")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="LeaveStartDt" HeaderText="Leave Start Date" DataFormatString="{0:dd-MMM-yyyy}">
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Set Status">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfStartDt" runat="server" Value='<%#Eval("LeaveStartDt")%>' />
                                            <asp:HiddenField ID="hfLeaveTo" runat="server" Value='<%#Eval("LeaveTo")%>' />
                                            <asp:HiddenField ID="hfEmpId" runat="server" Value='<%#Eval("EmpId")%>' />
                                            <asp:HiddenField ID="hfLeaveId" runat="server" Value='<%#Eval("LeaveId")%>' />
                                            <asp:HiddenField ID="hfAppliedDt" runat="server" Value='<%#Eval("LeaveAppliedDt")%>' />
                                            <asp:Button ID="btnSet" runat="server" Text="Set Status" CommandName="SetStatus"
                                                CommandArgument='<%#Container.DataItemIndex %>' onfocus="active(this);" onblur="inactive(this);" />
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("LeaveApplyId")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle Width="70px" HorizontalAlign="Center" />
                                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record(s)
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
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
            <asp:AsyncPostBackTrigger ControlID="dtpEndDt" EventName="SelectionChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


