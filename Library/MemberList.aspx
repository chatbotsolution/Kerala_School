<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="MemberList.aspx.cs" Inherits="Library_MemberList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">

        function ToggleAll(e) {
            if (e.checked) {
                CheckAll();
            }
            else {
                ClearAll();
            }
        }

        function CheckAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = true;
                }
            }
            ml.toggleAll.checked = true;
        }

        function ClearAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = false;
                }
            }
            ml.toggleAll.checked = false;
        }
        function valid() {
            var flag;
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {

                    if (e.checked) {
                        flag = true;
                        break;
                    }
                    else
                        flag = false;
                }
            }
            //alert(flag);
            if (flag == true)
                return true;
            else {
                alert("Please select any record");
                return false;
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

    <asp:UpdatePanel ID="uppSubList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Member List</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <div style="background-color: #666; padding: 1px; margin: 0 auto;">
                <div style="background-color: #FFF; padding: 10px;">
                    <table border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                        <tr>
                            <td>
                                Member Name :
                                <asp:TextBox ID="txtName" runat="server" MaxLength="50" CssClass="largetb"></asp:TextBox>
                                &nbsp;&nbsp; Member Id :
                                <asp:TextBox ID="txtEmpId" runat="server" MaxLength="20" CssClass="smalltb"></asp:TextBox>
                                &nbsp;&nbsp;
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />

                                 <asp:Button ID="btnImport" runat="server" Text="Import Member" OnClick="btnImport_Click"  Visible="false"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="background-color: #666; padding: 1px; margin: 0 auto; margin-top: 15px;">
                <div style="background-color: #FFF; padding: 10px;">
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tbltxt">
                        <tr>
                            <td align="left">
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />&nbsp;
                                <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                            </td>
                            <td align="right">
                                <asp:Label ID="lblRecords" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="grdMemberList" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                    PageSize="15" Width="100%" OnPageIndexChanging="grdMemberList_PageIndexChanging"
                                    CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <input type="checkbox" name="Checkb" value='<%#Eval("MemberId")%>' />
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Left" Width="20px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderTemplate>
                                                <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <a href='MemberEntry.aspx?MemberId=<%#Eval("MemberId")%>'>Edit </a>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Left" Width="20px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Member Name">
                                            <ItemTemplate>
                                                <%#Eval("MemberName")%>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Left" Width="80px" />
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID Number">
                                            <ItemTemplate>
                                                <%#Eval("EmpNo")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                            <FooterStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Phone No">
                                            <ItemTemplate>
                                                <%#Eval("Phone")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                            <FooterStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Member Type">
                                            <ItemTemplate>
                                                <%#Eval("MemberType")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                            <FooterStyle Width="100px" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Record
                                    </EmptyDataTemplate>
                                    <FooterStyle BackColor="#5e5e5e" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                                    <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

