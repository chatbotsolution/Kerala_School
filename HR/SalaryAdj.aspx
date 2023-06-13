<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="SalaryAdj.aspx.cs" Inherits="HR_SalaryAdj" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function cnf() {

            if (confirm("Are you sure to Save ?")) {
                return true;
            }
            else {

                return false;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <div style="float: left; width: 250px;">
            <h2>
                Salary Modification/Adjustment</h2>
        </div>
        <div style="float: left; padding-top: 3px;">
            <strong>
                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></strong>
        </div>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="95%">
        <tr>
            <td>
                <div style="background-color: #666; padding: 2px;">
                    <div style="background-color: #FFF; padding: 10px;">
                        <strong>Salary Year&nbsp;:&nbsp;</strong><asp:DropDownList ID="drpYear" runat="server"
                            TabIndex="1" onselectedindexchanged="drpYear_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <strong>Salary Month&nbsp;:&nbsp;</strong><asp:DropDownList ID="drpMonth" runat="server"
                            TabIndex="2" onselectedindexchanged="drpMonth_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="JAN" Value="JAN"></asp:ListItem>
                            <asp:ListItem Text="FEB" Value="FEB"></asp:ListItem>
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
                        </asp:DropDownList>
                        <strong>Designation&nbsp;:&nbsp;</strong><asp:DropDownList ID="drpDesignation" runat="server"
                            OnSelectedIndexChanged="drpDesignation_SelectedIndexChanged" AutoPostBack="True"
                            TabIndex="3">
                        </asp:DropDownList>
                        <strong>Employee&nbsp;:&nbsp;</strong><asp:DropDownList ID="drpEmployee" runat="server"
                            Width="200px" TabIndex="4" OnSelectedIndexChanged="drpEmployee_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" onfocus="active(this);"
                            onblur="inactive(this);" TabIndex="5" Visible="false"/>
                            <br />
                                *Modify salary of an employee if the salary has not been paid for the selected month.
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div style="background-color: #FFF;">
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="2">
                                Basic Salary + D.A :
                                <asp:TextBox ID="txtBasic" runat="server" TabIndex="3" Width="80px" MaxLength='20'
                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                    onkeypress="return blockNonNumbers(this, event, true, false);" Text="0"></asp:TextBox>
                                    &nbsp;
                                <asp:Label ID="lblClaimDesc" runat="server" Text="Leave Claim:" Visible="false"></asp:Label>
                                
                                <asp:TextBox ID="txtClaimAmt" runat="server" Text="0" Visible="false"></asp:TextBox>
                                
                                &nbsp;
                                <asp:Label ID="lblEncashDesc" runat="server" Text="Leave Encashment:" Visible="false"></asp:Label>
                                
                                <asp:TextBox ID="txtEncshAmt" runat="server" Text="0" Visible="false"></asp:TextBox>
                                
                                
                                <asp:Label ID="lblDis" runat="server" Text="Loss of Pay:" Visible="false"></asp:Label>
                                
                                <asp:TextBox ID="txtDischLoss" runat="server" Text="0" Visible="false"></asp:TextBox>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                            
                                <asp:GridView ID="grdEmpAlw" runat="server" AutoGenerateColumns="False" Width="400px"
                                    EmptyDataText="" TabIndex="4">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Allowance">
                                            <ItemTemplate>
                                               <%#Eval("Allowance")%>
                                               <asp:HiddenField ID="hfAlwId" runat="server" Value='<%#Eval("AllowanceId")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAlwAmt" runat="server" Width="80px" TabIndex="5" MaxLength='20'
                                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';} onTextFocus();"
                                                    onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("Amount")%>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            <br />
                                <asp:GridView ID="grdEmpDed" runat="server" AutoGenerateColumns="False" Width="400px"
                                    EmptyDataText="" TabIndex="4">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Deduction">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDedDtls" runat="server" Text='<%#Eval("DedDetails")%>'></asp:Label>
                                                
                                                <asp:HiddenField ID="hfDedId" runat="server" Value='<%#Eval("DedTypeId")%>' />
                                                <asp:HiddenField ID="hfAccId" runat="server" Value='<%#Eval("AcctsHeadId")%>' />
                                                <asp:HiddenField ID="hfArrer" runat="server" Value='<%#Eval("Arrer")%>' />
                                                <asp:HiddenField ID="hfDedDtlsOrgnl" runat="server" Value='<%#Eval("DedDetailsDup")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Share">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEmpAmt" runat="server" Width="80px" TabIndex="5" MaxLength='20'
                                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';} onTextFocus();"
                                                    onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("EmpShare")%>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employer Share">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEmplrAmt" runat="server" Width="80px" TabIndex="5" MaxLength='20'
                                                    onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                    onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("EmployrShare")%>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding: 5px; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                                color: #000; border: 1px solid #333; background-color: Transparent; width: 400px">
                                Loan Amount : <asp:Label ID="lblLoan" runat="server" Text="0"></asp:Label>
                                <asp:HiddenField ID="hfSalBefChange" runat="server" />
                                <asp:HiddenField ID="hfGrossTotDisc" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="font-size: small; color: Red" colspan="2">
                                <b>Total Payable Amount&nbsp;:&nbsp;<asp:Label ID="lblPayblSal" runat="server" Text="0.00"></asp:Label></b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnCalculate" runat="server" Text="Calculate Payble Salary" Visible="false"
                                    OnClick="btnCalculate_Click" />
                                &nbsp;
                                <asp:Button ID="btnSave1" runat="server" Text="Save" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="6" OnClick="btnSave_Click" OnClientClick="return valSubmit1();" Visible="False"
                                    Width="100px" />
                                &nbsp;
                                <asp:Button ID="btnClear" runat="server" Text="Clear" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="7" Visible="False" Width="100px" OnClick="btnClear_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>


