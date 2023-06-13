<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="HostFeeComponentList.aspx.cs" Inherits="Hostel_HostFeeComponentList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
        function CnfSave() {

            if (confirm("You are going to Set Account Head . Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }

        function CnfDelete() {

            if (confirm("You are going to Delete a Record . Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }
        function ToggleAll(e) {
            if (e.checked) {
                CheckAll();
            }
            else {
                ClearAll();
            }
        }

        function CheckAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = true;
                }
            }
            ml.toggleAll.checked = true;
        }
        function ClearAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = false;
                }
            }
            ml.toggleAll.checked = false;
        }
        function valid() {
            var flag;
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {

                    if (e.checked) {
                        flag = true;
                        break;
                    }
                    else
                        flag = false;
                }
            }
            //alert(flag);
            if (flag == true) {
                CnfDelete();
                return true;
            }
            else {
                alert("Please select any record");
                return false;
            }
        }
   
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fee Head</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table width="100%" style="height: 365px;">
                <tr>
                    <td width="100%" valign="top">
                        <table width="100%">
                            <tr>
                                <td style="width: 180px;" valign="top">
                                    <asp:Button ID="btndelete" runat="server" CausesValidation="False" OnClick="btndelete_Click"
                                        OnClientClick="return valid();" Text="Delete" Width="74px" TabIndex="2" />&nbsp;
                                    <asp:Button ID="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" TabIndex="4" />
                                </td>
                                <td class="tbltxt" align="left">
                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%;" valign="top" colspan="2">
                                    <asp:GridView ID="grdFeeComponent" runat="server" AutoGenerateColumns="False" Width="100%"
                                        CssClass="mGrid" AlternatingRowStyle-CssClass="alt" DataKeyNames="FeeID" TabIndex="1"
                                        OnRowDataBound="grdFeeComponent_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                <ItemTemplate>
                                                    <input name="Checkb" type="checkbox" value='<%#Eval("FeeID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fee Name">
                                                <ItemTemplate>
                                                    <a href='HostFeeComponents.aspx?fid=<%#Eval("FeeID")%>'>
                                                        <asp:Label ID="lblFeeName" runat="server" Text='<%#Eval("FeeName")%>'></asp:Label>
                                                    </a>
                                                </ItemTemplate>
                                                <ItemStyle Width="200px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Periodicity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPeriodicityID" runat="server" Text='<%#Eval("PeriodicityType")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="90px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fine Applicable">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFineApplicable" runat="server" Text='<%#Eval("FineApplicable")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="75px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Concession Applicable">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConcessionApplicable" runat="server" Text='<%#Eval("ConcessionApplicable")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="75px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Refundable" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRefundable" runat="server" Text='<%#Eval("Refundable")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="75px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account Heads">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpAccHeads" runat="server" OnSelectedIndexChanged="drpAccHeads_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:LinkButton ID="lnkbtnAccHead" runat="server" Text="Save" OnClick="lnkbtnAccHead_Click"
                                                        Visible="false" OnClientClick="return CnfSave()"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" />
                                        <AlternatingRowStyle CssClass="alt" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

