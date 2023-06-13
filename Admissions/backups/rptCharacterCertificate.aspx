<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptCharacterCertificate.aspx.cs" Inherits="Admissions_rptCharacterCertificate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script language="javascript" type="text/javascript">


    function isValid() {
        var Session = document.getElementById("<%=drpSession.ClientID%>").value;
        var studclass = document.getElementById("<%=drpclass.ClientID%>").value;
        var students = document.getElementById("<%=drpstudents.ClientID%>").value;
        var AdmYr = document.getElementById("<%=txtAdmYr.ClientID%>").value;
        var PassYr = document.getElementById("<%=txtPassYr.ClientID%>").value;
        var ExamType = document.getElementById("<%=txtExamType.ClientID%>").value;
        var ExamYr = document.getElementById("<%=txtExamYr.ClientID%>").value;
        var PassDiv = document.getElementById("<%=txtPassDiv.ClientID%>").value;
        if (Session == 0) {
            alert("Please Select a Session");
            document.getElementById("<%=drpSession.ClientID %>").focus();
            return false;
        }
        if (studclass == 0) {
            alert("Please Select a Class");
            document.getElementById("<%=drpclass.ClientID %>").focus();
            return false;
        }
        if (students == 0) {
            alert("Please Select a Students");
            document.getElementById("<%=drpstudents.ClientID %>").focus();
            return false;
        }
        if (AdmYr == "") {
            alert("Please Select a Admission Year");
            document.getElementById("<%=txtAdmYr.ClientID %>").focus();
            return false;
        }
        if (PassYr == "") {
            alert("Please Select a Passing Year");
            document.getElementById("<%=txtPassYr.ClientID %>").focus();
            return false;
        }
        if (ExamType == "") {
            alert("Please Select a Exam Type");
            document.getElementById("<%=txtExamType.ClientID %>").focus();
            return false;
        }
        if (ExamYr == "") {
            alert("Please Select a Exam Year");
            document.getElementById("<%=txtExamYr.ClientID %>").focus();
            return false;
        }
        if (PassDiv == "") {
            alert("Please Select a Pass Division");
            document.getElementById("<%=txtPassDiv.ClientID %>").focus();
            return false;
        }
        else {
            return true;
        }

    }
</script> 
    
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Character Certificate
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
   
        <fieldset class="cnt-box4 tbltxt">
    <legend class="tbltxt3">Student Information </legend>
        <table border="0" cellspacing="3" cellpadding="3">
            <tr>
         
                <td width="90" class="tbltxt">
                    Session
                </td>
                <td width="5" class="tbltxt">
                    :
                </td>
                <td width="180">
                    <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" TabIndex="1"
                        AutoPostBack="True" 
                        onselectedindexchanged="drpSession_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td width="100" class="tbltxt">
                    Class
                </td>
                <td width="5" class="tbltxt">
                    :
                </td>
                <td width="100">
                    <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="true" CssClass="vsmalltb"
                        OnSelectedIndexChanged="drpclass_SelectedIndexChanged" TabIndex="2">
                    </asp:DropDownList>
                </td>
                <td width="70" class="tbltxt">
                    Section
                </td>
                <td width="5" class="tbltxt">
                    :
                </td>
                <td width="100">
                    <asp:DropDownList ID="drpSection" runat="server" CssClass="vsmalltb" AutoPostBack="True"
                        OnSelectedIndexChanged="drpSection_SelectedIndexChanged" TabIndex="3">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tbltxt">
                    Students
                </td>
                <td class="tbltxt">
                    :
                </td>
                <td>
                    <asp:DropDownList ID="drpstudents" runat="server" CssClass="vsmalltb" AutoPostBack="True"
                         OnSelectedIndexChanged="drpstudents_SelectedIndexChanged" TabIndex="4">
                    </asp:DropDownList>
                </td>
                <td class="tbltxt">
                    Admission no.
                </td>
                <td class="tbltxt">
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtadminno" runat="server" CssClass="" TabIndex="5"></asp:TextBox>
                </td>
                
                
            </tr>
            </table>
         
          </fieldset> 
            
     <fieldset class="cnt-box4 tbltxt">
    <legend class="tbltxt3" >Character Certificate Information </legend>
            <table border="0" cellspacing="2" cellpadding="2">
            <asp:Panel runat="Server" ID="ConductPanel" Visible="false">
             <tr>
            <td width="90" class="tbltxt">
                    Admission Year
                </td>
                <td width="5" class="tbltxt">
                    :
                </td>
                <td width="180">
                    <asp:TextBox ID="txtAdmYr" runat="server" CssClass="vsmalltb" TabIndex="6"></asp:TextBox>
                </td>
             
             <td width="100" class="tbltxt">
             TC/Passout Year
            </td>
            <td width="5" class="tbltxt">
             :
            </td>
            <td width="100">
                    <asp:TextBox ID="txtPassYr" runat="server" CssClass="vsmalltb" TabIndex="7"></asp:TextBox>
            </td>
            </tr>
            
            <tr>
             <td width="70" class="tbltxt">
             Exam Type
            </td>
            <td width="5" class="tbltxt">
             :
            </td>
            <td width="100" >
                    <asp:TextBox ID="txtExamType" runat="server" CssClass="vsmalltb" TabIndex="8" ></asp:TextBox>
            </td>
            
            <td class="tbltxt">
             Exam Year
            </td>
            <td class="tbltxt">
             :
            </td>
            <td>
                    <asp:TextBox ID="txtExamYr" runat="server" CssClass="vsmalltb" TabIndex="9"></asp:TextBox>
            </td>
            
            
            <td class="tbltxt">
             Passing Division
            </td>
            <td class="tbltxt">
             :
            </td>
            <td >
                    <asp:TextBox ID="txtPassDiv" runat="server" CssClass="vsmalltb" TabIndex="10"></asp:TextBox>
            </td>
            </tr>
            </asp:Panel>
           <tr>
            <td width="90" class="tbltxt">
                    General Character
                </td>
                <td width="5" class="tbltxt">
                    :
                   </td>
                    <td width="180">
                    <asp:TextBox ID="TxtConduct" runat="server" CssClass="vsmalltb" TabIndex="6"></asp:TextBox>
                </td>
                 <td width="90" class="tbltxt">
                    Certificate 
                </td>
                <td width="5" class="tbltxt">
                    :
                   </td>
                    <td width="180">
                    <asp:TextBox ID="TxtCertificate" runat="server" CssClass="vsmalltb" width="200" TabIndex="6"></asp:TextBox>
                </td>
           </tr>
            <tr>
            <td colspan="6" class="tbltxt">
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate Character Certificate" 
                        OnClick="btnGenerate_Click" TabIndex="11"
                        OnClientClick="return isValid();"/>
                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click"
                        TabIndex="12" />
            </td>
                        
            </tr>
           
        </table>
        
        </fieldset>
        <table cellspacing="2" cellpadding="2" Width="100%">
         <tr>
                <td>
                    <asp:Label ID="lblReport" runat="server" Width="700px" ></asp:Label>
                </td>
            </tr>
        </table>
</asp:Content>
