<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="MemberEntry.aspx.cs" Inherits="Library_MemberEntry" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {

            var RegdDt = document.getElementById("<%=txtRegdDt.ClientID %>").value;
            //            var ExpDt = document.getElementById("<%=txtExpDt.ClientID %>").value;
            var MemId = document.getElementById("<%=txtMemId.ClientID %>").value;
            var MemberName = document.getElementById("<%=txtName.ClientID %>").value;
            //            var Phno = document.getElementById("<%=txtPhno.ClientID %>").value;
            var AllowDay = document.getElementById("<%=txtAllowDay.ClientID %>").value;
            var AllwBook = document.getElementById("<%=txtAllowBook.ClientID %>").value;

            if (RegdDt.trim() == "") {
                alert("Please Enter Registration Date !");
                document.getElementById("<%=txtRegdDt.ClientID %>").focus();
                document.getElementById("<%=txtRegdDt.ClientID %>").select();
                return false;
            }
            //            if (ExpDt.trim() == "") {
            //                alert("Please Enter Registration Expire Date");
            //                document.getElementById("<%=txtExpDt.ClientID %>").focus();
            //                document.getElementById("<%=txtExpDt.ClientID %>").select();
            //                return false;
            //            }            
            //            if (Date.parse(RegdDt.trim()) >= Date.parse(ExpDt.trim())) {
            //                alert("Expire Date must greater then Registration Date");
            //                document.getElementById("<%=txtExpDt.ClientID %>").focus();
            //                return false;
            //            }
            if (MemId.trim() == "") {
                alert("Please Enter Member Id !");
                document.getElementById("<%=txtMemId.ClientID %>").focus();
                document.getElementById("<%=txtMemId.ClientID %>").select();
                return false;
            }
            if (MemberName.trim() == "") {
                alert("Please Enter Member Name !");
                document.getElementById("<%=txtName.ClientID %>").focus();
                document.getElementById("<%=txtName.ClientID %>").select();
                return false;
            }

            //            if (Phno.trim() == "") {
            //                alert("Please Enter Phone Number !");
            //                document.getElementById("<%=txtPhno.ClientID %>").focus();
            //                document.getElementById("<%=txtPhno.ClientID %>").select();
            //                return false;
            //            }
            if (AllowDay.trim() == "") {
                alert("Please Enter Number Allowed Days !");
                document.getElementById("<%=txtAllowDay.ClientID %>").focus();
                document.getElementById("<%=txtAllowDay.ClientID %>").select();
                return false;
            }
            if (AllwBook.trim() == "") {
                alert("Please Enter Number Of Books Allowed !");
                document.getElementById("<%=txtAllowBook.ClientID %>").focus();
                document.getElementById("<%=txtAllowBook.ClientID %>").select();
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

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Add/Modify Member</h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="20" width="10" /></div>
        
    <table class="tbltxt">
        <tr>
            <td style="padding-left: 120px;">
                <div style="float: left;">
                    <fieldset style="width: 400px;">
                        <legend>Member Details</legend>
                        <table width="100%" cellpadding="0px" cellspacing="0px" class="tbltxt">
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Member Type :
                                </td>
                                <td>
                                    <asp:Label ID="lblMemberType" runat="server" Text="Employee"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <img src="../images/mask.gif" height="10" width="10" /></div>
                                </td>
                            </tr>                            
                            <tr>
                                <td>
                                    Regd Date :&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRegdDt" runat="server" MaxLength="100" CssClass="largetb" ReadOnly="true"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpRegdDt" runat="server" Control="txtRegdDt" AutoPostBack="True"
                                        Format="dd mmm yyyy" onselectionchanged="dtpRegdDt_SelectionChanged"></rjs:PopCalendar>
                                    <asp:LinkButton ID="lnkcldate" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtRegdDt.value='';return false;"
                                        Text="Clear" ></asp:LinkButton>
                                    <span style="color: Red; font-size: small;">*</span>
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <img src="../images/mask.gif" height="10" width="10" /></div>
                                </td>
                            </tr>
                            
            
                            <tr>
                                <td>
                                    Expiry Date :&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExpDt" runat="server" MaxLength="100" CssClass="largetb" ReadOnly="true"></asp:TextBox>
                                    <rjs:PopCalendar ID="dtpExpireDt" runat="server" Control="txtExpDt" AutoPostBack="False"
                                        Format="dd mmm yyyy"></rjs:PopCalendar>
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtExpDt.value='';return false;"
                                        Text="Clear" ></asp:LinkButton>
                                    <%--<span style="color: Red; font-size: small;">*</span>--%>
                                </td>
                            </tr>                            
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <img src="../images/mask.gif" height="10" width="10" /></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    ID Number :&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMemId" runat="server" MaxLength="20" CssClass="largetb"></asp:TextBox>
                                    <span style="color: Red; font-size: small;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <img src="../images/mask.gif" height="10" width="10" /></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Member Name :&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" MaxLength="50" CssClass="largetb"></asp:TextBox>
                                    <span style="color: Red; font-size: small;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <img src="../images/mask.gif" height="10" width="10" /></div>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Address :&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Width="250px" Height="59px"
                                        CssClass="largeta"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <img src="../images/mask.gif" height="10" width="10" /></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Email Id :&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailId" runat="server" CssClass="largetb"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <img src="../images/mask.gif" height="10" width="10" /></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Phone No :&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPhno" runat="server" CssClass="largetb"></asp:TextBox>
                                    <%--<span style="color: Red; font-size: small;">*</span>--%>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 120px;">
                <div style="float: left;">
                    <fieldset style="width: 400px;">
                        <legend>Other Details</legend>
                        <table class="tbltxt">
                            <tr>
                                <td>
                                    Allowed Days :&nbsp;&nbsp;
                                    <asp:TextBox ID="txtAllowDay" runat="server" Width="50px" MaxLength="4" CssClass="smalltb"
                                        onkeypress="return blockNonNumbers(this, event,false, false);"></asp:TextBox>
                                    <span style="color: Red; font-size: small;">*</span>
                                </td>
                                <td>
                                    Allowed Books :&nbsp;&nbsp;
                                    <asp:TextBox ID="txtAllowBook" runat="server" Width="50px" MaxLength="3" CssClass="smalltb"
                                        onkeypress="return blockNonNumbers(this, event,false, false);"></asp:TextBox>
                                    <span style="color: Red; font-size: small;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Is Fine Applicable :                                    
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rdbtnFineStatus" runat="server" RepeatDirection="Horizontal"
                                        CssClass="tbltxt">
                                        <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Member Fee :&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFee" runat="server" Width="100px" MaxLength="10" CssClass="smalltb"
                                        onkeypress="return blockNonNumbers(this, event,true, false);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 120px;">
                <fieldset style="width: 400px;">
                    <asp:Button ID="btnSaveAddNew" runat="server" Text="Save & AddNew" Font-Bold="True"
                        OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveAddNew_Click" />&nbsp;
                    <asp:Button ID="btnSaveGotoList" runat="server" Text="Save & GotoList" Font-Bold="True"
                        OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveGotoList_Click" />&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Clear" Font-Bold="True" Font-Size="8pt"
                        Width="60px" OnClick="btnCancel_Click" />&nbsp;
                    <asp:Button ID="btnShow" runat="server" Text="Back" Font-Bold="True" Font-Size="8pt"
                        Width="70px" OnClick="btnShow_Click" />
                </fieldset>
            </td>
        </tr>
    </table>
            
</asp:Content>

