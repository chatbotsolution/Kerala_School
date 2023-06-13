<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="FeeAdjustmentList.aspx.cs" Inherits="FeeManagement_FeeAdjustmentList" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                 if (e.checked)
                     flag = true;
                 else
                     flag = false;
             }
         }
         if (flag == true)
             return true;
         else {
             alert("Please select any record");
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
            Fee Adjustment</h2>
    </div>
  <div class="spacer"><img src="../images/mask.gif" height="8" width="10"  /></div>
   
<table width="100%" border="0" cellpadding="2" cellspacing="2">
  <tr>
    <td class="tbltxt"> Received Date : 
      <asp:TextBox ID="txtdate" runat="server" CssClass="vsmalltb" TabIndex="1"></asp:TextBox> &nbsp;<rjs:PopCalendar
                                ID="PopCalendar2" runat="server" Control="txtdate"></rjs:PopCalendar> 
      <asp:Button ID="btnshow" runat="server" CausesValidation="False" OnClick="btnshow_Click"
                                Text="Show" TabIndex="2" /></td>
  </tr>
  <tr>
    <td align="left"><asp:Button ID="btndelete" runat="server" CausesValidation="False" OnClick="btndelete_Click"
                                OnClientClick="return valid();" 
            Text="Delete Selected Record" TabIndex="4" />
      <asp:Button ID="btnNew" runat="server" OnClick="btnNew_Click" Text="Add New" 
            TabIndex="5" /></td>
  </tr>
  <tr>
    <td>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" 
            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" 
            Width="100%" TabIndex="3">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <input type="checkbox" name="Checkb" value='<%# Eval("AdjustmentId") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Width="10px" HorizontalAlign="Left" />
                                        <HeaderTemplate>
                                            <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Adjustment Date">
                                        <ItemTemplate>
                                            <%#Eval("recvddate")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                        <HeaderStyle Width="110px" HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <%--Added the transcation date(jaskirat-- 20-03-2009) --%>
                                    <asp:TemplateField HeaderText="Admission No">
                                        <ItemTemplate>
                                            <%#Eval("AdmnNo")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                        <HeaderStyle Width="90px" HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Student">
                                        <ItemTemplate>
                                            <%#Eval("fullname")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Class">
                                        <ItemTemplate>
                                            <%#Eval("classname")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle Width="60px" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reason">
                                        <ItemTemplate>
                                            <%#Eval("Reason")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle  HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <%#Eval("Amt")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle Width="80px" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <a href='FeeAdjustment.aspx?aid=<%#Eval("AdjustmentId")%>&amt=<%#Eval("Amt")%>'>Edit</a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                        <HeaderStyle Width="50px" HorizontalAlign="center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="error">No Record</div>
                                </EmptyDataTemplate>
      </asp:GridView></td>
  </tr>
</table>
</asp:Content>


