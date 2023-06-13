<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="rptLeaveClaim.aspx.cs" Inherits="HR_rptLeaveClaim" %>

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

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Leave Claim Report</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset>
                <table width="100%">
                    <tr>
                        <td align="left" valign="baseline">
                            Employee Name&nbsp;:&nbsp;<asp:DropDownList ID="drpEmpName" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="drpEmpName_SelectedIndexChanged" TabIndex="1" Width="250px">
                            </asp:DropDownList>
                            Claim Status&nbsp;:&nbsp;<asp:DropDownList ID="drpStatus" runat="server" AutoPostBack="True"
                                TabIndex="2" OnSelectedIndexChanged="drpStatus_SelectedIndexChanged">
                                <asp:ListItem Text="- ALL -" Value="ALL" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Pendiing" Value="Pending"></asp:ListItem>
                                <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                <asp:ListItem Text="Closed/Paid" Value="Closed"></asp:ListItem>
                                <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" onfocus="active(this);" onblur="inactive(this);"
                                OnClick="btnSearch_Click" />
                            <asp:Button ID="btnPrint" runat="server" Text="Print" Enabled="false" onfocus="active(this);"
                                onblur="inactive(this);" OnClientClick="javascript:window.open('rptLeaveClaimPrint.aspx');" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <br />
            <asp:Label ID="lblReport" runat="server"></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

