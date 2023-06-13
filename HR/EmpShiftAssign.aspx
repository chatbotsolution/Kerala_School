<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpShiftAssign.aspx.cs" Inherits="HR_EmpShiftAssign" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<style type="text/css">
        .parentDisable
        {
            z-index: 999;
            width: 100%;
            height: 100%;
            display: none;
            position: absolute;
            top: 0;
            left: 0;
        }
        #popup
        {
            width: 400px;
            height: 170px;
            position: relative;
            top: 120px;
            border-style: groove;
            background-color: #F0F0F0;
            border-color: Aqua;
            cursor: auto;
        }
        #close
        {
            position: absolute;
            top: 0;
            right: 0;
        }
</style>

<script type="text/javascript">
    function isValid() {
        var Dt = document.getElementById("<%=txtStartDt.ClientID %>").value;
        var Shift = document.getElementById("<%=drpShift.ClientID %>").value;
        if (Dt.trim() == "") {
            alert("Please Enter Date!");
            document.getElementById("<%=txtStartDt.ClientID %>").focus();
            return false;
        }
        else if (Shift == "0") {
            alert("Please Enter Shift Start Time!");
            document.getElementById("<%=drpShift.ClientID %>").focus();
            return false;
        }
        else {
            return true;
        }
    }
    function ReValid() {

        var Dt = document.getElementById("<%=txtReShftDt.ClientID %>").value;
        var Shift = document.getElementById("<%=drpShiftAsign.ClientID %>").value;
        var Emp = document.getElementById("<%=drpEmpAsign.ClientID %>").value;
        if (Emp == "0") {
            alert("Please Select Employee!");
            document.getElementById("<%=drpEmpAsign.ClientID %>").focus();
            return false;
        }
        else if (Shift == "0") {
            alert("Please Select Shift !");
            document.getElementById("<%=drpShiftAsign.ClientID %>").focus();
            return false;
        }
        else if (Dt.trim() == "") {
            alert("Please Enter Date!!");
            document.getElementById("<%=txtReShftDt.ClientID %>").focus();
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

                if (e.checked) {
                    flag = true;
                    break;
                }
                else
                    flag = false;
            }
        }
        //alert(flag);
        if (flag == true) {
            return CnfDelete();
        }
        else {
            alert("Select any Employee");
            return false;
        }
    }
    function CnfDelete() {

        if (confirm("You are going to Assign Shift To selected Employee(s). Do you want to continue ?")) {

            return true;
        }
        else {

            return false;
        }
    }

    function pop(div) {
        document.getElementById(div).style.display = 'block';
        return false;
    }
    function hide(div) {
        document.getElementById(div).style.display = 'none';
        return false;
    }
</script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Assign Working Shift</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <table width="100%">
        <tr>
            <td width="250px">
                Designation&nbsp;:&nbsp;<asp:DropDownList runat="server" ID="drpDesig" 
                    onselectedindexchanged="drpDesig_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                &emsp;
            </td>
            <td width="250px">
                Type&nbsp;:&nbsp;<asp:DropDownList runat="server" ID="drpType" 
                    onselectedindexchanged="drpType_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">-- All --</asp:ListItem>
                    <asp:ListItem>Permanent</asp:ListItem>
                    <asp:ListItem>Temporary</asp:ListItem>
                </asp:DropDownList>
                &emsp;
            </td>
            <td>
                Name&nbsp;:&nbsp;<asp:DropDownList runat="server" ID="drpEmp" 
                    onselectedindexchanged="drpEmp_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Assign Date:&nbsp;&nbsp;<asp:TextBox ID="txtStartDt" TabIndex="1" Width="88px" 
                    runat="server"></asp:TextBox>&nbsp;
                <rjs:PopCalendar ID="dtpStrtDt" runat="server" Control="txtStartDt"></rjs:PopCalendar>
                &emsp;
            </td>
            <td colspan="2">
                Shift&nbsp;:&nbsp;<asp:DropDownList runat="server" ID="drpShift">
                </asp:DropDownList> 
                 <asp:Button ID="btnSave" Text="Save" runat="server" onclick="btnSave_Click" OnClientClick="return isValid();" />
               
                <%--<input type="submit" value="Reassign Shift" onclick="return pop('pop1');"/>--%>
                <asp:Button ID="btnReassign" Text="Modify Existing Shift" runat="server" 
                    onclick="btnReassign_Click" />
            </td>
          
        </tr>
        <tr>
        <td colspan="2">
        <asp:Label Text="" ID="lblMsg" runat="server"></asp:Label>
        </td>
        <td align="right"><asp:Label Text="" ID="lblCount" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:GridView ID="grdEmployee" runat="server" AutoGenerateColumns="False" Width="100%"
                    EmptyDataText="No Record">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                            <HeaderStyle HorizontalAlign="Center" Width="20px" />
                            <ItemTemplate>
                                <input name="Checkb" type="checkbox" value='<%#Eval("EmpId")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee">
                            <ItemTemplate>
                                <%#Eval("SevName")%> - (<%#Eval("Designation")%>)
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phone No.">
                            <ItemTemplate>
                                <%#Eval("Mobile")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <div id="pop1" class="parentDisable">
  <center>
    <div id="popup">
      <a href="#" onclick="return hide('pop1')">
         <img id="close" src="../images/refresh_icon.png" alt="Close" />
      </a>
      <div align="center">
        <h2>Modify Existing Shift</h2> 
	    <table>
	    <tr><td colspan="3"><asp:Label runat="server" Text="" ID="lblMsg2" Width="390px"></asp:Label></td></tr>
	    <tr>
	        <td>
	        Name
	        </td>
	        <td>
	        :
	        </td>
	        <td>
	        <asp:DropDownList runat="server" ID="drpEmpAsign" AutoPostBack="true" 
                    onselectedindexchanged="drpEmpAsign_SelectedIndexChanged">
                </asp:DropDownList>
	        </td>
	    </tr>
	    <tr>
	        <td>
	        Shift
	        </td>
	        <td>
	        :
	        </td>
	        <td>
	        <asp:DropDownList runat="server" ID="drpShiftAsign">
                </asp:DropDownList>
	        </td>
	    </tr>
	    <tr>
	        <td>
	        Date
	        </td>
	        <td>
	        :
	        </td>
	        <td>
	        <asp:TextBox ID="txtReShftDt" TabIndex="1" Width="100px" runat="server"></asp:TextBox>&nbsp;
                <rjs:PopCalendar ID="dtpReShiftDt" runat="server" Control="txtReShftDt"></rjs:PopCalendar>
	        </td>
	    </tr>
	    <tr>
	        <td colspan="2">
	        </td>
	        <td>
                <asp:Button ID="btnSaveShift" runat="server" Text="Save" 
                onclick="btnSaveShift_Click" OnClientClick="return ReValid();"/>
	        </td>
	    </tr>
	    </table>
      </div>
    </div>
  </center>
</div>
</asp:Content>

