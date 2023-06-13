<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="rptEmpTransactions.aspx.cs" Inherits="HR_rptEmpTransactions" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=850,height=650,left = 250,top=10');");
        }

        function valSearch() {
            var Employee = document.getElementById("<%=drpEmpName.ClientID %>").selectedIndex;
            var month = document.getElementById("<%=drpMonth.ClientID %>").selectedIndex;
            var year = document.getElementById("<%=drpYear.ClientID %>").selectedIndex;
            if (Employee == 0) {
                alert("Select an Employee");
                document.getElementById("<%=drpEmpName.ClientID %>").focus();
                return false;
            }
            if (month == 0) {
                alert("Select a Month");
                document.getElementById("<%=drpMonth.ClientID %>").focus();
                return false;
            }
            if (year == 0) {
                alert("Select a Year");
                document.getElementById("<%=drpYear.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Employee Transactions History</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset>
                <table width="100%">
                    <tr>
                        <td align="left" valign="baseline">
                            Employee Name<font color="red">*</font>&nbsp;:&nbsp;<asp:DropDownList ID="drpEmpName"
                                runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="drpEmpName_SelectedIndexChanged"
                                TabIndex="1">
                            </asp:DropDownList>
                            &nbsp;<strong>Month&nbsp;:&nbsp;</strong>
                            <asp:DropDownList ID="drpMonth" runat="server" TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="drpMonth_SelectedIndexChanged">
                                <asp:ListItem Value="0">- SELECT -</asp:ListItem>
                                <asp:ListItem Value="JAN">January</asp:ListItem>
                                <asp:ListItem Value="FEB">February</asp:ListItem>
                                <asp:ListItem Value="MAR">March</asp:ListItem>
                                <asp:ListItem Value="APR">April</asp:ListItem>
                                <asp:ListItem Value="MAY">May</asp:ListItem>
                                <asp:ListItem Value="JUN">June</asp:ListItem>
                                <asp:ListItem Value="JUL">July</asp:ListItem>
                                <asp:ListItem Value="AUG">August</asp:ListItem>
                                <asp:ListItem Value="SEP">September</asp:ListItem>
                                <asp:ListItem Value="OCT">October</asp:ListItem>
                                <asp:ListItem Value="NOV">November</asp:ListItem>
                                <asp:ListItem Value="DEC">December</asp:ListItem>
                            </asp:DropDownList>
                            <strong>&nbsp;Year : </strong>
                            <asp:DropDownList ID="drpYear" runat="server" TabIndex="3" AutoPostBack="True" OnSelectedIndexChanged="drpYear_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp;<asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                Width="70px" onfocus="active(this);" onblur="inactive(this);" OnClientClick="return valSearch();"
                                TabIndex="4" />&nbsp;<asp:Button ID="btnPrint" Text="Print" runat="server" Visible="false"
                                    OnClientClick="javascript:popUp('rptEmpTransactionsPrint.aspx')" Width="70px"
                                    onfocus="active(this);" onblur="inactive(this);" TabIndex="5" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:Label ID="lblReport" runat="server"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

