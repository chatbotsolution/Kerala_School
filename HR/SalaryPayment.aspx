<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="SalaryPayment.aspx.cs" Inherits="HR_SalaryPayment" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript" type="text/javascript">
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
                alert("Select any Record");
                return false;
            }
        }
        function isValid() {
            var yr = document.getElementById("<%=drpYear.ClientID %>").selectedIndex;

            if (yr == 0) {
                alert("Select a Year");
                document.getElementById("<%=drpYear.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }

        function CnfPay() {

            var dt = document.getElementById("<%=txtPaymentDate.ClientID %>").value;
            var billNo = document.getElementById("<%=txtPayBill.ClientID %>").value;
            if (dt.trim() == "") {
                alert("Select the Payment Date");
                document.getElementById("<%=txtPaymentDate.ClientID %>").focus();
                return false;
            }
            if (billNo.trim() == "") {
                alert("Enter Bill No");
                document.getElementById("<%=txtPayBill.ClientID %>").focus();
                return false;
            }

            if (confirm("You are going to Pay Salary for the selected Month. Do you want to Continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }
        function CnfRevert() {

            var yr = document.getElementById("<%=drpYear.ClientID %>").selectedIndex;

            if (confirm("You are going to Revert Salary for the Selected Month. Do you want to Continue ?")) {

                return true;
            }
            else {

                return false;
            }
        }
        
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_fm.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <div style="float: left; width: 250px;">
                    <h2>
                        Salary Payment</h2>
                </div>
                <div style="float: left; padding-top: 3px;">
                    <strong>
                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></strong>
                </div>
            </div>
            <div class="spacer">
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <table width="95%">
                <tr>
                    <td>
                        <div style="background-color: #666; padding: 2px;">
                            <div style="background-color: #FFF; padding: 10px;">
                                <strong>Salary Year&nbsp;:&nbsp;</strong><asp:DropDownList ID="drpYear" runat="server"
                                    TabIndex="1">
                                </asp:DropDownList>
                                <strong>Salary Month&nbsp;:&nbsp;</strong><asp:DropDownList ID="drpMonth" runat="server"
                                    TabIndex="2">
                                    <asp:ListItem Text="JAN" Value="JAN"></asp:ListItem>
                                    <asp:ListItem Text="FEB" Value="FEB"></asp:ListItem>
                                    <asp:ListItem Text="MAR" Value="MAR"></asp:ListItem>
                                    <asp:ListItem Text="APR" Value="APR"></asp:ListItem>
                                    <asp:ListItem Text="MAY" Value="MAY"></asp:ListItem>
                                    <asp:ListItem Text="JUN" Value="JUN"></asp:ListItem>
                                    <asp:ListItem Text="JUL" Value="JUL"></asp:ListItem>
                                    <asp:ListItem Text="AUG" Value="AUG"></asp:ListItem>
                                    <asp:ListItem Text="SEP" Value="SEP"></asp:ListItem>
                                    <asp:ListItem Text="OCT" Value="OCT"></asp:ListItem>
                                    <asp:ListItem Text="NOV" Value="NOV"></asp:ListItem>
                                    <asp:ListItem Text="DEC" Value="DEC"></asp:ListItem>
                                </asp:DropDownList>
                                <strong>Designation&nbsp;:&nbsp;</strong><asp:DropDownList ID="drpDesignation" runat="server"
                                    OnSelectedIndexChanged="drpDesignation_SelectedIndexChanged" AutoPostBack="True"
                                    TabIndex="3">
                                </asp:DropDownList>
                                <strong>Employee&nbsp;:&nbsp;</strong><asp:DropDownList ID="drpEmployee" runat="server"
                                    Width="200px" TabIndex="4" 
                                    onselectedindexchanged="drpEmployee_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" onfocus="active(this);"
                                    onblur="inactive(this);" TabIndex="5" />
                                <asp:Button ID="btnShowPending" runat="server" Text="Show Pending Sal" onfocus="active(this);"
                                    onblur="inactive(this);" TabIndex="5" onclick="btnShowPending_Click" />
                                <asp:Button ID="bnRevert" runat="server" Text="Revert Salary" OnClientClick="return CnfRevert();"
                                    onfocus="active(this);" onblur="inactive(this);" OnClick="bnRevert_Click" TabIndex="6" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="background-color: #FFF;">
                            <table width="100%">
                                <tr>
                                    <td colspan="2" align="left" valign="baseline">
                                        <div style="background-color: ButtonFace; height: 33px; text-align: left; padding-top: 2px;">
                                            <strong>Date :</strong>
                                            <asp:TextBox ID="txtPaymentDate" TabIndex="7" runat="server" Width="80px"></asp:TextBox>
                                            <rjs:PopCalendar ID="dtpPaymentDt" runat="server" Control="txtPaymentDate"></rjs:PopCalendar>
                                            <asp:LinkButton ID="lnkbtnStartDt" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtPaymentDate.value='';return false;"
                                                Text="Clear"></asp:LinkButton>
                                            <strong>&nbsp;Pay Bill No :</strong>
                                            <asp:TextBox ID="txtPayBill" Width="80px" TabIndex="8" runat="server"></asp:TextBox>
                                            <strong>Pay Mode&nbsp;:&nbsp;</strong><asp:RadioButtonList ID="rdbtnPayMode" runat="server"
                                                RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbtnPayMode_SelectedIndexChanged"
                                                AutoPostBack="true" RepeatLayout="Flow" TabIndex="9">
                                                <asp:ListItem Text="Cash" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Bank" Value="1" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:DropDownList ID="drpBank" runat="server" TabIndex="10">
                                            </asp:DropDownList>
                                            <strong>Instrument No :</strong>
                                            <asp:TextBox ID="txtInstrNo" TabIndex="11" runat="server" Width="80px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Button ID="btnPay" runat="server" OnClientClick="return CnfPay();" Text="Pay Now"
                                            OnClick="btnPay_Click" Visible="false" onfocus="active(this);" onblur="inactive(this);"
                                            TabIndex="12" />
                                        <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblNoOfRec" runat="server" Style="text-align: right"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:GridView ID="grdEmpPament" runat="server" AutoGenerateColumns="False" Width="100%"
                                            OnRowDataBound="grdEmpPament_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <input name="Checkb" type="checkbox" value='<%# Eval("SalId") %>' />
                                                        <asp:HiddenField ID="hdnSalId" runat="server" Value='<%# Eval("SalId") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderTemplate>
                                                        <input name="toggleAll" onclick="ToggleAll(this)" type="checkbox" value="ON" />
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Employee Name">
                                                    <ItemTemplate>
                                                        <%#Eval("EmpName")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Designation">
                                                    <ItemTemplate>
                                                        <%#Eval("Designation")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Basic Pay" DataField="Pay" DataFormatString="{0:F2}"
                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px"
                                                    HeaderStyle-Width="100px" />
                                                <asp:BoundField HeaderText="Paid Days" DataField="PaidDays" ItemStyle-HorizontalAlign="Right"
                                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="50px" HeaderStyle-Width="50px" />
                                                <asp:TemplateField HeaderText="Gross Total">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGross" runat="server" Text='<%#Eval("GrossTot","{0:F2}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                    <HeaderStyle HorizontalAlign="Right" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Deduction" DataField="TotalDeduction" DataFormatString="{0:F2}"
                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px"
                                                    HeaderStyle-Width="100px" />
                                                <asp:TemplateField HeaderText="Net Payable (Current Month)">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfDed" runat="server" Value='<%#Eval("TotalDeduction")%>' />
                                                        <asp:HiddenField ID="hfGross" runat="server" Value='<%#Eval("GrossTot")%>' />
                                                        <asp:Label ID="lblCurMonth" runat="server" Text="0"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Old Arrear/Prev. Month Pending">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrevAmt" runat="server" Text="0"></asp:Label>
                                                        <asp:HiddenField ID="hfEmpId" runat="server" Value='<%#Eval("EmpId")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Payable">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPaidAmt" runat="server" Text='<%#Eval("NetSal","{0:F2}") %>'
                                                            onkeypress="return blockNonNumbers(this, event, true, false);" TabIndex="13"></asp:TextBox>
                                                        <asp:Label ID="lblPaid" runat="server" Text='<%#Eval("PaidAmount","{0:F2}") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnNetAmt" runat="server" Value='<%#Eval("NetSal")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No Record
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        
                                        
                                        
                                        <asp:GridView ID="grdEmpPending" runat="server" AutoGenerateColumns="False" Width="100%">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <input name="Checkb" type="checkbox" value='<%# Eval("SalId") %>' />
                                                        <asp:HiddenField ID="hdnSalId" runat="server" Value='<%# Eval("SalId") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderTemplate>
                                                        <input name="toggleAll" onclick="ToggleAll(this)" type="checkbox" value="ON" />
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Employee Name">
                                                    <ItemTemplate>
                                                        <%#Eval("EmpName")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Designation">
                                                    <ItemTemplate>
                                                        <%#Eval("Designation")%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Basic Pay" DataField="Pay" DataFormatString="{0:F2}"
                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px"
                                                    HeaderStyle-Width="100px" />
                                                <asp:BoundField HeaderText="Paid Days" DataField="PaidDays" ItemStyle-HorizontalAlign="Right"
                                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="50px" HeaderStyle-Width="50px" />
                                                <asp:TemplateField HeaderText="Gross Total">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGross" runat="server" Text='<%#Eval("GrossTot","{0:F2}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                    <HeaderStyle HorizontalAlign="Right" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Deduction" DataField="TotalDeduction" DataFormatString="{0:F2}"
                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px"
                                                    HeaderStyle-Width="100px" />
                                                <asp:TemplateField HeaderText="Net Payable (Current Month)">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfDed" runat="server" Value='<%#Eval("TotalDeduction")%>' />
                                                        <asp:HiddenField ID="hfGross" runat="server" Value='<%#Eval("GrossTot")%>' />
                                                        <asp:Label ID="lblCurMonth" runat="server" Text='<%#Eval("CurrMonth")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Old Arrear/Prev. Month Pending">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrevAmt" runat="server" Text='<%#Eval("PrevAmt")%>'></asp:Label>
                                                        <asp:HiddenField ID="hfEmpId" runat="server" Value='<%#Eval("EmpId")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Paid Amount" DataField="PaidAmount" DataFormatString="{0:F2}"
                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px"
                                                    HeaderStyle-Width="100px" />
                                                <asp:TemplateField HeaderText="Total Payable Pending">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPaidAmt" runat="server" Text='<%#Eval("NetSal","{0:F2}") %>'
                                                            onkeypress="return blockNonNumbers(this, event, true, false);" TabIndex="13"></asp:TextBox>
                                                        <asp:Label ID="lblPaid" runat="server" Text='<%#Eval("NetSal","{0:F2}") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnNetAmt" runat="server" Value='<%#Eval("NetSal")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No Record
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="font-size: small; color: Red" colspan="2">
                                        <b>Total Payable Amount&nbsp;:&nbsp;<asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label></b>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="drpDesignation" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

