<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="VerifyStock.aspx.cs" Inherits="Inventory_VerifyStock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Verify stock</h2>
    </div>
    <br />
    <table width="100%">
        <tr>
            <td align="right" colspan="2">
                <asp:GridView ID="grdVerify" runat="Server" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                    AllowPaging="true" PageSize="10" AutoGenerateColumns="false" PagerStyle-CssClass="pgr"
                    OnPageIndexChanging="grdVerify_PageIndexChanging" Width="100%" TabIndex="1">
                    <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                    <Columns>
                        <asp:TemplateField HeaderText="Item Code">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("ItemCode")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Name">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("ItemName")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actual Stock">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("AvlQty")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity AsPer System">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("Qty")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFEFEF" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#FDFDFD" />
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
