<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="SetSalaryStatus.aspx.cs" Inherits="HR_SetSalaryStatus" %>

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
                alert("Select any Record");
                return false;
            }
        }
        function CnfWithHeld() {

            if (confirm("You are going to withheld salary for the selected month. Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }
        function CnfRevertWithHeld() {

            if (confirm("You are going to Revert withheld salary for the selected month. Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }

        function IsValid() {

            var year = document.getElementById("<%=drpYear.ClientID %>").selectedIndex;

            if (year == 0) {
                alert("Select a Year");
                document.getElementById("<%=drpYear.ClientID %>").focus();
                return false;
            }
        }
        
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Withheld Salary</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="90%">
                <tr>
                    <td colspan="2">
                        <div style="width: 100%; background-color: #666; padding: 2px;">
                            <div style="background-color: #FFF; padding: 10px;">
                                <strong>Salary Year&nbsp;:&nbsp;</strong><asp:DropDownList ID="drpYear" runat="server" TabIndex="1">
                                </asp:DropDownList>
                                <strong>Salary Month&nbsp;:&nbsp;</strong><asp:DropDownList ID="drpMonth" runat="server" TabIndex="2">
                                    <asp:ListItem>JAN</asp:ListItem>
                                    <asp:ListItem>FEB</asp:ListItem>
                                    <asp:ListItem>MAR</asp:ListItem>
                                    <asp:ListItem>APR</asp:ListItem>
                                    <asp:ListItem>MAY</asp:ListItem>
                                    <asp:ListItem>JUN</asp:ListItem>
                                    <asp:ListItem>JUL</asp:ListItem>
                                    <asp:ListItem>AUG</asp:ListItem>
                                    <asp:ListItem>SEP</asp:ListItem>
                                    <asp:ListItem>OCT</asp:ListItem>
                                    <asp:ListItem>NOV</asp:ListItem>
                                    <asp:ListItem>DEC</asp:ListItem>
                                </asp:DropDownList>
                                <strong>Designation&nbsp;:&nbsp;</strong><asp:DropDownList ID="drpDesignation" runat="server"
                                    OnSelectedIndexChanged="drpDesignation_SelectedIndexChanged" AutoPostBack="True" TabIndex="3">
                                </asp:DropDownList>
                                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return IsValid();"
                                    onfocus="active(this);" onblur="inactive(this);" TabIndex="4" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table width="100%">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Button ID="btnWithheld" runat="server" OnClick="btnWithheld_Click" OnClientClick="return CnfWithHeld();"
                                        Text="Withheld Pay" onfocus="active(this);" onblur="inactive(this);" TabIndex="4" />
                                    <asp:Button ID="btnCancelWithheld" runat="server" OnClientClick="return CnfRevertWithHeld();"
                                        Text="Cancel Withheld" OnClick="btnCancelWithheld_Click" Visible="false" onfocus="active(this);"
                                        onblur="inactive(this);" TabIndex="5" />
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblNoOfRec" runat="server" Style="text-align: right"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" width="100%">
                                    <asp:GridView ID="grdEmp" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                        OnPageIndexChanging="gvEmpList_PageIndexChanging" PageSize="20" Width="100%"
                                        EmptyDataText="No Record">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <input name="Checkb" type="checkbox" value='<%# Eval("EmpId") %>' />
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <input name="toggleAll" onclick="ToggleAll(this)" type="checkbox" value="ON" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Employee Name">
                                                <HeaderStyle />
                                                <ItemTemplate>
                                                    <%#Eval("EmpName")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Designation">
                                                <ItemTemplate>
                                                    <%#Eval("Designation")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Address">
                                                <ItemTemplate>
                                                    <%#Eval("EmpAddress")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contact No">
                                                <ItemTemplate>
                                                    <%#Eval("Mobile")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="D.O.B.">
                                                <ItemTemplate>
                                                    <%#Eval("EmpDOB")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="D.O.J.">
                                                <ItemTemplate>
                                                    <%#Eval("JoiningDate")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qualification">
                                                <ItemTemplate>
                                                    <%#Eval("EduQual")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Withheld">
                                                <ItemTemplate>
                                                    <%#Eval("PayWithHeld")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="drpDesignation" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
