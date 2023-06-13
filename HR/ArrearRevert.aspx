<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="ArrearRevert.aspx.cs" Inherits="HR_ArrearRevert" %>

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
        function valSearch() {
            var month = document.getElementById("<%=drpMonth.ClientID %>").value;
            var year = document.getElementById("<%=drpYear.ClientID %>").value;
            if (month.trim() == "0") {
                alert("Select a Month");
                document.getElementById("<%=drpMonth.ClientID %>").focus();
                return false;
            }
            if (year.trim() == "0") {
                alert("Select a Year");
                document.getElementById("<%=drpYear.ClientID %>").focus();
                return false;
            }
        }
        function CnfRevert() {

            if (confirm("Are you sure to Revert the Arrear ?\nNote :- This transaction can not be reverted back.")) {

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
            Revert Arrear</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%; float: left;" align="left">
                <fieldset>
                    <table width="100%" cellpadding="2" cellspacing="2">
                        <tr id="trMsg" runat="server">
                            <td style="height: 20px;" align="center">
                                <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline">
                                Employee Name&nbsp;:&nbsp;<asp:DropDownList ID="drpEmpName" runat="server" TabIndex="1">
                                </asp:DropDownList>
                                &nbsp;&nbsp;Month&nbsp;:&nbsp;<asp:DropDownList ID="drpMonth" runat="server" TabIndex="2">
                                    <asp:ListItem Selected="True" Text="- SELECT -" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Januanry" Value="JAN"></asp:ListItem>
                                    <asp:ListItem Text="February" Value="FEB"></asp:ListItem>
                                    <asp:ListItem Text="March" Value="MAR"></asp:ListItem>
                                    <asp:ListItem Text="April" Value="APR"></asp:ListItem>
                                    <asp:ListItem Text="May" Value="MAY"></asp:ListItem>
                                    <asp:ListItem Text="June" Value="JUN"></asp:ListItem>
                                    <asp:ListItem Text="July" Value="JUL"></asp:ListItem>
                                    <asp:ListItem Text="August" Value="AUG"></asp:ListItem>
                                    <asp:ListItem Text="September" Value="SEP"></asp:ListItem>
                                    <asp:ListItem Text="October" Value="OCT"></asp:ListItem>
                                    <asp:ListItem Text="November" Value="NOV"></asp:ListItem>
                                    <asp:ListItem Text="December" Value="DEC"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;&nbsp;Year&nbsp;:&nbsp;<asp:DropDownList ID="drpYear" runat="server" TabIndex="3">
                                </asp:DropDownList>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="4" OnClick="btnSearch_Click" Width="70px" OnClientClick="return valSearch();" />
                                <asp:Button ID="btnRevert" runat="server" Text="Revert" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="5" Width="70px" Enabled="false" OnClick="btnRevert_Click" OnClientClick="return CnfRevert();" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="grdEmp" runat="server" AutoGenerateColumns="False" Width="100%">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="30px" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <span style="display: <%#Eval("chk") %>">
                                                    <input name="Checkb" type="checkbox" value='<%# Eval("AllwId") %>' /></span>
                                                <asp:HiddenField ID="hfEmpId" runat="server" Value='<%# Eval("EmpId") %>' />
                                                <asp:HiddenField ID="hfAllwId" runat="server" Value='<%# Eval("AllwId") %>' />
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
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <%#Eval("Designation")%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Arrear Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesc" runat="server" MaxLength="100" Width="400px" Text='<%#Eval("ArrearDesc")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="400px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmt" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                    Text='<%#Eval("Amount")%>' Width="80px"></asp:Label>
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
