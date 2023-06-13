<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="Holiday.aspx.cs" Inherits="HR_Holiday" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
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
                alert("Select any Record");
                return false;
            }
        }
        function CnfDelete() {

            if (confirm("You are going to delete selected Record(s). Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }
        function CnfWorking() {

            if (confirm("You are going to set this holiday as working. Do you want to continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Holiday List</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <fieldset style="width: 70%">
                <table width="100%">
                    <tr id="trMsg" runat="server">
                        <td colspan="2" align="center">
                            <asp:Label runat="server" ID="lblMsg" ForeColor="White" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            <b>Year&nbsp;:&nbsp;</b><asp:DropDownList ID="drpYear" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="drpYear_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" />&nbsp;
                            <asp:Button ID="btnDelete" runat="server" Text="Delete Selected Records" OnClick="btnDelete_Click"
                                Visible="False" OnClientClick="return CnfDelete();" />
                            <asp:Button ID="btnWorking" runat="server" Text="Set As Working"
                            Visible="False" OnClientClick="return CnfWorking();" 
                                onclick="btnWorking_Click" />
                        </td>
                        <td align="right">
                            <asp:Label ID="lblRecCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grdHoliday" runat="server" AutoGenerateColumns="False" 
                                Width="100%" AllowPaging="true" PageSize="20" 
                                onpageindexchanging="grdHoliday_PageIndexChanging" 
                                onrowdatabound="grdHoliday_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input name="toggleAll" onclick='ToggleAll(this)' disabled="disabled" type="checkbox" value="ON" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="15px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                        <ItemTemplate>
                                            <input name="Checkb" type="checkbox" value='<%#Eval("HolidayID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                           <asp:Label runat="server" ID="lblEdit" ><a href='Holidaydetail.aspx?hId=<%#Eval("HolidayID")%>'>Edit</a></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Holiday Name">
                                        <ItemTemplate>
                                            <%#Eval("HolidayName")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From Date">
                                        <ItemTemplate>
                                            <%#Eval("FromDt")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Date">
                                        <ItemTemplate>
                                            <%#Eval("ToDt")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tithi">
                                        <ItemTemplate>
                                            <%#Eval("HolidayTithi")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%#Eval("Wrkng")%>' ID="lblStatus" ></asp:Label>     
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:TemplateField>
                                    
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record(s)
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

