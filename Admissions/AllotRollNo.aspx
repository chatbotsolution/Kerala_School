<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="AllotRollNo.aspx.cs" Inherits="Admissions_AllotRollNo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function valid() {
            var Class = document.getElementById("<%=drpClass.ClientID %>").value;
            var Sec = document.getElementById("<%=drpSection.ClientID %>").value;
            if (Class == "0") {
                alert("Please Select Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            if (Sec == "0") {
                alert("Please Select Section !");
                document.getElementById("<%=drpSection.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
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
    <%--<script type="text/javascript">
        //variable that will store the id of the last clicked row
        var previousRow;
        var previousrowColor;

        function ChangeRowColor(row) {
            debugger;
            var color = document.getElementById(row).style.backgroundColor;
            //If last clicked row and the current clicked row are same
            if (previousRow == row)
                return; //do nothing
            //If there is row clicked earlier
            else if (previousRow != null)
            //change the color of the previous row back to white
                document.getElementById(previousRow).style.backgroundColor = previousrowColor;

            //change the color of the current row to light yellow

            document.getElementById(row).style.backgroundColor = "#f1d3c9";
            //assign the current row id to the previous row id
            //for next row to be clicked
            previousRow = row;
            previousrowColor = color;
        }
</script>--%>
<%--<script type="text/javascript">
    var SelectedRow = null;
  //  var SelectedRowIndex = null;
//    var UpperBound = null;
    //    var LowerBound = null;
    var UpperBound = document.getElementById("<%= grdstudents.ClientID %>").rows.length;
    var LowerBound = 0;
    var SelectedRowIndex = -1;

//    window.onload = function () {
//        debugger;
//        UpperBound = document.getElementById("<%= grdstudents.ClientID %>").rows.length;
//       // UpperBound = parseInt('<%= this.grdstudents.Rows.Count %>') - 1;
//        LowerBound = 0;
//        SelectedRowIndex = -1;
//    }

    function SelectRow(CurrentRow, RowIndex) {
        debugger;
        if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
            return;

        if (SelectedRow != null) {
            SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
            SelectedRow.style.color = SelectedRow.originalForeColor;
        }

        if (CurrentRow != null) {
            CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
            CurrentRow.originalForeColor = CurrentRow.style.color;
            CurrentRow.style.backgroundColor = '#c5eda9';
            CurrentRow.style.color = 'Black';
           // CurrentRow.cells[2]
        }

        SelectedRow = CurrentRow;
        SelectedRowIndex = RowIndex;
        setTimeout("SelectedRow.focus();", 0);
    }

    function SelectSibling(e) {
        var e = e ? e : window.event;
        var KeyCode = e.which ? e.which : e.keyCode;
        if (KeyCode == 9)
            SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
        if (KeyCode == 40)
            SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
        else if (KeyCode == 38)
            SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);

        return false;
    }
    </script>--%>
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Allot Roll Number</h2>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="height: 360px;" width="100%">
                <tr>
                    <td style="width: 100%;" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top" class="tbltxt cnt-box" width="100%;"
                                    colspan="2">
                                    <div class="cnt-sec">
                                    <span class="ttl3"><asp:Label ID="lblSession" runat="server" Text="Session :"></asp:Label></span>
                                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" Width="100px"
                                        OnSelectedIndexChanged="drpSession_SelectedIndexChanged" CssClass="tbltxtbox"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                    </div>
                                     <div class="cnt-sec">
                                    <span class="ttl3">
                                    <asp:Label ID="lblClass" runat="server" Text="Class :"></asp:Label></span>
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" Width="100px"
                                        OnSelectedIndexChanged="drpClass_SelectedIndexChanged" CssClass="tbltxtbox" TabIndex="2">
                                    </asp:DropDownList>
                                      </div>
                                     <div class="cnt-sec">
                                    <span class="ttl3">
                                    <asp:Label ID="lblSection" runat="server" Text="Section :"></asp:Label></span>
                                    <asp:DropDownList ID="drpSection" runat="server" Width="100px" AutoPostBack="True"
                                        OnSelectedIndexChanged="drpSection_SelectedIndexChanged" CssClass="tbltxtbox"
                                        TabIndex="3">
                                    </asp:DropDownList>
                                      </div>
                                     <div class="cnt-sec">
                                    <span class="ttl3">
                                    <asp:Label ID="lblSelectStudent" runat="server" Text="Select Student :"></asp:Label></span>
                                    <asp:DropDownList ID="drpSelectStudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSelectStudent_SelectedIndexChanged"
                                        CssClass="tbltxtbox" TabIndex="4">
                                    </asp:DropDownList>
                                      </div>
                                     <div class="cnt-sec">
                                    <span class="ttl3">
                                    <asp:Label ID="Label1" runat="server" Text="Order By :"></asp:Label></span>
                                    <asp:DropDownList ID="drpOrder" runat="server" AutoPostBack="True" CssClass="tbltxtbox" Width="100px"
                                        TabIndex="4">
                                        <asp:ListItem Value="0">Roll No.</asp:ListItem>
                                        <asp:ListItem Value="1">Admission No.</asp:ListItem>
                                        <asp:ListItem Value="2">Name</asp:ListItem>
                                    </asp:DropDownList>
                                    </div>
                                    <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" TabIndex="6" Text="Show"
                                        OnClientClick="return valid();" />
                                </td>
                            </tr>
                             <tr>
                             <td colspan="2"> &nbsp;</td>
                             </tr>
                            <tr>
                                <td colspan="2" class="tbltxt cnt-box2 spaceborder" >
                                    <asp:Button ID="btnUpdtRoll" runat="server" OnClick="btnUpdtRoll_Click" Text="Allot roll number starting from 1"
                                        TabIndex="9" />
                                    &nbsp;
                                    <asp:Button ID="btnAllotRoll" runat="server" OnClick="btnAllotRoll_Click" Text="Allot roll number as entered into the textbox"
                                        TabIndex="10" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="tbltxt" style="height: 15px; width: 50%" valign="top">
                                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                </td>
                                <td align="right" class="tbltxt" style="height: 15px" valign="top">
                                    <asp:Label ID="lblRecCount" runat="server" Text="Label" Width="300px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:GridView ID="grdstudents" runat="server" AlternatingRowStyle-CssClass="alt" OnRowDataBound="OnRowDataBound"
                                        AutoGenerateColumns="false" CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr"  
                                        TabIndex="5" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Admission No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbladmnno" runat="server" Text='<%#Eval("Admissionno")%>'></asp:Label>
                                                    <asp:HiddenField ID="hfAdNo" runat="server" Value='<%#Eval("Admissionno") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("fullname")%>'></asp:Label>
                                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("Admissionno")%>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Roll Number">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRoll" runat="server" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                        Text='<%#Eval("RollNo")%>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                </tr>
            </table>
            </td> </tr> </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
