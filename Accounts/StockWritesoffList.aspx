<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="StockWritesoffList.aspx.cs" Inherits="Accounts_StockWritesoffList" %>

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
        
    </script>
    
    <asp:UpdatePanel ID="upp" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                                WritesOff
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
                        <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: 40px;
                            overflow: auto;">
                            <div style="width: 80%; float: left; margin:10px 0px 0px 0px">
                                &nbsp;<strong>From Date&nbsp;:</strong>&nbsp;
                                <asp:TextBox Width="80px" runat="server" ID="txtFromDt"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpFromDt" runat="server" Control="txtFromDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                                <asp:LinkButton ID="lnkbtnClearFromDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFromDt.value='';return false;"
                    Text="Clear" ></asp:LinkButton>
                                &nbsp;&nbsp;<strong>To Date&nbsp;:</strong>&nbsp;
                                <asp:TextBox Width="80px" runat="server" ID="txtToDt"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                                <asp:LinkButton ID="lnkbtnClearToDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtToDt.value='';return false;"
                    Text="Clear" ></asp:LinkButton>
                    
                                <asp:Button ID="btnSearch" runat="server" Text="Search" onclick="btnSearch_Click" />
                                
                                <asp:Button ID="btnAddNew" runat="server" Text="Add New" onclick="btnAddNew_Click" />
                                <%--&nbsp;&nbsp;<strong>Category&nbsp;:</strong>&nbsp;
                                <asp:DropDownList runat="server" ID="ddlCategory" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:DropDownList runat="server" ID="ddlItem" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                                    <asp:ListItem Text="--No Items--" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gvWritesOffList" runat="server" Width="100%" 
                            AutoGenerateColumns="false" onrowcommand="gvWritesOffList_RowCommand" 
                            ondatabound="gvWritesOffList_DataBound" >                                                
                        <Columns>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Cancel" ImageUrl="~/Images/icon_delete.gif"
                                        ToolTip="Delete" CommandName="Remove" CommandArgument='<%#Eval("WritesoffId") %>'
                                      Visible="false"  OnClientClick="return confirm('Are you Sure To cancel Writesoff Quantity ?')" />
                                </ItemTemplate>
                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>  
                            <%--<asp:BoundField DataField="Category" HeaderText="Category">
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="ItemName" HeaderText="Item">
                                <ItemStyle HorizontalAlign="Left" Width="200px"/>
                                <HeaderStyle HorizontalAlign="Left" Width="200px"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="WritesOffDt" HeaderText="WritesOffDt">
                                <ItemStyle HorizontalAlign="Left" Width="80px"/>
                                <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                            </asp:BoundField>                                             
                            <asp:TemplateField HeaderText="Quantity">
                                <ItemTemplate>
                                    <%#Eval("Qty")%>            
                                    <%#Eval("MesuringUnit")%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Reason" HeaderText="Reason">
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AuthorizedBy" HeaderText="Authorized By">
                                <ItemStyle HorizontalAlign="Left" Width="200px"/>
                                <HeaderStyle HorizontalAlign="Left" Width="200px"/>
                            </asp:BoundField>                                                                                                                
                            <%--<asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ImageUrl="~/Images/icon_edit.gif"
                                        ToolTip="Edit" CommandName="Modify" CommandArgument='<%#Eval("ItemCode") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>--%>                                                                            
                                                      
                        </Columns>
                        <EmptyDataTemplate>
                            No Record
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


