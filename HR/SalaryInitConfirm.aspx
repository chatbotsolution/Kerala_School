<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="SalaryInitConfirm.aspx.cs" Inherits="HR_SalaryInitConfirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <div style="float: left;">
            <h2>
                Finalize Salary Initialization
            </h2>
        </div>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="95%">
        <tr style="background-color: #D3E7EE;">
            <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                color: #000; border: 1px solid #333; background-color: Transparent;">
                Guidelines:-
                <ol type="1" style="color: Red">
                    <li>Previously initialized salary is displayed here if not already generated. </li>
                    <li>Verify all the payble salary and total deduction of each employee.</li>
                    <li>If Ok then click on &quot;Finalize Initialized Salary&quot; button to generate salary.</li>
                    <li>Warnning : Once Finalized/Generated the changes cannot be reverted.</li>
                </ol>
            </td>
        </tr>
        <tr id="trMsg" runat="server">
            <td align="center">
                <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblCount" runat="server" Text=""></asp:Label>
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
        <tr>
            <td align="left">
                <asp:Button ID="btnBack" runat="server" Text="Back To HR Initialization" 
                            onclick="btnBack_Click" />
                &nbsp;&nbsp;&nbsp;            
                <asp:Button ID="btnReInit" runat="server" Text="Re-Initialize Salary" OnClick="btnReInit_Click" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnFinalize" runat="server" Text="Finalize Initialized Salary" Visible="false"
                    OnClick="btnFinalize_Click" />
                    &nbsp;&nbsp;&nbsp;
                 
                <asp:HiddenField ID="hfMonth" runat="server" />
                <asp:HiddenField ID="hfYear" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
