<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptSalesDly.aspx.cs" Inherits="Accounts_rptSalesDly" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function beginRequest(sender, args) {
            // show the popup
            $find('<%=mdlloading.ClientID %>').show();

        }

        function endRequest(sender, args) {
            //  hide the popup
            $find('<%=mdlloading.ClientID %>').hide();

        }
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=800,height=600,left = 100,top = 100');");
        } 
    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnPrint" EventName="Click" />
            <asp:PostBackTrigger ControlID="BtnExcel" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="4">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor">
                            <h1>
                                Sales
                            </h1>
                            <h2>
                                Report</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <strong>&nbsp;
                            <asp:Label ID="lblToDt" runat="server" Text="Sales To Date : "></asp:Label>
                            <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" CssClass="tbltxtbox" Width="80px"
                                TabIndex="1"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <asp:Label ID="lblFrmDt" runat="server" Text="Sales From Date : "></asp:Label>
                            <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" CssClass="tbltxtbox" Width="80px"
                                TabIndex="2"></asp:TextBox>
                            <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            &nbsp;&nbsp;&nbsp; </strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Button ID="btnView" runat="server" TabIndex="3" Text="View Report" OnClick="btnView_Click" />
                        &nbsp;
                        <asp:Button ID="btnPrint" Text="Print" TabIndex="4" runat="server" OnClick="btnPrint_Click" />
                        &nbsp;
                        <asp:Button ID="btnExcel" TabIndex="5" runat="server" Text="Export to Excel" OnClick="btnExpExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblReport" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../Images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


