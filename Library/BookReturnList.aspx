<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="BookReturnList.aspx.cs" Inherits="Library_BookReturnList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="Javascript" type="text/javascript">

        function isValid() {
            var FromDt = document.getElementById("<%=txtFrmDt.ClientID %>").value;
            var ToDt = document.getElementById("<%=txtToDt.ClientID %>").value;

            if (FromDt != 0) {
                //                alert("Please Select From Date !");
                //                document.getElementById("<%=txtFrmDt.ClientID %>").focus();
                //return false;
                if (ToDt == 0) {
                    alert("Please Select To Date !");
                    document.getElementById("<%=txtToDt.ClientID %>").focus();
                    return false;
                }
                else {
                    if (ToDt < FromDt) {
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
    <%--<div class="bedcromb">
        <asp:Label ID="lblTitle" runat="server" Text="Book Return List"></asp:Label></div>--%>
    <asp:UpdatePanel ID="uppSubList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="padding: 30px;" align="center">
                <div style="width: 875px; background-color: #666; padding: 2px; margin: 0 auto;">
                    <div style="background-color: #FFF; padding: 10px;">
                        <table width="850px" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="120" class="tabletxt">
                                    Return From :
                                </td>
                                <td width="150">
                                    <asp:TextBox ID="txtFrmDt" runat="server" Width="100px" MaxLength="100"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpFrmDt" runat="server" Control="txtFrmDt" AutoPostBack="False"
                                        Format="dd mmm yyyy"></rjs:PopCalendar>
                                </td>
                                <td width="100" class="tabletxt">
                                    Return To :
                                </td>
                                <td width="150">
                                    <asp:TextBox ID="txtToDt" runat="server" Width="100px" MaxLength="100"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                                        Format="dd mmm yyyy"></rjs:PopCalendar>
                                </td>
                                <td width="100" class="tabletxt">
                                    Member Id :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMemberId" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" 
                                        OnClientClick="return isValid()" onclick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="width: 875px; background-color: #666; padding: 2px; margin: 0 auto; margin-top: 15px;">
                    <div style="background-color: #FFF; padding: 10px;">
                        <table width="850" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="left">
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" onclick="btnDelete_Click" />&nbsp;
                                    <asp:Button ID="btnAdd" runat="server" Text="Add New" onclick="btnAdd_Click" />
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblRecords" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="grdReturnList" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                        PageSize="15" Width="850" onpageindexchanging="grdReturnList_PageIndexChanging" >
                                        <EmptyDataRowStyle HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <input type="checkbox" name="Checkb" value='<%#Eval("IRId")%>' />
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Left" Width="20px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderTemplate>
                                                    <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <a href='BookIssue.aspx?IRId=<%#Eval("IRId")%>'>Edit </a>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Left" Width="20px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Member Id">
                                                <ItemTemplate>
                                                    <%#Eval("MemberId")%>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Left" Width="80px" />
                                                <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Member Name">
                                                <ItemTemplate>
                                                    <%#Eval("MemberName")%>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Left" Width="80px" />
                                                <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Accession No">
                                                <ItemTemplate>
                                                    <%#Eval("AccessionNo")%>
                                                    <asp:HiddenField ID="hdnAccessionNo" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                                <FooterStyle Width="150px" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Return Date">
                                                <ItemTemplate>
                                                    <%#Eval("ReturnDate")%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                                <FooterStyle Width="100px" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fine Amount">
                                                <ItemTemplate>
                                                    <%#Eval("FineAmt")%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                                <FooterStyle Width="100px" />
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
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hfUserid" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

