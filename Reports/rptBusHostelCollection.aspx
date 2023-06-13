<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptBusHostelCollection.aspx.cs" Inherits="Reports_rptBusHostelCollection" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function valShow() {
            var session = document.getElementById("<%=drpClass.ClientID %>").value;
            if (session == "0") {
                alert("Please select a class !");
                document.getElementById("<%=drpClass.ClientID %>").focus;
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

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Bus/Hostel Fee Collection
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="tbltxt">
                        Session :
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                            TabIndex="1">
                        </asp:DropDownList>
                        &nbsp; Class :
                        <asp:DropDownList ID="drpClass" runat="server" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                            AutoPostBack="true" CssClass="vsmalltb" TabIndex="2">
                        </asp:DropDownList>
                        &nbsp;&nbsp; From Date :
                        <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" CssClass="tbltxtbox" Width="80px"
                            TabIndex="4"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpfrmDt" runat="server" Control="txtFromDt" AutoPostBack="False"
                            Format="dd mmm yyyy"></rjs:PopCalendar>
                        &nbsp; To Date :
                        <asp:TextBox ID="txtToDt" runat="server" CssClass="tbltxtbox" ReadOnly="true" Width="80px"
                            TabIndex="5"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="False"
                            Format="dd mmm yyyy"></rjs:PopCalendar>
                        <asp:RadioButton ID="rBtnBus" Text="Bus" runat="server" Checked="True" GroupName="FT" />
                        &nbsp;<asp:RadioButton ID="rBtnHostel" Text="Hostel" runat="server" GroupName="FT" />
                        &nbsp;
                        <asp:Button ID="btnShow" OnClick="btnShow_Click" CausesValidation="false" runat="server"
                            Text="Show" ToolTip="Click to show student list." TabIndex="5"></asp:Button>
                        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="8" />
                        <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                            TabIndex="9" />
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt" align="center">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="error"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="tbltxt">
                        &nbsp;<asp:Label ID="lblReport" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShow" />
            <asp:PostBackTrigger ControlID="btnExpExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

