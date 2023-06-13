<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="JournalVoucher.aspx.cs" Inherits="Accounts_JournalVoucher" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script language="javascript" type="text/javascript">
    function IsValidate() {
        var transdate = document.getElementById("<%=txtTransactionDate.ClientID %>").value;
        var Details = document.getElementById("<%=txtDetails.ClientID %>").value;
        var grid = (document.getElementById("<%=grdJrnl.ClientID%>")) ? document.getElementById("<%=grdJrnl.ClientID%>") : null;

        if (transdate.trim() == "") {
            alert("Please Select Transaction date");
            document.getElementById("<%=txtTransactionDate.ClientID %>").focus();
            return false;
        }

        if (grid) {
            if (grid.rows.length == 0) {
                alert("Please Add Entries to save");
                return false;
            }
        }
        else {
            alert("Please Add Entries to save");
            return false;
        }

        if (Details.trim() == "") {
            alert("Please Enter Details");
            document.getElementById("<%=txtDetails.ClientID %>").focus();
            return false;
        }

        else {
            return true;
        }

    }

    function ValidateAdd() {

        var AccHd = document.getElementById("<%=drpAccHead.ClientID %>").value;
        var AccGrp = document.getElementById("<%=drpAccGroup.ClientID %>").value;
        var Amount = document.getElementById("<%=txtAmount.ClientID %>").value;

        if (AccGrp == "0") {
            alert("Please Account Group");
            document.getElementById("<%=drpAccGroup.ClientID %>").focus();
            return false;
        }

        if (AccHd == "0") {
            alert("Please Account Head");
            document.getElementById("<%=drpAccHead.ClientID %>").focus();
            return false;
        }

        if (Amount.trim() == "") {
            alert("Please Enter Amount");
            document.getElementById("<%=txtAmount.ClientID %>").focus();
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

    </script>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="200px" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor" style="width: 200px;">
                            <h1>
                                Journal
                            </h1>
                            <h2>
                                Voucher</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="75%" border="0" cellspacing="5px" cellpadding="0">
                <tr>
                    <td align="left" style="width:200px">
                        Transaction Date<span class="mandatory">*</span>:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtTransactionDate" TabIndex="1" runat="server" ReadOnly="true"></asp:TextBox>&nbsp;<rjs:PopCalendar
                            ID="dtpTransDt" runat="server" Control="txtTransactionDate" To-Today="true"></rjs:PopCalendar>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                <td>Type</td>
                <td>Account Group</td>
                <td>Account Head</td>
                <td>Amount</td>
                </tr>
                <tr>
                     <td align="left" style="width: 172px">
                        <asp:DropDownList ID="drpType" runat="server">
                            <asp:ListItem Value="Debit">Debit</asp:ListItem>
                            <asp:ListItem Value="Credit">Credit</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="left" style="width: 172px">
                       <asp:DropDownList ID="drpAccGroup" runat="server" AutoPostBack="true"
                            onselectedindexchanged="drpAccGroup_SelectedIndexChanged">
                       </asp:DropDownList>
                    </td>
                    <td align="left" style="width: 172px">
                       <asp:DropDownList ID="drpAccHead" runat="server">
                       </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAmount" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                    </td>
                    <td>
                       <asp:Button ID="btnAdd" runat="server" Text="Add"
                            CausesValidation="false" Width="84px" onfocus="active(this);" OnClientClick="return ValidateAdd();"
                            onblur="inactive(this);" onclick="btnAdd_Click">
                       </asp:Button>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                
                <tr>
                    <td colspan="4">
                          <asp:GridView ID="grdJrnl" runat="server" AutoGenerateColumns="False" Width="500px"
                            TabIndex="4" onrowdeleting="grdJrnl_RowDeleting" ShowFooter="true"
                              onrowdatabound="grdJrnl_RowDataBound">
                            <FooterStyle BackColor="#424242" Font-Bold="True" ForeColor="White" 
                        HorizontalAlign="Right" />
                            <Columns>
                                <asp:TemplateField HeaderText="Acccount Head">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfAccId" runat="server" Value='<%#Eval("AccHdId")%>' />
                                        <asp:Label ID="lblAccId" runat="server" Text='<%#Eval("AccName")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="300px" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type" Visible="false">
                                    <ItemTemplate>
                                       <asp:Label ID="lblCrDr" runat="server" Text='<%#Eval("CrDr")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Debit">
                                    <ItemTemplate>
                                       <asp:Label ID="lblDrAmount" runat="server" Text='<%#Eval("AmountDr")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="150px" />
                                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="150px" />
                                    <FooterTemplate>
                                            <asp:Label ID="lblDrTot" runat="server" Text=''></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Credit">
                                    <ItemTemplate>
                                       <asp:Label ID="lblCrAmount" runat="server" Text='<%#Eval("AmountCr")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="150px" />
                                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="150px" />
                                    <FooterTemplate>
                                            <asp:Label ID="lblCrTot" runat="server" Text=''></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                
                                <asp:CommandField ButtonType="Button" ShowDeleteButton="true" HeaderText="Delete"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                                
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                
                <tr><td>&nbsp;</td></tr>
                
                <tr>
                      <td>
                            Transaction Details<span class="mandatory">*</span>
                      </td>
                      <td colspan="3">
                            <asp:TextBox ID="txtDetails" runat="server" TabIndex="10" Width="440px"></asp:TextBox>
                      </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                
                <tr>
                       <td></td>
                       <td valign="top" align="left" colspan="3">
                        <asp:Button ID="btnSubmit" TabIndex="11" OnClick="btnSubmit_Click" runat="server"
                            Text="Save & Add New" OnClientClick="return IsValidate();" onfocus="active(this);"
                            onblur="inactive(this);"></asp:Button>
                        <asp:Button ID="btnClear" TabIndex="12" OnClick="btnClear_Click" runat="server" Text="Clear"
                            CausesValidation="false" Width="120px" onfocus="active(this);" onblur="inactive(this);">
                        </asp:Button>
                        <asp:Button ID="btnList" TabIndex="13" runat="server" Text="Go To List" OnClick="btnList_Click"
                            Width="120px" onfocus="active(this);" onblur="inactive(this);" />
                       </td>
                </tr>
            </table>

</asp:Content>


