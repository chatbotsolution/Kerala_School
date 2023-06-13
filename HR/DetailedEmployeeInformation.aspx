<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="DetailedEmployeeInformation.aspx.cs" Inherits="HR_DetailedEmployeeInformation" %>

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
    <div align="center">
        <br />
        <div class="innerdiv" style="width: 99%">
            <div class="tblhead">
                <asp:Button ID="btnBack" runat="server" Text="Back To List" TabIndex="1" 
                    onclick="btnBack_Click" />&nbsp;&nbsp;
                <asp:Label ID="lblEmpName" runat="server"></asp:Label></div>
            <div style="padding: 8px;">
                <div style="text-align: left;">
                    <ajaxToolkit:Accordion ID="Accordion1" runat="server" FadeTransitions="True" SelectedIndex="0"
                        TransitionDuration="300" HeaderCssClass="accordionHeader" ContentCssClass="accordionContent">
                        <Panes>
                            <ajaxToolkit:AccordionPane ID="APEmpDet" runat="server">
                                <Header>
                                    <div id="header">
                                        &raquo;Sevabrati Details</div>
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
                                                                        <asp:Button ID="btnAddNew" Text="Add a New Sevabrati Directly" runat="server" OnClick="btnAddNew_Click" />&nbsp;<asp:Button
                                                                            ID="btnAddNewSS" Text="Add a New Sevabrati Through Acharya Chayana" runat="server"
                                                                            OnClick="btnAddNewSS_Click" />
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
                                                                                <td class="datalisttxt" align="left" style="width: 150px">
                                                                                    Appointment Order No:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt" style="width: 200px">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "AppointmentOrderNo")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left" style="width: 200px">
                                                                                    Acharya Chayan Exam Place:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "ExamCenter")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td rowspan="8" style="width: 120px; height: 150px; padding: 2px;">
                                                                                    <img style="border: solid 2px black; height: 140px;" width="100%" src='../Up_Images/emp/<%#Eval("ImageFileName") %>' />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Appointment Date:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "AppointmentDt")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Exam Date:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "WrittenExamDate")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Present Office Type:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EstablishmentType")%>', "<font color='gray'>Not Defined</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Exam Penal Year:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "SessionYr")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Present Office Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EstablishmentName")%>', "<font color='gray'>Not Defined</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    First Joining Place:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "FirstJoiningPlace")%>', "<font color='gray'>Not Defined</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Designation:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
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
                                                                                    Sevabrati Details
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left" style="width: 150px">
                                                                                    Father's Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt" style="width: 300px">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "FatherName")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td class="datalisttxt" align="left" style="width: 200px">
                                                                                    Gender:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Sex")%>&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Mother's Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "MotherName")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Category:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "Category")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Spouse's Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "SpouseName")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Religion:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "Religion")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Date Of Birth:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "DOB")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Educational Qualification:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
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
                                                                                <td class="datalisttxt" align="left" style="width: 150px">
                                                                                    Present Address:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt" style="width: 300px">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PresAddress")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left" style="width: 200px">
                                                                                    Permanent Address:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PermAddress")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    District:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PresDist")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    District:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PermDist")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Pincode:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PresAddrPin")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Pincode:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PermAddrPin")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Contact No.:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Phone")%>,
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Mobile")%>&nbsp;
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Email ID:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
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
                                                                                <td class="datalisttxt" align="left">
                                                                                    EPF Ac No:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EPFAcNo")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Bank A/c No:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EmpBankAcNo")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    EPF A/c Dt:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EPFAcDt")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Bank Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "BankName")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    GSLI Done:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "GSLI_Done")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Branch Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "Branch")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Diposit Amount & Date:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <b>Rs.<%#DataBinder.Eval(Container.DataItem, "GSLIDipAmt")%></b>on&nbsp;<b><script
                                                                                        language="javascript">                                                                                                                                                                   ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "GSLIDate")%>', "<font color='gray'>N/A</font>")</script></b>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Acharya Type:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
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
                                                                                <td align="left" class="datalisttxt" colspan="3">
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
                            <ajaxToolkit:AccordionPane ID="APAppointDet" runat="server">
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
                                                    <asp:GridView ID="grdPastAppDetails" OnRowDataBound="grdPastAppDetails_RowDataBound"
                                                        runat="server" AutoGenerateColumns="False" DataKeyNames="PastApptId" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="grdPastAppDetails_PageIndexChanging"
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
                                                                        <a class="viewdetail" href='PastAppointmentDetails.aspx?Appointno=<%#Eval("PastApptId")%>'>
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
                                                            <asp:TemplateField HeaderText="Establishment Type">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("EstablishmentType")%>
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
                            <ajaxToolkit:AccordionPane ID="APTeachDet" runat="server">
                                <Header>
                                    <div id="header">
                                        &raquo;Teaching Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="btnAddNewTeach" Text="Add New" runat="server" OnClick="btnAddNewTeach_Click" />
                                                    <asp:Button ID="btnDeleteTeach" Text="Delete Selected Records" runat="server" OnClick="btnDeleteTeach_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblTeachSubRC" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:GridView ID="grdTeachingDetails" runat="server" AutoGenerateColumns="False"
                                                        DataKeyNames="EmpTeachingSubId" Width="650px" AllowPaging="True" OnPageIndexChanging="grdTeachingDetails_PageIndexChanging" OnPreRender="grdTeachingDetails_PreRender"
                                                        CssClass="gridtext">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                                <ItemTemplate>
                                                                    <input name="Checkb" type="checkbox" value='<%#Eval("EmpTeachingSubId")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <HeaderStyle Width="40px" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <a class="viewdetail" href='TeachingDetails.aspx?teachingno=<%#Eval("EmpTeachingSubId")%>'>
                                                                        Edit</a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="SchoolType" HeaderText="School Type" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Stream" HeaderText="Stream" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:TemplateField HeaderText="Subject">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Subject")%>
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
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="APGyanPariksha" runat="server">
                                <Header>
                                    <div id="header">
                                        &raquo;Sanskruti Gyan Pariksha Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="btnAddNewPariksha" Text="Add New" runat="server" OnClick="btnAddNewPariksha_Click" />
                                                    <asp:Button ID="btnDeletePariksha" Text="Delete Selected Records" runat="server"
                                                        OnClick="btnDeletePariksha_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblPrikshaRC" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:GridView ID="grdAddExamDetails" runat="server" AutoGenerateColumns="False" DataKeyNames="EmpAddnlExamId"
                                                        Width="700px" AllowPaging="True" OnPageIndexChanging="grdAddExamDetails_PageIndexChanging"
                                                        CssClass="gridtext">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                                <ItemTemplate>
                                                                    <input name="Checkb" type="checkbox" value='<%#Eval("EmpAddnlExamId")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                                                <ItemTemplate>
                                                                    <a class="viewdetail" href='AdditionalExamDetails.aspx?examno=<%#Eval("EmpAddnlExamId")%>'>
                                                                        Edit</a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Exam Date">
                                                                <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ExamDate")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Exam Details">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ExamDetails")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Exam Result">
                                                                <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ExamResult")%>
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
                                                                    <a class="viewdetail" href='TrainingDetails.aspx?trainingno=<%#Eval("EmpTrgId")%>'>Edit</a>
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
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="APDakshina" runat="server">
                                <Header>
                                    <div id="header">
                                        &raquo;Dakshina Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="btnAddNewDakshina" Text="Add New" runat="server" OnClick="btnAddNewDakshina_Click" />
                                                    <asp:Button ID="btnDeleteDakshina" Text="Delete Selected Records" runat="server"
                                                        OnClick="btnDeleteDakshina_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblDakshinaRC" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="grdDakshinaDetails" runat="server" AutoGenerateColumns="False"
                                                        DataKeyNames="EmpSalId" Width="99%" AllowPaging="True" OnPageIndexChanging="grdDakshinaDetails_PageIndexChanging"
                                                        CssClass="gridtext">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="15px" />
                                                                <HeaderStyle />
                                                                <ItemTemplate>
                                                                    <input name="Checkb" type="checkbox" value='<%#Eval("EmpSalId")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <HeaderStyle Width="20px" />
                                                                <ItemTemplate>
                                                                    <a class="viewdetail" href='SalaryDetails.aspx?salary=<%#Eval("EmpSalId")%>'>Edit</a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Date">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("FromDtate")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To Date">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ToDt")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Basic">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Basic")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DA">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("DA")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Allowance">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Allow")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Special Allowance">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("SplAllow")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="HRA">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("HR")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Others">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Other")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Gross Salary">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Gross")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="EPF Deduction">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("EPFDed")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="GSLI Deduction">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("GSLIDed")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Insurance">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Insurance")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Advance Deduction">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("AdvDed")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Net Salary">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("NetPaybleAmt")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
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
                        </Panes>
                    </ajaxToolkit:Accordion>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
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
    <div align="center">
        <br />
        <div class="innerdiv" style="width: 99%">
            <div class="tblhead">
                <asp:Button ID="Button1" runat="server" Text="Back To List" TabIndex="1" 
                    onclick="btnBack_Click" />&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server"></asp:Label></div>
            <div style="padding: 8px;">
                <div style="text-align: left;">
                    <ajaxToolkit:Accordion ID="Accordion2" runat="server" FadeTransitions="True" SelectedIndex="0"
                        TransitionDuration="300" HeaderCssClass="accordionHeader" ContentCssClass="accordionContent">
                        <Panes>
                            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                                <Header>
                                    <div id="Div1">
                                        &raquo;Sevabrati Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:DataList ID="DataList1" runat="server" BackColor="Transparent" BorderStyle="None"
                                                        BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="Both" Width="99%"
                                                        OnItemDataBound="DLEmployee_ItemDataBound">
                                                        <ItemTemplate>
                                                            <table border="0" width="100%">
                                                                <tr>
                                                                    <td align="left" colspan="4" class="td-all">
                                                                        <asp:Button ID="btnEditEmp" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"EmpId")%>'
                                                                            CommandName="edit" Text="Edit" OnClick="btnEditEmp_Click" />
                                                                        <asp:Button ID="btnAddNew" Text="Add a New Sevabrati Directly" runat="server" OnClick="btnAddNew_Click" />&nbsp;<asp:Button
                                                                            ID="btnAddNewSS" Text="Add a New Sevabrati Through Acharya Chayana" runat="server"
                                                                            OnClick="btnAddNewSS_Click" />
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
                                                                                <td class="datalisttxt" align="left" style="width: 150px">
                                                                                    Appointment Order No:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt" style="width: 200px">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "AppointmentOrderNo")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left" style="width: 200px">
                                                                                    Acharya Chayan Exam Place:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "ExamCenter")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td rowspan="8" style="width: 120px; height: 150px; padding: 2px;">
                                                                                    <img style="border: solid 2px black; height: 140px;" width="100%" src='../Up_Images/emp/<%#Eval("ImageFileName") %>' />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Appointment Date:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "AppointmentDt")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Exam Date:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "WrittenExamDate")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Present Office Type:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EstablishmentType")%>', "<font color='gray'>Not Defined</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Exam Penal Year:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "SessionYr")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Present Office Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EstablishmentName")%>', "<font color='gray'>Not Defined</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    First Joining Place:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "FirstJoiningPlace")%>', "<font color='gray'>Not Defined</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Designation:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
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
                                                                                    Sevabrati Details
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left" style="width: 150px">
                                                                                    Father's Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt" style="width: 300px">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "FatherName")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td class="datalisttxt" align="left" style="width: 200px">
                                                                                    Gender:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Sex")%>&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Mother's Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "MotherName")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Category:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "Category")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Spouse's Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "SpouseName")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Religion:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "Religion")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Date Of Birth:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "DOB")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Educational Qualification:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
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
                                                                                <td class="datalisttxt" align="left" style="width: 150px">
                                                                                    Present Address:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt" style="width: 300px">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PresAddress")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left" style="width: 200px">
                                                                                    Permanent Address:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PermAddress")%>', "<font color='gray'>N/A</font>")</script>
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    District:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PresDist")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    District:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PermDist")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Pincode:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PresAddrPin")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Pincode:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "PermAddrPin")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Contact No.:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Phone")%>,
                                                                                    <%#DataBinder.Eval(Container.DataItem, "Mobile")%>&nbsp;
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Email ID:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
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
                                                                                <td class="datalisttxt" align="left">
                                                                                    EPF Ac No:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EPFAcNo")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Bank A/c No:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EmpBankAcNo")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    EPF A/c Dt:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "EPFAcDt")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Bank Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "BankName")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    GSLI Done:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "GSLI_Done")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Branch Name:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <script language="javascript">                                                                                        ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "Branch")%>', "<font color='gray'>N/A</font>")</script>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Diposit Amount & Date:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
                                                                                    <b>Rs.<%#DataBinder.Eval(Container.DataItem, "GSLIDipAmt")%></b>on&nbsp;<b><script
                                                                                        language="javascript">                                                                                                                                                                   ReplaceEmptyFields('<%#DataBinder.Eval(Container.DataItem, "GSLIDate")%>', "<font color='gray'>N/A</font>")</script></b>
                                                                                </td>
                                                                                <td class="datalisttxt" align="left">
                                                                                    Acharya Type:
                                                                                </td>
                                                                                <td align="left" class="datalisttxt">
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
                                                                                <td align="left" class="datalisttxt" colspan="3">
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
                            <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                                <Header>
                                    <div id="Div2">
                                        &raquo;Appointment Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="Button2" Text="Add New" runat="server" OnClick="btnAddNewApp_Click" />
                                                    <asp:Button ID="Button3" Text="Delete Selected Records" runat="server" OnClick="btnDeleteApp_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label2" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:GridView ID="GridView1" OnRowDataBound="grdPastAppDetails_RowDataBound"
                                                        runat="server" AutoGenerateColumns="False" DataKeyNames="PastApptId" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="grdPastAppDetails_PageIndexChanging"
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
                                                                        <a class="viewdetail" href='PastAppointmentDetails.aspx?Appointno=<%#Eval("PastApptId")%>'>
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
                                                            <asp:TemplateField HeaderText="Establishment Type">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("EstablishmentType")%>
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
                            <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server">
                                <Header>
                                    <div id="Div3">
                                        &raquo;Teaching Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="Button4" Text="Add New" runat="server" OnClick="btnAddNewTeach_Click" />
                                                    <asp:Button ID="Button5" Text="Delete Selected Records" runat="server" OnClick="btnDeleteTeach_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label3" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                                                        DataKeyNames="EmpTeachingSubId" Width="650px" AllowPaging="True" OnPageIndexChanging="grdTeachingDetails_PageIndexChanging" OnPreRender="grdTeachingDetails_PreRender"
                                                        CssClass="gridtext">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                                <ItemTemplate>
                                                                    <input name="Checkb" type="checkbox" value='<%#Eval("EmpTeachingSubId")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <HeaderStyle Width="40px" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <a class="viewdetail" href='TeachingDetails.aspx?teachingno=<%#Eval("EmpTeachingSubId")%>'>
                                                                        Edit</a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="SchoolType" HeaderText="School Type" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Stream" HeaderText="Stream" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:TemplateField HeaderText="Subject">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Subject")%>
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
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                                <Header>
                                    <div id="Div4">
                                        &raquo;Sanskruti Gyan Pariksha Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="Button6" Text="Add New" runat="server" OnClick="btnAddNewPariksha_Click" />
                                                    <asp:Button ID="Button7" Text="Delete Selected Records" runat="server"
                                                        OnClick="btnDeletePariksha_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label4" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" DataKeyNames="EmpAddnlExamId"
                                                        Width="700px" AllowPaging="True" OnPageIndexChanging="grdAddExamDetails_PageIndexChanging"
                                                        CssClass="gridtext">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                                <ItemTemplate>
                                                                    <input name="Checkb" type="checkbox" value='<%#Eval("EmpAddnlExamId")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                                                <ItemTemplate>
                                                                    <a class="viewdetail" href='AdditionalExamDetails.aspx?examno=<%#Eval("EmpAddnlExamId")%>'>
                                                                        Edit</a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Exam Date">
                                                                <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ExamDate")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Exam Details">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ExamDetails")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Exam Result">
                                                                <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ExamResult")%>
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
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="AccordionPane5" runat="server">
                                <Header>
                                    <div id="Div5">
                                        &raquo;Training Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="Button8" Text="Add New" runat="server" OnClick="btnAddNewTraining_Click" />
                                                    <asp:Button ID="Button9" Text="Delete Selected Records" runat="server"
                                                        OnClick="btnDeleteTraining_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label5" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:GridView ID="GridView4" runat="server" AllowPaging="True" AutoGenerateColumns="False"
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
                                                                    <a class="viewdetail" href='TrainingDetails.aspx?trainingno=<%#Eval("EmpTrgId")%>'>Edit</a>
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
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="AccordionPane6" runat="server">
                                <Header>
                                    <div id="Div6">
                                        &raquo;Dakshina Details</div>
                                </Header>
                                <Content>
                                    <div>
                                        <table width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="Button10" Text="Add New" runat="server" OnClick="btnAddNewDakshina_Click" />
                                                    <asp:Button ID="Button11" Text="Delete Selected Records" runat="server"
                                                        OnClick="btnDeleteDakshina_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label6" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="False"
                                                        DataKeyNames="EmpSalId" Width="99%" AllowPaging="True" OnPageIndexChanging="grdDakshinaDetails_PageIndexChanging"
                                                        CssClass="gridtext">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="15px" />
                                                                <HeaderStyle />
                                                                <ItemTemplate>
                                                                    <input name="Checkb" type="checkbox" value='<%#Eval("EmpSalId")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <HeaderStyle Width="20px" />
                                                                <ItemTemplate>
                                                                    <a class="viewdetail" href='SalaryDetails.aspx?salary=<%#Eval("EmpSalId")%>'>Edit</a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Date">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("FromDtate")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To Date">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("ToDt")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Basic">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Basic")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DA">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("DA")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Allowance">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Allow")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Special Allowance">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("SplAllow")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="HRA">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("HR")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Others">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Other")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Gross Salary">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Gross")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="EPF Deduction">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("EPFDed")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="GSLI Deduction">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("GSLIDed")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Insurance">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Insurance")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Advance Deduction">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("AdvDed")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Net Salary">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemTemplate>
                                                                    <%#Eval("NetPaybleAmt")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
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
                        </Panes>
                    </ajaxToolkit:Accordion>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

