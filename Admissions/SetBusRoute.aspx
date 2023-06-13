<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="SetBusRoute.aspx.cs" Inherits="Admissions_SetBusRoute" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="Javascript">
    function CnfSave() {

        if (confirm("You are going to Set Bus Route . Do you want to continue?")) {

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
           Assign Bus Route</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="cnt-box tbltxt">
                <tr>
                    
                    <td class="tbltxt">
                         
                       
                        Session :
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                            TabIndex="1" OnSelectedIndexChanged ="drpSession_SelectedIndexChanged" >
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvSession" runat="Server" ErrorMessage="*Requited"
                            SetFocusOnError="true" Display="dynamic" ControlToValidate="drpSession" CssClass="error"></asp:RequiredFieldValidator>
                         Class :
                        <asp:DropDownList ID="drpclass" runat="server" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                            AutoPostBack="true" CssClass="vsmalltb" TabIndex="2">
                        </asp:DropDownList>
                        &nbsp; Section :
                        <asp:DropDownList ID="drpSection" runat="server" OnSelectedIndexChanged="drpSection_SelectedIndexChanged"
                            AutoPostBack="True" CssClass="vsmalltb" TabIndex="3">
                        </asp:DropDownList>
                        <%--&nbsp; Month :
                        <asp:DropDownList ID="ddlMonth" runat="server"
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
                        </asp:DropDownList>--%>
                       <br /><br />
                       
                        <asp:Button ID="btngo" CausesValidation="false" runat="server" Text="Search" ToolTip="Click to show student list."
                            TabIndex="5" onclick="btngo_Click"></asp:Button>
                        
                    </td>
                    
                </tr>
                <tr>
                    <td class="tbltxt" align="center">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="error"></asp:Label>
                    </td>
                </tr>
              </table>
              
              <table width="100%" class="cnt-box2" >
                <tr>
                    <td align="left" class="tbltxt">
                         &nbsp;<asp:Label ID="lblRecCount" runat="server" Font-Bold="true" CssClass="gridtxt"></asp:Label>
                        </td>
                </tr>
              
                <tr>
                    <td >
                        <asp:GridView ID="grvBusRoute" runat="server" AutoGenerateColumns="False" Width="100%"
                            CssClass="mGrid cnt-box2 spaceborder pdng" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            TabIndex="6" OnRowDataBound="grvBusRoute_RowDataBound" style="padding:10px">
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
                                <asp:TemplateField HeaderText="Set Bus Route">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRouteID" runat="server" Text='<%#Eval("BusRouteID")%>' Visible ="false" ></asp:Label>
                                        <asp:DropDownList ID="drpBusRoute" runat="server" OnSelectedIndexChanged ="drpBusRoute_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:Label ID="Fee" runat="server" Text='<%#Eval("Fee")%>' Visible ="True" ></asp:Label>
                                        <asp:LinkButton ID="lnkbtnBusRoute" runat="server" Text="Save"
                                            OnClientClick="return CnfSave()" OnClick ="lnkbtnBusRoute_Click" Visible="false"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Fee Amount" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="left" ItemStyle-Width="70px">
                                    <ItemTemplate>
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
                               <asp:TemplateField HeaderText="Hostel Fee Amount" HeaderStyle-HorizontalAlign="Left"
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
            
   
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btngo" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
