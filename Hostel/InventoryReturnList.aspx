<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="InventoryReturnList.aspx.cs" Inherits="Hostel_InventoryReturnList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            document.getElementById("<%=txtFrmDt.ClientID %>").focus();
        });

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=500,left = 390,top = 150');");
        }               
    
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Return Details
                </h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px;">
                <table width="100%" cellspacing="0" cellpadding="0" class="tbltxt">
                    <tr>
                        <td align="left">
                            <strong>From:</strong>
                            <asp:TextBox ID="txtFrmDt" runat="server" Width="80px" CssClass="tbltxtbox"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtFrmDt" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFrmDt.value='';return false;"
                                Text="Clear"></asp:LinkButton>&nbsp;&nbsp; <strong>To:</strong>
                            <asp:TextBox ID="txtTo" runat="server" Width="80px" CssClass="tbltxtbox"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtTo" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtTo.value='';return false;"
                                Text="Clear"></asp:LinkButton>
                            &nbsp;&nbsp;&nbsp; <strong>Returned By:</strong>
                            <asp:DropDownList ID="drpReturnedBy" runat="server" CssClass="tbltxtbox" Width="150px">
                                <asp:ListItem Text="--SELECT--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSearch" runat="server" Text="Search" Width="65px" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnAddNew" runat="server" Text="Add New" Width="65px" OnClick="btnAddNew_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:GridView ID="grdItem" runat="server" AllowPaging="true" AlternatingRowStyle-CssClass="alt"
                AutoGenerateColumns="False" CssClass="mGrid" OnPageIndexChanging="grdItem_PageIndexChanging"
                PagerStyle-CssClass="pgr" PageSize="15" Width="100%" OnPreRender="grdItem_PreRender">
                <Columns>                                        
                    <%--<asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Cancel" ImageUrl="~/Images/icon_delete.gif"
                                ToolTip="Delete Purchase Order" CommandName="Remove" CommandArgument='<%#Eval("ReturnId") %>' 
                                OnClientClick="return confirm('Are you Sure To Cancel Return ?')" />
                        </ItemTemplate>
                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Returned By">
                        <ItemTemplate>
                            <%#Eval("ReturnedBy")%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Return Date">
                        <ItemTemplate>
                            <%#Eval("RDATE")%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item Name">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%#Eval("ItemName")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <%#Eval("Qty")%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right" Width="60px" />
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="60px" />
                    </asp:TemplateField>
                </Columns>
                <RowStyle BackColor="#EFEFEF" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

