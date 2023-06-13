<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="BrandList.aspx.cs" Inherits="Masters_BrandList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
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
        function CnfDelete() {

            if (confirm("You are going to delete a record. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }  
      
    </script>
 <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Brand Master</h2> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
    </div>
  
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tbody>
            <tr>
                <td>
                    <asp:Button ID="btnAddNew" runat="server" OnClick="btnAddNew_Click" Text="Add New"
                        TabIndex="5" />
                    <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" OnClientClick="return CnfDelete();"
                        Text="Delete" TabIndex="6" />
                </td>
                <td align="right" class="Totrec">
                    <b>
                        <asp:Label ID="lblRecord" runat="server" CssClass="gridtxt" ForeColor="Red"></asp:Label></b>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3">
                    <asp:GridView ID="grdBrandMaster" runat="server" AllowPaging="true" PageSize="15"
                        AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="grdBrandMaster_PageIndexChanging"
                        CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        DataKeyNames="BrandId" TabIndex="4">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="15px" />
                                <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                <ItemTemplate>
                                    <input name="Checkb" type="checkbox" value='<%#Eval("BrandId")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <a href='BrandMaster.aspx?Id=<%#Eval("BrandId")%>'>Edit</a>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Brand Name">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:Label ID="lblCompId" Text='<%#Eval("BrandName")%>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" colspan="3" class="tbltxt">
                    <asp:Label ID="lblerr" runat="server" Font-Bold="true">
                    </asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

