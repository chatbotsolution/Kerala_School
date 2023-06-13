<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="HostExpenseList.aspx.cs" Inherits="Hostel_HostExpenseList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
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
            Hostel Expenditure Details</h2>
    </div>
    <div class="linegap">
        <img src="../images/mask.gif" height="5" width="10" />
    </div>
    <div class="linegap">
        <img src="../images/mask.gif" height="5" width="10" />
    </div>
    <table width="100%">
        <tr>
            <td style="height: 10px" colspan="2">
            </td>
        </tr>
        <tr>
            <td valign="bottom" colspan="2">
                <fieldset style="height: 40px; vertical-align: bottom;">
                    <legend style="background-color: Transparent;" class="tbltxt"><strong>Selection Criteria
                    </strong></legend><strong class="tbltxt">&nbsp;&nbsp;From Date:</strong>&nbsp;
                    <asp:TextBox ID="txtFromDate" runat="server" ReadOnly="True"
                        CssClass="tbltxt"></asp:TextBox>
                    <rjs:PopCalendar ID="dtpFromDate" runat="server" Control="txtFromDate" AutoPostBack="False"
                        Format="dd mmm yyyy"></rjs:PopCalendar>
                    &nbsp; <strong class="tbltxt">&nbsp;&nbsp;To Date:</strong>&nbsp;
                    <asp:TextBox ID="txtToDate" runat="server" ReadOnly="True"
                        CssClass="tbltxt"></asp:TextBox>
                    <rjs:PopCalendar ID="dtpToDate" runat="server" Control="txtToDate" AutoPostBack="False"
                        Format="dd mmm yyyy"></rjs:PopCalendar>
                    &nbsp; &nbsp;&nbsp;
                    <asp:Button ID="btnshow" runat="server" CausesValidation="False" Text="Show" OnClick="btnshow_Click" />
                </fieldset>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 455px">
                &nbsp;<asp:Button ID="btnNew" runat="server" OnClick="btnNew_Click" Text="New Expense" />
                &nbsp;
                <asp:Button ID="btndelete" runat="server" CausesValidation="False" OnClientClick="return CnfDelete();"
                    OnClick="btndelete_Click" Text="Cancel Selected Record" Enabled="false" />
            </td>
            <td style="width: 20px;" align="right" class="tdMsg">
                <asp:Label ID="lblNoOfRec" runat="server" Text="" Style="text-align: right"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="gvMiscExp" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="mGrid tbltxt" AllowPaging="True" PageSize="15" OnPageIndexChanging="gvMiscExp_PageIndexChanging">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="10px" />
                            <ItemTemplate>
                                <input type="checkbox" name="Checkb" value='<%# Eval("PR_Id") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Exp Date">
                            <HeaderStyle Width="100px" HorizontalAlign="Left"/>
                            <ItemTemplate>
                                <%#Eval("TransDateStr")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Expenditure Details">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("Particulars")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Receipt/Voucher No.">
                            <HeaderStyle Width="130px" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <%#Eval("PmtRecptVoucherNo")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <HeaderStyle Width="100px" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <%#Eval("TransAmt")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFEFEF" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#FDFDFD" />
                    <EmptyDataTemplate>
                        No Record
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>

