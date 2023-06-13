<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true"
    CodeFile="BookPurchase.aspx.cs" Inherits="Library_BookPurchase" %>


<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="javascript">

        function isValid() {
            var Category = document.getElementById("<%=ddlBookCat.ClientID %>").selectedIndex;
            var Subject = document.getElementById("<%=ddlSubject.ClientID %>").selectedIndex;
            var BookName = document.getElementById("<%=ddlBookTitle.ClientID %>").selectedIndex;

            var date = document.getElementById("<%=txtPurchaseDt.ClientID %>").value;
            var price = document.getElementById("<%=txtPrice.ClientID %>").value;
            var PubYr = document.getElementById("<%=txtPubYr.ClientID %>").value;
            var sourceofacqr = document.getElementById("<%=ddlSouceAcqr.ClientID %>").selectedIndex;
            var receivedfrom = document.getElementById("<%=txtPurchaseFrm.ClientID %>").value;

            //get current date
            var curTime = new Date();

            if (Category == "0") {
                alert("Please Select a Category !");
                document.getElementById("<%=ddlBookCat.ClientID %>").focus();
                return false;
            }
            if (Subject == "0") {
                alert("Please Select a Subject !");
                document.getElementById("<%=ddlSubject.ClientID %>").focus();
                return false;
            }
            if (BookName == "0") {
                alert("Please Select Book Name !");
                document.getElementById("<%=ddlBookTitle.ClientID %>").focus();
                return false;
            }

            if (date.trim() == "") {
                alert("Please Enter Purchase Date !");
                document.getElementById("<%=txtPurchaseDt.ClientID %>").focus();
                document.getElementById("<%=txtPurchaseDt.ClientID %>").select();
                return false;
            }

            if (price == "") {
                alert("Please Fill Price !");
                document.getElementById("<%=txtPrice.ClientID %>").focus();
                return false;
            }

            //            if (PubYr.trim() == "") {
            //                alert("Please Provide Year of Publication !")
            //                document.getElementById("<%=txtPubYr.ClientID %>").focus();
            //                document.getElementById("<%=txtPubYr.ClientID %>").select();
            //                return false;
            //            }
            if (PubYr.trim() != "" && PubYr.trim().length < 4 || PubYr.trim() > curTime.getFullYear()) {
                alert("Please provide corect publish year !");
                document.getElementById("<%=txtPubYr.ClientID %>").focus();
                document.getElementById("<%=txtPubYr.ClientID %>").select();
                return false;
            }

            if (sourceofacqr == "0") {
                alert("Please Select Source Of Aquiring !");
                document.getElementById("<%=ddlSouceAcqr.ClientID %>").focus();
                return false;
            }

            if (receivedfrom.trim() == "") {
                alert("Please Fill Received from !");
                document.getElementById("<%=txtPurchaseFrm.ClientID %>").focus();
                document.getElementById("<%=txtPurchaseFrm.ClientID %>").select();
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

        function extractNumber(obj, decimalPlaces, allowNegative) {
            var temp = obj.value;

            // avoid changing things if already formatted correctly
            var reg0Str = '[0-9]*';
            if (decimalPlaces > 0) {
                reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
            } else if (decimalPlaces < 0) {
                reg0Str += '\\.?[0-9]*';
            }
            reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
            reg0Str = reg0Str + '$';
            var reg0 = new RegExp(reg0Str);
            if (reg0.test(temp)) return true;

            // first replace all non numbers
            var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
            var reg1 = new RegExp(reg1Str, 'g');
            temp = temp.replace(reg1, '');

            if (allowNegative) {
                // replace extra negative
                var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
                var reg2 = /-/g;
                temp = temp.replace(reg2, '');
                if (hasNegative) temp = '-' + temp;
            }

            if (decimalPlaces != 0) {
                var reg3 = /\./g;
                var reg3Array = reg3.exec(temp);
                if (reg3Array != null) {
                    // keep only first occurrence of .
                    //  and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
                    var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
                    reg3Right = reg3Right.replace(reg3, '');
                    reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;
                    temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;
                }
            }
            obj.value = temp;
        }
        function updateCount() {
            var len = document.getElementById("<%=txtISBN.ClientID %>").value.length;
            document.getElementById("isbncount").innerHTML = len;
        }
    </script>
    <asp:UpdatePanel ID="upp1" runat="server">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Add/Modify Purchase</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="790" border="0" cellspacing="0" cellpadding="0" align="center">
                <tr>
                    <td>
                        <asp:Panel ID="pnl1" runat="server" Visible="false">
                            <div style="width: 100%; border: 1px solid #333; padding: 1px; margin: 0 auto;" class="tbltxt">
                                <div style="background-color: #FCD1E6; padding: 5px; overflow: auto;">
                                    <div style="float: left; width: 380px;">
                                        <strong>Publisher :
                                            <asp:Label ID="lblPublisher" runat="server" Text=""></asp:Label></strong>
                                    </div>
                                    <div style="float: left; width: 380px;">
                                        <strong>Author :
                                            <asp:Label ID="lblAuthor" runat="server" Text=""></asp:Label></strong>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="float: left; width: 390px;">
                            <fieldset style="height: 270px;">
                                <legend class="tbltxt"><strong>Purchase Details</strong></legend>
                                <table class="tbltxt">
                                    <tr id="row1" runat="server">
                                        <td width="100px">
                                            New AccnNo :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewAccNo" runat="server" CssClass="tbltxtbox" MaxLength="12"
                                                Enabled="false"></asp:TextBox>
                                            <span style="color: Red; font-size: small;">*</span>
                                            <asp:LinkButton Visible="false" ID="lbClear" CssClass="tbltxt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtNewAccNo.value='';return false;"
                                                Text="Clear"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100px">
                                            Book Category :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBookCat" runat="server" AutoPostBack="true" CssClass="tbltxtbox"
                                                OnSelectedIndexChanged="ddlBookCat_SelectedIndexChanged">
                                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: Red; font-size: small;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Subject :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSubject" runat="server" CssClass="tbltxtbox" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: Red; font-size: small;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            Book Title :
                                        </td>
                                        <td>
                                            <%--<asp:TextBox ID="txtTitle" runat="server" MaxLength="100" CssClass="tbltxtbox"></asp:TextBox>--%>
                                            <asp:DropDownList ID="ddlBookTitle" runat="server" CssClass="tbltxtbox" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlBookTitle_SelectedIndexChanged">
                                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: Red; font-size: small;">*</span>
                                            <asp:Button ID="btnNotInList" runat="server" Text="Book Not In List" OnClick="btnNotInList_Click"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            Book Sub-Title :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtsubtitle" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Accn Date :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPurchaseDt" runat="server" CssClass="tbltxtbox" Width="150px"
                                                ReadOnly="true"></asp:TextBox>
                                            <rjs:PopCalendar ID="dtpPurchaseDt" runat="server" Control="txtPurchaseDt" AutoPostBack="True"
                                                Format="dd mmm yyyy" OnSelectionChanged="dtpPurchaseDt_SelectionChanged"></rjs:PopCalendar>
                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtPurchaseDt.value='';return false;"
                                                Text="Clear"></asp:LinkButton>
                                            <span style="color: Red; font-size: small;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Quantity :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQty" runat="server" Text="1" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Price per piece :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPrice" runat="server" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                                            <span style="color: Red; font-size: small;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Bill No :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillNo" runat="server" CssClass="tbltxtbox" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Bill Date :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillDt" runat="server" CssClass="tbltxtbox" Width="150px" ReadOnly="true"></asp:TextBox>
                                            <rjs:PopCalendar ID="dtpBillDt" runat="server" Control="txtBillDt" AutoPostBack="False"
                                                Format="dd mmm yyyy"></rjs:PopCalendar>
                                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtBillDt.value='';return false;"
                                                Text="Clear"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <div style="float: left; width: 400px;">
                            <fieldset>
                                <legend class="tbltxt"><strong>Other Details</strong></legend>
                                <table width="100%" border="0" cellspacing="2" cellpadding="2" class="tbltxt">
                                    <tr>
                                        <td>
                                            Publication Place :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPubPlace" runat="server" MaxLength="50" CssClass="tbltxtbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Year of Publication :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPubYr" runat="server" MaxLength="4" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Pages :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPages" runat="server" MaxLength="4" CssClass="tbltxtbox" onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            ISBN No :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtISBN" runat="server" MaxLength="50" CssClass="tbltxtbox" onkeyup="return updateCount();"
                                                upkeydown="return updateCount();"></asp:TextBox>
                                            <span id="isbncount"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="120px">
                                            Edition :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEdition" runat="server" CssClass="tbltxtbox" MaxLength="4"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Volume :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVol" runat="server" CssClass="tbltxtbox" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Attached Media :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAttachedMedia" runat="server" CssClass="tbltxtbox">
                                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Cd" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Files" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Source Of Acquiring :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSouceAcqr" runat="server" CssClass="tbltxtbox">
                                                <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Purchased" Value="Purchased"></asp:ListItem>
                                                <asp:ListItem Text="Gifted" Value="Gifted"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: Red; font-size: small;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Received From :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPurchaseFrm" runat="server" CssClass="tbltxtbox" MaxLength="100"></asp:TextBox>
                                            <span style="color: Red; font-size: small;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Reference :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReference" runat="server" CssClass="tbltxtbox" MaxLength="100"></asp:TextBox>
                                           
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend class="tbltxt"><strong>Remarks</strong></legend>
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="100%" Height="50px"></asp:TextBox>
                        </fieldset>
                        <div>
                            <img src="../images/mask.gif" height="8" width="10" /></div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSaveAddNew" runat="server" Text="Save & AddNew" Font-Bold="True"
                            OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveAddNew_Click" />&nbsp;
                        <asp:Button ID="btnSaveGotoList" runat="server" Text="Save & GotoList" Font-Bold="True"
                            OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveGotoList_Click" />&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" Font-Bold="True" Font-Size="8pt"
                            Width="60px" OnClick="btnCancel_Click" />&nbsp;
                        <asp:Button ID="btnShow" runat="server" Text="Back" Font-Bold="True" Font-Size="8pt"
                            Width="70px" OnClick="btnShow_Click" />
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
