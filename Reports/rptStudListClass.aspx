<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptStudListClass.aspx.cs" Inherits="Reports_rptStudListClass" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>
    
<asp:Content ID="contentRemarkReport" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<asp:UpdatePanel id="UpdatePanel1" runat="server">--%>

    <table width="100%" style="height:340px;">
      <TR>
                <TD vAlign="top" align="center" bgColor=#dfdfdf colspan="3" style="width:100%;">
                <STRONG>Student Details</STRONG> 
                </TD>
            </TR>
       
        
        <tr>
        <td valign="top" colspan="5">
        <table width="100%" >
       
                <tr>
                <td colspan="4" align="left" style="height: 26px">
                    <asp:Label ID="lblClasses" runat="server" Text="Class" ></asp:Label>
                   <asp:DropDownList ID="drpClasses" runat="server" AutoPostBack="True"  
                        Width="116px" TabIndex="1">
                    </asp:DropDownList>
                    &nbsp; 
                    <asp:Label ID="lblSession" runat="server" Text="Session" ></asp:Label>
                    <asp:TextBox ID="txtSession" runat="server" Width="100px" TabIndex="2"></asp:TextBox>
                    <asp:Label ID="lblStudentDropDown" runat="server" Text="Student Name" Visible="False" ></asp:Label>
                    <asp:DropDownList ID="drpStudentName" runat="server" Visible="False" 
                        TabIndex="3">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    
                </td>
                <td align="right">
                    &nbsp; &nbsp;&nbsp;
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" 
                        TabIndex="4" />
                    <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" 
                        Text="Export to Excel" TabIndex="6" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" TabIndex="7" />
                </td>
            </tr>
            
        </table>
        </td>
        </tr>
        <tr>
            <td style="height:65%;" align="center" valign="top">
                <asp:GridView ID="grdStudentMasterReport" runat="server" 
                    AutoGenerateColumns="false" TabIndex="5" >
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblAdmnNo" runat="server" Text='<%#Eval("AdmnNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblAdmnDate" runat="server" Text='<%#Eval("AdmnDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblFullName" runat="server" Text='<%#Eval("FullName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblFatherName" runat="server" Text='<%#Eval("FatherName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblClass" runat="server" Text='<%#Eval("ClassName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblSession" runat="server" Text='<%#Eval("Session") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblPresentAddress" runat="server" Text='<%#Eval("PresentAddress") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        
        <tr>
            <td align="center" valign="top">
                <asp:Label ID="lblReport" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
<%--</asp:UpdatePanel>--%>
</asp:Content>

