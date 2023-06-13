<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="rptLoanAdvancePrint.aspx.cs" Inherits="HR_rptLoanAdvancePrint" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function valid() {
            var Employee = document.getElementById("<%=drpEmp.ClientID %>").selectedIndex;

            if (Employee == 0) {
                alert("Select an Employee");
                document.getElementById("<%=drpEmp.ClientID %>").focus();
                return false;
            }
        }

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=850,height=650,left = 250,top=10');");
        }
    </script>

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
            Loan/Advance Report</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border='1' cellpadding='4' cellspacing='0' style='border: none 0px;'>
                <tr>
                    <td align="left" valign="baseline">
                        Employee Name<font color="red">*</font>&nbsp;:&nbsp;<asp:DropDownList ID="drpEmp"
                            runat="server" OnSelectedIndexChanged="drpEmp_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        &nbsp;From Date&nbsp;:&nbsp;<asp:TextBox ID="txtFromDt" runat="server" Width="80px"></asp:TextBox>&nbsp;
                        <rjs:PopCalendar ID="dtpFromDt" runat="server" Control="txtFromDt"></rjs:PopCalendar>
                        To Date&nbsp;:&nbsp;<asp:TextBox ID="txtToDt" runat="server" Width="80px"></asp:TextBox>&nbsp;
                        <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt"></rjs:PopCalendar>
                        &nbsp;
                        <asp:Button ID="btnSearch" Text="Search" runat="server" onfocus="active(this);" onblur="inactive(this);"
                            TabIndex="2" OnClick="btnSearch_Click" Width="100px"  />
                        <asp:Button ID="btnPrint" Text="Print" runat="server" onfocus="active(this);" onblur="inactive(this);"
                            Visible="false" OnClientClick="javascript:popUp('rptLoanAdvancePrint.aspx')"
                            TabIndex="3" Width="100px" />
                    </td>
                </tr>
            </table>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
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

