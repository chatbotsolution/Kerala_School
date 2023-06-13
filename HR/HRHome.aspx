<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="HRHome.aspx.cs" Inherits="HR_HRHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_home.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Home</h2>
    </div>
    <div style="font-family: 'Comic Sans MS', cursive; font-size: 92px; text-align: center;
        color: #e7e7e7; padding-top: 20px;">
        HR & Pay Roll
    </div>
    <div style="font-family: 'Comic Sans MS'; font-size: 20px; text-align: center; color: red;
        padding-top: 20px;">
        <asp:Label ID="lblMsg" runat="server" Width="500px"></asp:Label></div>
</asp:Content>

