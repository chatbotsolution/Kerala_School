<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDetails.aspx.cs" Inherits="Inventory_PurchaseDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Item List</title>
    <link rel="stylesheet" href="../css/sms.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-top: 5px;">
        <h2>
            Item List</h2>
    </div>
    <table width="100%" cellpadding="0" cellspacing="0" align="left">
        <tr>
            <td>
                <div align="center">
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                </div>
                <asp:GridView ID="grdShowDtls" runat="server" CellPadding="4" ForeColor="#333333"
                    AutoGenerateColumns="False" EmptyDataText="No Data Found" Width="100%" CssClass="mGrid">
                    <EmptyDataRowStyle Font-Bold="True" Font-Size="10pt" Height="30px" HorizontalAlign="Left" />
                    <Columns>
                        <asp:BoundField DataField="ItemName" HeaderText="ItemName">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:f2}">
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <RowStyle BackColor="#EFEFEF" />
                    <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </td>
        </tr>        
    </table>
    <div align="center">
        <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="self.close();" />
    </div>
    </form>
</body>
</html>
