<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptSiblingDtl.aspx.cs" Inherits="Reports_rptSiblingDtl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function isValid() {
            var name = document.getElementById("<%=drpclass.ClientID %>").value;


            if (name == 0) {
                alert("Please Select Class !");
                document.getElementById("<%=drpclass.ClientID %>").focus();
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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlSummary" runat="server">
                <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                    <img src="../images/icon_rep.jpg" width="29" height="29">
                </div>
                <div style="padding-top: 5px;">
                    <h2>
                        Sibling Details
                    </h2>
                </div>
                <div class="spacer">
                    <img src="../images/mask.gif" height="8" width="10" />
                </div>
                <table width="100%" cellspacing="2" cellpadding="2">
                    <tr>
                        <td width="70" class="tbltxt">
                            Select Class
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="110" class="tbltxt">
                            <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                                CssClass="smalltb" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="80">
                            Select Section
                        </td>
                        <td class="tbltxt" width="5">
                            :
                        </td>
                        <td class="tbltxt" width="90">
                            <asp:DropDownList ID="ddlSection" runat="server" CssClass="vsmalltb" TabIndex="2">
                            </asp:DropDownList>
                        </td>
                        <td rowspan="2" valign="bottom" class="tbltxt">
                            <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" Text="Search" OnClientClick="return isValid();"
                                ToolTip="Click to Show Defaulter Students" TabIndex="3" />
                            <asp:Button ID="btnPrint" Text="Print" runat="server" OnClick="btnPrint_Click" TabIndex="6" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="padding-top: 10px;">
                <asp:Label ID="lblReport" runat="server"></asp:Label>
                <asp:Label ID="lblGrdMsg" runat="server" CssClass="error"></asp:Label>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

