<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="SchoolMCMasterEdit.aspx.cs" Inherits="HR_SchoolMCMasterEdit" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function clearText(btn) {
            switch (btn) {
                case '1':
                    {
                        document.getElementById("<%=txtToDt.ClientID %>").value = "";
                        return false;
                    }
                case '2':
                    {
                        document.getElementById("<%=txtFromDt.ClientID %>").value = "";
                        return false;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        function CnfSave() {

            if (confirm("You are going to Replace an Existing Member of the Managing Committe. Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }  
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Modify Managing Commitee</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <fieldset>
                <table align="center" width="100%">
                    <tr>
                        <td align="left" valign="top">
                            <b>Approval Letter No </b>
                        </td>
                        <td align="left" valign="top">
                            &nbsp;:&nbsp;
                            <asp:Label ID="lblApLtrNo" runat="server"></asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <b>Approval Date </b>
                        </td>
                        <td align="left" valign="top">
                            &nbsp;:&nbsp;
                            <asp:Label ID="lblAprDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <b>Effective From Date</b>
                        </td>
                        <td align="left" valign="top">
                            &nbsp;:&nbsp;
                            <asp:Label ID="lblStartDt" runat="server"></asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <b>Approval End Date </b>
                        </td>
                        <td align="left" valign="top">
                            &nbsp;:&nbsp;
                            <asp:Label ID="lblEndDt" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div style="float: left; width: 50%;">
                <fieldset align="center">
                    <legend style="font-weight: bold">Existing Member Details</legend>
                    <table align="center" width="100%" height="200px">
                        <tr>
                            <td align="left" valign="top">
                                <b>Designation</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:Label ID="lblDesg" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <b>Name</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:Label ID="lblName" runat="server" Width="300px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <b>Membership Start Date</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:Label ID="lblFromDate" runat="server" Width="100px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <b>Membership End Date</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:TextBox ID="txtToDt" runat="server" Width="100px"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" Format="dd mmm yyyy">
                                </rjs:PopCalendar>
                                <asp:LinkButton ID="lnkToDt" runat="server" Text="Clear" OnClientClick="return clearText('1');" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <b>Contact No</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:Label ID="lblContactNo" runat="server" Width="300px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <b>Email Id</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:Label ID="lblEmail" runat="server" Width="300px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <b>Address</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:Label ID="lblAddress" runat="server" Width="300px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div style="float: right; width: 50%;">
                <fieldset align="center" style="font-weight: bold">
                    <legend>New Member Details</legend>
                    <table align="center" width="100%" height="200px">
                        <tr>
                            <td align="left" valign="top">
                                <b>Designation</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:TextBox ID="txtDesg" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="hfDesgId" runat="server" Value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <b>Name</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:TextBox ID="txtName" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <b>From Date</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:TextBox ID="txtFromDt" runat="server" Width="100px"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpFromDt" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
                                </rjs:PopCalendar>
                                <asp:LinkButton ID="lnkFromDt" runat="server" Text="Clear" OnClientClick="return clearText('2');" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <b>Contact No</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:TextBox ID="txtContact" runat="server" Width="100px" onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <b>Email Id</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <b>Address</b>
                            </td>
                            <td align="left" valign="top">
                                &nbsp;:&nbsp;<asp:TextBox ID="txtAddress" runat="server" Width="200px" Height="40px"
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <table width="100%">
                <tr>
                    <td align="center" valign="top">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="70px" OnClick="btnSave_Click"
                            OnClientClick="return CnfSave();" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="70px" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
            <div style="margin: 5px; padding: 10px; background-color: #E9FFDA; border: solid 1px #C0EDDA;
                font-weight: bold; text-align: left; color: #00514D; box-shadow: 3px 3px 5px #888888;">
                <div id="msg">
                    <asp:Label Text="" runat="server" ID="litMsg"></asp:Label>
                </div>
                Note :-<br />
                1. To end the Membership of a Member enter the <b style="color:Red">Membership End Date</b><br />
                2. If you want to replace another Member with the Existing Member, then fill all
                the details in New Member Section, otherwise left it.
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
