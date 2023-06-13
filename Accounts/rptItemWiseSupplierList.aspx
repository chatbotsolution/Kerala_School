<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptItemWiseSupplierList.aspx.cs" Inherits="Accounts_rptItemWiseSupplierList" %>

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

        function isValid() {

            var Cat = document.getElementById("<%=drpCat.ClientID %>").value;



            if (Cat == 0) {
                alert("Please Select Catagory !");
                document.getElementById("<%=drpCat.ClientID %>").focus();
                return false;
            }

            else {
                return true;
            }
        }

    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnPrint" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="4">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor">
                            <h1>
                                Item Wise
                            </h1>
                            <h2>
                                Supplier List</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <strong>
                            <asp:Label ID="lblCat" runat="server" Text="Catagory : "></asp:Label><span class="mandatory">*</span>
                            <asp:DropDownList ID="drpCat" runat="server" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="drpCat_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Label ID="lblItem" runat="server" Text="Item : "></asp:Label>
                            <asp:DropDownList ID="drpItem" runat="server" TabIndex="2">
                            </asp:DropDownList>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Button ID="btnView" runat="server" TabIndex="3" Text="View List" OnClick="btnView_Click"
                            OnClientClick="return isValid();" />
                        &nbsp;
                        <asp:Button ID="btnPrint" Text="Print" TabIndex="4" runat="server" OnClick="btnPrint_Click" />
                        &nbsp;
                        <asp:Button ID="BtnExcel" TabIndex="5" runat="server" Text="Export to Excel" Visible="false"
                            OnClick="btnExpExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblReport" runat="server"></asp:Label>
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

