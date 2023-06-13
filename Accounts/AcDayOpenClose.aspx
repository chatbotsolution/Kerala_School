<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="AcDayOpenClose.aspx.cs" Inherits="Accounts_AcDayOpenClose" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function Warn(msg) {
            if (confirm(msg)) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>

    <%--<asp:UpdatePanel ID="upp" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr style="background-color: #ededed;">
            <td width="350" align="left" valign="middle">
                <div>
                    <h1>
                        Account
                    </h1>
                    <h2>
                        Status</h2>
                </div>
            </td>
            <td height="35" align="left" valign="middle">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="margin-top: 10px; padding-left: 8px;">
                    Transaction Date :&nbsp;
                    <asp:TextBox ID="txtdate" runat="server" Width="80px"></asp:TextBox>&nbsp;
                    <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtdate" />
                    <asp:Button ID="btnOpen" runat="server" Text="Open Transaction" OnClick="btnOpen_Click"
                        OnClientClick="return Warn('Are you Sure to Open Transaction for Selected date ?');" />
                    <asp:Button ID="btnClose" runat="server" Text="Close Transaction" OnClick="btnClose_Click"
                        OnClientClick="return Warn('Are you Sure to Close Transaction for Selected date ?');" />
                </div>
                <div class="spacer">
                    <img src="../Images/mask.gif" height="10" width="10" /></div>
            </td>
        </tr>        
        <tr>
            <td colspan="2">
                &nbsp;&nbsp; &nbsp;</td>
        </tr>        
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnlSummary" Visible="false" runat="server">
                    <div style="width:250px;float:left;">
                    <div style=" background-color: #f5f5f5; border: 1px solid #CCC;"> <h3>Current 
                        Financial Year</h3></div>
                    <table>
                     
                        <tr>
                            <td width="150px">
                                MISC INCOME
                            </td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblIncome" runat="server" ForeColor="Red"></asp:Label><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                MISC EXPENSES
                            </td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblExpense" runat="server" ForeColor="Red"></asp:Label><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PURCHASE
                            </td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblPurchase" runat="server" ForeColor="Red"></asp:Label><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SALES
                            </td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblSale" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>
                    <div style="width: 250px; float: left; margin-left: 20px">
                        <div style="background-color: #f5f5f5; border: 1px solid #CCC;">
                            <h3>
                                Current Transaction Date</h3>
                        </div>
                        <table>
                            <tr>
                                <td width="150px">
                                    MISC INCOME
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="lblIncomeOnDt" runat="server" ForeColor="Red" Text="0.00"></asp:Label><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    MISC EXPENSES
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="lblExpenseOnDt" runat="server" ForeColor="Red" Text="0.00"></asp:Label><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    PURCHASE
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="lblPurchaseOnDt" runat="server" ForeColor="Red" Text="0.00"></asp:Label><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    SALES
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="lblSaleOnDt" runat="server" ForeColor="Red" Text="0.00"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
        <td>
        &nbsp;
        </td>
        </tr>
        <tr>
            <td  align="right">
               <strong> CASH BALANCE TILL DATE:
                <asp:Label ID="lblCashInHand" runat="server" ForeColor="Red" Text="0.00"></asp:Label></strong>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>


