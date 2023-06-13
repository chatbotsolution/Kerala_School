<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="LibSubjectMaster.aspx.cs" Inherits="Library_LibSubjectMaster" %>

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
            if (SubNm.trim() == "") {
                alert("Please Fill Subject Name!");
                document.getElementById("<%=txtSubName.ClientID %>").focus();
                document.getElementById("<%=txtSubName.ClientID %>").select();
                return false;
            }
//            if (SubDec.trim() == "") {
//                alert("Please Fill Subject Desciption!");
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
            Add/Modify Subject</h2>
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
                        Category Name:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCatName" runat="server" CssClass="smalltb">
                        </asp:DropDownList>
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
                        Subject Name:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubName" runat="server" Width="200px" MaxLength="30" CssClass="largetb"></asp:TextBox>
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
                        Classification No.:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtClsNo" runat="server" Width="200px" MaxLength="30" CssClass="largetb"></asp:TextBox>
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
                        Description:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtDesc" runat="server" Width="200px" MaxLength="30" CssClass="largetb"></asp:TextBox>
                     
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
</asp:Content>
