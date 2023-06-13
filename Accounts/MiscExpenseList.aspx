<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="MiscExpenseList.aspx.cs" Inherits="Accounts_MiscExpenseList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        function CnfDelete() {

            if (confirm("You are going to delete a record. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Miscellaneous Expenditure Details</h2>
    </div>
    <div class="linegap">
        <img src="../images/mask.gif" height="5" width="10" />
    </div>
    <div class="linegap">
        <img src="../images/mask.gif" height="5" width="10" />
    </div>
    <table width="100%">
        <tr>
            <td style="height: 10px" colspan="2">
            </td>
        </tr>
        <tr>
            <td valign="bottom" colspan="2">
                <fieldset style="height: 40px; vertical-align: bottom;">
                    <legend style="background-color: Transparent;" class="tbltxt"><strong>Selection Criteria
                    </strong></legend><strong class="tbltxt">&nbsp;&nbsp;From Date:</strong>&nbsp;
                    <asp:TextBox ID="txtFromDate" runat="server" Height="17px" Width="80px" 
                        CssClass="tbltxt"></asp:TextBox>
                    <rjs:PopCalendar ID="dtpFromDate" runat="server" Control="txtFromDate" AutoPostBack="False"
                        Format="dd mmm yyyy"></rjs:PopCalendar>
                    &nbsp; <strong class="tbltxt">&nbsp;&nbsp;To Date:</strong>&nbsp;
                    <asp:TextBox ID="txtToDate" runat="server" Height="17px" Width="80px" 
                        CssClass="tbltxt"></asp:TextBox>
                    <rjs:PopCalendar ID="dtpToDate" runat="server" Control="txtToDate" AutoPostBack="False"
                        Format="dd mmm yyyy"></rjs:PopCalendar>
                    &nbsp; &nbsp;&nbsp;
                    <asp:Button ID="btnshow" runat="server" CausesValidation="False" Text="Show" OnClick="btnshow_Click" />
                &nbsp;<asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 455px;">
                &nbsp;<asp:Button ID="btnNew" runat="server" OnClick="btnNew_Click" Text="New Expense" />
                &nbsp;
            </td>
            <td style="width: 20px;" align="right" class="tdMsg">
                TotalRecords:&nbsp;<asp:Label ID="lblNoOfRec" runat="server" Text="" Style="text-align: right"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gvMiscExp" runat="server" Width="100%" AutoGenerateColumns="False"
                    AllowPaging="True" PageSize="15" 
                    OnPageIndexChanging="gvMiscExp_PageIndexChanging" ShowFooter="True" 
                    onrowdatabound="gvMiscExp_RowDataBound" 
                    onrowdeleting="gvMiscExp_RowDeleting" onrowediting="gvMiscExp_RowEditing" >
                    <FooterStyle BackColor="#424242" Font-Bold="True" ForeColor="White" 
                        HorizontalAlign="Right" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action" >
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("ExpId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblType" runat="server" Text='<%#Eval("eType") %>' Visible="false"></asp:Label>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit" ImageUrl="~/images/icon_edit.gif" Visible="false"/>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif"
                                                OnClientClick="return CnfDelete()" Visible="false" /></center>
                                    </ItemTemplate>
                                    <HeaderStyle Width="60px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Print">
                                                <ItemTemplate>
                                   <a href='rptActExpensePrint.aspx?ExpId=<%#Eval("ExpId")%>&eType=<%#Eval("eType") %>'>
                                                        <asp:Label ID="Print" runat="server" Text='Print' Font-Size="Small" Font-Underline="True" Font-Bold="True"></asp:Label>
                                                        <%--<asp:LinkButton ID="lnkFeeName" CausesValidation="false" Text='<%#Eval("FeeName")%>' CommandName="show" CommandArgument='<%#Eval("FeeID")%>' Runat="server"></asp:LinkButton>--%>
                                                    </a>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Exp Date">
                            <HeaderStyle Width="100px" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("TransDateStr")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                       <%-- <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderText="Expense Account Head" DataField="AcctsHead"  />--%>
                        <asp:TemplateField HeaderText="Expenditure Details">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("ExpDetails")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                       <%-- <asp:TemplateField HeaderText="Receipt/Voucher No.">
                            <HeaderStyle Width="130px" HorizontalAlign="Right" />
                            <FooterStyle ForeColor="White" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <%#Eval("PmtRecptVoucherNo")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Amount">
                            <HeaderStyle Width="100px" HorizontalAlign="Right" />
                            <%--<FooterStyle ForeColor="White" HorizontalAlign="Right" />--%>
                            <ItemTemplate>
                            <asp:Label ID="lblAmnt" runat="server" Text='<%#Eval("Amount")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate >
                            No Data
                    </EmptyDataTemplate>
                    <%--<RowStyle BackColor="#EFEFEF" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#FDFDFD" />--%>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>



