<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="TestRepeater.aspx.cs" Inherits="Masters_TestRepeater" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <hgroup class="title">
        <h1>Test</h1>
        <h2> Your contact page.</h2>
    </hgroup>

    <section>
        <div>
            <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <ul>
                            <li>
                                <asp:Label ID="lblId" CssClass="primary-key" runat="server" Text='<%# Eval("NoticeId") %>'></asp:Label>
                            </li>
                            <li>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("NoticeDate") %>'></asp:Label>
                            </li>
                            <li>
                                <asp:Label ID="lblPhone" runat="server" Text='<%# Eval("NoticeShortDesc") %>'></asp:Label>
                            </li>
                            <li>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("NoticeDetails") %>'></asp:Label>
                            </li>
                            
                                </ul>
                            </li>
                        </ul>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        </div>
       
    </section>


</asp:Content>