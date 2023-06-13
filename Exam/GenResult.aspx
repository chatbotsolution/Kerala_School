<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="GenResult.aspx.cs" Inherits="Exam_GenResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function isValid() {
            var Session = document.getElementById("<%=drpSession.ClientID %>").selectedIndex;
            var StdClass = document.getElementById("<%=drpClass.ClientID %>").selectedIndex;
            var Exam = document.getElementById("<%=drpExam.ClientID %>").selectedIndex;

            //get current date
            if (Session == "0") {
                alert("Select a Session");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            else if (StdClass == "0") {
                alert("Select a Class");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            else if (Exam == "0") {
                alert("Select an Exam");
                document.getElementById("<%=drpExam.ClientID %>").focus();
                return false;
            }
            else {
                return cnfSubmit();
            }
        }
        function cnfSubmit() {

            if (confirm("Are you sure to Generate the Result for the selected Exam ?")) {
                CheckLoader();
                return true;
            }
            else {
                return false;
            }
        }
        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';
        }       
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 3px;">
        <h2>
            Generate Result</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="text-align: center;">
                <asp:Label ID="lblMsg" Font-Bold="true" runat="server" Text=""></asp:Label>
            </div>
            <table width="100%">
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        Session<span class="mandatory">*</span>&nbsp;:&nbsp;
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                            TabIndex="1">
                        </asp:DropDownList>
                        Class <span class="mandatory">*</span>&nbsp;:&nbsp;
                        <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="true" TabIndex="2" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                        </asp:DropDownList>
                        Exam&nbsp;:&nbsp;
                        <asp:DropDownList ID="drpExam" runat="server" TabIndex="3" AutoPostBack="True" OnSelectedIndexChanged="drpExam_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate Result" OnClientClick="return isValid();"
                            OnClick="btnGenerate_Click" TabIndex="4" onfocus="active(this);" onblur="inactive(this);" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfUserid" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="drpSession" />
            <asp:PostBackTrigger ControlID="drpClass" />
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

