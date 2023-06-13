<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="InventoryIndividualList.aspx.cs" Inherits="Hostel_InventoryIndividualList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            document.getElementById("<%=txtFrmDt.ClientID %>").focus();
        });

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=700,height=500,left = 390,top = 150');");
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

            if (confirm("You are going to delete a record. Do you want to continue?")) {

                return true;
            }
            else {

                return false;
            }
        }  
      
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Issue Details
                </h2>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px;">
                <table width="100%" cellspacing="0" cellpadding="0" class="tbltxt">
                    <tr>
                        <td align="left">
                            <strong>Date From:</strong>
                            <asp:TextBox ID="txtFrmDt" runat="server" Width="80px" CssClass="tbltxtbox"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtFrmDt" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtFrmDt.value='';return false;"
                                Text="Clear"></asp:LinkButton>&nbsp;&nbsp; <strong>Date To:</strong>
                            <asp:TextBox ID="txtTo" runat="server" Width="80px" CssClass="tbltxtbox"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtTo" Format="dd mmm yyyy">
                            </rjs:PopCalendar>
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtTo.value='';return false;"
                                Text="Clear"></asp:LinkButton>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSearch" runat="server" Text="Search" Width="80px" onclick="btnSearch_Click" />&nbsp;
                            <asp:Button ID="btnAddNew" runat="server" Text="Add New" Width="80px" OnClick="btnAddNew_Click" />&nbsp;
                            <%--<asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" OnClick="btnDelete_Click" />--%>
                        </td>
                    </tr>
                </table>                
            </div>
            <asp:GridView ID="grdItem" runat="server" AllowPaging="true" 
                    AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="mGrid" 
                    DataKeyNames="IssueId" OnPageIndexChanging="grdItem_PageIndexChanging" 
                    onrowcommand="grdItem_RowCommand" PagerStyle-CssClass="pgr" PageSize="12" 
                    TabIndex="4" Width="100%">
                    <Columns>
                        <%--<asp:TemplateField>
                            <ItemTemplate>                                
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="" CommandName="Delete" CommandArgument='<%#Eval("IssueId") %>' />
                            </ItemTemplate>                            
                            <HeaderStyle HorizontalAlign="Center" Width="20px" />
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Issue Date">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("IDate")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Issued By">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("IssuedBy")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Received By">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#Eval("IssuedTo")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="To Location">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%#Eval("Location")%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="View Details">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <a href="javascript:popUp('ViewIndividualIssuedDetails.aspx?Id=<%#Eval("IssueId")%>')">
                                View Items</a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFEFEF" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#FDFDFD" />
                </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

