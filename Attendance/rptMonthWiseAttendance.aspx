<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Attendance.master" AutoEventWireup="true" CodeFile="rptMonthWiseAttendance.aspx.cs" Inherits="Attendance_rptMonthWiseAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function valid() {
            var Class = document.getElementById("<%=drpClass.ClientID %>").value;
            if (Class == "0") {
                alert("Please Select Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
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
            Student Attendance</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" />
    </div>
    <div style="text-align: left;">
        <asp:UpdatePanel runat="server" ID="updtPnl">
            <ContentTemplate>
                <table width="95%">
                    <tr>
                        <td style="width: 98%; height: 40px" valign="bottom" align="center">
                            <div align="center">
                                <div style="width: 99%; background-color: #666; padding: 2px; margin: 0 auto;">
                                    <div style="background-color: #FFF; padding: 10px">
                                        <div>
                                            <strong>Session&nbsp;:&nbsp;</strong>
                                            <asp:DropDownList ID="drpSession" runat="server" Width="100px" CssClass="tbltxtbox">
                                            </asp:DropDownList>
                                            &nbsp; <strong>Month&nbsp;:&nbsp;</strong>
                                            <asp:DropDownList ID="drpMonth" runat="server" Width="90px" CssClass="tbltxtbox">
                                                <asp:ListItem Text="APR" Value="04" />
                                                <asp:ListItem Text="MAY" Value="05" />
                                                <asp:ListItem Text="JUN" Value="06" />
                                                <asp:ListItem Text="JUL" Value="07" />
                                                <asp:ListItem Text="AUG" Value="08" />
                                                <asp:ListItem Text="SEP" Value="09" />
                                                <asp:ListItem Text="OCT" Value="10" />
                                                <asp:ListItem Text="NOV" Value="11" />
                                                <asp:ListItem Text="DEC" Value="12" />
                                                <asp:ListItem Text="JAN" Value="01" />
                                                <asp:ListItem Text="FEB" Value="02" />
                                                <asp:ListItem Text="MAR" Value="03" />
                                            </asp:DropDownList>
                                            &nbsp; <strong>Class&nbsp;:&nbsp;</strong>
                                            <asp:DropDownList ID="drpClass" runat="server" Width="100px" CssClass="tbltxtbox">
                                            </asp:DropDownList>
                                            &nbsp;
                                    <strong class="gridtext">Section:</strong>&nbsp;
                                    <asp:DropDownList runat="server" ID="drpSection"></asp:DropDownList>
                                            &nbsp;
                                            <asp:Button Text="Show" runat="server" ID="btnShow" OnClick="btnShow_Click" OnClientClick="return valid();" />
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
                                <div style="width: 99%; background-color: #666; padding: 2px; margin: 0 auto;">
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
