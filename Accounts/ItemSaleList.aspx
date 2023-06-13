<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true"
    CodeFile="ItemSaleList.aspx.cs" Inherits="Accounts_ItemSaleList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function formatThousandPlace(orgNum) {
            arr = orgNum.split('.');    //split integer part & fractional part
            intPart = arr[0];           //store integer part in a variable
            fractionPart = arr.length > 1 ? '.' + arr[1] : '';  //store fractional part in a variable
            var rgx = /(\d+)(\d{3})/;   //regexp to find thousand place
            while (rgx.test(intPart)) {
                intPart = intPart.replace(rgx, '$1' + ',' + '$2');  //replace integer part after inserting a comma in the thousandth place
            }
            document.write(intPart + fractionPart); //write final value to the document
        }
        function CnfDelete() {

            if (confirm("You are going Delete This Sale. Do you want to continue?")) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <table width="100%" cellspacing="0" style="border: solid 1px gray; border-radius: 10px 10px;
        background-color: #ededed; box-shadow: 3px 3px 5px #888888;">
        <tr>
            <td width="350" align="left" valign="middle" style="background-color: #D3C8BD; border: solid 1px #D3C8BD;">
                <div class="headingcontainor">
                    <h1>
                        Items&nbsp;Sold</h1>
                </div>
            </td>
            <td height="35" align="left" valign="middle" style="background-color: #D3C8BD; border: solid 1px #D3C8BD;">
                <asp:Literal ID="lblMsg" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding: 3px; border-top: solid 1px gray; font-weight: bold;">
                From&nbsp;Date&nbsp;:&nbsp;<asp:TextBox runat="server" ID="txtFromDt" TabIndex="1" />&nbsp;<rjs:PopCalendar
                    ID="dtpFromDt" runat="server" Control="txtFromDt"></rjs:PopCalendar>
                &nbsp;To&nbsp;Date&nbsp;:&nbsp;<asp:TextBox runat="server" ID="txtToDt" TabIndex="2" />&nbsp;<rjs:PopCalendar
                    ID="dtpToDt" runat="server" Control="txtToDt"></rjs:PopCalendar>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" style="font-weight: bold; padding: 3px;">
                Session :&nbsp;<asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb"
                    Width="70px" AutoPostBack="true" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                    TabIndex="3">
                </asp:DropDownList>
                &nbsp;Class :&nbsp;<asp:DropDownList ID="drpclass" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="drpclass_SelectedIndexChanged" CssClass="vsmalltb" Width="75px"
                    TabIndex="4">
                </asp:DropDownList>
                &nbsp;Student :&nbsp;<asp:DropDownList ID="drpstudent" runat="server" Width="200px"
                    AutoPostBack="True" OnSelectedIndexChanged="drpstudent_SelectedIndexChanged"
                    CssClass="largetb" TabIndex="5">
                </asp:DropDownList>
                &nbsp;Student Id :&nbsp;<asp:TextBox ID="txtadmnno" runat="server" CssClass="vsmalltb"
                    Width="90px" TabIndex="6"></asp:TextBox><span class="error">*</span>&nbsp;<asp:Button
                        Text="Search Student" runat="server" ID="btnSearch" OnClick="btnSearch_Click"
                        TabIndex="7" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="font-weight: bold; padding: 3px;">
                Bill&nbsp;No.&nbsp;:&nbsp;<asp:TextBox runat="server" ID="txtBillNo" TabIndex="8" />&nbsp;<asp:Button
                    Text="Show" runat="server" ID="btnShow" OnClick="btnShow_Click" TabIndex="9" />&nbsp;<asp:Button
                        Text="New Sale" runat="server" OnClick="btnNewSale_Click" ID="btnNewSale" TabIndex="10" />
            </td>
        </tr>
    </table>
    <div style="height: 10px;">
    </div>
    <table style="width: 100%;">
        <tr>
            <td colspan="2" align="left">
                <asp:GridView ID="gvBills" runat="server" OnPageIndexChanging="gvBills_PageIndexChanging"
                    DataKeyNames="InvNo" Width="100%" PageSize="15" AllowPaging="true" AutoGenerateColumns="false"
                    OnRowCreated="gvBills_RowCreated" OnRowDataBound="gvBills_RowDataBound" CssClass="mGrid"
                    PagerStyle-CssClass="pgr" TabIndex="11" OnRowDeleting="gvBills_RowDeleting">
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl2" runat="server" Text='<%#Eval("InvNo") %>' Visible="false"></asp:Label>
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif"
                                        OnClientClick="return CnfDelete()" Visible="false" /></center>
                            </ItemTemplate>
                            <HeaderStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Details">
                            <ItemTemplate>
                                <asp:Image Height="20px" runat="server" ID="imgDetails" ImageUrl="~/Images/binoculars.png"
                                    AlternateText="Sale Details" CssClass="cursor-hand" />
                                <ajaxToolkit:PopupControlExtender ID="pceDetails" runat="server" DynamicServiceMethod="GetDynamicContent"
                                    DynamicContextKey='<%# Eval("InvNo") %>' DynamicControlID="pnlPopUp" TargetControlID="imgDetails"
                                    PopupControlID="pnlPopUp" Animations="" Position="Center">
                                </ajaxToolkit:PopupControlExtender>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="InvDateStr" HeaderText="Date" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Bill No">
                            <ItemTemplate>
                                <asp:Label ID="lblBill" runat="server" Text='<%#Eval("PhysicalBillNo")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Party Name">
                            <ItemTemplate>
                                <%#Eval("FullName")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="NetPayableAmt" HeaderText="Bill Amt" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" DataFormatString="{0:F2}" HtmlEncode="False"
                            HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Pmt Mode">
                            <ItemTemplate>
                                <%#Eval("IsCashInvoice")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="pgr"></PagerStyle>
                    <EmptyDataTemplate>
                        No Record
                    </EmptyDataTemplate>
                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="Gray" ForeColor="White" Font-Size="Large"
                        Font-Bold="true" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:Panel ID="pnlPopUp" runat="server" Width="500px">
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnCnf" runat="server" />
</asp:Content>
