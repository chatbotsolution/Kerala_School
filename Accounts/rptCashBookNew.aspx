<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptCashBookNew.aspx.cs" Inherits="Accounts_rptCashBookNew" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<script type="text/javascript" language="javascript">
    function PrintContent() {

        var DocumentContainer = document.getElementById('<%=viewcon.ClientID%>');


        var WindowObject = window.open('', "TrackData",
                              "width=420,height=225,top=250,left=345,toolbars=no,scrollbars=no,status=no,resizable=no");

        WindowObject.document.write(DocumentContainer.innerHTML);
        WindowObject.document.close();
        WindowObject.focus();
        WindowObject.print();
        WindowObject.close();
    }
    function validate() {

        var startdate = document.getElementById('<%=txtstartdate.ClientID%>').value;
        var endate = document.getElementById('<%=txtenddate.ClientID%>').value;
        if (startdate == "") {
            alert("Please enter Start date");
            return false;
        }
        if (endate == "") {
            alert("Please enter End date");
            return false;
        } var sdayfield = startdate.split("-")[0];
        var smonthfield = startdate.split("-")[1];
        var syearfield = startdate.split("-")[2];
        smonthfield = smonthfield - 1;


        var edayfield = endate.split("-")[0];
        var emonthfield = endate.split("-")[1];
        var eyearfield = endate.split("-")[2];
        emonthfield = emonthfield - 1;

        var startdatemain = new Date(syearfield, smonthfield, sdayfield);
        var enddatemain = new Date(eyearfield, emonthfield, edayfield);

        if (startdatemain > enddatemain) {
            alert("Start date can not be greater than end date");
            return false;
        }
    }
    </script>

    <div align="center">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div>
                            <h1>
                                Cash
                            </h1>
                            <h2>
                                Book
                            </h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 14px" colspan="2">
                        <asp:Label ID="lblStDt" runat="server" Text="Start Date:"></asp:Label>
                        <asp:TextBox ID="txtstartdate" runat="server" TabIndex="1" Width="80px"></asp:TextBox>
                        <rjs:popcalendar id="PopCalendar1" runat="server" control="txtstartdate"></rjs:popcalendar>
                        <asp:LinkButton ID="lnkbtnStartDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtstartdate.value='';return false;"
                            Text="Clear"></asp:LinkButton>
                        &nbsp;<asp:Label ID="lblEndDt" runat="server" Text="End Date:"></asp:Label>
                        <asp:TextBox ID="txtenddate" runat="server" TabIndex="2" Width="80px"></asp:TextBox>&nbsp;
                        <rjs:popcalendar id="PopCalendar2" runat="server" control="txtenddate"></rjs:popcalendar>
                        <asp:LinkButton ID="lnkbtnEndDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtenddate.value='';return false;"
                            Text="Clear"></asp:LinkButton>&nbsp;&nbsp;
                        <asp:Button ID="btnView" runat="server" Text="Show Detailed Report" 
                            OnClientClick="return validate();" TabIndex="3" onclick="btnView_Click" />
                        <asp:Button ID="btnShowConsol" runat="server" Text="Show Consolidated Report" 
                            TabIndex="3" onclick="btnShowConsol_Click"/>
                        <asp:Button ID="btnSplit" runat="server" Text="Show Cash Book For Audit" 
                                    TabIndex="4" onclick="btnSplit_Click"/>
                        <asp:Button ID="btnprnt" runat="server" Text="Print" Width="80px" OnClientClick='javascript:PrintContent()'
                            TabIndex="5" CausesValidation="False" Visible="false"/>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <div id="viewcon" runat="server">
                            <asp:Literal runat="server" ID="Literal1"></asp:Literal>
                        </div>
                    </td>
                </tr>
            </table>
</asp:Content>

