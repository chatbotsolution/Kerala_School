<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="ReceiptVoucherList.aspx.cs" Inherits="Accounts_ReceiptVoucherList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        function CnfDelete() {

            if (confirm("You are going to delete a record. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }
        function SaveReason() {
            var Del = document.getElementById("<%=txtDelReason.ClientID %>").value;
            if (Del.trim() == "") {
                alert("Please enter Reason for Deletion !");
                document.getElementById("<%=txtDelReason.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor">
                            <h1>
                                Receipt
                            </h1>
                            <h2>
                                Voucher List</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="98%">
                <tr>
                    <td align="left" colspan="2">
                        <asp:Label ID="lblToDt" runat="server" Text="From Date : "></asp:Label>
                        <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" Width="80px" TabIndex="1"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
                        </rjs:PopCalendar>
                        &nbsp;<asp:Label ID="lblFrmDt" runat="server" Text="To Date : "></asp:Label>
                        <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" Width="80px" TabIndex="2"></asp:TextBox>
                        <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" Format="dd mmm yyyy">
                        </rjs:PopCalendar>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnView" runat="server" TabIndex="3" Text="View List" onfocus="active(this);" onblur="inactive(this);" OnClick="btnView_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnAdd" runat="server" Text="Receipt New" onfocus="active(this);" onblur="inactive(this);" OnClick="btnAdd_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        
                    </td>
                    <td align="right">
                        <asp:Label ID="lblRecord" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:GridView ID="grdParty" runat="server" OnPageIndexChanging="grdParty_PageIndexChanging"
                            DataKeyNames="PR_Id" Width="100%" PageSize="20" AllowPaging="true" AutoGenerateColumns="false"
                            AllowSorting="True" OnRowDeleting="grdParty_RowDeleting" 
                            ondatabound="grdParty_DataBound" >
                            <Columns>
                                <asp:TemplateField HeaderText="Action" >
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("PR_Id") %>' Visible="false"></asp:Label>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif"
                                                OnClientClick="return CnfDelete()" Visible="false" /></center>
                                    </ItemTemplate>
                                    <HeaderStyle Width="60px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Ag_Code" Visible="False"></asp:BoundField>
                                
                                
                                <asp:TemplateField HeaderText="Transaction Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<% #Bind("TransDtStr")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="PmtMode" HeaderText="Payment Mode">
                                <ItemStyle Width="100px" HorizontalAlign="Left"/>
                                <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="Description" HeaderText="Description">
                                <ItemStyle HorizontalAlign="Left"/>
                                <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Received From">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartyName" runat="server" Text='<%#Bind("RcvdFrom") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Received Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%#Bind("Amount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" Width="120px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                <td align="left" colspan="2">
                    <asp:Panel ID="pnlDelReason" runat="server" Width="70%" Style="border: solid 1px #ACA899; padding: 5px 5px 5px 5px"
                        CssClass="tbltxt" Visible="false">
                        <asp:Literal ID="litDetails" runat="server"></asp:Literal>
                        <br />
                        <b>Enter Reason &nbsp;:-&nbsp;</b>
                        <br />
                        <asp:TextBox ID="txtDelReason" runat="server" CssClass="vsmalltb" Height="30px" Width="100%" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                        <br />
                        <asp:Button ID="btnSaveReason" runat="server" Text="Cancel Receipt" OnClick="btnSaveReason_Click"
                            OnClientClick="return SaveReason();" />
                        <asp:Button ID="btnCancel" runat="server" Text="Back to List" OnClick="btnCancel_Click" />
                    </asp:Panel>
                    <asp:HiddenField ID="hfRcptId" runat="server" />
                </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

