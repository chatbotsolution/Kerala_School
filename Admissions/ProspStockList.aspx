<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="ProspStockList.aspx.cs" Inherits="Admissions_ProspStockList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
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
            if (flag == true)
                return true;
            else {
                alert("Please select any record");
                return false;
            }
        }
        function CnfDelete() {

            if (confirm("You are going to delete a record. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }
        function Isvalid() {
            var Frmdt = document.getElementById("<%=txtFromDt.ClientID %>").value;
            var Todt = document.getElementById("<%=txtToDt.ClientID %>").value;

            if (Date.parse(Frmdt.trim()) > Date.parse(Todt.trim())) {
                alert("Please check date range! From Date can't be greater than To date!")
                return false;
            }
            else {
                return true;
            }
        } 
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
             Prospectus Stock List
        </h2>
    </div><br />
    <div style="" class="tbltxt cnt-box">
        <span class="ttl">Session Year </span>:
        <asp:DropDownList ID="drpSession" runat="server" CssClass="tbltxtbox largetb1 wdth-250" TabIndex="1">
        </asp:DropDownList>
        <br /><br /><span class="ttl">From Date </span>:
        <asp:TextBox ID="txtFromDt" runat="server"  ReadOnly="true" 
            CssClass="tbltxtbox" Width="80px" TabIndex="2"></asp:TextBox>
        <rjs:PopCalendar ID="dtpPros" runat="server" Control="txtFromDt" AutoPostBack="False"
            Format="dd mmm yyyy"></rjs:PopCalendar> <br /><br />
        <span class="ttl">To Date </span>:
        <asp:TextBox ID="txtToDt" runat="server" CssClass="tbltxtbox" ReadOnly="true" 
            Width="80px" TabIndex="3"></asp:TextBox>
        <rjs:PopCalendar ID="dtpPros1" runat="server" Control="txtToDt" AutoPostBack="False"
            Format="dd mmm yyyy"></rjs:PopCalendar>
        &nbsp;&nbsp;<br /><br />
        <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" 
            onfocus="active(this);" onblur="inactive(this);" 
            OnClientClick="return Isvalid();" TabIndex="4" CssClass="btn wdth fa fa-"/>
    </div>
    <br />
    <div class="cnt-box2">
    <table  width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Button ID="btnDelete" runat="server" Text="Delete" onfocus="active(this);" onblur="inactive(this);" 
                    OnClick="btnDelete_Click" TabIndex="6" CssClass="btn2" />&nbsp;
                <asp:Button ID="btnAddNew" runat="server" Text="Add New" onfocus="active(this);" onblur="inactive(this);" 
                    OnClick="btnAddNew_Click" TabIndex="7" CssClass="btn2" />
                &nbsp;&nbsp;<asp:Label ID="lblmsg1" runat="server"
                    Text="" CssClass="error"></asp:Label>
            </td>
            <td align="right">
                <asp:Label ID="lblRecCount" runat="server" Text="" CssClass="txtlink"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="2">
                <asp:GridView ID="grdProspect" Width="100%" runat="server" AutoGenerateColumns="false"
                    CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    OnPageIndexChanging="grdProspect_PageIndexChanging" AllowSorting="True" 
                    TabIndex="5">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="15px" />
                            <HeaderStyle HorizontalAlign="Left" Width="15px" />
                            <ItemTemplate>
                                <input name="Checkb" type="checkbox" value='<%#Eval("TransId")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <a href='ProspStock.aspx?RId=<%#Eval("TransId")%>'>Edit</a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Session Year">
                            <ItemTemplate>
                                <asp:Label ID="lblname" runat="server" Text='<%#Eval("SessionYr")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Transaction date">
                            <ItemTemplate>
                                <asp:Label ID="lbladmnno" runat="server" Text='<%#Eval("TransDateStr")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Prospectus Type">
                            <ItemTemplate>
                                <asp:Label ID="lblProspType" runat="server" Text='<%#Eval("ProspectusType")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" Width="200px"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit Sale Price">
                            <ItemTemplate>
                                <asp:Label ID="lblProspType" runat="server" Text='<%#Eval("UnitSalePrice")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" Width="80px"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity In">
                            <ItemTemplate>
                                <asp:Label ID="lblQtyIn" runat="server" Text='<%#Eval("QtyIn")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" Width="100px"/>
                        </asp:TemplateField>
                    </Columns>

<PagerStyle CssClass="pgr"></PagerStyle>
                    <EmptyDataTemplate>
                        No Record
                    </EmptyDataTemplate>

<AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                </asp:GridView>
            </td>
        </tr>
    </table></div>
</asp:Content>

