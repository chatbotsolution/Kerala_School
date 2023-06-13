<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="rptHostAdmnList.aspx.cs" Inherits="Hostel_rptHostAdmnList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Hostel Student Details
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <table width="100%" border="0" cellspacing="2" cellpadding="2">
                <tr>
                    <td class="tbltxt" style="width: 55px">
                        Session
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td style="width: 100px">
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                           TabIndex="1">
                        </asp:DropDownList>
                    </td>
                    <td class="tbltxt" style="width: 50px">
                        Class
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td width="120">
                        <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="true" 
                            CssClass="vsmalltb" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                    <td class="tbltxt" style="width: 45px">
                        Gender
                    </td>
                    <td class="tbltxt" style="width: 5px">
                        :
                    </td>
                    <td class="tbltxt" style="width: 100px">
                        <asp:DropDownList ID="drpGender" runat="server" CssClass="vsmalltb" TabIndex="4">
                            <asp:ListItem>--All--</asp:ListItem>
                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                     <td class="tbltxt" style="width: 45px">
                        Status
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td class="tbltxt">
                        <asp:DropDownList ID="drpStatus" runat="server" CssClass="vsmalltb" TabIndex="7">
                        <asp:ListItem>--All--</asp:ListItem>
                            <asp:ListItem>Active</asp:ListItem>
                            <asp:ListItem>Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td colspan="12" class="tbltxt">
                        <asp:Button ID="btngo" runat="server" OnClick="btngo_Click" Text="Search" ToolTip="Click to Search Student Details"
                            TabIndex="8" />&nbsp;
                        <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                            TabIndex="9" />&nbsp; 
                                <asp:Button ID="btnprint" runat="server" Visible="false" Text="Print" TabIndex="10"
                                    OnClick="btnprint_Click" Width="60px" />
                            
                    </td>
                </tr>
            </table>
             <div runat="server" visible="false" id="Tr1">
                <asp:Label ID="lblRecCount" runat="server" Text="Label"></asp:Label>
            </div>
             <div>
            </div>
            <div id="trgrd" runat="server" style="height: 400px; width: 1000px; overflow: scroll;">
                <asp:Label ID="lblreport" runat="server"></asp:Label></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExpExcel" />
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
