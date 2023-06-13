<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="Brands.aspx.cs" Inherits="Accounts_Brands" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
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
            var Name = document.getElementById("<%=txtBrandName.ClientID %>").value;

            if (Name.trim() == "") {
                alert("Please Enter Brand Name");
                document.getElementById("<%=txtBrandName.ClientID %>").focus();
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
        $(function () {
            $('#editBox_imgAdd').click(function () {
                $('#dialogAdd').show();
                return false;
            });
            $('#editBox_btnList').click(function () {
                $('#dialogAdd').hide();
                $('#dialogList').show();
                return false;
            });
        });

    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnList" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="grdBrands" EventName="RowEditing" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div>
                            <h1>
                                Brand
                            </h1>
                            <h2>
                                Master</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="padding-top: 10px;">
                            <div id="dialogAdd">
                                <asp:Panel ID="pnlAddDetail" runat="Server" Width="100%" HorizontalAlign="Center">
                                    <table width="100%" border="0" cellspacing="1" cellpadding="9">
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:Label ID="lblBrand" runat="server" Text="Brand Name"></asp:Label>
                                                <span class="mandatory">*</span>
                                            </td>
                                            <td width="5">
                                                :
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtBrandName" runat="server" MaxLength="100" Width="200px" TabIndex="1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Save & Add New"
                                                    TabIndex="2" Width="150px" OnClientClick="return Valid()"></asp:Button>&nbsp;
                                                <asp:Button ID="btnList" runat="server" Text="Go To List" OnClick="btnList_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                                    TabIndex="3"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <div id="dialogList">
                                <asp:Panel ID="pnlList" runat="server" Width="100%" HorizontalAlign="Center">
                                    <table cellspacing="0" cellpadding="0" width="95%" border="0">
                                        <tbody>
                                            <tr>
                                                <td style="vertical-align: middle; text-align: left">
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                                                </td>
                                            </tr>
                                            <tr id="norecord" runat="server">
                                                <td style="height: 10px">
                                                    <asp:Label ID="lblnorecord" runat="server" Text="No Record Found" Font-Bold="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="left">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:GridView ID="grdBrands" runat="server" OnRowEditing="grdBrands_RowEditing" OnPageIndexChanging="grdBrands_PageIndexChanging"
                                                                    DataKeyNames="BrandId" Width="100%" PageSize="10" AllowPaging="true"
                                                                    AutoGenerateColumns="false" AllowSorting="True" OnRowDeleting="grdBrands_RowDeleting"
                                                                    OnRowDataBound="grdBrands_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                
                                                                                <asp:Label ID="lbl2" runat="server" Text='<%#Eval("BrandId") %>' Visible="false"></asp:Label>
                                                                                <span title="Click TO Edit"><asp:ImageButton ID="imgEdit" runat="server" CommandName="Edit" ImageUrl="~/images/icon_edit.gif" /></span>
                                                                                <span title="Click To Delete"><asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif" 
                                                                                        OnClientClick="return CnfDelete()" /></span>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="Ag_Code" Visible="False"></asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Brand Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblBrandName" runat="server" Text='<% #Bind("BrandName")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
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
