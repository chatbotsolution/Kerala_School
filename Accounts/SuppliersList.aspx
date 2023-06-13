<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="SuppliersList.aspx.cs" Inherits="Accounts_SuppliersList" %>

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

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=500,height=240,left = 490,top = 184');");
        }
        
    </script>

    <asp:UpdatePanel ID="upp" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                                Supplier
                            </h1>
                            <h2>
                                List</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" Width="150px" OnClick="btnAdd_Click" />
                        <asp:GridView ID="gvSupplierList" runat="server" Width="100%" AutoGenerateColumns="false"
                            PageSize="15" AllowPaging="true" OnRowCommand="gvSupplierList_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="PartyName" HeaderText="Supplier Name">
                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person">
                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Address" HeaderText="Address">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Mobile" HeaderText="Mobile">
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Email" HeaderText="Email">
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Item List">
                                    <ItemTemplate>
                                        <a href="javascript:popUp('ViewItemList.aspx?SupplierId=<%#Eval("PartyId")%>&Name=<%#Eval("PartyName") %>')">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon_view.gif" ToolTip="View Item List For Supplier" /></a>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="60px" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <a href="Suppliers.aspx?SupplierId=<%#Eval("PartyId")%>">
                                            <asp:Image ID="img2" runat="server" ImageUrl="~/Images/icon_edit.gif" ToolTip="Edit Supplier" /></a>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Cancel" ImageUrl="~/Images/icon_delete.gif"
                                            ToolTip="Delete Supplier" CommandName="Remove" CommandArgument='<%#Eval("PartyId") %>'
                                            OnClientClick="return confirm('Are you Sure To Delete Supplier ?')" />
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Suppliers
                            </EmptyDataTemplate>                            
                        </asp:GridView>
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

