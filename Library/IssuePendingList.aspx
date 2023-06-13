<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true"
    CodeFile="IssuePendingList.aspx.cs" Inherits="Library_IssuePendingList" %>
   
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="Javascript" type="text/javascript">

        function isValid() {
            var name = document.getElementById("<%=ddlMemberId.ClientID %>").selectedIndex;
            var accno = document.getElementById("<%=txtAccno.ClientID %>").value;
            var FromDt = document.getElementById("<%=txtFrmDt.ClientID %>").value;
            var ToDt = document.getElementById("<%=txtToDt.ClientID %>").value;

            if (name == 0 && accno == "") {
                alert("Please Select Member or Enter Accession No. !");
                document.getElementById("<%=ddlMemberId.ClientID %>").focus();
                return false;
            }
            if (FromDt != 0) {
                //alert("Please Select From Date !");
                //document.getElementById("<%=txtFrmDt.ClientID %>").focus();
                //return false;
                if (ToDt == 0) {
                    alert("Please Select To Date !");
                    document.getElementById("<%=txtToDt.ClientID %>").focus();
                    return false;
                }
                else {
                    if (new Date(ToDt) < new Date(FromDt)) {
                        alert("To date must be greater than From date");
                        document.getElementById("<%=txtToDt.ClientID %>").focus();
                        return false;
                    }
                    else
                        return true;
                }
            }
            else {
                return true;
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
    <asp:UpdatePanel ID="uppSubList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Issue Pending List</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <div style="width: 100%; background-color: #666; padding: 1px; margin: 0 auto;">
                <div style="background-color: #FFF; padding: 10px;">
                    <table border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rdbtnlstUsertype" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                    class="tbltxt" OnSelectedIndexChanged="rdbtnlstUsertype_SelectedIndexChanged">
                                    <asp:ListItem Text="Staff" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Student" Value="1" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Class :
                                <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="true" CssClass="largetb"
                                    OnSelectedIndexChanged="ddlClass_SelectedIndexChanged">
                                    <asp:ListItem Text="---All---" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;&nbsp; Section :
                                <asp:DropDownList ID="ddlSection" runat="server" AutoPostBack="true" CssClass="largetb"
                                    OnSelectedIndexChanged="ddlSection_SelectedIndexChanged">
                                    <asp:ListItem Text="---All---" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;&nbsp; Member :
                                <asp:DropDownList ID="ddlMemberId" runat="server" AutoPostBack="true" CssClass="largetb">
                                </asp:DropDownList>
                                &nbsp;&nbsp; Accession No :
                                <asp:TextBox ID="txtAccno" runat="server" MaxLength="50" CssClass="largetb" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Issue From :
                                <asp:TextBox ID="txtFrmDt" runat="server" MaxLength="100" CssClass="smalltb"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpFrmDt" runat="server" Control="txtFrmDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFrmDt.value='';return false;"
                                    Text="Clear"></asp:LinkButton>
                                &nbsp;&nbsp; Issue To :
                                <asp:TextBox ID="txtToDt" runat="server" MaxLength="100" CssClass="smalltb"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                                <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtToDt.value='';return false;"
                                    Text="Clear"></asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="dvIrlist" runat="server" visible="false" style="width: 100%; background-color: #666;
                padding: 1px; margin: 0 auto; margin-top: 15px;">
                <div style="background-color: #FFF; padding: 10px;">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblRecords" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="grdIssueList" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                    PageSize="15" OnPageIndexChanging="grdIssueList_PageIndexChanging" CssClass="mGrid"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Member Id" Visible="false">
                                            <ItemTemplate>
                                                <%#Eval("EmpNo")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Admn no">
                                            <ItemTemplate>
                                                <%#Eval("OldAdmnno")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Class">
                                            <ItemTemplate>
                                                <%#Eval("ClassName")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Section">
                                            <ItemTemplate>
                                                <%#Eval("Section")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" HorizontalAlign="Left" />
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
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                           <a href='BookReturn.aspx?IRId=<%#Eval("IRId")%>&Name=<%#Eval("MemberName")%>&Accno=<%#Eval("AccessionNo") %>&MemId=<%#Eval("MemberId")%>&BookTitle=<%#Eval("BookTitle") %>&issuedt=<%#Eval("issuedt") %>&duedt=<%#Eval("duedt") %>'>
                                                    Return </a>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Issues Pending
                                    </EmptyDataTemplate>
                                    <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                                    <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
             
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
