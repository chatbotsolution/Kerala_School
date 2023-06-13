<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="ReceiveStockPOEdit.aspx.cs" Inherits="Accounts_ReceiveStockPOEdit" %>

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
            var rcvQty = document.getElementById("<%=txtRcvQty.ClientID %>").value;
            var uniMRP = document.getElementById("<%=txtUnitMRP.ClientID %>").value;
            var unitPurPrice = document.getElementById("<%=txtUnitPurPrice.ClientID %>").value;

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
            var purDt = document.getElementById("<%=txtPurchaseDt.ClientID %>").value;
            var invNo = document.getElementById("<%=txtInvoice.ClientID %>").value;
            var purAmt = document.getElementById("<%=txtPurAmt.ClientID %>").value;
            if (purDt.trim() == "") {
                alert("Select Purchase Date");
                document.getElementById("<%=txtPurchaseDt.ClientID %>").focus();
                return false;
            }
            if (invNo.trim() == "") {
                alert("Enter Invoice Numeber");
                document.getElementById("<%=txtInvoice.ClientID %>").focus();
                return false;
            }
            if (purAmt.trim() == "") {
                alert("Enter Purchase Amount");
                document.getElementById("<%=txtPurAmt.ClientID %>").focus();
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
                        <asp:HiddenField ID="hfPurchaseId" runat="server" Value="0" />
                        <asp:HiddenField ID="hfPurchaseDtlId" runat="server" Value="0" />
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <div style="float: right">
                            <asp:Button ID="btnShowList" runat="server" Text="Go to List Page" OnClick="btnShowList_Click"
                                TabIndex="0" onfocus="active(this);" onblur="inactive(this);" />
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
                            Purchase Date<span class="mandatory">*</span>&nbsp;:&nbsp;<asp:TextBox ID="txtPurchaseDt"
                                runat="server" Width="80px" TabIndex="1" Enabled="False"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpPurchaseDt" runat="server" Control="txtPurchaseDt" AutoPostBack="False"
                                Format="dd mmm yyyy" Enabled="False"></rjs:PopCalendar>
                        </td>
                        <td align="left">
                            Invoice No.<span class="mandatory">*</span>&nbsp;:&nbsp;<asp:TextBox ID="txtInvoice"
                                runat="server" Width="80px" TabIndex="2" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            Supplier&nbsp;:&nbsp;<asp:Label ID="lblSupplier" runat="server" Width="200px" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="spacer">
                <img src="../Images/mask.gif" height="10" width="10" /></div>
            <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                overflow: auto;">
                <table width="100%">
                    <tr>
                        <td>
                            <b>Item </b>
                        </td>
                        <td>
                            <b>Received Qty </b><span class="mandatory">*</span>
                        </td>
                        <td>
                            <b>Unit MRP</b> <span class="mandatory">*</span>
                        </td>
                        <td>
                            <b>Unit Purchase Price</b> <span class="mandatory">*</span>
                        </td>
                        <td>
                            <b>Unit Sale Price </b>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblItemName" runat="server" Width="200px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRcvQty" runat="server" Width="100px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="6">0</asp:TextBox>
                            <asp:Label ID="lblUnit" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUnitMRP" runat="server" Width="100px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="7">0</asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUnitPurPrice" runat="server" Width="100px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="8">0</asp:TextBox>
                            <i>(<span style="font-size: small">excluding Discount</span>)</i></td>
                        <td>
                            <asp:TextBox ID="txtUnitSalePrice" runat="server" Width="100px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                TabIndex="9">0</asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                                OnClientClick="return IsDetailValid();" TabIndex="10" onfocus="active(this);"
                                onblur="inactive(this);" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" TabIndex="11" onfocus="active(this);"
                                onblur="inactive(this);" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grdPurchase" runat="server" Width="100%" AutoGenerateColumns="False"
                    OnRowCommand="grdPurchase_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ItemName" HeaderText="Item">
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Received Qty">
                            <ItemTemplate>
                                <%#Eval("QtyIn")%>
                                <%#Eval("MesuringUnit")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sold Qty">
                            <ItemTemplate>
                                <%#Eval("QtyOut")%>
                                <%#Eval("MesuringUnit")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Unit_MRP" HeaderText="Unit MRP" DataFormatString="{0:0.00}">
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit_PurPrice" HeaderText="Unit Purchase Price" DataFormatString="{0:0.00}">
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit_SalePrice" HeaderText="Unit Sale Price" DataFormatString="{0:0.00}">
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" CommandArgument='<%# Eval("PurchaseDetailId") %>'
                                    CommandName="Modify" ImageUrl="~/Images/icon_edit.gif" onblur="inactive(this);"
                                    onfocus="active(this);" TabIndex="12" ToolTip="Edit" />
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
            <table style="width: 100%" align="right">
                <tr>
                    <td align="right" style="font-weight: bold; width: 100%;" valign="baseline">
                        Puchase Amount:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtPurAmt" runat="server" Width="207px" onkeypress="return blockNonNumbers(this, event, true, false);"
                            Enabled="False" Text="0.00" TabIndex="13"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        Discount:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtDiscPer" runat="server" Width="60px" onkeypress="return blockNonNumbers(this, event, true, false);"
                            Text="0.0000" TabIndex="14" onkeyup="return calDiscAmt();"></asp:TextBox><i> %</i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtDiscAmt" runat="server" Width="100px" onkeypress="return blockNonNumbers(this, event, true, false);"
                            Text="0.0000" TabIndex="15" onkeyup="return calDiscPer();"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        VAT/CST Amount:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtVAT" runat="server" Width="207px" onkeypress="return blockNonNumbers(this, event, true, false);"
                            Text="0.00" TabIndex="16" onkeyup="return getInvAmt();"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        Shipping Charges:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtShipCharge" runat="server" Text="0.00" Width="207px" onkeypress="return blockNonNumbers(this, event, true, false);"
                            TabIndex="17" onkeyup="return getInvAmt();"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        Additional Charges:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtOtherCharge" runat="server" Width="207px" Text="0.00" onkeypress="return blockNonNumbers(this, event, true, false);"
                            TabIndex="18" onkeyup="return getInvAmt();"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        Additional Charge Desc:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtDesc" runat="server" Width="207px" TabIndex="19"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="font-weight: bold;" valign="baseline">
                        Invoice Amount:
                    </td>
                    <td align="left" valign="baseline" colspan="2">
                        <asp:TextBox ID="txtInvoiceAmt" runat="server" Width="207px" Enabled="false" Text="0.00"
                            TabIndex="20"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div class="spacer">
                <img src="../Images/mask.gif" height="10" width="10" /></div>
            <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                overflow: auto;" align="right">
                <asp:TextBox ID="txtRemarks" runat="server" Width="320px" Height="80px" TextMode="MultiLine"
                    TabIndex="21"></asp:TextBox>
                <ajaxToolkit:TextBoxWatermarkExtender ID="txtwe" runat="server" TargetControlID="txtRemarks"
                    WatermarkText="Enter Remarks(Within 200 Characters)" Enabled="True">
                </ajaxToolkit:TextBoxWatermarkExtender>
                <br />
                <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" TabIndex="22" onfocus="active(this);" onblur="inactive(this);" OnClientClick="return confirm('You are going Modify Stock. All related records will be Modified. Do you want to continue ?')" />
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

