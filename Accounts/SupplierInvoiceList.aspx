<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="SupplierInvoiceList.aspx.cs" Inherits="Accounts_SupplierInvoiceList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
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

    <table style="width: 100%;" cellspacing="0" cellpadding="0">
        <tr>
            <td height="20" valign="top">
                <div class="bedcromb">
                    Expenses &raquo; Payments
                </div>
            </td>
        </tr>
        <tr style="width: 100%" align="center">
            <td>
                <br />
                <div align="center">
                    <div style="width: 90%; background-color: #666; padding: 2px; margin: 0 auto;">
                        <div style="background-color: #FFF; padding: 10px;">
                            <div class="linegap">
                                <img src="../images/mask.gif" height="5" width="10">
                            </div>
                            <table cellspacing="0" cellpadding="0" width="100%">
                                <tr>
                                    <td valign="bottom" colspan="2" class="gridtext">
                                        <fieldset style="height: 40px; vertical-align: bottom;">
                                            <legend style="background-color: Transparent;" class="gridtext"><strong>Selection Criteria
                                            </strong></legend><strong class="gridtext">&nbsp;&nbsp;From Date:</strong>&nbsp;
                                            <asp:TextBox ID="txtFromDate" runat="server" Height="17px" Width="150px" ReadOnly="True"
                                                CssClass="gridtext"></asp:TextBox>
                                            <rjs:PopCalendar ID="dtpFromDate" runat="server" Control="txtFromDate" AutoPostBack="False"
                                                Format="dd mmm yyyy"></rjs:PopCalendar>
                                            &nbsp; <strong class="gridtext">&nbsp;&nbsp;To Date:</strong>&nbsp;
                                            <asp:TextBox ID="txtToDate" runat="server" Height="17px" Width="150px" ReadOnly="True"
                                                CssClass="gridtext"></asp:TextBox>
                                            <rjs:PopCalendar ID="dtpToDate" runat="server" Control="txtToDate" AutoPostBack="False"
                                                Format="dd mmm yyyy"></rjs:PopCalendar>
                                            &nbsp; &nbsp;<strong class="gridtext">Creditor :</strong>&nbsp;
                                            <asp:DropDownList ID="drpCreditor" runat="server" Width="200px" CssClass="gridtext">
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:Button ID="btnshow" runat="server" CausesValidation="False" Text="Show" OnClick="btnshow_Click" />&nbsp;
                                            &nbsp;
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <asp:Button ID="btnNew" runat="server" Text="New Payment" OnClick="btnNew_Click" />&nbsp;
                                        <asp:Button ID="btnDelete" runat="server" Text="Cancel Selected Invoice" OnClick="btnDelete_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 90%">
                                        <asp:Label ID="lblMsg" runat="server" CssClass="tbltxt"></asp:Label>
                                    </td>
                                    <td class="totalrec">
                                        <div style="float: right;">
                                            <asp:Label ID="lblRecCount" runat="server" Text=""></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="grdSupplierMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="InvoiceId"
                                            Width="100%" AllowPaging="True" OnPageIndexChanging="grdSupplierMaster_PageIndexChanging"
                                            OnRowDataBound="grdSupplierMaster_RowDataBound"
                                            OnRowCreated="grdSupplierMaster_RowCreated">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <%--<HeaderTemplate>
                                                        <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                    </HeaderTemplate>--%>
                                                    <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                    <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                    <ItemTemplate>
                                                        <div id="divReal" runat="server">
                                                            <input name="Checkb" type="checkbox" value='<%#Eval("InvoiceId")%>' />
                                                        </div>
                                                        <div id="divDummy" runat="server" visible="false">
                                                            <input type="checkbox" name="Checkb" value="" disabled="disabled" />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlEdit" runat="server">Edit</asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Creditor Name">
                                                    <ItemTemplate>
                                                        <%#Eval("SupName")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MR No">
                                                    <ItemTemplate>
                                                        <%#Eval("MRNo")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MR Date">
                                                    <ItemTemplate>
                                                        <%#Eval("MRDateStr")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pay Slip No">
                                                    <ItemTemplate>
                                                        <%#Eval("PaySlipNo")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payment Details">
                                                    <ItemTemplate>
                                                        <%#Eval("PmtDetails")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payment Mode">
                                                    <ItemTemplate>
                                                        <%#Eval("PaymentMode")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <%#Eval("Amount")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Voucher">
                                                    <ItemTemplate>
                                                        <a target="_blank" href='../Reports/rptCashVouchers.aspx?inv=<%#Eval("InvoiceId")%>'>Print</a>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Management">
                                                    <HeaderStyle Width="80px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnSecr" runat="server" Text="" OnClick="btnStatus_Click" />
                                                        <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("InvoiceId")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Authorised Person">
                                                    <HeaderStyle Width="120px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnHeadClerk" runat="server" Text="" OnClick="btnHeadClerk_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No Record
                                            </EmptyDataTemplate>
                                            <PagerStyle CssClass="gridtext" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="headergrid" />
                                            <RowStyle CssClass="gridtext" />
                                            <AlternatingRowStyle CssClass="gridtext" />
                                            <EmptyDataRowStyle CssClass="gridtext" HorizontalAlign="Center" BackColor="Gray"
                                                ForeColor="White" Font-Size="Large" Font-Bold="true" />
                                        </asp:GridView>
                                        <div style="height: 5px;"></div>
                                        <div style="width: 99%; text-align: left; padding: 4px; color: maroon; background-color: silver; border: solid 1px gray;">
                                            *** Records with red color are not approved yet by either Management or Authorized Person or both. 
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

