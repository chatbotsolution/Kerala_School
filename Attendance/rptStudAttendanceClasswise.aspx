<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Attendance.master" AutoEventWireup="true" CodeFile="rptStudAttendanceClasswise.aspx.cs" Inherits="Attendance_rptStudAttendanceClasswise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function valid() {
            var Class = document.getElementById("<%=drpClass.ClientID %>").value;
            var Stud = document.getElementById("<%=drpStudent.ClientID %>").value;
            var Admn = document.getElementById("<%=txtadmnno.ClientID %>").value;
            if (Class == "0") {
                alert("Please Select Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            if (Stud == "0") {
                alert("Please Select Student !");
                document.getElementById("<%=drpStudent.ClientID %>").focus();
                return false;
            }
            if (Admn.trim() == "") {
                alert("Please Enter Student Admission number !");
                document.getElementById("<%=txtadmnno.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        } 
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29">
    </div>
    <div style="padding-top: 5px;">
        <h2>
            Studentwise Attendance</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" />
    </div>
    <div style="text-align: left;">
        <asp:UpdatePanel runat="server" ID="updtPnl">
            <ContentTemplate>
                <table width="95%">
                    <tr>
                        <td style="width: 95%; height: 40px" valign="bottom" align="center">
                            <div align="center">
                                <div style="width: 97%; background-color: #666; padding: 2px; margin: 0 auto;">
                                    <div style="background-color: #FFF; padding: 10px">
                                        <div>
                                            <strong>Session&nbsp;:&nbsp;</strong>
                                            <asp:DropDownList ID="drpSession" runat="server" Width="100px" CssClass="vsmalltb"
                                                AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            &nbsp; <strong>Class&nbsp;:&nbsp;</strong>
                                            <asp:DropDownList ID="drpClass" runat="server" Width="100px" CssClass="vsmalltb"
                                                AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;
                                             <strong class="gridtext">Section:</strong>&nbsp;
                                              <asp:DropDownList runat="server" ID="drpSection" AutoPostBack="true" onselectedindexchanged="drpSection_SelectedIndexChanged" 
                                             ></asp:DropDownList>
                                            &nbsp; <strong>Student&nbsp;:&nbsp;</strong>
                                            <asp:DropDownList ID="drpStudent" runat="server" Width="200px" CssClass="vsmalltb"
                                                AutoPostBack="True" OnSelectedIndexChanged="drpStudent_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            &nbsp; <strong>Student Id&nbsp;:&nbsp;</strong>
                                            <asp:TextBox ID="txtadmnno" runat="server" CssClass="vsmalltb" TabIndex="4" Width="70px"></asp:TextBox>
                                            &nbsp;
                                            <asp:Button Text="Show" OnClientClick="return valid();" runat="server" ID="btnShow"
                                                OnClick="btnShow_Click" />
                                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
                                            <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel" />
                                            <br />
                                            <asp:Literal Text="" runat="server" ID="litFinalMsg" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div align="center">
                                <div style="width: 97%; background-color: #666; padding: 2px; margin: 0 auto;">
                                    <div style="background-color: #FFF; padding: 10px">
                                        <div>
                                            <asp:Literal Text="" runat="server" ID="litReport" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExpExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
