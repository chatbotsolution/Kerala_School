<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="ItemPriceMod.aspx.cs" Inherits="Accounts_ItemPriceMod" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
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
    </script>

    <asp:UpdatePanel ID="upp" runat="server">
    <ContentTemplate>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr style="background-color: #ededed;">
            <td width="350" align="left" valign="middle">
                <div>
                    <h1>
                        Modify
                    </h1>
                    <h2>
                        Sale Price</h2>
                </div>
            </td>
            <td height="35" align="left" valign="middle">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="True" meta:resourcekey="lblMsgResource1"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
        overflow: auto;">
        <table width="100%">
            <tr>
                <td width="250px">
                    &nbsp;Brand :
                    <asp:DropDownList ID="drpBrand" runat="server" Width="200px">
                    </asp:DropDownList>
                </td>
                <td width="270px">
                    Category :
                    <asp:DropDownList ID="drpCategory" runat="server" Width="200px" AutoPostBack="True"
                        OnSelectedIndexChanged="drpCategory_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <asp:Panel ID="pnlclass" runat="server" Visible="False">
                    <td width="180px">
                        Class :
                        <asp:DropDownList ID="drpClass" runat="server" Width="120px">
                        </asp:DropDownList>
                    </td>
                </asp:Panel>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="CheckLoader();" />
                </td>
            </tr>
            <tr>
                <td><div class="separator"></div></td>
            </tr>
        </table>
    </div>
    <div style="width: 50%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
        overflow: auto;">
        <asp:GridView ID="grdItemPrice" runat="server" Width="100%" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="SubjectId" Visible="false">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%#Eval("ItemCode")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ItemName" HeaderText="Item" meta:resourcekey="BoundFieldResource1">
                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Unit Sale Price" meta:resourcekey="TemplateFieldResource2">
                    <ItemTemplate>
                        <asp:TextBox ID="txtUnitSalePrice" runat="server" Width="70px" Text='<%#Eval("SalePriceStr")%>'
                        onkeypress="return blockNonNumbers(this, event, true, false);" >
                        </asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                No Record
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    <div>
        <asp:Button ID="btnSubmit" runat="server" Text="Save" Visible="false" OnClick="btnSubmit_Click" OnClientClick="CheckLoader();" />
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    <div id="Loader" style="text-align: center; vertical-align: middle; position: absolute;
        top: 0px; left: 0px; z-index: 99; width: 100%; height: 100%; background-color: #ededed;
        -ms-filter: 'progid:DXImageTransform.Microsoft.Alpha(Opacity=60)'; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=60);
        -moz-opacity: 0.8; opacity: 0.8;">
        <div style="width: 48px; height: 48px; margin: 0 auto; margin-top: 275px;">
            <img src="../images/spinner.gif">
        </div>
        <div style="font-family: Trebuchet MS; font-size: 12px; color: Green; text-align: center;">
            Please Wait...</div>
    </div>
</asp:Content>
