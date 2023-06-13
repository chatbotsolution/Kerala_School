<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="SubjectMaster.aspx.cs" Inherits="Library_SubjectMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var category = document.getElementById("<%=ddlCatName.ClientID %>").selectedIndex;
            var SubNm = document.getElementById("<%=txtSubName.ClientID %>").value;
            var SubDec = document.getElementById("<%=txtDesc.ClientID %>").value;
            if (category == 0) {
                alert("Please Select Category !");
                document.getElementById("<%=ddlCatName.ClientID %>").focus();
                return false;
            }
            if (SubNm == "") {
                alert("Please Fill Subject Name!");
                document.getElementById("<%=txtSubName.ClientID %>").focus();
                return false;
            }
            if (SubDec == "") {
                alert("Please Fill Subject Desciption!");
                document.getElementById("<%=txtDesc.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <div class="bedcromb">
        <asp:Label ID="lblTitle" runat="server" Text="Subject Master"></asp:Label></div>
    <table style="height: 480px;" width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" valign="top">
                <div style="padding: 30px;" align="center">
                    <div style="width: 470px; background-color: #666; padding: 2px; margin: 0 auto;">
                        <div style="background-color: #FFF; padding: 10px;">
                            <table cellpadding="0px" cellspacing="0px" align="center" width="100%">
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
                                        <asp:DropDownList ID="ddlCatName" runat="server">                                            
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Subject Name:&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubName" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
                                    </td>
                                </tr>
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
                                        <asp:TextBox ID="txtDesc" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>                                                                                                                    
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="width: 260px">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" Font-Bold="True" OnClientClick="return isValid();"
                                            Font-Size="8pt" Width="60px" OnClick="btnSave_Click" />&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Font-Bold="True" Font-Size="8pt"
                                            Width="60px" OnClick="btnCancel_Click" />&nbsp;
                                        <asp:Button ID="btnShow" runat="server" Text="Show List" Font-Bold="True" Font-Size="8pt"
                                            Width="70px" OnClick="btnShow_Click" />
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td colspan="2">
                                        <asp:HiddenField ID="hfFilePath" runat="server" />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td colspan="2">
                                        <asp:HiddenField ID="hfUserId" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>