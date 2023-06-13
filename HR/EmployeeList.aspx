<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmployeeList.aspx.cs" Inherits="HR_EmployeeList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
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
        <asp:Label ID="lblTitle" runat="server" Text="Employee List"></asp:Label></div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="padding: 30px;" align="center">
                    <table width="825px">
                        <tr>
                            <td style="height: 10px" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="width: 825px; background-color: #666; padding: 2px; margin: 0 auto;">
                                    <div style="background-color: #FFF; padding: 10px;">
                                        <strong>&nbsp;&nbsp;Department:</strong>&nbsp;<asp:DropDownList ID="drpDept" runat="server">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;<strong>Designation:</strong>&nbsp;<asp:DropDownList ID="drpDesignation"
                                            runat="server">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;<strong>Employee Name:</strong> &nbsp;<asp:TextBox ID="txtEmpName"
                                            runat="server" Height="17px"></asp:TextBox>&nbsp;
                                        <asp:Button ID="btnShow" runat="server" CausesValidation="False" Text="Show" OnClick="btnShow_Click" />&nbsp;
                                        &nbsp;
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <div style="width: 825px; background-color: #666; padding: 2px; margin: 0 auto;">
                                    <div style="background-color: #FFF; padding: 10px;">
                                        <table width="800px">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="btnNew" runat="server" OnClick="btnNew_Click" Text="Add New" />
                                                    &nbsp;
                                                    <asp:Button ID="btndelete" runat="server" CausesValidation="False" OnClientClick="return CnfDelete();"
                                                        OnClick="btndelete_Click" Text="Delete Selected Records" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblNoOfRec" runat="server" Style="text-align: right"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:GridView ID="grdEmp" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        AllowPaging="True" PageSize="15" OnPageIndexChanging="gvEmpList_PageIndexChanging">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <input type="checkbox" name="Checkb" value='<%# Eval("EmpId") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <HeaderTemplate>
                                                                    <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                                                </HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <a href='Employee.aspx?eno=<%#Eval("EmpId")%>'>Edit</a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Employee Name">
                                                                <HeaderStyle Width="150px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("EmpName")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Department">
                                                                <ItemTemplate>
                                                                    <%#Eval("DeptName")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Designation">
                                                                <ItemTemplate>
                                                                    <%#Eval("Designation")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Address">
                                                                <ItemTemplate>
                                                                    <%#Eval("EmpAddress")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="D.O.B.">
                                                                <HeaderStyle Width="75px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("EmpDOB")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Contact No">
                                                                <HeaderStyle Width="90px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ContactTel")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <%#Eval("Remarks")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Qualification">
                                                                <HeaderStyle Width="110px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Qualification")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle HorizontalAlign="Left" />
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
                            </td>
                        </tr>
                    </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnShow" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
</asp:Content>


