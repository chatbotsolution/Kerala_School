<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="PartyMasterList.aspx.cs" Inherits="Accounts_PartyMasterList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
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

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div>
                            <h1>
                                Party
                            </h1>
                            <h2>
                                List</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 450px; padding-top: 10px;">
                        Account Groups :
                        <asp:DropDownList ID="drpACGroup" runat="server" TabIndex="3" AutoPostBack="True"
                            OnSelectedIndexChanged="drpACGroup_SelectedIndexChanged">
                            <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            <asp:ListItem Value="11">Sundry Creditors</asp:ListItem>
                            <asp:ListItem Value="12">Sundry Debtors</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp; Party Type :
                        <asp:DropDownList ID="drpPartyType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPartyType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td align="left">
                        <asp:Button ID="btnView" runat="server" Text="View List" OnClick="btnView_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top" style="padding-top: 10px;">
                        <table width="100%">
                            <tr>
                                <td width="250" align="left" valign="middle">
                                    <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                                </td>
                                <td align="right" valign="middle">
                                    <asp:Label ID="lblRecord" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:GridView ID="grdParty" runat="server" OnPageIndexChanging="grdParty_PageIndexChanging"
                                        DataKeyNames="PartyId" Width="100%" PageSize="15" AllowPaging="true" AutoGenerateColumns="false"
                                        AllowSorting="True" OnRowDeleting="grdParty_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl2" runat="server" Text='<%#Eval("PartyId") %>' Visible="false">
                                                    </asp:Label>
                                                    <a href='PartyMaster.aspx?PId=<%#Eval("PartyId")%>' title="Click To Edit">
                                                        <img alt="Edit" src="../images/icon_edit.gif" /></a> <span title="Click To Delete">
                                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif"
                                                                OnClientClick="return CnfDelete()" /></span>
                                                </ItemTemplate>
                                                <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PartyId" Visible="False"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Party Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPartyName" runat="server" Text='<% #Bind("PartyName")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contact Person">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPerson" runat="server" Text='<% #Bind("ContactPerson")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Party Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblType" runat="server" Text='<% #Bind("Party")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account Groups">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBrandName" runat="server" Text='<% #Bind("AG_Name")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Record
                                        </EmptyDataTemplate>
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="Gray" ForeColor="White" Font-Size="Large"
                                            Font-Bold="true" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
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
</asp:Content>
