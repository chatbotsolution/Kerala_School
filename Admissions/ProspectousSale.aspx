<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="ProspectousSale.aspx.cs" Inherits="Admissions_ProspectousSale" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {
            var SessionYear = document.getElementById("<%=drpSession.ClientID %>").value;
            var SaleDt = document.getElementById("<%=txtSaledt.ClientID %>").value;
            var Name = document.getElementById("<%=txtName.ClientID %>").value;
            var ProspectusType = document.getElementById("<%=drpProspectusType.ClientID %>").value;
            var SlNo = document.getElementById("<%=txtSlNo.ClientID %>").value;
            var ForClass = document.getElementById("<%=drpForCls.ClientID %>").value;

            var Ammount = document.getElementById("<%=txtAmm.ClientID %>").value;


            if (SessionYear == 0) {
                alert("Please Select Session Year!");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            if (SaleDt == "") {
                alert("Please Enter Sale Date!");
                document.getElementById("<%=txtSaledt.ClientID %>").focus();
                return false;
            }
            if (Name == "") {
                alert("Please Enter Student Name!");
                document.getElementById("<%=txtName.ClientID %>").focus();
                return false;
            }
            if (ProspectusType == "0") {
                alert("Please Select Prospectus Type!");
                document.getElementById("<%=drpProspectusType.ClientID %>").focus();
                return false;
            }
            if (ForClass == "0") {
                alert("Please Select Class!");
                document.getElementById("<%=drpForCls.ClientID %>").focus();
                return false;
            }
            if (SlNo == "") {
                alert("Please Enter Prospectus Serial Number!");
                document.getElementById("<%=txtSlNo.ClientID %>").focus();
                return false;
            }
            if (Ammount == "") {
                alert("Please Enter Amount!");
                document.getElementById("<%=txtAmm.ClientID %>").focus();
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
            Prospectus Sale Details</h2>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="3" width="100%" border="0" class="cnt-box">
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt" width="25%">
                        Session Year
                    </td>
                    <td align="left" class="tbltxt">
                        :<asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True"  Enabled="false"
                            CssClass="tbltxtbox largetb1 wdth-134" TabIndex="1"  
                            onselectedindexchanged="drpSession_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt">
                        Sale Date
                    </td>
                    <td align="left" valign="top" class="tbltxt">
                        :<asp:TextBox ID="txtSaledt" runat="server"   ReadOnly="true" CssClass="tbltxtbox largetb wdth-238" TabIndex="2"></asp:TextBox>&nbsp;<rjs:PopCalendar
                            ID="dtpSaledt" runat="server" Control="txtSaledt" Format="dd mmm yyyy" To-Today="true">
                        </rjs:PopCalendar>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt">
                        Student Name
                    </td>
                    <td align="left" valign="top" class="tbltxt">
                        :<asp:TextBox ID="txtName" runat="server"  CssClass="tbltxtbox largetb wdth-238" TabIndex="3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
            <td align="left" class="tbltxt">
                Prospectus Type</td>
            <td align="left" colspan="2" class="tbltxt">
                :<asp:DropDownList ID="drpProspectusType" runat="server"   
                    CssClass="tbltxtbox largetb1 wdth-250" TabIndex="3" AppendDataBoundItems="True" 
                    AutoPostBack="True" 
                    onselectedindexchanged="drpProspectusType_SelectedIndexChanged">
                </asp:DropDownList>
                </td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt">
                        Class
                    </td>
                    <td align="left" class="tbltxt">
                        :<asp:DropDownList ID="drpForCls" runat="server"   CssClass="tbltxtbox largetb1 wdth-134"
                            TabIndex="4">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt">
                        Contact Number</td>
                    <td align="left" class="tbltxt">
                        :<asp:TextBox ID="txtContNo" runat="server"  CssClass="tbltxtbox largetb wdth-238" onkeypress="return blockNonNumbers(this, event, false, false);"
                            MaxLength="15" TabIndex="5"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt" valign="top">
                        Current Address</td>
                    <td align="left" class="tbltxt" valign="top">
                        <span style="float:left">:</span><asp:TextBox ID="txtaddr" runat="server"  CssClass="tbltxtbox largetb wdth-238" MaxLength="100" 
                            TabIndex="5" Height="73px" TextMode="MultiLine" Width="217px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt">
                        Prospectus Sl.No
                    </td>
                    <td align="left" class="tbltxt" valign="top">
                        :<asp:TextBox ID="txtSlNo" runat="server"   CssClass="tbltxtbox largetb wdth-238" onkeypress="return blockNonNumbers(this, event, false, false);"
                            MaxLength="15" TabIndex="5"></asp:TextBox>
                    </td>
                    
                    </tr>
                    <tr>
                    <td align="left" class="tbltxt">
                        Amount
                    </td>
                    <td align="left" class="tbltxt">
                        :<asp:TextBox ID="txtAmm" runat="server" CssClass="tbltxtbox largetb wdth-238" MaxLength="6" 
                            onkeypress="return blockNonNumbers(this, event, false, true);" TabIndex="6" 
                            ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td  align="left">
                        <asp:Label ID="lblerr" runat="server" CssClass="error">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td valign="top" align="left"   style="height: 32px">
                        <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Submit" Width="64px"
                            OnClientClick="return isValid();" TabIndex="7" onfocus="active(this);" 
                            onblur="inactive(this);" CssClass="btn"></asp:Button>&nbsp;
                            <asp:Button ID="btnPrint" onfocus="active(this);" onblur="inactive(this);"
                                OnClick="btnPrint_Click" runat="server" Text="Print Receipt" CssClass="btn" TabIndex="8">
                            </asp:Button>&nbsp;
                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" onfocus="active(this);" onblur="inactive(this);"
                                Text="Clear All" TabIndex="9" CssClass="btn"></asp:Button>&nbsp;
                                <asp:Button ID="btnback" runat="server" onfocus="active(this);" onblur="inactive(this);"
                                    Text="List Page" OnClick="btnback_Click" TabIndex="10"  CssClass="btn"/>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
