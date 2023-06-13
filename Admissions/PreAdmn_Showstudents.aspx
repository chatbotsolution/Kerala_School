<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="PreAdmn_Showstudents.aspx.cs" Inherits="Admissions_PreAdmn_Showstudents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
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
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Pre-Admission List</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="tbltxt" ForeColor="Red"></asp:Label></div>
            <table style="height: 360px;" width="100%">
                <tr>
                    <td style="width: 100%;" valign="top">
                        <table width="100%">
                            <tr>
                                <td align="left" valign="top" class="tbltxt" style="border: solid 1px Black; width: 100%;">
                                    <asp:CheckBox ID="chkCasual" Text="Casual Student" runat="server" />&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblSession" runat="server" Text="Session :"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" Width="70px"
                                        OnSelectedIndexChanged="drpSession_SelectedIndexChanged" CssClass="tbltxtbox"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:Label ID="lblClass" runat="server" Text="Class :"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" Width="80px" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                        CssClass="tbltxtbox" TabIndex="2">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblSelectStudent" runat="server" Text="Select Student :"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="drpSelectStudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSelectStudent_SelectedIndexChanged"
                                        CssClass="tbltxtbox" TabIndex="4">
                                    </asp:DropDownList>
                                    <br />
                                    OR
                                    <br />
                                    Search by student Name/Student ID:&nbsp;<asp:TextBox ID="txtStudentName" runat="server"
                                        CssClass="tbltxtbox" TabIndex="5" Width="170px"></asp:TextBox>&nbsp;
                                    <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" TabIndex="6" Text="Show" />
                                    <asp:Button ID="btnDelete" OnClientClick="return CnfDelete()" Enabled="false" runat="server" OnClick="btnDelete_Click" Text="Delete Student"
                                        TabIndex="7" />
                                                                      
                                   
                                </td>
                                <td align="right" valign="top">
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                 <fieldset style="height: 40px; vertical-align: bottom;">
                                        <legend style="background-color: Transparent;" class="tbltxt"><strong>Student Admission</strong></legend>
                                    
                                      <asp:Button ID="btnNew" runat="server" OnClick="btnNew_Click" Text="New Student" TabIndex="9" />
                                    
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2" style="height: 15px" class="tbltxt">
                                    <asp:Label ID="lblRecCount" runat="server" Text="Label" Width="908px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:GridView ID="grdstuddet" runat="server" Width="98%" AutoGenerateColumns="False"
                                        AllowPaging="True" OnPageIndexChanging="grdstuddet_PageIndexChanging" PageSize="15"
                                        CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                        TabIndex="7">
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input name="toggleAll" onclick='ToggleAll(this)' disabled="disabled" type="checkbox" value="ON" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="15px" />
                                                <HeaderStyle Width="15px" />
                                                <ItemTemplate>
                                                    <input name="Checkb" type="checkbox" value='<%#Eval("Admissionno")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Admission No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrollno" Font-Bold="true" runat="server" Text='<%# Eval("Admissionno") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                           <%-- <asp:BoundField HeaderText="Old Admn No." DataField="OldAdmnNo"/>--%>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <a href='PreAdmn_Addstudentinfo.aspx?sid=<%#Eval("Admissionno")%>&sy=<%# drpSession.SelectedValue %> '>
                                                        <asp:Label ID="lblname" runat="server" Text='<%#Eval("FullName")%>'></asp:Label>
                                                    </a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            
                                            <asp:BoundField DataField="FatherName" HeaderText="Father Name" />
                                            <asp:BoundField DataField="MotherName" HeaderText="Mother Name" />
                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldob" runat="server" Text='<%#Eval("dateofbirth")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Class">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclass" runat="server" Text='<%#Eval("classname")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                          <%--  <asp:TemplateField HeaderText="Section">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclass" runat="server" Text='<%#Eval("Section")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Contact No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclass" runat="server" Text='<%#Eval("TelNoResidence")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Admission Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclass" runat="server" Text='<%#Eval("AdmnDate")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            
                                            <%--  <asp:TemplateField HeaderText="Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclass" runat="server" Text='<%#Eval("StudType")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>--%>
                                        </Columns>
                                        <RowStyle BackColor="#EFEFEF" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="#FDFDFD" />
                                        <EmptyDataTemplate>
                                            No Record
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblReport" runat="server" Visible="false"></asp:Literal>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShow" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
