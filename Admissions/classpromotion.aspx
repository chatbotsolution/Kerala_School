<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="classpromotion.aspx.cs" Inherits="Admissions_classpromotion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
        function checkclass(aspCheckBoxID, rows, gridname, sts) {
            if (checkStream() == false) { return false; }
            // alert("hi");
            if (document.getElementById('<%=drpclass.ClientID%>').value == document.getElementById('<%=drpnewclass.ClientID%>').value) {
                alert("Can't promote in same class");
                return false;
            }
            if (Checkchecked(aspCheckBoxID, rows, gridname) != true)
                return true;
            else {
                alert("Please select any record");
                return false;
            }
        }

        function checkclassDet(aspCheckBoxID, rows, gridname, sts) {
            if (checkStream() == false) {return false;}
            // alert("hi");
            if (document.getElementById('<%=drpclass.ClientID%>').value != document.getElementById('<%=drpnewclass.ClientID%>').value) {
                alert("New class should be same in case of detained student");
                return false;
            }
            if (Checkchecked(aspCheckBoxID, rows, gridname) != true)
                return true;
            else {
                alert("Please select any record");
                return false;
            }
            

        }
       function checkStream()
        {
            var newcls = document.getElementById("<%=drpnewclass.ClientID %>").value;
            var stream = document.getElementById("<%=drpStream.ClientID %>").value;
            if (newcls > 13 && stream == 0) {
                alert("Select Stream");
                return false;
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
    
    
    <script type = "text/javascript">
        function DisableButtons() {
            var inputs = document.getElementsByTagName("INPUT");
            for (var i in inputs) {
                if (inputs[i].type == "button" || inputs[i].type == "submit") {
                    inputs[i].disabled = true;
                }
            }
        }
        window.onbeforeunload = DisableButtons;
    </script>
    
    
    
     <%-- <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>--%>
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Class Promotion
        </h2>
    </div>
    <div class="spacer"></div>
    <table width="100%" style="height: 360px;">
        <tr>
            <td class="tbltxt cnt-box" >
                
                <div class="cnt-sec">
                <span class="ttl3">Select Session:</span>
                <asp:DropDownList ID="drpsession" runat="server" OnSelectedIndexChanged="drpsession_SelectedIndexChanged"
                    AutoPostBack="True" CssClass="tbltxtbox" TabIndex="1">
                </asp:DropDownList>
                </div>
                 <div class="cnt-sec">
                <span >Select Class:</span><asp:DropDownList ID="drpclass" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="drpclass_SelectedIndexChanged" CssClass="tbltxtbox" TabIndex="2">
                </asp:DropDownList>
                 <span >Section:</span><asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="drpSection_SelectedIndexChanged" CssClass="tbltxtbox" TabIndex="3">
                </asp:DropDownList>
                </div>
                
                 <div class="cnt-sec">
                <span class="ttl3">New Session:</span>
                <asp:TextBox ID="txtnewsession" runat="server" Width="65px" ReadOnly="True" TabIndex="4"></asp:TextBox>
               </div>
                 <div class="cnt-sec">
                <span style="margin-right: 10px;" >New Class:</span><asp:DropDownList ID="drpnewclass" runat="server" CssClass="tbltxtbox"
                     TabIndex="5">
                </asp:DropDownList>
                 <span >Stream:</span>
               <asp:DropDownList ID="drpStream" runat="server" CssClass="tbltxtbox"  Enabled="false"
                     TabIndex="6">
                    <asp:ListItem Value="0">Select Stream</asp:ListItem>
                 <%--     <asp:ListItem Value="2">Arts</asp:ListItem>--%>
                       <asp:ListItem Value="3">Commerce</asp:ListItem>
                        <asp:ListItem Value="4">Science</asp:ListItem>
                      <%--  <asp:ListItem Value="5">Pure Science</asp:ListItem>--%>
                </asp:DropDownList> 
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="error">
                    Note : Select <b>Session</b> and <b>Class</b> to show respective students list for promotion
                </div>
            </td>
        </tr>
        
        <tr>
        <td align="right">
        <table width="100%" >
        <tr>
            <td align="left">
            <asp:Label ID="lblMsg" runat="server" Text="" Font-Bold="true" CssClass="tbltxt" ></asp:Label>
            </td>
            <td align="right"><asp:Label ID="lblRecCount" runat="server" Text="" Font-Bold="true"  CssClass="tbltxt" ></asp:Label>
            </td>
        </tr>
        </table>
         
        </td>
        </tr>
        <tr>
            <td valign="top" style="height: 195px">
                <asp:GridView ID="grdstudents" Width="100%" runat="server" AutoGenerateColumns="false"
                    CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    TabIndex="5">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--   <input type="checkbox" name="Checkb" value='<%#Eval("id")%>' runat="server" id="Checkb"/>--%>
                                <asp:CheckBox ID="chk" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <input id="chkAll1" type="checkbox" onclick="CheckAllDataGridCheckBoxes('chk',this.checked,<%=grdrowcount %>,'grdstudents')" />
                                <%--
                                <input type="checkbox" value="ON" name="toggleAll" id="Checkb" runat="server" onclick='ToggleAll(this)' />Select
--%>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblname" runat="server" Text='<%#Eval("fullname")%>'></asp:Label>
                                <asp:Label ID="lblid" runat="server" Visible="false" Text='<%#Eval("id")%>'></asp:Label>
                                <asp:Label ID="lblStudType" runat="server" Visible="false" Text='<%#Eval("StudType")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Admission No.">
                            <ItemTemplate>
                             <asp:Label ID="lblOldAdmnNo" runat="server" Text='<%#Eval("AdmnNo")%>'></asp:Label>
                                <asp:Label ID="lbladmnno" runat="server" Text='<%#Eval("AdmnNo")%>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Section">
                            <ItemTemplate>
                                <asp:DropDownList ID="drpsection" runat="server" DataSource='<%#getsection()%>' DataTextField="section"
                                    DataValueField="section">
                                    <%-- AutoPostBack="True" OnSelectedIndexChanged="drpsection_SelectedIndexChanged"--%>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Grade">
                            <ItemTemplate>
                                <asp:DropDownList ID="drpgrade" runat="server" DataSource='<%#getgrades()%>' DataTextField="gradename"
                                    DataValueField="gradeid">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:TemplateField>
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
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btndetain" runat="server" OnClientClick="return checkclassDet('chk','0','grdstudents','0');"
                    OnClick="btndetain_Click" Text="Detain" CausesValidation="False" TabIndex="6" />
                <asp:Button ID="btnprom" OnClientClick="return checkclass('chk','0','grdstudents','1');"
                    runat="server" OnClick="btnprom_Click" Text="Promote" TabIndex="7" />
                <asp:Button ID="btncancel" runat="server" OnClick="btncancel_Click" Text="Cancel"
                    CausesValidation="False" TabIndex="8" />
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
        </tr>
    </table>
 <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
          <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

