<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="StudentReligion.aspx.cs" Inherits="Masters_StudentReligion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
        function ToggleAll(e) {
            if (e.checked) {
                CheckAll();
            }
            else {
                ClearAll();
            }
        }

        function CheckAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = true;
                }
            }
            ml.toggleAll.checked = true;
        }

        function ClearAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = false;
                }
            }
            ml.toggleAll.checked = false;
        }
   
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Student Religion</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="height: 365px" cellspacing="0" cellpadding="3" width="100%" border="0">
                <tbody>
                    <tr>
                        <td style="width: 100%" valign="top" align="center">
                            <table style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td style="width: 100%" valign="top" align="center" colspan="3">
                                            <asp:Label ID="lblerr" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" colspan="3">
                                            <asp:GridView ID="grdReligion" runat="server" AllowPaging="true" OnRowCommand="grdReligion_RowCommand"
                                                Width="498px" AutoGenerateColumns="False" OnPageIndexChanging="grdReligion_PageIndexChanging"
                                                PageSize="8" CssClass="mGrid" PagerStyle-CssClass="pgr" 
                                                AlternatingRowStyle-CssClass="alt" TabIndex="3">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                        <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                        <ItemTemplate>
                                                            <input name="Checkb" type="checkbox" value='<%#Eval("ReligionId")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="15px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Religion">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkReligion" CausesValidation="false" Text='<%#Eval("Religion")%>'
                                                                CommandName="show" CommandArgument='<%#Eval("ReligionId")%>' runat="server"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
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
                                            <table width="500px" class="tbltxt">
                                                <tr>
                                                    <td style="width: 100px;" valign="top" align="left">
                                                        Religion&nbsp;Description:
                                                    </td>
                                                    <td valign="top" align="left" colspan="2">
                                                        <asp:TextBox ID="txtReligion" runat="server" Width="250px" CssClass="tbltxtbox" 
                                                            TabIndex="1"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvReligion" runat="server" ErrorMessage="*" ControlToValidate="txtReligion"
                                                            Display="Dynamic">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center" colspan="3">
                                                        <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Submit" 
                                                            Width="64px" TabIndex="2">
                                                        </asp:Button>
                                                        <asp:Button ID="btnDelete" OnClick="btnDelete_Click" runat="server" Text="Delete"
                                                            CausesValidation="False" TabIndex="4"></asp:Button>
                                                        <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                                            CausesValidation="False" TabIndex="5"></asp:Button>
                                                        <input id="hdnsts" type="hidden" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

