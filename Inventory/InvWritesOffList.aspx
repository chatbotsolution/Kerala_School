<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="InvWritesOffList.aspx.cs" Inherits="Inventory_InvWritesOffList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        $(document).ready(function () {
            document.getElementById("<%=ddlLocation.ClientID %>").focus();
        });
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

    <asp:UpdatePanel ID="uppWritesoffList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Inventory Writesoff List
                </h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px;">
                <table border="0" cellspacing="0" cellpadding="0" class="gridtxt">
                    <tr>
                        <td>
                            <asp:Label ID="lbllocation" runat="server" Text="Location :"></asp:Label>
                            <asp:DropDownList ID="ddlLocation" runat="server" Width="150px" CssClass="tbltxtbox">
                            </asp:DropDownList>
                            &nbsp;&nbsp; Category :
                            <asp:DropDownList ID="ddlCategory" Width="150px" runat="server" AutoPostBack="true"
                                CssClass="tbltxtbox" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp;&nbsp; Item :
                            <asp:DropDownList ID="ddlItem" Width="150px" runat="server" CssClass="tbltxtbox">
                                <asp:ListItem Text="--All--" Value="0" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="gridtext">
                <tr>
                    <td align="left">
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />&nbsp;
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblRecords" runat="server" CssClass="tbltxt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grdWriteOffList" runat="server" Width="100%" AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="10" DataKeyNames="WriteOffId" OnPageIndexChanging="grdWriteOffList_PageIndexChanging"
                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" CssClass="mGrid"
                            TabIndex="4">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input type="checkbox" name="Checkb" value='<%# Eval("WriteOffId") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderTemplate>
                                        <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Item Name">
                                    <ItemTemplate>
                                        <%#Eval("ItemName")%>
                                    </ItemTemplate>
                                    <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <%#Eval("Quantity")%>
                                    </ItemTemplate>
                                    <HeaderStyle Width="60px" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="WriteOff Date">
                                    <ItemTemplate>
                                        <%#Eval("writeoffDt")%>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <%#Eval("Description")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle BackColor="#EFEFEF" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="#FDFDFD" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
