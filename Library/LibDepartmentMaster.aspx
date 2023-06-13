<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="LibDepartmentMaster.aspx.cs" Inherits="Library_LibDepartmentMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function isValid() {
            var DeptName = document.getElementById("<%=txtDeptName.ClientID %>").value;

            if (DeptName == "") {
                alert("Please Fill Department Name!");
                document.getElementById("<%=txtDeptName.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>    
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
                                        <span style="color:Red; font-size:small;">*</span>
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
                                    <td colspan="2" align="center">                                            
                                        <asp:Button ID="btnSaveAddNew" runat="server" Text="Save & AddNew" Font-Bold="True" 
                                            OnClientClick="return isValid();" Font-Size="8pt" Width="120px" onclick="btnSaveAddNew_Click" />&nbsp;
                                        <asp:Button ID="btnSaveGotoList" runat="server" Text="Save & GotoList" Font-Bold="True" 
                                            OnClientClick="return isValid();" Font-Size="8pt" Width="120px" onclick="btnSaveGotoList_Click"/>&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Clear" Font-Bold="True" Font-Size="8pt"
                                            Width="60px" onclick="btnCancel_Click" />&nbsp;
                                        <asp:Button ID="btnShow" runat="server" Text="Back" Font-Bold="True" Font-Size="8pt"
                                            Width="70px" onclick="btnShow_Click" />                                            
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
                </div>
            </td>
        </tr>
    </table>        
</asp:Content>

