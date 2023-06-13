<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="InvProcurement.aspx.cs" Inherits="Inventory_InvProcurement" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function isValid() {
            var purdt = document.getElementById("<%=txtDate.ClientID %>").value;
            var invoice = document.getElementById("<%=txtInvoiceNo.ClientID %>").value;
            var amt = document.getElementById("<%=txtTotAmt.ClientID %>").value;
            var supplier = document.getElementById("<%=txtSupplierName.ClientID %>").value;

            if (invoice.trim() == "") {
                alert("Please Enter Invoice Number !");
                document.getElementById("<%=txtInvoiceNo.ClientID %>").focus();
                return false;
            }
            if (purdt.trim() == "") {
                alert("Please Enter Date of Purchase !");
                document.getElementById("<%=txtDate.ClientID %>").focus();
                return false;
            }

            if (amt.trim() == "") {
                alert("Please Enter Amount !");
                document.getElementById("<%=txtTotAmt.ClientID %>").focus();
                return false;
            }
            if (supplier.trim() == "") {
                alert("Please Enter the Name of Supplier !");
                document.getElementById("<%=txtSupplierName.ClientID %>").focus();
                return false;
            }

            else {
                return true;
            }
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


        function extractNumber(obj, decimalPlaces, allowNegative) {
            var temp = obj.value;

            // avoid changing things if already formatted correctly
            var reg0Str = '[0-9]*';
            if (decimalPlaces > 0) {
                reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
            } else if (decimalPlaces < 0) {
                reg0Str += '\\.?[0-9]*';
            }
            reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
            reg0Str = reg0Str + '$';
            var reg0 = new RegExp(reg0Str);
            if (reg0.test(temp)) return true;

            // first replace all non numbers
            var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
            var reg1 = new RegExp(reg1Str, 'g');
            temp = temp.replace(reg1, '');

            if (allowNegative) {
                // replace extra negative
                var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
                var reg2 = /-/g;
                temp = temp.replace(reg2, '');
                if (hasNegative) temp = '-' + temp;
            }

            if (decimalPlaces != 0) {
                var reg3 = /\./g;
                var reg3Array = reg3.exec(temp);
                if (reg3Array != null) {
                    // keep only first occurrence of .
                    //  and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
                    var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
                    reg3Right = reg3Right.replace(reg3, '');
                    reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;
                    temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;
                }
            }

            obj.value = temp;
        }

        function IsValidItem() {
            var Category = document.getElementById("<%=ddlItemCat.ClientID %>").selectedIndex;
            var ItemCode = document.getElementById("<%=ddlItem.ClientID %>").selectedIndex;
            var Quantity = document.getElementById("<%=txtQty.ClientID %>").value;
            var UnitPrice = document.getElementById("<%=txtUnitPrice.ClientID %>").value;

            if (Category == "0") {
                alert("Please Select Category !");
                document.getElementById("<%=ddlItemCat.ClientID %>").focus();
                return false;
            }
            if (ItemCode == "0") {
                alert("Please Select Item !");
                document.getElementById("<%=ddlItem.ClientID %>").focus();
                return false;
            }
            if (Quantity.trim() == "") {
                alert("Please Enter Quantity !");
                document.getElementById("<%=txtQty.ClientID %>").focus();
                return false;
            }
            if (UnitPrice.trim() == "") {
                alert("Please Enter Unit Price !");
                document.getElementById("<%=txtUnitPrice.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Procurement Detail
                </h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblmsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
            </div>
            <div style="width: 760px; text-align: left; border: solid 1px black; margin-top: 5px;
                margin-left: 2px;">
                <table width="100%" class="tbltxt" cellpadding="0px;" cellspacing="2px;">
                    <tr>
                        <td>
                            Invoice No :
                            <asp:TextBox ID="txtInvoiceNo" runat="server" Width="80px" MaxLength="20" CssClass="tbltxtbox"></asp:TextBox>
                            <span style="color: Red; font-size: small;">*</span> Date :
                            <asp:TextBox ID="txtDate" runat="server" Width="80px" CssClass="tbltxtbox"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtDate" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <span style="color: Red; font-size: small;">*</span> Invoice Amt :
                            <asp:TextBox ID="txtTotAmt" runat="server" Width="60px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                MaxLength="12" CssClass="tbltxtbox"></asp:TextBox>
                            <span style="color: Red; font-size: small;">*</span> Other Charges :
                            <asp:TextBox ID="txtAddnlCharge" runat="server" Width="50px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                MaxLength="10" CssClass="tbltxtbox"></asp:TextBox>
                            Source :
                            <asp:DropDownList ID="ddlSource" CssClass="tbltxtbox" runat="server" Width="70">                                
                                <asp:ListItem Text="Purchased" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Gifted" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <fieldset style="width: 740px;">
                <legend class="tbltxt">Supplier Details :</legend>
                <table width="740px" class="tbltxt" cellpadding="0px;" cellspacing="2px;">
                    <tr>
                        <td align="left" style="width: 80px" valign="top">
                            Name :
                        </td>
                        <td valign="top" style="width: 215px">
                            <asp:TextBox ID="txtSupplierName" runat="server" MaxLength="50" Width="150px" CssClass="tbltxtbox"></asp:TextBox>
                            <span style="color: Red; font-size: small;">*</span>
                        </td>
                        <td align="left" style="width: 120px" valign="top">
                            Ph No :
                        </td>
                        <td valign="top" style="width: 120px">
                            <asp:TextBox ID="txtSupplierNo" runat="server" MaxLength="14" Width="80px" CssClass="tbltxtbox"></asp:TextBox>
                        </td>
                        <td style="width: 100px" valign="top">
                            Address :
                        </td>
                        <td valign="top" style="width: 365px">
                            <asp:TextBox ID="txtSupplierAddress" runat="server" MaxLength="200" Width="280px"
                                Height="40" TextMode="MultiLine" CssClass="tbltxtbox"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div style="width: 760px; text-align: left; border: solid 1px black; margin-top: 5px;
                margin-left: 2px;">
                <table width="100%" class="tbltxt" cellpadding="0px;" cellspacing="2px;">
                    <tr>
                        <td align="left" valign="top" style="width: 60px">
                            Remarks :
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="200" Width="100%" TextMode="MultiLine"
                                CssClass="tbltxtbox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="color: Red; font-size: 12px;" colspan="3">
                            (Check the Invoice No for correctness and change accordingly)
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 760px; text-align: left; border: solid 1px black; margin-top: 5px;
                margin-left: 2px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="tbltxt">
                    <tr>
                        <td width="200">
                            Category :<span style="color: Red; font-size: small;">*</span>
                            <asp:DropDownList ID="ddlItemCat" runat="server" Width="150" AutoPostBack="true"
                                CssClass="tbltxtbox" OnSelectedIndexChanged="ddlItemCat_SelectedIndexChanged">
                            </asp:DropDownList>                            
                        </td>
                        <td width="200">
                            Item Name :<span style="color: Red; font-size: small;">*</span>
                            <asp:DropDownList ID="ddlItem" runat="server" Width="150" CssClass="tbltxtbox">
                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                            </asp:DropDownList>                            
                        </td>
                        <td width="120">
                            Quantity :<span style="color: Red; font-size: small;">*</span>
                            <asp:TextBox ID="txtQty" runat="server" Width="80px" onkeypress="return blockNonNumbers(this, event, false, false);"
                                CssClass="tbltxtbox"></asp:TextBox>                            
                        </td>
                        <td width="120">
                            Unit Price :<span style="color: Red; font-size: small;">*</span>
                            <asp:TextBox ID="txtUnitPrice" runat="server" Width="80px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                CssClass="tbltxtbox"></asp:TextBox>                            
                        </td>
                        <td width="200">
                            Warranty/AMC :
                            <asp:TextBox ID="txtWarranty" runat="server" MaxLength="50" CssClass="tbltxtbox" Width="100px"></asp:TextBox>
                            <rjs:PopCalendar ID="PCWarranty" runat="server" Control="txtWarranty" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                        </td>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            MfdDate :
                            <asp:TextBox ID="txtMfdDt" runat="server" Width="80px" CssClass="tbltxtbox"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtMfdDt" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtMfdDt.value='';return false;"
                                Text="Clear"></asp:LinkButton>&nbsp;&nbsp;
                        
                            ExpDate :
                            <asp:TextBox ID="txtExpDt" runat="server" Width="80px" CssClass="tbltxtbox"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar4" runat="server" Control="txtExpDt" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtExpDt.value='';return false;"
                                Text="Clear"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        
                            <asp:Button ID="btnAdd" runat="server" Text="Add" Font-Bold="True" Width="80px" OnClientClick="return IsValidItem();"
                                Style="height: 22px" OnClick="btnAdd_Click" />                            
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Font-Bold="True" Width="80px"
                                OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 760px; text-align: left; margin-top: 5px; margin-left: 2px;">
                <asp:GridView ID="grvItemPurchase" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    CssClass="gridtxt" ForeColor="#333333" Width="100%" EmptyDataText="No Items added"
                    OnRowCommand="grvItemPurchase_RowCommand">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <EmptyDataRowStyle Font-Bold="True" Font-Size="10pt" ForeColor="Black" Height="30px"
                        HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="ItemName" HeaderText="ItemName">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Quantity" HeaderText="Qty" DataFormatString="{0:f2}">
                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitPrice" HeaderText="Price" DataFormatString="{0:f2}">
                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Warranty" HeaderText="Warranty">
                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ImageUrl="~/Inventory/images/edit.gif"
                                    ToolTip="Edit" CommandName="Modify" CommandArgument='<%#Eval("ItemCode") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Cancel" ImageUrl="~/Inventory/images/delete_icon.jpg"
                                    ToolTip="Cancel" CommandName="Remove" CommandArgument='<%#Eval("ItemCode") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataRowStyle HorizontalAlign="Center" Font-Bold="true" />
                    <FooterStyle BackColor="#5e5e5e" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                    <HeaderStyle />
                    <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                    <AlternatingRowStyle />
                </asp:GridView>
            </div>
            <div style="width: 760px; text-align: right; margin-top: 5px; margin-left: 2px;">
                <asp:Button ID="btnSave" runat="server" Text="Save" Font-Bold="True" Width="100px"
                    OnClientClick="return isValid();" OnClick="btnSave_Click" />&nbsp;
                <asp:Button ID="btnCancel2" runat="server" Text="Cancel" Font-Bold="True" Width="100px"
                    OnClick="btnCancel2_Click" />&nbsp;
                <asp:Button ID="btnShowDetails" runat="server" Text="Show List" Font-Bold="True"
                    Width="100px" OnClick="btnShowDetails_Click" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
