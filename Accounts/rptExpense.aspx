<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="rptExpense.aspx.cs" Inherits="Accounts_rptExpense" %>

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

        function beginRequest(sender, args) {
            // show the popup
            $find('<%=mdlloading.ClientID %>').show();
        }

        function endRequest(sender, args) {
            //  hide the popup
            $find('<%=mdlloading.ClientID %>').hide();
        }
        
    </script>

    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <div align="center">
                <asp:Panel ID="pnlcat" runat="server" Width="100%">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr style="background-color: #ededed;">
                            <td width="350" align="left" valign="middle" style="background-color: ">
                                <div>
                                    <h1>
                                        EXPENSE
                                    </h1>
                                    <h2>
                                        REPORT
                                    </h2>
                                </div>
                            </td>
                           <%-- <td height="35" align="left" valign="middle">
                                <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                            </td>--%>
                        </tr>
                        <tr>
                        <td height="30" align="left" valign="middle">
                                <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                               Account Head:
                                <asp:DropDownList ID="drpAccHead" runat="server" TabIndex="1" 
                                     AutoPostBack="true">
                                </asp:DropDownList>&nbsp;&nbsp;
                                <asp:Label ID="Label1" runat="server" Text="Start Date:"></asp:Label>&nbsp;&nbsp;
                                <asp:TextBox ID="txtstartdate" runat="server" TabIndex="2" Width="80px"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpStart" runat="server" Control="txtstartdate"></rjs:PopCalendar>
                                &nbsp;
                                <asp:Label ID="Label2" runat="server" Text="End Date:"></asp:Label>
                                &nbsp;<asp:TextBox ID="txtenddate" runat="server" TabIndex="3" Width="80px"></asp:TextBox>&nbsp;
                                <rjs:PopCalendar ID="dtpEnd" runat="server" Control="txtenddate"></rjs:PopCalendar>
                                &nbsp;
                                <asp:Button ID="btnView" runat="server" Text="Show Report" OnClick="btnView_Click"
                                    OnClientClick="return validate();" TabIndex="4"/>
                                &nbsp; &nbsp;
                                <asp:Button ID="Button2" runat="server" Text="Print" Width="80px" OnClientClick='javascript:PrintContent()'
                                    Visible="False" TabIndex="5" CausesValidation="False" />
                                    <asp:Button ID="btnPrint" runat="server" Text="Print" Width="80px" OnClientClick='javascript:PrintContent()'
           TabIndex="5"  />
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
