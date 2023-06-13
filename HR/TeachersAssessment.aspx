<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="TeachersAssessment.aspx.cs" Inherits="HR_TeachersAssessment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';
        }

        function chk() {
            var sessionYr = document.getElementById("<%=drpSession.ClientID %>").value;

            if (sessionYr == "0") {
                alert("Select a Session");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 3px;">
        <h2>
            Teachers Assessment</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr id="trMsg" runat="server" style="padding: 2px;">
                    <td align="center" colspan="2">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        Session<span class="mandatory">*</span>&nbsp;:
                        <asp:DropDownList ID="drpSession" runat="server" TabIndex="1">
                        </asp:DropDownList>
                        Teacher&nbsp;:&nbsp;<asp:DropDownList ID="drpTeacher" runat="server" TabIndex="2">
                        </asp:DropDownList>
                        <asp:Button ID="btnShow" runat="server" Text="Show" onfocus="active(this);" onblur="inactive(this);"
                            TabIndex="3" OnClick="btnShow_Click" OnClientClick="return chk();" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" onfocus="active(this);" onblur="inactive(this);"
                            Visible="false" TabIndex="4" onclick="btnPrint_Click" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblReport" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShow" />
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>
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
