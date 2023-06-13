<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="NewAdmnFeeGen.aspx.cs" Inherits="Admissions_NewAdmnFeeGen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
           New Admn Fee</h2>
    </div>
<%--    <br />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <table style="height: 360px;" width="100%">
                <tr>
                    <td style="width: 100%;" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top" class="tbltxt cnt-box" width="100%;"
                                    colspan="2">
                                    <div class="cnt-sec">
                                    <span class="ttl3"><asp:Label ID="lblSession" runat="server" Text="Session :"></asp:Label></span>
                                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" Width="100px"
                                         CssClass="tbltxtbox"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                    </div>
                                     <div class="cnt-sec">
                                    <span class="ttl3">
                                    <asp:Label ID="lblClass" runat="server" Text="Class :"></asp:Label></span>
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" Width="100px"
                                        OnSelectedIndexChanged="drpClass_SelectedIndexChanged" CssClass="tbltxtbox" TabIndex="2">
                                    </asp:DropDownList>
                                      </div>
                                    
                                    
                                    
                                    </div>
                                    <asp:Button ID="btnFee" runat="server" OnClick="btnFee_Click" TabIndex="6" Text="Gen Fee"
                                        />
                                </td>
                            </tr>
                             <tr>
                             <td colspan="2"> &nbsp;</td>
                             </tr>
                            
                            <tr>
                                <td align="right" valign="top">
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" style="height: 15px; width: 50%" valign="top">
                                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                </td>
                                <td align="right" class="tbltxt" style="height: 15px" valign="top">
                                    <asp:Label ID="lblRecCount" runat="server" Text="Label" Width="300px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:GridView ID="grdstudents" runat="server" AlternatingRowStyle-CssClass="alt"
                                        AutoGenerateColumns="false" CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr"
                                        TabIndex="5" Width="100%">
                                        <Columns>
                         
                                            <asp:TemplateField HeaderText="Admission No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbladmnno" runat="server" Text='<%#Eval("OldAdmnNo")%>'></asp:Label>
                                                    <asp:HiddenField ID="hfAdNo" runat="server" Value='<%#Eval("AdmsnNo") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("fullname")%>'></asp:Label>
                                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("AdmsnNo")%>' Visible="false"></asp:Label>
                                                     <asp:Label ID="lblSixthSub" runat="server" Text='<%#Eval("SixthOptional")%>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                          
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                </tr>
            </table>
            
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

