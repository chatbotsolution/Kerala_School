<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="rptSixthSubjectList.aspx.cs" Inherits="Reports_rptSixthSubjectList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="Javascript" type="text/javascript">

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


    function chkFirstNo() {

        var firstno = document.getElementById("<%=txtFirstNo.ClientID %>").value;
        if (firstno.trim() == "") {
            alert("FirstNo can't be Empty");
            document.getElementById("<%=txtFirstNo.ClientID %>").focus();

        }
    }


    function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
        var key;
        var isCtrl = false;
        var keychar;
        var reg;

        if (window.event) {
            key = e.keyCode;
            isCtrl = window.event.ctrlKey
        }
        else if (e.which) {
            key = e.which;
            isCtrl = e.ctrlKey;
        }

        if (isNaN(key)) return true;

        keychar = String.fromCharCode(key);

        // check for backspace or delete, or if Ctrl was pressed
        if (key == 8 || isCtrl) {
            return true;
        }

        reg = /\d/;
        var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
        var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

        return reg.test(keychar);
    }
        </script>


<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            SixthSubject Student List.
        </h2>
    </div>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <table width="100%" border="0" cellspacing="2" cellpadding="2"class="cnt-box">
                    <tr>
                    <td>
                    <span class="tbltxt"> Admission Session : &nbsp
                    <asp:DropDownList ID="ddlSession" runat="Server" CssClass="vsmalltb"></asp:DropDownList></span>
                    <span class="tbltxt"> Roll No Between : &nbsp  <asp:TextBox  ID="txtFirstNo" runat="server" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox>
                    And <asp:TextBox  ID="txtSecondNo" runat="server"  onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="return chkFirstNo();"></asp:TextBox></span>
                    <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Clicked" />
                     <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Clicked" />
                    </td>
                    </tr>
                    </table>
                    <div>
                    
                    <asp:Label ID="LblReport" runat="server"></asp:Label>
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
          <Triggers>
           
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
          </asp:UpdatePanel>
</asp:Content>

