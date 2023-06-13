<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpTrainingDetails.aspx.cs" Inherits="HR_EmpTrainingDetails" %>

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
                default:
                    {
                        break;
                    }
            }
        }
        function validateform() {
            var TrainingName = document.getElementById("<%= drpTrgName.ClientID %>").value;
            var TrainingPlace = document.getElementById("<%=txtTrainingPlace.ClientID %>").value;
            var FromDt = document.getElementById("<%=txtFromDt.ClientID %>").value;
            var ToDt = document.getElementById("<%=txtToDt.ClientID %>").value;

            if (TrainingName.trim() == "0") {
                alert("Enter Training Name !");
                document.getElementById("<%=drpTrgName.ClientID %>").focus();
                return false;
            }
            if (TrainingPlace.trim() == "") {
                alert("Enter Training Place !");
                document.getElementById("<%=txtTrainingPlace.ClientID %>").focus();
                return false;
            }
            if (FromDt.trim() == "") {
                alert("Enter From Date !");
                document.getElementById("<%=txtFromDt.ClientID %>").focus();
                return false;
            }
            if (ToDt.trim() == "") {
                alert("Enter To Date !");
                document.getElementById("<%=txtToDt.ClientID %>").focus();
                return false;
            }
            if (Date.parse(FromDt.trim()) > Date.parse(ToDt.trim())) {
                alert("Invalid Date!\nFrom Date cannot be after To Date!")
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
                    Employee Training Details</h2>
                <div style="float:left;">
                    <asp:Label ID="lblMsgTop" Font-Bold="true" ForeColor="Green" runat="server" Text=""
                        CssClass="tbltxt"></asp:Label>
                </div>
            </div>
    <div align="left">
        <br />
        <div class="innerdiv" style="width: 520px;">
            <div class="linegap">
                <img src="../images/mask.gif" width="10" height="10" />
            </div>
            <div style="padding: 8px;">
                <table width="100%">
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">Employee Name
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">:
                        </td>
                        <td align="left" valign="top">
                            <asp:Label ID="lblEmp" runat="server" CssClass="totalrec"></asp:Label>
                        </td>
                        <td align="left" valign="top" class="mandatory"></td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">Name Of Training
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">:
                        </td>
                        <td align="left" valign="top">
                            <span class="mandatory">
                                <asp:DropDownList ID="drpTrgName" runat="server"
                                    CssClass="tbltxtbox" TabIndex="1" Width="330px">
                                </asp:DropDownList>
                                *</span>
                                <asp:Button runat="server" ID="btnAddTrng" Text="Add" 
                                onclick="btnAddTrng_Click"/>
                        </td>
                        <td align="left" valign="top" class="mandatory"></td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">Place of Training
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">:
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtTrainingPlace" runat="server" CssClass="tbltxtbox" MaxLength="50" Width="330px"
                                TabIndex="2"></asp:TextBox><span class="mandatory">*</span>
                        </td>
                        <td align="left" valign="top" class="mandatory"></td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">From Date
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">:
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtFromDt" runat="server" CssClass="tbltxtbox_mid" TabIndex="3"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpFrom" runat="server" Control="txtFromDt" Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                OnClientClick="return clearText('1');" />
                        </td>
                        <td align="left" valign="top" class="mandatory"></td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">To Date
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">:
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtToDt" runat="server" CssClass="tbltxtbox_mid" TabIndex="4"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpTo" runat="server" Control="txtToDt" Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                OnClientClick="return clearText('2');" />
                        </td>
                        <td align="left" valign="top" class="mandatory"></td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">Remarks
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">:
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtRemarks" TextMode="MultiLine" Height="50px" runat="server" CssClass="tbltxtbox_mid"
                                Width="330px" MaxLength="200" TabIndex="5"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory"></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center" class="td-help">** All fields are mandatory.
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top" class="tbltxt" colspan="4">
                            <asp:Button ID="btnSave" runat="server" Text="Save & Add New" OnClientClick="return validateform();"
                                OnClick="btnSave_Click" ValidationGroup="grpTraining" TabIndex="6" />
                            <asp:Button ID="btnShow" OnClientClick="return validateform();" runat="server" Text="Save & Go to List"
                                OnClick="btnShow_Click" TabIndex="7" />
                            <asp:Button ID="btnCancel" runat="server" Text="Clear"
                                OnClick="btnCancel_Click" TabIndex="8" />
                            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click"
                                TabIndex="9" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%-- <div id="Loader" style="text-align: center; vertical-align: middle; position: absolute;
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
    
