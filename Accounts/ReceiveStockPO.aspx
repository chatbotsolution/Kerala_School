<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="ReceiveStockPO.aspx.cs" Inherits="Accounts_ReceiveStockPO" %>

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

        function IsDetailValid() {
            var category = document.getElementById("<%=ddlCategory.ClientID %>").selectedIndex;
            var item = document.getElementById("<%=ddlItem.ClientID %>").selectedIndex;
            var rcvQty = document.getElementById("<%=txtRcvQty.ClientID %>").value;
            var uniMRP = document.getElementById("<%=txtUnitMRP.ClientID %>").value;
            var unitPurPrice = document.getElementById("<%=txtUnitPurPrice.ClientID %>").value;

            if (category == "0") {
                alert("Select a Category");
                document.getElementById("<%=ddlCategory.ClientID %>").focus();
                return false;
            }
            if (item == "0") {
                alert("Select an Item");
                document.getElementById("<%=ddlItem.ClientID %>").focus();
                return false;
            }
            if (rcvQty.trim() == "") {
                alert("Enter Received Quantity");
                document.getElementById("<%=txtRcvQty.ClientID %>").focus();
                return false;
            }
            if (uniMRP.trim() == "") {
                alert("Enter MRP Per Unit");
                document.getElementById("<%=txtUnitMRP.ClientID %>").focus();
                return false;
            }
            if (unitPurPrice.trim() == "") {
                alert("Enter Purchase Price Per Unit");
                document.getElementById("<%=txtUnitPurPrice.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }

        function IsDocValid() {
           <%-- var supplier = document.getElementById("<%=ddlSupplier.ClientID %>").selectedIndex;--%>
            var rdbtnRcvStock = document.getElementById("<%=rdbtnRcvStock.ClientID %>");
            var purDt = document.getElementById("<%=txtPurchaseDt.ClientID %>").value;
          <%--  var invNo = document.getElementById("<%=txtInvoice.ClientID %>").value;--%>
            var purAmt = document.getElementById("<%=txtPurAmt.ClientID %>").value;
            var Remks = document.getElementById("<%=txtRemarks.ClientID %>").value;
            if (purDt.trim() == "") {
                alert("Select Purchase Date");
                document.getElementById("<%=txtPurchaseDt.ClientID %>").focus();
                return false;
            }
            <%--if (invNo.trim() == "") {
                alert("Enter Invoice Numeber");
                document.getElementById("<%=txtInvoice.ClientID %>").focus();
                return false;
            }--%>
            <%--if (supplier == "0") {
                alert("Select a Supplier");
                document.getElementById("<%=ddlSupplier.ClientID %>").focus();
                return false;
            }--%>
            if (purAmt.trim() == "") {
                alert("Enter Purchase Amount");
                document.getElementById("<%=txtPurAmt.ClientID %>").focus();
                return false;
            }
            if (Remks.trim() == "") {
                alert("Enter Remarks");
                document.getElementById("<%=txtRemarks.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
        //Calculate Discount Amount
        function calDiscAmt() {
            var purAmt = document.getElementById("<%= txtPurAmt.ClientID%>").value;
            var discPer = document.getElementById("<%=txtDiscPer.ClientID %>").value;
            if (discPer.trim() == "") {
                discPer = 0;
            }
            if (parseFloat(discPer) >= 100) {
                document.getElementById("<%=txtDiscPer.ClientID %>").value = 0;
                document.getElementById("<%=txtDiscAmt.ClientID %>").value = 0;
                alert("Discount Percentage shouldn't be greater than OR equals to 100.");
            }
            else {
                var discAmt = (parseFloat(discPer) * parseFloat(purAmt)) / 100;
                document.getElementById("<%=txtDiscAmt.ClientID %>").value = discAmt.toFixed(4);
            }
            getInvAmt();
        }
        //Calculate Discount Percentage
        function calDiscPer() {
            var purAmt = document.getElementById("<%= txtPurAmt.ClientID%>").value;
            var discAmt = document.getElementById("<%=txtDiscAmt.ClientID %>").value;
            if (discAmt.trim() == "") {
                discAmt = 0;
            }
            if (parseFloat(discAmt) >= parseFloat(purAmt)) {
                document.getElementById("<%=txtDiscPer.ClientID %>").value = 0;
                document.getElementById("<%=txtDiscAmt.ClientID %>").value = 0;
                alert("Discount Amount shouldn't be greater than OR equals to the Purchase Amount.");
            }
            else {
                var discPer = (parseFloat(discAmt) / parseFloat(purAmt)) * 100;
                document.getElementById("<%=txtDiscPer.ClientID %>").value = discPer.toFixed(4);
            }
            getInvAmt();
        }
        //Calculate Invoice Amount
        function getInvAmt() {
            var purAmt = document.getElementById("<%= txtPurAmt.ClientID%>").value;
            var discAmt = document.getElementById("<%=txtDiscAmt.ClientID %>").value
            var taxAmt = document.getElementById("<%=txtVAT.ClientID %>").value;
            var shipCharges = document.getElementById("<%=txtShipCharge.ClientID %>").value;
            var addnlCharges = document.getElementById("<%=txtOtherCharge.ClientID %>").value;
            var totalAmt = 0;
            var invAmt = 0;
            if (purAmt.trim() == "")
                purAmt = 0;
            if (discAmt.trim() == "")
                discAmt = 0;
            if (taxAmt.trim() == "")
                taxAmt = 0;
            if (shipCharges.trim() == "")
                shipCharges = 0;
            if (addnlCharges.trim() == "")
                addnlCharges = 0;
            var otherCharges = parseFloat(taxAmt) + parseFloat(shipCharges) + parseFloat(addnlCharges);
            invAmt = parseFloat(purAmt) - parseFloat(discAmt) + parseFloat(otherCharges);
            document.getElementById("<%=txtInvoiceAmt.ClientID %>").value = parseFloat(invAmt).toFixed(4);
            return false;
        }
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=480,height=350,left = 490,top = 184');");
        }
    </script>

    <asp:UpdatePanel ID="upp" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                                Receive
                            </h1>
                            <h2>
                                Stock</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                            overflow: auto;">
                            <div style="float: left;">
                                <div style="float: left; width: 300px">
                                    <asp:RadioButtonList ID="rdbtnRcvStock" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rdbtnRcvStock_SelectedIndexChanged"
                                        TabIndex="1" Visible="false">
                                        <asp:ListItem Text="Receive Stock Directly" Value="1" Selected="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="float: left;">
                                    Payment Mode&nbsp;:&nbsp;</div>
                                <div style="float: left; background-color: Highlight;">
                                    <asp:RadioButtonList ID="rdbtnPayMode" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rdbtnPayMode_SelectedIndexChanged"
                                        TabIndex="1">
                                        <asp:ListItem Text="Credit" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Cash/Bank" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div style="float: right">
                                <asp:Button ID="btnShowList" runat="server" Text="Go to List Page" OnClick="btnShowList_Click"
                                    TabIndex="0" onfocus="active(this);" onblur="inactive(this);" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="spacer">
                <img src="../Images/mask.gif" height="10" width="10" /></div>
            <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                overflow: auto;">
                <table width="100%">
                    <tr>
                        <td align="left">
                            Purchase Date <span class="mandatory">*</span>&nbsp;:&nbsp;
                            <asp:TextBox ID="txtPurchaseDt" runat="server" Width="80px" TabIndex="2" ReadOnly="true"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpPurchaseDt" runat="server" Control="txtPurchaseDt" AutoPostBack="False" To-Today="true"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                        </td>
                        <td align="left">
                            Invoice No. <span class="mandatory">*</span>&nbsp;:&nbsp;
                            <asp:TextBox ID="txtInvoice" runat="server" Width="80px" TabIndex="3"></asp:TextBox>
                        </td>
                        <td align="left">
                            Supplier <span class="mandatory">*</span>&nbsp;:&nbsp;<asp:DropDownList ID="ddlSupplier"
                                runat="server" TabIndex="4">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="spacer">
                <img src="../Images/mask.gif" height="10" width="10" /></div>
            <asp:Panel ID="pnlRcvDirect" runat="server" Visible="False">
                <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                    overflow: auto;">
                    <table width="100%">
                        <tr>
                            <td>
                                Category <span class="mandatory">*</span>
                            </td>
                            <asp:Panel ID="pnlclass" runat="server" Visible="False">
                                <td>
                                    Class
                                </td>
                                <td>
                                    Stream
                                </td>
                            </asp:Panel>
                            <td>
                                Item <span class="mandatory">*</span>
                            </td>
                            <td>
                                Received Qty <span class="mandatory">*</span>
                            </td>
                            <td>
                                Unit MRP <span class="mandatory">*</span>
                            </td>
                            <td>
                                Unit Pur Price <span class="mandatory">*</span>
                            </td>
                            <td>
                                Unit Sale Price
                            </td>
                            <td>
                                Currency
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlCategory" runat="server" Width="120px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" TabIndex="5">
                                </asp:DropDownList>
                            </td>
                            <asp:Panel ID="pnlclassVal" runat="server" Visible="False">
                                <td>
                                    <asp:DropDownList ID="ddlClass" runat="server" Width="120px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlClass_SelectedIndexChanged" TabIndex="6">
                                        <asp:ListItem Text="- SELECT -" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStream" runat="server" Width="120px" AutoPostBack="True" Enabled="false"
                                        OnSelectedIndexChanged="ddlStream_SelectedIndexChanged" TabIndex="6">
                                        <asp:ListItem Text="- SELECT -" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </asp:Panel>
                            <td>
                                <asp:DropDownList ID="ddlItem" runat="server" Width="120px" TabIndex="7">
                                    <asp:ListItem Text="- SELECT -" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRcvQty" runat="server" Width="70px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                    TabIndex="8"></asp:TextBox>
                                <asp:Label ID="lblUnit" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUnitMRP" runat="server" Width="70px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                    TabIndex="9"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUnitPurPrice" runat="server" Width="70px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                    TabIndex="10"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUnitSalePrice" runat="server" Width="70px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                    TabIndex="11"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCurrency" runat="server" Width="80px" Style="font-family: Rupee Foradian;"
                                    TabIndex="12">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnAdd" runat="server" Text="ADD" OnClick="btnAdd_Click" OnClientClick="return IsDetailValid();"
                                    TabIndex="13" onfocus="active(this);" onblur="inactive(this);" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gvDirectPur" runat="server" Width="100%" AutoGenerateColumns="False"
                        OnRowCommand="gvDirectPur_RowCommand" TabIndex="14">
                        <Columns>
                            <asp:BoundField DataField="Category" HeaderText="Category">
                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ItemName" HeaderText="Item">
                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                <HeaderStyle HorizontalAlign="Left" Width="200px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Received Qty">
                                <ItemTemplate>
                                    <%#Eval("RcvQty")%>
                                    <%#Eval("MeasuringUnit")%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="UnitMRP" HeaderText="Unit MRP">
                                <ItemStyle HorizontalAlign="Left" Width="70px" />
                                <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitPurPrice" HeaderText="Unit Pur Price">
                                <ItemStyle HorizontalAlign="Left" Width="70px" />
                                <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitSalePrice" HeaderText="Unit Sale Price">
                                <ItemStyle HorizontalAlign="Left" Width="70px" />
                                <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Currency" HeaderText="Currency">
                                <ItemStyle HorizontalAlign="Left" Width="70px" CssClass="CurSymbol" />
                                <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ImageUrl="~/Images/icon_edit.gif"
                                        ToolTip="Edit" CommandName="Modify" CommandArgument='<%# Eval("ItemCode") %>'
                                        TabIndex="15" onfocus="active(this);" onblur="inactive(this);" />
                                </ItemTemplate>
                                <HeaderStyle Width="20px" HorizontalAlign="Center" />
                                <ItemStyle Width="20px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Cancel" ImageUrl="~/Images/icon_delete.gif"
                                        ToolTip="Delete" CommandName="Remove" CommandArgument='<%# Eval("ItemCode") %>'
                                        OnClientClick="return confirm('Are you Sure To Remove Item From Cart ?')" TabIndex="15"
                                        onfocus="active(this);" onblur="inactive(this);" />
                                </ItemTemplate>
                                <HeaderStyle Width="20px" HorizontalAlign="Center" />
                                <ItemStyle Width="20px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Record
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <table style="width: 100%" align="right">
                <tr>
                    <td align="right" style="font-weight: bold; width: 100%;" valign="baseline">
                        Puchase Amount:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtPurAmt" runat="server" Width="207px" onkeypress="return blockNonNumbers(this, event, true, false);"
                            Enabled="False" Text="0.00" TabIndex="17"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        Discount:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtDiscPer" runat="server" Width="60px" onkeypress="return blockNonNumbers(this, event, true, false);"
                            Text="0.0000" TabIndex="18" onkeyup="return calDiscAmt();"></asp:TextBox><i> %</i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtDiscAmt" runat="server" Width="100px" onkeypress="return blockNonNumbers(this, event, true, false);"
                            Text="0.0000" TabIndex="18" onkeyup="return calDiscPer();"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        VAT/CST Amount:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtVAT" runat="server" Width="207px" onkeypress="return blockNonNumbers(this, event, true, false);"
                            Text="0.00" TabIndex="18" onkeyup="return getInvAmt();"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        Shipping Charges:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtShipCharge" runat="server" Text="0.00" Width="207px" onkeypress="return blockNonNumbers(this, event, true, false);"
                            TabIndex="19" onkeyup="return getInvAmt();"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        Additional Charges:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtOtherCharge" runat="server" Width="207px" Text="0.00" onkeypress="return blockNonNumbers(this, event, true, false);"
                            TabIndex="20" onkeyup="return getInvAmt();"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        Additional Charge Desc:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtDesc" runat="server" Width="207px" TabIndex="21"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        Invoice Amount:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtInvoiceAmt" runat="server" Width="207px" Enabled="false" Text="0.00"
                            TabIndex="22"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlSave" runat="server" Enabled="false">
                <div class="spacer">
                    <img src="../Images/mask.gif" height="10" width="10" /></div>
                <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                    overflow: auto;">
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: 100px">
                                <strong>Payment Mode</strong><span class="mandatory">*</span>
                            </td>
                            <td align="left">
                                <strong>: </strong>&nbsp;
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="rbtnMode" runat="server" OnSelectedIndexChanged="rbtnMode_SelectedIndexChanged"
                                    AutoPostBack="True" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0"
                                    RepeatLayout="Flow" TabIndex="23">
                                    <asp:ListItem Value="C">Cash</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="B">Bank</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 100px">
                                <strong>Bank Name</strong><span class="mandatory" id="spBankNm" runat="server">*</span>
                            </td>
                            <td align="left">
                                <strong>: </strong>&nbsp;
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="drpBankName" runat="server" TabIndex="24">
                                </asp:DropDownList>
                                <strong>Instrument No.</strong><span class="mandatory" id="spInstNo" runat="server">*</span>
                                <strong>: </strong>&nbsp;
                                <asp:TextBox ID="txtInstrumentNo" runat="server" TabIndex="25"></asp:TextBox>
                                <strong>Instrument Date</strong><span class="mandatory" id="spInstDt" runat="server">*</span>
                                <strong>: </strong>&nbsp;
                                <asp:TextBox ID="txtInstrumentDt" runat="server" TabIndex="26"></asp:TextBox>&nbsp;
                                <rjs:PopCalendar ID="dtpInstrumentDt" runat="server" Control="txtInstrumentDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <div class="spacer">
                <img src="../Images/mask.gif" height="10" width="10" /></div>
                Remarks:
            <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                overflow: auto;">
                
                <asp:TextBox ID="txtRemarks" runat="server" Width="400px" Height="80px" TextMode="MultiLine" MaxLength="199"
                    TabIndex="27"></asp:TextBox>
            </div>
            <div class="spacer">
                <img src="../Images/mask.gif" height="10" width="10" /></div>
            <div>
                <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" OnClientClick="return IsDocValid()"
                    TabIndex="28" onfocus="active(this);" onblur="inactive(this);" Enabled="False" />
            </div>
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

