<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="BookWritesOff.aspx.cs" Inherits="Library_BookWritesOff" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var categoryNm = document.getElementById("<%=ddlCatName.ClientID %>").selectedIndex;
            var SubNm = document.getElementById("<%=ddlSubject.ClientID %>").selectedIndex;
            var AccessionNo = document.getElementById("<%=ddlAccessionNo.ClientID %>").selectedIndex;
            var date = document.getElementById("<%=txtWriteOffDt.ClientID %>").value;
            var Reason = document.getElementById("<%=ddlReason.ClientID %>").selectedIndex;
            var description = document.getElementById("<%=txtDescription.ClientID %>").value;

            if (categoryNm == 0) {
                alert("Please Select Category !");
                document.getElementById("<%=ddlCatName.ClientID %>").focus();
                return false;
            }
            if (SubNm == 0) {
                alert("Please Select Subject !");
                document.getElementById("<%=ddlSubject.ClientID %>").focus();
                return false;
            }
            if (AccessionNo == 0) {
                alert("Please Select Accession Number !");
                document.getElementById("<%=ddlAccessionNo.ClientID %>").focus();
                return false;
            }
            if (date.trim() == "") {
                alert("Please Select Date !");
                document.getElementById("<%=txtWriteOffDt.ClientID %>").focus();
                document.getElementById("<%=txtWriteOffDt.ClientID %>").select();
                return false;
            }
            if (Reason == 0) {
                alert("Please Select Reason !");
                document.getElementById("<%=ddlReason.ClientID %>").focus();
                return false;
            }
            if (description.trim() == "") {
                alert("Please Write Description within 100 characters !");
                document.getElementById("<%=txtDescription.ClientID %>").focus();
                document.getElementById("<%=txtDescription.ClientID %>").select();
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <asp:UpdatePanel ID="upp1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Add/Cancel Book WritesOff</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="40" width="10" /></div>
            <div style="width: 450px; background-color: #666; padding: 1px; margin: 0 auto;">
                <div style="background-color: #FFF; padding: 10px;">
                    <table cellpadding="0px" cellspacing="0px" width="100%" class="tbltxt">
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Category :&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCatName" runat="server" AutoPostBack="true" CssClass="largetb"
                                    OnSelectedIndexChanged="ddlCatName_SelectedIndexChanged">
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
                                Subject :&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSubject" runat="server" AutoPostBack="true" CssClass="largetb"
                                    OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                                    <asp:ListItem Text="---Select---" Value="0" Selected="True"></asp:ListItem>
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
                                Accession No:&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAccessionNo" runat="server" CssClass="largetb">
                                    <asp:ListItem Text="---Select---" Value="0" Selected="True"></asp:ListItem>
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
                                WriteOff Date:&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtWriteOffDt" runat="server" MaxLength="30" CssClass="smalltb"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpWriteOffDt" runat="server" Control="txtWriteOffDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtWriteOffDt.value='';return false;"
                                    Text="Clear" ></asp:LinkButton>
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
                                Reason :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlReason" runat="server" CssClass="smalltb">
                                    <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Lost" Value="L"></asp:ListItem>
                                    <asp:ListItem Text="Torn Off" Value="T"></asp:ListItem>
                                    <asp:ListItem Text="Gifted" Value="G"></asp:ListItem>
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
                            <td valign="top">
                                Description:&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" Width="250px" Height="50px" MaxLength="100"
                                    CssClass="largeta" TextMode="MultiLine"></asp:TextBox>
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
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>