<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="BusRouteMaster.aspx.cs" Inherits="Masters_BusRouteMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function IsValidate() {
            var Route = document.getElementById("<%=txtRouteNm.ClientID %>").value;
            var Fee = document.getElementById("<%=txtFee.ClientID %>").value;
            if (Route.trim() == "") {
                alert("Please Enter Route Name");
                document.getElementById("<%=txtRouteNm.ClientID %>").focus();
                return false;
            }

            if (Fee.trim() == "") {
                alert("Please Enter Fee");
                document.getElementById("<%=txtFee.ClientID %>").focus();
                return false;
            }

            else {

                var result = confirm("Want to Save");
                if (result) {
                    return true;
                }
                else {
                    return false;
                }

            }
        }


        //        function ClearAll()
        //         {
        //             document.getElementById("<%=txtRouteNm.ClientID %>").value == "";
        //             document.getElementById("<%=txtDistance.ClientID %>").value == "";
        //             document.getElementById("<%=txtFee.ClientID %>").value == "";
        //             document.getElementById("<%=txtRemarks.ClientID %>").value == "";
        //          
        //                          
        //        }

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

            return isFirstN || isFirstD || reg.test(keychar);
        }


    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Bus Route Master</h2>
    </div>
    <div class="spacer"></div>
    <table width="100%" border="0" cellspacing="5px" cellpadding="0"  class="cnt-box tbltxt">
        <tr>
            <td>
                 &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 130px"  align="left">
                Route Name<span class="mandatory">*</span>
            </td>
            <td style="width: 5px" align="left" >
                :
            </td>
            <td align="left">
                <asp:TextBox ID="txtRouteNm" TabIndex="1" runat="server"
                    CssClass="wdth-200"></asp:TextBox>&nbsp;
            </td>
        </tr>
        
        <tr>
            <td  align="left">
                Distance (in km)
            </td>
            <td  align="left">
                :
            </td>
            <td align="left">
                <asp:TextBox ID="txtDistance" runat="server" CssClass="wdth-200" onkeypress="return blockNonNumbers(this, event, true, true);"
                    TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td  align="left">
                Bus Fee<span class="mandatory" id="spInstNo" runat="server">*</span>
            </td>
            <td  align="left">
                :
            </td>
            <td align="left">
                <asp:TextBox ID="txtFee" onkeypress="return blockNonNumbers(this, event, true, true);"
                    runat="server" TabIndex="3" CssClass="wdth-200"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td style="width: 130px"  align="left" valign="top">
                Remarks
            </td>
            <td style="width: 5px" align="left"  valign="top">
                :
            </td>
            <td align="left">
                <asp:TextBox ID="txtRemarks" TabIndex="4" runat="server" 
                   CssClass="wdth-200" ></asp:TextBox>&nbsp;
            </td>
        </tr>
         
        <tr>
            <td style="width: 130px"  align="left">
                Active Status<span class="mandatory" id="spDrawn" runat="server">*</span>
            </td>
            <td style="width: 5px" align="left" >
                :
            </td>
            <td align="left">
                <asp:CheckBox ID="chkActive" runat="server" TabIndex="5" Checked="True"></asp:CheckBox>
            </td>
        </tr>
       
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
            <td valign="top" align="left">
                <asp:Button ID="btnSubmit" TabIndex="6" OnClick="btnSubmit_Click" runat="server"
                    Width="100px" onfocus="active(this);" onblur="inactive(this);" Text="Save" OnClientClick="return IsValidate();">
                </asp:Button>
               <%-- <asp:Button ID="btnClear" TabIndex="7" OnClientClick="ClearAll();" OnClick="btnClear_Click" runat="server" Text="Clear"
                    onfocus="active(this);" onblur="inactive(this);" CausesValidation="false" Width="120px">
                </asp:Button>--%>
                <asp:Button ID="btnList" runat="server" Text="Go To List" OnClick="btnList_Click"
                    onfocus="active(this);" onblur="inactive(this);" Width="120px" TabIndex="8" />
            </td>
        </tr>
       
        <tr>
            <td colspan="3">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
