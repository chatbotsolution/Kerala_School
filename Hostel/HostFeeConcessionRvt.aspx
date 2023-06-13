<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="HostFeeConcessionRvt.aspx.cs" Inherits="Hostel_HostFeeConcessionRvt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript">
    function valDetails() {
        var Admno = document.getElementById("<%=txtadmnno.ClientID %>").value;
        if (Admno.trim() == "") {
            alert("Please enter admission number !");
            document.getElementById("<%=txtadmnno.ClientID %>").focus();
            return false;
        }
        else {
            return true;
        }
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
    function valid() {
        var flag;
        var ml = document.aspnetForm;
        var len = ml.elements.length;
        for (var i = 0; i < len; i++) {
            var e = ml.elements[i];
            if (e.name == "Checkb") {
                if (e.checked)
                    flag = true;
                else
                    flag = false;
            }
        }
        if (flag == true)
            return true;
        else {
            alert("Please select any record");
            return false;
        }
    }
    function CnfRevert() {

        if (confirm("You are going to Revert Consession of The Selected Student!!. Do you want to continue?")) {

            return true;
        }
        else {

            return false;
        }
    }  
</script>
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Revert Fee Concession</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" />
</div>
<center>
                <div>
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="tbltxt" Text=""></asp:Label></div>
            </center>
                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <td width="50px" class="tbltxt">
                            Session :
                        </td>
                        <td width="100px" class="tbltxt">
                            <asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb"
                                AutoPostBack="true" TabIndex="3" 
                                onselectedindexchanged="drpSession_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="40px">
                            Class :
                        </td>
                        <td class="tbltxt" width="70px">
                            <asp:DropDownList ID="drpclass" runat="server" AutoPostBack="True"
                                CssClass="vsmalltb" Width="70px" meta:resourcekey="drpclassResource1" 
                                TabIndex="4" onselectedindexchanged="drpclass_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="55px">
                            Student :
                        </td>
                        <td class="tbltxt" Width="170px">
                            <asp:DropDownList ID="drpstudent" runat="server" Width="170px" AutoPostBack="True"
                                OnSelectedIndexChanged="drpstudent_SelectedIndexChanged" 
                                CssClass="largetb" TabIndex="5">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" width="70">
                            Student Id :
                        </td>
                        <td class="tbltxt">
                            <asp:TextBox ID="txtadmnno" runat="server" CssClass="vsmalltb" TabIndex="6" 
                                Width="75px"></asp:TextBox><span
                                class="error">*</span> &nbsp;
                            <asp:Button ID="btnDetail" runat="server" Text="Show" OnClientClick="return valDetails();"  onfocus="active(this);" onblur="inactive(this);"
                                OnClick="btnDetail_Click" Style="width: 48px; height: 26px;" 
                                TabIndex="7" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6"><br /></td>
                    </tr>
                    <tr>
                        <td colspan="6">
                             <asp:Button ID="btnReveret" runat="server" Text="Revert Concession" OnClientClick="return CnfRevert();"
                            onclick="btnReveret_Click" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6"><br /></td>
                    </tr>
                    <tr>
                    <td colspan="6">
                        <asp:GridView ID="grdConsession" runat="server" AutoGenerateColumns="false">
                        <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15px" />
                            <HeaderStyle HorizontalAlign="Center" Width="15px" />
                            <ItemTemplate>
                                <input name="Checkb" type="checkbox" value='<%#Eval("AdjustmentId")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FeeDtls" HeaderText="Fee">
                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Amnt" HeaderText="Consession Amount">
                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                        </asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                           No Concession Exists For This Student
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        </asp:GridView>
                    </td>
                    </tr>
                </table>

</asp:Content>
