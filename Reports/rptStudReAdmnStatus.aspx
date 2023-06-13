<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptStudReAdmnStatus.aspx.cs" Inherits="Reports_rptStudReAdmnStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var Session = document.getElementById("<%=drpSession.ClientID %>").value;
            var currentdate = new Date();
            var currday = currentdate.getDate();

            if (Session == "0") {
                alert("Please Select Session !");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }

            else {
                return true;
            }
        }
    </script>
     <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Student Admission & Readmission Status
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%" border="0" cellspacing="2" cellpadding="2" class="cnt-box">
        <tr>
            <td width="60" class="tbltxt">
                Session
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            
            <td width="90">
                <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" AutoPostBack="true"
                    TabIndex="1" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
           
          <td width="30" class="tbltxt">
                Class
            </td>
            <td width="5" class="tbltxt">
                :
            </td>
            <td width="100">
                <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="true" CssClass="vsmalltb"
                    OnSelectedIndexChanged="drpclass_SelectedIndexChanged" TabIndex="2">
                </asp:DropDownList>
            </td>
            
           
            <td style="width:160px;" class="tbltxt">
                <asp:RadioButton ID="rbad" runat="server" Text="New Admission" GroupName="1" Checked="true" />  
            </td>
            <td style="width:160px;" class="tbltxt">
            <asp:RadioButton ID="rbread" runat="server" Text="Readmission Done" 
                    GroupName="1" />
            </td>
            <td class="tbltxt">
                <asp:RadioButton ID="rbreadnd" runat="server" Text="Readmission Not Done" 
                    GroupName="1" />
            </td>
            
        </tr>
        <tr>
            
            <td colspan="9">
                <asp:Button ID="btngo" runat="server" OnClick="btngo_Click" Text="Show" TabIndex="8"
                    OnClientClick="return isValid();" />&nbsp;
               
                <asp:Button ID="btnprint" runat="server" Text="Print" TabIndex="9" OnClick="btnprint_Click" />&nbsp;
              
                <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                    Width="106px" TabIndex="10" />
                </td>
        </tr>
        <tr>
            <td align="center" colspan="9">
                <asp:Label ID="lblReport" runat="server" Font-Bold="True" ></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>