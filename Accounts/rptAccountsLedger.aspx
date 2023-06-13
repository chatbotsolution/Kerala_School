<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptAccountsLedger.aspx.cs" Inherits="Accounts_rptAccountsLedger" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../css/sadashiv-admin.css" rel="stylesheet" type="text/css" media="print" />

    <script type="text/javascript" language="javascript">

        function PrintContent() {
            var DocumentContainer = document.getElementById('<%=viewcon.ClientID%>');
            var WindowObject = window.open('', "TrackData", "width=420,height=225,top=250,left=345,toolbars=no,scrollbars=no,status=no,resizable=no");

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
            }
            if (document.getElementById('<%=drpAccHead.ClientID%>').value == 0) {
                alert("Please select Account Head");
                document.getElementById('<%=drpAccHead.ClientID%>').focus();
                return false;
            }
            var sdayfield = startdate.split("-")[0];
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

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

      
        
    </script>

<%--    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
        <asp:PostBackTrigger ControlID="btnView"  />
          
        </Triggers>
        <ContentTemplate>--%>
            <div align="center">
                <asp:Panel ID="pnlcat" runat="server" Width="100%">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr style="background-color: #ededed;">
                            <td width="350" align="left" valign="middle" style="background-color: ">
                                <div>
                                    <h1>
                                        ACCOUNT LEDGER
                                    </h1>
                                    <h2>
                                        REPORT
                                    </h2>
                                </div>
                            </td>
                            <td height="35" align="left" valign="middle">
                                <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                               Account Group:  <asp:DropDownList ID="drpAcGroup" runat="server" TabIndex="1" 
                                    onselectedindexchanged="drpAcGroup_SelectedIndexChanged" 
                                    AutoPostBack="True">
                                </asp:DropDownList>
                               &nbsp;&nbsp; Account Head:
                                <asp:DropDownList ID="drpAccHead" runat="server" TabIndex="2" 
                                    onselectedindexchanged="drpAccHead_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                    &nbsp;&nbsp; 
                                 <asp:Label ID="lblFee" runat="server" Text="Fee Heads" Visible="false"></asp:Label>
                                <asp:DropDownList ID="drpFeeHead" runat="server" TabIndex="3" Visible="false">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="height: 14px;" colspan="2">
                                <asp:Label ID="lblStDt" runat="server" Text="Start Date:"></asp:Label>
                                <asp:TextBox ID="txtstartdate" runat="server" TabIndex="2" Width="160px"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpStart" runat="server" Control="txtstartdate"></rjs:PopCalendar>
                                <asp:LinkButton ID="lnkbtnStartDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtstartdate.value='';return false;"
                Text="Clear" ></asp:LinkButton>
                                <asp:Label ID="lblEndDt" runat="server" Text="End Date:"></asp:Label>
                                &nbsp;<asp:TextBox ID="txtenddate" runat="server" TabIndex="3" Width="160px"></asp:TextBox>&nbsp;
                                <rjs:PopCalendar ID="dtpEnd" runat="server" Control="txtenddate"></rjs:PopCalendar>
                                <asp:LinkButton ID="lnkbtnEndDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtenddate.value='';return false;"
                Text="Clear" ></asp:LinkButton>
                                <asp:Button ID="btnView" runat="server" Text="Show Report" OnClick="btnView_Click"
                                    OnClientClick="return validate();" TabIndex="4" />
                                &nbsp; &nbsp;
                                <asp:Button ID="btnprnt" runat="server" Text="Print" Width="80px" OnClientClick='javascript:PrintContent()'
                                    Visible="False" TabIndex="5" CausesValidation="False" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <div id="viewcon" runat="server">
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
          <%--  <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../Images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

