<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="RaisePO.aspx.cs" Inherits="Accounts_RaisePO" %>

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

        function CheckItem() {

            var flag;
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {

                    if (e.checked) {
                        flag = true;
                        break;
                    }
                    else
                        flag = false;
                }
            }
            //alert(flag);

            if (flag == true)
                return true;
            else {
                alert("Please select Item");
                return false;
            }
        }

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=580,height=340,left = 490,top = 184');");
        }

        function IsValid() {
            var OrderDt = document.getElementById("<%=txtOrderDt.ClientID %>").value;
            var ExpDelDt = document.getElementById("<%=txtExpDelDt.ClientID %>").value;
            var Supplier = document.getElementById("<%=drpSupplier.ClientID %>").value;

            if (OrderDt.trim() == "") {
                alert("Please Provide Order Date !");
                document.getElementById("<%=txtOrderDt.ClientID %>").focus();
                return false;
            }

            if (ExpDelDt.trim() == "") {
                alert("Please Provide Expected Delivery Date !");
                document.getElementById("<%=txtExpDelDt.ClientID %>").focus();
                return false;
            }
            if (Date.parse(ExpDelDt.trim()) < Date.parse(OrderDt.trim())) {
                alert("Order Date cannot be greater than Expected Delivery Date!");
                return false;
            }
            if (Supplier == "0") {
                alert("Please Select Supplier !");
                document.getElementById("<%=drpSupplier.ClientID %>").focus();
                return false;
            }
            else
                return true;
        }
        
    </script>

    <asp:UpdatePanel ID="upp" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                                Purchase
                            </h1>
                            <h2>
                                Order</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                            overflow: auto;">
                            <div style="width: 100%; float: left;">
                                <table>
                                    <tr>
                                        <td>
                                &nbsp;<strong>Category&nbsp;:</strong>&nbsp;
                                <asp:DropDownList runat="server" ID="ddlCategory" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                </asp:DropDownList>&nbsp;</td>
                                <asp:Panel ID="pnlclass" runat="server" Visible="false">
                                    <td>
                                    <strong>Class &nbsp;:</strong>&nbsp;
                                        <asp:DropDownList ID="ddlClass" runat="server" Width="150px" 
                                            AutoPostBack="true" onselectedindexchanged="ddlClass_SelectedIndexChanged">                                            
                                        </asp:DropDownList>
                                    </td>
                                </asp:Panel>
                                <td>
                                <strong>Item&nbsp;:</strong>&nbsp;
                                <asp:DropDownList runat="server" ID="ddlItem" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                                    <asp:ListItem Text="--No Items--" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList></td>
                                <td>
                                    <strong>Supplier&nbsp;:</strong>&nbsp;
                                    <asp:DropDownList ID="drpSupplier" runat="server">
                                    </asp:DropDownList>  
                                </td>
                                    </tr>
                                </table>
                            </div>
                            
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                            <div style="width: 70%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto; overflow: auto;">
                            <div style="width: 100%; float: left;">
                                &nbsp;<strong>Order Date&nbsp;:</strong>&nbsp;
                            <asp:TextBox Width="80px" runat="server" ID="txtOrderDt"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpOrederDt" runat="server" Control="txtOrderDt" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="lnkbtnClearOrderDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtOrderDt.value='';return false;"
                Text="Clear" ></asp:LinkButton>
                            &nbsp;&nbsp;<strong>Expected Delivery Date&nbsp;:</strong>&nbsp;
                            <asp:TextBox ID="txtExpDelDt" runat="server" Width="80px"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpExpDelDt" runat="server" Control="txtExpDelDt" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="lnkbtnClearDelDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtExpDelDt.value='';return false;"
                Text="Clear" ></asp:LinkButton>
                            </div>
                        </div>
                        <asp:Panel ID="pnl1" runat="server" Visible="false">
                            <table width="100%">
                                <tr>
                                    <td style="width: 50%">                                        
                                        <asp:GridView ID="gvItemList" runat="server" Width="100%" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <input type="checkbox" name="Checkb" value='<%#Eval("ItemCode") %>' />
                                                        <asp:HiddenField ID="hdnItemCode" runat="server" Value='<%#Eval("ItemCode") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item">
                                                    <ItemTemplate>
                                                        <%#Eval("ItemName")%>
                                                        <asp:HiddenField ID="hdnItemName" runat="server" Value='<%#Eval("ItemName") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQty" runat="server" Width="50px"></asp:TextBox>
                                                        <asp:Label ID="lblUnit" runat="server" Text='<%#Eval("MesuringUnit")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No Items Available
                                            </EmptyDataTemplate>
                                            <EmptyDataRowStyle />
                                        </asp:GridView>                                                                                    
                                    </td>
                                    <td style="width: 50%">
                                    </td>                
                                </tr>
                                <tr>
                                    <td valign="top" style="width: 50%; text-align:right;">
                                        <asp:Button ID="btnAddItem" runat="server" Text="Add Item To Purchase Order" 
                                            Visible="false" OnClientClick="return CheckItem();" OnClick="btnAddItem_Click" />
                                    </td>
                                    <td style="width: 50%">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <%--<div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>--%>
                        
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                    </td>
                </tr>
                <asp:Panel ID="pnl2" runat="server" Visible="false">
                    <tr>
                        <td colspan="2">
                            <div style="width: 70%; float: left;">
                                <asp:GridView ID="gvItemsToPurchase" runat="server" Width="100%" AutoGenerateColumns="false"
                                    OnRowCommand="gvItemsToPurchase_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="Category" HeaderText="Category">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ItemName" HeaderText="Item">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Unit" HeaderText="Measuring Unit">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Cancel" ImageUrl="~/Images/icon_delete.gif"
                                                    ToolTip="Delete" CommandName="Remove" CommandArgument='<%#Eval("ItemCode") %>'
                                                    OnClientClick="return confirm('Are you Sure To Remove Item From Cart ?')" />
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
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="spacer">
                                <img src="../Images/mask.gif" height="10" width="10" /></div>                                                                                  
                            <asp:Button ID="btnSubmit" runat="server" Text="Save Purchase Order" Width="200px"
                                OnClientClick="return IsValid();" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel Purchase Order" Width="200px"
                                OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </asp:Panel>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlCategory" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

