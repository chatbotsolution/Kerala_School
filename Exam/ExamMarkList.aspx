<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="ExamMarkList.aspx.cs" Inherits="Exam_ExamMarkList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=500,height=300,left = 500,top = 200');");
        }
        
    </script>

    <div style="width: 30px; height: 30px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 3px;">
        <h2>
            Exam Performance Remarks</h2>
    </div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="height: 100%;">
        <tr>
            <td align="center" valign="top">
                <div style="text-align: center;">
                    <asp:Label ID="lblMsg" Font-Bold="true" runat="server" Text=""></asp:Label>
                </div>
                <div align="left">
                    <div style="width: 100%">
                        <div>
                            <img src="../images/mask.gif" width="10" height="10" /></div>
                        <div>
                            <table width="100%">
                                <tr style="background-color: #D3E7EE;">
                                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                                        color: #000; border: 1px solid #333; background-color: Transparent;">
                                        Session :
                                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="true" TabIndex="1"
                                            OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        Class :
                                        <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" TabIndex="2" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        Section :
                                        <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSection_SelectedIndexChanged"
                                            TabIndex="3">
                                        </asp:DropDownList>
                                        Exam :
                                        <asp:DropDownList ID="drpExam" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpExam_SelectedIndexChanged"
                                            TabIndex="4">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblTotStud" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:GridView ID="grdMarks" runat="server" AutoGenerateColumns="False" Width="100%"
                                            AllowPaging="True" BorderStyle="Solid" BorderWidth="0.5px" OnPageIndexChanging="grdMarks_PageIndexChanging"
                                            PageSize="20" OnRowDataBound="grdMarks_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Student Name">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%#Eval("FullName")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Admission No">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%#Eval("AdmnNo")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Marks">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%#Eval("MaxMarks")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Marks Secured">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%#Eval("MarksObtained")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Percentage">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%#Eval("PerObtained")%> %
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Set Remarks">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" Visible="false" runat="server" Text='<%#Eval("Remarks")%>'></asp:Label>
                                                        <a href="javascript:popUp('ExamRemarks.aspx?ExamId=<%#Eval("ExamId")%>&AdmnNo=<%#Eval("AdmnNo")%>&SName=<%#Eval("FullName")%>&Marks=<%#Eval("MarksObtained")%>')">
                                                            <asp:Label ID="lblView" runat="server" Text="Set Remarks" Font-Underline="true"></asp:Label>
                                                        </a>
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
</asp:Content>
