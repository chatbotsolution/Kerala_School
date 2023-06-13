<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="FeeDueNotification.aspx.cs" Inherits="Reports_FeeDueNotification" %>
 <%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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

     function printcontent() {

         var DocumentContainer = document.getElementById('divreport');

         var documentheader = document.getElementById('divhdr');
         var WindowObject = window.open('', "TrackData",
                             "width=420,height=225,top=250,left=345,toolbars=no,scrollbars=no,status=no,resizable=yes");
         WindowObject.document.write(documentheader.innerHTML + "\n" + DocumentContainer.innerHTML);
         WindowObject.document.close();
         WindowObject.focus();
         WindowObject.print();
         WindowObject.close();
         return false;
     }
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

         <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                    <img src="../images/icon_rep.jpg" width="29" height="29">
                </div>
                <div style="padding-top: 5px;">
                    <h2>
            <strong>Fee Due Notification</strong>
            </h2>
        </div>
        <div class="spacer"></div>
        <table class="cnt-box tbltxt" width="100%">
            <tr>
                <td >
                    <b>
                      Welcome Note  &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTotRec" runat="server" Text=""></asp:Label></b>
                  
                </td>
              
            </tr>
            <tr>
              <td >
                <asp:TextBox ID="txtWrite" runat="server"  Width="98%" Rows="5" TextMode="MultiLine"
                        onClientClick="selectText();" TabIndex="1"></asp:TextBox>
                 </td>
            </tr>
            <tr>
                <td  class="tbltxt cnt-box2">
                For Class :
                    <asp:DropDownList ID="drpForCls" runat="server" Width="100px" CssClass="tbltxtbox"
                        TabIndex="3" AutoPostBack="True" OnSelectedIndexChanged="drpForCls_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp; &nbsp;&nbsp;&nbsp;
                   Defaulter Since:
                    <asp:TextBox ID="txtTillDate" runat="server" Width="70px" TabIndex="6"></asp:TextBox>
                    <rjs:popcalendar ID="pcalTillDate" runat="server" Control="txtTillDate" />
                    &nbsp;&nbsp; &nbsp;Fee Type :
                    <asp:DropDownList ID="drpFeeType" runat="server" CssClass="vsmalltb" TabIndex="3">
                        <asp:ListItem Value="0">-All-</asp:ListItem>
                       <%-- <asp:ListItem Value="1">Quarterly Fee</asp:ListItem>
                        <asp:ListItem Value="2">Admn./Re-admn Fee</asp:ListItem>--%>
                    </asp:DropDownList>
                    &nbsp;
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" TabIndex="4"
                        OnClientClick="return selectText();" />
                    &nbsp;<asp:Button ID="btnPrint" runat="server" Text="Print"  TabIndex="5"  OnClientClick="printcontent();"/>
                </td>
            </tr>
           <%-- <tr>
                <td>
                    <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                </td>--%>
                
        </table>
         <table width="100%" class="tbltxt">
        <tr>
            <td valign="top">
                <div id="divreport" style=" overflow:scroll; height:500px; ">
                    <table cellspacing="0" cellpadding="0" width="100%">
                        <tr id="trgrd" runat="server" visible="false">
                            <td valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl2">
                                <asp:Label ID="lblReport" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div style="display: none" id="divhdr">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label4" runat="server" Text="Fee Received" Font-Bold="True" Font-Underline="True"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblPrintDate" Font-Bold="true" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
     
</asp:Content>



