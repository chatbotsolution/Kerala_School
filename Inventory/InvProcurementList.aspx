<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="InvProcurementList.aspx.cs" Inherits="Inventory_InvProcurementList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        $(document).ready(function () {
            document.getElementById("<%=txtFrmDt.ClientID %>").focus();
        });

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
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=580,height=340,left = 490,top = 184');");
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Procurement List</h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblmsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px">
                <table>
                    <tr>
                        <td>
                            <strong>Date From:</strong>
                            <asp:TextBox ID="txtFrmDt" runat="server" Width="80px" CssClass="tbltxtbox"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtFrmDt" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFrmDt.value='';return false;"
                                Text="Clear"></asp:LinkButton>
                            &nbsp;&nbsp;&nbsp; <strong>Date To:</strong>
                            <asp:TextBox ID="txtTo" runat="server" Width="80px" CssClass="tbltxtbox"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtTo" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtTo.value='';return false;"
                                Text="Clear"></asp:LinkButton>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                            <asp:Button ID="btnAddNew" runat="server" Text="Add New" OnClick="btnAddNew_Click" />&nbsp;
                            <asp:Button ID="btnDel" runat="server" Text="Delete" OnClick="btnDel_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: left; padding-left: 0px; padding-right: 0px;" class="tbltxt">
                <asp:GridView ID="grdShowDtls" runat="server" CellPadding="4" ForeColor="#333333"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No Data Found"
                    Width="100%" OnPageIndexChanging="grdShowDtls_PageIndexChanging" PagerStyle-CssClass="pgr"
                    AlternatingRowStyle-CssClass="alt" CssClass="mGrid">
                    <EmptyDataRowStyle Font-Bold="True" Font-Size="10pt" HorizontalAlign="Left" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <input name="Checkb" type="checkbox" value='<%#Eval("PurchaseId")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="PurDate" HeaderText="Purchase Dt" DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="InvNo" HeaderText="Invoice No">
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SupplierName" HeaderText="Supplier">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SupplierAddress" HeaderText="Address">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SupplierContactNo" HeaderText="Phone">
                            <HeaderStyle HorizontalAlign="Center" Width="70px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Item List">
                            <ItemTemplate>
                                <a href="javascript:popUp('PurchaseDetails.aspx?PurchaseId=<%#Eval("PurchaseId")%>')">
                                    View Items</a>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TotAmt" HeaderText="Total Amt" DataFormatString="{0:f2}">
                            <HeaderStyle HorizontalAlign="Right" Width="100px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                    <RowStyle BackColor="#EFEFEF" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#FDFDFD" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

