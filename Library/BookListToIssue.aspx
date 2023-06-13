<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="BookListToIssue.aspx.cs" Inherits="Library_BookListToIssue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="uppSubList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Book List To Issue</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <div style="width: 100%; background-color: #666; padding: 1px; margin: 0 auto;">
                <div style="background-color: #FFF; padding: 10px;">
                    <table border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                        <tr>
                            <td>
                                Category :
                                <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" CssClass="smalltb"
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                               &nbsp;&nbsp;Subject :
                                <asp:DropDownList ID="ddlSubject" runat="server" CssClass="largetb">
                                    <asp:ListItem Text="---Select---" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;&nbsp; Publisher :
                                <asp:DropDownList ID="ddlPublisher" runat="server" CssClass="largetb" >
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="100">
                                Author :
                                <asp:TextBox ID="txtAuthor" runat="server" MaxLength="50" CssClass="largetb"></asp:TextBox>&nbsp;&nbsp;
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="width: 100%; background-color: #666; padding: 1px; margin: 0 auto; margin-top: 15px;">
                <div style="background-color: #FFF; padding: 10px;">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                        <tr>
                            <td align="left">
                            </td>
                            <td align="right">
                                <asp:Label ID="lblRecords" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="grdCatList" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                    PageSize="10" OnPageIndexChanging="grdCatList_PageIndexChanging" CssClass="mGrid"
                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <a href='BookIssue.aspx?select=true&AccNo=<%#Eval("AccessionNo")%>&catname=<%#Eval("CatName")%>&subject=<%#Eval("SubName") %>&BookTitle=<%#Eval("BookTitle") %>'>
                                                    Select</a>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acc No">
                                            <ItemTemplate>
                                                <%#Eval("AccessionNo")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Book Title">
                                            <ItemTemplate>
                                                <%#Eval("BookTitle")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category">
                                            <ItemTemplate>
                                                <%#Eval("CatName")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subject">
                                            <ItemTemplate>
                                                <%#Eval("SubName")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Author">
                                            <ItemTemplate>
                                                <%#Eval("AuthorName1")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Publisher">
                                            <ItemTemplate>
                                                <%#Eval("PublisherName")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
