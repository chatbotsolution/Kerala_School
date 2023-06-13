<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptDupRecieptCash.aspx.cs" Inherits="Reports_rptDupRecieptCash" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript" language="javascript">

     function isValid() {

         var Stud = document.getElementById("<%=rbtnStud.ClientID %>").checked;
         var Others = document.getElementById("<%=rbtnOthers.ClientID %>").checked;
         var RcptNo = document.getElementById("<%=drpReciept.ClientID%>").value;

         if (Stud == true) {
             var student = document.getElementById("<%=drpstudent.ClientID%>").value;
             if (student == "" || student == "0") {
                 alert("Please select a student !");
                 document.getElementById("<%=drpstudent.ClientID%>").focus();
                 return false;
             }
         }
         if (Others == true) {
             var FDt = document.getElementById("<%=txtFromDate.ClientID %>").value;
             var TDt = document.getElementById("<%=txtToDate.ClientID %>").value;
             if (FDt.trim() == "") {
                 alert("Please Select From Date !");
                 document.getElementById("<%=txtFromDate.ClientID%>").focus();
                 return false;
             }
             if (TDt.trim() == "") {
                 alert("Please Select To Date !");
                 document.getElementById("<%=txtToDate.ClientID%>").focus();
                 return false;
             }
             if (Date.parse(FDt.trim()) > Date.parse(TDt.trim())) {
                 alert("From Date cannot be greater than To Date!")
                 return false;
             }
         }
         if (RcptNo == "" || RcptNo == "0") {
             alert("Please Select Receipt Number !");
             document.getElementById("<%=drpReciept.ClientID%>").focus();
             return false;
         }
         else {

             return true;
         }
     }
     function ValidDt() {

         var Others = document.getElementById("<%=rbtnOthers.ClientID %>").checked;

         if (Others == true) {
             var FDt = document.getElementById("<%=txtFromDate.ClientID %>").value;
             var TDt = document.getElementById("<%=txtToDate.ClientID %>").value;
             if (FDt.trim() == "") {
                 alert("Please Select From Date !");
                 document.getElementById("<%=txtFromDate.ClientID%>").focus();
                 return false;
             }
             if (TDt.trim() == "") {
                 alert("Please Select To Date !");
                 document.getElementById("<%=txtToDate.ClientID%>").focus();
                 return false;
             }
             if (Date.parse(FDt.trim()) > Date.parse(TDt.trim())) {
                 alert("From Date cannot be greater than To Date!")
                 return false;
             }
         }

         else {
             return true;
         }
     }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Duplicate Receipt</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="cnt-box tbltxt">
                &nbsp;&nbsp; Recieved From
                <asp:RadioButton ID="rbtnOthers" CssClass="tbltxt" runat="server" Text="Others" AutoPostBack="True"
                    Checked="True" GroupName="s" OnCheckedChanged="rbtnStud_CheckedChanged" TabIndex="3">
                </asp:RadioButton>
                <asp:RadioButton ID="rbtnStud" CssClass="tbltxt" runat="server" Text="Student" AutoPostBack="True"
                    GroupName="s" OnCheckedChanged="rbtnStud_CheckedChanged" TabIndex="4"></asp:RadioButton>
             </div>
            <fieldset id="fsStudDetails" runat="server" class="cnt-box2">
                <legend  id="lgprchs" class="tbltxt" runat="server">
                    Student Details :-</legend>
                <table border="0" cellspacing="2" cellpadding="2" width="100%">
                    <tr>
                        <td width="60" class="tbltxt">
                            Session
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td style="width: 100px">
                            <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                                CssClass="vsmalltb" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" style="width: 34px">
                            Class
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="120">
                            <asp:DropDownList ID="drpClasses" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                                OnSelectedIndexChanged="drpClasses_SelectedIndexChanged" TabIndex="4">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt">
                            Student
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpstudent" runat="server" AutoPostBack="True" CssClass="largetb"
                                OnSelectedIndexChanged="drpstudent_SelectedIndexChanged" TabIndex="2">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" style="width: 79px">
                            Admission No
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtadminno" runat="server" CssClass="vsmalltb" AutoPostBack="True"
                                OnTextChanged="txtadminno_TextChanged" TabIndex="5"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="fsMisc" runat="server" class="cnt-box2">
                <table>
                    <tr>
                        <td width="60" class="tbltxt">
                            From Date
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="180">
                            <asp:TextBox ID="txtFromDate" runat="server"   TabIndex="2"></asp:TextBox>&nbsp;<rjs:PopCalendar
                                ID="dtpFromDate" runat="server" Control="txtFromDate" />
                        </td>
                        <td width="50" class="tbltxt">
                            To Date
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="180" class="tbltxt">
                            <asp:TextBox ID="txtToDate" runat="server"   TabIndex="3"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpToDate" runat="server" Control="txtToDate" />
                        </td>
                        <td>
                            <asp:Button ID="btnFillRcpt" runat="server" Text="Fill Receipt" OnClick="btnFillRcpt_Click"
                                OnClientClick="return ValidDt();" TabIndex="8" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset class="cnt-box2">
                <table border="0" cellspacing="2" cellpadding="2" width="100%">
                    <tr>
                        <td width="60" class="tbltxt">
                            Receipt No
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="160" class="tbltxt">
                            <asp:DropDownList ID="drpReciept" runat="server" CssClass="vsmalltb" Width="150px"
                                TabIndex="3">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return isValid();"
                                TabIndex="8" />&nbsp;
                            <asp:Button ID="btnPrint" runat="server" Text="Print Detail" TabIndex="9" 
                                OnClick="btnPrint_Click" style="height: 26px" />
                            <asp:Button ID="btnConsolidat" runat="server" Text="Print Consolidated" TabIndex="10"
                                OnClick="btnConsolidat_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div class="spacer"></div>
            <div align="center" class="cnt-box2"  >
                <asp:Label ID="lbldetail" runat="server"></asp:Label></div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="drpstudent" EventName="SelectedIndexChanged">
            </asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>



