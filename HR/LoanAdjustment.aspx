<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LoanAdjustment.aspx.cs" Inherits="HR_LoanAdjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<script language="javascript" type="text/javascript">
    function cnf() {

        if (confirm("You Are Going To Adjust Loan Amount ? Are You Sure???")) {
            return true;
        }
        else {

            return false;
        }
    }
    function valSubmit() {
        var totalAmt = document.getElementById("<%=lblTotAmt.ClientID %>").innerHTML;

        if (totalAmt == "" || parseFloat(totalAmt) <= 0) {
            alert("No Pending Amount To Recover");
            return false;
        }
        else {
            return cnf();
        }
    }
    function valAdjLoan() {
        var totalAmt = document.getElementById("<%=lblPendAmt.ClientID %>").innerHTML;
        var totalRec = document.getElementById("<%=txtLoanAmt.ClientID %>").value;
        if (totalAmt == "" || parseFloat(totalAmt) <= 0) {
            alert("No Amount Available To Recover to Recover!!");
            return false;
        }
        if (totalRec == "" || parseFloat(totalAmt) <= 0) {
            alert("Enter Total Pending Amount to Recover");
            document.getElementById("<%=txtLoanAmt.ClientID %>").focus();
            return false;
        }
        else {
            return cnf();
        }
    }
    function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
        var key;
        var isCtrl = false;
        var keychar;
        var reg;

        if (window.event) {
            key = e.keyCode;
            isCtrl = window.event.ctrlKey
        }
        else if (e.which) {
            key = e.which;
            isCtrl = e.ctrlKey;
        }

        if (isNaN(key)) return true;

        keychar = String.fromCharCode(key);

        // check for backspace or delete, or if Ctrl was pressed
        if (key == 8 || isCtrl) {
            return true;
        }

        reg = /\d/;
        var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
        var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

        return isFirstN || isFirstD || reg.test(keychar);
    }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_fm.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <div style="float: left;">
                    <h2>
                        Loan Adjustment (To be Used for first time implementation Only)</h2>
                </div>
            </div>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="95%">
            <tr><td align="center">
                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></td></tr>
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        <asp:RadioButton ID="rbEmp" runat="server" GroupName="Loan" Text="For Employee"
                            AutoPostBack="true" Checked="true" 
                            oncheckedchanged="rbLoan_CheckedChanged" />
                        <asp:RadioButton ID="rbLoan" runat="server" GroupName="Loan" Text="For Loan Account Head"
                            AutoPostBack="true" oncheckedchanged="rbLoan_CheckedChanged" />
                        &nbsp;||&nbsp;
                            Select a Loan&nbsp;:&nbsp;<asp:DropDownList ID="drpLoan" runat="server" TabIndex="3"
                            AutoPostBack="True" OnSelectedIndexChanged="drpLoan_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <b style="color:Red;">NOTE: Loan Amount Once Adjusted Cannot Be Reverted!! 
                        Please Check The Amount Before Adjustment!!</b>
                    </td>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlEmp" runat="server" Visible="true">
                                <table>
                                    <tr>
                                        <td>
                                            &nbsp;Select an Employee&nbsp;:&nbsp;<asp:DropDownList ID="drpEmp" runat="server" 
                                                AutoPostBack="True" onselectedindexchanged="drpEmp_SelectedIndexChanged" 
                                                TabIndex="2">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Adjust Loan Upto :
                                            <asp:DropDownList ID="drpMonth" runat="server" AutoPostBack="true" 
                                                onselectedindexchanged="drpMonth_SelectedIndexChanged" TabIndex="4">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="APR" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="MAY" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="JUN" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="JUL" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="AUG" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="SEP" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="OCT" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="NOV" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="DEC" Value="12"></asp:ListItem>
                                                <asp:ListItem Text="JAN" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="FEB" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="MAR" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTot" runat="server" Text="Total Amount:"></asp:Label>
                                            <asp:Label ID="lblTotAmt" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnAdjust" runat="server" onblur="inactive(this);" 
                                                onclick="btnAdjust_Click" OnClientClick="return valSubmit();" 
                                                onfocus="active(this);" TabIndex="11" Text="Adjust" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="grdDisplay" runat="server" AutoGenerateColumns="False" 
                                                Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Month">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMonth" runat="server" Text='<%#Eval("CalMonth")%>'></asp:Label>
                                                            <asp:HiddenField ID="hfLoanRecId" runat="server" 
                                                                Value='<%#Eval("LoanRecId")%>' />
                                                            <asp:HiddenField ID="hfRecType" runat="server" Value='<%#Eval("RecType")%>' />
                                                            <asp:HiddenField ID="hfGenLedgerId" runat="server" 
                                                                Value='<%#Eval("GenLedgerId")%>' />
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
                                                            <asp:TextBox ID="txtAmount" runat="server" MaxLength="20" 
                                                                onblur="if (this.value == '') {this.value = '0';}" 
                                                                onfocus="if(this.value == '0') {this.value = '';}" 
                                                                onkeypress="return blockNonNumbers(this, event, true, false);" TabIndex="5" 
                                                                Text='<%#Eval("RecAmt")%>' Width="80px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlLoan" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPend" runat="server" Text="Total Amount:"></asp:Label>
                                            <asp:Label ID="lblPendAmt" runat="server" Text="0"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLoanAmt" runat="server" 
                                                onkeypress="return blockNonNumbers(this, event, true, false);" Text="0"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnAdjLoan" runat="server" onblur="inactive(this);" 
                                                onclick="btnAdjLoan_Click" OnClientClick="return valAdjLoan();" 
                                                onfocus="active(this);" TabIndex="11" Text="Adjust" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btnAdjust" />
        <asp:PostBackTrigger ControlID="btnAdjLoan" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
