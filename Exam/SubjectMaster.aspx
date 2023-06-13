<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="SubjectMaster.aspx.cs" Inherits="Exam_SubjectMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function CnfDelete() {

            if (confirm("You are going to delete selected Record(s). Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }
        function ValidateCheckBoxList(sender, args) {
            var checkBoxList = document.getElementById("<%=chkListClass.ClientID %>");
            var checkboxes = checkBoxList.getElementsByTagName("input");
            var isValid = false;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isValid = true;
                    break;
                }
            }
            args.IsValid = isValid;
        }
        function CnfDelete() {

            if (confirm("Are you sure to Delete this Subject ?")) {

                return true;
            }
            else {

                return false;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Subject Master</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <fieldset style="width: 90%">
                <legend><b>Add New Subject</b></legend>
                <table width="100%">
                    <tr id="trMsg" runat="server">
                        <td style="height: 20px;" align="center" colspan="2">
                            <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline" width="50px">
                            Class<font color="red">*</font>
                        </td>
                        <td align="left" valign="baseline">
                            &nbsp;:<asp:CheckBoxList ID="chkListClass" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="chkListClass_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                     <tr>
                        <td align="left" valign="baseline" width="50px">
                            Stream<font color="red">*</font>
                        </td>
                        <td align="left" valign="baseline">
                            &nbsp;:<asp:CheckBoxList ID="ChkListStream" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" TabIndex="2" AutoPostBack="True" Visible="false">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Subject<font color="red">*</font>
                        </td>
                        <td align="left" valign="baseline">
                            &nbsp;:&nbsp;<asp:TextBox ID="txtSubject" runat="server" MaxLength="50" TabIndex="2"></asp:TextBox>
                            <asp:CheckBox ID="chkOpt" runat="server" Text="Optional" />
                            <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="3" onfocus="active(this);"
                                onblur="inactive(this);" OnClick="btnSave_Click" /><asp:CustomValidator ID="CustomValidator1"
                                    ErrorMessage="Select at least One Class" ForeColor="Red" ClientValidationFunction="ValidateCheckBoxList"
                                    runat="server" />
                            <asp:HiddenField ID="hfSubjectId" runat="server" Value="0" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <br />
            <fieldset style="width: 90%">
                <legend><b>Subject List</b></legend>
                <table width="100%">
                    <tr>
                        <td align="left" valign="baseline">
                           Select a Class <font color="red">*</font>:&nbsp;<asp:DropDownList ID="drpSelectClass" 
                                runat="server" TabIndex="4" AutoPostBack="True" OnSelectedIndexChanged="drpSelectClass_SelectedIndexChanged">
                                </asp:DropDownList>
                                 <asp:DropDownList ID="DrpSelectStream"
                                runat="server" TabIndex="5" AutoPostBack="True" 
                                onselectedindexchanged="DrpSelectStream_SelectedIndexChanged" Visible="False">
                                 
                            </asp:DropDownList>
                        </td>
                       
                        <td align="right">
                            <asp:Label ID="lblRecords" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:GridView ID="grdSubject" runat="server" AutoGenerateColumns="False" 
                                Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%#Eval("SubjectId")%>'
                                                AlternateText="Edit" ImageUrl="~/images/icon_edit.gif" ToolTip="Click to Edit"
                                                OnClick="btnEdit_Click" CausesValidation="false" />
                                            <asp:HiddenField ID="hfOpt" runat="server" Value='<%#Eval("IsOptional")%>' />
                                            <asp:HiddenField ID="hfClassId" runat="server" Value='<%#Eval("ClassId")%>' />
                                            <asp:HiddenField ID="hfStreamId" runat="server" Value='<%#Eval("StreamID")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%#Eval("SubjectId")%>'
                                                AlternateText="Delete" ImageUrl="~/images/icon_delete.gif" ToolTip="Click to Delete"
                                                OnClick="btnDelete_Click" CausesValidation="false" OnClientClick="return CnfDelete();" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Class">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClassName" runat="server" Text='<%#Eval("ClassName")%>'></asp:Label>
                                            <asp:HiddenField ID="hfClassId" runat="server" Value='<%#Eval("ClassId")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Subject Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubjectName" runat="server" Text='<%#Eval("SubjectName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Optional/Compulsory">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubType" runat="server" Text='<%#Eval("SubType")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record(s)
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

