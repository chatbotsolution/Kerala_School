<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="StockWritesoff.aspx.cs" Inherits="Accounts_StockWritesoff" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
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

        function validateSubmit() {
            var Date = document.getElementById("<%=txtWritesOffDt.ClientID%>").value;
            var OthBy = document.getElementById("<%=txtAuthorizedBy.ClientID%>").value;
            var Reason = document.getElementById("<%=txtReason.ClientID%>").value;
            if (Date.trim() == "") {
                alert("Please provide WritesOff Date !");
                document.getElementById("<%=txtWritesOffDt.ClientID%>").focus();
                return false;
            }
            if (OthBy.trim() == "") {
                alert("Please specify Authorization !");
                document.getElementById("<%=txtAuthorizedBy.ClientID%>").focus();
                return false;
            }
            if (Reason.trim() == "") {
                alert("Please specify Reason !");
                document.getElementById("<%=txtReason.ClientID%>").focus();
                return false;
            }
            else {
                return true;
            }
        }

        function validateSearch() {
            var Date = document.getElementById("<%=txtWritesOffDt.ClientID%>").value;
            if (Date.trim() == "") {
                alert("Please provide WritesOff Date !");
                document.getElementById("<%=txtWritesOffDt.ClientID%>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>       
    
    <asp:UpdatePanel ID="upp" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle">
                        <div>
                            <h1>
                                Stock
                            </h1>
                            <h2>
                                WritesOff</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <div style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: 40px;
                            overflow: auto;">
                            <div style="width: 90%; float: left; margin-top:10px;">
                                  &nbsp;Brand&nbsp;:
                                <asp:DropDownList ID="drpBrand" runat="server" TabIndex="1">
                                </asp:DropDownList>
                                &nbsp;<strong>Category&nbsp;:</strong>&nbsp;
                                <asp:DropDownList runat="server" ID="ddlCategory" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                                &nbsp;Class&nbsp;:&nbsp;
                                    <asp:DropDownList ID="drpClass" runat="server" TabIndex="3" Enabled="false">
                                 </asp:DropDownList>
                                 &nbsp;
                                WritesOff Date&nbsp;:&nbsp;
                                                <asp:TextBox ID="txtWritesOffDt" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>   
                                                <rjs:PopCalendar ID="dtpWritesOff" runat="server" 
                                      Control="txtWritesOffDt" Format="dd MMM yyyy" To-Today="true" AutoPostBack="True"
                                      onselectionchanged="dtpWritesOff_SelectionChanged">
                                                </rjs:PopCalendar> 
                                  &nbsp;
                                 <asp:Button ID="btnSearch" runat="server" Text="Search" Width="100px" OnClientClick="return validateSearch();"
                                      onclick="btnSearch_Click" />
                                 &nbsp;
                                  <asp:Button ID="btnView" runat="server" Text="Go To List" Width="100px" 
                                    onclick="btnView_Click" />
                                <%--&nbsp;<strong>Item&nbsp;:</strong>&nbsp;                                                                
                                <asp:DropDownList runat="server" ID="ddlItem" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                                    <asp:ListItem Text="--No Items--" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="spacer">
                            <img src="../Images/mask.gif" height="10" width="10" /></div>
                        <asp:Panel ID="pnl1" runat="server" >
                            <table width="100%">
                                <tr>
                                    <td>
                                        <div id="dv1" runat="server" style="width: 100%; background-color: #f5f5f5; border: 1px solid #CCC; height: 40px;
                                            overflow: auto; text-align: left;">
                                            <div style="width: 100%; float: left; height: 40px;padding-top:10px;">
                                                
                                                &nbsp;Authorized By&nbsp;:&nbsp;
                                                <asp:TextBox ID="txtAuthorizedBy" runat="server" Width="300px" MaxLength="50"></asp:TextBox>
                                                 
                                                <%--<rjs:PopCalendar ID="dtpReturnDt" runat="server" Control="txtReturnDt" AutoPostBack="False"
                                                    Format="dd mmm yyyy"></rjs:PopCalendar>--%>
                                                <%--<img src="../PopCalendar/Calendar.gif" style="cursor: pointer; vertical-align: middle;" onclick="displayCalendar(document.form1.txtReturnDt,'mm-dd-yyyy',this)" />--%>                                               
                                               <%-- <div id="divCalendar1" style="position: absolute;">
                                                    <asp:Calendar ID="cdrCalendar" runat="server" BackColor="#FFFFCC" BorderColor="#FFCC66"
                                                        BorderWidth="1px" DayNameFormat="Shortest" FirstDayOfWeek="Monday" Font-Names="Verdana"
                                                        Font-Size="8pt" ForeColor="#663399" Height="150px" Width="186px" TargetControlID="TBIhaleBaslamaTarihi"
                                                        Visible="False" OnSelectionChanged="cdrCalendar_SelectionChanged" ShowGridLines="True"
                                                        SelectionMode="Day">
                                                        <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                                                        <SelectorStyle BackColor="#FFCC66" />
                                                        <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                                                        <OtherMonthDayStyle ForeColor="#CC9966" />
                                                        <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                                                        <DayHeaderStyle BackColor="#FFCC66" Height="1px" Font-Bold="True" />
                                                        <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" ForeColor="#FFFFCC" />
                                                    </asp:Calendar>
                                                </div>--%>
                                            </div>
                                        </div>                                        
                                        <asp:GridView ID="gvItemList" runat="server" Width="100%" AutoGenerateColumns="false">
                                            <Columns>
                                                <%--<asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <input type="checkbox" name="Checkb" value='<%#Eval("ItemCode") %>' />
                                                        <asp:HiddenField ID="hdnItemCode" runat="server" Value='<%#Eval("ItemCode") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20px" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Item">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName")%>'></asp:Label>&nbsp;                                                        
                                                        <asp:HiddenField ID="hdnPurDtlId" runat="server" Value='<%#Eval("PurchaseDetailId") %>'/>
                                                        <asp:HiddenField ID="hfItemCode" runat="server" Value='<%#Eval("ItemCode") %>'/>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Avl Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAvlQty" runat="server" Text='<%#Eval("BalQty")%>'></asp:Label>&nbsp;
                                                        <asp:Label ID="lblMeasuringUnit" runat="server" Text='<%#Eval("MesuringUnit")%>'></asp:Label>
                                                        <asp:Label ID="lblPurPrice" Visible="false" runat="server" Text='<%#Eval("Unit_PurPrice")%>'></asp:Label>                                                       
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="WritesOff Qty">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtWritesOffQty" runat="server" Width="50px" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                                                        <asp:Label ID="lblUnit" runat="server" Text='<%#Eval("MesuringUnit")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PurchaseDt" HeaderText="Purchase Dt">
                                                    <ItemStyle HorizontalAlign="Left" Width="80px"/>
                                                    <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="InvNo" HeaderText="Invoice No.">
                                                    <ItemStyle HorizontalAlign="Left" Width="80px"/>
                                                    <HeaderStyle HorizontalAlign="Left" Width="80px"/>
                                                </asp:BoundField>
                                                <%--<asp:BoundField DataField="PStatus" HeaderText="Status">
                                                    <ItemStyle HorizontalAlign="Left" Width="60px"/>
                                                    <HeaderStyle HorizontalAlign="Left" Width="60px"/>
                                                </asp:BoundField>--%>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No Items Available
                                            </EmptyDataTemplate>
                                            <EmptyDataRowStyle />
                                        </asp:GridView>
                                        <div id="dv2" runat="server" style="width:100%; background-color: #f5f5f5; border: 1px solid #CCC; height: auto;
                                            overflow: auto; text-align: left;">
                                            <div style="width: 60px; float: left;" class="innertbltxt">
                                                Reason :</div>
                                            <div style="float: left; margin: 5px 0px 5px 0px" class="innertbltxt">
                                                <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Width="520px" Height="80px"></asp:TextBox>
                                                <ajaxToolkit:TextBoxWatermarkExtender ID="txtwe" runat="server" TargetControlID="txtReason"
                                                    WatermarkText="Enter Remarks(Within 200 Characters)">
                                                </ajaxToolkit:TextBoxWatermarkExtender>
                                            </div>
                                        </div>                                                                                  
                                    </td>
                                                 
                                </tr>
                                <tr>
                                    <td valign="top" style="width: 50%; text-align:right;">
                                    <div style='float:left'><asp:Label runat="server" ID="lblMsg2" Text=""></asp:Label></div>
                                        <asp:Button ID="btnWritesOff" runat="server" Text="Click To WritesOff" Visible="false" onclick="btnWritesOff_Click" OnClientClick="return validateSubmit();" />                                            
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click" />                                            
                                    </td>
                                    
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <%--<ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../Images/loading.gif" />
                    <span>Loading ...</span>
                </div>--%>
            </asp:Panel>
        </ContentTemplate>        
    </asp:UpdatePanel>
</asp:Content>
