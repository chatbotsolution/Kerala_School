<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewIssuedDetails.aspx.cs" Inherits="Inventory_ViewIssuedDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Fee Details</title>
    <link rel="stylesheet" href="../css/sms.css" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <strong>Issue Details</strong></div>
    <asp:GridView Width="100%" ID="grdIssueDetails" runat="server" AutoGenerateColumns="False"
        HorizontalAlign="Center" CssClass="mGrid">
        <Columns>
            <asp:TemplateField HeaderText="Item Name">
                <ItemTemplate>
                    <asp:Label ID="lblFeeName" runat="server" Text='<%#Eval("ItemName")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                <HeaderStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <asp:Label ID="lblAmtDue" runat="server" Text='<%#Eval("Qty")%>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Right" Width="100px" />
                <ItemStyle HorizontalAlign="Right" Width="100px" />
            </asp:TemplateField>
        </Columns>
        <RowStyle BackColor="#EFEFEF" />
        <EditRowStyle BackColor="#2461BF" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="#FDFDFD" />
    </asp:GridView>
    <div align="center">
        <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="self.close();" /></div>
    </form>
</body>
</html>
