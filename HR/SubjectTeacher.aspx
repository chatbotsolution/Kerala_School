<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="SubjectTeacher.aspx.cs" Inherits="HR_SubjectTeacher" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Assign Class/Subjects wise Teachers</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <fieldset style="width: 90%">
                <table width="100%">
                    <tr id="trMsg" runat="server">
                        <td style="height: 20px;" align="center">
                            <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Session<font color="red">*</font>&nbsp;:&nbsp;<asp:DropDownList ID="drpSession" runat="server"
                                TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                            </asp:DropDownList>
                            Select a Class<font color="red">*</font>&nbsp;:&nbsp;<asp:DropDownList ID="drpClass"
                                runat="server" TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                            </asp:DropDownList>
                            Select a Section<font color="red">*</font>&nbsp;:&nbsp;<asp:DropDownList ID="drpSection"
                                runat="server" TabIndex="3" AutoPostBack="True" OnSelectedIndexChanged="drpSection_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="btnSave" runat="server" Text="Save" Visible="False" TabIndex="4"
                                onfocus="active(this);" onblur="inactive(this);" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:GridView ID="grdAssign" runat="server" AutoGenerateColumns="False" Width="100%"
                                OnRowDataBound="grdAssign_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Subject">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("SubjectName")%>'></asp:Label>
                                            <asp:HiddenField ID="hfSubjectId" runat="server" Value='<%#Eval("SubjectId")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Teacher's Name">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpTeacher" runat="server" TabIndex="5">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hfTeacherId" runat="server" Value='<%#Eval("TeacherEmpId")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtEffDate" runat="server" Width="80px" TabIndex="5" Text='<%#Eval("strEffDt")%>'
                                                MaxLength='10'></asp:TextBox>&nbsp;(DD-MM-YYYY)
                                            <asp:HiddenField ID="hfEffDate" runat="server" Value='<%#Eval("EffectiveDt")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%" />
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

