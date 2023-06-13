<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true"
    CodeFile="AddAStudent.aspx.cs" Inherits="Admissions_AddAStudent" %>
    <%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>
   <%-- <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <script language="javascript" type="text/javascript">

        function SetEnbDisDrp() {
            var StudType = document.getElementById("<%=drpStudType.ClientID %>").value;
            var drpAdmnSess = document.getElementById("<%=drpAdmSessYr.ClientID %>").value;

            if (StudType.trim() == "N") {
                drpAdmnSess.disabled = true;
                drpAdmnSess.selectedIndex = 0;
            }
            else {
                drpAdmnSess.disabled = false;
            }
        }

//        $(document).ready(function () {
//            debugger;
//            $(this.target).find('input').autocomplete();
//            var corrcity = document.getElementById('<%=txtPreCity.ClientID%>').value;
//            $("#<%=txtPreCity.ClientID%>").autocomplete({
//            source:'AutoCompleteHandler.ashx'
//            })
//        });


        function CnfSubmit() {

            var Name = document.getElementById("<%=txtname.ClientID %>").value;
            var DOB = document.getElementById("<%=txtdob.ClientID %>").value;
            var POB = document.getElementById("<%=txtPoB.ClientID %>").value;
            var Gender = document.getElementById("<%=drpGender.ClientID %>").value;
            var Relg = document.getElementById("<%=drprelgn.ClientID %>").value;
            var Nationality = document.getElementById("<%=txtnationality.ClientID %>").value;
            var cat = document.getElementById("<%=drpcat.ClientID %>").value;
            var MotherT = document.getElementById("<%=txtMotherTongue.ClientID %>").value;
//            var locality = document.getElementById("drpLocality.ClientID ").value;
            var FName = document.getElementById("<%=txtfathername.ClientID %>").value;
            var MName = document.getElementById("<%=txtmothername.ClientID %>").value;
            var newclass = document.getElementById("<%=drpnewclass.ClientID %>").value;


            var feestmonth = document.getElementById("<%=drpFeeStartFrom.ClientID %>").value;
            var AdmnSess = document.getElementById("<%=drpAdmSessYr.ClientID %>").value;
            var saveMode = document.getElementById("<%=hfSaveMode.ClientID %>").value;
            var FeeConcess = document.getElementById("<%=txtFeeConcession.ClientID %>").value;
            var ConcessReas = document.getElementById("<%=txtConceReason.ClientID %>").value;

            var currentdate = new Date();
            var currday = currentdate.getDate();

            if (Name.trim() == "") {
                alert("Please Enter Student Name !");
                document.getElementById("<%=txtname.ClientID %>").focus();
                return false;
            }
            if (DOB.trim() == "") {
                alert("Please Enter Date Of Birth !");
                document.getElementById("<%=txtdob.ClientID %>").focus();
                return false;
            }
            if (POB.trim() == "") {
                alert("Please Enter Place Of Birth !");
                document.getElementById("<%=txtPoB.ClientID %>").focus();
                return false;
            }
            if (Gender == "0") {
                alert("Please Select Gender !");
                document.getElementById("<%=drpGender.ClientID %>").focus();
                return false;
            }
            if (Relg == "0") {
                alert("Please Select Religion !");
                document.getElementById("<%=drprelgn.ClientID %>").focus();
                return false;
            }
            if (Nationality.trim() == "") {
                alert("Please Enter Nationality !");
                document.getElementById("<%=txtnationality.ClientID %>").focus();
                return false;
            }
            if (cat == "0") {
                alert("Please Select Category !");
                document.getElementById("<%=drpcat.ClientID %>").focus();
                return false;
            }
            if (MotherT.trim() == "") {
                alert("Please Enter Mother Tongue !");
                document.getElementById("<%=txtMotherTongue.ClientID %>").focus();
                return false;
            }
            //            if (locality == "0") {
            //                alert("Please Select Locality !");
            //                document.getElementById("drpLocality.ClientID ").focus();
            //                return false;
            //            }
            if (FName.trim() == "") {
                alert("Please Enter Father's Name !");
                document.getElementById("<%=txtfathername.ClientID %>").focus();
                return false;
            }
            if (MName.trim() == "") {
                alert("Please Enter Mother's Name !");
                document.getElementById("<%=txtmothername.ClientID %>").focus();
                return false;
            }

            if (Date.parse(DOB.trim()) > Date.parse(currentdate)) {
                alert("Please check date range! Date of birth can't be greater than current date!")
                return false;
            }


            if (newclass == "0") {
                alert("Please Select Present Class of the Student !");
                document.getElementById("<%=drpnewclass.ClientID %>").focus();
                return false;
            }

            if (feestmonth == "0" && saveMode == "Insert") {
                alert("Please select the month from which the fee to be collected");
                document.getElementById("<%=drpFeeStartFrom.ClientID %>").focus();
                return false;
            }
            if (FeeConcess.trim() > "0" && ConcessReas.trim() == "") {
                alert("Please Enter Reason For Fee Concession !");
                document.getElementById("<%=txtConceReason.ClientID %>").focus();
                return false;
            }

            else {
                return true;
            }
        }
        function filladdress() {
            // alert("filling address");
            var corradd1 = document.getElementById('<%=txtPreAddr1.ClientID%>').value;
            var corradd2 = document.getElementById('<%=txtPreAddr2.ClientID%>').value;
            var corrdist = document.getElementById('<%=txtPreDist.ClientID%>').value;
            var corrState = document.getElementById('<%=txtPreState.ClientID%>').value;
            var corrCon = document.getElementById('<%=txtPreCountry.ClientID%>').value;
            var corrpin = document.getElementById('<%=txtPrePin.ClientID%>').value;
            var corrcity = document.getElementById('<%=txtPreCity.ClientID%>').value;

            document.getElementById('<%=txtPermAddr1.ClientID%>').value = corradd1;
            document.getElementById('<%=txtPermAddr2.ClientID%>').value = corradd2;
            document.getElementById('<%=txtPermState.ClientID%>').value = corrState;
            document.getElementById('<%=txtPermCountry.ClientID%>').value = corrCon;
            document.getElementById('<%=txtPermDist.ClientID%>').value = corrdist;
            document.getElementById('<%=txtPermCity.ClientID%>').value = corrCity;
            document.getElementById('<%=txtPermPin.ClientID%>').value = corrpin;
                       
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

            if (key == 45) {
                return true;
            }
            // check for dash or hyphen was pressed
            reg = /\d/;
            var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
            var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

            return isFirstN || isFirstD || reg.test(keychar);
        }

        function clearText(btn) {
            switch (btn) {
                case '1':
                    {
                        document.getElementById("<%=txtAdmndate.ClientID %>").value = "";
                        return false;
                    }
                case '2':
                    {
                        document.getElementById("<%=txtdob.ClientID %>").value = "";
                        return false;
                    }

                default:
                    {
                        return false;
                    }
            }
        }


        function ToggleAll(e) {
            if (e.checked) {
                CheckAll();
            }
            else {
                ClearAll();
            }
        }

        function CheckAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = true;
                }
            }
            ml.toggleAll.checked = true;
        }

        function ClearAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = false;
                }
            }
            ml.toggleAll.checked = false;
        }

        function checktext(e) {
            debugger;
            e = e ? e : window.event;
            var charCode = e.which ? e.which : e.keyCode;
            if  (charCode < 48 || charCode > 57 )
             {
                alert('Only digits allowed');
                return false;
            }
        }
        function readURL(input) {
            debugger;
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#imgStud').attr('src', e.target.result);
                }

                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
  
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Student Admission</h2>
                <div style="float: right;">
                    <asp:Label ID="lblMsgTop" Font-Bold="true" ForeColor="Green" runat="server" Text=""
                        CssClass="tbltxt"></asp:Label>
                </div>
            </div>
            <div style="margin-left: 400px;">
                <h2>
                    <span>For </span><span>
                        <asp:Label ID="lblFormtype" runat="server" Text="Label"></asp:Label>
                    </span>
                </h2>
            </div>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="100%" border="0" cellspacing="5" cellpadding="0" class="cnt-box">
                <tr>
                    <td colspan="7" class="tbltxt">
                        <div class="cnt-sec">
                            <span class="ttl2">
                                <asp:Label ID="lblStudType" runat="server" Text="Student Type&nbsp;"></asp:Label></span>
                            :
                            <asp:DropDownList ID="drpStudType" runat="server" onClick="SetEnbDisDrp();" TabIndex="1"
                                CssClass="wdth-134 tbltxt">
                                <asp:ListItem Text="Existing" Value="E" />
                                <asp:ListItem Text="New" Value="N" Selected="True" />
                                <asp:ListItem Text="TC" Value="T" />
                               <%-- <asp:ListItem Text="Casual" Value="C" />--%>
                            </asp:DropDownList>
                        </div>
                        <div class="cnt-sec">
                            <span class="ttl2">
                                Present Session</span>
                            :
                           <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" TabIndex="74">
                                                </asp:DropDownList>
                        </div>
                        
                        <div class="cnt-sec">
                           <span class="tt12" style="margin-right:43px">
                                Admission No <span class="error">*</span> </span>
                          
                            : <span>
                                <asp:TextBox ID="txtadmsnno" MaxLength="20" placeholder="as per register" runat="server" CssClass="" TabIndex="4"></asp:TextBox></span>
                        </div>
                        <div class="cnt-sec">
                             <span class="ttl2" style="margin-right: 72px;">
                                Present Class
                            </span>
                            :
                            <asp:DropDownList ID="drpnewclass" runat="server" CssClass="vsmalltb" TabIndex="75" OnSelectedIndexChanged="drpnewclass_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                        </div>
                        <div class="cnt-sec">
                            <span class="ttl2" style="margin-right:38px;">
                                <asp:Label ID="lblAdmnSess" runat="server" Text="Admission Session"></asp:Label></span>
                            :
                            <asp:DropDownList ID="drpAdmSessYr" runat="server" CssClass="tbltxtbox wdth-134"
                                TabIndex="2">
                            </asp:DropDownList>
                        </div>
                       
                        <div class="cnt-sec">
                            <span class="ttl2" style="margin-right: 104px;">
                                Section</span>
                            :
                             <asp:DropDownList ID="drpsection" runat="server" TabIndex="76" CssClass="vsmalltb">
                                                </asp:DropDownList>
                        </div>
                        <div class="cnt-sec">
                            <div class="ttl3" style="margin-right: 20px;">
                                Admission Date <span class="error">*</span>
                            </div>
                            :
                            <asp:TextBox ID="txtAdmndate" runat="server" ReadOnly="False" TabIndex="3" CssClass="wdth-120"></asp:TextBox>&nbsp;
                            <rjs:PopCalendar ID="PopCalAdmnDt" runat="server" Control="txtAdmndate"></rjs:PopCalendar>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/refresh_icon.png"
                                OnClientClick="return clearText('1');" ImageAlign="Absmiddle" />
                        </div>
                        <div class="cnt-sec">
                            <span class="ttl2" style="margin-right: 104px;">
                                Roll No</span>
                            :
                              <asp:TextBox ID="txtrollno" runat="server" CssClass="" TabIndex="77" onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
                        </div>
                         <div class="cnt-sec">
                            <span class="ttl2" style="margin-right: 104px;">
                               House</span>
                            :
                             <asp:DropDownList ID="drpHouse" runat="server" CssClass="tbltxtbox wdth-134"
                                TabIndex="2">
                            </asp:DropDownList>
                             </div>
                         </td>

                </tr>
            </table>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
                <asp:Panel ID= "OptionalPanel" runat="Server" Visible="false" >
                  <table width="100%" border="0" cellspacing="5" cellpadding="0" class="cnt-box">
                    <tr>
                     <td class="tbltxt">
                      <div class="cnt-sec">
                         <span class="ttl2" style="margin-right: 46px;">
                                Second language </span>
                                :
                        <asp:DropDownList  ID="drpsecondlang" runat="Server" >
                        <asp:ListItem Value=" ">-Select-</asp:ListItem>
                        <asp:ListItem Value="Odia" >Odia</asp:ListItem>
                        <asp:ListItem Value="Hindi">Hindi</asp:ListItem>
                        </asp:DropDownList>
                       </div>
                        <div class="cnt-sec">
                         <span class="ttl2" style="margin-right: 72px;">
                        Sixth Subject</span>
                        :
                         <asp:DropDownList ID="drpSixthSubject" runat="server" Enabled="false" OnSelectedIndexChange="drpBindSixth_SelectedIndexchanged">
                         </asp:DropDownList> 
                         </div>                
                                </td>
                        </tr>
                        </table>

                </asp:Panel>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <asp:Panel ID="CoursePanel" runat="server" Visible="false">
                <table width="100%" border="0" cellspacing="5" cellpadding="0" class="cnt-box">
                    <tr>
                        <td class="tbltxt">
                            Course Applied For :
                        </td>
                        <td align="left" class="tbltxt">
                            <asp:RadioButtonList ID="RbCourse" runat="server" RepeatDirection="Horizontal" Font-Bold="true"
                                OnSelectedIndexChanged="RbCourse_SelectedIndexChanged" AutoPostBack="true" TabIndex="5">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr class="tbltxt">
                        <td>
                            Optional Subjects <span style="font-style: italic">(Select Appropriate Option)</span>
                        </td>
                        <td class="tbltxt">
                            <asp:RadioButtonList ID="RbOptional" runat="server" RepeatDirection="Horizontal"
                                Font-Size="12px" TabIndex="6" OnSelectedIndexChanged="RbOptional_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="tbltxt">
                            <asp:CheckBox ID="ChksixthSub" runat="server" Visible="false" Text="Computer Science As 6th Subject" TabIndex="7" OnCheckedChanged="ChksixthSub_Changed" AutoPostBack="true" Checked="false" />
                        </td>
                    </tr>
                    <tr>
                    <td colspan="7">
                    <table  class="mGrid tbltxt" width="100%" height="30px">
                    <tr>
                    <td> <asp:Label ID="lbltd1" runat="server"></asp:Label></td>
                      <td> <asp:Label ID="lbltd2" runat="server"></asp:Label></td>
                      <td> <asp:Label ID="lbltd3" runat="server"></asp:Label></td>
                        <td> <asp:Label ID="lbltd4" runat="server"></asp:Label></td>
                       <td> <asp:Label ID="lbltd5" runat="server"></asp:Label></td>
                         <td> <asp:Label ID="lbltd6" runat="server"></asp:Label></td>
                    </tr>
                    </table>
                    </td>
                    </tr>
                </table>
            </asp:Panel>
            <table class="cnt-box2 spaceborder" width="100%">
                <tr>
                    <td colspan="7" class="spacer">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7" class="addmnhead">
                        Student Information
                    </td>
                </tr>
                <tr>
                    <td colspan="7" class="spacer">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>
                <tr>
                    <td width="110" class="tbltxt">
                        Student Name<span class="error"> *</span>
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td width="220">
                        <asp:TextBox ID="txtname" MaxLength="50" runat="server" CssClass="largetb" TabIndex="8" ></asp:TextBox>
                    </td>
                    <td class="tbltxt">
                         Gender <span class="error">*</span>
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                     <asp:DropDownList ID="drpGender" runat="server" CssClass="vsmalltb" TabIndex="12">
                            <asp:ListItem Value="Male">Boy</asp:ListItem>
                            <asp:ListItem Value="Female">Girl</asp:ListItem>
                        </asp:DropDownList>
                     
                    </td>
                    <td width="135" rowspan="7" align="center">
                        <span style="color: #ccc; bottom: 5px; font-family: tahoma; font-size: 11px;">space
                            for Photo</span>
                        <%-- <asp:Image ID="imgStud" runat="server" width="120" height="150" borderstyle="Groove" borderwidth="2px" />--%>
                        <img id="imgStud" runat="server" width="150" height="150" borderstyle="Groove" borderwidth="2px"
                            src="https://kemschampua.co.in/Temp_Files/temp_img/20220505124701.jpg" />
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Date Of Birth <span class="error">*</span>
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtdob" runat="server" ReadOnly="False" TabIndex="10"></asp:TextBox>&nbsp;<rjs:PopCalendar
                            ID="PopCalDOB" runat="server" Control="txtdob"></rjs:PopCalendar>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/refresh_icon.png"
                            OnClientClick="return clearText('2');" ImageAlign="Absmiddle" />
                    </td>
                    <td class="tbltxt">
                        Place Of Birth <span class="error">*</span>
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPoB" runat="server" MaxLength="30" CssClass="" TabIndex="11"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td class="tbltxt">
                        Blood Group <span class="error"></span>
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpBloodGroup" runat="server" CssClass="vsmalltb" TabIndex="13">
                            <asp:ListItem>-Blood Group-</asp:ListItem>
                            <asp:ListItem>A+</asp:ListItem>
                            <asp:ListItem>A-</asp:ListItem>
                            <asp:ListItem>B+</asp:ListItem>
                            <asp:ListItem>B-</asp:ListItem>
                            <asp:ListItem>O+</asp:ListItem>
                            <asp:ListItem>O-</asp:ListItem>
                            <asp:ListItem>AB+</asp:ListItem>
                            <asp:ListItem>AB-</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                     <td class="tbltxt">
                        Mother Tongue <span class="error">*</span>
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMotherTongue" runat="server" MaxLength="20" CssClass="" TabIndex="18"></asp:TextBox>
                    </td>
                    
                    
                </tr>
                <tr>
                    <td class="tbltxt">
                        Religion <span class="error">*</span>
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drprelgn" runat="server" CssClass="vsmalltb" TabIndex="14">
                        </asp:DropDownList>
                    </td>
                    <%-- <td class="tbltxt">
                        Denomination(Only for Christian)
                    </td>
                    <td class="tbltxt">
                        :
                    </td>--%>
                    <td>
                       <asp:DropDownList ID="drpDenomination" runat="server" Visible="false" CssClass="vsmalltb">
                       <asp:ListItem Value=" ">-Select</asp:ListItem>
                       <asp:ListItem Value="Catholic">Catholic</asp:ListItem>
                       </asp:DropDownList>
                    </td>
                   
                </tr>
                <tr>
                    <td class="tbltxt">
                        Category <span class="error">*</span>
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpcat" runat="server" CssClass="vsmalltb" TabIndex="16" OnSelectedIndexChanged="drpcat_Changed"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                <%--    <td class="tbltxt">
                        Tribe Name(if ST)
                    </td>
                    <td class="tbltxt">
                        :
                    </td>--%>
                    <td>
                        <asp:TextBox ID="txtTribeName" runat="server" Visible="false" MaxLength="20" CssClass="" TabIndex="17"
                            Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                 <td align="left" class="tbltxt" valign="top">
                        Local Guardian Name
                    </td>
                    <td align="left" class="tbltxt" valign="top">
                        :
                    </td>
                    <td align="left" valign="top">
                        <asp:TextBox ID="txtLocalGuard" MaxLength="30" CssClass="largetb" runat="server"
                            ValidationGroup="a" TabIndex="23"></asp:TextBox>
                    </td>
                   
                    <td align="left" class="tbltxt" valign="top">
                        Relation with Local Guardian
                    </td>
                    <td align="left" class="tbltxt" valign="top">
                        :
                    </td>
                    <td align="left" valign="top">
                        <asp:TextBox ID="txtRelWithLocalGuard" MaxLength="30" CssClass="largetb" runat="server"
                            ValidationGroup="a" TabIndex="24"></asp:TextBox>
                    </td>
                 
                    <td align="center">
                        <asp:Label runat="server" Font-Bold="true" ID="lblAdmnNo" Visible="false" ></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td align="left" class="tbltxt" valign="top">
                        Adhar Card No<span class="error">*</span>
                    </td>
                    <td align="left" class="tbltxt" valign="top">
                        :
                    </td>
                    <td align="left" valign="top">
                        <asp:TextBox ID="txtStudAdhar" CssClass="largetb" MaxLength="30" runat="server" ValidationGroup="a"
                            TabIndex="20" onkeypress="return checktext(event);"></asp:TextBox>
                    </td>
                     <td class="tbltxt">
                        Nationality<span class="error"> *</span>
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtnationality" MaxLength="30" CssClass="" runat="server" ValidationGroup="a"
                            TabIndex="15"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td class="tbltxt">
                     
                    </td>
                    <td class="tbltxt">
                      
                    </td>
                    <td>
                        <asp:TextBox ID="txtnickname" MaxLength="30" runat="server" CssClass="" TabIndex="9" Visible="false"></asp:TextBox>
                    </td>
                   
                       <td class="tbltxt">
                        
                    </td>
                    <td class="tbltxt">
                      
                    </td>
                    <td>
                        <asp:DropDownList ID="drpLocality" runat="server" CssClass="vsmalltb" TabIndex="19" Visible="false">
                            <asp:ListItem>Urban</asp:ListItem>
                            <asp:ListItem>Rural</asp:ListItem>
                            <asp:ListItem>Tribal</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt" valign="top">
                        Upload Doc
                    </td>
                    <td align="left" class="tbltxt" valign="top">
                        :
                    </td>
                    <td align="left" valign="top">
                        <asp:FileUpload ID="fldUpDoc" runat="server" TabIndex="25" Width="200px" />
                    </td>
                    <td class="tbltxt">
                        Upload Image
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:FileUpload ID="fldUpImage" runat="server" TabIndex="21" Width="200px" />
                      
                       <%-- <input id="File1" type="file" runat="server" onchange="readURL(this);" />--%>
                    </td>
                    <td>
                        <asp:Button ID="btnPreview" runat="server" Text="Preview" OnClick="btnPreview_Click"
                            TabIndex="22" />
                    </td>
                    </tr>
                <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:HyperLink ID="hlDoc" runat="server" Target="_blank" CssClass="tbltxt">[hlDoc]</asp:HyperLink>
                        </td>
                    </tr>
                
            </table>
            <table class="cnt-box2 spaceborder" width="100%">
                <tr>
                    <td colspan="7" class="spacer">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>
                <tr>
                    <td width="50%" valign="top">
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td class="addmnhead" colspan="3" valign="top" align="left">
                                    Father Details
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" colspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td width="250" align="left" valign="top" class="tbltxt">
                                    Father's Name <span class="error">*</span>
                                </td>
                                <td width="5" align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtfathername" MaxLength="30" CssClass="largetb" runat="server"
                                        ValidationGroup="a" TabIndex="26"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Educational Qualification
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFatEQ" CssClass="largetb" MaxLength="30" runat="server" ValidationGroup="a"
                                        TabIndex="27"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                           <%--     <td align="left" class="tbltxt" valign="top">
                                    School Attended
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFatSchlName" CssClass="largetb" MaxLength="30" runat="server" Visible="false"
                                        ValidationGroup="a" TabIndex="28"></asp:TextBox>
                                </td>
                            </tr>
                            <tr> 
                         <%--       <td align="left" class="tbltxt" valign="top">
                                    Place Of School
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFatSchlPlc" CssClass="largetb" MaxLength="30" runat="server" Visible="false"
                                        ValidationGroup="a" TabIndex="29"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                               <%-- <td align="left" class="tbltxt" valign="top">
                                    College Attended
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFatClgName" CssClass="largetb" MaxLength="30" runat="server" Visible="false"
                                        ValidationGroup="a" TabIndex="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                              <%--  <td align="left" class="tbltxt" valign="top">
                                    Place Of College
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFatClgPlc" CssClass="largetb" MaxLength="30" runat="server" Visible="false" ValidationGroup="a"
                                        TabIndex="31"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Father's Occupation
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFathOcc" CssClass="largetb" MaxLength="30" runat="server" ValidationGroup="a"
                                        TabIndex="32"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="left" class="tbltxt" valign="top">
                                    Designation
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFatDesg" CssClass="largetb" MaxLength="30" runat="server" Visible="false" ValidationGroup ="a"
                                        TabIndex="33"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                              <%--  <td align="left" class="tbltxt" valign="top">
                                    Department
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFatDep" CssClass="largetb" MaxLength="30" runat="server" Visible="false"  ValidationGroup="a"
                                        TabIndex="34"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Establishment/Institution/Company Name
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFatCompName" CssClass="largetb" MaxLength="30" runat="server"
                                        ValidationGroup="a" TabIndex="35"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Father's Adhar Card No
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFatAdhar" CssClass="largetb" MaxLength="30" runat="server" ValidationGroup="a" onkeypress="return checktext(event);" TabIndex="36"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="50%" valign="top">
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td class="addmnhead" colspan="3" valign="top" align="left">
                                    Mother Details
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" colspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Mother's Name <span class="error">*</span>
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtmothername" MaxLength="30" CssClass="largetb" runat="server"
                                        ValidationGroup="a" TabIndex="37"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Educational Qualification
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMotEQ" CssClass="largetb" MaxLength="30" runat="server" ValidationGroup="a"
                                        TabIndex="38"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                               <%-- <td align="left" class="tbltxt" valign="top">
                                    School Attended
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMotSchlName" CssClass="largetb" MaxLength="30" runat="server" Visible="false"
                                        ValidationGroup="a" TabIndex="39"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                               <%-- <td align="left" class="tbltxt" valign="top">
                                    Place Of School
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMotSchlPlc" CssClass="largetb" MaxLength="30" runat="server" Visible="false"
                                        ValidationGroup="a" TabIndex="40"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                               <%-- <td align="left" class="tbltxt" valign="top">
                                    College Attended
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMotClgName" CssClass="largetb" MaxLength="30" runat="server" Visible="false"
                                        ValidationGroup="a" TabIndex="41"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                               <%-- <td align="left" class="tbltxt" valign="top">
                                    Place Of College
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMotClgPlc" CssClass="largetb" MaxLength="30" runat="server" Visible="false" ValidationGroup="a"
                                        TabIndex="42"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Mother's Occupation
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMothOcc" CssClass="largetb" MaxLength="30" runat="server" ValidationGroup="a"
                                        TabIndex="43"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                             <%--   <td align="left" class="tbltxt" valign="top">
                                    Designation
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMotDeg" CssClass="largetb" MaxLength="30" runat="server" Visible="false" ValidationGroup="a"
                                        TabIndex="44"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                              <%--  <td align="left" class="tbltxt" valign="top">
                                    Department
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>--%>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMotDep" CssClass="largetb" MaxLength="30" runat="server" Visible="false" ValidationGroup="a"
                                        TabIndex="45"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Establishment/Institution/Company Name
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMotCompName" CssClass="largetb" MaxLength="30" runat="server"
                                        ValidationGroup="a" TabIndex="46"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Mother's Aadhar Card No
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMotAdhar" CssClass="largetb" MaxLength="30" runat="server" ValidationGroup="a"
                                      onkeypress="return checktext(event);"  TabIndex="47"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table class="cnt-box2 spaceborder" width="100%">
                <%--<tr>
                    <td colspan="7" class="">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="7">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="50%" valign="top">
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                    <%--<tr>
                                            <td colspan="3" align="left" valign="top" class="spacer">
                                                <img src="../images/mask.gif" width="8" height="8" />
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td class="addmnhead" colspan="3" valign="top" align="left">
                                                Present Address
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td width="100" align="left" valign="top" class="tbltxt">
                                            </td>
                                            <td width="5" align="left" valign="top" class="tbltxt">
                                            </td>
                                            <td colspan="3" align="left" valign="top">
                                                <input class="button" name="btnCopyAddress" onclick="filladdress();" type="button"
                                                    value="Same As Permanent Address" tabindex="28" />
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td width="100" align="left" valign="top" class="tbltxt">
                                                Address Line 1
                                            </td>
                                            <td width="5" align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPreAddr1" runat="server" CssClass="largeta" MaxLength="60" TextMode="MultiLine"
                                                    Width="200px" Rows="1" TabIndex="56"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100" align="left" valign="top" class="tbltxt">
                                                Address Line 2
                                            </td>
                                            <td width="5" align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPreAddr2" runat="server" CssClass="largeta" MaxLength="60" TextMode="MultiLine"
                                                    Width="200px" Rows="1" TabIndex="57"></asp:TextBox>
                                                     <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"  TargetControlID="txtPreAddr2" MinimumPrefixLength="1"  CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" runat="server" ContextKey="4"
                                                 FirstRowSelected="false" ServiceMethod="AutoComplete"  CompletionListCssClass="AutoExtender" CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"  ></ajaxToolkit:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                City&nbsp;
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPreCity" runat="server" CssClass="" Width="" MaxLength="30" TabIndex="58"></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender TargetControlID="txtPreCity" MinimumPrefixLength="1"  CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" runat="server" ContextKey="1"
                                                 FirstRowSelected="false" ServiceMethod="AutoComplete"  CompletionListCssClass="AutoExtender" CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"  ></ajaxToolkit:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Dist
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPreDist" runat="server" CssClass="" Width="" MaxLength="30" TabIndex="59"></asp:TextBox>
                                                 <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1"  TargetControlID="txtPreDist" MinimumPrefixLength="1"  CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" runat="server" ContextKey="3"
                                                 FirstRowSelected="false" ServiceMethod="AutoComplete"  CompletionListCssClass="AutoExtender" CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"  ></ajaxToolkit:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                State
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPreState" runat="server" CssClass="" Width="" MaxLength="30" Text="Odisha"
                                                    TabIndex="60"></asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender  TargetControlID="txtPreState" MinimumPrefixLength="1"  CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" runat="server" ContextKey="2"
                                                 FirstRowSelected="false" ServiceMethod="AutoComplete"  CompletionListCssClass="AutoExtender" CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"  ></ajaxToolkit:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Country
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPreCountry" runat="server" CssClass="" Width="" MaxLength="30" Text="India"
                                                    TabIndex="61"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Pin&nbsp;
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPrePin" runat="server" CssClass="" Width="" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    MaxLength="6" TabIndex="62"></asp:TextBox>
                                            </td>
                                        </tr>
                                        
                                        
                                    </table>
                                </td>
                                <td valign="top">
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="addmnhead" colspan="3" valign="top">
                                                Permanent Address
                                                <asp:CheckBox ID="chkboxAdd" runat="server" OnCheckedChanged="chkboxAdd_Clicked"
                                                    AutoPostBack="true" TabIndex="55" Text="Same As Present Address?" CssClass="tbltxt"
                                                    Font-Size="12px" Style="color: #e29a52;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="left" valign="top" class="spacer">
                                                <img src="../images/mask.gif" width="8" height="8" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="110" align="left" valign="top" class="tbltxt">
                                                Address Line 1
                                            </td>
                                            <td width="5" align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPermAddr1" MaxLength="100" runat="server" CssClass="" Width="200px"
                                                    Rows="1" TextMode="MultiLine" TabIndex="48"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="110" align="left" valign="top" class="tbltxt">
                                                Address Line 2
                                            </td>
                                            <td width="5" align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPermAddr2" MaxLength="100" runat="server" CssClass="" Width="200px"
                                                    Rows="1" TextMode="MultiLine" TabIndex="49"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                City
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPermCity" runat="server" MaxLength="30" CssClass="" Width=""
                                                    TabIndex="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Dist
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPermDist" MaxLength="30" runat="server" CssClass="" TabIndex="51"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                State
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPermState" runat="server"  CssClass="" Width=""
                                                    MaxLength="30" TabIndex="52"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Country
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPermCountry" runat="server"  CssClass="" Width=""
                                                    MaxLength="30" TabIndex="53"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Pin
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPermPin" runat="server" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    MaxLength="6" CssClass="" Width="" TabIndex="54"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table class="cnt-box2 spaceborder" width="100%">
                <tr>
                    <td colspan="7" class="spacer">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>
                <tr>
                    <td class="addmnhead" colspan="7" valign="top" align="left">
                        Contact Information
                    </td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt" valign="top">
                        Phone (Residence)&nbsp <span class="error">*</span>
                    </td>
                    <td align="left" class="tbltxt" valign="top">
                        :
                    </td>
                    <td align="left" valign="top">
                        <asp:TextBox ID="txtPrePhone1" runat="server" CssClass="" Width="" onkeypress="return blockNonNumbers(this, event, false, false);"
                            MaxLength="15" TabIndex="63"></asp:TextBox>
                        <td align="left" class="tbltxt" valign="top">
                            Phone (Office)&nbsp;
                        </td>
                        <td align="left" class="tbltxt" valign="top">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtPrePhone2" runat="server" CssClass="" Width="" onkeypress="return blockNonNumbers(this, event, false, false);"
                                MaxLength="15" TabIndex="64"></asp:TextBox>
                </tr>
                <tr>
                    <td align="left" class="tbltxt" valign="top">
                        Mobile 1(For SMS Alert)&nbsp <span class="error">*</span>
                        <td align="left" class="tbltxt" valign="top">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtPreMob1" runat="server" CssClass="" Width="" onkeypress="return blockNonNumbers(this, event, false, false);"
                                MaxLength="14" TabIndex="65"></asp:TextBox>
                        </td>
                        <td align="left" class="tbltxt" valign="top">
                            Mobile 2&nbsp;
                        </td>
                        <td align="left" class="tbltxt" valign="top">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtPreMob2" runat="server" CssClass="" Width="" onkeypress="return blockNonNumbers(this, event, false, false);"
                                MaxLength="14" TabIndex="66"></asp:TextBox>
                        </td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt" valign="top">
                        Email Id (1)&nbsp <span class="error">*</span>
                    </td>
                    <td align="left" class="tbltxt" valign="top">
                        :
                    </td>
                    <td align="left" valign="top">
                        <asp:TextBox ID="txtPreEmail1" runat="server" CssClass="" MaxLength="60" TabIndex="67"></asp:TextBox>
                    </td>
                    <td align="left" class="tbltxt" valign="top">
                        Email Id (2)&nbsp;
                    </td>
                    <td align="left" class="tbltxt" valign="top">
                        :
                    </td>
                    <td align="left" valign="top">
                        <asp:TextBox ID="txtPreEmail2" runat="server" CssClass="" MaxLength="60" TabIndex="68"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <%--<tr>
                    <td colspan="7" class="spaceborder">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>--%>
            <table class="cnt-box2 spaceborder" width="100%">
                <tr>
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="50%" valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td class="addmnhead" colspan="7" valign="top" align="left">
                                                Previous School Details
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="250" align="left" valign="top" class="tbltxt">
                                                School Last attended
                                            </td>
                                            <td width="5" align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top" width="225">
                                                <asp:TextBox ID="txtschoolname" runat="server"  CssClass="" TabIndex="69"></asp:TextBox>
                                            </td>
                                       
                                        <td align="left" class="tbltxt" valign="top" width="140">
                                                Class Last attended
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                <asp:DropDownList ID="drpclass" runat="server" TabIndex="70" CssClass="vsmalltb">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                TC No
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                <asp:TextBox ID="txtTCNo" runat="server" TabIndex="72" CssClass=""></asp:TextBox>
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                TC Date
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                <asp:TextBox ID="txtTCDt" runat="server" TabIndex="73" CssClass=""></asp:TextBox>
                                                <rjs:PopCalendar ID="PopCalTCDate" runat="server" Control="txtTCDt" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Medium of Instruction
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                <asp:TextBox ID="txtprevmedium" runat="server" TabIndex="71" CssClass=""></asp:TextBox>
                                            </td>
                                        </tr>
                                            <tr>
                                            <td class="addmnhead" colspan="7" valign="top" align="left">
                                                Other Facilities
                                            </td>
                                        </tr>
                                        <tr>
                                           <td align="left" class="tbltxt" valign="top">
                                                <asp:CheckBox ID="ChkBus" runat="server" Text="Bus Facility" TabIndex="42"  />
                                                <asp:CheckBox ID="ChkHostel" runat="server" Text="Hostel Facility" TabIndex="78"  />
                                            </td>
                                 
                                        </tr>
                                    </table>
                                </td>
                                
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <%-- <tr>
                    <td colspan="7" class="spaceborder">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>--%>
            
            <%--<tr>
                    <td colspan="7" class="spaceborder">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>--%>
            <table class="cnt-box2 spaceborder" width="100%">
                <tr>
                    <td colspan="7">
                        <table style="width: 100%;" cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan="8" class="addmnhead">
                                    Bank Account Details
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td width="160" align="left" valign="top" class="tbltxt">
                                    Bank Name
                                </td>
                                <td align="left" valign="top" class="tbltxt mr-btm">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtBank" runat="server" TabIndex="81" Height="17px" Width="206px"></asp:TextBox>
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    Bank A/c No.
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtEmpBankAcNo" runat="server" CssClass="smalltb" TabIndex="82"
                                        MaxLength="20" Width="145px"></asp:TextBox>
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8" height="8px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="tbltxt">
                                    IFSC Code
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtIFSC" runat="server" CssClass="smalltb" TabIndex="83" Height="17px"
                                        Width="205px"></asp:TextBox>
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    Branch
                                </td>
                                <td align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtBranch" runat="server" CssClass="smalltb" TabIndex="84" MaxLength="20"
                                        Width="145px"></asp:TextBox>
                                </td>
                                <td align="left" valign="top" class="mandatory">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <%--<tr>
                    <td colspan="7" class="spaceborder">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>--%>
            <asp:Panel ID="StdXAvgAPanel" runat="server" Visible="false">
                <table class="cnt-box2 spaceborder" width="100%">
                    <tr>
                        <td class="addmnhead" colspan="3">
                            Averages For STD-X
                        </td>
                        <tr>
                            <td colspan="3">
                                <table class="cnt-box2 spaceborder" width="100%" class="mGrid">
                                    <tr align="left">
                                        <th class="tbltxt">
                                            TERM
                                        </th>
                                        <th class="tbltxt">
                                            English I&II(Avg)
                                        </th>
                                        <th class="tbltxt">
                                            Eco/Acc/Computer
                                        </th>
                                    </tr>
                                    <tr>
                                        <td class="tbltxt" width="170px">
                                            1ST TERM :
                                        </td>
                                        <td class="tbltxt">
                                        </td>
                                        <td class="tbltxt">
                                            <asp:TextBox ID="txtEAC1st" runat="server" Width="200px" TabIndex="86"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbltxt">
                                            2ND TERM :
                                        </td>
                                        <td class="tbltxt">
                                            <asp:TextBox ID="txtEng2nd" runat="server" Width="200px" TabIndex="87"></asp:TextBox>
                                        </td>
                                        <td class="tbltxt">
                                            <asp:TextBox ID="txtEAC2nd" runat="server" Width="200px" TabIndex="88"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbltxt">
                                            3RD TERM :
                                        </td>
                                        <td class="tbltxt">
                                            <asp:TextBox ID="txtEng3rd" runat="server" Width="200px" TabIndex="89"></asp:TextBox>
                                        </td>
                                        <td class="tbltxt">
                                            <asp:TextBox ID="txtEAC3rd" runat="server" Width="200px" TabIndex="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" class="spaceborder">
                                            <img src="../images/mask.gif" width="8" height="8" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tbltxt">
                                            Total :
                                        </td>
                                        <td class="tbltxt">
                                            <asp:TextBox ID="txtEngTot" runat="server" Width="200px" TabIndex="91"></asp:TextBox>
                                        </td>
                                        <td class="tbltxt">
                                            <asp:TextBox ID="txtEACtot" runat="server" Width="200px" TabIndex="92"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </tr>
                </table>
            </asp:Panel>
            <table class="cnt-box2 spaceborder" width="100%">
                <tr>
                    <td colspan="7">
                        <table width="100%">
                            <tr>
                                <td  class="addmnhead" colspan="3">
                                    Sibling Detail
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="tbltxt">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:RadioButton ID="radiosiblingyes" CssClass="tbltxt" runat="server" Text="Yes" Visible="true"
                                                AutoPostBack="True" GroupName="s" OnCheckedChanged="radiosiblingyes_CheckedChanged"
                                                TabIndex="93"></asp:RadioButton>
                                            <asp:RadioButton ID="radiosiblingno" CssClass="tbltxt" runat="server" Text="No" AutoPostBack="True"
                                                GroupName="s" OnCheckedChanged="radiosiblingno_CheckedChanged" Checked="True" Visible="true"
                                                TabIndex="94"></asp:RadioButton>
                                            <asp:Label ID="lblsibcount" runat="server" CssClass="tbltxt"></asp:Label>
                                            <table style="width: 100%" id="tblsibling" runat="server" visible="false">
                                                <tr>
                                                    <td width="50%">
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="tbltxt" width="70">
                                                                    Class
                                                                </td>
                                                                <td class="tbltxt" width="5">
                                                                    :
                                                                </td>
                                                                <td align="left" width="100px">
                                                                    <asp:DropDownList ID="drpsiblingclass" runat="server" OnSelectedIndexChanged="drpsiblingclass_SelectedIndexChanged"
                                                                        AutoPostBack="True" CssClass="vsmalltb" TabIndex="95">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tbltxt">
                                                                    Admission
                                                                </td>
                                                                <td class="tbltxt">
                                                                    :
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="drpsiblingadminno" runat="server" CssClass="vsmalltb" TabIndex="97">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                    <td colspan="3">
                                                        <asp:LinkButton ID="lkaddmore" OnClick="lkaddmore_Click" runat="server" Visible="True"
                                                            CausesValidation="False" OnClientClick="return checkadmn();" CssClass="txtlink"
                                                            TabIndex="101"><i class="fa fa-plus"></i><strong>Add</strong></asp:LinkButton>&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lnkdelete" OnClick="lnkdelete_Click" runat="server" Text="Delete"
                                                            Font-Bold="true" CausesValidation="False" CssClass="txtlink" TabIndex="102"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="tbltxt">
                                                                    <asp:CheckBox ID="ChkSibling" runat="server" Text="Not Kerala School" OnCheckedChanged="ChkSibling_CheckedChanged"
                                                                        AutoPostBack="true" TabIndex="96" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="NewSibPanel" runat="server" Visible="false">
                                                                        <table width="100%">
                                                                        <tr>
                                                                         <td class="tbltxt">
                                                                                Name :
                                                                                <asp:TextBox ID="txtSibName" runat="server" TabIndex="98"></asp:TextBox>
                                                                            </td>
                                                                            <td class="tbltxt">
                                                                                School Name :
                                                                                <asp:TextBox ID="txtSibSchl" runat="server" TabIndex="99"></asp:TextBox>
                                                                            </td>
                                                                            <td class="tbltxt">
                                                                                Class :
                                                                                <asp:TextBox ID="txtSibCls" runat="server" TabIndex="100"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                        <td colspan="3" align="right">
                                                                        <asp:LinkButton ID="lkaddmore1" OnClick="lkaddmore1_Click" runat="server" Visible="True"
                                                                                  CausesValidation="False" OnClientClick="return checkadmn();" CssClass="txtlink"
                                                                                    TabIndex="106"><i class="fa fa-plus"></i><strong>Add</strong></asp:LinkButton>&nbsp;&nbsp;
                                                                                            <asp:LinkButton ID="lnkdelete1" OnClick="lnkdelete1_Click" runat="server" Text="Delete"
                                                                                        Font-Bold="true" CausesValidation="False" CssClass="txtlink" TabIndex="107"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                        </td>
                                                                        </tr>
                                                                           
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                               
                                               
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="grdsiblings" runat="server" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" DataKeyNames="SiblingAdmnNo">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="15" />
                                                                    <HeaderStyle Width="15" />
                                                                    <ItemTemplate>
                                                                        <input name="Checkb" type="checkbox" value='<%#Eval("SiblingNo")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Class" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblclassId" runat="server" Text='<%#Eval("ClassID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Class">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblclass" runat="server" Text='<%#Eval("ClassName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Admission no">
                                                                    <ItemTemplate>
                                                                        <asp:Label Visible="false" ID="lbladmnno" runat="server" Text='<%#Eval("SiblingAdmnNo") %>'></asp:Label>
                                                                        <asp:LinkButton ID="lnkadminno" OnClick="FillSiblingForEdit" CausesValidation="false"
                                                                            Text='<%#Eval("SiblingAdmnNo")%>' runat="server"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblname" runat="server" Text='<%#Eval("FullName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                    <td>
                                                        <asp:GridView ID="grdsiblings2" runat="server" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False">
                                                            <Columns>
                                                            <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <input name="toggleAll2" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="15" />
                                                                    <HeaderStyle Width="15" />
                                                                    <ItemTemplate>
                                                                        <input name="Checkb" type="checkbox" value='<%#Eval("SiblingNo")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Class">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblclass" runat="server" Text='<%#Eval("SiblingClass") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50" />
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="SchoolName">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblclass" runat="server" Text='<%#Eval("SiblingSchool") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblname" runat="server" Text='<%#Eval("SiblingName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>

                          <table width="100%">
                            <tr>
                                <td  class="addmnhead" colspan="3">
                                    House Detail
                                </td>
                            </tr>


                            </table>



                    </td>
                </tr>
                <tr>
                    <td colspan="7" valign="top" class="spacer">
                        <asp:Panel ID="feeOptionRow" runat="server" Visible="false" >
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td colspan="2" class="spaceborder">
                                        <img src="../images/mask.gif" width="8" height="8" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <table width="100%" cellpadding="1" cellspacing="1">
                                            <tr>
                                                <td width="130px">
                                                    <asp:CheckBox ID="chkGenFee" Checked="true" runat="server" Text="Generate Fee" TabIndex="52"
                                                        CssClass="tbltxt" />
                                                </td>
                                                <td class="tbltxt" align="left" width="250px">
                                                    Fee to Include&nbsp;:&nbsp;
                                                    <asp:CheckBox ID="chkOT" runat="server" Text="One Time" TabIndex="103" />
                                                    &nbsp;<asp:CheckBox ID="chkAnnual" runat="server" Text="Annual" TabIndex="54" />&nbsp;
                                                </td>
                                                <td class="tbltxt" valign="top">
                                                    Fee Starts From&nbsp;:&nbsp;
                                                    <asp:DropDownList ID="drpFeeStartFrom" runat="server" CssClass="vsmalltb" TabIndex="104">
                                                        <asp:ListItem Value="04">Apr</asp:ListItem>
                                                        <asp:ListItem Value="05">May</asp:ListItem>
                                                        <asp:ListItem Value="06">June</asp:ListItem>
                                                        <asp:ListItem Value="07">July</asp:ListItem>
                                                        <asp:ListItem Value="08">Aug</asp:ListItem>
                                                        <asp:ListItem Value="09">Sep</asp:ListItem>
                                                        <asp:ListItem Value="10">Oct</asp:ListItem>
                                                        <asp:ListItem Value="11">Nov</asp:ListItem>
                                                        <asp:ListItem Value="12">Dec</asp:ListItem>
                                                        <asp:ListItem Value="01">Jan</asp:ListItem>
                                                        <asp:ListItem Value="02">Feb</asp:ListItem>
                                                        <asp:ListItem Value="03">Mar</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tbltxt" align="left" colspan="3" valign="top">
                                                    Fee Concession&nbsp;:
                                                    <asp:TextBox ID="txtFeeConcession" runat="server" CssClass="smalltb" TabIndex="56"
                                                        Width="40px" Text="0" onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
                                                    &nbsp;% &nbsp;&nbsp; Reason&nbsp;:
                                                    <asp:TextBox ID="txtConceReason" runat="server" CssClass="" TabIndex="105" Width="400px"
                                                        Text="" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="7" class="spaceborder">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7" class="spacer" align="center">
                        <input id="Hidden1" runat="server" type="hidden" />
                        <asp:Label ID="lblMsgBottom" Font-Bold="true" ForeColor="Green" runat="server" Text=""
                            CssClass="tbltxt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="7" class="spacer">
                        <div align="center">
                            <font face="Verdana">
                                <asp:Button ID="btnSubmit" runat="server" CssClass="defaultfont10" OnClick="btnSubmit_Click"
                                    OnClientClick="return CnfSubmit();" Text="Save" TabIndex="58" Width="97px" />
                                <asp:Button ID="btnSetAddonFacility" runat="server" CausesValidation="False" CssClass="defaultfont10"
                                    OnClick="btnSetAddonFacility_Click" Text="Set Additional Facility" TabIndex="61" Visible="false" />
                                <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="defaultfont10"
                                    OnClick="btnClear_Click" Text="Clear" TabIndex="59" Width="95px" />
                                <asp:Button ID="btnPrintReceipt1" runat="server" CausesValidation="False" CssClass="defaultfont10"
                                    OnClick="btnPrintReceipt1_Click" Text="Receive Fee" TabIndex="60" />
                                <asp:Button ID="btnList" runat="server" CausesValidation="False" CssClass="defaultfont10"
                                    OnClick="btnList_Click" Text="Student List" TabIndex="61" />
                                    <asp:Button ID="btnTC" runat="server" CausesValidation="False" CssClass="defaultfont10"
                                    OnClick="btnTC_Click" Text="Give TC" TabIndex="62" />
                            </font>
                        </div>
                        <%--this hidden field will contain value to check the save mode(insert/update)--%>
                        <asp:HiddenField ID="hfSaveMode" runat="server" Value="Insert" />
                    </td>
                </tr>
            </table>
            <table class="cnt-box2 spaceborder" width="100%">
                <tr>
                    <td colspan="7">
                        <table width="100%">
                            <%--<tr>
                                <td colspan="6" class="addmnhead">
                                    Extra Activities
                                </td>
                            </tr>--%>
                            <tr>
                                <td width="160" align="left" valign="top" class="tbltxt">
                                    <%--Proficiency in games/sports--%>
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                   <%-- :--%>
                                </td>
                                <td width="260" align="left" valign="top" class="tbltxt">
                                    <asp:TextBox ID="txtgames" runat="server" TextMode="MultiLine" Width="250" CssClass="largeta"
                                        TabIndex="79" Visible="false"></asp:TextBox>
                                </td>
                                <td width="60" align="left" valign="top" class="tbltxt">
                                  <%--  Hobbies--%>
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                  <%--  :--%>
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    <asp:TextBox ID="txthobbies" runat="server" TextMode="MultiLine" CssClass="largeta"
                                        Width="250" TabIndex="80" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
       </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
           
       <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
