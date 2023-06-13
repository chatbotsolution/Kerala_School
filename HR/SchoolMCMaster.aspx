<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="SchoolMCMaster.aspx.cs" Inherits="HR_SchoolMCMaster" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=800,height=500,left = 300,top = 100');");
        }

        function scrollToMsg() {
            var msg = document.getElementById("<%=litMsg.ClientID%>").innerHTML;
            if (msg.trim() != "") {
                msg.scrollIntoView();
                msg.focus();
            }
        }
        function CnfNew() {
            var msg = "You are going to Create a New Managing Committe. The Existing Committee will be automatically Closed. Do you want to continue ?";
            if (confirm(msg)) {

                return true;
            }
            else {

                return false;
            }
        }
        function CnfModify() {
            var msg = "You are going to Modify the Existing Managing Committe. Do you want to continue ?";
            if (confirm(msg)) {

                return true;
            }
            else {

                return false;
            }
        }
        function clearText(btn) {
            switch (btn) {
                case '1':
                    {
                        document.getElementById("<%=txtAprDate.ClientID %>").value = "";
                        return false;
                    }
                case '2':
                    {
                        document.getElementById("<%=txtPerFrom.ClientID %>").value = "";
                        return false;
                    }
                case '3':
                    {
                        document.getElementById("<%=txtPerTo.ClientID %>").value = "";
                        return false;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        function validateform() {
            var ApLtrNo = document.getElementById("<%=txtApLtrNo.ClientID %>");
            var AprDate = document.getElementById("<%=txtAprDate.ClientID %>");
            var PerFrom = document.getElementById("<%=txtPerFrom.ClientID %>");
            var PerTo = document.getElementById("<%=txtPerTo.ClientID %>");
            var chk = document.getElementById("<%=hfCheck.ClientID %>");


            if (ApLtrNo.disabled == false && ApLtrNo.value.trim() == "") {
                alert("Enter Approval Letter No");
                document.getElementById("<%=txtApLtrNo.ClientID %>").focus();
                return false;
            }
            if (AprDate.disabled == false && AprDate.value.trim() == "") {
                alert("Enter Approval Date");
                document.getElementById("<%=txtAprDate.ClientID %>").focus();
                return false;
            }
            if (PerFrom.disabled == false && PerFrom.value.trim() == "") {
                alert("Enter Effective Date");
                document.getElementById("<%=txtPerFrom.ClientID %>").focus();
                return false;
            }
            if (PerTo.disabled == false && PerTo.value.trim() == "") {
                alert("Approval End Date");
                document.getElementById("<%=txtPerTo.ClientID %>").focus();
                return false;
            }
            else {
                if (chk.value == "0") {
                    return CnfNew();
                }
                else if (chk.value == "1") {
                    return CnfModify();
                }
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
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Managing Commitee</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <div align="right" valign="top">
        <asp:Button ID="btnNew" runat="server" Text="New Managing Committee" OnClick="btnNew_Click"
            Visible="False" />
        <input type="button" value="Managing Committee List" onclick="javascript:popUp('SchoolMCList.aspx')" />
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <fieldset>
                    <table align="center" width="100%">
                        <tr>
                            <td align="left" valign="top">
                                Approval Letter No<span class="mandatory">*</span>
                            </td>
                            <td align="left" valign="top">
                                :
                            </td>
                            <td align="left" valign="top">
                                <asp:TextBox ID="txtApLtrNo" runat="server" TabIndex="1"></asp:TextBox>
                            </td>
                            <td align="left" valign="top" class="mandatory">
                            </td>
                            <td align="left" valign="top">
                                Approval Date<span class="mandatory">*</span>
                            </td>
                            <td align="left" valign="top">
                                :
                            </td>
                            <td align="left" valign="top">
                                <asp:TextBox ID="txtAprDate" runat="server" TabIndex="2"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpAprDate" runat="server" Control="txtAprDate" Format="dd mmm yyyy">
                                </rjs:PopCalendar>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                    OnClientClick="return clearText('1');" />
                            </td>
                            <td align="left" valign="top" class="mandatory">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                Effective From Date<span class="mandatory">*</span>
                            </td>
                            <td align="left" valign="top">
                                :
                            </td>
                            <td align="left" valign="top">
                                <asp:TextBox ID="txtPerFrom" runat="server" TabIndex="3"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpPerFrom" runat="server" Control="txtPerFrom" Format="dd mmm yyyy">
                                </rjs:PopCalendar>
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                    OnClientClick="return clearText('2');" />
                            </td>
                            <td align="left" valign="top" class="mandatory">
                            </td>
                            <td align="left" valign="top">
                                Approval End Date<span class="mandatory">*</span>
                            </td>
                            <td align="left" valign="top">
                                :
                            </td>
                            <td align="left" valign="top">
                                <asp:TextBox ID="txtPerTo" runat="server" TabIndex="3"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpPerTo" runat="server" Control="txtPerTo" Format="dd mmm yyyy">
                                </rjs:PopCalendar>
                                <asp:ImageButton ID="ImageButton3" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/refresh_icon.png"
                                    OnClientClick="return clearText('3');" />
                            </td>
                            <td align="left" valign="top" class="mandatory">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="8">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="8">
                                <asp:GridView ID="gvMC" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="ReconstId"
                                    OnRowDataBound="gvMC_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfDesgId" runat="server" Value='<%#Eval("DesignationId") %>' />
                                                <asp:HiddenField ID="hfSortOrder" runat="server" Value='<%#Eval("SortOrder") %>' />
                                                <asp:HiddenField ID="hfReconstId" runat="server" Value='<%#Eval("ReconstId") %>' />
                                                <asp:Label Text='<%#Eval("Designation") %>' runat="server" ID="lblDesignation"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Member Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEmployee" runat="server" Text='<%#Eval("Name") %>' Width="200px"
                                                    TabIndex="5"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Contact No">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtContactNo" Text='<%#Eval("ContactTel") %>' Width="100px" runat="server"
                                                    onkeypress="return blockNonNumbers(this, event, false, false);" TabIndex="5"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email Id">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEmailID" Text='<%#Eval("EmailId") %>' runat="server" Width="150px"
                                                    TabIndex="5"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAddress" Text='<%#Eval("Address") %>' runat="server" MaxLength="100"
                                                    Width="250px" TabIndex="5"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Add/Remove">
                                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                                            <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                            <ItemTemplate>
                                                <asp:ImageButton ImageUrl="~/images/plus.jpg" runat="server" ID="btnAdd" OnClick="btnAdd_Click"
                                                    ToolTip="Click to Add New" />
                                                <asp:ImageButton ImageUrl="~/images/minus.png" runat="server" ID="btnRemove" ToolTip="Click to Remove"
                                                    Height="16px" Width="16px" OnClick="btnRemove_Click" CommandArgument='<%# Container.DataItemIndex %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Replace">
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                            <ItemTemplate>
                                                <a href="SchoolMCMasterEdit.aspx?ReconstId=<%#Eval("ReconstId") %>">Replace</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Management Designation is defined. First define the Required Designations for
                                        Management Committee.
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" colspan="8">
                                <asp:Button ID="btnCreate" runat="server" Text="Save" OnClientClick="return validateform();"
                                    OnClick="btnCreate_Click" TabIndex="6" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" TabIndex="7" OnClick="btnCancel_Click" />
                                <asp:HiddenField ID="hfCheck" runat="server" Value="0" />
                                <asp:HiddenField ID="hfApprLtrNo" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div style="margin: 5px; padding: 10px; background-color: #E9FFDA; border: solid 1px #C0EDDA;
                    font-weight: bold; text-align: left; color: #00514D; box-shadow: 3px 3px 5px #888888;">
                    <div id="msg">
                        <asp:Label Text="" runat="server" ID="litMsg"></asp:Label>
                    </div>
                    At first, make sure that you have the required structure of the Managing Committee.<br />
                    For Example: If you need to add another member then first add an empty member row
                    by clicking
                    <img src="../images/plus.jpg" alt="Add New" />
                    button on the right side of the corresponding row and then start entering data.
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="divNoDesg" runat="server" style="margin: 5px; padding: 10px; background-color: #E9FFDA;
            border: solid 1px #C0EDDA; font-weight: bold; text-align: left; color: #00514D;
            box-shadow: 3px 3px 5px #888888;" visible="false">
            No Management Designation is defined. First define the Required Designations for
            Management Committee.
            <asp:Button ID="btnDesg" runat="server" Text="Click Here to Define Designation" TabIndex="8"
                OnClick="btnDesg_Click" />
        </div>
</asp:Content>

