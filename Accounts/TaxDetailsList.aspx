<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="TaxDetailsList.aspx.cs" Inherits="Accounts_TaxDetailsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">


        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function beginRequest(sender, args) {
            // show the popup
            $find('<%=mdlloading.ClientID %>').show();

        }

        function endRequest(sender, args) {
            //  hide the popup
            $find('<%=mdlloading.ClientID %>').hide();

        }

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
        <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor">
                            <h1>
                                Tax
                            </h1>
                            <h2>
                                Type</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top" style="padding-top: 10px;">
                        <table width="100%">
                            <tr>
                                <td width="250" align="left" valign="middle">
                                    <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click"  onfocus="active(this);" onblur="inactive(this);"/>
                                </td>
                                <td align="right" valign="middle">
                                    <asp:Label ID="lblRecord" runat="server" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:GridView ID="grdTax" runat="server" OnPageIndexChanging="grdTax_PageIndexChanging"
                                        DataKeyNames="TaxId" ToolTip="CurrencyGroups" Width="100%" PageSize="10" AllowPaging="true"
                                        AutoGenerateColumns="false" AllowSorting="True" OnRowDeleting="grdTax_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl2" runat="server" Text='<%#Eval("TaxId") %>' Visible="false">
                                                        </asp:Label>
                                                        <a href='TaxDetails.aspx?Id=<%#Eval("TaxId")%>'>
                                                            <img alt="Edit" src="../images/icon_edit.gif" /></a>
                                                        <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif"
                                                            OnClientClick="return CnfDelete()" />
                                                    </center>
                                                </ItemTemplate>
                                                <HeaderStyle Width="40px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TaxId" Visible="False"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Tax Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxCode" runat="server" Text='<% #Bind("TaxCode")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                                <ItemStyle HorizontalAlign="Left" Width="110px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxDesc" runat="server" Text='<% #Bind("TaxDesc")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxRate" runat="server" Font-Names="Rupee Foradian" Text='<% #Bind("TaxRate")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                                <ItemStyle HorizontalAlign="Left" Width="90px" />
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText="Is VAT">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIsVAT" runat="server" Text='<% #Bind("IsVAT")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                                <ItemStyle HorizontalAlign="Left" Width="90px" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="From Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<% #Bind("EffectiveDt")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Record
                                        </EmptyDataTemplate>
                                        <EmptyDataRowStyle />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                    PopupControlID="pnlloading" BackgroundCssClass="Background" />
                <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                    <div align="center" style="margin-top: 13px;">
                        <img src="../Images/loading.gif" />
                        <span>Loading ...</span>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </table>
</asp:Content>

