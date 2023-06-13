<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="ClassMaster.aspx.cs" Inherits="Masters_ClassMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function valClass() {
            var Str = document.getElementById("<%=drpStream.ClientID %>").value;
            var Cls = document.getElementById("<%=txtClassName.ClientID %>").value;
            var NoOfSec = document.getElementById("<%=txtNoOfSections.ClientID %>").value;

//            if (Str == "0") {
//                alert("Select Stream !");
//                document.getElementById("<%=drpStream.ClientID %>").focus();
//                return false;
//            }

            if (Cls.trim() == "") {
                alert("Enter Class Name !");
                document.getElementById("<%=txtClassName.ClientID %>").focus();
                return false;
            }
            if (NoOfSec.trim() == "") {
                alert("Enter No. Of Section !");
                document.getElementById("<%=txtNoOfSections.ClientID %>").focus();
                return false;
            }

            else {
                return true;
            }
        }
        function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
            var key;
            var isCtrl = false;
            var keychar;
            var reg;

            if (window.event) {
                key = e.keyCode;
                isCtrl = window.event.ctrlKey
            }
            else if (e.which) {
                key = e.which;
                isCtrl = e.ctrlKey;
            }

            if (isNaN(key)) return true;

            keychar = String.fromCharCode(key);

            // check for backspace or delete, or if Ctrl was pressed
            if (key == 8 || isCtrl) {
                return true;
            }

            reg = /\d/;
            var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
            var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

            return isFirstN || isFirstD || reg.test(keychar);
        }

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

        function ShowMe(e) {
            if (e.checked) {
                document.getElementById("divgrddesg").style.display = "block";
            }
            else {
                document.getElementById("divgrddesg").style.display = "none";
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Class Master</h2>
    </div>
    <table cellspacing="0" cellpadding="3" width="100%" border="0">
        <tbody>
           
            <tr>
                <td valign="top" align="left" colspan="3">
                    <asp:GridView ID="grdClassMaster" runat="server" AllowPaging="true" PageSize="15"
                        OnRowCommand="grdClassMaster_RowCommand" AutoGenerateColumns="False" Width="498px"
                        OnPageIndexChanging="grdClassMaster_PageIndexChanging" CssClass="mGrid" PagerStyle-CssClass="pgr"
                        AlternatingRowStyle-CssClass="alt" TabIndex="4">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="15px" />
                                <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                <ItemTemplate>
                                    <input name="Checkb" type="checkbox" value='<%#Eval("ClassID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Class Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDesign" CommandName="show" CausesValidation="false" Text='<%#Eval("ClassName")%>'
                                        CommandArgument='<%#Eval("ClassID")%>' runat="server"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No. of Sections">
                                <ItemTemplate>
                                    <asp:Label ID="lblNoOfSection" runat="server" Text='<%#Eval("NoOfSections") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
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
            <tr>
                <td>
                    <table width="500px" class="tbltxt">
                        <tr>
                            <td valign="top" align="left">
                               
                            </td>
                            <td valign="top" align="left" colspan="2">
                                <asp:DropDownList ID="drpStream" runat="server" Width="200px" TabIndex="1" Visible="false">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left">
                                Class Name:
                            </td>
                            <td valign="top" align="left" colspan="2">
                                <asp:TextBox ID="txtClassName" runat="server" Width="200px" TabIndex="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left">
                                No. of Sections:
                            </td>
                            <td valign="top" align="left" colspan="3">
                                <asp:TextBox ID="txtNoOfSections" runat="server" Width="100px" TabIndex="3" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" colspan="3">
                                <font face="Verdana">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Submit" Width="64px"
                                        TabIndex="4" OnClientClick="return valClass();"></asp:Button>
                                    <asp:Button ID="btnDelete" OnClick="btnDelete_Click" runat="server" Text="Delete"
                                        CausesValidation="False" TabIndex="5"></asp:Button>
                                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                        CausesValidation="False" TabIndex="6"></asp:Button>
                                    <input id="hdnsts" type="hidden" runat="server" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
