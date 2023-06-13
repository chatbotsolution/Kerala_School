<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="PreAdmn_Addstudentinfo.aspx.cs" Inherits="Admissions_PreAdmn_Addstudentinfo" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function SetEnbDisDrp() {

            var drpAdmnSess = document.getElementById("<%=drpAdmSessYr.ClientID %>").value;

            if (StudType.trim() == "N") {
                drpAdmnSess.disabled = true;
                drpAdmnSess.selectedIndex = 0;
            }
            else {
                drpAdmnSess.disabled = false;
            }
        }



        function CnfSubmit() {

            var Name = document.getElementById("<%=txtname.ClientID %>").value;
            var DOB = document.getElementById("<%=txtdob.ClientID %>").value;
            var Gender = document.getElementById("<%=drpGender.ClientID %>").value;
            var Relg = document.getElementById("<%=drprelgn.ClientID %>").value;
            var Nationality = document.getElementById("<%=txtnationality.ClientID %>").value;
            var cat = document.getElementById("<%=drpcat.ClientID %>").value;
            var MotherT = document.getElementById("<%=txtMotherTongue.ClientID %>").value;
            var locality = document.getElementById("<%=drpLocality.ClientID %>").value;
            var FName = document.getElementById("<%=txtfathername.ClientID %>").value;
            var MName = document.getElementById("<%=txtmothername.ClientID %>").value;
            var newclass = document.getElementById("<%=drpnewclass.ClientID %>").value;



            var AdmnSess = document.getElementById("<%=drpAdmSessYr.ClientID %>").value;
            var saveMode = document.getElementById("<%=hfSaveMode.ClientID %>").value;

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
            if (locality == "0") {
                alert("Please Select Locality !");
                document.getElementById("<%=drpLocality.ClientID %>").focus();
                return false;
            }
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

            else {
                return true;
            }
        }
        function filladdress() {
            // alert("filling address");
            var corradd1 = document.getElementById('<%=txtPermAddr.ClientID%>').value;
            var corrdist = document.getElementById('<%=txtPermDist.ClientID%>').value;
            var corrpin = document.getElementById('<%=txtPermPin.ClientID%>').value;
            var corrps = document.getElementById('<%=txtPermPS.ClientID%>').value;

            document.getElementById('<%=txtPreAddr.ClientID%>').value = corradd1;
            document.getElementById('<%=txtPreDist.ClientID%>').value = corrdist;
            document.getElementById('<%=txtPrePS.ClientID%>').value = corrps;
            document.getElementById('<%=txtPrePin.ClientID%>').value = corrpin;
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
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="100%" border="0" cellspacing="1" cellpadding="1">
                <tr>
                    <td colspan="7" class="tbltxt">
                        &nbsp;<asp:Label ID="lblAdmnSess" runat="server" Text="Admission Session"></asp:Label>
                        <asp:DropDownList ID="drpAdmSessYr" runat="server" CssClass="tbltxtbox" 
                            Width="70px" TabIndex="2">
                        </asp:DropDownList>
                        &nbsp;Pre-Admission Date <span class="error">*</span> <span>
                            <asp:TextBox ID="txtAdmndate" CssClass="vsmalltb" runat="server" ReadOnly="False"
                                TabIndex="3" Width="100px"></asp:TextBox>&nbsp;
                            <rjs:PopCalendar ID="PopCalAdmnDt" runat="server" Control="txtAdmndate"></rjs:PopCalendar>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/refresh_icon.png"
                                OnClientClick="return clearText('1');" ImageAlign="AbsBottom" />
                        </span>&nbsp; &nbsp; &nbsp; Admission Misc Ref No : <span>
                            <asp:TextBox ID="txtadmsnno" MaxLength="20" Width="100px" runat="server" CssClass="smalltb"
                                TabIndex="4"></asp:TextBox></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="7" class="spacer">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="addmnhead">
                        Personal Details
                    </td>
                    <td colspan="4" class="addmnhead">
                        Parents Detail
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
                        <asp:TextBox ID="txtname" MaxLength="50" runat="server" CssClass="largetb" 
                            TabIndex="5"></asp:TextBox>
                    </td>
                    <td colspan="3" rowspan="10" valign="top">
                        <table width="100%" cellpadding="1" cellspacing="1" style="height: 219px">
                            <tr>
                                <td align="left" valign="top" colspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td width="200" align="left" valign="top" class="tbltxt">
                                    Father's Name <span class="error">*</span>
                                </td>
                                <td width="5" align="left" valign="top" class="tbltxt">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtfathername" MaxLength="30" CssClass="largetb" runat="server"
                                        ValidationGroup="a" TabIndex="17"></asp:TextBox>
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
                                        TabIndex="181"></asp:TextBox>
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
                                    <asp:TextBox ID="txtFatAdhar" CssClass="largetb" MaxLength="30" runat="server" ValidationGroup="a"
                                        TabIndex="19"></asp:TextBox>
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
                                        ValidationGroup="a" TabIndex="25"></asp:TextBox>
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
                                        TabIndex="18"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Mother's Adhar Card No
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtMotAdhar" CssClass="largetb" MaxLength="30" runat="server" ValidationGroup="a"
                                        TabIndex="16"></asp:TextBox>
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
                                        ValidationGroup="a" TabIndex="19"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Relation with Local Guardian
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtRelWithLocalGuard" MaxLength="30" CssClass="largetb" runat="server"
                                        ValidationGroup="a" TabIndex="20"></asp:TextBox>
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
                                    <asp:FileUpload ID="fldUpDoc" runat="server" TabIndex="21" Width="200px" />
                                </td> 
                                <td>
                                <asp:HyperLink ID="hlDoc" runat="server" Target="_blank">[hlDoc]</asp:HyperLink>
                                </td>   
                            
                             </tr>
                        </table>
                    </td>
                    <td width="135" rowspan="7" align="center" bgcolor="#c4c4c4">
                        <img id="imgStud" runat="server" width="120" height="150" borderstyle="Groove" borderwidth="2px" />
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Nick Name
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtnickname" MaxLength="30" runat="server" CssClass="smalltb" 
                            TabIndex="6"></asp:TextBox>
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
                        <asp:TextBox ID="txtdob" runat="server" ReadOnly="False" CssClass="vsmalltb" 
                            TabIndex="7"></asp:TextBox>&nbsp;<rjs:PopCalendar
                            ID="PopCalDOB" runat="server" Control="txtdob"></rjs:PopCalendar>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/refresh_icon.png"
                            OnClientClick="return clearText('2');" ImageAlign="AbsBottom" />
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Gender <span class="error">*</span>
                    </td>
                    <td class="tbltxt">
                        &nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="drpGender" runat="server" CssClass="vsmalltb" 
                            TabIndex="8">
                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>
                        </asp:DropDownList>
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
                        <asp:DropDownList ID="drprelgn" runat="server" CssClass="vsmalltb" TabIndex="9">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Nationality<span class="error"> *</span>
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtnationality" MaxLength="30" CssClass="smalltb" runat="server"
                            ValidationGroup="a" TabIndex="10"></asp:TextBox>
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
                        <asp:DropDownList ID="drpcat" runat="server" CssClass="vsmalltb" TabIndex="11">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Mother Tongue <span class="error">*</span>
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMotherTongue" runat="server" MaxLength="20" CssClass="smalltb"
                            TabIndex="12"></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:Label runat="server" Font-Bold="true" ID="lblAdmnNo"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Locality <span class="error">*</span>
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpLocality" runat="server" CssClass="smalltb" 
                            TabIndex="13">
                            <asp:ListItem>Urban</asp:ListItem>
                            <asp:ListItem>Rural</asp:ListItem>
                            <asp:ListItem>Tribal</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;
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
                    
                     <asp:DropDownList ID="drpBloodGroup" runat="server" CssClass="vsmalltb" 
                            TabIndex="14">
                            <asp:ListItem>---</asp:ListItem>
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
                    <td align="center">
                        
                    </td>
                </tr>
                 <tr>
                                <td align="left" class="tbltxt" valign="top">
                                    Adhar Card No
                                </td>
                                <td align="left" class="tbltxt" valign="top">
                                    :
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtStudAdhar" CssClass="largetb" MaxLength="30" runat="server" ValidationGroup="a"
                                        TabIndex="15"></asp:TextBox>
                                </td>
                            </tr>
                
                <tr>
                    <td class="tbltxt">
                        Upload Image
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:FileUpload ID="fldUpImage" runat="server" TabIndex="16" Width="200px" />
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="7" class="spaceborder">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="50%" valign="top">
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="addmnhead" colspan="3" valign="top" align="left">
                                                Permanent Address
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="left" valign="top" class="spacer">
                                                <img src="../images/mask.gif" width="8" height="8" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="110" align="left" valign="top" class="tbltxt">
                                                Address
                                            </td>
                                            <td width="5" align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPermAddr" MaxLength="60" runat="server" CssClass="smalltb" Width="250px"
                                                    TabIndex="21" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Police Station
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPermPS" runat="server" MaxLength="30" CssClass="smalltb" Width="150px"
                                                    TabIndex="22"></asp:TextBox>
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
                                                <asp:TextBox ID="txtPermDist" MaxLength="30" runat="server" CssClass="smalltb" 
                                                    TabIndex="23"></asp:TextBox>
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
                                                    MaxLength="6" CssClass="smalltb" Width="100px" TabIndex="24"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Phone&nbsp;
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top" class="tbltxt">
                                                <asp:TextBox ID="txtPrePhone" runat="server" CssClass="smalltb" Width="100px" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    MaxLength="15" TabIndex="25"></asp:TextBox>
                                                Mobile&nbsp;:
                                                <asp:TextBox ID="txtPreMob" runat="server" CssClass="smalltb" Width="100px" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    MaxLength="14" TabIndex="26"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Email Id&nbsp;
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPreEmail" runat="server" CssClass="smalltb" MaxLength="60" 
                                                    TabIndex="27"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="addmnhead" colspan="3" valign="top">
                                                Present Address
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="left" valign="top" class="spacer">
                                                <img src="../images/mask.gif" width="8" height="8" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="left" valign="top">
                                                <input class="button" name="btnCopyAddress" onclick="filladdress();" type="button"
                                                    value="Same As Permanent Address" tabindex="28" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100" align="left" valign="top" class="tbltxt">
                                                Address
                                            </td>
                                            <td width="5" align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPreAddr" runat="server" CssClass="largeta" MaxLength="60" TextMode="MultiLine"
                                                    TabIndex="29"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Police Station&nbsp;
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtPrePS" runat="server" CssClass="smalltb" Width="150px" MaxLength="30"
                                                    TabIndex="30"></asp:TextBox>
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
                                                <asp:TextBox ID="txtPreDist" runat="server" CssClass="smalltb" Width="150px" MaxLength="30"
                                                    TabIndex="31"></asp:TextBox>
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
                                                <asp:TextBox ID="txtPrePin" runat="server" CssClass="smalltb" Width="100px" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    MaxLength="6" TabIndex="32"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="7" class="spaceborder">
                        <img src="../images/mask.gif" width="8" height="8" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="50%" valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td class="addmnhead" colspan="3" valign="top" align="left">
                                                Previous School Details
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="130" align="left" valign="top" class="tbltxt">
                                                School Last attended
                                            </td>
                                            <td width="5" align="left" valign="top" class="tbltxt">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                <asp:TextBox ID="txtschoolname" runat="server" TabIndex="33" CssClass="smalltb"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Class Last attended
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                <asp:DropDownList ID="drpclass" runat="server" TabIndex="34" CssClass="smalltb">
                                                </asp:DropDownList>
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
                                                <asp:TextBox ID="txtprevmedium" runat="server" TabIndex="35" CssClass="smalltb"></asp:TextBox>
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
                                                <asp:TextBox ID="txtTCNo" runat="server" TabIndex="36" CssClass="smalltb"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                TC Date
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                <asp:TextBox ID="txtTCDt" runat="server" TabIndex="37" CssClass="smalltb"></asp:TextBox>
                                                <rjs:PopCalendar ID="PopCalTCDate" runat="server" Control="txtTCDt" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td class="addmnhead" colspan="3" valign="top" align="left">
                                                Pre - Admission Details
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="130" align="left" valign="top" class="tbltxt">
                                                Present Session
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                <asp:DropDownList ID="drpSession" runat="server" CssClass="smalltb" 
                                                    TabIndex="38">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Present Class
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                <asp:DropDownList ID="drpnewclass" runat="server" CssClass="smalltb" 
                                                    TabIndex="39">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" style="height: 20px" valign="top">
                                                Section
                                            </td>
                                            <td align="left" class="tbltxt" style="height: 20px" valign="top">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top" style="height: 20px">
                                                <asp:DropDownList ID="drpsection" runat="server" TabIndex="40" 
                                                    CssClass="smalltb">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="tbltxt" valign="top">
                                                Roll No
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                :
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                <asp:TextBox ID="txtrollno" runat="server" CssClass="smalltb" TabIndex="41" 
                                                    onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                            <td align="left" class="tbltxt" valign="top">
                                                <asp:CheckBox ID="ChkBus" runat="server" Text="Bus Facility" TabIndex="42" />
                                                <asp:CheckBox ID="ChkHostel" runat="server" Text="Hostel Facility" 
                                                    TabIndex="43" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="7" class="spaceborder">
                        <img src="../images/mask.gif" width="8" height="8" />
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
                                    OnClientClick="return CnfSubmit();" Text="Save" TabIndex="58" 
                                Width="97px" />
                                
                                <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="defaultfont10"
                                    OnClick="btnClear_Click" Text="Clear" TabIndex="59" Width="95px" />
                                <asp:Button ID="btnPrintReceipt1" runat="server" CausesValidation="False" CssClass="defaultfont10"
                                    OnClick="btnPrintReceipt1_Click" Text="Receive Fee" TabIndex="60" />
                                <asp:Button ID="btnList" runat="server" CausesValidation="False" CssClass="defaultfont10"
                                    OnClick="btnList_Click" Text="Student List" TabIndex="61" />
                            </font>
                        </div>
                        <%--this hidden field will contain value to check the save mode(insert/update)--%>
                        <asp:HiddenField ID="hfSaveMode" runat="server" Value="Insert" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

