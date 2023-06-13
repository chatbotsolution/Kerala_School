<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="ItemCatagory.aspx.cs" Inherits="Masters_ItemCatagory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            document.getElementById("<%=btnsubmit.ClientID %>").focus();
        });
        function isValid() {
            var Name = document.getElementById("<%=txtcatname.ClientID %>").value;

            debugger;

            if (Name.trim() == "") {
                alert("Please Enter Catagory Name !");
                document.getElementById("<%=txtcatname.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }

        }       
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Item Category</h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMsg" runat="server" Font-Bold="true"
                    CssClass="gridtxt"></asp:Label>
            </div>
            <table width="100%" border="0" cellspacing="2" cellpadding="2">
                <tbody>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt">
                            Parent Category:
                        </td>
                        <td class="labeltxt" align="left">
                            <asp:DropDownList ID="dptcat" TabIndex="1" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt">
                            Category Name:
                        </td>
                        <td class="labeltxt" align="left">
                            <asp:TextBox ID="txtcatname" TabIndex="2" runat="server" CssClass="largetb"></asp:TextBox>&nbsp;<span
                                style="color: Red; font-size: small;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="font-family: Times New Roman">
                        <td colspan="2" align="left">
                            <asp:Button ID="btnsubmit" TabIndex="3" OnClick="Submit_Click" runat="server" Text="Submit"
                                ValidationGroup="Insert" OnClientClick="return isValid();"></asp:Button>
                            <asp:Button ID="btncancel" TabIndex="4" OnClick="btncancel_Click" runat="server"
                                Text="Cancel" CausesValidation="false"></asp:Button>
                            <asp:Button ID="btnGo" TabIndex="5" OnClick="btnGo_Click" runat="server" Text="Goto List"
                                CausesValidation="false"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 116px">
                            <asp:HiddenField ID="hdtype" runat="server" />
                        </td>
                        <td style="width: 100px">
                            <asp:HiddenField ID="hdCatId" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>