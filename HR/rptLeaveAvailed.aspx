<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="rptLeaveAvailed.aspx.cs" Inherits="HR_rptLeaveAvailed" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=850,height=650,left = 250,top=10');");
        }

        //        function valSearch() {
        //            var Employee = document.getElementById("<%=drpEmpName.ClientID %>").selectedIndex;
        //            if (Employee == 0) {
        //                alert("Select an Employee");
        //                document.getElementById("<%=drpEmpName.ClientID %>").focus();
        //                return false;
        //            }
        //            else {
        //                return true;
        //            }
        //        }
    </script>

    <script language="javascript" type="text/javascript">

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
        
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Employee Leave Availed Report</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <fieldset>
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="baseline">
                            Employee Name<font color="red">*</font>&nbsp;:&nbsp;<asp:DropDownList ID="drpEmpName"
                                runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="drpEmpName_SelectedIndexChanged"
                                TabIndex="1">
                            </asp:DropDownList>
                            &nbsp;Leave Type&nbsp;:&nbsp;<asp:DropDownList ID="drpLeave" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="drpLeave_SelectedIndexChanged" TabIndex="2">
                            </asp:DropDownList>
                            &nbsp;Accounting Year :<strong>
                            <asp:DropDownList ID="drpAccYear" runat="server" AutoPostBack="True" 
                                OnSelectedIndexChanged="drpAccYear_SelectedIndexChanged" TabIndex="2">
                            </asp:DropDownList>
                            &nbsp;</strong>&nbsp;<asp:TextBox ID="txtFromDt" runat="server" TabIndex="4" 
                                Width="80px" ReadOnly="True" Visible="False"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpFromDt" runat="server" Control="txtFromDt" ShowMessageBox="True"
                                TextMessage="Enter From Date" Format="dd mmm yyyy" Visible="False"></rjs:PopCalendar>
                            &nbsp;&nbsp;<asp:TextBox ID="txtToDt" runat="server" TabIndex="4"
                                Width="80px" ReadOnly="True" Visible="False"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" ShowMessageBox="True"
                                TextMessage="Enter From Date" Format="dd mmm yyyy" Visible="False"></rjs:PopCalendar>
                            &nbsp;<asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                Width="70px" onfocus="active(this);" onblur="inactive(this);" OnClientClick="return valSearch();"
                                TabIndex="4" />&nbsp;<asp:Button ID="btnPrint" Text="Print" runat="server" Visible="false"
                                    OnClientClick="javascript:popUp('rptLeaveAvailedPrint.aspx')" Width="70px" onfocus="active(this);"
                                    onblur="inactive(this);" TabIndex="5" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:Label ID="lblReport" runat="server"></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading...</span>
                </div>
            </asp:Panel>
      <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>


