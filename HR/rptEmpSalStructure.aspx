<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="rptEmpSalStructure.aspx.cs" Inherits="HR_rptEmpSalStructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Current Salary Structure</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table width="100%" style="border: ridge 2px black;">
                            <tr style="background-color: Black;">
                                <td align="center" style="color: White;">
                                    <strong>FILTER CRITERIA</strong>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <strong>Designation&nbsp;:&nbsp;</strong>
                                    <asp:DropDownList runat="server" ID="drpDesgn" AutoPostBack="true" OnSelectedIndexChanged="drpDesgn_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;<strong>Employee&nbsp;: &nbsp;<asp:DropDownList runat="server" ID="drpEmp">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnShow" Text="Show Report" runat="server" OnClick="btnShow_Click" />&nbsp;<asp:Button
                                        ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />&nbsp;<asp:Button
                                            ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:UpdatePanel ID="updtpnl1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblReport" runat="server"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

