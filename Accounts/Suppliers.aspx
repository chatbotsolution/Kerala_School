<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="Suppliers.aspx.cs" Inherits="Accounts_Suppliers" %>

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

        function IsValid() {
            var category = document.getElementById("<%=ddlCategory.ClientID %>").selectedIndex;
            var item = document.getElementById("<%=ddlItem.ClientID %>").selectedIndex;
            var maxqty = document.getElementById("<%=txtMaxSupply.ClientID %>").value;
            var delay = document.getElementById("<%=txtDelayInDays.ClientID %>").value;

            if (category == "0") {
                alert("Please Select Category !");
                document.getElementById("<%=ddlCategory.ClientID %>").focus();
                return false;
            }
            if (item == "0") {
                alert("Please Select Item !");
                document.getElementById("<%=ddlItem.ClientID %>").focus();
                return false;
            }
            if (maxqty.trim() == "") {
                alert("Please Enter Maximum Supply Quantity !");
                document.getElementById("<%=txtMaxSupply.ClientID %>").focus();
                return false;
            }
            if (delay.trim() == "") {
                alert("Please Enter Supply Delay in no of days !");
                document.getElementById("<%=txtDelayInDays.ClientID %>").focus();
                return false;
            }

            else {
                return true;
            }
        }

        function CheckSupplier() {
            var supplier = document.getElementById("<%=ddlSuppliers.ClientID %>").selectedIndex;

            if (supplier == "0") {
                alert("Please Select Supplier !");
                document.getElementById("<%=ddlSuppliers.ClientID %>").focus();
                return false;
            }
            else
                return true;
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

    </script>

    <asp:UpdatePanel ID="upp" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                                Add Items
                            </h1>
                            <h2>
                                Under Supplier</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div width="100%">
                            Suppliers :
                            <asp:DropDownList ID="ddlSuppliers" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSuppliers_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="btnView" runat="server" Text="View List" Width="150px" OnClick="btnView_Click" />
                        </div>
                        <asp:Panel ID="pnlSupplierDtls" runat="server" Visible="false">
                            <div style="float: left; width: 100%; background-color: #f5f5f5; border: 1px solid #CCC;
                                height: auto; overflow: auto;">
                                <div style="padding: 8px; line-height: 22px;">
                                    <asp:Label ID="lblSupplierDtls" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                            overflow: auto;">
                            <table width="100%">
                                <tr>
                                    <td >
                                        Category :
                                        <asp:DropDownList ID="ddlCategory" runat="server" Width="150px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <asp:Panel ID="pnlclass" runat="server" Visible="false">
                                    <td>
                                    Class &nbsp;:
                                        <asp:DropDownList ID="ddlClass" runat="server" Width="150px" 
                                            AutoPostBack="true" onselectedindexchanged="ddlClass_SelectedIndexChanged">                                            
                                        </asp:DropDownList>
                                    </td>
                                </asp:Panel>
                                    <td >
                                        Item :
                                        <asp:DropDownList ID="ddlItem" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                                            <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td >
                                        Max Supply :
                                        <asp:TextBox ID="txtMaxSupply" runat="server" Width="50px" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                                        <asp:Label ID="lblUnit" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td >
                                        Supply Delay :
                                        <asp:TextBox ID="txtDelayInDays" runat="server" MaxLength="3" Width="50px" onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>(Days)
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="70px" OnClientClick="return IsValid();"
                                            OnClick="btnAdd_Click" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="70px" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <asp:Panel ID="pnl" runat="server" Visible="false">
                            <div>
                                <asp:GridView ID="gvItemSupply" runat="server" Width="100%" AutoGenerateColumns="false"
                                    OnRowCommand="gvItemSupply_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="Category" HeaderText="Category">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ItemName" HeaderText="Item">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MaxSupply" HeaderText="MaxSupply">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Unit" HeaderText="Unit">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SupplyDelay" HeaderText="Delay In Supply">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ImageUrl="~/Images/icon_edit.gif"
                                                    ToolTip="Edit" CommandName="Modify" CommandArgument='<%#Eval("ItemCode") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Cancel" ImageUrl="~/Images/icon_delete.gif"
                                                    ToolTip="Cancel" CommandName="Remove" CommandArgument='<%#Eval("ItemCode") %>' />
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
                            <div class="spacer">
                                <img src="../Images/mask.gif" height="10" width="10" /></div>
                            <div>
                                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClientClick="return CheckSupplier();"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnCancel1" runat="server" Text="Cancel" Width="100px" OnClick="btnCancel1_Click" />
                            </div>
                        </asp:Panel>
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

