<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="InitStock.aspx.cs" Inherits="Accounts_InitStock" %>

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

        function Check() {
            var brand = document.getElementById("<%=ddlBrand.ClientID %>").selectedIndex;
            var category = document.getElementById("<%=ddlCategory.ClientID %>").selectedIndex;

            if (brand == 0) {
                alert("Select a Brand");
                document.getElementById("<%=ddlBrand.ClientID %>").focus();
                return false;
            }
            if (category == 0) {
                alert("Select a Category");
                document.getElementById("<%=ddlCategory.ClientID %>").focus();
                return false;
            }
            else
                return true;
        }

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=480,height=350,left = 490,top = 184');");
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                                Stock
                            </h1>
                            <h2>
                                Initialize</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="spacer">
                <img src="../Images/mask.gif" height="10" width="10" /></div>
            <asp:Panel ID="pnlSelection" runat="server">
                <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                    overflow: auto;">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                 &nbsp;Initilization Date&nbsp;:
                                <asp:TextBox ID="txtInitDt" runat="server" ReadOnly="true" Width="80px" TabIndex="2"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpInitDt" runat="server" Control="txtInitDt" Format="dd mmm yyyy">
                                </rjs:PopCalendar>
                                &nbsp;Brand&nbsp;:
                                <asp:DropDownList ID="ddlBrand" runat="server" TabIndex="1">
                                </asp:DropDownList>
                                &nbsp;Category&nbsp;:&nbsp;<asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" TabIndex="2">
                                </asp:DropDownList>
                                <span id="spanClass" runat="server" visible="false">&nbsp;Class&nbsp;:&nbsp;
                                    <asp:DropDownList ID="ddlClass" runat="server" TabIndex="3">
                                        <asp:ListItem Text="- SELECT -" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>&nbsp<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    OnClientClick="return Check();" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="4" />&nbsp;<a href="InitializeStockList.aspx" tabindex="10" onfocus="active(this);"
                                        onblur="inactive(this);">Go To Initialize Stock List</a>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlInitialize" runat="server">
                <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                    overflow: auto;">
                    <asp:GridView ID="gvStockInitialize" runat="server" Width="100%" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="ItemName" HeaderText="Item">
                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQty" runat="server" Width="70px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        TabIndex="5" Text="0"></asp:TextBox>
                                    <%#Eval("MesuringUnit")%>
                                    <asp:HiddenField ID="hdnItemCode" runat="server" Value='<%# Eval("ItemCode") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit MRP">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUnitMRP" runat="server" Width="70px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        TabIndex="5" Text="0"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UnitPurPrice">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUnitPurPrice" runat="server" Width="70px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        TabIndex="5" Text="0"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UnitSalePrice">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUnitSalePrice" runat="server" Width="70px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        TabIndex="5" Text="0"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlCurrency" runat="server" Width="70px" Style="font-family: Rupee Foradian;"
                                        TabIndex="5">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Record
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlUpdate" runat="server" Visible="False">
                <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                    overflow: auto;">
                    <a href="InitializeStockList.aspx" tabindex="6" onfocus="active(this);" onblur="inactive(this);">
                        Go To Initialize Stock List</a>
                    <div class="spacer">
                        <img src="../Images/mask.gif" height="10" width="10" /></div>
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
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit MRP">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUnitMRP" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Text='<%# Eval("Unit_MRP") %>' TabIndex="7" Width="70px"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UnitPurPrice">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUnitPurPrice" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Text='<%# Eval("Unit_PurPrice") %>' TabIndex="7" Width="70px"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UnitSalePrice">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUnitSalePrice" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Text='<%# Eval("Unit_SalePrice") %>' Width="70px" TabIndex="7"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlCurrency" runat="server" Style="font-family: Rupee Foradian;"
                                        TabIndex="7" Width="70px">
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
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlSave" runat="server" Visible="False">
                <div class="spacer">
                    <img src="../Images/mask.gif" height="10" width="10" /></div>
                <div>
                    <asp:TextBox ID="txtRemarks" runat="server" Width="400px" Height="80px" TextMode="MultiLine"
                        TabIndex="8"></asp:TextBox>
                    <ajaxToolkit:TextBoxWatermarkExtender ID="txtwe" runat="server" TargetControlID="txtRemarks"
                        WatermarkText="Enter Remarks(Within 200 Characters)" Enabled="True">
                    </ajaxToolkit:TextBoxWatermarkExtender>
                </div>
                <div class="spacer">
                    <img src="../Images/mask.gif" height="10" width="10" /></div>
                <div>
                    <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" onfocus="active(this);"
                        onblur="inactive(this);" TabIndex="9" />
                </div>
            </asp:Panel>
            <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="False" OnClick="btnUpdate_Click"
                onfocus="active(this);" onblur="inactive(this);" TabIndex="10" />
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" DynamicServicePath=""
                Enabled="True" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../Images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

