<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="rptEmployeeList.aspx.cs" Inherits="HR_rptEmployeeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
        function valShow() {
            var EstType = document.getElementById("<%=drpEstType.ClientID %>").value;
            if (EstType == "0") {
                alert("Please select an establishment type !");
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
    </script>

    <div align="center">
        <div class="innerdiv" style="width: 99%">
            <div class="linegap">
                <img src="../images/mask.gif" width="10" height="10" /></div>
            <div style="padding: 8px;">
                <asp:UpdatePanel ID="updtPnl" runat="server">
                    <ContentTemplate>
                        <table width="100%" cellspacing="0">
                            <tr class="filter-background">
                                <td align="left" valign="top" class="tbltxt td-top">
                                    Establishment Type:&nbsp;
                                    <asp:DropDownList ID="drpEstType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpEstType_SelectedIndexChanged"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                </td>
                                <td class="td-help">
                                    ** Please select appropriate establishment dropdown to view the employee list
                                </td>
                            </tr>
                           
                            <tr style="background-color: #D3E7EE;">
                                <td align="left" class="td-bottom-left">
                                    Designation&nbsp;:&nbsp;<asp:DropDownList ID="drpDesig" runat="server" CssClass="tbltxtbox_mid"
                                        TabIndex="2">
                                    </asp:DropDownList>
                                    &nbsp;Employee Name&nbsp;:&nbsp;<asp:TextBox ID="txtEmp" runat="server" Width="200px"
                                        AutoPostBack="True" BackColor="White" CssClass="tbltxtbox" TabIndex="3"></asp:TextBox>
                                </td>
                                <td align="left" class="td-bottom-right">
                                    <asp:Button ID="btnShow" runat="server" Text="Show Report" OnClick="btnShow_Click"
                                        OnClientClick="return valShow();" TabIndex="4" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="5" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click"
                                        TabIndex="6" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center" valign="top" class="tbltxt" style="background-color: #FFC4A6;
                                    padding: 2px; height: 3px; color: #000; border-bottom: 2px solid #333; border-left: 2px solid #333;
                                    border-right: 1px solid #333; padding: 5px 10px 5px 20px;">
                                    <div style="text-align: left; width: 500px;">
                                        ** Select filters till SSVM and click -&gt; <b>Show</b> to view the Employee Details<br />
                                        <span style="padding-left: 190px;">-> <b>Export To Excel</b> to export the list to an
                                            excel sheet</span><br />
                                        <span style="padding-left: 190px;">-> <b>Print</b> to print the list using your printer</span></div>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #333;">
                            <tr>
                                <td align="center" valign="top" class="tbltxt">
                                    <asp:Label runat="server" ID="lblReport" Text="No Records"></asp:Label>
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

