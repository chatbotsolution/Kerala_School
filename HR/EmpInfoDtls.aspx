<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpInfoDtls.aspx.cs" Inherits="HR_EmpInfoDtls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
        function ReplaceEmptyFields(orig, repl) {
            if (orig == "") {
                document.write(repl);
            }
            else {
                document.write(orig);
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
        function valid() {
            var flag;
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {

                    if (e.checked) {
                        flag = true;
                        break;
                    }
                    else
                        flag = false;
                }
            }
            //alert(flag);
            if (flag == true)
                return true;
            else {
                alert("Please select any record");
                return false;
            }
        }
        function CnfDelete() {

            if (confirm("You are going to delete a record. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Employee Details</h2>
        <div style="float: right;">
            <asp:Label ID="lblMsgTop" Font-Bold="true" ForeColor="Green" runat="server" Text=""
                CssClass="tbltxt"></asp:Label>
        </div>
    </div>
    <div class="tblhead" style="padding-left: 15px;">
        <br />
        <asp:Label ID="lblEmpName" runat="server" Font-Size="18px"></asp:Label>
        <div style="float: right; padding-right: 15px;">
            <asp:Button ID="btnBack" runat="server" Text="Back To List" TabIndex="1" OnClick="btnBack_Click" />&nbsp;&nbsp;
        </div>
    </div>
    <div align="center">
        <div class="innerdiv" style="width: 99%">
            <div style="padding: 8px;">
                <div style="text-align: left;">
                    <ajaxToolkit:Accordion ID="Accordion1" runat="server" FadeTransitions="True" SelectedIndex="0"
                        TransitionDuration="300" HeaderCssClass="accordionHeader" ContentCssClass="accordionContent">
                        <Panes>
                            <ajaxToolkit:AccordionPane ID="APEmpDet" runat="server">
                                <Header>
                                    <div id="header">
                                        &raquo;Employee Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:DataList ID="DLEmployee" runat="server" BackColor="Transparent" BorderStyle="None"
                                                        BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="Both" Width="99%"
                                                        OnItemDataBound="DLEmployee_ItemDataBound">
                                                        <ItemTemplate>
                                                            <table border="0" width="100%">
                                                                <tr>
                                                                    <td align="left" colspan="4" class="td-all">
                                                                        <asp:Button ID="btnEditEmp" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"EmpId")%>'
                                                                            CommandName="edit" Text="Edit" OnClick="btnEditEmp_Click" />
                                                                        <asp:Button ID="btnAddNew" Text="Add a New Employee" runat="server" OnClick="btnAddNew_Click" />
                                                                        <asp:HiddenField ID="hfAppId" runat="server" Value='<%#Eval("ApplicantId") %>' />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%;">
                                                                        <table align="left" border="0" width="100%" style="border: solid 1px gray; height: 150px;"
                                                                            cellpadding="3" cellspacing="3">
                                                                            <tr>
                                                                                <td colspan="5" style="height: 25px;" class="headerrow">
                                                                                    Appointment Details
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" style="width: 150px">
                                                                                    Appointment Order No:
                                                                                </td>
                                                                                <td align="left" style="width: 200px">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "AppointmentOrderNo")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                                <td rowspan="8" style="width: 120px; height: 150px; padding: 2px;">
                                                                                    <img style="border: solid 2px black; height: 140px;" width="100%" src='../Up_Files/EmpImages/<%#Eval("ImageFileName") %>' />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Present Office Type:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "SchoolAddress")%>', "<font color='gray'>Not Defined</font>")</script>

                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Present Office Name:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EstablishmentName")%>', "<font color='gray'>Not Defined</font>")</script>

                                                                                </td>
                                                                                <td align="right">
                                                                                    First Joining Place:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "FirstJoiningPlace")%>', "<font color='gray'>Not Defined</font>")</script>

                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Designation:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "Designation")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table align="left" border="0" width="100%" style="border: solid 1px gray; height: 180px;"
                                                                            cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td colspan="4" class="headerrow" style="height: 25px;">
                                                                                    Employee Details
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" style="width: 150px">
                                                                                    Father's Name:
                                                                                </td>
                                                                                <td align="left" style="width: 300px">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "FatherName")%>', "<font color='gray'>N/A</font>")</script>

                                                                                    &nbsp;
                                                                                </td>
                                                                                <td align="left" style="width: 200px">
                                                                                    Gender:
                                                                                </td>
                                                                                <td align="left">
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Sex")%>&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Mother's Name:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "MotherName")%>', "<font color='gray'>N/A</font>")</script>

                                                                                    &nbsp;
                                                                                </td>
                                                                                <td align="left">
                                                                                    Category:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "Category")%>', "<font color='gray'>N/A</font>")</script>

                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Spouse's Name:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "SpouseName")%>', "<font color='gray'>N/A</font>")</script>

                                                                                    &nbsp;
                                                                                </td>
                                                                                <td align="left">
                                                                                    Religion:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "Religion")%>', "<font color='gray'>N/A</font>")</script>

                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Date Of Birth:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "DOB")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                                <td align="left">
                                                                                    Educational Qualification:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EduQual")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table align="left" border="0" width="100%" style="border: solid 1px gray; height: 150px;"
                                                                            cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td colspan="4" style="height: 25px;" class="headerrow">
                                                                                    Address Details
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" style="width: 150px">
                                                                                    Present Address:
                                                                                </td>
                                                                                <td align="left" style="width: 300px">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PresAddress")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                                <td align="left" style="width: 200px">
                                                                                    Permanent Address:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PermAddress")%>', "<font color='gray'>N/A</font>")</script>

                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    District:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PresDist")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                                <td align="left">
                                                                                    District:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PermDist")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Pincode:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PresAddrPin")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                                <td align="left">
                                                                                    Pincode:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PermAddrPin")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Contact No.:
                                                                                </td>
                                                                                <td align="left">
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Phone")%>,
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Mobile")%>&nbsp;
                                                                                </td>
                                                                                <td align="left">
                                                                                    Email ID:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "emailid")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%;">
                                                                        <table align="left" border="0" width="100%" style="border: solid 1px gray;" cellpadding="0"
                                                                            cellspacing="0">
                                                                            <tr>
                                                                                <td colspan="4" style="height: 25px;" class="headerrow">
                                                                                    Other Details
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    EPF Ac No:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EPFAcNo")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                                <td align="left">
                                                                                    Bank A/c No:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EmpBankAcNo")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    EPF A/c Dt:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EPFAcDt")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                                <td align="left">
                                                                                    Bank Name:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "BankName")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    GSLI Done:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "GSLI_Done")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                                <td align="left">
                                                                                    Branch Name (IFSC Code) :
                                                                                </td>
                                                                                <td align="left">
                                                                                    
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "Branch")%>', "<font color='gray'>N/A</font>")</script>
                                                                                 (<%#DataBinder.Eval(Container.DataItem, "IFSCCode")%>)
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Diposit Amount & Date:
                                                                                </td>
                                                                                <td align="left">
                                                                                    <b>Rs.<%#DataBinder.Eval(Container.DataItem, "GSLIDipAmt")%></b>&nbsp;on&nbsp;<b><script
                                                                                        language="javascript">                                                                                                                                                                         ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "GSLIDate")%>', "<font color='gray'>N/A</font>")</script></b>
                                                                                </td>
                                                                                <td align="left">
                                                                                    Acharya Type:
                                                                                </td>
                                                                                <td align="left">

                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "AcharyaType")%>', "<font color='gray'>N/A</font>")</script>

                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%;">
                                                                        <table align="left" border="0" width="100%" style="border: solid 1px gray;" cellpadding="0"
                                                                            cellspacing="0">
                                                                            <tr>
                                                                                <td colspan="4" style="height: 25px;" class="headerrow">
                                                                                    Educational Qualification
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" colspan="3">
                                                                                    <asp:GridView ID="grdEduQualApp" runat="server" AutoGenerateColumns="False" DataKeyNames="EmpQualId"
                                                                                        Width="100%" AllowPaging="True" OnPageIndexChanging="grdEduQualApp_PageIndexChanging"
                                                                                        CssClass="gridtext">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Sl. No.">
                                                                                                <HeaderStyle Width="40px" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblSlNo" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText=" Qualification">
                                                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                                <ItemTemplate>
                                                                                                    <%#Eval("QualName")%>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText=" Board/University Name">
                                                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                                <ItemTemplate>
                                                                                                    <%#Eval("BoardUnivName")%>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Subject">
                                                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                                <ItemTemplate>
                                                                                                    <%#Eval("Subjects")%>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Mark(%)">
                                                                                                <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                                                                                <ItemTemplate>
                                                                                                    <%#Eval("MarkPercent")%>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="center" Width="90px" VerticalAlign="Middle" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                        <EmptyDataTemplate>
                                                                                            No Records
                                                                                        </EmptyDataTemplate>
                                                                                        <PagerStyle CssClass="headergrid" HorizontalAlign="Center" />
                                                                                        <HeaderStyle CssClass="headergrid" />
                                                                                        <AlternatingRowStyle CssClass="gridtext" />
                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                            <%--<ajaxToolkit:AccordionPane ID="APAppointDet" runat="server">
                                <Header>
                                    <div id="header">
                                        &raquo;Appointment Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="btnAddNewApp" Text="Add New" runat="server" OnClick="btnAddNewApp_Click" />
                                                    <asp:Button ID="btnDeleteApp" Text="Delete Selected Records" runat="server" OnClick="btnDeleteApp_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblAppontRC" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:GridView ID="grdPastAppDetails" runat="server" AutoGenerateColumns="False" DataKeyNames="PastApptId"
                                                        Width="100%" AllowPaging="True" OnPageIndexChanging="grdPastAppDetails_PageIndexChanging"
                                                        CssClass="gridtext">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                                <HeaderStyle Width="15px" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div visible="false" id="divDisabled" runat="server">
                                                                        <input name="des" type="checkbox" disabled="disabled" />
                                                                    </div>
                                                                    <div id="divEnabled" runat="server">
                                                                        <input name="Checkb" type="checkbox" value='<%#Eval("PastApptId")%>' /></div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <HeaderStyle Width="30px" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div visible="false" id="divDisabled1" runat="server">
                                                                        <a href="#">Edit</a></div>
                                                                    <div id="divEnabled1" runat="server">
                                                                        <a class="viewdetail" href='EmpPastAppmtDtls.aspx?Appointno=<%#Eval("PastApptId")%>'>
                                                                            Edit</a>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sl. No.">
                                                                <HeaderStyle Width="40px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSlNo" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Establishment Name">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("EstablishmentName")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Address">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("SchoolAddress")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Designation">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Designation")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Date">
                                                                <HeaderStyle Width="90px" HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("FromDtD")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To Date">
                                                                <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ToDt")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="90px" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Transfer Order No">
                                                                <HeaderStyle Width="130px" HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("TransferOrderNo")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="130px" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Transfer Date">
                                                                <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("TransferDt")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="90px" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No Records
                                                        </EmptyDataTemplate>
                                                        <PagerStyle CssClass="headergrid" HorizontalAlign="Center" />
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <AlternatingRowStyle CssClass="gridtext" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="APTraining" runat="server">
                                <Header>
                                    <div id="header">
                                        &raquo;Training Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="btnAddNewTraining" Text="Add New" runat="server" OnClick="btnAddNewTraining_Click" />
                                                    <asp:Button ID="btnDeleteTraining" Text="Delete Selected Records" runat="server"
                                                        OnClick="btnDeleteTraining_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblTrainingRC" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:GridView ID="grdTrainingDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        DataKeyNames="EmpTrgId" OnPageIndexChanging="grdTrainingDetails_PageIndexChanging"
                                                        Width="695px" CssClass="gridtext">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="15px" />
                                                                <HeaderStyle />
                                                                <ItemTemplate>
                                                                    <input name="Checkb" type="checkbox" value='<%#Eval("EmpTrgId")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <HeaderStyle Width="20px" />
                                                                <ItemTemplate>
                                                                    <a class="viewdetail" href='EmpTrainingDetails.aspx?trainingno=<%#Eval("EmpTrgId")%>'>
                                                                        Edit</a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sl.No.">
                                                                <HeaderStyle Width="30px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("TrgSlNo")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Name of Training">
                                                                <HeaderStyle Width="250px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("TrgName")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Place of Training">
                                                                <HeaderStyle Width="200px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("TrgPlace")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Date">
                                                                <HeaderStyle Width="90px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("FromDt")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To Date">
                                                                <HeaderStyle Width="90px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ToDt")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No Records
                                                        </EmptyDataTemplate>
                                                        <PagerStyle CssClass="headergrid" HorizontalAlign="Center" />
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <AlternatingRowStyle CssClass="gridtext" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </Content>
                            </ajaxToolkit:AccordionPane>--%>
                            <ajaxToolkit:AccordionPane ID="APDakshina" runat="server">
                                <Header>
                                    <div id="header">
                                        &raquo;Salary Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="btnAddNewDakshina" Text="Add New" runat="server" OnClick="btnAddNewDakshina_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblDakshinaRC" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblSalary" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                        </Panes>
                    </ajaxToolkit:Accordion>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
