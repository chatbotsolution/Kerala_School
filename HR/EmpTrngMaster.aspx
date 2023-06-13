<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpTrngMaster.aspx.cs" Inherits="HR_EmpTrngMaster" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var Name = document.getElementById("<%=txtName.ClientID %>").value;
            var FromDt = document.getElementById("<%=txtFrom.ClientID %>").value;
            var ToDt = document.getElementById("<%=txtTo.ClientID %>").value;

            if (Name.trim() == "") {
                alert("Enter Training Name");
                document.getElementById("<%=txtName.ClientID %>").focus();
                return false;
            }
            else if (FromDt.trim() == "") {
                alert("Select Training Starting Date");
                document.getElementById("<%=txtFrom.ClientID %>").focus();
                return false;
            }
            else if (ToDt.trim() == "") {
                alert("Select Training End Date");
                document.getElementById("<%=txtTo.ClientID %>").focus();
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
            Employee Training Master</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
<table>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                                <asp:Label runat="server" ID="Label1" ForeColor="White" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline">
                                Training Name<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtName" runat="server" Height="17px" MaxLength="100"
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
                                Place</td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtPlace" runat="server" Height="17px" MaxLength="100"
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
                                From<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtFrom" runat="server" Height="17px" 
                                    Width="100px"></asp:TextBox>
                                    <rjs:popcalendar ID="dtpFrom" runat="server" AutoPostBack="False" Control="txtFrom" 
                                    Format="dd mmm yyyy" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="baseline">
                                To<font color="red">*</font>
                            </td>
                            <td align="left" valign="baseline">
                                :&nbsp;<asp:TextBox ID="txtTo" runat="server" Height="17px" 
                                    Width="100px"></asp:TextBox>
                                <rjs:popcalendar ID="dtpTo" runat="server" AutoPostBack="False" Control="txtTo" 
                                Format="dd mmm yyyy" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                Details</td>
                            <td align="left" valign="top">
                                :&nbsp;<asp:TextBox ID="txtDetails" runat="server" Height="50px" TextMode="MultiLine"  Width="200px" MaxLength="199"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                Status<font color="red">*</font>
                            </td>
                            <td align="left" valign="top">
                                <asp:RadioButtonList ID="rbtnStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem>Active</asp:ListItem>
                                    <asp:ListItem>Inactive</asp:ListItem>
                                </asp:RadioButtonList>
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
                                &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                                     OnClientClick="return isValid();" onclick="btnSubmit_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                                    onclick="btnCancel_Click"  />
                                <asp:Button ID="btnShow" runat="server" Text="Show List" onclick="btnShow_Click" 
                                    />
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
</asp:Content>
    
