<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="LibStoreEntry.aspx.cs" Inherits="Library_LibStoreEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var StoreNm = document.getElementById("<%=txtStoreName.ClientID %>").value;
            var Location = document.getElementById("<%=txtLocation.ClientID %>").value;

            if (StoreNm.trim() == "") {
                alert("Please Fill Store Name !");
                document.getElementById("<%=txtStoreName.ClientID %>").focus();
                document.getElementById("<%=txtStoreName.ClientID %>").select();
                return false;
            }
            if (Location.trim() == "") {
                alert("Please Fill Store Location !");
                document.getElementById("<%=txtLocation.ClientID %>").focus();
                document.getElementById("<%=txtLocation.ClientID %>").select();
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Add/Modify Store</h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="40" width="10" /></div>
    <div style="width: 470px; background-color: #666; padding: 1px; margin: 0 auto;">
        <div style="background-color: #FFF; padding: 10px;">
            <table cellpadding="0px" cellspacing="0px" align="center" width="100%" class="tbltxt">
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Store Name:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtStoreName" runat="server" Width="200px" MaxLength="30" CssClass="largetb"></asp:TextBox>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Store Location:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtLocation" runat="server" Width="200px" MaxLength="30" CssClass="largetb"></asp:TextBox>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnSaveAddNew" runat="server" Text="Save & AddNew" Font-Bold="True"
                            OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveAddNew_Click" />&nbsp;
                        <asp:Button ID="btnSaveGotoList" runat="server" Text="Save & GotoList" Font-Bold="True"
                            OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveGotoList_Click" />&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" Font-Bold="True" Font-Size="8pt"
                            Width="60px" OnClick="btnCancel_Click" />&nbsp;
                        <asp:Button ID="btnShow" runat="server" Text="Back" Font-Bold="True" Font-Size="8pt"
                            Width="70px" OnClick="btnShow_Click" />
                    </td>
                </tr>                
            </table>
        </div>
    </div>
</asp:Content>
