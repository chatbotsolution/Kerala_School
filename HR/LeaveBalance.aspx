<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveBalance.aspx.cs" Inherits="HR_LeaveBalance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function CnfSubmit() {

            if (confirm("Do you want to continue ?")) {

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
                        Balance Leave Entry</h2>
                </div>
            </div>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="95%">
                <tr style="background-color: #D3E7EE;">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        Select an Employee&nbsp;:&nbsp;<asp:DropDownList ID="drpEmp" runat="server" TabIndex="1"
                            AutoPostBack="True" OnSelectedIndexChanged="drpEmp_SelectedIndexChanged">
                        </asp:DropDownList> 
                        &nbsp;&nbsp;
                        <asp:Button ID="btnBack" runat="server" Text="Back To HR Initialization" 
                            onclick="btnBack_Click" />
                    </td>
                </tr>
                <tr id="trMsg" runat="server" style="padding: 2px;">
                    <td align="center">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr id="row1" runat="server" visible="false">
                    <td>
                        <table width="95%">
                            <tr>
                                <td>
                                    <b>Enter Balance Leave as on</b>
                                    <asp:Label ID="lblFinYr" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdLeave" runat="server" AutoGenerateColumns="False" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Leave Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLeaveCode" Text='<%#Eval("LeaveCode")%>' runat="server"></asp:Label>
                                                    (<%#Eval("LeaveDesc")%>)
                                                    <asp:HiddenField ID="hfLeaveDtlsId" runat="server" Value='<%#Eval("LeaveDtlsId")%>' />
                                                    <asp:HiddenField ID="hfLeaveId" runat="server" Value='<%#Eval("LeaveId")%>' />
                                                    <asp:HiddenField ID="hfDaysAvailed" runat="server" Value='<%#Eval("AvlDays")%>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40%" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance Leave">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtBalance" runat="server" Width="80px" TabIndex="2" Text='<%#Eval("BalanceLeave")%>'
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
                                <td align="right">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" TabIndex="3"
                                        onfocus="active(this);" onblur="inactive(this);" OnClientClick="return CnfSubmit();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
