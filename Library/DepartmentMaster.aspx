<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="DepartmentMaster.aspx.cs" Inherits="Library_DepartmentMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function isValid() {
            var DesigName = document.getElementById("<%=txtDeptName.ClientID %>").value;

            if (DesigName == "") {
                alert("Please Fill Department Name!");
                document.getElementById("<%=txtDeptName.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <div class="bedcromb">
        <asp:Label ID="lblTitle" runat="server" Text="Department Master"></asp:Label></div>
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
                                        Department Name:&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDeptName" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
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
                                        <asp:TextBox ID="txtDesc" runat="server" Width="200px" MaxLength="100"></asp:TextBox>
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
                                            Font-Size="8pt" Width="60px" onclick="btnSave_Click" 
                                            style="height: 22px" />&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Font-Bold="True" Font-Size="8pt"
                                            Width="60px" onclick="btnCancel_Click" />&nbsp;
                                        <asp:Button ID="btnShow" runat="server" Text="Show List" Font-Bold="True" Font-Size="8pt"
                                            Width="70px" onclick="btnShow_Click" />
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
