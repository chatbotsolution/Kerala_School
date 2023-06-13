<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="NewsMagazineList.aspx.cs" Inherits="Library_NewsMagazineList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">

        function isValid() {
            var SubscribeFrom = document.getElementById("<%=txtFrmDt.ClientID %>").value;
            var SubscribeTo = document.getElementById("<%=txtToDt.ClientID %>").value;
            if ((SubscribeFrom.trim() != "" && SubscribeTo == "") || (SubscribeFrom.trim() == "" && SubscribeTo != "")) {
                alert("Both the date must be entered To Filter!");
                return false;
            }
            else {
                if (Date.parse(SubscribeFrom.trim()) > Date.parse(SubscribeTo.trim())) {
                    alert("Subscription To Date Must be Greater than Subscription From Date!");
                    document.getElementById("<%=txtToDt.ClientID %>").focus();
                    return false;
                }
                else {
                    return true;
                }
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
                    NewsMagazine List</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <div style="width: 100%; background-color: #666; padding: 1px; margin: 0 auto;">
                <div style="background-color: #FFF; padding: 10px;">
                    <table border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                        <tr>
                            <td width="120">
                                Subscribe From :
                            </td>
                            <td width="150">
                                <asp:TextBox ID="txtFrmDt" runat="server" Width="100px" MaxLength="100" CssClass="smalltb"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpFrmDt" runat="server" Control="txtFrmDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                            </td>
                            <td width="100">
                                Subscribe To :
                            </td>
                            <td width="150">
                                <asp:TextBox ID="txtToDt" runat="server" Width="100px" MaxLength="100" CssClass="smalltb"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                            </td>
                            <td width="100">
                                Periodicity :
                            </td>
                            <td width="150">
                                <asp:DropDownList ID="ddlPeriodicity" runat="server" CssClass="smalltb">
                                    <asp:ListItem Text="--All--" Value="--Select--" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                                    <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                                    <asp:ListItem Text="ByMonthly" Value="ByMonthly"></asp:ListItem>
                                    <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                    <asp:ListItem Text="HalfYearly" Value="HalfYearly"></asp:ListItem>
                                    <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    OnClientClick="return isValid();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="background-color: #666; padding: 1px; margin: 0 auto; margin-top: 15px;">
                <div style="background-color: #FFF; padding: 10px;">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tbltxt">
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
                            <td colspan="2">
                                <asp:GridView ID="grdNewsMagazineList" runat="server" AutoGenerateColumns="False"
                                    Width="100%" AllowPaging="True" PageSize="15" OnPageIndexChanging="grdNewsMagazineList_PageIndexChanging"
                                    CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <input type="checkbox" name="Checkb" value='<%#Eval("MagazineId")%>' />
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Left" Width="20px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <HeaderTemplate>
                                                <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <a href='NewsMagazineMaster.aspx?MagazineId=<%#Eval("MagazineId")%>'>Edit </a>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Left" Width="20px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Magazine Name">
                                            <ItemTemplate>
                                                <%#Eval("MagazineName")%>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Left" Width="80px" />
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subscription Date">
                                            <ItemTemplate>
                                                <%#Eval("SubscriptionDate")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                            <FooterStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Expired Date">
                                            <ItemTemplate>
                                                <%#Eval("SubscriptionExpired")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                            <FooterStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Periodicity">
                                            <ItemTemplate>
                                                <%#Eval("Periodicity")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                            <FooterStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount Paid">
                                            <ItemTemplate>
                                                <%#Eval("TotalAmountPaid")%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                            <FooterStyle Width="100px" />
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
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

