<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="RevertPromotionDetain.aspx.cs" Inherits="Admissions_RevertPromotionDetain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" language="javascript">
    function valDetails() {
        var Admno = document.getElementById("<%=txtAdmnno.ClientID %>").value;
        var cls = document.getElementById("<%=drpClass.ClientID %>").value;
        var sec = document.getElementById("<%=drpSection.ClientID %>").value;
        if (Admno.trim() == "") {
            alert("Please enter admission number !");
            document.getElementById("<%=txtAdmnno.ClientID %>").focus();
            return false;
        }
        else if (cls < 1)
        {
        alert("Please Select Class !");
            document.getElementById("<%=drpClass.ClientID %>").focus();
            return false;
        }
        else if(sec < 1)
        {
        alert("Please Select Setion !");
            document.getElementById("<%=drpSection.ClientID %>").focus();
            return false;
        }
        else {
            return true;
        }
    }
</script>
<script type="text/javascript" language="javascript">


    function ToggleAll(e) {
        if (e.checked) { CheckAll(); }
        else { ClearAll(); }
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
    function CheckAllDataGridCheckBoxes(aspCheckBoxID, checkVal, rows, gridname) {
        rows = rows + 2;
        for (i = 0; i < document.forms[0].elements.length; i++) {
            elm = document.forms[0].elements[i].type
            if (elm == "checkbox") {
                for (j = 2; j < rows; j++) {
                    var a;
                    if (j < 10) {
                        a = "ctl00_ContentPlaceHolder1_" + gridname + "_ctl0" + j + "_chk";
                    }
                    else {
                        a = "ctl00_ContentPlaceHolder1_" + gridname + "_ctl" + j + "_chk";
                    }
                    document.getElementById(a).checked = checkVal;
                }

            }

        }

    }



    function Checkchecked(aspCheckBoxID, rows, gridname) {
        var flag;
        rows = rows + 2;
        for (i = 0; i < document.forms[0].elements.length; i++) {
            elm = document.forms[0].elements[i].type
            if (elm == "checkbox") {
                for (j = 2; j < rows; j++) {
                    var a;
                    if (j < 10) {
                        a = "ctl00_ContentPlaceHolder1_" + gridname + "_ctl0" + j + "_chk";
                    }
                    else {
                        a = "ctl00_ContentPlaceHolder1_" + gridname + "_ctl" + j + "_chk";
                    }
                    if (document.getElementById(a).checked)
                        flag = "true";
                    else
                        flag = "false";
                }

            }

        }
        if (flag == "false") {
            return true;
        }
        else
            return false;

    }


    </script>
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;width:600px;">
        <h2>
            Revert Class Promotion
        </h2>
        
        
        
        
        <asp:Label ID="lblMsg" Text="" runat="server" ></asp:Label>
    </div>
    <div class="spacer"></div>
<table width="100%" >
    <tr>
        <td class="tbltxt cnt-box">
            <div class="cnt-sec">
            <span class="ttl3">Select Session:</span><asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True"
                CssClass="tbltxtbox" TabIndex="1" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
            </asp:DropDownList>
            </div> 
            <div class="cnt-sec">
            <span class="ttl3">Select Class:</span><asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True"
                CssClass="tbltxtbox" TabIndex="2" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
            </asp:DropDownList>
            </div>
            <div class="cnt-sec">
            <span class="ttl3">Select Section:</span><asp:DropDownList ID="drpSection" runat="server" 
                CssClass="tbltxtbox" TabIndex="3" >
            </asp:DropDownList>
            </div>
              <div class="cnt-sec">
                 <asp:Button ID="btnShow" runat="server" Text="Show" 
                    onfocus="active(this);" onblur="inactive(this);" onclick="btnShow_Click"/>
              </div>
            
        </td>
    </tr>
        <tr>
        <td class="tbltxt cnt-box2" >
        <span class="ttl3" style="width:300px;" >OR      Search For Single Student </span>
           
        </td>
        </tr>
        <tr>
            <td class="tbltxt cnt-box" >
            <%--<div class="cnt-sec">
            <span class="ttl3">Select Student:</span>
            <asp:DropDownList ID="drpStudent" runat="server" AutoPostBack="True" CssClass="tbltxtbox"
                TabIndex="5" OnSelectedIndexChanged="drpStudent_SelectedIndexChanged">
            </asp:DropDownList>
            </div>--%>
                <div class="cnt-sec">
                <span class="ttl3"> Admission No :</span>
                <asp:TextBox ID="txtAdmnno" runat="server" CssClass="vsmalltb" ></asp:TextBox>
                    <asp:Button ID="BtnSearch" runat="server" Text="Search" OnClick="BtnSearch_clicked" OnClientClick="return valDetails();" />
                </div>
             
                &nbsp;
                <asp:Button ID="btnRevert" runat="server" Text="Revert"
                    Enabled="false" OnClick="btnRevert_Click"  />
            </td>
        </tr>
        <tr>
        </tr>
        <tr>
        <td>
        <asp:GridView ID="grdstudents" Width="100%" runat="server" AutoGenerateColumns="false"
                    CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    TabIndex="5">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--   <input type="checkbox" name="Checkb" value='<%#Eval("id")%>' runat="server" id="Checkb"/>--%>
                                <asp:CheckBox ID="chk" runat="server"  />
                                <asp:Label ID="hftxtAdmno"  runat="server" Text='<%#Eval("Admnno") %>' visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <input id="chkAll1" type="checkbox" onclick="CheckAllDataGridCheckBoxes('chk',this.checked,<%=grdrowcount %>,'grdstudents')"  />
                                <%--
                                <input type="checkbox" value="ON" name="toggleAll" id="Checkb" runat="server" onclick='ToggleAll(this)' />Select
--%>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblname" runat="server" Text='<%#Eval("fullname")%>'></asp:Label>
                             <%--   <asp:Label ID="lblid" runat="server" Visible="false" Text='<%#Eval("id")%>'></asp:Label>
                                <asp:Label ID="lblStudType" runat="server" Visible="false" Text='<%#Eval("StudType")%>'></asp:Label>--%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Admission No.">
                            <ItemTemplate>
                                <asp:Label ID="lbladmnno" runat="server" Text='<%#Eval("AdmnNo")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                     <%--   <asp:TemplateField HeaderText="Section">
                            <ItemTemplate>
                                <asp:DropDownList ID="drpsection" runat="server" DataSource='<%#getsection()%>' DataTextField="section"
                                    DataValueField="section" AutoPostBack="True" OnSelectedIndexChanged="drpsection_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>--%>
                        <%--<asp:TemplateField HeaderText="Grade">
                            <ItemTemplate>
                                <asp:DropDownList ID="drpgrade" runat="server" DataSource='<%#getgrades()%>' DataTextField="gradename"
                                    DataValueField="gradeid">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Roll No. Assigned">
                            <ItemTemplate>
                                <asp:TextBox ID="txtrollno" runat="server" Text='<%#Eval("rollno")%>'></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
        </td>
        </tr>
  </table>
                
</asp:Content>

