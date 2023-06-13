<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptDupRecieptCheque.aspx.cs" Inherits="Reports_rptDupRecieptCheque" %>

<asp:Content ID="contentRemarkReport" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<asp:UpdatePanel id="UpdatePanel1" runat="server">--%>
<ContentTemplate></ContentTemplate>
    <table width="100%" style="height:410px;">
      <TR>
                <TD vAlign="top" align="center" bgColor=#dfdfdf colspan="3" style="width:100%;">
                <STRONG>Cheque Receipt</STRONG> 
                </TD>
            </TR>
       
        
        <tr>
        <td valign="top" colspan="5">
        <table width="100%" >
       
                <tr>
                <td colspan="4" align="left">
                    <asp:Label ID="lblSession" runat="server" Text="Session" ></asp:Label>
                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True"  
                        Width="89px" OnSelectedIndexChanged="drpSession_SelectedIndexChanged" 
                        TabIndex="1" ></asp:DropDownList>
                    
                    <asp:Label ID="lblClasses" runat="server" Text="Class" ></asp:Label>
                    <asp:DropDownList ID="drpClasses" runat="server" AutoPostBack="True"  
                        Width="83px" OnSelectedIndexChanged="drpClasses_SelectedIndexChanged" 
                        TabIndex="2" ></asp:DropDownList>
                    <asp:Label ID="lblSection" runat="server" Text="Section" ></asp:Label>
                    <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True"  
                        Width="74px" OnSelectedIndexChanged="drpSection_SelectedIndexChanged" 
                        TabIndex="3" ></asp:DropDownList>
                     
                    <asp:Label ID="lblstudent" runat="server" Text="Student" ></asp:Label>
                    <asp:DropDownList ID="drpstudent" runat="server" AutoPostBack="True"  
                        Width="186px" OnSelectedIndexChanged="drpstudent_SelectedIndexChanged" 
                        TabIndex="4" ></asp:DropDownList>
                    <asp:Label ID="lbladminno" runat="server" Text="Admission No"></asp:Label>
                    <asp:TextBox ID="txtadminno"  runat="server"  Width="105px" AutoPostBack="True" 
                        OnTextChanged="txtadminno_TextChanged" TabIndex="5" ></asp:TextBox>
                   <%-- <asp:DropDownList ID="drpadminno" runat="server" AutoPostBack="True"  Width="93px" OnSelectedIndexChanged="drpadminno_SelectedIndexChanged"></asp:DropDownList>--%>
                    </td>
            </tr>
            <tr>
                <td colspan="4" align="left" style="height: 27px">
                    <asp:Label ID="lblfeetype" runat="server" Text="Fee Type"></asp:Label>
                    <asp:DropDownList ID="drpfeetype" runat="server" 
                        OnSelectedIndexChanged="drpfeetype_SelectedIndexChanged" AutoPostBack="True" 
                        TabIndex="6">
                        <asp:ListItem Text="Fee Reciept" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Additional Fee Reciept" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lblreciept" runat="server" Text="Reciept No" ></asp:Label>
                    <asp:DropDownList ID="drpReciept" runat="server" AutoPostBack="True"  
                        Width="93px" TabIndex="7" ></asp:DropDownList>
                             
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" 
                        TabIndex="8" />&nbsp;
                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" 
                        TabIndex="9" />
                    <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" 
                        Text="Export to Excel" Visible="False" Width="106px" TabIndex="10" />&nbsp;
                    &nbsp;&nbsp; &nbsp;&nbsp;
                </td>
            </tr>
            
        </table>
        </td>
        </tr>
        <tr>
        <td ></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lbldetail" runat="server"></asp:Label>    
            </td>
        </tr>
        <tr>
            <td style="height:65%;" align="center" valign="top">
                <asp:Label ID="lblReport" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
<%--</asp:UpdatePanel>--%>
</asp:Content>
