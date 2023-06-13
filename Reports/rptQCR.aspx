<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptQCR.aspx.cs" Inherits="Reports_rptQCR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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


        function valid() {
            var Class = document.getElementById("<%=drpClass.ClientID %>").value;
            if (Class == "0") {
                alert("Please Select Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
           Quarterly Collection Report(QCR)
        </h2>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
<%--    <asp:UpdatePanel ID="upp" runat="server">
        <ContentTemplate>--%>
            <table width="100%" cellspacing="2" cellpadding="2" class="cnt-box tbltxt">
                <tr>
                    <td>
                        Session :
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                             >
                        </asp:DropDownList>
                        &nbsp;&nbsp; Class :
                        <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                            CssClass="vsmalltb">
                        </asp:DropDownList>
                        &nbsp;&nbsp; Section :
                        <asp:DropDownList ID="ddlSection" runat="server" CssClass="vsmalltb"  >
                            <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Button ID="btngo" runat="server" Text="Search" ToolTip="Click to Search Student Details"
                            OnClick="btngo_Click" OnClientClick="return valid();" />
                        <asp:Button ID="btnExpExcel" runat="server" Text="Export to Excel" Enabled="false"
                            OnClick="btnExpExcel_Click"  />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="false" OnClick="btnPrint_Click" />
                    </td>
                </tr>
            </table>
             <div class="spacer"></div>
            <div class="lbl2 tbltxt cnt-box2">
            <asp:Label ID="lblreport" runat="server"></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../Images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
            </div>
        <%--  </ContentTemplate>
      <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btnExpExcel" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExpExcel" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>

