<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="MiscRecoveryList.aspx.cs" Inherits="HR_MiscRecoveryList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
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

            if (confirm("You are going to delete a record. Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }  
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Miscellaneous Recovery List</h2>
    </div>
    <div class="spacer">
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="98%">
                <tr id="trMsg" runat="server">
                    <td style="height: 20px;" align="center">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <%-- From Date&nbsp;:&nbsp;
                        <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" Width="80px" TabIndex="1"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
                        </rjs:PopCalendar>
                        &nbsp;To Date&nbsp;:&nbsp;
                        <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" Width="80px" TabIndex="2"></asp:TextBox>
                        <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" Format="dd mmm yyyy">
                        </rjs:PopCalendar>
                        &nbsp;
                        <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" onfocus="active(this);"
                            onblur="inactive(this);" />&nbsp;--%>
                        <asp:Button ID="btnNew" runat="server" Text="Miscellaneous Recovery" onfocus="active(this);"
                            onblur="inactive(this);" OnClick="btnNew_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblRecord" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:GridView ID="grdMiscRec" runat="server" DataKeyNames="PR_Id" Width="100%" PageSize="20"
                            AllowPaging="true" AutoGenerateColumns="false" OnRowCommand="grdMiscRec_RowCommand"
                            EmptyDataText="No Record" 
                            onpageindexchanging="grdMiscRec_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Cancel">
                                    <ItemTemplate>
                                        <center>
                                            <asp:HiddenField ID="hfPRId" runat="server" Value='<%#Eval("PR_Id") %>' />
                                            <asp:HiddenField ID="hfMiscId" runat="server" Value='<%#Eval("MiscId") %>' />
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="CM" ImageUrl="~/images/icon_delete.gif"
                                                OnClientClick="return CnfDelete()" CommandArgument='<%#Eval("MiscId") %>' /></center>
                                    </ItemTemplate>
                                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee Name">
                                    <ItemTemplate>
                                        <%#Eval("EmpName")%>
                                        <asp:HiddenField ID="hfEmpId" runat="server" Value='<%#Eval("EmpId")%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Recovery Type">
                                    <ItemTemplate>
                                        <%#Eval("RecType")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reason">
                                    <ItemTemplate>
                                        <%#Eval("RecReason")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <%#Eval("RecAmt")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Recovery Mode">
                                    <ItemTemplate>
                                        <%#Eval("RecMode")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <%#Eval("RecStatus")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <img src="../images/mask.gif" height="8" width="10" />
    <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
        PopupControlID="pnlloading" BackgroundCssClass="Background" />
    <asp:Panel ID="pnlloading" runat="server" Style="display: none">
        <div align="center" style="margin-top: 13px;">
            <img src="../images/loading.gif" />
            <span>Loading...</span>
        </div>
    </asp:Panel>
</asp:Content>

