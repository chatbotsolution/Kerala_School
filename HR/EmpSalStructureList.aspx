<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpSalStructureList.aspx.cs" Inherits="HR_EmpSalStructureList" %>

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

    <div class="bedcromb">
        <asp:Label ID="lblTitle" runat="server" Text="Employee Salary List"></asp:Label></div>
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="middle" align="center">
                <table width=>
                    <tr>
                        <td align="center" colspan="2" style="height: 60px">
                            <div style="padding: 30px;" align="center">
                                <div style="width: 850px; background-color: #666; padding: 2px; margin: 0 auto;">
                                    <div style="background-color: #FFF; padding: 10px;">
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    Dept Name :
                                                    <asp:DropDownList ID="drpDeptName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpDeptName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Employe Name :
                                                    <asp:DropDownList ID="drpEmpName" runat="server">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" />
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <div style="padding: 0px 30px 30px 30px;" align="center">
                                <div style="width: 850px; background-color: #666; padding: 2px; margin: 0 auto;">
                                    <div style="background-color: #FFF; padding: 10px;">
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                                                    <asp:Label ID="lbledit" runat="server"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblCount" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:GridView ID="grdEmployeSalary" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        AllowPaging="True" PageSize="15" DataKeyNames="SalStrId">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <input type="checkbox" name="Checkb" value='<%# Eval("SalStrId") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20px" />
                                                                <HeaderTemplate>
                                                                    <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                                                </HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <a href='EmpSalStructure.aspx?em=<%#Eval("SalStrId")%>'>Edit</a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dept Name">
                                                                <HeaderStyle Width="110px" HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("DeptName")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Emp Name">
                                                                <HeaderStyle Width="110px" HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("EmpName")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Date">
                                                                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("FromStr")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To Date">
                                                                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ToStr")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Pay">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Pay")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="GP">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <%#Eval("GP")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DA">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <%#Eval("DA")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="HR">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <%#Eval("HR")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Medicine">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Medicine")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="EPF">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <%#Eval("EPF")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="GrossTot">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <%#Eval("GrossTot")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
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
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
