<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="ArrearList.aspx.cs" Inherits="HR_ArrearList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
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

            if (confirm("Are you sure to Delete ths selected records ?")) {

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
            Authorized Interim Payment List</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%; float: left;" align="left">
                <fieldset>
                    <table width="100%" cellpadding="2" cellspacing="2">
                        <tr id="trMsg" runat="server">
                            <td colspan="2" style="height: 20px;" align="center">
                                <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline" width="100px">
                                Designation
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:DropDownList ID="drpDesignation" runat="server" TabIndex="1" AutoPostBack="true"
                                    OnSelectedIndexChanged="drpDesignation_SelectedIndexChanged">
                                </asp:DropDownList>
                                Employee Name&nbsp;:&nbsp;<asp:DropDownList ID="drpEmpName" runat="server" TabIndex="2"
                                    AutoPostBack="True" OnSelectedIndexChanged="drpEmpName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline">
                                Description
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:DropDownList ID="drpDesc" runat="server" TabIndex="3" AutoPostBack="True"
                                    Width="435px">
                                </asp:DropDownList>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="4" OnClick="btnSearch_Click" Width="100px" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="5" Width="100px" Enabled="False" OnClick="btnDelete_Click" OnClientClick="return CnfDelete();" />
                                <asp:Button ID="btnNew" runat="server" Text="Authorize Interim Payment" 
                                    onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="6" onclick="btnNew_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="grdList" runat="server" AutoGenerateColumns="False" Width="100%"
                                    EmptyDataText="No Records">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="30px" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <input name="Checkb" type="checkbox" value='<%# Eval("EmpId") %>' />
                                                <asp:HiddenField ID="hfEmpId" runat="server" Value='<%# Eval("EmpId") %>' />
                                                <asp:HiddenField ID="hfClaimId" runat="server" Value='<%# Eval("ClaimId") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderTemplate>
                                                <input name="toggleAll" onclick="ToggleAll(this)" type="checkbox" value="ON" />
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Name">
                                            <ItemTemplate>
                                                <%#Eval("EmpName")%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <%#Eval("Designation")%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="txtDesc" runat="server" MaxLength="100" Width="400px"
                                                    Text='<%#Eval("ArrearDesc")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="400px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="txtAmt" runat="server" Text='<%#Eval("Amt")%>' Width="80px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                            <HeaderStyle HorizontalAlign="Right" Width="100px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

