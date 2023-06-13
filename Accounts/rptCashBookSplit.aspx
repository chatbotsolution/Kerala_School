<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptCashBookSplit.aspx.cs" Inherits="Accounts_rptCashBookSplit" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<style type="text/css">
        @media print
        {
            table
            {
                page-break-inside: avoid;
            }
        }
        @page
        {
            margin:0cm;
        }
  </style>
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
        var arc = MSapp.getHtmlPrintDocumentSource(document);
        src.rightMargin = 0;
        src.leftMargin = 0;
        src.topMargin = 0;
        src.bottomMargin = 0;
        src.shrinkToFit = false;
        args.SetSource(src);
    }
    </script>

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
                        Date:
                        <asp:TextBox ID="txtstartdate" runat="server" TabIndex="1" Width="80px"></asp:TextBox>
                        <rjs:popcalendar id="dtpDt" runat="server" control="txtstartdate"></rjs:popcalendar>
                        <asp:LinkButton ID="lnkbtnStartDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtstartdate.value='';return false;"
                            Text="Clear"></asp:LinkButton>
                        &nbsp;
                        <asp:Button ID="btnView" runat="server" Text="Show Cash Book For Audit" 
                            OnClientClick="return validate();" TabIndex="3" onclick="btnView_Click" />
                        <asp:Button ID="btnShowConsol" runat="server" Text="Show Consolidated Report" 
                            TabIndex="3" onclick="btnShowConsol_Click" />
                        <asp:Button ID="btnShowDtls" runat="server" Text="Show Detailed Report" Visible="false"
                                    TabIndex="4" onclick="btnShowDtls_Click"/>
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

