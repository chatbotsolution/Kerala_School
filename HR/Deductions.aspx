<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="Deductions.aspx.cs" Inherits="HR_Deductions" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function isValid() {
            var DedDesc = document.getElementById("<%=txtDeduction.ClientID %>").value;
            var AcHead = document.getElementById("<%=drpAccHead.ClientID %>").selectedIndex;
            var EffDate = document.getElementById("<%=txtDate.ClientID %>").value;
            if (DedDesc.trim() == "") {
                alert("Enter Deduction Type");
                document.getElementById("<%=txtDeduction.ClientID %>").focus();
                return false;
            }
            else if (AcHead == 0) {
                alert("Select Account Head To Link with Deduction");
                document.getElementById("<%=drpAccHead.ClientID %>").focus();
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

        function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
            var key;
            var isCtrl = false;
            var keychar;
            var reg;

            if (window.event) {
                key = e.keyCode;
                isCtrl = window.event.ctrlKey
            }
            else if (e.which) {
                key = e.which;
                isCtrl = e.ctrlKey;
            }

            if (isNaN(key)) return true;

            keychar = String.fromCharCode(key);

            // check for backspace or delete, or if Ctrl was pressed
            if (key == 8 || isCtrl) {
                return true;
            }

            reg = /\d/;
            var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
            var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

            return isFirstN || isFirstD || reg.test(keychar);
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

        function resetAccHead() {
            document.getElementById('<%=drpAccGroup.ClientID %>').selectedIndex = 0;
            document.getElementById('<%=txtAccHead.ClientID%>').value = "";
            document.getElementById('<%=lblmsg1.ClientID%>').innerHTML = "";
        }

        function validAccHead() {

            if (document.getElementById('<%=drpAccGroup.ClientID%>').selectedIndex == 0) {
                alert("Select Account Group !");
                document.getElementById('<%=drpAccGroup.ClientID%>').focus();
                return false;
            }
            if (document.getElementById('<%=txtAccHead.ClientID%>').value.trim() == "") {
                alert("Please Enter Account Head");
                document.getElementById('<%=txtAccHead.ClientID%>').focus();
                return false;
            }
            else {
                return true;
            }
        }
        
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Deduction Master</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr id="trMsg" runat="server">
                    <td style="height: 20px;" align="center">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <fieldset>
                <legend style="color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana">
                    Add New Deduction</legend>
                <table width="100%">
                    <tr>
                        <td align="left" valign="baseline">
                            Deduction Type<font color="red">*</font>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtDeduction" runat="server" MaxLength="50" Width="200px"
                                TabIndex="2"></asp:TextBox>
                        </td>
                        <td align="left" valign="baseline">
                            Link Account Head<font color="red">*</font>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:DropDownList ID="drpAccHead" runat="server" Width="200px" TabIndex="3">
                            </asp:DropDownList>
                        </td>
                        <td align="left" valign="baseline">
                            Effective Date
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtDate" runat="server" TabIndex="3" Width="80px"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpDate" runat="server" Control="txtDate" AutoPostBack="False"
                                ShowMessageBox="True" TextMessage="Enter date" Format="dd mmm yyyy"></rjs:PopCalendar>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Employee Share
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtEmpShare" runat="server" Width="100px" TabIndex="4" onkeypress="return blockNonNumbers(this, event, true, false);"
                                onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                Text="0"></asp:TextBox>
                        </td>
                        <td align="left" valign="baseline">
                            Employer Share
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:TextBox ID="txtOrgShare" runat="server" Width="100px" TabIndex="5" onkeypress="return blockNonNumbers(this, event, true, false);"
                                onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                Text="0"></asp:TextBox>
                            <asp:DropDownList ID="rdbtnPerAmt" runat="server" RepeatDirection="Horizontal" 
                                TabIndex="6">
                                <asp:ListItem Text="Amount" Value="A" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Percentage" Value="P"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="left" valign="baseline" colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return isValid();"
                                TabIndex="7" onfocus="active(this);" onblur="inactive(this);" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" TabIndex="8"
                                onfocus="active(this);" onblur="inactive(this);" />
                            <asp:Button ID="btnCreateNewHead" runat="server" Text="Create AC Head" OnClientClick="resetAccHead();"
                                TabIndex="9" onfocus="active(this);" onblur="inactive(this);" />
                            <asp:HiddenField ID="hfDedId" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr>
                    <td colspan="6" style="background-color:Yellow; font-weight:bold; width:100%;">
                    While Adding New Deduction Type. Please do not add for "LOSSS OF PAY","Misc Recovery". And whatever Deduction type available in the list try to modify instead of adding new one.Like EPF,GSLI,ESI,AATF etc.
                    </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlAddAccHead" runat="server" Style="display: none">
                    <table align="center" style="background-color: #CFE9FF; border: 1px solid #135e8a;
                        padding: 15px;" width="400px">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblmsg1" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="100px">
                                Account Group&nbsp;:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpAccGroup" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Account Head&nbsp;:
                            </td>
                            <td>
                                <asp:TextBox ID="txtAccHead" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" Width="70px" OnClientClick="return validAccHead();"
                                    OnClick="btnAdd_Click" onfocus="active(this);" onblur="inactive(this);" />
                                &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="70px" onfocus="active(this);"
                                    onblur="inactive(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                While creating Account Head Select Account Group as "CURRENT LIABILITIES & PROVISIONS"
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <fieldset>
                <legend style="color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana">
                    List Of Deduction</legend>
                <table width="100%">
                    <tr>
                        <td align="left">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete Selected Records" OnClick="btnDelete_Click"
                                OnClientClick="return valid();" TabIndex="10" onfocus="active(this);" onblur="inactive(this);" />
                        </td>
                        <td align="right">
                            <asp:Label ID="lblRecCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grdDeduction" runat="server" AutoGenerateColumns="False" Width="100%"
                                EmptyDataText="No Record">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                        <ItemTemplate>
                                            <input name="Checkb" type="checkbox" value='<%#Eval("DedTypeId")%>' />
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("DedTypeId")%>' />
                                            <asp:HiddenField ID="hfAccHeadId" runat="server" Value='<%#Eval("AcctsHeadId")%>' />
                                            <asp:HiddenField ID="hfType" runat="server" Value='<%#Eval("Ded_Type")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%#Eval("DedTypeId")%>'
                                                AlternateText="Edit" ImageUrl="~/images/icon_edit.gif" ToolTip="Click to Edit"
                                                OnClick="btnEdit_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="20px" />
                                        <HeaderStyle HorizontalAlign="center" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDedDesc" runat="server" Text=' <%#Eval("DedDetails")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Head">
                                        <ItemTemplate>
                                            <%#Eval("AcctsHead")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Percentage/Amount">
                                        <ItemTemplate>
                                            <%#Eval("strPerAmt")%>
                                            <asp:HiddenField ID="hfPerAmt" runat="server" Value='<%#Eval("PerAmt")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Employee Share">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpShare" runat="server" Text=' <%#Eval("EmpValue")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Employer Share">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrgShare" runat="server" Text=' <%#Eval("OrgValue")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective Date">
                                        <ItemTemplate>
                                            <%#Eval("strStartDate")%>
                                            <asp:HiddenField ID="hfStartDt" runat="server" Value='<%#Eval("StartDate")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <ajaxToolkit:ModalPopupExtender ID="modPopUpNewAcHead" runat="server" TargetControlID="btnCreateNewHead"
                BackgroundCssClass="modalBackground" PopupControlID="pnlAddAccHead" CancelControlID="btnCancel" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


