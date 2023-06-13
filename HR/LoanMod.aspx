<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LoanMod.aspx.cs" Inherits="HR_LoanMod" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server" >



  <script language="javascript" type="text/javascript">

      function cnf() {

          if (confirm("Are you sure to Save this Loan ?")) {
              return true;
          }
          else {

              return false;
          }
      }

      function cnfPost() {

          if (confirm("Are you sure to Postpone Loan Recovery of this Month ?")) {
              return true;
          }
          else {

              return false;
          }

      }
        
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_fm.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <div style="float: left;">
                    <h2>
                        Loan/Advance Modification</h2>
                </div>
            </div>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="95%">
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        <asp:RadioButton ID="rbModify" runat="server" GroupName="Loan" Text="Loan Modification"
                            AutoPostBack="true" OnCheckedChanged="rbModify_CheckedChanged" Checked="true"/>
                        <asp:RadioButton ID="rbPostpone" runat="server" GroupName="Loan" Text="Postpone Loan Recovery"
                            AutoPostBack="true" OnCheckedChanged="rbPostpone_CheckedChanged" />
                            &nbsp;
                        <asp:Button ID="btnBack" runat="server" Text="Back To HR Initialization" 
                            onclick="btnBack_Click" />
                    </td>
                </tr>
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        Select an Employee&nbsp;:&nbsp;<asp:DropDownList ID="drpEmp" runat="server" TabIndex="2"
                            AutoPostBack="True" OnSelectedIndexChanged="drpEmp_SelectedIndexChanged">
                        </asp:DropDownList>
                        ||&nbsp;Select a Loan&nbsp;:&nbsp;<asp:DropDownList ID="drpLoan" runat="server" TabIndex="3"
                            AutoPostBack="True" OnSelectedIndexChanged="drpLoan_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="trMsg" runat="server" style="padding: 2px;">
                    <td align="center">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr id="trLoan" runat="server" visible="false">
                    <td align="left" width="100%">
                        <table width="100%">
                            <tr style="background-color: #D3E7EE;" id="row2" runat="server" visible="false">
                                <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                                    color: #000; border: 1px solid #333; background-color: Transparent;">
                                    Guidelines:-
                                    <ol type="1" style="color: Red">
                                        <li>Enter month wise amount to be recovered against each month</li>
                                        <li>Click on &quot;Calculate Interest&quot; to Calculate Total Interest Amount</li>
                                        <li>Change Total Interest Amount (if required)</li>
                                        <li>Click on "Save"</li>
                                    </ol>
                                    Total Loan Amount&nbsp;:&nbsp;<asp:TextBox ID="txtLoanAmt" runat="server" TabIndex="7"
                                        Width="80px" MaxLength='20' onblur="if (this.value == '') {this.value = '0';}"
                                        onfocus="if(this.value == '0') {this.value = '';}" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Text="0" Enabled="false"></asp:TextBox>
                                    Principal Amount to be Recovered&nbsp;:&nbsp;<asp:TextBox ID="txtPending" runat="server"
                                        TabIndex="8" Width="80px" MaxLength='20' onblur="if (this.value == '') {this.value = '0';}"
                                        onfocus="if(this.value == '0') {this.value = '';}" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Text="0" Enabled="false"></asp:TextBox>
                                    &nbsp;Interest Amount to be Recovered&nbsp;:&nbsp;<asp:TextBox ID="txtInt" runat="server"
                                        TabIndex="9" Width="80px" MaxLength='20' onblur="if (this.value == '') {this.value = '0';}"
                                        onfocus="if(this.value == '0') {this.value = '';}" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Text="0"></asp:TextBox>
                                    <asp:Button ID="btnCalculate" runat="server" Text="Calculate Interest" onfocus="active(this);"
                                        onblur="inactive(this);" TabIndex="10" OnClick="btnCalculate_Click" />
                                    <asp:Button ID="btnModify" runat="server" Text="Save" onfocus="active(this);" onblur="inactive(this);"
                                        TabIndex="11" OnClick="btnModify_Click" OnClientClick="return cnf();" />
                                </td>
                            </tr>
                            <tr style="background-color: #D3E7EE;" id="row3" runat="server" visible="false">
                                <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                                    color: #000; border: 1px solid #333; background-color: Transparent;">
                                    Guidelines:-
                                    <ol type="1" style="color: Red">
                                        <li>Click on Postpone Button.</li>
                                        <li>Postponed Loan Recovery Amount wil be added at the Last.</li>
                                        <li>Postponed Loan Recovery can not be Reverted.</li>
                                    </ol>
                                    Month&nbsp;:&nbsp;<asp:TextBox ID="txtMonth" runat="server" TabIndex="7" Width="80px"
                                        Enabled="false"></asp:TextBox>
                                    Year&nbsp;:&nbsp;<asp:TextBox ID="txtYear" runat="server" TabIndex="8" Width="80px"
                                        Enabled="false"></asp:TextBox>
                                    &nbsp;Amount to be Recovered&nbsp;:&nbsp;<asp:TextBox ID="txtRecAmt" runat="server"
                                        TabIndex="9" Width="80px" Enabled="false" Text="0"></asp:TextBox>
                                    <asp:Button ID="btnPostpone" runat="server" Text="Postpone" onfocus="active(this);"
                                        onblur="inactive(this);" TabIndex="12" OnClientClick="return cnfPost();" OnClick="btnPostpone_Click" />
                                    <asp:HiddenField ID="hfLoanRecId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hfGenLedgerId" runat="server" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblRecords" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdLoanInit" runat="server" AutoGenerateColumns="False" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Month">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonth" runat="server" Text='<%#Eval("CalMonth")%>'></asp:Label>
                                                    <asp:HiddenField ID="hfLoanRecId" runat="server" Value='<%#Eval("LoanRecId")%>' />
                                                    <asp:HiddenField ID="hfRecType" runat="server" Value='<%#Eval("RecType")%>' />
                                                    <asp:HiddenField ID="hfGenLedgerId" runat="server" Value='<%#Eval("GenLedgerId")%>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Year">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYear" runat="server" Text='<%#Eval("CalYear")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Principal/Interest">
                                                <ItemTemplate>
                                                    <%#Eval("RecType")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Principal Amount to be Deducted">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAmount" runat="server" Width="80px" TabIndex="5" Text='<%#Eval("RecAmt")%>'
                                                        MaxLength='20' onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                        onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

