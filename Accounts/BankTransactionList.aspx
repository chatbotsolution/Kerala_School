<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="BankTransactionList.aspx.cs" Inherits="Accounts_BankTransactionList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        function CnfDelete() {
            if (confirm("You are going to delete bank transaction. Do you want to continue?")) {
                return true;
            }
            else {
                return false;
            }
        }

        function isValid() {
            var Bank = document.getElementById("<%=drpBank.ClientID %>").value;

            if (Bank == "0") {
                alert("Please Select a Bank A/C !");
                document.getElementById("<%=drpBank.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    
    </script>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr style="background-color: #ededed;">
            <td width="350" align="left" valign="middle" style="background-color: ">
                <div class="headingcontainor">
                    <h2>
                        Bank Transactions</h2>
                </div>
            </td>
            <td height="35" align="left" valign="middle" colspan="2">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <table width="95%">
        <tr>
            <td align="left" colspan="2">
                Bank A/C:
                <asp:DropDownList ID="drpBank" runat="server">
                </asp:DropDownList>
                &nbsp;From Date:
                <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" Width="80px" TabIndex="1"></asp:TextBox>
                <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
                </rjs:PopCalendar>
                To Date:
                <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" Width="80px" TabIndex="2"></asp:TextBox>
                <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" Format="dd mmm yyyy">
                </rjs:PopCalendar>
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnView" runat="server" TabIndex="3" Text="View List" OnClientClick="return isValid();"
                    OnClick="btnView_Click" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="right">
                <asp:Label ID="lblRecord" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <asp:GridView ID="grdTransactions" runat="server" OnPageIndexChanging="grdTransactions_PageIndexChanging"
                    Width="100%" PageSize="15" AllowPaging="true" AutoGenerateColumns="false" AllowSorting="True"
                    OnRowDeleting="grdTransactions_RowDeleting" OnRowDataBound="grdTransactions_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Cancel">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lblTransId" runat="server" Text='<%#Eval("TransId") %>' Visible="false"></asp:Label>
                                    <%--<asp:Label ID="lbl2" runat="server" Text='<%#Eval("GenLedgerId") %>' Visible="false"></asp:Label>--%>
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif"
                                        OnClientClick="return CnfDelete()" Visible="false" />
                                </center>
                            </ItemTemplate>
                            <HeaderStyle Width="60px" />
                        </asp:TemplateField>
                        <%--  <asp:BoundField DataField="Bank Account" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderText="Bank Account"  Visible="True"></asp:BoundField>--%>
                        <asp:BoundField DataField="TransDateStr" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" HeaderText="Date" Visible="True"></asp:BoundField>
                        <%--<asp:BoundField DataField="Particulars" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            HeaderText="Particulars" Visible="True"></asp:BoundField>--%>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblBankTransRks" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Diposit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                            HeaderText="Diposit" Visible="True"></asp:BoundField>
                        <asp:BoundField DataField="Withdrwal" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                            HeaderText="Withdrwal" Visible="True"></asp:BoundField>
                        <asp:BoundField DataField="ClosingBal" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                            HeaderText="Closing Bal" Visible="True"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDip" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
            </td>
            <td align="right">
                <asp:Label ID="lblWDL" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

