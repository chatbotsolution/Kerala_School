<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="SubjectList.aspx.cs" Inherits="Library_SubjectList" %>

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

    <div class="bedcromb">
        <asp:Label ID="lblTitle" runat="server" Text="Subject List"></asp:Label></div>
    <asp:UpdatePanel ID="uppSubList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="padding: 30px;" align="center">
                <div style="width: 875px; background-color: #666; padding: 2px; margin: 0 auto;">
                    <div style="background-color: #FFF; padding: 10px;">
                        <table width="850px" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="40">
                                    Class :
                                </td>
                                <td width="130">
                                    <asp:DropDownList ID="ddlCategory" runat="server">                                        
                                    </asp:DropDownList>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="width: 875px; background-color: #666; padding: 2px; margin: 0 auto; margin-top: 15px;">
                    <div style="background-color: #FFF; padding: 10px;">
                        <table width="850" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="left">
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />&nbsp;
                                    <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                                </td>
                                <td align="right">
                                    No Of Records :&nbsp;<asp:Label ID="lblRecords" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="grdSubjectShow" runat="server" AutoGenerateColumns="False"
                                        AllowPaging="True" PageSize="15" OnPageIndexChanging="gidSubjectShow_PageIndexChanging"
                                        Width="850">
                                        <EmptyDataRowStyle HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <input type="checkbox" name="Checkb" value='<%# Eval("SubjectId") %>' />
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Left" Width="20px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20px" />
                                                <HeaderTemplate>
                                                    <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <a href='SubjectMaster.aspx?SubjectId=<%#Eval("SubjectId")%>'>
                                                        Edit</a>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Left" Width="20px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Category">
                                                <ItemTemplate>
                                                    <%#Eval("CatName")%>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Left" Width="80px" />
                                                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Subject">
                                                <ItemTemplate>
                                                    <%#Eval("SubName")%>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Left" Width="80px" />
                                                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <FooterStyle Width="150px" />
                                                <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <%#Eval("SubDescription")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Record
                                        </EmptyDataTemplate>
                                        <FooterStyle BackColor="#5e5e5e" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                                        <HeaderStyle CssClass="datalisttopbar" />
                                        <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                                        <AlternatingRowStyle CssClass="datalistalternaterow" />
                                        <AlternatingRowStyle CssClass="datalistalternaterow" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hfUserid" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

