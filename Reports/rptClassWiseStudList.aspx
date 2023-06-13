<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptClassWiseStudList.aspx.cs" Inherits="Reports_rptClassWiseStudList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script language="javascript" type="text/javascript">
  function printcontent() {

            var DocumentContainer = document.getElementById('divreport');

            var documentheader = document.getElementById('divhdr');
            var WindowObject = window.open('', "TrackData",
                             "width=800,height=600,top=20,left=20,toolbars=no,scrollbars=no,status=no,resizable=yes");
            WindowObject.document.write(documentheader.innerHTML + "\n" + DocumentContainer.innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            WindowObject.close();
            return false;
        }
        </script>

 <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
           Class Wise Student List
        </h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="8" width="10" /></div>
         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <table width="100%"  border="0" cellspacing="2" cellpadding="2" class="cnt-box">
                <tr>
                    <td class="tbltxt" style="width: 55px">
                        Session
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td style="width: 150px">
                        <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                            OnSelectedIndexChanged="drpSession_SelectedIndexChanged" TabIndex="1">
                        </asp:DropDownList>
                    </td>
                    <td class="tbltxt" style="width: 50px">
                        Class
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td width="120">
                        <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpclass_SelectedIndexChanged"
                            CssClass="vsmalltb" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                    <td class="tbltxt" style="width: 70px">
                        Section
                    </td>
                    <td width="5" class="tbltxt">
                        :
                    </td>
                    <td style="width: 100px">
                        <asp:DropDownList ID="ddlSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSection_SelectedIndexChanged"
                            CssClass="vsmalltb" TabIndex="3">
                        </asp:DropDownList>
                    </td>
                    <td>
                     <asp:DropDownList ID="drpstudents" runat="server" CssClass="vsmalltb" Visible="false"
                            TabIndex="6">
                        </asp:DropDownList>
                    </td>
                </tr>
                

                
                
                <tr>
                    
                    <td colspan="12" align="left" class="tbltxt">
                        <asp:Button ID="btngo" runat="server"  Text="Search" ToolTip="Click to Search Student Details"
                            TabIndex="8" OnClick="btngo_Click"/>&nbsp;
                        <asp:Button ID="btnExpExcel" runat="server"  Text="Export to Excel"
                            TabIndex="9"  OnClick="btnExpExcel_Click"/>&nbsp; <span id="trbtn" runat="server" visible="false">
                                <asp:Button ID="btnprint" runat="server" Visible="false" Text="Print" TabIndex="10"
                                     />
                                    <asp:Button ID="btnprint1" runat="server" Visible="false" Text="Print" TabIndex="10"
                                     OnClientClick="printcontent();" />
                            </span>
                    </td>
                </tr>
            </table>

              <div runat="server" visible="false" id="Tr1">
                <asp:Label ID="lblRecCount" runat="server" Text="Label"></asp:Label>
            </div>
          
<%--
            <div id="trgrd" runat="server" style="height: 450px; width: 100%; overflow: scroll;">
               
                <table style="width: 100%; height: 450px;overflow: scroll; table-layout: fixed;" class="tbltxt">
                    <tr>
                        <td style="width:auto">
                            <asp:Label ID="lblreport" runat="server"> </asp:Label></td>
                    </tr>
                </table>
            </div>--%>


            <table width="100%" class="tbltxt">
        <tr>
            <td valign="top">
                <div id="divreport" style=" overflow:scroll; height:500px; ">
                    <table cellspacing="0" cellpadding="0" width="100%">
                        <tr id="tr2" runat="server" visible="false">
                            <td valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="lbl2">
                                <asp:Label ID="lblreport" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div style="display: none" id="divhdr">
                        <table width="100%">
                            <tr>
                               <%-- <td align="left">
                                    <asp:Label ID="Label4" runat="server" Text="Fee Received" Font-Bold="True" Font-Underline="True"></asp:Label>
                                </td>--%>
                                <td align="right">
                                    <asp:Label ID="lblPrintDate" Font-Bold="true" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>



          </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExpExcel" />
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

