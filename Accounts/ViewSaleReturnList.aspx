<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewSaleReturnList.aspx.cs" Inherits="Accounts_ViewSaleReturnList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sale Return List</title>
      <script src="../Scripts/CommonScript.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Sale Return List Against Invoice :&nbsp;
        <asp:Label ID="lblInvoice" runat="server" ForeColor="Black" Font-Bold="true" Font-Size="14px"></asp:Label>
    </div>
    <div align="center" style="background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
        overflow: auto;">
        <asp:GridView ID="gvStockList" runat="server" Width="100%" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="CatName" HeaderText="Category">
                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="ItemName" HeaderText="Item">
                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Qty Received">
                    <ItemTemplate>
                        <%#Eval("QtyIn")%>&nbsp;
                        <%#Eval("MesuringUnit")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                </asp:TemplateField>
                <%--<asp:BoundField DataField="ManufactureOrBatchNo" HeaderText="Batch No">
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>--%>
                <asp:BoundField DataField="Unit_MRP" HeaderText="MRP" DataFormatString="{0:f2}">
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Unit_PurPrice" HeaderText="PurPrice" DataFormatString="{0:f2}">
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Unit_SalePrice" HeaderText="SalePrice" DataFormatString="{0:f2}">
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>
                <%--<asp:BoundField DataField="QtyInEachPack" HeaderText="Qty Per Pack">
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>--%>
                <%--<asp:BoundField DataField="LooseQtyIn" HeaderText="Loose Qty">
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>--%>
                <%--<asp:BoundField DataField="LooseSalePrice" HeaderText="Loose SalePrice" DataFormatString="{0:f2}">
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>--%>
                <%--<asp:BoundField DataField="Status" HeaderText="Status">
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>--%>
            </Columns>
            <EmptyDataTemplate>
                No Record
            </EmptyDataTemplate>
            <EmptyDataRowStyle />
        </asp:GridView>
    </div>
    <div class="spacer">
        <img src="../Images/mask.gif" height="10" width="10" /></div>
    <div align="center">
        <asp:Button ID="btnCancel" runat="server" Text="Close"  onfocus="active(this);" onblur="inactive(this);" OnClientClick="self.close()" />
    </div>
    </form>
</body>
</html>
