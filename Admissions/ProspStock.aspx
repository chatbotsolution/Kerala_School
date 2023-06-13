<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="ProspStock.aspx.cs" Inherits="Admissions_ProspStock" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var SessionYear = document.getElementById("<%=drpSession.ClientID %>").value;
            var TransactionDate = document.getElementById("<%=txtTransdt.ClientID %>").value;
            var ProspectusType = document.getElementById("<%=drpProspectusType.ClientID %>").value;
            var Qty = document.getElementById("<%=txtQtyIn.ClientID %>").value;
            var Price = document.getElementById("<%=txtPrice.ClientID %>").value;

            if (SessionYear == 0) {
                alert("Please Select Session Year!");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            if (TransactionDate == "") {
                alert("Please Enter Transaction Date!");
                document.getElementById("<%=txtTransdt.ClientID %>").focus();
                return false;
            }
            if (ProspectusType == 0) {
                alert("Please Enter Prospectus Type!");
                document.getElementById("<%=drpProspectusType.ClientID %>").focus();
                return false;
            }
            if (Qty == "") {
                alert("Please Enter Quantity!");
                document.getElementById("<%=txtQtyIn.ClientID %>").focus();
                return false;
            }
            if (Price == "") {
                alert("Please Enter Price!");
                document.getElementById("<%=txtPrice.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }

        }

    </script>

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
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
             Prospectous Stock Details</h2>
    </div>
    <br />
    <table cellspacing="0" cellpadding="3"  border="0"  class="cnt-box" width="100%">
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="left" class="tbltxt ttl" style="height: 25px">
                Session Year
            </td>
            <td align="left" class="tbltxt" style="height: 25px">
                :<asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True"   Enabled="false"
                    CssClass="tbltxtbox" Width="134" TabIndex="1">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="left" class="tbltxt ttl">
                Transaction Date
            </td>
            <td align="left" colspan="2" valign="top" class="tbltxt">
                :<asp:TextBox ID="txtTransdt" runat="server" CssClass="tbltxtbox wdth-250" 
                    TabIndex="2"></asp:TextBox>&nbsp;<rjs:PopCalendar
                    ID="PopCalendar2" runat="server" Control="txtTransdt" Format="dd mmm yyyy" To-Today="true"></rjs:PopCalendar>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="left" class="tbltxt">
                Prospectus Type</td>
            <td align="left" colspan="2" class="tbltxt">
                :<asp:DropDownList ID="drpProspectusType" runat="server"  
                    CssClass="tbltxtbox largetb1 wdth-250" TabIndex="3" AppendDataBoundItems="True">
                </asp:DropDownList>
            &nbsp;&nbsp;<asp:Button ID="btnAdd" OnClick="btnAdd_Click" runat="server" onfocus="active(this);" onblur="inactive(this);"
                    Text="Add New" CssClass="btn2">
                </asp:Button>
                            
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="left" class="tbltxt">
                Quantity Received
            </td>
            <td align="left" colspan="2" class="tbltxt">
                :<asp:TextBox ID="txtQtyIn" runat="server" CssClass="tbltxtbox largetb wdth-250" onkeypress="return blockNonNumbers(this, event, false, false);"
                    MaxLength="14" TabIndex="4"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="left" class="tbltxt">
                Unit Sale Price</td>
            <td align="left" colspan="2" class="tbltxt">
                :<asp:TextBox ID="txtPrice" runat="server"   CssClass="tbltxtbox largetb wdth-250" onkeypress="return blockNonNumbers(this, event, false, false);"
                    MaxLength="14" TabIndex="4"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td ></td>
            <td valign="top" align="left" colspan="2" style="padding-left:8px;">
                <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Submit"  CssClass="btn"
                    OnClientClick="return isValid();" onfocus="active(this);" onblur="inactive(this);" TabIndex="5"></asp:Button>&nbsp;
                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" 
                    onfocus="active(this);" onblur="inactive(this);"
                    Text="Cancel" TabIndex="6" CssClass="btn">
                </asp:Button>&nbsp;
                <asp:Button ID="btnback" runat="server" Text="Back" OnClick="btnback_Click" onfocus="active(this);" onblur="inactive(this);"
                    TabIndex="7" CssClass="btn" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" colspan="3">
                <asp:Label ID="lblerr" runat="server" ForeColor="Red">
                </asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

