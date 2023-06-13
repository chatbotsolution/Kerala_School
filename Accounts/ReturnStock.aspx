<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="ReturnStock.aspx.cs" Inherits="Accounts_ReturnStock" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

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

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=500,height=250,left = 490,top = 184');");
        }

        function popUp1(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=640,height=400,left = 490,top = 184');");
        }
        
    </script>

    <asp:UpdatePanel ID="upp" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                                Return
                            </h1>
                            <h2>
                                Against Stock Before Sale</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                            overflow: auto;">
                            &nbsp;<strong>From Date&nbsp;:</strong>&nbsp;<asp:TextBox Width="80px" runat="server"
                                ID="txtFrmDt" TabIndex="1"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpFromDt" runat="server" Control="txtFrmDt" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="lnkbtnClearFrmDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFrmDt.value='';return false;"
                                Text="Clear" TabIndex="2" onfocus="active(this);" onblur="inactive(this);"></asp:LinkButton>
                            &nbsp;<strong>To Date&nbsp;:</strong>&nbsp;<asp:TextBox Width="80px" ID="txtToDt"
                                runat="server" TabIndex="3"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="lnkbtnClearToDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtToDt.value='';return false;"
                                Text="Clear" TabIndex="4" onfocus="active(this);" onblur="inactive(this);"></asp:LinkButton>
                            &nbsp;<b>Invoice No : </b>
                            <asp:TextBox ID="txtInvoice" runat="server" TabIndex="5" Width="100px"></asp:TextBox>
                            &nbsp;<strong>Supplier&nbsp;:</strong>
                            <asp:DropDownList ID="ddlReceiveFrom" runat="server" Width="180px" TabIndex="6">
                            </asp:DropDownList>
                            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                TabIndex="7" onfocus="active(this);" onblur="inactive(this);" />
                            <asp:Button ID="btnList" runat="server" Text="Return List" OnClick="btnList_Click"
                                TabIndex="8" onfocus="active(this);" onblur="inactive(this);" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                            overflow: auto;">
                            <asp:GridView ID="gvStockList" runat="server" Width="100%" AutoGenerateColumns="false"
                                PageSize="15" TabIndex="9" AllowPaging="true" DataKeyNames="PurchaseId" OnPageIndexChanging="gvStockList_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="InvNo" HeaderText="Invoice">
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RcvDt" HeaderText="Receive Dt">
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PartyName" HeaderText="Supplier">
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TotPurAmt" HeaderText="Total PurAmt" DataFormatString="{0:f2}">
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VAT_CST_TotAmt" HeaderText="VAT/CST" DataFormatString="{0:f2}">
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShippingCharges" HeaderText="Ship Charge" DataFormatString="{0:f2}">
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AddnlChargesAmt" HeaderText="Other Charges" DataFormatString="{0:f2}">
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TotBillAmt" HeaderText="Total Bill" DataFormatString="{0:f2}">
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Item List">
                                        <ItemTemplate>
                                            <a href="javascript:popUp('ViewStockList.aspx?PId=<%#Eval("PurchaseId")%>&Invoice=<%#Eval("InvNo") %>')">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon_view.gif" ToolTip="View Details"
                                                    TabIndex="10" /></a>
                                            <asp:HiddenField ID="hdnPOID" runat="server" Value='<%#Eval("PurOrderId") %>' />
                                            <asp:HiddenField ID="hdnPID" runat="server" Value='<%#Eval("PurchaseId") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <a href="javascript:popUp1('ItemReturn.aspx?PId=<%#Eval("PurchaseId")%>&POID=<%#Eval("PurOrderId") %>')"
                                                onfocus="active(this);" onblur="inactive(this);" tabindex="10">Select</a>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Stock Received
                                </EmptyDataTemplate>
                                <EmptyDataRowStyle />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <div style="height: 50px; clear: both;">
                <img src="../Images/mask.gif" height="50" width="10" /></div>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../Images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


