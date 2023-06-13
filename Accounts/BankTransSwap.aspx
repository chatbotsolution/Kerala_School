<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="BankTransSwap.aspx.cs" Inherits="Accounts_BankTransSwap" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor">
                            <h1>
                                Modify Bank Account For Received Cheque
                            </h1>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
    <table width="100%">
        <tr>
            <td colspan="2">
                From Date :
                <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" Width="80px" TabIndex="1"></asp:TextBox>
                <rjs:popcalendar id="dtpfromdt" runat="server" control="txtFromDt" format="dd mmm yyyy">
                            </rjs:popcalendar>
                To Date :
                <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" Width="80px" TabIndex="2"></asp:TextBox>
                <rjs:popcalendar id="dtptodt" runat="server" control="txtToDt" format="dd mmm yyyy">
                            </rjs:popcalendar>
                &nbsp;
                Bank : 
                <asp:DropDownList ID="drpBank" runat="server">
                </asp:DropDownList>
                &nbsp;
                <asp:Button ID="btnView" runat="server" TabIndex="3" Text="View List" 
                    onclick="btnView_Click" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="right">
                <asp:Label ID="lblRecord" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <asp:GridView ID="grdDisplay" runat="server" DataKeyNames="PR_Id" Width="100%"
                    AutoGenerateColumns="false" onrowdatabound="grdDisplay_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Bankname" HeaderText="Existing Bank Ac" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="TransDtStr" HeaderText="Trans Dt" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                        <asp:TemplateField HeaderText="Received From">
                            <ItemTemplate>
                                <asp:Label ID="lblPartyName" runat="server" Text='<%#Bind("RcvdFrom") %>'></asp:Label>
                                <asp:HiddenField ID="hfPRNo" runat="server" Value='<%#Eval("PR_Id") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="DrawanOnBank" HeaderText="Payble Bank Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                       
                         <asp:BoundField DataField="InstrumentNo" HeaderText="Inst No" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
          
                         <asp:BoundField DataField="InstrumentDtstr" HeaderText="Instrument Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                        
                         <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                         <asp:TemplateField HeaderText="Credited To Bank Ac">
                            <ItemTemplate>
                                <asp:DropDownList ID="drpBankSwp" runat="server">
                                 </asp:DropDownList>
                                 <asp:Label ID="lblBnkAc" runat="server" Text='<%#Bind("BankAcHeadId") %>' Visible="false"></asp:Label>
                                <asp:Button ID="btnSwap" runat="server" Text="Modify"
                                onclick="btnSwap_Click" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="70px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                
            </td>
        </tr>
    </table>
    </ContentTemplate>
        <Triggers>
              <asp:PostBackTrigger ControlID="btnView" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

