<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="FeeAmount.aspx.cs" Inherits="Masters_FeeAmount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function pageLoad() {
            document.getElementById("Loader").style.visibility = 'hidden';
        }
        function CheckLoader() {
            document.getElementById("Loader").style.visibility = 'visible';
        }

        function isValid() {
            var Str = document.getElementById("<%=drpStream.ClientID %>").value;
            var Str1 = document.getElementById("<%=drpClass.ClientID %>").value;
            debugger;
            if (Str1 == 1) {
                alert("Please Select a Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }

            if (Str == 0) {
                alert("Please Select a Stream !");
                document.getElementById("<%=drpStream.ClientID %>").focus();
                return false;
            }
            else {
                CheckLoader();
                return true;
            }

        }

       
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Fee Amount</h2>
    </div>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <table width="95%">
                <tr>
                    <td class="tbltxt">
                        &nbsp; Session Year:
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="tbltxtbox"
                            Width="100px" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                        </asp:DropDownList>
                         Class:
                        <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" CssClass="tbltxtbox"
                            Width="120px" onselectedindexchanged="drpClass_SelectedIndexChanged" >
                        </asp:DropDownList>
                        Stream:
                        <asp:DropDownList ID="drpStream" runat="server" AutoPostBack="True" CssClass="tbltxtbox"
                            Width="120px" onselectedindexchanged="drpStream_SelectedIndexChanged" Visible="true">
                        </asp:DropDownList>
                        &nbsp;<asp:Button ID="btnShow" OnClientClick="return isValid();" runat="server" Text="Show"
                            OnClick="btnShow_Click" style="height: 26px" />&nbsp;<asp:Button ID="btnSaveAddNew2" runat="server" OnClick="btnSaveAddNew_Click"
                                Text="Set Fee Amount" TabIndex="3" />
                        &nbsp;<asp:Button ID="BtnReGenFee" runat="server" Enabled="false" onclick="BtnReGenFee_Click" 
                            Text="Re-Generate Fee All" OnClientClick="return isValid();" Visible="false"/>
                        &nbsp;
                        <asp:Button ID="btnCopyAmt" runat="server" Visible="false"
                            Text="Copy Amount" onclick="btnCopyAmt_Click"  />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                

                <asp:Panel ID="grid" runat="server">
                <tr>
                <td class="tbltxt">
                   Class: <asp:Label runat="server" ID="lblClass" ></asp:Label> <span style="margin-left:200px;"> Stream :  <asp:Label ID="lblStream" runat="server" ></asp:Label></span>

                  
                </td>
                </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grdFeeAmount" runat="server" AlternatingRowStyle-CssClass="alt" Visible="true"
                                ShowFooter="true" AutoGenerateColumns="False" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                Width="100%">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFeeID" runat="server" Text='<%#Eval("FeeID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStreamID" runat="server" Text='<%#Eval("StreamID")%>'></asp:Label>
                                             <asp:Label ID="lblClassID" runat="server" Text='<%#Eval("ClassID")%>'></asp:Label>

                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Class" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClassName" runat="server" Text='<%#Eval("ClassName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stream" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fee Components">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFeeName" runat="server" Text='<%#Eval("FeeName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="400px" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Existing Students">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtExistingAmount" runat="server" Text='<%#Eval("ExistAmnt")%>'
                                                Width="95px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotTc" runat="server" Font-Bold="true" Text='<%#GetTot("ExistAmnt")%>'></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="New Students">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNewAmount" runat="server" Text='<%#Eval("NewAmnt")%>' Width="95px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotTc" Font-Bold="true" runat="server" Text='<%#GetTot("NewAmnt")%>'></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                   <%-- <asp:TemplateField HeaderText="TC Student">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTCAmount" runat="server" Text='<%#Eval("TCAmnt")%>' Width="95px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotTc" Font-Bold="true" runat="server" Text='<%#GetTot("TCAmnt")%>'></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Casual Student">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCasualAmount" runat="server" Text='<%#Eval("CasualAmnt")%>' Width="95px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotTc" Font-Bold="true" runat="server" Text='<%#GetTot("CasualAmnt")%>'></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                                <FooterStyle CssClass="logintxt" ForeColor="Black" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblTotFee" Font-Bold="true" runat="server" Text=''></asp:Label>&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnSaveAddNew" Width="125px" runat="server" OnClick="btnSaveAddNew_Click"
                                Text="Set Fee Amount" />
                            &nbsp;<asp:Button ID="btncancel" Width="125px" runat="server" OnClick="btncancel_Click"
                                Text="Cancel" />
                            <asp:HiddenField ID="hdnsts" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                </asp:Panel>
            </table>
        <%--</ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSaveAddNew" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>--%>
    
  <div id="Loader" style="text-align: center; vertical-align: middle; position: absolute;
        top: 0px; left: 0px; z-index: 99; width: 100%; height: 100%; background-color: #ededed;
        -ms-filter: 'progid:DXImageTransform.Microsoft.Alpha(Opacity=60)'; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=60);
        -moz-opacity: 0.8; opacity: 0.8;">
        <div style="width: 48px; height: 48px; margin: 0 auto; margin-top: 275px;">
            <img src="../images/spinner.gif">
        </div>
        <div style="font-family: Trebuchet MS; font-size: 12px; color: Green; text-align: center;">
            Please Wait ...</div>
    </div>
</asp:Content>

