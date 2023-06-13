<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="InitializeStockList.aspx.cs" Inherits="Accounts_InitializeStockList" %>

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
        function Check() {

            var category = document.getElementById("<%=ddlCategory.ClientID %>").selectedIndex;
            if (category == 0) {
                alert("Select a Category");
                document.getElementById("<%=ddlCategory.ClientID %>").focus();
                return false;
            }
            else
                return true;
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                                Modify
                            </h1>
                            <h2>
                                Openning Stock</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlInitializeList" runat="server">
                <div class="spacer">
                    <img src="../Images/mask.gif" height="10" width="10" /></div>
                <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                    overflow: auto;">
                    <asp:Button ID="btnAdd" runat="server" Text="Set New Openning Stock" OnClick="btnAdd_Click" onfocus="active(this);"
                        onblur="inactive(this);" TabIndex="1" />
                    <asp:GridView ID="gvStockInitializeList" runat="server" Width="100%" AutoGenerateColumns="false" Visible="true">
                        <Columns>
                            <asp:BoundField DataField="InvNo" HeaderText="Invoice">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="purDt" HeaderText="Purchase Dt">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotPurAmt" HeaderText="Purchase Amt">
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <a href="InitStock.aspx?PID=<%#Eval("PurchaseId")%>&PurDate=<%#Eval("PurDate")%>&InvNo=<%#Eval("InvNo")%>"
                                        tabindex="2" onfocus="active(this);" onblur="inactive(this);">Edit</a>
                                </ItemTemplate>
                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Record
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle />
                    </asp:GridView>
                </div>
                <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                    overflow: auto;">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                &nbsp;Brand&nbsp;:
                                <asp:DropDownList ID="ddlBrand" runat="server" TabIndex="1">
                                </asp:DropDownList>
                                &nbsp;Category&nbsp;:&nbsp;<asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True"
                                    TabIndex="2" onselectedindexchanged="ddlCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span id="spanClass" runat="server" visible="false">&nbsp;Class&nbsp;:&nbsp;
                                    <asp:DropDownList ID="ddlClass" runat="server" TabIndex="3">
                                        <asp:ListItem Text="- SELECT -" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>&nbsp<asp:Button ID="btnSearch" runat="server" Text="Search"
                                    OnClientClick="return Check();" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="4" onclick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:GridView ID="gvUpdtStock" runat="server" Width="100%" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="ItemName" HeaderText="Item">
                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQty" runat="server" Width="70px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Text='<%# Eval("QtyIn") %>' TabIndex="7"></asp:TextBox>
                                    <%#Eval("MesuringUnit")%>
                                    <asp:HiddenField ID="hdnItemCode" runat="server" Value='<%# Eval("ItemCode") %>' />
                                    <asp:HiddenField ID="hdnPurDtlId" runat="server" Value='<%# Eval("PurchaseDetailId") %>' />
                                    <asp:HiddenField ID="hfQtyOut" runat="server" Value='<%# Eval("QtyOut") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit MRP">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUnitMRP" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Text='<%# Eval("Unit_MRP") %>' TabIndex="7" Width="70px" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UnitPurPrice">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUnitPurPrice" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Text='<%# Eval("Unit_PurPrice") %>' TabIndex="7" Width="70px" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UnitSalePrice">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUnitSalePrice" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Text='<%# Eval("Unit_SalePrice") %>' Width="70px" TabIndex="7" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlCurrency" runat="server" Style="font-family: Rupee Foradian;"
                                        TabIndex="7" Width="70px" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdncurrency" runat="server" Value='<%# Eval("CurrencyId") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Record
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="False"
                        onfocus="active(this);" onblur="inactive(this);" TabIndex="10" 
                    onclick="btnUpdate_Click" />
            </asp:Panel>
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
