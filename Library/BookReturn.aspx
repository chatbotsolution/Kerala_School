<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="BookReturn.aspx.cs" Inherits="Library_BookReturn" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function isValid() {

            var ReturnDt = document.getElementById("<%=txtReturnDt.ClientID %>").value;
            var ARec = document.getElementById("<%=txtAmntRcvd.ClientID %>").value;
            var Fine = document.getElementById("<%=hfFines.ClientID %>").value;
            var FineAmnt = document.getElementById("<%=hfFineAmnt.ClientID %>").value;


            if (ReturnDt == 0) {
                alert("Please Select Return Date !");
                document.getElementById("<%=txtReturnDt.ClientID %>").focus();
                return false;
            }
            if (ARec.trim() == "" && document.getElementById("<%=txtAmntRcvd.ClientID %>").disabled == false) {
                alert("Please Enter Received Amount !");
                document.getElementById("<%=txtAmntRcvd.ClientID %>").focus();
                return false;
            }

            if (Fine == 0) {
                alert("Please Calculate Fine !");
                return false;
            }
            if (Number(ARec) < Number(FineAmnt)) {
                alert("Received Amount can't be less than Fine Amount !");
                return false;
            }
            else {
                return confirm('You are going save the record. Do you want to continue ?');
            }
        }                   
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Return Books</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="40" width="10" /></div>
            <div style="width: 1000px; background-color: #666; padding: 1px; margin: 0 auto; display:inline-block; float: left;">
                <div style="background-color: #FFF; padding: 10px; width:498px; float: left; ">
                    <table cellpadding="2px" cellspacing="2px" align="center" width="100%" class="tbltxt" style=" margin-left:50px;">
                        <tr>
                            <td style="width: 100px;">
                               
                               &nbsp;
                            </td>
                            <td> IssueDate: &nbsp;<asp:Label ID="lblIssudt" runat="server" Text=""></asp:Label>&nbsp; DueDate: &nbsp;<asp:Label ID="lblDuedt" runat="server" Text=""></asp:Label></td>
                            
                        </tr>
                        <tr>
                            <td>
                                Return Date :&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtReturnDt" runat="server" Width="100px" MaxLength="30"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpReturnDt" runat="server" Control="txtReturnDt" 
                                    AutoPostBack="true" 
                                    Format="dd mmm yyyy" onselectionchanged="dtpReturnDt_Changed"></rjs:PopCalendar>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Member Name :&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblMemberName" runat="server" Text=""></asp:Label>
                                <asp:HiddenField ID="hdnMemberId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Accession No :&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblAccNo" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Book Title :
                            </td>
                            <td>
                                <asp:Label ID="lblBookTitle" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnCalFine" runat="server" Text="Calculate Fine" Font-Bold="True"
                                    Font-Size="8pt" OnClick="btnCalFine_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fine Amount :
                            </td>
                            <td>
                                <asp:Label ID="lblFineAmnt" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Amount Received :
                            </td>
                            <td>
                                <asp:TextBox ID="txtAmntRcvd" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Narration :
                            </td>
                            <td>
                                <asp:TextBox ID="txtNarration" runat="server" Width="200px" TextMode="MultiLine"
                                    Height="64px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HiddenField ID="hfFineAmnt" runat="server" />
                            </td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Return" Font-Bold="True" OnClientClick="return isValid(); "
                                    Font-Size="8pt" Width="60px" OnClick="btnSave_Click" />&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Font-Bold="True" Font-Size="8pt"
                                    Width="60px" OnClick="btnCancel_Click" />&nbsp;
                                <asp:Button ID="btnShow" runat="server" Text="Show List" Font-Bold="True" Font-Size="8pt"
                                    Width="70px" OnClick="btnShow_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HiddenField ID="hfFines" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblmsg" runat="server" Text="" ForeColor=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                 <div style="background-color:#FFF; padding: 10px; width:460px; float: right; overflow: scroll;height:323px; ">
                 <center><span class="tbltxt"><h2>HOLIDAY LIST</h2></span></center>
                 <asp:Label ID="lblCalendar" runat="server" Text="" ></asp:Label>
                 </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

