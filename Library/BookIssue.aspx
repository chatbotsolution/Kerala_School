<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="BookIssue.aspx.cs" Inherits="Library_BookIssue" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var IssueDt = document.getElementById("<%=txtIssueDt.ClientID %>").value;
            var DueDt = document.getElementById("<%=txtDueDt.ClientID %>").value;
            var memId = document.getElementById("<%=ddlMemberId.ClientID %>").selectedIndex;
            var AccNo = document.getElementById("<%=txtAccNo.ClientID %>").value;
            if (memId == 0) {
                alert("Please Select Member");
                document.getElementById("<%=ddlMemberId.ClientID %>").focus();
                return false;
            }
            if (IssueDt.trim() == "") {
                alert("Please Select Issue Date !");
                document.getElementById("<%=txtIssueDt.ClientID %>").focus();
                return false;
            }
            if (DueDt.trim() == "") {
                alert("Please Select Due Date !");
                document.getElementById("<%=txtDueDt.ClientID %>").focus();
                return false;
            }
            if (Date.parse(IssueDt.trim()) > Date.parse(DueDt.trim())) {
                alert("Due date must be greater than Issue date");
                document.getElementById("<%=txtDueDt.ClientID %>").focus();
                return false;
            }

            if (AccNo.trim() == "") {
                alert("Please Enter Accession No !");
                document.getElementById("<%=txtAccNo.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Are you ready to Issue ?');
            }
        }
        function ValidBook() {
            var AccNo = document.getElementById("<%=txtAccNo.ClientID %>").value;

            if (AccNo.trim() == "") {
                alert("Please Enter Accession No !");
                document.getElementById("<%=txtAccNo.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        } 
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Issue Books</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="40" width="10" /></div>
            <div style="width: 400px; background-color: #666; padding: 1px; margin: 0 auto;">
                <div style="background-color: #FFF; padding: 10px;">
                    <table cellpadding="2px" cellspacing="2px" width="100%" class="tbltxt">
                        <tr>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rdbtnlstUsertype" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                    OnSelectedIndexChanged="rdbtnlstUsertype_SelectedIndexChanged" CssClass="tbltxt">
                                    <asp:ListItem Text="Staff" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Student" Value="1" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Class :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="true"
                                    CssClass="largetb" onselectedindexchanged="ddlClass_SelectedIndexChanged">
                                    <asp:ListItem Text="---All---" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                Section :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSection" runat="server" AutoPostBack="true"
                                    CssClass="largetb" onselectedindexchanged="ddlSection_SelectedIndexChanged">
                                    <asp:ListItem Text="---All---" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Member :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMemberId" runat="server" AutoPostBack="true" CssClass="largetb"
                                    OnSelectedIndexChanged="ddlMemberId_SelectedIndexChanged">
                                </asp:DropDownList>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Issue Date:
                            </td>
                            <td>
                                <asp:TextBox ID="txtIssueDt" runat="server" MaxLength="30" CssClass="smalltb" ReadOnly="true"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpIssueDt" runat="server" Control="txtIssueDt" AutoPostBack="True"
                                    Format="dd mmm yyyy" OnSelectionChanged="dtpIssueDt_SelectionChanged"></rjs:PopCalendar>
                                <asp:LinkButton ID="lnkcldate" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtIssueDt.value='';return false;"
                                    Text="Clear" ></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Due Date:
                            </td>
                            <td>
                                <asp:TextBox ID="txtDueDt" runat="server" MaxLength="30" CssClass="smalltb" ReadOnly="true"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpDueDt" runat="server" Control="txtDueDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtDueDt.value='';return false;"
                                    Text="Clear" ></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Accession No:
                            </td>
                            <td>
                                <asp:TextBox ID="txtAccNo" runat="server" CssClass="smalltb"></asp:TextBox>
                            </td>
                        </tr>
                        <asp:Panel ID="pnl3" runat="server" Visible="false" CssClass="tbltxt">
                            <tr>
                                <td>
                                    Category :
                                </td>
                                <td>
                                    <asp:Label ID="lblCategory" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Subject :
                                </td>
                                <td>
                                    <asp:Label ID="lblSubject" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Book Name :
                                </td>
                                <td>
                                    <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Panel ID="pnl1" runat="server" Visible="false">
                                    <asp:Button ID="btnSave" runat="server" Text="Issue" Font-Bold="True" OnClientClick="return isValid(); "
                                        Font-Size="8pt" Width="60px" OnClick="btnSave_Click" />&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" Font-Bold="True" Font-Size="8pt"
                                        Width="60px" OnClick="btnCancel_Click" />&nbsp;
                                    <asp:Button ID="btnShow" runat="server" Text="Show List" Font-Bold="True" Font-Size="8pt"
                                        Width="70px" OnClick="btnShow_Click" />
                                </asp:Panel>
                                <asp:Panel ID="pnl2" runat="server">
                                    <asp:Button ID="btnBookDtls" runat="server" Text="Get Book Details" OnClick="btnBookDtls_Click"
                                        OnClientClick="return ValidBook();" />
                                    <asp:Button ID="btnSearch" runat="server" Text="Search Book" Width="100px" OnClick="btnSearch_Click" />
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblmsg" runat="server" Text="" ForeColor=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="dvIssue" runat="server" visible="false" style="width: 100%; background-color: #666;
                padding: 1px; margin: 0 auto; margin-top: 15px;">
                <div style="background-color: #FFF; padding: 10px;">
                    <asp:GridView ID="grdIssueList" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        PageSize="15" Width="100%" OnPageIndexChanging="grdIssueList_PageIndexChanging"
                        CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="Member Id">
                                <ItemTemplate>
                                    <%#Eval("MemberId")%>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Emp/Admn No">
                                <ItemTemplate>
                                    <%#Eval("EmpNo")%>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Member Name">
                                <ItemTemplate>
                                    <%#Eval("MemberName")%>
                                </ItemTemplate>
                                <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Accession No">
                                <ItemTemplate>
                                    <%#Eval("AccessionNo")%>
                                    <asp:HiddenField ID="hdnAccessionNo" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Book Title">
                                <ItemTemplate>
                                    <%#Eval("BookTitle")%>
                                </ItemTemplate>
                                <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issue Date">
                                <ItemTemplate>
                                    <%#Eval("IssueDate")%>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Due Date">
                                <ItemTemplate>
                                    <%#Eval("DueDate")%>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Record
                        </EmptyDataTemplate>
                        <FooterStyle BackColor="#5e5e5e" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                        <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
