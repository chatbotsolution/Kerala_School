<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="ItemMasterList.aspx.cs" Inherits="Masters_ItemMasterList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        function IsValid() {
            var Category = document.getElementById("<%=drpCat.ClientID%>").value;
            var Class = document.getElementById("<%=drpclass.ClientID%>").value;
            if (Category == "1" && Class == "0") {
                alert("Please Select Class!");
                document.getElementById("<%=drpclass.ClientID%>").focus();
                return false;
            }

            else {
                return true;
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
        function DoWaterMarkOnFocus(txt, text) {
            if (txt.value == text) {
                txt.value = "";
            }
        }
        function DoWaterMarkOnBlur(txt, text) {
            if (txt.value == "") {
                txt.value = text;
            }
        }
    </script>

    <div>
        <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                    <img src="../images/icon_cp.jpg" width="29" height="29"></div>
                <div style="padding-top: 5px;">
                    <h2>
                        Item List</h2>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                </div>
                <div>
                    <img src="../images/mask.gif" height="8" width="10" /></div>
                <div style="background-color: #666; padding: 1px; margin: 0 auto;">
                    <div style="background-color: #FFF; padding: 5px;">
                        <table border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                            <tr>
                                <td style="vertical-align: middle; text-align: left" valign="top" colspan="2">
                                    Brand&nbsp; :&nbsp;
                                    <asp:DropDownList ID="drpBrand" runat="server" CssClass="smalltb" Width="80px">
                                    </asp:DropDownList>
                                    &nbsp;
                                    Category&nbsp; :&nbsp;
                                    <asp:DropDownList ID="drpCat" runat="server" CssClass="smalltb" AutoPostBack="true" 
                                        onselectedindexchanged="drpCat_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                     Class :
                                        <asp:DropDownList ID="drpclass" runat="server" Enabled="false" 
                                            CssClass="vsmalltb" Width="75px">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                    <asp:TextBox ID="txtItem" Text="Search by Item Name or Item Barcode" onfocus="DoWaterMarkOnFocus(this,'Search by Item Name or Item Barcode')"
                                        onblur="DoWaterMarkOnBlur(this,'Search by Item Name or Item Barcode')" runat="server"
                                        CssClass="largetb" Width="200px"></asp:TextBox>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="return IsValid();" />
                                    <asp:Button ID="btnView" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <%--<div style="background-color: #666; padding: 1px; margin: 0 auto; margin-top: 5px;">
                    <div style="background-color: #FFF; padding: 10px;">--%>
                <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tbltxt">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblRecord" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" style="padding-top: 5px;">
                            <asp:GridView ID="grdItem" runat="server" OnPageIndexChanging="grdItem_PageIndexChanging"
                                DataKeyNames="ItemCode" ToolTip="List Of Items" Width="100%" PageSize="20" AllowPaging="true"
                                AutoGenerateColumns="false" AllowSorting="True" OnRowDeleting="grdItem_RowDeleting"
                                CssClass="mGrid">
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl2" runat="server" Text='<%#Eval("ItemCode") %>' Visible="false"></asp:Label>
                                                <a href='ItemMaster.aspx?PId=<%#Eval("ItemCode")%>'>
                                                    <img alt="Edit" src="../images/icon_edit.gif" /></a>
                                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif"
                                                    OnClientClick="return CnfDelete()" /></center>
                                        </ItemTemplate>
                                        <HeaderStyle Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemCode" Visible="False"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItem" runat="server" Text='<% #Bind("ItemName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Category Name" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategory" runat="server" Text='<% #Bind("CatName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Brand Name" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBrand" runat="server" Text='<% #Bind("BrandName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sale Price">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalePrice" runat="server" Text='<% #Bind("SalePrice")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MesuringUnit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMesuringUnit" runat="server" Text='<% #Bind("MesuringUnit")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record
                                </EmptyDataTemplate>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="Gray" ForeColor="White" Font-Size="Large"
                                    Font-Bold="true" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
               
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>