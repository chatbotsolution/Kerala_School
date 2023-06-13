<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="rptFineRecieved.aspx.cs" Inherits="Library_rptFineRecieved" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=800,height=500,left = 200,top = 184');");
        } 
    </script>
    
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fine Received Report</h2>
    </div>
    <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px">
        <table class="tbltxt" cellpadding="2px">
            <tr>
                <td>
                    Member Type:
                    <asp:DropDownList ID="drpMemType" runat="server" Width="150px" CssClass="smalltb">
                        <asp:ListItem Text="---All---" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Student" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                    <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="Print" />&nbsp;
                    <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export To Excel" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <img src="../images/mask.gif" height="10" width="10" /></div>
    <div style="text-align: left;">
        <asp:Label ID="lblReport" runat="server"></asp:Label>
    </div>
</asp:Content>
