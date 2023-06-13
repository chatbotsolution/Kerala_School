<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptAdmnSum.aspx.cs" Inherits="Reports_rptAdmnSum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" language="javascript">
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
<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">--%>
      <%--  <ContentTemplate>--%>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_rep.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Admission Summary
                </h2>
            </div>
            
            <fieldset id="fsCons" runat="server" class="cnt-box">
                <table width="100%" border="0" cellspacing="2" cellpadding="2">
                    <tr>
                        <td class="tbltxt" align="left" colspan="10">
                         &nbsp;<asp:RadioButton ID="rbtnAll" runat="server" GroupName="Chk1" Text="All"
                                CssClass="tbltxt" AutoPostBack="True" 
                                oncheckedchanged="rbtnAll_CheckedChanged"  />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="rbtnExisting" runat="server" GroupName="Chk1" Text="Existing"
                                CssClass="tbltxt" AutoPostBack="True" 
                                oncheckedchanged="rbtnExisting_CheckedChanged"  />
                                &nbsp;&nbsp;
                            <asp:RadioButton ID="rbtnNew" runat="server" GroupName="Chk1" Text="New"
                                CssClass="tbltxt" AutoPostBack="True" 
                                oncheckedchanged="rbtnNew_CheckedChanged"  />
                            &nbsp;&nbsp; Class :
                            <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="true" 
                                CssClass="vsmalltb" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp;&nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td colspan="7" align="left">
                            <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" 
                                TabIndex="1" style="width: 48px" /> &nbsp;
                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="2" />&nbsp;
                            <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                                TabIndex="3" />
                        </td>
                       
                    </tr>
                </table>
            </fieldset>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <div style=" height: 450px; overflow: scroll; padding:3px;" class="cnt-box2 tbltxt">
                <asp:Label ID="lblReport" runat="server"></asp:Label>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
        <%--</ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExpExcel" />
        </Triggers>--%>
    <%--</asp:UpdatePanel>--%>
</asp:Content>
