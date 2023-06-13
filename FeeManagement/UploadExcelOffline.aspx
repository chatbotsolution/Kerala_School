<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="UploadExcelOffline.aspx.cs" Inherits="FeeManagement_UploadExcelOffline" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29">
    </div>
    <div style="padding-top: 5px;">
        <h2>
            Receive Fee Through Bank (Offline)</h2>
    </div>
    <div class="spacer">
    </div>
    <table width="100%" class="cnt-box" style="padding:0px !important";>
        <tr>
            <td style="width: 100%;" valign="middle" class="tbltxt">
                &nbsp; Upload Bank Excel File :
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="width: 100%;" valign="middle" class="tbltxt">
                <asp:Button ID="Button1" Text="Upload File" runat="server" OnClick="Button1_Click" />
            </td>
        </tr>
        
    </table>
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="2" valign="top">
                <asp:GridView ID="grdstudentsColl" runat="server" AlternatingRowStyle-CssClass="alt"
                    AutoGenerateColumns="False" CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr"
                    TabIndex="5" Width="100%" OnSelectedIndexChanged="grdstudentsColl_SelectedIndexChanged">
                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                    <Columns>
                     <asp:TemplateField HeaderText="Trans No">
                            <ItemTemplate>
                                <asp:Label ID="lbltrnno" runat="server" Text='<%#Eval("TRNSCTN_NMBR")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>

                     

                        <asp:TemplateField HeaderText="Admission No">
                            <ItemTemplate>
                                <asp:Label ID="lbladmnno" runat="server" Text='<%#Eval("ADMISSIONNO")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Session">
                            <ItemTemplate>
                                <asp:Label ID="lblSession" runat="server" Text='<%#Eval("SESSION")%>'  ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Quarter">
                            <ItemTemplate>
                                <asp:Label ID="lblquater" runat="server" Text='<%#Eval("QUARTER")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fee Description">
                            <ItemTemplate>
                                <asp:Label ID="lblfeedesc" runat="server" Text='<%#Eval("FEE_DESC")%>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Amt">
                            <ItemTemplate>
                                <asp:Label ID="lbltotamt" runat="server" Text='<%#Eval("TOTL_AMNT")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                       <%-- <asp:TemplateField HeaderText="Mode">
                            <ItemTemplate>
                                <asp:Label ID="lblmode" runat="server" Text='<%#Eval("TRNSCTN_MODE")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cheq Number">
                            <ItemTemplate>
                                <asp:Label ID="lblcheqno" runat="server" Text='<%#Eval("CHQE_NMBR")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cheq Dt">
                            <ItemTemplate>
                                <asp:Label ID="lblchqdt" runat="server" Text='<%#Eval("CHQE_DATE")%>'></asp:Label>
                                

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Authorise">
                            <ItemTemplate>
                                <asp:Label ID="lblauth" runat="server" Text='<%#Eval("ATHRSD_FLAG")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Clearance">
                            <ItemTemplate>
                                <asp:Label ID="lblclear" runat="server" Text='<%#Eval("CLRNCE_FLAG")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Branch Name">
                            <ItemTemplate>
                                <asp:Label ID="lblBrnch" runat="server" Text='<%#Eval("BRNCH_NAME")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Receipt Dt">
                            <ItemTemplate>
                                <asp:Label ID="lblrecdt" runat="server" Text='<%#Eval("RECIEPTDATE")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        
                        <%--<asp:CommandField ShowSelectButton="True"/>--%>
                    </Columns>
                    <PagerStyle CssClass="pgr"></PagerStyle>
                </asp:GridView>
            </td>
        </tr>
        <tr>
        <td >
        <asp:Panel ID="Panel1" runat="server" style="margin-top:20px;">
                <div width="100%">
                <div style="display:inline-block"> Student Name:  <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label></div>
                 <div style="display:inline-block; margin-left:200px;">Admission No : <asp:Label ID="lblAdmnNo" runat="server" Text="Label"></asp:Label></div>
                  <div style="display:inline-block;  margin-left:200px;"> Class: <asp:Label ID="lblClass" runat="server" Text="Label"></asp:Label></div>
                  <div style="display:inline-block;float:right"> 
                     <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Hide"/></div>
                </div>
                    <table  cellspacing="0" frame="border" width="100%" class="mGrid tbltxt">
                         <tr style="background-color:#1a7fb5; color:white;">
                            <th> 
                                Tuition Fee
                            </th>
                            <th >
                                Special Activity
                            </th>
                            <th >
                                Smart Class
                            </th>
                            <th>
                                Eng Lab
                            </th>
                            <th>
                                Computer Economics
                            </th>
                            <th>
                                General Fee
                            </th>
                            <th>
                                Library
                            </th>
                            <th>
                                Maintainance
                            </th>
                            <th>
                                SSS
                            </th>
                            <th>
                                Sc.Lab
                            </th>
                            <th>
                                Stationary& Exam
                            </th>
                            <th>
                                Late Fine
                            </th>
                            <th>
                                Commision Amount
                            </th>
                            <th >
                                Total Amount
                            </th>
                        </tr>
                        <tr style="text-align: center">
                        
                        <td> <asp:Label ID="lblTtnFee" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblSplAct" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblSmtCls" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblEngLab" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblComp" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblGfee" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblLib" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblMain" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblsss" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblSclab" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblSExam" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblLfine" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblCAmount" runat="server" Text="Label"></asp:Label></td>
                        <td> <asp:Label ID="lblTotalAmnt" runat="server" Text="Label"></asp:Label></td>

                        </tr>
                        
                    </table>
                </asp:Panel>
        </td>
        </tr>
    </table>

</asp:Content>

