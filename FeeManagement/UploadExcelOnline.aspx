<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="UploadExcelOnline.aspx.cs" Inherits="FeeManagement_UploadExcelOnline" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29">
    </div>
    <div style="padding-top: 5px;">
        <h2>
            Receive Fee Through Bank </h2>
    </div>
    <div class="spacer">
    </div>
    <table width="100%" class="cnt-box" style="padding:0px !important";>
        <tr>
        <td style="width: 100%;" valign="middle" class="tbltxt" align="right">
        <%--<asp:RadioButtonList runat="server" ID="RbUploadType" RepeatDirection="Horizontal">
        <asp:ListItem Value="0" Selected="True">Dailywise</asp:ListItem>
        <asp:ListItem Value="1">Datewise</asp:ListItem>
        </asp:RadioButtonList>--%>
        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
        </td>
        </tr>
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
        <td  class="tbltxt">
        <span id="spanError" runat="server" style="color:Red"> Fee Can not be uploaded for below students, kindly check again.</span>
       
        </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <asp:GridView ID="grdstudentsColl" runat="server" AlternatingRowStyle-CssClass="alt"
                    AutoGenerateColumns="False" CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr"
                    TabIndex="5" Width="100%" 
                    OnSelectedIndexChanged="grdstudentsColl_SelectedIndexChanged" 
                    EmptyDataText="All Fees Inserted." >
                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Admission No">
                            <ItemTemplate>
                                <asp:Label ID="lbladmnno" runat="server" Text='<%#Eval("AdmnNo")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                      <%--  <asp:TemplateField HeaderText="Transaction No">
                            <ItemTemplate>
                                <asp:Label ID="lbTRNSCTN_NMBR" runat="server" Text='<%#Eval("TRNSCTN_NMBR")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Student Name">
                            <ItemTemplate>
                                <asp:Label ID="lblname" runat="server" Text='<%#Eval("Student_Name")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Class">
                            <ItemTemplate>
                                <asp:Label ID="lblClass1" runat="server" Text='<%#Eval("Class")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sec">
                            <ItemTemplate>
                                <asp:Label ID="lblSec" runat="server" Text='<%#Eval("Section")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Year">
                            <ItemTemplate>
                                <asp:Label ID="lblYear" runat="server" Text='<%#Eval("Year")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tution Fee">
                            <ItemTemplate>
                                <asp:Label ID="lblTuition" runat="server" Text='<%#Eval("Tuition_Fees")%>'></asp:Label>
                                  <asp:Label ID="lblSplAct1" runat="server" Text='<%#Eval("Activities_Fees")%>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblSmtCls1" runat="server" Text='<%#Eval("Smart_Class_Fees")%>' Visible="false"></asp:Label>
                                      <asp:Label ID="lblEngLab1" runat="server" Text='<%#Eval("English_LL_ Fees")%>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblComp1" runat="server" Text='<%#Eval("Computer_Fees")%>' Visible="false"></asp:Label>
                                          <asp:Label ID="lblGfee1" runat="server" Text='<%#Eval("General_Fees")%>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblLib1" runat="server" Text='<%#Eval("Library_Fees")%>' Visible="false"></asp:Label>
                                              <asp:Label ID="lblMain1" runat="server" Text='<%#Eval("Maintenance_Fees")%>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblsss1" runat="server" Text='<%#Eval("SSS_Fees")%>' Visible="false"></asp:Label>
                                                 <asp:Label ID="lblSclab1" runat="server" Text='<%#Eval("SC_Lab_ Fees")%>' Visible="false"></asp:Label>
                                                  <asp:Label ID="lblSExam1" runat="server" Text='<%#Eval("Stationery_Exam_Fees")%>' Visible="false"></asp:Label>
                                                  

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Late Fine">
                            <ItemTemplate>
                                <asp:Label ID="lblFine" runat="server" Text='<%#Eval("Late_Fees")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                      <%--  <asp:TemplateField HeaderText="Commission Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblCAmnt1" runat="server" Text='<%#Eval("COMISSION_AMNT")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>--%>
                       <%-- <asp:TemplateField HeaderText="Total Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblTamnt" runat="server" Text='<%#Eval("Total Amount")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>--%>
                       <%-- <asp:TemplateField HeaderText="Payment Mode">
                            <ItemTemplate>
                                <asp:Label ID="lblTmode" runat="server" Text='<%#Eval("Transaction Mode")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Error Message">
                            <ItemTemplate>
                                <asp:Label ID="lblCdate" runat="server" Text='<%#Eval("Error")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                       <%-- <asp:CommandField ShowSelectButton="True" />--%>
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

