<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpShiftMaster.aspx.cs" Inherits="HR_EmpShiftMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript" language="javascript">
    function isValid() {
        var Shift = document.getElementById("<%=txtShift.ClientID %>").value;
        var StTime = document.getElementById("<%=txtStrtTime.ClientID %>").value;
        var EndTime = document.getElementById("<%=txtEndTime.ClientID %>").value;
        if (Shift.trim() == "") {
            alert("Enter Shift Name");
            document.getElementById("<%=txtShift.ClientID %>").focus();
            return false;
        }
        else if (StTime.trim() == "") {
            alert("Enter Shift Start Time");
            document.getElementById("<%=txtStrtTime.ClientID %>").focus();
            return false;
        }
        else if (StTime.trim().length < 4) {
            alert("Please Enter 4 Digit Time Format");
            document.getElementById("<%=txtStrtTime.ClientID %>").focus();
            return false;
        }
        else if (parseInt(StTime) <= 0 || parseInt(StTime) > 2359) {
            alert("Please Enter Correct Shift Start Time");
            document.getElementById("<%=txtStrtTime.ClientID %>").focus();
            return false;
        }
        else if (EndTime.trim() == "") {
            alert("Enter Shift End Time");
            document.getElementById("<%=txtEndTime.ClientID %>").focus();
            return false;
        }
        else if (EndTime.trim().length < 4) {
            alert("Please Enter 4 Digit Time Format");
            document.getElementById("<%=txtEndTime.ClientID %>").focus();
            return false;
        }
        else if (parseInt(EndTime) <= 0 || parseInt(EndTime) > 2359) {
            alert("Please Enter Correct Shift End Time");
            document.getElementById("<%=txtEndTime.ClientID %>").focus();
            return false;
        }
        else if (parseInt(StTime) > parseInt(EndTime) || parseInt(EndTime) == parseInt(StTime)) {
            alert("Endtime Cannot Be Less than or equal to Start");
            document.getElementById("<%=txtEndTime.ClientID %>").focus();
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

        return isFirstN || reg.test(keychar);
    }

    </script>
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Shift Master</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
        <table>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" valign="baseline">
                    Shift Name/Code<font color="red">*</font>
                </td>
                <td align="left" valign="baseline">
                    :&nbsp;<asp:TextBox ID="txtShift" runat="server" Height="17px" Width="200px" 
                        TabIndex="1"></asp:TextBox> 
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" valign="baseline">
                    Start Time<font color="red">*</font>
                </td>
                <td align="left" valign="baseline">
                    :&nbsp;<asp:TextBox ID="txtStrtTime" runat="server" Height="17px" Width="60px" onkeypress="return blockNonNumbers(this, event, true, true);"
                       MaxLength="4" TabIndex="2"></asp:TextBox>&nbsp;(In 24 hr Format Without &quot;:&quot;)
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 23px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" valign="baseline">
                    End Time<font color="red">*</font>
                </td>
                <td align="left" valign="baseline">
                    :&nbsp;<asp:TextBox ID="txtEndTime" runat="server" Height="17px" Width="60px" onkeypress="return blockNonNumbers(this, event, true, true);" 
                       MaxLength="4" TabIndex="3"></asp:TextBox>&nbsp;(In 24 hr Format Without &quot;:&quot;)
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
                    &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                        OnClientClick="return isValid();" onclick="btnSubmit_Click" TabIndex="4" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                        onclick="btnCancel_Click" TabIndex="5" />
                    <asp:Button ID="btnList" runat="server" Text="Show List" 
                        onclick="btnList_Click" TabIndex="6" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                    <asp:HiddenField runat="server" ID="hfShiftId" />
                </td>
            </tr>
            <tr id="trMsg" runat="server">
                <td colspan="2" align="center">
                    <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                </td>
            </tr>
        </table>
    <table width="50%">
    <tr>
        <td><asp:Button ID="btnAdd" runat="server" Text="Add New" onclick="btnAdd_Click" Visible="false"
                TabIndex="7" /> 
            <asp:Button ID="btnDelete" runat="server" Text="Delete" Visible="false" OnClientClick="return CnfDelete();"
                onclick="btnDelete_Click" TabIndex="8" /></td>
                <td><asp:Label runat="server" ID="lblRecCount" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:GridView ID="grdShift" runat="server" AutoGenerateColumns="False" Width="100%"
                                EmptyDataText="No Record">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                        <ItemTemplate>
                                            <input name="Checkb" type="checkbox" value='<%#Eval("ShiftId")%>' />
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("ShiftId")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%#Eval("ShiftId")%>'
                                                AlternateText="Edit" ImageUrl="~/images/icon_edit.gif" 
                                                ToolTip="Click to Edit" onclick="btnEdit_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="20px" />
                                        <HeaderStyle HorizontalAlign="center" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shift Name">
                                        <ItemTemplate>
                                            <%#Eval("ShiftCode")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Time">
                                        <ItemTemplate>
                                            <%#Eval("StartTime")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Time">
                                        <ItemTemplate>
                                            <%#Eval("EndTime")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
        </td>
    </tr>
    </table>
    
</asp:Content>

