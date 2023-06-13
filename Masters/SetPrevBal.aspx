<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="SetPrevBal.aspx.cs" Inherits="Masters_SetPrevBal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
 
    function textBoxOnBlur(txtPrevAmountId, txtPrevAmount2Id, labelId)
{
 var txtPrevAmountRef = document.getElementById(txtPrevAmountId);
 var txtPrevAmount2Ref = document.getElementById(txtPrevAmount2Id);
 var labelRef = document.getElementById(labelId);

 var txtPrevAmountValue = parseInt(txtPrevAmountRef.value);
 var txtPrevAmount2Value = parseInt(txtPrevAmount2Ref.value);

 if ( isNaN(txtPrevAmountValue) == true )
  txtPrevAmountValue = 0;
 if ( isNaN(txtPrevAmount2Value) == true )
  txtPrevAmount2Value = 0;

 labelRef.innerHTML = txtPrevAmountValue - txtPrevAmount2Value;
}
// -->
//------------------------------------
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


        function ToggleAll(e) {
            if (e.checked) {
                CheckAll();
            }
            else {
                ClearAll();
            }
        }

        function CheckAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = true;
                }
            }
            ml.toggleAll.checked = true;
        }

        function ClearAll() {
            var ml = document.aspnetForm;
            var len = ml.elements.length;
            for (var i = 0; i < len; i++) {
                var e = ml.elements[i];
                if (e.name == "Checkb") {
                    e.checked = false;
                }
            }
            ml.toggleAll.checked = false;
        }
        function add() {

            var Class = document.getElementById("<%=drpClass.ClientID %>").value;


            if (Class == 0) {
                alert("Select a Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
        }
       
        }
   
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Previous Balance Entry</h2>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table style="height: 360px;" width="100%">
                <tr>
                    <td style="width: 100%;" valign="top">
                        <table width="100%">
                            <tr>
                                <td align="left" valign="top" class="tbltxt" style="border: solid 1px Black; width: 100%;"
                                    colspan="2">
                                    <asp:Label ID="lblSession" runat="server" Text="Session:"></asp:Label>
                                    <asp:DropDownList Enabled="false" ID="drpSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                                        Width="100px" CssClass="tbltxtbox" TabIndex="1">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblClass" runat="server" Text="Class:"></asp:Label><span class="error">*</span>
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                        Width="100px" CssClass="tbltxtbox" TabIndex="2">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblSection" runat="server" Text="Section:"></asp:Label>
                                    <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSection_SelectedIndexChanged"
                                        Width="50px" CssClass="tbltxtbox" TabIndex="3">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblStudent" runat="server" Text="Select Student:"></asp:Label>
                                    <asp:DropDownList ID="drpSelectStudent" runat="server" CssClass="tbltxtbox" AutoPostBack="True"
                                        OnSelectedIndexChanged="drpSelectStudent_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" TabIndex="6" OnClientClick="return add();"
                                        Text="Show Students" />&nbsp;<asp:Button ID="btnSetActHead" runat="server" 
                                        onclick="btnSetActHead_Click" Text="Set Account Head" />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Text="" CssClass="tbltxt"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt">
                                    <asp:Label ID="lblRecCount" runat="server" Text="Label" CssClass="tbltxt"></asp:Label>
                                </td>
                                <td align="right" valign="top" style="height: 15px; width: 100px;" class="tbltxt">
                                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Submit"
                                        TabIndex="8" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:GridView ID="grdStudPrevAC" runat="server" AutoGenerateColumns="False" Width="100%"
                                        CssClass="mGrid" AlternatingRowStyle-CssClass="alt" TabIndex="7" 
                                        onrowdatabound="grdStudPrevAC_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Admission No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmnNo" runat="server" Text='<%#Eval("Admissionno")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("FullName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Father's Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFName" runat="server" Text='<%#Eval("FatherName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldob" runat="server" Text='<%#Eval("dateofbirth")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Previous Balance" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="left" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPrevAmount" runat="Server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        MaxLength="6" Text='<%#Eval("Balance")%>' CssClass="vsmalltb"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="left" ItemStyle-Width="120px" >
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPrevAmount2" runat="Server" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        MaxLength="6" Text='<%#Eval("Balance")%>' CssClass="vsmalltb" Enabled="False"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BalanceAdd" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="left" ItemStyle-Width="120px" Visible="False">
                                               <ItemTemplate>
                                                <asp:label id="Label1" runat="server"></asp:label>
                                               </ItemTemplate>
                                              </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;
                                </td>
                                <td align="right" valign="top" style="height: 15px; width: 100px;" class="tbltxt">
                                    <asp:Button ID="btnUpdate1" runat="server" OnClick="btnUpdate_Click" Text="Submit"
                                        TabIndex="9" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
          <Triggers>
           <%-- <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnUpdate1" EventName="Click" />
              <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
            --%>
             <asp:PostBackTrigger ControlID="btnUpdate" />
              <asp:PostBackTrigger ControlID="btnUpdate1" />
            <asp:PostBackTrigger ControlID="btnShow" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

