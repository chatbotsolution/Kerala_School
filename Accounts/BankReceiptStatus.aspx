<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="BankReceiptStatus.aspx.cs" Inherits="Accounts_BankReceiptStatus" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
        function isValid() {
            var Bank = document.getElementById("<%=drpBank.ClientID %>").value;
            var Status = document.getElementById("<%=drpStatus.ClientID %>").value;
            var StatusDt = document.getElementById("<%=txtStatusDt.ClientID %>").value;
            if (Bank == "0") {
                alert("Please Select Bank !");
                document.getElementById("<%=drpBank.ClientID %>").focus();
                return false;
            }
            if (Status == "0") {
                alert("Please Select Status !");
                document.getElementById("<%=drpStatus.ClientID %>").focus();
                return false;
            }
            if (StatusDt.trim() == "") {
                alert("Please Select Status Date !");
                document.getElementById("<%=txtStatusDt.ClientID %>").focus();
                return false;
            }
            if (!CnfDelete()) {
                return false;
            }
            else {
                return true;
            }
        }

        function CnfDelete() {

            if (confirm("You are going to set Bank Receipt Status. Do you want to continue?")) {
                return true;
            }
            else {
                return false;
            }
        }

        function SelectAll(name) {

            var grid = document.getElementById("<%= grdReceiptStatus.ClientID %>");
            var cell;

            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[0];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            cell.childNodes[j].checked = name.checked;
                        }
                    }
                }
            }
        }
    
    </script>

    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor">
                            <h1>
                                Bank Transaction Status
                            </h1>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlList" runat="server">
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            From Date :
                            <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" Width="80px" TabIndex="1"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            To Date :
                            <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" Width="80px" TabIndex="2"></asp:TextBox>
                            <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            &nbsp;
                            Status : 
                            <asp:DropDownList ID="drpStatusFilter" runat="server">
                                <asp:ListItem Value="0">-Pending-</asp:ListItem>
                             <%--  <asp:ListItem Value="d">Deposited</asp:ListItem>--%>
                                <asp:ListItem Value="e">Encashed</asp:ListItem>
                                <asp:ListItem Value="b">Bounced</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;<asp:RadioButton ID="rBtnReceipt" GroupName="p" Text="Receipt" Checked="true"
                                runat="server" OnCheckedChanged="rBtnReceipt_CheckedChanged" AutoPostBack="True" />
                            &nbsp;<asp:RadioButton ID="rBtnPayment" GroupName="p" Text="Payment" Checked="false"
                                runat="server" OnCheckedChanged="rBtnPayment_CheckedChanged" AutoPostBack="True" />
                            &nbsp;<asp:Button ID="btnView" runat="server" TabIndex="3" Text="View List" OnClick="btnView_Click" />
                            <asp:Button ID="btnTrans" runat="server" TabIndex="4" Width="170px" Text="Cash Deposit / Withdrawl"
                                OnClick="btnTrans_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            <asp:Label ID="lblRecord" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:GridView ID="grdReceiptStatus" runat="server" DataKeyNames="PR_Id" Width="100%"  AutoGenerateColumns="false"
                                onrowdatabound="grdReceiptStatus_RowDataBound">
                                <Columns>
                                 <asp:TemplateField HeaderText="Bank Account">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDepositedBank" runat="server" Text='<%#Bind("Bankname") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Received From">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartyName" runat="server" Text='<%#Bind("RcvdFrom") %>'></asp:Label>
                                            <asp:HiddenField ID="hfPRNo" runat="server" Value='<%#Eval("PR_Id") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payble Bank Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaybleBankName" runat="server" Text='<% #Bind("DrawanOnBank")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="130px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Instrument No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstrumentNo" runat="server" Text='<% #Bind("InstrumentNo")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Instrument Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstrumentDt" runat="server" Text='<% #Bind("InstrumentDtstr")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%#Bind("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="Status (Date)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Select" ForeColor="Red" OnClick="btnSelect_Click" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="70px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                             <asp:GridView ID="grdPayment" runat="server" DataKeyNames="PR_Id" Width="100%" AutoGenerateColumns="false"
                                onrowdatabound="grdPayment_RowDataBound">
                                <Columns>
                                 <asp:TemplateField HeaderText="Bank Account">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDepositedBank" runat="server" Text='<%#Bind("Bankname") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transaction Details">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartyName" runat="server" Text='<%#Eval("PartyName") %>'></asp:Label>
                                            <asp:HiddenField ID="hfPRNo" runat="server" Value='<%#Eval("PR_Id") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Instrument No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstrumentNo" runat="server" Text='<% #Bind("InstrumentNo")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Instrument Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstrumentDt" runat="server" Text='<% #Bind("InstrumentDt")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%#Bind("Amount1") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="Status (Date)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatusPaid" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelectPaid" runat="server" Text="Select" ForeColor="Red" OnClick="btnSelectPaid_Click" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="70px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
             <asp:Panel ID="pnlEntry" runat="server" Visible="false">

                <table width="100%">
                <tr><td></td></tr>
                    <tr>
                        <td style="width: 130px;">
                            <b>Bank Account</b></td>
                        <td style="width: 5px;">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpBank" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr><td></td></tr>
                    <tr>
                        <td>
                            <b>Current Status</b>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpStatus" runat="server">
                                <asp:ListItem Value="0">-Select-</asp:ListItem>
                               <%-- <asp:ListItem Value="d">Deposited</asp:ListItem>--%>
                                <asp:ListItem Value="e">Encashed</asp:ListItem>
                                <asp:ListItem Value="b">Bounced</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr><td></td></tr>
                    <tr>
                        <td>
                            <b>Status Date</b>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtStatusDt" runat="server" Width="80px" TabIndex="2" 
                                Height="22px"></asp:TextBox>
                           <%-- <rjs:PopCalendar ID="dtpStatusDt" runat="server" Control="txtStatusDt"></rjs:PopCalendar>--%>
                           <asp:ImageButton ImageUrl="~/Images/Calendar.png" ID="btnStatusDt" CausesValidation="false"
                                runat="server" Text="..." OnClick="btnStatusDt_Click" />&nbsp;
                            <div id="divCalendar1" style="position: absolute;">
                                <asp:Calendar ID="dtpStatusDt" runat="server" BackColor="#FFFFCC" BorderColor="#FFCC66"
                                    BorderWidth="1px" DayNameFormat="Shortest" FirstDayOfWeek="Monday" Font-Names="Verdana"
                                    Font-Size="8pt" ForeColor="#663399" Height="150px" Width="186px" TargetControlID="TBIhaleBaslamaTarihi"
                                    Visible="False" OnSelectionChanged="cdrCalendar_SelectionChanged" ShowGridLines="True"
                                    SelectionMode="Day" ondayrender="dtpStatusDt_DayRender" >
                                    <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                                    <SelectorStyle BackColor="#FFCC66" />
                                    <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                                    <OtherMonthDayStyle ForeColor="#CC9966" />
                                    <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                                    <DayHeaderStyle BackColor="#FFCC66" Height="1px" Font-Bold="True" />
                                    <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" ForeColor="#FFFFCC" />
                                </asp:Calendar>
                            </div>
                            [dd-MM-yyyy]</td>
                    </tr>
                    <tr><td></td></tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnStatus" runat="server" TabIndex="3" Text="Set Status" OnClick="btnStatus_Click"
                                OnClientClick="return isValid();" />
                            <asp:Button ID="btnCancel" runat="server" TabIndex="4" Text="Cancel" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
            
    </asp:Panel>
            
            
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
            <asp:PostBackTrigger ControlID="grdReceiptStatus" />
              <asp:PostBackTrigger ControlID="btnStatus" />
                <asp:PostBackTrigger ControlID="btnCancel" />
        </Triggers>
    </asp:UpdatePanel>
   
</asp:Content>
