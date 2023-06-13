<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LoanInit.aspx.cs" Inherits="HR_LoanInit" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function disableBtn() {
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }

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

        function valSubmit() {
            var totalAmt = document.getElementById("<%=txtTotalAmt.ClientID %>").value;

            if (totalAmt == "" || parseFloat(totalAmt) <= 0) {
                alert("Enter Total Pending Amount to Recover");
                document.getElementById("<%=txtTotalAmt.ClientID %>").focus();
                return false;
            }
            else {
                return cnf();
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
                        Previous Loan/Advance Entry</h2>
                </div>
            </div>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="95%">
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        <asp:RadioButton ID="rbInit" runat="server" GroupName="Loan" Text="Outstanding Loan Entry"
                            AutoPostBack="true" Checked="true" OnCheckedChanged="rbInit_CheckedChanged" />
                        &nbsp; &nbsp; &nbsp; &nbsp;
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
                            <tr style="background-color: #D3E7EE;" id="row1" runat="server" visible="false">
                                <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                                    color: #000; border: 1px solid #333; background-color: Transparent;" valign="baseline">
                                    Guidelines:-
                                    <ol type="1" style="color: Red">
                                        <li>Enter total outstanding principal and interest loan/advance amount to be 
                                            recovered as 31<sup>st</sup> of the March.</li>
                                        <li>Enter month wise instalment amount to be recovered against each month.</li>
                                        <li>Please verify that sum of the instalments is equal to the principal amount.</li>
                                        <li>Click on &quot;Save&quot; to save loan details.</li>
                                    </ol>
                                    Total Outstanding Loan/Advance as on 31<sup>st</sup> of
                                    <asp:DropDownList ID="drpMonth" runat="server" TabIndex="4" OnSelectedIndexChanged="drpMonth_SelectedIndexChanged"
                                        AutoPostBack="true" Enabled="false">
                                        <asp:ListItem Text="MAR" Value="MAR"></asp:ListItem>
                                        <asp:ListItem Text="APR" Value="APR"></asp:ListItem>
                                        <asp:ListItem Text="MAY" Value="MAY"></asp:ListItem>
                                        <asp:ListItem Text="JUN" Value="JUN"></asp:ListItem>
                                        <asp:ListItem Text="JUL" Value="JUL"></asp:ListItem>
                                        <asp:ListItem Text="AUG" Value="AUG"></asp:ListItem>
                                        <asp:ListItem Text="SEP" Value="SEP"></asp:ListItem>
                                        <asp:ListItem Text="OCT" Value="OCT"></asp:ListItem>
                                        <asp:ListItem Text="NOV" Value="NOV"></asp:ListItem>
                                        <asp:ListItem Text="DEC" Value="DEC"></asp:ListItem>
                                        <asp:ListItem Text="JAN" Value="JAN"></asp:ListItem>
                                        <asp:ListItem Text="FEB" Value="FEB"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;
                                     Principal Amount
                                    :&nbsp;<asp:TextBox ID="txtTotalAmt" runat="server" TabIndex="4" Width="80px" MaxLength='20'
                                        onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text="0"></asp:TextBox>
                                        &nbsp;
                                        Interest Amount
                                        :&nbsp;<asp:TextBox ID="txtInterst" runat="server" TabIndex="4" Width="80px" MaxLength='20'
                                        onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" Text="0"></asp:TextBox>
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
                            <tr>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" onfocus="active(this);" onblur="inactive(this);" 
                                        TabIndex="6" OnClick="btnSave_Click" OnClientClick="return valSubmit();"  Font-Size="15px"
                                        Height="30px" Width="187px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
