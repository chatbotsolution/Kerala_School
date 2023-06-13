<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LoanAdvanceList.aspx.cs" Inherits="HR_LoanAdvanceList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function CnfDelete() {

            if (confirm("You are going to delete this Loan. Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=500,left = 300,top = 100');");
        }
        
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Loan/Advance List</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr id="trMsg" runat="server">
                    <td colspan="2" align="center">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr style="background-color: #D3E7EE;" align="left" valign="baseline">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        Select an Employee&nbsp;:&nbsp;<asp:DropDownList ID="drpEmp" runat="server" TabIndex="1"
                            AutoPostBack="True" OnSelectedIndexChanged="drpEmp_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                            onfocus="active(this);" onblur="inactive(this);" TabIndex="2" />
                        <asp:Button ID="btnNew" runat="server" Text="New Loan/Advance Entry" OnClick="btnNew_Click"
                            onfocus="active(this);" onblur="inactive(this);" TabIndex="3" />
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblRecords" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdLoan" runat="server" AutoGenerateColumns="False" Width="100%"
                            OnRowCommand="grdLoan_RowCommand" AllowPaging="true" PageSize="20" OnPageIndexChanging="grdLoan_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" AlternateText="Delete" ImageUrl="~/images/icon_delete.gif"
                                            ToolTip="Click to Delete Loan" CommandName="DeleteLoan" CommandArgument='<%#Eval("GenLedgerId")%>'
                                            OnClientClick="return CnfDelete();" />
                                        <asp:HiddenField ID="hfGenLedgerId" runat="server" Value='<%#Eval("GenLedgerId")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                    <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="TransDate" HeaderText="Loan/Advance Date" DataFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Employee Name">
                                    <ItemTemplate>
                                        <%#Eval("EmpName")%>
                                        <asp:HiddenField ID="hfEmpAccHeadId" runat="server" Value='<%#Eval("AccountHead")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="TransAmt" HeaderText="Amount" DataFormatString="{0:F2}">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Description" HeaderText="Description">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Details">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    <ItemTemplate>
                                        <span style="display: <%#Eval("SetDisplay1") %>"><a href="javascript:popUp('LoanAdvanceDetails.aspx?amt=<%#Eval("TransAmt")%>&emp=<%#Eval("EmpName")%>&gl_id=<%#Eval("GenLedgerId")%>')">
                                            View</a></span> <span style="display: <%#Eval("SetDisplay2") %>">Direct Loan</span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Record(s)
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


