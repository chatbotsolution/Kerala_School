<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveMaster.aspx.cs" Inherits="HR_LeaveMaster" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function isValid() {
            var leaveCode = document.getElementById("<%=txtLeaveCode.ClientID %>").value;
            var leaveDesc = document.getElementById("<%=txtLeaveDesc.ClientID %>").value;

            if (leaveCode.trim() == "") {
                alert("Enter Leave Code");
                document.getElementById("<%=txtLeaveCode.ClientID %>").focus();
                return false;
            }
            else if (leaveDesc.trim() == "") {
                alert("Enter Leave Description");
                document.getElementById("<%=txtLeaveDesc.ClientID %>").focus();
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
                CnfDelete();
                return true;
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
            Leave Master</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="93%">
                <tr id="trMsg" runat="server">
                    <td colspan="2" style="height: 20px;" align="center">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <fieldset style="width: 90%">
                <legend style="color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana">
                    Add New Leave</legend>
                <table width="100%">
                    <tr>
                       
                        <td align="left" valign="baseline">
                            Leave Code<font color="red">*</font>:&nbsp;<br />
                            <asp:TextBox ID="txtLeaveCode" runat="server" MaxLength="4" Width="70px" 
                                TabIndex="1"></asp:TextBox>
                        </td>
                      
                        <td align="left" >
                              Leave Description<font color="red">*</font>:&nbsp;<br />
                              <asp:TextBox ID="txtLeaveDesc" runat="server" MaxLength="50" Width="200px"
                                TabIndex="2"></asp:TextBox>
                        </td>
                        <td align="right" valign="baseline">
                            Salary Deduction<font color="red">*</font>
                        </td>
                        <td align="left" valign="baseline">
                            :&nbsp;<asp:DropDownList ID="drpSalDed" runat="server" TabIndex="3">
                                <asp:ListItem Text="Yes" Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right" valign="baseline">
                            <asp:CheckBox ID="chkCF" runat="server" Text="Carry Forward Allowed" TabIndex="4"
                                TextAlign="Left" />
                        </td>
                        <td align="left" valign="baseline">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return isValid();"
                                TabIndex="5" onfocus="active(this);" onblur="inactive(this);" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" TabIndex="6"
                                onfocus="active(this);" onblur="inactive(this);" />
                            <asp:HiddenField ID="hfLeaveId" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" valign="baseline" style="color: Red; font-weight: bold">
                            # Leave Code Example - ML for Medical Leave,&nbsp;CL for Casual Leave&nbsp;(Only 4 Characters Allowed for Leave Code)
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <fieldset style="width: 90%">
                <legend style="color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana">
                    Leave List</legend>
                <table width="100%">
                    <tr>
                        <td align="left">
                            &nbsp;<asp:Button ID="btnDelete" runat="server" Text="Delete Selected Records" OnClick="btnDelete_Click"
                                Visible="False" OnClientClick="return valid();" TabIndex="7" onfocus="active(this);"
                                onblur="inactive(this);" />
                        </td>
                        <td align="right">
                            <asp:Label ID="lblRecCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grdDesignation" runat="server" AutoGenerateColumns="False" Width="100%"
                                TabIndex="8">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                        <ItemTemplate>
                                            <input name="Checkb" type="checkbox" value='<%#Eval("LeaveId")%>' />
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("LeaveId")%>' />
                                            <asp:HiddenField ID="hfSalDed" runat="server" Value='<%#Eval("SalaryDeduction")%>' />
                                            <asp:HiddenField ID="hfCFAllowed" runat="server" Value='<%#Eval("CFAllowed")%>' />
                                            <asp:HiddenField ID="hfLeaveType" runat="server" Value='<%#Eval("LeaveType")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%#Eval("LeaveId")%>'
                                                AlternateText="Edit" ImageUrl="~/images/icon_edit.gif" ToolTip="Click to Edit"
                                                OnClick="btnEdit_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="20px" />
                                        <HeaderStyle HorizontalAlign="center" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Leave Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeaveCode" runat="server" Text='<%#Eval("LeaveCode")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Leave Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeaveDesc" runat="server" Text=' <%#Eval("LeaveDesc")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Salary Deduction">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalDed" runat="server" Text='<%#Eval("SalDed")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Carry Forward Allowed">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCFAllowed" runat="server" Text='<%#Eval("CF")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record(s)
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

