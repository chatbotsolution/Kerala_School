<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="I-card.aspx.cs" Inherits="Reports_I_card" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script language="javascript" type="text/javascript">
    function valShow() {

        var Class = document.getElementById("<%=drpClass.ClientID %>").value;
        if (Class == "0") {
            alert("Please select a class !");
            document.getElementById("<%=drpClass.ClientID %>").focus;
            return true;
        }
        else {
            return true;
        }
    }
    function popUp(URL) {
        day = new Date();
        id = day.getTime();
        eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=500,left = 390,top = 184');");
    } 
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Temporary I-Card
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
  
  <table width="100%" border="0" cellspacing="2" cellpadding="2" class="cnt-box">
        <tr>
            <td width="50" class="tbltxt">
                Session :
            </td>
        <%--    <td width="5" class="tbltxt">
                :
            </td>--%>
            <td width="110">
                <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb">
                </asp:DropDownList>
            </td>
            <td class="tbltxt" style="width: 84px">
                 Class</td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="120">
                <asp:DropDownList ID="drpClass" runat="server" CssClass="vsmalltb" 
                    Height="16px" AutoPostBack="True" 
                    onselectedindexchanged="drpClass_SelectedIndexChanged">
                </asp:DropDownList></td>
             <td width="50" class="tbltxt">
                Section
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="120">
                <asp:DropDownList ID="drpSection" runat="server" CssClass="vsmalltb" Height="16px" AutoPostBack="true" OnSelectedIndexChanged="drpSection_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td width="50" class="tbltxt">
                Student
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="120">
                 <asp:DropDownList ID="drpSelectStudent" runat="server"
                      CssClass="tbltxtbox" TabIndex="4" ></asp:DropDownList>
            </td>
            <td>
                Select F/B :
            </td>
            <td width="120">
                 <asp:DropDownList ID="drpside" runat="server" CssClass="tbltxtbox" TabIndex="5" >
                      <asp:ListItem Text="-Select-" Value="0"/>
                      <asp:ListItem Text="Front" Value="1"/>
                      <asp:ListItem Text="Back" Value="2"/>
                 </asp:DropDownList>
            </td>
            </tr>
      <tr>
            <td width="50" class="tbltxt">
               Admission No :
            </td>
          <td>
              <asp:TextBox ID="txtAdmNo" runat="server"></asp:TextBox>
          </td>
          <td>
              <asp:Button ID="BtnAdmSave" runat="server" Text="ADD"  OnClick="BtnAdmSave_Click"/>
          </td>
          </tr>
      <tr>
          <td colspan="4">
          <asp:GridView ID="grddatalist" runat="server"></asp:GridView>
              </td>
      </tr>
            <tr>
            <td colspan="4">
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Show"/>
                  <%--  OnClientClick="return valShow();"--%>
                     
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
                <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                    Width="106px" />
            </td>
        </tr>
      <tr>
                            <td class="tbltxt lbl2">
                                <asp:Label ID="lblReport" runat="server"></asp:Label>
                            </td>
                        </tr>
    </table>
</asp:Content>