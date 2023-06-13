<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="Designation.aspx.cs" Inherits="HR_Designation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var Designation = document.getElementById("<%=txtDesignation.ClientID %>").value;
            var Type = document.getElementById("<%=drpType.ClientID %>").value;
            if (Designation == "") {
                alert("Enter Designation");
                document.getElementById("<%=txtDesignation.ClientID %>").focus();
                return false;
            }
            else if (Type == "0") {
                alert("Select a Type of Designation");
                document.getElementById("<%=drpType.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Add New Designation</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 400px; background-color: #666; padding: 2px;" align="left">
                <div style="background-color: #FFF; padding: 10px;" align="left">
                    <table>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline">
                                Designation<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtDesignation" runat="server" Height="17px" 
                                    Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline">
                                Type<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:DropDownList ID="drpType" runat="server" Width="200px">
                                    <asp:ListItem Text="- Select -" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Teaching Staff" Value="T"></asp:ListItem>
                                    <asp:ListItem Text="Non-Teaching Staff" Value="N"></asp:ListItem>
                                    <asp:ListItem Text="Management" Value="M"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hfSortOrder" runat="server" Value="0" />
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
                            <td align="left">
                                &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="grpDesignations"
                                    OnClick="btnSubmit_Click" OnClientClick="return isValid();" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                <asp:Button ID="Button3" runat="server" Text="Show List" OnClick="btnShow_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="trMsg" runat="server">
                            <td colspan="2" align="center">
                                <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
