<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="ProspectusSaleList.aspx.cs" Inherits="Admissions_ProspectusSaleList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
        function SaveReason() {

            var Del = document.getElementById("<%=txtDelReason.ClientID %>").value;
            if (Del.trim() == "") {
                alert("Please enter Reason for Deletion !");
                document.getElementById("<%=txtDelReason.ClientID %>").focus();
                return false;
            }
            else {
                return true;
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

            if (confirm("You are going to delete a record. Do you want to continue  ?")) {

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
            Prospectus Sale List
        </h2>
    </div>
    <br />
    <table style="width: 100%" class="cnt-box">
        <tr>
            <td colspan="2" class="tbltxt" style="display:block">
                <div class="cnt-sec">
                <div class="ttl">Session Year </div>:
                <asp:DropDownList ID="drpSession" runat="server" Width="134px" CssClass="tbltxtbox largetb1 wdth-250" 
                    TabIndex="1">
                </asp:DropDownList>
                </div>
                <div class="cnt-sec">
                <div class="ttl">For Class </div>: 
                <asp:DropDownList ID="drpForCls" runat="server" 
                    CssClass="tbltxtbox largetb1 wdth-250" TabIndex="4">
                </asp:DropDownList>
                </div>
                 <div class="cnt-sec">
                <div class="ttl">From Date </div>: 
                <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" CssClass="tbltxtbox largetb wdth-238" 
                    TabIndex="2"></asp:TextBox>
                <rjs:PopCalendar ID="dtpPros" runat="server" Control="txtFromDt" AutoPostBack="False"
                    Format="dd mmm yyyy"></rjs:PopCalendar>
                    </div>
                    <div class="cnt-sec">
               <div class="ttl">To Date </div>: 
                <asp:TextBox ID="txtToDt" runat="server" CssClass="tbltxtbox largetb wdth-238" ReadOnly="true" Width="100px"
                    TabIndex="3"></asp:TextBox>
                <rjs:PopCalendar ID="dtpPros1" runat="server" Control="txtToDt" AutoPostBack="False"
                    Format="dd mmm yyyy"></rjs:PopCalendar>
                    </div> 
                    
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return Isvalid();" onfocus="active(this);" onblur="inactive(this);"
                    TabIndex="5" CssClass="btn" />
                <br />
            </td>
        </tr>
        </table>
        <div class="spacer"></div>
        <div class="spacer"></div>
        <table class="cnt-box2" width="100%">
        <tr>
            <td>
                <asp:Button ID="btnDelete" runat="server" Text="Delete " OnClick="btnDelete_Click" onfocus="active(this);" onblur="inactive(this);"
                    TabIndex="7" OnClientClick="return CnfDelete();" Visible="false" CssClass="btn2"/>&nbsp;&nbsp;
                <asp:Button ID="btnAddNew" runat="server" Text="Add New" OnClick="btnAddNew_Click" onfocus="active(this);" onblur="inactive(this);"
                    TabIndex="8" CssClass="btn2" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblmsg1" runat="server"
                    Text="" CssClass="tbltxt" Font-Bold="true"></asp:Label>
            </td>
            <td align="right">
                <asp:Label ID="lblRecCount" runat="server" Text="" CssClass="tbltxt txtlink"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="2">
                <asp:Panel ID="pnlRcptList" runat="server">
                    <asp:GridView ID="grdProspect" Width="100%" runat="server" AutoGenerateColumns="false"
                        CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        OnPageIndexChanging="grdProspect_PageIndexChanging" AllowSorting="True" TabIndex="6">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input name="toggleAll" onclick='ToggleAll(this)' disabled="disabled" type="checkbox"
                                        value="ON" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="15px" />
                                <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                <ItemTemplate>
                                    <input name="Checkb" type="checkbox" value='<%#Eval("SaleId")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sl. No">
                                <ItemTemplate>
                                    <asp:Label ID="lblSlNo" runat="server" Text='<%#Eval("ProspectusSlNo")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50px" />
                                <HeaderStyle Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Student Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("StudentName")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            
                           
                             <asp:BoundField DataField="ContactNo" HeaderText="Contact No."></asp:BoundField>
                              <asp:BoundField DataField="CurrAddress" HeaderText="Address"></asp:BoundField>
                            <asp:TemplateField HeaderText="Session Year">
                                <ItemTemplate>
                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("SessionYr")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sale date">
                                <ItemTemplate>
                                    <asp:Label ID="lbladmnno" runat="server" Text='<%#Eval("SaleDateStr")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="For Class">
                                <ItemTemplate>
                                    <asp:Label ID="lbladmnno" runat="server" Text='<%#Eval("ForClass")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lbladmnno" runat="server" Text='<%#Eval("Amount")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="pgr"></PagerStyle>
                        <EmptyDataTemplate>
                            No Record
                        </EmptyDataTemplate>
                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnlDelReason" runat="server" Width="70%" Style="border: solid 1px #ACA899;
                    padding: 5px 5px 5px 5px" CssClass="tbltxt" Visible="false">
                    <asp:Literal ID="litDetails" runat="server"></asp:Literal>
                    <br />
                    <b>Enter Reason &nbsp;:-&nbsp;</b>
                    <br />
                    <asp:TextBox ID="txtDelReason" runat="server" CssClass="vsmalltb" Width="100%" TextMode="MultiLine"
                        MaxLength="200"></asp:TextBox>
                    <br />
                    <asp:Button ID="btnSaveReason" runat="server" Text="Cancel Receipt" OnClick="btnSaveReason_Click"
                        OnClientClick="return SaveReason();" onfocus="active(this);" onblur="inactive(this);" />
                    <asp:Button ID="btnCancel" runat="server" Text="Back to List" OnClick="btnCancel_Click" onfocus="active(this);" onblur="inactive(this);" />
                </asp:Panel>
                <asp:HiddenField ID="hfRcptId" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
