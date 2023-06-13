<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="RptIndexNoList.aspx.cs" Inherits="Reports_RptIndexNoList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Exam Index No.
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <center><table  width="96%" cellspacing="2" cellpadding="2" >
        <tr>
            <td class="tbltxt" >
                Session
            </td>
            <td class="tbltxt" style="width: 8px">
                :
            </td>
            <td style="width: 120px" >
                <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                    TabIndex="1" onselectedindexchanged="drpSession_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="tbltxt"  >
                Class
            </td>
            <td  class="tbltxt" style="width: 5px" >
                :
            </td>
            <td style="width: 93px" >
                <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="true" 
                    CssClass="vsmalltb" TabIndex="2" 
                    onselectedindexchanged="drpclass_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="tbltxt" >
                Section
            </td>
            <td  class="tbltxt" style="width: 5px">
                :
            </td>
            <td >
                <asp:DropDownList ID="ddlSection" runat="server" AutoPostBack="True"
                    CssClass="vsmalltb" TabIndex="3">
                </asp:DropDownList>
            </td>
            </tr>
            <tr>
            <td  class="tbltxt">
            From Which RollNo.</td>
             <td  class="tbltxt" style="width: 8px">
                :
            </td>
            <td >
                <asp:TextBox ID="txtFrmRoll" runat="server" TabIndex="4"></asp:TextBox>
            </td>
            <td class="tbltxt">
            To Which RollNo.</td>
             <td  class="tbltxt" style="width: 5px">
                :
            </td>
            <td >
            <asp:TextBox ID="txtToRoll" runat="server"  TabIndex="5"></asp:TextBox>
            </td>
            <td class="tbltxt">
            Sort By:
            </td>
             <td  class="tbltxt" style="width: 5px">
                :
            </td>
            <td class="tbltxt">
            <asp:DropDownList ID="drpSort" runat="server" AutoPostBack="True"
                    CssClass="vsmalltb" TabIndex="3">
                    <asp:ListItem Value="1">By RollNo</asp:ListItem>
                    <asp:ListItem Value="2">By Student Name</asp:ListItem>
                    <asp:ListItem Value="3">By Index No.</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="9" class="tbltxt" style="height: 31px">
            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return isValid();"  Text="Get Student List" ToolTip="Click to Get Student List"
                    TabIndex="6" />&nbsp;
                <asp:Button ID="btngo" runat="server" OnClick="btngo_Click" OnClientClick="return isValid();"  Text="Get Index No. List" ToolTip="Click to Get Index No. List"
                    TabIndex="6" />&nbsp;
                <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel" Enabled="false"
                    TabIndex="7" />&nbsp;
                 <asp:Button ID="btnprint" runat="server" Text="Print" TabIndex="8" Enabled="false"
                            OnClick="btnprint_Click" />
                            <div style="float:right"><asp:Label ID="lblCount" runat="server"></asp:Label></div>
                            
            </td>
        </tr>
        <tr>
        <td colspan='9' align="center">
        <asp:Label ID="lblmsg" runat="server"></asp:Label>
        </td>
        </tr>
    </table></center>
 
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExpExcel" />
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

