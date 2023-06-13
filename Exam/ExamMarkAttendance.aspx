<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="ExamMarkAttendance.aspx.cs" Inherits="Exam_ExamMarkAttendance" %>

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
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 3px;">
        <h2>
            Exam Attendance</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr id="trMsg" runat="server" style="padding: 2px;">
                    <td align="center">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        Session<span class="mandatory">*</span>&nbsp;:
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                            TabIndex="1" onchange="CheckLoader();">
                        </asp:DropDownList>
                        Class<span class="mandatory">*</span>&nbsp;:
                        <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                            TabIndex="2" onchange="CheckLoader();">
                        </asp:DropDownList>
                        Section<span class="mandatory">*</span>&nbsp;:
                        <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSection_SelectedIndexChanged"
                            TabIndex="3" onchange="CheckLoader();">
                        </asp:DropDownList>
                        Exam Name<span class="mandatory">*</span>&nbsp;:
                        <asp:DropDownList ID="drpExamName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpExamName_SelectedIndexChanged"
                            TabIndex="3" onchange="CheckLoader();">
                        </asp:DropDownList>
                        Subject<span class="mandatory">*</span>&nbsp;:
                        <asp:DropDownList ID="drpSubject" runat="server" TabIndex="4" AutoPostBack="True"
                            OnSelectedIndexChanged="drpSubject_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        <asp:Button ID="btnSubmit" runat="server" Text="Mark As Present" OnClick="btnSubmit_Click"
                            onfocus="active(this);" onblur="inactive(this);" Enabled="false" TabIndex="5"
                            OnClientClick="CheckLoader();" />
                        <asp:Button ID="btnAbsent" runat="server" Text="Mark As Absent" OnClick="btnAbsent_Click"
                            onfocus="active(this);" onblur="inactive(this);" Enabled="false" TabIndex="6"
                            OnClientClick="CheckLoader();" />
                        <asp:Button ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click"
                            onfocus="active(this);" onblur="inactive(this);" TabIndex="7" OnClientClick="CheckLoader();" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblNote" runat="server" Font-Size="12px" ForeColor="Red" Text="Note : Select the Records to Mark the Attendance"
                                        Visible="false"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblRecCount" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:GridView ID="gvExamAtten" runat="server" AutoGenerateColumns="False" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Status">
                                                <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                <HeaderTemplate>
                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                <ItemTemplate>
                                                    <input name="Checkb" type="checkbox" value='<%#Container.DataItemIndex%>' />
                                                    <asp:HiddenField ID="hfAttenID" runat="server" Value='<%#Eval("ExamMarksId") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Admn No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmn" runat="server" Text='<%#Eval("AdmnNo")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FullName" HeaderText="Name" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ExStatus" HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="drpClass" />
            <asp:PostBackTrigger ControlID="drpSession" />
            <asp:PostBackTrigger ControlID="drpSection" />
            <asp:PostBackTrigger ControlID="drpExamName" />
            <asp:PostBackTrigger ControlID="drpSubject" />
            <asp:PostBackTrigger ControlID="btnSubmit" />
            <asp:PostBackTrigger ControlID="btnAbsent" />
            <asp:PostBackTrigger ControlID="btnCancel" />
        </Triggers>
    </asp:UpdatePanel>
    <div id="Loader" style="text-align: center; vertical-align: middle; position: absolute;
        top: 0px; left: 0px; z-index: 99; width: 34%; height: 38%; background-color: #ededed;
        -ms-filter: 'progid:DXImageTransform.Microsoft.Alpha(Opacity=60)'; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=60);
        -moz-opacity: 0.8; opacity: 0.8;">
        <div style="width: 48px; height: 48px; margin: 0 auto; margin-top: 275px;">
            <img src="../images/spinner.gif">
        </div>
        <div style="font-family: Trebuchet MS; font-size: 12px; color: Green; text-align: center;">
            Please Wait ...</div>
    </div>
</asp:Content>
