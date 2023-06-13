<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="VerifyStockList.aspx.cs" Inherits="Inventory_VerifyStockList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        $(document).ready(function () {
            document.getElementById("<%=drpchkdt.ClientID %>").focus();
        });

        function GridCheck() {
            for (i = 0; i < grd_remarks.length; i++) {
                var remark = document.getElementById(grd_remarks[i]);

                if (remark.value == "") {
                    alert("Remarks can not be blank");
                    remark.focus();
                    return false;
                }
            }
            return true;
        }
    
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Verify Stock
        </h2>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
    </div>
    <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px">
        <table width="100%" cellspacing="2" cellpadding="2">
            <tr>
                <td width="60" class="tbltxt">
                    Date
                </td>
                <td class="tbltxt" width="5">
                    :
                </td>
                <td class="tbltxt" width="140">
                    <asp:DropDownList ID="drpchkdt" runat="server" CssClass="vsmalltb" TabIndex="1">
                    </asp:DropDownList>
                </td>
                <td width="60" class="tbltxt">
                    Location
                </td>
                <td class="tbltxt" width="5">
                    :
                </td>
                <td class="tbltxt" width="140">
                    <asp:DropDownList ID="drpLoc" runat="server" CssClass="vsmalltb" TabIndex="2">
                    </asp:DropDownList>
                </td>
                <td colspan="4" class="tbltxt">
                    <asp:Button ID="btnShow" runat="server" Text="All Stock" OnClick="btnShow_Click"
                        TabIndex="3" />
                    &nbsp;<asp:Button ID="btnMismatched" runat="server" Text="Mismatched Stock" OnClick="btnMismatched_Click"
                        TabIndex="4" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:GridView ID="grdVerify" runat="Server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
            AllowPaging="true" PageSize="10" AutoGenerateColumns="false" PagerStyle-CssClass="pgr"
            OnPageIndexChanging="grdVerify_PageIndexChanging" Width="100%" TabIndex="1" onprerender="grdVerify_PreRender">
            <PagerSettings Mode="NumericFirstLast"></PagerSettings>
            <Columns>
                <asp:TemplateField HeaderText="Location">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:HiddenField ID="hfStockCheckId" Value='<%#Eval("StockCheckId")%>' runat="server" />
                        <%#Eval("Location")%>
                        <asp:HiddenField ID="hfLoc" Value='<%#Eval("LocationId")%>' runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Name">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%#Eval("ItemName")%>
                        <asp:HiddenField ID="hfItemCode" Value='<%#Eval("ItemCode")%>' runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Physical Stock">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%#Eval("PhysicalStock")%>
                        <asp:HiddenField ID="hfPhyStock" Value='<%#Eval("PhysicalStock")%>' runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Transaction Stock">
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemTemplate>
                        <%#Eval("TransactionStock")%>
                        <asp:HiddenField ID="hfTranStock" Value='<%#Eval("TransactionStock")%>' runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Difference">
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemTemplate>
                        <%#Eval("diffqun")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Remarks">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:TextBox ID="txtRemarks" runat="server" Width="100%" Text='<%#Eval("Remarks")%>'></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
            <RowStyle BackColor="#EFEFEF" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="#FDFDFD" />
        </asp:GridView>
    </div>
    <div align="right">
        <asp:Button ID="btnSubmit" runat="server" Text="Verify" OnClick="btnSubmit_Click"
            ToolTip="Click to Verify" />
    </div>
</asp:Content>
