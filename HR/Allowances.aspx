<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="Allowances.aspx.cs" Inherits="HR_Allowances" %>


<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function isValid() {
            var Allowance = document.getElementById("<%=txtAllowance.ClientID %>").value;
            var EffDate = document.getElementById("<%=txtDate.ClientID %>").value;
            if (Allowance.trim() == "") {
                alert("Enter Allowance Name");
                document.getElementById("<%=txtAllowance.ClientID %>").focus();
                return false;
            }
            else if (EffDate.trim() == "") {
                alert("Enter Effective Date");
                document.getElementById("<%=txtDate.ClientID %>").focus();
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
            if (flag == true) {
                return CnfDelete();
            }
            else {
                alert("Select any Record");
                return false;
            }
        }
        function CnfDelete() {

            if (confirm("You are going to delete selected Record(s). Do you want to continue ?")) {

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
            Allowance Master</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="90%">
                <tr id="trMsg" runat="server">
                    <td style="height: 20px;" align="center" width="100%">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <fieldset style="width: 90%">
                <legend style="color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana">
                    Add New Allowance</legend>
                <table width="100%" align="right">
                    <tr>
                        <td align="left" valign="baseline">
                            Allowance Name<font color="red">*</font>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtAllowance" runat="server" MaxLength="50" Width="200px"
                                TabIndex="1"></asp:TextBox>
                        </td>
                        <td align="right" valign="baseline">
                            <asp:DropDownList ID="drpPerAmt" runat="server" RepeatDirection="Horizontal"
                                TabIndex="2">
                                <asp:ListItem Text="Amount" Value="A" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Percentage" Value="P"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtValue" runat="server" MaxLength="5" Width="50px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                Text="0" TabIndex="3"></asp:TextBox>
                        </td>
                        <td align="right" width="10%" valign="baseline">
                            Effective Date
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtDate" runat="server" TabIndex="4" Width="80px"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpDate" runat="server" Control="txtDate" AutoPostBack="False"
                                ShowMessageBox="True" TextMessage="Enter date" Format="dd mmm yyyy"></rjs:PopCalendar>
                        </td>
                        <td align="left" valign="baseline">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return isValid();"
                                TabIndex="5" onfocus="active(this);" onblur="inactive(this);" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" TabIndex="6"
                                onfocus="active(this);" onblur="inactive(this);" />
                            <asp:HiddenField ID="hfAllowanceId" runat="server" Value="0" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <fieldset style="width: 90%">
                <legend style="color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana">
                    List Of Allowance</legend>
                <table width="100%">
                    <tr>
                        <td align="left">
                            &nbsp;<asp:Button ID="btnDelete" runat="server" Text="Delete Selected Records" OnClick="btnDelete_Click"
                                Visible="False" OnClientClick="return valid();" onfocus="active(this);" onblur="inactive(this);"
                                TabIndex="6" />
                        </td>
                        <td align="right">
                            <asp:Label ID="lblRecCount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:GridView ID="grdAllowance" runat="server" AutoGenerateColumns="False" Width="100%"
                                EmptyDataText="No Record">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                        <ItemTemplate>
                                            <input name="Checkb" type="checkbox" value='<%#Eval("AllowanceId")%>' />
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("AllowanceId")%>' />
                                            <asp:HiddenField ID="hfType" runat="server" Value='<%#Eval("AllowanceType")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%#Eval("AllowanceId")%>'
                                                AlternateText="Edit" ImageUrl="~/images/icon_edit.gif" ToolTip="Click to Edit"
                                                OnClick="btnEdit_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="20px" />
                                        <HeaderStyle HorizontalAlign="center" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Allowance">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAllowance" runat="server" Text=' <%#Eval("Allowance")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Percentage/Amount">
                                        <ItemTemplate>
                                            <%#Eval("strPerAmt")%>
                                            <asp:HiddenField ID="hfPerAmt" runat="server" Value='<%#Eval("PerAmt")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lblValue" runat="server" Text=' <%#Eval("Value")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective Date">
                                        <ItemTemplate>
                                            <%#Eval("strStartDate")%>
                                            <asp:HiddenField ID="hfEffectiveDt" runat="server" Value='<%#Eval("StartDate")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
