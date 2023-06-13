<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.master" AutoEventWireup="true" CodeFile="Restore.aspx.cs" Inherits="Administrations_Restore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">


        function isValid1() {
            var database = document.getElementById("<%=drpDatabase.ClientID %>").value;
            var filechoosen = document.getElementById("<%=Restore.ClientID %>").value;

            if (database == 0) {
                alert("Please Select a database !");
                document.getElementById("<%=drpDatabase.ClientID %>").focus();
                return false;
            }
            if (filechoosen == 0) {
                alert("Please Select a file !");
                document.getElementById("<%=Restore.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }  
    
    </script>
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_admin.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Restore Database</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table cellspacing="0" cellpadding="3" width="50%" border="0" class="tbltxt">
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="tbltxt" width="200px">
                Select Database to be Restored
            </td>
            <td width="5px" class="tbltxt">
                :
            </td>
            <td>
                <asp:DropDownList ID="drpDatabase" runat="server" CssClass="largetb1">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;&nbsp;&nbsp; &nbsp;
            </td>
        </tr>
        <tr>
            <td class="tbltxt" width="200px">
                Select Backup File Location
            </td>
            <td width="5px" class="tbltxt">
                :
            </td>
            <td>
                <asp:FileUpload ID="Restore" runat="server"  />
                <asp:Label ID="lblFile" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left" colspan="3">
                <asp:Label runat="server" ID="Lblmsg" CssClass="error"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnRestore" runat="server" Text="Restore" Width="150px" OnClick="btnRestore_Click"
                    OnClientClick="return isValid1();" />
            </td>
        </tr>
    </table>
</asp:Content>

