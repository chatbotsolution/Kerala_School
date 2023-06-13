<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="InventoryReturn.aspx.cs" Inherits="Hostel_InventoryReturn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <script type="text/javascript" language="javascript">

        function IsValid() {
            var returnby = document.getElementById("<%=drpReturnedBy.ClientID %>").selectedIndex;

            if (returnby == 0) {
                alert("Please Select Student !");
                document.getElementById("<%=drpReturnedBy.ClientID %>").focus();
                return false;
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

        function GridCheck() {
            for (i = 0; i < grd_returnQty.length; i++) {
                var returnQty = document.getElementById(grd_returnQty[i]);
                var MaxQtyToReturn = document.getElementById(grd_MaxQtyToReturn[i]);

                if (returnQty.value != "") {
                    if (returnQty.value > MaxQtyToReturn.value) {
                        alert("Return Quantity Must not be greater than Maximum quantity to Return");
                        returnQty.focus();
                        return false;
                    }
                }
            }
            return true;
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
                            <strong>Items return From :</strong>
                            <asp:DropDownList ID="drpReturnedBy" runat="server" CssClass="tbltxtbox" Width="150px">
                                <asp:ListItem Text="--SELECT--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                            <strong>To :</strong>
                            <asp:DropDownList ID="drpReturnTo" runat="server" CssClass="tbltxtbox" Width="140px">
                            </asp:DropDownList>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" onclick="btnSearch_Click" OnClientClick="return IsValid();" />                                      
                        </td>
                    </tr>
                </table>
            </div>            
            <asp:GridView ID="grdItem" runat="server" AutoGenerateColumns="False" CssClass="mGrid" Width="100%"                 
                onprerender="grdItem_PreRender" >                
                <Columns>
                    <asp:TemplateField HeaderText="Item Name">
                        <ItemTemplate>
                            <%#Eval("ItemName")%>
                            <asp:HiddenField ID="hdnItemCode" runat="server" Value='<%#Eval("ItemCode")%>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Max Qty To Return">
                        <ItemTemplate>
                            <%#Eval("AvlQty")%>
                            <asp:HiddenField ID="hdnMaxQtyToReturn" runat="server" Value='<%#Eval("AvlQty")%>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Return Qty">                        
                        <ItemTemplate>
                            <asp:TextBox ID="txtReturnQty" runat="server" Width="80px" onkeypress="return blockNonNumbers(this, event, true, false);" MaxLength="6"></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="100px"/>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"/>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No Items To Return
                </EmptyDataTemplate>
                <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <%--<RowStyle BackColor="#EFEFEF" />--%>
            </asp:GridView>
            <div align="right">
                <asp:Button ID="btnReturn" runat="server" Text="Return" Visible="false" onclick="btnReturn_Click"/>                    
            </div>
        </ContentTemplate>   
    </asp:UpdatePanel>
</asp:Content>
