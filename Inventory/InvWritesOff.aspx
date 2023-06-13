<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="InvWritesOff.aspx.cs" Inherits="Inventory_InvWritesOff" %>


<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            document.getElementById("<%=btnSave.ClientID %>").focus();
        });
        function isValid() {

            var loc = document.getElementById("<%=ddlLocation.ClientID %>").selectedIndex;
            var categoryNm = document.getElementById("<%=ddlCatName.ClientID %>").selectedIndex;
            var Item = document.getElementById("<%=ddlItem.ClientID %>").selectedIndex;
            var Qty = document.getElementById("<%=txtQty.ClientID %>").value;
            var WrtsOffDt = document.getElementById("<%=txtWriteOffDt.ClientID %>").value;

            if (loc == 0) {
                alert("Please Select Location");
                document.getElementById("<%=ddlLocation.ClientID %>").focus();
                return false;
            }
            if (categoryNm == 0) {
                alert("Please Select Category !");
                document.getElementById("<%=ddlCatName.ClientID %>").focus();
                return false;
            }
            if (Item == 0) {
                alert("Please Select Item !");
                document.getElementById("<%=ddlItem.ClientID %>").focus();
                return false;
            }
            if (Qty == "") {
                alert("Please Enter Qty !");
                document.getElementById("<%=txtQty.ClientID %>").focus();
                return false;
            }
            if (WrtsOffDt == "") {
                alert("Please Select Wrires Off Date !");
                document.getElementById("<%=txtWriteOffDt.ClientID %>").focus();
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

    <asp:UpdatePanel ID="upp1" runat="server">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Inventory Writesoff
                </h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
            </div>
            <table cellpadding="5px" cellspacing="0px" align="center" width="100%">
                <tr>
                    <td style="width: 85px">
                        Location
                    </td>
                    <td style="width: 5px;">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="tbltxtbox">
                        </asp:DropDownList>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        Category
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCatName" runat="server" CssClass="tbltxtbox" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlCatName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        Item
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlItem" runat="server" CssClass="tbltxtbox" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                            <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <span style="color: Red; font-size: small;">*</span>
                        <asp:Label ID="lblQty" runat="server" Style="color: Red; font-size: 12px;" Text=""
                            Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Quantity
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtQty" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"
                            CssClass="tbltxtbox"></asp:TextBox>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        WriteOff Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtWriteOffDt" runat="server" Width="110px" MaxLength="30" CssClass="tbltxtbox"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpWriteOffDt" runat="server" Control="txtWriteOffDt" AutoPostBack="False"
                            Format="dd mmm yyyy"></rjs:PopCalendar>
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtWriteOffDt.value='';return false;"
                            Text="Clear"></asp:LinkButton>
                        <span style="color: Red; font-size: small;">*</span>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        Description
                    </td>
                    <td valign="top">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="txtarea" Width="250px"
                            Height="80px" MaxLength="100" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Font-Bold="True" OnClientClick="return isValid();"
                            Width="80px" OnClick="btnSave_Click" />&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Font-Bold="True" Width="80px"
                            OnClick="btnCancel_Click" />&nbsp;
                        <asp:Button ID="btnShow" runat="server" Text="Show List" Font-Bold="True" Width="90px"
                            OnClick="btnShow_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
