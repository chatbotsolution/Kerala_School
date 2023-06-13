<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="rptPurchaseDetail.aspx.cs" Inherits="Library_rptPurchaseDetail" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=1000,height=500,left = 200,top = 184');");
        }
    </script>

    <asp:UpdatePanel ID="upp1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Purchase Detail Report</h2>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px">
                <table class="tbltxt" cellpadding="2px">
                    <tr>
                        <td>
                            From Date&nbsp;:&nbsp;
                            <asp:TextBox runat="server" ID="txtFromDate" Width="80px" CssClass="smalltb"></asp:TextBox>&nbsp;
                            <rjs:PopCalendar ID="dtpFromDate" runat="server" Control="txtFromDate" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFromDate.value='';return false;"
                                Text="Clear"></asp:LinkButton>
                            &nbsp;To Date&nbsp;:&nbsp;
                            <asp:TextBox runat="server" ID="txtToDate" Width="80px" CssClass="smalltb"></asp:TextBox>&nbsp;
                            <rjs:PopCalendar ID="dtpToDate" runat="server" Control="txtToDate" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            &nbsp;
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtToDate.value='';return false;"
                                Text="Clear"></asp:LinkButton>&nbsp;
                            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="Print" />
                            <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export To Excel" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: left; padding-left: 0px; padding-right: 0px;" class="tbltxt">
                <asp:Label ID="lblReport" runat="server"></asp:Label>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>