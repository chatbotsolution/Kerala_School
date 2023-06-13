<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="BrandMaster.aspx.cs" Inherits="Masters_BrandMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function isValid() {
            var Name = document.getElementById("<%=txtBrandName.ClientID %>").value;

            debugger;

            if (Name.trim() == "") {
                alert("Please Enter Brand Name !");
                document.getElementById("<%=txtBrandName.ClientID %>").focus();
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
                    Brand Master</h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMsg" runat="server" Font-Bold="true"
                    CssClass="gridtxt"></asp:Label>
            </div>
            <table width="100%" border="0" cellspacing="2" cellpadding="2">
                <tbody>
                    <tr>
                        <td class="tbltxt" style="width: 85px">
                            Brand Name
                        </td>
                        <td style="width: 7px">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtBrandName" runat="server" CssClass="largetb" TabIndex="1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" style="width: 85px">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" style="width: 85px">
                            <font face="Verdana">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Submit" TabIndex="2"
                                    OnClientClick="return isValid();"></asp:Button>
                        </td>
                        <td colspan="2">
                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                CausesValidation="False" TabIndex="3"></asp:Button>
                            <asp:Button ID="btngoto" runat="server" CausesValidation="False" OnClick="btngoto_Click"
                                TabIndex="4" Text="Goto List" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
