<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptStockRecvd.aspx.cs" Inherits="Accounts_rptStockRecvd" %>

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

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=500,left = 390,top = 184');");
        } 
    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnPrint" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td align="left" valign="middle" style="background-color: ; width: 260px;">
                        <div>
                            <h1>
                                Purchase
                            </h1>
                            <h2>
                                Book</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <strong>
                            <asp:Label ID="lblCat" runat="server" Text="Catagory Name : "></asp:Label>
                            <asp:DropDownList ID="drpCat" runat="server" TabIndex="1" OnSelectedIndexChanged="drpCat_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Label ID="lblItem" runat="server" Text="Item Name : "></asp:Label>
                            <asp:DropDownList ID="drpItem" runat="server" TabIndex="2">
                            </asp:DropDownList>
                            &nbsp; </strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="5px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <strong>
                            <asp:Label ID="lblFromdt" runat="server" Text="From Date : "></asp:Label>
                            <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" CssClass="tbltxtbox" Width="80px"
                                TabIndex="3"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFromDt" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="lnkbtnFrmDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFromDt.value='';return false;"
                                Text="Clear"></asp:LinkButton>
                            &nbsp;
                            <asp:Label ID="lblToDt" runat="server" Text="To Date : "></asp:Label>
                            <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" CssClass="tbltxtbox" Width="80px"
                                TabIndex="4"></asp:TextBox>
                            <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" AutoPostBack="False"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                            <asp:LinkButton ID="lnkbtnToDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtToDt.value='';return false;"
                                Text="Clear"></asp:LinkButton>
                        </strong>
                        <asp:Button ID="btnView" runat="server" TabIndex="5" Text="View List" OnClick="btnView_Click" />
                        <asp:Button ID="btnPrint" Text="Print" TabIndex="6" runat="server" OnClick="btnPrint_Click" />
                        <asp:Button ID="BtnExcel" TabIndex="7" runat="server" Text="Export to Excel" Visible="false"
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

