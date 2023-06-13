<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.master" AutoEventWireup="true" CodeFile="Permission.aspx.cs" Inherits="Administrations_Permission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function check(chk) {
            chk.checked = false;
        }
    </script>

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
   
    </script>
  <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_admin.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>User Permission</h2>
    </div>
  
  <div class="spacer"><img src="../images/mask.gif" height="8" width="10"  /></div>
    
<table width="100%" border="0" cellspacing="2" cellpadding="2" class="cnt-box">
  <tr>
    <td width="150" class="tbltxt"><b>Select User Privileges</b></td>
    <td width="5"  class="tbltxt">:-</td>
    <td align="left"  class="tbltxt">&nbsp;</td>
    <td  class="tbltxt">&nbsp;</td>
  </tr>
  <tr>
    <td colspan="4" class="tbltxt" align="left">
   <asp:CheckBoxList ID="chklistapp" runat="server"  AutoPostBack="True" RepeatDirection="Horizontal" repeatcolumns="7" repeatlayout="flow" Width="100%"
                                           OnSelectedIndexChanged="chklistapp_SelectedIndexChanged" CssClass="tbltxt box">
                                        </asp:CheckBoxList></td>
    
  </tr>
  <tr>
    
    <td align="left"  class="tbltxt" colspan="4"><asp:Button ID="btnassperm" runat="server" Text="Assign Permission" OnClick="btnassperm_Click">
                                        </asp:Button>
    <asp:Button ID="btnUnassign" runat="server" Text="Un-Assign Permission" OnClick="btnUnassign_Click"
                                            Width="173px"></asp:Button>                                    
                                        </td>
  </tr>
  </table>
  <table width="100%" border="0" cellspacing="2" cellpadding="2">
  <tr>
    <td colspan="4" class="tbltxt"><asp:GridView Width="100%" ID="gridperm" runat="server" AutoGenerateColumns="False"
                                            CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="15px" />
                                                    <ItemTemplate>
                                                        <input name="Checkb" type="checkbox" value='<%#Eval("PermID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Module">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblModule" Text='<%#Eval("application")%>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Page">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPage" Text='<%#Eval("PageName")%>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rights">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRights" Text='<%#Eval("assigned")%>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblId" Text='<%#Eval("PageName")%>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle BackColor="#dfdfdf" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
      </asp:GridView></td>
    </tr>
</table>
<script language="javascript" type="text/javascript">
    function calendarPicker(strField) {
        window.open('Datepicker.aspx?field=' + strField, 'CalenderPopUp', 'width=235,height=220, screenX=100,screenY=200,left=200,top=375,titlebar=no,toolbar=no,resizable=no');
    }
    </script>

</asp:Content>
