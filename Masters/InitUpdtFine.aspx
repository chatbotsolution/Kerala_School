<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="InitUpdtFine.aspx.cs" Inherits="Masters_InitUpdtFine" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">


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

        function add() {

            var Class = document.getElementById("<%=drpClass.ClientID %>").value;
            var finedate = document.getElementById("<%=txtDate.ClientID %>").value;

            if (finedate == "") {
                alert("Enter  fine date !");
                document.getElementById("<%=txtDate.ClientID %>").focus();
                return false;
            }
            if (Class == 0) {
                alert("Select a Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }

        }
       
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fine Entry</h2>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="height: 360px;" width="100%">
                <tr>
                    <td style="width: 100%;" valign="top">
                        <table width="100%">
                            <tr>
                                <td align="left" valign="top" class="tbltxt" style="border: solid 1px Black; width: 100%;"
                                    colspan="2">
                                    <asp:Label ID="lblSession" runat="server" Text="Session:"></asp:Label>
                                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                                        Width="100px" CssClass="tbltxtbox" TabIndex="1">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblDate" runat="server" Text="Fine Date: "></asp:Label>
                                    <span class="error">*</span>
                                    <asp:TextBox ID="txtDate" runat="server" Width="80px" CssClass="tbltxtbox" TabIndex="5"></asp:TextBox>&nbsp;<rjs:PopCalendar
                                        ID="dtpFine" runat="server" Control="txtDate" Format="dd mm yyyy"></rjs:PopCalendar>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblClass" runat="server" Text="Class:"></asp:Label>
                                    <span class="error">*</span>
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                        Width="100px" CssClass="tbltxtbox" TabIndex="2">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblSection" runat="server" Text="Section:"></asp:Label>
                                    <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSection_SelectedIndexChanged"
                                        Width="50px" CssClass="tbltxtbox" TabIndex="3">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" TabIndex="6" OnClientClick="return add();"
                                        Text="Show All Students" />
                                    <asp:Button ID="BtnShowDefaulters" runat="server" OnClick="BtnShowDefaulters_Click"
                                        TabIndex="6" OnClientClick="return add();" Text="Show Defaulters" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" class="tbltxt">
                                    <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Font-Bold="true" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt">
                                    <asp:Label ID="lblRecCount" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td align="right" valign="top" style="height: 15px; width: 100px;" class="tbltxt">
                                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update Fine Amount"
                                        TabIndex="8" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:GridView ID="grdStudAC" runat="server" AutoGenerateColumns="False" Width="100%"
                                        CssClass="mGrid" AlternatingRowStyle-CssClass="alt" TabIndex="7">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Admission No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmnNo" runat="server" Text='<%#Eval("Admissionno")%>'></asp:Label>
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
                                            <asp:TemplateField HeaderText="Fine Due">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFinepaid" runat="server" Text='<%#Eval("Balance")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fine Amount" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left"
                                                ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFineAmount" runat="Server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        MaxLength="6" Text='<%#Eval("Amount")%>' CssClass="vsmalltb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                </td>
                                <td align="right" valign="top" style="height: 15px; width: 100px;" class="tbltxt">
                                    <asp:Button ID="btnUpdate2" runat="server" OnClick="btnUpdate_Click" OnClientClick="return addInit();"
                                        Text="Update Fine Amount" TabIndex="8" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
