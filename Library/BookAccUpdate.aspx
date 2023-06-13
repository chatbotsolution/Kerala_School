<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="BookAccUpdate.aspx.cs" Inherits="Library_BookAccUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" language="javascript">

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

     function isValid() {

         var Category = document.getElementById("<%=ddlBookCat.ClientID %>").selectedIndex;
         var Subject = document.getElementById("<%=ddlSubject.ClientID %>").selectedIndex;
         var BookName = document.getElementById("<%=txtTitle.ClientID %>").value;

         var Publisher = document.getElementById("<%=ddlPublisher.ClientID %>").value;

         if (Category == "0") {
             alert("Please select a Category !");
             document.getElementById("<%=ddlBookCat.ClientID %>").focus();
             return false;
         }
         if (Subject == "0") {
             alert("Please select a Subject !");
             document.getElementById("<%=ddlSubject.ClientID %>").focus();
             return false;
         }

         if (BookName.trim() == "") {
             alert("Please Enter book name !");
             document.getElementById("<%=txtTitle.ClientID %>").focus();
             document.getElementById("<%=txtTitle.ClientID %>").select();
             return false;
         }

         if (Publisher == "0") {
             alert("Please select Publisher !");
             document.getElementById("<%=ddlPublisher.ClientID %>").focus();
             return false;
         }

         else {
             return true;
         }
     }

     function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
         var key;
         var isCtrl = false;
         var keychar;
         var reg;

         if (window.event) {
             key = e.keyCode;
             isCtrl = window.event.ctrlKey
         }
         else if (e.which) {
             key = e.which;
             isCtrl = e.ctrlKey;
         }

         if (isNaN(key)) return true;

         keychar = String.fromCharCode(key);

         // check for backspace or delete, or if Ctrl was pressed
         if (key == 8 || isCtrl) {
             return true;
         }

         reg = /\d/;
         var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
         var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

         return isFirstN || isFirstD || reg.test(keychar);
     }
                
    </script>

    <asp:UpdatePanel ID="upp1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Add/Modify Accession</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <div style="width: 800px; margin: 0 auto;">
                <div style="float: left;">
                    <fieldset style="width: 350px;">
                        <legend class="tbltxt"><strong>Accession Details</strong></legend>
                        <table class="tbltxt">
                            <tr>
                            <td width="100px">
                                    Book Accession<span style="color: Red; font-size: small;">*</span> :
                                </td>
                                 <td>
                                    <asp:DropDownList ID="ddlBookAcc" runat="server" AutoPostBack="true" CssClass="tbltxt"
                                        OnSelectedIndexChanged="ddlBookAcc_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    
                                </td>
                                 </tr>

                                  <tr>
                                <td width="100px">
                                    Book Category<span style="color: Red; font-size: small;">*</span> :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBookCat" runat="server" AutoPostBack="true" CssClass="tbltxt"
                                        OnSelectedIndexChanged="ddlBookCat_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Subject <span style="color: Red; font-size: small;">*</span>:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSubject" runat="server" CssClass="tbltxt">
                                        <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Book Title <span style="color: Red; font-size: small;">*</span>:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" CssClass="tbltxtbox"></asp:TextBox>
                                    
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    Author1 :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAuthor1" runat="server" MaxLength="50" CssClass="tbltxtbox" 
                                        ontextchanged="txtAuthor1_TextChanged" AutoPostBack="True"></asp:TextBox>                               
                                </td>

                                <td>
                                <asp:DropDownList ID="ddlAuthortp" Width="80px" CssClass="tbltxtbox" runat="server" 
                                        AutoPostBack="True" onselectedindexchanged="ddlAuthortp_SelectedIndexChanged">
                               <asp:ListItem Value="Author" Selected="True">Author</asp:ListItem>
                               <asp:ListItem Value="Editor" >Editor</asp:ListItem>
                               <asp:ListItem Value="Illustrator" >Illustrator</asp:ListItem>
                               <asp:ListItem Value="Compiler" >Compiler</asp:ListItem>
                               <asp:ListItem Value="Publisher" >Publisher</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                               
                            </tr>
                            <tr valign="top">
                                <td>
                                    Author2 :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAuthor2" runat="server" MaxLength="50" CssClass="tbltxtbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    Author3 :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAuthor3" runat="server" MaxLength="50" CssClass="tbltxtbox"></asp:TextBox>
                                </td>
                            </tr>

                             

                        </table>
                    </fieldset>
                </div>
                <div style="float: left;">
                    <fieldset style="width: 350px;">
                        <legend class="tbltxt"><strong>Other Details</strong></legend>
                        <table width="100%" border="0" cellspacing="3" cellpadding="3" class="tbltxt">
                            <tr>
                                <td width="100px">
                                    Publisher Name <span style="color: Red; font-size: small;">*</span>:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPublisher" runat="server" CssClass="tbltxt">
                                        <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    
                                 
                                 <td>
                                   <asp:Button ID="BtnAddPublisher" runat="server" Text="Add Publisher" 
                                        Font-Bold="True" Font-Size="8pt"
                                         Width="100px" onclick="BtnAddPublisher_Click"  />
                                  </td>
                                  </tr>


                            <tr valign="top">
                                <td>
                                    Dimension :
                                </td>
                                <td>
                                    <asp:TextBox ID="Txtdimension" runat="server" MaxLength="50" CssClass="tbltxtbox"></asp:TextBox>
                                </td>
                            </tr>

                             <tr valign="top">
                                <td>
                                    Book No :
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtBookno" runat="server" MaxLength="50" CssClass="tbltxtbox" ></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td valign="top">
                                    Remarks :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="45px" CssClass="tbltxtbox"></asp:TextBox>
                                </td>
                            </tr>


                        </table>
                    </fieldset>
                </div>
                <div style="clear: both;">
                </div>
            </div>
            <div style="height: 10px;">
            </div>
            <div style="width: 800px; margin: 0 auto; text-align: center;">
                <asp:Button ID="btnSaveAddNew" runat="server" Text="Save & AddNew" Font-Bold="True"
                    OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveAddNew_Click" />&nbsp;
                <asp:Button ID="btnSaveGotoList" runat="server" Text="Save & GotoList" Font-Bold="True"
                    OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveGotoList_Click" Visible="false" />&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Font-Bold="True" Font-Size="8pt"
                    Width="60px" OnClick="btnCancel_Click" />&nbsp;
                <asp:Button ID="btnShow" runat="server" Text="Back" Font-Bold="True" Font-Size="8pt"
                    Width="70px" OnClick="btnShow_Click" />
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

