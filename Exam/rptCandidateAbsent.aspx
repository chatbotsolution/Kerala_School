<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="rptCandidateAbsent.aspx.cs" Inherits="Exam_rptCandidateAbsent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';

        }
        function valShow() {
            var cls = document.getElementById("<%=drpClass.ClientID %>").value;
            var exam = document.getElementById("<%=drpExamName.ClientID %>").value;
            var sub = document.getElementById("<%=drpSubject.ClientID %>").value;
            if (cls.trim() == "0") {
                alert("Select a Class");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            else if (exam.trim() == "0") {
                alert("Select an Exam");
                document.getElementById("<%=drpExamName.ClientID %>").focus();
                return false;
            }
            else if (sub.trim() == "0") {
                alert("Select a Subject");
                document.getElementById("<%=drpSubject.ClientID %>").focus();
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
    <div style="padding-top: 3px;">
        <h2>
            Exam Attendance Report</h2>
    </div>
    <div align="left">
        <br />
        <div class="innerdiv" style="width: 100%;">
            <div style="padding: 8px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr style="background-color: #D3E7EE;">
                                <td align="left" style="padding: 5px; height: 3px; color: #000; border-top: 2px solid #333;
                                    border-left: 2px solid #333; border-right: 2px solid #333;">
                                    Session&nbsp;:
                                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                                        TabIndex="1" onchange="CheckLoader();">
                                    </asp:DropDownList>
                                    Class&nbsp;:
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                        TabIndex="2" onchange="CheckLoader();">
                                    </asp:DropDownList>
                                    Section&nbsp;:
                                    <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSection_SelectedIndexChanged"
                                        TabIndex="3" onchange="CheckLoader();">
                                    </asp:DropDownList>
                                    Exam Name<span class="mandatory">*</span>&nbsp;:
                                    <asp:DropDownList ID="drpExamName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpExamName_SelectedIndexChanged"
                                        TabIndex="3" onchange="CheckLoader();">
                                    </asp:DropDownList>
                                    Subject<span class="mandatory">*</span>&nbsp;:
                                    <asp:DropDownList ID="drpSubject" runat="server" TabIndex="4" AutoPostBack="True"
                                        OnSelectedIndexChanged="drpSubject_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    Status&nbsp;:&nbsp;
                                    <asp:DropDownList ID="drpStatus" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="drpStatus_SelectedIndexChanged">
                                        <asp:ListItem>- ALL -</asp:ListItem>
                                        <asp:ListItem>Present</asp:ListItem>
                                        <asp:ListItem>Absent</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="background-color: #D3E7EE;">
                                <td align="center" style="padding: 5px; height: 3px; color: #000; border-left: 2px solid #333;
                                    border-right: 2px solid #333; border-bottom: 2px solid #333;">
                                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" onfocus="active(this);"
                                        onblur="inactive(this);" OnClientClick="return valShow();" />
                                    &nbsp;<asp:Button ID="btnExport" Text="Export to Excel" runat="server" OnClick="btnExport_Click"
                                        onfocus="active(this);" onblur="inactive(this);" Enabled="false" />
                                    &nbsp;<asp:Button ID="btnPrint" Text="Print" runat="server" OnClick="btnPrint_Click"
                                        onfocus="active(this);" onblur="inactive(this);" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                        <div style="height: 5px;">
                        </div>
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="center" valign="top">
                                    <asp:Label runat="server" ID="lblReport"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExport" />
                        <asp:PostBackTrigger ControlID="drpClass" />
                        <asp:PostBackTrigger ControlID="btnShow" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div id="Loader" style="text-align: center; vertical-align: middle; position: absolute;
        top: 0px; left: 0px; z-index: 99; width: 100%; height: 100%; background-color: #ededed;
        -ms-filter: 'progid:DXImageTransform.Microsoft.Alpha(Opacity=60)'; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=60);
        -moz-opacity: 0.8; opacity: 0.8;">
        <div style="width: 48px; height: 48px; margin: 0 auto; margin-top: 275px;">
            <img src="../images/spinner.gif">
        </div>
        <div style="font-family: Trebuchet MS; font-size: 12px; color: Green; text-align: center;">
            Please Wait ...</div>
    </div>
</asp:Content>
