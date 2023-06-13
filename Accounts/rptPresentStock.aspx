<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptPresentStock.aspx.cs" Inherits="Accounts_rptPresentStock" %>

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
    </script>

    <%--<asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnPrint" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
        <ContentTemplate>--%>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor">
                            <h1>
                                Consolidated
                            </h1>
                            <h2>
                                Stock</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table cellspacing="0" cellpadding="6" width="100%" border="0">
                            <tr>
                                <td align="left">
                                    Accounting Yr:&nbsp;<asp:DropDownList ID="drpSession" runat="server" 
                                        AutoPostBack="true" 
                                        onselectedindexchanged="drpSession_SelectedIndexChanged" >
                                    </asp:DropDownList>&nbsp;&nbsp;
                                    &nbsp;Category&nbsp;:
                                    <asp:DropDownList ID="drpCat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpCat_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    Class &nbsp;:
                                    <asp:DropDownList ID="drpClass" runat="server" Enabled="false">
                                    </asp:DropDownList>
                                    &nbsp;
                                    From Date&nbsp;:
                                    <asp:TextBox ID="txtFrmDt" runat="server" Width="77px"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFrmDt" Format="dd MMM yyyy">
                                    </rjs:PopCalendar>
                                    &nbsp;
                                    To Date&nbsp;:
                                    <asp:TextBox ID="txtToDt" runat="server" Width="77px"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" Format="dd MMM yyyy">
                                    </rjs:PopCalendar>
                                    &nbsp;
                                    <asp:Button ID="btnView" runat="server" Text="View List" OnClick="btnView_Click"
                                        Width="110px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    
                                        &nbsp;
                                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" 
                                        Width="110px" style="height: 26px" />
                                    &nbsp;
                                    <asp:Button ID="btnExcel" runat="server" Text="Export To Excel" OnClick="btnExcel_Click" />
                                </td>
                                </tr>
                            <tr>
                                <td colspan="2" style="width: 16%; ">
                                    <asp:Label ID="lblReport" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
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
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
