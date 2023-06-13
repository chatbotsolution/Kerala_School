<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="ProspTypeAddNew.aspx.cs" Inherits="Admissions_ProspTypeAddNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       
    <script type="text/javascript" language="javascript">
        function validateCheckBoxList() {
            var isAnyCheckBoxChecked = false;
            var checkBoxes = document.getElementById("ctl00_ContentPlaceHolder1_chklClass").getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox") {
                    if (checkBoxes[i].checked) {
                        isAnyCheckBoxChecked = true;
                        break;
                    }
                }
            }
            if (!isAnyCheckBoxChecked) {
                alert("Select a Class");
            }

            return isAnyCheckBoxChecked;
        }

        function isValid() {
            var ProsType = document.getElementById("<%=txtProsType.ClientID %>").value;
            if (ProsType == "") {
                alert("Please Enter Prospectus Type!");
                document.getElementById("<%=txtProsType.ClientID %>").focus();
                return false;
            }
        }
         </script>
      <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
             Define New Prospectus Type</h2>
    </div>
    <br />
    <table cellspacing="0" cellpadding="3" width="100%" border="0" class="cnt-box">
        <tr>
            <td align="left" class="tbltxt" style="height: 25px" width="25%">
                Prospectus Type
            </td>
            <td align="left" class="tbltxt ttl" style="height: 25px">
                :<asp:TextBox ID="txtProsType" runat="server"   CssClass="tbltxtbox largetb  " TabIndex="1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" class="tbltxt">
                Applicable for Class
            </td>
            <td align="left" valign="top" class="tbltxt ttl">
                <span style="float:left">:</span><asp:CheckBoxList ID="chklClass" RepeatDirection="Vertical" runat="server" TabIndex="3"
                    AutoPostBack="false" CssClass="chklst largetb wdth-250" >
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td></td>
            <td valign="top" align="left" colspan="2" style="padding-left:8px;">
                <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn"
                    OnClientClick="return isValid(); validateCheckBoxList();" onfocus="active(this);" onblur="inactive(this);" TabIndex="5"></asp:Button>&nbsp;
               
            </td>
        </tr>
           <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" colspan="3">
                <asp:Label ID="lblerr" runat="server" ForeColor="Red">
                </asp:Label>
            </td>
        </tr>
        </table>
</asp:Content>

