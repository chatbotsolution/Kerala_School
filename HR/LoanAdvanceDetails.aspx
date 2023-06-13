<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoanAdvanceDetails.aspx.cs" Inherits="HR_LoanAdvanceDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Loan/Advance Details</title>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%">
        <tr id="trMsg" runat="server">
            <td align="left">
                <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdLoan" runat="server" AutoGenerateColumns="False" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="CalMonth" HeaderText="Month">
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CalYear" HeaderText="Year">
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RecAmt" HeaderText="Amount" DataFormatString="{0:F2}">
                            <HeaderStyle HorizontalAlign="Right" Width="100px" />
                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RecType" HeaderText="Recovery Type (Principal/Interest)" DataFormatString="{0:F2}">
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RcvdStatus" HeaderText="Recovery Status" DataFormatString="{0:F2}">
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        No Record(s)
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
