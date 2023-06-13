<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Fee.master" AutoEventWireup="true" CodeFile="rptNewFeedefaulter.aspx.cs" Inherits="Reports_rptNewFeedefaulter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript" language="javascript">
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
     function validatePage() {
      
//         var classid = document.getElementById("<%=drpClass.ClientID %>").value;
//         if (classid == "0") {
//             alert("Select Class !");
//             return false;
//         }
//         else
//             return true;
     }

    </script>
    <style type="text/css">
        #reporttable td{ border-color:#d45757}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_rep.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Fee Register
                </h2>
                <span style="float:right; color:Red;"><asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></span>
            </div>
            <fieldset id="fsCons" runat="server" class="cnt-box">
                 <table width="100%" border="0" cellspacing="2" cellpadding="2">
                    <tr>
                    <td class="tbltxt">
                            Session
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                                TabIndex="5">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt">
                            Class
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                                OnSelectedIndexChanged="drpClass_SelectedIndexChanged" TabIndex="5">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt">
                            Section
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpSection" runat="server" CssClass="vsmalltb" OnSelectedIndexChanged="drpSection_SelectedIndexChanged1"
                                TabIndex="6" AutoPostBack="True" Width="70px">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt">
                            Student
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td class="tbltxt">
                            <asp:DropDownList ID="drpstudents" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                                OnSelectedIndexChanged="drpstudents_SelectedIndexChanged" TabIndex="4">
                            </asp:DropDownList>
                          
                           
                        </td>
                        <td class="tbltxt"> &nbsp;<asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return validatePage();"
                                TabIndex="5" /></td>
                        <tr>
                         <td class="tbltxt" colspan="10" align="right">
                            Search By Admission No 
                            
                        </td >
                        <td>:</td>
                        <td class="tbltxt"><asp:TextBox ID="txtadminno" runat="server" CssClass="vsmalltb" TabIndex="7">  </asp:TextBox></td>
                        <td><asp:Button runat="server" ID="BtnSearch" Text="Search" 
                                onclick="BtnSearch_Click"  ></asp:Button ></td>
                        </tr>
                       
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" TabIndex="8" />
                            <asp:Button ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click" Text="Export to Excel"
                                TabIndex="9" />
                        </td>
                       
                    </tr>
                </table>
            </fieldset>
            <div class="spacer"></div><div class="spacer"></div>
            <div style="padding: 3px;   height: 450px; overflow: scroll;" class="cnt-box2 lbl2 tbltxt">
            <asp:Label  ID="lblReport" runat="server"></asp:Label>
                
                 <div class="spacer"></div>
                <asp:Label ID="lblSaleRet" runat="server" CssClass="tbltxt"></asp:Label>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExpExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

