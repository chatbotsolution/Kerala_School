<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveAuthorize.aspx.cs" Inherits="HR_LeaveAuthorize" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Authorize Leave
        </h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <fieldset style="width: 90%">
                <table width="100%">
                    <tr id="trMsg" runat="server">
                        <td colspan="2" style="height: 20px;" align="center">
                            <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Button ID="btnModAuth" runat="server" Text="Authorize Leave (Individual Employee)"
                                onfocus="active(this);" onblur="inactive(this);" OnClick="btnModAuth_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline" style="height: 18px">
                            <%-- Effective From Year&nbsp;:&nbsp;<asp:Label ID="lblYear" runat="server" Font-Bold="true"
                                ForeColor="Red"></asp:Label>
                            &nbsp;--%>Select a Designation<font color="red">*</font> :&nbsp;<asp:DropDownList
                                ID="drpDesignation" runat="server" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="drpDesignation_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblRecCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:GridView ID="grdLeave" runat="server" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Leave Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeaveCode" runat="server" Text='<%#Eval("LeaveCode")%>'></asp:Label>
                                            &nbsp;(<%#Eval("LeaveDesc")%>)
                                            <asp:HiddenField ID="hfLeaveId" runat="server" Value='<%#Eval("LeaveId")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Days Authorized">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTotDaysAuth" runat="server" MaxLength="3" Width="40px" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                TabIndex="2" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                Text='<%#Eval("TotDaysAuth")%>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Max. Days Allowed">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMaxDaysAllowed" runat="server" MaxLength="3" Width="40px" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                TabIndex="2" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                Text='<%#Eval("MaxDaysAllowed")%>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Encash Allowed">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtEncAllowed" runat="server" MaxLength="3" Width="40px" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                TabIndex="2" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                Text='<%#Eval("TotEncashAllowed")%>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Min. Attandance Required">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMinAttReq" runat="server" MaxLength="3" Width="40px" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                TabIndex="2" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                Text='<%#Eval("MinAttandReq")%>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Update">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfYear" runat="server" Value='<%#Eval("CalYear")%>' />
                                            <asp:HiddenField ID="hfDesgId" runat="server" Value='<%#Eval("DesignationId")%>' />
                                            <asp:Button ID="btnUpdate" runat="server" ToolTip="Click to Update" OnClick="btnUpdate_Click"
                                                Text="Update" TabIndex="2" onfocus="active(this);" onblur="inactive(this);" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="20px" />
                                        <HeaderStyle HorizontalAlign="center" Width="20px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record(s)
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnUpdateList" runat="server" Text="Update All" Visible="False" TabIndex="3"
                                OnClick="btnUpdateList_Click" onfocus="active(this);" onblur="inactive(this);" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

