<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReturnItemList.aspx.cs" Inherits="Accounts_ReturnItemList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Return ItemList</title>

    <script src="../Scripts/CommonScript.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        Invoice No :
        <asp:Label runat="server" ID="lblInvoice" ForeColor="Black" Font-Bold="true" Font-Size="14px"></asp:Label>
    </div>
    <div align="center">
        <asp:GridView ID="gvRetItemList" runat="server" Width="100%" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="CatName" HeaderText="Category">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="ItemName" HeaderText="Item">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Return Qty">
                    <ItemTemplate>
                        <%#Eval("RetQty")%>&nbsp;
                        <%#Eval("MesuringUnit")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
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
    <div align="center">
        <asp:Button ID="btnCancel" runat="server" Text="Close" OnClientClick="self.close()"
            onfocus="active(this);" onblur="inactive(this);" TabIndex="1" />
    </div>
    </form>
</body>
</html>
