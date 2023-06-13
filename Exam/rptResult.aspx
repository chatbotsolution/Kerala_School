<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="rptResult.aspx.cs" Inherits="Exam_rptResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function isValid() {
            var Session = document.getElementById("<%=drpSession.ClientID %>").selectedIndex;
            var Exam = document.getElementById("<%=drpExam.ClientID %>").selectedIndex;
            var Class = document.getElementById("<%=drpClass.ClientID %>").selectedIndex;

            if (Session == "0") {
                alert("Select a Session");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            if (Class == "0") {
                alert("Select a Class");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            else {
                CheckLoader();
                return true;
            }
        }
        function clearLbl() {
            document.getElementById("<%=lblReport.ClientID %>").innerHTML = "";
            document.getElementById("<%=btnExport.ClientID %>").disabled = true;
            document.getElementById("<%=btnPrint.ClientID %>").disabled = true;
        }
        function openwindow() {
            var pageurl = "rptResultPrint.aspx";
            window.open(pageurl, 'true', 'true');
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
            Exam Result</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" cellpadding="1px" style="background-color: #D3E7EE; outline-style: solid;
                outline-width: 1px">
                <tr>
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;">
                        Session&nbsp;:&nbsp;
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                        </asp:DropDownList>
                        Class&nbsp;:&nbsp;
                        <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                        </asp:DropDownList>
                        Section&nbsp;:&nbsp;
                        <asp:DropDownList ID="drpSection" runat="server">
                        </asp:DropDownList>
                        &nbsp;Examination&nbsp;:&nbsp;<asp:DropDownList ID="drpExam" runat="server" 
                            onchange="clearLbl();" AutoPostBack="True" 
                            onselectedindexchanged="drpExam_SelectedIndexChanged">
                        </asp:DropDownList>
                        Status&nbsp;:&nbsp;
                        <asp:DropDownList ID="drpStatus" runat="server" onchange="clearLbl();" 
                            Enabled="False">
                            <asp:ListItem Selected="True" Value="0" Text="- ALL -"></asp:ListItem>
                            <asp:ListItem Value="PASS" Text="PASS"></asp:ListItem>
                            <asp:ListItem Value="FAIL" Text="FAIL"></asp:ListItem>
                            <asp:ListItem Value="ABSENT" Text="ABSENT"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;">
                        <asp:Button ID="btnShow" runat="server" Text="Show" OnClientClick="return isValid();"
                            OnClick="btnShow_Click" onfocus="active(this);" onblur="inactive(this);" />
                        <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click"
                            onfocus="active(this);" onblur="inactive(this);" />
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
            <asp:PostBackTrigger ControlID="btnExport" />
            <asp:PostBackTrigger ControlID="drpClass" />
            <asp:PostBackTrigger ControlID="btnShow" />
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

