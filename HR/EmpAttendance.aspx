<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpAttendance.aspx.cs" Inherits="HR_EmpAttendance" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">

    function valSubmit() {
        var HolidayName = document.getElementById("<%=txtHolidayDesc.ClientID %>").value;

        if (HolidayName.trim() == "") {
            alert("Enter Holiday Name");
            document.getElementById("<%=txtHolidayDesc.ClientID %>").focus();
            return false;
        }

        else {
            return Cnfrm();
        }
    }

    function Cnfrm() {

        if (confirm("You are going to set this day as holiday!! Do you want to continue?")) {
            return true;
        }
        else {
            return false;
        }
    }

    function CnfAtt() {

        if (confirm("You are going to set this day's Attendance. Do you want to continue?")) {
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
            Staff Attendance</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <%#Eval("EmpName")%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="center">
                        <div style="width: 100%; background-color: #666; padding: 2px; margin: 0 auto;">
                            <div style="background-color: #FFF; padding: 10px;">
                                <strong>Shift&nbsp;:&nbsp;<asp:DropDownList runat="server" ID="drpShift" 
                                    onselectedindexchanged="drpShift_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                <strong>Date&nbsp;:&nbsp;</strong><asp:TextBox ID="txtDate" runat="server" Width="100px"
                                    AutoPostBack="True" OnTextChanged="txtDate_TextChanged" ReadOnly="True"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpDate" runat="server" Control="txtDate" AutoPostBack="True" To-Today="true"
                                    Format="dd mmm yyyy" OnSelectionChanged="dtpDate_SelectionChanged"></rjs:PopCalendar>
                                 <asp:CheckBox ID="chkbHolidy" runat="server" Text="Is Holiday" AutoPostBack="true" 
                                    TextAlign="Left" oncheckedchanged="chkbHolidy_CheckedChanged" Enabled="false"/>
                                 <strong>Holiday Desc&nbsp;:&nbsp;</strong><asp:TextBox ID="txtHolidayDesc" runat="server" Enabled="false"></asp:TextBox>
                                 <asp:Button ID="btnSaveHoliday" runat="server" Text="Save Holiday" OnClientClick="return valSubmit();"
                                    onclick="btnSaveHoliday_Click" Visible="false" />
                                 <asp:Button ID="btnExtraAtt" runat="server" Text="Mark Extra Attendance" 
                                    onclick="btnExtraAtt_Click" />
                            </div>
                        </div>
                    </td>
                </tr>
                 <tr>
                <td align="center">
                        <asp:Label ID="lblNote" runat="server" ForeColor="Red" Font-Bold="true" Text="Please define holidays before marking attendance for any day."></asp:Label>
                    </td>
                </tr>
                <tr>
                <td align="center">
                        <asp:Label ID="lblMsg3" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                    <div style="text-align:left;">
                        <asp:Label ID="lblLastAtt" runat="server" Text=""></asp:Label>
                        </div>
                    <div style="text-align:right;">
                        <asp:Label ID="lblRecords" runat="server" Text=""></asp:Label>
                        </div>
                        <div style="width: 100%; background-color: #666; padding: 2px; margin: 0 auto;">
                            <div style="background-color: #FFF; padding: 10px;">
                                <asp:GridView ID="grdEmpAttendance" runat="server" Width="100%" AutoGenerateColumns="False"
                                    OnRowDataBound="grdEmpAttendance_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Employee ID">
                                            <HeaderStyle Width="70px" HorizontalAlign="Left" />
                                            <ItemStyle Width="70px" HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpId" Text='<%#Eval("EmpId")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Name">
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%#Eval("EmpName")%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="In Time">
                                            <HeaderStyle Width="70px" HorizontalAlign="Left" />
                                            <ItemStyle Width="70px" HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInTime" runat="server" Width="88%" Height="17px" ValidationGroup="MKE" MaxLength="4"
                                                    Text='<%#Eval("InTime")%>' onkeypress="return blockNonNumbers(this, event, true, true);"/>
                                                    <asp:HiddenField runat="server" ID="hfIn" Value='<%#Eval("InTime")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Out Time">
                                            <HeaderStyle Width="70px" HorizontalAlign="Left" />
                                            <ItemStyle Width="70px" HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOutTime" runat="server" Width="88%" Height="17px" ValidationGroup="MKE" MaxLength="4"
                                                    Text='<%#Eval("OutTime")%>' onkeypress="return blockNonNumbers(this, event, true, true);" />
                                                    <asp:HiddenField runat="server" ID="hfOut" Value='<%#Eval("OutTime")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AttendStatus" Visible="false">
                                            <HeaderStyle Width="70px" HorizontalAlign="Left" />
                                            <ItemStyle Width="70px" HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" Text='<%#Eval("AttendStatus")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <HeaderStyle Width="70px" HorizontalAlign="Left" />
                                            <ItemStyle Width="70px" HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                &nbsp;<asp:DropDownList ID="drpStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpStatus_SelectedIndexChanged">
                                                    <asp:ListItem Value="P">Present</asp:ListItem>
                                                    <asp:ListItem Value="A">Absent</asp:ListItem>
                                                   <%-- <asp:ListItem Value="L">Leave</asp:ListItem>
                                                    <asp:ListItem Value="HDL">Half Day Leave</asp:ListItem>--%>
                                                    <asp:ListItem Value="Off">Off</asp:ListItem>
                                                    <asp:ListItem Value="Tour">Tour</asp:ListItem>
                                                    <asp:ListItem Value="DL">Duty Leave</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Width="92%" Text='<%#Eval("Remarks")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="ChkID" runat="server" Text='<%#Eval("id")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Record
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblMsg1" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width: 100%">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" 
                            Enabled="False" ValidationGroup="MKE" CausesValidation="true" onfocus="active(this);"
                            onblur="inactive(this);" OnClientClick="return CnfAtt();" />
                        <asp:Button ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click"
                            CausesValidation="false" onfocus="active(this);" onblur="inactive(this);" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblMsg2" runat="server" Font-Size="8pt" ForeColor="Red" Text="Note : Attendance already made for the Day"
                            Visible="False" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="dtpDate" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

