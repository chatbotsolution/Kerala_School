<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="LoanPremRecList.aspx.cs" Inherits="HR_LoanPremRecList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" language="javascript">

        function CnfDelete() {

            if (confirm("You are going to delete this Loan. Do you want to continue ?")) {

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
            Loan Recovery (Premature) List</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                    <table width="80%">
                <tr id="trMsg" runat="server">
                    <td align="center">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr style="background-color: #D3E7EE;" align="left" valign="baseline">
                    <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                        color: #000; border: 1px solid #333; background-color: Transparent;">
                        Select an Employee&nbsp;:&nbsp;<asp:DropDownList ID="drpEmp" runat="server" 
                            TabIndex="1">
                        </asp:DropDownList>
                        <asp:Button ID="btnSearch" runat="server" Text="Search"
                            onfocus="active(this);" onblur="inactive(this);" TabIndex="2" 
                            onclick="btnSearch_Click" />
                        <asp:Button ID="btnNewRec" runat="server" Text="Recover Loan (Premature)"
                            onfocus="active(this);" onblur="inactive(this);" TabIndex="3" 
                            onclick="btnNewRec_Click" />
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblRecords" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdLoan" runat="server" AutoGenerateColumns="False" Width="100%"
                             AllowPaging="true" PageSize="20"
                            onpageindexchanging="grdLoan_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" AlternateText="Delete" ImageUrl="~/images/icon_delete.gif"
                                            ToolTip="Click to Delete Loan Revovery" OnClientClick="return CnfDelete();" onclick="btnDelete_Click" />
                                        <asp:HiddenField ID="hfPRId" runat="server" Value='<%#Eval("PR_Id")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                    <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="TransDtStr" HeaderText="Recovery Date/Month">
                                    <HeaderStyle HorizontalAlign="Left" Width="140px" />
                                    <ItemStyle HorizontalAlign="Left" Width="140px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Employee Name">
                                    <ItemTemplate>
                                        <%#Eval("EmpName")%>
                                        <asp:HiddenField ID="hfEmpId" runat="server" Value='<%#Eval("EmpId")%>' />
                                        <asp:HiddenField ID="hfGenLedgerId" runat="server" Value='<%#Eval("GenledgerId")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:F2}">
                                    <HeaderStyle HorizontalAlign="Right" Width="100px" />
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="LoanStats" HeaderText="Status">
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
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

