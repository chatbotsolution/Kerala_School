<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="Adjustment.aspx.cs" Inherits="Accounts_Adjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function CnfSave() {

            if (confirm("Are you sure to save ?\nNote :- Please verify the details before save. Once data saved can be modified.")) {

                return true;
            }
            else {

                return false;
            }
        }
        function CnfDelete() {

            if (confirm("You are going to delete a record. Are you sure to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }
    </script>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr style="background-color: #ededed;">
            <td width="350" align="left" valign="middle" style="background-color: ">
                <div class="headingcontainor">
                    <h2>
                        Loan/Advance Adjustment with Salary</h2>
                </div>
            </td>
            <td height="35" align="left" valign="middle" colspan="2">
            </td>
        </tr>
    </table>
    <br />
    <div align="center">
        <asp:Label ID="lblMsg1" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
    </div>
    <div>
        Accounting Year&nbsp;:&nbsp;<asp:DropDownList ID="drpSession" runat="server" AutoPostBack="true"
            Enabled="false">
        </asp:DropDownList>
        <br />
        <span style="color: #FF0000">Note:- Those not Using HR & Payroll Module will use
            this.
            <br />
            Enter the Loan Amount deducted from Salary, for each month in the current financial
            year. The transaction can not be reverted. So, verify data before save.</span>
        <br />
        <div align="right">
            <asp:Label ID="lblrecords" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
        </div>
        <asp:GridView ID="grdMonth" runat="server" AutoGenerateColumns="False" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%#Eval("GenLedgerId")%>'
                            AlternateText="Delete" ImageUrl="~/images/icon_delete.gif" ToolTip="Click to Delete"
                            Visible='<%#Eval("Btn")%>' OnClick="btnDelete_Click" OnClientClick="return CnfDelete();" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="5%" />
                    <HeaderStyle HorizontalAlign="center" Width="5%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Transaction Date">
                    <ItemTemplate>
                        <asp:Label ID="lblTransDate" runat="server" Text='<%#Eval("TransDateStr")%>'></asp:Label>
                        <asp:HiddenField ID="hfGenLedgerId" runat="server" Value='<%#Eval("GenLedgerId")%>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" />
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblTransAmt" runat="server" Text='<%#Eval("TransAmt")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="10%" />
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Receipt/Voucher No">
                    <ItemTemplate>
                        <asp:Label ID="lblVrNo" runat="server" Text='<%#Eval("PmtRecptVoucherNo")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" />
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Particulars">
                    <ItemTemplate>
                        <asp:Label ID="lblParticulars" runat="server" Text='<%#Eval("Particulars")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40%" />
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Amount Deducted from Salary against Loan/Advance">
                    <ItemTemplate>
                        <asp:TextBox ID="txtAmount" runat="server" Width="80px" TabIndex="1" Text='<%#Eval("AdjAmt")%>'
                            MaxLength='20' onblur="if (this.value == '') {this.value = '0.00';}" onfocus="if(this.value == '0.00') {this.value = '';}"
                            onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Save">
                    <ItemTemplate>
                        <asp:Button ID="btnSaveIndv" runat="server" Text="Save" onfocus="active(this);" onblur="inactive(this);"
                            TabIndex="1" Width="70px" OnClick="btnSaveIndv_Click" OnClientClick="return CnfSave();" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div align="right">
        <asp:Label ID="lblTotalAdj" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
    </div>
    <br />
    <div style="float: left">
        <asp:Button ID="btnSave" runat="server" Text="Save All" onfocus="active(this);" onblur="inactive(this);"
            TabIndex="2" Width="120px" OnClick="btnSave_Click" OnClientClick="return CnfSave();" />
        <asp:Button ID="btnClear" runat="server" Text="Reset" onfocus="active(this);" onblur="inactive(this);"
            TabIndex="3" Width="120px" OnClick="btnClear_Click" />
        <asp:Button ID="btnExpense" runat="server" Text="Expense Entry" onfocus="active(this);"
            onblur="inactive(this);" TabIndex="4" Width="120px" OnClick="btnExpense_Click" />
        &nbsp;<asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
    </div>
</asp:Content>

