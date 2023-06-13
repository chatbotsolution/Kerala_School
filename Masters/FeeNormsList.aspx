<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="FeeNormsList.aspx.cs" Inherits="Masters_FeeNormsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
        function CnfDelete() {

            if (confirm("You are going to finalise a record. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
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
   
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fee Norms</h2>
    </div>
    <br />
    <table width="100%">
        <tr>
            <td valign="top" align="left">
                <asp:Button ID="btnNew" runat="server" OnClick="btnNew_Click" Text="Add New" TabIndex="3" />
                <asp:Button ID="btndel" runat="server" OnClick="btndel_Click" Text="Delete" Visible="False"
                    TabIndex="2" />
            &nbsp;
                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left">
                <asp:GridView ID="grdFeeNorms" runat="server" Width="100%" AutoGenerateColumns="False"
                    AllowPaging="True" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    TabIndex="1" OnRowDataBound="grdFeeNorms_RowDataBound" DataKeyNames="SessionID">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <input type="checkbox" name="Checkb" value='<%# Eval("SessionID") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle Width="15px" HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Session">
                            <ItemTemplate>
                                <div id="dvEdit" runat="server">
                                    <a href='feenoms.aspx?sid=<%#Eval("SessionID")%>'>
                                        <asp:Label ID="lblSession" Font-Bold="true" runat="server" Text='<%# Eval("SessionYr") %>'></asp:Label>
                                    </a>
                                </div>
                                <div id="dvShow" runat="server">
                                    <asp:Label ID="Label1" Font-Bold="true" runat="server" Text='<%# Eval("SessionYr") %>'></asp:Label></div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="100px" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Date">
                            <ItemTemplate>
                                <asp:Label ID="lblStartDate" runat="server" Text='<%#Eval("StartDate")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date1">
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate1" runat="server" Text='<%#Eval("DueDate1")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date2">
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate2" runat="server" Text='<%#Eval("DueDate2")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date3">
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate3" runat="server" Text='<%#Eval("DueDate3")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date4">
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate4" runat="server" Text='<%#Eval("DueDate4")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date5">
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate5" runat="server" Text='<%#Eval("DueDate5")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date6">
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate6" runat="server" Text='<%#Eval("DueDate6")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date7">
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate7" runat="server" Text='<%#Eval("DueDate7")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date8">
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate8" runat="server" Text='<%#Eval("DueDate8")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date9">
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate9" runat="server" Text='<%#Eval("DueDate9")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date10">
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate10" runat="server" Text='<%#Eval("DueDate10")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date11">
                            <ItemTemplate>
                                <asp:Label ID="lblDueDate11" runat="server" Text='<%#Eval("DueDate11")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment Period" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPayPeriod" runat="server" Text='<%#Eval("FeeCollPeriod")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Finalized">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnFinalise" runat="server" Text='<%#Eval("Finalised")%>'
                                    OnClick="lnkbtnFinalise_Click" OnClientClick="return CnfDelete()"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fine Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblFineAmount" runat="server" Text='<%#Eval("FineAmount")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFEFEF" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#FDFDFD" />
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
