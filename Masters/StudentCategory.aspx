<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="StudentCategory.aspx.cs" Inherits="Masters_StudentCategory" %>

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
            Student Category</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="3" width="100%" border="0">
                <tbody>
                    <tr>
                        <td valign="top" align="left" colspan="3">
                            <asp:GridView ID="grdcategory" runat="server" OnPageIndexChanging="grdcategory_PageIndexChanging"
                                AutoGenerateColumns="False" Width="498px" OnRowCommand="grdcategory_RowCommand"
                                PageSize="8" AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" 
                                AlternatingRowStyle-CssClass="alt" TabIndex="3">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON"/>
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="15px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                        <ItemTemplate>
                                            <input name="Checkb" type="checkbox" value='<%#Eval("CatID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Student Category">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDesign" CausesValidation="false" Text='<%#Eval("CatName")%>'
                                                CommandName="show" CommandArgument='<%#Eval("CatID")%>' runat="server"></asp:LinkButton>
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
                            <table width="500px" class="tbltxt">
                                <tr>
                                    <td style="width: 100px;" valign="top" align="left">
                                        Category&nbsp;Description:
                                    </td>
                                    <td valign="top" align="left">
                                        <asp:TextBox ID="txtCategory" runat="server" Width="250px" CssClass="tbltxtbox" 
                                            TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCategory" runat="server" 
                                            Display="Dynamic" ControlToValidate="txtCategory" ErrorMessage="*">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="blanktd">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="center" colspan="2">
                                        <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Submit" 
                                            TabIndex="2"></asp:Button>
                                        <asp:Button ID="btnDelete" OnClick="btnDelete_Click" runat="server" Text="Delete"
                                            CausesValidation="False" TabIndex="4"></asp:Button>
                                        <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                            CausesValidation="False" TabIndex="5"></asp:Button>
                                        <input id="hdnsts" type="hidden" runat="server" />
                                    </td>
                                </tr>
                                <tr id="trMsg" runat="server">
                                    <td valign="top" align="center" colspan="2" class="tdMsg">
                                        <asp:Label ID="lblerr" runat="server" Font-Bold="true" ForeColor="White"></asp:Label>
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
