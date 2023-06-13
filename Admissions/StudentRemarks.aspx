<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="StudentRemarks.aspx.cs" Inherits="Admissions_StudentRemarks" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <script language="javascript" type="text/javascript">

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



                function TABLE1_onclick() {

                }

            </script>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Student Remarks</h2>
            </div>
            <table style="vertical-align: top; float: left;" width="100%">
                <tbody>
                    <tr>
                        <td valign="top" align="center">
                            <asp:Label ID="lblerr" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table id="TABLE1" onclick="return TABLE1_onclick()" width="100%">
                                <tbody>
                                    <tr>
                                        <td align="left">
                                            <table class="cnt-box" style="width: 100%;">
                                                <tr>
                                                    <td class="tbltxt">
                                                        <asp:Label ID="lblSession" runat="server" Text="Session :"></asp:Label>
                                                        <asp:TextBox ID="txtsession" runat="server" ReadOnly="True"   CssClass="tbltxtbox"
                                                            TabIndex="1"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Label ID="lblClass" runat="server" Text="Class :"></asp:Label>
                                                        <asp:DropDownList ID="drpClass" runat="server"   OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                                            AutoPostBack="True" CssClass="vsmalltb" TabIndex="2">
                                                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Label ID="lblsection" runat="server" Text="Section :"></asp:Label>
                                                        <asp:DropDownList ID="drpSection" runat="server"  OnSelectedIndexChanged="drpSection_SelectedIndexChanged"
                                                            AutoPostBack="True" CssClass="vsmalltb" TabIndex="3">
                                                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        &nbsp;&nbsp;<asp:Label ID="lblSelectStudent" runat="server" Text="Student :"></asp:Label>
                                                        <asp:DropDownList ID="drpSelectStudent" runat="server"  OnSelectedIndexChanged="drpSelectStudent_SelectedIndexChanged"
                                                            AutoPostBack="True" Height="17px" CssClass="vsmalltb" TabIndex="4">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tbltxt">
                                                        <asp:Label ID="lblAdminno" runat="server"></asp:Label><asp:Label ID="lblRemarkDate"
                                                            runat="server" Text="Remark's Date :"></asp:Label>
                                                        <asp:TextBox ID="txtremarksdate" runat="server"  OnTextChanged="txtremarksdate_TextChanged"
                                                            CssClass="tbltxtbox" TabIndex="5"></asp:TextBox>
                                                        <rjs:PopCalendar ID="PopCalendar2" runat="server" Format="dd mm yyyy" Control="txtremarksdate">
                                                        </rjs:PopCalendar>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                                            ControlToValidate="txtremarksdate"></asp:RequiredFieldValidator>
                                                        <asp:Label ID="lblTeacherName" runat="server" Text="Teacher's Name :"></asp:Label>
                                                        <asp:TextBox ID="txtteachers" runat="server" Width="95px" CssClass="tbltxtbox" TabIndex="6"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                                            ControlToValidate="txtteachers"></asp:RequiredFieldValidator>
                                                    <%--<input ID="Text1" onchange="" onkeypress= type="text" />--%>
                                                    </td>
                                                </tr>
                                        </td>
                                    </tr>
                            </table>
                            <tr>
                                <td>
                                    <div class="spaer"></div><div class="spacer"></div>
                                    <div style="float: left; width:100%">
                                        <table id="table9" cellspacing="0" cellpadding="4" border="0" class="cnt-box2 tbltxt" width="100%">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <table id="table10" cellspacing="0" cellpadding="4" align="left" border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td align="left" colspan="2" class="tbltxt">
                                                                        Study&nbsp;Remarks :
                                                                    </td>
                                                                    <td align="left" valign="middle">
                                                                        <asp:TextBox ID="txtStudyremarks" runat="server" Width="460px" Height="40px" TextMode="MultiLine"
                                                                            CssClass="tbltxtbox" TabIndex="7"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="2" valign="top" class="tbltxt">
                                                                        Sports Remarks :
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtSportremarks" runat="server" Width="460px" Height="40px" TextMode="MultiLine"
                                                                            CssClass="tbltxtbox" TabIndex="8"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="2" class="tbltxt">
                                                                        Cultural Remarks :
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtCulturalremarks" runat="server" Width="460px" Height="40px" TextMode="MultiLine"
                                                                            CssClass="tbltxtbox" TabIndex="9"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="2" class="tbltxt">
                                                                        Special Reference :
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtSpecialReference" runat="server" Width="460px" Height="40px"
                                                                            TextMode="MultiLine" CssClass="tbltxtbox" TabIndex="10"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="2" class="tbltxt">
                                                                        Annual Result&nbsp;:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtAnnualResult" runat="server" Width="460px" Height="40px" TextMode="MultiLine"
                                                                            CssClass="tbltxtbox" TabIndex="11"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center">
                                    <asp:Button ID="btnSaveAddNew" OnClick="btnSaveAddNew_Click" runat="server" Text="Submit"
                                        CausesValidation="False" TabIndex="12"></asp:Button>
                                    <asp:Button ID="btnDelete" OnClick="btnDelete_Click" runat="server" Text="Delete"
                                        CausesValidation="False" TabIndex="13"></asp:Button>
                                    <asp:Button ID="btncancel" OnClick="btncancel_Click" runat="server" Text="Cancel"
                                        CausesValidation="False" TabIndex="14"></asp:Button>
                                    <asp:HiddenField ID="hdnsts" runat="server"></asp:HiddenField>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="grdadmnno" runat="server" Width="100%" OnRowCommand="grdadmnno_RowCommand"
                                        OnPageIndexChanging="grdadmnno_PageIndexChanging" AutoGenerateColumns="False"
                                        PageSize="5" AllowPaging="true" CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr"
                                        AlternatingRowStyle-CssClass="alt" TabIndex="15">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input name="Checkb" type="checkbox" value='<%#Eval("RemarksID")%>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="15px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkAdmnNo" runat="server" CausesValidation="false" CommandArgument='<%#Eval("RemarksID")%>'
                                                        Text='<%#Eval("FullName")%>'>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarksDate" runat="server" Text='<%#Eval("RemarksDate")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Teacher">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTeacher" runat="server" Text='<%#Eval("Teacher")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Study Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStudyRemarks" runat="server" Text='<%#Eval("StudyRemarks")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sports Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSportsRemarks" runat="server" Text='<%#Eval("SportsRemarks")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cultural Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCulturalRemarks" runat="server" Text='<%#Eval("CulturalRemarks")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Special Reference">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSpecialReference" runat="server" Text='<%#Eval("SpecialReference")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Annual Result">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAnnualResult" runat="server" Text='<%#Eval("AnnualResult")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                </tbody>
            </table>
            <p>
                &nbsp;</p>
            <p>
                &nbsp;</p>
            <p>
                &nbsp;</p>
            </td> </tr> </tbody> </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
