<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="ChangeStudClass.aspx.cs" Inherits="Admissions_ChangeStudClass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script language="javascript" type="text/javascript">


    function isValid() {
        var studtype = document.getElementById("<%=txtStudType.ClientID %>").value;
        var studclass = document.getElementById("<%=drpClass.ClientID%>").value;
        var student = document.getElementById("<%=drpstudent.ClientID%>").value;
        var newclass = document.getElementById("<%=drpNewClass.ClientID%>").value;


        if (studclass == 0) {
            alert("Please Select Student Class");
            document.getElementById("<%=drpClass.ClientID %>").focus();
            return false;
        }

        if (student == "0") {
            alert("Please Select a Student !");
            document.getElementById("<%=drpstudent.ClientID %>").focus();
            return false;
        }
        if (studtype.trim() == "") {
            alert("Please Enter Student Type (E for Existing, N for New");
            document.getElementById("<%=txtStudType.ClientID %>").focus();
            return false;
        }

        if (newclass == 0) {
            alert("Please Select New Class");
            document.getElementById("<%=drpNewClass.ClientID %>").focus();
            return false;
        }
        else {
            return CnfSave();
        }

    }
    function CnfSave() {

        if (confirm("You are Going to modify class for the selected student!!. Do you want to continue?")) {
            return true;
        }
        else {

            return false;
        }
    }

    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Modify Student Class</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" />
        </div>
    
    <table width="100%" border="0" cellpadding="0"  >
        <tr>
            <td align="left" valign="top" class="tbltxt cnt-box">
                <div class="cnt-sec">
                <span class="ttl3"><asp:Label ID="lblSession" runat="server" Text="Session : "></asp:Label></span>
                <asp:DropDownList ID="drpsession" runat="server"   Enabled="false"
                    CssClass="vsmalltb" TabIndex="1" >
                </asp:DropDownList>
                </div>
                 <div class="cnt-sec">
                <div class="ttl3">
                <asp:Label ID="lblClass" runat="server" Text="Class : "></asp:Label><span class="error">*</span></div>
                <asp:DropDownList ID="drpClass" runat="server" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                     CssClass="" TabIndex="2" AutoPostBack="True">
                </asp:DropDownList>
                </div>
                 <div class="cnt-sec">
                <span class="ttl3">
                      Student Name:</span><asp:DropDownList ID="drpstudent" runat="server" AutoPostBack="True" 
                            CssClass="vsmalltb" meta:resourcekey="drpstudentResource1" 
                    TabIndex="3" onselectedindexchanged="drpstudent_SelectedIndexChanged">
                        </asp:DropDownList>
                        </div>
                 <div class="cnt-sec">
                <span class="ttl3">Student Type:</span>
                 <asp:TextBox ID="txtStudType" runat="server"  TabIndex="4"></asp:TextBox>    
                </div>
                <div class="cnt-sec"></div>
                <div class="cnt-sec" style="margin-top:-9px;">
                <span class="ttl3"><asp:Label ID="Label2" runat="server" Text="New Class : "></asp:Label><span class="error">*</span></span>
                <asp:DropDownList ID="drpNewClass" runat="server"
                     CssClass="vsmalltb" TabIndex="2">
                </asp:DropDownList>
                
                </div>
                 <div class="cnt-sec">
                 <span  class="ttl3" >Stream:</span>
               <asp:DropDownList ID="drpStream" runat="server" CssClass="tbltxtbox"  Enabled="false"
                     TabIndex="6">
                    <asp:ListItem Value="1">Select Stream</asp:ListItem>
                      <asp:ListItem Value="2">Arts</asp:ListItem>
                       <asp:ListItem Value="3">Commerce</asp:ListItem>
                        <asp:ListItem Value="4">BioScience</asp:ListItem>
                        <asp:ListItem Value="5">Pure Science</asp:ListItem>
                </asp:DropDownList> 

                </div>

            </td>
        </tr>
        
         
        <tr>
            <td align="left" valign="top" class="tbltxt">
            <table width="100%">
                <tr>
                    <td align="left"><asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                     TabIndex="6" OnClientClick="return isValid();" onclick="btnSubmit_Click" />&nbsp;<asp:Label ID="lblerr" runat="server" CssClass="tbltxt2" ></asp:Label></td>
                    <td align="right"> <asp:Label ID="Label1" Text="Enter Student Type 'E' for existing, 'N' for New and 'T' for Transfer Case" runat="server" CssClass="error"></asp:Label></td>
                </tr>
            </table>
                 
            </td>
        </tr>
        <tr>
            <td>
            
            </td>
        </tr>
    </table>
</asp:Content>
