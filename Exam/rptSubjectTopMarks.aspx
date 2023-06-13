<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="rptSubjectTopMarks.aspx.cs" Inherits="Exam_rptSubjectTopMarks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';
        } 
    </script>

    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="height: 100%;">
        <tr>
            <td align="center" valign="top">
                <br />
                <div align="center">
                    <div class="innerdiv" style="width: 80%">
                        <div class="linegap">
                            <img src="../images/mask.gif" width="10" height="10" /></div>
                        <div style="padding: 8px;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table cellspacing="0" cellpadding="0" width="100%">
                                        <tr style="background-color: #D3E7EE;">
                                            <td align="center" class="tbltxt" colspan="4" style="padding: 5px; font-weight: bold;
                                                height: 3px; font-family: Tahoma, Geneva, sans-serif; color: #000; border: 1px solid #333;
                                                background-color: transparent;">
                                                <b>Session Year :<asp:DropDownList ID="drpSession" runat="server" Width="150px" AutoPostBack="True"
                                                    CssClass="tbltxtbox" BackColor="White">
                                                </asp:DropDownList>
                                                    &nbsp;&nbsp; Exam Name:<asp:DropDownList ID="drpExamName" runat="server" Width="150px"
                                                        AutoPostBack="True" CssClass="tbltxtbox" BackColor="White" OnSelectedIndexChanged="drpExamName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp; Subject :
                                                    <asp:DropDownList ID="drpSubject" runat="server" CssClass="tbltxtbox" BackColor="White">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="CheckLoader()"
                                                        OnClick="btnSearch_Click" />
                                                    &nbsp;&nbsp;<asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click" />
                                                    &nbsp;<asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
                                                </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tbltxt">
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                            </td>
                                            <td align="left" valign="top">
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lblRecCount" CssClass="totalrec" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="top" class="tbltxt" colspan="4">
                                                <asp:Label ID="lblReport" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                               <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExport" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
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

