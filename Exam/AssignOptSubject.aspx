<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="AssignOptSubject.aspx.cs" Inherits="Exam_AssignOptSubject" %>

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

        function cnfSubmit() {

            if (confirm("Are you sure to continue ?")) {

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
            Assign Student wise Optional Subjects</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <fieldset style="width: 90%">
        <table width="100%">
            <tr id="trMsg" runat="server">
                <td style="height: 20px;" align="center">
                    <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" valign="baseline">
                    Session<font color="red">*</font>&nbsp;:&nbsp;<asp:DropDownList ID="drpSession" runat="server"
                        TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                    </asp:DropDownList>
                    Select a Class<font color="red">*</font>&nbsp;:&nbsp;<asp:DropDownList ID="drpClass"
                        runat="server" TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                    </asp:DropDownList>
                    Select a Section&nbsp;:&nbsp;<asp:DropDownList ID="drpSection" runat="server" TabIndex="3"
                        AutoPostBack="True" OnSelectedIndexChanged="drpSection_SelectedIndexChanged">
                    </asp:DropDownList>
                    Optional Subject<font color="red">*</font>&nbsp;:&nbsp;<asp:DropDownList ID="drpOptSub"
                        runat="server" TabIndex="4" AutoPostBack="True" OnSelectedIndexChanged="drpOptSub_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Button ID="btnAssign" runat="server" Text="Assign" Visible="False" TabIndex="5"
                        onfocus="active(this);" onblur="inactive(this);" OnClick="btnAssign_Click" OnClientClick="return cnfSubmit();" />
                    <asp:Button ID="btnUnAssign" runat="server" Text="Un-Assign" Visible="False" TabIndex="6"
                        onfocus="active(this);" onblur="inactive(this);" 
                        OnClientClick="return cnfSubmit();" onclick="btnUnAssign_Click" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblRecords" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:GridView ID="grdStudent" runat="server" AutoGenerateColumns="False" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Status">
                                <HeaderStyle HorizontalAlign="Center" Width="2%" />
                                <HeaderTemplate>
                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                <ItemTemplate>
                                    <input name="Checkb" type="checkbox" value='<%#Eval("AdmnNo")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Student Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblStudentName" runat="server" Text='<%#Eval("FullName")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="45%" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="45%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Admission No">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdmnNo" runat="server" Text='<%#Eval("AdmnNo")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="35%" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="35%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assigned/Not Assigned">
                                <ItemTemplate>
                                    <asp:Label ID="lblIsAssigned" runat="server" Text='<%#Eval("IsAssigned")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="18%" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="18%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </fieldset>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

