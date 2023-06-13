<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpAttndnceModify.aspx.cs" Inherits="HR_EmpAttndnceModify" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Modify Staff Attendance</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>

    <table width="100%">
        <tr>
            <td align="center">
            <fieldset>
                Date&nbsp;:&nbsp;<asp:TextBox ID="txtDate" runat="server" Width="100px" AutoPostBack="True"
                    OnTextChanged="txtDate_TextChanged" ReadOnly="True"></asp:TextBox>
                     <rjs:PopCalendar ID="dtpDate" runat="server" Control="txtDate" AutoPostBack="True" To-Today="true"
                                    Format="dd mmm yyyy" OnSelectionChanged="dtpDate_SelectionChanged"></rjs:PopCalendar>
                &nbsp; Shift &nbsp; : &nbsp;<asp:DropDownList runat="server" ID="drpShift" 
                    AutoPostBack="true" onselectedindexchanged="drpShift_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp; Employee &nbsp; : &nbsp;<asp:DropDownList runat="server" ID="drpEmp" 
                    AutoPostBack="true" onselectedindexchanged="drpEmp_SelectedIndexChanged">
                </asp:DropDownList>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td align="center">
            <fieldset>
                In Time &nbsp; : &nbsp;<asp:TextBox ID="txtInTime" runat="server" Width="40px" 
                     MaxLength="4" Text="0000" onkeypress="return blockNonNumbers(this, event, true, true);" />
                     <asp:HiddenField runat="server" ID="hfIn" Value="" />
                Out Time &nbsp; : &nbsp;<asp:TextBox ID="txtOutTime" runat="server"  Width="40px"
                     MaxLength="4" Text="0000" onkeypress="return blockNonNumbers(this, event, true, true);" />
                     <asp:HiddenField runat="server" ID="hfOut" Value='' />
                &nbsp;
                Status&nbsp;:&nbsp;<asp:DropDownList ID="drpStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpStatus_SelectedIndexChanged">
                                                    <asp:ListItem Value="P">Present</asp:ListItem>
                                                    <asp:ListItem Value="A">Absent</asp:ListItem>
                                                    <asp:ListItem Value="DL">Duty Leave</asp:ListItem>
                                                    <asp:ListItem Value="Off">Off</asp:ListItem>
                                                    <asp:ListItem Value="Tour">Tour</asp:ListItem>
                                                </asp:DropDownList>
               &nbsp;
               Remarks&nbsp;:&nbsp;
                <asp:TextBox ID="txtRemarks" runat="server" Width="200px"></asp:TextBox>
                &nbsp;
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" Enabled="false"
                    onclick="btnSubmit_Click" />
                    </fieldset>
            </td>
        </tr>
        <tr>
        <td align="center">
            <asp:Label ID="lblMsg" runat="server" Text="Label"></asp:Label></td>
        </tr>
    </table>
</asp:Content>
