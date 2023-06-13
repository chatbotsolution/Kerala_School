<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptPOStatus.aspx.cs" Inherits="Accounts_rptPOStatus" %>

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
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=600,left = 290,top = 184');");
        } 
    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnPrint" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div>
                            <h1>
                                Purchase
                            </h1>
                            <h2>
                                Order Status</h2>
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
                                <td style="vertical-align: middle; text-align: left; width: 80%;" valign="top">                                    
                                    &nbsp;Order From Date&nbsp; :&nbsp;
                                    <asp:TextBox ID="txtFrmDt" runat="server" Width="80px"></asp:TextBox>&nbsp;
                                    <rjs:PopCalendar ID="dtpFrmDt" runat="server" Control="txtFrmDt"></rjs:PopCalendar>
                                    <asp:LinkButton ID="lnkbtnFrmDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFrmDt.value='';return false;"
                    Text="Clear" ></asp:LinkButton>
                                    &nbsp;To Date&nbsp; :&nbsp;
                                    <asp:TextBox ID="txtToDt" runat="server" Width="80px"></asp:TextBox>&nbsp;
                                    <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt"></rjs:PopCalendar>
                                    <asp:LinkButton ID="lnkbtnToDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtToDt.value='';return false;"
                    Text="Clear" ></asp:LinkButton>
                                    &nbsp; Supplier List&nbsp;:&nbsp<asp:DropDownList ID="drpSupplier" runat="server"
                                        Width="300px">
                                    </asp:DropDownList>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:Button ID="btnView" runat="server" Text="View List" OnClick="btnView_Click"
                                        Width="120px" />
                                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" Width="120px" />
                                    <asp:Button ID="btnExcel" runat="server" Text="Export To Excel" OnClick="btnExcel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

