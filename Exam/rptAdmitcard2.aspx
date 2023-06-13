<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="rptAdmitcard2.aspx.cs" Inherits="Exam_rptAdmitcard2" %>

  <%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script language="javascript" type="text/javascript">
    function printcontent() {

        var DocumentContainer = document.getElementById('divreport');

       // var documentheader = document.getElementById('divhdr');
        var WindowObject = window.open('', "TrackData",
                             "width=800,height=600,top=20,left=20,toolbars=no,scrollbars=no,status=no,resizable=yes");
        WindowObject.document.write(DocumentContainer.innerHTML);
        WindowObject.document.close();
        WindowObject.focus();
        WindowObject.print();
        WindowObject.close();
        return false;
    }
        </script>
<script language="javascript" type="text/javascript">


    function isValid() {

        var studclass = document.getElementById("<%=drpclass.ClientID%>").value;
        var studExam = document.getElementById("<%=drpExam.ClientID%>").value;

        if (studclass == 0) {
            alert("Please Select a Class");
            document.getElementById("<%=drpclass.ClientID %>").focus();
            return false;
        }
        else if (studExam == 0) {
            alert("Please Select Exam Name");
            document.getElementById("<%=drpExam.ClientID %>").focus();
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

        return reg.test(keychar);
    }
    </script> 

     <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Admit Card
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>

   
        <table width="100%" border="0" cellspacing="2" cellpadding="2">
            <tr>
                <td width="60" class="tbltxt">
                    Session
                </td>
                <td width="5" class="tbltxt">
                    :
                </td>
                <td width="180">
                    <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" TabIndex="1"
                        AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td width="80" class="tbltxt">
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
                <td width="50" class="tbltxt">
                    Section
                </td>
                <td width="5" class="tbltxt">
                    :
                </td>
                <td>
                    <asp:DropDownList ID="drpSection" runat="server" CssClass="vsmalltb" AutoPostBack="True"
                        OnSelectedIndexChanged="drpSection_SelectedIndexChanged" TabIndex="3">
                    </asp:DropDownList>
                </td>
                 <td width="80" class="tbltxt">
                     Exam Room
                </td>
                <td width="5" class="tbltxt">
                    :
                </td>
                <td width="200">
                      <asp:TextBox ID="txtExamRoom" runat="server"></asp:TextBox>
                </td>
              
            </tr>
            <tr>
            <td width="60" class="tbltxt">
            Roll No From 
            </td>
            <td width="5" class="tbltxt">
                    :
                </td>
            <td>
            <asp:TextBox ID="txtRollfrom" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
           
            </td>
            <td width="60" class="tbltxt">
           To 
            </td>
            <td width="5" class="tbltxt">
                    :
                </td>
            <td> 
            <asp:TextBox ID="txtRollNoTo" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
            </td>
            <td width="60" class="tbltxt">Select Exam</td> <td width="5" class="tbltxt">
                    :
                </td>
            <td>
            <asp:DropDownList ID="drpExam" runat="server" AutoPostBack="true" CssClass="vsmalltb"
                        OnSelectedIndexChanged="drpExam_SelectedIndexChanged" TabIndex="2" >
                    </asp:DropDownList>
          
            </td>
                <td class="tbltxt">
                    Students
                </td>
                <td class="tbltxt">
                    :
                </td>
                <td>
                    <asp:DropDownList ID="drpstudents" runat="server" CssClass="smalltb" AutoPostBack="True"
                        Width="137px" OnSelectedIndexChanged="drpstudents_SelectedIndexChanged" TabIndex="4">
                    </asp:DropDownList>
                </td>
               
            </tr>
            <tr>
             <td class="tbltxt">
                    Admission no.
                </td>
                <td class="tbltxt">
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtadminno" runat="server" CssClass="vsmalltb" TabIndex="5"></asp:TextBox>
                </td>
                
            <td colspan="6" class="tbltxt">
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" TabIndex="4"
                        OnClientClick="return isValid();" />
                    &nbsp;<asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="printcontent();"
                        TabIndex="5" />
                </td>
            </tr>
            <tr>
                <td colspan="9">
                <div id="divreport">
                    <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                    </div>
                 </td>
            </tr>
        </table>
       <%-- <table width='95%' class='tbltd' cellpadding='2' cellspacing='2'>
            <tr>
                <td rowspan='4'>
                    <img src='../images/leftlogo.jpg' width='75px' height='80px' />
                </td>
                <td align='center' style='font-size: 20px; white-space: nowrap;'>
                    <strong>SARASWATI SHISHU VIDYA MANDIR, SALIPUR</strong>
                </td>
                <td rowspan='4'>
                    <img src='../images/rightlogo.jpg' width='85px' height='80px' />
                </td>
            </tr>
            <tr>
                <td align='center' style='font-size: 14px;'>
                    <strong>HALF YEARLY/ANNUAL EXAMINATION , 2015-16</strong>
                </td>
            </tr>
            <tr>
                <td align='center' style='font-size: 14px;'>
                    <strong><u>ADMIT CARD</u></strong>
                </td>
            </tr>
            <tr>
                <td align='center' style='font-size: 14px;'>
                    <strong></strong>
                </td>
            </tr>
        </table>
        
        <table width='95%' class='txtadmitcard' cellpadding='2' cellspacing='2'>
            <tr>
                <td align='left' style='width: 100px; white-space: nowrap; height: 30px;'><b>Student Name</b></td>
                <td align='left' style='white-space: nowrap; height: 30px;' colspan="2">:&nbsp;</td>
                <td rowspan="2" style="border: thin solid #808080; width: 170px; "><b>Roll No:</b>&nbsp;</td>
            </tr>
            <tr>
                <td align='left' style="height: 28px"><b>Mother Name</b></td>
                <td align='left' colspan="2" style="height: 28px">:&nbsp;</td>
            </tr>
            <tr>
                <td align='left'><b>Father Name</b></td>
                <td align='left' colspan="2">:&nbsp;</td>
                <td rowspan="4" style="border: thin solid #808080; height: 190px;">&nbsp;</td>
            </tr>
            <tr>
                <td align='left' style='height: 15px;'><b>Date of Birth</b></td>
                <td align='left' style='width: 120px; height: 15px;'>:&nbsp; 02 Sep 2010</td>
                <td align='left' style='height: 15px;'><b>Sex:</b> &nbsp;</td>
            </tr>
            <tr>
                <td align='left' style="height: 15px"><b>Class</b></td>
                <td align='left' style="height: 15px">:&nbsp; III</td>
                <td align='left' style="height: 15px"><b>Section:</b>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" style="height: 25px; border: thin solid #808080" align="center" valign="bottom"><br /><strong>Pradhan Acharya</strong></td>
                <td style="border: thin solid #808080" align="center" valign="bottom"><br /><strong>Candidate's Signature</strong></td>
            </tr>
            <tr>
            <td colspan="4">
            <hr />
            </td>
            </tr>
        </table>--%>
    </div>
</asp:Content>

