<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="Fine_Master.aspx.cs" Inherits="Library_Fine_Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <%--<div>--%>
 <table border="0" cellspacing="0" cellpadding="0" class="tbltxt">
 

 <tr>
<td style="padding-left: 120px;"  >
                <div style="float: left;">
                    <fieldset style="width: 400px;">
                        <legend>Fine Master</legend>
                        <table class="tbltxt">

                        <tr>
 
 <td colspan="2">
                                <asp:RadioButtonList ID="rdbtnlstUsertype" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                    CssClass="tbltxt" 
                                    onselectedindexchanged="rdbtnlstUsertype_SelectedIndexChanged">
                                    <asp:ListItem Text="Staff" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Student" Value="1" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
 
 </tr>

 <tr>
                            <td >
                                Class :&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="true"
                                    CssClass="largetb" >
                                    <asp:ListItem Text="---All---" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                Member :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMemberId" runat="server" AutoPostBack="true" CssClass="largetb"
                                    >
                                </asp:DropDownList>
                                &nbsp;
                            </td>
                        </tr>
      




                            <tr>
                                <td>
                                    Allowed Days :&nbsp;&nbsp;
                                    <asp:TextBox ID="txtAllowDay" runat="server" Width="50px" MaxLength="4" CssClass="smalltb"
                                        onkeypress="return blockNonNumbers(this, event,false, false);"></asp:TextBox>
                                    <span style="color: Red; font-size: small;">*</span>
                                </td>
                                <td>
                                    Allowed Books :&nbsp;&nbsp;
                                    <asp:TextBox ID="txtAllowBook" runat="server" Width="50px" MaxLength="3" CssClass="smalltb"
                                        onkeypress="return blockNonNumbers(this, event,false, false);"></asp:TextBox>
                                    <span style="color: Red; font-size: small;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Is Fine Applicable :                                    
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rdbtnFineStatus" runat="server" RepeatDirection="Horizontal"
                                        CssClass="tbltxt">
                                        <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Member Fee :&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFee" runat="server" Width="100px" MaxLength="10" CssClass="smalltb"
                                        onkeypress="return blockNonNumbers(this, event,true, false);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </td>
          
</tr>

<tr>
            <td  style="padding-left: 120px;">
                <fieldset style="width: 400px;">
                    <asp:Button ID="btnSaveAddNew" runat="server" Text="Save & AddNew" Font-Bold="True"
                        OnClientClick="return isValid();" Font-Size="8pt" Width="120px" 
                        onclick="btnSaveAddNew_Click"  />&nbsp;
                    <asp:Button ID="btnSaveGotoList" runat="server" Text="Save & GotoList" Font-Bold="True"
                        OnClientClick="return isValid();" Font-Size="8pt" Width="120px" 
                        onclick="btnSaveGotoList_Click"  />&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Clear" Font-Bold="True" Font-Size="8pt"
                        Width="60px" />&nbsp;
                    <asp:Button ID="btnShow" runat="server" Text="Back" Font-Bold="True" Font-Size="8pt"
                        Width="70px"  />
                </fieldset>
            </td>
            </tr>



            
            </table>
            
           <%-- </div>--%>

</asp:Content>

