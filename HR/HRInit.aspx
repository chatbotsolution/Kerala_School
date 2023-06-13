<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="HRInit.aspx.cs" Inherits="HR_HRInit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <div style="float: left;">
            <h2>
                HR Initialization
            </h2>
        </div>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="95%">
        <tr style="background-color: #D3E7EE;">
            <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                color: #000; border: 1px solid #333; background-color: Transparent;" valign="baseline">
                Steps To Follow:-
                <ol type="1" style="color: Red">
                    <li>Initialize Outstanding Employee Loan/Advance Amount for employees as on 31st March(ie including EMI of March) </li>
                    <li>If Required modify the Loan/Advance EMI Amount</li>
                    <li>Initialize Outstanding Employee Salary (including deductions) as on 1st April</li>
                    <li>Verify and Finalize the above Initilized Salary</li>
                    <li>Initialize Outstanding Employee Leave</li>
                </ol>
            </td>
          </tr>
          <tr style="background-color: #D3E7EE;">
            <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                color: #000; border: 1px solid #333; background-color: Transparent;">
                Initilize Outstanding Employee Loan/Advance Amount :
                <asp:Button ID="btnLoanInit" runat="server" Text="Initialize" OnClick="btnLoanInit_Click" />
                &nbsp;
                <asp:Label ID="lblLoan" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr style="background-color: #D3E7EE;">
            <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                color: #000; border: 1px solid #333; background-color: Transparent;">
                Modify Loan/Advance EMI Amount :
                <asp:Button ID="btnLoanMod" runat="server" Text="Modify" OnClick="btnLoanMod_Click" />
                &nbsp;
                <asp:Label ID="lblEmi" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr style="background-color: #D3E7EE;">
            <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                color: #000; border: 1px solid #333; background-color: Transparent;">
                Initilize Outstanding Employee Salary :
                <asp:Button ID="btnSalaryInit" runat="server" Text="Initialize" OnClick="btnSalaryInit_Click" />
                &nbsp;
                <asp:Label ID="lblSal" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr style="background-color: #D3E7EE;">
            <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                color: #000; border: 1px solid #333; background-color: Transparent;">
                Finalize Initilized Salary :
                <asp:Button ID="btnSalaryFin" runat="server" Text="Finalize" OnClick="btnSalaryFin_Click" />
                &nbsp;
                <asp:Label ID="lblFin" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr style="background-color: #D3E7EE;">
            <td align="left" style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;
                color: #000; border: 1px solid #333; background-color: Transparent;">
                Initilize Outstanding Employee Leave:
                <asp:Button ID="btnLeave" runat="server" Text="Initialize" Width="67px" OnClick="btnLeave_Click" />
                &nbsp;
                <asp:Label ID="lblLeave" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

