<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true"
    CodeFile="NewBookEntry.aspx.cs" Inherits="Library_NewBookEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
        function isValidForPub() {
            var PublisherName = document.getElementById("<%=txtPublisherName.ClientID %>").value;

            if (PublisherName.trim() == "") {
                alert("Please Fill Publisher Name!");
                document.getElementById("<%=txtPublisherName.ClientID %>").focus();
                document.getElementById("<%=txtPublisherName.ClientID %>").select();
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
        function OnBlurfun() {
            debugger;
            var author1 = document.getElementById("<%=txtAuthor1.ClientID %>").value;
            var lastname1 = document.getElementById("<%=txtLName1.ClientID %>").value;
            var author2 = document.getElementById("<%=txtAuthor1.ClientID %>").value;
            var lastname2 = document.getElementById("<%=txtLName1.ClientID %>").value;
            var author3 = document.getElementById("<%=txtAuthor3.ClientID %>").value;
            var lastname3 = document.getElementById("<%=txtLName3.ClientID %>").value;
            if(author1=="" && lastname1=="" && author2=="" && lastname2=="")
            {
             bookNo(author3,lastname3);
            }
            else if(author1=="" && lastname1=="")
            {
             bookNo(author2,lastname2);
            }
            else
            {
             bookNo(author1,lastname1);
            }
           
        }

        function bookNo(Author,Lastname) {
            debugger;
            var bookno="";
            if (Lastname == "") {
                if (Author == "")
                    return;
                else if (Author.length <= 3) {
                    bookno = Author.toUpperCase();
                }
                else {
                    bookno = Author.substring(0, 3).toUpperCase();
                }
            }
            else if (Author.length <= 3) {
                bookno = Lastname.toUpperCase();
            }
            else {
                bookno = Lastname.substring(0, 3).toUpperCase();
            }
            document.getElementById("<%=TxtBookno.ClientID %>").value = bookno;
        }
                
    </script>
    <asp:UpdatePanel ID="upp1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Add/Modify Book</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <div style="width: 800px; margin: 0 auto;">
                <div style="float: left;">
                    <fieldset style="width: 350px;">
                        <legend class="tbltxt"><strong>Book Details</strong></legend>
                        <table class="tbltxt">
                            <tr>
                                <td width="100px">
                                    Book Category<span style="color: Red; font-size: small;">*</span> :
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlBookCat" runat="server" AutoPostBack="true" CssClass="tbltxt" Width="99%"
                                        OnSelectedIndexChanged="ddlBookCat_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Subject <span style="color: Red; font-size: small;">*</span>:
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlSubject" runat="server" CssClass="tbltxt" Width="99%">
                                        <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Book Title <span style="color: Red; font-size: small;">*</span>:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" CssClass="tbltxtbox" Width="95%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Book SubTitle <span style="color: Red; font-size: small;">*</span>:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtsubtitle" runat="server" MaxLength="100" CssClass="tbltxtbox" Width="95%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    Author1 :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAuthor1" runat="server" MaxLength="50" CssClass="tbltxtbox" placeholder="First Name"></asp:TextBox>
                                </td>

                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1"  TargetControlID="txtAuthor1" MinimumPrefixLength="1"  CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" runat="server" ContextKey="53"
  FirstRowSelected="false" ServiceMethod="AutoCompleteLib"  CompletionListCssClass="AutoExtender" CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"  ></ajaxToolkit:AutoCompleteExtender>
                          

                                <td>
                                    <asp:TextBox ID="txtLName1" runat="server" MaxLength="50" CssClass="tbltxtbox" placeholder="Last Name" onblur="OnBlurfun()"
                                        ></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    Author2 :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAuthor2" runat="server" MaxLength="50" CssClass="tbltxtbox" placeholder="First Name"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLName2" runat="server" MaxLength="50" CssClass="tbltxtbox" placeholder="Last Name"  onblur="OnBlurfun()"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                   <asp:DropDownList ID="ddlAuthortp" Width="85px" CssClass="tbltxtbox" runat="server"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlAuthortp_SelectedIndexChanged">
                                        <asp:ListItem Value="Author" Selected="True">Author</asp:ListItem>
                                        <asp:ListItem Value="Editor">Editor</asp:ListItem>
                                        <asp:ListItem Value="Illustrator">Illustrator</asp:ListItem>
                                        <asp:ListItem Value="Compiler">Compiler</asp:ListItem>
                                        <asp:ListItem Value="Publisher">Publisher</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAuthor3" runat="server" MaxLength="50" CssClass="tbltxtbox" placeholder="First Name"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLName3" runat="server" MaxLength="50" CssClass="tbltxtbox" placeholder="Last Name" onblur="OnBlurfun()"></asp:TextBox>
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
                                        <asp:Button ID="BtnAddPublisher" runat="server" Text="Add Publisher" Font-Bold="True"
                                            Font-Size="8pt" Width="100px"  />
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
                                    <asp:TextBox ID="TxtBookno" runat="server" MaxLength="50" CssClass="tbltxtbox"></asp:TextBox>
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
                    OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveGotoList_Click" />&nbsp;
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
            <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="BtnAddPublisher"
                CancelControlID="btnClose" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="display: none">
                <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                    <img src="../images/icon_cp.jpg" width="29" height="29"></div>
                <div style="padding-top: 5px;">
                    <h2>
                        Add/Modify Publisher</h2>
                </div>
                <div>
                    <img src="../images/mask.gif" height="40" width="10" /></div>
                <div style="width: 440px; background-color: #666; padding: 1px; margin: 0 auto;">
                    <div style="background-color: #FFF; padding: 10px;">
                        <table cellpadding="0px" cellspacing="0px" align="center" width="100%" class="tbltxt">
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Publisher Name:&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPublisherName" runat="server" Width="200px" MaxLength="50" CssClass="largetb"></asp:TextBox>
                                    <span style="color: Red; font-size: small;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Publisher Place:&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPublisherPlace" runat="server" Width="200px" MaxLength="50" CssClass="largetb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Remarks:&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="200px" MaxLength="100" CssClass="largeta"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="btnSaveClose" runat="server" Text="Save & Close" Font-Bold="True"
                                        OnClientClick="return isValidForPub();" Font-Size="8pt" Width="120px" OnClick="btnSaveClose_Click" />&nbsp;
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" Font-Bold="True" Font-Size="8pt"
                                        Width="70px" OnClick="btnClear_Click" />
                                    <asp:Button ID="btnClose" runat="server" Text="Close" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
