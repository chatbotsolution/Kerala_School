﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Inventory_Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_home.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Home</h2>
    </div>
    <div>
        <asp:Label ID="lblReport" runat="server" Text="" CssClass="tbltxt"></asp:Label></div>
    <div style="font-family: 'Comic Sans MS', cursive; font-size: 92px; text-align: center;
        color: #e7e7e7; padding-top: 20px;">
        Inventory Management System</div>
</asp:Content>
