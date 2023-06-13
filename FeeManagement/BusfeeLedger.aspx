<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="BusfeeLedger.aspx.cs" Inherits="FeeManagement_BusfeeLedger" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Define Bus/Hostel Fee</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <table width="100%" class="tbltxt">
            <tr>
            <td width="49%" border="0" cellpadding="0" cellspacing="0" class="cnt-box3" align="left" valign="top">
                <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="tbltxt">
                        <div class="cnt-sec2">
                        <div class="ttl3">Session </div>:
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                            TabIndex="1" >
                        </asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvSession" runat="Server" ErrorMessage="*Requited"
                            SetFocusOnError="true" Display="dynamic" ControlToValidate="drpSession" CssClass="error"></asp:RequiredFieldValidator>
                         <div class="cnt-sec2">
                        <div class="ttl3">Class </div>:
                        <asp:DropDownList ID="drpclass" runat="server" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                            AutoPostBack="true" CssClass="vsmalltb" TabIndex="2">
                        </asp:DropDownList>
                        </div>
                         <div class="cnt-sec2">
                        <div class="ttl3">Section </div>:
                        <asp:DropDownList ID="ddlSection" runat="server" OnSelectedIndexChanged="ddlSection_SelectedIndexChanged"
                            AutoPostBack="True" CssClass="vsmalltb" TabIndex="3">
                        </asp:DropDownList>
                         </div>
                         <div class="cnt-sec2">
                        <div class="ttl3">Select Student </div>:
                                    <asp:DropDownList ID="drpSelectStudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSelectStudent_SelectedIndexChanged"
                                        CssClass="vsmalltb"  TabIndex="4">
                                    </asp:DropDownList></div>
                        </td>
                        </tr>
                </table>
            </td>
            <td width="2%" valign="top" >OR</td>
            <td width="49%" border="0" cellpadding="0" cellspacing="0" class="cnt-box3">
                <table  cellpadding="0" cellspacing="0" width="100%">
                <tr>
                        <td class="tbltxt">
                           Search by student Name/Student ID:&nbsp;&nbsp;<asp:TextBox ID="txtStudentName" runat="server" TabIndex="5"  ></asp:TextBox>
                        </td>
                </tr>
                <tr>
                <td class="tbltxt">
                         <span class="ttl3" style="width:193px;">Month :</span>
                        <asp:DropDownList ID="ddlMonth" runat="server" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                            AutoPostBack="True" CssClass="vsmalltb" TabIndex="4">
                            <asp:ListItem Text="April" Value="4"></asp:ListItem>
                            <asp:ListItem Text="May" Value="5"></asp:ListItem>
                            <asp:ListItem Text="June" Value="6"></asp:ListItem>
                            <asp:ListItem Text="July" Value="7"></asp:ListItem>
                            <asp:ListItem Text="August" Value="8"></asp:ListItem>
                            <asp:ListItem Text="September" Value="9"></asp:ListItem>
                            <asp:ListItem Text="October" Value="10"></asp:ListItem>
                            <asp:ListItem Text="November" Value="11"></asp:ListItem>
                            <asp:ListItem Text="December" Value="12"></asp:ListItem>
                             <asp:ListItem Text="January" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Feburary" Value="2"></asp:ListItem>
                            <asp:ListItem Text="March" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                        <br /><br />
                        <asp:RadioButton ID="rBtnBus" Text="Bus" runat="server" Checked="True" 
                            GroupName="FT" oncheckedchanged="rBtnBus_CheckedChanged" />
                         &nbsp;<asp:RadioButton ID="rBtnHostel"  Text="Hostel" runat="server" 
                            GroupName="FT" oncheckedchanged="rBtnHostel_CheckedChanged" />
                        <br /><br />
                        <asp:Button ID="btngo" OnClick="btngo_Click" CausesValidation="false" 
                            runat="server" Text="Search" ToolTip="Click to show student list."
                            TabIndex="5" style="height: 26px"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td class="tbltxt" align="center">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="error"></asp:Label>
                    </td>
                </tr>
              
                
                </table>
            </td>
            </tr>
            
        </table>
        <table>
        
        <tr>
                    <td align="left" class="tbltxt">
                         &nbsp;<asp:Label ID="lblRecCount" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
                        
                        </td>
                </tr>
              
                <tr>
                    <td style="width:100%">
                        <asp:GridView ID="grdStudentList" runat="server" AutoGenerateColumns="False" Width="100%"
                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            TabIndex="6">
                            <Columns>

                                <asp:BoundField DataField="admnno" HeaderText="Admn No" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RollNo" HeaderText="Roll No" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FullName" HeaderText="Student Name" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                
                                <asp:BoundField DataField="PresAddr1" HeaderText="Present Address" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RouteName" HeaderText="Bus Route Name" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                  <asp:BoundField DataField="fee" HeaderText="Planned Amt" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Chargable Amt" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRouteID" runat="server" Text='<%#Eval("BusRouteID")%>' Visible ="false" ></asp:Label>
                                        <asp:HiddenField ID="ClassId" runat="server" Value='<%#Eval("ClassWiseId")%>' />
                                        <asp:TextBox ID="txtFeeAmountBus" runat="Server" CssClass="vsmalltb" Text='<%#Eval("Amount")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFeeAmountBus" runat="server" ControlToValidate="txtFeeAmountBus"
                                            ErrorMessage="*Required" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revFeeAmtBus" runat="server" ControlToValidate="txtFeeAmountBus"
                                            ErrorMessage="Numeric Only" SetFocusOnError="true" Display="Dynamic" ValidationExpression="^[0-9]+"></asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="70px" />
                                </asp:TemplateField>
                              <%--  <asp:TemplateField HeaderText="Hostel Fee Amount" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFeeAmountHostel" runat="Server" CssClass="vsmalltb" Text='<%#Eval("HostelFee")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFeeAmountHostel" runat="server" ControlToValidate="txtFeeAmountHostel"
                                            ErrorMessage="*Required" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revFeeAmtHostel" runat="server" ControlToValidate="txtFeeAmountHostel"
                                            ErrorMessage="Numeric Only" SetFocusOnError="true" Display="Dynamic" ValidationExpression="^[0-9]+"></asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="70px" />
                                </asp:TemplateField>--%>
                            </Columns>
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                        </asp:GridView>
                        </td></tr>
                         <tr><td style="width:100%">
                        <asp:GridView ID="grdHostel" runat="server" AlternatingRowStyle-CssClass="alt" 
                            AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr" 
                            TabIndex="6" Width="100%" >
                            <Columns>
                                <asp:BoundField DataField="admnno" HeaderStyle-HorizontalAlign="Left" 
                                    HeaderText="Admn No" ItemStyle-Width="100px">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RollNo" HeaderStyle-HorizontalAlign="Left" 
                                    HeaderText="Roll No" ItemStyle-Width="80px">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FullName" HeaderStyle-HorizontalAlign="Left" 
                                    HeaderText="Student Name" ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PresAddr1" HeaderStyle-HorizontalAlign="Left" 
                                    HeaderText="Present Address" ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" 
                                    HeaderText="Chargable Amt" ItemStyle-HorizontalAlign="left" 
                                    ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="ClassId" runat="server" Value='<%#Eval("ClassWiseId")%>' />
                                        <asp:TextBox ID="txtFeeAmountBus" runat="Server" CssClass="vsmalltb" 
                                            Text='<%#Eval("Amount")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFeeAmountBus" runat="server" 
                                            ControlToValidate="txtFeeAmountBus" Display="Dynamic" ErrorMessage="*Required" 
                                            SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revFeeAmtBus" runat="server" 
                                            ControlToValidate="txtFeeAmountBus" Display="Dynamic" 
                                            ErrorMessage="Numeric Only" SetFocusOnError="true" 
                                            ValidationExpression="^[0-9]+"></asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="70px" />
                                </asp:TemplateField>
                                <%--  <asp:TemplateField HeaderText="Hostel Fee Amount" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFeeAmountHostel" runat="Server" CssClass="vsmalltb" Text='<%#Eval("HostelFee")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFeeAmountHostel" runat="server" ControlToValidate="txtFeeAmountHostel"
                                            ErrorMessage="*Required" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revFeeAmtHostel" runat="server" ControlToValidate="txtFeeAmountHostel"
                                            ErrorMessage="Numeric Only" SetFocusOnError="true" Display="Dynamic" ValidationExpression="^[0-9]+"></asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="70px" />
                                </asp:TemplateField>--%>
                            </Columns>
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                        </asp:GridView>
                        
         
                    </td>
                </tr>
        </table>
            
   <div style="padding-top: 10px;" class="tbltxt">
      <asp:CheckBox ID="chkFullSession" Text="Update the Fee For Full Session" 
            runat="server" TabIndex="7" /><br /><br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="100px" OnClick="btnSubmit_Click"
            TabIndex="8" />
    </div>
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btngo" />
            <asp:PostBackTrigger ControlID="btnSubmit" />
            <asp:PostBackTrigger ControlID="drpclass" />
        </Triggers>
    </asp:UpdatePanel>
 
</asp:Content>

