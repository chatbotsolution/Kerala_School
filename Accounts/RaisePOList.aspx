<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="RaisePOList.aspx.cs" Inherits="Accounts_RaisePOList" %>

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

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=500,height=240,left = 490,top = 184');");
        }
        
    </script>

    <asp:UpdatePanel ID="upp" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                                Purchase Order
                            </h1>
                            <h2>
                                List</h2>
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
                            <div style="width: 80%; float: left; margin-top:5px;">
                                &nbsp;<strong>From Date&nbsp;:</strong>&nbsp;
                                <asp:TextBox Width="80px" ID="txtFrmDt" runat="server" ></asp:TextBox>
                                <rjs:PopCalendar ID="dtpFromDt" runat="server" Control="txtFrmDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                                <asp:LinkButton ID="lnkbtnClearFrmDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFrmDt.value='';return false;"
                    Text="Clear" ></asp:LinkButton>
                                &nbsp;&nbsp;<strong>To Date&nbsp;:</strong>&nbsp;
                                <asp:TextBox Width="80px" ID="txtToDt" runat="server" ></asp:TextBox>
                                <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                                <asp:LinkButton ID="lnkbtnClearToDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtToDt.value='';return false;"
                    Text="Clear" ></asp:LinkButton>                                
                            </div>
                            <div style="float: right; width: 20%; text-align: right;">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" Width="100px" OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" Width="150px" OnClick="btnAdd_Click" />
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <asp:GridView ID="gvPOList" runat="server" Width="100%" AutoGenerateColumns="false"
                            PageSize="15" AllowPaging="true" OnRowCommand="gvPOList_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="PartyName" HeaderText="Supplier Name">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Address" HeaderText="Address">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Mobile" HeaderText="Mobile">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PODate" HeaderText="OrderDt">
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ExpDeliveryDt" HeaderText="Exp Delivery">
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RcvDt" HeaderText="ReceiveDt">
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <a href="javascript:popUp('ViewOrderDtls.aspx?POId=<%#Eval("PurOrderId")%>&Party=<%#Eval("PartyName")%>&POdt=<%#Eval("PODate")%>')">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon_view.gif" ToolTip="View Order Details" /></a>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Cancel" ImageUrl="~/Images/icon_delete.gif"
                                            ToolTip="Delete Purchase Order" CommandName="Remove" CommandArgument='<%#Eval("PurOrderId") %>' 
                                            OnClientClick="return confirm('Are you Sure To Delete Purchase Order ?')" />
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Purchase Order Available
                            </EmptyDataTemplate>
                            <EmptyDataRowStyle />
                        </asp:GridView>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

