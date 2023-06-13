<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewSupplier.aspx.cs" Inherits="Accounts_ViewSupplier" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>List Of Suppliers</title>
    <style type="text/css">
        .headingcontainor
        {
            margin-bottom: 0px;
            margin-top: 0px;
            width: 350px;
            float: left;
            padding-left: 8px;
        }
        h1
        {
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
            font-size: 22px;
            color: #0091d2;
            margin: 0px 0px 0px 0px;
            padding: 0px 0px 0px 0px;
            display: inline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="headingcontainor">
        <h1>
            Supplier List For
        </h1>
        <asp:Label ID="lblItem" runat="server" Text="" Font-Size="22px" Font-Names="Trebuchet MS"></asp:Label>
    </div>
    <table width="100%" cellpadding="0" cellspacing="0" align="left">
        <tr>
            <td>
                <%--<div align="center">
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                </div>--%>
                <asp:GridView ID="gvSuppliers" runat="server" Width="100%" AutoGenerateColumns="false"
                    OnRowCommand="gvSuppliers_RowCommand">
                    <Columns>
                        <%--<asp:BoundField DataField="PartyName" HeaderText="Supplier Name">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>--%>
                        <asp:TemplateField HeaderText="Supplier Name">
                            <ItemTemplate>
                                <%#Eval("PartyName")%>
                                <asp:HiddenField ID="hdnSupplierName" runat="server" Value='<%#Eval("PartyName")%>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="MaxSupplyCapacity" HeaderText="Supply Capacity">
                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SupplyDelay" HeaderText="Supply Delay(In days)">
                            <HeaderStyle HorizontalAlign="Center" Width="150px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <%--<asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Cancel" ImageUrl="~/Images/icon_delete.gif"
                                    ToolTip="Delete" CommandName="Remove" CommandArgument='<%#Eval("PartyId") %>'
                                    OnClientClick="return confirm('Are you Sure To Remove Item From Cart ?')" />--%>
                                <asp:LinkButton ID="lnkbtnSelect" runat="server" CommandName="Select" CommandArgument='<%#Eval("PartyId") %>'>Select</asp:LinkButton>
                                <asp:HiddenField ID="hdnSupplierId" Value='<%#Eval("PartyId") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="70px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No Suppliers Available
                    </EmptyDataTemplate>
                    <EmptyDataRowStyle />
                </asp:GridView>
                <div class="spacer">
                    <img src="../Images/mask.gif" height="10" width="10" /></div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="self.close()" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
