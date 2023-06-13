<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="HouseMaster.aspx.cs" Inherits="Masters_HouseMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
        function enter(e) {

            var keycode = e.keyCode;
            //var txt=document.getElementById("txtsearch").value;
            if (keycode == 13) {
                __doPostBack(document.getElementById('<%=btnSave.ClientID%>'));

            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            House Master</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="3" width="100%" border="0">
                <tbody>
                    <%-- <tr>
                        <td style="width: 100%" valign="top" align="center" bgcolor="#dfdfdf" colspan="2">
                            <strong>House Master</strong>
                        </td>
                    </tr>--%>
                    <tr>
                        <td valign="top" align="center" width="100%" colspan="2">
                            <asp:Label ID="lblerr" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" colspan="2">
                            <asp:GridView ID="grdHouseMaster" runat="server" OnPageIndexChanging="grdHouseMaster_PageIndexChanging"
                                AutoGenerateColumns="False" Width="498px" OnRowCommand="grdHouseMaster_RowCommand"
                                PageSize="8" AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" 
                                AlternatingRowStyle-CssClass="alt" TabIndex="3">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="15px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                        <ItemTemplate>
                                            <input name="Checkb" type="checkbox" value='<%#Eval("HouseID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="House Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDesign" CausesValidation="false" Text='<%#Eval("HouseName")%>'
                                                CommandName="show" CommandArgument='<%#Eval("HouseID")%>' runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <hr />
                        </td>
                    </tr>
                    <table width="500px" class="tbltxt">
                        <tr>
                            <td style="width: 100px;" valign="top" align="left">
                                House Description:
                            </td>
                            <td valign="top" align="left">
                                <asp:TextBox ID="txtHouse" runat="server" Width="268px" onkeypress="return enter(event)"
                                    CssClass="tbltxtbox" TabIndex="1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvHouse" runat="server" Display="Dynamic" ControlToValidate="txtHouse"
                                    ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 16px" valign="top" align="center" colspan="2">
                                <font face="Verdana">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" 
                                    Text="Submit" Width="64px" TabIndex="2">
                                    </asp:Button>
                                    <asp:Button ID="btnDelete" OnClick="btnDelete_Click" runat="server" Text="Delete"
                                        CausesValidation="False" TabIndex="4"></asp:Button>
                                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                        CausesValidation="False" TabIndex="5"></asp:Button>
                                    <input id="hdnsts" type="hidden" runat="server" />
                                </font>
                            </td>
                        </tr>
                    </table>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

