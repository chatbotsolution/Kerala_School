<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="Holiday.aspx.cs" Inherits="Masters_Holiday" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <script language="Javascript">
     function ToggleAll(e) {
         if (e.checked) {
             CheckAll();
         }
         else {
             ClearAll();
         }
     }

     function CheckAll() {
         var ml = document.aspnetForm;
         var len = ml.elements.length;
         for (var i = 0; i < len; i++) {
             var e = ml.elements[i];
             if (e.name == "Checkb") {
                 e.checked = true;
             }
         }
         ml.toggleAll.checked = true;
     }
     function ClearAll() {
         var ml = document.aspnetForm;
         var len = ml.elements.length;
         for (var i = 0; i < len; i++) {
             var e = ml.elements[i];
             if (e.name == "Checkb") {
                 e.checked = false;
             }
         }
         ml.toggleAll.checked = false;
     }
     function valid() {
         var flag;
         var ml = document.aspnetForm;
         var len = ml.elements.length;
         for (var i = 0; i < len; i++) {
             var e = ml.elements[i];
             if (e.name == "Checkb") {

                 if (e.checked) {
                     flag = true;
                     break;
                 }
                 else
                     flag = false;
             }
         }
         //alert(flag);
         if (flag == true)
             return true;
         else {
             alert("Please select any record");
             return false;
         }
     }
   
    </script>
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_admin.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>Holidays</h2>
    </div>
  
  <div class="spacer"><img src="../images/mask.gif" height="8" width="10"  /></div>
  
  <table width="100%" border="0" cellspacing="2" cellpadding="2">
  <tr>
    <td>
    <asp:Button ID="btndelete" runat="server" CausesValidation="False" OnClick="btndelete_Click"
                    OnClientClick="return valid();" Text="Delete" Width="74px" 
            TabIndex="2" />&nbsp;
            <asp:Button ID="btnNew" runat="server" Text="Add New" 
            OnClick="btnNew_Click" TabIndex="3" />
    
    </td>
    </tr>
  <tr>
    <td><asp:GridView ID="grdHoliday" runat="server" AutoGenerateColumns="False" 
            Width="100%" AllowPaging="True"  PageSize="15" 
            OnPageIndexChanging="grdHoliday_PageIndexChanging" CssClass="mGrid" 
            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" TabIndex="1">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                           <input type="checkbox" name="Checkb" value='<%# Eval("HolidayID") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="10px" />
                        <HeaderTemplate>
                            <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                        </HeaderTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Holiday Description">
                        <ItemTemplate>
                            <a href='Holidaydetail.aspx?hno=<%#Eval("HolidayID")%>' >
                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("HolidayName")%>'></asp:Label>
                                </a>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Holiday Date">
                        <ItemTemplate>
                           <%#Eval("HolidayDate")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                        <HeaderStyle HorizontalAlign="center"  Width="100px"/>
                    </asp:TemplateField>
                    
                </Columns>
                    <EmptyDataTemplate>
                       <div class="error"> No Record Found</div>
                    </EmptyDataTemplate>
                    <PagerStyle HorizontalAlign="Center" />
                </asp:GridView></td>
    </tr>
</table>

  
  
  
  
</asp:Content>


