<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="HostLeaving.aspx.cs" Inherits="Hostel_HostLeaving" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function valDetails() {
            var Admno = document.getElementById("<%=txtStudId.ClientID %>").value;
            if (Admno.trim() == "") {
                alert("Please enter admission number !");
                document.getElementById("<%=txtStudId.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }

        function CnfDelete() {

            if (confirm("You are going set leaving date for the selected student. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }

        function valid() {
            var Class = document.getElementById("<%=drpclass.ClientID %>").value;
            var Stud = document.getElementById("<%=drpstudent.ClientID %>").value;
            var StatusDt = document.getElementById("<%=txtWithdrawalDate.ClientID %>").value;

            var Remarks = document.getElementById("<%=txtReason.ClientID %>").value;
            if (Class == "0") {
                alert("Please Select Class !");
                document.getElementById("<%=drpclass.ClientID %>").focus();
                return false;
            }
            if (Stud == "0") {
                alert("Please Select a Student !");
                document.getElementById("<%=drpstudent.ClientID %>").focus();
                return false;
            }
            if (StatusDt.trim() == "") {
                alert("Please Select Status Date !");
                document.getElementById("<%=txtWithdrawalDate.ClientID %>").focus();
                return false;
            }

            if (Remarks.trim() == "") {
                alert("Please Enter Remarks !");
                document.getElementById("<%=txtReason.ClientID %>").focus();
                return false;
            }
            else {
                return CnfDelete();
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Hostel Leaving Status</h2>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="3" width="45%" border="0" style="border: solid 1px Black;">
                <tbody>
                    <tr>
                        <td valign="top" align="center" colspan="2">
                            <asp:Label ID="lblerr" runat="server" ForeColor="Red">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="120" class="tbltxt">
                            Session
                        </td>
                        <td class="tbltxt">
                            :&nbsp;<asp:DropDownList ID="drpSession" runat="server" CssClass="tbltxtbox" TabIndex="1"
                                Width="80px" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt">
                            Select Class
                        </td>
                        <td class="tbltxt">
                            :&nbsp;<asp:DropDownList ID="drpclass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                                Width="80px" CssClass="tbltxtbox" TabIndex="2">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt">
                            Student
                        </td>
                        <td class="tbltxt">
                            :&nbsp;<asp:DropDownList ID="drpstudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpstudent_SelectedIndexChanged"
                                meta:resourcekey="drpstudentResource1" CssClass="tbltxtbox" TabIndex="3">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt">
                            Search By Student Id
                        </td>
                        <td>
                            :&nbsp;<asp:TextBox ID="txtStudId" runat="server" CssClass="tbltxtbox" Width="100px"
                                MaxLength="12"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return valDetails();"
                                OnClick="btnSearch_Click" />
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left" class="tbltxt">
                            &nbsp;</td>
                        <td align="left" valign="top" class="tbltxt">
                            <asp:Label ID="lblStudDet" runat="server" Font-Bold="False" Font-Size="Small" 
                                ForeColor="Blue" Visible="true"></asp:Label>
                        </td>
                    </tr>
                    <%--<tr>
                        <td align="left" class="tbltxt">
                            Inventory Details</td>
                        <td align="left" class="tbltxt" valign="top">
                            :
                            <asp:Label ID="lblInvDet" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                            &nbsp;
                            <asp:Button ID="btnRecvInv" runat="server" CausesValidation="False" 
                                OnClick="btnFeeReceive_Click" Text="Receive Inventory" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="left" class="tbltxt">
                            Hostel Dues</td>
                        <td align="left" class="tbltxt" valign="top">
                            :
                            <asp:Label ID="lblHostelDues" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                            &nbsp;
                            <asp:Button ID="btnFeeReceive" runat="server" CausesValidation="False" 
                                OnClick="btnFeeReceive_Click" Text="Receive Dues" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tbltxt">
                            Hostel Leaving Date
                        </td>
                        <td align="left" valign="top" class="tbltxt">
                            :&nbsp;<asp:TextBox ID="txtWithdrawalDate" runat="server" CssClass="tbltxtbox" 
                                TabIndex="4" Width="100px"></asp:TextBox>
                            &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtWithdrawalDate" 
                                Format="dd mm yyyy" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tbltxt" valign="top">
                            Reason</td>
                        <td align="left" class="tbltxt" valign="top">
                            :&nbsp;<asp:TextBox ID="txtReason" runat="server" CssClass="tbltxtbox" Height="62px" 
                                TabIndex="6" TextMode="MultiLine" Width="245px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tbltxt" style="color:Red;" colspan="2">
                            Please verify all details before setting leaving date for any student.
                            <br />
                            Once Status is set all pending fee will be removed and student will not be active
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" valign="top">
                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" 
                                OnClientClick="return valid();" TabIndex="5" Text="Submit" Width="64px" />
                            &nbsp;
                            <asp:Button ID="btnCancel" runat="server" CausesValidation="False" 
                                OnClick="btnCancel_Click" TabIndex="6" Text="Cancel" />
                            <input id="hdnsts" runat="server" style="width: 62px" type="hidden" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

