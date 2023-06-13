<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="AddonFacility.aspx.cs" Inherits="Admissions_AddonFacility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function CnfSubmit() {




            var studclass = document.getElementById("<%=drpClass.ClientID %>").value;
            var student = document.getElementById("<%=drpSelectStudent.ClientID %>").value;



            if (studclass == "0") {
                alert("Please Select Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            if (student == "0") {
                alert("Please Select Student !");
                document.getElementById("<%=drpSelectStudent.ClientID %>").focus();
                return false;
            }

            else {
                return true;
            }
        }

        function CnfDelete() {

            if (confirm("You are going to delete the selected choice. Do you wish to continue?")) {

                return true;
            }
            else {

                return false;
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






        function cnf() {

            if (confirm("You Are Going To Initialize A Students Account ? Do You Want To Continue??")) {
                return true;
            }
            else {

                return false;
            }
        }
        
    </script>


    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Set Choice For Additional Facilities</h2>
                <div style="text-align:center;">
                    <asp:Label ID="lblMsg" Font-Bold="true" runat="server" Text=""
                        CssClass="tbltxt"></asp:Label>
                </div>
            </div>
            
                
            <table  width="100%">
                <tr>
                    <td style="width: 100%;" valign="top" class="tbltxt cnt-box">
                        <table width="100%">
                            <tr>
                                <td align="left" valign="top"  colspan="2">
                                    <div class="cnt-sec">
                                    <span class="ttl3"><asp:Label ID="lblSession" runat="server" Text="Session :"></asp:Label></span>
                                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                                        Width="100px" CssClass="tbltxtbox" TabIndex="1">
                                    </asp:DropDownList>
                                    </div>
                                    
                                     <div class="cnt-sec">
                                    <span class="ttl3"><asp:Label ID="lblClass" runat="server" Text="Class :"></asp:Label>
                                    <span class="error">*</span></span>
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                        Width="100px" CssClass="tbltxtbox" TabIndex="2">
                                    </asp:DropDownList>
                                    </div>
                                     <div class="cnt-sec">
                                    <span class="ttl3"><asp:Label ID="lblSection" runat="server" Text="Section :"></asp:Label> </span>
                                    <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSection_SelectedIndexChanged"
                                        Width="50px" CssClass="tbltxtbox" TabIndex="3">
                                    </asp:DropDownList>
                                   </div>
                                    <div class="cnt-sec">
                                    <span class="ttl3">Select Student :</span>
                                    <asp:DropDownList ID="drpSelectStudent" runat="server" 
                                        onselectedindexchanged="drpSelectStudent_SelectedIndexChanged" 
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                    </div>
                                    </td>
                                    
                            </tr>
                            </table>
                            <table width="100%">
                            <tr>
                                <td valign="bottom" >
                                     <fieldset id="Fieldset2" runat="server" class="cnt-box2 spaceborder1">
                                       <legend style="background-color: Transparent;" class="tbltxt"><strong>Choice
                                            for Additional Facilities </strong></legend>
                                            <asp:CheckBoxList ID="chkFee" RepeatDirection="Horizontal" runat="server">
                                            </asp:CheckBoxList>
                                            &nbsp;
                                            <br />
                                            <asp:Button ID="btnAssign" runat="server" Text="Set Choice for Additional Facilities"
                                               OnClientClick="return CnfSubmit();"  OnClick="btnAssign_Click" />&nbsp; 
                                                <asp:Button ID="btnDelChoice" runat="server" Text="Delete Selected Choice"
                                               OnClientClick="return CnfDelete()" OnClick="btnDelChoice_Click" />&nbsp; 
                                               <asp:Button ID="btnStudAdmn" 
                                            runat="server" Text="Student Admission" onclick="btnStudAdmn_Click" />
                                                &nbsp; <asp:Button ID="btnStudList" runat="server" Text="Student List" 
                                            onclick="btnStudList_Click" />
                                        </fieldset>
                                </td>
                            </tr>
                            <tr>
                                 <td>
                                         &nbsp;
                                 </td>
                            </tr>
                        </table>
                    </td>
            </table>
                                            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

