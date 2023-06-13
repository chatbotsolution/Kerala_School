<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveApproveList.aspx.cs" Inherits="HR_LeaveApproveList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<script language="javascript" type="text/javascript">
        function clearText(btn) {
            switch (btn) {
                case '1':
                    {
                        document.getElementById("<%=txtFromDt.ClientID %>").value = "";
                        return false;
                    }
                case '2':
                    {
                        document.getElementById("<%=txtToDt.ClientID %>").value = "";
                        return false;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    </script>--%>

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
            Pending Leave Approval</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="left" valign="baseline">
                        <%--From Date :&nbsp;<asp:TextBox ID="txtFromDt" runat="server" TabIndex="1" Width="100px"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpFromDt" runat="server" Control="txtFromDt" AutoPostBack="False"
                            ShowMessageBox="True" TextMessage="Enter From date" Format="dd mmm yyyy"></rjs:PopCalendar>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                            OnClientClick="return clearText('1');" TabIndex="2" />
                        &nbsp;To Date :&nbsp;<asp:TextBox ID="txtToDt" runat="server" TabIndex="3" Width="100px"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                            ShowMessageBox="True" TextMessage="Enter To date" Format="dd mmm yyyy"></rjs:PopCalendar>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                            OnClientClick="return clearText('2');" TabIndex="4" />--%>
                        <asp:RadioButtonList ID="rbAction" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                            Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="rbAction_SelectedIndexChanged">
                            <asp:ListItem Text="Approve Leave" Selected="True" Value="A"></asp:ListItem>
                            <asp:ListItem Text="Modify Leave" Value="M"></asp:ListItem>
                        </asp:RadioButtonList>
                        <%--&nbsp;<asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                            onfocus="active(this);" onblur="inactive(this);" TabIndex="5" />--%>
                    </td>
                    <td align="right" valign="baseline">
                        <asp:Label ID="lblRecCount" runat="server" Font-Bold="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grdLeave" runat="server" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("LeaveApplyId")%>' />
                                        <a href='LeaveApprove.aspx?action=<%#Eval("Action")%>&appId=<%#Eval("LeaveApplyId")%>'>
                                            <%#Eval("SetText")%></a>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee Name">
                                    <ItemTemplate>
                                        <%#Eval("EmpName")%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Desigantion">
                                    <ItemTemplate>
                                        <%#Eval("Designation")%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Leave Type">
                                    <ItemTemplate>
                                        <%#Eval("LeaveCode")%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Applied Date">
                                    <ItemTemplate>
                                        <%#Eval("LeaveAppliedDt")%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Days Applied">
                                    <ItemTemplate>
                                        <%#Eval("DaysApplied")%>
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
                            </Columns>
                            <EmptyDataTemplate>
                                No Record(s)
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
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

