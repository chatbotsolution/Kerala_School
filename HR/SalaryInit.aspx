<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="SalaryInit.aspx.cs" Inherits="HR_SalaryInit" %>

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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_fm.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <div style="float: left;">
                    <h2>
                        Salary Initialization (before Computerisation)</h2>
                </div>
            </div>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="100%">
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        Guidelines:-
                        <ol type="1" style="color: Red">
                            <li>Enter Employee wise OutStanding Authorized Salary upto the Month of March 
                                (including Salary of that month)</li>
                            <li>Enter deductions both employee and employer share forthe selected employee.</li>
                            <li>Verify the loan/advance amount if any and check the payble salary.</li>
                            <li>Click on &quot;Save&quot; to save the salary.</li>
                        </ol>
                    </td>
                </tr>
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        <%--Select a Designation&nbsp;:&nbsp;<asp:DropDownList ID="drpDesignation" runat="server"
                            TabIndex="1" AutoPostBack="True" 
                            onselectedindexchanged="drpDesignation_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" onfocus="active(this);" onblur="inactive(this);"
                            TabIndex="3" OnClick="btnSearch_Click" />--%>
                        Select an Employee&nbsp;:&nbsp;<asp:DropDownList ID="drpEmp" runat="server" TabIndex="1"
                        AutoPostBack="True" OnSelectedIndexChanged="drpEmp_SelectedIndexChanged">
                        </asp:DropDownList>
                        &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                        <asp:Button ID="btnBack" runat="server" Text="Back To HR Initialization" 
                            onclick="btnBack_Click" />
                        &nbsp;
                        <br />
                        Total Authorized Salary (Including deductions Only Employee share shown below) 
                        upto the Month of
                        <asp:DropDownList ID="drpMonth" runat="server" OnSelectedIndexChanged="drpMonth_SelectedIndexChanged"
                            AutoPostBack="true" Enabled="false" TabIndex="2">
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
                       <%-- &nbsp;(including Salary of
                        <asp:Label ID="lblMonth1" runat="server"></asp:Label>)&nbsp;:&nbsp;--%>
                            <asp:TextBox ID="txtGrossSal"
                            runat="server" TabIndex="3" Width="80px" MaxLength='20' onblur="if (this.value == '') {this.value = '0';}"
                            onfocus="if(this.value == '0') {this.value = '';}" onkeypress="return blockNonNumbers(this, event, true, false);"
                            Text="0" onkeyup="TotalAmt1();"></asp:TextBox>
                        <%--<asp:Button ID="btnCalculate1" runat="server" Text="Calculate Total" onfocus="active(this);"
                            onblur="inactive(this);" TabIndex="3" Visible="False" OnClick="btnCalculate_Click" />--%>
                        &nbsp; &nbsp; &nbsp; &nbsp; <asp:Label ID="lblPayblSal" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr id="trMsg" runat="server">
                    <td align="center">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdEmpDed" runat="server" AutoGenerateColumns="False" Width="400px"
                            EmptyDataText="No Employee Available" TabIndex="4">
                            <Columns>
                                <asp:TemplateField HeaderText="Deduction">
                                    <ItemTemplate>
                                        <%#Eval("DedDetails")%>
                                        <asp:HiddenField ID="hfDedId" runat="server" Value='<%#Eval("DedTypeId")%>' />
                                        <asp:HiddenField ID="hfAccId" runat="server" Value='<%#Eval("AcctsHeadId")%>' />
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
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;width:400px">
                        <asp:Label ID="lblLoan" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnCalculate" runat="server" Text="Calculate Payble Salary" Visible="false"
                            onclick="btnCalculate_Click" />
                            &nbsp;
                         <asp:Button ID="btnSave1" runat="server" Text="Save" onfocus="active(this);" onblur="inactive(this);"
                            TabIndex="6" OnClick="btnSave_Click" OnClientClick="return valSubmit1();" Visible="False"
                            Width="100px" />
                         &nbsp;
                         <asp:Button ID="btnClear" runat="server" Text="Clear" onfocus="active(this);" onblur="inactive(this);"
                            TabIndex="7" Visible="False" Width="100px" onclick="btnClear_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger  ControlID="drpEmp" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

