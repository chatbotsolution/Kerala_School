<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="InitStudentACTilDt.aspx.cs" Inherits="Masters_InitStudentACTilDt" %>

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
         var Month = document.getElementById("<%=drpMonth.ClientID %>").value;
         if (Month == "0") {
             alert("Select Month For Account Initialization !");
             document.getElementById("<%=drpMonth.ClientID %>").focus();
             return false;
         }
         else {
             return cnf();
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

     function cnf() {

         if (confirm("You Are Going To Initialize A Students Account ? Do You Want To Continue??")) {
             return true;
         }
         else {

             return false;
         }
     }

     function cnfDel() {

         if (confirm("You Are Going To Revert Initialized Student Account ? Do You Want To Continue??")) {
             return true;
         }
         else {

             return false;
         }
     }
        
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Initialize Student Account Till Date</h2>
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
                                        Text="Show Students" />&nbsp;<asp:Button ID="btnInitializedAC" runat="server" TabIndex="6"
                                            Text="Show Students(Already Initialized)" OnClick="btnInitializedAC_Click" OnClientClick="return add();" />
                                    <br />
                                    <br />
                                    
                                    <asp:Label ID="lblYear" runat="server" Text="Year: "></asp:Label>
                                    <span class="error">*</span>
                                    <asp:DropDownList ID="drpYear" runat="server" CssClass="tbltxtbox" Width="60px">
                                    </asp:DropDownList>
                                        &nbsp;&nbsp;
                                    <asp:Label ID="lblMonth" runat="server" Text="Month: "></asp:Label>
                                    <asp:DropDownList ID="drpMonth" runat="server" CssClass="tbltxtbox" Width="80px">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        <asp:ListItem Value="Apr">Apr</asp:ListItem>
                                        <asp:ListItem Value="May">May</asp:ListItem>
                                        <asp:ListItem Value="Jun">Jun</asp:ListItem>
                                        <asp:ListItem Value="Jul">Jul</asp:ListItem>
                                        <asp:ListItem Value="Aug">Aug</asp:ListItem>
                                        <asp:ListItem Value="Sep">Sep</asp:ListItem>
                                        <asp:ListItem Value="Oct">Oct</asp:ListItem>
                                        <asp:ListItem Value="Nov">Nov</asp:ListItem>
                                        <asp:ListItem Value="Dec">Dec</asp:ListItem>
                                        <asp:ListItem Value="Jan">Jan</asp:ListItem>
                                        <asp:ListItem Value="Feb">Feb</asp:ListItem>
                                        <asp:ListItem Value="Mar">Mar</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" OnClientClick="return addInit();"
                                        Text="Intialize Account" TabIndex="8" />
                                    <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Button ID="btnDelete" runat="server" Text="Revert A/c Initialization" Visible="false" OnClientClick="cnfDel();"
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
                                            
                                            <asp:TemplateField HeaderText="Fee Due">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalance" runat="server" Text='<%#Eval("Balance")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fee Paid" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left"
                                                ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFeeAmount" runat="Server" Text='<%#Eval("Balance")%>' CssClass="vsmalltb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField  HeaderText="Bus Fee Due">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBusFee" runat="server" Text='<%#Eval("BusFee")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField  HeaderText="Bus Fee Paid" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left"
                                                ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtBusAmount" runat="Server" Text='<%#Eval("BusFee")%>' CssClass="vsmalltb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField  HeaderText="Hostel Fee Due">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHostFee" runat="server" Text='<%#Eval("HostFee")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField  HeaderText="Hostel Fee Paid" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left"
                                                ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtHostAmount" runat="Server" Text='<%#Eval("HostFee")%>' CssClass="vsmalltb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Due">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotDue" runat="server" Text='<%#Eval("Total")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
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
                                                    <input name="Checkb" type="checkbox" value='<%#Eval("InvoiceReceiptNo")%>' />
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

