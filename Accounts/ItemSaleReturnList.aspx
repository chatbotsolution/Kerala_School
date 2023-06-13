<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="ItemSaleReturnList.aspx.cs" Inherits="Accounts_ItemSaleReturnList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<script src="../Scripts/ModalPopups.js" type="text/javascript" language="javascript"></script>

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

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=500,height=250,left = 490,top = 184');");
        }

        function ModalPopupsAlert1(Detail, DetailDes) {
            ModalPopups.Alert("jsAlert1", Detail,
                "<div style='padding:25px;width:400px;height:150px'>" + DetailDes + "</div>",
                {
                    okButtonText: "Close"
                }
            );
        }
    </script>

   <%-- <asp:UpdatePanel ID="upp" runat="server">
        <ContentTemplate>--%>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                               Sale
                            </h1>
                            <h2>
                                Return List</h2>
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
                            &nbsp;
                           
                            </div>
                    </td>
                </tr>
                 <tr>
            <td colspan="2" style="font-weight: bold; padding: 3px;">&nbsp;
            Received From : <asp:DropDownList ID="drpstudent" 
                    runat="server" Width="200px" AutoPostBack="True"
                OnSelectedIndexChanged="drpstudent_SelectedIndexChanged" CssClass="largetb" 
                    TabIndex="5">
            </asp:DropDownList>&nbsp;&nbsp;<asp:TextBox ID="txtadmnno" 
                    runat="server" CssClass="vsmalltb" Width="90px" TabIndex="6" 
                    Visible="False"></asp:TextBox>
                    &nbsp;<asp:DropDownList 
                    ID="drpSession" runat="server" CssClass="vsmalltb" Width="70px"
                AutoPostBack="true" OnSelectedIndexChanged="drpSession_SelectedIndexChanged" 
                    TabIndex="3" Visible="False">
            </asp:DropDownList>&nbsp;<asp:DropDownList ID="drpclass" 
                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                CssClass="vsmalltb" Width="75px" TabIndex="4" Visible="False">
            </asp:DropDownList>&nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                TabIndex="7" onfocus="active(this);" onblur="inactive(this);" />
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" TabIndex="8"
                            onfocus="active(this);" onblur="inactive(this);" />
                           
                     </td>
        </tr>
                <tr>
                    <td colspan="2">
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                            overflow: auto;">
                            <asp:GridView ID="gvStockList" runat="server" Width="100%" AutoGenerateColumns="False"
                                PageSize="15" AllowPaging="True" OnRowCommand="gvStockList_RowCommand" 
                                OnPageIndexChanging="gvStockList_PageIndexChanging" 
                                ondatabound="gvStockList_DataBound">
                                <Columns>
                                    <%--<asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <a href="ReceiveStockPOEdit.aspx?PId=<%#Eval("PurchaseId") %>">
                                                <img src="../images/icon_edit.gif" alt="Edit" title="Edit" /></a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Cancel" ImageUrl="~/Images/icon_delete.gif"
                                                ToolTip="Delete Stock" CommandName="Remove" CommandArgument='<%#Eval("PurchaseId") %>'
                                                OnClientClick="return confirm('You are going Delete Stock. All related records will be Deleted. Do you want to continue ?')"
                                                TabIndex="9" Visible="false"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RcvDt" HeaderText="Receive Dt">
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PartyName" HeaderText="Received From">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TotBillAmt" HeaderText="Total Amount" 
                                        DataFormatString="{0:f2}">
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Details">
                                        <ItemTemplate>
                                            <a href="javascript:popUp('ViewSaleReturnList.aspx?PId=<%#Eval("PurchaseId")%>&Invoice=<%#Eval("InvNo") %>')">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icon_view.gif" ToolTip="View Sale Return Details" /></a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="60px" />
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
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../Images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

