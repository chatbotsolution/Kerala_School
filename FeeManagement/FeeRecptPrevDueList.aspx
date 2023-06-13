<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="FeeRecptPrevDueList.aspx.cs" Inherits="FeeManagement_FeeRecptPrevDueList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="Javascript">

        function CnfDelete() {

            if (confirm("You are going to delete a record. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
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


        function lowercase(txt) {
            var sometext = txt.value;

            var casechanged = sometext.ToLowerCase();


            return true;
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


        function checkinteger(txt) {

            if (isNaN(txt.value)) {
                alert("Enter number");


                return false;
            }
            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
 <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29">
    </div>
    <div style="padding-top: 5px;">
        <h2>
            Receive Old Dues</h2>
    </div>
    <div class="spacer"></div>
    <table width="100%" class="cnt-box">
        <tr>
            <td style="width: 100%;" valign="middle" class="tbltxt">
                Session :
                <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                    TabIndex="1">
                </asp:DropDownList>
                &nbsp; Payment Mode :
                <asp:DropDownList ID="drpPmtMode" runat="server" onclick="return ViewChequeDetails();"
                    TabIndex="2" CssClass="vsmalltb">
                    <asp:ListItem>All</asp:ListItem>
                    <asp:ListItem>Cash</asp:ListItem>
                    <asp:ListItem>Bank</asp:ListItem>
                </asp:DropDownList>
                From Date :
                <asp:TextBox ID="txtdate" runat="server" CssClass="" TabIndex="3"></asp:TextBox>&nbsp;<rjs:PopCalendar
                    ID="PopCalFmDt" runat="server" Control="txtdate"></rjs:PopCalendar>
                To Date :
                <asp:TextBox ID="txtDateTo" runat="server" CssClass="" TabIndex="4"></asp:TextBox>&nbsp;<rjs:PopCalendar
                    ID="PopCalToDt" runat="server" Control="txtDateTo" /><br /><br />
                Student Id :
                <asp:TextBox ID="txtadmnno" runat="server" AutoPostBack="True" onkeypress="return blockNonNumbers(this, event, false, false);"
                    onkeyup="return checkinteger(this);" OnTextChanged="txtadminno_TextChanged" TabIndex="5"
                    CssClass=""></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 100%" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 100%;" valign="top">
                <asp:Button ID="btnshow" runat="server" CausesValidation="False" OnClick="btnshow_Click"
                    Text="Show" TabIndex="6" />
                <asp:Button ID="btndelete" runat="server" CausesValidation="False" OnClick="btndelete_Click"
                    OnClientClick="return CnfDelete();" Text="Cancel Selected Receipt" Width="157px"
                    TabIndex="8" />
                <asp:Button ID="btnNew" runat="server" OnClick="btnNew_Click" Text="Receive Fee"
                    TabIndex="9" />
                <asp:Label ID="lblMsg" runat="server" CssClass="tbltxt"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:GridView ID="grdStud" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" Width="100%" TabIndex="7">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input name="toggleAll" onclick='ToggleAll(this)' disabled="disabled" type="checkbox"
                                    value="ON" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input name="Checkb" type="checkbox" value='<%# Eval("InvoiceReceiptNo") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="10px" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Received Date">
                            <ItemTemplate>
                                <%#Eval("Recvddate")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="90px" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Receipt/Voucher_No">
                            <ItemTemplate>
                                <%#Eval("InvoiceReceiptNo")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Session">
                            <ItemTemplate>
                                <%#Eval("SessionYr")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Admission No">
                            <ItemTemplate>
                                <%#Eval("PartyId")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="90px" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Student">
                            <ItemTemplate>
                                <%#Eval("RcvdFrom")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Class">
                            <ItemTemplate>
                                <%#Eval("ClassName")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Drawn On Bank">
                            <ItemTemplate>
                                <%#Eval("DrawanOnBank")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="90px" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cheque No">
                            <ItemTemplate>
                                <%#Eval("InstrumentNo")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <%#Eval("Amt")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="80px" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="error">
                            No Record Found</div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>




