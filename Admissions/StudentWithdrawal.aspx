<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="StudentWithdrawal.aspx.cs" Inherits="Admissions_StudentWithdrawal" %>

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
        function valid() {
            var Class = document.getElementById("<%=drpclass.ClientID %>").value;
            var Stud = document.getElementById("<%=drpstudent.ClientID %>").value;
            var StatusDt = document.getElementById("<%=txtWithdrawalDate.ClientID %>").value;
            var Status = document.getElementById("<%=drpStatus.ClientID %>").value;
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
            if (Status == "0") {
                alert("Please Select Status !");
                document.getElementById("<%=drpStatus.ClientID %>").focus();
                return false;
            }
//            if (Remarks.trim() == "") {
//                alert("Please Enter Remarks !");
//                document.getElementById("<%=txtReason.ClientID %>").focus();
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
            Modify Student Status (Individual)</h2>
    </div>
    <div class="spacer"></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="3" width="100%" border="0" class="cnt-box">
                <tbody>
                    <tr>
                        <td valign="top" align="center" colspan="2">
                            <asp:Label ID="lblerr" runat="server" ForeColor="Red">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="160" class="tbltxt">
                            Session
                        </td>
                        <td class="tbltxt">
                            :&nbsp;<asp:DropDownList ID="drpSession" runat="server" CssClass="tbltxtbox" TabIndex="1"
                                  AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt">
                            Select Class
                        </td>
                        <td class="tbltxt">
                            :&nbsp;<asp:DropDownList ID="drpclass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                                  CssClass="tbltxtbox" TabIndex="2">
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
                            Search By Student Admn No
                        </td>
                        <td>
                            :&nbsp;<asp:TextBox ID="txtStudId" runat="server" CssClass="tbltxtbox"  
                                MaxLength="12"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return valDetails();"
                                OnClick="btnSearch_Click" /> <asp:Label ID="lblStudDet" runat="server" Font-Bold="False" CssClass="tbltxt" Font-Size="Small" 
                                ForeColor="green" Visible="true"></asp:Label>
                            &nbsp;</td>
                    </tr>
                    <%--<tr>
                        <td align="left" class="tbltxt">
                            &nbsp;</td>
                        <td align="left" valign="top" class="tbltxt">
                           
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="left" class="tbltxt">
                            Status Date
                        </td>
                        <td align="left" valign="top" class="tbltxt">
                            :&nbsp;<asp:TextBox ID="txtWithdrawalDate" runat="server" CssClass="tbltxtbox" 
                                TabIndex="4"  ></asp:TextBox>
                            &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtWithdrawalDate" 
                                Format="dd mm yyyy" />
                        </td>
                        <asp:TextBox runat="server" ID="hfTxtAdmno" Visible="false"></asp:TextBox> 
                    </tr>
                    <tr>
                        <td align="left" class="tbltxt">
                            Status
                        </td>
                        <td align="left" valign="top" class="tbltxt">
                            :&nbsp;<asp:DropDownList ID="drpStatus" runat="server" AutoPostBack="True"  
                                CssClass="tbltxtbox" TabIndex="5" >
                            </asp:DropDownList>&nbsp;&nbsp;<asp:Label ID="lblDues" runat="server"  CssClass="error" Font-Bold="true" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             
                            <td class="tbltxt">
                                
                                &nbsp;&nbsp;<asp:Button ID="btnFeeReceive" runat="server" CausesValidation="False" 
                                    OnClick="btnFeeReceive_Click" Text="Receive Fee" />
                            </td>
                        </td>
                        <tr>
                            <td align="left" class="tbltxt" valign="top">
                                Remarks
                            </td>
                            <td align="left" class="tbltxt" valign="top">
                                <span style="float:left">:</span>&nbsp;<asp:TextBox ID="txtReason" runat="server" CssClass="tbltxtbox" Height="62px" 
                                    TabIndex="6" TextMode="MultiLine" Width="245px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="left" valign="top">
                                &nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" 
                                    OnClientClick="return valid();" TabIndex="5" Text="Submit" Width="64px" />
                                &nbsp;
                                <asp:Button ID="btnCancel" runat="server" CausesValidation="False" 
                                    OnClick="btnCancel_Click" TabIndex="6" Text="Cancel" />
                                <input id="hdnsts" runat="server" style="width: 62px" type="hidden" />
                                &nbsp;
                                <asp:Button ID="btnPrint" runat="server" CausesValidation="False" 
                                     TabIndex="6" Text="Print TC" Enabled="false" onclick="btnPrint_Click"/>
                            </td>
                        </tr>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

