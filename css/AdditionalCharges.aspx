<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="AdditionalCharges.aspx.cs" Inherits="FeeManagement_AdditionalCharges" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function Submit() {
            var Session = document.getElementById("<%=drpSession.ClientID %>").value;
            var Class = document.getElementById("<%=drpclass.ClientID %>").value;
            var AddFee = document.getElementById("<%=drpAdditionalFee.ClientID %>").value;

            if (Session == 0) {
                alert("Please Select Session !");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            if (Class == 0) {
                alert("Please Select Class !");
                document.getElementById("<%=drpclass.ClientID %>").focus();
                return false;
            }
            if (AddFee == 0) {
                alert("Please Select Additional Fee !");
                document.getElementById("<%=drpAdditionalFee.ClientID %>").focus();
                return false;
            }
            else {
                Submit();
                return true;
            }

        }
        function valSubmit() {

            var FeeDt = document.getElementById("<%=txtFeeDt.ClientID %>").value;
            var Fee = document.getElementById("<%=txtAddFee.ClientID %>").value;

            var currentdate = new Date();
            var currday = currentdate.getDate();


            if (FeeDt.trim() == "") {
                alert("Please Enter Fee Date !");
                document.getElementById("<%=txtFeeDt.ClientID %>").focus();
                return false;
            }
            if (Fee.trim() == "") {
                alert("Please Enter Additional Fee !");
                document.getElementById("<%=txtAddFee.ClientID %>").focus();
                return false;
            }
            else {
                //CheckLoader();
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
        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';
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

    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Define Miscellaneous Fees</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table cellspacing="0" cellpadding="0" width="100%" border="0" class="cnt-box">
                    <tr>
                        <td class="tbltxt" align="left">
                            Session :
                            <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                                TabIndex="1" onselectedindexchanged="drpSession_SelectedIndexChanged">
                            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <%--<asp:RequiredFieldValidator ID="rfvSession" runat="Server" ControlToValidate="drpSession"
                                Display="dynamic" SetFocusOnError="true" ErrorMessage="*Requited"></asp:RequiredFieldValidator>--%>
                            Class : <span class="error">*</span>
                            <asp:DropDownList ID="drpclass" runat="server" ValidationGroup="search" AutoPostBack="true"
                                OnSelectedIndexChanged="drpclass_SelectedIndexChanged" CssClass="vsmalltb" TabIndex="2">
                            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            Section :
                            <asp:DropDownList ID="ddlSection" runat="server" ValidationGroup="search" CssClass="vsmalltb"
                                TabIndex="3">
                            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            Misc Fee : <span class="error">*</span>
                            <asp:DropDownList ID="drpAdditionalFee" runat="server" ValidationGroup="search" CssClass="vsmalltb"
                                TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="drpAdditionalFee_SelectedIndexChanged">
                            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <br /><br />
                            <asp:Button ID="btngo" runat="server" Text="Search Students" ToolTip="Click to show student list."
                                TabIndex="13" OnClick="btngo_Click" OnClientClick="return Submit();"></asp:Button>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btngo"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="drpclass"/>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="cnt-box4">
                    <tr>
                        <td class="tbltxt">
                            Fee Date : <span class="error">*</span>
                            <asp:TextBox ID="txtFeeDt" runat="server" CssClass="largetb" Width="80px" TabIndex="5"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpFeeDt" runat="server" Format="dd mmm yyyy" Control="txtFeeDt">
                            </rjs:PopCalendar>
                            &nbsp; Fee Amount : <span class="error">*</span><asp:TextBox ID="txtAddFee" runat="server"
                                CssClass="largetb" Width="100px" TabIndex="6" onkeypress="return blockNonNumbers(this, event, true, false);"
                                MaxLength="15"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSubmit" runat="server" Text="Generate Misc Fee" Visible="false"
                                TabIndex="15" OnClick="btnSubmit_Click" OnClientClick="return valSubmit();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblMsg" runat="server" CssClass="gridtxt" Font-Bold="true" ForeColor="Black"
                                Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btnSubmit" />
        <asp:PostBackTrigger ControlID="drpSession" />
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdStudentList" runat="server" AutoGenerateColumns="false" Width="100%"
                    CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    TabIndex="14">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input name="Checkb" type="checkbox" value='<%# Eval("AdmnNo") %>' />
                                <asp:HiddenField ID="hfAdmnID" runat="server" Value='<%# Eval("AdmnNo") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="10px" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Admn No">
                            <HeaderStyle HorizontalAlign="Left" Width="90px" />
                            <ItemTemplate>
                                <%#Eval("Admnno")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="90px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Roll No">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("RollNo")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Student Name">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("fullname")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="lblGrdMsg" runat="server" CssClass="error"></asp:Label>
    </div>
</asp:Content>
