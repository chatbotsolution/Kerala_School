<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="SalarySettlement.aspx.cs" Inherits="HR_SalarySettlement" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function valSubmit() {
            var disDate = document.getElementById("<%=txtDisDate.ClientID %>").value;
            var disReason = document.getElementById("<%=txtReason.ClientID %>").value;
            if (disDate.trim() == "") {
                alert("Enter Discharge Date");
                document.getElementById("<%=txtDisDate.ClientID %>").focus();
                return false;
            }
            else if (disReason.trim() == "") {
                alert("Enter Discharge Reason");
                document.getElementById("<%=txtReason.ClientID %>").focus();
                return false;
            }
            else {
                return cnf();
            }
        }
        function cnf() {

            if (confirm("Are you sure to Continue ?\nNote :- Check correctness of Data provided before continue.")) {

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
            Settlement on Discharge</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 90%; border: solid 1px;">
                <tr>
                    <td style="height: 10px;">
                        <strong>Designation&nbsp;:</strong>&nbsp;<asp:DropDownList AutoPostBack="true" ID="drpDesignation"
                            runat="server" OnSelectedIndexChanged="drpDesignation_SelectedIndexChanged" TabIndex="1">
                        </asp:DropDownList>
                        <strong>Employee&nbsp;:</strong>&nbsp;<asp:DropDownList ID="drpEmployee" runat="server"
                            TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="drpEmployee_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel ID="pnlSal" runat="server" Enabled="false">
                <table width="90%" cellpadding="0" cellspacing="0" style="font-size: 12px;">
                    <tr style="background-color: #00628c; color: White; font-size: 17px; font-weight: bold;
                        height: 20px;">
                        <td align="center" style="color: White; width: 100%;" colspan="2">
                            CURRENT SALARY STRUCTURE
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Basic</strong>&nbsp;:&nbsp;<asp:TextBox ID="txtPay" runat="server" Enabled="false" Text="0" Width="100px"></asp:TextBox>
                            <strong>DA</strong>&nbsp;:&nbsp;<asp:TextBox ID="txtDA" runat="server" Text="0" Enabled="false"
                                Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="90%" cellpadding="0" cellspacing="0" style="font-size: 12px;">
                    <tr>
                        <td style="height: 5px;" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 44%;" valign="top">
                            <table width="100%" cellpadding="0" cellspacing="0" style="font-size: 12px;">
                                <tr style="background-color: #00628c; color: White; font-size: 17px; font-weight: bold;
                                    height: 20px;">
                                    <td align="center" style="color: White;">
                                        ALLOWANCE DETAILS
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdAllowance" runat="server" Width="100%" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="AllowanceId" Visible="false" />
                                                <asp:BoundField HeaderText="Allowance Name" DataField="Allowance" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Amount" DataField="Amount" HeaderStyle-HorizontalAlign="Right"
                                                    DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Middle"
                                                    HeaderStyle-VerticalAlign="Middle" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No Allowances
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 2%;">
                            &nbsp;
                        </td>
                        <td style="width: 44%;" valign="top">
                            <table width="100%" cellpadding="0" cellspacing="0" style="font-size: 12px;">
                                <tr style="background-color: #00628c; color: White; font-size: 17px; font-weight: bold;
                                    height: 20px;">
                                    <td align="center" style="color: White;">
                                        DEDUCTION DETAILS
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdDeductions" runat="server" Width="100%" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="DedTypeId" Visible="false" />
                                                <asp:BoundField HeaderText="Deduction Type" DataField="DedDetails" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Employee Share" DataField="AmtFromEmp" HeaderStyle-HorizontalAlign="Right"
                                                    DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Middle"
                                                    HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Employer Share" DataField="AmtFromOrg" HeaderStyle-HorizontalAlign="Right"
                                                    DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Middle"
                                                    HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No Deductions
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;" colspan="3">
                        </td>
                    </tr>
                </table>
                <table width="90%" cellpadding="0" cellspacing="0" style="font-size: 12px;">
                    <tr>
                        <td align="left" style="width: 120px;">
                            <strong>Gross Total</strong>
                        </td>
                        <td align="left" colspan="2">
                            :&nbsp;<asp:TextBox ID="txtGrossTot" Enabled="false" runat="server" Text="0" Width="100px"></asp:TextBox>
                            <strong>OutStanding Loan</strong> :&nbsp;<asp:TextBox ID="txtTotalLoan" Enabled="false"
                                runat="server" Text="0" Width="100px"></asp:TextBox>
                            <strong>Total Deduction</strong>
                            <asp:TextBox ID="txtTotalDed" Enabled="false" runat="server" Text="0" Width="100px"></asp:TextBox>
                            <strong>Net Payable</strong>&nbsp;:&nbsp;
                            <asp:TextBox ID="txtNetPayable" Enabled="false" runat="server" Text="0" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 120px;">
                            <strong>Last Working days upto<span style="color: #FF0000">*</span>
                        </td>
                        <td align="left" colspan="2">
                            :&nbsp;</strong><asp:TextBox ID="txtDisDate" runat="server" TabIndex="3" Width="80px"
                                ReadOnly="true"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpDisDate" runat="server" Control="txtDisDate" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <strong>Discharge Reason <span style="color: #FF0000">*</span>&nbsp;:&nbsp;</strong><asp:TextBox
                                ID="txtReason" runat="server" TabIndex="4" Width="400px"></asp:TextBox>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                TabIndex="5" OnClientClick="return valSubmit();" onfocus="active(this);" onblur="inactive(this);" />
                            <asp:Button ID="btnCancel" runat="server" TabIndex="6" Text="Cancel" OnClick="btnCancel_Click"
                                onfocus="active(this);" onblur="inactive(this);" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <div id="divMsg" runat="server" style="width: 100%; height: 20px; text-align: center;">
                <asp:Label runat="server" ID="lblMsg" Font-Size="12px" ForeColor="White" Font-Bold="true"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

