<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="ItemMaster.aspx.cs" Inherits="Masters_ItemMaster" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function isValid() {
            var Brand = document.getElementById("<%=drpBrand.ClientID %>").value;
            var Cat = document.getElementById("<%=drpCat.ClientID %>").value;
            var Item = document.getElementById("<%=txtItemName.ClientID %>").value;
            var ItemDesc = document.getElementById("<%=txtItemDesc.ClientID %>").value;
            var Measure = document.getElementById("<%=drpMeasureUnit.ClientID %>").value;
            var Books = document.getElementById("<%=rbtBooks.ClientID %>").checked;
            if (Brand == "0") {
                alert("Please Select Brand");
                document.getElementById("<%=drpBrand.ClientID %>").focus();
                return false;
            }
            if (Cat == "0") {
                alert("Please Select Category");
                document.getElementById("<%=drpCat.ClientID %>").focus();
                return false;
            }
            if (Item.trim() == "") {
                alert("Please Enter Item Name !");
                document.getElementById("<%=txtItemName.ClientID %>").focus();
                return false;
            }
            if (ItemDesc.trim() == "") {
                alert("Please Enter Item Description !");
                document.getElementById("<%=txtItemDesc.ClientID %>").focus();
                return false;
            }
            if (Measure.trim() == "0") {
                alert("Please Select Mesuring Unit");
                document.getElementById("<%=drpMeasureUnit.ClientID %>").focus();
                return false;
            }
            else {
                if (document.getElementById("<%=rbtOther.ClientID %>").checked) {
                    return true;
                }
                else {

                    return validateCheckBoxList();
                }
            }
        }

        function validateCheckBoxList() {
            var isAnyCheckBoxChecked = false;
            var checkBoxes = document.getElementById("ctl00_ContentPlaceHolder1_chklClass").getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox") {
                    if (checkBoxes[i].checked) {
                        isAnyCheckBoxChecked = true;
                        break;
                    }
                }
            }
            if (!isAnyCheckBoxChecked) {
                alert("Select an Class");
            }

            return isAnyCheckBoxChecked;
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

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Item List</h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="100%" border="0" cellspacing="1" cellpadding="2">
                <tr>
                    <td>
                        <asp:RadioButton ID="rbtBooks" Text="Books And Stationary" runat="server" 
                            Checked="true" GroupName="Items" oncheckedchanged="rbtBooks_CheckedChanged" AutoPostBack="true"/>
                        <asp:RadioButton ID="rbtOther" Text="Others" runat="server" GroupName="Items" 
                            oncheckedchanged="rbtOther_CheckedChanged" AutoPostBack="true"/>
                    </td>
                    <tr>
                        <td style="width: 450px" valign="top">
                            <table>
                    </tr>
                    <tr>
                        <td class="tbltxt" style="width: 150px">
                            Brand<span class="mandatory">*</span>
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td style="width: 272px">
                            <asp:DropDownList ID="drpBrand" runat="server" TabIndex="2">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt">
                            Category<span class="mandatory">*</span>
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td style="width: 272px">
                            <asp:DropDownList ID="drpCat" runat="server" TabIndex="3" AutoPostBack="True" 
                                style="height: 22px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" class="tbltxt">
                            For Class
                        </td>
                        <td width="5" align="left" valign="top" class="tbltxt">
                            :
                        </td>
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="updtpnl1" runat="server">
                                <ContentTemplate>
                                    <%--<asp:ListBox ID="listClass" runat="server" SelectionMode="Multiple" Height="20px">
                                    </asp:ListBox>--%>
                                    <asp:CheckBoxList ID="chklClass" RepeatDirection="Vertical" runat="server" OnSelectedIndexChanged="chklClass_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:CheckBoxList>
                                    <asp:DropDownList ID="ddlStream" runat="server" Visible="false">
                                   
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt">
                            Bar Code
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td style="width: 272px">
                            <asp:TextBox ID="txtBarCode" runat="server" TabIndex="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt">
                            Item Name<span class="mandatory">*</span>
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td style="width: 272px">
                            <asp:TextBox ID="txtItemName" runat="server" Width="250" MaxLength="100" TabIndex="1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tbltxt" valign="top">
                            Item Description<span class="mandatory">*</span>
                        </td>
                        <td class="tbltxt" valign="top">
                            :
                        </td>
                        <td valign="top" style="width: 272px">
                            <asp:TextBox ID="txtItemDesc" runat="server" TextMode="MultiLine" Width="250px" Height="60px"
                                MaxLength="200" TabIndex="5"></asp:TextBox>
                        </td>
                    </tr>
            </table>
                    </td>
                    <td valign="top">
                        <table>
                        
                            <tr>
                                <td class="tbltxt">
                                    ROL Quantity
                                </td>
                                <td class="tbltxt">
                                    :
                                </td>
                                <td style="width: 272px">
                                    <asp:TextBox ID="txtRol" runat="server" onkeypress="return blockNonNumbers(this, event, true, true);"
                                        TabIndex="14"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltxt">
                                    Depreciation
                                </td>
                                <td class="tbltxt">
                                    :
                                </td>
                                <td class="tbltxt" style="width: 272px">
                                    <asp:TextBox ID="txtDepreciation" runat="server" CssClass="tbltxtbox-no-rb" MaxLength="3"
                                        Style="border-right: solid 0px transparent;" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        Width="50px" TabIndex="52"></asp:TextBox>
                                    <asp:TextBox ID="TextBox1" runat="server" ReadOnly="true" Text="%" Width="20px" CssClass="tbltxtbox-no-lb"
                                        Style="border-left: solid 0px transparent;" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltxt">
                                    Measuring Unit<span class="mandatory">*</span>
                                </td>
                                <td class="tbltxt">
                                    :
                                </td>
                                <td style="width: 272px">
                                    <asp:DropDownList ID="drpMeasureUnit" runat="server" TabIndex="6">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltxt">
                                    Is Salable
                                </td>
                                <td class="tbltxt">
                                    :
                                </td>
                                <td style="width: 272px">
                                    <asp:RadioButton ID="rbtnSaleYes" runat="server" Text="Yes" AutoPostBack="True" GroupName="Sal"
                                        Checked="True" TabIndex="12" OnCheckedChanged="rbtnSaleYes_CheckedChanged"></asp:RadioButton>
                                    <asp:RadioButton ID="rbtnSaleNo" runat="server" Text="No" GroupName="Sal" TabIndex="13"
                                        AutoPostBack="True" OnCheckedChanged="rbtnSaleYes_CheckedChanged"></asp:RadioButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltxt">
                                    Is Consumable
                                </td>
                                <td class="tbltxt">
                                    :
                                </td>
                                <td style="width: 272px">
                                    <asp:RadioButton ID="rbtnConsumeYes" runat="server" Text="Yes" AutoPostBack="True"
                                        GroupName="s" TabIndex="10" OnCheckedChanged="rbtnConsumeYes_CheckedChanged">
                                    </asp:RadioButton>
                                    <asp:RadioButton ID="rbtnConsumeNo" runat="server" Text="No" GroupName="s" TabIndex="11"
                                        Checked="True" AutoPostBack="True" OnCheckedChanged="rbtnConsumeYes_CheckedChanged">
                                    </asp:RadioButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltxt">
                                    Is Capital Asset
                                </td>
                                <td class="tbltxt">
                                    :
                                </td>
                                <td style="width: 272px">
                                    <asp:RadioButton ID="rbtnCapitalYes" runat="server" Text="Yes" AutoPostBack="True"
                                        GroupName="C" TabIndex="12" OnCheckedChanged="rbtnCapitalYes_CheckedChanged">
                                    </asp:RadioButton>
                                    <asp:RadioButton ID="rbtnCapitalNo" runat="server" Text="No" GroupName="C" TabIndex="13"
                                        Checked="True" AutoPostBack="True" OnCheckedChanged="rbtnCapitalYes_CheckedChanged">
                                    </asp:RadioButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltxt">
                                    Applicable Tax
                                </td>
                                <td class="tbltxt">
                                    :
                                </td>
                                <td style="width: 272px">
                                    <asp:DropDownList ID="drpTax" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tbltxt">
                                    Active Status
                                </td>
                                <td class="tbltxt">
                                    :
                                </td>
                                <td class="tbltxt" style="width: 272px">
                                    <asp:RadioButton ID="rbtnStatusYes" runat="server" Text="True" AutoPostBack="True"
                                        GroupName="Sta" Checked="True" TabIndex="12"></asp:RadioButton>
                                    <asp:RadioButton ID="rbtnStatusNo" runat="server" Text="False" GroupName="Sta" TabIndex="13">
                                    </asp:RadioButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="padding-top: 5px;" colspan="2">
                        <asp:Button ID="btnSubmit" runat="server" Text="Save & Add New" Width="150px" OnClick="btnSubmit_Click"
                            OnClientClick="return isValid();" TabIndex="22" />&nbsp;
                        <asp:Button ID="btnShow" runat="server" Text="Go To List" OnClick="btnShow_Click"
                            Width="150px" TabIndex="23" />&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" OnClick="btnCancel_Click1"
                            Width="150px" TabIndex="24" />&nbsp;
                        <asp:Button ID="btnback" runat="server" Text="Cancel" OnClick="btnback_Click" Width="150px"
                            TabIndex="25" />
                    </td>
                </tr>
                <%--<tr>
                    <td class="tbltxt">
                        Warranty/AMC Validity
                    </td>
                    <td class="tbltxt">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtWarenty" runat="server" ReadOnly="False" TabIndex="15"></asp:TextBox>&nbsp;
                        <rjs:PopCalendar ID="dtpWarentyDt" runat="server" Control="txtWarenty"></rjs:PopCalendar>
                    </td>
                </tr>--%>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
           <asp:PostBackTrigger ControlID="btnShow" />
            <asp:PostBackTrigger ControlID="btnCancel" />
            <asp:PostBackTrigger ControlID="btnback" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
