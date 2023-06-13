<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="StudentTC.aspx.cs" Inherits="Admissions_StudentTC" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
       <%--TC student status can not be modify--%>
 <script type="text/javascript" language="javascript">
        function valDetails() {
            var Admno = document.getElementById("<%=txtAdmnno.ClientID %>").value;
            if (Admno.trim() == "") {
                alert("Please enter admission number !");
                document.getElementById("<%=txtAdmnno.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
        function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
            var key;
            var isCtrl = false;
            var keychar;
            var reg;

            if (window.event) {
                key = e.keyCode;
                isCtrl = window.event.ctrlKey
            }
            else if (e.which) {
                key = e.which;
                isCtrl = e.ctrlKey;
            }

            if (isNaN(key)) return true;

            keychar = String.fromCharCode(key);

            // check for backspace or delete, or if Ctrl was pressed
            if (key == 8 || isCtrl) {
                return true;
            }

            reg = /\d/;
            var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
            var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;
            return isFirstN || isFirstD || reg.test(keychar);
        }

     function Select() {
                 <%-- var id = document.getElementById("<%=txtid.ClientID %>").value;--%>
             var ResnLevingScl = document.getElementById("<%=drpReasonForleave.ClientID %>").value;
             var ApplReqstFor = document.getElementById("<%=txtApplReqForCerft.ClientID %>").value;
             var IssueDtCerft = document.getElementById("<%=txtIsueDtCerft.ClientID %>").value;
          

             if (ResnLevingScl.trim() == "Select") {
                 alert("Please Select Reason for Leaving the School");
                 document.getElementById("<%=drpReasonForleave.ClientID %>").focus();
                return false;
            }
            if (ApplReqstFor.trim() == "") {
                alert("Please Select  Date of Application For certhificate ");
                document.getElementById("<%=txtApplReqForCerft.ClientID %>").focus();
                return false;
            }


            if (IssueDtCerft.trim() == "") {
                alert("Please Select  Date of Issue of Certificate");
                document.getElementById("<%=txtIsueDtCerft.ClientID %>").focus();
                return false;
            }
       
     }

 </script><div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px; width: 600px;">
        <h2>
            Student TC Print
        </h2>
        
        
    </div><asp:Label ID="lblMsg" Text="" runat="server"></asp:Label>
    <div class="spacer"></div>
    <table width="100%" class="cnt-box">
        <tr>
            <td class="tbltxt"  colspan="4">
                Select Session:<asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True"
                    CssClass="tbltxtbox" TabIndex="1" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                </asp:DropDownList>&nbsp;&nbsp;&nbsp;
               Select Class:<asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True"
                    CssClass="tbltxtbox" TabIndex="2" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                 Select Section:<asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True"
                    CssClass="tbltxtbox" TabIndex="2" OnSelectedIndexChanged="drpSection_SelectedIndexChanged">
                </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                Select Student:
                <asp:DropDownList ID="drpStudent" runat="server" AutoPostBack="True" CssClass="tbltxtbox"
                    TabIndex="5" OnSelectedIndexChanged="drpStudent_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="spacer" colspan="4"></td>
        </tr>
        <tr>
            <td class="tbltxt cnt-box2" colspan="4"  style="background:white; padding:15px;">
                Student AdmnNo :
                <asp:TextBox ID="txtAdmnno" runat="server" CssClass="vsmalltb" Width="70px"></asp:TextBox>
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClientClick="return valDetails();"
                    onfocus="active(this);" onblur="inactive(this);" OnClick="btnShow_Click" />
                &nbsp;
                <asp:Button ID="btnTcForm" runat="server" Text="Generate Tc Form" 
                    Enabled="false" onclick="btnTcForm_Click" />
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlStud" Visible="false">
    <fieldset class="cnt-box2 tbltxt">
    <table width="100%">
      <tr>
            <td class="cnt-box tbltxt" colspan="2" align="left" style="width:100%">
               Certificate Title:
                <asp:TextBox ID="txtTitle" runat="server" CssClass="vsmalltb tbltxt" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">

                      <tr>
                        <td>
                           Sl No&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                            <asp:TextBox ID="txtSlno" runat="server" Width="170px" ></asp:TextBox>
                        </td>
                        
                    </tr>

                    <tr>
                        <td>
                            Student Name&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                            <asp:TextBox ID="txtStudName" runat="server" Width="170px" Enabled="False"></asp:TextBox>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            Mother's Name&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtMotherNm" runat="server" Width="170px" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Father's Name&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                            <asp:TextBox ID="txtFAtherNm" runat="server" Width="170px" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            DOB&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                            <asp:TextBox ID="txtDOB" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpDOB" runat="server" Control="txtDOB" Format="dd-MMM-yyyy"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Admission No.&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="txtAdmno" runat="server" Width="170px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 117px">
                            Admission Date&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtAdmnDt" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpAdmnDt" runat="server" Control="txtAdmnDt" Format="dd-MMM-yyyy"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Admission Class&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="TxtAdmnCls" runat="server" Width="170px" Visible="false" ></asp:TextBox>
                            <asp:TextBox ID="txtAdmtclass" runat="server" Width="170px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            From School&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="TxtFromSchl" runat="server" Width="170px" placeholder="Kerela English Medium School"></asp:TextBox>
                        </td>
                    </tr>
                    
                     <tr>
                        <td>
                            Last Board (Exam)Result&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="txtSchBoardLastExam" runat="server" Width="170px" ></asp:TextBox>
                        </td>
                    </tr>
                    
                     <tr>
                        <td>
                          If Failed in Same class &nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="txtFailedStatus" runat="server" Width="170px" ></asp:TextBox>
                        </td>
                    </tr>

                     <tr>
                        <td>
                           Subjects Studied&nbsp;:&nbsp;
                        </td>
                        <td >
                            <asp:TextBox ID="txtSubStudies" runat="server" placeholder="1. ENGLISH 2.MATHEMATICS 3.SCIENCE 4.SOCIAL SCIENCE 5.HINDI 6.INFORMATION TECHNOLOGY"  TextMode="MultiLine" Width="170px"></asp:TextBox>
                           
                        </td>
                    </tr>


                     <tr>
                        <td>
                            if Extra curricular activities took part (mention Achievement level)&nbsp;:&nbsp;
                        </td>
                        <td >
                            <asp:TextBox ID="txtExtraCurActivity" runat="server" Width="170px" ></asp:TextBox>
                           
                        </td>
                    </tr>


                     <%--<tr>
                        <td>
                          Last Payment School Dues&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="txtMontLastPaidDue" runat="server" Width="170px" ></asp:TextBox>
                        </td>
                    </tr>
                    --%>
                   

                   <%-- <tr>
                        <td>
                            Permanent Address&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                            <asp:TextBox ID="txtAddr" runat="server" TextMode="MultiLine" Height="40px" 
                                Width="170px" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Police Station&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                            <asp:TextBox ID="txtPS" runat="server" Width="170px" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            District&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                            <asp:TextBox ID="txtDist" runat="server" Width="170px" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Pin&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                            <asp:TextBox ID="txtPin" runat="server" Width="170px" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>--%>
                </table>
            </td>
            <td valign="top" >
                <table width="100%">
               <%-- <tr>
                    <td >
                            Letter No.&nbsp;:&nbsp;
                        </td>
                        <td >
                            <asp:TextBox ID="txtLetterNo" runat="server" Width="170px"></asp:TextBox>
                        </td>
                </tr>
                <tr>
                        <td >
                            Recognition Date&nbsp;:&nbsp;&nbsp;&nbsp;
                        </td>
                        <td >
                            <asp:TextBox ID="txtRecogDt" runat="server" Width="150px"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpRecogDt" runat="server" Control="txtRecogDt" Format="dd-MMM-yyyy"/>
                        </td>
                    </tr>--%>
                    <%--<tr>
                        <td>
                            Education Circle&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtEduCir" runat="server" Width="170px"></asp:TextBox>
                            &nbsp;
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            TC/SLC SlNo.&nbsp;:&nbsp;

                        </td>
                         <td>
                                 <asp:TextBox ID="txtTcNo" runat="server" Width="74px" onkeypress="return blockNonNumbers(this, event, true, false)"></asp:TextBox>
                                /<asp:TextBox ID="txtTcSl" runat="server" Width="75px"></asp:TextBox>

                        </td>
                     
                    </tr>
                     
                    <tr>
                        <td>
                            Date Of Leaving&nbsp;:&nbsp;
                        </td>
                        <td >
                            <asp:TextBox ID="txtTcDt" runat="server" Width="150px"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpTcDt" runat="server" Control="txtTcDt"  Format="dd-MMM-yyyy" />
                        </td>
                    </tr>
                  <%--  <tr>
                        <td>
                            Reasons For Leaving &nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtReason" TextMode="MultiLine" runat="server" Width="170px" Height="40px"></asp:TextBox>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            General Conduct/Character&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtCharactr" runat="server" Width="170px" Text="GOOD"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Remarks&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" Width="170px" Height="40px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Current Class&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="txtCurrCls" runat="server" Width="170px" Enabled="False" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Promotion Status&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="TxtPrmSts" runat="server" Width="170px" Enabled="true"   ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Date Of Issue&nbsp;:&nbsp;
                        </td>
                        <td >
                            <asp:TextBox ID="txtIssueDate" runat="server" Width="150px"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpIssdt" runat="server" Control="txtIssueDate" />
                        </td>
                    </tr>
                    
                       <tr>
                        <td>
                          Qualified For Promotion for higher class (in figures)&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="txtPromotionClass" runat="server" Width="170px" ></asp:TextBox>
                        </td>
                    </tr>

                     <tr>
                        <td>
                         <%-- Qualified For Promotion for higher class --%>(in Word)&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="txtPromotionClassInWord" runat="server" Width="170px" ></asp:TextBox>
                        </td>
                    </tr>


                       <tr>
                        <td>
                           Reason for leaving the school&nbsp;:&nbsp;
                        </td>
                        <td >
                     <asp:TextBox ID="txtResLeavingSchool" runat="server" Width="200px" Visible="false"></asp:TextBox>
                            <asp:DropDownList ID="drpReasonForleave" runat="server"  Width="182px">
                                <asp:ListItem text="Select" Value="Select" />

                                <asp:ListItem text="Parent Request" Value="Parent Request" />
                                        <asp:ListItem text="Higher Studies" Value="Higher Studies" />
                            </asp:DropDownList>
                        </td>
                    </tr>

                       <tr>
                        <td>
                          Date of application for Certificate&nbsp;:&nbsp;
                        </td>
                        <td >
                            <asp:TextBox ID="txtApplReqForCerft" runat="server" Width="150px"></asp:TextBox>
                                <rjs:PopCalendar ID="dtAppln" runat="server" Control="txtApplReqForCerft"  Format="dd-MMM-yyyy" />
                          
                        </td>
                    </tr>

                      <tr>
                        <td>
                          Date of issue of Certificate&nbsp;:&nbsp;
                        </td>
                        <td >
                            <asp:TextBox ID="txtIsueDtCerft" runat="server" Width="150px"></asp:TextBox>
                                <rjs:PopCalendar ID="dtIsue" runat="server" Control="txtIsueDtCerft"  Format="dd-MMM-yyyy" />
                           
                        </td>
                    </tr>

                     <tr>
                        <td>
                        Ncc Cadet/Boy Scout/Girl Guide&nbsp;:&nbsp;
                        </td>
                        <td >
                            <asp:TextBox ID="txtStudNccScoutDtl" runat="server" Width="170px"></asp:TextBox>
                          
                        </td>
                    </tr>

                    <tr>
                        <td>
                          Last Payment School Dues&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="txtMontLastPaidDue" runat="server" Width="170px" ></asp:TextBox>
                        </td>
                    </tr>
                    

                   <%-- <tr>
                        <td>
                    if Extracurricular activitie took part (mention Achivment level)&nbsp;:&nbsp;
                        </td>
                        <td >
                            <asp:TextBox ID="txtExtraCurActivity" runat="server" Width="170px" ></asp:TextBox>
                           
                        </td>
                    </tr>--%>

                       <tr>
                        <td>
                         If fee concession availed So, the nature of this&nbsp;:&nbsp;
                        </td>
                        <td style="width: 140px">
                          <asp:TextBox ID="txtFeeConcession" runat="server" Width="170px" ></asp:TextBox>
                        </td>
                    </tr>

                   <%-- <tr>
                        <td>
                            Whether Insured Or Not&nbsp;:&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtIns" runat="server" Width="170px"></asp:TextBox>
                        </td>
                    </tr>--%>
                </table>
            </td>
        </tr>

        
                    <tr>
                        <td colspan="2">
                            <b>(If Leaving During Mid-Session)</b>
                            <br /><br />
                            Total No Of Working Days&nbsp;:&nbsp;
                            <asp:TextBox ID="txtWorking" runat="server"></asp:TextBox>
                            Total No Of Days Present&nbsp;:&nbsp;
                            <asp:TextBox ID="txtPresent" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                     <asp:Panel ID="PmtPanel" runat="server" Visible="true">
                    <tr>
                        <td colspan="2">
                            Month Upto Which Student Has Paid School Dues
                            <asp:TextBox ID="txtMonth" runat="server"></asp:TextBox>
                             Mark Sheet Attached For the Session Year
                            <asp:TextBox ID="txtSyr" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr >
        
                        <td colspan="2" style="padding-top:20px; border-top:2px solid #eee">
                           
                           Whether Qualified for Promotion to Higher Class &nbsp
                            <asp:TextBox ID="txtPromot" runat="server"></asp:TextBox>
                            &nbsp; If so, From&nbsp;Which Class
                            <asp:TextBox ID="txtClass1" runat="server" Width="70px"></asp:TextBox>
                            &nbsp;To Which Class&nbsp;
                            <asp:TextBox ID="txtClass2" runat="server" Width="71px"></asp:TextBox>
                                 </td>
                              </tr>
                     </asp:Panel>
                   
                    <tr>
                        <td colspan="2">
                           
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnPrint" runat="server" onclick="btnPrint_Click" 
                             OnClientClick="return Select();"   Text="Save &amp; Print Form"/>
                            <asp:HiddenField ID="hifStudNationality" runat="server" />
                            <asp:HiddenField ID="hifStudCat" runat="server" />
                            <asp:HiddenField ID="hifStudfstClass" runat="server" />
                            <asp:HiddenField ID="HifStudentClass" runat="server" />
                        </td>
                        </tr>
    </table>
    </fieldset></asp:Panel>
</asp:Content>

