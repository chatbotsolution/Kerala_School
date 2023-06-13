<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="AssignBusHostel.aspx.cs" Inherits="Admissions_AssignBusHostel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="Javascript">
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
        function Cnf() {

            if (confirm("You Change Bus/Hostel Facility Status. Do you want to continue?")) {

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
            Assign Bus/Hostel</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel runat="server" ID="uptdpnlStuds" UpdateMode="Conditional"><ContentTemplate>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="cnt-box tbltxt">
        <tr>
            <td >
                
                <div class="ttl">Session </div> :
                <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True"
                    TabIndex="1" onselectedindexchanged="drpSession_SelectedIndexChanged">
                </asp:DropDownList>
                <br /><br /> <div class="ttl">Class </div> :
                <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="true" 
                    TabIndex="2" onselectedindexchanged="drpclass_SelectedIndexChanged">
                </asp:DropDownList>
                <br /><br /> <div class="ttl">Section </div> :
                <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True"
                    TabIndex="3" onselectedindexchanged="drpSection_SelectedIndexChanged">
                </asp:DropDownList>
                <br /><br /><div class="ttl">&nbsp;</div>
                <asp:RadioButton ID="rbtnBus" Text="Bus" runat="server" Checked="True" GroupName="FT" />
                &nbsp;<asp:RadioButton ID="rbtnHostel" Text="Hostel" runat="server" GroupName="FT" />
               <br /><br />
                <div class="ttl">&nbsp;</div>&nbsp;&nbsp;<asp:Button ID="btnAssign" runat="server" Text="Include Bus/Hostel Facility" onclick="btnAssign_Click" OnClientClick="return Cnf();" />
                <asp:Button ID="btnDeny" runat="server" Text="Exclude Bus/Hostel Facility" onclick="btnDeny_Click" OnClientClick="return Cnf();"/>
                <br /><br />
                Existing List Bus/Hostel :
                <asp:DropDownList ID="drpExistingList" runat="server" AutoPostBack="True" 
                        TabIndex="3" onselectedindexchanged="drpExistingList_SelectedIndexChanged" >
                    <asp:ListItem>-- All --</asp:ListItem>
                    <asp:ListItem>Bus Facility</asp:ListItem>
                    <asp:ListItem>Hostel Facility</asp:ListItem>
                </asp:DropDownList>
                
                
                 
            </td>
        </tr>
       
        <tr>
            <td align="left">
                
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="lblRecCount" runat="server" Font-Bold="true" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdStudentList" runat="server" AutoGenerateColumns="False" Width="100%"
                    CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    TabIndex="6">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15px" />
                            <HeaderStyle HorizontalAlign="Center" Width="15px" />
                            <ItemTemplate>
                                <input name="Checkb" type="checkbox" value='<%#Eval("AdmnNo")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="FullName" HeaderText="Student Name" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OldAdmnNo" HeaderText="School Admn No" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="admnno" HeaderText="Admn No" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StudAddress" HeaderText="Present Address" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="BusF" HeaderText="Bus Facility" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80px"></asp:BoundField>
                        <asp:BoundField DataField="HostelF" HeaderText="Hostel Facility" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80px"></asp:BoundField>
                    </Columns>
                    <PagerStyle />
                    <AlternatingRowStyle />
                </asp:GridView>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    <Triggers>
    <asp:PostBackTrigger ControlID="btnAssign" />
    <asp:PostBackTrigger ControlID="btnDeny" />
    </Triggers>
    </asp:UpdatePanel>
</asp:Content>

