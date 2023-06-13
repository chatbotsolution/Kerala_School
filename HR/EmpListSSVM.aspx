<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpListSSVM.aspx.cs" Inherits="HR_EmpListSSVM" %>

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
                alert("Select any Record");
                return false;
            }
        }
        function CnfDelete() {

            if (confirm("You are going to delete the Employee Record Completely. Do you want to Continue ?")) {

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
            Employee Details</h2>
        <div style="float: right;">
            <asp:Label ID="lblMsgTop" Font-Bold="true" ForeColor="Green" runat="server" Text=""
                CssClass="tbltxt"></asp:Label>
        </div>
    </div>
    <div align="left">
        <div class="innerdiv" style="width: 99%">
            <div style="padding: 8px;">
                <asp:UpdatePanel ID="updtPnl" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 100%;" cellspacing="0" cellpadding="0">
                            <tr style="background-color: #D3E7EE;">
                                <td align="left" class="td-bottom-left" colspan="2">
                                    Designation&nbsp;:&nbsp;<asp:DropDownList ID="drpDesig" runat="server" CssClass="tbltxtbox_mid"
                                        TabIndex="2">
                                    </asp:DropDownList>
                                    &nbsp;Employee Name&nbsp;:&nbsp;<asp:TextBox ID="txtEmp" runat="server" Width="200px"
                                        AutoPostBack="True" BackColor="White" CssClass="tbltxtbox" TabIndex="3"></asp:TextBox>&nbsp;
                                           Service&nbsp;:&nbsp;<asp:DropDownList ID="drpStatus" runat="server" CssClass="tbltxtbox_mid"
                                        TabIndex="2">
                                        <asp:ListItem Text="--All--" Value="--All--"></asp:ListItem>
                                        <asp:ListItem Text="In Service" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Out Of Service" Value="0"></asp:ListItem>
                                    </asp:DropDownList>&nbsp;
                                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" Width="65px"
                                        TabIndex="4" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="tbltxt">
                                    <asp:Button ID="btnNewDir" runat="server" Text="Add a New Employee" OnClick="btnNew_Click"
                                        TabIndex="5" />&nbsp;
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete Employee" OnClientClick="return CnfDelete();"
                                        OnClick="btnDelete_Click" TabIndex="7" />
                                </td>
                                <td align="right" class="totalrec">
                                    <asp:Label ID="lblRecCount" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="tbltxt" colspan="2">
                                    <asp:GridView ID="grdEmpDetails" runat="server" AutoGenerateColumns="False" DataKeyNames="EmpId" PageSize="20"
                                        Width="100%" AllowPaging="true" OnPageIndexChanging="grdEmpDetails_PageIndexChanging"
                                        CssClass="gridtext">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="15px" />
                                                <HeaderStyle Width="15px" />
                                                <ItemTemplate>
                                                    <input name="Checkb" type="checkbox" value='<%#Eval("EmpId")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <a class="viewdetail" href='EmpInfoDtls.aspx?empno=<%#Eval("EmpId")%>'>View Details</a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                <HeaderStyle HorizontalAlign="Center" Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Employee Name">
                                                <ItemTemplate>
                                                    <%#Eval("SevName")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Sex" DataField="Sex" HeaderStyle-HorizontalAlign="left"
                                                ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle" />
                                            <asp:TemplateField HeaderText="Designation">
                                                <ItemTemplate>
                                                    <%#Eval("Designation")%>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Address">
                                                <ItemTemplate>
                                                    <%#Eval("PresAddress")%>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contact No">
                                                <ItemTemplate>
                                                    <%#Eval("Mobile")%>
                                                    <br />
                                                    <%#Eval("Phone")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Email ID" DataField="emailid" HeaderStyle-HorizontalAlign="left"
                                                ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Records
                                        </EmptyDataTemplate>
                                        <PagerStyle CssClass="headergrid" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="headergrid" />
                                        <AlternatingRowStyle CssClass="gridtext" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnShow" />
                        <asp:PostBackTrigger ControlID="grdEmpDetails" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>


