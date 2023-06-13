<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="ExamMarkEntry.aspx.cs" Inherits="Exam_ExamMarkEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function checkObtMarks(tMaxMarks, objObtMarks) {
            var tObtMarks = document.getElementById(objObtMarks);
            if (parseFloat(tObtMarks.value) > parseFloat(tMaxMarks)) {
                document.getElementById(objObtMarks).value = "";
                alert("Secured Marks shouldn't be greater than Max Marks");
                document.getElementById(objObtMarks).focus();
                return false;
            }
        }
        function cnfSubmit() {

            if (confirm("Do you want to continue ?")) {

                return true;
            }
            else {
                return false;
            }
        }
    </script>

    <script type="text/javascript" language="javascript">

        function isValid() {
            var Session = document.getElementById("<%=drpSession.ClientID %>").selectedIndex;
            var Class = document.getElementById("<%=drpClass.ClientID %>").selectedIndex;
            var Exam = document.getElementById("<%=drpExam.ClientID %>").selectedIndex;
            var Student = document.getElementById("<%=drpStudent.ClientID %>").selectedIndex;
            //get current date
            if (Session == "0") {
                alert("Select a Session");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            else if (Class == "0") {
                alert("Select a Class");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            else if (Exam == "0") {
                alert("Select an Exam");
                document.getElementById("<%=drpExam.ClientID %>").focus();
                return false;
            }
            else if (Student == "0") {
                alert("Select a Student");
                document.getElementById("<%=drpStudent.ClientID %>").focus();
                return false;
            }
            else {
                CheckLoader();
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

        function extractNumber(obj, decimalPlaces, allowNegative) {
            var temp = obj.value;

            // avoid changing things if already formatted correctly
            var reg0Str = '[0-9]*';
            if (decimalPlaces > 0) {
                reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
            } else if (decimalPlaces < 0) {
                reg0Str += '\\.?[0-9]*';
            }
            reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
            reg0Str = reg0Str + '$';
            var reg0 = new RegExp(reg0Str);
            if (reg0.test(temp)) return true;

            // first replace all non numbers
            var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
            var reg1 = new RegExp(reg1Str, 'g');
            temp = temp.replace(reg1, '');

            if (allowNegative) {
                // replace extra negative
                var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
                var reg2 = /-/g;
                temp = temp.replace(reg2, '');
                if (hasNegative) temp = '-' + temp;
            }

            if (decimalPlaces != 0) {
                var reg3 = /\./g;
                var reg3Array = reg3.exec(temp);
                if (reg3Array != null) {
                    // keep only first occurrence of .
                    //  and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
                    var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
                    reg3Right = reg3Right.replace(reg3, '');
                    reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;
                    temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;
                }
            }
            obj.value = temp;
        }

        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';
        }       
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 3px;">
        <h2>
            Enter Exam Marks</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="height: 100%;">
                <tr>
                    <td align="center" valign="top">
                        <div style="text-align: center;">
                            <asp:Label ID="lblMsg" Font-Bold="true" runat="server" Text=""></asp:Label>
                        </div>
                        <div align="left">
                            <div class="innerdiv" style="width: 100%">
                                <div class="linegap">
                                    <img src="../images/mask.gif" width="10" height="10" /></div>
                                <div>
                                    <table width="100%">
                                        <tr style="background-color: #D3E7EE;">
                                            <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                                                color: #000; border: 1px solid #333; background-color: Transparent;">
                                                Session&nbsp;:&nbsp;
                                                <asp:DropDownList ID="drpSession" runat="server" TabIndex="1" AutoPostBack="true"
                                                    OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                Class&nbsp;:&nbsp;
                                                <asp:DropDownList ID="drpClass" runat="server" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                Section&nbsp;:&nbsp;
                                                <asp:DropDownList ID="drpSection" runat="server" TabIndex="3" AutoPostBack="true"
                                                    OnSelectedIndexChanged="drpSection_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                Exam&nbsp;:&nbsp;
                                                <asp:DropDownList ID="drpExam" runat="server" TabIndex="4" AutoPostBack="True" OnSelectedIndexChanged="drpExam_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RadioButtonList ID="rbType" runat="server" AutoPostBack="true" TabIndex="5"
                                                    Enabled="false" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rbType_SelectedIndexChanged">
                                                    <asp:ListItem Text="Studentwise" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Subjectwise" Value="1"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #D3E7EE;">
                                            <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                                                color: #000; border: 1px solid #333; background-color: Transparent;">
                                                Student Name&nbsp;:&nbsp;
                                                <asp:DropDownList ID="drpStudent" runat="server" TabIndex="6" AutoPostBack="True"
                                                    OnSelectedIndexChanged="drpStudent_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                Subject&nbsp;:&nbsp;
                                                <asp:DropDownList ID="drpSubject" runat="server" TabIndex="7" AutoPostBack="True"
                                                    OnSelectedIndexChanged="drpSubject_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <%--<asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return isValid();"
                                                        Text="Search" />--%>
                                                <asp:Label ID="lblRecords" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:GridView ID="grdStudentwise" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    BorderStyle="Solid" BorderWidth="0.5px" OnRowDataBound="grdStudentwise_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Subject">
                                                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSubject" Text='<%#Eval("SubjectName")%>' runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hfExamSubId" runat="server" Value='<%#Eval("ExamSubId")%>' />
                                                                <asp:HiddenField ID="hfPassMarks" runat="server" Value='<%#Eval("PassMarks")%>' />
                                                                <asp:HiddenField ID="hfExamMarksId" runat="server" Value='<%#Eval("ExamMarksId")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Theory Marks">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtTheory" runat="server" onkeypress="return blockNonNumbers(this, event, true, false)"
                                                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                                    Text="0" TabIndex="8" Width="50px"></asp:TextBox>
                                                                <b>&nbsp;/&nbsp;<asp:Label ID="lblTheory" runat="server" Text='<%#Eval("MaxThMarks")%>'></asp:Label></b>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PT-1">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtProject" runat="server" onkeypress="return blockNonNumbers(this, event, true, false)"
                                                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                                    Text="0" TabIndex="8" Width="50px"></asp:TextBox>
                                                                <b>&nbsp;/&nbsp;<asp:Label ID="lblProject" runat="server" Text='<%#Eval("MaxProjMarks")%>'></asp:Label></b>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SE">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPract" runat="server" onkeypress="return blockNonNumbers(this, event, true, false)"
                                                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                                    Text="0" TabIndex="8" Width="50px"></asp:TextBox>
                                                                <b>&nbsp;/&nbsp;<asp:Label ID="lblPract" runat="server" Text='<%#Eval("MaxPractMarks")%>'></asp:Label></b>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="MA/Viva Marks">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="tctmaviva" runat="server" onkeypress="return blockNonNumbers(this, event, true, false)"
                                                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                                    Text="0" TabIndex="8" Width="50px"></asp:TextBox>
                                                                <b>&nbsp;/&nbsp;<asp:Label ID="lblmaviva" runat="server" Text='<%#Eval("MaxMAVIVAMarks")%>'></asp:Label></b>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Project Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtProjName" runat="server" TabIndex="8" Width="200px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Save">
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" TabIndex="8"
                                                                    onfocus="active(this);" onblur="inactive(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:GridView ID="grdSubjectwise" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    BorderStyle="Solid" BorderWidth="0.5px" OnRowDataBound="grdSubjectwise_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Student Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStudName" runat="server" Text='<%#Eval("FullName")%>'></asp:Label>
                                                                <asp:HiddenField ID="hfExamMarksId" runat="server" Value='<%#Eval("ExamMarksId")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Admission No">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAdmnNo" runat="server" Text='<%#Eval("AdmnNo")%>'></asp:Label>
                                                                <asp:HiddenField ID="hfPassMarks" runat="server" Value='<%#Eval("PassMarks")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Theory Marks">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtTheory" runat="server" onkeypress="return blockNonNumbers(this, event, true, false)"
                                                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                                    Text="0" TabIndex="8" Width="50px"></asp:TextBox>
                                                                <b>&nbsp;/&nbsp;<asp:Label ID="lblTheory" runat="server" Text='<%#Eval("MaxThMarks")%>'></asp:Label></b>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Proj. Marks">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtProject" runat="server" onkeypress="return blockNonNumbers(this, event, true, false)"
                                                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                                    Text="0" TabIndex="8" Width="50px"></asp:TextBox>
                                                                <b>&nbsp;/&nbsp;<asp:Label ID="lblProject" runat="server" Text='<%#Eval("MaxProjMarks")%>'></asp:Label></b>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pract. Marks">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPract" runat="server" onkeypress="return blockNonNumbers(this, event, true, false)"
                                                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                                    Text="0" TabIndex="8" Width="50px"></asp:TextBox>
                                                                <b>&nbsp;/&nbsp;<asp:Label ID="lblPract" runat="server" Text='<%#Eval("MaxPractMarks")%>'></asp:Label></b>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="MA/Viva Marks">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="tctmaviva" runat="server" onkeypress="return blockNonNumbers(this, event, true, false)"
                                                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                                    Text="0" TabIndex="8" Width="50px"></asp:TextBox>
                                                                <b>&nbsp;/&nbsp;<asp:Label ID="lblmaviva" runat="server" Text='<%#Eval("MaxMAVIVAMarks")%>'></asp:Label></b>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Project Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtProjName" runat="server" TabIndex="8" Width="200px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Save">
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" onfocus="active(this);"
                                                                    onblur="inactive(this);" TabIndex="8" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="drpSession" />
            <asp:PostBackTrigger ControlID="drpClass" />
            <asp:PostBackTrigger ControlID="drpSection" />
            <asp:PostBackTrigger ControlID="drpExam" />
            <asp:PostBackTrigger ControlID="drpStudent" />
            <asp:PostBackTrigger ControlID="drpSubject" />
            <asp:PostBackTrigger ControlID="rbType" />
            <%--<asp:PostBackTrigger ControlID="btnSearch" />--%>
            <asp:PostBackTrigger ControlID="grdStudentwise" />
            <asp:PostBackTrigger ControlID="grdSubjectwise" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
