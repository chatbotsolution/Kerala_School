<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpPastAppmtDtls.aspx.cs" Inherits="HR_EmpPastAppmtDtls" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript">
        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';

        }
        function clearText(btn) {
            switch (btn) {
                case '1':
                    {
                        document.getElementById("<%=txtFromDt.ClientID %>").value = "";
                        return false;
                    }
                case '2':
                    {
                        document.getElementById("<%=txtToDt.ClientID %>").value = "";
                        return false;
                    }
                case '3':
                    {
                        document.getElementById("<%=txtTransferDt.ClientID %>").value = "";
                        return false;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        function validateform() {
            var Designation = document.getElementById("<%=drpDesignation.ClientID %>").value;
            var FromDt = document.getElementById("<%=txtFromDt.ClientID %>").value;
            var TransNo = document.getElementById("<%=txtTransferOrdNo.ClientID %>").value;
            var TransDt = document.getElementById("<%=txtTransferDt.ClientID %>").value;
            var vSSVM = document.getElementById("<%=txtSSVM.ClientID %>");

            if (vSSVM.value.trim() == "") {
                alert("Enter School Name !");
                vSSVM.focus();
                return false;
            }
            if (Designation == "0") {
                alert("Select Designation !");
                document.getElementById("<%=drpDesignation.ClientID %>").focus();
                return false;
            }
            if (FromDt.trim() == "") {
                alert("Enter From Date !");
                document.getElementById("<%=txtFromDt.ClientID %>").focus();
                return false;
            }
            if (TransDt.trim() == "") {
                alert("Enter Transfer Date !");
                document.getElementById("<%=txtTransferDt.ClientID %>").focus();
                return false;
            }
            if (Date.parse(TransDt.trim()) > Date.parse(FromDt.trim())) {
                alert("Invalid Date !\nTransfer Date cannot be after Joining Date!")
                return false;
            }
            else {
                CheckLoader();
                return true;
            }
        }
            
    </script>
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
                <h2>
                    Employee Appointment Details</h2>
                <div style="float: right;">
                    <asp:Label ID="lblMsgTop" Font-Bold="true" ForeColor="Green" runat="server" Text=""
                        CssClass="tbltxt"></asp:Label>
                </div>
            </div>
    <div align="left">
        <br />
        <div class="innerdiv" style="width: 480px">
            <div class="linegap">
                <img src="../images/mask.gif" width="10" height="10" /></div>
            <div style="padding: 8px;">
                <asp:UpdatePanel ID="updtPnl" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td width="130" align="left" valign="top" class="tbltxt">
                                    Employee Name
                                </td>
                                <td width="5" align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:Label ID="lblEmp" runat="server" CssClass="totalrec"></asp:Label>
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="tbltxt">
                                    School Name
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top" colspan="2">
                                    <asp:TextBox ID="txtSSVM" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="tbltxt">
                                    School Address
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top" colspan="2">
                                    <asp:TextBox ID="txtSchoolAddr" runat="server" TextMode="MultiLine" Width="200px" Height="50px" MaxLength="199"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="tbltxt">
                                    Designation
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:DropDownList ID="drpDesignation" runat="server" TabIndex="7" CssClass="tbltxtbox">
                                    </asp:DropDownList>
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                            </tr>
                            <%--<tr>
                                <td align="left" valign="top" class="tbltxt">
                                    To Date
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtToDt" runat="server" CssClass="tbltxtbox_mid" TabIndex="4"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpTo" runat="server" Control="txtToDt" Format="dd mmm yyyy">
                                    </rjs:PopCalendar>
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                        OnClientClick="return clearText('2');" />
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                            </tr>--%>
                            <tr>
                                <td align="left" valign="top" class="tbltxt">
                                    Transfer Order No
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtTransferOrdNo" MaxLength="20" runat="server"
                                        TabIndex="8"></asp:TextBox>
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="tbltxt">
                                    Transfer Date
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtTransferDt" runat="server" CssClass="tbltxtbox_mid" TabIndex="9"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpTransfer" runat="server" Control="txtTransferDt" Format="dd mmm yyyy">
                                    </rjs:PopCalendar>
                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                        OnClientClick="return clearText('3');" />
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="tbltxt">
                                    Joining Date
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFromDt" runat="server" CssClass="tbltxtbox_mid" TabIndex="10"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpFrom" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
                                    </rjs:PopCalendar>
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                        OnClientClick="return clearText('1');" />
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="tbltxt">
                                    Leaving Date
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtToDt" runat="server" CssClass="tbltxtbox_mid" TabIndex="10"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" Format="dd mmm yyyy">
                                    </rjs:PopCalendar>
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                        OnClientClick="return clearText('2');" />
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="tbltxt">
                                    Remarks
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="tbltxtbox_mid" TextMode="MultiLine"
                                        Height="50px" Width="200px" MaxLength="100" TabIndex="11"></asp:TextBox>
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                            </tr>
                            <tr style="background-color: #FFB598;">
                                <td colspan="4" align="left" class="td-help tbltxt">
                                    ** All fields are mandatory except <b>Remarks</b> . Here the latest entry will 
                                    be considered as current working School</td>
                            </tr>
                            <tr>
                                <td align="center" valign="top" class="tbltxt" colspan="4">
                                    <asp:Button ID="btnSave" runat="server" Text="Save & Add New" OnClientClick="return validateform();"
                                        OnClick="btnSave_Click" TabIndex="12" />&nbsp;<asp:Button ID="btnShow" runat="server"
                                            Text="Save & Go to List" OnClientClick="return validateform();" OnClick="btnShow_Click"
                                            TabIndex="13" />&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="Clear" OnClick="btnCancel_Click"
                                        TabIndex="14" />&nbsp;<asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click"
                                            TabIndex="15" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <%--<div id="Loader" style="text-align: center; vertical-align: middle; position: absolute;
        top: 0px; left: 0px; z-index: 99; width: 100%; height: 100%; background-color: #ededed;
        -ms-filter: 'progid:DXImageTransform.Microsoft.Alpha(Opacity=60)'; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=60);
        -moz-opacity: 0.8; opacity: 0.8;">
        <div style="width: 48px; height: 48px; margin: 0 auto; margin-top: 275px;">
            <img src="../images/spinner.gif">
        </div>
        <div style="font-family: Trebuchet MS; font-size: 12px; color: Green; text-align: center;">
            Please Wait ...</div>
    </div>--%>
</asp:Content>

