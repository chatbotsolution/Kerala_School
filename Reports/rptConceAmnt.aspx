<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptConceAmnt.aspx.cs" Inherits="Reports_rptConceAmnt" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" language="javascript">

    function popUp(URL) {
        day = new Date();
        id = day.getTime();
        eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=500,left = 390,top = 184');");
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Concession Report
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td class="tbltxt cnt-box">
                        Class :<asp:DropDownList ID="drpForCls" runat="server" Width="100px" CssClass="tbltxtbox"
                            TabIndex="4" AutoPostBack="True" OnSelectedIndexChanged="drpForCls_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; Student :<asp:DropDownList ID="drpStud" runat="server" Width="100px" CssClass="tbltxtbox"
                            TabIndex="4">
                        </asp:DropDownList>
                        &nbsp; From Date :
                        <asp:TextBox ID="txtFromDt" runat="server" CssClass="tbltxtbox" Width="80px" TabIndex="2"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpFrmDt" runat="server" Control="txtFromDt" AutoPostBack="False"
                            Format="dd mmm yyyy"></rjs:PopCalendar>
                        &nbsp; To Date :
                        <asp:TextBox ID="txtToDt" runat="server" CssClass="tbltxtbox" Width="80px" TabIndex="3"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                            Format="dd mmm yyyy"></rjs:PopCalendar>
                        &nbsp;
                        <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return isValid();"
                            TabIndex="5" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="6" />
                        <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                            TabIndex="8" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="tbltxt cnt-box2 lbl2">
                        <asp:Label ID="lblReport" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExpExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


