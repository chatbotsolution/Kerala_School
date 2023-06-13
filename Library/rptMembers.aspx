<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="rptMembers.aspx.cs" Inherits="Library_rptMembers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function openwindow() {
            var pageurl = "rptMembersPrint.aspx";
            window.open(pageurl, 'true', 'true');
        }
                
    </script>

    <asp:UpdatePanel ID="uppSubList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Library Members</h2>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px">
                <table class="tbltxt">
                    <tr>
                        <td>
                            Member Type :
                            <asp:DropDownList ID="ddlType" runat="server" Width="150px" CssClass="smalltb">
                                <asp:ListItem Text="---All---" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Student" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            Member Name :&nbsp;
                            <asp:TextBox ID="txtName" runat="server" CssClass="smalltb"></asp:TextBox>
                            &nbsp;<asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" />&nbsp;
                            <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click" />&nbsp;
                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="openwindow();" />
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
