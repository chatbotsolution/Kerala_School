<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveCancel.aspx.cs" Inherits="HR_LeaveCancel" %>

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
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Cancel Leave</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <fieldset>
                <legend style="color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana">
                    All Applied Leave</legend>
                <table width="100%">
                    <tr id="trMsg" runat="server">
                        <td align="center" colspan="2">
                            <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            From Date :&nbsp;<asp:TextBox ID="txtFromDt" runat="server" TabIndex="4" Width="100px"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpFromDt" runat="server" Control="txtFromDt" AutoPostBack="False"
                                ShowMessageBox="True" TextMessage="Enter From date" Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                OnClientClick="return clearText('1');" />
                            &nbsp;To Date :&nbsp;<asp:TextBox ID="txtToDt" runat="server" TabIndex="4" Width="100px"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                                ShowMessageBox="True" TextMessage="Enter To date" Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                OnClientClick="return clearText('2');" />
                            &nbsp;<asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                onfocus="active(this);" onblur="inactive(this);" />
                        </td>
                        <td align="right" valign="baseline">
                            <asp:Label ID="lblRecCount" runat="server" Font-Bold="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grdLeave" runat="server" AutoGenerateColumns="False" Width="100%">
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
                                            <%#Eval("LeaveCode")%>(<%#Eval("LeaveDesc")%>)
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Applied Date">
                                        <ItemTemplate>
                                            <%#Eval("LeaveAppliedDt")%>
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
                                    <asp:TemplateField HeaderText="Cancel Leave">
                                        <ItemTemplate>
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" onfocus="active(this);" onblur="inactive(this);"  />
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("LeaveApplyId")%>' />
                                            <asp:Panel ID="pnlCancel" runat="server" Style="display: none">
                                                <table align="center" style="background-color: #CFE9FF; border: 1px solid #135e8a;
                                                    padding: 20px;">
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                            <h4 style="color: Blue">
                                                                You are going to Cancel this Leave. Do you want to continue ?</h4>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="baseline">
                                                            Employee Name
                                                        </td>
                                                        <td align="left" valign="top">
                                                            :&nbsp;<%#Eval("EmpName")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="baseline">
                                                            Applied Date
                                                        </td>
                                                        <td align="left" valign="top">
                                                            :&nbsp;<%#Eval("LeaveAppliedDt")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="baseline">
                                                            Leave Type
                                                        </td>
                                                        <td align="left" valign="top">
                                                            :&nbsp;<%#Eval("LeaveCode")%>(<%#Eval("LeaveDesc")%>)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="baseline">
                                                            Days Applied
                                                        </td>
                                                        <td align="left" valign="top">
                                                            :&nbsp;<%#Eval("DaysApplied")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="baseline">
                                                            Reason of Cancel&nbsp;(If Yes)<font color="red" size="bold">*</font>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            :&nbsp;<asp:TextBox ID="txtReason" runat="server" Width="200px" Height="40px" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                            <asp:Button ID="btnYes" runat="server" Text="Yes" Width="60px" OnClick="btnYes_Click"
                                                                onfocus="active(this);" onblur="inactive(this);" />
                                                            <asp:Button ID="btnNo" runat="server" Text="No" Width="60px" onfocus="active(this);"
                                                                onblur="inactive(this);" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <ajaxToolkit:ModalPopupExtender ID="mdlPopUp" runat="server" BackgroundCssClass="modalBackground"
                                                PopupControlID="pnlCancel" DropShadow="false" TargetControlID="btnCancel" CancelControlID="btnNo" />
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
    </asp:UpdatePanel>
</asp:Content>

