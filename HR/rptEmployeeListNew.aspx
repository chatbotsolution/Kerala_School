<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="rptEmployeeListNew.aspx.cs" Inherits="HR_rptEmployeeListNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>

        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';
        }
        {
            CheckLoader();

        }
    </script>
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Staff List</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <div align="center">
        <div class="innerdiv" style="width: 100%">
            <div class="linegap">
                <img src="../images/mask.gif" width="10" height="10" /></div>
            <div style="padding: 8px;">
                <asp:UpdatePanel ID="updtPnl" runat="server">
                    <ContentTemplate>
                        <table width="100%" cellspacing="0">
                            <tr style="background-color: #D3E7EE;" class="tbltxt">
                                <td align="left" style="padding: 5px; height: 3px; color: #000; border: 2px solid #333;">
                                    Designation&nbsp;:&nbsp;<asp:DropDownList ID="drpDesig" runat="server" CssClass="tbltxtbox_mid"
                                        TabIndex="2">
                                    </asp:DropDownList>
                                    &nbsp;Employee Name&nbsp;:&nbsp;
                                    <asp:TextBox ID="txtSevabratiName" runat="server" TabIndex="6"></asp:TextBox>
                                    &nbsp;&nbsp; Qualification&nbsp;:&nbsp;
                                    <asp:DropDownList ID="drpEduQual" runat="server" CssClass="tbltxtbox_mid">
                                    </asp:DropDownList>
                                     Service&nbsp;:&nbsp;<asp:DropDownList ID="drpStatus" runat="server" CssClass="tbltxtbox_mid"
                                        TabIndex="2">
                                        <asp:ListItem Text="--All--" Value="--All--"></asp:ListItem>
                                        <asp:ListItem Text="In Service" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Out Of Service" Value="0"></asp:ListItem>
                                    </asp:DropDownList>&nbsp;
                                    <asp:Button ID="btnShow" runat="server" Text="Show Report" OnClick="btnShow_Click"
                                        OnClientClick="return CheckLoader();" TabIndex="7" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click"
                                        TabIndex="8" />
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top" class="tbltxt" style="background-color: #FFC4A6;
                                    padding: 2px; height: 3px; color: #000; border-bottom: 2px solid #333; border-left: 2px solid #333;
                                    border-right: 1px solid #333; padding: 5px 10px 5px 20px;">
                                    <div style="text-align: left; width: 800px; height: 14px;">
                                        ** Select filters and click -&gt; <b>Show</b> to view the Employee Details
                                        &amp; click -&gt; <b>Export To Excel</b> to export the list to an excel sheet</span><br />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="center" valign="top" class="tbltxt">
                                    <asp:Label runat="server" ID="lblReport" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExport" />
                        <asp:PostBackTrigger ControlID="btnShow" />
                    </Triggers>
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
