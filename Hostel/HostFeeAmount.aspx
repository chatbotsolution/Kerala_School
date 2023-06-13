<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="HostFeeAmount.aspx.cs" Inherits="Hostel_HostFeeAmount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function isValid() {
            var Str = document.getElementById("<%=drpClass.ClientID %>").value;

            if (Str == 0) {
                alert("Please Select a Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }

        }

    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fee Amount</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="height: 340px; vertical-align: top;" width="100%">
                <tbody>
                    <tr>
                        <td valign='top' align="left">
                            <table width="100%">
                                <tr>
                                    <td class="tbltxt">
                                        &nbsp; Session Year:
                                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="tbltxtbox"
                                            Width="100px" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        Class:
                                        <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" CssClass="tbltxtbox"
                                            Width="120px">
                                        </asp:DropDownList>
                                        &nbsp;<asp:Button ID="btnShow" OnClientClick="return isValid();" runat="server" Text="Show"
                                            OnClick="btnShow_Click" />&nbsp;<asp:Button ID="btnSaveAddNew2" runat="server" OnClick="btnSaveAddNew_Click"
                                                Text="Set Fee Amount" TabIndex="3" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <asp:Panel ID="grid" runat="server">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grdFeeAmount" runat="server" AlternatingRowStyle-CssClass="alt"
                                                ShowFooter="true" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                Width="100%">
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFeeID" runat="server" Text='<%#Eval("FeeID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClassID" runat="server" Text='<%#Eval("ClassId")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Class">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClass" runat="server" Text='<%#Eval("ClassName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fee Components">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFeeName" runat="server" Text='<%#Eval("FeeName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="400px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Existing Students">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtExistingAmount" runat="server" Text='<%#Eval("ExistAmnt")%>'
                                                                Width="95px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotTc" runat="server" Font-Bold="true" Text='<%#GetTot("ExistAmnt")%>'></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="New Students">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtNewAmount" runat="server" Text='<%#Eval("NewAmnt")%>' Width="95px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotTc" Font-Bold="true" runat="server" Text='<%#GetTot("NewAmnt")%>'></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="TC Student">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtTCAmount" runat="server" Text='<%#Eval("TCAmnt")%>' Width="95px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotTc" Font-Bold="true" runat="server" Text='<%#GetTot("TCAmnt")%>'></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Casual Student">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtCasualAmount" runat="server" Text='<%#Eval("CasualAmnt")%>' Width="95px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotTc" Font-Bold="true" runat="server" Text='<%#GetTot("CasualAmnt")%>'></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>--%>
                                                </Columns>
                                                <FooterStyle CssClass="logintxt" ForeColor="Black" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblTotFee" Font-Bold="true" runat="server" Text=''></asp:Label>&nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnSaveAddNew" Width="125px" runat="server" OnClick="btnSaveAddNew_Click"
                                                Text="Set Fee Amount" />
                                            <asp:HiddenField ID="hdnsts" runat="server"></asp:HiddenField>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSaveAddNew" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
