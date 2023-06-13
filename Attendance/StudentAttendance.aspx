<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Attendance.master" AutoEventWireup="true" CodeFile="StudentAttendance.aspx.cs" Inherits="Attendance_StudentAttendance" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script language="javascript" type="text/javascript">
    function CnfAtt() {

        if (confirm("You are going to set this day's Attendance. Do you want to continue?")) {
            return true;
        }
        else {
            return false;
        }
    } 
</script>
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29">
    </div>
    <div style="padding-top: 5px;">
        <h2>Student Attendance</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" />
    </div>
    <div style="text-align: left;">
        <asp:UpdatePanel runat="server" ID="updtPnl">
            <ContentTemplate>
                <table width="80%">
                    <tr>
                        <td style="width: 90%; height: 40px" valign="bottom" colspan="2" align="center">
                            <div align="center">
                                <div style="width: 97%; background-color: #666; padding: 2px; margin: 0 auto;">
                                    <div style="background-color: #FFF; padding: 10px">
                                        <div class="linegap">
                                            <img src="../images/mask.gif" height="5" width="10">
                                        </div>
                                        <div>
                                            <strong class="gridtext">&nbsp;&nbsp;Date:</strong>&nbsp;
                                    <asp:TextBox ID="txtDate" runat="server" Width="150px" AutoPostBack="True" OnTextChanged="txtDate_TextChanged" CssClass="tbltxtbox"
                                        ReadOnly="True"></asp:TextBox>
                                            <rjs:PopCalendar ID="dtpDate" runat="server" Control="txtDate" AutoPostBack="True"
                                                Format="dd mmm yyyy" OnSelectionChanged="dtpDate_SelectionChanged" To-Today="true"></rjs:PopCalendar>
                                            &nbsp;&nbsp;&nbsp;<strong class="gridtext">Class:</strong>&nbsp;
                                    <asp:DropDownList ID="drpClass" runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                        CssClass="tbltxtbox">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;
                                    <strong class="gridtext">Section:</strong>&nbsp;
                                    <asp:DropDownList runat="server" ID="drpSection" AutoPostBack="true" 
                                                onselectedindexchanged="drpSection_SelectedIndexChanged"></asp:DropDownList>
                                            <br />
                                            <asp:Label ID="lblMsg" runat="server" Font-Size="8pt" ForeColor="Red" Text="Note: You have already marked attendance for this date. In case of any modification click on Modify."
                                                Visible="False"></asp:Label>
                                            <asp:Literal Text="" runat="server" ID="litFinalMsg" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <div align="center">
                                <div style="width: 97%; background-color: #666; padding: 2px; margin: 0 auto;">
                                    <div style="background-color: #FFF; padding: 10px">
                                        <div class="linegap">
                                            <img src="../images/mask.gif" height="5" width="10" />
                                        </div>
                                        <asp:GridView ID="gvStudAttendance" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                            AlternatingRowStyle-CssClass="alt" OnRowDataBound="gvStudAttendance_RowDataBound">
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Admission No">
                                                    <HeaderStyle Width="80px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdmnNo" Text='<%#Eval("AdmnNo")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Student Name">
                                                    <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%#Eval("FullName")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status">
                                                    <HeaderStyle Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox Text="Is Present" runat="server" ID="chkStatus" Checked="true" OnCheckedChanged="chkStatus_CheckedChanged" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Height="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <HeaderStyle Width="200px" />
                                                    <ItemTemplate>
                                                        &nbsp;<asp:TextBox ID="txtRemarks" runat="server" Width="95%" Text='<%#Eval("Remarks")%>' MaxLength="100"></asp:TextBox>&nbsp;
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                Please select a class to view students
                                            </EmptyDataTemplate>
                                            <EmptyDataRowStyle CssClass="pgr" HorizontalAlign="Center" Height="30px" Font-Size="Large" Font-Bold="true" />
                                        </asp:GridView>
                                        <asp:Label runat="server" Text="" ID="lblMsg2"></asp:Label>
                                        <div class="boxedMessage">
                                            Records containing remarks will be treated as granted leaves.
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="width: 100%">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                Enabled="False" ValidationGroup="MKE" CausesValidation="true" OnClientClick="return CnfAtt();"/>&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click"
                        CausesValidation="false" />&nbsp;
                    <asp:Button ID="btnHome" runat="server" Text="Home" OnClick="btnHome_Click" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>


