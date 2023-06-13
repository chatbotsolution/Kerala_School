<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="PaymentVoucherList.aspx.cs" Inherits="Accounts_PaymentVoucherList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    &nbsp;<asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor">
                            <h1>
                                Payment
                            </h1>
                            <h2>
                                Voucher List</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="98%">
                <tr>
                    <td align="left" colspan="2">
                        <asp:Label ID="lblToDt" runat="server" Text="From Date : "></asp:Label>
                        <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" Width="80px" TabIndex="1"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
                        </rjs:PopCalendar>
                        &nbsp;<asp:Label ID="lblFrmDt" runat="server" Text="To Date : "></asp:Label>
                        <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" Width="80px" TabIndex="2"></asp:TextBox>
                        <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" Format="dd mmm yyyy">
                        </rjs:PopCalendar>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnView" runat="server" TabIndex="3" Text="View List" OnClick="btnView_Click" />
                        &nbsp;<asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add New" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td align="right">
                        <asp:Label ID="lblRecord" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:GridView ID="grdParty" runat="server" OnPageIndexChanging="grdParty_PageIndexChanging"
                            DataKeyNames="PR_Id" Width="100%" PageSize="20" AllowPaging="True" AutoGenerateColumns="False"
                            AllowSorting="True" OnRowDeleting="grdParty_RowDeleting" 
                            ondatabound="grdParty_DataBound" >
                            <Columns>
                                <asp:TemplateField HeaderText="Action" >
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("PR_Id") %>' Visible="false"></asp:Label>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif"
                                                OnClientClick="return CnfDelete()" Visible="false" />
                                            
                                            </center>
                                    </ItemTemplate>
                                    <HeaderStyle Width="60px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Print">
                                                <ItemTemplate>
                                 <a href='rptActPaymentVoucherPrint.aspx?PrId=<%#Eval("PR_Id")%>'>
                                                        <asp:Label ID="Print" runat="server" Text='Print' Font-Size="Small" Font-Underline="True" Font-Bold="True"></asp:Label>
                                                        <%--<asp:LinkButton ID="lnkFeeName" CausesValidation="false" Text='<%#Eval("FeeName")%>' CommandName="show" CommandArgument='<%#Eval("FeeID")%>' Runat="server"></asp:LinkButton>--%>
                                                    </a>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                <asp:BoundField DataField="Ag_Code" Visible="False"></asp:BoundField>
                                <asp:TemplateField HeaderText="Transaction Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<% #Bind("TransDtStr")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Party name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartyName" runat="server" Text='<%#Bind("AcctsHead") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                 <asp:BoundField DataField="Description" HeaderText="Description" ></asp:BoundField>
                                
                                <asp:TemplateField HeaderText="Paid Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%#Bind("Amount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" Width="100px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


