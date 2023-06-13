<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="ItemLocationMaster.aspx.cs" Inherits="Masters_ItemLocationMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function isValid() {
            var LocName = document.getElementById("<%=txtLocName.ClientID %>").value;
            var LocIn = document.getElementById("<%=drpLocInCharge.ClientID %>").value;

            debugger;

            if (LocName.trim() == "") {
                alert("Please Enter Location Name !");
                document.getElementById("<%=txtLocName.ClientID %>").focus();
                return false;
            }
            if (LocIn.trim() == 0) {
                alert("Please Select Location Incharge !");
                document.getElementById("<%=drpLocInCharge.ClientID %>").focus();
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
            Store Location</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
            
    </div>
    
            <table width="100%" border="0" cellspacing="2" cellpadding="2">
                <tbody>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" style="width: 121px">
                            Location Name:
                        </td>
                        <td valign="top" align="left">
                            <asp:TextBox ID="txtLocName" runat="server" CssClass="largetb" TabIndex="1"></asp:TextBox><span
                                style="color: Red; font-size: small;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" style="width: 121px">
                            Location Incharge:
                        </td>
                        <td valign="top" align="left">
                            <asp:DropDownList ID="drpLocInCharge" runat="server" CssClass="largetb" TabIndex="2">
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
                        <td align="left" valign="top" colspan="2">
                            <font face="Verdana">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Submit" TabIndex="4"
                                    OnClientClick="return isValid();"></asp:Button>
                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                    CausesValidation="False" TabIndex="5"></asp:Button>
                                <asp:Button ID="btngoto" OnClick="btngoto_Click" runat="server" Text="Goto List"
                                    CausesValidation="False" TabIndex="6"></asp:Button>
                            </font>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

