<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="AccountHead.aspx.cs" Inherits="Accounts_AccountHead" %>

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

            if (confirm("You are going to delete record(s). Do you want to continue?")) {
                return true;
            }
            else {
                return false;
            }
        }

        function isValid() {
            var AccHead = document.getElementById("<%=txtAccHead.ClientID %>").value;
            var AccType = document.getElementById("<%=drpAccType.ClientID %>").value;

            if (AccHead.trim() == "") {
                alert("Please enter Account Head !");
                return false;
            }

            if (AccType == "0") {
                alert("Please select Account Type !");
                return false;
            }
            else {
                return true;
            }
        }
   
   
    </script>

    <div class="bedcromb">
        <asp:Label ID="lblTitle" runat="server" Text="Account Head"></asp:Label></div>
    <div style="padding: 30px;" align="center">
        <table style="width: 100%" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top" align="center">
                    <asp:Panel ID="pnl1" runat="server">
                        <div style="width: 550px; background-color: #666; padding: 2px; margin: 0 auto;">
                            <div style="background-color: #FFF; padding: 10px;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="left">
                                            Account Head&nbsp;:&nbsp;<asp:TextBox ID="txtAccHead" runat="server"></asp:TextBox>
                                            Account Type&nbsp;:&nbsp;<asp:DropDownList ID="drpAccType" runat="server" Width="80px">
                                                <asp:ListItem Value="Cr">Credit</asp:ListItem>
                                                <asp:ListItem Value="Dr">Debit</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" OnClientClick="return isValid();"
                                                Text="Submit" />&nbsp;
                                            <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" />&nbsp;
                                            <asp:Button ID="btnShow" Text="Show List" runat="server" OnClick="btnShow_Click" />
                                        </td>
                                    </tr>
                                    <tr id="trMsg" runat="server">
                                        <td align="center">
                                            <asp:Label ID="lblMsgSucc" runat="server" Font-Bold="True" Font-Italic="True" ForeColor="White"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div style="height: 20px;">
                        </div>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td valign="top" align="center">
                    <asp:Panel ID="Pnl2" runat="server">
                        <div style="width: 550px; background-color: #666; padding: 2px; margin: 0 auto;">
                            <div style="background-color: #FFF; padding: 10px;">
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left" style="padding-right: 7px;">
                                            Account Type&nbsp;:&nbsp;<asp:DropDownList ID="drpAccountType" runat="server"
                                                Width="80px" AutoPostBack="True" 
                                                onselectedindexchanged="drpAccountType_SelectedIndexChanged">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                                <asp:ListItem Value="Cr">Credit</asp:ListItem>
                                                <asp:ListItem Value="Dr">Debit</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                                            <asp:Button ID="btnDelete" runat="server" OnClientClick="CnfDelete();" OnClick="btnDelete_Click"
                                                Text="Delete" />
                                        </td>
                                        <td align="right" style="padding-right: 7px;">
                                            <asp:Label ID="lblRecCount" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" style="padding-left: 5px;">
                                            <asp:GridView ID="gidAccHead" runat="server" Width="100%" AutoGenerateColumns="False"
                                                AllowPaging="True" PageSize="25" OnPageIndexChanging="gidAccHead_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <input type="checkbox" name="Checkb" value='<%# Eval("AccountHeadId") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="23" />
                                                        <HeaderTemplate>
                                                            <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                                        </HeaderTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <a href='AccountHead.aspx?em=<%#Eval("AccountHeadId")%>'>Edit</a>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Account Head">
                                                        <ItemTemplate>
                                                            <%#Eval("AccountHeadDetail")%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Account Type">
                                                        <HeaderStyle Width="100px" />
                                                        <ItemTemplate>
                                                            <%#Eval("AccountTypeCrDr")%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    No Record
                                                </EmptyDataTemplate>
                                                <EmptyDataRowStyle HorizontalAlign="Center" Font-Bold="true" />
                                                <FooterStyle BackColor="#5e5e5e" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                                                <HeaderStyle CssClass="datalisttopbar" />
                                                <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                                                <AlternatingRowStyle CssClass="datalistalternaterow" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

