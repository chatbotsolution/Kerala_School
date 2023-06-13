<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="Holidaydetail.aspx.cs" Inherits="HR_Holidaydetail" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function valSubmit() {
            var HolidayName = document.getElementById("<%=txtHolidayName.ClientID %>").value;
            var FrmDate = document.getElementById("<%=txtFrmDt.ClientID %>").value;
            var ToDate = document.getElementById("<%=txtToDt.ClientID %>").value;

            if (HolidayName.trim() == "") {
                alert("Enter Holiday Name");
                document.getElementById("<%=txtHolidayName.ClientID %>").focus();
                return false;
            }
            else if (FrmDate.trim() == "") {
                alert("Enter Holiday Start Date");
                document.getElementById("<%=txtFrmDt.ClientID %>").focus();
                return false;
            }
            else if (ToDate.trim() == "") {
                alert("Enter Holiday End Date");
                document.getElementById("<%=txtToDt.ClientID %>").focus();
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
            Holiday Master</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <div style="width: 500px; background-color: #666; padding: 2px;" align="left">
        <div style="background-color: #FFF;" align="left">
            <table style="width: 100%;">
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" width="150px" valign="baseline">
                        Holiday Name<font color="red">*</font>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:TextBox ID="txtHolidayName" runat="server" TabIndex="1" Width="265px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" width="150px" valign="baseline">
                        Holiday Tithi
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:TextBox ID="txtHolidayTithi" runat="server" TabIndex="2" Width="265px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" width="150px" valign="baseline">
                       Start Date<font color="red">*</font>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:TextBox ID="txtFrmDt" runat="server" TabIndex="3" Width="100px"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpFrmDt" runat="server" Control="txtFrmDt" AutoPostBack="False"
                            ShowMessageBox="True" TextMessage="Enter Date" Format="dd mmm yyyy"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" width="150px" valign="baseline">
                       End Date<font color="red">*</font>
                    </td>
                    <td align="left" valign="baseline">
                        :&nbsp;<asp:TextBox ID="txtToDt" runat="server" TabIndex="3" Width="100px"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                            ShowMessageBox="True" TextMessage="Enter Date" Format="dd mmm yyyy"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right" width="150px">
                        &nbsp;
                    </td>
                    <td align="left" valign="baseline">
                        &nbsp;
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                            OnClientClick="return valSubmit();" TabIndex="4" Width="100px" />
                        <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" TabIndex="5"
                            Width="100px" />
                        <asp:Button ID="btnList" runat="server" Text="Holiday List" TabIndex="6" Width="100px"
                            OnClick="btnList_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr id="trMsg" runat="server">
                    <td colspan="2" style="height: 20px;" align="center">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>


