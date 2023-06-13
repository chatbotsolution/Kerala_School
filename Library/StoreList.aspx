<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="StoreList.aspx.cs" Inherits="Library_StoreList" %>

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
                    Store List</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tbltxt">
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
                        <asp:GridView ID="grdStoreList" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            PageSize="15" OnPageIndexChanging="grdStoreList_PageIndexChanging" CssClass="mGrid"
                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input type="checkbox" name="Checkb" value='<%# Eval("StoreId") %>' />
                                    </ItemTemplate>                                    
                                    <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20px" />                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <a href='LibStoreEntry.aspx?Str=<%#Eval("StoreId")%>& title=Edit Category'>Edit</a>
                                    </ItemTemplate>                                    
                                    <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                    <ItemStyle HorizontalAlign="Left" Width="20px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Store Name">
                                    <ItemTemplate>
                                        <%#Eval("StoreName")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="250px" />                                    
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="250px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Store Location">
                                    <ItemTemplate>
                                        <%#Eval("StoreLocation")%>
                                    </ItemTemplate>                                    
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Record
                            </EmptyDataTemplate>                            
                            <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                            <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

