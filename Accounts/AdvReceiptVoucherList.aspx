<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="AdvReceiptVoucherList.aspx.cs" Inherits="Accounts_AdvReceiptVoucherList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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
    
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="400" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor">
                            <h2>
                                 Receipt Voucher Aginst Advance Paid to Party
                               </h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle" colspan="2">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="98%">
                <tr>
                    <td align="left" colspan="2">
                        <asp:Label ID="lblToDt" runat="server" Text="Receipt To Date : "></asp:Label>
                        <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" Width="80px" TabIndex="1"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
                        </rjs:PopCalendar>
                        <asp:Label ID="lblFrmDt" runat="server" Text="Receipt From Date : "></asp:Label>
                        <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" Width="80px" TabIndex="2"></asp:TextBox>
                        <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" Format="dd mmm yyyy">
                        </rjs:PopCalendar>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnView" runat="server" TabIndex="3" Text="View List" OnClick="btnView_Click" />
                        &nbsp;<asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" 
                            style="width: 78px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        
                    </td>
                    <td align="right">
                        <asp:Label ID="lblRecord" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:GridView ID="grdParty" runat="server" OnPageIndexChanging="grdParty_PageIndexChanging"
                            DataKeyNames="PR_Id" Width="100%" PageSize="20" AllowPaging="true" AutoGenerateColumns="false"
                            AllowSorting="True" OnRowDeleting="grdParty_RowDeleting">
                            <Columns>
                                <asp:TemplateField HeaderText="Action" >
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("PR_Id") %>' Visible="false"></asp:Label>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif"
                                                OnClientClick="return CnfDelete()" /></center>
                                    </ItemTemplate>
                                    <HeaderStyle Width="60px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Ag_Code" Visible="False"></asp:BoundField>
                                
                                
                                <asp:TemplateField HeaderText="Transaction Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<% #Bind("TransDtStr")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="Description" HeaderText="Description">
                                <ItemStyle HorizontalAlign="Left"/>
                                <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Received From">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartyName" runat="server" Text='<%#Bind("RcvdFrom") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Received Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%#Bind("Amount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" Width="120px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
</asp:Content>
