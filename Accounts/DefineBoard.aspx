<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="DefineBoard.aspx.cs" Inherits="Accounts_DefineBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="Javascript" type="text/javascript">
        function isValid() {
            var Name = document.getElementById("<%=txtBoardName.ClientID %>").value;


            if (Name.trim() == "") {
                alert("Please Enter Board Name !");
                document.getElementById("<%=txtBoardName.ClientID %>").focus();
                return false;
            }


            else {
                return true;
            }
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
    <table width="100%" border="0" cellspacing="0" cellpadding="0" align="left">
        <tr style="background-color: #ededed;">
            <td width="350" align="left" valign="middle" style="background-color: ">
                <div>
                    <h1>
                        Board/University
                    </h1>
                    <h2>
                        Details</h2>
                </div>
                <td height="35" align="left" valign="middle">
                    <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                </td>
            </td>
        </tr>
        <tr>
            <td id="BoardDetails" runat="server" colspan="2" valign="top" style="padding-top: 10px;">
                <table width="100%" border="0" cellspacing="1" cellpadding="9" align="left">
                    <tr>
                        <td style="width: 160px">
                            Board/University Name<span class="mandatory">*</span>
                        </td>
                        <td width="5">
                            :
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtBoardName" runat="server" MaxLength="100" TabIndex="1" Width="35%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2">
                            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Submit"
                                ValidationGroup="AddAccount" TabIndex="2" OnClientClick="return isValid();"></asp:Button>&nbsp;&nbsp;
                                 <asp:Button ID="btnView" OnClick="btnView_Click" runat="server" Text="View List" CausesValidation="false"
                                TabIndex="3"></asp:Button>&nbsp;&nbsp;
                            <asp:Button ID="btnClear" OnClick="btnClear_Click" runat="server" Text="Clear" CausesValidation="false"
                                TabIndex="4"></asp:Button>&nbsp;&nbsp;
                            <asp:Button ID="btncancel" OnClick="btncancel_Click" runat="server" Text="Cancel"
                                CausesValidation="false" TabIndex="5"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td id="BoardList" runat="server" colspan="2" valign="top" style="padding-top: 10px;">
                <table width="100%">
                    <tr>
                        <td width="250" align="left" valign="middle">
                            <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                        </td>
                        <td align="right" valign="middle">
                            <asp:Label ID="lblRecord" runat="server" ForeColor="Black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:GridView ID="grdBoard" runat="server" OnPageIndexChanging="grdBoard_PageIndexChanging"
                                DataKeyNames="BoardOrUnivId" Width="100%" PageSize="10"
                                AllowPaging="true" AutoGenerateColumns="false" AllowSorting="True" OnRowDeleting="grdBoard_RowDeleting"
                                OnRowEditing="grdBoard_RowEditing1">
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>                                            
                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("BoardOrUnivId") %>' Visible="false">
                                            </asp:Label>
                                            <span title="Click To Edit">
                                            <asp:ImageButton ID="imgEdit" runat="server" CommandName="Edit" ImageUrl="~/images/icon_edit.gif" /></span>
                                            <span title="Click To Delete"><asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="return CnfDelete()"
                                                ImageUrl="~/images/icon_delete.gif" /></span>                                            
                                        </ItemTemplate>
                                        <HeaderStyle Width="50px" HorizontalAlign="Center"/>
                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CurrencyId" Visible="False"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Board/University Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoardOrUnivName" runat="server" Text='<% #Bind("BoardOrUnivName")%>'></asp:Label>
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
</asp:Content>

