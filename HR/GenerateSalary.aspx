<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="GenerateSalary.aspx.cs" Inherits="HR_GenerateSalary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        /* Added by bidhu on 12 May 2014 */
        function valGenreate() {
            var year = document.getElementById("<%=drpYear.ClientID %>").selectedIndex;
            if (year == 0) {
                alert("Select a Year");
                document.getElementById("<%=drpYear.ClientID %>").focus();
                return false;
            }
            else {
                return CnfGenerate();
            }
        }

        function valShow() {
            var year = document.getElementById("<%=drpYear.ClientID %>").selectedIndex;

            if (year == 0) {
                alert("Select a Year");
                document.getElementById("<%=drpYear.ClientID %>").focus();
                return false;
            }
        }

        function valDelete() {
            var year = document.getElementById("<%=drpYear.ClientID %>").selectedIndex;

            if (year == 0) {
                alert("Select a Year");
                document.getElementById("<%=drpYear.ClientID %>").focus();
                return false;
            }
            else {
                return CnfDelete();
            }
        }

        function CnfGenerate() {

            if (confirm("Are you sure to Generate Salary for this Month. Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }

        function CnfDelete() {

            if (confirm("You are going to Delete Monthly Salary. Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }

        function CnfCancel() {

            if (confirm("You are going to delete this Record. Do you want to continue ?")) {

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
        <h2>
            Generate Salary</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <div style="width: 99%; background-color: #666; padding: 2px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="background-color: #FFF; padding: 10px;">
                    <table style="width: 100%;">
                        <tr>
                            <td style="height: 10px;">
                                <strong>Salary Year&nbsp;:</strong>&nbsp;<asp:DropDownList ID="drpYear" runat="server"
                                    TabIndex="1" OnSelectedIndexChanged="drpYear_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <strong>Salary Month&nbsp;:</strong>&nbsp;<asp:DropDownList ID="drpMonth" runat="server"
                                    TabIndex="2" OnSelectedIndexChanged="drpMonth_SelectedIndexChanged" AutoPostBack="true">
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
                                &nbsp;
                                <asp:RadioButtonList ID="rdbtnlstGenSal" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="4" RepeatLayout="Flow">
                                    <asp:ListItem Text="With Deduction For Excess Leave" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Without Deduction For Excess Leave" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnGenSalary" runat="server" Text="Generate Salary" OnClick="btnGenSalary_Click"
                                    OnClientClick="return valGenreate();" onfocus="active(this);" onblur="inactive(this);"
                                    TabIndex="5" />
                                <asp:Button ID="btnShow" runat="server" Text="Show Generated Salary" OnClientClick="return valShow();"
                                    onfocus="active(this);" onblur="inactive(this);" TabIndex="6" OnClick="btnShow_Click" />
                                <asp:Button ID="btnDelMonthSal" runat="server" Text="Delete Monthly Salary" OnClick="btnDelMonthSal_Click"
                                    OnClientClick="return valDelete();" TabIndex="7" onfocus="active(this);" onblur="inactive(this);" />
                                <asp:Button ID="btnPrintPayRoll" runat="server" OnClick="btnPrintPayRoll_Click" Text="Print Pay Roll"
                                    onfocus="active(this);" onblur="inactive(this);" TabIndex="8" Visible="false" />
                                <asp:Button ID="btnPrintSalSlip" runat="server" OnClick="btnPrintSalSlip_Click" Text="Print Salary Slip"
                                    onfocus="active(this);" onblur="inactive(this);" TabIndex="9" Visible="false" />
                            </td>
                        </tr>
                        <tr id="trMsg" runat="server">
                            <td align="center">
                                <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <td style="color:Red;font-size:13px;font-weight:bold">
                            Warnning: Deleting the Initialized salary will delete all initialized details of employees 
                            and reinitialization will be required to generate salary!!
                        </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblRecords" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="grdSalary" runat="server" AutoGenerateColumns="False" 
                                    Width="100%" onrowdatabound="grdSalary_RowDataBound">
                                    <Columns>
                                        <asp:BoundField HeaderText="Employee Name" DataField="EmpName" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200px" HeaderStyle-Width="200px" />
                                        <asp:BoundField HeaderText="Designation" DataField="Designation" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
                                        <asp:BoundField HeaderText="Basic Salary" DataField="Pay" DataFormatString="{0:F2}"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="100px"
                                            HeaderStyle-Width="100px" />
                                        <asp:BoundField HeaderText="Paid Days" DataField="PaidDays" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="50px" HeaderStyle-Width="50px" />
                                        <asp:TemplateField HeaderText="Gross Total">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGross" runat="server" Text='<%#Eval("GrossTot","{0:F2}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                            <HeaderStyle HorizontalAlign="Right" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Total Deduction" DataField="TotalDeduction" DataFormatString="{0:F2}"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="100px"
                                            HeaderStyle-Width="100px" />
                                        <asp:TemplateField HeaderText="Old Arrear/Prev. Month Pending">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrevAmt" runat="server" Text="0"></asp:Label>
                                                <asp:HiddenField ID="hfEmpId" runat="server" Value='<%#Eval("EmpId")%>' />
                                                <asp:HiddenField ID="hfGross" runat="server" Value='<%#Eval("GrossTot")%>' />
                                                <asp:HiddenField ID="hfDed" runat="server" Value='<%#Eval("TotalDeduction")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                            <HeaderStyle HorizontalAlign="Right" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Salary Payable">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPayable" runat="server" Text='<%#Eval("NetSal","{0:F2}") %>'></asp:Label>
                                                <asp:HiddenField ID="hfNetSal" runat="server" Value='<%#Eval("NetSal")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                            <HeaderStyle HorizontalAlign="Right" Width="100px" />
                                        </asp:TemplateField>
                                        <%--<asp:BoundField HeaderText="Salary Payable" DataField="NetSal" DataFormatString="{0:F2}"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="100px"
                                            HeaderStyle-Width="100px" />--%>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="font-size: small; color: Red" colspan="2">
                                <b>Total Payable Amount&nbsp;:&nbsp;<asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label></b>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

