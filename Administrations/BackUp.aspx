<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.master" AutoEventWireup="true" CodeFile="BackUp.aspx.cs" Inherits="Administrations_BackUp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function isValid() {
            var database = document.getElementById("<%=drpDatabase.ClientID %>").value;

            if (database == 0) {
                alert("Please Select a database !");
                document.getElementById("<%=drpDatabase.ClientID %>").focus();
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
            Backup Database</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table cellspacing="0" cellpadding="3" width="100%" border="0" class="tbltxt">
       
        <tr>
            <td class="tbltxt" style="width: 115px">
                &nbsp;</td>
            <td width="5" class="tbltxt">
                &nbsp;</td>
            <td>
                &nbsp;&nbsp;&nbsp; &nbsp;</td>
        </tr>
        <tr>
            <td class="tbltxt" style="width: 115px">
                Select Database
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td>
                <asp:DropDownList ID="drpDatabase" runat="server" CssClass="largetb1">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="3">
                <asp:Label runat="server" ID="Lblmsg" CssClass="error"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 115px">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>

            <asp:Button ID="btnBackUp" runat="server" Text="BackUp" 
                        OnClick="btnBackUp_Click" OnClientClick="return isValid();" CssClass="btn" />
               
            </td>
        </tr>
    </table>
</asp:Content>
