<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="DefineROL.aspx.cs" Inherits="Accounts_DefineROL" %>

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
        function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
            var key;
            var isCtrl = false;
            var keychar;
            var reg;

            if (window.event) {
                key = e.keyCode;
                isCtrl = window.event.ctrlKey
            }
            else if (e.which) {
                key = e.which;
                isCtrl = e.ctrlKey;
            }

            if (isNaN(key)) return true;

            keychar = String.fromCharCode(key);

            // check for backspace or delete, or if Ctrl was pressed
            if (key == 8 || isCtrl) {
                return true;
            }

            reg = /\d/;
            var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
            var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

            return isFirstN || isFirstD || reg.test(keychar);
        }
       
    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div>
                            <h1>
                                Define
                            </h1>
                            <h2>
                                ROL</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px;" colspan="2">
                        Category :
                        <asp:DropDownList ID="drpCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                        Item Name :<asp:DropDownList ID="drpItem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpItem_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" Text="Show" 
                            Width="120px" />
                        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" 
                            Text="Set Rol" Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="width: 45%; float: right; padding: 5px;">
                            <asp:Label ID="lblRecCount" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grdRolItem" runat="server" DataKeyNames="ItemCode" ToolTip="Item ROL List"
                            Width="70%" AllowPaging="false" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Item Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemName" runat="server" Text='<% #Bind("ItemName")%>'></asp:Label>
                                        <asp:HiddenField ID="hfItemCode" runat="server" Value='<%#Eval("ItemCode") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Existing ROL">
                                    <ItemTemplate>
                                        <asp:Label ID="lblROLItem" runat="server" Text='<% #Bind("RolMesure")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Set New ROL">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtROL" runat="server" Width="80px" Text='<%#Eval("Rol")%>' onkeypress="return blockNonNumbers(this, event, true, true);"></asp:TextBox>
                                        <asp:Label ID="lblUnit" runat="server" Text='<% #Bind("Unit")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="width: 42%; float: right; padding-top: 10px;">
                            <asp:Button ID="btnSubmit2" runat="server" Text="Set Rol" OnClick="btnSubmit_Click"
                                Width="120px" />
                        </div>
                    </td>
                </tr>
            </table>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../Images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

