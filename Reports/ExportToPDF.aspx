<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="ExportToPDF.aspx.cs" Inherits="Reports_ExportToPDF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnExpWord" runat="server" onclick="btnExpWord_Click" 
        Text="Export Word" />
&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnExpExcel" runat="server" onclick="btnExpExcel_Click" 
        Text="Export Excel" />
&nbsp;&nbsp;
    <asp:Button ID="btnExpPDF" runat="server" onclick="btnExpPDF_Click" 
        Text="EXport PDF Grid" />
&nbsp;<asp:Button ID="btnExpPDFString" runat="server" 
        onclick="btnExpPDFString_Click" Text="Export PDF String" />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:GridView ID="gv" runat="server">
    </asp:GridView>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    
</asp:Content>

