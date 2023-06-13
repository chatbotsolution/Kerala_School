<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptProsStockStatus.aspx.cs" Inherits="Reports_rptProsStockStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <script language="javascript" type="text/javascript">
     function Isvalid() {

         var SessionYear = document.getElementById("<%=drpSession.ClientID%>").value;

         if (SessionYear == 0) {
             alert("Please Select Session Year!");
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
            Prospectus Stock Position
        </h2>
    </div>
    <div class="spacer"></div>
      <table width="100%" class="tbltxt cnt-box" >  
                
          <tr>
              <td >
                  Session Year :
                  <asp:DropDownList ID="drpSession" runat="server" CssClass="tbltxtbox" Width="80px"
                      TabIndex="1">
                  </asp:DropDownList>
                  
                  &nbsp;
                  
                  Prospectus Type: : <asp:DropDownList ID="drpProspectusType" runat="server" Width="149px"
                      CssClass="tbltxtbox" TabIndex="3" AppendDataBoundItems="True">
                  </asp:DropDownList>
                  
                  <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return Isvalid();"
                      TabIndex="2" />
              </td>
          </tr>
          <tr>
                    <td align="left">
                        
                    </td>
                </tr>
             
            
            </table>
            <div class="cnt-box2">
            <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
            </div>
    
            
     
</asp:Content>
