<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="AccountGroups.aspx.cs" Inherits="Accounts_AccountGroups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <script type="text/javascript" language="javascript">
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
        function Valid() {

            var Group = document.getElementById("<%=txtGroupName.ClientID %>").value;
            if (Group.trim() == "") {
                alert("Please Enter Account sub Group Name");
                document.getElementById("<%=txtGroupName.ClientID %>").focus();
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

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnGotoList" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="grdAccountGroup" EventName="RowEditing" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div>
                            <h1>
                                Account
                            </h1>
                            <h2>
                                Groups</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="padding-top: 10px;">
                            <asp:Panel ID="pnlAddDetail" runat="Server" Width="100%" HorizontalAlign="Center">
                                <table width="100%" border="0" cellspacing="1" cellpadding="9">
                                    <tr>
                                        <td>
                                            Parent Account Group<span class="mandatory">*</span>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlParentAccountGroup" runat="server" TabIndex="1">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px">
                                            Account Sub Group Name<span class="mandatory">*</span>
                                        </td>
                                        <td width="5">
                                            :
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtGroupName" runat="server" MaxLength="100" TabIndex="2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Save & And New"
                                                OnClientClick="return Valid()" TabIndex="3" Width="130px"></asp:Button>
                                            <asp:Button ID="btnGotoList" OnClick="btnGotoList_Click" runat="server" Text="Go To List"
                                                TabIndex="4" Width="120px"></asp:Button>
                                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" Width="120px" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                                Width="120px" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlList" runat="server" Width="100%" HorizontalAlign="Center">
                                <table cellspacing="0" cellpadding="0" width="95%" border="0">
                                    <tbody>
                                        <tr>
                                            <td style="vertical-align: middle; text-align: left">
                                                &nbsp;<asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                                            </td>
                                        </tr>
                                        <tr id="norecord" runat="server">
                                            <td style="height: 10px">
                                                <asp:Label ID="lblnorecord" runat="server" Text="No Record Found" Font-Bold="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">
                                                <table width="80%">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:GridView ID="grdAccountGroup" runat="server" OnRowEditing="grdAccountGroup_RowEditing"
                                                                OnPageIndexChanging="grdAccountGroup_PageIndexChanging" DataKeyNames="Ag_Code"
                                                                ToolTip="AccountGroups" Width="100%" AllowPaging="false" AutoGenerateColumns="false"
                                                                AllowSorting="True" OnRowDeleting="grdAccountGroup_RowDeleting" 
                                                                onrowdatabound="grdAccountGroup_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Action">
                                                                        <ItemTemplate>                                                                            
                                                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("AG_Code") %>' Visible="false"></asp:Label>
                                                                            <asp:ImageButton ID="imgEdit" runat="server" CommandName="Edit" ImageUrl="~/images/icon_edit.gif" />
                                                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif"
                                                                                OnClientClick="return CnfDelete()" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Ag_Code" Visible="False"></asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Account Group">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAccountName" runat="server" Text='<% #Bind("AG_Name")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Account Parent">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAccountParent" runat="server" Text='<%#Bind("Parent") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAGType" runat="server" Text='<%#Bind("AG_Type") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </div>
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
