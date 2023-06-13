<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="ExamSubjectList.aspx.cs" Inherits="Exam_ExamSubjectList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';
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
                alert("Select any Record");
                return false;
            }
        }
        function CnfDelete() {

            if (confirm("You are going to Delete a Record. Do you want to Continue ?")) {
                CheckLoader();
                return true;
            }
            else {
                return false;
            }
        }
        function IsValid() {
            var Class = document.getElementById("<%=drpClass.ClientID %>").value;
            var exam = document.getElementById("<%=drpExamName.ClientID %>").value;
            if (Class == "0") {
                alert("Select a Class");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            else if (exam == "0") {
                alert("Select an Exam Name");
                document.getElementById("<%=drpExamName.ClientID %>").focus();
                return false;
            }
            else {
                CheckLoader();
                return true;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 3px;">
        <h2>
            Define Exams Subjects</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="0" width="100%">
                <tr id="trMsg" runat="server">
                    <td style="height: 20px;" align="center">
                        <asp:Label runat="server" ID="lblMsg" Font-Bold="true" ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="font-family: Tahoma, Geneva, sans-serif; color: #000; padding-left: 5px;
                        border: 1px solid #333; background-color: transparent; height: 40px">
                        Session&nbsp;:
                        <asp:DropDownList ID="drpSession" runat="server" Width="100px" BackColor="White"
                            AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                            TabIndex="1" onchange="CheckLoader();">
                        </asp:DropDownList>
                        Class&nbsp;:&nbsp;<asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="drpClass_SelectedIndexChanged" onchange="CheckLoader();"
                            TabIndex="2">
                        </asp:DropDownList>
                        Exam Name&nbsp;:&nbsp;<asp:DropDownList ID="drpExamName" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="drpExamName_SelectedIndexChanged" onchange="CheckLoader();"
                            TabIndex="3">
                        </asp:DropDownList>
                        <%--Subject Name&nbsp;:&nbsp;<asp:DropDownList ID="drpSubject" runat="server" TabIndex="4">
                        </asp:DropDownList>--%>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                            onfocus="active(this);" onblur="inactive(this);" OnClientClick="return IsValid();"
                            TabIndex="5" />
                        <asp:Button ID="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" onfocus="active(this);"
                            onblur="inactive(this);" TabIndex="6" />
                        <asp:Button ID="btnDelete" runat="server" Text="Delete Subject" OnClientClick="return CnfDelete();"
                            OnClick="btnDelete_Click" onfocus="active(this);" onblur="inactive(this);" TabIndex="7" />
                        <%--<asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click"  onfocus="active(this);" onblur="inactive(this);" TabIndex="7" />--%>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblRecCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <asp:GridView ID="gvExamSubject" runat="server" AutoGenerateColumns="False" DataKeyNames="ExamSubId"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="15px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                    <ItemTemplate>
                                        <input name="Checkb" type="checkbox" value='<%#Eval("ExamSubId")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <a href='ExamSubject.aspx?Sid=<%#Eval("ExamSubId")%>'>Edit</a>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exam Name">
                                    <ItemTemplate>
                                        <%#Eval("ExamName")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subject">
                                    <ItemTemplate>
                                        <%#Eval("SubjectName")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Theory Marks">
                                    <ItemTemplate>
                                        <%#Eval("FullMarks")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Practical Marks">
                                    <ItemTemplate>
                                        <%#Eval("PractMarks")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Project Marks">
                                    <ItemTemplate>
                                        <%#Eval("ProjMarks")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pass Marks (Th.+Pract.+Proj.) ">
                                    <ItemTemplate>
                                        <%#Eval("PassMarks")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnExport" />--%>
            <asp:PostBackTrigger ControlID="drpClass" />
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>
    <div id="Loader" style="text-align: center; vertical-align: middle; position: absolute;
        top: 0px; left: 0px; z-index: 99; width: 100%; height: 100%; background-color: #ededed;
        -ms-filter: 'progid:DXImageTransform.Microsoft.Alpha(Opacity=60)'; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=60);
        -moz-opacity: 0.8; opacity: 0.8;">
        <div style="width: 48px; height: 48px; margin: 0 auto; margin-top: 275px;">
            <img src="../images/spinner.gif">
        </div>
        <div style="font-family: Trebuchet MS; font-size: 12px; color: Green; text-align: center;">
            Please Wait ...</div>
    </div>
</asp:Content>

