<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="rptHostDCR.aspx.cs" Inherits="Hostel_rptHostDCR" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>
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
<%--
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_rep.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Fee Collection Report
                </h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <fieldset id="fsCons" runat="server">
                <table width="100%" border="0" cellspacing="2" cellpadding="2">
                    <tr>
                        <td width="130" class="tbltxt">
                            Fee Collection Counter
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="130">
                            <asp:DropDownList ID="drpFeeCounter" runat="server" CssClass="smalltb" AutoPostBack="True"
                                OnSelectedIndexChanged="drpFeeCounter_SelectedIndexChanged" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                        <td width="60" class="tbltxt">
                            From Date
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="120">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="vsmalltb" TabIndex="2"></asp:TextBox>&nbsp;<rjs:PopCalendar
                                ID="dtpFromDate" runat="server" Control="txtFromDate" />
                        </td>
                        <td width="50" class="tbltxt">
                            To Date
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="120">
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="vsmalltb" TabIndex="3"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpToDate" runat="server" Control="txtToDate" />
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
                </table>
            </fieldset>
            <fieldset id="fsStid" runat="server">
                <table width="100%" border="0" cellspacing="2" cellpadding="2">
                    <tr>
                        <td class="tbltxt">
                            Class
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                                OnSelectedIndexChanged="drpClass_SelectedIndexChanged" TabIndex="5">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt">
                            Section
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpSection" runat="server" CssClass="vsmalltb" OnSelectedIndexChanged="drpSection_SelectedIndexChanged1"
                                TabIndex="6" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt">
                            Student
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td class="tbltxt">
                            <asp:DropDownList ID="drpstudents" runat="server" AutoPostBack="True" CssClass="smalltb"
                                OnSelectedIndexChanged="drpstudents_SelectedIndexChanged" TabIndex="4">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt">
                            Search By Student Id &nbsp:&nbsp;
                            <asp:TextBox ID="txtadminno" runat="server" CssClass="vsmalltb" TabIndex="5"></asp:TextBox>
                            &nbsp;<asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click"
                                TabIndex="7" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="8" />
                            <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                                TabIndex="9" />
                        </td>
                        <td colspan="7" class="tbltxt" style="color:Red;" >
                       <b>Note:</b>  For current session leave the From Date & To Date field blank
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div style="padding: 3px; width: 100%; height: 450px; overflow: scroll; border: solid 1px #666;">
                <asp:Label ID="lblReport" runat="server"></asp:Label>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
       <%-- </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExpExcel" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>

