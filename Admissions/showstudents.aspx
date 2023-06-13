<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="showstudents.aspx.cs" Inherits="Admissions_showstudents" %>

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
            Admission</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<div class="spacer"></div>
            <table width="100%">
                            <tr>
                                <td align="left" valign="top" class="tbltxt cnt-box2 " width="49%"   >
                                    
                                     
                                   <span class="ttl3"><asp:Label ID="lblSession" runat="server" Text="Session :"></asp:Label></span>
                                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True"  
                                        OnSelectedIndexChanged="drpSession_SelectedIndexChanged" CssClass="tbltxtbox largetb1 wdth-134"
                                        TabIndex="1">
                                    </asp:DropDownList><br /><br />
                                     
                                   <span class="ttl3"><asp:Label ID="lblClass" runat="server" Text="Class :"></asp:Label></span>
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                        CssClass="tbltxtbox largetb1  wdth-134" TabIndex="2">
                                    </asp:DropDownList><br /><br />
                                      
                                   
                                     <span class="ttl3"><asp:Label ID="lblSection" runat="server" Text="Section :"></asp:Label></span>
                                    <asp:DropDownList ID="drpSection" runat="server"   AutoPostBack="True"
                                        OnSelectedIndexChanged="drpSection_SelectedIndexChanged" CssClass="tbltxtbox largetb1  wdth-134"
                                        TabIndex="3">
                                    </asp:DropDownList><br /><br />
                                    
                                     <span class="ttl3"><asp:Label ID="lblSelectStudent" runat="server" Text="Select Student :"></asp:Label></span>
                                    <asp:DropDownList ID="drpSelectStudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSelectStudent_SelectedIndexChanged"
                                        CssClass="tbltxtbox largetb1  wdth-134" TabIndex="4">
                                    </asp:DropDownList>
                                  <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" TabIndex="6" Text="Show" CssClass="btn" />
                                       <asp:CheckBox ID="chkCasual" Text="Casual Student" runat="server" CssClass="chklst1" Visible="false" />                               
                                   
                                </td>
                                <td width="2%" align="center" valign="middle" class="tbltxt">OR</td>
                                <td align="left" valign="top" class="tbltxt cnt-box2" width="49%">
                                    <div >  
                                    <div class="tbltxt">Search by student Name/Student Admn No:</div>
                                    <asp:TextBox ID="txtStudentName" runat="server"
                                        CssClass="tbltxtbox largetb wdth-250 mr-btm" TabIndex="5" ></asp:TextBox>
                                         <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" TabIndex="6" Text="Search" CssClass="btn" />
                                        </div>
                                        <br /><br />
                                    
                                    <asp:Button ID="btnDelete" OnClientClick="return CnfDelete()" Enabled="false" runat="server" OnClick="btnDelete_Click" Text="Delete Student" CssClass="btn"
                                        TabIndex="7" />
                                    <asp:Button ID="btnExport" runat="server" Text="Export Data For Website" OnClick="btnExport_Click" CssClass="btn"
                                        TabIndex="8" />
                                        <div class="spacer"></div><div class="spacer"></div>
                                        <div>
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="tbltxt" ForeColor="Red"></asp:Label></div>
                                </td>
                            </tr>
                            </table>--%>
                            
            
            <div class="spacer"></div>
            <table width="100%" class="cnt-box">
                <tr>
                    <td style="width: 100%;" valign="top">
                        
                            <table>
                            <tr>
                            <td colspan="2">
                             <legend style="background-color: Transparent; " class="tbltxt mr-btm"><strong>Student Admission</strong></legend>
                            </td>
                            </tr>
                            <tr>
                            <td>
                             <asp:RadioButtonList ID="RbFormSelect" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem Value="2">NUR-UKG</asp:ListItem>
                                <asp:ListItem Value="0">I-X</asp:ListItem>
                                 <asp:ListItem Value="1">XI-XII</asp:ListItem>
                                </asp:RadioButtonList>
                                 
                            </td>
                               <td align="left">
                                <asp:Label ID="lblError" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                               </td>
                               
                               
                            </tr>

                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnNew" runat="server" OnClick="btnNew_Click" Text="New Student" TabIndex="9" />
                                    <asp:Button ID="btnTc" runat="server" Text="TC Student" TabIndex="11" 
                                        onclick="btnTc_Click" Visible="False"/>
                                         <asp:Button ID="btnCasual" runat="server" Text="Other School Student" 
                                        TabIndex="12" onclick="btnCasual_Click" Visible="false" />
                                      <asp:Button ID="btnExst" runat="server" Text="Old Student Entry" 
                                        TabIndex="10" onclick="btnExst_Click" />
                                    
                                    
                                </td>
                            </tr>
                            
                            <tr>
                                <td valign="top" colspan="2" style="height: 15px" class="tbltxt">
                                   <%-- <asp:Label ID="lblRecCount" runat="server" Text="Label" Width="908px"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <%--<asp:GridView ID="grdstuddet" runat="server" Width="98%" AutoGenerateColumns="False"
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
                                            <asp:TemplateField HeaderText="Admission No." Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrollno" Font-Bold="true" runat="server" Text='<%# Eval("Admissionno") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Admission No." DataField="OldAdmnNo"/>
                                            <asp:TemplateField HeaderText="Name" >
                                                <ItemTemplate>
                                                    <a href='AddAStudent.aspx?sid=<%#Eval("Admissionno")%>&sy=<%# drpSession.SelectedValue %>&Class=<%#Eval("ClassId")%> '>
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
                                            <asp:TemplateField HeaderText="Section">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclass" runat="server" Text='<%#Eval("Section")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
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
                                            
                                              <asp:TemplateField HeaderText="Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclass" runat="server" Text='<%#Eval("StudType")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle BackColor="#EFEFEF" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle BackColor="#333333" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="#FDFDFD" />
                                        <EmptyDataTemplate>
                                            No Record
                                        </EmptyDataTemplate>
                                    </asp:GridView>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                       <%-- <asp:Literal ID="lblReport" runat="server" Visible="false"></asp:Literal>--%>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
<%--            <asp:PostBackTrigger ControlID="btnExport" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
