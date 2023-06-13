<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="FeeConsolidate.aspx.cs" Inherits="Masters_FeeConsolidate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="btnConsolidt" runat="server" Text="Consolidate Fee" 
    onclick="btnConsolidt_Click" />
    <asp:Button ID="btnInit" runat="server" Text="Consolidate Student Init" 
        onclick="btnInit_Click" />
        &nbsp;
    <asp:Label ID=lblMsg runat="server" Text=""></asp:Label>
    </asp:Content>

