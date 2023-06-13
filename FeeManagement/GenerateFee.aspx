﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="GenerateFee.aspx.cs" Inherits="FeeManagement_GenerateFee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript">
    function CnfSave() {

        if (confirm("You are Regenerate Fee!!. Do you want to continue?")) {
            return true;
        }
        else {

            return false;
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
 <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Generate Fee</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    
    <table width="100%" border="0" cellpadding="0" cellspacing="10">
        <tr>
            <td align="left" valign="top" class="tbltxt">
                <asp:Label ID="lblSession" runat="server" Text="Session : "></asp:Label>
                <asp:DropDownList ID="drpsession" runat="server" Width="100px"
                    CssClass="vsmalltb" TabIndex="1" >
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:Label ID="lblClass" runat="server" Text="Class : "></asp:Label><span class="error">*</span>
                <asp:DropDownList ID="drpClass" runat="server" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                    Width="100px" CssClass="vsmalltb" TabIndex="2" AutoPostBack="True">
                </asp:DropDownList>
                &nbsp;&nbsp;
                      Stud Name:&nbsp;<asp:DropDownList ID="drpstudent" runat="server" AutoPostBack="True" 
                            CssClass="largetb" meta:resourcekey="drpstudentResource1" 
                    TabIndex="3" onselectedindexchanged="drpstudent_SelectedIndexChanged">
                        </asp:DropDownList>
                    &nbsp;&nbsp;
                    Student Type:&nbsp;<asp:TextBox ID="txtStudType" Text="E" runat="server" 
                    Width="30px" TabIndex="4"></asp:TextBox>
                  &nbsp;&nbsp;
               
            </td>
        </tr>
        <tr>
        <td>
        <asp:Label ID="lblerr" runat="server" CssClass="error"></asp:Label>
        </td>
        </tr>
        <tr>
            <td align="left" valign="top" class="tbltxt">
                 <asp:Button ID="btnGenFee" runat="server" Text="Re-Generate Fee" OnClientClick="return CnfSave();"
                    OnClick="btnGenFee_Click" TabIndex="6" /></td>
        </tr>
        <tr>
            <td>
             <asp:Label ID="Label1" Text="Enter Student Type 'E' for existing, 'N' for New and 'T' for Transfer Case" runat="server" CssClass="error"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>



