<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="InvStockCheckList.aspx.cs" Inherits="Inventory_InvStockCheckList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        $(document).ready(function () {
            document.getElementById("<%=btnAddNew.ClientID %>").focus();
        });
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
      
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Stock Check List
                </h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
            </div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Button ID="btnAddNew" runat="server" Text="Add New" OnClick="btnAddNew_Click"
                            TabIndex="5" />&nbsp;&nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"
                            OnClientClick="return CnfDelete();" TabIndex="6" />
                    </td>
                    <td align="right">
                        <b>
                            <asp:Label ID="lblRecord" runat="server" CssClass="tbltxt" ForeColor="Red"></asp:Label></b>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:GridView ID="grdStock" runat="server" Width="100%" AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="10" DataKeyNames="StockCheckId" OnPageIndexChanging="grdStock_PageIndexChanging"
                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" CssClass="mGrid"
                            TabIndex="4" OnRowDataBound="grdStock_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input type="checkbox" name="Checkb" value='<%# Eval("StockCheckId") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7px" />
                                    <HeaderTemplate>
                                        <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                    </HeaderTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="7px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <div id="dvEdit" runat="server">
                                            <a href='InvStockCheck.aspx?Id=<%#Eval("StockCheckId")%>'>Edit </a>
                                        </div>
                                        <div id="dvHide" runat="server" visible="false">
                                            <asp:Label ID="lblEdit" runat="server" Text="Edit"></asp:Label></div>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                    <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Checked Date">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <%#Eval("SDate")%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stock Location">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <%#Eval("Location")%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Checked By">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <%#Eval("CheckedBy")%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Verified By">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <%#Eval("VerifiedBy")%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle BackColor="#EFEFEF" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="#FDFDFD" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" id="tdMsg" runat="server" align="center">
                        <asp:Label ID="lblDel" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

