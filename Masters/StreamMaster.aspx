<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="StreamMaster.aspx.cs" Inherits="Masters_StreamMaster" %>

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
        <img src="../images/icon_cp.jpg" width="29" height="29">
        </div>
    <div style="padding-top: 5px;">
        <h2>
            Student Stream</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="3" width="100%" border="0">
                <tbody>
                    <tr>
                        <td valign="top" align="center" colspan="3">
                            <asp:Label ID="lblerr" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 55%;">
                        <td valign="top" align="left" colspan="3">
                            <asp:GridView ID="grdStreamMaster" runat="server" OnPageIndexChanging="grdStreamMaster_PageIndexChanging"
                                AutoGenerateColumns="False" Width="498px" OnRowCommand="grdStreamMaster_RowCommand"
                                PageSize="15" AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                TabIndex="3">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="15px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                        <ItemTemplate>
                                            <input name="Checkb" type="checkbox" value='<%#Eval("StreamID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stream">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDesign" CausesValidation="false" CommandName="show" Text='<%#Eval("Description")%>'
                                                CommandArgument='<%#Eval("StreamID")%>' runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
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
                                Stream:
                            </td>
                            <td valign="top" align="left">
                                <asp:TextBox ID="txtStream" runat="server" Width="268px" CssClass="tbltxtbox" TabIndex="1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvStream" runat="server" Display="Dynamic" ControlToValidate="txtStream"
                                    ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" colspan="3">
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Submit" Width="64px"
                                    TabIndex="2"></asp:Button>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
