<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inventory.master" AutoEventWireup="true" CodeFile="rptLocationwiseStockList.aspx.cs" Inherits="Inventory_rptLocationwiseStockList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            document.getElementById("<%=drpLocation.ClientID %>").focus();
        });
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Locationwise Stock</h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblmsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px">
                <table>
                    <tr>
                        <td>
                            Select Location :
                            <asp:DropDownList ID="drpLocation" runat="server" TabIndex="1" AutoPostBack="True" Width="150" CssClass="tbltxtbox"
                                OnSelectedIndexChanged="drpLocation_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp; Select Item :
                            <asp:DropDownList ID="drpItem" runat="server" TabIndex="4" CssClass="tbltxtbox" Width="150">
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return Isvalid();"
                                TabIndex="5" />
                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="6" />&nbsp;<asp:Button
                                ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click"
                                TabIndex="7" />
                            <asp:Panel ID="pnlIncharge" runat="server">
                                <asp:Label ID="lblIncharge" runat="server" ForeColor="Red" CssClass="gridtxt"></asp:Label></asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="height: 5px;">
                &nbsp;</div>
            <div>
                <asp:Label ID="lblReport" runat="server" Text=""></asp:Label></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
