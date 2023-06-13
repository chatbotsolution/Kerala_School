<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="HostFeeDueNotification.aspx.cs" Inherits="Hostel_HostFeeDueNotification" %>

 <%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <script language="javascript" type="text/javascript">

     function selectText() {

         var Cls = document.getElementById("<%=drpForCls.ClientID %>").value;
         if (Cls == "0") {
             alert("Please select a class!");
             document.getElementById("<%=drpForCls.ClientID %>").focus();
             return false;
         }
         else {
             return true;
         }
     }
    
      
    </script>

    <div style="width: 680px;">
        <div align="center" style="border: solid 1px black; background: #dfdfdf;">
            <strong>Fee Due Notification</strong>
        </div>
        <table style="border: solid 1px black" width="100%">
            <tr>
                <td id="td1" runat="server">
                    <b>
                        <asp:Label ID="Label1" runat="server" Text="Welcome Note"></asp:Label></b>
                    <asp:TextBox ID="txtWrite" runat="server" Width="670px" CssClass="txtarea" TextMode="MultiLine"
                        onClientClick="selectText();" TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="border: solid 1px black;" class="tbltxt">
                For Class :
                    <asp:DropDownList ID="drpForCls" runat="server" Width="100px" CssClass="tbltxtbox"
                        TabIndex="3" AutoPostBack="True" OnSelectedIndexChanged="drpForCls_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp; &nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rbtnTillDt" CssClass="tbltxt" runat="server" Text="Till Date"
                        GroupName="s" Checked="true"></asp:RadioButton>
                    <asp:TextBox ID="txtTillDate" runat="server" Width="70px" TabIndex="6"></asp:TextBox>
                    <rjs:popcalendar ID="pcalTillDate" runat="server" Control="txtTillDate" />
                    <asp:RadioButton ID="rbtnFullSess" CssClass="tbltxt" runat="server" Text="Full Session"
                        GroupName="s"></asp:RadioButton>
                    &nbsp;&nbsp; &nbsp;Fee Type :
                    <asp:DropDownList ID="drpFeeType" runat="server" CssClass="vsmalltb" TabIndex="3">
                        <asp:ListItem Value="0">-All-</asp:ListItem>
                        <asp:ListItem Value="1">Regular Fee</asp:ListItem>
                        <asp:ListItem Value="2">Admn./Re-admn Fee</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" TabIndex="4"
                        OnClientClick="return selectText();" />
                    &nbsp;<asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="5" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                </td>
        </table>
    </div>
</asp:Content>

