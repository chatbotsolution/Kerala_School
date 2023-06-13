<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptStudFineLedger.aspx.cs" Inherits="Reports_rptStudFineLedger" %>

<asp:Content ID="contentRemarkReport" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<asp:UpdatePanel id="UpdatePanel1" runat="server">--%>

    <table width="100%" style="height:340px;">
      <TR>
                <TD vAlign="top" align="center" bgColor=#dfdfdf colspan="3" style="width:100%;">
                <STRONG>Fine Ledger</STRONG> 
                </TD>
            </TR>
       
        
        <tr>
        <td valign="top" colspan="5">
            <table width="100%" >
       
                
            <tr>
                <td colspan="2" align="left" style="height: 24px">
                    
                </td>
                <td align="right" style="height: 24px">
                    &nbsp; &nbsp;
                    
                    <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" 
                        Text="Export to Excel" TabIndex="2" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" 
                        TabIndex="3" />
                </td>
            </tr>
            
        </table>
        </td>
        </tr>
        <tr>
            <td  align="center" valign="top">
                <asp:GridView ID="grdDefaultersReport" runat="server" 
                    AutoGenerateColumns="false" OnRowCommand="grdDefaultersReport_RowCommand" 
                    TabIndex="1" >
                    <Columns>
                        <asp:TemplateField HeaderText="Admission No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkAdmissionNo" runat="server" text='<%#Eval("AdmnNo")%>' CommandArgument='<%#Eval("TransNo")%>' CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Student Name">
                            <ItemTemplate>
                                <asp:Label ID="lblAdmnDate" runat="server" Text='<%#Eval("FullName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Class">
                            <ItemTemplate>
                                <asp:Label ID="lblFullName" runat="server" Text='<%#Eval("ClassName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Session">
                            <ItemTemplate>
                                <asp:Label ID="lblFatherName" runat="server" Text='<%#Eval("SessionYear") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblClass" runat="server" Text='<%#Eval("TransAmount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        
        <tr>
            <td align="center" valign="top">
                <asp:Label ID="lblReport" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
<%--</asp:UpdatePanel>--%>
</asp:Content>
