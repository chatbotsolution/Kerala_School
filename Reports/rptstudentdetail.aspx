<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptstudentdetail.aspx.cs" Inherits="Reports_rptstudentdetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function printcontent() {

            var DocumentContainer = document.getElementById('divreport1');

            var documentheader = document.getElementById('divhdr');
            var WindowObject = window.open('', "TrackData",
                             "width=420,height=225,top=250,left=345,toolbars=no,scrollbars=no,status=no,resizable=yes");
            WindowObject.document.write(documentheader.innerHTML + "\n" + DocumentContainer.innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            WindowObject.close();
            return false;
        }
 

    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Student Details
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%"  border="0" cellspacing="2" cellpadding="2" class="cnt-box">
                <tr>
                    <td class="tbltxt" style="width: 55px">
                        Session
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td style="width: 150px">
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                            OnSelectedIndexChanged="drpSession_SelectedIndexChanged" TabIndex="1">
                        </asp:DropDownList>
                    </td>
                    <td class="tbltxt" style="width: 50px">
                        Class
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td width="120">
                        <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                            CssClass="vsmalltb" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                    <td class="tbltxt" style="width: 70px">
                        Section
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td style="width: 100px">
                        <asp:DropDownList ID="ddlSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSection_SelectedIndexChanged"
                            CssClass="vsmalltb" TabIndex="3">
                        </asp:DropDownList>
                    </td>
                     <td class="tbltxt">
                        Stream
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td class="tbltxt">
                        <asp:DropDownList ID="ddlStream" runat="server" CssClass="vsmalltb" TabIndex="13" AutoPostBack="true">
                        
                        </asp:DropDownList>
                    </td>
                   
                </tr>
                <tr>
                 <td class="tbltxt" style="width: 45px">
                        Gender
                    </td>
                    <td class="tbltxt" style="width: 5px">
                        :
                    </td>
                    <td class="tbltxt">
                        <asp:DropDownList ID="drpGender" runat="server" CssClass="vsmalltb" TabIndex="4">
                            <asp:ListItem  Value=0>All</asp:ListItem>
                            <asp:ListItem Value="Male">Male</asp:ListItem>
                            <asp:ListItem Value="Female">Female</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="tbltxt">
                        Category
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                            CssClass="vsmalltb" TabIndex="4">
                        </asp:DropDownList>
                    </td>
                    <td class="tbltxt">
                        Religion
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlReligion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlReligion_SelectedIndexChanged"
                            CssClass="vsmalltb" TabIndex="5">
                        </asp:DropDownList>
                    </td>
                    <td class="tbltxt">
                        Denomination
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDenomination" runat="server" 
                            CssClass="vsmalltb" TabIndex="5">
                            <asp:ListItem Value="ALL">ALL</asp:ListItem>
                             <asp:ListItem Value="Catholic">Catholic</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    
                   
                </tr>
                <tr>
                   
                    <td class="tbltxt">
                        Admn Type
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpType" runat="server" CssClass="vsmalltb" TabIndex="7">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="N">New</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                     <td class="tbltxt">
                        Status
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td class="tbltxt">
                        <asp:DropDownList ID="drpStatus" runat="server" CssClass="vsmalltb" TabIndex="7">
                        </asp:DropDownList>
                    </td>
                    <td class="tbltxt">
                        Student
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td class="tbltxt" colspan="1" >
                        <asp:DropDownList ID="drpstudents" runat="server" CssClass="vsmalltb"  
                            TabIndex="6">
                        </asp:DropDownList>
                    </td>

                      <td class="tbltxt">
                        Hostel/Bus
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td class="tbltxt" colspan="7" >
                        <asp:DropDownList ID="drpHostStud" runat="server" CssClass="vsmalltb"  
                            TabIndex="6">
                              <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="1">Hostel</asp:ListItem>
                            <asp:ListItem Value="2">Bus</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    
                    <td colspan="12" align="left" class="tbltxt">
                        <asp:Button ID="btngo" runat="server" OnClick="btngo_Click" Text="Search" ToolTip="Click to Search Student Details"
                            TabIndex="8" />&nbsp;
                        <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                            TabIndex="9" />&nbsp; <span id="trbtn" runat="server" visible="false">
                                <asp:Button ID="btnprint" runat="server" Visible="false" Text="Print" TabIndex="10"
                                    OnClick="btnprint_Click" />
                                    <asp:Button ID="btnprint1" runat="server" Visible="false" Text="Print" TabIndex="10"
                                    OnClick="btnprint1_Click" />
                            </span>
                    </td>
                </tr>
            </table>
            <div runat="server" visible="false" id="Tr1">
                <asp:Label ID="lblRecCount" runat="server" Text="Label"></asp:Label>
            </div>
            <div style="background-color: #92d6fb; padding:3px; ">
                <asp:Label ID="lblNote" runat="server" CssClass="tbltxt" Text="<b>Note1 :</b>Select a student from student dropdown to view particular student details."></asp:Label>
            </div>
            <div style="background-color: #6fa4c1; padding:3px;" >
                <asp:Label ID="lblNote0" runat="server" CssClass="tbltxt" 
                    Text="&lt;b&gt;Note2 :&lt;/b&gt;You can select the below CheckBoxes For Required Columns"></asp:Label>
            </div>
            <div id="trgrd" runat="server" style="height: 450px; width: 100%; overflow: scroll;">
               
                <table style="width: 100%; height: 450px;overflow: scroll; table-layout: fixed;" class="tbltxt">
                    <tr>
                        <td style="width: 150px; vertical-align: top;">
                        <asp:CheckBox ID="cbAll" runat="server" Text="All" 
                                oncheckedchanged="cbAll_CheckedChanged" />
                            <br />
                            <asp:CheckBox ID="cbAdmn" runat="server" Text="Admission No." 
                                oncheckedchanged="cbAdmn_CheckedChanged" />
                            <br />
                             <asp:CheckBox ID="cbRollNo" runat="server" Text="Roll No." 
                                oncheckedchanged="cbRollNo_CheckedChanged" />
                            <br />
                           
                            <asp:CheckBox ID="cbAdmnDate" runat="server" Text="Admn Date" 
                                oncheckedchanged="cbAdmnDate_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbAdmnYear" runat="server" Text="Admn Session Year" 
                                oncheckedchanged="cbAdmnYear_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbClass" runat="server" Text="Class" 
                                oncheckedchanged="cbClass_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbSection" runat="server" Text="Section" 
                                oncheckedchanged="cbSection_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbPresentSes" runat="server" Text="Present Session" 
                                oncheckedchanged="cbPresentSes_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbName" runat="server" Text="Name" 
                                oncheckedchanged="cbName_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbDOB" runat="server" Text="DOB" oncheckedchanged="cbDOB_CheckedChanged"/>
                            <br />
                             <asp:CheckBox ID="cbAge" runat="server" Text="Age" 
                                oncheckedchanged="cbAge_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbSex" runat="server" Text="Gender" 
                                oncheckedchanged="cbSex_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbReligion" runat="server" Text="Religion" 
                                oncheckedchanged="cbReligion_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbDenom" runat="server" Text="Denomination" 
                                oncheckedchanged="cbDenom_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbCat" runat="server" Text="Category" 
                                oncheckedchanged="cbCat_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbFather" runat="server" Text="Father's Name" 
                                oncheckedchanged="cbFather_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbMother" runat="server" Text="Mother's Name" 
                                oncheckedchanged="cbMother_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbLocalGrdName" runat="server" Text="Guardian Name" 
                                oncheckedchanged="cbLocalGrdName_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbStream" runat="server" Text="Stream" 
                                oncheckedchanged="cbStream_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbPresentAddress" runat="server" Text="Present Address" 
                                oncheckedchanged="cbPresentAddress_CheckedChanged"/>
                             <br />
                            <asp:CheckBox ID="cbTelephone" runat="server" Text="Telephone" 
                                oncheckedchanged="cbTelephone_CheckedChanged"/>
                             <br />
                            <asp:CheckBox ID="cbEmail" runat="server" Text="EmailId" 
                                oncheckedchanged="cbEmail_CheckedChanged"/>
                             <br />
                            <asp:CheckBox ID="cbStatus" runat="server" Text="Status"/>
                             <br />
                            <asp:CheckBox ID="cbStatusDate" runat="server" Text="Status Date"/>
                             <br />
                              <%--<asp:CheckBox ID="cbOldAdmn" runat="server" Text="Old Admn No." 
                                oncheckedchanged="cbOldAdmn_CheckedChanged" Visible="false" />
                            <br />--%>
                        </td>
                        <td style="width:auto">
                            <asp:Label ID="lblreport" runat="server"> </asp:Label></td>
                    </tr>
                </table>
            </div>
            <div id="trlist" runat="server" visible="false" >
                <asp:DataList ID="liststudent" runat="server" BorderWidth="0" Width="100%" CssClass="tbltxt cnt-box">
                    <ItemTemplate>
                        <table width="100%" border="0" cellspacing="1" cellpadding="1" >
                            <tr>
                                <td colspan="2" width="150" class="tblheader">
                                    <b>Personal Details</b>
                                </td>
                                <td colspan="3" width="150" class="tblheader">
                                    <b>To whomsoever it may concern</b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" width="150" class="tbltd">
                               
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Name :
                                </td>
                                <td class="tbltd" width="150">
                                    <%#Eval("fullname")%>
                                </td>
                                <td  class="tbltd">
                                    Nick Name:
                                </td>
                                <td class="tbltd" width="150">
                                    <%#Eval("nickname")%>
                                </td>
                                <td class="tbltd" rowspan="5" align="left" valign="top">
                                    <img src='../Up_Files/Studimage/<%#Eval("StudentPhoto")%>' alt='<%#Eval("FullName")%>'
                                        width="100px" height="130px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Father's Name :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("fathername")%>
                                </td>
                                <td class="tbltd">
                                    Mother's Name:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("mothername")%>
                                </td>
                                
                               
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Father's Occupation :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("fatheroccupation")%>
                                </td>
                                <td class="tbltd">
                                    Mother's Occupation:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("motheroccupation")%>
                                </td>
                                 
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Local Guardian Name :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("LocalGuardianName")%>
                                </td>
                                <td class="tbltd">
                                    Relation With Local Guardian :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("RelationWithLG")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Present Address:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PresAddr1")%> ,   <%#Eval("PresAddr2")%> 
                                </td>
                                <td class="tbltd">
                                    Permanent Address :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PermAddr1")%>,  <%#Eval("PermAddr2")%>
                                </td>
                                
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Present Dist :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PresAddrDist")%>
                                </td>
                                <td class="tbltd">
                                    Permanent Dist:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PermAddrDist")%>
                                </td>
                                
                                <td class="tbltd">
                                    Admn No :
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Pin :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PresAddrPin")%>
                                </td>
                                <td class="tbltd">
                                    Pin :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("PermAddrPin")%>
                                </td>
                                
                                 <td class="tbltd">
                                    <%#Eval("AdmnNo")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Category :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("CatName")%>
                                </td>
                                <td class="tbltd">
                                    Nationality:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("nationality")%>
                                </td>
                                 <td class="tbltd">
                                    Admn No as per Register:
                                </td>
                                
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Home Phone :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("telnoresidence")%>
                                </td>
                                <td class="tbltd">
                                    Work Phone :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("telenooffice")%>
                                </td>
                                 <td class="tbltd">
                                    <%#Eval("AdmnNo")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Date Of Birth :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("dateofbirth")%>
                                </td>
                                <td class="tbltd">
                                    Gender :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("sex")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Religion :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("Relig")%>
                                </td>
                                <td class="tbltd">
                                    Nationality :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("Nationality")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Stream :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("Description")%>
                                </td>
                                <td class="tbltd">
                                    Mother Tongue :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("MotherTongue")%>
                                </td>
                            </tr>
                             <tr>
                                <td class="tbltd">
                                    Student AdharCard No :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("StudAdhar")%>
                                </td>
                                <td class="tbltd">
                                    Father's AdharCard No :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("FatherAdhar")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Mother's AdharCard No :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("MotherAdhar")%>
                                </td>
                                <td class="tbltd">
                                   
                                </td>
                                <td class="tbltd">
                                   
                                </td>
                            </tr>
                            <tr>
                                <td height="10" colspan="5"  >
                                    <img src="../images/mask.gif" height="8" width="10" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" class="tblheader">
                                    Previous school Details
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Previous School Name :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("prevschoolname")%>
                                </td>
                                <td class="tbltd">
                                    Previous Class:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("prevclassname")%>
                                </td>
                                <td class="tbltd">
                                </td>
                            </tr>
                            <tr>
                                <td height="10" colspan="5" >
                                    <img src="../images/mask.gif" height="8" width="10" />
                                </td>
                                
                            </tr>
                            <tr>
                                <td colspan="5" class="tblheader">
                                    Class Details
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Session :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("sessionyear")%>
                                </td>
                                <td class="tbltd">
                                    Class:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("currentclass")%>
                                </td>
                                <td class="tbltd">
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    Join Date :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("joindate")%>
                                </td>
                                <td class="tbltd">
                                    Section:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("section")%>
                                </td>
                                <td class="tbltd">
                                </td>
                            </tr>
                            <tr>
                                <td height="10" colspan="5"  >
                                    <img src="../images/mask.gif" height="8" width="10" />
                                </td>
                                
                            </tr>
                            <tr>
                                <td colspan="5" class="tblheader">
                                    Bank Details
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    BankName :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("BankName")%>
                                </td>
                                <td class="tbltd">
                                    Bank Ac No:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("BankAcNo")%>
                                </td>
                                <td class="tbltd">
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltd">
                                    IFSCCode :
                                </td>
                                <td class="tbltd">
                                    <%#Eval("IFSCCode")%>
                                </td>
                                <td class="tbltd">
                                    Branch:
                                </td>
                                <td class="tbltd">
                                    <%#Eval("Branch")%>
                                </td>
                                <td class="tbltd">
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExpExcel" />
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
