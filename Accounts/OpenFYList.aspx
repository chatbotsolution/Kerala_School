<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="OpenFYList.aspx.cs" Inherits="Accounts_OpenFYList" %>

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
        function Cnf() {

            if (confirm("You are going to Finalize the Financial Year. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }


        function Confirm() {
            if (confirm("You are going to Finalize the Financial Year!! Please make sure that you have taken a backup of the Database If not do so immediately!!. Do you want to continue?")) {
                var x = confirm("Do you Want to Re-Initialization the Financial Year ??");

                var control = '<%=inpHide.ClientID%>';
                if (x == true) {
                    document.getElementById(control).value = "1";
                }
                else {
                    document.getElementById(control).value = "0";
                }
            }
            else {
                return false;
            }
        }
    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td align="left" valign="middle" height="35">
                        <div class="headingcontainor" style="width:250px;">
                            <h1>
                                Open 
                            </h1>
                            <h2>
                                Financial Year</h2>
                        </div>
                   <%-- </td>
                    <td align="left" valign="middle">--%>
                    <div style="float:left; vertical-align:middle; padding-top:5px;">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label></div>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="padding: 5px 0px 5px 0px;">
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                        
                        <asp:Button ID="btnReInit" runat="server" Text="Reset Current Openning Balance" 
                            Visible="false" onclick="btnReInit_Click"/>
                        
                        <input id="inpHide" type="hidden" runat="server" />  
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td align="left"><span style="color:Red;font-size:14px;"><b>Make sure that you have a BACKUP of the data before doing any Transaction On this Page!!</b></span></td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdFinancialYear" runat="server" DataKeyNames="FY_Id" ToolTip="Financial Years"
                            Width="700px" PageSize="10" AllowPaging="true" AutoGenerateColumns="false" AllowSorting="false"
                            EmptyDataText="No Financial Year Exists" OnRowDeleting="grdFinancialYear_RowDeleting"
                            OnRowEditing="grdFinancialYear_RowEditing" 
                            OnRowDataBound="grdFinancialYear_RowDataBound" >
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkYearReset" runat="server" Text="Initialize Year" CommandName="edit"></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkFinalizeYear" runat="server" OnClientClick="return Confirm();"
                                            Text="Finalize Year" CommandName="delete"></asp:LinkButton>
                                        <asp:Label ID="lblStatus" runat="server" Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="160px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Financial Year">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFyr" runat="server" Text='<% #Bind("FYear")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="90px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Year Start Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblYrStDt" runat="server" Text='<% #Bind("StartDateStr")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Year End Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblYrEndDt" runat="server" Text='<% #Bind("EndDateStr")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Entry Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLstEntryDt" runat="server" Text='<% #Bind("LastEntryDate")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                               <%--  <asp:BoundField DataField="IsInitialized" HeaderText="Intialized" />--%>
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:GridView>
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
