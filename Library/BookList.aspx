<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="BookList.aspx.cs" Inherits="Library_BookList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">

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

        function openwindow() {
            var pageurl = "rptBookDetailsPrint.aspx";
            window.open(pageurl, 'true', 'true');
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
        function CnfDelete() {

            if (confirm("You are going to delete a record. Do you want to continue?")) {

                return true;
            }
            else {
                return false;
            }
        }


        function checkboxes() {
            var inputElems = document.getElementsByTagName("input"),
            count = 0;

            for (var i = 0; i < inputElems.length; i++) {
                if (inputElems[i].type == "checkbox" && inputElems[i].checked == true) {
                    count++;
                }
            }
                       alert(count);
//            if (count > 5) {

//            }
        }

    </script>

    <asp:UpdatePanel ID="upp1" runat="server">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Book List</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <div style="width: 100%; background-color: #666; padding: 1px; margin: 0 auto;">
                <div style="background-color: #FFF; padding: 5px;">
                    <table border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                        <tr>
                            <td>
                                Category :
                                <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" CssClass="tbltxtbox"
                                    Width="100px" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                    <asp:ListItem Text="---All---" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                Subject :
                                <asp:DropDownList ID="ddlSubject" runat="server" CssClass="tbltxtbox">
                                    <asp:ListItem Text="---All---" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                               
                                Publisher :
                                <asp:DropDownList ID="ddlPublisher" runat="server" CssClass="tbltxtbox">
                                    <asp:ListItem Text="---All---" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Author :
                                <asp:TextBox ID="txtAuthor" runat="server" MaxLength="50" CssClass="tbltxtbox"></asp:TextBox>
                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1"  TargetControlID="txtAuthor" MinimumPrefixLength="1"  CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" runat="server" ContextKey="51"
  FirstRowSelected="false" ServiceMethod="AutoCompleteLib"  CompletionListCssClass="AutoExtender" CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"  ></ajaxToolkit:AutoCompleteExtender>

                                
                                
                                Book Name :
                                <asp:TextBox ID="txtBookName" runat="server" MaxLength="50" CssClass="tbltxtbox"></asp:TextBox>
                                 <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"  TargetControlID="txtBookName" MinimumPrefixLength="1"  CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" runat="server" ContextKey="60"
  FirstRowSelected="false" ServiceMethod="AutoCompleteLib"  CompletionListCssClass="AutoExtender" CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"  ></ajaxToolkit:AutoCompleteExtender>

                                

                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="width: 100%; background-color: #666; padding: 1px; margin: 0 auto; margin-top: 15px;">
                <div style="background-color: #FFF; padding: 5px;">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                        <tr>
                            <td align="left">
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" OnClientClick="return CnfDelete();" />&nbsp;
                                <asp:Button ID="btnAdd" runat="server" Text="Add New" OnClick="btnAdd_Click" />
                            </td>
                            <td align="right">
                                <asp:Label ID="Label4" runat="server" Text="From Acc.No." Visible="false"></asp:Label>
                                <asp:TextBox ID="TxtfrmAcc" runat="server" Visible="false"></asp:TextBox>
                                <asp:Label ID="Label5" runat="server" Text="ToAcc.No." Visible="false"></asp:Label>
                                <asp:TextBox ID="TxttoAcc" runat="server" Visible="false"></asp:TextBox>
                                 </td>

                        <td align="right">
                        <asp:Button ID="ButnSearch" runat="server" Text="Search Grid" Visible="false" 
                                onclick="ButnSearch_Click"  />
                       </td>
                        </tr>

                        <tr>
                        <td colspan="2">
<%--
                           <asp:CheckBoxList ID="Cblist" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="Cblist_IndexChanged" AutoPostBack="true">
                           <asp:ListItem Value="1" Selected="True">Book Title</asp:ListItem>
                           <asp:ListItem Value="2">Book SubTitle</asp:ListItem>
                           <asp:ListItem Value="3" Selected="True">Category</asp:ListItem>
                           <asp:ListItem Value="4" Selected="True">Author Name1</asp:ListItem>
                           <asp:ListItem Value="5">Author Name2</asp:ListItem>
                           <asp:ListItem Value="6">Author Name3</asp:ListItem>
                            <asp:ListItem Value="7" Selected="True">Publisher</asp:ListItem>
                           <asp:ListItem Value="8" Selected="True">Book No</asp:ListItem>
                           <asp:ListItem Value="9" >Dimension</asp:ListItem>
                           
                           </asp:CheckBoxList>--%>

                            <asp:CheckBox ID="cbBkTitle" runat="server" Text="BookTitle" oncheckedchanged="cbBkTitle_CheckedChanged" Checked="true" AutoPostBack="true"/>
                            <asp:CheckBox ID="cbBkSubTitle" runat="server" Text="BookSubTitle" oncheckedchanged="cbBkSubTitle_CheckedChanged" AutoPostBack="true" />
                          
                           <asp:CheckBox ID="cbCat" runat="server" Text="Category"   Checked="true"
                                oncheckedchanged="cbCat_CheckedChanged"  AutoPostBack="true"/>
                                 <asp:CheckBox ID="cbSubject" runat="server" Text="Subject"   Checked="true"
                                oncheckedchanged="cbSubject_CheckedChanged"  AutoPostBack="true"/>


                             <asp:CheckBox ID="cbAuthNm1" runat="server" Text="AuthorName1"  Checked="true"
                                oncheckedchanged="cbAuthNm1_CheckedChanged"  AutoPostBack="true"/>
                           
                            <asp:CheckBox ID="cbAuthNm2" runat="server" Text="AuthorName2" 
                                oncheckedchanged="cbAuthNm2_CheckedChanged"  AutoPostBack="true"/>
                          
                            <asp:CheckBox ID="cbAuthNm3" runat="server" Text="AuthorName3" 
                                oncheckedchanged="cbAuthNm3_CheckedChanged"  AutoPostBack="true"/>
                                                    
                                <asp:CheckBox ID="cbPublishNm" runat="server" Text="Publisher"  Checked="true"
                                oncheckedchanged="cbPublishNm_CheckedChanged"  AutoPostBack="true"/>

                            <asp:CheckBox ID="cbBk_No" runat="server" Text="Book No"  Checked="true"
                                oncheckedchanged="cbBk_No_CheckedChanged"  AutoPostBack="true"/>
                                
                            <asp:CheckBox ID="cbDimen" runat="server" Text="Dimension" 
                                oncheckedchanged="cbDimen_CheckedChanged"  AutoPostBack="true"/>
                                
                            
                         
                        
                        </td>
    </tr>



                        <tr>
                            <td colspan="3">
                                <asp:GridView ID="grdBookList" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                    PageSize="10" Width="100%" OnPageIndexChanging="grdBookList_PageIndexChanging"
                                    CssClass="mGrid">
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="checkbox" name="Checkb" value='<%# Eval("BookId") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <a href='NewBookEntry.aspx?BookId=<%#Eval("BookId")%>'>Edit</a>
                                                <asp:Label ID="LblBkid" runat="server" Text='<%#Eval("BookId")%>' Visible="false"></asp:Label>

                                                 <asp:Label ID="Label1" runat="server" Text='<%#Eval("SubjectId")%>' Visible="false"></asp:Label>
                                                  <asp:Label ID="Label2" runat="server" Text='<%#Eval("Remarks")%>' Visible="false"></asp:Label>
                                                   <asp:Label ID="Label3" runat="server" Text='<%#Eval("Authortype")%>' Visible="false"></asp:Label>
                                                    <%--<asp:Label ID="Label4" runat="server" Text='<%#Eval("BookId")%>' Visible="false"></asp:Label>--%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>

                                        <%--  <asp:TemplateField HeaderText="Accession No">
                                            <ItemTemplate>
                                                <%#Eval("AccessionNo")%>
                                                <asp:TextBox ID="TxtAccNo" runat="server" Text='<%#Bind("AccessionNo")%>' Enabled="false" Visible="false" ></asp:TextBox>
                                                  
                                            </ItemTemplate>
                                            <HeaderStyle   HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
--%>
                                        <asp:TemplateField HeaderText="Book Title">
                                            <ItemTemplate>
                                              <%--  <%#Eval("BookTitle")%>--%>
                                                <asp:TextBox ID="TxtBktitle" runat="server" Text='<%#Bind("BookTitle")%>' Enabled="false" Width="92%" ></asp:TextBox>
                                                  
                                            </ItemTemplate>
                                            <HeaderStyle   HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Book SubTitle"  Visible="false">
                                            <ItemTemplate>
                                              <%--  <%#Eval("BookTitle")%>--%>
                                                <asp:TextBox ID="TxtBkSubtitle" runat="server" Text='<%#Bind("BookSubTitle")%>' Enabled="false" Width="92%" ></asp:TextBox>
                                                  
                                            </ItemTemplate>
                                            <HeaderStyle   HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Category" >
                                            <ItemTemplate>
                                             <%--   <%#Eval("CatName")%> --%>
                                             <asp:Label ID="hfDdlCatValue" runat="server" Text='<%#Bind("CatCode")%>' Visible="false"></asp:Label>
                                                <asp:DropDownList ID="DdlCat" runat="server" Enabled="false" Width="95%"> </asp:DropDownList>
                                            </ItemTemplate>
                                            <HeaderStyle  HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Subject" >
                                            <ItemTemplate>
                                             <%--   <%#Eval("CatName")%> --%>
                                             <asp:Label ID="hfDdlSubject" runat="server" Text='<%#Bind("SubjectId")%>' Visible="false"></asp:Label>
                                                <asp:DropDownList ID="DdlSubject" runat="server" Enabled="false" Width="95%"> </asp:DropDownList>
                                            </ItemTemplate>
                                            <HeaderStyle  HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Author1"  >
                                            <ItemTemplate>
                                               <%-- <%#Eval("AuthorName1")%>--%>
                                                <%--<asp:TextBox ID="TxtAutNm1" runat="server" Text='<%#Eval("AuthorName1")+" "+Eval(" AuthorLastName1")%>' Enabled="false"  Width="66px"></asp:TextBox>--%>
                                                <asp:TextBox ID="TxtAutNm1" runat="server" Text='<%#Eval("AuthorName1")%>' Enabled="false"  Width="43%"></asp:TextBox>
                                                 <asp:TextBox ID="txtLName1" runat="server" Text='<%#Bind("AuthorLastName1")%>' Enabled="false" Width="43%"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle  HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                         <asp:TemplateField  HeaderText="Author2" Visible="false">
                                            <ItemTemplate>
                                               <%-- <%#Eval("AuthorName2")%>--%>
                                                <asp:TextBox ID="TxtAutNm2" runat="server" Text='<%#Bind("AuthorName2")%>' Enabled="false"  Width="43%"></asp:TextBox>
                                                <asp:TextBox ID="txtLName2" runat="server" Text='<%#Bind("AuthorLastName2")%>' Enabled="false" Width="43%"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle  HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                         <asp:TemplateField  HeaderText="Author3"  Visible="false">
                                            <ItemTemplate>
                                              <%--  <%#Eval("AuthorName3")%>--%>
                                               <asp:TextBox ID="TxtAutNm3" runat="server" Text='<%#Bind("AuthorName3")%>' Enabled="false" Width="92%"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle  HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Publisher" >
                                            <ItemTemplate>
                                              <%--  <%#Eval("PublisherName")%>--%>
                                               <asp:Label ID="hfDdlPubValue" runat="server" Text='<%#Bind("PublisherId")%>' Visible="false"></asp:Label>
                                                <asp:DropDownList ID="DdlPub" runat="server" Enabled="false" Width="95%"> </asp:DropDownList>
                                            </ItemTemplate>
                                            <HeaderStyle  HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="BookNo">
                                            <ItemTemplate>
                                               <%-- <%#Eval("Book_No")%>--%>
                                                <asp:TextBox ID="TxtBkNo" runat="server" Text='<%#Bind("Book_No")%>' Enabled="false" Width="92%" ></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle  HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Dimension"  Visible="false">
                                            <ItemTemplate>
                                              <%--  <%#Eval("Dimension")%>--%>
                                               <asp:TextBox ID="TxtDimen" runat="server" Text='<%#Bind("Dimension")%>' Enabled="false"  Width="92%"  ></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle  HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Record
                                    </EmptyDataTemplate>
                                    <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                                    <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                        <td align="left">
                            <asp:Button ID="BtnEdit" runat="server" Text="Edit Grid" OnClick= "BtnEdit_Click" />
                        <asp:Button ID="BtnUpdate" runat="server" Text="Update" onclick="BtnUpdate_Click" />
                        </td>

                         <td align="right">
                                <asp:Label ID="lblRecords" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                      

                        </tr>
                    </table>
                </div>
            </div>
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
