<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="CatagoryList.aspx.cs" Inherits="Masters_CatagoryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">


        $(document).ready(function () {
            document.getElementById("<%=btnAddNew.ClientID %>").focus();
        });
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
           Item Category</h2> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
    </div>
   
    <table width="100%">
        <tr>
            <td align="left" style="width: 167px">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New" OnClick="btnAddNew_Click"
                    TabIndex="2" />&nbsp;&nbsp;
                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"
                    OnClientClick="return CnfDelete();" TabIndex="3" />
            </td>
            <td align="right">
                <asp:Label ID="lblTotRecord" runat="server" Visible="False" CssClass="gridtxt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2">
                <asp:GridView ID="grdcatmaster" runat="Server" DataKeyNames="Catid" AllowPaging="true"
                    PageSize="10" AutoGenerateColumns="false" OnPageIndexChanging="grdcatmaster_PageIndexChanging"
                    Width="100%" TabIndex="1">
                    <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <input type="checkbox" name="Checkb" value='<%# Eval("CatId") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7px" />
                            <HeaderTemplate>
                                <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                            </HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="7px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <HeaderStyle ForeColor="White"></HeaderStyle>
                            <ItemTemplate>
                                <a href='ItemCatagory.aspx?Id=<%#Eval("CatId")%>'>Edit</a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Catagory Name">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("CatName")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="2">
                <asp:Label ID="lblnorecord" runat="server" CssClass="pageError" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:HiddenField ID="HiddenField1" runat="server" __designer:wfdid="w2"></asp:HiddenField>
                <asp:HiddenField ID="catpageindex" runat="server"></asp:HiddenField>
            </td>
        </tr>
    </table>
</asp:Content>
