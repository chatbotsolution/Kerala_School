<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="DesignationList.aspx.cs" Inherits="HR_DesignationList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
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
                alert("Select any Record");
                return false;
            }
        }
        function CnfDelete() {

            if (confirm("You are going to delete selected Record(s). Do you want to continue ?")) {

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
            Designation List</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <fieldset style="width:70%">
                <table width="100%">
                    <tr id="trMsg" runat="server">
                        <td colspan="2" align="center">
                            <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Button ID="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" />&nbsp;
                            <asp:Button ID="btnDelete" runat="server" Text="Delete Selected Records" OnClick="btnDelete_Click"
                                Visible="False" OnClientClick="return CnfDelete();" />
                            <asp:Button ID="btnSortOrder" runat="server" Text="Update Sort Order" Visible="false"
                                OnClick="btnSortOrder_Click" />
                        </td>
                        <td align="right">
                            <asp:Label ID="lblRecCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grdDesignation" runat="server" AutoGenerateColumns="False" DataKeyNames="DesgId"
                                Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="15px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                        <ItemTemplate>
                                            <input name="Checkb" type="checkbox" value='<%#Eval("DesgId")%>' />
                                            <asp:HiddenField ID="hfDesgId" runat="server" Value='<%#Eval("DesgId")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <a href='Designation.aspx?dgn=<%#Eval("DesgId")%>'>Edit</a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <%#Eval("Designation")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <%#Eval("DesgType")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sort Order">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSortOrder" runat="server" Text='<%#Eval("SortOrder")%>' onkeypress="return blockNonNumbers(this, event, false, false);"
                                                Width="70px" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record(s)
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
