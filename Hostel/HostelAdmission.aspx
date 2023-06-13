<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="HostelAdmission.aspx.cs" Inherits="Hostel_HostelAdmission" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function valDetails() {
            var Admno = document.getElementById("<%=txtadmnno.ClientID %>").value;
            if (Admno.trim() == "") {
                alert("Please Enter Admission Number");
                document.getElementById("<%=txtadmnno.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }

        function valSearch() {
            var session = document.getElementById("<%=drpSession.ClientID %>").value;
            var Class = document.getElementById("<%=drpclass.ClientID %>").value;
            if (session == "0") {
                alert("Please Select Session!!");
                document.getElementById("<%=drpSession.ClientID %>").focus();
                return false;
            }
            if (Class == "0") {
                alert("Please Select Class!!");
                document.getElementById("<%=drpclass.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }


        function CnfSubmit() {
            var FeeDt = document.getElementById("<%=txtAdmnDt.ClientID %>").value;
            var feestmonth = document.getElementById("<%=drpFeeStartFrom.ClientID %>").value;
            var FeeConcess = document.getElementById("<%=txtFeeConcession.ClientID %>").value;
            var ConcessReas = document.getElementById("<%=txtConceReason.ClientID %>").value;

            if (FeeDt.trim() == "") {
                alert("Please Enter Admission Date");
                document.getElementById("<%=txtAdmnDt.ClientID %>").focus();
                return false;
            }
            if (feestmonth == "0" && saveMode == "Insert") {
                alert("Please Select the month from which the fee is to be collected.");
                document.getElementById("<%=drpFeeStartFrom.ClientID %>").focus();
                return false;
            }
            if (FeeConcess.trim() > "0" && ConcessReas.trim() == "") {
                alert("Please Enter Reason For Fee Concession");
                document.getElementById("<%=txtConceReason.ClientID %>").focus();
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
            if (flag == true)
                return true;
            else {
                alert("Please select any record");
                return false;
            }
        }
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Hostel Admission</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="true" CssClass="tbltxt" Text=""></asp:Label></div>
            <fieldset id="fsselection" runat="server">
                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <td align="left" class="tbltxt" style="border: solid 1px Black; width: 100%;">
                            Session Year :&nbsp;<asp:DropDownList ID="drpSession" runat="server" CssClass="vsmalltb"
                                Width="70px" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="drpSession_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp; Class :&nbsp;<asp:DropDownList ID="drpclass" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="drpclass_SelectedIndexChanged" CssClass="vsmalltb" Width="70px"
                                meta:resourcekey="drpclassResource1" TabIndex="2">
                            </asp:DropDownList>
                            &nbsp; Section :&nbsp;
                            <asp:DropDownList ID="drpSection" runat="server" Width="60px" AutoPostBack="True"
                                 CssClass="tbltxtbox"
                                TabIndex="3" onselectedindexchanged="drpSection_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp; Student :&nbsp;<asp:DropDownList ID="drpstudent" runat="server" Width="200px"
                                AutoPostBack="True" OnSelectedIndexChanged="drpstudent_SelectedIndexChanged"
                                CssClass="largetb" TabIndex="3">
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return valSearch();"
                                onclick="btnSearch_Click" />
                          
                            <%--Student Id :&nbsp;--%>
                            <asp:TextBox ID="txtadmnno" runat="server" CssClass="vsmalltb" TabIndex="4" Width="70px"
                                Visible="false"></asp:TextBox> &nbsp;
                            <asp:Button ID="btnDetail" runat="server" Text="Show" OnClientClick="return valDetails();"
                                OnClick="btnDetail_Click" Style="width: 48px; height: 26px;" Visible="false" />
                        </td>
                        <tr>
                            <td align="left" class="tbltxt" style="border: solid 1px Black; width: 100%;">
                                Admission Date :&nbsp;<asp:TextBox ID="txtAdmnDt" runat="server" CssClass="vsmalltb"
                                    meta:resourcekey="txtdateResource1" TabIndex="5"></asp:TextBox>&nbsp;<rjs:PopCalendar
                                        ID="dtpAdmnDt" runat="server" Control="txtAdmnDt"></rjs:PopCalendar>
                                <span class="error">*</span> &nbsp; Fee Starts From : &nbsp;
                                <asp:DropDownList ID="drpFeeStartFrom" runat="server" CssClass="smalltb" Width="70px"
                                    TabIndex="9">
                                    <asp:ListItem Value="04">Apr</asp:ListItem>
                                    <asp:ListItem Value="05">May</asp:ListItem>
                                    <asp:ListItem Value="06">June</asp:ListItem>
                                    <asp:ListItem Value="07">July</asp:ListItem>
                                    <asp:ListItem Value="08">Aug</asp:ListItem>
                                    <asp:ListItem Value="09">Sep</asp:ListItem>
                                    <asp:ListItem Value="10">Oct</asp:ListItem>
                                    <asp:ListItem Value="11">Nov</asp:ListItem>
                                    <asp:ListItem Value="12">Dec</asp:ListItem>
                                    <asp:ListItem Value="01">Jan</asp:ListItem>
                                    <asp:ListItem Value="02">Feb</asp:ListItem>
                                    <asp:ListItem Value="03">Mar</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp; Fee Concession :&nbsp;<asp:TextBox ID="txtFeeConcession" runat="server" CssClass="smalltb"
                                    TabIndex="10" Width="40px" Text="0" onkeypress="return blockNonNumbers(this, event, false, false);"></asp:TextBox>
                                &nbsp;% 
                                <br /><br />
                                Reason :&nbsp;<asp:TextBox ID="txtConceReason" runat="server" CssClass="smalltb"
                                    TabIndex="11" Width="400px" Text=""></asp:TextBox>
                              <%--  <asp:CheckBox ID="chkGenFee" Checked="true" runat="server" Text="Generate Fee" TabIndex="6" />
                                <asp:CheckBox ID="chkOT" runat="server" Text="One Time" TabIndex="7" />
                                &nbsp;<asp:CheckBox ID="chkAnnual" runat="server" Text="Annual" TabIndex="8" />--%>
                            </td>
                        </tr>
                    </tr>
                    <tr>
                        <td>
                            <div style="text-align:right;"><asp:Label ID="lblCount" runat="server" Text=""></asp:Label> </div>
                            <asp:GridView ID="grdstuddet" runat="server" Width="98%" AutoGenerateColumns="False"
                                        CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                        TabIndex="7">
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox"
                                                        value="ON" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="15px" />
                                                <HeaderStyle Width="15px" />
                                                <ItemTemplate>
                                                    <input name="Checkb" type="checkbox" value='<%#Eval("AdmnNo")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Admission No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrollno" Font-Bold="true" runat="server" Text='<%# Eval("AdmnNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                        <asp:Label ID="lblname" runat="server" Text='<%#Eval("FullName")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FatherName" HeaderText="Father Name" HeaderStyle-HorizontalAlign="Left" />
                                           <%-- <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldob" runat="server" Text='<%#Eval("dateofbirth")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Admn Session">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdmnSess" runat="server" Text='<%#Eval("AdmnSessYr")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                          
                                           <%-- <asp:TemplateField HeaderText="Admission Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclass" runat="server" Text='<%#Eval("AdmnDate")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>--%>
                                          
                                        </Columns>
                                        <RowStyle BackColor="#EFEFEF" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle BackColor="#999999" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="#FDFDFD" />
                                        <EmptyDataTemplate>
                                            No Record
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 31px">
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                OnClientClick="return CnfSubmit();" Width="100px" TabIndex="12" />
                            <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClick="btnClear_Click"
                                Text="Clear" TabIndex="13" Width="100px" />
                            <asp:Button ID="btnFeeRcv" runat="server" CausesValidation="False" OnClick="btnFeeRcv_Click"
                                Text="Receive Fee" TabIndex="14" Enabled="False" />
                            <asp:Button ID="btnList" runat="server" CausesValidation="False" OnClick="btnList_Click"
                                Text="Student List" TabIndex="15" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

