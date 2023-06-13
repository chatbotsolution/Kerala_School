<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Attendance.master" AutoEventWireup="true" CodeFile="rptMonthWiseYearlyAttendance.aspx.cs" Inherits="Attendance_rptMonthWiseYearlyAttendance" %>

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
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29">
    </div>
    <div style="padding-top: 5px;">
        <h2>
            Yearly Student Attendance</h2>
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
                                            &nbsp; <strong>Class&nbsp;:&nbsp;</strong>
                                            <asp:DropDownList ID="drpClass" runat="server" Width="100px" CssClass="tbltxtbox"
                                                AutoPostBack="true" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            &nbsp; <strong>Section&nbsp;:&nbsp;</strong>
                                            <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" Width="50px"
                                                CssClass="tbltxtbox" TabIndex="3">
                                            </asp:DropDownList>
                                            &nbsp; <%--<strong>Present&nbsp;:&nbsp;</strong><asp:TextBox ID="txtPercentage" runat="server"
                                                CssClass="tbltxtbox-no-rb" MaxLength="5" Style="border-right: solid 0px transparent;"
                                                onkeypress="return blockNonNumbers(this, event, true, false);" Width="50px" TabIndex="52"></asp:TextBox><asp:TextBox
                                                    ID="TextBox1" runat="server" ReadOnly="true" Text="%" Width="20px" CssClass="tbltxtbox-no-lb"
                                                    Style="border-left: solid 0px transparent;" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>--%>
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

