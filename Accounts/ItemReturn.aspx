<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemReturn.aspx.cs" Inherits="Accounts_ItemReturn" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Return Item</title>

    <script src="../Scripts/CommonScript.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function IsValid() {
            var purDt = document.getElementById("txtReturnDt").value;
            var invNo = document.getElementById("txtreturnInvNo").value;
            var rmks = document.getElementById("txtRemarks").value;

            if (purDt.trim() == "") {
                alert("Select Return Date");
                document.getElementById("txtReturnDt").focus();
                return false;
            }
            if (invNo.trim() == "") {
                alert("Enter Return Invoice Numeber");
                document.getElementById("txtreturnInvNo").focus();
                return false;
            }
            if (rmks.trim() == "") {
                alert("Enter Remarks");
                document.getElementById("txtRemarks").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script>
        window.onunload = refreshParent;
        function refreshParent() {
            window.opener.location.reload();
        }
</script>

    <style type="text/css">
        .innertbltxt
        {
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
            font-size: 13px;
            color: #000;
            padding: 3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center">
        <div>
            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
        </div>
        <div id="dv1" runat="server" style="width: 598px; background-color: #f5f5f5; border: 1px solid #CCC;
            height: auto; overflow: auto; overflow-x: hidden; text-align: left; margin-bottom: 5px">
            <div style="width: 100%; float: left;" class="innertbltxt">
                &nbsp;Return Date&nbsp;:&nbsp;
                <asp:TextBox ID="txtReturnDt" runat="server" Width="80px" TabIndex="1" ReadOnly="true"></asp:TextBox>
                <rjs:PopCalendar ID="dtpReturnDt" runat="server" Control="txtReturnDt" AutoPostBack="False"
                    Format="dd mmm yyyy" To-Today="true"></rjs:PopCalendar>
                &nbsp;&nbsp; Return Invoice :<asp:TextBox ID="txtreturnInvNo" runat="server" 
                    TabIndex="2"></asp:TextBox>
            </div>
        </div>
        <div style="width: 600px; background-color: #f5f5f5; height: auto; overflow: auto;">
            <asp:GridView ID="gvStockDtls" runat="server" Width="100%" 
                AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="CatName" HeaderText="Category">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Item">
                        <ItemTemplate>
                            <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName")%>'></asp:Label>
                            <asp:HiddenField ID="hdnItemCode" runat="server" Value='<%#Eval("ItemCode") %>' />
                            <asp:HiddenField ID="hdnPurDtlId" runat="server" Value='<%#Eval("PurchaseDetailId") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity To Return">
                        <ItemTemplate>
                            <asp:TextBox ID="txtReturnQty" runat="server" Width="50px" Text='<%#Eval("BalQty") %>'
                                onkeypress="return blockNonNumbers(this, event, true, false);" TabIndex="3"></asp:TextBox>
                            <asp:HiddenField ID="hdnRcvQty" runat="server" Value='<%#Eval("BalQty") %>' />
                            <asp:HiddenField ID="hdnUniPurPrice" runat="server" Value='<%#Eval("Unit_PurPrice") %>' />
                            <asp:HiddenField ID="hfLandingCost" runat="server" Value='<%#Eval("UnitLandingCost") %>' />
                            <asp:Label ID="lblMeasuringUnit" runat="server" Text='<%#Eval("MesuringUnit")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="120px" />
                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No Items To Return
                </EmptyDataTemplate>
                <EmptyDataRowStyle />
            </asp:GridView>
        </div>
        <div id="dv2" runat="server" style="width: 598px; background-color: #f5f5f5; border: 1px solid #CCC;
            height: auto; overflow: auto; text-align: left;">
            <div style="width: 60px; float: left;" class="innertbltxt">
                Remarks :</div>
            <div style="float: left; margin: 5px 0px 5px 0px" class="innertbltxt">
                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="520px" 
                    Height="80px" TabIndex="4"></asp:TextBox>
                <%--<ajaxToolkit:TextBoxWatermarkExtender ID="txtwe" runat="server" TargetControlID="txtRemarks"
                    WatermarkText="Enter Remarks(Within 200 Characters)">
                </ajaxToolkit:TextBoxWatermarkExtender>--%>
            </div>
        </div>
        <div class="spacer">
            <img src="../Images/mask.gif" height="10" width="10" /></div>
        <div>
            <asp:Button ID="btnReturn" runat="server" Text="Return" OnClientClick="return IsValid()"
                OnClick="btnReturn_Click" onfocus="active(this);" onblur="inactive(this);" 
                TabIndex="5" />
            <asp:Button ID="btnCancel" runat="server" Text="Close" OnClientClick="return self.close()"
                onfocus="active(this);" onblur="inactive(this);" TabIndex="6" 
                 />
        </div>
    </div>
    </form>
</body>
</html>
