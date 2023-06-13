<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptClassStrength.aspx.cs" Inherits="Reports_rptClassStrength" %>

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
                    Class Strength
                </h2>
            </div>
              <table width="100%" border="0" cellspacing="2" cellpadding="2"class="cnt-box">
                    <tr>
                    <td>
                    <span class="tbltxt"> Admission Session : &nbsp
                    <asp:DropDownList ID="ddlSession" runat="Server" CssClass="vsmalltb"></asp:DropDownList></span>
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Clicked" />
                     <%--<asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Clicked" />--%>
                     <asp:Button ID="btnPrint" runat="server" Text="Print"  OnClientClick="printcontent();" />
                    </td>
                    </tr>
                    </table>

                    <div>                    
                    <%--<asp:Label ID="LblReport" runat="server"></asp:Label>--%>
                    </div>
                    
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
                                <asp:Label ID="LblReport" runat="server"></asp:Label>
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

                    </asp:Content>

