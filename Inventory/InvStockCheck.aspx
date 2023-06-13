<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="InvStockCheck.aspx.cs" Inherits="Inventory_InvStockCheck" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
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
        function valid() {
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
                alert("Please select any record");
                return false;
            }
        }
        function CnfDelete() {

            if (confirm("You are going to delete a record. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }

        function isValid() {
            var Chkdt = document.getElementById("<%=txtChkDate.ClientID %>").value;
            var Loc = document.getElementById("<%=drpLoc.ClientID %>").value;
            var ChkBy = document.getElementById("<%=txtChkBy.ClientID %>").value;
            var CatNm = document.getElementById("<%=drpCat.ClientID %>").value;
            var ItemNm = document.getElementById("<%=ddlItem.ClientID %>").value;
            var Qty = document.getElementById("<%=txtQty.ClientID %>").value;

            debugger;

            if (Chkdt.trim() == "") {
                alert("Please Enter Date !");
                document.getElementById("<%=txtChkDate.ClientID %>").focus();
                return false;
            }

            if (Loc == 0) {
                alert("Please Select Location !");
                document.getElementById("<%=drpLoc.ClientID %>").focus();
                return false;
            }
            if (ChkBy.trim() == "") {
                alert("Please Enter Checked By !");
                document.getElementById("<%=txtChkBy.ClientID %>").focus();
                return false;
            }
            if (CatNm == 0) {
                alert("Please Select Catagory Name !");
                document.getElementById("<%=drpCat.ClientID %>").focus();
                return false;
            }
            if (ItemNm == 0) {
                alert("Please Select Item Name !");
                document.getElementById("<%=ddlItem.ClientID %>").focus();
                return false;
            }
            if (Qty.trim() == "") {
                alert("Please Enter Available Quantity !");
                document.getElementById("<%=txtQty.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }

        }       
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Stock Check
                </h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMessage" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px;
                margin-left: 2px;">
                <table width="100%" class="tbltxt">
                    <tr>
                        <td class="tbltxt" style="width: 250px">
                            Check Date :&nbsp;
                            <asp:TextBox ID="txtChkDate" runat="server" Width="80px" ReadOnly="True" CssClass="tbltxtbox"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpChkDate" runat="server" Control="txtChkDate" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtChkDate.value='';return false;"
                                Text="Clear"></asp:LinkButton>
                            <span style="color: Red; font-size: small;">*</span>&nbsp;&nbsp; Location :&nbsp;
                            <asp:DropDownList ID="drpLoc" runat="server" AutoPostBack="True" Width="150px" CssClass="tbltxtbox"
                                OnSelectedIndexChanged="drpLoc_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span style="color: Red; font-size: small;">*</span>&nbsp;&nbsp; Checked By :&nbsp;
                            <asp:TextBox ID="txtChkBy" runat="server" CssClass="tbltxtbox" Width="150px"></asp:TextBox>
                            <span style="color: Red; font-size: small;">*</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px;
                margin-left: 2px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                    <tr>
                        <td class="tbltxt">
                            Category Name :
                            <asp:DropDownList ID="drpCat" runat="server" Width="150px" OnSelectedIndexChanged="drpCat_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="tbltxtbox">
                            </asp:DropDownList>
                            <span style="color: Red; font-size: small;">*</span>&nbsp;&nbsp; Item Name :
                            <asp:DropDownList ID="ddlItem" runat="server" AutoPostBack="True" Width="150px" CssClass="tbltxtbox"
                                OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span style="color: Red; font-size: small;">*</span>&nbsp;&nbsp; Available Quantity
                            :
                            <asp:TextBox ID="txtQty" runat="server" Width="80px" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
                            <span style="color: Red; font-size: small;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="3">
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%; text-align: right; margin-top: 5px; margin-left: 2px;">
                <asp:Button ID="btnSave" runat="server" Text="Save" Font-Bold="True" Font-Size="8pt"
                    OnClick="btnSave_Click1" OnClientClick="return isValid();" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Font-Bold="True" Font-Size="8pt"
                    OnClick="btnCancel_Click" />
                <asp:Button ID="btnGoto" runat="server" Text="GotoList" Font-Bold="True" Font-Size="8pt"
                    OnClick="btnGoList_Click" />
                <asp:Button ID="btnChkList" runat="server" Text="Checked List" Font-Bold="True" Font-Size="8pt"
                    OnClick="btnChkList_Click" />
            </div>
            <asp:Panel ID="Pnl" runat="server">
                <%--<asp:Button ID="btnDel" runat="server" Text="Delete" OnClick="btnDel_Click1" />--%>
                <asp:GridView ID="grvItemPurchase" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    CssClass="Gridtxt" ForeColor="#333333" Width="100%" EmptyDataText="No Items added"
                    OnPageIndexChanging="grvItemPurchase_PageIndexChanging" OnRowEditing="grvItemPurchase_RowEditing">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <EmptyDataRowStyle Font-Bold="True" Font-Size="10pt" ForeColor="Black" Height="30px"
                        HorizontalAlign="Center" />
                    <Columns>
                        <%--<asp:TemplateField>
                            <ItemTemplate>
                                <input type="checkbox" name="Checkb" value='<%# Eval("StockCheckDetailId ") %>' />                                
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7px" />
                            <HeaderTemplate>
                                <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                            </HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="7px" />
                        </asp:TemplateField>--%>
                        <asp:CommandField ShowEditButton="True" HeaderText="Action" ItemStyle-Width="30px"
                            HeaderStyle-Width="30px" />
                        <asp:TemplateField HeaderText="Item Name">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("ItemName")%>
                                <asp:HiddenField ID="hfid" runat="server" Value='<%#Eval("StockCheckDetailId") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Available Quantity">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("AvlQty")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>