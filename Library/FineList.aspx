<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="FineList.aspx.cs" Inherits="Library_FineList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        function isValid() {
            var Fine = document.getElementById("<%=drpFineType.ClientID %>").value;

            if (Fine == "0") {
                alert("Please select Fine Type !");
                document.getElementById("<%=drpFineType.ClientID %>").focus();
                return false;
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

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fine List</h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <div style="width: 100%; background-color: #666; padding: 1px; margin: 0 auto;">
        <div style="background-color: #FFF; padding: 10px;">
            <table border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                <tr>
                    <td>
                        Fine Type :
                        <asp:DropDownList ID="drpFineType" runat="server" Width="150px" CssClass="smalltb">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return isValid(); "
                            OnClick="btnSearch_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="background-color: #666; padding: 1px; margin: 0 auto; margin-top: 15px;">
        <div style="background-color: #FFF; padding: 10px;">
            <table border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                <tr>
                    <td align="left">
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />&nbsp;
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblRecords" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Panel ID="pnlFixed" runat="server">
                            <asp:GridView ID="grdFixedFine" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <input type="checkbox" name="Checkb" value='<%# Eval("FineId") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderTemplate>
                                            <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <a href='FineMaster.aspx?Id=<%#Eval("FineId")%>'>Edit</a>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective Date">
                                        <ItemTemplate>
                                            <%#Eval("FromDtStr")%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fine Type">
                                        <ItemTemplate>
                                            <%#Eval("FType")%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <%#Eval("FixedAmtStr")%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="130px" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record
                                </EmptyDataTemplate>
                                <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                                <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Panel ID="pnlVar" runat="server">
                            <asp:GridView ID="grdVarFine" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <input type="checkbox" name="Checkb" value='<%# Eval("FineId") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderTemplate>
                                            <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <a href='FineMaster.aspx?Id=<%#Eval("FineId")%>'>Edit</a>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective Date">
                                        <ItemTemplate>
                                            <%#Eval("FromDtStr")%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="120px" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fine Type">
                                        <ItemTemplate>
                                            <%#Eval("FType")%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Daily Rate">
                                        <ItemTemplate>
                                            <%#Eval("DailyRateStr")%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="130px" HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Weekly Rate">
                                        <ItemTemplate>
                                            <%#Eval("WeeklyRateStr")%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="130px" HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fortnightly Rate">
                                        <ItemTemplate>
                                            <%#Eval("FortnightlyRateStr")%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="130px" HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Monthly Rate">
                                        <ItemTemplate>
                                            <%#Eval("MonthlyRateStr")%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="130px" HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Yearly Rate">
                                        <ItemTemplate>
                                            <%#Eval("YearlyRateStr")%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="130px" HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record
                                </EmptyDataTemplate>
                                <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                                <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center" class="tbltxt">
                        <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
