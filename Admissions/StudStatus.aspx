<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="StudStatus.aspx.cs" Inherits="Admissions_StudStatus" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

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
        function add() {
            var clas = document.getElementById("<%=drpClass.ClientID %>").value;
            var date = document.getElementById("<%=txtDate.ClientID %>").value;
            var status = document.getElementById("<%=drpStatus.ClientID %>").value;
            if (clas == 0) {
                alert("Select Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            if (date == "") {
                alert("Enter  date !");
                document.getElementById("<%=txtDate.ClientID %>").focus();
                return false;
            }

            if (status == 0) {
                alert("Select status !");
                document.getElementById("<%=drpStatus.ClientID %>").focus();
                return false;
            }
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
            Modify Student Status</h2>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="height: 360px;" width="100%">
                <tr>
                    <td style="width: 100%;" valign="top">
                        <table width="100%">
                            <tr>
                                <td align="left" valign="top" class="tbltxt" style="border: solid 1px Black; width: 100%;">
                                    <asp:Label ID="lblSession" runat="server" Text="Session:"></asp:Label>
                                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                                        Width="100px" CssClass="tbltxtbox" TabIndex="1">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblClass" runat="server" Text="Class:"></asp:Label>
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                        Width="100px" CssClass="tbltxtbox" TabIndex="2">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblSection" runat="server" Text="Section:"></asp:Label>
                                    <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSection_SelectedIndexChanged"
                                        Width="50px" CssClass="tbltxtbox" TabIndex="3">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" TabIndex="6" Text="Show" />
                                    <br />
                                    <br />
                                    <asp:Label ID="lblDate" runat="server" Text="Date:"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="txtDate" runat="server" Width="80px" CssClass="tbltxtbox" TabIndex="5"></asp:TextBox>&nbsp;<rjs:PopCalendar
                                        ID="PopCalendar3" runat="server" Control="txtDate" Format="dd mm yyyy"></rjs:PopCalendar>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblstatus" runat="server" Text="Status:"></asp:Label>
                                    <asp:DropDownList ID="drpStatus" runat="server" Width="100px" CssClass="tbltxtbox"
                                        TabIndex="6">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" OnClientClick="return add();"
                                        Text="Update" TabIndex="8" />
                                    <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                                <td align="right" valign="top">
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2" style="height: 15px" class="tbltxt">
                                    <asp:Label ID="lblRecCount" runat="server" Text="Label" Width="908px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:GridView ID="grdStudStatus" runat="server" AutoGenerateColumns="False" Width="100%"
                                        CssClass="mGrid" AlternatingRowStyle-CssClass="alt" TabIndex="7">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                <ItemTemplate>
                                                    <input name="Checkb" type="checkbox" value='<%#Eval("Admissionno")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Admission Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbladmndt" runat="server" Text='<%#Eval("AdmnDate")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("FullName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldob" runat="server" Text='<%#Eval("dateofbirth")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Class">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclass" runat="server" Text='<%#Eval("classname")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Section">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSection" runat="server" Text='<%#Eval("Section")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contact No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblphno" runat="server" Text='<%#Eval("TelNoResidence")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

