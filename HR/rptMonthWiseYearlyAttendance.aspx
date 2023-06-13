<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="rptMonthWiseYearlyAttendance.aspx.cs" Inherits="HR_rptMonthWiseYearlyAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

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
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Yearly Attendance</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <div style="text-align: left;">
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <div style="width: 99%; background-color: #666; padding: 2px; margin: 0 auto;">
                    <div style="background-color: #FFF; padding: 10px">
                        <div>
                            <asp:RadioButton ID="rbtnCal" runat="server" Text="Calender Year" AutoPostBack="true"
                                Checked="true" oncheckedchanged="rbtnCal_CheckedChanged" GroupName="A"/>
                            <asp:RadioButton ID="rbtnSy" runat="server" Text="Session Year" oncheckedchanged="rbtnCal_CheckedChanged" AutoPostBack="true" GroupName="A"/>
                            <%--<strong>Year&nbsp;:&nbsp;</strong>--%>
                            <asp:DropDownList ID="drpYear" runat="server">
                            </asp:DropDownList>
                            <asp:Button Text="Show" runat="server" ID="btnShow" OnClick="btnShow_Click" />
                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
                            <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel" />
                            <br />
                            <asp:Literal Text="" runat="server" ID="litFinalMsg" />
                        </div>
                    </div>
                </div>
                <div class="spacer">
                    <img src="../images/mask.gif" height="8" width="10" /></div>
                <div style="padding-left: 5px; padding-right: 3px;">
                    <asp:Literal Text="" runat="server" ID="litReport" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExpExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

