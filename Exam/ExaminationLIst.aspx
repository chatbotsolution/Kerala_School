<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="ExaminationLIst.aspx.cs" Inherits="Exam_ExaminationLIst" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
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
                alert("Select any record");
                return false;
            }
        }
        function CnfDelete() {

            if (confirm("You are going to delete a record. Do you want to continue ?")) {

                return true;
            }
            else {
                return false;
            }
        }

        function CnfImport() {

            if (confirm("Are you sure to Import Exam Details from the Previous Session ?")) {

                return true;
            }
            else {
                return false;
            }
        }
        function validateform() {
            var Session = document.getElementById("<%=drpSession.ClientID %>").value;


            if (Session == "0") {
                alert("Select a Session");
                document.getElementById("<%=drpStatus.ClientID %>").selectedIndex = 0;
                document.getElementById("<%=drpClass.ClientID %>").selectedIndex = 0;
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            else {
                return true;

            }
        }
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=480,height=200,left = 490,top = 184');");
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 3px;">
        <h2>
            Define Exams</h2>
    </div>
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
                                    <td align="left" colspan="4" style="padding: 5px; font-weight: bold; height: 3px;
                                        font-family: Tahoma, Geneva, sans-serif; color: #000; border: 1px solid #333;
                                        background-color: Transparent;">
                                        Session :
                                        <asp:DropDownList ID="drpSession" runat="server" Width="100px" BackColor="White"
                                            AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        &nbsp;Status :
                                        <asp:DropDownList ID="drpStatus" runat="server" Width="100px" BackColor="White" OnSelectedIndexChanged="drpStatus_SelectedIndexChanged"
                                            AutoPostBack="True">
                                            <asp:ListItem Value="- ALL -">- ALL -</asp:ListItem>
                                            <asp:ListItem Value="1">Active</asp:ListItem>
                                            <asp:ListItem Value="0">InActive</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;Class :
                                        <asp:DropDownList ID="drpClass" runat="server" Width="100px" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        &nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                            OnClientClick="return validateform();" onfocus="active(this);" 
                                            onblur="inactive(this);" style="height: 26px" />&nbsp;<asp:Button
                                                ID="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" onfocus="active(this);"
                                                onblur="inactive(this);" />
                                        <asp:Button ID="btnExpXam" runat="server" Text="Import Data From Prev. Session" OnClick="btnExpXam_Click"
                                            OnClientClick="return CnfImport();" onfocus="active(this);" onblur="inactive(this);" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <fieldset id="fldexamination" runat="server">
                                <table cellspacing="0" cellpadding="0" width="100%">
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:Button ID="btnDelete" runat="server" Text="Delete Exam Details" OnClick="btnDelete_Click"
                                                OnClientClick="return CnfDelete();" Width="150px" onfocus="active(this);" onblur="inactive(this);" />&nbsp;<asp:Button
                                                    ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click"
                                                    Width="150px" onfocus="active(this);" onblur="inactive(this);" />
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblRecCount" runat="server" Text="" CssClass="totalrec"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" valign="top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:GridView ID="gvExam" runat="server" AutoGenerateColumns="False" DataKeyNames="ExamId"
                                                Width="100%">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                        <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                        <ItemTemplate>
                                                            <input name="Checkb" type="checkbox" value='<%#Eval("ExamId")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%#Eval("ExamId")%>'
                                                                OnClick="lbtnEdit_Click">Edit</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Session Year">
                                                        <ItemTemplate>
                                                            <%#Eval("SessionYr")%>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="70px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Examination Name">
                                                        <ItemTemplate>
                                                            <%#Eval("ExamName")%>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Applicable for Class">
                                                        <ItemTemplate>
                                                            <a href="javascript:popUp('ExamClass.aspx?ExamId=<%#Eval("ExamId")%>&ExamNm=<%#Eval("ExamName")%>')">
                                                                View</a>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="70px" />
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Exam Type">
                                                        <ItemTemplate>
                                                            <%#Eval("ExamType")%>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="70px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start Date">
                                                        <ItemTemplate>
                                                            <%#Eval("FromDate")%>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="End Date">
                                                        <ItemTemplate>
                                                            <%#Eval("ToDate")%>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pass All Subject Required">
                                                        <ItemTemplate>
                                                            <%#Eval("DisPassAllSubRequired")%>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pass Percentage">
                                                        <ItemTemplate>
                                                            <%#Eval("PassPercent")%>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <%#Eval("DisStatus")%>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    No Records
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
