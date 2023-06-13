<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="InitStudHostAC.aspx.cs" Inherits="Hostel_InitStudHostAC" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script language="javascript" type="text/javascript">
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
        function add() {

            var Class = document.getElementById("<%=drpClass.ClientID %>").value;


            if (Class == 0) {
                alert("Select a Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
        }
        function addInit() {
            var date = document.getElementById("<%=txtDate.ClientID %>").value;
            if (date == "") {
                alert("Enter  date for Initialize Account !");
                document.getElementById("<%=txtDate.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }

        }
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function beginRequest(sender, args) {
            // show the popup
            $find('<%=mdlloading.ClientID %>').show();

        }

        function endRequest(sender, args) {
            //  hide the popup
            $find('<%=mdlloading.ClientID %>').hide();

        }
    </script>
    
     <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Initialize Student Hostel Account</h2>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="height: 360px;" width="100%">
                <tr>
                    <td style="width: 100%;" valign="top">
                        <table width="100%">
                            <tr>
                                <td align="left" valign="top" class="tbltxt" style="border: solid 1px Black; width: 100%;"
                                    colspan="2">
                                    <asp:Label ID="lblSession" runat="server" Text="Session:"></asp:Label>
                                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                                        Width="100px" CssClass="tbltxtbox" TabIndex="1">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblClass" runat="server" Text="Class:"></asp:Label>
                                    <span class="error">*</span>
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                        Width="100px" CssClass="tbltxtbox" TabIndex="2">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblSection" runat="server" Text="Section:"></asp:Label>
                                    <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSection_SelectedIndexChanged"
                                        Width="50px" CssClass="tbltxtbox" TabIndex="3">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblStudent" runat="server" Text="Select Student:"></asp:Label>
                                    <asp:DropDownList ID="drpSelectStudent" runat="server" CssClass="tbltxtbox" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="drpSelectStudent_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" TabIndex="6" OnClientClick="return add();"
                                        Text="Show Students" />&nbsp;
                                        <asp:Button ID="btnFullSession" runat="server" Text="Show Full Session Amount" OnClientClick="return add();"
                                        Visible="true" onclick="btnFullSession_Click"/>
                                        &nbsp;
                                        <asp:Button ID="btnInitializedAC" runat="server" TabIndex="6"
                                            Text="Show Students(Already Initialized)" OnClick="btnInitializedAC_Click" OnClientClick="return add();" />
                                    <br />
                                    <br />
                                    <asp:Label ID="lblDate" runat="server" Text="Date Initialized: "></asp:Label>
                                    <span class="error">*</span>
                                    <asp:TextBox ID="txtDate" runat="server" Width="80px" CssClass="tbltxtbox" TabIndex="5"></asp:TextBox>&nbsp;<rjs:PopCalendar
                                        ID="PopCalendar3" runat="server" Control="txtDate" Format="dd mm yyyy"></rjs:PopCalendar>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" OnClientClick="return addInit();"
                                        Text="Intialize Account" TabIndex="8" />
                                    <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Button ID="btnDelete" runat="server" Text="Revert A/c Initialization" Visible="false"
                                        OnClick="btnDelete_Click" />
                                </td>
                                <td align="right" valign="top" style="height: 15px; width: 100px;" class="tbltxt">
                                    <asp:Label ID="lblRecCount" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:GridView ID="grdStudAC" runat="server" AutoGenerateColumns="False" Width="100%"
                                        CssClass="mGrid" AlternatingRowStyle-CssClass="alt" TabIndex="7">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Admission No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmnNo" runat="server" Text='<%#Eval("Admissionno")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("FullName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldob" runat="server" Text='<%#Eval("dateofbirth")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Due">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotDue" runat="server" Text='<%#Eval("Total")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Fee Due">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalance" runat="server" Text='<%#Eval("Balance")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Fee Paid" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left"
                                                ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFeeAmount" runat="Server" Text='<%#Eval("Amount")%>' CssClass="vsmalltb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gvInitializedAc" runat="server" AutoGenerateColumns="False" Width="100%"
                                        CssClass="mGrid" AlternatingRowStyle-CssClass="alt" TabIndex="8">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                <ItemTemplate>
                                                    <input name="Checkb" type="checkbox" value='<%#Eval("Admissionno")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Admission No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmnNo" runat="server" Text='<%#Eval("Admissionno")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("FullName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldob" runat="server" Text='<%#Eval("dateofbirth")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Initialized Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInitAmount" runat="server" Text='<%#Eval("InitAmount")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../Images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


