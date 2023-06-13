<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="ReceiptVoucher.aspx.cs" Inherits="Accounts_ReceiptVoucher" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function IsValidate() {
            var DrAcc = document.getElementById("<%=drpBankAc.ClientID %>").value;
            var TransDt = document.getElementById("<%=txtTransactionDate.ClientID %>").value;
            var DrawnOn = document.getElementById("<%=txtDrawnBank.ClientID %>").value;
            var Amount = document.getElementById("<%=txtAmount.ClientID %>").value;
            var Desc = document.getElementById("<%=txtDesc.ClientID %>").value;
            var InstNo = document.getElementById("<%=txtInstrumentNo.ClientID %>").value;
            var InstDt = document.getElementById("<%=txtInstrumentDt.ClientID %>").value;
            if (TransDt.trim() == "") {
                alert("Please Enter Transaction Date");
                document.getElementById("<%=txtTransactionDate.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=drpBankAc.ClientID %>").disabled == false && DrAcc.trim() == "0") {
                alert("Please select bank name");
                document.getElementById("<%=drpBankAc.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtInstrumentNo.ClientID %>").disabled == false && InstNo.trim() == "") {
                alert("Please Enter Instrument Number");
                document.getElementById("<%=txtInstrumentNo.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtInstrumentDt.ClientID %>").disabled == false && InstDt.trim() == "") {
                alert("Please select Instrument Date");
                document.getElementById("<%=txtInstrumentDt.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDrawnBank.ClientID %>").disabled == false && DrawnOn.trim() == "") {
                alert("Please Enter Drawn On Bank");
                document.getElementById("<%=txtDrawnBank.ClientID %>").focus();
                return false;
            }
            if (Amount.trim() == "") {
                alert("Please Enter Amount");
                document.getElementById("<%=txtAmount.ClientID %>").focus();
                return false;
            }
            if (Desc.trim() == "") {
                alert("Please Enter Description");
                document.getElementById("<%=txtDesc.ClientID %>").focus();
                return false;
            }

            var trdate = document.getElementById('<%=txtTransactionDate.ClientID%>').value;
            var dayfield = trdate.split("-")[0];
            var monthfield = trdate.split("-")[1];
            var yearfield = trdate.split("-")[2];
            monthfield = monthfield - 1;
            var myDate = new Date(yearfield, monthfield, dayfield);
            var today = new Date();

            if (myDate > today) {
                if (confirm("Transaction date is future date!Do you want to continue?")) {
                    return true;
                }
                else {
                    return false;
                }
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

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function beginRequest(sender, args) {
            // show the popup
            $find('<%=mdlloading.ClientID %>').show();

        }

        function endRequest(sender, args) {
            //  hide the popup
            $find('<%=mdlloading.ClientID %>').hide();

        }
    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
            <%--  <asp:AsyncPostBackTrigger ControlID="btnGotoList" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="grdAccountGroup" EventName="RowEditing" />--%>
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="200" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor" style="width:250px;">
                            <h1>
                                Receipt
                            </h1>
                            <h2>
                                Voucher</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="5px" cellpadding="0">
                <tr>
                    <td style="width: 130px" class="innertbltxt" align="left">
                        Transaction Date<span class="mandatory">*</span>
                    </td>
                    <td style="width: 5px" align="left" class="innertbltxt">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtTransactionDate" TabIndex="1" runat="server" ReadOnly="true"></asp:TextBox>&nbsp;<rjs:PopCalendar
                            ID="dtpTransDt" runat="server" Control="txtTransactionDate" AutoPostBack="True" To-Today="true"
                            onselectionchanged="dtpTransDt_SelectionChanged"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        Received From<span class="mandatory">*</span>
                    </td>
                    <td class="innertbltxt" align="left">
                       :</td>
                    <td align="left">
                       <asp:TextBox ID="txtGiver" runat="server" Width="200px" TabIndex="2"></asp:TextBox>
                       <asp:DropDownList ID="drpParty" runat="server" TabIndex="2"></asp:DropDownList>
                    &nbsp;
                        <asp:RadioButtonList ID="rBtnParty" runat="server" AutoPostBack="True" 
                            CellPadding="0" CellSpacing="0" 
                            OnSelectedIndexChanged="rBtnParty_SelectedIndexChanged" 
                            RepeatDirection="Horizontal" RepeatLayout="Flow" TabIndex="3">
                            <asp:ListItem Value="p">Party</asp:ListItem>
                            <asp:ListItem Selected="True" Value="o">Others</asp:ListItem>
                        </asp:RadioButtonList>
                        
                        
                        </td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        Income Head<span class="mandatory">*</span>
                    </td>
                    <td class="innertbltxt" align="left">
                        :
                    </td>
                    <td align="left">
                       <asp:DropDownList ID="drpAcHead" runat="server" Width="250px" TabIndex="3"></asp:DropDownList>
                    
                    </td>
                </tr>
                <tr>
                    <td class="innertbltxt">
                        Received Mode<span class="mandatory">*</span>
                    </td>
                    <td align="left" class="innertbltxt">
                        :
                    </td>
                    <td class="pageLabel" align="left">
                        <asp:RadioButtonList ID="rbtnMode" TabIndex="4" runat="server" OnSelectedIndexChanged="rbtnMode_SelectedIndexChanged"
                            AutoPostBack="True" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="C">Cash</asp:ListItem>
                            <asp:ListItem Selected="True" Value="B">Bank</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        Bank Name<span class="mandatory" id="spBankNm" runat="server">*</span>
                    </td>
                    <td class="innertbltxt" align="left">
                        :
                    </td>
                    <td align="left">
                       <asp:DropDownList ID="drpBankAc" runat="server" Width="250px" TabIndex="5"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        Instrument No.<span class="mandatory" id="spInstNo" runat="server">*</span>
                    </td>
                    <td class="innertbltxt" align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtInstrumentNo" runat="server" TabIndex="6"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 130px" class="innertbltxt" align="left">
                        Instrument Date<span class="mandatory" id="spInstDt" runat="server">*</span>
                    </td>
                    <td style="width: 5px" align="left" class="innertbltxt">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtInstrumentDt" TabIndex="7" runat="server"></asp:TextBox>&nbsp;<rjs:PopCalendar
                            ID="dtpInstDt" runat="server" Control="txtInstrumentDt"></rjs:PopCalendar>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 130px" class="innertbltxt" align="left">
                        Drawn on Bank<span class="mandatory" id="spDrawn" runat="server">*</span>
                    </td>
                    <td style="width: 5px" align="left" class="innertbltxt">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtDrawnBank" TabIndex="8" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        Amount<span class="mandatory">*</span>
                    </td>
                    <td class="innertbltxt" align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtAmount" runat="server" 
                            onkeypress="return blockNonNumbers(this, event, true, true);" TabIndex="9"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="innertbltxt" align="left">
                        Description<span class="mandatory">*</span>
                    </td>
                    <td class="innertbltxt" align="left">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtDesc" runat="server" TabIndex="10" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                    <td valign="top" align="left">
                        <asp:Button ID="btnSubmit" TabIndex="11" OnClick="btnSubmit_Click" 
                            runat="server" Width="100px" onfocus="active(this);" onblur="inactive(this);"
                            Text="Save" OnClientClick="return IsValidate();"></asp:Button>
                        <asp:Button ID="btnPrint" TabIndex="12" onfocus="active(this);" onblur="inactive(this);"
                                OnClick="btnPrint_Click" runat="server" Text="Print Receipt" Width="100px" Enabled="false"></asp:Button>                                                        
                        <asp:Button ID="btnClear" TabIndex="13" OnClick="btnClear_Click" runat="server" Text="Clear"
                           onfocus="active(this);" onblur="inactive(this);" CausesValidation="false" Width="120px"></asp:Button>
                        <asp:Button ID="btnList" runat="server" Text="Go To List" OnClick="btnList_Click"
                           onfocus="active(this);" onblur="inactive(this);" Width="120px" TabIndex="14" />
                        <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                           onfocus="active(this);" onblur="inactive(this);" Width="120px" TabIndex="15"></asp:Button>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                <td colspan="3">
                    <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                </td>
                </tr>
            </table>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


