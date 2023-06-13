<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="ItemSale.aspx.cs" Inherits="Accounts_ItemSale" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .hid
        {
        display:none;
        }

        .style1
        {
            font-family: Tahoma, Geneva, sans-serif;
            font-size: 12px;
            color: #000;
            width: 178px;
            height: 51px;
            padding: 3px;
        }
        .style2
        {
            font-family: Tahoma, Geneva, sans-serif;
            font-size: 12px;
            color: #000;
            height: 51px;
            padding: 3px;
        }
        .style3
        {
            height: 51px;
        }
        .style4
        {
            height: 23px;
        }

    </style>
    <script src="Js\AjaxClient.js"></script>

    <script language="Javascript" type="text/javascript">
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


        function checkdates() {
            if (getdates("Form Date") == true && getdates("Issue Date") == true)
                return true;
            else
                return false;
            return true;
        }
        function getdates(type) {
            var start_date;
            if (type == "Issue Date")
                start_date = document.getElementById('<%=txtissuedate.ClientID%>').value;
            start_date = start_date.split("-")
            year = start_date[2];
            month = start_date[1] - 1;
            day = start_date[0];
            var currentdate = new Date();
            var myDate = new Date(year, month, day);
            var currday = currentdate.getDate();
            if (myDate > currentdate) {
                alert(type + " can't be greater than current date");
                return false;
            }

            return true;
        }
        function PrintReceipt(url) {
            var newWin = window.open(url);

            if (!newWin || newWin.closed || typeof newWin.closed == 'undefined') {
                alert("Please allow pop-up");
            }
        }
        function validateShow() {
            var session = document.getElementById("<%=drpSession.ClientID %>").value;
            var studClass = document.getElementById("<%=drpclass.ClientID %>").value;
            var student = document.getElementById("<%=drpstudent.ClientID%>").value;
            if (session.trim() == "") {
                alert("Please select Session !");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            if (studClass.trim() == "" || studClass == "0") {
                alert("Please select Class !");
                document.getElementById("<%=drpclass.ClientID %>").focus();
                return false;
            }
            if (student.trim() == "" || student == "0") {
                alert("Please select a Student !");
                document.getElementById("<%=drpclass.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
        function validatePrepBill() {
            var gvRecCount = document.getElementById("<%=hfGvRecCount.ClientID%>").value;
            if (gvRecCount <= 0) {
                alert("There is no items added to prepare bill. Please add some items to list !");
                return false;
            }
            else {
                return true;
            }
        }
        function validateSubmit() {
            var invDate = document.getElementById("<%=txtissuedate.ClientID%>").value;
            var receivedFrom = document.getElementById("<%=txtRcvdFrom.ClientID%>").value;
            var pmtCash = document.getElementById("<%=optPmtCash.ClientID%>").checked;
            var bankName = document.getElementById("<%=txtBankName.ClientID%>").value;
            var instrNo = document.getElementById("<%=txtInstrNo.ClientID%>").value;
            var instrDate = document.getElementById("<%=txtInstrDate.ClientID%>").value;
            var bankHd = document.getElementById("<%=drpBank.ClientID%>").value;
            if (invDate.trim() == "") {
                alert("Please provide Invoice Date !");
                document.getElementById("<%=txtissuedate.ClientID%>").focus();
                return false;
            }
            if (receivedFrom.trim() == "") {
                alert("Please specify Received From !");
                document.getElementById("<%=txtRcvdFrom.ClientID%>").focus();
                return false;
            }
            if (!pmtCash) {
                if (bankHd == "0") {
                    alert("Please Select Bank Head !");
                    document.getElementById("<%=drpBank.ClientID%>").focus();
                    return false;
                }
                if (bankName.trim() == "") {
                    alert("Please provide Drawn On Bank name !");
                    document.getElementById("<%=txtBankName.ClientID%>").focus();
                    return false;
                }
                if (instrNo.trim() == "") {
                    alert("Please provide instrument no. !");
                    document.getElementById("<%=txtInstrNo.ClientID%>").focus();
                    return false;
                }
                if (instrDate.trim() == "") {
                    alert("Please provide instrument date !");
                    document.getElementById("<%=txtInstrDate.ClientID%>").focus();
                    return false;
                }
            }
            else {
                return true;
            }
        }
        function IsDetailValid() {
            var category = document.getElementById("<%=ddlCategory.ClientID %>").selectedIndex;
            var item = document.getElementById("<%=ddlItem.ClientID %>").selectedIndex;
            if (category == "0") {
                alert("Please Select Category !");
                document.getElementById("<%=ddlCategory.ClientID %>").focus();
                return false;
            }
            if (item == "0") {
                alert("Please Select Item !");
                document.getElementById("<%=ddlItem.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }



    </script>

    <script language="javascript">
        function roundNumber(num, dec) {
            var num1 = new Number(num);
            var result = num1.toFixed(2);
            return result;
        }
    </script>

    <div align="center">
        <asp:UpdatePanel ID="updadd" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table style="width: 100%" id="Table1" runat="server" cellspacing="0" cellpadding="0">
                    <tr id="Tr1" style="background-color: #ededed;" runat="server">
                        <td id="Td1" align="left" valign="middle" style="width: 45%;" runat="server">
                            <div class="headingcontainor" style="width: 350px; float: left;">
                                <h1>Book Material Sale
                                </h1>
                            </div>
                        </td>
                    </tr>
                    <tr id="Tr2" runat="server">
                        <td id="Td2" valign="top" align="left" style="padding-top: 2px;" 
                            runat="server">
                            <table width="99%" cellpadding="0" cellspacing="0" style="border: solid 2px gray; box-shadow: silver 8px 8px 5px; padding: 5px;">
                                <tr>
                                    <td class="style1" align="left" style="padding-left: 5px; ">Received Date&nbsp;:&nbsp;<asp:TextBox 
                                                                ID="txtissuedate" runat="server" CssClass="smalltb" Width="60px" 
                                                                TabIndex="17" ReadOnly="true" ></asp:TextBox>
                                                            <rjs:PopCalendar ID="dtpSaleDate" runat="server" AutoPostBack="True" To-Today="true"
                                            Control="txtissuedate" onselectionchanged="dtpSaleDate_SelectionChanged"></rjs:PopCalendar> 
                                                        </td>
                                    <td class="style2">
                                        <%--<asp:RadioButton ID="rbtnStud" CssClass="tbltxt" runat="server" Text="Student" AutoPostBack="True"
                                            GroupName="s" OnCheckedChanged="rbtnStud_CheckedChanged" TabIndex="3"></asp:RadioButton>
                                        <asp:RadioButton ID="rbtnOthers" CssClass="tbltxt" runat="server" Text="Others" AutoPostBack="True"
                                            Checked="True" GroupName="s" OnCheckedChanged="rbtnOthers_CheckedChanged" 
                                            TabIndex="4">
                                        </asp:RadioButton>--%>
                                        Sale Type:
                                        <asp:DropDownList ID="drpSaleList" runat="server" AutoPostBack="true" 
                                            onselectedindexchanged="drpSaleList_SelectedIndexChanged" 
                                            CssClass="vsmalltb" Width="125px" TabIndex="1">
                                            <asp:ListItem>For School Students</asp:ListItem>
                                            <asp:ListItem>For Other School Students</asp:ListItem>
                                            <asp:ListItem>For Others</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style2">
                                        Session Year :
                                        <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb" 
                                            AutoPostBack="True"  Width="65px"
                                            OnSelectedIndexChanged="drpSession_SelectedIndexChanged" TabIndex="2">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style2">
                                        Class :
                                        <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                                            CssClass="vsmalltb" Width="70px" TabIndex="3">
                                        </asp:DropDownList>
                                    </td>
                                     <td class="style2">
                                        Stream :
                                        <asp:DropDownList ID="ddlStream" Enabled="false" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStream_SelectedIndexChanged"
                                            CssClass="vsmalltb" Width="70px" TabIndex="3">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style2">
                                        Select Student :
                                        <asp:DropDownList ID="drpstudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpstudent_SelectedIndexChanged"
                                            CssClass="largetb" TabIndex="4">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style2">
                                        Student Id :
                                        <asp:TextBox ID="txtadmnno" runat="server" CssClass="vsmalltb" Width="70px" 
                                            TabIndex="5"></asp:TextBox>
                                    </td>
                                    <td align="left" class="style3">
                                        <asp:Button Text="Search" runat="server" ID="btnSearch" 
                                            OnClick="btnSearch_Click" TabIndex="6" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                    <table>
                                    <tr>
                                    <td>Tax</td>
                                    <td>
                                    <asp:RadioButtonList ID="rbtnInvType" runat="server" AutoPostBack="true"
                                    RepeatDirection="Horizontal"
                                    onselectedindexchanged="rbtnInvType_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="I">Inclusive</asp:ListItem>
                                    <asp:ListItem  Value="E">Extra</asp:ListItem>
                                    </asp:RadioButtonList>
                                    </td>
                                    </tr>
                                    </table>
                                    </td>
                                    <td align="left" colspan="4" class="tbltxt">
                                    <asp:Literal ID="litHeaderMsg" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="ItemGrid" runat="server">
                        <td id="Td3" runat="server">
                            <table width="100%">
                                <tr>
                                    <td class="style4"></td>
                                </tr>
                                <tr>
                                    <td>
                                    <asp:Panel ID="pnlAddItem" runat="server" meta:resourcekey="pnlAddItemResource1" 
                                            BackColor="#f5f5f5" Visible="false">
                                        <table width="100%">
                                            <tr>
                                                <td class="tbltxt" style="width:200px">
                                                    Category <span class="mandatory">*</span>
                                                    <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True"
                                                         CssClass="smalltb"
                                                        TabIndex="9" onselectedindexchanged="ddlCategory_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="tbltxt" style="width:250px">
                                                    Item <span class="mandatory">*</span>
                                                    <asp:DropDownList ID="ddlItem" runat="server" CssClass="largetb"
                                                        TabIndex="10">
                                                        <asp:ListItem Text="--Select--" Value="0" Selected="True" ></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                
                                                <td rowspan="2" class="tbltxt" style="width:20px">
                                                    <asp:Button ID="btnAdd" runat="server" Text="ADD" OnClientClick="return IsDetailValid();"
                                                        meta:resourcekey="btnAddResource1" TabIndex="11" onclick="btnAdd_Click"/>
                                                </td>
                                                <td class="tbltxt">
                                                    <asp:Label ID="lblMsg2" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvItemsForSale" runat="server" DataKeyNames="ItemCode" 
                                            CssClass="mGrid" autoGenerateColumns="False"
                                            Width="100%" TabIndex="12" onrowdatabound="gvItemsForSale_RowDataBound">
                                            <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:TemplateField >
                                                    <ItemTemplate>
                                                        <asp:CheckBox Text="" Checked="true" runat="server" ID="optItem"  OnCheckedChanged="optItem_CheckedChanged" AutoPostBack="true" TabIndex="-1" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="0px"/>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox Text=""  runat="server" ID="optItemAll" OnCheckedChanged="optItemAll_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="0px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%#Eval("ItemName")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                               
                                              
                                                 <asp:TemplateField HeaderText="Stock Qty">
                                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#Eval("AvlQty")%>' ID="lblAvlQty" runat="server"></asp:Label>&nbsp;<%#Eval("MesuringUnit")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="60px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sale Qty.">
                                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtQty" CssClass="vsmalltb" onkeypress="return blockNonNumbers(this, event, true, false);" Text='<%#Eval("Qty")%>' MaxLength="3" TabIndex="12"/>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="60px" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Price">
                                                    <HeaderStyle HorizontalAlign="right" />
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#Eval("SalePrice")%>' ID="lblSalePrice" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Rate" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#Eval("TaxRate")%>' ID="lblTaxRate" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tax">
                                                    <HeaderStyle HorizontalAlign="Left" Width="70px" CssClass="hid"/>
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#Eval("TaxAmount")%>' ID="lblTaxAmount" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="70px"  CssClass="hid"/>
                                                   
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tot. Amount">
                                                    <HeaderStyle HorizontalAlign="right" />
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#Eval("Amount") %>' runat="server" ID="lblAmt" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                
                                            </Columns>
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="padding-left: 50px;">
                                        <asp:Button Text="Calculate and Prepare Bill" runat="server" ID="btnPrepBill" 
                                            OnClick="btnPrepBill_Click" Width="200px" 
                                            OnClientClick="return validatePrepBill();" TabIndex="13" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="Tr3" runat="server">
                        <td id="Td4" align="center" runat="server">
                            <asp:UpdatePanel runat="server" ID="updtPnlSummary">
                                <ContentTemplate>
                                    <table id="tbllist" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
                                        <tr>
                                            <td style="height: 20px" align="center"></td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" class="tbltxt">
                                                <div style="width: 100%;">
                                                    <table style="width: 400px" align="right">
                                                        <tr>
                                                            <td align="right" style="font-weight: bold;" class="tbltxt">Total Bill Amount:</td>
                                                            <td>&nbsp;<asp:Label ID="txttotalbill" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="font-weight: bold;" class="tbltxt">Total Discount:</td>
                                                            <td>&nbsp;<asp:Label ID="txttotaldis" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:PlaceHolder runat="server" ID="phVatDesc"></asp:PlaceHolder>
                                                            </td>
                                                            <td>
                                                                <asp:PlaceHolder runat="server" ID="phVatValue"></asp:PlaceHolder>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="font-weight: bold;" class="tbltxt">Total Tax:</td>
                                                            <td>&nbsp;<asp:Label ID="txttotalvat" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td align="right" style="font-weight: bold;" class="tbltxt">Additional Discount:</td>
                                                            <td>&nbsp;<asp:TextBox ID="txtaddDis"
                                                                onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                runat="server" ReadOnly="True" MaxLength="8" AutoPostBack="true" 
                                                                    OnTextChanged="txtaddDis_TextChanged" TabIndex="14"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="font-weight: bold;">
                                                                <asp:TextBox Text=" Additional Charges" runat="server" ID="txtAddChargeDesc" 
                                                                    onfocus="watermark();" 
                                                                    ToolTip="Please provide additional charges description !" TabIndex="15" />
                                                                    function watermark() {
            var textBox = document.getElementById("<%=txtAddChargeDesc.ClientID%>");
            if (textBox.value.length > 0) {
                if (textBox.value == " Additional Charges") {
                    textBox.value = "";
                }
            }
            else
                textBox.value = " Additional Charges";
        }
                                                                    </td>
                                                            <td>&nbsp;<asp:TextBox ID="txtaddcharge"
                                                                onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                runat="server" ReadOnly="True" MaxLength="13" AutoPostBack="true" 
                                                                    OnTextChanged="txtaddcharge_TextChanged" TabIndex="16"></asp:TextBox></td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td align="right" style="font-weight: bold;" class="tbltxt">Total Amount:</td>
                                                            <td>&nbsp;<asp:Label ID="txttotalamt" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td >
                                                <table id="tblPaymentDetails" width="100%" style="border: solid 1px gray;">
                                                    <tr>
                                                        <td align="left" colspan="4" 
                                                            style="color: white; background-color: gray; font-size: 14px;">PAYMENT DETAILS
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        
                                                        <td style="width: 228px" class="tbltxt" align="left">
                                                            &nbsp;Received From&nbsp;:&nbsp;<asp:TextBox runat="server" ID="txtRcvdFrom" 
                                                                CssClass="smalltb" MaxLength="50" TabIndex="18" Width="130px"/>
                                                        </td>
                                                        <td align="left" style="width: 191px;" class="tbltxt">Payment Mode&nbsp;:&nbsp;<asp:RadioButton 
                                                                ID="optPmtCash" Text="Cash" runat="server" GroupName="pmtmode" Checked="true" 
                                                                AutoPostBack="true" OnCheckedChanged="PaymentMode_CheckedChanged" 
                                                                TabIndex="20" />&nbsp;<asp:RadioButton ID="optPmtBank" Text="Bank" 
                                                                runat="server" GroupName="pmtmode" AutoPostBack="true" 
                                                                OnCheckedChanged="PaymentMode_CheckedChanged" TabIndex="18" /></td>
                                                        <td align="left" class="tbltxt">
                                                           Received Amount :<asp:TextBox ID="txtRcvdAmt" runat="server" CssClass="smalltb" 
                                                                MaxLength="50" TabIndex="18" 
                                                                Width="130px">0</asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="4" style="padding-left: 5px; padding-top: 5px;" 
                                                            class="tbltxt">
                                                        Bank A/c Head &nbsp;:&nbsp;<asp:DropDownList runat="server" ID="drpBank" 
                                                                CssClass="smalltb" TabIndex="22">
                                                        </asp:DropDownList>&nbsp;
                                                        Drawn On Bank&nbsp;:&nbsp;<asp:TextBox runat="server" ID="txtBankName" 
                                                                CssClass="smalltb" MaxLength="50" TabIndex="24" />&nbsp;Instrument No :&nbsp;<asp:TextBox 
                                                                runat="server" ID="txtInstrNo" CssClass="smalltb" Width="150px" 
                                                                TabIndex="21" />
                                                        &nbsp;Instrument Date&nbsp;:&nbsp;<asp:TextBox runat="server" ID="txtInstrDate" 
                                                                CssClass="smalltb" Width="90px" ReadOnly="true" TabIndex="5" />
                                                        <rjs:PopCalendar ID="dtpInstDate" runat="server" Control="txtInstrDate"></rjs:PopCalendar>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                        <td>
                                        &nbsp;
                                        </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 1px;" valign="top" align="center">
                                            <asp:Button ID="btnSubmitPrint"
                                                        runat="server" Text="Save and Print" Visible="False" 
                                                    OnClick="btnSubmitPrint_Click" OnClientClick="return validateSubmit();" 
                                                    TabIndex="27"></asp:Button>&nbsp;
                                                <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server"
                                                    Text="Save" Visible="False" ValidationGroup="First"
                                                    OnClientClick="return validateSubmit();" style="height: 26px" 
                                                    TabIndex="26"></asp:Button>&nbsp;<asp:Button ID="btnSubmitReturn" runat="server" 
                                                    OnClick="btnSubmitReturn_Click" OnClientClick="return validateSubmit();" 
                                                    style="height: 26px" TabIndex="26" Text="Save With Return" 
                                                    ValidationGroup="First" Visible="False" />
&nbsp;<asp:Button ID="btncancel" OnClick="btncancel_Click" runat="server"
                                                    Text="Cancel" CausesValidation="False" TabIndex="28"></asp:Button>&nbsp;<asp:Button 
                                                    Text="Sold List" runat="server" ID="btnSoldList" OnClick="btnSoldList_Click" 
                                                    TabIndex="29" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="txtInstrDate"/>
                                    
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr id="Tr4" runat="server">
                        <td id="Td5" style="padding:10px;" runat="server">
                            <asp:Literal id="litFooterMsg" runat="server" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hfGvRecCount" runat="server" Value="0" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSubmit" />
                <asp:PostBackTrigger ControlID="btnSubmit" />
                <asp:PostBackTrigger ControlID="dtpSaleDate" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>


