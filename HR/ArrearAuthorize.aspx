<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="ArrearAuthorize.aspx.cs" Inherits="HR_ArrearAuthorize" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<script language="javascript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function beginRequest(sender, args) {
            // show the popup
            $find('<%=mdlloading.ClientID %>').show();

        }

        function endRequest(sender, args) {
            //  hide the popup
            $find('<%=mdlloading.ClientID %>').hide();

        }
        
    </script>--%>

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
        function valApply() {
            var desc = document.getElementById("<%=txtArrearDesc.ClientID %>").value;
            var amt = document.getElementById("<%=txtAmount.ClientID %>").value;

            if (desc.trim() == "") {
                alert("Enter Description");
                document.getElementById("<%=txtArrearDesc.ClientID %>").focus();
                return false;
            }
            if (amt.trim() == "" || parseFloat(amt) == 0) {
                alert("Enter Amount");
                document.getElementById("<%=txtAmount.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
        function valSave() {
            var date = document.getElementById("<%=txtDate.ClientID %>").value;
            if (date.trim() == "") {
                alert("Enter Authorized Date");
                document.getElementById("<%=txtDate.ClientID %>").focus();
                return false;
            }
            else {
                return CnfSave();
            }
        }
        function CnfSave() {

            if (confirm("Are you sure to save ?\nNote :- Please verify the details before save.")) {

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
            Authorize Interim Payment</h2>
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
                            <td align="left" valign="baseline" width="120px">
                                Designation
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:DropDownList ID="drpDesignation" runat="server" TabIndex="1" AutoPostBack="true"
                                    OnSelectedIndexChanged="drpDesignation_SelectedIndexChanged">
                                </asp:DropDownList>
                                Employee Name&nbsp;:&nbsp;<asp:DropDownList ID="drpEmpName" runat="server" TabIndex="2"
                                    AutoPostBack="True" OnSelectedIndexChanged="drpEmpName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="3" OnClick="btnSearch_Click" Width="70px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline">
                                Description<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtArrearDesc" runat="server" Width="400px" TabIndex="4"
                                    MaxLength="100"></asp:TextBox>
                                Amount<font color="red">*</font>&nbsp;:&nbsp;<asp:TextBox ID="txtAmount" runat="server"
                                    Width="100px" TabIndex="5" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                                <asp:Button ID="btnApplyToAll" runat="server" Text="Apply to All" onfocus="active(this);"
                                    onblur="inactive(this);" TabIndex="6" Width="100px" Enabled="false" OnClientClick="return valApply();"
                                    OnClick="btnApplyToAll_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline">
                                Authorized Date<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtDate" runat="server" Width="80px" TabIndex="8"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpDate" runat="server" Control="txtDate"></rjs:PopCalendar>
                                &nbsp;
                                <asp:DropDownList ID="drpPmtMode" runat="server" TabIndex="9" AutoPostBack="true"
                                    OnSelectedIndexChanged="drpPmtMode_SelectedIndexChanged">
                                    <asp:ListItem Text="Pay in Next Salary" Value="S" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Pay Instantly" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline">
                                Payment Mode
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:RadioButtonList ID="rbPmtMode" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" TabIndex="7" OnSelectedIndexChanged="rbPmtMode_SelectedIndexChanged"
                                    AutoPostBack="true" BorderStyle="Solid" BorderWidth="1px">
                                    <asp:ListItem Text="Cash" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Bank" Value="B" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                                &nbsp;<b>||</b> Bill No&nbsp;:&nbsp;<asp:TextBox ID="txtBillNo" Width="100px" TabIndex="8"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline">
                                Payment Date
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtPaymentDate" runat="server" Width="80px" TabIndex="9"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpPaymentDt" runat="server" Control="txtPaymentDate"></rjs:PopCalendar>
                                &nbsp;<b>||</b> Bank Name&nbsp;:
                                <asp:DropDownList ID="drpBank" runat="server" TabIndex="10">
                                </asp:DropDownList>
                                &nbsp;<b>||</b> Instrument No&nbsp;:&nbsp;<asp:TextBox ID="txtInstrNo" TabIndex="11"
                                    runat="server" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSave" runat="server" Text="Save" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="10" Width="100px" Enabled="false" OnClick="btnSave_Click" OnClientClick="return valSave();" />
                                <%--<asp:Button ID="btnDelete" runat="server" Text="Delete" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="11" Width="100px" Enabled="False" OnClick="btnDelete_Click" />--%>
                                <asp:Button ID="btnList" runat="server" Text="Go to List" onfocus="active(this);"
                                    onblur="inactive(this);" TabIndex="11" Width="100px" OnClick="btnList_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="12" Width="100px" OnClick="btnClear_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="grdEmp" runat="server" AutoGenerateColumns="False" Width="100%">
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
                                                <asp:TextBox ID="txtDesc" runat="server" MaxLength="100" TabIndex="12" Width="400px"
                                                    Text='<%#Eval("ArrearDesc")%>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="400px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAmt" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                    TabIndex="12" Text='<%#Eval("Amt")%>' Width="80px"></asp:TextBox>
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
            <%-- <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />--%>
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


