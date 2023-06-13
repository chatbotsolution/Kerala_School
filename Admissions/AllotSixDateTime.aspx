<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="AllotSixDateTime.aspx.cs" Inherits="Admissions_AllotSixDateTime" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="javascript" type="text/javascript">
    function valid() {
        var Class = document.getElementById("<%=drpClass.ClientID %>").value;
        if (Class == "0") {
            alert("Please Select Class !");
            document.getElementById("<%=drpClass.ClientID %>").focus();
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


     Sys.Application.add_load(function () {
    $('.Tdate').datepicker({
            dateFormat: 'dd-mm-yy',
            changeMonth: true,
            changeYear: true,
            
        });
          });
    
    </script>
   
     <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Modify Admission Date
        </h2>
    </div>
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
                                     <asp:Label ID="lblSection" runat="server" Text="Section :"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="drpSection" runat="server" AutoPostBack="True" Width="100px"
                                        OnSelectedIndexChanged="drpSection_SelectedIndexChanged" CssClass="tbltxtbox" TabIndex="2">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:Label ID="lblSelectStudent" runat="server" Text="Select Student :"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="drpSelectStudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSelectStudent_SelectedIndexChanged"
                                        CssClass="tbltxtbox" TabIndex="4">
                                    </asp:DropDownList><br /><br />
                                    <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" TabIndex="6" Text="Show"
                                        OnClientClick="return valid();" />&nbsp;&nbsp;
                                    <asp:Button ID="btnUpdtSec" runat="server"  Text="Modify Admission Date" TabIndex="9" OnClick="btnUpdtSixSub_Click"/>
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
                                        TabIndex="5">
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
                                            <asp:TemplateField HeaderText="Admission Date Time">
                                                <ItemTemplate>
                                                <asp:TextBox ID="txtDt" runat="server" Width="180px" CssClass="Tdate" placeholder="DateTime"></asp:TextBox>
                                                   <%-- <asp:TextBox ID="txtDt" runat="server" Width="180px" ></asp:TextBox>
                                                      <rjs:PopCalendar ID="dtpDt" runat="server" Control="txtDt" />--%>
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

