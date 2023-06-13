<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="IssueMaster.aspx.cs" Inherits="Inventory_IssueMaster" %>


<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function isValid() {
            var item = document.getElementById("<%=drpItem.ClientID %>").value;
            var qty = document.getElementById("<%=txtQty.ClientID %>").value;
            var AvlQty = document.getElementById("<%=hfAvlQty.ClientID %>").value;

            if (item == 0) {
                alert("Please select Item !");
                document.getElementById("<%=drpItem.ClientID %>").focus();
                return false;
            }
            if (qty.trim() == "") {
                alert("Please Enter Quantity !");
                document.getElementById("<%=txtQty.ClientID %>").focus();
                return false;
            }
            if (parseFloat(qty) > parseFloat(AvlQty)) {
                alert("Quantity could not more than Available quantity!");
                document.getElementById("<%=txtQty.ClientID %>").focus();
                return false;
            }

            else {
                return true;
            }

        }
        function add() {
            var IssuDt = document.getElementById("<%=txtIssueDt.ClientID %>").value;
            var RcvdBy = document.getElementById("<%=txtIssuedto.ClientID %>").value;
            var Loc = document.getElementById("<%=drpIssuedtoLoc.ClientID %>").value;

            if (IssuDt.trim() == "") {
                alert("Please Enter Issue Date !");
                document.getElementById("<%=txtIssueDt.ClientID %>").focus();
                return false;
            }
            if (RcvdBy.trim() == "") {
                alert("Please Enter Received By !");
                document.getElementById("<%=txtIssuedto.ClientID %>").focus();
                return false;
            }
            if (Loc == 0) {
                alert("Please select Location !");
                document.getElementById("<%=drpIssuedtoLoc.ClientID %>").focus();
                return false;
            }

            else {
                return true;
            }

        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Issue Master
                </h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px;">
                <table width="100%" class="tbltxt">
                    <tr>
                        <td style="width: 145px">
                            Issue Date :
                            <asp:TextBox ID="txtIssueDt" runat="server" CssClass="tbltxtbox" ReadOnly="True"
                                TabIndex="9" Width="100px"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpdt" runat="server" Control="txtIssueDt" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtIssueDt.value='';return false;"
                                Text="Clear"></asp:LinkButton>
                            <span style="color: Red; font-size: small;">*</span>
                        </td>
                        <td style="width: 259px">
                            Issued From Location:
                            <asp:DropDownList ID="drpIssuedFrom" runat="server" CssClass="tbltxtbox" OnSelectedIndexChanged="drpIssuedFrom_SelectedIndexChanged"
                                AutoPostBack="True">
                                <asp:ListItem Text="--SELECT--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 145px">
                            Received By :
                            <asp:TextBox ID="txtIssuedto" runat="server" CssClass="tbltxtbox" TabIndex="7"></asp:TextBox>
                            <span style="color: Red; font-size: small;">*</span>
                        </td>
                        <td style="padding-top: 8px; width: 259px;" class="tabletxt">
                            Issued to Location :&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="drpIssuedtoLoc" runat="server" CssClass="tbltxtbox" 
                                >
                                <asp:ListItem Text="--SELECT--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <span style="color: Red; font-size: small;">*</span> &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                    <tr valign="bottom">
                        <td align="left" style="width: 220px" valign="bottom">
                            Item:
                            <asp:DropDownList ID="drpItem" runat="server" CssClass="tbltxtbox" AutoPostBack="True"
                                OnSelectedIndexChanged="drpItem_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span style="color: Red; font-size: small;">*</span>
                        </td>
                        <td align="left" style="width: 120px" valign="bottom">
                            Quantity:
                            <asp:TextBox ID="txtQty" runat="server" onkeypress="return blockNonNumbers(this, event, false, false);"
                                CssClass="tbltxtbox" Width="80px"></asp:TextBox>
                            <span style="color: Red; font-size: small;">*</span>
                        </td>
                        <td valign="bottom">
                            <asp:Label ID="lblAvlQty" runat="server"></asp:Label>
                            <asp:HiddenField ID="hfAvlQty" runat="server" />
                        </td>
                        <td align="right" valign="bottom">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" Font-Bold="True" Width="80px" OnClick="btnAdd_Click"
                                OnClientClick="return isValid();" Style="height: 22px" />
                            &nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Font-Bold="True" Width="80px"
                                OnClick="btnCancel_Click" Style="height: 22px" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:GridView ID="grvAdd" runat="server" AutoGenerateColumns="False" CellPadding="4"
                ForeColor="#333333" OnRowEditing="grvAdd_RowEditing" Width="100%" EmptyDataText="No Item added"
                DataKeyNames="SlNo" OnRowDeleting="grvAdd_RowDeleting" PagerStyle-CssClass="pgr"
                AlternatingRowStyle-CssClass="alt" CssClass="mGrid" OnPageIndexChanging="grvAdd_PageIndexChanging">
                <EmptyDataRowStyle Font-Bold="True" Font-Size="10pt" ForeColor="Black" Height="30px"
                    HorizontalAlign="Center" />
                <Columns>
                    <asp:CommandField HeaderText="Action" ShowEditButton="True" ButtonType="Button" ShowDeleteButton="true"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" Width="120px" />
                        <ItemStyle HorizontalAlign="Center" Width="120px" />
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="Item Name">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="lblItemName" Text='<%#Eval("ItemName")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item Code">
                        <ItemTemplate>
                            <asp:Label ID="lblItemCode" Text='<%#Eval("ItemCode")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" Text='<%#Eval("Qty")%>' runat="server"></asp:Label>
                            <asp:HiddenField ID="hfqty" runat="server" Value='<%#Eval("Qty") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Slno" Visible="False">
                        <ItemTemplate>
                            <input type="checkbox" name="Checkb" value='<%# Eval("SlNo") %>' />
                            <asp:HiddenField ID="hfSlno" runat="server" Value='<%#Eval("SlNo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle BackColor="#EFEFEF" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#FDFDFD" />
            </asp:GridView>
            <div style="width: 100%; text-align: right; margin-top: 5px;">
                <asp:Button ID="btnSave" runat="server" Text="Save" Font-Bold="True" Width="100px"
                    OnClientClick="return add();" OnClick="btnSave_Click" />&nbsp;
                <asp:Button ID="btnCancel2" runat="server" Text="Cancel" Font-Bold="True" Width="100px"
                    OnClick="btnCancel2_Click" />&nbsp;
                <asp:Button ID="btnShowDetails" runat="server" Text="Show List" Font-Bold="True"
                    Width="100px" OnClick="btnShowDetails_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
