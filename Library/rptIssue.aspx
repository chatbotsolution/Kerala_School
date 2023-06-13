<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="rptIssue.aspx.cs" Inherits="Library_rptIssue" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function openwindow() {
            var pageurl = "rptIssuePrint.aspx";
            window.open(pageurl, 'true', 'true');
        }
        
    </script>

    <asp:UpdatePanel ID="uppSubList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Issue Report</h2>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px">
                <table class="tbltxt" cellpadding="1px">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rdbtnlstUsertype" runat="server" AutoPostBack="true" CssClass="tbltxt"
                                RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbtnlstUsertype_SelectedIndexChanged">
                                <asp:ListItem Text="Staff" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Student" Value="1" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Class :
                            <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="true"
                                CssClass="largetb" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged">
                                <asp:ListItem Text="---All---" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp; &nbsp Member :
                            <asp:DropDownList ID="ddlMemberId" runat="server" AutoPostBack="true" Width="220px"
                                CssClass="largetb">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Issue From :
                            <asp:TextBox ID="txtFrmDate" runat="server" CssClass="smalltb"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpFromdt" runat="server" Control="txtFrmDate" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFrmDate.value='';return false;"
                                Text="Clear"></asp:LinkButton>
                            &nbsp To :
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="smalltb"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpTodt" runat="server" Control="txtToDate" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtToDate.value='';return false;"
                                Text="Clear"></asp:LinkButton>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" />&nbsp;
                            <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click" />&nbsp;
                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="openwindow();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: left; padding-left:0px; padding-right:0px;" class="tbltxt">
                <asp:Label ID="lblReport" runat="server"></asp:Label>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

