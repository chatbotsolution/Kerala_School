<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptDCRMix.aspx.cs" Inherits="Reports_rptDCRMix" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" language="javascript">
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
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_rep.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Daily Collection Report(DCR)
                </h2>
            </div>
           
            <fieldset id="fsCons" runat="server" class="cnt-box">
                <table width="100%" border="0" cellspacing="2" cellpadding="2">
                 <tr>
                        <td class="tbltxt" align="left" colspan="10" style="border-bottom:2px solid #fff; padding-bottom:10px; ">
                            <asp:RadioButton ID="rbtnConsol" runat="server" GroupName="Chk1" Text="Consolidated"
                                CssClass="tbltxt" AutoPostBack="True" 
                                oncheckedchanged="rbtnConsol_CheckedChanged" />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="rbtnFeeHead" runat="server" GroupName="Chk1" Text="Headwise"
                                CssClass="tbltxt" AutoPostBack="True" Checked="true" />
                        </td>
                        
                    </tr>
                    <tr>
                        <td width="130" class="tbltxt">
                            Fee Collection Counter
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="170">
                            <asp:DropDownList ID="drpFeeCounter" runat="server" CssClass="vsmalltb" AutoPostBack="True"
                                 TabIndex="1" onselectedindexchanged="drpFeeCounter_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="70" class="tbltxt">
                            Select Date
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="180">
                            <asp:TextBox ID="txtDate" runat="server" CssClass=" " TabIndex="2"></asp:TextBox>&nbsp;<rjs:PopCalendar
                                ID="dtpDate" runat="server" Control="txtDate" />
                        </td>
                        <td class="tbltxt">
                            Payment Mode :
                            <asp:DropDownList ID="drpPmtMode" runat="server" CssClass="vsmalltb" onclick="return ViewChequeDetails();"
                                TabIndex="4">
                                <asp:ListItem Value="0">All</asp:ListItem>
                                <asp:ListItem>Cash</asp:ListItem>
                                <asp:ListItem>Bank</asp:ListItem>
                                <asp:ListItem>Cheque</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" align="left">
                            <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" TabIndex="7" />
                            <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                                TabIndex="9"  />
                                <asp:Button ID="btnCancelled" runat="server" Text="Show Cancelled Receipt"
                                TabIndex="10" onclick="btnCancelled_Click" />
                        </td>
                       
                    </tr>
                </table>
            </fieldset>
             <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <div style="height: 450px; overflow: auto;" class="cnt-box2 tbltxt">
                 <asp:Label ID="lblFee" runat="server"></asp:Label> 
                            <div class="spacer"></div>
                <asp:Label ID="lblAccHd" runat="server"></asp:Label>
             <div class="spacer"></div>
                <asp:Label ID="lblAdj" runat="server"></asp:Label>
            <div class="spacer"></div>
                <asp:Label ID="lblTotal" runat="server"></asp:Label>
                <div class="spacer"></div>
                <asp:Label ID="lblSaleRet" runat="server"></asp:Label>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />--%>
            <asp:PostBackTrigger ControlID="btnExpExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

