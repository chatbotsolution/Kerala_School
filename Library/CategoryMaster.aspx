<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="CategoryMaster.aspx.cs" Inherits="Library_CategoryMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var CatName = document.getElementById("<%=txtCatName.ClientID %>").value;
            var CatDesc = document.getElementById("<%=txtDesc.ClientID %>").value;

            if (CatName.trim() == "") {
                alert("Please Fill Category Name!");
                document.getElementById("<%=txtCatName.ClientID %>").focus();
                document.getElementById("<%=txtCatName.ClientID %>").select();
                return false;
            }
//            if (CatDesc.trim() == "") {
//                alert("Please Fill Category Desciption!");
//                document.getElementById("<%=txtDesc.ClientID %>").focus();
//                document.getElementById("<%=txtDesc.ClientID %>").select();
//                return false;
//            }
            else {
                return true;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Add/Modify Category</h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="40" width="10" /></div>
        <asp:UpdatePanel ID="upp1" runat="server">
        <ContentTemplate>
    <div style="width: 470px; background-color: #666; padding: 2px; margin: 0 auto;">
        <div style="background-color: #FFF; padding: 10px;">
            <table cellpadding="0px" cellspacing="0px" align="center" width="100%" class="tbltxt">
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Category Name:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtCatName" runat="server" MaxLength="30" CssClass="largetb"></asp:TextBox>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
               <%-- <tr>
                    <td>
                        Category Code:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtCatInShort" runat="server" MaxLength="30" CssClass="largetb"></asp:TextBox>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Description:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtDesc" runat="server" MaxLength="30" CssClass="largetb"></asp:TextBox>
                       
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <%--<tr>
                    <td>
                        Category Image:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:FileUpload ID="fuCatImg" runat="server" />
                    </td>
                </tr>--%>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblImg" runat="server" Text=""></asp:Label>
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
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="hfUserId" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>