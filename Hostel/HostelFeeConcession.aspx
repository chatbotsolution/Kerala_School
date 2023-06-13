<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="HostelFeeConcession.aspx.cs" Inherits="Hostel_HostelFeeConcession" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" language="javascript">

        function isValid() {

            var admnno = document.getElementById("<%=txtadmnno.ClientID %>").value;
            var condate = document.getElementById("<%=txtdate.ClientID %>").value;
            var reason = document.getElementById("<%=txtdesc.ClientID %>").value;
            var Rbtn = document.getElementById("<%=rBtnFeeHeadAmt.ClientID %>");
            var RbtnM = document.getElementById("<%=rBtnMlyAmt.ClientID %>");

            if (admnno == "") {
                alert("Please enter admission number");
                document.getElementById("<%=txtadmnno.ClientID %>").focus();
                return false;
            }
            if (condate == "") {
                alert("Please select discount date!");
                document.getElementById("<%=txtdate.ClientID %>").focus();
                return false;
            }
            if (reason == "") {
                alert("Please enter reason of discount!");
                document.getElementById("<%=txtdesc.ClientID %>").focus();
                return false;
            }
            if (!Rbtn.checked && !RbtnM.checked) {
                var amt = document.getElementById("<%=txtamt.ClientID %>").value;
                if (amt.trim() == "" || parseFloat(amt) == 0 || parseFloat(amt) > 100) {
                    alert("Please enter correct discount percent/amount!");
                    document.getElementById("<%=txtamt.ClientID %>").focus();
                    return false;
                }
            }
            if (FeeAmt.length > 0) {
                var count = 0;
                for (i = 1; i < FeeAmt.length + 1; i++) {
                    if (document.getElementById(ConcAmt[i - 1]).value != "") {
                        count++;
                        if (parseFloat(document.getElementById(ConcAmt[i - 1]).value) > parseFloat(document.getElementById(FeeAmt[i - 1]).value)) {
                            alert("Sl No(" + i + "):  Concession Amount Can't  more than Fee Amount !");
                            document.getElementById(ConcAmt[i - 1]).focus();
                            return false;
                        }
                    }
                }
                if (count == 0) {
                    alert("Please Enter Concession Amount !");
                    return false;
                }
            }
            else {
                CnfSave();
                return true;
            }
        }

        function CnfSave() {

            if (confirm("You are going to give discount. Do you want to continue?")) {
                return true;
            }
            else {

                return false;
            }
        }
        function valDetails() {
            var Admno = document.getElementById("<%=txtadmnno.ClientID %>").value;
            if (Admno.trim() == "") {
                alert("Please enter admission number !");
                document.getElementById("<%=txtadmnno.ClientID %>").focus();
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


        function extractNumber(obj, decimalPlaces, allowNegative) {
            var temp = obj.value;

            // avoid changing things if already formatted correctly
            var reg0Str = '[0-9]*';
            if (decimalPlaces > 0) {
                reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
            } else if (decimalPlaces < 0) {
                reg0Str += '\\.?[0-9]*';
            }
            reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
            reg0Str = reg0Str + '$';
            var reg0 = new RegExp(reg0Str);
            if (reg0.test(temp)) return true;

            // first replace all non numbers
            var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
            var reg1 = new RegExp(reg1Str, 'g');
            temp = temp.replace(reg1, '');

            if (allowNegative) {
                // replace extra negative
                var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
                var reg2 = /-/g;
                temp = temp.replace(reg2, '');
                if (hasNegative) temp = '-' + temp;
            }

            if (decimalPlaces != 0) {
                var reg3 = /\./g;
                var reg3Array = reg3.exec(temp);
                if (reg3Array != null) {
                    // keep only first occurrence of .
                    //  and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
                    var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
                    reg3Right = reg3Right.replace(reg3, '');
                    reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;
                    temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;
                }
            }

            obj.value = temp;
        }
       

       
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Give Instant Concession For Hostel Fee</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="2" cellspacing="2">
                <tr>
                    <td width="120" class="tbltxt">
                        Session
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" TabIndex="1"
                            AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Select Class
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                            CssClass="vsmalltb" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Student
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpstudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpstudent_SelectedIndexChanged"
                            CssClass="largetb" TabIndex="3">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Student Id
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <span class="error">*</span>
                        <asp:TextBox ID="txtadmnno" runat="server" Width="100px" CssClass="vsmalltb"
                            TabIndex="4"></asp:TextBox>
                        <asp:Button ID="btnDetail" runat="server" Text="Show" OnClientClick="return valDetails();"
                            OnClick="btnDetail_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Discount Date
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <span class="error">*</span>
                        <asp:TextBox ID="txtdate" runat="server" meta:resourcekey="txtdateResource1" CssClass="vsmalltb"
                            TabIndex="5"></asp:TextBox>&nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server"
                                Control="txtdate"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt">
                        Reason
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <span class="error">*</span>
                        <asp:TextBox ID="txtdesc" runat="server" CssClass="largetb" TabIndex="6"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt" align="left">
                        Discount Type
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td class="tbltxt">
                        <span class="error">*</span>
                          <asp:RadioButton ID="rBtnFeeHeadAmt" runat="server"
                            Text="Annual &amp; Misc Fee" ValidationGroup="contype"
                            GroupName="contype" CssClass="tbltxt" OnCheckedChanged="rBtnFeeHeadAmt_CheckedChanged"
                            AutoPostBack="True" />
                        &nbsp;
                        <asp:RadioButton ID="rBtnMlyAmt" runat="server" Text="Monthly Fee" ValidationGroup="contype"
                            GroupName="contype" AutoPostBack="True" CssClass="tbltxt" 
                            OnCheckedChanged="rBtnFeeHeadAmt_CheckedChanged" />&nbsp;
                        <asp:RadioButton ID="rBtnPer" runat="server" Text="Percentage"  ValidationGroup="contype" Checked="true"
                            GroupName="contype" AutoPostBack="True" CssClass="tbltxt" OnCheckedChanged="rBtnFeeHeadAmt_CheckedChanged" />
                        &nbsp;
                        <%--<asp:RadioButton ID="rBtnTotAmt" runat="server" Text="Tot Amount" ValidationGroup="contype"
                            GroupName="contype" AutoPostBack="True" CssClass="tbltxt" OnCheckedChanged="rBtnFeeHeadAmt_CheckedChanged" />--%>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="2" cellspacing="2">
                <tr>
                    <td id="tdAmt" runat="server">
                        <table>
                            <tr>
                                <td class="tbltxt" style="height: 23px" width="120">
                                    Percent
                                </td>
                                <td class="tbltxt" style="height: 23px" width="5">
                                    :
                                </td>
                                <td style="height: 23px">
                                    <span class="error">*</span>
                                    <asp:DropDownList ID="drpConsession" runat="server">
                                    <asp:ListItem>--All--</asp:ListItem>
                                    <asp:ListItem>Annual Fee</asp:ListItem>
                                    <asp:ListItem>Monthly Fee</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtamt" runat="server" Width="118px" CssClass="vsmalltb" TabIndex="7"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" MaxLength="4"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnCopyAll" runat="server" Visible="false"
                            Text="Copy Existing Amount For Concession" onclick="btnCopyAll_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdRegFeeAmt" runat="server" AutoGenerateColumns="False" Width="600px"
                            CssClass="mGrid" AlternatingRowStyle-CssClass="alt" DataKeyNames="TransNo" TabIndex="1"
                            OnPreRender="grdRegFeeAmt_PreRender">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl. No">
                                    <ItemTemplate>
                                        <%# ((GridViewRow)Container).RowIndex + 1%>
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fee Names">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPeriodicityID" runat="server" Text='<%#Eval("FeeName")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="300px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fee Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFineApplicable" runat="server" Text='<%#Eval("Balance")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Concession Amount">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfActualAmt" Value='<%#Eval("Balance") %>' runat="server" />
                                        <asp:TextBox ID="txtConsAmt" runat="server" Width="120px" onkeypress="return blockNonNumbers(this, event, true, false);"
                                            MaxLength="10" Style="text-align: right"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="130px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt" align="left">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="error"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnsave" runat="server" OnClientClick="return  isValid();" OnClick="btnsave_Click"
                            Text="Submit" TabIndex="8" />&nbsp;
                        <asp:Button ID="btncancel" runat="server" CausesValidation="False" OnClick="btncancel_Click"
                            Text="Cancel" TabIndex="9" />
                        &nbsp;
                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" OnClick="btnBack_Click"
                            TabIndex="9" Text="Back to Fee Receipt" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
            <asp:PostBackTrigger ControlID="btnDetail" />
            <asp:PostBackTrigger ControlID="rBtnFeeHeadAmt" />
            <asp:PostBackTrigger ControlID="rBtnMlyAmt" />
            <asp:PostBackTrigger ControlID="drpstudent" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>

