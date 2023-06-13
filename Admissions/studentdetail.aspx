<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="studentdetail.aspx.cs" Inherits="Admissions_studentdetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td align="left">
                <fieldset id="fsselection" runat="server">
                 <legend style="background-color: window" id="lgprchs" class="pageLabel" runat="server">
                            Selection Criteria</legend>
                    <table width="100%">
                        <tr>
                            <td>
                                <strong>Session:<asp:TextBox ID="txtsession" runat="server" TabIndex="1"></asp:TextBox></strong></td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Select Class:<asp:DropDownList ID="drpclass" runat="server" 
                                    TabIndex="2">
                                </asp:DropDownList>&nbsp; Select Student:<asp:DropDownList ID="drpstudents" 
                                    runat="server" TabIndex="3">
                                </asp:DropDownList>
                                    <asp:Button ID="btngo" runat="server" OnClick="btngo_Click" Text="GO" 
                                    TabIndex="3" /></strong></td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdstudlist" runat="server" Width="100%" AutoGenerateColumns="False"
                    AllowPaging="True" TabIndex="5">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Admission No.">
                            <ItemTemplate>
                                <asp:Label ID="lblrollno" Font-Bold="true" runat="server" Text='<%# Eval("admnno") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblname" runat="server" Text='<%#Eval("fullname")%>'></asp:Label>
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
                        <asp:TemplateField HeaderText="Class">
                            <ItemTemplate>
                                <asp:Label ID="lblclass" runat="server" Text='<%#Eval("class")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFEFEF" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#FDFDFD" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataList ID="DataList1" runat="server" BackColor="Transparent" BorderColor="#DEBA84"
                    BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="Both"
                    Width="100%" TabIndex="6">
                    <ItemTemplate>
                        <table border="0" width="100%">
                            <tr>
                                <td>
                                    <table align="left" border="0" width="100%">
                                        <tr>
                                            <td align="right" colspan="4">
                                                <asp:Button ID="btnedit" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"sid")%>'
                                                    CommandName="edit" Text="Edit" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span style="color: red"><strong><u>Personal Details</u></strong></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Name:</strong>
                                            </td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"NAME")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>SSN:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"SSN")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="210">
                                                <strong>Address: </strong>
                                            </td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"STREET")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Sex: </strong>
                                            </td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"SEX")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>City:</strong>
                                            </td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"CITY")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Marital Status:</strong>
                                            </td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"MARSTAT")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>State:</strong>
                                            </td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"STATE")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Ethnic: </strong>
                                            </td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"ETHNIC")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>ZIP: </strong>
                                            </td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"ZIP")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>US Citizen:</strong>
                                            </td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"USCITI")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Home Phone: </strong>
                                            </td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"HPHONE")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>US Present Resident: </strong>
                                            </td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"USPRES")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Work Phone:</strong>
                                            </td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"WPHONE")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Lead Source: </strong>
                                            </td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"LEADS")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Date Of Birth: </strong>
                                            </td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"strDOB")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Status: </strong>
                                            </td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"STATUS")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table align="left" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <span style="color: red"><strong><u>Previous school Details</u></strong></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Previous School Name:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"HSNAME")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>HS Grad or GED Date(mmyy):</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"HSGRADGED")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Scheduled Grad Date:</strong></td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"SGRADATE")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>HS Transcript Recvd:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"HSTRAN")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Highest Degree:</strong>
                                            </td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"HDEGREE")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table align="left" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <span style="color: red"><strong><u>Class Details</u></strong></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Major Code:</strong>
                                            </td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"MAJORC")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Enrolment Date:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"ENRODATE")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Start Date:</strong></td>
                                            <td align="left" style="width: 320px">
                                                <%#DataBinder.Eval(Container.DataItem,"STARTDATE")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Class Date:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"CLASSDATE")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>LDA-LDG:</strong></td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"LDA_LDG")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>RETDD-LOA:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"RETDD_LOA")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>ACADPROB:</strong></td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"ACADPROB")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>ACP Date:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"ACPDATE")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>ATTPROB:</strong></td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"ATTPROB")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>ATP Date:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"ATPDATE")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Major Init:</strong></td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"MAJORINIT")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>RET-Shift:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"RETSHIFT")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Original Shift:</strong></td>
                                            <td align="left" style="width: 320px">
                                                <%#DataBinder.Eval(Container.DataItem,"ORISHIFT")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Current Shift:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"CURRSHIFT")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>FIN-AID:</strong></td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"FIN_AID")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>ICT-I20:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"ICTI_20")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Down Payment:</strong></td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"DEPODNPAY")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>FT_PT:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"FT_PT")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Drop Code:</strong></td>
                                            <td align="left" style="width: 320px">
                                                <%#DataBinder.Eval(Container.DataItem,"DROPCODE")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Remarks:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"REMARKS")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>LOA Code:</strong></td>
                                            <td align="left" width="320">
                                                <%#DataBinder.Eval(Container.DataItem,"LOACODE")%>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Update Date:</strong></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(Container.DataItem,"UPDATE")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <table border="0" width="100%">
                            <tr>
                                <td>
                                    <table align="left" border="0" width="100%">
                                        <tr>
                                            <td align="right" colspan="4">
                                                <asp:Button ID="Button1" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"sid")%>'
                                                    CommandName="update" Text="Update" />
                                                <asp:Button ID="Button2" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"sid")%>'
                                                    CommandName="cancel" Text="Cancel" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span style="color: red"><strong><u>Personal Details</u></strong></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Name:</strong>
                                            </td>
                                            <td align="left" width="320px">
                                                <asp:TextBox ID="txtname" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"NAME")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>SSN:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtssn" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SSN")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="210px">
                                                <strong>Address: </strong>
                                            </td>
                                            <td align="left" width="320px">
                                                <asp:TextBox ID="txtstreet" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"STREET")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Sex: </strong>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtsex" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SEX")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>City:</strong>
                                            </td>
                                            <td align="left" width="320px">
                                                <asp:TextBox ID="txtcity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CITY")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Marital Status:</strong>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtmarstat" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MARSTAT")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>State:</strong>
                                            </td>
                                            <td align="left" width="320px">
                                                <asp:TextBox ID="txtstate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"STATE")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Ethnic: </strong>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtethnic" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ETHNIC")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>ZIP: </strong>
                                            </td>
                                            <td align="left" width="320px">
                                                <asp:TextBox ID="txtzip" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ZIP")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>US Citizen:</strong>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtusciti" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"USCITI")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Home Phone: </strong>
                                            </td>
                                            <td align="left" width="320px">
                                                <asp:TextBox ID="txthpphone" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"HPHONE")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>US Present Resident: </strong>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtuspres" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"USPRES")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Work Phone:</strong>
                                            </td>
                                            <td align="left" width="320px">
                                                <asp:TextBox ID="txtwphone" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"WPHONE")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Lead Source: </strong>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtleads" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"LEADS")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Date Of Birth: </strong>
                                            </td>
                                            <td align="left" width="320px">
                                                <asp:TextBox ID="txtdob" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"strDOB")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Status: </strong>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtstatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"STATUS")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table align="left" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <span style="color: red"><strong><u>Highschool Details</u></strong></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>High School Name:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txthsname" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"HSNAME")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>HS Grad or GED Date(mmyy):</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txthsgrad" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"HSGRADGED")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Scheduled Grad Date:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtsgra" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SGRADATE")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>HS Transcript Recvd:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txthstran" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"HSTRAN")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Highest Degree:</strong>
                                            </td>
                                            <td align="left" width="320px">
                                                <asp:TextBox ID="txthdegree" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"HDEGREE")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table align="left" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <span style="color: red"><strong><u>Class Details</u></strong></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Major Code:</strong>
                                            </td>
                                            <td align="left" width="320px">
                                                <asp:TextBox ID="txtmajorc" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MAJORC")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Enrolment Date:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtenrodate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ENRODATE")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Start Date:</strong></td>
                                            <td align="left" style="width: 320px">
                                                <asp:TextBox ID="txtstartdate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"STARTDATE")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Class Date:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtclassdate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CLASSDATE")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>LDA-LDG:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtlda" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"LDA_LDG")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>RETDD-LOA:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtloa" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"RETDD_LOA")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>ACADPROB:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtaca" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ACADPROB")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>ACP Date:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtacp" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ACPDATE")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>ATTPROB:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtatt" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ATTPROB")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>ATP Date:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtatp" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ATPDATE")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Major Init:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtmajor" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MAJORINIT")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>RET-Shift:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtretshift" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"RETSHIFT")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Original Shift:</strong></td>
                                            <td align="left" style="width: 320px">
                                                <asp:TextBox ID="txtorishift" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ORISHIFT")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Current Shift:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtcurrshift" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CURRSHIFT")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>FIN-AID:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtfinaid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FIN_AID")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>ICT-I20:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txticti" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ICTI_20")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Down Payment:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtdownpayment" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"DEPODNPAY")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>FT_PT:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtftpt" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FT_PT")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Drop Code:</strong></td>
                                            <td align="left" style="width: 320px">
                                                <asp:TextBox ID="txtdropcode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"DROPCODE")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Remarks:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtremarks" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"REMARKS")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>LOA Code:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtloacode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"LOACODE")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Update Date:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtupdate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UPDATE")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table align="left" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <span style="color: red"><strong><u>Employment Details</u></strong></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Emp Job Title:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtempjob" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EJOBTITLE")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Emp Start Date:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtempstartdate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ESTARTDATE")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Salary ($):</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtsalary" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ESALARY")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Employer:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtemployer" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EMPLOYER")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Emp City:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtempcity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ECITY")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Emp State:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtempstate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ESTATE")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Emp Zip:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtempzip" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EZIP")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Emp Phone:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtempphone" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EPHONE")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Emp Contact:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtempcontact" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ECONTACT")%>'>
																</asp:TextBox>
                                            </td>
                                            <td align="left" style="width: 210px">
                                                <strong>Emp Month/Year:</strong></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtempmonyear" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EMOYR")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 210px">
                                                <strong>Placement Code:</strong></td>
                                            <td align="left" width="320">
                                                <asp:TextBox ID="txtpcode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PLACMNTCOD")%>'>
																</asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                </asp:DataList></td>
        </tr>
    </table>
</asp:Content>
