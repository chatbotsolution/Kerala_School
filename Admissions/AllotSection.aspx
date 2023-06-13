<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="AllotSection.aspx.cs" Inherits="Admissions_AllotSection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function valid() {
            var Class = document.getElementById("<%=drpClass.ClientID %>").value;
            var stream = document.getElementById("<%=drpStream.ClientID %>").value;

            if (Class == "0") {
                alert("Please Select Class !");
                document.getElementById("<%=drpClass.ClientID %>").focus();
                return false;
            }
            else if (stream == "0" & Class>"13") {
                alert("Please Select stream !");
                document.getElementById("<%=drpStream.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
        function SelectAll(name) {

            var grid = document.getElementById("<%= grdstudents.ClientID %>");
            var cell;

            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[0];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            cell.childNodes[j].checked = name.checked;
                        }
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">
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
            }

            SelectedRow = CurrentRow;
            SelectedRowIndex = RowIndex;
          
          
            setTimeout("SelectedRow.focus();", 0);
        }

        function SelectSibling(e) {
       
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;

            if (KeyCode == 40) {
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
                
            }
            
            if (KeyCode == 9) 
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
        
            else if (KeyCode == 38) 
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
           
            return false;
        }
    </script>
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Allot Section</h2>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="cnt-box" width="100%">
                <tr>
                    <td style="width: 100%;" valign="top">
                        <table width="100%">
                            <tr>
                                <td align="left" valign="top" class="tbltxt"    colspan="2">
                                    <asp:Label ID="lblSession" runat="server" Text="Session :"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" Width="100px"
                                        OnSelectedIndexChanged="drpSession_SelectedIndexChanged" CssClass="tbltxtbox"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:Label ID="lblClass" runat="server" Text="Class :"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" Width="100px"
                                        OnSelectedIndexChanged="drpClass_SelectedIndexChanged" CssClass="tbltxtbox" TabIndex="2">
                                    </asp:DropDownList>
                                    &nbsp;
                                    
                                    <asp:Label ID="lblStream" runat="server" Text="Stream :"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="drpStream" runat="server" AutoPostBack="True" Width="100px"
                                        OnSelectedIndexChanged="drpStream_SelectedIndexChanged" CssClass="tbltxtbox" Enabled="false" TabIndex="2">
                                    </asp:DropDownList>
                                           
                                    &nbsp;
                                     <asp:Label ID="lblSection" runat="server" Text="Section :"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" Width="100px"
                                        OnSelectedIndexChanged="drpSection_SelectedIndexChanged" CssClass="tbltxtbox" TabIndex="2">
                                    </asp:DropDownList>
                                 
                                    &nbsp;
                                    </td>
                                </tr>
                                          <tr>
                                              <td>
                                    <asp:Label ID="lblSelectStudent" runat="server" Text="Select Student :"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="drpSelectStudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSelectStudent_SelectedIndexChanged"
                                        CssClass="tbltxtbox" TabIndex="4" Width="40%">
                                    </asp:DropDownList>
                                               
                                              <br /><br />
                                    <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" TabIndex="6" Text="Show"
                                        OnClientClick="return valid();" />&nbsp;&nbsp;
                                    <asp:Button ID="btnUpdtSec" runat="server" OnClick="btnUpdtSec_Click" Text="Allot Section" TabIndex="9" />
                                </td>
                                <td align="right" valign="top">
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="height: 15px; width: 50%" align="left" class="tbltxt">
                                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                </td>
                                <td valign="top" style="height: 15px" class="tbltxt" align="right">
                                    <asp:Label ID="lblRecCount" runat="server" Text="Label" Width="300px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:GridView ID="grdstudents" Width="100%" runat="server" AutoGenerateColumns="false" 
                                        CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                        TabIndex="5" OnRowDataBound="grdstudents_RowDataBound" >
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <input type="checkbox" name="Checkb" id="Checkb" runat="server" value='<%# Eval("Admissionno") %>' />
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <input type="checkbox" name="cbSelectAll" onclick='SelectAll(this)' />
                                                </HeaderTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("fullname")%>'></asp:Label>
                                                    <asp:Label ID="lblid" runat="server" Visible="false" Text='<%#Eval("Admissionno")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Admission No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbladmnno" runat="server" Text='<%#Eval("Admissionno")%>'></asp:Label>
                                                    <asp:HiddenField ID="hfAdNo" runat="server" Value='<%#Eval("Admissionno") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Section">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpsection" runat="server">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
