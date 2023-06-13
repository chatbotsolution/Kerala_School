<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="SupplierList.aspx.cs" Inherits="Accounts_SupplierList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
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

    <table style="width: 100%;" cellspacing="0" cellpadding="0">
        <tr>
            <td style="width: 100%" align="center">
                <table style="width: 100%;" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="20" valign="top">
                            <div class="bedcromb">
                                Expenses &raquo; List Of Creditors</div>
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="width: 100%" align="center">
            <td>
                <div align="center">
                    <div style="width: 90%; background-color: #666; padding: 2px; margin: 0 auto;">
                        <div style="background-color: #FFF; padding: 10px;">
                            <div class="linegap">
                                <img src="../images/mask.gif" height="5" width="10"></div>
                            <table cellspacing="0" cellpadding="0" width="100%">
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Button ID="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" />&nbsp;
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete Record" OnClick="btnDelete_Click" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblRecCount" runat="server" Text="" CssClass="totalrec"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="grdSupplierMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="SupId"
                                            Width="100%" AllowPaging="True" OnPageIndexChanging="grdSupplierMaster_PageIndexChanging"
                                            CssClass="gridtext">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                    <ItemTemplate>
                                                        <input name="Checkb" type="checkbox" value='<%#Eval("SupId")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <a href='SupplierMaster.aspx?suppno=<%#Eval("SupId")%>'>Edit</a>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Creditor Name">
                                                    <ItemTemplate>
                                                        <%#Eval("SupName")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <%#Eval("Address")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Telephone No">
                                                    <ItemTemplate>
                                                        <%#Eval("ContactTel")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FAX No">
                                                    <ItemTemplate>
                                                        <%#Eval("Fax")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Email Id">
                                                    <ItemTemplate>
                                                        <%#Eval("EmailID")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="TIN No">
                                                    <ItemTemplate>
                                                        <%#Eval("TIN")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CST No">
                                                    <ItemTemplate>
                                                        <%#Eval("CST")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No Record
                                            </EmptyDataTemplate>
                                            <RowStyle CssClass="gridtext" BackColor="#EFEFEF" />
                                            <EditRowStyle CssClass="gridtext" BackColor="#2461BF" />
                                            <SelectedRowStyle CssClass="gridtext" BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle CssClass="gridtext" ForeColor="White" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="headergrid" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle CssClass="gridtext" BackColor="#FDFDFD" />
                                            <EmptyDataRowStyle CssClass="gridtext" HorizontalAlign="Center" BackColor="Gray"
                                                ForeColor="White" Font-Size="Large" Font-Bold="true" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

