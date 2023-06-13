<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpQual.aspx.cs" Inherits="HR_EmpQual" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function isValid() {
            var Qual = document.getElementById("<%=txtQual.ClientID%>").value;

            if (Qual == "") {
                alert("Please Enter Qualification");
                document.getElementById("<%=txtQual.ClientID%>").focus();
                return false;
            }
            else {
                return true;
            }
        }
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


    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Employee Qualification</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table>
        <tr id="Tr1" runat="server">
            <td colspan="2" align="center">
                <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
                <asp:Label runat="server" ID="Label1" ForeColor="White" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" valign="baseline">
                Qualification<font color="red">*</font>
            </td>
            <td align="left" valign="baseline">
                :&nbsp;<asp:TextBox ID="txtQual" runat="server" Height="17px" MaxLength="100" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="left">
                &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return isValid();"
                    OnClick="btnSubmit_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="btnShow" runat="server" Text="Show List" OnClick="btnShow_Click" />
            </td>
        </tr>
    </table>
    <div id="View_grd" runat="server" visible="false">
        <div style="height: 15px;">
            <hr />
        </div>
        <table style="padding-top: 10px;">
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="lblMsg2" ForeColor="White" Font-Bold="True"></asp:Label>
            </td>
        </tr>
            <tr>
                <td>
                    <div align="left">
                        <asp:TextBox ID="txtSerch" runat="server" Height="17px" MaxLength="100" Width="200px"></asp:TextBox>&nbsp;
                        <asp:Button ID="btnSearch" runat="server" Text="Search" 
                            onclick="btnSearch_Click" />
                        &nbsp;
                        <asp:Button ID="btnAddNew" runat="server" Text="Add New" OnClick="btnAddNew_Click" />&nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdQual" runat="server" Width="500px" AutoGenerateColumns="False" AllowPaging="true"
                        PageSize="15" DataKeyNames="QualId" OnPageIndexChanging="grdQual_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <input type="checkbox" name="Checkb" value='<%# Eval("QualId") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="1px" />
                                <HeaderTemplate>
                                    <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="5px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <%--<a href="DesignationList.aspx?id=<%#Eval("DesignationId") %>">Edit</a>--%>
                                    <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" onclick="btnEdit_Click" 
                                    ></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Designation">
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblCompId" Text='<%#Eval("QualName")%>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                            Font-Size="10px" />
                        <EmptyDataTemplate>
                            No Record
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    
</asp:Content>
