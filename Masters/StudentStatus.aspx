<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="StudentStatus.aspx.cs" Inherits="Masters_StudentStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Student Status</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="3" width="100%" border="0">
                <tbody>
                    <tr>
                        <td style="width: 100%" valign="top" align="left" colspan="3">
                            <asp:Label ID="lblerr" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" colspan="3">
                            <asp:GridView ID="grdStudentStatus" runat="server" OnPageIndexChanging="grdStudentStatus_PageIndexChanging"
                                Width="498px" AutoGenerateColumns="False" OnRowCommand="grdStudentStatus_RowCommand"
                                PageSize="8" AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" 
                                AlternatingRowStyle-CssClass="alt" TabIndex="3">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="left" />
                                        <ItemTemplate>
                                            <input name="Checkb" type="checkbox" value='<%#Eval("StatusID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDesign" CausesValidation="false" Text='<%#Eval("Status")%>'
                                                CommandName="show" CommandArgument='<%#Eval("StatusID")%>' runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="400px" class="tbltxt">
                                <tr>
                                    <td valign="top" align="left">
                                        Description:
                                    </td>
                                    <td valign="top" align="left">
                                        <asp:TextBox ID="txtDescription" runat="server" Width="300px" 
                                            CssClass="tbltxtbox" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="*" ControlToValidate="txtDescription"
                                            Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="center" colspan="3">
                                        <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Submit" 
                                            Width="64px" TabIndex="2">
                                        </asp:Button>
                                        <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                            CausesValidation="False" TabIndex="4"></asp:Button>
                                        <input id="hdnsts" type="hidden" runat="server" />
                                        <asp:Button ID="btnDelet" runat="server" OnClick="btnDelet_Click" Text="Delet" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
