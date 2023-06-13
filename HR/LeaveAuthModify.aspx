<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LeaveAuthModify.aspx.cs" Inherits="HR_LeaveAuthModify" %>

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
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Authorize/Modify Leave (Individual Employee)
        </h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <fieldset style="width: 90%">
                <table width="100%">
                    <tr id="trMsg" runat="server">
                        <td colspan="2" style="height: 20px;" align="center">
                            <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Button ID="btnAuthLeave" runat="server" Text="Back to Authorize Leave"
                                onfocus="active(this);" onblur="inactive(this);" 
                                onclick="btnAuthLeave_Click"  />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline" style="height: 18px">
                            Year&nbsp;:&nbsp;<asp:Label ID="lblYear" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                            &nbsp;Select an Employee<font color="red">*</font> :&nbsp;<asp:DropDownList ID="drpEmp"
                                runat="server" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="drpEmp_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblRecCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:GridView ID="grdLeave" runat="server" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Leave Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeaveCode" runat="server" Text='<%#Eval("LeaveCode")%>'></asp:Label>
                                            (<%#Eval("LeaveDesc")%>)
                                            <asp:HiddenField ID="hfLeaveId" runat="server" Value='<%#Eval("LeaveId")%>' />
                                            <asp:HiddenField ID="hfLeaveDtlsId" runat="server" Value='<%#Eval("LeaveDtlsId")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Days Authorized">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAuthDays" runat="server" MaxLength="3" Width="40px" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                TabIndex="2" onblur="if (this.value == '') {this.value = '0';}" onfocus="if(this.value == '0') {this.value = '';}"
                                                Text='<%#Eval("AuthDays")%>'></asp:TextBox>
                                            <asp:HiddenField ID="hfAvlDays" runat="server" Value='<%#Eval("AvlDays")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnUpdateList" runat="server" Text="Update All" Visible="False" TabIndex="3"
                                OnClick="btnUpdateList_Click" onfocus="active(this);" onblur="inactive(this);" OnClientClick="return CnfSubmit();" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="drpEmp" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

