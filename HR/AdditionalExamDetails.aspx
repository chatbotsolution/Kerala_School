<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="AdditionalExamDetails.aspx.cs" Inherits="HR_AdditionalExamDetails" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript">
        function clearText(btn) {
            switch (btn) {
                case '1':
                    {
                        document.getElementById("<%=txtExamDate.ClientID %>").value = "";
                        return false;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        function validateform() {
            var ExamDate = document.getElementById("<%=txtExamDate.ClientID %>").value;
            var ExamDetails = document.getElementById("<%=txtExamDetails.ClientID %>").value;
            var ExamResult = document.getElementById("<%=txtExamResult.ClientID %>").value;

            if (ExamDate.trim() == "") {
                alert("Enter Exam Date !");
                document.getElementById("<%=txtExamDate.ClientID %>").focus();
                return false;
            }
            if (ExamDetails.trim() == "") {
                alert("Enter Exam Details !");
                document.getElementById("<%=txtExamDetails.ClientID %>").focus();
                return false;
            }
            if (ExamResult.trim() == "") {
                alert("Enter Exam Result !");
                document.getElementById("<%=txtExamResult.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <div align="center">
        <br />
        <div class="innerdiv" style="width: 450px;">
            <div class="linegap">
                <img src="../images/mask.gif" width="10" height="10" /></div>
            <div style="padding: 8px;">
                <table width="100%" cellspacing="0">
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            Employee Name
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:Label ID="lblEmp" runat="server" CssClass="totalrec"></asp:Label>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            Exam Date
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtExamDate" runat="server" CssClass="tbltxtbox_mid" TabIndex="1"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpExamDate" runat="server" Control="txtExamDate" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                OnClientClick="return clearText('1');" />
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            Exam Details
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtExamDetails" runat="server" CssClass="tbltxtbox" MaxLength="50"
                                TabIndex="2"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                    </tr>
                    <tr>
                        <td width="100" align="left" valign="top" class="tbltxt">
                            Exam Results
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtExamResult" runat="server" CssClass="tbltxtbox" TabIndex="3"
                                MaxLength="30"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" class="mandatory">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="td-bottom-mid">
                        &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top" colspan="4">
                            <asp:Button ID="btnSave" runat="server" Text="Save & Add New" OnClientClick="return validateform();"
                                OnClick="btnSave_Click" ValidationGroup="grpExam" TabIndex="4" />&nbsp;<asp:Button ID="btnShow"
                                    OnClientClick="return validateform();" runat="server" Text="Save & Go to List"
                                    OnClick="btnShow_Click" TabIndex="5" />&nbsp;<asp:Button ID="btnCancel" 
                                runat="server" Text="Clear"
                                        OnClick="btnCancel_Click" TabIndex="6" />&nbsp;<asp:Button 
                                ID="btnBack" runat="server" Text="Back"
                                            OnClick="btnBack_Click" TabIndex="7" />
                        </td>
                    </tr>
                    <tr style="background-color: #FFB598;">
                        <td colspan="4" align="center" class="td-help" >
                            ** All fields are mandatory.
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

