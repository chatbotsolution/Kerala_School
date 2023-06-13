<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="rptStudMarksheet.aspx.cs" Inherits="Exam_rptStudMarksheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function isValid() {
            var Session = document.getElementById("<%=drpSession.ClientID %>").selectedIndex;
            var Exam = document.getElementById("<%=drpExam.ClientID %>").selectedIndex;
            var rollno = document.getElementById("<%=drpStudent.ClientID %>").selectedIndex;
            if (Session == "0") {
                alert("Select a Session");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            if (Exam == "0") {
                alert("Select an Exam");
                document.getElementById("<%=drpExam.ClientID %>").focus();
                return false;
            }
            if (rollno == "0") {
                alert("Select a Student");
                document.getElementById("<%=drpStudent.ClientID %>").focus();
                return false;
            }
            else {
                CheckLoader();
                return true;
            }
        }
        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';
        }
        function openwindow() {
            var pageurl = "rptStudMarksheetPrint.aspx";
            window.open(pageurl, 'true', 'true');
        }       
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 3px;">
        <h2>
            Student Marksheet</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" cellpadding="1px" style="background-color: #D3E7EE; outline-style: solid;
                outline-width: 1px">
                <tr>
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;">
                        <strong>Session :</strong>
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                            onchange="CheckLoader();">
                        </asp:DropDownList>
                        Class :
                        <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                            onchange="CheckLoader();">
                        </asp:DropDownList>
                        <strong>Exam :</strong>
                        <asp:DropDownList ID="drpExam" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpExam_SelectedIndexChanged"
                            onchange="CheckLoader();">
                        </asp:DropDownList>
                        <strong>Student :</strong>
                        <asp:DropDownList ID="drpStudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpStudent_SelectedIndexChanged"
                            onchange="CheckLoader();">
                        </asp:DropDownList>
                        <asp:Button ID="btnShow" runat="server" Text="Show" OnClientClick="return isValid();"
                            OnClick="btnShow_Click" Visible="false" onfocus="active(this);" onblur="inactive(this);" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="openwindow();"
                            onfocus="active(this);" onblur="inactive(this);" />
                    </td>
                </tr>
            </table>
            <br />
            <center>
                <asp:Label ID="lblReport" runat="server"></asp:Label></center>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="drpClass" />
            <asp:PostBackTrigger ControlID="drpSession" />
            <asp:PostBackTrigger ControlID="drpStudent" />
            <asp:PostBackTrigger ControlID="drpExam" />
        </Triggers>
    </asp:UpdatePanel>
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

